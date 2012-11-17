namespace PG4Map.Formats
{
    using System;
    using System.IO;

    public class PkmnMapDemuxer
    {
        public int _gen;
        public BinaryReader _reader;
        public PkmnMapHeader map;

        #region Constants
        public const short GENERAL_DIRECTORY = 0;
        public const short NARC_FILE = 1;
        public const short AB_FILE = 2;
        public const short DPPMAP = 0;
        public const short HGSSMAP = 1;
        public const short BWMAP = 2;
        public const short BW2MAP = 5;
        public const short NSBMD_MODEL = 3;
        public const short PLMAP = 4;
        public const short MAXMOVSIZE = 32;
        #endregion

        public PkmnMapDemuxer(BinaryReader reader, int is4or5gen)
        {
              _reader = reader;
              _gen = is4or5gen;
        }

        public byte[] DemuxBdhcBytes(PkmnMapHeader map)
        {
              _stream.Position = map.BdhcOffset;
            return   _reader.ReadBytes((int) map.BDHCSize);
        }

        public byte[] DemuxBMDBytes(PkmnMapHeader map, int type)
        {
              _stream.Position = 0L;
            if (  _stream.GetRemainingLength() < 0x18L)
            {
                throw new Exception("File too short to contain NSBMD!");
            }
            if (type == DPPMAP || type == PLMAP)
            {
                  _stream.Position = map.BMDOffset;
            }
            else
            {
                  _stream.Position = 4L;
                map.BMDSize = (uint)   _reader.ReadInt32();
                  _stream.Position = 0L;
            }
            if (  _stream.GetRemainingLength() < 0x10L)
            {
                throw new Exception("File too short to contain NSBMD!");
            }
            int num = 0;
            for (int i = 0; i < 4; i++)
            {
                num =   _reader.ReadInt32();
                if (num == 0x30444d42)
                {
                    break;
                }
            }
            if (num != 0x30444d42)
            {
                throw new InvalidDataException("No BMD0 Header at expected offset!");
            }
            Stream stream1 =   _stream;
            stream1.Position -= 4L;
            return   _reader.ReadBytes((int) map.BMDSize);
        }

        public byte[] DemuxMovBytes(PkmnMapHeader map)
        {
              _stream.Position = map.MovOffset;
            return   _reader.ReadBytes((int) map.MovSize);
        }

        public byte[] DemuxObjBytes(PkmnMapHeader map)
        {
              _stream.Position = map.ObjOffset;
            return   _reader.ReadBytes((int) map.ObjSize);
        }

        public Stream _stream
        {
            get
            {
                return   _reader.BaseStream;
            }
        }
    }
}

