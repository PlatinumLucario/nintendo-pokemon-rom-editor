namespace PG4Map.Formats
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct PkmnMapHeader_Bw
    {
        public uint BMDOffset { get; set; }
        public uint MovOffset { get; set; }
        public uint ObjOffset { get; set; }
        public uint BdhcOffset { get; set; }
        public uint BMDOSize
        {
            get
            {
                return (  MovOffset -   BMDOffset);
            }
            set
            {
            }
        }
        public uint MovSize
        {
            get
            {
                return (  ObjOffset -   MovOffset);
            }
            set
            {
            }
        }
        public uint ObjSize
        {
            get
            {
                return (  BdhcOffset -   ObjOffset);
            }
            set
            {
            }
        }
        public static PkmnMapHeader_Bw FromReader(BinaryReader reader)
        {
            PkmnMapHeader_Bw bw = new PkmnMapHeader_Bw();
            reader.BaseStream.Seek(2L, SeekOrigin.Current);
            byte num = reader.ReadByte();
            reader.ReadByte();
            switch (num)
            {
                case 3:
                    bw.BMDOffset = reader.ReadUInt32();
                    bw.MovOffset = reader.ReadUInt32();
                    bw.ObjOffset = reader.ReadUInt32();
                    bw.BdhcOffset = reader.ReadUInt32();
                    return bw;

                case 2:
                    bw.BMDOffset = reader.ReadUInt32();
                    bw.MovOffset = reader.ReadUInt32();
                    bw.ObjOffset = reader.ReadUInt32();
                    bw.BdhcOffset = bw.ObjOffset;
                    return bw;

                case 1:
                    bw.BMDOffset = reader.ReadUInt32();
                    bw.MovOffset = reader.ReadUInt32();
                    bw.ObjOffset = bw.MovOffset;
                    bw.BdhcOffset = bw.MovOffset;
                    break;
                default:
                    bw.BMDOffset = reader.ReadUInt32();
                    bw.MovOffset = reader.ReadUInt32();
                    bw.ObjOffset = reader.ReadUInt32();
                    bw.BdhcOffset = reader.ReadUInt32();
                    return bw;
            }
            return bw;
        }
    }
}

