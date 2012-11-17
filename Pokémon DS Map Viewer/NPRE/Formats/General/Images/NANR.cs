using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

public class NANR
{
    public static NANR_s Leer(string archivo, int id)
    {
        int num;
        BinaryReader reader = new BinaryReader(File.OpenRead(archivo));
        NANR_s _s = new NANR_s {
            id = (uint) id
        };
        _s.header.id = reader.ReadChars(4);
        _s.header.endianess = reader.ReadUInt16();
        if (_s.header.endianess == 0xfffe)
        {
            _s.header.id.Reverse<char>();
        }
        _s.header.constant = reader.ReadUInt16();
        _s.header.file_size = reader.ReadUInt32();
        _s.header.header_size = reader.ReadUInt16();
        _s.header.nSection = reader.ReadUInt16();
        _s.abnk.id = reader.ReadChars(4);
        _s.abnk.length = reader.ReadUInt32();
        _s.abnk.nBanks = reader.ReadUInt16();
        _s.abnk.tFrames = reader.ReadUInt16();
        _s.abnk.constant = reader.ReadUInt32();
        _s.abnk.offset1 = reader.ReadUInt32();
        _s.abnk.offset2 = reader.ReadUInt32();
        _s.abnk.padding = reader.ReadUInt64();
        _s.abnk.anis = new Animation[_s.abnk.nBanks];
        for (num = 0; num < _s.abnk.nBanks; num++)
        {
            Animation animation;
            reader.BaseStream.Position = 0x30 + (num * 0x10);
            animation = new Animation {
                nFrames = reader.ReadUInt32(),
                dataType = reader.ReadUInt16(),
                unknown1 = reader.ReadUInt16(),
                unknown2 = reader.ReadUInt16(),
                unknown3 = reader.ReadUInt16(),
                offset_frame = reader.ReadUInt32(),
                //frames = new Frame[nFrames]
            };
            for (int i = 0; i < animation.nFrames; i++)
            {
                reader.BaseStream.Position = ((0x18 + _s.abnk.offset1) + (i * 8)) + animation.offset_frame;
                Frame frame = new Frame {
                    offset_data = reader.ReadUInt32(),
                    unknown1 = reader.ReadUInt16(),
                    constant = reader.ReadUInt16()
                };
                reader.BaseStream.Position = (0x18 + _s.abnk.offset2) + frame.offset_data;
                frame.data.nCell = reader.ReadUInt16();
                animation.frames[i] = frame;
            }
            _s.abnk.anis[num] = animation;
        }
        reader.BaseStream.Position = _s.header.header_size + _s.abnk.length;
        List<uint> list = new List<uint>();
        List<string> list2 = new List<string>();
        _s.labl.names = new string[_s.abnk.nBanks];
        _s.labl.id = reader.ReadChars(4);
        if (new string(_s.labl.id) == "LBAL")
        {
            _s.labl.section_size = reader.ReadUInt32();
            for (num = 0; num < _s.abnk.nBanks; num++)
            {
                uint item = reader.ReadUInt32();
                if (item >= (_s.labl.section_size - 8))
                {
                    Stream baseStream = reader.BaseStream;
                    baseStream.Position -= 4L;
                    break;
                }
                list.Add(item);
            }
            _s.labl.offset = list.ToArray();
            for (num = 0; num < _s.labl.offset.Length; num++)
            {
                list2.Add("");
                for (byte j = reader.ReadByte(); j != 0; j = reader.ReadByte())
                {
                    List<string> list3;
                    int num5;
                    (list3 = list2)[num5 = num] = list3[num5] + ((char) j);
                }
            }
        }
        for (num = 0; num < _s.abnk.nBanks; num++)
        {
            if (list2.Count > num)
            {
                _s.labl.names[num] = list2[num];
            }
            else
            {
                _s.labl.names[num] = num.ToString();
            }
        }
        _s.uext.id = reader.ReadChars(4);
        if (!(new string(_s.uext.id) != "TXEU"))
        {
            _s.uext.section_size = reader.ReadUInt32();
            _s.uext.unknown = reader.ReadUInt32();
        }
        reader.Close();
        return _s;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ABNK
    {
        public char[] id;
        public uint length;
        public ushort nBanks;
        public ushort tFrames;
        public uint constant;
        public uint offset1;
        public uint offset2;
        public ulong padding;
        public NANR.Animation[] anis;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Animation
    {
        public uint nFrames;
        public ushort dataType;
        public ushort unknown1;
        public ushort unknown2;
        public ushort unknown3;
        public uint offset_frame;
        public NANR.Frame[] frames;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Frame
    {
        public uint offset_data;
        public ushort unknown1;
        public ushort constant;
        public NANR.Frame_Data data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Frame_Data
    {
        public ushort nCell;
        public ushort[] transform;
        public short xDisplacement;
        public short yDisplacement;
        public ushort constant;
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
    public struct LABL
    {
        public char[] id;
        public uint section_size;
        public uint[] offset;
        public string[] names;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NANR_s
    {
        public NANR.Header header;
        public NANR.ABNK abnk;
        public NANR.LABL labl;
        public NANR.UEXT uext;
        public object other;
        public uint id;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct UEXT
    {
        public char[] id;
        public uint section_size;
        public uint unknown;
    }
}

