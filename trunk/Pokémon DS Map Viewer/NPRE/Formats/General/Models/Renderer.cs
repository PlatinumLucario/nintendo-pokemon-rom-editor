namespace PG4Map.Formats
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Tao.OpenGl;
    using System.Windows.Forms;

    public class Renderer
    {
        #region Public
        public List<Nsbmd> modelList = new List<Nsbmd>();
        public int polyval = 0;
        public int text_on;
        public int[] polMatList = new int[0];
        public int cKeyPressInc = 20;
        public int idActualMaterial;
        public int num_obj;
        public int idActualPolygon;
        #endregion 

        private RGBA[] image;
        private MTX44 CurrentMatrix;
        private NsbmdModel.MatTexPalStruct actualMat;
        private MapEditor mapEditor;
        private static int gCurrentVertex = 0;
        private float transZ;
        private float rotY;
        private float rotX;
        private float transX;
        private float rotZ;
        private static MTX44[] MatrixStack = new MTX44[0x1f];
        private const float SCALE_IV = 4096f;
        private int stackID;
        private static readonly string[] TEXTURE_FORMATS = new string[] { "", "A3I5", "4-Color", "16-Color", "256-Color", "4x4-Texel", "A5I3", "Direct Texture" };
        private int idModel = 0;
        private float transY;
        public  int mode;
        private int xBuffer;
        private int zBuffer;
        private int yBuffer;


        public Renderer(MapEditor mapEditor)
        {
            for (int i = 0; i < MatrixStack.Length; i++)
                MatrixStack[i] = new MTX44();
            this.mapEditor = mapEditor;
        }

        #region Convert
        public bool convert_4x4texel(uint[] tex, int width, int height, ushort[] data, RGBA[] pal, RGBA[] rgbaOut)
        {

            int fixedWidth = width / 4;
            int fixedHeight = height / 4;
            if (data.Length <=fixedHeight*fixedWidth)
                return true;
            
            for (int heightCounter = 0; heightCounter < fixedHeight; heightCounter++)
            {
                for (int widthCounter = 0; widthCounter < fixedWidth; widthCounter++)
                {
                    int index = (heightCounter * fixedWidth) + widthCounter;
                    uint actualTexId = tex[index];
                    ushort actualData = data[index];
                    ushort address = (ushort)(actualData & 0x3fff);
                    ushort mode = (ushort)((actualData >> 14) & 3);

                    startTextureAnalisys(width, pal, rgbaOut, heightCounter, widthCounter, actualTexId, address, mode);
                }
            }
            return true;
        }

        private static void startTextureAnalisys(int width, RGBA[] pal, RGBA[] rgbaOut, int heightCounter, int widthCounter, uint actualTexId, ushort address, ushort mode)
        {
            for (int k = 0; k < 4; k++)
            {
                for (int m = 0; m < 4; m++)
                {
                    int texel = ((int)(actualTexId >> (((k * 4) + m) * 2))) & 3;
                    RGBA pixel = rgbaOut[(((heightCounter * 4) + k) * width) + ((widthCounter * 4) + m)];
                    pixel = loadPixel(pal, address, mode, texel, pixel);

                }
            }
        }

        private static RGBA loadPixel(RGBA[] pal, ushort address, ushort mode, int texel, RGBA pixel)
        {
            switch (mode)
            {
                case 0:
                    {
                        pixel = pal[(address << 1) + texel];
                        if (texel == 3)
                            pixel = RGBA.Transparent;
                        break;
                    }
                case 1:
                    {
                        switch (texel)
                        {
                            case 2:
                                pixel.R = (byte)(((long)(pal[address << 1].R + pal[(address << 1) + 1].R)) / 2L);
                                pixel.G = (byte)(((long)(pal[address << 1].G + pal[(address << 1) + 1].G)) / 2L);
                                pixel.B = (byte)(((long)(pal[address << 1].B + pal[(address << 1) + 1].B)) / 2L);
                                pixel.A = 0xff;
                                break;
                            case 3:
                                pixel = RGBA.Transparent;
                                break;
                        }
                        break;
                    }
                case 2:
                    {
                        pixel = pal[(address << 1) + texel];
                        break;
                    }
                case 3:
                    {
                        switch (texel)
                        {
                            case 0:
                            case 1:
                                pixel = pal[(address << 1) + texel];
                                break;
                            case 2:
                                pixel.R = (byte)(((pal[address << 1].R * 5L) + (pal[(address << 1) + 1].R * 3L)) / ((long)8L));
                                pixel.G = (byte)(((pal[address << 1].G * 5L) + (pal[(address << 1) + 1].G * 3L)) / ((long)8L));
                                pixel.B = (byte)(((pal[address << 1].B * 5L) + (pal[(address << 1) + 1].B * 3L)) / ((long)8L));
                                pixel.A = 0xff;
                                break;
                            case 3:
                                pixel.R = (byte)(((pal[address << 1].R * 3L) + (pal[(address << 1) + 1].R * 5L)) / ((long)8L));
                                pixel.G = (byte)(((pal[address << 1].G * 3L) + (pal[(address << 1) + 1].G * 5L)) / ((long)8L));
                                pixel.B = (byte)(((pal[address << 1].B * 3L) + (pal[(address << 1) + 1].B * 5L)) / ((long)8L));
                                pixel.A = 0xff;
                                break;
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            pixel = pal[(address << 1) + texel];
            return pixel;
        }

        public void convert_4x4texel_b(byte[] tex, int width, int height, byte[] data, RGBA[] pal, RGBA[] rgbaOut)
        {
            List<uint> texList = makeTexList(tex);
            List<ushort> dataList = makeDataList(data);
            bool flag = convert_4x4texel(texList.ToArray(), width, height, dataList.ToArray(), pal, rgbaOut);
        }

        private static List<ushort> makeDataList(byte[] data)
        {
            List<ushort> dataList = new List<ushort>();
            for (int dataCounter = 0; dataCounter < ((data.Length + 1) / 2); dataCounter++)
                dataList.Add(Utils.Read2BytesAsUInt16(data, dataCounter * 2));
            return dataList;
        }

        private static List<uint> makeTexList(byte[] tex)
        {
            List<uint> texList = new List<uint>();
            for (int texCounter = 0; texCounter < ((tex.Length + 1) / 4); texCounter++)
                texList.Add(Utils.Read4BytesAsUInt32(tex, texCounter * 4));
            return texList;
        }

        #endregion

        #region Painting Texture

        public void MakeTexture(NsbmdModel mod, List<NsbmdModel.MatTexPalStruct> matList, int idActualMaterial, Boolean modeSingular)
        {
            if (matList == null)
                matList = mod.matTexPalList;
            if (matList[idActualMaterial].format == 0)
                return;

            actualMat = matList[idActualMaterial];
            if (actualMat.palData == null)
                return;

            checkFormat();

            checkImageFlipRotated();

            //MapEditor.Console.AppendText("\nFinish create image " + image.Length);
            List<byte> list = new List<byte>();
            for (int j = 0; j < image.Length; j++)
            {
                list.Add(image[j].R);
                list.Add(image[j].G);
                list.Add(image[j].B);
                list.Add(image[j].A);
            }
            byte[] pixels = list.ToArray();

            if (isOneMaterialMap(matList, idActualMaterial))
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, (int)0);
            if (modeSingular == true)
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, (int)1);
            else
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, (int)idActualMaterial + 1);

            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, 0x1908, actualMat.width, actualMat.heigth, 0, 0x1908, 0x1401, pixels);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, 0x2801, 0x2600);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, 0x2800, 0x2600);
        }

        private static bool isOneMaterialMap(List<NsbmdModel.MatTexPalStruct> matList, int idActualMaterial)
        {
            return idActualMaterial == 0 && matList.Count == 1;
        }

        private void checkImageFlipRotated()
        {
            int pixelNum = actualMat.width * actualMat.heigth;
            if ((actualMat.flipS == 1) && (actualMat.repeatS > 0))
            {
                var newImage = new RGBA[pixelNum * 2];
                int newWidth = actualMat.width * 2;
                int newWidth2 = newWidth - 1;
                int heightCounter = 0;
                while (heightCounter < actualMat.heigth)
                {
                    int texBase = heightCounter * actualMat.width;
                    int newBase = heightCounter * newWidth;
                    int widthCounter = 0;
                    while (widthCounter < actualMat.width)
                    {
                        var pixel = image[texBase + widthCounter];
                        newImage[newBase + widthCounter] = pixel;
                        newImage[(newBase + newWidth2) - widthCounter] = pixel;
                        widthCounter++;
                    }
                    heightCounter++;
                }
                actualMat.width = newWidth;
                image = newImage;
            }
            if ((actualMat.flipT == 1) && (actualMat.repeatT > 0))
            {
                var newImage = new RGBA[pixelNum * 2];
                var newHeigth = actualMat.heigth * 2;
                var newHeight2 = actualMat.heigth - 1;
                int heigthCounter = 0;
                while (heigthCounter < actualMat.heigth)
                {
                    int texBase = heigthCounter * actualMat.width;
                    int newBase = (newHeight2 - heigthCounter) * actualMat.width;
                    int widthCounter = 0;
                    while (widthCounter < actualMat.width)
                    {
                        newImage[newBase + widthCounter] = image[texBase + widthCounter];
                        widthCounter++;
                    }
                    heigthCounter++;
                }
                actualMat.heigth = newHeigth;
                image = newImage;
            }
            if ((((actualMat.flipS == 1) && (actualMat.repeatS > 0)) && (actualMat.flipT == 1)) && (actualMat.repeatT > 0))
            {
                var newImage = new RGBA[pixelNum * 4];
                int newWidth = actualMat.width * 2;
                int newWidth2 = newWidth - 1;
                int newHeigth = actualMat.heigth * 2;
                int newHeigth2 = newHeigth - 1;
                for (int heigthCounter = 0; heigthCounter < actualMat.heigth; heigthCounter++)
                {
                    int texBase = heigthCounter * actualMat.width;
                    int topBase = heigthCounter * newWidth;
                    int bottomBase = (newHeigth2 - heigthCounter) * newWidth;
                    for (int widthCounter = 0; widthCounter < actualMat.width; widthCounter++)
                    {
                        var pixel = image[texBase + widthCounter];
                        newImage[topBase + widthCounter] = pixel;
                        newImage[(topBase + newWidth2) - widthCounter] = pixel;
                        newImage[bottomBase + widthCounter] = pixel;
                        newImage[(bottomBase + newWidth2) - widthCounter] = pixel;
                    }
                }
                actualMat.width = newWidth;
                actualMat.heigth = newHeigth;
                image = newImage;
            }
        }

        private void checkFormat()
        {
            int pixelNum = actualMat.width * actualMat.heigth;
            image = new RGBA[pixelNum];
            if (actualMat.colorBase != 0)
                actualMat.palData[0] = RGBA.Transparent;
            switch (actualMat.format)
            {
                case 0:
                    {
                        break;
                    }
                case 1:
                    int pixelCounter = 0;
                    while (pixelCounter < pixelNum)
                    {
                        int index = actualMat.texData[pixelCounter] & 0x1f;
                        int alpha = (actualMat.texData[pixelCounter] >> 5) & 7;
                        alpha = ((alpha * 4) + (alpha / 2)) << 3;
                        image[pixelCounter] = actualMat.palData[index];
                        image[pixelCounter].A = (byte)alpha;
                        pixelCounter++;
                    }
                    break;

                case 2:
                    if (actualMat.colorBase != 0)
                        actualMat.palData[0] = RGBA.Transparent;
                    pixelCounter = 0;
                    while (pixelCounter < pixelNum)
                    {
                        int index = actualMat.texData[pixelCounter / 4];
                        index = (index >> ((pixelCounter % 4) << 1)) & 3;
                        image[pixelCounter] = actualMat.palData[index];
                        pixelCounter++;
                    }
                    break;
                case 3:
                    if (actualMat.colorBase != 0)
                        actualMat.palData[0] = RGBA.Transparent;
                    pixelCounter = 0;
                    while (pixelCounter < pixelNum)
                    {
                        int matIndex = pixelCounter / 2;
                        if (actualMat.texData.Length >= matIndex)
                        {
                            int index = actualMat.texData[matIndex];
                            index = (index >> ((pixelCounter % 2) << 2)) & 15;
                            if (((actualMat.palData != null) && ((index >= 0) && (index < actualMat.palData.Length))) && ((pixelCounter >= 0) && (pixelCounter < pixelNum)))
                                image[pixelCounter] = actualMat.palData[index];
                        }
                        pixelCounter++;
                    }
                    break;
                case 4:
                    if (actualMat.colorBase != 0)
                        actualMat.palData[0] = RGBA.Transparent;
                    pixelCounter = 0;
                    while (pixelCounter < pixelNum)
                    {
                        image[pixelCounter] = actualMat.palData[actualMat.texData[pixelCounter]];
                        pixelCounter++;
                    }
                    break;
                case 5:
                    convert_4x4texel_b(actualMat.texData, actualMat.width, actualMat.heigth, actualMat.spData, actualMat.palData, image);
                    break;
                case 6:
                    //if (actualMat.colorBase != 0)
                    //    actualMat.palData[0] = RGBA.Transparent;
                    pixelCounter = 0;
                    while (pixelCounter < pixelNum)
                    {
                        int index = actualMat.texData[pixelCounter] & 7;
                        int aplha = (actualMat.texData[pixelCounter] >> 3) & 0x1f;
                        aplha = ((aplha * 4) + (aplha / 2)) << 3;
                        image[pixelCounter] = actualMat.palData[index];
                        image[pixelCounter].A = (byte)aplha;
                        pixelCounter++; ;
                    }
                    break;

                case 7:
                    if (actualMat.colorBase != 0)
                        actualMat.palData[0] = RGBA.Transparent;
                    pixelCounter = 0;
                    pixelCounter = 0;
                    while (pixelCounter < pixelNum)
                    {
                        uint parameter = (ushort)(actualMat.texData[pixelCounter * 2] + (actualMat.texData[(pixelCounter * 2) + 1] << 8));
                        image[pixelCounter].R = (byte)((parameter & 0x1f) << 3);
                        image[pixelCounter].G = (byte)(((parameter >> 5) & 0x1f) << 3);
                        image[pixelCounter].B = (byte)(((parameter >> 10) & 0x1f) << 3);
                        image[pixelCounter].A = ((parameter & 0x8000) != 0) ? ((byte)0xff) : ((byte)0);
                        pixelCounter++;
                    }
                    break;
                default:
                    break;
            }
        }

        public void MakeTexture(NsbmdModel mod, List<NsbmdModel.MatTexPalStruct> matList, int[] polMatList)
        {
            if (matList == null)
                return;
            Console.WriteLine("DEBUG: making texture for model '{0}'...");
            for (int i = 0; i < matList.Count; i++)
                MakeTexture(mod, matList, i, false);
        }
        #endregion

        public NsbmdModel.ShapeInfoStruct process3DCommand(byte[] polydata, NsbmdModel.ShapeInfoStruct poly)
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
                int blockCounter = 0;
                float[] vtx_state = { 0.0f, 0.0f, 0.0f };
                float[] vtx_trans = { 0.0f, 0.0f, 0.0f };
                cur_vertex = gCurrentVertex; // for vertex_mode
                CurrentMatrix = MatrixStack[stackID].Clone();
                while (reader.BaseStream.Position < polyStream.Length)
                {
                    idCounter = initId(ref poly, polyStream, reader);
                    renderPackedCommand(ref poly, polyStream, reader, ref actualCommand, ref cur_vertex, ref idCounter, ref blockCounter, vtx_state, ref vtx_trans);
                }
                return poly;
            }
        }

        private void renderPackedCommand(ref NsbmdModel.ShapeInfoStruct poly, MemoryStream polyStream, BinaryReader reader, ref NsbmdModel.CommandStruct actualCommand, ref int cur_vertex, ref int idCounter, ref int blockCounter, float[] vtx_state, ref float[] vtx_trans)
        {
            for (idCounter = 0; idCounter < 4 && reader.BaseStream.Position < polyStream.Length; idCounter++)
            {
                switch (poly.commandList[blockCounter + idCounter].id)
                {
                    case 0: // No Operation (for padding packed GXFIFO commands)
                        break;
                    case 0x14:
                        reactionMatrixRestore(reader);
                        break;
                    case 0x1b:
                        reactionMatrixScale(ref poly, reader, ref actualCommand, idCounter, blockCounter);
                        break;
                    case 0x20: // Directly Set Vertex Color (W)
                        reactionColor(ref poly, reader, ref actualCommand, idCounter, blockCounter);
                        break;

                    case 0x21:
                        reactionNormal(ref poly, reader, ref actualCommand, idCounter, blockCounter);
                        break;
                    case 0x22:
                        reactionTextureCoordinate(ref poly, reader, ref actualCommand, idCounter, blockCounter);
                        break;
                    case 0x23:
                        reactionVtx_16(ref poly, reader, ref actualCommand, idCounter, blockCounter, vtx_state, ref vtx_trans);
                        break;
                    case 0x24:
                        reactionVtx_10(ref poly, reader, ref actualCommand, idCounter, blockCounter, vtx_state, ref vtx_trans);
                        break;
                    case 0x25:
                        reactionVtx_XY(ref poly, reader, ref actualCommand, idCounter, blockCounter, vtx_state, ref vtx_trans);
                        break;
                    case 0x26:
                        reactionVtx_XZ(ref poly, reader, ref actualCommand, idCounter, blockCounter, vtx_state, ref vtx_trans);
                        break;
                    case 0x27:
                        reactionVtx_YZ(ref poly, reader, ref actualCommand, idCounter, blockCounter, vtx_state, ref vtx_trans);
                        break;
                    case 0x28:
                        reactionVtx_Diff(ref poly, reader, ref actualCommand, idCounter, blockCounter, vtx_state, ref vtx_trans);
                        break;
                    case 0x40: // Start of Vertex List (W)
                        reactionBegin(ref poly, reader, ref actualCommand, idCounter, blockCounter);
                        break;
                    case 0x41: // End of Vertex List (W)
                        Gl.glEnd();
                        cur_vertex--;
                        break;
                    default:
                        break;
                }
                if (idCounter == 3)
                    blockCounter += 4;
            }
        }

        private static void reactionBegin(ref NsbmdModel.ShapeInfoStruct poly, BinaryReader reader, ref NsbmdModel.CommandStruct actualCommand, int idCounter, int blockCounter)
        {
            int mode;
            actualCommand.par = reader.ReadInt32();
            mode = actualCommand.par;
            actualCommand.id = 0x40;
            actualCommand.x = -1;
            switch (mode)
            {
                case 0:
                    mode = Gl.GL_TRIANGLES;
                    break;
                case 1:
                    mode = Gl.GL_QUADS;
                    break;
                case 2:
                    mode = Gl.GL_TRIANGLE_STRIP;
                    break;
                case 3:
                    mode = Gl.GL_QUAD_STRIP;
                    break;
            }

            Gl.glBegin(mode);
            poly.commandList[blockCounter + idCounter] = actualCommand;
        }

        private void reactionVtx_XY(ref NsbmdModel.ShapeInfoStruct poly, BinaryReader reader, ref NsbmdModel.CommandStruct actualCommand, int idCounter, int blockCounter, float[] vtx_state, ref float[] vtx_trans)
        {
            /*
                  VTX_XY - Set Vertex XY Coordinates (W)
                  Parameter 1, Bit 0-15   X-Coordinate (signed, with 12bit fractional part)
                  Parameter 1, Bit 16-31  Y-Coordinate (signed, with 12bit fractional part)
                */
            {
                actualCommand = poly.commandList[blockCounter + idCounter];

                actualCommand.par = reader.ReadInt32();

                actualCommand.x = (actualCommand.par >> 0) & 0xFFFF;
                if ((actualCommand.x & 0x8000) != 0) actualCommand.x |= -65536;
                actualCommand.y = (actualCommand.par >> 16) & 0xFFFF;
                if ((actualCommand.y & 0x8000) != 0) actualCommand.y |= -65536;

                xBuffer = actualCommand.x;
                yBuffer = actualCommand.y;
                actualCommand.z = zBuffer;

                vtx_state[0] = ((float)actualCommand.x) / SCALE_IV;
                vtx_state[1] = ((float)actualCommand.y) / SCALE_IV;

                Gl.glPushName((int)actualCommand.startParameterOffset);
                if (stackID != -1)
                {
                    vtx_trans = CurrentMatrix.MultVector(vtx_state);
                    Gl.glVertex3fv(vtx_trans);
                }
                else
                {
                    Gl.glVertex3fv(vtx_state);
                }
                poly.commandList[blockCounter + idCounter] = actualCommand;

            }
        }

        private void reactionVtx_XZ(ref NsbmdModel.ShapeInfoStruct poly, BinaryReader reader, ref NsbmdModel.CommandStruct actualCommand, int idCounter, int blockCounter, float[] vtx_state, ref float[] vtx_trans)
        {
            /*
                  VTX_XZ - Set Vertex XZ Coordinates (W)
                  Parameter 1, Bit 0-15   X-Coordinate (signed, with 12bit fractional part)
                  Parameter 1, Bit 16-31  Z-Coordinate (signed, with 12bit fractional part)
                */
            {
                actualCommand = poly.commandList[blockCounter + idCounter];

                actualCommand.par = reader.ReadInt32();

                actualCommand.x = (actualCommand.par >> 0) & 0xFFFF;
                if ((actualCommand.x & 0x8000) != 0) actualCommand.x |= -65536;
                actualCommand.z = (actualCommand.par >> 16) & 0xFFFF;
                if ((actualCommand.z & 0x8000) != 0) actualCommand.z |= -65536;

                xBuffer = actualCommand.x;
                actualCommand.y = yBuffer;
                zBuffer = actualCommand.z;

                vtx_state[0] = ((float)actualCommand.x) / SCALE_IV;
                vtx_state[2] = ((float)actualCommand.z) / SCALE_IV;

                Gl.glPushName((int)actualCommand.startParameterOffset);

                if (stackID != -1)
                {
                    vtx_trans = CurrentMatrix.MultVector(vtx_state);
                    Gl.glVertex3fv(vtx_trans);
                }
                else
                {
                    Gl.glVertex3fv(vtx_state);
                }
                poly.commandList[blockCounter + idCounter] = actualCommand;
            }
        }

        private void reactionVtx_YZ(ref NsbmdModel.ShapeInfoStruct poly, BinaryReader reader, ref NsbmdModel.CommandStruct actualCommand, int idCounter, int blockCounter, float[] vtx_state, ref float[] vtx_trans)
        {
            /*
                  VTX_YZ - Set Vertex YZ Coordinates (W)
                  Parameter 1, Bit 0-15   Y-Coordinate (signed, with 12bit fractional part)
                  Parameter 1, Bit 16-31  Z-Coordinate (signed, with 12bit fractional part)
                */
            {
                actualCommand = poly.commandList[blockCounter + idCounter];

                actualCommand.par = reader.ReadInt32();

                actualCommand.y = (actualCommand.par >> 0) & 0xFFFF;
                if ((actualCommand.y & 0x8000) != 0) actualCommand.y |= -65536;
                actualCommand.z = (actualCommand.par >> 16) & 0xFFFF;
                if ((actualCommand.z & 0x8000) != 0) actualCommand.z |= -65536;

                Gl.glPushName((int)actualCommand.startParameterOffset);

                zBuffer = actualCommand.z;
                yBuffer = actualCommand.y;
                actualCommand.x = xBuffer;

                vtx_state[1] = ((float)actualCommand.y) / SCALE_IV;
                vtx_state[2] = ((float)actualCommand.z) / SCALE_IV;

                if (stackID != -1)
                {
                    vtx_trans = CurrentMatrix.MultVector(vtx_state);
                    Gl.glVertex3fv(vtx_trans);
                }
                else
                {
                    Gl.glVertex3fv(vtx_state);
                }
                poly.commandList[blockCounter + idCounter] = actualCommand;
            }
        }

        private void reactionVtx_Diff(ref NsbmdModel.ShapeInfoStruct poly, BinaryReader reader, ref NsbmdModel.CommandStruct actualCommand, int idCounter, int blockCounter, float[] vtx_state, ref float[] vtx_trans)
        {
            /*
                  VTX_DIFF - Set Relative Vertex Coordinates (W)
                  Parameter 1, Bit 0-9    X-Difference (signed, with 9bit fractional part)
                  Parameter 1, Bit 10-19  Y-Difference (signed, with 9bit fractional part)
                  Parameter 1, Bit 20-29  Z-Difference (signed, with 9bit fractional part)
                  Parameter 1, Bit 30-31  Not used
                */
            {
                actualCommand.id = 0x28;
                actualCommand.par = reader.ReadInt32();

                actualCommand.x = (actualCommand.par >> 0) & 0x3FF;
                if ((actualCommand.x & 0x200) != 0) actualCommand.x |= -1024;
                actualCommand.y = (actualCommand.par >> 10) & 0x3FF;
                if ((actualCommand.y & 0x200) != 0) actualCommand.y |= -1024;
                actualCommand.z = (actualCommand.par >> 20) & 0x3FF;
                if ((actualCommand.z & 0x200) != 0) actualCommand.z |= -1024;

                xBuffer = actualCommand.x;
                yBuffer = actualCommand.y;
                zBuffer = actualCommand.z;

                vtx_state[0] += ((float)actualCommand.x) / SCALE_IV;
                vtx_state[1] += ((float)actualCommand.y) / SCALE_IV;
                vtx_state[2] += ((float)actualCommand.z) / SCALE_IV;

                Gl.glPushName((int)actualCommand.startParameterOffset);

                if (stackID != -1)
                {
                    vtx_trans = CurrentMatrix.MultVector(vtx_state);
                    Gl.glVertex3fv(vtx_trans);
                }
                else
                {
                    Gl.glVertex3fv(vtx_state);
                }
                poly.commandList[blockCounter + idCounter] = actualCommand;
            }
        }

        private void reactionVtx_10(ref NsbmdModel.ShapeInfoStruct poly, BinaryReader reader, ref NsbmdModel.CommandStruct actualCommand, int idCounter, int blockCounter, float[] vtx_state, ref float[] vtx_trans)
        {
            /*
                  VTX_10 - Set Vertex XYZ Coordinates (W)
                  Parameter 1, Bit 0-9    X-Coordinate (signed, with 6bit fractional part)
                  Parameter 1, Bit 10-19  Y-Coordinate (signed, with 6bit fractional part)
                  Parameter 1, Bit 20-29  Z-Coordinate (signed, with 6bit fractional part)
                  Parameter 1, Bit 30-31  Not used
                */
            {
                actualCommand = poly.commandList[blockCounter + idCounter];

                actualCommand.par = reader.ReadInt32();

                actualCommand.x = (actualCommand.par >> 0) & 0x3FF;
                if ((actualCommand.x & 0x200) != 0) actualCommand.x |= -1024;
                actualCommand.y = (actualCommand.par >> 10) & 0x3FF;
                if ((actualCommand.y & 0x200) != 0) actualCommand.y |= -1024;
                actualCommand.z = (actualCommand.par >> 20) & 0x3FF;
                if ((actualCommand.z & 0x200) != 0) actualCommand.z |= -1024;

                xBuffer = actualCommand.x;
                yBuffer = actualCommand.y;
                zBuffer = actualCommand.z;

                vtx_state[0] = ((float)actualCommand.x) / 64.0f;
                vtx_state[1] = ((float)actualCommand.y) / 64.0f;
                vtx_state[2] = ((float)actualCommand.z) / 64.0f;
                Gl.glPushName((int)actualCommand.startParameterOffset);
                if (stackID != -1)
                {
                    vtx_trans = CurrentMatrix.MultVector(vtx_state);
                    Gl.glVertex3fv(vtx_trans);
                }
                else
                {
                    Gl.glVertex3fv(vtx_state);
                }
                poly.commandList[blockCounter + idCounter] = actualCommand;

            }
        }

        private void reactionVtx_16(ref NsbmdModel.ShapeInfoStruct poly, BinaryReader reader, ref NsbmdModel.CommandStruct actualCommand, int idCounter, int blockCounter, float[] vtx_state, ref float[] vtx_trans)
        {
            /*
                  VTX_16 - Set Vertex XYZ Coordinates (W)
                  Parameter 1, Bit 0-15   X-Coordinate (signed, with 12bit fractional part)
                  Parameter 1, Bit 16-31  Y-Coordinate (signed, with 12bit fractional part)
                  Parameter 2, Bit 0-15   Z-Coordinate (signed, with 12bit fractional part)
                  Parameter 2, Bit 16-31  Not used
                */
            {
                actualCommand = poly.commandList[blockCounter + idCounter];

                actualCommand.par = reader.ReadInt32();

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

                vtx_state[0] = ((float)actualCommand.x) / SCALE_IV;
                vtx_state[1] = ((float)actualCommand.y) / SCALE_IV;
                vtx_state[2] = ((float)actualCommand.z) / SCALE_IV;
                Gl.glPushName((int)actualCommand.startParameterOffset);
                if (stackID != -1)
                {
                    vtx_trans = CurrentMatrix.MultVector(vtx_state);
                    Gl.glVertex3fv(vtx_trans);
                }
                else
                {
                    Gl.glVertex3fv(vtx_state);
                }
                poly.commandList[blockCounter + idCounter] = actualCommand;

            }
        }

        private void reactionTextureCoordinate(ref NsbmdModel.ShapeInfoStruct poly, BinaryReader reader, ref NsbmdModel.CommandStruct actualCommand, int idCounter, int blockCounter)
        {
            /*
                  Set Texture Coordinates (W)
                  Parameter 1, Bit 0-15   S-Coordinate (X-Coordinate in Texture Source)
                  Parameter 1, Bit 16-31  T-Coordinate (Y-Coordinate in Texture Source)
                  Both values are 1bit sign + 11bit integer + 4bit fractional part.
                  A value of 1.0 (=1 SHL 4) equals to one Texel.
                */
            {
                actualCommand = poly.commandList[blockCounter + idCounter];
                actualCommand.par = reader.ReadInt32();
                actualCommand.x = (actualCommand.par >> 0) & 0xffff;
                if ((actualCommand.x & 0x8000) != 0) actualCommand.x |= -65536;
                actualCommand.y = (actualCommand.par >> 16) & 0xffff;
                if ((actualCommand.y & 0x8000) != 0) actualCommand.y |= -65536;
                Gl.glPushName((int)actualCommand.startParameterOffset);
                Gl.glTexCoord2f(((float)actualCommand.x) / 16.0f, ((float)actualCommand.y) / 16.0f);
                poly.commandList[blockCounter + idCounter] = actualCommand;

            }
        }

        private void reactionNormal(ref NsbmdModel.ShapeInfoStruct poly, BinaryReader reader, ref NsbmdModel.CommandStruct actualCommand, int idCounter, int blockCounter)
        {
            /*
                  Set Normal Vector (W)
                  0-9   X-Component of Normal Vector (1bit sign + 9bit fractional part)
                  10-19 Y-Component of Normal Vector (1bit sign + 9bit fractional part)
                  20-29 Z-Component of Normal Vector (1bit sign + 9bit fractional part)
                  30-31 Not used
                */
            actualCommand = poly.commandList[blockCounter + idCounter];
            actualCommand.par = reader.ReadInt32();
            actualCommand.x = (actualCommand.par >> 0) & 0x3FF;
            if ((actualCommand.x & 0x200) != 0)
                actualCommand.x |= -1024;
            actualCommand.y = (actualCommand.par >> 10) & 0x3FF;
            if ((actualCommand.y & 0x200) != 0) actualCommand.y |= -1024;
            actualCommand.z = (actualCommand.par >> 20) & 0x3FF;
            if ((actualCommand.z & 0x200) != 0) actualCommand.z |= -1024;
            Gl.glPushName((int)actualCommand.startParameterOffset);
            Gl.glNormal3f(((float)actualCommand.x) / 512.0f, ((float)actualCommand.y) / 512.0f, ((float)actualCommand.z) / 512.0f);
            Gl.glPopName();
            poly.commandList[blockCounter + idCounter] = actualCommand;
        }

        private void reactionColor(ref NsbmdModel.ShapeInfoStruct poly, BinaryReader reader, ref NsbmdModel.CommandStruct actualCommand, int idCounter, int blockCounter)
        {
            actualCommand = poly.commandList[blockCounter + idCounter];
            actualCommand.par = reader.ReadInt32();

            actualCommand.x = (actualCommand.par >> 0) & 0x1F;
            actualCommand.y = (actualCommand.par >> 5) & 0x1F;
            actualCommand.z = (actualCommand.par >> 10) & 0x1F;
            Gl.glColor3f(((float)actualCommand.x) / 31.0f, ((float)actualCommand.y) / 31.0f, ((float)actualCommand.z) / 31.0f);
            Gl.glColor3f(1, 1, 1);
            poly.commandList[blockCounter + idCounter] = actualCommand;
        }

        private void reactionMatrixScale(ref NsbmdModel.ShapeInfoStruct poly, BinaryReader reader, ref NsbmdModel.CommandStruct actualCommand, int idCounter, int blockCounter)
        {
            /*
                  MTX_SCALE - Multiply Current Matrix by Scale Matrix (W)
                  Sets C=M*C. Parameters: 3, m[0..2] (MTX_SCALE doesn't change Vector Matrix)
                */
            actualCommand = poly.commandList[blockCounter + idCounter];
            actualCommand.x = reader.ReadInt32();
            actualCommand.y = reader.ReadInt32();
            actualCommand.z = reader.ReadInt32();
            CurrentMatrix.Scale(actualCommand.x / SCALE_IV, actualCommand.y / SCALE_IV, actualCommand.z / SCALE_IV);
            poly.commandList[blockCounter + idCounter] = actualCommand;
        }

        private void reactionMatrixRestore(BinaryReader reader)
        {
            /*
                  MTX_RESTORE - Restore Current Matrix from Stack (W)
                  Sets C=[N]. The stack pointer S is not used, and is left unchanged.
                  Parameter Bit0-4:  Stack Address (0..30) (31 causes overflow in GXSTAT.15)
                  Parameter Bit5-31: Not used
                */

            stackID = reader.ReadInt32() & 0x0000001F;
            CurrentMatrix = MatrixStack[stackID].Clone();
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



        public NsbmdModel renderSinglePolygon(int idActualPol, int isTextured, int idActualMaterial)
        {
            if (wrongModel())
                return null;
            if (wrongPolygon(idActualPol))
            {
                mapEditor.Console.AppendText("Wrong id Polygon: " + idActualPol);
                return null;
            }
            if (wrongMaterial(idActualMaterial))
            {
                mapEditor.Console.AppendText("Wrong id Material: " + idActualMaterial);
                return null;
            }
            var actualPolygon = modelList[idModel].getMDL0at(0).shapeInfo.shapeList[idActualPol];
            var actualMaterials = modelList[idModel].getMaterials()[idActualMaterial];
            var actualMatInfo = modelList[idModel].getMDL0at(0).material.matInfoList[idActualMaterial];

            for (int num_obj = 0; num_obj < modelList[idModel].getMDL0at(0).nodeInfoList.Count; num_obj++)
                processObjects(idActualPol, isTextured, ref actualPolygon, ref actualMaterials, num_obj, idActualMaterial);
            return modelList[idModel].getMDL0at(0);
        }

        private void processObjects(int idActualPol, int isTextured, ref NsbmdModel.ShapeInfoStruct actualPolygon, ref NsbmdModel.MatTexPalStruct actualMaterials, int num_obj, int idActualMateria)
        {
            NsbmdModel.NodeInfoStruct actualNodeInfo = modelList[idModel].getMDL0at(0).nodeInfoList[num_obj];
         
            if (actualNodeInfo.restoreId != -1)
                Gl.glLoadMatrixf(MatrixStack[actualNodeInfo.restoreId].Floats);
            if (actualNodeInfo.stackId != -1)
            {
                Gl.glGetFloatv(Gl.GL_MODELVIEW_MATRIX, MatrixStack[actualNodeInfo.stackId].Floats);
                stackID = actualNodeInfo.stackId;
            }

            Gl.glLoadIdentity();

            actualPolygon = modelList[idModel].getMDL0at(0).shapeInfo.shapeList[idActualPol];
            var actualMaterial = modelList[idModel].getMaterials()[idActualMaterial];

            if (isTextured == 0)
                Gl.glPolygonMode(0x408, 0x1b01);
            else
                Gl.glPolygonMode(0x408, 0x1b02);

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, idActualMaterial +  1);
            Gl.glMatrixMode(Gl.GL_TEXTURE);
            Gl.glLoadIdentity();
            if ((actualMaterials.flipS == 1) && (actualMaterials.repeatS > 0))
                Gl.glScalef(2f / ((float)actualMaterials.width), 1f / ((float)actualMaterials.heigth), 1f);
            else if ((actualMaterials.flipT == 1) && (actualMaterials.repeatT > 0))
                Gl.glScalef(1f / ((float)actualMaterials.width), 2f / ((float)actualMaterials.heigth), 1f);
            else
                Gl.glScalef(1f / ((float)actualMaterials.width), 1f / ((float)actualMaterials.heigth), 1f);

            Gl.glColor3f(1f, 1f, 1f);
            stackID = actualPolygon.stackId;
            modelList[idModel].getMDL0at(0).shapeInfo.shapeList[idActualPol] = process3DCommand(actualPolygon.polygonData, actualPolygon);
        }

        private bool wrongMaterial(int idActualMaterial)
        {
            return idActualMaterial > modelList[idModel].getMaterials().Count;
        }

        private bool wrongPolygon(int idActualPol)
        {
            return idActualPol > modelList[idModel].getMDL0at(0).shapeInfo.shapeList.Count;
        }

        private bool wrongModel()
        {
            return idModel > modelList.Count || modelList.Count <= 0;
        }

        public NsbmdModel renderAllPolygon(int idActualPoly, int isTextured, int[] idMaterialArray)
        {
            
            if (modelList.Count == 0)
                return null;
            for (int num_obj = 0; num_obj < modelList[idModel].getMDL0at(0).nodeInfoList.Count; num_obj++)
            {
                NsbmdModel.NodeInfoStruct actualNodeInfo = modelList[0].getMDL0at(0).nodeInfoList[num_obj];
                if (actualNodeInfo.restoreId != -1)
                    Gl.glLoadMatrixf(MatrixStack[0].Floats);
                Gl.glGetFloatv(Gl.GL_MODELVIEW_MATRIX, MatrixStack[0].Floats);
                stackID = 0;
                Gl.glLoadIdentity();
                for (int polyCounter = 0; polyCounter <= idActualPoly; polyCounter++)
                    renderPolygon(isTextured, idMaterialArray, polyCounter);
            }
            return modelList[idModel].getMDL0at(0);
        }

        private void renderPolygon(int isTextured, int[] idMaterialArray, int polyCounter)
        {
            var actualPolygon = modelList[idModel].getMDL0at(0).shapeInfo.shapeList[polyCounter];
            var matId = idMaterialArray[polyCounter];
            var actualMaterial = modelList[idModel].getMaterials()[matId];

            if (isTextured == 0)
                Gl.glPolygonMode(0x408, 0x1b01);
            else
                Gl.glPolygonMode(0x408, 0x1b02);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, (int)(matId + 1));
            Gl.glMatrixMode(0x1702);
            Gl.glLoadIdentity();
            if ((actualMaterial.flipS == 1) && (actualMaterial.repeatS > 0))
                Gl.glScalef(2f / ((float)actualMaterial.width), 1f / ((float)actualMaterial.heigth), 1f);
            if ((actualMaterial.flipT == 1) && (actualMaterial.repeatT > 0))
                Gl.glScalef(1f / ((float)actualMaterial.width), 2f / ((float)actualMaterial.heigth), 1f);
            else
                Gl.glScalef(1f / ((float)actualMaterial.width), 1f / ((float)actualMaterial.heigth), 1f);

            Gl.glColor3f(1f, 1f, 1f);
            stackID = actualPolygon.stackId;
            modelList[idModel].getMDL0at(0).shapeInfo.shapeList[polyCounter] = process3DCommand(actualPolygon.polygonData, actualPolygon);
        }

        public void renderSingularAction()
        {
            prepareRenderScene();
            renderSinglePolygon(polyval, text_on, polMatList[polyval]);

        }

        public void renderMultipleAction()
        {
            prepareRenderScene();
            renderAllPolygon(polyval, text_on, polMatList);

        }

        private void prepareRenderScene()
        {
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glAlphaFunc(Gl.GL_GREATER, 0.0f);
            Gl.glEnable(Gl.GL_ALPHA_TEST);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glColor3f(1f, 1f, 1f);
            Gl.glVertex3f(0f, 0f, 0f);
            Gl.glVertex3f(1f, 0f, 0f);
            Gl.glVertex3f(0f, 0f, 0f);
            Gl.glVertex3f(0f, 1f, 0f);
            Gl.glVertex3f(0f, 0f, 0f);
            Gl.glVertex3f(0f, 0f, 1f);
            Gl.glEnd();

            Gl.glRasterPos3f(1f, 0f, 0f);
            Gl.glRasterPos3f(0f, 1f, 0f);
            Gl.glRasterPos3f(0f, 0f, 1f);

            transX = -(((float)mapEditor.trackBarTransX.Value) / 10f);
            transY = -(((float)mapEditor.trackBarTransY.Value) / 10f);
            transZ = -(((float)mapEditor.trackBarTransZ.Value) / 10f);
            rotY = (((float)mapEditor.trackBarRotX.Value) / 10f);
            rotX = -(((float)mapEditor.trackBarRotY.Value) / 10f);
            rotZ = -(((float)mapEditor.trackBarRotZ.Value) / 10f);
            Gl.glTranslatef(transX, 0f, 0f);
            Gl.glTranslatef(0f, transY, 0f);
            Gl.glTranslatef(0f, 0f, transZ);
            Gl.glRotatef(rotX, 1f, 0f, 0f);
            Gl.glRotatef(rotY, 0f, 1f, 0f);
            Gl.glRotatef(rotZ, 0f, 0f, 1f);
            Gl.glClearColor(0f, 0f, 1f, 1f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
        }

        public Nsbmd getModel(int modelId)
        {
            return modelList[modelId];
        }

        public void setModel(Nsbmd model, Boolean mode)
        {
            modelList.Add(model);
            //if (model.actualTex != null)
            //{

            if (polyval > 1)
            {
                if (mode)
                {
                    //MapEditor.Console.AppendText("\nStart texturing(Singular mode)");
                    MakeTexture(model.getMDL0at(0), model.getMaterials(), idActualMaterial, true);
                }
                else
                {
                    //MapEditor.Console.AppendText("\nStart texturing(Normal map)");
                    MakeTexture(model.getMDL0at(0), model.getMaterials(), polMatList);
                }
            }
            else
                if (polyval == 0)
                {
                    //MapEditor.Console.AppendText("\nStart texturing (One polygon map)");
                    MakeTexture(model.getMDL0at(0), model.getMaterials(), 0, true);
                }
        }
    }
}

