namespace PG4Map.Formats
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    public class Nsbtx
    {
        public const int BTX0 = 0x30585442;
        public int idBTX0;
        public int idTEX0;
        public short version;
        public int fileSize;
        public short sectionNum;
        public int startOffset;
        public Nsbtx.TEX0Struct tex0;
        public static int type;
        public static byte[] texStream;

        public List<NsbmdModel.MatTexPalStruct> LoadBTX0(ClosableMemoryStream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            return LoadBTX0(reader);
        }

        public List<NsbmdModel.MatTexPalStruct> LoadBTX0(FileInfo fileInfo)
        {
            using (FileStream stream = new FileStream(fileInfo.FullName, FileMode.Open))
                return LoadBTX0(stream);
        }

        public List<NsbmdModel.MatTexPalStruct> LoadBTX0(FileStream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            return LoadBTX0(reader);
        }

        public List<NsbmdModel.MatTexPalStruct> LoadBTX0(BinaryReader reader)
        {
            reader.BaseStream.Seek(0L, SeekOrigin.Begin);
            tex0.matTexPalList = new List<NsbmdModel.MatTexPalStruct>();
            idBTX0 = reader.ReadInt32();
            if (idBTX0 != BTX0)
                throw new Exception();
            reader.BaseStream.Seek(2L, SeekOrigin.Current);
            version = reader.ReadInt16();
            fileSize = reader.ReadInt32();
            reader.BaseStream.Seek(2L, SeekOrigin.Current);
            sectionNum = reader.ReadInt16();
            startOffset = reader.ReadInt32();
            idTEX0 = reader.ReadInt32();
            tex0.matTexPalList.AddRange(ReadTex0(reader.BaseStream, startOffset, 0));
            reader.BaseStream.Close();
            return tex0.matTexPalList;
        }

        public void SaveTex0(BinaryWriter writer)
        {
            writer.Write(texStream);
        }
  

        public List<NsbmdModel.MatTexPalStruct> ReadTex0(Stream stream, int startOffset, int flag)
        {
            BinaryReader reader = new BinaryReader(stream);
            if (reader.BaseStream.Position == stream.Length)
                return null;
            if (flag==1)
                reader.BaseStream.Seek(8L, SeekOrigin.Begin);
            tex0.startOffset = startOffset + 4;
            texStream = reader.ReadBytes((int)stream.Length - startOffset);
            reader.BaseStream.Position = startOffset + 4;
            tex0.blockSize = (int) reader.ReadUInt32();           
            tex0.endOffset = tex0.blockSize + startOffset;
            stream.Skip((uint) 4);
            tex0.texDataSize = (ushort) (reader.ReadUInt16() << 3);
            tex0.texInfoOffset = reader.ReadUInt16();
            stream.Skip((uint) 4);
            tex0.texDataOffset = reader.ReadUInt32() + ((uint) startOffset);
            stream.Skip((uint) 4);
            tex0.spTexSize = (ushort) (reader.ReadUInt16() << 3);
            tex0.spInfoOffset = reader.ReadUInt16();
            stream.Skip((uint) 4);
            tex0.spDataOffset = reader.ReadUInt32() + ((uint) startOffset);
            tex0.spDataInfoOffset = reader.ReadUInt32() + ((uint) startOffset);
            stream.Skip((uint) 4);
            tex0.palDataSize = reader.ReadUInt16() << 3;
            tex0.palDataInfo = reader.ReadUInt16();
            tex0.palDefinitionOffset = reader.ReadUInt32() + ((uint) startOffset);
            tex0.palDataOffset = reader.ReadUInt32() + ((uint) startOffset);
            stream.Skip((uint) 1);
            readTexture(reader);
            readPalette(reader);
            makeMatTex();
            fixStuff(stream, reader);
            return tex0.matTexPalList;
        }

        private void fixStuff(Stream stream, BinaryReader reader)
        {
            stream.Seek((long)(tex0.palDefinitionOffset + 2), SeekOrigin.Begin);
            stream.Seek((long)(14 + (tex0.palNum * 4)), SeekOrigin.Current);
            for (int palCounter = 0; palCounter < tex0.palNum; palCounter++)
            {
                if (palCounter < tex0.matTexPalList.Count)
                {
                    uint palOffset = ((uint)(reader.ReadUInt16() << 3)) + tex0.palDataOffset;
                    stream.Seek(2L, SeekOrigin.Current);
                    var actualMatTexPal = tex0.matTexPalList[palCounter];
                    actualMatTexPal.palOffset = palOffset;
                    actualMatTexPal.palSize = ((uint)tex0.endOffset) - actualMatTexPal.palOffset;
                    tex0.matTexPalList[palCounter] = actualMatTexPal;
                }
            }
            for (int texCounter = 0; texCounter < tex0.texNum; texCounter++)
            {
                var actualMatTex = tex0.matTexPalList[texCounter];
                stream.Seek((long)actualMatTex.texOffset, SeekOrigin.Begin);
                actualMatTex.texData = reader.ReadBytes((int)actualMatTex.texSize);
                uint spDataInfoOffsetInc = actualMatTex.texSize / 0x100;
                stream.Seek((long)tex0.spDataInfoOffset, SeekOrigin.Begin);
                actualMatTex.spData = reader.ReadBytes((int)spDataInfoOffsetInc);
                tex0.spDataInfoOffset += spDataInfoOffsetInc;
                tex0.matTexPalList[texCounter] = actualMatTex;
            }
            for (int palCounter = 0; palCounter < tex0.palNum; palCounter++)
            {
                if (palCounter < tex0.matTexPalList.Count)
                {
                    var actualMatTexPal = tex0.matTexPalList[palCounter];
                    uint palDataSize = actualMatTexPal.palSize >> 1;
                    RGBA[] palDataArray = new RGBA[palDataSize];
    
                    stream.Seek((long)actualMatTexPal.palOffset, SeekOrigin.Begin);
                    for (int i = 0; i < palDataSize-1; i++)
                    {
                        ushort actualPalData = reader.ReadUInt16();
                        palDataArray[i].R = (byte)((actualPalData & 0x1f) << 3);
                        palDataArray[i].G = (byte)(((actualPalData >> 5) & 0x1f) << 3);
                        palDataArray[i].B = (byte)(((actualPalData >> 10) & 0x1f) << 3);
                        palDataArray[i].A = 0xff;
                    }
                    actualMatTexPal.palData = palDataArray;
                    tex0.matTexPalList[palCounter] = actualMatTexPal;
                }
            }
        }

        private void makeMatTex()
        {
            tex0.matTexPalList = new List<NsbmdModel.MatTexPalStruct>();
            for (int texCounter = 0; texCounter < tex0.texNum; texCounter++)
            {
                var actualTexInfo = tex0.texInfoArray[texCounter];
                NsbmdModel.MatTexPalStruct actualMatTex = new NsbmdModel.MatTexPalStruct
                {
                    heigth = actualTexInfo.heigth,
                    format = actualTexInfo.format
                };
                if (actualMatTex.format == 5)
                    actualMatTex.texOffset = (uint)(actualTexInfo.vramOffset + tex0.spTexOff);
                else
                    actualMatTex.texOffset = ((uint)actualTexInfo.vramOffset) + tex0.texDataOffset;
                actualMatTex.width = actualTexInfo.width;
                actualMatTex.flipS = actualTexInfo.flipS;
                actualMatTex.flipT = actualTexInfo.flipT;
                actualMatTex.repeatS = actualTexInfo.repeatS;
                actualMatTex.repeatT = actualTexInfo.repeatT;
                actualMatTex.transFlag = actualTexInfo.transFlag;
                actualMatTex.texDataOffset = tex0.texDataOffset;
                actualMatTex.texName = tex0.texNameArray[texCounter];
                actualMatTex.palName = tex0.palNameArray[texCounter];
                actualMatTex.palMatId = (uint)actualTexInfo.palId;
                if (texCounter < tex0.palInfoArray.Length)
                    actualMatTex.palOffset = (uint)tex0.palInfoArray[texCounter];
                actualMatTex.colorBase = actualTexInfo.colorBase;
                int[] dimList = new int[] { 0, 8, 2, 4, 8, 2, 8, 0x10 };
                actualMatTex.texSize = (uint)(((actualMatTex.width * actualMatTex.heigth) * dimList[actualMatTex.format]) / 8);
                actualMatTex.palSize = ((uint)tex0.endOffset) - actualMatTex.palOffset;
                tex0.matTexPalList.Add(actualMatTex);
            }
        }

        private void readPalette(BinaryReader reader)
        {
            tex0.palNum = reader.ReadByte();
            tex0.startOffset = (int)reader.BaseStream.Position;
            tex0.palSection = reader.ReadInt16();
            tex0.palHeaderSize = reader.ReadInt16();
            tex0.palSectionSize = reader.ReadInt16();
            reader.ReadInt16();
            tex0.palDataArray = new int[tex0.palNum];
            for (int palCounter = 0; palCounter < tex0.palNum; palCounter++)
            {
                Array.Resize<int>(ref tex0.palDataArray, palCounter + 1);
                tex0.palDataArray[palCounter] = reader.ReadInt32();
            }
            tex0.palHeaderSize2 = reader.ReadInt16();
            tex0.palSectionSize2 = reader.ReadInt16();
            tex0.palInfoArray = new int[tex0.palNum];
            for (int palCounter = 0; palCounter < tex0.palNum; palCounter++)
            {
                Array.Resize<int>(ref tex0.palInfoArray, palCounter + 1);
                tex0.palInfoArray[palCounter] = reader.ReadInt32();
            }
            reader.ReadInt16();
            tex0.palNameArray = new string[tex0.texNum];
            for (int texCounter = 0; texCounter < tex0.texNum; texCounter++)
            {
                texCounter = 0;
                while (texCounter < tex0.texNum)
                {
                    if (texCounter < tex0.palNum)
                        tex0.palNameArray[texCounter] =  Encoding.UTF8.GetString(reader.ReadBytes(0x10)).Split('\0')[0];
                    else
                        tex0.palNameArray[texCounter] = "";
                    texCounter++;
                }
            }
        }

        private void readTexture(BinaryReader reader)
        {
            tex0.texNum = reader.ReadByte();
            tex0.startOffset = (int)reader.BaseStream.Position;
            tex0.texSection = reader.ReadInt16();
            tex0.texHeaderSize = reader.ReadInt16();
            tex0.texSectionSize = reader.ReadInt16();
            reader.ReadInt16();
            tex0.texDataArray = new int[tex0.texNum];
            for (int texCounter = 0; texCounter < tex0.texNum; texCounter++)
            {
                Array.Resize<int>(ref tex0.texDataArray, texCounter + 1);
                tex0.texDataArray[texCounter] = reader.ReadInt32();
            }
            tex0.texHeaderSize2 = reader.ReadInt16();
            tex0.texSection2Size = reader.ReadInt16();
            tex0.texInfoArray = new TexInfo_c[tex0.texNum];
            reader.ReadInt16();
            for (int texCounter = 0; texCounter < tex0.texNum; texCounter++)
            {
                Array.Resize<TexInfo_c>(ref tex0.texInfoArray, texCounter + 1);
                tex0.texInfoArray[texCounter].vramOffset = reader.ReadUInt16() << 3;
                short texInfoData = (short)(reader.ReadInt16() + 3);
                tex0.texInfoArray[texCounter].palId = reader.ReadInt32();
                tex0.texInfoArray[texCounter].repeatS = (texInfoData >> 15) & 1;
                tex0.texInfoArray[texCounter].repeatT = (texInfoData >> 14) & 1;
                tex0.texInfoArray[texCounter].flipS = (texInfoData >> 13) & 1;
                tex0.texInfoArray[texCounter].flipT = (texInfoData >> 12) & 1;
                tex0.texInfoArray[texCounter].format = (texInfoData >> 10) & 7;
                tex0.texInfoArray[texCounter].width = ((int)8) << ((texInfoData >> 4) & 7);
                tex0.texInfoArray[texCounter].heigth = ((int)8) << ((texInfoData >> 7) & 7);
                tex0.texInfoArray[texCounter].transFlag = texInfoData & 3;
                tex0.texInfoArray[texCounter].colorBase = (texInfoData >> 13) & 1;
            }
            tex0.texNameArray = new string[tex0.texNum];
            for (int texCounter = 0; texCounter < tex0.texNum; texCounter++)
                tex0.texNameArray[texCounter] =Encoding.UTF8.GetString(reader.ReadBytes(0x10)).Split(new char[1])[0];
            reader.BaseStream.Skip((uint)1);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TEX0Struct
        {
            public int blockSize;
            public int startOffset;
            public int endOffset;
            public int texNum;
            public ushort texDataSize;
            public ushort texInfoOffset;
            public uint texDataOffset;
            public ushort spTexOff;
            public ushort spInfoOffset;
            public ushort spTexSize;
            public uint spDataOffset;
            public uint spDataInfoOffset;
            public int texSection;
            public int texHeaderSize;
            public int texSectionSize;
            public int[] texDataArray;
            public int texHeaderSize2;
            public int texSection2Size;
            public Nsbtx.TexInfo_c[] texInfoArray;
            public string[] texNameArray;
            public int palSection;
            public int palHeaderSize;
            public int palSectionSize;
            public int[] palDataArray;
            public int palHeaderSize2;
            public int palSectionSize2;
            public int[] palInfoArray;
            public string[] palNameArray;
            public int palNum;
            public int palDataSize;
            public ushort palDataInfo;
            public uint palDefinitionOffset;
            public uint palDataOffset;
            public List<NsbmdModel.MatTexPalStruct> matTexPalList;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TexInfo_c
        {
            public int vramOffset;
            public int palId;
            public int repeatS;
            public int repeatT;
            public int flipS;
            public int flipT;
            public int heigth;
            public int width;
            public int format;
            public int colorBase;
            public int transFlag;
        }


    }
}

