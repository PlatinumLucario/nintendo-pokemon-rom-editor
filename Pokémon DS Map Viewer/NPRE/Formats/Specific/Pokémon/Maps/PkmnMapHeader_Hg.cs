namespace PG4Map.Formats
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct PkmnMapHeader_Hg
    {
        public uint BMDOffset
        {
            get
            {
                return ((((this.MovSize + this.ObjSize) + this.S0Size) + 0x10) + 4);
            }
            set
            {
            }
        }
        public uint MovOffset
        {
            get
            {
                return (20 + this.S0Size);
            }
            set
            {
            }
        }
        public uint ObjOffset
        {
            get
            {
                return (((this.S0Size + this.MovSize) + 0x10) + 4);
            }
            set
            {
            }
        }
        public uint BdhcOffset
        {
            get
            {
                return (((((this.S0Size + this.MovSize) + this.ObjSize) + this.BMDSize) + 0x10) + 4);
            }
            set
            {
            }
        }
        public uint S0Offset
        {
            get
            {
                return 0x10;
            }
            set
            {
            }
        }
        public uint S0Size { get; set; }
        public uint BMDSize { get; set; }
        public uint MovSize { get; set; }
        public uint ObjSize { get; set; }
        public uint BDHCSize { get; set; }
        public static PkmnMapHeader_Hg FromReader(BinaryReader reader)
        {
            PkmnMapHeader_Hg hg = new PkmnMapHeader_Hg {
                MovSize = reader.ReadUInt32(),
                ObjSize = reader.ReadUInt32(),
                BMDSize = reader.ReadUInt32(),
                BDHCSize = reader.ReadUInt32()
            };
            reader.BaseStream.Seek(2L, SeekOrigin.Current);
            hg.S0Size = reader.ReadUInt16();
            return hg;
        }
    }
}

