using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public static class Convertir
{
    public static Color[] BGR555(byte[] bytes)
    {
        Color[] colorArray = new Color[bytes.Length / 2];
        for (int i = 0; i < (bytes.Length / 2); i++)
        {
            colorArray[i] = BGR555(bytes[i * 2], bytes[(i * 2) + 1]);
        }
        return colorArray;
    }

    public static Color BGR555(byte byte1, byte byte2)
    {
        short num4 = BitConverter.ToInt16(new byte[] { byte1, byte2 }, 0);
        int red = (num4 & 0x1f) * 8;
        int green = ((num4 & 0x3e0) >> 5) * 8;
        int blue = ((num4 & 0x7c00) >> 10) * 8;
        return Color.FromArgb(red, green, blue);
    }

    public static byte[] Bit4ToBit8(byte[] bits4)
    {
        List<byte> list = new List<byte>();
        for (int i = 0; i < bits4.Length; i += 2)
        {
            int num2 = bits4[i];
            int num3 = bits4[i + 1] << 4;
            list.Add((byte) (num2 + num3));
        }
        return list.ToArray();
    }

    public static byte[] Bit8ToBit4(byte[] bits8)
    {
        List<byte> list = new List<byte>();
        for (int i = 0; i < bits8.Length; i++)
        {
            list.Add((byte) (bits8[i] & 15));
            list.Add((byte) ((bits8[i] & 240) >> 4));
        }
        return list.ToArray();
    }

    public static byte[][] BytesToTiles(byte[] bytes)
    {
        List<byte[]> list = new List<byte[]>();
        List<byte> list2 = new List<byte>();
        for (int i = 0; i < (bytes.Length / 0x40); i++)
        {
            for (int j = 0; j < 0x40; j++)
            {
                list2.Add(bytes[j + (i * 0x40)]);
            }
            list.Add(list2.ToArray());
            list2.Clear();
        }
        return list.ToArray();
    }

    public static byte[][] BytesToTiles_NoChanged(byte[] bytes, int tilesX, int tilesY)
    {
        List<byte[]> list = new List<byte[]>();
        List<byte> list2 = new List<byte>();
        for (int i = 0; i < tilesY; i++)
        {
            for (int j = 0; j < tilesX; j++)
            {
                for (int k = 0; k < 8; k++)
                {
                    for (int m = 0; m < 8; m++)
                    {
                        list2.Add(bytes[((j * 8) + ((i * tilesX) * 0x40)) + (m + ((k * 8) * tilesX))]);
                    }
                }
                list.Add(list2.ToArray());
                list2.Clear();
            }
        }
        return list.ToArray();
    }

    public static void Change_Color(ref byte[][] tiles, int oldIndex, int newIndex)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                if (tiles[i][j] == oldIndex)
                {
                    tiles[i][j] = (byte) newIndex;
                }
                else if (tiles[i][j] == newIndex)
                {
                    tiles[i][j] = (byte) oldIndex;
                }
            }
        }
    }

    public static byte[] ColorToBGR555(Color[] colores)
    {
        List<byte> list = new List<byte>(colores.Length * 2);
        for (int i = 0; i < colores.Length; i++)
        {
            int num2 = colores[i].R / 8;
            int num3 = (colores[i].G / 8) << 5;
            int num4 = (colores[i].B / 8) << 10;
            ushort num5 = (ushort) ((num2 + num3) + num4);
            list.AddRange(BitConverter.GetBytes(num5));
        }
        return list.ToArray();
    }

    public static ushort MapInfo(NSCR.NTFS map)
    {
        int num = map.nPalette << 12;
        int num2 = map.yFlip << 11;
        int num3 = map.xFlip << 10;
        int num4 = ((num + num2) + num3) + map.nTile;
        return (ushort) num4;
    }

    public static NSCR.NTFS MapInfo(ushort value)
    {
        return new NSCR.NTFS { nTile = (ushort) (value & 0x3ff), xFlip = (byte) ((value >> 10) & 1), yFlip = (byte) ((value >> 11) & 1), nPalette = (byte) ((value >> 12) & 15) };
    }

    public static NCLR.TTLP Palette_4bppTo8bpp(NCLR.TTLP palette)
    {
        NCLR.TTLP ttlp = new NCLR.TTLP {
            ID = palette.ID,
            length = palette.length,
            unknown1 = palette.unknown1
        };
        List<Color> list = new List<Color>();
        for (int i = 0; i < palette.palettes.Length; i++)
        {
            list.AddRange(palette.palettes[i].colors);
        }
        ttlp.palettes = new NCLR.NTFP[1];
        ttlp.palettes[0].colors = list.ToArray();
        ttlp.nColors = (uint) ttlp.palettes[0].colors.Length;
        ttlp.paletteLength = ttlp.nColors * 2;
        ttlp.depth = ColorDepth.Depth8Bit;
        return ttlp;
    }

    public static Color[][] Palette_4bppTo8bpp(Color[][] palette)
    {
        List<Color> list = new List<Color>();
        for (int i = 0; i < palette.Length; i++)
        {
            list.AddRange(palette[i]);
        }
        return new Color[][] { list.ToArray() };
    }

    public static Color[][] Palette_8bppTo4bpp(Color[][] palette)
    {
        Color[][] colorArray;
        int num2;
        int length = palette[0].Length % 0x10;
        if (length == 0)
        {
            colorArray = new Color[palette[0].Length / 0x10][];
            for (num2 = 0; num2 < colorArray.Length; num2++)
            {
                colorArray[num2] = new Color[0x10];
                Array.Copy(palette[0], num2 * 0x10, colorArray[num2], 0, 0x10);
            }
            return colorArray;
        }
        colorArray = new Color[(palette[0].Length / 0x10) + 1][];
        for (num2 = 0; num2 < (colorArray.Length - 1); num2++)
        {
            colorArray[num2] = new Color[0x10];
            Array.Copy(palette[0], num2 * 0x10, colorArray[num2], 0, 0x10);
        }
        Color[] destinationArray = new Color[length];
        Array.Copy(palette[0], palette[0].Length / 0x10, destinationArray, 0, length);
        colorArray[colorArray.Length - 1] = destinationArray;
        return colorArray;
    }

    public static NCLR.TTLP Palette_8bppTo4bpp(NCLR.TTLP palette)
    {
        int num2;
        Color[] colorArray;
        NCLR.TTLP ttlp = new NCLR.TTLP {
            ID = palette.ID,
            length = palette.length,
            unknown1 = palette.unknown1,
            nColors = 0x10,
            paletteLength = 0x20,
            depth = ColorDepth.Depth4Bit
        };
        int num = (int) (palette.nColors % 0x10);
        if (num == 0)
        {
            ttlp.palettes = new NCLR.NTFP[palette.nColors / 0x10];
            for (num2 = 0; num2 < ttlp.palettes.Length; num2++)
            {
                colorArray = new Color[0x10];
                Array.Copy(palette.palettes[0].colors, num2 * 0x10, colorArray, 0, 0x10);
                ttlp.palettes[num2].colors = colorArray;
            }
            return ttlp;
        }
        ttlp.palettes = new NCLR.NTFP[(palette.nColors / 0x10) + 1];
        for (num2 = 0; num2 < (ttlp.palettes.Length - 1); num2++)
        {
            colorArray = new Color[0x10];
            Array.Copy(palette.palettes[0].colors, num2 * 0x10, colorArray, 0, 0x10);
            ttlp.palettes[num2].colors = colorArray;
        }
        Color[] destinationArray = new Color[num];
        Array.Copy(palette.palettes[0].colors, (long) (palette.nColors / 0x10), destinationArray, 0L, (long) num);
        ttlp.palettes[ttlp.palettes.Length - 1].colors = destinationArray;
        return ttlp;
    }

    public static int Remove_DuplicatedColors(ref Color[] palette, ref byte[][] tiles)
    {
        List<Color> list = new List<Color>();
        int num = -1;
        for (int i = 0; i < palette.Length; i++)
        {
            if (!list.Contains(palette[i]))
            {
                list.Add(palette[i]);
            }
            else
            {
                int index = list.IndexOf(palette[i]);
                Replace_Color(ref tiles, i, index);
                list.Add(Color.FromArgb(0xf8, 0, 0xf8));
                if (num == -1)
                {
                    num = i;
                }
            }
        }
        palette = list.ToArray();
        return num;
    }

    public static int Remove_DuplicatedColors(ref NCLR.NTFP palette, ref byte[][] tiles)
    {
        List<Color> list = new List<Color>();
        int num = -1;
        for (int i = 0; i < palette.colors.Length; i++)
        {
            if (!list.Contains(palette.colors[i]))
            {
                list.Add(palette.colors[i]);
            }
            else
            {
                int index = list.IndexOf(palette.colors[i]);
                Replace_Color(ref tiles, i, index);
                list.Add(Color.FromArgb(0xf8, 0, 0xf8));
                if (num == -1)
                {
                    num = i;
                }
            }
        }
        palette.colors = list.ToArray();
        return num;
    }

    public static int Remove_NotUsedColors(ref Color[] palette, ref byte[][] tiles)
    {
        int num2;
        int num = -1;
        List<bool> list = new List<bool>();
        for (num2 = 0; num2 < palette.Length; num2++)
        {
            list.Add(false);
        }
        for (num2 = 0; num2 < tiles.Length; num2++)
        {
            for (int i = 0; i < tiles[num2].Length; i++)
            {
                list[tiles[num2][i]] = true;
            }
        }
        for (num2 = 0; num2 < list.Count; num2++)
        {
            if (!list[num2])
            {
                num = num2;
            }
        }
        return num;
    }

    public static int Remove_NotUsedColors(ref NCLR.NTFP palette, ref byte[][] tiles)
    {
        int num2;
        int num = -1;
        List<bool> list = new List<bool>();
        for (num2 = 0; num2 < palette.colors.Length; num2++)
        {
            list.Add(false);
        }
        for (num2 = 0; num2 < tiles.Length; num2++)
        {
            for (int i = 0; i < tiles[num2].Length; i++)
            {
                list[tiles[num2][i]] = true;
            }
        }
        for (num2 = 0; num2 < list.Count; num2++)
        {
            if (!list[num2])
            {
                num = num2;
            }
        }
        return num;
    }

    public static void Replace_Color(ref byte[][] tiles, int oldIndex, int newIndex)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                if (tiles[i][j] == oldIndex)
                {
                    tiles[i][j] = (byte) newIndex;
                }
            }
        }
    }

    public static byte[] TilesToBytes(byte[][] tiles, int startByte = 0)
    {
        List<byte> list = new List<byte>();
        for (int i = 0; i < tiles.Length; i++)
        {
            list.AddRange(tiles[i]);
        }
        byte[] destinationArray = new byte[list.Count - startByte];
        Array.Copy(list.ToArray(), startByte, destinationArray, 0, destinationArray.Length);
        return destinationArray;
    }
}

