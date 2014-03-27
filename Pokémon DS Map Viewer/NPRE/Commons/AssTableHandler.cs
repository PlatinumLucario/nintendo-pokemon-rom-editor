using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPRE.Formats.Specific.Pokémon.Scripts;
using System.IO;
using PG4Map;

namespace NPRE.Commons
{
	public class AssTableHandler
	{
        private Main main;
        private List<tableRow> actualTable;
        private int romType;

        public AssTableHandler(Main main)
        {
            this.main = main;
        }

        public List<tableRow> InitAssTable(string Title, string Code)
        {
            if (Title == "POKEMON P\0\0\0")
                return initAssTablePearl(Code);
            else if (Title == "POKEMON D\0\0\0")
                return initAssTableDiamond(Code);
            else if (Title == "POKEMON PL\0\0")
                return initAssTablePlatinum(Code);
            else if ((Title == "POKEMON HG\0\0") || (Title == "POKEMON SS\0\0"))
                return LoadMapAss(0xf6be2, 0x21b, 2);
            else if ((Title == "POKEMON W\0\0\0") || (Title == "POKEMON B\0\0\0") || (Title == "POKEMON W2\0\0") || (Title == "POKEMON B2\0\0"))
                return LoadMapAss(0x0, 0x0, 3);
            else
                return null;
        }

        private List<tableRow> initAssTablePlatinum(string Code)
        {
            if (Code == "CPUD")
                return LoadMapAss(0xe6074, 0x251, 1);
            else if (Code == "CPUE")
                return LoadMapAss(0xe601c, 0x251, 1);
            else if (Code == "CPUF")
                return LoadMapAss(0xe60a4, 0x251, 1);
            else if (Code == "CPUI")
                return LoadMapAss(0xe6038, 0x251, 1);
            else if (Code == "CPUJ")
                return LoadMapAss(0xe56f0, 0x251, 1);
            else if (Code == "CPUK")
                return LoadMapAss(0xe6aa4, 0x251, 1);
            else if (Code == "CPUS")
                return LoadMapAss(0xe60b0, 0x251, 1);
            else
                return null;
        }

        private List<tableRow> initAssTableDiamond(string Code)
        {
            if (Code == "ADAD")
                return LoadMapAss(0xeedcc, 0x22f, 0);
            else if (Code == "ADAE")
                return LoadMapAss(0xeedbc, 0x22f, 0);
            else if (Code == "ADAF")
                return LoadMapAss(0xeedfc, 0x22f, 0);
            else if (Code == "ADAI")
                return LoadMapAss(0xeed70, 0x22f, 0);
            else if (Code == "ADAJ")
                return LoadMapAss(0xf0c28, 0x22f, 0);
            else if (Code == "ADAK")
                return LoadMapAss(0xea408, 0x22f, 0);
            else if (Code == "ADAS")
                return LoadMapAss(0xeee08, 0x22f, 0);
            else
                return null;
        }

        private  List<tableRow> initAssTablePearl(string Code)
        {
            if (Code == "APAD")
                return LoadMapAss( 0xeedcc, 0x22f, 0);
            else if (Code == "APAE")
                return LoadMapAss( 0xeedbc, 0x22f, 0);
            else if (Code == "APAF")
                return LoadMapAss(0xeedfc, 0x22f, 0);
            else if (Code == "APAI")
                return LoadMapAss(0xeed70, 0x22f, 0);
            else if (Code == "APAJ")
                return LoadMapAss(0xf0c2c, 0x22f, 0);
            else if (Code == "APAK")
                return LoadMapAss(0xea408, 0x22f, 0);
            else if (Code == "APAS")
                return LoadMapAss(0xeee08, 0x22f, 0);
                        else
                return null;
        }

        private List<tableRow> LoadMapAss(int offset, int num, int type)
        {
            Stream ARM9 = getARM9(type);
            var reader = new BinaryReader(ARM9);
            var MapTex = new List<tableRow>();
            romType = type;
            if (type != 3)
                LoadMapAssDPPHGSS(MapTex, offset, num, type, reader);
            else
                LoadMapAssBW(MapTex, reader);
            return MapTex;
        }

        private void LoadMapAssBW(List<tableRow> MapTex, BinaryReader reader)
        {
            reader.BaseStream.Position = 0;
            readTableBW(MapTex, reader);
        }

        private void LoadMapAssDPPHGSS(List<tableRow> MapTex, int offset, int num, int type, BinaryReader reader)
        {

            //Temp_File = new List<ClosableMemoryStream>();
            //Temp_File.Add(mapMatrixStream);


            reader.BaseStream.Position = offset;
            if (type == 0 || type == 1)
                readTableDPP(MapTex, num, reader, null);
            else if (type == 2)
                readTableHGSS(MapTex, num, reader, null);
        }

        private short getMapNamePosition(int type)
        {
            short mapNamePosition = 0;

            if (type == 0)
                mapNamePosition = (short)main.Sys.Nodes[0].Nodes[7].Nodes[6].Nodes[0].Tag;
            else if (type == 1)
                mapNamePosition = (short)main.Sys.Nodes[0].Nodes[8].Nodes[6].Nodes[0].Tag;
            else if (type == 2)
                mapNamePosition = (short)main.Sys.Nodes[0].Nodes[3].Nodes[1].Nodes[0].Tag;
            return mapNamePosition;
        }

        private ClosableMemoryStream getStream(short position)
        {
            uint startOffset = main.actualNds.getFat().getFileStartAt(position);
            uint lenght = main.actualNds.getFat().getFileEndAt(position) - startOffset;
            var reader = new BinaryReader(main.ROM);
            reader.BaseStream.Seek((long)startOffset, SeekOrigin.Begin);
            var stream = new ClosableMemoryStream();
            Utils.loadStream(reader, lenght, stream);
            return stream;
        }

        private Stream getARM9(int type)
        {
            Stream ARM9 = new ClosableMemoryStream();
            if (type == 0 || type == 1)
                ARM9 = main.actualNds.getARM9();
            else if (type == 2)
                ARM9 = File.OpenRead("Textures/arm9.bin");
            else if (type == 3)
            {
                var tableNarc = new Narc();
                tableNarc.LoadNarc(new BinaryReader(main.actualNds.getFat().getFileStreamAt((short)main.Sys.Nodes[0].Nodes[0].Nodes[0].Nodes[1].Nodes[2].Tag)));
                ARM9 = tableNarc.figm.fileData[0];
            }
            return ARM9;
        }

        private void readTableBW(List<tableRow> MapTex, BinaryReader reader)
        {
            for (int i = 0; i < 500 && reader.BaseStream.Position < reader.BaseStream.Length; i++)
                readTableBWEntry(MapTex, reader);
        }

        private void readTableBWEntry(List<tableRow> MapTex, BinaryReader reader)
        {
            tableRow item = new tableRow
            {
                tileset = reader.ReadUInt32(),
                mapTerrainId = reader.ReadUInt16().ToString(),
                scripts = reader.ReadUInt16(),
                scriptB = reader.ReadUInt16(),
                texts = reader.ReadUInt16(),
                musicA = reader.ReadUInt16(),
                musicB = reader.ReadUInt16(),
                musicC = reader.ReadUInt16(),
                musicD = reader.ReadUInt16(),
                encount = reader.ReadUInt16(),
                mapId = reader.ReadUInt16(),
                zoneId = reader.ReadUInt16(),
                unk = reader.ReadUInt16(),
                cameraAngle = reader.ReadUInt16(),
                flag = reader.ReadUInt16(),
                cameraAngle2 = reader.ReadUInt16(),
                flytoX = reader.ReadUInt32(),
                flytoZ = reader.ReadUInt32(),
                flytoY = reader.ReadUInt32(),
                unk3 = reader.ReadUInt16()
            };
            MapTex.Add(item);
        }

        private void readTableDPP(List<tableRow> MapTex, int num, BinaryReader reader, BinaryReader reader3)
        {
            //reader.BaseStream.Position = 0;
            for (int j = 0; j < num; j++)
                readTableDPPEntry(MapTex, reader, reader3, j);
        }

        private void readTableDPPEntry(List<tableRow> MapTex, BinaryReader reader, BinaryReader reader3, int id)
        {
            tableRow item = new tableRow();
            item.headerId = id;
            item.tex_ext = reader.ReadByte();
            item.tex_int = reader.ReadByte();
            item.map_matrix = reader.ReadInt16();
            item.scripts = reader.ReadInt16();
            item.trainers = reader.ReadInt16();
            item.texts = reader.ReadInt16();
            item.musicA = reader.ReadUInt16();
            item.musicB = reader.ReadUInt16();
            item.encount = reader.ReadInt16();
            item.events = reader.ReadInt16();
            item.name = reader.ReadByte();
            reader.ReadByte();
            item.weather = reader.ReadByte();
            item.cameraAngle = reader.ReadByte();
            item.nameStyle = reader.ReadByte();
            item.flag = reader.ReadByte();
            MapTex.Add(item);
        }

        private void readTableHGSS(List<tableRow> MapTex, int num, BinaryReader reader, BinaryReader reader3)
        {
            for (int j = 0; j < num; j++)
                readTableHGSSEntry(MapTex, reader, reader3, j);
        }

        private void readTableHGSSEntry(List<tableRow> MapTex, BinaryReader reader, BinaryReader reader3, int id)
        {
            tableRow item = new tableRow();
            item.headerId = id;
            item.tex_ext = reader.ReadByte();
            item.tex_int = reader.ReadByte();
            item.map_matrix = reader.ReadInt16();
            item.scripts = reader.ReadInt16();
            item.trainers = reader.ReadInt16();
            item.texts = reader.ReadInt16();
            item.musicA = reader.ReadUInt16();
            item.musicB = reader.ReadUInt16();
            item.events = reader.ReadInt16();
            item.name = reader.ReadByte();
            item.encount = reader.ReadInt16();
            reader.ReadByte();
            item.weather = reader.ReadByte();
            item.cameraAngle = reader.ReadByte();
            item.nameStyle = reader.ReadByte();
            item.flag = reader.ReadByte();
            MapTex.Add(item);
        }
    }
}
