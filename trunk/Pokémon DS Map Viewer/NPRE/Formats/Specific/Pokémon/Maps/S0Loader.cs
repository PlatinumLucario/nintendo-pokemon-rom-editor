namespace PG4Map
{
    using System;
    using System.IO;

    internal class S0Loader
    {
        public static short[] LoadS0(Stream stream)
        {
            short[] array = new short[0];
            BinaryReader reader = new BinaryReader(stream);
            reader.BaseStream.Seek(0L, SeekOrigin.Begin);
            int index = 0;
            for (index = 0; index < (stream.Length / 4L); index++)
            {
                Array.Resize<short>(ref array, index + 1);
                array[index] = reader.ReadInt16();
            }
            return array;
        }
    }
}

