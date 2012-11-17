namespace System.IO
{
    using System;
    using System.Runtime.CompilerServices;

    public static class StreamExt
    {
        public static long GetRemainingLength(this Stream stream)
        {
            return (stream.Length - stream.Position);
        }

        public static void Skip(this Stream stream, long skipCount)
        {
            stream.Seek(skipCount, SeekOrigin.Current);
        }

        public static void Skip(this Stream stream, uint skipCount)
        {
            stream.Skip((long) skipCount);
        }
    }
}

