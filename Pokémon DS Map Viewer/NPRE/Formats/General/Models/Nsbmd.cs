namespace PG4Map.Formats
{
    using PG4Map;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using PG4Map.Formats;
    using System.Windows.Forms;

    public class Nsbmd
    {
        #region Constants
        public const int BMD0 = 0x30444d42;
        public const int BTX0 = 0x30585442;
        public const int MAGIC1 = 0x1feff;
        public const int MAGIC2 = 0x2feff;
        public const int MDL0 = 0x304c444d;
        public const int TEX0 = 0x30584554;
        #endregion

        public Nsbtx actualTex;
        public List<Table_s> assTableList;
        private static int[] id;
        public List<NsbmdModel.MatTexPalStruct> matTexPalList;
        public NsbmdModel[] models;
        private static int gCurrentVertex = 0;
        private bool gOptTexture = true;
        private bool gOptVertexMode = false;
        private bool gOptWireFrame = false;
        public int sectionNum;
        public int[] sectionData; 
        public int removedSize;
        public int sizeBMD0;
        private static int type;
        private static int versionBMD0;
        private int startMaterials;
        private int polygonLength;
        private static int unkBuffer;
        private static int unkBuffer2;
        public int padOffset;
        public List<Sbc_s> sbc;
        public int addedSize;
        private int zBuffer;
        private int xBuffer;
        private int position;
        private int yBuffer;


        #region Getters
        public List<NsbmdModel.MatTexPalStruct> getMaterials()
        {
            return matTexPalList;
        }

        public NsbmdModel[] getModelsList()
        {
            return models;
        }

        public NsbmdModel getMDL0at(int i)
        {
            return models[i];
        }

        public String[] getTexNameArray()
        {
            List<String> texNameList = getTexNameList();
            return texNameList.ToArray();
        }

        private List<String> getTexNameList()
        {
            List<String> texNameList = new List<String>();
            foreach (NsbmdModel model in getModelsList())
            {
                foreach (NsbmdModel.MatTexPalStruct matTexPalElement in model.matTexPalList)
                {
                    if (!String.IsNullOrEmpty(matTexPalElement.texName))
                    {
                        string[] texNameSplitted = matTexPalElement.texName.Split('\0');
                        texNameList.Add(texNameSplitted[0]);
                    }
                }
            }
            texNameList.Sort();
            return texNameList;
        }

        #endregion

        #region Loading Part

        public void LoadBMD0(BinaryReader reader, int padOffset)
        {
            reader.BaseStream.Seek(0L, SeekOrigin.Begin);
            this.padOffset = padOffset;
            type = reader.ReadInt32();
            if (type != BMD0)
            {
                MessageBox.Show("It's not a model!");
                return;
            }
            versionBMD0 = reader.ReadInt32();
            sizeBMD0 = reader.ReadInt32();
            reader.ReadInt16();
            sectionNum = reader.ReadInt16();
            if (sectionNum == 0)
            {
                MessageBox.Show("DEBUG: no of block zero.\n");
                return;
            }
            sectionData = new int[sectionNum];
            for (int sectionCounter = 0; sectionCounter < sectionNum; sectionCounter++)
                sectionData[sectionCounter] = reader.ReadInt32();
            id = new int[sectionNum];
            for (int sectionCounter = 0; sectionCounter < sectionNum; sectionCounter++)
            {
                reader.BaseStream.Position = sectionData[sectionCounter];
                id[sectionCounter] = reader.ReadInt32();
                switch (id[sectionCounter])
                {
                    case MDL0:
                        models = LoadMDL0(reader);
                        break;

                    case TEX0:
                        if (actualTex == null)
                        {
                            actualTex = new Nsbtx();
                            matTexPalList = actualTex.ReadTex0(reader.BaseStream, sectionData[sectionCounter], 0);
                            actualTex.tex0.matTexPalList = matTexPalList;
                        }

                        break;
                }
                if (actualTex == null && sectionCounter == sectionNum-1)
                {
                    string textureFileToUseForModel = Program.GetTextureFileToUseForModel(getTexNameArray(), Maps.type);
                    if (Program._textureDirectory.GetFiles(textureFileToUseForModel).Length > 0)
                    {
                        FileInfo info = Program._textureDirectory.GetFiles(textureFileToUseForModel)[0];
                        MapEditor.textureName = info.Name;
                        if (actualTex == null)
                        {
                            actualTex = new Nsbtx();
                            actualTex.LoadBTX0(info.Open(FileMode.Open));
                        }
                    }
                }
            }
        }

        private NsbmdModel[] LoadMDL0(BinaryReader reader)
        {
            List<NsbmdModel> modelsList = new List<NsbmdModel>();
            Int32 sizeMDL0 = reader.ReadInt32();
            reader.BaseStream.Position++;
            Byte modelNumber = reader.ReadByte();
            if (modelNumber <= 0)
                throw new Exception();
            for (int modelCounter = 0; modelCounter < modelNumber; modelCounter++)
                modelsList.Add(new NsbmdModel());

            readModel(reader, modelsList, sizeMDL0, modelNumber);
            long startModelInfo = reader.BaseStream.Position;
            readModelInfo(reader, modelsList);
            readNodes(reader, modelsList);

            //MapEditor.Console.AppendText("\nThere are " + modelsList[0].node.nodeNum + " node into this map");
            //for (int i = 0; i < modelsList[0].node.nodeNum; i++)
            //    MapEditor.Console.AppendText("\n\nThe name of node " + i + " is " + modelsList[0].node.nodeNameList[i].name);

            modelsList[0].sequenceBoneCommandList = new ArrayList();
            UInt32 sbcStart = (uint)reader.BaseStream.Position;
            UInt32 sbcSize = modelsList[0].modelInfo.matOffset - modelsList[0].modelInfo.sbcOffset;
            readSbc(reader, sbcStart, sbcSize, modelsList[0], 0);

            skip(reader, modelsList, startModelInfo);
            startMaterials = (int)reader.BaseStream.Position;
            modelsList[0].material.dictTexOffset = reader.ReadUInt16();
            modelsList[0].material.dictPalOffset = reader.ReadUInt16();
            readMaterials(reader, modelsList);

            //MapEditor.Console.AppendText("\n\nThere are " + modelsList[0].material.dictMat.matNum + " material into map");
            //for (int i = 0; i < modelsList[0].material.dictMat.matNum; i++)
            //    MapEditor.Console.AppendText("\nThe name of material " + i + " is" + modelsList[0].material.dictMat.matNameList[i].name);

            readTextures(reader, modelsList);

            //MapEditor.Console.AppendText("\n\nThere are " + modelsList[0].material.dictTex.texNum + " texture into map");
            //for (int i = 0; i < modelsList[0].material.dictTex.texNum; i++)
            //    MapEditor.Console.AppendText("\nThe name of texture " + i + " is" + modelsList[0].material.dictTex.texNameList[i].name);

            readPalette(reader, modelsList);

            //MapEditor.Console.AppendText("\n\nThere are " + modelsList[0].material.dictPal.palNum + " palette into map");
            //for (int i = 0; i < modelsList[0].material.dictPal.palNum; i++)
            //    MapEditor.Console.AppendText("\nThe name of palette " + i + " is" + modelsList[0].material.dictPal.palNameList[i].name);

            readAssMatrix(reader, modelsList, startMaterials);

            //MapEditor.Console.AppendText("\n\nThe TexIndexData length is " + modelsList[0].material.texIndexDataList.Count);
            //for (int i = 0; i < modelsList[0].material.texIndexDataList.Count; i++)
            //    MapEditor.Console.AppendText("At " + i + " is" + modelsList[0].material.texIndexDataList[i]);

            readMatInfo(reader, modelsList);
            readShape(reader, modelsList);
            var nameTextureList = makeNameTextureList(modelsList);
            var namePaletteList = makeNamePaletteList(modelsList);
            makeMatTex(modelsList, nameTextureList, namePaletteList);
            makeAssTable(modelsList);
            return modelsList.ToArray();
        }

        private void makeAssTable(List<NsbmdModel> modelsList)
        {
            assTableList = new List<Table_s>();
            var tabCounter = 0;
            int actualNode = 0;
            var sequenceBoneCommandList = modelsList[0].sequenceBoneCommandList;
            sbc = new List<Sbc_s>();
            for (int sbcCounter = 0; sbcCounter < sequenceBoneCommandList.Count; sbcCounter++)
            {
                var actualSequenceBoneCommand = new Sbc_s();
                var actualElement = sequenceBoneCommandList[sbcCounter];
                if ((sbcCounter + 1) < sequenceBoneCommandList.Count)
                {
                    actualSequenceBoneCommand.id = (Byte)actualElement;
                    if (actualElement.Equals((byte)0x1) || actualElement.Equals((byte)0x21) || actualElement.Equals((byte)0x41))
                    {

                        /*
                         * Return Command
                         */
                        sbcCounter++;

                    }
                    else if (actualElement.Equals((byte)0x2) || actualElement.Equals((byte)0x22) || actualElement.Equals((byte)0x42))
                    {

                        /*
                         * Node command.
                         * 1st par: Node Id.
                         * 2st par: O if invisible, visible either.
                         */
                        actualSequenceBoneCommand.parameters = new List<Byte>();
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 1]);
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 2]);
                        sbcCounter += 2;

                    }
                    else if (actualElement.Equals((byte)0x3) || actualElement.Equals((byte)0x23) || actualElement.Equals((byte)0x43))
                    {

                        /*
                         * Matrix Command
                         * Par: Id
                         */
                        actualSequenceBoneCommand.parameters = new List<Byte>();
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 1]);
                        sbcCounter++;

                    }
                    else if (actualElement.Equals((byte)0x4) || actualElement.Equals((byte)0x24) || actualElement.Equals((byte)0x44))
                    {
                        /*
                         * Material Paring commands
                         * Par: Material Id
                         */
                        actualSequenceBoneCommand.parameters = new List<Byte>();
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 1]);

                        Table_s actualSbc = new Table_s
                        {
                            pol = (byte)sequenceBoneCommandList[sbcCounter + 3],
                            mat = (byte)sequenceBoneCommandList[sbcCounter + 1],
                            node = actualNode
                        };
                          assTableList.Add(actualSbc);
                        tabCounter++;
                        sbcCounter += 1;
                    }
                    else if (actualElement.Equals((byte)0x5)||actualElement.Equals((byte)0x25))
                    {

                        /*
                         * Shape Pairing Command
                         * Par: Shape Id
                         */
                        actualSequenceBoneCommand.parameters = new List<Byte>();
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 1]);
                        sbcCounter++;

                    }
                    else if (actualElement.Equals((byte)0x6))
                    {

                        /*
                         * Node Description Command
                         * 1st par: Node Id
                         * 2nd par: Model Id
                         * 3st par: Dummy 0x0
                         */
                        actualSequenceBoneCommand.parameters = new List<Byte>();
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 1]);
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 2]);
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 3]);
                        sbcCounter += 3;
                    }
                    else if (actualElement.Equals((byte)0x7)){
                        
                        /*
                         * Node Description BB
                         * 1st par: Node Id
                         */
                        actualSequenceBoneCommand.parameters = new List<Byte>();
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 1]);
                        sbcCounter += 1;
                    }
                    else if (actualElement.Equals((byte)0x8))
                    {
                        /*
                        * Node Description BBY
                        * 1st par: Node Id
                        */
                        actualSequenceBoneCommand.parameters = new List<Byte>();
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 1]);
                        sbcCounter += 1;
                    }
                    else if (actualElement.Equals((byte)0x9) || actualElement.Equals((byte)0x29) || actualElement.Equals((byte)0x49))
                    {
                        /*
                        * Node Mix.
                        * 1st par: Store Stack Id
                        * 2st par: Number of Blending
                        * 3...N X 3 par: List of Blending (Sid, Mid, Float)
                        */
                        actualSequenceBoneCommand.parameters = new List<Byte>();
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 1]);
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 2]);
                        for (int i = sbcCounter + 2; i < (int)sequenceBoneCommandList[sbcCounter + 2]; i += 3)
                        {
                            actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[i + 1]);
                            actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[i + 2]);
                            actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[i + 3]);
                        }


                        sbcCounter += 2 + (byte)sequenceBoneCommandList[sbcCounter + 2] * 3;
                    }
                    else if (actualElement.Equals((byte)0xb))
                    {
                        /*
                        * PosScale[000]
                        */
                    }

                    else if (actualElement.Equals((byte)0xc) || actualElement.Equals((byte)0x2c) || actualElement.Equals((byte)0x4c))
                    {
                        /*
                        * EnvMap
                        * 1st par: Material Id.
                        * 2st par: Flag
                        */

                        actualSequenceBoneCommand.parameters = new List<Byte>();
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 1]);
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 2]);
                        sbcCounter += 3;
                        if (sbcCounter + 1 < (byte)sequenceBoneCommandList.Count)
                        {
                            var assToFix = assTableList[tabCounter - 1];
                            assToFix.pol = (byte)sequenceBoneCommandList[sbcCounter + 1];
                            assTableList[tabCounter - 1] = assToFix;
                            sbcCounter -= 1;
                        }
                    }
                    else if (actualElement.Equals((byte)0xd) || actualElement.Equals((byte)0x2d) || actualElement.Equals((byte)0x4d))
                    {
                        /*
                        * Projection Map
                        * 1st par: Material Id.
                        * 2st par: Flag
                        */
                        actualSequenceBoneCommand.parameters = new List<Byte>();
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 1]);
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 2]);
                        sbcCounter += 2;
                    }

                    else if (actualElement.Equals((byte)0x26))
                    {
                                                /*
                         * Node Description Command
                         * 1st par: Node Id
                         * 2nd par: Model Id
                         * 3st par: Dummy 0x0
                         * 4st par: Stack Id
                         */
                        actualSequenceBoneCommand.parameters = new List<Byte>();
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 1]);
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 2]);
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 3]);
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 4]);

                        actualNode = (byte)sequenceBoneCommandList[sbcCounter + 1];
                        sbcCounter += 4;
                    }
                    else if (actualElement.Equals((byte)0x27))
                    {

                        /*
                         * Node Description BB
                         * 1st par: Node Id
                         * 2st par: Stack Id.
                         */
                        actualSequenceBoneCommand.parameters = new List<Byte>();
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 1]);
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 2]);
                        sbcCounter += 2;
                    }
                    else if (actualElement.Equals((byte)0x28))
                    {
                        /*
                        * Node Description BBY
                        * 1st par: Node Id
                        * 2st par: Stack Id
                        */
                        actualSequenceBoneCommand.parameters = new List<Byte>();
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 1]);
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 2]);
                        sbcCounter += 2;
                    }
                    else if (actualElement.Equals((byte)0x2a))
                    {
                        /*
                        * PosScale[001]
                        */
                    }
                    else if (actualElement.Equals((byte)0x46))
                    {
                        /*
                        * Node Description Command
                        * 1st par: Node Id
                        * 2nd par: Model Id
                        * 3st par: Dummy 0x0
                        * 4st par: RestoreStack Id
                         */
                        actualSequenceBoneCommand.parameters = new List<Byte>();
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 1]);
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 2]);
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 3]);
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 4]);

                        actualNode = (byte)sequenceBoneCommandList[sbcCounter + 1];
                        sbcCounter += 4;
                    }
                    else if (actualElement.Equals((byte)0x47))
                    {

                        /*
                         * Node Description BB
                         * 1st par: Node Id
                         * 2st par: Restore Stack Id.
                         */
                        actualSequenceBoneCommand.parameters = new List<Byte>();
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 1]);
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 2]);

                        sbcCounter += 2;
                    }
                    else if (actualElement.Equals((byte)0x48))
                    {
                        /*
                        * Node Description BBY
                        * 1st par: Node Id
                        * 2st par: Restore Stack Id
                        */
                        actualSequenceBoneCommand.parameters = new List<Byte>();
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 1]);
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 2]);

                        sbcCounter += 2;
                    }

                    else if (actualElement.Equals((byte)0x66))
                    {
                        /*
                        * Node Description Command
                        * 1st par: Node Id
                        * 2nd par: Model Id
                        * 3st par: Dummy 0x0
                        * 4st par: RestoreStack Id
                        * 5st par: StoreStack Id
                         */
                        actualSequenceBoneCommand.parameters = new List<Byte>();
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 1]);
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 2]);
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 3]);
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 4]);
                        actualSequenceBoneCommand.parameters.Add((Byte)sequenceBoneCommandList[sbcCounter + 5]);

                        actualNode = (byte)sequenceBoneCommandList[sbcCounter + 1];
                        sbcCounter += 5;
                    }
                    sbc.Add(actualSequenceBoneCommand);
                }
            }
        }

        private static void makeMatTex(List<NsbmdModel> modelsList, List<string> nameTextureList, List<string> namePaletteList)
        {
            modelsList[0].matTexPalList = new List<NsbmdModel.MatTexPalStruct>();
            for (int matCounter = 0; matCounter < modelsList[0].modelInfo.matNum; matCounter++)
            {
                NsbmdModel.MatTexPalStruct actualMatTex = new NsbmdModel.MatTexPalStruct
                {
                    repeat = (byte)(modelsList[0].material.matInfoList[matCounter].texImageParam / 0x10000),
                    matName = modelsList[0].material.dictMat.matNameList[matCounter].name
                };
                int texIndexCounter = 0;
                while (texIndexCounter < modelsList[0].material.texIndexDataList.Count)
                {
                    if (matCounter == ((byte)modelsList[0].material.texIndexDataList[texIndexCounter]))
                    {
                        actualMatTex.texName = nameTextureList[texIndexCounter];
                        actualMatTex.textMatId = (uint)texIndexCounter;
                        break;
                    }
                    texIndexCounter++;
                }
                for (texIndexCounter = 0; texIndexCounter < modelsList[0].material.palIndexDataList.Count; texIndexCounter++)
                {
                    if ((texIndexCounter < namePaletteList.Count) && (matCounter == ((byte)modelsList[0].material.palIndexDataList[texIndexCounter])))
                    {
                        actualMatTex.palName = namePaletteList[texIndexCounter];
                        actualMatTex.palMatId = (uint)texIndexCounter;
                        break;
                    }
                }
                int texImageParam = modelsList[0].material.matInfoList[matCounter].texImageParam;
                actualMatTex.format = (texImageParam << 0x1a) >> 0x1d;
                modelsList[0].matTexPalList.Add(actualMatTex);
            }
            for (int matTexCounter = 0; matTexCounter < modelsList[0].matTexPalList.Count; matTexCounter++)
            {
                if (matTexCounter < modelsList[0].matTexPalList.Count)
                {
                    NsbmdModel.MatTexPalStruct actualMatTex = modelsList[0].matTexPalList[matTexCounter];
                    if (actualMatTex.texName == null)
                        actualMatTex.texName = "";
                    if (actualMatTex.palName == null)
                        actualMatTex.palName = "";
                    modelsList[0].matTexPalList[matTexCounter] = actualMatTex;
                }
            }
        }

        private static List<string> makeNamePaletteList(List<NsbmdModel> modelsList)
        {
            List<string> namePaletteList = new List<string>();
            for (int palCounter = 0; palCounter < modelsList[0].material.dictPal.palNum; palCounter++)
            {
                var actualPalName = modelsList[0].material.dictPal.palNameList[palCounter].name;
                int actualMatId = (int)modelsList[0].material.dictPal.palMaterialIdList[palCounter];
                int palMatCounter = 0;
                while (palMatCounter < (actualMatId - 1))
                {
                    namePaletteList.Add(actualPalName);
                    palMatCounter++;
                }
                namePaletteList.Add(actualPalName);
            }
            return namePaletteList;
        }

        private static List<string> makeNameTextureList(List<NsbmdModel> modelsList)
        {
            List<string> nameTextureList = new List<string>();
            for (int texCounter = 0; texCounter < modelsList[0].material.dictTex.texNum; texCounter++)
            {
                String actualTexName = modelsList[0].material.dictTex.texNameList[texCounter].name;
                int actualMatId = (int)modelsList[0].material.dictTex.texMaterialIdList[texCounter];
                int matIdCounter = 0;
                while (matIdCounter < (actualMatId - 1))
                {
                    nameTextureList.Add(actualTexName);
                    matIdCounter++;
                }
                nameTextureList.Add(actualTexName);
            }
            return nameTextureList;
        }

        private int readShape(BinaryReader reader, List<NsbmdModel> modelsList)
        {
            modelsList[0].shapeInfo.shapeList = new List<NsbmdModel.ShapeInfoStruct>();
            modelsList[0].dlSize = 0;
            modelsList[0].shapeInfo.dictShp.revisionId = reader.ReadByte();
            modelsList[0].shapeInfo.dictShp.shapeNum = reader.ReadByte();
            modelsList[0].shapeInfo.dictShp.size = reader.ReadUInt16();
            modelsList[0].shapeInfo.dictShp.pad = reader.ReadUInt16();
            modelsList[0].shapeInfo.dictShp.shapeDataSize = reader.ReadUInt16();
            modelsList[0].shapeInfo.dictShp.shapeDataList = new ArrayList();
            modelsList[0].shapeInfo.dictShp.magic = reader.ReadInt32();
            int shpCounter = 0;
            while (shpCounter < modelsList[0].shapeInfo.dictShp.shapeNum)
            {
                modelsList[0].shapeInfo.dictShp.shapeDataList.Add(reader.ReadUInt32());
                shpCounter++;
            }
            modelsList[0].shapeInfo.dictShp.unitSize = reader.ReadInt16();
            modelsList[0].shapeInfo.dictShp.shapeDefinitionOffsetSize = reader.ReadInt16();
            modelsList[0].shapeInfo.dictShp.shapeDefinitionOffsetList = new ArrayList();
            modelsList[0].shapeInfo.dictShp.shapeMaterialIdList = new ArrayList();
            for (shpCounter = 0; shpCounter < modelsList[0].shapeInfo.dictShp.shapeNum; shpCounter++)
            {
                int actualShpOff = reader.ReadUInt16();
                int actualShpMatId = reader.ReadByte();
                reader.ReadByte();
                modelsList[0].shapeInfo.dictShp.shapeDefinitionOffsetList.Add(actualShpOff);
                modelsList[0].shapeInfo.dictShp.shapeMaterialIdList.Add(actualShpMatId);
            }
            modelsList[0].shapeInfo.dictShp.shapeNameList = new List<NsbmdModel.DictNameStruct>();
            for (shpCounter = 0; shpCounter < modelsList[0].shapeInfo.dictShp.shapeNum; shpCounter++)
                modelsList[0].shapeInfo.dictShp.shapeNameList.Add(new NsbmdModel.DictNameStruct()
                {
                    name = Encoding.UTF8.GetString(reader.ReadBytes(0x10))
                });
            long num19 = reader.BaseStream.Position;
            for (shpCounter = 0; shpCounter < modelsList[0].modelInfo.shapeNum; shpCounter++)
            {
                NsbmdModel.ShapeInfoStruct info = new NsbmdModel.ShapeInfoStruct
                {
                    itemTag = reader.ReadUInt16(),
                    size = reader.ReadUInt16(),
                    flag = (int)reader.ReadUInt32(),
                    offsetDL = reader.ReadUInt32(),
                    sizeDL = (int)reader.ReadUInt32(),
                };
                modelsList[0].shapeInfo.shapeList.Add(info);
            }
            
            NsbmdModel.ShapeInfoStruct actualShapeInfo = modelsList[0].shapeInfo.shapeList[0];
            actualShapeInfo.startShapeInfo = reader.BaseStream.Position ;
            modelsList[0].shapeInfo.shapeList[0] = actualShapeInfo;


            for (shpCounter = 0; shpCounter < modelsList[0].modelInfo.shapeNum; shpCounter++)
            {
                actualShapeInfo = modelsList[0].shapeInfo.shapeList[shpCounter];

                position = (int)reader.BaseStream.Position;
                //MessageBox.Show("Reading polygon: " + shpCounter +  " with length " + actualShapeInfo.sizeDL.ToString("X") + " at offset " + position.ToString("X"));
  
                Int32 extension= modelsList[0].shapeInfo.shapeList[shpCounter].sizeDL;
                //reader.BaseStream.Seek(actualShapeInfo.offsetDL - (16*modelsList[0].modelInfo.shapeNum), SeekOrigin.Current);
                Int32 module = extension + position;
                if (module> reader.BaseStream.Length)
                {
                    MessageBox.Show("Errore nella lettura del poligono" + shpCounter);
                    goto Lab1;
                }
                actualShapeInfo.polygonData = reader.ReadBytes(extension);
                actualShapeInfo = readCommands(actualShapeInfo.polygonData, actualShapeInfo);
                actualShapeInfo.endShapeInfo = reader.BaseStream.Position;
            Lab1:

                if (shpCounter < modelsList[0].modelInfo.shapeNum - 1)
                {
                    var nextShapeInfo = modelsList[0].shapeInfo.shapeList[shpCounter + 1];
                    nextShapeInfo.startShapeInfo = reader.BaseStream.Position;
                    modelsList[0].shapeInfo.shapeList[shpCounter + 1] = nextShapeInfo;
                }

                modelsList[0].shapeInfo.shapeList[shpCounter] = actualShapeInfo;

            }
            return shpCounter;
        }


        public NsbmdModel.ShapeInfoStruct readCommands(byte[] polydata, NsbmdModel.ShapeInfoStruct poly)
        {
            if (polydata == null)
                return poly;
            else
            {
                MemoryStream polyStream = new MemoryStream();
                polyStream.Write(polydata, 0, polydata.Length);
                var reader = new BinaryReader(polyStream);
                reader.BaseStream.Position = 0;
                var actualCommand = new NsbmdModel.CommandStruct();
                poly.commandList = new List<NsbmdModel.CommandStruct>();
                int cur_vertex, idCounter;
                cur_vertex = gCurrentVertex;
                int blockCounter = 0;
                while (reader.BaseStream.Position < polyStream.Length)
                {
                    idCounter = initId(ref poly, polyStream, reader);


                    for (idCounter = 0; idCounter < 4 && reader.BaseStream.Position < polyStream.Length - 2; idCounter++)
                    {
                        actualCommand = poly.commandList[blockCounter + idCounter];
                        actualCommand.startParameterOffset = reader.BaseStream.Position;
                        
                        try
                        {
                            if (actualCommand.id != 0x41 || actualCommand.id != 0x0)
                                actualCommand.par = reader.ReadInt32();
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Error! Position: " + position.ToString("X") + " Id:" + actualCommand.id);
                        }

                        switch (poly.commandList[blockCounter + idCounter].id)
                        {
                            case 0: // No Operation (for padding packed GXFIFO commands)
                                break;
                            case 0x14:
                                {
                                    actualCommand.x = actualCommand.par;
                                    break;
                                }
                            case 0x1B:
                                {
                                    actualCommand.x = actualCommand.par;
                                    actualCommand.y = reader.ReadInt32();
                                    actualCommand.z = reader.ReadInt32();
                                    break;
                                }
                            case 0x20:
                                {
                                    actualCommand.x = (actualCommand.par >> 0) & 0x1F;
                                    actualCommand.y = (actualCommand.par >> 5) & 0x1F;
                                    actualCommand.z = (actualCommand.par >> 10) & 0x1F;
                                }
                                break;

                            case 0x21:
                                {
                                    actualCommand.x = (actualCommand.par >> 0) & 0x3FF;
                                    if ((actualCommand.x & 0x200) != 0)
                                        actualCommand.x |= -1024;
                                    actualCommand.y = (actualCommand.par >> 10) & 0x3FF;
                                    if ((actualCommand.y & 0x200) != 0) actualCommand.y |= -1024;
                                    actualCommand.z = (actualCommand.par >> 20) & 0x3FF;
                                    if ((actualCommand.z & 0x200) != 0) actualCommand.z |= -1024;
                                    break;
                                }
                            case 0x22:
                                {
                                    actualCommand.x = (actualCommand.par >> 0) & 0xffff;
                                    if ((actualCommand.x & 0x8000) != 0) actualCommand.x |= -65536;
                                    actualCommand.y = (actualCommand.par >> 16) & 0xffff;
                                    if ((actualCommand.y & 0x8000) != 0) actualCommand.y |= -65536;
                                    break;
                                }
                            case 0x23:
                                {
                                    actualCommand.x = (actualCommand.par >> 0) & 0xFFFF;
                                    if ((actualCommand.x & 0x8000) != 0) actualCommand.x |= -65536;
                                    actualCommand.y = (actualCommand.par >> 16) & 0xFFFF;
                                    if ((actualCommand.y & 0x8000) != 0) actualCommand.y |= -65536;
                                    actualCommand.par2 = reader.ReadInt32();
                                    actualCommand.z = actualCommand.par2 & 0xFFFF;
                                    if ((actualCommand.z & 0x8000) != 0) actualCommand.z |= -65536;
                                    xBuffer = actualCommand.x;
                                    yBuffer = actualCommand.y;
                                    zBuffer = actualCommand.z;
                                    break;
                                }
                            case 0x24:
                                /*
                                      VTX_10 - Set Vertex XYZ Coordinates (W)
                                      Parameter 1, Bit 0-9    X-Coordinate (signed, with 6bit fractional part)
                                      Parameter 1, Bit 10-19  Y-Coordinate (signed, with 6bit fractional part)
                                      Parameter 1, Bit 20-29  Z-Coordinate (signed, with 6bit fractional part)
                                      Parameter 1, Bit 30-31  Not used
                                    */
                                {
                                    actualCommand.x = (actualCommand.par >> 0) & 0x3FF;
                                    if ((actualCommand.x & 0x200) != 0) actualCommand.x |= -1024;
                                    actualCommand.y = (actualCommand.par >> 10) & 0x3FF;
                                    if ((actualCommand.y & 0x200) != 0) actualCommand.y |= -1024;
                                    actualCommand.z = (actualCommand.par >> 20) & 0x3FF;
                                    if ((actualCommand.z & 0x200) != 0) actualCommand.z |= -1024;
                                    xBuffer = actualCommand.x;
                                    yBuffer = actualCommand.y;
                                    zBuffer = actualCommand.z;
                                    break;
                                }
                            case 0x25:
                                /*
                                      VTX_XY - Set Vertex XY Coordinates (W)
                                      Parameter 1, Bit 0-15   X-Coordinate (signed, with 12bit fractional part)
                                      Parameter 1, Bit 16-31  Y-Coordinate (signed, with 12bit fractional part)
                                    */
                                {
                                    actualCommand.x = (actualCommand.par >> 0) & 0xFFFF;
                                    if ((actualCommand.x & 0x8000) != 0) actualCommand.x |= -65536;
                                    actualCommand.y = (actualCommand.par >> 16) & 0xFFFF;
                                    if ((actualCommand.y & 0x8000) != 0) actualCommand.y |= -65536;
                                    actualCommand.z = zBuffer;
                                    yBuffer = actualCommand.y;
                                    xBuffer = actualCommand.x;
                                    break;
                                }
                            case 0x26:
                                /*
                                      VTX_XZ - Set Vertex XZ Coordinates (W)
                                      Parameter 1, Bit 0-15   X-Coordinate (signed, with 12bit fractional part)
                                      Parameter 1, Bit 16-31  Z-Coordinate (signed, with 12bit fractional part)
                                    */
                                {
                                    actualCommand.x = (actualCommand.par >> 0) & 0xFFFF;
                                    if ((actualCommand.x & 0x8000) != 0) actualCommand.x |= -65536;                               
                                    actualCommand.z = (actualCommand.par >> 16) & 0xFFFF;
                                    if ((actualCommand.z & 0x8000) != 0) actualCommand.z |= -65536;
                                    xBuffer = actualCommand.x;
                                    actualCommand.y  = yBuffer;
                                    zBuffer = actualCommand.z;
                                    break;
                                }
                            case 0x27:
                                /*
                                      VTX_YZ - Set Vertex YZ Coordinates (W)
                                      Parameter 1, Bit 0-15   Y-Coordinate (signed, with 12bit fractional part)
                                      Parameter 1, Bit 16-31  Z-Coordinate (signed, with 12bit fractional part)
                                    */
                                {
                                    actualCommand.y = (actualCommand.par >> 0) & 0xFFFF;
                                    if ((actualCommand.y & 0x8000) != 0) actualCommand.y |= -65536;
                                    actualCommand.z = (actualCommand.par >> 16) & 0xFFFF;
                                    if ((actualCommand.z & 0x8000) != 0) actualCommand.z |= -65536;
                                    actualCommand.x = xBuffer;
                                    yBuffer = actualCommand.y;
                                    zBuffer = actualCommand.z;
                                    break;
                                }
                            case 0x28:
                                /*
                                      VTX_DIFF - Set Relative Vertex Coordinates (W)
                                      Parameter 1, Bit 0-9    X-Difference (signed, with 9bit fractional part)
                                      Parameter 1, Bit 10-19  Y-Difference (signed, with 9bit fractional part)
                                      Parameter 1, Bit 20-29  Z-Difference (signed, with 9bit fractional part)
                                      Parameter 1, Bit 30-31  Not used
                                    */
                                {
                                    actualCommand.id = 0x28;
                                    actualCommand.x = (actualCommand.par >> 0) & 0x3FF;
                                    if ((actualCommand.x & 0x200) != 0) actualCommand.x |= -1024;
                                    actualCommand.y = (actualCommand.par >> 10) & 0x3FF;
                                    if ((actualCommand.y & 0x200) != 0) actualCommand.y |= -1024;
                                    actualCommand.z = (actualCommand.par >> 20) & 0x3FF;
                                    if ((actualCommand.z & 0x200) != 0) actualCommand.z |= -1024;
                                    xBuffer = actualCommand.x;
                                    yBuffer = actualCommand.y;
                                    zBuffer = actualCommand.z;
                                    break;
                                }
                            case 0x40: // Start of Vertex List (W)
                                {
                                    actualCommand.id = 0x40;
                                    actualCommand.x = 0;
                                    break;
                                }
                            case 0x41: // End of Vertex List (W)
                                break;
                            default:
                                break;
                        }
                        poly.commandList[blockCounter + idCounter] = actualCommand;

                        if (idCounter == 3)
                            blockCounter += 4;
                    }
                }
                return poly;
            }
        }

        private static int initId(ref NsbmdModel.ShapeInfoStruct poly, MemoryStream polyStream, BinaryReader reader)
        {
            int idCounter;
            idCounter = 0;
            while (idCounter < 4)
            {
                if (reader.BaseStream.Position < polyStream.Length)
                    poly.commandList.Add(new NsbmdModel.CommandStruct()
                    {
                        id = reader.ReadByte(),
                        startIdOffset = reader.BaseStream.Position
                    });
                idCounter++;
            }
            return idCounter;
        }



        private void readMatInfo(BinaryReader reader, List<NsbmdModel> modelsList)
        {
            modelsList[0].material.matInfoList = new List<NsbmdModel.MatInfoStruct>();
            for (int matCounter = 0; matCounter < modelsList[0].modelInfo.matNum; matCounter++)
            {
                reader.BaseStream.Position = ((uint)modelsList[0].material.dictMat.matDefinitionOffsetList[matCounter]) + startMaterials;
                NsbmdModel.MatInfoStruct actualMatInfo = new NsbmdModel.MatInfoStruct
                {
                    itemTag = reader.ReadUInt16(),
                    size = reader.ReadUInt16(),
                    diffAmb = reader.ReadInt32(),
                    specEmi = reader.ReadInt32(),
                    polyAttr = reader.ReadInt32(),
                    polyAttrMask = reader.ReadInt32(),
                    texImageParam = reader.ReadInt32(),
                    texImageParamMask = reader.ReadInt32(),
                    texPlttBase = reader.ReadUInt16(),
                    flag = reader.ReadUInt16(),
                    origWidth = reader.ReadUInt16(),
                    origHeight = reader.ReadUInt16(),
                    magW = reader.ReadInt32(),
                    magH = reader.ReadInt32()
                };
                if ((actualMatInfo.flag + 2) == 0)
                {
                    actualMatInfo.scaleS = reader.ReadInt32();
                    actualMatInfo.scaleT = reader.ReadInt32();
                }
                else if ((actualMatInfo.flag + 4) == 0)
                {
                    actualMatInfo.rotSin = reader.ReadInt32();
                    actualMatInfo.rotCos = reader.ReadInt32();
                }
                else if ((actualMatInfo.flag + 8) == 0)
                {
                    actualMatInfo.transS = reader.ReadInt32();
                    actualMatInfo.transT = reader.ReadInt32();
                }
                else if (((actualMatInfo.flag & 2) == 0) && ((actualMatInfo.flag & 1) == 1))
                {
                    actualMatInfo.rotSin = reader.ReadInt32();
                    actualMatInfo.rotCos = reader.ReadInt32();
                }
                else if ((actualMatInfo.flag + 0x2000) == 0x7d0)
                {
                    Array.Resize<double>(ref actualMatInfo.effectMtx, 0x11);
                    for (int i = 0; i <= 15; i++)
                        actualMatInfo.effectMtx[i] = reader.ReadInt32();
                }
                else if (actualMatInfo.flag == 0x3fcf)
                    reader.BaseStream.Seek(16 * 4, SeekOrigin.Current);
                if (actualMatInfo.flag == 0x1fc7)
                    actualMatInfo.unkBuffer = (int)reader.ReadUInt64();

                modelsList[0].material.matInfoList.Add(actualMatInfo);
            }
        }

        private static void readAssMatrix(BinaryReader reader, List<NsbmdModel> modelsList, int startMaterials)
        {
            modelsList[0].material.matLength = modelsList[0].material.dictTex.texNum + modelsList[0].material.dictPal.palNum;
            modelsList[0].material.texIndexDataList = new ArrayList();
            modelsList[0].material.palIndexDataList = new ArrayList();
            int texDataNum = 0;
            int palDataNum = 0;
            for (int texmatCounter = 0; texmatCounter < modelsList[0].material.dictTex.texMaterialIdList.Count; texmatCounter++)
                texDataNum += (int)modelsList[0].material.dictTex.texMaterialIdList[texmatCounter];
            for (int palmatCounter = 0; palmatCounter < modelsList[0].material.dictPal.palMaterialIdList.Count; palmatCounter++)
                palDataNum += (int)modelsList[0].material.dictPal.palMaterialIdList[palmatCounter];
            for (int tesDataCounter = 0; tesDataCounter < texDataNum; tesDataCounter++)
                modelsList[0].material.texIndexDataList.Add(reader.ReadByte());
            for (int palDataCounter = 0; palDataCounter < palDataNum; palDataCounter++)
                modelsList[0].material.palIndexDataList.Add(reader.ReadByte());
            long realPos = ((uint)modelsList[0].material.dictMat.matDefinitionOffsetList[0]) + ((uint)startMaterials);
            long skipLength =  realPos- (uint)reader.BaseStream.Position;
            for (int skipCounter = 0; skipCounter < skipLength; skipCounter++)
                modelsList[0].material.palIndexDataList.Add((Byte) 0);
        }

        private static void readPalette(BinaryReader reader, List<NsbmdModel> modelsList)
        {
            modelsList[0].material.dictPal.revisionId = reader.ReadByte();
            modelsList[0].material.dictPal.palNum = reader.ReadByte();
            modelsList[0].material.dictPal.size = reader.ReadUInt16();
            modelsList[0].material.dictPal.pad = reader.ReadUInt16();
            modelsList[0].material.dictPal.palDataSize = reader.ReadUInt16();
            modelsList[0].material.dictPal.palDataList = new ArrayList();
            modelsList[0].material.dictPal.magic = reader.ReadInt32();
            for (int palCounter = 0; palCounter < modelsList[0].material.dictPal.palNum; palCounter++)
                modelsList[0].material.dictPal.palDataList.Add(reader.ReadUInt32());
            modelsList[0].material.dictPal.unitSize = reader.ReadInt16();
            modelsList[0].material.dictPal.palDefinitionOffsetSize = reader.ReadInt16();
            modelsList[0].material.dictPal.palDefinitionOffsetList = new ArrayList();
            modelsList[0].material.dictPal.palMaterialIdList = new ArrayList();
            for (int palCounter = 0; palCounter < modelsList[0].material.dictPal.palNum; palCounter++)
            {
                int actualPalDef = reader.ReadUInt16();
                int actualPalMat = reader.ReadByte();
                reader.ReadByte();
                modelsList[0].material.dictPal.palDefinitionOffsetList.Add(actualPalDef);
                modelsList[0].material.dictPal.palMaterialIdList.Add(actualPalMat);
            }
            modelsList[0].material.dictPal.palNameList = new List<NsbmdModel.DictNameStruct>();
            for (int palCounter = 0; palCounter < modelsList[0].material.dictPal.palNum; palCounter++)
                modelsList[0].material.dictPal.palNameList.Add(new NsbmdModel.DictNameStruct()
                {
                    name = Encoding.UTF8.GetString(reader.ReadBytes(0x10))
                });
        }

        private static void readTextures(BinaryReader reader, List<NsbmdModel> modelsList)
        {
            modelsList[0].material.dictTex.revisionId = reader.ReadByte();
            modelsList[0].material.dictTex.texNum = reader.ReadByte();
            modelsList[0].material.dictTex.size = reader.ReadUInt16();
            modelsList[0].material.dictTex.pad = reader.ReadUInt16();
            modelsList[0].material.dictTex.texDataSize = reader.ReadUInt16();
            modelsList[0].material.dictTex.magic = reader.ReadInt32();
            modelsList[0].material.dictTex.texDataList = new List<NsbmdModel.texDataStruct>();
            for (int texCounter = 0; texCounter < modelsList[0].material.dictTex.texNum; texCounter++)
            {
                NsbmdModel.texDataStruct data;
                int par = reader.ReadInt16();
                data = new NsbmdModel.texDataStruct
                {
                    par = par,
                    format = (par >> 10) & 7,
                    par2 = reader.ReadByte(),
                    par3 = reader.ReadByte()
                };
                modelsList[0].material.dictTex.texDataList.Add(data);
            }
            modelsList[0].material.dictTex.unitSize = reader.ReadInt16();
            modelsList[0].material.dictTex.texDefinitionOffsetSize = reader.ReadInt16();
            modelsList[0].material.dictTex.texDefinitionOffsetList = new ArrayList();
            modelsList[0].material.dictTex.texMaterialIdList = new ArrayList();
            for (int texCounter = 0; texCounter < modelsList[0].material.dictTex.texNum; texCounter++)
            {
                int actualTexDef = reader.ReadUInt16();
                int actualTexMat = reader.ReadByte();
                reader.ReadByte();
                modelsList[0].material.dictTex.texDefinitionOffsetList.Add(actualTexDef);
                modelsList[0].material.dictTex.texMaterialIdList.Add(actualTexMat);
            }
            modelsList[0].material.dictTex.texNameList = new List<NsbmdModel.DictNameStruct>();
            for (int texCounter = 0; texCounter < modelsList[0].material.dictTex.texNum; texCounter++)
                modelsList[0].material.dictTex.texNameList.Add(new NsbmdModel.DictNameStruct()
                {
                    name = Encoding.UTF8.GetString(reader.ReadBytes(0x10))
                });
        }

        private static void readMaterials(BinaryReader reader, List<NsbmdModel> modelsList)
        {
            modelsList[0].material.dictMat.revisionId = reader.ReadByte();
            modelsList[0].material.dictMat.matNum = reader.ReadByte();
            modelsList[0].material.dictMat.size = reader.ReadUInt16();
            modelsList[0].material.dictMat.pad = reader.ReadUInt16();
            modelsList[0].material.dictMat.matDataSize = reader.ReadUInt16();
            modelsList[0].material.dictMat.magic = reader.ReadInt32();
            modelsList[0].material.dictMat.matDataList = new ArrayList();
            for (int matCounter = 0; matCounter < modelsList[0].material.dictMat.matNum; matCounter++)
                modelsList[0].material.dictMat.matDataList.Add(reader.ReadUInt32());
            modelsList[0].material.dictMat.unitSize = reader.ReadInt16();
            modelsList[0].material.dictMat.matDefinitionOffsetSize = reader.ReadInt16();
            modelsList[0].material.dictMat.matDefinitionOffsetList = new ArrayList();
            for (int matCounter = 0; matCounter < modelsList[0].material.dictMat.matNum; matCounter++)
                modelsList[0].material.dictMat.matDefinitionOffsetList.Add(reader.ReadUInt32());
            modelsList[0].material.dictMat.matNameList = new List<NsbmdModel.DictNameStruct>();
            for (int matCounter = 0; matCounter < modelsList[0].material.dictMat.matNum; matCounter++)
                modelsList[0].material.dictMat.matNameList.Add(new NsbmdModel.DictNameStruct()
                {
                    name = Encoding.UTF8.GetString(reader.ReadBytes(0x10))
                });
        }

        private static long skip(BinaryReader reader, List<NsbmdModel> modelsList, long position)
        {
            long skipLength = modelsList[0].modelInfo.matOffset + position - reader.BaseStream.Position;
            for (int skipCounter = 0; skipCounter < skipLength; skipCounter++)
                reader.BaseStream.Seek(1L, SeekOrigin.Current);
            return skipLength;
        }

        private static bool readSbc(BinaryReader reader, uint startSbcOffset, uint sizeSbc, NsbmdModel model, int number)
        {
            Boolean flag = false;
            int stackId = 0;
            int sbcCounter = 0;
            int restoreId = 0;
            reader.BaseStream.Seek((long)startSbcOffset, SeekOrigin.Begin);
            while (sbcCounter < sizeSbc)
            {
                model.sequenceBoneCommandList.Add(reader.ReadByte());
                sbcCounter++;
            }

            reader.BaseStream.Seek((long)startSbcOffset, SeekOrigin.Begin);
            sbcCounter = 0;

            while (sbcCounter < sizeSbc)
            {
                int par, par2, par3, par4, par5, par6, par7, par8;
                NsbmdModel.NodeInfoStruct actualNodeInfo;
                switch (reader.ReadByte())
                {
                    case 0x44:
                    case 0x4:
                    case 0x24:
                        {
                            par = reader.ReadByte();
                            par2 = reader.ReadByte();
                            par3 = reader.ReadByte();
                            sbcCounter += 4;
                            stackId++;
                            continue;
                        }
                    case 0x46:
                        {
                            par = reader.ReadByte();
                            par2 = reader.ReadByte();
                            par3 = reader.ReadByte();
                            par4 = reader.ReadByte();
                            sbcCounter += 5;
                            actualNodeInfo = model.nodeInfoList[number];
                            actualNodeInfo.parentId = par2;
                            actualNodeInfo.stackId = -1;
                            actualNodeInfo.restoreId = restoreId = par4;
                            model.nodeInfoList[number] = actualNodeInfo;
                            continue;
                        }
                    case 0x66:
                        {
                            par = reader.ReadByte();
                            par2 = reader.ReadByte();
                            par3 = reader.ReadByte();
                            par4 = reader.ReadByte();
                            par5 = reader.ReadByte();
                            sbcCounter += 6;
                            actualNodeInfo = model.nodeInfoList[number];
                            actualNodeInfo.parentId = par2;
                            actualNodeInfo.stackId = par4;
                            actualNodeInfo.restoreId = par5;
                            model.nodeInfoList[number] = actualNodeInfo;
                            continue;
                        }
                    case 0x2b:
                        {
                            flag = true;
                            sbcCounter++;
                            number++;
                            continue;
                        }
                    case 0x1:
                        sbcCounter++;
                        return true;

                    case 0x2:
                        {
                            par = reader.ReadByte();
                            par2 = reader.ReadByte();
                            sbcCounter += 3;
                            continue;
                        }
                    case 0x3:
                        {
                            restoreId = reader.ReadByte();
                            sbcCounter += 2;
                            continue;
                        }
                    case 0x6:
                        {
                            par = reader.ReadByte();
                            par2 = reader.ReadByte();
                            par3 = reader.ReadByte();
                            sbcCounter += 4;
                            actualNodeInfo = model.nodeInfoList[number];
                            actualNodeInfo.parentId = par2;
                            actualNodeInfo.stackId = -1;
                            actualNodeInfo.restoreId = -1;
                            model.nodeInfoList[number] = actualNodeInfo;
                            continue;
                        }
                    case 0x7:
                    case 0x8:
                        {
                            par = reader.ReadByte();
                            sbcCounter += 2;
                            continue;
                        }
                    case 0x9:
                        {
                            par = reader.ReadByte();
                            par2 = reader.ReadByte();
                            par3 = reader.ReadByte();
                            par4 = reader.ReadByte();
                            par5 = reader.ReadByte();
                            par6 = reader.ReadByte();
                            par7 = reader.ReadByte();
                            par8 = reader.ReadByte();
                            sbcCounter += 9;
                            continue;
                        }
                    case 0xb:
                        {
                            flag = true;
                            sbcCounter++;
                            continue;
                        }
                    case 0x26:
                        {
                            par = reader.ReadByte();
                            par2 = reader.ReadByte();
                            par3 = reader.ReadByte();
                            par4 = reader.ReadByte();
                            sbcCounter += 5;
                            actualNodeInfo = model.nodeInfoList[number];
                            actualNodeInfo.parentId = par2;
                            actualNodeInfo.stackId = par4;
                            actualNodeInfo.restoreId = -1;
                            model.nodeInfoList[number] = actualNodeInfo;
                            continue;
                        }
                }
                return false;
            }
            return false;
        }

        private static void readNodes(BinaryReader reader, List<NsbmdModel> modelsList)
        {
            modelsList[0].node.revision = reader.ReadByte();
            modelsList[0].node.nodeNum = reader.ReadByte();
            modelsList[0].node.size = reader.ReadUInt16();
            modelsList[0].node.pad = reader.ReadUInt16();
            modelsList[0].node.nodeDataSize = reader.ReadUInt16();
            modelsList[0].node.magic = reader.ReadInt32();
            modelsList[0].node.nodeDataList = new ArrayList();
            for (int objCounter = 0; objCounter < modelsList[0].node.nodeNum; objCounter++)
                modelsList[0].node.nodeDataList.Add(reader.ReadUInt32());
            modelsList[0].node.unitSize = reader.ReadInt16();
            modelsList[0].node.nodeDefinitionOffsetSize = reader.ReadInt16();
            modelsList[0].node.nodeDefinitionOffsetList = new ArrayList();
            for (int objCounter = 0; objCounter < modelsList[0].node.nodeNum; objCounter++)
                modelsList[0].node.nodeDefinitionOffsetList.Add(reader.ReadUInt32());
            modelsList[0].node.nodeNameList = new List<NsbmdModel.DictNameStruct>();
            for (int objCounter = 0; objCounter < modelsList[0].node.nodeNum; objCounter++)
                modelsList[0].node.nodeNameList.Add(new NsbmdModel.DictNameStruct()
                {
                    name = Encoding.UTF8.GetString(reader.ReadBytes(0x10))
                });
            modelsList[0].nodeInfoList = new List<NsbmdModel.NodeInfoStruct>();
            for (int objCounter = 0; objCounter < modelsList[0].node.nodeNum; objCounter++)
                modelsList[0].nodeInfoList.Add(readNodeInfo(reader));
        }

        private static void readModelInfo(BinaryReader reader, List<NsbmdModel> modelsList)
        {
            modelsList[0].modelInfo.size = reader.ReadUInt32();
            modelsList[0].modelInfo.sbcOffset = reader.ReadUInt32();
            modelsList[0].modelInfo.matOffset = reader.ReadUInt32();
            modelsList[0].modelInfo.shapeOffset = reader.ReadUInt32();
            modelsList[0].modelInfo.bdhcOffset = reader.ReadUInt32();
            modelsList[0].modelInfo.sbcType = reader.ReadByte();
            modelsList[0].modelInfo.scalingRule = reader.ReadByte();
            modelsList[0].modelInfo.textMatrixMode = reader.ReadByte();
            modelsList[0].modelInfo.nodeNum = reader.ReadByte();
            modelsList[0].modelInfo.matNum = reader.ReadByte();
            modelsList[0].modelInfo.shapeNum = reader.ReadByte();
            modelsList[0].modelInfo.matUns = reader.ReadByte();
            modelsList[0].modelInfo.pad = reader.ReadByte();
            modelsList[0].modelInfo.scalePosition = reader.ReadInt32();
            modelsList[0].modelInfo.scalePositionInverse = reader.ReadInt32();
            modelsList[0].modelInfo.vertexNum = reader.ReadUInt16();
            modelsList[0].modelInfo.polygonNum = reader.ReadUInt16();
            modelsList[0].modelInfo.triangleNum = reader.ReadUInt16();
            modelsList[0].modelInfo.quadsNum = reader.ReadUInt16();
            modelsList[0].modelInfo.boundingBoxX = Convert.ToDouble((double)(((double)reader.ReadInt16()) / 4096.0));
            modelsList[0].modelInfo.boundingBoxY = Convert.ToDouble((double)(((double)reader.ReadInt16()) / 4096.0));
            modelsList[0].modelInfo.boundingBoxZ = Convert.ToDouble((double)(((double)reader.ReadInt16()) / 4096.0));
            modelsList[0].modelInfo.boundingBoxWidth = Convert.ToDouble((double)(((double)reader.ReadInt16()) / 4096.0));
            modelsList[0].modelInfo.boundingBoxHeigth = Convert.ToDouble((double)(((double)reader.ReadInt16()) / 4096.0));
            modelsList[0].modelInfo.boundingBoxDepth = Convert.ToDouble((double)(((double)reader.ReadInt16()) / 4096.0));
            modelsList[0].modelInfo.boundingBoxScalePosition = reader.ReadInt32();
            modelsList[0].modelInfo.boundingBoxScalePositionInverse = reader.ReadInt32();
        }

        private static void readModel(BinaryReader reader, List<NsbmdModel> modelsList, int sizeMDL0, int modelNumber)
        {
            modelsList[0].model.revisionId = 0;
            modelsList[0].model.modelNum = modelNumber;
            modelsList[0].blockSize = sizeMDL0;
            modelsList[0].model.size = reader.ReadUInt16();
            modelsList[0].model.pad = reader.ReadUInt16();
            modelsList[0].model.modelDataSize = reader.ReadUInt16();
            modelsList[0].model.modelDataList = new ArrayList();
            modelsList[0].model.magic = reader.ReadInt32();
            for (int modelCounter = 0; modelCounter < modelsList[0].model.modelNum; modelCounter++)
                modelsList[0].model.modelDataList.Add(reader.ReadInt32());
            modelsList[0].model.unitSize = reader.ReadUInt16();
            modelsList[0].model.modelDefinitionOffsetSize = reader.ReadUInt16();
            modelsList[0].model.modelDefinitionOffsetList = new ArrayList();
            for (int modelCounter = 0; modelCounter < modelsList[0].model.modelNum; modelCounter++)
                modelsList[0].model.modelDefinitionOffsetList.Add(reader.ReadInt32());
            modelsList[0].model.modelNameList = new List<NsbmdModel.DictNameStruct>();
            for (int modelCounter = 0; modelCounter < modelsList[0].model.modelNum; modelCounter++)
                modelsList[0].model.modelNameList.Add(new NsbmdModel.DictNameStruct()
                {
                    name = Encoding.UTF8.GetString(reader.ReadBytes(0x10))
                });
        }

        #endregion

        #region Saving Part

        public void SaveBMD0(BinaryWriter writer)
        {
            writer.Write((Int32)type);
            writer.Write((Int32)versionBMD0);
            writer.Write((Int32)sizeBMD0);
            writer.Write((Int16)0x10);
            writer.Write((Int16)sectionNum);

            for (int sectionCounter = 0; sectionCounter < sectionNum; sectionCounter++)
                writer.Write((Int32)sectionData[sectionCounter]);
            for (int sectionCounter = 0; sectionCounter < sectionNum; sectionCounter++)
            {
                while (writer.BaseStream.Position != sectionData[sectionCounter] && Maps.type == 3)
                    writer.Write((Byte)0);
                writer.Write((Int32)id[sectionCounter]);
                switch (id[sectionCounter])
               { 
                    case MDL0:
                        SaveMDL0(writer);
                        break;
                    case TEX0:
                        actualTex.SaveTex0(writer);
                        break;
                }
            }
        }

        private void SaveMDL0(BinaryWriter writer)
        {
            SaveModel(writer);
            SaveModelInfo(writer);
            SaveNodes(writer);
            
            foreach (Sbc_s sbcCommand in sbc)
            {
                writer.Write(sbcCommand.id);
                if (sbcCommand.parameters != null)
                {
                    foreach (Byte parameter in sbcCommand.parameters)
                        writer.Write(parameter);
                }
            }

            //for (int sbcCounter = 0; sbcCounter < models[0].sequenceBoneCommandList.Count; sbcCounter++)
            //    writer.Write((Byte)models[0].sequenceBoneCommandList[sbcCounter]);
            int actualPos = (Int32)writer.BaseStream.Position;
            while (actualPos < startMaterials + padOffset)
            {
                writer.Write((Byte)0);
                actualPos++;
            }
            writer.Write((UInt16)models[0].material.dictTexOffset);
            writer.Write((UInt16)models[0].material.dictPalOffset);
            SaveMaterials(writer);
            SaveTextures(writer);
            SavePalette(writer);
            SaveAssMatrix(writer);
            SaveMatInfo(writer);
            SaveShape(writer);

        }

        public void SaveModel(BinaryWriter writer)
        {
            writer.Write((Int32)models[0].blockSize);
            writer.Write((Byte)0);
            writer.Write((Byte)models[0].model.modelNum);
            writer.Write((UInt16)models[0].model.size);
            writer.Write((UInt16)models[0].model.pad);
            writer.Write((UInt16)models[0].model.modelDataSize);
            writer.Write((Int32)models[0].model.magic);
            for (int modelCounter = 0; modelCounter < models[0].model.modelNum; modelCounter++)
                writer.Write((Int32)models[0].model.modelDataList[modelCounter]);
            writer.Write((UInt16)models[0].model.unitSize);
            writer.Write((UInt16)models[0].model.modelDefinitionOffsetSize);
            for (int modelCounter = 0; modelCounter < models[0].model.modelNum; modelCounter++)
                writer.Write((Int32)models[0].model.modelDefinitionOffsetList[modelCounter]);
            for (int modelCounter = 0; modelCounter < models[0].model.modelNum; modelCounter++)
                foreach (char c in models[0].model.modelNameList[modelCounter].name)
                    writer.Write((Byte)c);
        }

        public void SaveModelInfo(BinaryWriter writer)
        {
            writer.Write((UInt32)models[0].modelInfo.size);
            writer.Write((UInt32)models[0].modelInfo.sbcOffset);
            writer.Write((UInt32)models[0].modelInfo.matOffset);
            writer.Write((UInt32)models[0].modelInfo.shapeOffset);
            writer.Write((UInt32)models[0].modelInfo.bdhcOffset);
            writer.Write((Byte)models[0].modelInfo.sbcType);
            writer.Write((Byte)models[0].modelInfo.scalingRule);
            writer.Write((Byte)models[0].modelInfo.textMatrixMode);
            writer.Write((Byte)models[0].modelInfo.nodeNum);
            writer.Write((Byte)models[0].modelInfo.matNum);
            writer.Write((Byte)models[0].modelInfo.shapeNum);
            writer.Write((Byte)models[0].modelInfo.matUns);
            writer.Write((Byte)models[0].modelInfo.pad);
            writer.Write((Int32)models[0].modelInfo.scalePosition);
            writer.Write((Int32)models[0].modelInfo.scalePositionInverse);
            writer.Write((UInt16)models[0].modelInfo.vertexNum);
            writer.Write((UInt16)models[0].modelInfo.polygonNum);
            writer.Write((UInt16)models[0].modelInfo.triangleNum);
            writer.Write((UInt16)models[0].modelInfo.quadsNum);
            writer.Write((Int16)(models[0].modelInfo.boundingBoxX * 4096.0));
            writer.Write((Int16)(models[0].modelInfo.boundingBoxY * 4096.0));
            writer.Write((Int16)(models[0].modelInfo.boundingBoxZ * 4096.0));
            writer.Write((Int16)(models[0].modelInfo.boundingBoxWidth * 4096.0));
            writer.Write((Int16)(models[0].modelInfo.boundingBoxHeigth * 4096.0));
            writer.Write((Int16)(models[0].modelInfo.boundingBoxDepth * 4096.0));
            writer.Write((Int32)models[0].modelInfo.boundingBoxScalePosition);
            writer.Write((Int32)models[0].modelInfo.boundingBoxScalePositionInverse);
        }

        public void SaveNodes(BinaryWriter writer)
        {
            writer.Write((Byte)models[0].node.revision);
            writer.Write((Byte)models[0].node.nodeNum);
            writer.Write((UInt16)models[0].node.size);
            writer.Write((UInt16)models[0].node.pad);
            writer.Write((UInt16)models[0].node.nodeDataSize);
            writer.Write((Int32)models[0].node.magic);
            for (int objCounter = 0; objCounter < models[0].node.nodeNum; objCounter++)
                writer.Write((UInt32)models[0].node.nodeDataList[objCounter]);
            writer.Write((Int16)models[0].node.unitSize);
            writer.Write((Int16)models[0].node.nodeDefinitionOffsetSize);
            for (int objCounter = 0; objCounter < models[0].node.nodeNum; objCounter++)
                writer.Write((UInt32)models[0].node.nodeDefinitionOffsetList[objCounter]);
            for (int objCounter = 0; objCounter < models[0].node.nodeNum; objCounter++)
                foreach (char c in models[0].node.nodeNameList[objCounter].name)
                    writer.Write((Byte)c);
            for (int objCounter = 0; objCounter < models[0].nodeInfoList.Count; objCounter++)
                SaveNodeInfo(models[0].nodeInfoList[objCounter], writer);
        }

        public void SaveMaterials(BinaryWriter writer)
        {
            writer.Write((Byte)models[0].material.dictMat.revisionId);
            writer.Write((Byte)models[0].material.dictMat.matNum);
            writer.Write((UInt16)models[0].material.dictMat.size);
            writer.Write((UInt16)models[0].material.dictMat.pad);
            writer.Write((UInt16)models[0].material.dictMat.matDataSize);
            writer.Write((Int32)models[0].material.dictMat.magic);
            for (int EntryCounter = 0; EntryCounter < models[0].material.dictMat.matNum; EntryCounter++)
                writer.Write((UInt32)models[0].material.dictMat.matDataList[EntryCounter]);
            writer.Write((Int16)models[0].material.dictMat.unitSize);
            writer.Write((Int16)models[0].material.dictMat.matDefinitionOffsetSize);
            for (int EntryCounter = 0; EntryCounter < models[0].material.dictMat.matNum; EntryCounter++)
                writer.Write((UInt32)models[0].material.dictMat.matDefinitionOffsetList[EntryCounter]);
            for (int EntryCounter = 0; EntryCounter < models[0].material.dictMat.matNum; EntryCounter++)
                foreach (char c in models[0].material.dictMat.matNameList[EntryCounter].name)
                    writer.Write((Byte)c);
        }

        public void SaveTextures(BinaryWriter writer)
        {
            writer.Write((Byte)models[0].material.dictTex.revisionId);
            writer.Write((Byte)models[0].material.dictTex.texNum);
            writer.Write((UInt16)models[0].material.dictTex.size);
            writer.Write((UInt16)models[0].material.dictTex.pad);
            writer.Write((UInt16)models[0].material.dictTex.texDataSize);
            writer.Write((Int32)models[0].material.dictTex.magic);
            for (int EntryCounter = 0; EntryCounter < models[0].material.dictTex.texNum; EntryCounter++)
            {
                writer.Write((Int16)models[0].material.dictTex.texDataList[EntryCounter].par);
                writer.Write((Byte)models[0].material.dictTex.texDataList[EntryCounter].par2);
                writer.Write((Byte)models[0].material.dictTex.texDataList[EntryCounter].par3);
            }
            writer.Write((Int16)models[0].material.dictTex.unitSize);
            writer.Write((Int16)models[0].material.dictTex.texDefinitionOffsetSize);
            for (int EntryCounter = 0; EntryCounter < models[0].material.dictTex.texNum; EntryCounter++)
            {
                writer.Write((Convert.ToInt16(models[0].material.dictTex.texDefinitionOffsetList[EntryCounter])));
                writer.Write(Convert.ToSByte(models[0].material.dictTex.texMaterialIdList[EntryCounter]));
                writer.Write((Byte)0);
            }
            for (int EntryCounter = 0; EntryCounter < models[0].material.dictTex.texNum; EntryCounter++)
                foreach (char c in models[0].material.dictTex.texNameList[EntryCounter].name)
                    writer.Write((Byte)c);
        }

        public void SaveNodeInfo(NsbmdModel.NodeInfoStruct nsbmdObject, BinaryWriter writer)
        {
            writer.Write((UInt32)nsbmdObject.flag);
            if ((nsbmdObject.flag & 1) == 0)
            {
                nsbmdObject.translation = true;
                writer.Write((UInt32)nsbmdObject.x);
                writer.Write((UInt32)nsbmdObject.y);
                writer.Write((UInt32)nsbmdObject.z);
            }
            if ((nsbmdObject.flag & 10) == 8)
            {
                writer.Write((UInt16)nsbmdObject.a);
                writer.Write((UInt16)nsbmdObject.b);
            }
            if ((nsbmdObject.flag & 4) == 0)
            {
                writer.Write((UInt32)nsbmdObject.unk);
                writer.Write((UInt32)nsbmdObject.unk2);
                writer.Write((UInt32)nsbmdObject.Unk4);
                writer.Write((UInt32)nsbmdObject.Unk5);
                writer.Write((UInt32)nsbmdObject.Unk6);
                writer.Write((UInt32)nsbmdObject.Unk3);
            }
        }

        public void SavePalette(BinaryWriter writer)
        {
            writer.Write((Byte)models[0].material.dictPal.revisionId);
            writer.Write((Byte)models[0].material.dictPal.palNum);
            writer.Write((UInt16)models[0].material.dictPal.size);
            writer.Write((UInt16)models[0].material.dictPal.pad);
            writer.Write((UInt16)models[0].material.dictPal.palDataSize);
            writer.Write((Int32)models[0].material.dictPal.magic);
            for (int EntryCounter = 0; EntryCounter < models[0].material.dictPal.palNum; EntryCounter++)
                writer.Write((UInt32)models[0].material.dictPal.palDataList[EntryCounter]);
            writer.Write((Int16)models[0].material.dictPal.unitSize);
            writer.Write((Int16)models[0].material.dictPal.palDefinitionOffsetSize);
            for (int EntryCounter = 0; EntryCounter < models[0].material.dictPal.palNum; EntryCounter++)
            {
                writer.Write((Convert.ToInt16(models[0].material.dictPal.palDefinitionOffsetList[EntryCounter])));
                writer.Write(Convert.ToSByte(models[0].material.dictPal.palMaterialIdList[EntryCounter]));
                writer.Write((Byte)0);
            }
            for (int EntryCounter = 0; EntryCounter < models[0].material.dictPal.palNum; EntryCounter++)
                foreach (char c in models[0].material.dictPal.palNameList[EntryCounter].name)
                    writer.Write((Byte)c);
        }

        public void SaveAssMatrix(BinaryWriter writer)
        {
            for (int texDataCounter = 0; texDataCounter < models[0].material.texIndexDataList.Count; texDataCounter++)
                writer.Write((Byte)models[0].material.texIndexDataList[texDataCounter]);
            for (int palDataCounter = 0; palDataCounter < models[0].material.palIndexDataList.Count; palDataCounter++)
                writer.Write((Byte)models[0].material.palIndexDataList[palDataCounter]);
        }

        public void SaveMatInfo(BinaryWriter writer)
        {
            for (int matCounter = 0; matCounter < models[0].modelInfo.matNum; matCounter++)
            {
                writer.Write((UInt16)models[0].material.matInfoList[matCounter].itemTag);
                writer.Write((UInt16)models[0].material.matInfoList[matCounter].size);
                writer.Write((Int32)models[0].material.matInfoList[matCounter].diffAmb);
                writer.Write((Int32)models[0].material.matInfoList[matCounter].specEmi);
                writer.Write((Int32)models[0].material.matInfoList[matCounter].polyAttr);
                writer.Write((Int32)models[0].material.matInfoList[matCounter].polyAttrMask);
                writer.Write((Int32)models[0].material.matInfoList[matCounter].texImageParam);
                writer.Write((Int32)models[0].material.matInfoList[matCounter].texImageParamMask);
                writer.Write((UInt16)models[0].material.matInfoList[matCounter].texPlttBase);
                writer.Write((UInt16)models[0].material.matInfoList[matCounter].flag);
                writer.Write((UInt16)models[0].material.matInfoList[matCounter].origWidth);
                writer.Write((UInt16)models[0].material.matInfoList[matCounter].origHeight);
                writer.Write((Int32)models[0].material.matInfoList[matCounter].magW);
                writer.Write((Int32)models[0].material.matInfoList[matCounter].magH);
                if ((models[0].material.matInfoList[matCounter].flag + 2) == 0)
                {
                    writer.Write((Int32)models[0].material.matInfoList[matCounter].scaleS);
                    writer.Write((Int32)models[0].material.matInfoList[matCounter].scaleT);
                }
                else if ((models[0].material.matInfoList[matCounter].flag + 4) == 0)
                {
                    writer.Write((Int32)models[0].material.matInfoList[matCounter].rotSin);
                    writer.Write((Int32)models[0].material.matInfoList[matCounter].rotCos);
                }
                else if ((models[0].material.matInfoList[matCounter].flag + 8) == 0)
                {
                    writer.Write((Int32)models[0].material.matInfoList[matCounter].transS);
                    writer.Write((Int32)models[0].material.matInfoList[matCounter].transT);
                }
                else if (((models[0].material.matInfoList[matCounter].flag & 2) == 0) && ((models[0].material.matInfoList[matCounter].flag & 1) == 1))
                {
                    writer.Write((Int32)models[0].material.matInfoList[matCounter].rotSin);
                    writer.Write((Int32)models[0].material.matInfoList[matCounter].rotCos);
                }
                else if ((models[0].material.matInfoList[matCounter].flag + 0x2000) == 0x7d0)
                {
                    for (int i = 0; i <= 15; i++)
                        writer.Write((Int32)models[0].material.matInfoList[matCounter].effectMtx[i]);
                }else if (models[0].material.matInfoList[matCounter].flag == 0x1fc7)
                    writer.Write((Int64)models[0].material.matInfoList[matCounter].unkBuffer);
            }
        }

        public void SaveShape(BinaryWriter writer)
        {
            writer.Write((Byte)models[0].shapeInfo.dictShp.revisionId);
            writer.Write((Byte)models[0].shapeInfo.dictShp.shapeNum);
            writer.Write((UInt16)models[0].shapeInfo.dictShp.size);
            writer.Write((UInt16)models[0].shapeInfo.dictShp.pad);
            writer.Write((UInt16)models[0].shapeInfo.dictShp.shapeDataSize);
            writer.Write((Int32)models[0].shapeInfo.dictShp.magic);
            for (int EntryCounter = 0; EntryCounter < models[0].shapeInfo.dictShp.shapeNum; EntryCounter++)
                writer.Write((UInt32)models[0].shapeInfo.dictShp.shapeDataList[EntryCounter]);
            writer.Write((Int16)models[0].shapeInfo.dictShp.unitSize);
            writer.Write((Int16)models[0].shapeInfo.dictShp.shapeDefinitionOffsetSize);
            for (int EntryCounter = 0; EntryCounter < models[0].shapeInfo.dictShp.shapeNum; EntryCounter++)
            {
                writer.Write((Convert.ToInt16(models[0].shapeInfo.dictShp.shapeDefinitionOffsetList[EntryCounter])));
                writer.Write(Convert.ToSByte(models[0].shapeInfo.dictShp.shapeMaterialIdList[EntryCounter]));
                writer.Write((Byte)0);
            }
            for (int EntryCounter = 0; EntryCounter < models[0].shapeInfo.dictShp.shapeNum; EntryCounter++)
                foreach (char c in models[0].shapeInfo.dictShp.shapeNameList[EntryCounter].name)
                    writer.Write((Byte)c);

            for (int shpCounter = 0; shpCounter < models[0].shapeInfo.dictShp.shapeNum; shpCounter++)
            {
                NsbmdModel.ShapeInfoStruct info = models[0].shapeInfo.shapeList[shpCounter];
                writer.Write((UInt16)info.itemTag);
                writer.Write((UInt16)info.size);
                writer.Write((UInt32)info.flag);
                writer.Write((UInt32)info.offsetDL);
                writer.Write((UInt32)info.sizeDL);
            }

            for (int shpCounter = 0; shpCounter < models[0].shapeInfo.dictShp.shapeNum; shpCounter++)
            {
                SaveCommand(writer, models[0].shapeInfo.shapeList[shpCounter].commandList);
                if (shpCounter < models[0].shapeInfo.dictShp.shapeNum - 1)
                    while (writer.BaseStream.Position < models[0].shapeInfo.shapeList[shpCounter + 1].startShapeInfo)
                        writer.Write((Byte)0);
                else
                    while (writer.BaseStream.Position < models[0].shapeInfo.shapeList[shpCounter].endShapeInfo)
                        writer.Write((Byte)0);
                
            }           
        }

        private void SaveCommand(BinaryWriter writer, List<NsbmdModel.CommandStruct> list)
        {
            int commandCounter =0;
           //bool isFirst = true;
            while (commandCounter < list.Count)
            {
                int idCounter = 0;
                while (idCounter < 4 && commandCounter + idCounter < list.Count)
                {
                    writer.Write((Byte)list[commandCounter + idCounter].id);
                    idCounter++;
                }
                
                idCounter = 0;
                while (idCounter < 4 && commandCounter< list.Count)
                {
                    switch (list[commandCounter].id)
                    {
                        case 0: // No Operation (for padding packed GXFIFO commands)
                            break;
                        case 0x14:
                            {
                                writer.Write((Int32)list[commandCounter].x);                            
                                break;
                            }
                        case 0x1B:
                            {
                                writer.Write((Int32)list[commandCounter].x);
                                writer.Write((Int32)list[commandCounter].y);
                                writer.Write((Int32)list[commandCounter].z);
                                break;

                            }
                        case 0x20:
                            {
                                writer.Write((Int32)list[commandCounter].par);
                            }
                            break;

                        case 0x21:
                            {
                                writer.Write((Int32)list[commandCounter].par);
                                break;
                            }
                        case 0x22:
                            {
                                writer.Write((Int32)list[commandCounter].par);
                                break;
                            }
                        case 0x23:
                            {
                                writer.Write((Int32)list[commandCounter].par);
                                writer.Write((Int32)list[commandCounter].par2);
                                break;
                            }
                        case 0x24:
                            {
                                writer.Write((Int32)list[commandCounter].par);
                                break;
                            }
                        case 0x25:
                            {
                                writer.Write((Int32)list[commandCounter].par);
                                break;
                            }
                        case 0x26:
                            {
                                writer.Write((Int32)list[commandCounter].par);
                                break;
                            }
                        case 0x27:
                            {
                                writer.Write((Int32)list[commandCounter].par);
                                break;
                            }
                        case 0x28:
                            {
                                writer.Write((Int32)list[commandCounter].par);
                                break;
                            }
                        case 0x40:
                            {
                                writer.Write((Int32)list[commandCounter].par);
                                break;
                            }
                        case 0x41:
                            break;
                    }
                    if (idCounter < 4)
                        commandCounter++;
                    idCounter++;
                }
                //if (isFirst)
                //{
                //    commandCounter += 3;
                //    isFirst = false;
                //}
                //else
            }
        }


        #endregion

        public void MatchTextures()
        {
            for (int modelCounter = 0; modelCounter < models.Length; modelCounter++)
            {
                for (int matCounter = 0; matCounter < models[modelCounter].matTexPalList.Count; matCounter++)
                {
                    bool texMatched = false;
                    bool palMatched = false;
                    foreach (NsbmdModel.MatTexPalStruct actualMatTex in actualTex.tex0.matTexPalList)
                    {
                        if (matCounter <   actualTex.tex0.matTexPalList.Count)
                        {
                            NsbmdModel.MatTexPalStruct otherMatTex =   actualTex.tex0.matTexPalList[matCounter];
                            if (!(texMatched || !actualMatTex.texName.Equals(otherMatTex.texName)))
                            {
                                otherMatTex = models[modelCounter].CopyTo(actualMatTex, otherMatTex);
                                texMatched = true;
                            }
                            if (((otherMatTex.format != 7) && !palMatched) && actualMatTex.palName.Equals(otherMatTex.palName))
                            {
                                otherMatTex.palName = actualMatTex.palName;
                                otherMatTex.palSize = actualMatTex.palSize;
                                otherMatTex.palData = actualMatTex.palData;
                                palMatched = true;
                            }
                        }
                    }
                }
            }
        }

        private static NsbmdModel.NodeInfoStruct readNodeInfo(BinaryReader reader)
        {
            var nsbmdObject = new NsbmdModel.NodeInfoStruct();
            nsbmdObject.flag = reader.ReadUInt32();
            if ((nsbmdObject.flag & 1) == 0)
            {
                nsbmdObject.translation = true;
                nsbmdObject.x = reader.ReadUInt32();
                nsbmdObject.y = reader.ReadUInt32();
                nsbmdObject.z = reader.ReadUInt32();
            }
            if ((nsbmdObject.flag & 10) == 8)
            {
                nsbmdObject.isRotated = true;
                nsbmdObject.a = reader.ReadUInt16();
                nsbmdObject.rotA = nsbmdObject.a;
                nsbmdObject.b = reader.ReadUInt16();
                nsbmdObject.rotB = nsbmdObject.b;
                nsbmdObject.pivot = (nsbmdObject.flag >> 4) & 15;
                nsbmdObject.negation = (nsbmdObject.flag >> 8) & 15;
            }
            if ((nsbmdObject.flag & 4) == 0)
            {
                nsbmdObject.unk = reader.ReadUInt32();
                nsbmdObject.unk2 = reader.ReadUInt32();
                nsbmdObject.Unk4 = reader.ReadUInt32();
                nsbmdObject.Unk5 = reader.ReadUInt32();
                nsbmdObject.Unk6 = reader.ReadUInt32();
                nsbmdObject.Unk3 = reader.ReadUInt32();
            }
            return nsbmdObject;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Table_s
        {
            public int mat;
            public int pol;
            public int node;
        }

        public struct Sbc_s
        {
            public byte id;
            public List<byte> parameters;
        }

        public Nsbmd removePolygon(Int32 idPolygon)
        {
            var model = models[0];
            var modelInfo = model.modelInfo;
            var shapeInfo = model.shapeInfo;

            var polyDataSize = shapeInfo.shapeList[idPolygon].size;

            models[0].shapeInfo.dictShp = removeFromDictShp(idPolygon, shapeInfo.dictShp);

            removeFromSize(24);
            removedSize += 24;

            polygonLength = removeFromShapeList(idPolygon);
            removeFromSize(16 + polygonLength);
            removedSize += 16 + polygonLength;

            sbc = removeFromSbc(idPolygon);

            removeFromSize(4);
            removedSize += 4;

            startMaterials -= 4;

            models[0].modelInfo = removeFromModelInfo(idPolygon);

            if (sectionNum > 1)
                sectionData[sectionNum-1] -= removedSize - 4;

            return this;
        }

        private NsbmdModel.ModelInfoStruct removeFromModelInfo(int idPolygon)
        {
            var modelInfo = models[0].modelInfo;
            modelInfo.size -= (uint)removedSize;
            modelInfo.bdhcOffset -= (UInt32)polygonLength;
            modelInfo.shapeNum -= 1;
            modelInfo.matOffset -= 4;
            modelInfo.shapeOffset -= 4;
            return modelInfo;

        }

        private List<Sbc_s> removeFromSbc(int idPolygon)
        {
            var newSbc = new List<Sbc_s>();
            for (int i=0; i < sbc.Count-1; i++)
            {
                if (sbc[i].id == 0x4 && sbc[i+1].id == 0x5 && idPolygon == sbc[i + 1].parameters[0])
                    i++;
                else
                    newSbc.Add(sbc[i]);
            }
            newSbc.Add(sbc[sbc.Count-1]);
            sbc = newSbc;
            for (int i = 0; i < sbc.Count; i++)
            {
                var sbcElement = sbc[i];
                if (sbc[i].id == 0x4)
                {
                    Byte idPol = sbc[i + 1].parameters[0];
                    if (idPol > (Byte)idPolygon)
                      sbc[i + 1].parameters[0] -=1;
                }
            }

            return sbc;
        }

        private int removeFromShapeList(Int32 idPolygon)
        {
            var shapeList = models[0].shapeInfo.shapeList;
            var newShapeList = new List<NsbmdModel.ShapeInfoStruct>();

            var polygonLength = shapeList[idPolygon].polygonData.Length;

            for (int i = 0; i < shapeList.Count; i++)
            {
                if (i < idPolygon)
                    newShapeList.Add(shapeList[i]);
                else if (i > idPolygon)
                {
                    var shape = shapeList[i];
                    var precShape = shapeList[i - 1];
                    shape.offsetDL = (uint)(precShape.offsetDL + precShape.sizeDL - 16 - polygonLength);
                    shape.startShapeInfo = shape.offsetDL;
                    shape.endShapeInfo = shape.offsetDL + shape.sizeDL;
                    newShapeList.Add(shape);
                }
            }

            models[0].shapeInfo.shapeList = newShapeList;
        
            return polygonLength;
        }


        private void removeFromSize(int offset)
        {
            sizeBMD0 -= offset;
            models[0].blockSize -= offset;
        }

        private static NsbmdModel.DictShapeStruct removeFromDictShp(Int32 idPolygon, NsbmdModel.DictShapeStruct dictShp)
        {
            dictShp.shapeNum -= 1;
            dictShp.shapeDataList.RemoveAt(idPolygon);
            dictShp.shapeDataSize -= 4;
            dictShp.size -= 4;
            for (int i = dictShp.shapeNum; i > 0; i--)
            {
                if (i < idPolygon)
                    dictShp.shapeDefinitionOffsetList[i] = (Int32)((Int32)dictShp.shapeDefinitionOffsetList[i]);
                else
                    dictShp.shapeDefinitionOffsetList[i] = (Int32)((Int32)dictShp.shapeDefinitionOffsetList[i] - 24 - 16);
            }
            dictShp.shapeDefinitionOffsetList.RemoveAt(idPolygon);
            dictShp.shapeDefinitionOffsetSize -= 4;
            dictShp.size -= 4;

            dictShp.shapeMaterialIdList.RemoveAt(idPolygon);
            dictShp.size -= 4;
            for (int i = dictShp.shapeNum; i > idPolygon; i--)
                dictShp.shapeNameList[i] = dictShp.shapeNameList[i - 1];
            dictShp.shapeNameList.RemoveAt(idPolygon);

            dictShp.size -= 16;

            return dictShp;
        }


        public Nsbmd addPolygon(int idPolygon, int polygonLength, string newTexture, string newPalette)
        {
            var model = models[0];
            var modelInfo = model.modelInfo;
            var shapeInfo = model.shapeInfo;

            polygonLength = shapeInfo.shapeList[1].polygonData.Length;


            models[0].shapeInfo.dictShp = addInDictShp(idPolygon, shapeInfo.dictShp);

            addInSize(24);
            addedSize += 24;

            addInShapeList(idPolygon,polygonLength);

            addInSize(16 + polygonLength);
            addedSize += 16 + polygonLength;

            models[0].material = addInMaterials(models[0].material, newTexture, newPalette);

            sbc = addInSbc(idPolygon, models[0].material.dictMat.matNum - 1);

            addInSize(4);
            addedSize += 4;

            startMaterials += 4;

            models[0].modelInfo = addInModelInfo(idPolygon, polygonLength, true);

            if (sectionNum > 1)
                sectionData[sectionNum - 1] += addedSize;
            //}

            
            #region scene
            //int idMaterial = -1;
            //int idTexture = -1;
            //int idPalette = -1;
            //for (int i = 0; i < models[0].matTexPalList.Count && idMaterial == -1; i++)
            //{
            //    var actualMatTex = models[0].matTexPalList[i];
            //    if (actualMatTex.texName == newTexture + getZeroEnd(newTexture)){
            //        string matName = actualMatTex.matName;
            //        for (int j = 0; j < models[0].material.dictMat.matNameList.Count && idMaterial==-1; j++)
            //        {
            //            var dictName = models[0].material.dictMat.matNameList[j];
            //            if (dictName.name == matName)
            //                idMaterial = j;
            //        }
            //        string texName = actualMatTex.texName;
            //        for (int j = 0; j < models[0].material.dictTex.texNameList.Count && idTexture==-1; j++)
            //        {
            //            var dictName = models[0].material.dictTex.texNameList[j];
            //            if (dictName.name == texName)
            //                idTexture = j;
            //        }
            //        string palName = actualMatTex.palName;
            //        for (int j = 0; j < models[0].material.dictPal.palNameList.Count && idPalette==-1; j++)
            //        {
            //            var dictName = models[0].material.dictPal.palNameList[j];
            //            if (dictName.name == palName)
            //                idPalette = j;
            //        }
            //    }
            //}
            //if (idMaterial !=-1)
            //{

            //    var oldTexValue = models[0].material.dictTex.texMaterialIdList;
            //    oldTexValue[idTexture] = (int)oldTexValue[idTexture] + 1;
            //    models[0].material.dictTex.texMaterialIdList = oldTexValue;

            //    var oldPalValue = models[0].material.dictPal.palMaterialIdList;
            //    oldTexValue[idPalette] = (int)oldTexValue[idPalette] + 1;
            //    models[0].material.dictPal.palMaterialIdList = oldPalValue;


            //    models[0].material.palIndexDataList.Insert(idPalette,(Byte)(models[0].material.dictMat.matNum - 1)); 
            //    models[0].material.texIndexDataList.Insert(idTexture,(Byte)(models[0].material.dictMat.matNum - 1));


            //    addedSize += 2;
            //    addInSize(2);

            //    sbc = addInSbc(idPolygon, idMaterial);

            //    addInSize(4);
            //    addedSize += 4;

            //    startMaterials += 4;

            //    models[0].modelInfo = addInModelInfo(idPolygon, polygonLength, false);

            //    if (sectionNum > 1)
            //        sectionData[sectionNum - 1] += addedSize;

            //}
            //else
            //{ 
            #endregion

            return this;
        }

        private NsbmdModel.ModelInfoStruct addInModelInfo(int idPolygon, int size, bool isNewMat)
        {
            var modelInfo = models[0].modelInfo;
            modelInfo.size += (uint)addedSize;
            modelInfo.bdhcOffset += (uint)addedSize;
            if (isNewMat)
                modelInfo.matNum += 1;
            modelInfo.matOffset += 4;
            modelInfo.shapeNum += 1;
            if (isNewMat)
                modelInfo.shapeOffset += 4 + 24 + 24 + 24 + 2 + (uint)models[0].material.matInfoList[0].size ;
            else
                modelInfo.shapeOffset += 4 + 2;
            return modelInfo;
        }

        private NsbmdModel.MaterialSectionStruct addInMaterials(NsbmdModel.MaterialSectionStruct materialSectionStruct, string newTexture, string newPalette)
        {
            materialSectionStruct.dictMat = addInDictMat(materialSectionStruct.dictMat, newTexture);
            materialSectionStruct.dictTex = addInDictTex(materialSectionStruct.dictTex, newTexture);
            materialSectionStruct.dictPal = addInDictPal(materialSectionStruct.dictPal, newPalette);

            materialSectionStruct.dictTexOffset += 24;
            materialSectionStruct.dictPalOffset += 48;

            materialSectionStruct.matInfoList = addInMatInfo(materialSectionStruct.matInfoList);

            materialSectionStruct.palIndexDataList.Add((Byte)(materialSectionStruct.dictMat.matNum - 1));
            materialSectionStruct.texIndexDataList.Add((Byte)(materialSectionStruct.dictMat.matNum - 1));


            addedSize += 24 + 24 + 24 + materialSectionStruct.matInfoList[0].size + 2;
            addInSize(24 + 24 + 24 + materialSectionStruct.matInfoList[0].size + 2);

            return materialSectionStruct;
            
        }

        private static List<NsbmdModel.MatInfoStruct> addInMatInfo(List<NsbmdModel.MatInfoStruct> matInfoList)
        {
            matInfoList.Add(new NsbmdModel.MatInfoStruct()
            {
                diffAmb = matInfoList[0].diffAmb,
                effectMtx = matInfoList[0].effectMtx,
                flag = matInfoList[0].flag,
                heigth = matInfoList[0].heigth,
                itemTag = matInfoList[0].itemTag,
                magH = matInfoList[0].magH,
                magW = matInfoList[0].magW,
                origHeight = matInfoList[0].origHeight,
                origWidth = matInfoList[0].origWidth,
                polyAttr = matInfoList[0].polyAttr,
                polyAttrMask = matInfoList[0].polyAttrMask,
                rotCos = matInfoList[0].rotCos,
                rotSin = matInfoList[0].rotSin,
                scaleS = matInfoList[0].scaleS,
                scaleT = matInfoList[0].scaleT,
                size = matInfoList[0].size,
                specEmi = matInfoList[0].specEmi,
                texImageParam = matInfoList[0].texImageParam,
                texImageParamMask = matInfoList[0].texImageParamMask,
                texPlttBase = matInfoList[0].texPlttBase,
                transS = matInfoList[0].transS,
                transT = matInfoList[0].transT,
                unkBuffer = matInfoList[0].unkBuffer,
                width = matInfoList[0].width
            });

            return matInfoList;
        }

        private NsbmdModel.DictMatStruct addInDictMat(NsbmdModel.DictMatStruct dictMat, string newName)
        {
            dictMat.matNum+= 1;
            dictMat.matDataList.Add(dictMat.matDataList[dictMat.matDataList.Count - 1]);
            dictMat.matDataSize += 4;
            dictMat.size += 4;
            var newDefinitionOffset = UInt32.Parse(dictMat.matDefinitionOffsetList[dictMat.matDefinitionOffsetList.Count - 1].ToString());
            dictMat.matDefinitionOffsetList.Add(newDefinitionOffset + 44);

            for (int i = 0; i < dictMat.matDefinitionOffsetList.Count; i++)
            {
                var newDefinition = UInt32.Parse(dictMat.matDefinitionOffsetList[i].ToString());
                newDefinition = newDefinition + 24 + 24 + 24 + 2;
                dictMat.matDefinitionOffsetList[i] = newDefinition;
            }

            dictMat.matDefinitionOffsetSize += 4;
            dictMat.size += 4;

            dictMat.matNameList.Add(new NsbmdModel.DictNameStruct(){ name = newName + "_lm2" + getZeroEnd(newName + "_lm2") });

            dictMat.size += 16;

            return dictMat;
        }

        private NsbmdModel.DictTexStruct addInDictTex(NsbmdModel.DictTexStruct dictTex, string newName)
        {
            dictTex.texNum += 1;
            dictTex.texDataList.Add(dictTex.texDataList[dictTex.texDataList.Count - 1]);
            dictTex.texDataSize += 4;
            dictTex.size += 4;
            var newDefinitionOffset = Int16.Parse(dictTex.texDefinitionOffsetList[dictTex.texDefinitionOffsetList.Count - 1].ToString());
            dictTex.texDefinitionOffsetList.Add(newDefinitionOffset + 1);

            for (int i = 0; i < dictTex.texDefinitionOffsetList.Count; i++)
            {
                var newDefinition = UInt16.Parse(dictTex.texDefinitionOffsetList[i].ToString());
                newDefinition = (UInt16)(newDefinition + 24 + 24 + 24);
                dictTex.texDefinitionOffsetList[i] = newDefinition;
            }

            dictTex.texDefinitionOffsetSize += 4;
            dictTex.texMaterialIdList.Add((Byte)1);
            dictTex.size += 4;

            dictTex.texNameList.Add(new NsbmdModel.DictNameStruct() { name = newName + getZeroEnd(newName) });

            dictTex.size += 16;

            return dictTex;
        }

        private NsbmdModel.DictPalStruct addInDictPal(NsbmdModel.DictPalStruct dictPal, string newName)
        {
            dictPal.palNum += 1;
            dictPal.palDataList.Add(dictPal.palDataList[dictPal.palDataList.Count - 1]);
            dictPal.palDataSize += 4;
            dictPal.size += 4;
            var newDefinitionOffset = Int16.Parse(dictPal.palDefinitionOffsetList[dictPal.palDefinitionOffsetList.Count - 1].ToString());
            dictPal.palDefinitionOffsetList.Add(newDefinitionOffset + 1);

            for (int i = 0; i < dictPal.palDefinitionOffsetList.Count; i++)
            {
                var newDefinition = UInt16.Parse(dictPal.palDefinitionOffsetList[i].ToString());
                newDefinition = (UInt16)(newDefinition + 24 + 24 + 24 + 1);
                dictPal.palDefinitionOffsetList[i] = newDefinition;
            }

            dictPal.palDefinitionOffsetSize += 4;
            dictPal.palMaterialIdList.Add((Byte)1);
            dictPal.size += 4;

            dictPal.palNameList.Add(new NsbmdModel.DictNameStruct() { name = newName + getZeroEnd(newName) });

            dictPal.size += 16;

            return dictPal;
        }

        private List<Sbc_s> addInSbc(int idPolygon, int idMaterial)
        {
            var newSbc = new List<Sbc_s>();
            for (int i = 0; i < sbc.Count; i++)
            {
                newSbc.Add(sbc[i]);
                if (sbc[i].id == 0xB)
                {
                    var newMaterialSbc = new Sbc_s();
                    newMaterialSbc.id = 0x4;
                    newMaterialSbc.parameters = new List<byte>();
                    newMaterialSbc.parameters.Add((byte)idMaterial);
                    newSbc.Add(newMaterialSbc);

                    var newPolygonSbc = new Sbc_s();
                    newPolygonSbc.id = 0x5;
                    newPolygonSbc.parameters = new List<byte>();
                    newPolygonSbc.parameters.Add((Byte)idPolygon);
                    newSbc.Add(newPolygonSbc);
                }

            }
            sbc = newSbc;
            return sbc;
        }

        private void addInShapeList(int idPolygon, int size)
        {
            var shapeList = models[0].shapeInfo.shapeList;

            for (int i = 0; i < shapeList.Count; i++)
            {
                var shapeElement = shapeList[i];
                shapeElement.offsetDL += 16;
                shapeList[i] = shapeElement;
            }

            var newListElement = new NsbmdModel.ShapeInfoStruct();

            newListElement.size = 16;
            newListElement.offsetDL = (uint)(shapeList[idPolygon - 1].offsetDL + shapeList[idPolygon - 1].sizeDL - 16);
            newListElement.sizeDL = size;
            newListElement.itemTag = 0;
            newListElement.flag = 5;

            newListElement.commandList = new List<NsbmdModel.CommandStruct>();

            //for (int i = 0; i < size; i++)
            //    newListElement.commandList.Add(new NsbmdModel.CommandStruct() { id = 0 });

            newListElement.commandList = shapeList[1].commandList;

            shapeList.Add(newListElement);

            models[0].shapeInfo.shapeList = shapeList;         
  
        }

        private void addInSize(int offset)
        {
            sizeBMD0 += offset;
            models[0].blockSize += offset;
        }

        private NsbmdModel.DictShapeStruct addInDictShp(int newIdPolygon, NsbmdModel.DictShapeStruct dictShp)
        {
            dictShp.shapeNum += 1;
            dictShp.shapeDataList.Add(dictShp.shapeDataList[newIdPolygon - 1]);
            dictShp.shapeDataSize += 4;
            dictShp.size += 4;
            dictShp.shapeDefinitionOffsetList.Add(Int16.Parse(dictShp.shapeDefinitionOffsetList[newIdPolygon - 1].ToString()) + 16);

            for (int i = 0; i < dictShp.shapeDefinitionOffsetList.Count; i++)
            {
                var newDefinition = UInt16.Parse(dictShp.shapeDefinitionOffsetList[i].ToString());
                newDefinition = (UInt16)(newDefinition + 24);
                dictShp.shapeDefinitionOffsetList[i] = newDefinition;
            }

            dictShp.shapeDefinitionOffsetSize += 4;
            dictShp.shapeMaterialIdList.Add((Byte)0);
            
            dictShp.size += 4;

            dictShp.shapeNameList.Add(new NsbmdModel.DictNameStruct() { name = "polygon" + newIdPolygon + getZeroEnd("polygon" + newIdPolygon) });


            dictShp.size += 16;

            return dictShp;
        }

        private string getZeroEnd(string word)
        {
            var str = new StringBuilder();
            for (int i=0; i< 16 - word.Length; i ++)
                str.Append('\0');
            return str.ToString();
        }


        public Nsbmd resetPolygon(int idPolygon)
        {
            var polyData = new List<Byte>();
            var shapeInfo = models[0].shapeInfo.shapeList[idPolygon]; 
            var newCommandList = new List<NsbmdModel.CommandStruct>();
            for (int i=0; i< shapeInfo.sizeDL;i++)
            {
                var newCommand = new NsbmdModel.CommandStruct();
                newCommand.id = 0x0;
                newCommandList.Add(newCommand);
            }
            shapeInfo.commandList = newCommandList;
            models[0].shapeInfo.shapeList[idPolygon] = shapeInfo;
            return this;
        }

        public Nsbmd resizePolygon(int idPolygon, int polyDataSize)
        {
            var model = models[0];
            var modelInfo = model.modelInfo;
            var shapeInfo = models[0].shapeInfo;

            int oldSize = shapeInfo.shapeList[idPolygon].sizeDL;

            int pad =0;
            for (int i = 0; i < shapeInfo.shapeList.Count; i++)
            {
                var actualShape = shapeInfo.shapeList[i];
                if (i == idPolygon)
                {
                    actualShape.sizeDL = polyDataSize;
                    pad = polyDataSize - oldSize;
                    if (pad > 0)
                        for (int k = 0; k < pad; k++)
                        {
                            var newCommand = new NsbmdModel.CommandStruct();
                            newCommand.id = 0x0;
                            actualShape.commandList.Add(newCommand);
                        }
                    else
                        for (int k = 0; k < Math.Abs(pad); k++)
                        {
                            var oldCommand = actualShape.commandList[actualShape.commandList.Count - 1];
                            if (oldCommand.id == 0x40 || oldCommand.id == 0x21 || oldCommand.id == 0x22 || oldCommand.id == 0x24 || oldCommand.id == 0x25 || oldCommand.id == 0x26 || oldCommand.id == 0x27 || oldCommand.id == 0x28)
                                k += 4;
                            else if (oldCommand.id == 0x23)
                                k += 8;
                            actualShape.commandList.RemoveAt(actualShape.commandList.Count - 1);
                        }
                    Array.Resize(ref actualShape.polygonData, polyDataSize);
                }

                else if (i > idPolygon)
                    actualShape.offsetDL += (uint)(pad);
                shapeInfo.shapeList[i] = actualShape;
            }
            
            models[0].shapeInfo = shapeInfo;
            
            sizeBMD0 += pad;
            models[0].blockSize += pad;

            if (sectionNum == 2)
                sectionData[1] += pad;

            return this;
        }



        public void SaveIMD0(StreamWriter writer)
        {
            writer.WriteLine("<?xml version=\"1.0\" encoding=\"Shift_JIS\"?>");
            writer.WriteLine("<imd version=”1.2.0”>");
            writer.WriteLine("<head>");
            writer.WriteLine("<create user=\"nintendo\" host=\"nintendo-pc\" date=\"2003-10-31T14:25:43\"source=\"n64_mario.mb\"/>");
            writer.WriteLine("</head>");
        }
    }
}

