namespace PG4Map.Formats
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct PkmnMapHeader
    {
        public uint BMDOffset
        {
            get
            {
                return ((this.MovSize + this.ObjSize) + 0x10);
            }
            set
            {
            }
        }
        public uint MovOffset
        {
            get
            {
                return 0x10;
            }
            set
            {
            }
        }
        public uint ObjOffset
        {
            get
            {
                return (this.MovSize + 0x10);
            }
            set
            {
            }
        }
        public uint BdhcOffset
        {
            get
            {
                return (((this.MovSize + this.ObjSize) + this.BMDSize) + 0x10);
            }
            set
            {
            }
        }
        public uint BMDSize { get; set; }
        public uint MovSize { get; set; }
        public uint ObjSize { get; set; }
        public uint BDHCSize { get; set; }
        public static PkmnMapHeader FromReader(BinaryReader reader)
        {
            return new PkmnMapHeader { MovSize = reader.ReadUInt32(), ObjSize = reader.ReadUInt32(), BMDSize = reader.ReadUInt32(), BDHCSize = reader.ReadUInt32() };
        }
    }
}

