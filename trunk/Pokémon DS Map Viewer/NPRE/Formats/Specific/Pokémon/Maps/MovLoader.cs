namespace PG4Map
{
    using System;
    using System.IO;

    internal class MovLoader
    {
        public static Maps.Move_s[] LoadMov(Stream stream)
        {
            Maps.Move_s[] array = new Maps.Move_s[0];
            BinaryReader reader = new BinaryReader(stream);
            reader.BaseStream.Seek(0L, SeekOrigin.Begin);
            int index = 0;
            for (index = 0; index < (stream.Length / 2L); index++)
            {
                Array.Resize<Maps.Move_s>(ref array, index + 1);
                array[index].actualMov = reader.ReadByte();
                array[index].actualFlag = reader.ReadByte();
            }
            return array;
        }

        public static Maps.Move_s_bw[] LoadMov_Bw(Stream stream)
        {
            Maps.Move_s_bw[] array = new Maps.Move_s_bw[0];
            BinaryReader reader = new BinaryReader(stream);
            reader.BaseStream.Seek(0L, SeekOrigin.Begin);
            int index = 0;
            for (index = 0; index < (stream.Length / 8L); index++)
            {
                Array.Resize<Maps.Move_s_bw>(ref array, index + 1);
                array[index].actualMov = reader.ReadInt16();
                array[index].par = reader.ReadInt16();
                array[index].par2 = reader.ReadInt16();
                array[index].actualFlag = reader.ReadInt16();
            }
            if (stream.Length > 5L && reader.BaseStream.Position<stream.Length)
            {
                //array[0].par3 = reader.ReadInt32();
            }
            return array;
        }
    }
}

