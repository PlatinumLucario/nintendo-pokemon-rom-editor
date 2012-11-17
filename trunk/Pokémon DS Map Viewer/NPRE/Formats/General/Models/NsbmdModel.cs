namespace PG4Map.Formats
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class NsbmdModel
    {
        public int blockSize;
        public Nsbtx texture;
        public List<MatTexPalStruct> matTexPalList;
        public MaterialSectionStruct material;
        public DictModelStruct model;
        public ModelInfoStruct modelInfo;
        public DictNodeStruct node;
        public List<NodeInfoStruct> nodeInfoList;
        public ArrayList sequenceBoneCommandList;
        public ShapeSectionStruct shapeInfo;
        public int dlSize;

        public MatTexPalStruct CopyTo(MatTexPalStruct mat1, MatTexPalStruct other)
        {
            other.texName = mat1.texName;
            other.texOffset = mat1.texOffset;
            other.texSize = mat1.texSize;
            other.format = mat1.format;
            other.colorBase = mat1.colorBase;
            other.width = mat1.width;
            other.heigth = mat1.heigth;
            other.texData = mat1.texData;
            other.spData = mat1.spData;
            return other;
        }

        public string Name2 { get; set; }


        public struct CommandStruct
        {
            public int id;
            public Int32 x;
            public Int32 y;
            public Int32 z;
            public Int32 par;
            public Int32 par2;
            public int par3;
            public int par4;
            public long startParameterOffset;
            public long startIdOffset;
        }

        public struct DictMatStruct
        {
            public int revisionId;
            public int matNum;
            public int size;
            public int pad;
            public int matDataSize;
            public int magic;
            public ArrayList matDataList;
            public List<NsbmdModel.DictNameStruct> matNameList;
            public int unitSize;
            public int matDefinitionOffsetSize;
            public ArrayList matDefinitionOffsetList;
        }

        public struct DictModelStruct
        {
            public int revisionId;
            public int modelNum;
            public int size;
            public int pad;
            public int modelDataSize;
            public int magic;
            public ArrayList modelDataList;
            public List<NsbmdModel.DictNameStruct> modelNameList;
            public int unitSize;
            public int modelDefinitionOffsetSize;
            public ArrayList modelDefinitionOffsetList;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DictNameStruct
        {
            public string name;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DictNodeStruct
        {
            public int revision;
            public int nodeNum;
            public int size;
            public int pad;
            public int nodeDataSize;
            public int magic;
            public ArrayList nodeDataList;
            public List<NsbmdModel.DictNameStruct> nodeNameList;
            public int unitSize;
            public int nodeDefinitionOffsetSize;
            public ArrayList nodeDefinitionOffsetList;
        }

        public struct DictPalStruct
        {
            public int revisionId;
            public int palNum;
            public int size;
            public int pad;
            public int palDataSize;
            public int magic;
            public ArrayList palDataList;
            public List<NsbmdModel.DictNameStruct> palNameList;
            public int unitSize;
            public int palDefinitionOffsetSize;
            public ArrayList palDefinitionOffsetList;
            public ArrayList palMaterialIdList;
        }

        public struct DictShapeStruct
        {
            public int revisionId;
            public int shapeNum;
            public int size;
            public int pad;
            public int shapeDataSize;
            public int magic;
            public ArrayList shapeDataList;
            public List<NsbmdModel.DictNameStruct> shapeNameList;
            public int unitSize;
            public int shapeDefinitionOffsetSize;
            public ArrayList shapeDefinitionOffsetList;
            public ArrayList shapeMaterialIdList;
        }
        public struct DictTexStruct
        {
            public int revisionId;
            public int texNum;
            public int size;
            public int pad;
            public int texDataSize;
            public int magic;
            public List<texDataStruct> texDataList;
            public List<DictNameStruct> texNameList;
            public int unitSize;
            public int texDefinitionOffsetSize;
            public ArrayList texDefinitionOffsetList;
            public ArrayList texMaterialIdList;
        }

        public struct MaterialSectionStruct
        {
            public uint dictTexOffset;
            public uint dictPalOffset;
            public DictMatStruct dictMat;
            public DictTexStruct dictTex;
            public DictPalStruct dictPal;
            public ArrayList texIndexDataList;
            public ArrayList palIndexDataList;
            public List<MatInfoStruct> matInfoList;
            public int pad;
            public int matLength;
        }
        public struct MatTexPalStruct
        {
            public uint texOffset;
            public uint texDataOffset;
            public string texName;
            public string matName;
            public uint texSize;
            public uint palOffset;
            public string palName;
            public uint palSize;
            public byte[] texData;
            public RGBA[] palData;
            public uint textMatId;
            public uint palMatId;
            public byte[] spData;
            public byte repeat;
            public int parameter;
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

        [StructLayout(LayoutKind.Sequential)]
        public struct MatInfoStruct
        {
            public int itemTag;
            public int size;
            public int diffAmb;
            public int specEmi;
            public int polyAttr;
            public int polyAttrMask;
            public int texImageParam;
            public int texImageParamMask;
            public int texPlttBase;
            public int flag;
            public int origWidth;
            public int origHeight;
            public int magW;
            public int magH;
            public int scaleS;
            public int scaleT;
            public int rotSin;
            public int rotCos;
            public int transS;
            public int transT;
            public int width;
            public int heigth;
            public double[] effectMtx;
            public int unkBuffer;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ModelInfoStruct
        {
            public uint matOffset;
            public uint sbcOffset;
            public uint bdhcOffset;
            public uint size;
            public uint shapeOffset;
            public int sbcType;
            public int scalingRule;
            public int textMatrixMode;
            public int nodeNum;
            public int matNum;
            public int shapeNum;
            public int matUns;
            public int pad;
            public int scalePosition;
            public int scalePositionInverse;
            public int vertexNum;
            public int polygonNum;
            public int triangleNum;
            public int quadsNum;
            public double boundingBoxX;
            public double boundingBoxY;
            public double boundingBoxZ;
            public double boundingBoxHeigth;
            public double boundingBoxWidth;
            public double boundingBoxDepth;
            public double boundingBoxScalePosition;
            public double boundingBoxScalePositionInverse;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NodeInfoStruct
        {
            public const float FACTOR1 = 1f;
            public uint flag;
            public bool translation;
            public bool isRotated;
            public uint pivot;
            public uint x;
            public uint y;
            public uint z;
            public uint unk;
            public uint unk2;
            public uint Unk3;
            public uint Unk4;
            public uint Unk6;
            public uint Unk5;
            public int a;
            public int b;
            public int rotA;
            public int rotB;
            public uint negation;
            public int parentId;
            public int stackId;
            public int restoreId;
            public int parentStackId;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ShapeInfoStruct
        {
            public int itemTag;
            public int size;
            public int flag;
            public uint offsetDL;
            public int sizeDL;
            public int materialId;
            public int stackId;
            public byte[] polygonData;
            public List<CommandStruct> commandList;
            public long startShapeInfo;
            public long endShapeInfo;
        }

        public struct ShapeSectionStruct
        {
            public DictShapeStruct dictShp;
            public List<ShapeInfoStruct> shapeList;
            public int[] dl;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct texDataStruct
        {
            public int id;
            public int par;
            public int par2;
            public int par3;
            public int format;
            public int colorBase;
            public int width;
            public int height;
        }
    }
}

