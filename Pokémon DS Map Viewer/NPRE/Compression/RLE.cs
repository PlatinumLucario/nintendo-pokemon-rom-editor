namespace DSDecmp.Formats.Nitro
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class RLE : NitroCFormat
    {
        public RLE() : base(0x30)
        {
        }

        public override int Compress(Stream instream, long inLength, Stream outstream)
        {
            byte num6;
            int num7;
            List<byte> list = new List<byte>();
            byte[] buffer = new byte[130];
            int num = 0;
            int num2 = 0;
            int num4 = 1;
            while (num2 < inLength)
            {
                int num3;
                bool flag = false;
                while ((num < buffer.Length) && (num2 < inLength))
                {
                    num3 = instream.ReadByte();
                    num2++;
                    buffer[num++] = (byte) num3;
                    if (num > 1)
                    {
                        if (num3 == buffer[num - 2])
                        {
                            num4++;
                        }
                        else
                        {
                            num4 = 1;
                        }
                    }
                    flag = num4 > 2;
                    if (flag)
                    {
                        break;
                    }
                }
                int num5 = 0;
                if (flag)
                {
                    num5 = num - 3;
                }
                else
                {
                    num5 = Math.Min(num, buffer.Length - 2);
                }
                if (num5 > 0)
                {
                    num6 = (byte) (num5 - 1);
                    list.Add(num6);
                    num7 = 0;
                    while (num7 < num5)
                    {
                        list.Add(buffer[num7]);
                        num7++;
                    }
                    num7 = num5;
                    while (num7 < num)
                    {
                        buffer[num7 - num5] = buffer[num7];
                        num7++;
                    }
                    num -= num5;
                }
                if (flag)
                {
                    while ((num < buffer.Length) && (num2 < inLength))
                    {
                        num3 = instream.ReadByte();
                        num2++;
                        buffer[num++] = (byte) num3;
                        if (num3 != buffer[0])
                        {
                            break;
                        }
                        num4++;
                    }
                    num6 = (byte) (0x80 | (num4 - 3));
                    list.Add(num6);
                    list.Add(buffer[0]);
                    if (num4 != num)
                    {
                        buffer[0] = buffer[num - 1];
                    }
                    num -= num4;
                }
            }
            if (num > 0)
            {
                num6 = (byte) (num - 1);
                list.Add(num6);
                for (num7 = 0; num7 < num; num7++)
                {
                    list.Add(buffer[num7]);
                }
                num = 0;
            }
            outstream.WriteByte(0x30);
            int count = list.Count;
            outstream.WriteByte((byte) (inLength & 0xffL));
            outstream.WriteByte((byte) ((inLength >> 8) & 0xffL));
            outstream.WriteByte((byte) ((inLength >> 0x10) & 0xffL));
            outstream.Write(list.ToArray(), 0, count);
            return (count + 4);
        }

        public long Decompress(Stream instream, long inLength, Stream outstream)
        {
            long num = 0L;
            byte num2 = (byte) instream.ReadByte();
            byte[] buffer = new byte[3];
            instream.Read(buffer, 0, 3);
            int num3 = IOUtils.ToNDSu24(buffer, 0);
            num += 4L;
            if (num3 == 0)
            {
                buffer = new byte[4];
                instream.Read(buffer, 0, 4);
                num3 = IOUtils.ToNDSs32(buffer, 0);
                num += 4L;
            }
            int num4 = 0;
            while (num4 < num3)
            {
                int num5 = instream.ReadByte();
                num += 1L;
                bool flag = (num5 & 0x80) > 0;
                int num6 = num5 & 0x7f;
                if (flag)
                {
                    num6 += 3;
                }
                else
                {
                    num6++;
                }
                if (flag)
                {
                    int num7 = instream.ReadByte();
                    num += 1L;
                    byte num8 = (byte) num7;
                    for (int i = 0; i < num6; i++)
                    {
                        outstream.WriteByte(num8);
                        num4++;
                    }
                }
                else
                {
                    int count = num6;
                    if ((num + num6) > inLength)
                    {
                        count = (int) (inLength - num);
                    }
                    byte[] buffer2 = new byte[num6];
                    int num11 = instream.Read(buffer2, 0, count);
                    num += num11;
                    outstream.Write(buffer2, 0, num11);
                    num4 += num11;
                }
            }
            if (num < inLength)
            {
            }
            return (long) num3;
        }
    }
}

