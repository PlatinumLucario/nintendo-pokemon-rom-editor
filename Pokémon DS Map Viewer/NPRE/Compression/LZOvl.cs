using System;
using System.IO;
using System.Runtime.InteropServices;

public class LZOvl : CompressionFormat
{
    private static bool lookAhead = false;

    public override int Compress(Stream instream, long inLength, Stream outstream)
    {
        byte[] buffer = new byte[inLength];
        instream.Read(buffer, 0, (int) inLength);
        Array.Reverse(buffer);
        MemoryStream stream = new MemoryStream(buffer);
        MemoryStream stream2 = new MemoryStream();
        int num =   CompressNormal(stream, inLength, stream2);
        int num2 = ((int) stream2.Length) + 8;
        if ((num2 % 4) != 0)
        {
            num2 += 4 - (num2 % 4);
        }
        if (num2 < inLength)
        {
            byte[] array = stream2.ToArray();
            Array.Reverse(array);
            outstream.Write(array, 0, array.Length);
            for (int i = array.Length; (i % 4) != 0; i++)
            {
                outstream.WriteByte(0xff);
            }
            outstream.WriteByte((byte) (num & 0xff));
            outstream.WriteByte((byte) ((num >> 8) & 0xff));
            outstream.WriteByte((byte) ((num >> 0x10) & 0xff));
            int num4 = num2 - array.Length;
            outstream.WriteByte((byte) num4);
            int num5 = ((int) inLength) - num2;
            outstream.WriteByte((byte) (num5 & 0xff));
            outstream.WriteByte((byte) ((num5 >> 8) & 0xff));
            outstream.WriteByte((byte) ((num5 >> 0x10) & 0xff));
            outstream.WriteByte((byte) ((num5 >> 0x18) & 0xff));
            return num2;
        }
        Array.Reverse(buffer);
        outstream.Write(buffer, 0, (int) inLength);
        outstream.WriteByte(0);
        outstream.WriteByte(0);
        outstream.WriteByte(0);
        outstream.WriteByte(0);
        return (((int) inLength) + 4);
    }

    public unsafe int CompressNormal(Stream instream, long inLength, Stream outstream)
    {
        if (lookAhead)
        {
            return   CompressWithLA(instream, inLength, outstream);
        }
        byte[] buffer = new byte[inLength];
        int num = instream.Read(buffer, 0, (int) inLength);
        int num2 = 0;
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
                int oldLength = Math.Min(num5, 0x1001);
                int num8 = LZUtil.GetOccurrenceLength(numRef + num5, (int) Math.Min((long) (inLength - num5), (long) 0x12L), (numRef + num5) - oldLength, oldLength, out num6);
                if (num6 == 1)
                {
                    num8 = 1;
                }
                else if (num6 == 2)
                {
                    num8 = 1;
                }
                if (num8 < 3)
                {
                    buffer2[count++] = numRef[num5++];
                }
                else
                {
                    num5 += num8;
                    buffer2[0] = (byte) (buffer2[0] | ((byte) (((int) 1) << (7 - num4))));
                    buffer2[count] = (byte) (((num8 - 3) << 4) & 240);
                    buffer2[count] = (byte) (buffer2[count] | ((byte) (((num6 - 3) >> 8) & 15)));
                    count++;
                    buffer2[count] = (byte) ((num6 - 3) & 0xff);
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
        int num2 = 0;
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
            int optimalCompressionPartLength =   GetOptimalCompressionPartLength(numArray);
            while (index < optimalCompressionPartLength)
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
                    buffer2[count] = (byte) (buffer2[count] | ((byte) (((numArray2[index] - 3) >> 8) & 15)));
                    count++;
                    buffer2[count] = (byte) ((numArray2[index] - 3) & 0xff);
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
            while (index < inLength)
            {
                outstream.WriteByte(numRef[index++]);
            }
        }
        return num2;
    }

    public void Decompress(string infile, string outfile)
    {
        string directoryName = Path.GetDirectoryName(outfile);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }
        using (FileStream stream = new FileStream(infile, FileMode.Open))
        {
            using (FileStream stream2 = new FileStream(outfile, FileMode.Create))
            {
                long length = stream.Length;
                if (Path.GetFileName(infile) == "arm9.bin")
                {
                    length -= 12L;
                }
                  Decompress(stream, length, stream2);
            }
        }
    }

    public long Decompress(Stream instream, long inLength, Stream outstream)
    {
        instream.Position += inLength - 4L;
        byte[] buffer = new byte[4];
        try
        {
            instream.Read(buffer, 0, 4);
        }
        catch (EndOfStreamException)
        {
        }
        uint num = IOUtils.ToNDSu32(buffer, 0);
        if (num == 0)
        {
            instream.Position -= inLength;
            buffer = new byte[inLength - 4L];
            instream.Read(buffer, 0, (int) (inLength - 4L));
            outstream.Write(buffer, 0, (int) (inLength - 4L));
            instream.Position += 4L;
            return (inLength - 4L);
        }
        instream.Position -= 5L;
        int num2 = instream.ReadByte();
        instream.Position -= 4L;
        instream.Read(buffer, 0, 3);
        int count = (buffer[0] | (buffer[1] << 8)) | (buffer[2] << 0x10);
        if ((count + num2) >= inLength)
        {
            count = ((int) inLength) - num2;
        }
        buffer = new byte[(inLength - num2) - count];
        instream.Position -= inLength - 5L;
        instream.Read(buffer, 0, buffer.Length);
        outstream.Write(buffer, 0, buffer.Length);
        buffer = new byte[count];
        instream.Read(buffer, 0, count);
        byte[] buffer2 = new byte[(count + num2) + num];
        int num4 = 0;
        int length = buffer2.Length;
        int num6 = 0;
        byte num7 = 0;
        byte num8 = 1;
        while (num4 < length)
        {
            byte num15;
            if (num8 == 1)
            {
                num7 = buffer[(buffer.Length - 1) - num6];
                num6++;
                num8 = 0x80;
            }
            else
            {
                num8 = (byte) (num8 >> 1);
            }
            if ((num7 & num8) > 0)
            {
                int num9 = buffer[(count - 1) - num6];
                num6++;
                int num10 = buffer[(count - 1) - num6];
                num6++;
                int num11 = num9 >> 4;
                num11 += 3;
                int num12 = ((num9 & 15) << 8) | num10;
                num12 += 3;
                if (num12 > num4)
                {
                    num12 = 2;
                }
                int num13 = num4 - num12;
                for (int i = 0; i < num11; i++)
                {
                    num15 = buffer2[(buffer2.Length - 1) - num13];
                    num13++;
                    buffer2[(buffer2.Length - 1) - num4] = num15;
                    num4++;
                }
            }
            else
            {
                num15 = buffer[(buffer.Length - 1) - num6];
                num6++;
                buffer2[(buffer2.Length - 1) - num4] = num15;
                num4++;
            }
        }
        outstream.Write(buffer2, 0, buffer2.Length);
        instream.Position += num2;
        return (length + ((inLength - num2) - count));
    }

    public unsafe void GetOptimalCompressionLengths(byte* indata, int inLength, out int[] lengths, out int[] disps)
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
            int oldLength = Math.Min(0x1001, i);
            int num3 = LZUtil.GetOccurrenceLength(indata + i, Math.Min(inLength - i, 0x12), (indata + i) - oldLength, oldLength, out disps[i]);
            if (disps[i] < 3)
            {
                num3 = 1;
            }
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

    private int GetOptimalCompressionPartLength(int[] blocklengths)
    {
        int num4;
        int num = 0;
        int num2 = 0;
        int num3 = 0;
        for (num4 = 0; num4 < blocklengths.Length; num4 += blocklengths[num4])
        {
            if (num2 == 8)
            {
                num++;
                num2 = 0;
                num3++;
            }
            num2++;
            if (blocklengths[num4] >= 3)
            {
                num3 += 2;
            }
            else
            {
                num3++;
            }
        }
        int[] numArray = new int[blocklengths.Length];
        num = 0;
        num2 = 0;
        num4 = 0;
        while (num4 < blocklengths.Length)
        {
            if (num2 == 8)
            {
                num++;
                num2 = 0;
                num3--;
            }
            if (blocklengths[num4] >= 3)
            {
                num3 -= 2;
            }
            else
            {
                num3--;
            }
            numArray[num4] = num3;
            num4 += blocklengths[num4];
            num2++;
            if (num3 > (blocklengths.Length - num4))
            {
                return num4;
            }
        }
        return blocklengths.Length;
    }

    public override bool Supports(string file)
    {
        using (FileStream stream = File.OpenRead(file))
        {
            long length = stream.Length;
            if (Path.GetFileName(file) == "arm9.bin")
            {
                length -= 12L;
            }
            return   Supports(stream, length);
        }
    }

    public override bool Supports(Stream stream, long inLength)
    {
        if (inLength > 0xffffffffL)
        {
            return false;
        }
        if (inLength < 4L)
        {
            return false;
        }
        long position = stream.Position;
        byte[] buffer = new byte[Math.Min(inLength, 0x20L)];
        stream.Position += inLength - buffer.Length;
        stream.Read(buffer, 0, buffer.Length);
        stream.Position = position;
        if (IOUtils.ToNDSu32(buffer, buffer.Length - 4) == 0)
        {
            return false;
        }
        if (buffer.Length < 8)
        {
            return false;
        }
        byte num3 = buffer[buffer.Length - 5];
        if (inLength < num3)
        {
            return false;
        }
        int num4 = ((buffer[buffer.Length - 6] << 0x10) | (buffer[buffer.Length - 7] << 8)) | buffer[buffer.Length - 8];
        if ((num4 >= (inLength - num3)) && (num4 != inLength))
        {
            return false;
        }
        for (int i = buffer.Length - 9; i >= (buffer.Length - num3); i--)
        {
            if (buffer[i] != 0xff)
            {
                return false;
            }
        }
        return true;
    }

    public static bool LookAhead
    {
        set
        {
            lookAhead = value;
        }
    }
}

