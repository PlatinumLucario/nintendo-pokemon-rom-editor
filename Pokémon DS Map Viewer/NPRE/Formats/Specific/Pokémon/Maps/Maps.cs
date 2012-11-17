namespace PG4Map
{
    using System;
    using System.IO;
    using PG4Map.Formats;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class Maps
    {

        #region Constants
        public const short GENERAL_DIRECTORY = 0;
        public const short NARC_FILE = 1;
        public const short AB_FILE = 2;
        public const short DPMAP = 0;
        public const short HGSSMAP = 2;
        public const short BWMAP = 3;
        public const short BW2MAP = 4;
        public const short PLMAP = 1;
        public const short NSBMD_MODEL = 5;
        public const short MAXMOVSIZE = 32;
        #endregion

        public int size;
        public static int type;
        public ClosableMemoryStream streamUnknownSection;
        public ClosableMemoryStream streamObject;
        public ClosableMemoryStream streamNSBMD;
        public ClosableMemoryStream streamBDHC;
        public ClosableMemoryStream streamMovement;
        public List<Obj_S> listObjects;
        public List<Obj_S> listObjectsBW;
        public Nsbmd actualModel;
        public Move_s_bw[] arrayMovementBW;
        public Move_s[] arrayMovement;
        public short[] unknownSection;
        public PkmnMapHeader mapHeader;
        public PkmnMapHeader_Hg mapHeaderHG;
        public PkmnMapHeader_Bw mapHeaderBW;
        public List<Nsbmd.Table_s> associationTable;
        public string matrixName;
        public List<int> matrixCells;
        public string[] matrixInfo;

        [StructLayout(LayoutKind.Sequential)]
        public struct Move_s
        {
            public int actualMov;
            public int actualFlag;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Move_s_bw
        {
            public int actualMov;
            public int par2;
            public int par3;
            public int par;
            public int actualFlag;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Obj_S
        {
            public int idObject;
            public int xBig;
            public int yBig;
            public int zBig;
            public int xSmall;
            public int ySmall;
            public int zSmall;
            public int heightObject;
            public int lengthObject;
            public int widthObject;
            public int firstParameter;
            public int secondParameter;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Table_s
        {
            public int materialAss;
            public int polygonAss;
        }

        #region Loaders
        public void LoadMap(FileInfo file, Boolean isNarc, Boolean isMap, Boolean isModel)
        {
            FileStream input = new FileStream(file.FullName, FileMode.Open);
            var reader = new BinaryReader(input);
            LoadMap(reader, true);
        }

        private void LoadGenericNSBMD(BinaryReader reader)
        {
            PkmnMapDemuxer demuxer;
            Nsbmd nsbmd;
            List<NsbmdModel.MatTexPalStruct> list;
            Nsbtx.type = NSBMD_MODEL;
            demuxer = new PkmnMapDemuxer(reader, 1);
            PkmnMapHeader map = new PkmnMapHeader();
            streamNSBMD = new ClosableMemoryStream();
            new BinaryWriter(streamNSBMD).Write(demuxer.DemuxBMDBytes(map, 1));
            nsbmd = new Nsbmd();
            var reader2 = new BinaryReader(streamNSBMD);
            nsbmd.LoadBMD0(reader2,0);
            actualModel = nsbmd;
            list = null;
            list = nsbmd.getMaterials();
            if (actualModel.actualTex != null)
            {
                nsbmd.MatchTextures();
            }
        }

        private void LoadMapBW(BinaryReader reader)
        {
            Nsbmd nsbmd;
            BinaryWriter writer;
            string[] materialListForNsbmd;
            PkmnMapDemuxer_Bw bw = new PkmnMapDemuxer_Bw(reader, type);
            mapHeaderBW = PkmnMapHeader_Bw.FromReader(reader);
            streamNSBMD = new ClosableMemoryStream();
            writer = new BinaryWriter(streamNSBMD);
            writer.Write(bw.DemuxBMDBytes(mapHeaderBW, 0));
            streamMovement = new ClosableMemoryStream();
            writer = new BinaryWriter(streamMovement);
            writer.Write(bw.DemuxMovBytes(mapHeaderBW));
            arrayMovementBW = MovLoader.LoadMov_Bw(streamMovement);
            streamObject = new ClosableMemoryStream();
            writer = new BinaryWriter(streamObject);
            writer.Write(bw.DemuxObjBytes(mapHeaderBW));
            listObjectsBW = ObjLoader.LoadObj_Bw(streamObject);
            nsbmd = new Nsbmd();
            var reader2 = new BinaryReader(streamNSBMD);
            reader2.BaseStream.Position = 0;
            nsbmd.LoadBMD0(reader2, (int)mapHeaderBW.BMDOffset);
            actualModel = nsbmd;
            getMatrixInfo();
            materialListForNsbmd = actualModel.getTexNameArray();
            if (actualModel.actualTex != null)
                nsbmd.MatchTextures();
        }

        private void getMatrixInfo()
        {
            matrixInfo = actualModel.getMDL0at(0).model.modelNameList[0].name.Split('_');
            if (matrixInfo.Length > 1 && matrixInfo[0].Length > 1)
            {
                matrixName = matrixInfo[0].Remove(matrixInfo[0].Length - 2, 2);
                matrixInfo[matrixInfo.Length - 1] = matrixInfo[matrixInfo.Length - 1].TrimEnd('c', '\0');
                matrixCells = new List<int>();
                try
                {
                    matrixCells.Add(Int32.Parse(matrixInfo[0].Remove(0, matrixInfo[0].Length - 2)));
                    for (int cellCounter = 1; cellCounter < matrixInfo.Length; cellCounter++)
                        matrixCells.Add(Int32.Parse(matrixInfo[cellCounter]));
                }
                catch
                {
                }
            }
            else if (matrixInfo[0].Length > 1)
            {
                matrixName = matrixInfo[0].TrimEnd('c', '\0');
                matrixName = matrixName.Remove(matrixName.Length - 2, 2);
                matrixInfo[matrixInfo.Length - 1] = matrixInfo[matrixInfo.Length - 1].TrimEnd('c', '\0');
                matrixCells = new List<int>();
                try
                {
                    matrixCells.Add(Int32.Parse(matrixInfo[0].Remove(0, matrixInfo[0].Length - 2)));
                    for (int cellCounter = 1; cellCounter < matrixInfo.Length; cellCounter++)
                        matrixCells.Add(Int32.Parse(matrixInfo[cellCounter]));
                }
                catch
                {
                }
            }
            else
            {
                matrixName = matrixInfo[0] + "_" + matrixInfo[1] + "_";
                matrixInfo[matrixInfo.Length - 1] = matrixInfo[matrixInfo.Length - 1].TrimEnd('c', '\0');
                matrixCells = new List<int>();
                try
                {
                    for (int cellCounter = 2; cellCounter < matrixInfo.Length; cellCounter++)
                        matrixCells.Add(Int32.Parse(matrixInfo[cellCounter]));
                }
                catch
                {
                }
            }
        }

        private void LoadMapHgss(BinaryReader reader)
        {
            Nsbmd nsbmd;
            BinaryWriter writer;
            string[] materialListForNsbmd;
            PkmnMapDemuxer_Hg hg = new PkmnMapDemuxer_Hg(reader, type);
            mapHeaderHG = PkmnMapHeader_Hg.FromReader(reader);
            streamUnknownSection = new ClosableMemoryStream();
            writer = new BinaryWriter(streamUnknownSection);
            writer.Write(hg.DemuxS0Bytes(mapHeaderHG));
            unknownSection = S0Loader.LoadS0(streamUnknownSection);
            streamMovement = new ClosableMemoryStream();
            writer = new BinaryWriter(streamMovement);
            writer.Write(hg.DemuxMovBytes(mapHeaderHG));
            arrayMovement = MovLoader.LoadMov(streamMovement);
            streamObject = new ClosableMemoryStream();
            writer = new BinaryWriter(streamObject);
            writer.Write(hg.DemuxObjBytes(mapHeaderHG));
            listObjects = ObjLoader.LoadObj(streamObject);
            streamNSBMD = new ClosableMemoryStream();
            writer = new BinaryWriter(streamNSBMD);
            writer.Write(hg.DemuxBMDBytes(mapHeaderHG, 0));
            streamBDHC = new ClosableMemoryStream();
            writer = new BinaryWriter(streamBDHC);
            writer.Write(hg.DemuxBdhcBytes(mapHeaderHG));
            nsbmd = new Nsbmd();
            var reader2 = new BinaryReader(streamNSBMD);
            nsbmd.LoadBMD0(reader2, (int)mapHeaderHG.BMDOffset);
            actualModel = nsbmd;
            getMatrixInfo();
            materialListForNsbmd = actualModel.getTexNameArray();
            if (actualModel.actualTex != null)
                nsbmd.MatchTextures();
        }

        private void LoadMapDPP(BinaryReader reader)
        {
            Nsbmd nsbmd;
            BinaryWriter writer;
            string[] materialListForNsbmd;
            PkmnMapDemuxer demuxer = new PkmnMapDemuxer(reader, type);
            mapHeader = PkmnMapHeader.FromReader(reader);
            streamMovement = new ClosableMemoryStream();
            writer = new BinaryWriter(streamMovement);
            writer.Write(demuxer.DemuxMovBytes(mapHeader));
            arrayMovement = MovLoader.LoadMov(streamMovement);
            streamObject = new ClosableMemoryStream();
            writer = new BinaryWriter(streamObject);
            writer.Write(demuxer.DemuxObjBytes(mapHeader));
            listObjects = ObjLoader.LoadObj(streamObject);
            streamNSBMD = new ClosableMemoryStream();
            writer = new BinaryWriter(streamNSBMD);
            writer.Write(demuxer.DemuxBMDBytes(mapHeader, 0));
            streamBDHC = new ClosableMemoryStream();
            writer = new BinaryWriter(streamBDHC);
            writer.Write(demuxer.DemuxBdhcBytes(mapHeader));
            nsbmd = new Nsbmd();
            var reader2 = new BinaryReader(streamNSBMD);
            nsbmd.LoadBMD0(reader2, (int)mapHeader.BMDOffset);
            actualModel = nsbmd;
            getMatrixInfo();
            materialListForNsbmd = actualModel.getTexNameArray();
            if (actualModel.actualTex != null)
                nsbmd.MatchTextures();
        }

        public void LoadMap(BinaryReader reader, Boolean isFirst)
        {
            reader.BaseStream.Seek(0L, SeekOrigin.Begin);
            if (reader.BaseStream.Length >= 4L)
            {
                size = reader.ReadInt32();
                reader.BaseStream.Position = 0L;
                IsBWDialog dialog = new IsBWDialog();
                if (isFirst)
                    dialog.ShowDialog();
                if (type != NSBMD_MODEL)
                {
                    if (isFirst)
                        type = dialog.CheckGame(); 
                    Nsbtx.type = type;
                    if (type == DPMAP|| type ==PLMAP)
                        LoadMapDPP(reader);
                    else if (type == HGSSMAP)
                        LoadMapHgss(reader);
                    else if (type == BWMAP || type == BW2MAP)
                        LoadMapBW(reader);
                }
                else
                    LoadGenericNSBMD(reader);
            }
        }
        #endregion

        #region Save Methods
        public void SaveMov(BinaryWriter writer)
        {
            for (int i = 0; i < arrayMovement.Length; i++)
            {
                writer.Write((byte)arrayMovement[i].actualMov);
                writer.Write((byte)arrayMovement[i].actualFlag);
            }
        }

        public void SaveMov_Bw(BinaryWriter writer)
        {
            for (int i = 0; i < arrayMovementBW.Length; i++)
            {
                writer.Write((short)arrayMovementBW[i].actualMov);
                writer.Write((short)arrayMovementBW[i].par);
                writer.Write((short)arrayMovementBW[i].par2);
                writer.Write((short)arrayMovementBW[i].actualFlag);
            }
            writer.Write(arrayMovementBW[0].par3);
        }

        public void SaveObj(BinaryWriter writer)
        {
            for (int i = 0; i < listObjects.Count; i++)
            {
                writer.Write(listObjects[i].idObject);
                writer.Write((short)listObjects[i].ySmall);
                writer.Write((short)listObjects[i].yBig);
                writer.Write((short)listObjects[i].zSmall);
                writer.Write((short)listObjects[i].zBig);
                writer.Write((short)listObjects[i].xSmall);
                writer.Write((short)listObjects[i].xBig);
                writer.Write((long)0L);
                writer.Write(0);
                writer.Write((byte)0);
                writer.Write((short)listObjects[i].widthObject);
                writer.Write((short)0);
                writer.Write((short)listObjects[i].lengthObject);
                writer.Write((short)0);
                writer.Write((short)listObjects[i].heightObject);
                writer.Write((byte)0);
                writer.Write((long)0L);
            }
        }

        public void SaveObj_bw(BinaryWriter writer)
        {
            if (listObjectsBW.Count > 0)
            {
                writer.Write((Int32)listObjectsBW[0].firstParameter);
            }
            for (int i = 0; i < listObjectsBW.Count; i++)
            {
                writer.Write((short)listObjectsBW[i].ySmall);
                writer.Write((short)listObjectsBW[i].yBig);
                writer.Write((short)listObjectsBW[i].zSmall);
                writer.Write((short)listObjectsBW[i].zBig);
                writer.Write((short)listObjectsBW[i].xSmall);
                writer.Write((short)listObjectsBW[i].xBig);
                writer.Write((short)listObjectsBW[i].secondParameter);
                writer.Write((byte)0);
                writer.Write((byte)listObjectsBW[i].idObject);
            }
        }
    }
#endregion
}

