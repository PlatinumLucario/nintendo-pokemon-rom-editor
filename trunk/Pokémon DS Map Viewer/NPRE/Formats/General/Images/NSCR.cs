using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

public class NSCR
{
    public static NSCR_s Create_BasicMap(int nTiles, int width, int height, int startTile = 0, byte palette = 0)
    {
        NSCR_s _s = new NSCR_s();
        _s.header.id = "RCSN".ToCharArray();
        _s.header.endianess = 0xfeff;
        _s.header.constant = 0x100;
        _s.header.header_size = 0x10;
        _s.header.nSection = 1;
        _s.section.id = "NRCS".ToCharArray();
        _s.section.width = (ushort) width;
        _s.section.height = (ushort) height;
        _s.section.padding = 0;
        _s.section.data_size = (uint) (nTiles * 2);
        _s.section.mapData = new NTFS[nTiles];
        for (int i = 0; i < nTiles; i++)
        {
            _s.section.mapData[i] = new NTFS();
            _s.section.mapData[i].nPalette = palette;
            _s.section.mapData[i].yFlip = 0;
            _s.section.mapData[i].xFlip = 0;
            _s.section.mapData[i].nTile = (ushort) (i + startTile);
        }
        _s.section.section_size = _s.section.data_size + 20;
        _s.header.file_size = _s.section.section_size + _s.header.header_size;
        return _s;
    }

    public static NSCR_s Create_BasicMap(int width, int height, int startFillTile, int fillTile, int startTile = 0, byte palette = 0)
    {
        NSCR_s _s = new NSCR_s();
        int num = (width * height) / 0x40;
        _s.header.id = "RCSN".ToCharArray();
        _s.header.endianess = 0xfeff;
        _s.header.constant = 0x100;
        _s.header.header_size = 0x10;
        _s.header.nSection = 1;
        _s.section.id = "NRCS".ToCharArray();
        _s.section.width = (ushort) width;
        _s.section.height = (ushort) height;
        _s.section.padding = 0;
        _s.section.data_size = (uint) (num * 2);
        _s.section.mapData = new NTFS[num];
        for (int i = 0; i < num; i++)
        {
            _s.section.mapData[i] = new NTFS();
            _s.section.mapData[i].nPalette = palette;
            _s.section.mapData[i].yFlip = 0;
            _s.section.mapData[i].xFlip = 0;
            if (i >= startFillTile)
            {
                _s.section.mapData[i].nTile = (ushort) fillTile;
            }
            else
            {
                _s.section.mapData[i].nTile = (ushort) (i + startTile);
            }
        }
        _s.section.section_size = _s.section.data_size + 20;
        _s.header.file_size = _s.section.section_size + _s.header.header_size;
        return _s;
    }

    public static NSCR_s Read(BinaryReader br, int id)
    {
        NSCR_s _s = new NSCR_s {
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
        _s.section.id = br.ReadChars(4);
        _s.section.section_size = br.ReadUInt32();
        _s.section.width = br.ReadUInt16();
        _s.section.height = br.ReadUInt16();
        _s.section.padding = br.ReadUInt32();
        _s.section.data_size = br.ReadUInt32();
        _s.section.mapData = new NTFS[_s.section.data_size / 2];
        for (int i = 0; i < (_s.section.data_size / 2); i++)
        {
            ushort num2 = br.ReadUInt16();
            _s.section.mapData[i] = new NTFS();
            _s.section.mapData[i].nTile = (ushort) (num2 & 0x3ff);
            _s.section.mapData[i].xFlip = (byte) ((num2 >> 10) & 1);
            _s.section.mapData[i].yFlip = (byte) ((num2 >> 11) & 1);
            _s.section.mapData[i].nPalette = (byte) ((num2 >> 12) & 15);
        }
        br.Close();
        return _s;
    }

    public static NSCR_s Read_Basic(string archivo, int id)
    {
        BinaryReader reader = new BinaryReader(File.OpenRead(archivo));
        uint length = (uint) new FileInfo(archivo).Length;
        NSCR_s _s = new NSCR_s {
            id = (uint) id
        };
        _s.header.id = "UNKN".ToCharArray();
        _s.header.endianess = 0xfeff;
        _s.header.constant = 0x100;
        _s.header.file_size = length;
        _s.header.header_size = 0x10;
        _s.header.nSection = 1;
        _s.section.id = "UNKN".ToCharArray();
        _s.section.section_size = length;
        _s.section.width = 0x100;
        _s.section.height = 0xc0;
        _s.section.padding = 0;
        _s.section.data_size = length;
        _s.section.mapData = new NTFS[length / 2];
        for (int i = 0; i < (length / 2); i++)
        {
            ushort num3 = reader.ReadUInt16();
            _s.section.mapData[i] = new NTFS();
            _s.section.mapData[i].nTile = (ushort) (num3 & 0x3ff);
            _s.section.mapData[i].xFlip = (byte) ((num3 >> 10) & 1);
            _s.section.mapData[i].yFlip = (byte) ((num3 >> 11) & 1);
            _s.section.mapData[i].nPalette = (byte) ((num3 >> 12) & 15);
        }
        reader.Close();
        return _s;
    }

    public static NTFT Transform_Tile(NSCR_s nscr, NTFT tiles, int startInfo = 0)
    {
        NTFT ntft = new NTFT();
        List<byte[]> list = new List<byte[]>();
        List<byte> list2 = new List<byte>();
        for (int i = startInfo; i < nscr.section.mapData.Length; i++)
        {
            if (nscr.section.mapData[i].nTile >= tiles.tiles.Length)
            {
                nscr.section.mapData[i].nTile = 0;
            }
            byte[] tile = tiles.tiles[nscr.section.mapData[i].nTile];
            if (nscr.section.mapData[i].xFlip == 1)
            {
                tile = XFlip(tile);
            }
            if (nscr.section.mapData[i].yFlip == 1)
            {
                tile = YFlip(tile);
            }
            list.Add(tile);
            list2.Add(nscr.section.mapData[i].nPalette);
        }
        ntft.nPalette = list2.ToArray();
        ntft.tiles = list.ToArray();
        return ntft;
    }

    public static void Write(NSCR_s map, string fileout)
    {
        BinaryWriter writer = new BinaryWriter(File.OpenWrite(fileout));
        writer.Write(map.header.id);
        writer.Write(map.header.endianess);
        writer.Write(map.header.constant);
        writer.Write(map.header.file_size);
        writer.Write(map.header.header_size);
        writer.Write(map.header.nSection);
        writer.Write(map.section.id);
        writer.Write(map.section.section_size);
        writer.Write(map.section.width);
        writer.Write(map.section.height);
        writer.Write(map.section.padding);
        writer.Write(map.section.data_size);
        for (int i = 0; i < map.section.mapData.Length; i++)
        {
            int num2 = map.section.mapData[i].nPalette << 12;
            int num3 = map.section.mapData[i].yFlip << 11;
            int num4 = map.section.mapData[i].xFlip << 10;
            int num5 = ((num2 + num3) + num4) + map.section.mapData[i].nTile;
            writer.Write((ushort) num5);
        }
        writer.Flush();
        writer.Close();
    }

    public static byte[] XFlip(byte[] tile)
    {
        byte[] buffer = new byte[tile.Length];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                buffer[j + (i * 8)] = tile[(7 - j) + (i * 8)];
                buffer[(7 - j) + (i * 8)] = tile[j + (i * 8)];
            }
        }
        return buffer;
    }

    public static byte[] YFlip(byte[] tile)
    {
        byte[] buffer = new byte[tile.Length];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                buffer[j + (i * 8)] = tile[j + ((7 - i) * 8)];
                buffer[j + ((7 - i) * 8)] = tile[j + (i * 8)];
            }
        }
        return buffer;
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
    public struct NSCR_s
    {
        public NSCR.Header header;
        public NSCR.NSCR_Section section;
        public object other;
        public uint id;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NSCR_Section
    {
        public char[] id;
        public uint section_size;
        public ushort width;
        public ushort height;
        public uint padding;
        public uint data_size;
        public NSCR.NTFS[] mapData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NTFS
    {
        public byte nPalette;
        public byte xFlip;
        public byte yFlip;
        public ushort nTile;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NTFT
    {
        public byte[][] tiles;
        public byte[] nPalette;
    }
}

