using System;
using System.IO;

public abstract class NitroCFormat : CompressionFormat
{
    protected byte magicByte;
    public static int MaxPlaintextSize = 0x180000;
    public static bool SkipLargePlaintexts = true;

    public NitroCFormat(byte magicByte)
    {
          magicByte = magicByte;
    }

    public override bool Supports(Stream stream, long inLength)
    {
        bool flag;
        long position = stream.Position;
        try
        {
            if (stream.ReadByte() !=   magicByte)
            {
                return false;
            }
            if (!SkipLargePlaintexts)
            {
                return true;
            }
            byte[] buffer = new byte[3];
            stream.Read(buffer, 0, 3);
            int num3 = IOUtils.ToNDSu24(buffer, 0);
            if (num3 == 0)
            {
                buffer = new byte[4];
                stream.Read(buffer, 0, 4);
                num3 = (int) IOUtils.ToNDSu32(buffer, 0);
            }
            flag = num3 <= MaxPlaintextSize;
        }
        finally
        {
            stream.Position = position;
        }
        return flag;
    }
}

