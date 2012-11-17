using System;
using System.IO;

public abstract class CompressionFormat
{
    protected CompressionFormat()
    {
    }

    public int Compress(string infile, string outfile)
    {
        int num;
        string directoryName = Path.GetDirectoryName(outfile);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }
        using (FileStream stream = File.Open(infile, FileMode.Open))
        {
            using (FileStream stream2 = File.Create(outfile))
            {
                num =   Compress(stream, stream.Length, stream2);
            }
        }
        return num;
    }

    public abstract int Compress(Stream instream, long inLength, Stream outstream);
    public virtual bool Supports(string file)
    {
        using (FileStream stream = new FileStream(file, FileMode.Open))
        {
            return   Supports(stream, stream.Length);
        }
    }

    public abstract bool Supports(Stream stream, long inLength);
}

