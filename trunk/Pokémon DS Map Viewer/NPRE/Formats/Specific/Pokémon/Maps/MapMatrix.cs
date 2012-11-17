using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace NPRE.Formats.Specific.Pokémon.Maps
{
    public partial class MapMatrix : Form
    {

        public  MapMat_s MapMat;
        private ClosableMemoryStream actualNode;

        public MapMatrix()
        {
            InitializeComponent();
        }

        public MapMatrix(ClosableMemoryStream actualNode, int type)
        {
            InitializeComponent();
            this.actualNode = actualNode;
            if (type == 0)
                readMatrix(actualNode);
            else
                readMatrixBW(actualNode);
            showMatrix(MatrixHeaderInfo, MatrixBorderInfo,MatrixTerrainInfo);
        }

        private void readMatrixBW(ClosableMemoryStream actualNode)
        {
            BinaryReader reader = new BinaryReader(actualNode);
            reader.BaseStream.Position = 4;
            MapMat.Width = reader.ReadUInt16();
            MapMat.Height = reader.ReadUInt16();
            MapMat.Headers = new List<int>();
            for (int i = 0; i < MapMat.Width * MapMat.Height; i++)
            {
                int item = reader.ReadInt32();
                MapMat.Headers.Add(item);
            }
            if (reader.BaseStream.Position + 1 < reader.BaseStream.Length)
            {
                MapMat.Borders = new List<int>();
                for (int i = 0; i < MapMat.Width * MapMat.Height; i++)
                {
                    int item = reader.ReadInt32();
                    MapMat.Borders.Add(item);
                }
                //MapMat.Terrain = new List<int>();
                //for (int i = 0; i < MapMat.Width * MapMat.Height; i++)
                //{
                //    short item = reader.ReadInt16();
                //    MapMat.Terrain.Add(item);
                //}
            }
        }

        public void showMatrix(DataGridView MatrixHeaderInfo, DataGridView MatrixBorderInfo, DataGridView MatrixTerrainInfo)
        {
            initMatrix(MatrixHeaderInfo, MatrixBorderInfo, MatrixTerrainInfo);

            int dataCounter = 0;
            for (int rowCounter = 0; rowCounter < MapMat.Height; rowCounter++)
            {
                for (int columnCounter = 0; columnCounter < MapMat.Width; columnCounter++)
                {
                  MatrixHeaderInfo.Rows[rowCounter].Cells[columnCounter].Value = MapMat.Headers[dataCounter];
                  dataCounter++;
                }
            }
            dataCounter = 0;
            if (MapMat.Borders != null)
            {
                for (int rowCounter = 0; rowCounter < MapMat.Height; rowCounter++)
                {
                    for (int columnCounter = 0; columnCounter < MapMat.Width; columnCounter++)
                    {
                        MatrixBorderInfo.Rows[rowCounter].Cells[columnCounter].Value = MapMat.Borders[dataCounter];
                        dataCounter++;
                    }
                }
            }
            dataCounter = 0;
            if (MapMat.Terrain != null)
            {
                for (int rowCounter = 0; rowCounter < MapMat.Height; rowCounter++)
                {
                    for (int columnCounter = 0; columnCounter < MapMat.Width; columnCounter++)
                    {
                        MatrixTerrainInfo.Rows[rowCounter].Cells[columnCounter].Value = MapMat.Terrain[dataCounter];
                        dataCounter++;
                    }
                }
            }
        }

        private void initMatrix(DataGridView MatrixHeaderInfo, DataGridView MatrixBorderInfo, DataGridView MatrixTerrainInfo)
        {
            for (int dataCounter = 0; dataCounter < Math.Max(MapMat.Width, MapMat.Height); dataCounter++)
            {
                MatrixHeaderInfo.Columns.Add(dataCounter.ToString(), dataCounter.ToString());
                MatrixHeaderInfo.Rows.Add();
                MatrixBorderInfo.Columns.Add(dataCounter.ToString(), dataCounter.ToString());
                MatrixBorderInfo.Rows.Add();
                MatrixTerrainInfo.Columns.Add(dataCounter.ToString(), dataCounter.ToString());
                MatrixTerrainInfo.Rows.Add();
            }


        }

        public void readMatrix(MemoryStream file)
        {
            BinaryReader reader = new BinaryReader(file);
            reader.BaseStream.Seek(0L, SeekOrigin.Begin);
            MapMat.Width = reader.ReadByte();
            MapMat.Height = reader.ReadByte();
            MapMat.Unk = reader.ReadInt16();
            MapMat.Name_Size = reader.ReadByte();
            MapMat.Name = Encoding.UTF8.GetString(reader.ReadBytes(MapMat.Name_Size));
            MapMat.Headers = new List<int>();
            for (int i = 0; i < MapMat.Width * MapMat.Height; i++)
            {
                short item = reader.ReadInt16();
                MapMat.Headers.Add(item);
            }
            if (reader.BaseStream.Position + 1 < reader.BaseStream.Length)
            {
                MapMat.Borders = new List<int>();
                for (int i = 0; i < MapMat.Width * MapMat.Height; i++)
                {
                    var item = reader.ReadByte();
                    MapMat.Borders.Add(item);
                }
                MapMat.Terrain = new List<int>();
                for (int i = 0; i < MapMat.Width * MapMat.Height; i++)
                {
                    short item = reader.ReadInt16();
                    MapMat.Terrain.Add(item);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MapMat_s
        {
            public uint Height;
            public uint Width;
            public byte Name_Size;
            public string Name;
            public int Unk;
            public List<int> Headers;
            public List<int> Borders;
            public List<int> Terrain;
        }
    }
}
