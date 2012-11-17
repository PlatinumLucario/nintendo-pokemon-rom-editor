using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class NCGR
{
    public static NCGR_s BitmapToTile(string bitmap, TileOrder tileOrder)
    {
        int num7;
        int num8;
        int num9;
        int num10;
        NCGR_s _s = new NCGR_s();
        BinaryReader reader = new BinaryReader(File.OpenRead(bitmap));
        if (new string(reader.ReadChars(2)) != "BM")
        {
            _s.header.id = "RGCN".ToCharArray();
        }
        _s.header.endianess = 0xfeff;
        _s.header.constant = 1;
        _s.header.header_size = 0x10;
        _s.header.nSection = 1;
        reader.BaseStream.Position = 10L;
        uint num = reader.ReadUInt32();
        Stream baseStream = reader.BaseStream;
        baseStream.Position += 4L;
        uint num2 = reader.ReadUInt32();
        uint num3 = reader.ReadUInt32();
        _s.rahc.nTilesX = (ushort) num2;
        _s.rahc.nTilesY = (ushort) num3;
        if (tileOrder == TileOrder.Horizontal)
        {
            _s.rahc.nTilesX = (ushort) (_s.rahc.nTilesX / 8);
            _s.rahc.nTilesY = (ushort) (_s.rahc.nTilesY / 8);
        }
        _s.rahc.nTiles = (ushort) (_s.rahc.nTilesX * _s.rahc.nTilesY);
        Stream stream2 = reader.BaseStream;
        stream2.Position += 2L;
        switch (reader.ReadUInt16())
        {
            case 4:
                _s.rahc.depth = ColorDepth.Depth4Bit;
                break;

            case 8:
                _s.rahc.depth = ColorDepth.Depth8Bit;
                break;
        }
        uint num5 = reader.ReadUInt32();
        uint num6 = reader.ReadUInt32();
        _s.rahc.tileData.tiles = new byte[1][];
        ColorDepth depth = _s.rahc.depth;
        if (depth == ColorDepth.Depth4Bit)
        {
            _s.rahc.tileData.tiles[0] = new byte[(num2 * num3) * 2];
            _s.rahc.tileData.nPalette = new byte[(num2 * num3) * 2];
            num7 = (int) (num2 / 2);
            if ((num2 % 4) != 0)
            {
                Math.DivRem((int) (num2 / 2), 4, out num8);
                num7 = ((int) (num2 / 2)) + (4 - num8);
            }
            reader.BaseStream.Position = num;
            for (num9 = ((int) num3) - 1; num9 >= 0; num9--)
            {
                num10 = 0;
                while (num10 < num2)
                {
                    string str = string.Format("{0:X}", reader.ReadByte());
                    if (str.Length == 1)
                    {
                        str = '0' + str;
                    }
                    char ch = str[0];
                    _s.rahc.tileData.tiles[0][(int) ((IntPtr) (num10 + (num9 * num2)))] = Convert.ToByte(ch.ToString(), 0x10);
                    _s.rahc.tileData.nPalette[(int) ((IntPtr) (num10 + (num9 * num2)))] = 0;
                    if ((num10 + 1) != num2)
                    {
                        _s.rahc.tileData.tiles[0][(int) ((IntPtr) ((num10 + 1) + (num9 * num2)))] = Convert.ToByte(str[1].ToString(), 0x10);
                        _s.rahc.tileData.nPalette[(int) ((IntPtr) ((num10 + 1) + (num9 * num2)))] = 0;
                    }
                    num10 += 2;
                }
                reader.ReadBytes(num7 - ((int) (((float) num2) / 2f)));
            }
        }
        else if (depth == ColorDepth.Depth8Bit)
        {
            _s.rahc.tileData.tiles[0] = new byte[num2 * num3];
            _s.rahc.tileData.nPalette = new byte[num2 * num3];
            num7 = (int) num2;
            if ((num2 % 4) != 0)
            {
                Math.DivRem((int) num2, 4, out num8);
                num7 = ((int) num2) + (4 - num8);
            }
            reader.BaseStream.Position = num;
            for (num9 = ((int) num3) - 1; num9 >= 0; num9--)
            {
                for (num10 = 0; num10 < num2; num10++)
                {
                    _s.rahc.tileData.tiles[0][(int) ((IntPtr) (num10 + (num9 * num2)))] = reader.ReadByte();
                    _s.rahc.tileData.nPalette[(int) ((IntPtr) (num10 + (num9 * num2)))] = 0;
                }
                reader.ReadBytes(num7 - ((int) num2));
            }
        }
        if (tileOrder == TileOrder.Horizontal)
        {
            _s.rahc.tileData.tiles = Convertir.BytesToTiles_NoChanged(_s.rahc.tileData.tiles[0], _s.rahc.nTilesX, _s.rahc.nTilesY);
        }
        _s.rahc.id = "RAHC".ToCharArray();
        _s.rahc.size_tiledata = (uint) _s.rahc.tileData.nPalette.Length;
        _s.rahc.tiledFlag = (uint)((tileOrder == TileOrder.NoTiled) ? 1 : 0);
        _s.rahc.unknown1 = 0;
        _s.rahc.unknown2 = 0;
        _s.rahc.unknown3 = 0x18;
        _s.rahc.size_section = _s.rahc.size_tiledata + 0x20;
        _s.header.file_size = _s.rahc.size_section + _s.header.header_size;
        _s.order = tileOrder;
        reader.Close();
        return _s;
    }

    public static Bitmap Get_Image(NCGR_s tile, NCLR.NCLR_s paleta, int zoom = 1)
    {
        if (tile.rahc.nTilesX == 0xffff)
        {
            if (tile.order == TileOrder.NoTiled)
            {
                tile.rahc.nTilesX = 0x40;
            }
            else
            {
                tile.rahc.nTilesX = 8;
            }
        }
        if (tile.rahc.nTilesY == 0xffff)
        {
            if (tile.order == TileOrder.NoTiled)
            {
                if (tile.rahc.nTiles >= 0x40)
                {
                    tile.rahc.nTilesY = (ushort) ((tile.rahc.nTiles / 0x40) * 0x40);
                }
                else
                {
                    tile.rahc.nTilesY = 0x40;
                }
            }
            else if (tile.rahc.nTiles >= 0x40)
            {
                tile.rahc.nTilesY = (ushort) (tile.rahc.nTiles / 8);
            }
            else
            {
                tile.rahc.nTilesY = 8;
            }
        }
        switch (tile.order)
        {
            case TileOrder.NoTiled:
                return No_Tile(tile, paleta, 0, tile.rahc.nTilesX, tile.rahc.nTilesY, zoom);

            case TileOrder.Horizontal:
                return Horizontal(tile, paleta, 0, tile.rahc.nTilesX, tile.rahc.nTilesY, zoom);

            case TileOrder.Vertical:
                throw new NotImplementedException();
        }
        return new Bitmap(0, 0);
    }

    public static Bitmap Get_Image(NCGR_s tile, NCLR.NCLR_s paleta, int startTile, int zoom = 1)
    {
        if (tile.rahc.nTilesX == 0xffff)
        {
            if (tile.order == TileOrder.NoTiled)
            {
                tile.rahc.nTilesX = 0x40;
            }
            else
            {
                tile.rahc.nTilesX = 8;
            }
        }
        if (tile.rahc.nTilesY == 0xffff)
        {
            if (tile.order == TileOrder.NoTiled)
            {
                if (tile.rahc.nTiles >= 0x40)
                {
                    tile.rahc.nTilesY = (ushort) ((tile.rahc.nTiles / 0x40) * 0x40);
                }
                else
                {
                    tile.rahc.nTilesY = 0x40;
                }
            }
            else if (tile.rahc.nTiles >= 0x40)
            {
                tile.rahc.nTilesY = (ushort) (tile.rahc.nTiles / 8);
            }
            else
            {
                tile.rahc.nTilesY = 8;
            }
        }
        switch (tile.order)
        {
            case TileOrder.NoTiled:
                return No_Tile(tile, paleta, startTile, tile.rahc.nTilesX, tile.rahc.nTilesY, zoom);

            case TileOrder.Horizontal:
                return Horizontal(tile, paleta, startTile, tile.rahc.nTilesX, tile.rahc.nTilesY, zoom);

            case TileOrder.Vertical:
                throw new NotImplementedException();
        }
        return new Bitmap(1, 1);
    }

    public static Bitmap Get_Image(NCGR_s tile, NCLR.NCLR_s paleta, int startTile, int tilesX, int tilesY, int zoom = 1)
    {
        switch (tile.order)
        {
            case TileOrder.NoTiled:
                return No_Tile(tile, paleta, startTile, tilesX, tilesY, zoom);

            case TileOrder.Horizontal:
                return Horizontal(tile, paleta, startTile, tilesX, tilesY, zoom);

            case TileOrder.Vertical:
                throw new NotImplementedException();
        }
        return new Bitmap(0, 0);
    }

    public static Bitmap Get_Image(NTFT tile, Color[][] palette, TileOrder tileOrder, int startTile, int tilesX, int tilesY, int zoom = 1)
    {
        switch (tileOrder)
        {
            case TileOrder.NoTiled:
                return No_Tile(tile, palette, startTile, tilesX, tilesY, zoom);

            case TileOrder.Horizontal:
                return Horizontal(tile, palette, startTile, tilesX, tilesY, zoom);

            case TileOrder.Vertical:
                throw new NotImplementedException();
        }
        return new Bitmap(0, 0);
    }

    private static Bitmap Horizontal(NCGR_s tile, NCLR.NCLR_s paleta, int startTile, int tilesX, int tilesY, int zoom = 1)
    {
        if (zoom <= 0)
        {
            zoom = 1;
        }
        Bitmap bitmap = new Bitmap((tilesX * 8) * zoom, (tilesY * 8) * zoom);
        tile.rahc.tileData.tiles = Convertir.BytesToTiles(Convertir.TilesToBytes(tile.rahc.tileData.tiles, startTile));
        startTile = 0;
        for (int i = 0; i < tilesY; i++)
        {
            for (int j = 0; j < tilesX; j++)
            {
                for (int k = 0; k < 8; k++)
                {
                    for (int m = 0; m < 8; m++)
                    {
                        for (int n = 0; n < zoom; n++)
                        {
                            for (int num6 = 0; num6 < zoom; num6++)
                            {
                                try
                                {
                                    if (tile.rahc.tileData.tiles[j + (i * tilesX)].Length == 0)
                                    {
                                        return bitmap;
                                    }
                                    bitmap.SetPixel(((m + (j * 8)) * zoom) + num6, ((k + (i * 8)) * zoom) + n, paleta.pltt.palettes[tile.rahc.tileData.nPalette[startTile]].colors[tile.rahc.tileData.tiles[startTile][m + (k * 8)]]);
                                }
                                catch
                                {
                                    return bitmap;
                                }
                            }
                        }
                    }
                }
                startTile++;
            }
        }
        return bitmap;
    }

    private static Bitmap Horizontal(NTFT tile, Color[][] palette, int startTile, int tilesX, int tilesY, int zoom = 1)
    {
        if (zoom <= 0)
        {
            zoom = 1;
        }
        Bitmap bitmap = new Bitmap((tilesX * 8) * zoom, (tilesY * 8) * zoom);
        tile.tiles = Convertir.BytesToTiles(Convertir.TilesToBytes(tile.tiles, startTile));
        startTile = 0;
        for (int i = 0; i < tilesY; i++)
        {
            for (int j = 0; j < tilesX; j++)
            {
                for (int k = 0; k < 8; k++)
                {
                    for (int m = 0; m < 8; m++)
                    {
                        for (int n = 0; n < zoom; n++)
                        {
                            for (int num6 = 0; num6 < zoom; num6++)
                            {
                                try
                                {
                                    if (tile.tiles[j + (i * tilesX)].Length == 0)
                                    {
                                        return bitmap;
                                    }
                                    bitmap.SetPixel(((m + (j * 8)) * zoom) + num6, ((k + (i * 8)) * zoom) + n, palette[tile.nPalette[startTile]][tile.tiles[startTile][m + (k * 8)]]);
                                }
                                catch
                                {
                                    return bitmap;
                                }
                            }
                        }
                    }
                }
                startTile++;
            }
        }
        return bitmap;
    }

    public static byte[][] MergeImage(byte[][] originalTile, byte[][] newTiles, int startTile)
    {
        int num;
        List<byte[]> list = new List<byte[]>();
        for (num = 0; num < startTile; num++)
        {
            if (num >= originalTile.Length)
            {
                byte[] item = new byte[0x40];
                for (int i = 0; i < 0x40; i++)
                {
                    item[i] = 0;
                }
                list.Add(item);
            }
            else
            {
                list.Add(originalTile[num]);
            }
        }
        list.AddRange(newTiles);
        for (num = startTile + newTiles.Length; num < originalTile.Length; num++)
        {
            list.Add(originalTile[num]);
        }
        return list.ToArray();
    }

    private static Bitmap No_Tile(NCGR_s tile, NCLR.NCLR_s palette, int salto, int width, int height, int zoom = 1)
    {
        if (zoom <= 0)
        {
            zoom = 1;
        }
        Bitmap bitmap = new Bitmap(width * zoom, height * zoom);
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                for (int k = 0; k < zoom; k++)
                {
                    for (int m = 0; m < zoom; m++)
                    {
                        try
                        {
                            if (tile.rahc.tileData.tiles[0].Length == 0)
                            {
                                return bitmap;
                            }
                            bitmap.SetPixel((j * zoom) + m, (i * zoom) + k, palette.pltt.palettes[tile.rahc.tileData.nPalette[0]].colors[tile.rahc.tileData.tiles[0][(j + (i * width)) + salto]]);
                        }
                        catch
                        {
                            return bitmap;
                        }
                    }
                }
            }
        }
        return bitmap;
    }

    private static Bitmap No_Tile(NTFT tile, Color[][] palette, int salto, int width, int height, int zoom = 1)
    {
        if (zoom <= 0)
        {
            zoom = 1;
        }
        Bitmap bitmap = new Bitmap(width * zoom, height * zoom);
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                for (int k = 0; k < zoom; k++)
                {
                    for (int m = 0; m < zoom; m++)
                    {
                        try
                        {
                            if (tile.tiles[0].Length == 0)
                            {
                                return bitmap;
                            }
                            if (palette[tile.nPalette[0]].Length <= tile.tiles[0][(j + (i * width)) + salto])
                            {
                                return bitmap;
                            }
                            Color color = palette[tile.nPalette[0]][tile.tiles[0][(j + (i * width)) + salto]];
                            bitmap.SetPixel((j * zoom) + m, (i * zoom) + k, color);
                        }
                        catch
                        {
                            return bitmap;
                        }
                    }
                }
            }
        }
        return bitmap;
    }

    public static NCGR_s Read(BinaryReader br, int id)
    {
        NCGR_s _s = new NCGR_s {
            id = (uint) id
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
        _s.rahc.id = br.ReadChars(4);
        _s.rahc.size_section = br.ReadUInt32();
        _s.rahc.nTilesY = br.ReadUInt16();
        _s.rahc.nTilesX = br.ReadUInt16();
        _s.rahc.depth = (br.ReadUInt32() == 3) ? ColorDepth.Depth4Bit : ColorDepth.Depth8Bit;
        _s.rahc.unknown1 = br.ReadUInt16();
        _s.rahc.unknown2 = br.ReadUInt16();
        _s.rahc.tiledFlag = br.ReadUInt32();
        if ((_s.rahc.tiledFlag & 0xff) == 0)
        {
            _s.order = TileOrder.Horizontal;
        }
        else
        {
            _s.order = TileOrder.NoTiled;
            if (_s.rahc.nTilesX != 0xffff)
            {
                _s.rahc.nTilesX = (ushort) (_s.rahc.nTilesX * 8);
                _s.rahc.nTilesY = (ushort) (_s.rahc.nTilesY * 8);
            }
        }
        _s.rahc.size_tiledata = (_s.rahc.depth == ColorDepth.Depth8Bit) ? br.ReadUInt32() : (br.ReadUInt32() * 2);
        _s.rahc.unknown3 = br.ReadUInt32();
        if (_s.rahc.size_tiledata != 0)
        {
            _s.rahc.nTiles = (ushort) (_s.rahc.size_tiledata / 0x40);
        }
        else
        {
            _s.rahc.nTiles = (ushort) (_s.rahc.nTilesX * _s.rahc.nTilesY);
        }
        if (_s.order == TileOrder.Horizontal)
        {
            _s.rahc.tileData.tiles = new byte[_s.rahc.nTiles][];
        }
        else
        {
            _s.rahc.tileData.tiles = new byte[1][];
        }
        List<byte> list = new List<byte>();
        _s.rahc.tileData.nPalette = new byte[_s.rahc.nTiles];
        for (int i = 0; i < _s.rahc.nTiles; i++)
        {
            byte[] buffer;
            byte[] buffer2;
            int num2;
            if (_s.order == TileOrder.Horizontal)
            {
                if (_s.rahc.depth == ColorDepth.Depth4Bit)
                {
                    buffer = br.ReadBytes(0x20);
                    buffer2 = new byte[buffer.Length * 2];
                    num2 = 0;
                    while (num2 < buffer.Length)
                    {
                        buffer2[num2 * 2] = (byte) (buffer[num2] & 15);
                        buffer2[(num2 * 2) + 1] = (byte) ((buffer[num2] & 240) >> 4);
                        num2++;
                    }
                    _s.rahc.tileData.tiles[i] = buffer2;
                }
                else
                {
                    _s.rahc.tileData.tiles[i] = br.ReadBytes(0x40);
                }
            }
            else if (_s.rahc.depth == ColorDepth.Depth4Bit)
            {
                buffer = br.ReadBytes(0x20);
                buffer2 = new byte[buffer.Length * 2];
                for (num2 = 0; num2 < buffer.Length; num2++)
                {
                    buffer2[num2 * 2] = (byte) (buffer[num2] & 15);
                    buffer2[(num2 * 2) + 1] = (byte) ((buffer[num2] & 240) >> 4);
                }
                list.AddRange(buffer2);
            }
            else
            {
                list.AddRange(br.ReadBytes(0x40));
            }
            _s.rahc.tileData.nPalette[i] = 0;
        }
        if (_s.order == TileOrder.NoTiled)
        {
            _s.rahc.tileData.tiles[0] = list.ToArray();
        }
        if ((_s.header.nSection == 1) || (br.BaseStream.Position == br.BaseStream.Length))
        {
            br.Close();
            return _s;
        }
        _s.sopc.id = br.ReadChars(4);
        _s.sopc.size_section = br.ReadUInt32();
        _s.sopc.unknown1 = br.ReadUInt32();
        _s.sopc.charSize = br.ReadUInt16();
        _s.sopc.nChar = br.ReadUInt16();
        br.Close();
        return _s;
    }

    public static void Write(NCGR_s tile, string fileout)
    {
        BinaryWriter writer = new BinaryWriter(File.OpenWrite(fileout));
        writer.Write(tile.header.id);
        writer.Write(tile.header.endianess);
        writer.Write(tile.header.constant);
        writer.Write(tile.header.file_size);
        writer.Write(tile.header.header_size);
        writer.Write(tile.header.nSection);
        writer.Write(tile.rahc.id);
        writer.Write(tile.rahc.size_section);
        if (tile.order == TileOrder.Horizontal)
        {
            writer.Write(tile.rahc.nTilesY);
            writer.Write(tile.rahc.nTilesX);
        }
        else
        {
            writer.Write((ushort) (tile.rahc.nTilesY / 8));
            writer.Write((ushort) (tile.rahc.nTilesX / 8));
        }
        writer.Write((tile.rahc.depth == ColorDepth.Depth4Bit) ? 3 : 4);
        writer.Write(tile.rahc.unknown1);
        writer.Write(tile.rahc.unknown2);
        writer.Write(tile.rahc.tiledFlag);
        writer.Write((tile.rahc.depth == ColorDepth.Depth4Bit) ? (tile.rahc.size_tiledata / 2) : tile.rahc.size_tiledata);
        writer.Write(tile.rahc.unknown3);
        for (int i = 0; i < tile.rahc.tileData.tiles.Length; i++)
        {
            if (tile.rahc.depth == ColorDepth.Depth4Bit)
            {
                writer.Write(Convertir.Bit4ToBit8(tile.rahc.tileData.tiles[i]));
            }
            else
            {
                writer.Write(tile.rahc.tileData.tiles[i]);
            }
        }
        if (tile.header.nSection == 2)
        {
            writer.Write(tile.sopc.id);
            writer.Write(tile.sopc.size_section);
            writer.Write(tile.sopc.unknown1);
            writer.Write(tile.sopc.charSize);
            writer.Write(tile.sopc.nChar);
        }
        writer.Flush();
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
    public struct NCGR_s
    {
        public NCGR.Header header;
        public NCGR.RAHC rahc;
        public NCGR.SOPC sopc;
        public NCGR.TileOrder order;
        public object other;
        public uint id;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NTFT
    {
        public byte[][] tiles;
        public byte[] nPalette;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RAHC
    {
        public char[] id;
        public uint size_section;
        public ushort nTilesY;
        public ushort nTilesX;
        public ColorDepth depth;
        public ushort unknown1;
        public ushort unknown2;
        public uint tiledFlag;
        public uint size_tiledata;
        public uint unknown3;
        public NCGR.NTFT tileData;
        public uint nTiles;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SOPC
    {
        public char[] id;
        public uint size_section;
        public uint unknown1;
        public ushort charSize;
        public ushort nChar;
    }

    public enum TileOrder
    {
        NoTiled,
        Horizontal,
        Vertical
    }
}

