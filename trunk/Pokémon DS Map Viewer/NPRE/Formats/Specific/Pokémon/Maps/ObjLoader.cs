namespace PG4Map
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal class ObjLoader
    {
        public static List<Maps.Obj_S> LoadObj(Stream stream)
        {
            List<Maps.Obj_S> list = new List<Maps.Obj_S>();
            BinaryReader reader = new BinaryReader(stream);
            reader.BaseStream.Seek(0L, SeekOrigin.Begin);
            int num = 0;
            for (num = 0; num < (stream.Length / 0x30L); num++)
            {
                Maps.Obj_S item = new Maps.Obj_S {
                    idObject = reader.ReadInt32(),
                    ySmall = reader.ReadInt16(),
                    yBig = reader.ReadInt16(),
                    zSmall = reader.ReadInt16(),
                    zBig = reader.ReadInt16(),
                    xSmall = reader.ReadInt16(),
                    xBig = reader.ReadInt16()
                };
                stream.Seek(13L, SeekOrigin.Current);
                item.heightObject = reader.ReadInt32();
                item.lengthObject = reader.ReadInt32();
                item.widthObject = reader.ReadInt32();
                stream.Seek(7L, SeekOrigin.Current);
                list.Add(item);
            }
            return list;
        }

        public static List<Maps.Obj_S> LoadObj_Bw(Stream stream)
        {
            List<Maps.Obj_S> list = new List<Maps.Obj_S>();
            BinaryReader reader = new BinaryReader(stream);
            reader.BaseStream.Seek(0L, SeekOrigin.Begin);
            int num = 0;
            for (num = 0; num < ((stream.Length - 1L) / 0x10L); num++)
            {
                Maps.Obj_S item = new Maps.Obj_S();
                if (num == 0)
                {
                    item.firstParameter = reader.ReadInt32();
                }
                item.ySmall = reader.ReadInt16();
                item.yBig = reader.ReadInt16();
                item.zSmall = reader.ReadInt16();
                item.zBig = reader.ReadInt16();
                item.xSmall = reader.ReadInt16();
                item.xBig = reader.ReadInt16();
                item.secondParameter = reader.ReadInt16();
                reader.BaseStream.Seek(1L, SeekOrigin.Current);
                item.idObject = reader.ReadByte();
                item.heightObject = -1;
                item.lengthObject = -1;
                item.widthObject = -1;
                list.Add(item);
            }
            return list;
        }
    }
}

