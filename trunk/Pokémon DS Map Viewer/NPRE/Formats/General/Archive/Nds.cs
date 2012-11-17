using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using NPRE.Archive;
using System.Windows.Forms;

public class Nds
{
    private static ClosableMemoryStream ARM7;
    private static ClosableMemoryStream ARM9;
    private static NdsFat_s FAT;
    private static NdsFnt_s FNT;
    private static NdsHeader Header;
    private static ClosableMemoryStream Icon;
    private static NdsOverlay OverlayList;
    private BinaryReader reader;
    private static ClosableMemoryStream Secure;
    public Common.Folder_s Tree;

    public Nds(BinaryReader reader)
    {
        this.reader = reader;
    }



    public ClosableMemoryStream getARM7()
    {
        return ARM7;
    }

    public ClosableMemoryStream getARM9()
    {
        return ARM9;
    }

    public NdsFat_s getFat()
    {
        return FAT;
    }

    public NdsFnt_s getFnt()
    {
        return FNT;
    }

    public NdsHeader getHeader()
    {
        return Header;
    }

    public Nds LoadNds()
    {
        Header = new NdsHeader(this.reader);
        Header.ReadHeader();
        ARM9 = new ClosableMemoryStream();
        this.reader.BaseStream.Seek((long) Header.getARM9Offset(), SeekOrigin.Begin);
        Utils.loadStream(this.reader, Header.getARM9Size() + 12, ARM9);
        this.reader.BaseStream.Seek((long) Header.getARM9Ov_Off(), SeekOrigin.Begin);
        OverlayList = new NdsOverlay();
        OverlayList.readOverlays(this.reader, Header.getARM9Ov_Size());
        uint position = (uint) this.reader.BaseStream.Position;
        ARM7 = new ClosableMemoryStream();
        Utils.loadStream(this.reader, Header.getARM7Size(), ARM7);
        FAT = new NdsFat_s();
        FAT.ReadFat(this.reader, Header.getFAT_Size(), Header.getFAT_Off());
        OverlayList.readOverlaysData(this.reader, position, (int) Header.getARM9Ov_Size(), FAT);
        FNT = new NdsFnt_s();
        this.Tree = new Common.Folder_s();
        this.reader.BaseStream.Seek((long) Header.getFNT_Off(), SeekOrigin.Begin);
        this.Tree = NdsFnt_s.readFnt(this.reader, Header.getFNT_Size(), Header.getFNT_Off(), FAT);
        Icon = new ClosableMemoryStream();
        Utils.loadStream(this.reader, 0xa00, Icon);
        return this;
    }

    public void SaveFile(BinaryWriter writer)
    {
        int num4;
        uint position = (uint) writer.BaseStream.Position;
        uint num2 = (uint) writer.BaseStream.Position;
        int num3 = 0;
        List<NdsFat_s.FileOff_s> list = FAT.getFileOff();
        for (num4 = 0; num4 < (list.Count - 1); num4++)
        {
            for (int i = 0; i < (list.Count - 1); i++)
            {
                if (list[num4].File_Start < list[i].File_Start)
                {
                    NdsFat_s.FileOff_s _s = list[i];
                    list[i] = list[num4];
                    list[num4] = _s;
                }
            }
        }
        for (num4 = 0; num4 < list.Count; num4++)
        {
            position = (uint) writer.BaseStream.Position;
            num2 = (uint) writer.BaseStream.Position;
            uint num6 = FAT.getFileStartAt(num4);
            if (num6 >= position)
            {
                position = num6;
                num3 = num4;
                for (uint j = num2; j < position; j++)
                {
                    writer.Write((byte) 0xff);
                }
                BinaryReader reader = new BinaryReader(FAT.getFileStreamAt(num4));
                Utils.loadStream(reader, (uint) FAT.getFileStreamAt(num4).Length, writer.BaseStream);
            }
        }
    }

    public void SaveNds(BinaryWriter b)
    {
        Header.SaveHeader(b);
        ARM9.WriteTo(b.BaseStream);
        Utils.CreateVoid(b, Header.getARM9Ov_Off());
        OverlayList.saveOverlayInfo(b);
        Utils.CreateVoid(b, Header.getARM7Offset());
        ARM7.WriteTo(b.BaseStream);
        b.Close();
    }


    public class NdsFat_s
    {
        private static List<FileOff_s> FileOff;

        public uint getFileEndAt(int i)
        {
            return FileOff[i].File_End;
        }

        public List<FileOff_s> getFileOff()
        {
            return FileOff;
        }

        public uint getFileStartAt(int i)
        {
            return FileOff[i].File_Start;
        }

        public ClosableMemoryStream getFileStreamAt(int i)
        {
            return FileOff[i].File_Data;
        }

        public void ReadFat(BinaryReader reader, uint size, uint off)
        {
            reader.BaseStream.Seek((long) off, SeekOrigin.Begin);
            FileOff = new List<FileOff_s>();
            for (int i = 0; i < (size / 8); i++)
            {
                FileOff_s item = new FileOff_s {
                    File_Start = reader.ReadUInt32(),
                    File_End = reader.ReadUInt32()
                };
                FileOff.Add(item);
            }
        }

        public void SaveFat(BinaryWriter bw)
        {
            for (int i = 0; i < FileOff.Count; i++)
            {
                bw.Write(FileOff[i].File_Start);
                bw.Write(FileOff[i].File_End);
            }
        }

        public void setFileOffAt(int i, FileOff_s input)
        {
            FileOff_s actualFileOffset = FileOff[i];
            actualFileOffset.File_Start = input.File_Start;
            actualFileOffset.File_End = input.File_Start;
            FileOff[i] = actualFileOffset;
        }

        public void setFileStreamAt(int i, MemoryStream input)
        {
            FileOff_s actualFileOffset = FileOff[i];
            actualFileOffset.File_Data = new ClosableMemoryStream();
            input.WriteTo(actualFileOffset.File_Data);
            FileOff[i] = actualFileOffset;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FileOff_s
        {
            public uint File_Start;
            public uint File_End;
            public ClosableMemoryStream File_Data;
        }
    }

    public class NdsFnt_s
    {
        private static byte[] Fnt_Data;
        private static List<MainDir_s> MainDir;
        private static uint SubDir_Off;

        public byte[] getFNTData()
        {
            return Fnt_Data;
        }

        #region Reading

        public static Common.Folder_s readFnt(BinaryReader reader, uint size, uint offset, Nds.NdsFat_s FAT)
        {

            loadFntDataBinary(reader, size, offset);
            loadSubDirOff(reader, offset);
            MainDir = new List<MainDir_s>();
            int index = 0;
            List<Common.FileInfo_s> fileInfoList = new List<Common.FileInfo_s>();
            while (reader.BaseStream.Position < (offset + SubDir_Off))
                index = createMainDirList(reader, offset, FAT, index, fileInfoList);
            Common.Folder_s root = makeRoot();
            return root;
        }

        private static int createMainDirList(BinaryReader reader, uint offset, Nds.NdsFat_s FAT, int index, List<Common.FileInfo_s> fileInfoList)
        {
            MainDir_s actualMainDir = loadActualMainDir(reader);
            long nextMainDirOffset = reader.BaseStream.Position;
            actualMainDir = loadActualSubDir(reader, offset, FAT, ref index, fileInfoList, actualMainDir);
            MainDir.Add(actualMainDir);
            reader.BaseStream.Position = nextMainDirOffset;
            return index;
        }

        private static MainDir_s loadActualSubDir(BinaryReader reader, uint offset, Nds.NdsFat_s FAT, ref int index, List<Common.FileInfo_s> fileInfoList, MainDir_s actualMainDir)
        {
            reader.BaseStream.Position = offset + actualMainDir.Start;
            var actualSubDir = actualMainDir.SubDir;
            actualSubDir = initActualSubDir(reader, index, actualMainDir, actualSubDir);

            while (actualSubDir.Type != 0)
                loadSubElements(reader, FAT, ref index, fileInfoList, ref actualMainDir, ref actualSubDir);
            actualMainDir.SubDir = actualSubDir;
            return actualMainDir;
        }

        private static void loadSubElements(BinaryReader reader, Nds.NdsFat_s FAT, ref int index, List<Common.FileInfo_s> fileInfoList, ref MainDir_s actualMainDir, ref SubDir_s actualSubDir)
        {
            int type;
            if (actualSubDir.Type < 0x80)
                type = loadSubFiles(reader, FAT, fileInfoList, ref actualMainDir, ref actualSubDir);
            if (actualSubDir.Type > 0x80)
                type = loadSubFolders(reader, ref actualSubDir);
            updateTypes(reader, ref index, ref actualSubDir);
        }

        private static void updateTypes(BinaryReader reader, ref int index, ref SubDir_s actualSubDir)
        {
            index++;
            Array.Resize<byte>(ref actualSubDir.Type2, index + 1);
            actualSubDir.Type2[index] = reader.ReadByte();
            actualSubDir.Type = actualSubDir.Type2[index];
        }

        private static int loadSubFolders(BinaryReader reader, ref SubDir_s actualSubDir)
        {
            int type;
            Common.Folder_s actualFolder = new Common.Folder_s();
            if (actualSubDir.Folders == null)
            {
                actualSubDir.Folders = new List<Common.Folder_s>();
            }
            type = actualSubDir.Type - 0x80;
            actualFolder.Name = new string(Encoding.GetEncoding("shift_jis").GetChars(reader.ReadBytes(type)));
            actualFolder.ID = reader.ReadUInt16();
            actualSubDir.Folders.Add(actualFolder);
            return type;
        }

        private static int loadSubFiles(BinaryReader reader, Nds.NdsFat_s FAT, List<Common.FileInfo_s> fileInfoList, ref MainDir_s actualMainDir, ref SubDir_s actualSubDir)
        {
            int type;
            Common.FileInfo_s actualFileInfo = new Common.FileInfo_s();
            if (actualSubDir.Files == null)
            {
                actualSubDir.Files = new List<Common.FileInfo_s>();
            }
            type = actualSubDir.Type;
            actualFileInfo.Name = new string(Encoding.GetEncoding("shift_jis").GetChars(reader.ReadBytes(type)));
            actualFileInfo.ID = actualMainDir.FFile_ID;
            actualFileInfo.Offset = FAT.getFileStartAt((int)actualFileInfo.ID);
            actualFileInfo.Size = FAT.getFileEndAt((int)actualFileInfo.ID) - actualFileInfo.Offset;
            long num5 = reader.BaseStream.Position;
            reader.BaseStream.Seek((long)actualFileInfo.Offset, SeekOrigin.Begin);
            actualFileInfo.FileData = new ClosableMemoryStream();
            Utils.loadStream(reader, actualFileInfo.Size, actualFileInfo.FileData);
            reader.BaseStream.Seek((long)actualFileInfo.Offset, SeekOrigin.Begin);
            FAT.setFileStreamAt((int)actualFileInfo.ID, actualFileInfo.FileData);
            reader.BaseStream.Seek(num5, SeekOrigin.Begin);
            actualMainDir.FFile_ID = (ushort)(actualMainDir.FFile_ID + 1);
            actualSubDir.Files.Add(actualFileInfo);
            fileInfoList.Add(actualFileInfo);
            return type;
        }

        private static SubDir_s initActualSubDir(BinaryReader reader, int index, MainDir_s actualMainDir, SubDir_s actualSubDir)
        {
            Array.Resize<byte>(ref actualSubDir.Type2, index + 1);
            actualSubDir.Type2[index] = reader.ReadByte();
            actualSubDir.Type = actualSubDir.Type2[index];
            actualSubDir.Parent_ID = actualMainDir.FFile_ID;
            return actualSubDir;
        }

        private static MainDir_s loadActualMainDir(BinaryReader reader)
        {
            MainDir_s actualMainDir = new MainDir_s
            {
                Start = reader.ReadUInt32(),
                FFile_ID = reader.ReadUInt16(),
                Parent_ID = reader.ReadUInt16()
            };
            return actualMainDir;
        }

        private static void loadSubDirOff(BinaryReader reader, uint offset)
        {

            SubDir_Off = reader.ReadUInt32();
            reader.BaseStream.Position = offset;
        }

        private static void loadFntDataBinary(BinaryReader reader, uint size, uint offset)
        {
            Fnt_Data = new byte[size];
            Fnt_Data = reader.ReadBytes((int)size);
            reader.BaseStream.Position = offset;
        }

        #endregion

        #region Root & Ordering

        private static Common.Folder_s makeRoot()
        {
            Common.Folder_s root = new Common.Folder_s();
            root = orderFolder(MainDir, 0, "root");
            root.ID = 0xf000;
            return root;
        }

        public static Common.Folder_s orderFolder(List<MainDir_s> tables, int idFolder, string nameFolder)
        {
            Common.Folder_s actualFolder = initActualFolder(tables, idFolder, nameFolder);

            if (actualFolder.Files != null)
                actualFolder = updateParentIDFiles(idFolder, actualFolder);

            if (tables[idFolder & 0xfff].SubDir.Folders != null)
                actualFolder = orderSubFolder(tables, idFolder, actualFolder);
            return actualFolder;
        }

        private static Common.Folder_s orderSubFolder(List<MainDir_s> tables, int idFolder, Common.Folder_s actualFolder)
        {
            actualFolder.Folders = new List<Common.Folder_s>();
            foreach (Common.Folder_s subFolder in tables[idFolder & 0xfff].SubDir.Folders)
                actualFolder.Folders.Add(orderFolder(tables, (int)subFolder.ID, subFolder.Name));
            return actualFolder;
        }

        private static Common.Folder_s updateParentIDFiles(int idFolder, Common.Folder_s actualFolder)
        {
            for (int i = 0; i < actualFolder.Files.Count; i++)
            {
                Common.FileInfo_s actualFile = actualFolder.Files[i];
                actualFile.Parent_ID = (ushort)idFolder;
                actualFolder.Files[i] = actualFile;
            }
            return actualFolder;
        }

        private static Common.Folder_s initActualFolder(List<MainDir_s> tables, int idFolder, string nameFolder)
        {
            Common.Folder_s actualFolder = new Common.Folder_s
            {
                Name = nameFolder,
                ID = (ushort)idFolder,
                Files = tables[idFolder & 0xfff].SubDir.Files
            };
            return actualFolder;
        }

        #endregion

        #region Struct
        [StructLayout(LayoutKind.Sequential)]
        public struct MainDir_s
        {
            public Nds.NdsFnt_s.SubDir_s SubDir;
            public uint Start;
            public ushort FFile_ID;
            public ushort Parent_ID;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SubDir_s
        {
            public byte Type;
            public byte[] Type2;
            public ushort Parent_ID;
            public List<Common.FileInfo_s> Files;
            public List<Common.Folder_s> Folders;
        }

        #endregion
    }

    public class NdsHeader
    {
        private uint ARM7Auto_Off;
        private uint ARM7Entry_Off;
        private uint ARM7Ov_Off;
        private uint ARM7Ov_Size;
        private uint ARM7Ram_Off;
        private uint ARM7Rom_Off;
        private uint ARM7Size;
        private uint ARM9_Size;
        private uint ARM9Auto_Off;
        private uint ARM9Entry_Off;
        private uint ARM9Ov_Off;
        private uint ARM9Ov_Size;
        private uint ARM9Ram_Off;
        private uint ARM9Rom_Off;
        private byte Autostart;
        private uint DebugRam_Off;
        private uint DebugRom_Off;
        private uint DebugRom_Size;
        private byte Enc_Seed;
        private uint FAT_Off;
        private uint FAT_Size;
        private uint FNT_Off;
        private uint FNT_Size;
        private string Game_Code;
        private string Game_Title;
        private ushort Header_CRC;
        private uint Header_Size;
        private uint IconTitle_Off;
        private int Key_Sett;
        private byte[] Logo;
        private ushort Logo_CRC;
        private string Maker_Code;
        private int Norm_Sett;
        private BinaryReader reader;
        private byte[] Reserved;
        private byte[] Reserved2;
        private uint Reserved3;
        private byte[] Reserved4;
        private uint ROM_Size;
        private byte ROM_Version;
        private short Secure_Check;
        private ulong Secure_Dis;
        private ushort Secure_Time;
        private uint Size;
        private byte Unit_Code;

        public NdsHeader(BinaryReader reader)
        {
            this.reader = reader;
        }

        public uint getARM7Offset()
        {
            return this.ARM7Rom_Off;
        }

        public uint getARM7Ov_Off()
        {
            return this.ARM7Ov_Off;
        }

        public uint getARM7Ov_Size()
        {
            return this.ARM7Ov_Size;
        }

        public uint getARM7Size()
        {
            return this.ARM7Size;
        }

        public uint getARM9Offset()
        {
            return this.ARM9Rom_Off;
        }

        public uint getARM9Ov_Off()
        {
            return this.ARM9Ov_Off;
        }

        public uint getARM9Ov_Size()
        {
            return this.ARM9Ov_Size;
        }

        public uint getARM9Size()
        {
            return this.ARM9_Size;
        }

        public string getCode()
        {
            return this.Game_Code;
        }

        public uint getFAT_Off()
        {
            return this.FAT_Off;
        }

        public uint getFAT_Size()
        {
            return this.FAT_Size;
        }

        public uint getFNT_Off()
        {
            return this.FNT_Off;
        }

        public uint getFNT_Size()
        {
            return this.FNT_Size;
        }

        public uint getIconTitle_Off()
        {
            return this.IconTitle_Off;
        }

        public string getTitle()
        {
            return this.Game_Title;
        }

        public void ReadHeader()
        {
            Encoding encoding = Encoding.UTF8;
            this.Game_Title = encoding.GetString(this.reader.ReadBytes(12));
            this.Game_Code = encoding.GetString(this.reader.ReadBytes(4));
            this.Maker_Code = encoding.GetString(this.reader.ReadBytes(2));
            this.Unit_Code = this.reader.ReadByte();
            this.Enc_Seed = this.reader.ReadByte();
            this.Size = (uint) Math.Pow(2.0, (double) (20 + this.reader.ReadByte()));
            this.Reserved = this.reader.ReadBytes(9);
            this.ROM_Version = this.reader.ReadByte();
            this.Autostart = this.reader.ReadByte();
            this.ARM9Rom_Off = this.reader.ReadUInt32();
            this.ARM9Entry_Off = this.reader.ReadUInt32();
            this.ARM9Ram_Off = this.reader.ReadUInt32();
            this.ARM9_Size = this.reader.ReadUInt32();
            this.ARM7Rom_Off = this.reader.ReadUInt32();
            this.ARM7Entry_Off = this.reader.ReadUInt32();
            this.ARM7Ram_Off = this.reader.ReadUInt32();
            this.ARM7Size = this.reader.ReadUInt32();
            this.FNT_Off = this.reader.ReadUInt32();
            this.FNT_Size = this.reader.ReadUInt32();
            this.FAT_Off = this.reader.ReadUInt32();
            this.FAT_Size = this.reader.ReadUInt32();
            this.ARM9Ov_Off = this.reader.ReadUInt32();
            this.ARM9Ov_Size = this.reader.ReadUInt32();
            this.ARM7Ov_Off = this.reader.ReadUInt32();
            this.ARM7Ov_Size = this.reader.ReadUInt32();
            this.Norm_Sett = this.reader.ReadInt32();
            this.Key_Sett = this.reader.ReadInt32();
            this.IconTitle_Off = this.reader.ReadUInt32();
            this.Secure_Check = this.reader.ReadInt16();
            this.Secure_Time = this.reader.ReadUInt16();
            this.ARM9Auto_Off = this.reader.ReadUInt32();
            this.ARM7Auto_Off = this.reader.ReadUInt32();
            this.Secure_Dis = this.reader.ReadUInt64();
            this.ROM_Size = this.reader.ReadUInt32();
            this.Header_Size = this.reader.ReadUInt32();
            this.Reserved2 = this.reader.ReadBytes(0x38);
            this.Logo = this.reader.ReadBytes(0x9c);
            this.Logo_CRC = this.reader.ReadUInt16();
            this.Header_CRC = this.reader.ReadUInt16();
            this.DebugRom_Off = this.reader.ReadUInt32();
            this.DebugRom_Size = this.reader.ReadUInt32();
            this.DebugRam_Off = this.reader.ReadUInt32();
            this.Reserved3 = this.reader.ReadUInt32();
            this.Reserved4 = new byte[0x4000L - this.reader.BaseStream.Position];
            this.Reserved4 = this.reader.ReadBytes((int) (0x4000L - this.reader.BaseStream.Position));
        }

        public void SaveHeader(BinaryWriter writer)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            writer.Write(encoding.GetBytes(this.Game_Title));
            writer.Write(encoding.GetBytes(this.Game_Code));
            writer.Write(encoding.GetBytes(this.Maker_Code));
            writer.Write(this.Unit_Code);
            writer.Write(this.Enc_Seed);
            writer.Write((byte) (Math.Log((double) this.Size, 2.0) - 20.0));
            writer.Write(this.Reserved);
            writer.Write(this.ROM_Version);
            writer.Write(this.Autostart);
            writer.Write(this.ARM9Rom_Off);
            writer.Write(this.ARM9Entry_Off);
            writer.Write(this.ARM9Ram_Off);
            writer.Write(this.ARM9_Size);
            writer.Write(this.ARM7Rom_Off);
            writer.Write(this.ARM7Entry_Off);
            writer.Write(this.ARM7Ram_Off);
            writer.Write(this.ARM7Size);
            writer.Write(this.FNT_Off);
            writer.Write(this.FNT_Size);
            writer.Write(this.FAT_Off);
            writer.Write(this.FAT_Size);
            writer.Write(this.ARM9Ov_Off);
            writer.Write(this.ARM9Ov_Size);
            writer.Write(this.ARM7Ov_Off);
            writer.Write(this.ARM7Ov_Size);
            writer.Write(this.Norm_Sett);
            writer.Write(this.Key_Sett);
            writer.Write(this.IconTitle_Off);
            writer.Write((ushort) this.Secure_Check);
            writer.Write(this.Secure_Time);
            writer.Write(this.ARM9Auto_Off);
            writer.Write(this.ARM7Auto_Off);
            writer.Write(this.Secure_Dis);
            writer.Write(this.ROM_Size);
            writer.Write(this.Header_Size);
            writer.Write(this.Reserved2);
            writer.Write(this.Logo);
            writer.Write(this.Logo_CRC);
            writer.Write((uint) this.Header_CRC);
            writer.Write(this.DebugRom_Off);
            writer.Write(this.DebugRom_Size);
            writer.Write(this.DebugRam_Off);
            writer.Write((ushort) this.Reserved3);
            writer.Write(this.Reserved4);
        }
    }

    public class NdsOverlay
    {
        private static List<Overlay_s> Overlay = new List<Overlay_s>();

        public void readOverlays(BinaryReader reader, uint size)
        {
            for (int i = 0; i < (size / 0x20); i++)
                readOverlay(reader);
        }

        private static void readOverlay(BinaryReader reader)
        {
            Overlay_s item = new Overlay_s
            {
                OverlayID = reader.ReadUInt32(),
                RAM_Adress = reader.ReadUInt32(),
                RAM_Size = reader.ReadUInt32(),
                BSS_Size = reader.ReadUInt32(),
                stInitStart = reader.ReadUInt32(),
                stInitEnd = reader.ReadUInt32(),
                fileID = reader.ReadUInt32(),
                reserved = reader.ReadUInt32()
            };
            Overlay.Add(item);
        }

        public void readOverlaysData(BinaryReader reader, uint offset, int size, Nds.NdsFat_s FAT)
        {
            reader.BaseStream.Seek((long) offset, SeekOrigin.Begin);
            for (int i = 0; i < (size / 0x20); i++)
            {
                Overlay_s actualOverlay = Overlay[i];
                actualOverlay = readOverlayData(reader, FAT, i, actualOverlay);
                Overlay[i] = actualOverlay;
            }
        }

        private static Overlay_s readOverlayData(BinaryReader reader, Nds.NdsFat_s FAT, int i, Overlay_s actualOverlay)
        {
            actualOverlay.Over = new ClosableMemoryStream();
            actualOverlay.Ov_Start = FAT.getFileStartAt((int)actualOverlay.fileID);
            actualOverlay.Ov_End = FAT.getFileEndAt((int)actualOverlay.fileID);

            reader.BaseStream.Seek((long)actualOverlay.Ov_Start, SeekOrigin.Begin);
            Utils.loadStream(reader, actualOverlay.Ov_End - actualOverlay.Ov_Start, actualOverlay.Over);
            FAT.setFileStreamAt(i, actualOverlay.Over);
            return actualOverlay;
        }

        public void saveOverlayInfo(BinaryWriter br)
        {
            saveOverlays(br);
            saveOverlaysData(br);
        }

        private static void saveOverlaysData(BinaryWriter br)
        {
            for (int num = 0; num < Overlay.Count; num++)
                saveOverlayData(br, num);
        }

        private static void saveOverlayData(BinaryWriter br, int num)
        {
            for (long i = br.BaseStream.Position; i < Overlay[num].Ov_Start; i += 1L)
                br.Write((byte)0xff);
            BinaryReader reader = new BinaryReader(Overlay[num].Over);
            Utils.loadStream(reader, (uint)Overlay[num].Over.Length, br.BaseStream);
        }

        private static void saveOverlays(BinaryWriter br)
        {
            for (int num = 0; num < Overlay.Count; num++)
            {
                br.Write(Overlay[num].OverlayID);
                br.Write(Overlay[num].RAM_Adress);
                br.Write(Overlay[num].RAM_Size);
                br.Write(Overlay[num].BSS_Size);
                br.Write(Overlay[num].stInitStart);
                br.Write(Overlay[num].stInitEnd);
                br.Write(Overlay[num].fileID);
                br.Write(Overlay[num].reserved);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Overlay_s
        {
            public uint OverlayID;
            public uint RAM_Adress;
            public uint RAM_Size;
            public uint BSS_Size;
            public uint stInitStart;
            public uint stInitEnd;
            public uint fileID;
            public uint reserved;
            public uint Ov_Start;
            public uint Ov_End;
            public MemoryStream Over;
        }
    }
}

