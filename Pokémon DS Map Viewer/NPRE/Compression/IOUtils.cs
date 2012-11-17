using System;

public static class IOUtils
{
    public static byte[] FromNDSu32(uint value)
    {
        return new byte[] { ((byte) (value & 0xff)), ((byte) ((value >> 8) & 0xff)), ((byte) ((value >> 0x10) & 0xff)), ((byte) ((value >> 0x18) & 0xff)) };
    }

    public static int ToNDSs32(byte[] buffer, int offset)
    {
        return (((buffer[offset] | (buffer[offset + 1] << 8)) | (buffer[offset + 2] << 0x10)) | (buffer[offset + 3] << 0x18));
    }

    public static int ToNDSu24(byte[] buffer, int offset)
    {
        return ((buffer[offset] | (buffer[offset + 1] << 8)) | (buffer[offset + 2] << 0x10));
    }

    public static uint ToNDSu32(byte[] buffer, int offset)
    {
        return (uint) (((buffer[offset] | (buffer[offset + 1] << 8)) | (buffer[offset + 2] << 0x10)) | (buffer[offset + 3] << 0x18));
    }
}

