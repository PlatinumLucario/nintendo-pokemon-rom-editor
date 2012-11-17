namespace PG4Map.Formats
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct RGBA
    {
        public byte A;
        public byte R;
        public byte G;
        public byte B;
        public static RGBA Transparent;
        public byte this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return this.R;

                    case 1:
                        return this.G;

                    case 2:
                        return this.B;

                    case 3:
                        return this.A;
                }
                throw new Exception();
            }
            set
            {
                switch (i)
                {
                    case 0:
                        this.R = value;
                        break;

                    case 1:
                        this.G = value;
                        break;

                    case 2:
                        this.B = value;
                        break;

                    case 3:
                        this.A = value;
                        break;

                    default:
                        throw new Exception();
                }
            }
        }
        static RGBA()
        {
            RGBA rgba = new RGBA {
                R = 0xff,
                A = 0
            };
            Transparent = rgba;
        }
    }
}

