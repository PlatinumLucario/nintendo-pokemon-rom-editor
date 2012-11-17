namespace PG4Map.Formats
{
    using System;
    using System.IO;

    public class PkmnMapDemuxer_Bw
    {
        public int _gen;
        public BinaryReader _reader;
        public PkmnMapHeader_Bw map;

        public PkmnMapDemuxer_Bw(BinaryReader reader, int is4or5gen)
        {
              _reader = reader;
              _gen = is4or5gen;
        }

        public byte[] DemuxBMDBytes(PkmnMapHeader_Bw map, int type)
        {
            if (  _stream.GetRemainingLength() < 0x18L)
            {
                throw new Exception("File too short to contain NSBMD!");
            }
              _stream.Position = map.BMDOffset;
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
            return   _reader.ReadBytes((int) map.BMDOSize);
        }

        public byte[] DemuxMovBytes(PkmnMapHeader_Bw map)
        {
              _stream.Position = map.MovOffset;
            return   _reader.ReadBytes((int) map.MovSize);
        }

        public byte[] DemuxObjBytes(PkmnMapHeader_Bw map)
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

