using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class NCLR
{
    public static NCLR_s Palette;

    public static NCLR_s BitmapToPalette(string bitmap, int paletteIndex = 0)
    {
        NCLR_s _s = new NCLR_s();
        BinaryReader reader = new BinaryReader(File.OpenRead(bitmap));
        if (new string(reader.ReadChars(2)) != "BM")
        {
            _s.header.id = "RLCN".ToCharArray();
        }
        _s.header.endianess = 0xfeff;
        _s.header.constant = 0x100;
        _s.header.header_size = 0x10;
        _s.header.nSection = 1;
        reader.BaseStream.Position = 0x1cL;
        ushort num = reader.ReadUInt16();
        if (num == 4)
        {
            _s.pltt.depth = ColorDepth.Depth4Bit;
        }
        else if (num == 8)
        {
            _s.pltt.depth = ColorDepth.Depth8Bit;
        }
        else
        {
            Stream stream1 = reader.BaseStream;
            stream1.Position += 0x10L;
        }
        _s.pltt.nColors = reader.ReadUInt32();
        if (_s.pltt.nColors == 0)
        {
            _s.pltt.nColors = (uint)((num == 4) ? 0x10 : 0x100);
        }
        Stream baseStream = reader.BaseStream;
        baseStream.Position += 4L;
        _s.pltt.palettes = new NTFP[paletteIndex + 1];
        _s.pltt.palettes[paletteIndex].colors = new Color[_s.pltt.nColors];
        for (int i = 0; i < _s.pltt.nColors; i++)
        {
            byte[] buffer = reader.ReadBytes(4);
            _s.pltt.palettes[paletteIndex].colors[i] = Color.FromArgb(buffer[2], buffer[1], buffer[0]);
        }
        byte[] bytes = Convertir.ColorToBGR555(_s.pltt.palettes[paletteIndex].colors);
        _s.pltt.palettes[paletteIndex].colors = Convertir.BGR555(bytes);
        _s.pltt.ID = "TTLP".ToCharArray();
        _s.pltt.paletteLength = _s.pltt.nColors * 2;
        _s.pltt.unknown1 = 0;
        _s.pltt.length = _s.pltt.paletteLength + 0x18;
        _s.header.file_size = _s.pltt.length + _s.header.header_size;
        reader.Close();
        return _s;
    }

    public static void Escribir(NCLR_s paleta, string fileout)
    {
        BinaryWriter writer = new BinaryWriter(File.OpenWrite(fileout));
        writer.Write(paleta.header.id);
        writer.Write(paleta.header.endianess);
        writer.Write(paleta.header.constant);
        writer.Write(paleta.header.file_size);
        writer.Write(paleta.header.header_size);
        writer.Write(paleta.header.nSection);
        writer.Write(paleta.pltt.ID);
        writer.Write(paleta.pltt.length);
        writer.Write((paleta.pltt.depth == ColorDepth.Depth4Bit) ? ((ushort)3) : ((ushort)4));
        writer.Write((ushort)0);
        writer.Write((uint)0);
        writer.Write(paleta.pltt.paletteLength);
        writer.Write(0x10);
        for (int i = 0; i < paleta.pltt.palettes.Length; i++)
        {
            writer.Write(Convertir.ColorToBGR555(paleta.pltt.palettes[i].colors));
        }
        writer.Flush();
        writer.Close();
    }

    public static NCLR_s Leer(BinaryReader br, int id)
    {
        NCLR_s _s = new NCLR_s
        {
            id = (uint)id
        };
        _s.header.id = br.ReadChars(4);
        _s.header.endianess = br.ReadUInt16();
        if (_s.header.endianess == 0xfffe)
        {
            _s.header.id.Reverse<char>();
        }
        _s.header.constant = br.ReadUInt16();
        _s.header.file_size = br.ReadUInt32();
        _s.header.header_size = br.ReadUInt16();
        _s.header.nSection = br.ReadUInt16();
        if ((_s.header.nSection < 1) || (_s.header.nSection > 2))
        {
            _s.pltt = Seccion_PLTT(ref br);
        }
        if ((br.BaseStream.Length != br.BaseStream.Position) && (new string(br.ReadChars(4)) == "PMCP"))
        {
            Stream baseStream = br.BaseStream;
            baseStream.Position -= 4L;
            _s.pmcp = Section_PMCP(ref br);
            List<NTFP> list = new List<NTFP>();
            for (int i = 0; i < _s.pmcp.first_palette_num; i++)
            {
                NTFP item = new NTFP
                {
                    colors = new Color[0]
                };
                list.Add(item);
            }
            list.AddRange(_s.pltt.palettes);
            _s.pltt.palettes = list.ToArray();
        }
        br.Close();
        return _s;
    }

    public static NTFP Paleta_NTFP(ref BinaryReader br, uint colores)
    {
        return new NTFP { colors = Convertir.BGR555(br.ReadBytes((int)(colores * 2))) };
    }

    public static NCLR_s Read_WinPal(string file, ColorDepth depth)
    {
        BinaryReader reader = new BinaryReader(File.OpenRead(file));
        NCLR_s _s = new NCLR_s();
        _s.header.id = reader.ReadChars(4);
        _s.header.file_size = reader.ReadUInt32();
        _s.pltt.ID = reader.ReadChars(4);
        reader.ReadChars(4);
        reader.ReadUInt32();
        reader.ReadUInt16();
        _s.pltt.nColors = reader.ReadUInt16();
        _s.pltt.depth = depth;
        uint num = (depth == ColorDepth.Depth4Bit) ? 0x10 : _s.pltt.nColors;
        _s.pltt.paletteLength = num * 2;
        _s.pltt.palettes = new NTFP[(depth == ColorDepth.Depth4Bit) ? ((int)(_s.pltt.nColors / 0x10)) : 1];
        for (int i = 0; i < _s.pltt.palettes.Length; i++)
        {
            _s.pltt.palettes[i].colors = new Color[num];
            for (int j = 0; j < num; j++)
            {
                Color color = Color.FromArgb(reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
                reader.ReadByte();
                _s.pltt.palettes[i].colors[j] = color;
            }
        }
        reader.Close();
        return _s;
    }

    public static Color[][] Read_WinPal2(string file, ColorDepth depth)
    {
        BinaryReader reader = new BinaryReader(File.OpenRead(file));
        NCLR_s _s = new NCLR_s();
        _s.header.id = reader.ReadChars(4);
        _s.header.file_size = reader.ReadUInt32();
        _s.pltt.ID = reader.ReadChars(4);
        reader.ReadChars(4);
        reader.ReadUInt32();
        reader.ReadUInt16();
        _s.pltt.nColors = reader.ReadUInt16();
        _s.pltt.depth = depth;
        uint num = (depth == ColorDepth.Depth4Bit) ? 0x10 : _s.pltt.nColors;
        _s.pltt.paletteLength = num * 2;
        Color[][] colorArray = new Color[(depth == ColorDepth.Depth4Bit) ? ((int)(_s.pltt.nColors / 0x10)) : 1][];
        for (int i = 0; i < colorArray.Length; i++)
        {
            colorArray[i] = new Color[num];
            for (int j = 0; j < num; j++)
            {
                Color color = Color.FromArgb(reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
                reader.ReadByte();
                colorArray[i][j] = color;
            }
        }
        reader.Close();
        return colorArray;
    }

    public static TTLP Seccion_PLTT(ref BinaryReader br)
    {
        TTLP ttlp = new TTLP();
        long position = br.BaseStream.Position;
        ttlp.ID = br.ReadChars(4);
        ttlp.length = br.ReadUInt32();
        ttlp.depth = (br.ReadUInt16() == 3) ? ColorDepth.Depth4Bit : ColorDepth.Depth8Bit;
        br.ReadUInt16();
        ttlp.unknown1 = br.ReadUInt32();
        ttlp.paletteLength = br.ReadUInt32();
        uint num2 = br.ReadUInt32();
        ttlp.nColors = (ttlp.depth == ColorDepth.Depth4Bit) ? 0x10 : (ttlp.paletteLength / 2);
        ttlp.palettes = new NTFP[ttlp.paletteLength / (ttlp.nColors * 2)];
        if ((ttlp.paletteLength > ttlp.length) || (ttlp.paletteLength == 0))
        {
            ttlp.palettes = new NTFP[ttlp.length / (ttlp.nColors * 2)];
        }
        br.BaseStream.Position = 0x18 + num2;
        for (int i = 0; i < ttlp.palettes.Length; i++)
        {
            ttlp.palettes[i] = Paleta_NTFP(ref br, ttlp.nColors);
        }
        return ttlp;
    }

    public static PMCP Section_PMCP(ref BinaryReader br)
    {
        return new PMCP { ID = br.ReadChars(4), blockSize = br.ReadUInt32(), unknown1 = br.ReadUInt16(), unknown2 = br.ReadUInt16(), unknown3 = br.ReadUInt32(), first_palette_num = br.ReadUInt16() };
    }

    public static Bitmap[] Show(NCLR_s nclr)
    {
        Bitmap[] bitmapArray = new Bitmap[nclr.pltt.palettes.Length];
        for (int i = 0; i < bitmapArray.Length; i++)
        {
            bitmapArray[i] = new Bitmap(160, 160);
            bool flag = false;
            for (int j = 0; (j < 0x10) & !flag; j++)
            {
                for (int k = 0; k < 0x10; k++)
                {
                    if (nclr.pltt.palettes[i].colors.Length == (k + (0x10 * j)))
                    {
                        flag = true;
                        break;
                    }
                    for (int m = 0; m < 10; m++)
                    {
                        for (int n = 0; n < 10; n++)
                        {
                            bitmapArray[i].SetPixel((k * 10) + n, (j * 10) + m, nclr.pltt.palettes[i].colors[k + (0x10 * j)]);
                        }
                    }
                }
            }
        }
        return bitmapArray;
    }

    public static Bitmap Show(Color[] colors)
    {
        Bitmap bitmap = new Bitmap(160, 160);
        bool flag = false;
        for (int i = 0; (i < 0x10) && !flag; i++)
        {
            for (int j = 0; j < 0x10; j++)
            {
                if (colors.Length == (j + (0x10 * i)))
                {
                    flag = true;
                    break;
                }
                for (int k = 0; k < 10; k++)
                {
                    for (int m = 0; m < 10; m++)
                    {
                        bitmap.SetPixel((j * 10) + m, (i * 10) + k, colors[j + (0x10 * i)]);
                    }
                }
            }
        }
        return bitmap;
    }

    public static void Write_WinPal(string fileout, Color[][] palette)
    {
        int num2;
        if (File.Exists(fileout))
        {
            File.Delete(fileout);
        }
        BinaryWriter writer = new BinaryWriter(File.OpenWrite(fileout));
        int num = 0;
        for (num2 = 0; num2 < palette.Length; num2++)
        {
            num += palette[num2].Length;
        }
        writer.Write(new char[] { 'R', 'I', 'F', 'F' });
        writer.Write((uint)(0x10 + (num * 4)));
        writer.Write(new char[] { 'P', 'A', 'L', ' ' });
        writer.Write(new char[] { 'd', 'a', 't', 'a' });
        writer.Write((uint)0);
        writer.Write((ushort)0x300);
        writer.Write((ushort)num);
        for (num2 = 0; num2 < palette.Length; num2++)
        {
            for (int i = 0; i < palette[num2].Length; i++)
            {
                writer.Write(palette[num2][i].R);
                writer.Write(palette[num2][i].G);
                writer.Write(palette[num2][i].B);
                writer.Write((byte)0);
                writer.Flush();
            }
        }
        writer.Close();
    }

    public static void Write_WinPal(string fileout, NCLR_s palette)
    {
        int num2;
        if (File.Exists(fileout))
        {
            File.Delete(fileout);
        }
        BinaryWriter writer = new BinaryWriter(File.OpenWrite(fileout));
        int num = 0;
        for (num2 = 0; num2 < palette.pltt.palettes.Length; num2++)
        {
            num += palette.pltt.palettes[num2].colors.Length;
        }
        writer.Write(new char[] { 'R', 'I', 'F', 'F' });
        writer.Write((uint)(0x10 + (num * 4)));
        writer.Write(new char[] { 'P', 'A', 'L', ' ' });
        writer.Write(new char[] { 'd', 'a', 't', 'a' });
        writer.Write((uint)0);
        writer.Write((ushort)0x300);
        writer.Write((ushort)num);
        for (num2 = 0; num2 < palette.pltt.palettes.Length; num2++)
        {
            for (int i = 0; i < palette.pltt.palettes[num2].colors.Length; i++)
            {
                writer.Write(palette.pltt.palettes[num2].colors[i].R);
                writer.Write(palette.pltt.palettes[num2].colors[i].G);
                writer.Write(palette.pltt.palettes[num2].colors[i].B);
                writer.Write((byte)0);
                writer.Flush();
            }
        }
        writer.Close();
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Header
    {
        public char[] id;
        public ushort endianess;
        public ushort constant;
        public uint file_size;
        public ushort header_size;
        public ushort nSection;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NCLR_s
    {
        public NCLR.Header header;
        public NCLR.TTLP pltt;
        public NCLR.PMCP pmcp;
        public object other;
        public uint id;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NTFP
    {
        public Color[] colors;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PMCP
    {
        public char[] ID;
        public uint blockSize;
        public ushort unknown1;
        public ushort unknown2;
        public uint unknown3;
        public ushort first_palette_num;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TTLP
    {
        public char[] ID;
        public uint length;
        public ColorDepth depth;
        public uint unknown1;
        public uint paletteLength;
        public uint nColors;
        public NCLR.NTFP[] palettes;
    }
}

