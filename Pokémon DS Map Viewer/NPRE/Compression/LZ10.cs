using System;
using System.IO;
using System.Runtime.InteropServices;

public class LZ10 : NitroCFormat
{
    private static bool lookAhead = false;

    public LZ10() : base(0x10)
    {
    }

    public override unsafe int Compress(Stream instream, long inLength, Stream outstream)
    {
        if (lookAhead)
        {
            return   CompressWithLA(instream, inLength, outstream);
        }
        byte[] buffer = new byte[inLength];
        int num = instream.Read(buffer, 0, (int) inLength);
        outstream.WriteByte(base.magicByte);
        outstream.WriteByte((byte) (inLength & 0xffL));
        outstream.WriteByte((byte) ((inLength >> 8) & 0xffL));
        outstream.WriteByte((byte) ((inLength >> 0x10) & 0xffL));
        int num2 = 4;
        fixed (byte* numRef = buffer)
        {
            byte[] buffer2 = new byte[0x11];
            buffer2[0] = 0;
            int count = 1;
            int num4 = 0;
            int num5 = 0;
            while (num5 < inLength)
            {
                int num6;
                if (num4 == 8)
                {
                    outstream.Write(buffer2, 0, count);
                    num2 += count;
                    buffer2[0] = 0;
                    count = 1;
                    num4 = 0;
                }
                int oldLength = Math.Min(num5, 0x1000);
                int num8 = LZUtil.GetOccurrenceLength(numRef + num5, (int) Math.Min((long) (inLength - num5), (long) 0x12L), (numRef + num5) - oldLength, oldLength, out num6);
                if (num8 < 3)
                {
                    buffer2[count++] = numRef[num5++];
                }
                else
                {
                    num5 += num8;
                    buffer2[0] = (byte) (buffer2[0] | ((byte) (((int) 1) << (7 - num4))));
                    buffer2[count] = (byte) (((num8 - 3) << 4) & 240);
                    buffer2[count] = (byte) (buffer2[count] | ((byte) (((num6 - 1) >> 8) & 15)));
                    count++;
                    buffer2[count] = (byte) ((num6 - 1) & 0xff);
                    count++;
                }
                num4++;
            }
            if (num4 > 0)
            {
                outstream.Write(buffer2, 0, count);
                num2 += count;
            }
        }
        return num2;
    }

    public unsafe int CompressWithLA(Stream instream, long inLength, Stream outstream)
    {
        byte[] buffer = new byte[inLength];
        int num = instream.Read(buffer, 0, (int) inLength);
        outstream.WriteByte(base.magicByte);
        outstream.WriteByte((byte) (inLength & 0xffL));
        outstream.WriteByte((byte) ((inLength >> 8) & 0xffL));
        outstream.WriteByte((byte) ((inLength >> 0x10) & 0xffL));
        int num2 = 4;
        fixed (byte* numRef = buffer)
        {
            int[] numArray;
            int[] numArray2;
            byte[] buffer2 = new byte[0x11];
            buffer2[0] = 0;
            int count = 1;
            int num4 = 0;
            int index = 0;
              GetOptimalCompressionLengths(numRef, buffer.Length, out numArray, out numArray2);
            while (index < inLength)
            {
                if (num4 == 8)
                {
                    outstream.Write(buffer2, 0, count);
                    num2 += count;
                    buffer2[0] = 0;
                    count = 1;
                    num4 = 0;
                }
                if (numArray[index] == 1)
                {
                    buffer2[count++] = numRef[index++];
                }
                else
                {
                    buffer2[0] = (byte) (buffer2[0] | ((byte) (((int) 1) << (7 - num4))));
                    buffer2[count] = (byte) (((numArray[index] - 3) << 4) & 240);
                    buffer2[count] = (byte) (buffer2[count] | ((byte) (((numArray2[index] - 1) >> 8) & 15)));
                    count++;
                    buffer2[count] = (byte) ((numArray2[index] - 1) & 0xff);
                    count++;
                    index += numArray[index];
                }
                num4++;
            }
            if (num4 > 0)
            {
                outstream.Write(buffer2, 0, count);
                num2 += count;
            }
        }
        return num2;
    }

    public static long Decompress(ClosableMemoryStream instream, long inLength, ref ClosableMemoryStream outstream)
    {
        long num = 0L;
        instream.Seek(0L, SeekOrigin.Begin);
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
        int num4 = 0x1000;
        byte[] buffer2 = new byte[num4];
        int index = 0;
        int num6 = 0;
        int num7 = 0;
        int num8 = 1;
        outstream = new ClosableMemoryStream();
        while (num6 < num3)
        {
            if (num8 == 1)
            {
                num7 = instream.ReadByte();
                num += 1L;
                num8 = 0x80;
            }
            else
            {
                num8 = num8 >> 1;
            }
            if ((num7 & num8) > 0)
            {
                if (((num + 1L) >= inLength) && (num < inLength))
                {
                    instream.ReadByte();
                    num += 1L;
                }
                int num9 = instream.ReadByte();
                num += 1L;
                int num10 = instream.ReadByte();
                num += 1L;
                int num11 = num9 >> 4;
                num11 += 3;
                int num12 = ((num9 & 15) << 8) | num10;
                num12++;
                int num13 = (index + num4) - num12;
                for (int i = 0; i < num11; i++)
                {
                    byte num15 = buffer2[num13 % num4];
                    num13++;
                    outstream.WriteByte(num15);
                    buffer2[index] = num15;
                    index = (index + 1) % num4;
                }
                num6 += num11;
            }
            else
            {
                int num16 = instream.ReadByte();
                num += 1L;
                num6++;
                outstream.WriteByte((byte) num16);
                buffer2[index] = (byte) num16;
                index = (index + 1) % num4;
            }
        }
        if (num < inLength)
        {
        }
        return (long) num3;
    }

    private unsafe void GetOptimalCompressionLengths(byte* indata, int inLength, out int[] lengths, out int[] disps)
    {
        lengths = new int[inLength];
        disps = new int[inLength];
        int[] numArray = new int[inLength];
        for (int i = inLength - 1; i >= 0; i--)
        {
            numArray[i] = 0x7fffffff;
            lengths[i] = 1;
            if ((i + 1) >= inLength)
            {
                numArray[i] = 1;
            }
            else
            {
                numArray[i] = 1 + numArray[i + 1];
            }
            int oldLength = Math.Min(0x1000, i);
            int num3 = LZUtil.GetOccurrenceLength(indata + i, Math.Min(inLength - i, 0x12), (indata + i) - oldLength, oldLength, out disps[i]);
            for (int j = 3; j <= num3; j++)
            {
                int num5;
                if ((i + j) >= inLength)
                {
                    num5 = 2;
                }
                else
                {
                    num5 = 2 + numArray[i + j];
                }
                if (num5 < numArray[i])
                {
                    lengths[i] = j;
                    numArray[i] = num5;
                }
            }
        }
    }

    public static bool LookAhead
    {
        set
        {
            lookAhead = value;
        }
    }
}

