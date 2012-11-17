namespace PG4Map
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using NPRE.Archive;
    using System.Windows.Forms;

    public class Narc
    {
        public int magic;
        public NarcFat_s fatb;
        public uint fatbNum;
        public uint fatbSize;
        public Figm_s figm;
        public NarcFnt_s fntb;
        public uint fntbStartOffset;
        public uint fntbSize;
        public short sectionNum;
        public short sectionSize;
        public uint narcSize;
        public Common.Folder_s treeSystem;

        public Narc LoadNarc(BinaryReader reader)
        {
            reader.BaseStream.Seek(4L, SeekOrigin.Begin);
            magic = reader.ReadInt32();
            narcSize = reader.ReadUInt32();
            sectionSize = reader.ReadInt16();
            sectionNum = reader.ReadInt16();
            reader.ReadInt32();
            fatbSize = reader.ReadUInt32();
            fatbNum = reader.ReadUInt32();
            fatb = new NarcFat_s();
            uint fatbStartOffset = (uint)reader.BaseStream.Position;
            fatb.ReadFat(reader, fatbNum);
            reader.ReadInt32();
            fntbSize = reader.ReadUInt32();
            fntbStartOffset = (uint)reader.BaseStream.Position;
            fntb = new NarcFnt_s();
            treeSystem = fntb.ReadFnt(reader, fntbSize, fntbStartOffset, fatb);
            reader.BaseStream.Position = fntbStartOffset + fntbSize;
            figm.magicId = reader.ReadChars(4);
            figm.sizeFigm = reader.ReadInt32();
            figm.startFigm = (int)reader.BaseStream.Position;
            figm.fileData = new List<ClosableMemoryStream>();
            for (int fileCounter = 0; fileCounter < fatbNum; fileCounter++)
            {
                ClosableMemoryStream actualFileStream = new ClosableMemoryStream();
                //MessageBox.Show(fntb.getpad().ToString());
                reader.BaseStream.Position = fatb.getFileStartAt(fileCounter) + fntbStartOffset + fntbSize;
                Utils.loadStream(reader, fatb.getFileEndAt(fileCounter) - fatb.getFileStartAt(fileCounter), actualFileStream);
                figm.fileData.Add(actualFileStream);
            }
            return this;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Figm_s
        {
            public int sizeFigm;
            public int startFigm;
            public char[] magicId;
            public List<ClosableMemoryStream> fileData;
        }




        public class NarcFat_s
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

            public MemoryStream getFileStreamAt(int i)
            {
                return FileOff[i].File_Data;
            }

            public void ReadFat(BinaryReader reader, uint num)
            {
                FileOff = new List<FileOff_s>();
                for (int i = 0; i < num; i++)
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

            public void setFileOff(List<FileOff_s> input, uint pad)
            {
                for (int i = 0; i < input.Count; i++)
                {
                      setFileOffAt(i, FileOff[i], pad);
                }
            }

            public void setFileOffAt(int i, FileOff_s input, uint pad)
            {
                FileOff_s _s = FileOff[i];
                _s.File_Start = input.File_Start + pad;
                _s.File_End = input.File_End + pad;
                FileOff[i] = _s;
            }

            public void setFileStreamAt(int i, ClosableMemoryStream input)
            {
                FileOff_s _s = FileOff[i];
                _s.File_Data = new ClosableMemoryStream();
                input.WriteTo(_s.File_Data);
                FileOff[i] = _s;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct FileOff_s
            {
                public uint File_Start;
                public uint File_End;
                public MemoryStream File_Data;
            }
        }

        public class NarcFnt_s
        {
            private static byte[] Fnt_Data;
            private static List<MainDir_s> MainDir;
            private int pad;
            private static uint SubDir_Off;

            public Common.FileInfo_s getFileName(int i)
            {
                return MainDir[0].SubDir.Files[i];
            }

            public int getFntLenght()
            {
                return Fnt_Data.Length;
            }

            public uint getMainStart()
            {
                return MainDir[0].startOffset;
            }

            public int getpad()
            {
                return   pad;
            }

            public static Common.Folder_s Order_Folder(List<MainDir_s> tables, int idFolder, string nameFolder)
            {
                Common.Folder_s _s = new Common.Folder_s {
                    Name = nameFolder,
                    ID = (ushort) idFolder,
                    Files = tables[idFolder & 0xfff].SubDir.Files
                };
                if (_s.Files != null)
                {
                    for (int i = 0; i < _s.Files.Count; i++)
                    {
                        Common.FileInfo_s _s2 = _s.Files[i];
                        _s2.Parent_ID = (ushort) idFolder;
                        _s.Files[i] = _s2;
                    }
                }
                if (tables[idFolder & 0xfff].SubDir.Folders != null)
                {
                    _s.Folders = new List<Common.Folder_s>();
                    foreach (Common.Folder_s _s3 in tables[idFolder & 0xfff].SubDir.Folders)
                    {
                        _s.Folders.Add(Order_Folder(tables, (int) _s3.ID, _s3.Name));
                    }
                }
                return _s;
            }

            public Common.Folder_s ReadFnt(BinaryReader reader, uint size, uint offset, NarcFat_s FAT)
            {
                Common.Folder_s folderStruct = new Common.Folder_s();
                long position = reader.BaseStream.Position;
                Fnt_Data = new byte[size];
                Fnt_Data = reader.ReadBytes((int) size);
                reader.BaseStream.Position = offset;
                int index = 0;
                SubDir_Off = reader.ReadUInt32();
                reader.BaseStream.Position = offset;
                MainDir = new List<MainDir_s>();
                List<Common.FileInfo_s> fileInfoList = new List<Common.FileInfo_s>();
                while (reader.BaseStream.Position < (position + SubDir_Off))
                {
                    MainDir_s actualDir = new MainDir_s {
                        startOffset = reader.ReadUInt32(),
                        firstFileId = reader.ReadUInt16(),
                        parentId = reader.ReadUInt16()
                    };
                    long num3 = reader.BaseStream.Position;
                    reader.BaseStream.Position = offset + actualDir.startOffset;
                    Array.Resize<byte>(ref actualDir.SubDir.Type2, index + 1);
                    actualDir.SubDir.Type2[index] = reader.ReadByte();
                    actualDir.SubDir.Type = actualDir.SubDir.Type2[index];
                    actualDir.SubDir.Parent_ID = actualDir.firstFileId;
                    while (actualDir.SubDir.Type != 0)
                    {
                        int type;
                        if (actualDir.SubDir.Type < 0x80)
                        {
                            Common.FileInfo_s _s3 = new Common.FileInfo_s();
                            if (actualDir.SubDir.Files == null)
                            {
                                actualDir.SubDir.Files = new List<Common.FileInfo_s>();
                            }
                            type = actualDir.SubDir.Type;
                            _s3.Name = new string(Encoding.GetEncoding("shift_jis").GetChars(reader.ReadBytes(type)));
                              pad += _s3.Name.Length;
                            _s3.ID = actualDir.firstFileId;
                            _s3.Offset = FAT.getFileStartAt((int) _s3.ID);
                            _s3.Size = FAT.getFileEndAt((int) _s3.ID) - _s3.Offset;
                            long num5 = reader.BaseStream.Position;
                            reader.BaseStream.Seek((long) _s3.Offset, SeekOrigin.Begin);
                            _s3.FileData = new ClosableMemoryStream();
                            Utils.loadStream(reader, _s3.Size, _s3.FileData);
                            reader.BaseStream.Seek((long) _s3.Offset, SeekOrigin.Begin);
                            FAT.setFileStreamAt((int) _s3.ID, _s3.FileData);
                            reader.BaseStream.Seek(num5, SeekOrigin.Begin);
                            actualDir.firstFileId = (ushort) (actualDir.firstFileId + 1);
                            actualDir.SubDir.Files.Add(_s3);
                            fileInfoList.Add(_s3);
                        }
                        if (actualDir.SubDir.Type > 0x80)
                        {
                            Common.Folder_s _s4 = new Common.Folder_s();
                            if (actualDir.SubDir.Folders == null)
                            {
                                actualDir.SubDir.Folders = new List<Common.Folder_s>();
                            }
                            type = actualDir.SubDir.Type - 0x80;
                            _s4.Name = new string(Encoding.GetEncoding("shift_jis").GetChars(reader.ReadBytes(type)));
                              pad += _s4.Name.Length;
                            _s4.ID = reader.ReadUInt16();
                            actualDir.SubDir.Folders.Add(_s4);
                        }
                        index++;
                        Array.Resize<byte>(ref actualDir.SubDir.Type2, index + 1);
                        actualDir.SubDir.Type2[index] = reader.ReadByte();
                          pad++;
                        actualDir.SubDir.Type = actualDir.SubDir.Type2[index];
                    }
                    MainDir.Add(actualDir);
                    reader.BaseStream.Position = num3;
                }
                folderStruct = Order_Folder(MainDir, 0, "root");
                folderStruct.ID = 0xf000;
                return folderStruct;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct MainDir_s
            {
                public NarcFnt_s.SubDir_s SubDir;
                public uint startOffset;
                public ushort firstFileId;
                public ushort parentId;
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
        }
    }
}

