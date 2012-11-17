using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class NCER
{
    public static Size Calculate_Size(byte shape, byte size)
    {
        Size size2 = new Size();
        switch (shape)
        {
            case 0:
                switch (size)
                {
                    case 0:
                        return new Size(8, 8);

                    case 1:
                        return new Size(0x10, 0x10);

                    case 2:
                        return new Size(0x20, 0x20);

                    case 3:
                        return new Size(0x40, 0x40);
                }
                return size2;

            case 1:
                switch (size)
                {
                    case 0:
                        return new Size(0x10, 8);

                    case 1:
                        return new Size(0x20, 8);

                    case 2:
                        return new Size(0x20, 0x10);

                    case 3:
                        return new Size(0x40, 0x20);
                }
                return size2;

            case 2:
                switch (size)
                {
                    case 0:
                        return new Size(8, 0x10);

                    case 1:
                        return new Size(8, 0x20);

                    case 2:
                        return new Size(0x10, 0x20);

                    case 3:
                        return new Size(0x20, 0x40);
                }
                return size2;
        }
        return size2;
    }

    public static byte[][] Change_ColorCell(Cell cell, uint blockSize, NCGR.NCGR_s image, int oldIndex, int newIndex)
    {
        uint num;
        int num2;
        List<byte[]> list = new List<byte[]>();
        List<byte> list2 = new List<byte>();
        if (image.order == NCGR.TileOrder.Horizontal)
        {
            num = (image.rahc.depth == ColorDepth.Depth4Bit) ? (cell.obj2.tileOffset * 2) : cell.obj2.tileOffset;
            num *= (blockSize != 0) ? blockSize : 1;
            for (num2 = 0; num2 < image.rahc.tileData.tiles.Length; num2++)
            {
                if ((num2 >= num) && (num2 < (((int) num) + ((cell.width * cell.height) / 0x40))))
                {
                    byte[] item = new byte[0x40];
                    for (int i = 0; i < 0x40; i++)
                    {
                        if (image.rahc.tileData.tiles[num2][i] == oldIndex)
                        {
                            item[i] = (byte) newIndex;
                        }
                        else if (image.rahc.tileData.tiles[num2][i] == newIndex)
                        {
                            item[i] = (byte) oldIndex;
                        }
                        else
                        {
                            item[i] = image.rahc.tileData.tiles[num2][i];
                        }
                    }
                    list.Add(item);
                }
                else
                {
                    list.Add(image.rahc.tileData.tiles[num2]);
                }
            }
        }
        else if (image.order == NCGR.TileOrder.NoTiled)
        {
            num = ((image.rahc.depth == ColorDepth.Depth4Bit) ? (cell.obj2.tileOffset * 2) : cell.obj2.tileOffset) * 0x40;
            num *= (blockSize != 0) ? blockSize : 1;
            for (num2 = 0; num2 < image.rahc.tileData.tiles[0].Length; num2++)
            {
                if ((num2 >= num) && (num2 < (((int) num) + (cell.width * cell.height))))
                {
                    if (image.rahc.tileData.tiles[0][num2] == oldIndex)
                    {
                        list2.Add((byte) newIndex);
                    }
                    else if (image.rahc.tileData.tiles[0][num2] == newIndex)
                    {
                        list2.Add((byte) oldIndex);
                    }
                    else
                    {
                        list2.Add(image.rahc.tileData.tiles[0][num2]);
                    }
                }
                else
                {
                    list2.Add(image.rahc.tileData.tiles[0][num2]);
                }
            }
            list.Add(list2.ToArray());
        }
        return list.ToArray();
    }

    public static byte[][] Change_ImageCell(Cell cell, uint blockSize, NCGR.NCGR_s newTiles, NCGR.NCGR_s oldImage)
    {
        uint num3;
        int num4;
        List<byte[]> list = new List<byte[]>();
        List<byte[]> collection = new List<byte[]>();
        List<byte> list3 = new List<byte>();
        for (int i = 0; i < 0x200; i++)
        {
            for (int j = 0; j < 0x200; j++)
            {
                if (((i >= (0x100 + cell.obj0.yOffset)) && (i < ((0x100 + cell.obj0.yOffset) + cell.height))) && ((j >= (0x100 + cell.obj1.xOffset)) && (j < ((0x100 + cell.obj1.xOffset) + cell.width))))
                {
                    list3.Add(newTiles.rahc.tileData.tiles[0][j + (i * 0x200)]);
                }
            }
        }
        if (oldImage.order == NCGR.TileOrder.Horizontal)
        {
            collection.AddRange(Convertir.BytesToTiles_NoChanged(list3.ToArray(), cell.width / 8, cell.height / 8));
        }
        else
        {
            collection.Add(list3.ToArray());
        }
        list3.Clear();
        if (oldImage.order == NCGR.TileOrder.Horizontal)
        {
            num3 = (oldImage.rahc.depth == ColorDepth.Depth4Bit) ? (cell.obj2.tileOffset * 2) : cell.obj2.tileOffset;
            num3 *= (blockSize != 0) ? blockSize : 1;
            for (num4 = 0; num4 < num3; num4++)
            {
                list.Add(oldImage.rahc.tileData.tiles[num4]);
            }
            list.AddRange(collection);
            for (num4 = ((int) num3) + ((cell.width * cell.height) / 0x40); num4 < oldImage.rahc.tileData.tiles.Length; num4++)
            {
                list.Add(oldImage.rahc.tileData.tiles[num4]);
            }
        }
        else if (oldImage.order == NCGR.TileOrder.NoTiled)
        {
            num3 = ((oldImage.rahc.depth == ColorDepth.Depth4Bit) ? (cell.obj2.tileOffset * 2) : cell.obj2.tileOffset) * 0x40;
            num3 *= (blockSize != 0) ? blockSize : 1;
            for (num4 = 0; num4 < num3; num4++)
            {
                list3.Add(oldImage.rahc.tileData.tiles[0][num4]);
            }
            list3.AddRange(collection[0]);
            for (num4 = ((int) num3) + (cell.width * cell.height); num4 < oldImage.rahc.tileData.tiles[0].Length; num4++)
            {
                list3.Add(oldImage.rahc.tileData.tiles[0][num4]);
            }
            list.Add(list3.ToArray());
        }
        return list.ToArray();
    }

    private static int Comparision_Cell(Cell c1, Cell c2)
    {
        if (c1.obj2.priority < c2.obj2.priority)
        {
            return 1;
        }
        if (c1.obj2.priority > c2.obj2.priority)
        {
            return -1;
        }
        if (c1.num_cell < c2.num_cell)
        {
            return 1;
        }
        if (c1.num_cell > c2.num_cell)
        {
            return -1;
        }
        return 0;
    }

    public static Bitmap Get_Image(Bank banco, uint blockSize, NCGR.NCGR_s tile, NCLR.NCLR_s paleta, bool entorno, bool celda, bool numero, bool transparencia, bool image, int zoom = 1)
    {
        int num;
        if (banco.cells.Length == 0)
        {
            return new Bitmap(1, 1);
        }
        Size size = new Size(0x100 * zoom, 0x100 * zoom);
        Bitmap bitmap = new Bitmap(size.Width, size.Height);
        Graphics graphics = Graphics.FromImage(bitmap);
        if (entorno)
        {
            for (num = -size.Width; num < size.Width; num += 8)
            {
                graphics.DrawLine(Pens.LightBlue, (num + (size.Width / 2)) * zoom, 0, (num + (size.Width / 2)) * zoom, size.Height * zoom);
                graphics.DrawLine(Pens.LightBlue, 0, (num + (size.Height / 2)) * zoom, size.Width * zoom, (num + (size.Height / 2)) * zoom);
            }
            graphics.DrawLine(Pens.Blue, 0x80 * zoom, 0, 0x80 * zoom, 0x100 * zoom);
            graphics.DrawLine(Pens.Blue, 0, 0x80 * zoom, 0x100 * zoom, 0x80 * zoom);
        }
        Image[] imageArray = new Image[banco.nCells];
        for (num = 0; num < banco.nCells; num++)
        {
            if ((banco.cells[num].width != 0) && (banco.cells[num].height != 0))
            {
                uint tileOffset = banco.cells[num].obj2.tileOffset;
                if (blockSize > 4)
                {
                    blockSize = 4;
                }
                if (tile.rahc.depth == ColorDepth.Depth4Bit)
                {
                    tileOffset = tileOffset << ((byte) blockSize);
                }
                else
                {
                    tileOffset = (tileOffset << ((byte) blockSize)) / 2;
                }
                if (image)
                {
                    for (int i = 0; i < tile.rahc.tileData.nPalette.Length; i++)
                    {
                        tile.rahc.tileData.nPalette[i] = banco.cells[num].obj2.index_palette;
                    }
                    if (blockSize < 4)
                    {
                        if (tile.order == NCGR.TileOrder.NoTiled)
                        {
                            imageArray[num] = NCGR.Get_Image(tile, paleta, (int) (tileOffset * 0x40), banco.cells[num].width, banco.cells[num].height, zoom);
                        }
                        else
                        {
                            imageArray[num] = NCGR.Get_Image(tile, paleta, (int) (tileOffset * 0x40), banco.cells[num].width / 8, banco.cells[num].height / 8, zoom);
                        }
                    }
                    else
                    {
                        tileOffset /= blockSize / 2;
                        int nTilesX = tile.rahc.nTilesX;
                        int nTilesY = tile.rahc.nTilesY;
                        if (tile.order == NCGR.TileOrder.Horizontal)
                        {
                            nTilesX *= 8;
                            nTilesY *= 8;
                        }
                        int num6 = (int) (((long) tileOffset) % ((long) nTilesX));
                        int num7 = (int) (((long) tileOffset) / ((long) nTilesX));
                        if (tile.rahc.depth == ColorDepth.Depth4Bit)
                        {
                            num7 = (int) (num7 * (blockSize * 2));
                        }
                        else
                        {
                            num7 = (int) (num7 * blockSize);
                        }
                        if (num7 >= nTilesY)
                        {
                            num7 = num7 % nTilesY;
                        }
                        imageArray[num] = NCGR.Get_Image(tile, paleta, zoom).Clone(new Rectangle(num6 * zoom, num7 * zoom, banco.cells[num].width * zoom, banco.cells[num].height * zoom), PixelFormat.Undefined);
                    }
                    if ((banco.cells[num].obj1.flipX == 1) && (banco.cells[num].obj1.flipY == 1))
                    {
                        imageArray[num].RotateFlip(RotateFlipType.Rotate180FlipNone);
                    }
                    else if (banco.cells[num].obj1.flipX == 1)
                    {
                        imageArray[num].RotateFlip(RotateFlipType.RotateNoneFlipX);
                    }
                    else if (banco.cells[num].obj1.flipY == 1)
                    {
                        imageArray[num].RotateFlip(RotateFlipType.Rotate180FlipX);
                    }
                    if (transparencia)
                    {
                        ((Bitmap) imageArray[num]).MakeTransparent(paleta.pltt.palettes[tile.rahc.tileData.nPalette[0]].colors[0]);
                    }
                    graphics.DrawImageUnscaled(imageArray[num], (size.Width / 2) + (banco.cells[num].obj1.xOffset * zoom), (size.Height / 2) + (banco.cells[num].obj0.yOffset * zoom));
                }
                if (celda)
                {
                    graphics.DrawRectangle(Pens.Black, (int) ((size.Width / 2) + (banco.cells[num].obj1.xOffset * zoom)), (int) ((size.Height / 2) + (banco.cells[num].obj0.yOffset * zoom)), (int) (banco.cells[num].width * zoom), (int) (banco.cells[num].height * zoom));
                }
                if (numero)
                {
                    graphics.DrawString(banco.cells[num].num_cell.ToString(), SystemFonts.CaptionFont, Brushes.Black, (float) ((size.Width / 2) + (banco.cells[num].obj1.xOffset * zoom)), (float) ((size.Height / 2) + (banco.cells[num].obj0.yOffset * zoom)));
                }
            }
        }
        return bitmap;
    }

    public static Bitmap Get_Image(Bank banco, uint blockSize, NCGR.NCGR_s tile, NCLR.NCLR_s paleta, bool entorno, bool celda, bool numero, bool transparencia, bool image, int maxWidth, int maxHeight, int zoom = 1)
    {
        int num;
        if (banco.cells.Length == 0)
        {
            return new Bitmap(1, 1);
        }
        Size size = new Size(maxWidth * zoom, maxHeight * zoom);
        Bitmap bitmap = new Bitmap(size.Width, size.Height);
        Graphics graphics = Graphics.FromImage(bitmap);
        if (entorno)
        {
            for (num = -size.Width; num < size.Width; num += 8)
            {
                graphics.DrawLine(Pens.LightBlue, (num + (size.Width / 2)) * zoom, 0, (num + (size.Width / 2)) * zoom, size.Height * zoom);
                graphics.DrawLine(Pens.LightBlue, 0, (num + (size.Height / 2)) * zoom, size.Width * zoom, (num + (size.Height / 2)) * zoom);
            }
            graphics.DrawLine(Pens.Blue, (maxWidth / 2) * zoom, 0, (maxWidth / 2) * zoom, maxHeight * zoom);
            graphics.DrawLine(Pens.Blue, 0, (maxHeight / 2) * zoom, maxWidth * zoom, (maxHeight / 2) * zoom);
        }
        Image[] imageArray = new Image[banco.nCells];
        for (num = 0; num < banco.nCells; num++)
        {
            if ((banco.cells[num].width != 0) && (banco.cells[num].height != 0))
            {
                uint tileOffset = banco.cells[num].obj2.tileOffset;
                if (blockSize > 4)
                {
                    blockSize = 4;
                }
                if (tile.rahc.depth == ColorDepth.Depth4Bit)
                {
                    tileOffset = tileOffset << ((byte) blockSize);
                }
                else
                {
                    tileOffset = (tileOffset << ((byte) blockSize)) / 2;
                }
                if (image)
                {
                    for (int i = 0; i < tile.rahc.tileData.nPalette.Length; i++)
                    {
                        tile.rahc.tileData.nPalette[i] = banco.cells[num].obj2.index_palette;
                    }
                    if (blockSize < 4)
                    {
                        if (tile.order == NCGR.TileOrder.NoTiled)
                        {
                            imageArray[num] = NCGR.Get_Image(tile, paleta, (int) (tileOffset * 0x40), banco.cells[num].width, banco.cells[num].height, zoom);
                        }
                        else
                        {
                            imageArray[num] = NCGR.Get_Image(tile, paleta, (int) (tileOffset * 0x40), banco.cells[num].width / 8, banco.cells[num].height / 8, zoom);
                        }
                    }
                    else
                    {
                        tileOffset /= blockSize / 2;
                        int nTilesX = tile.rahc.nTilesX;
                        int nTilesY = tile.rahc.nTilesY;
                        if (tile.order == NCGR.TileOrder.Horizontal)
                        {
                            nTilesX *= 8;
                            nTilesY *= 8;
                        }
                        int num6 = (int) (((long) tileOffset) % ((long) nTilesX));
                        int num7 = (int) (((long) tileOffset) / ((long) nTilesX));
                        if (tile.rahc.depth == ColorDepth.Depth4Bit)
                        {
                            num7 = (int) (num7 * (blockSize * 2));
                        }
                        else
                        {
                            num7 = (int) (num7 * blockSize);
                        }
                        if (num7 >= nTilesY)
                        {
                            num7 = num7 % nTilesY;
                        }
                        imageArray[num] = NCGR.Get_Image(tile, paleta, zoom).Clone(new Rectangle(num6 * zoom, num7 * zoom, banco.cells[num].width * zoom, banco.cells[num].height * zoom), PixelFormat.Undefined);
                    }
                    if ((banco.cells[num].obj1.flipX == 1) && (banco.cells[num].obj1.flipY == 1))
                    {
                        imageArray[num].RotateFlip(RotateFlipType.Rotate180FlipNone);
                    }
                    else if (banco.cells[num].obj1.flipX == 1)
                    {
                        imageArray[num].RotateFlip(RotateFlipType.RotateNoneFlipX);
                    }
                    else if (banco.cells[num].obj1.flipY == 1)
                    {
                        imageArray[num].RotateFlip(RotateFlipType.Rotate180FlipX);
                    }
                    if (transparencia)
                    {
                        ((Bitmap) imageArray[num]).MakeTransparent(paleta.pltt.palettes[tile.rahc.tileData.nPalette[0]].colors[0]);
                    }
                    graphics.DrawImageUnscaled(imageArray[num], (size.Width / 2) + (banco.cells[num].obj1.xOffset * zoom), (size.Height / 2) + (banco.cells[num].obj0.yOffset * zoom));
                }
                if (celda)
                {
                    graphics.DrawRectangle(Pens.Black, (int) ((size.Width / 2) + (banco.cells[num].obj1.xOffset * zoom)), (int) ((size.Height / 2) + (banco.cells[num].obj0.yOffset * zoom)), (int) (banco.cells[num].width * zoom), (int) (banco.cells[num].height * zoom));
                }
                if (numero)
                {
                    graphics.DrawString(num.ToString(), SystemFonts.CaptionFont, Brushes.Black, (float) ((size.Width / 2) + (banco.cells[num].obj1.xOffset * zoom)), (float) ((size.Height / 2) + (banco.cells[num].obj0.yOffset * zoom)));
                }
            }
        }
        return bitmap;
    }

    private static Cell Get_LastCell(Bank bank)
    {
        for (int i = 0; i < bank.cells.Length; i++)
        {
            if (bank.cells[i].num_cell == (bank.cells.Length - 1))
            {
                return bank.cells[i];
            }
        }
        return new Cell();
    }

    public static NCER_s Read(string file, int id)
    {
        int num2;
        BinaryReader reader = new BinaryReader(File.OpenRead(file));
        NCER_s _s = new NCER_s {
            id = (uint) id
        };
        Console.WriteLine("NCER {0}<pre>", Path.GetFileName(file));
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
        _s.cebk.id = reader.ReadChars(4);
        _s.cebk.section_size = reader.ReadUInt32();
        _s.cebk.nBanks = reader.ReadUInt16();
        _s.cebk.tBank = reader.ReadUInt16();
        _s.cebk.constant = reader.ReadUInt32();
        _s.cebk.block_size = reader.ReadUInt32() & 0xff;
        _s.cebk.unknown1 = reader.ReadUInt32();
        _s.cebk.unknown2 = reader.ReadUInt64();
        _s.cebk.banks = new Bank[_s.cebk.nBanks];
        uint num = 0;
        for (num2 = 0; num2 < _s.cebk.nBanks; num2++)
        {
            _s.cebk.banks[num2].nCells = reader.ReadUInt16();
            _s.cebk.banks[num2].unknown1 = reader.ReadUInt16();
            _s.cebk.banks[num2].cell_offset = reader.ReadUInt32();
            if (_s.cebk.tBank == 1)
            {
                _s.cebk.banks[num2].xMax = reader.ReadInt16();
                _s.cebk.banks[num2].yMax = reader.ReadInt16();
                _s.cebk.banks[num2].xMin = reader.ReadInt16();
                _s.cebk.banks[num2].yMin = reader.ReadInt16();
            }
            long position = reader.BaseStream.Position;
            if (_s.cebk.tBank == 0)
            {
                Stream baseStream = reader.BaseStream;
                baseStream.Position += ((_s.cebk.nBanks - (num2 + 1)) * 8) + _s.cebk.banks[num2].cell_offset;
            }
            else
            {
                Stream stream2 = reader.BaseStream;
                stream2.Position += ((_s.cebk.nBanks - (num2 + 1)) * 0x10) + _s.cebk.banks[num2].cell_offset;
            }
            _s.cebk.banks[num2].cells = new Cell[_s.cebk.banks[num2].nCells];
            for (int i = 0; i < _s.cebk.banks[num2].nCells; i++)
            {
                _s.cebk.banks[num2].cells[i].num_cell = (ushort) i;
                ushort num5 = reader.ReadUInt16();
                ushort num6 = reader.ReadUInt16();
                ushort num7 = reader.ReadUInt16();
                _s.cebk.banks[num2].cells[i].obj0.yOffset = (sbyte) (num5 & 0xff);
                _s.cebk.banks[num2].cells[i].obj0.rs_flag = (byte) ((num5 >> 8) & 1);
                if (_s.cebk.banks[num2].cells[i].obj0.rs_flag == 0)
                {
                    _s.cebk.banks[num2].cells[i].obj0.objDisable = (byte) ((num5 >> 9) & 1);
                }
                else
                {
                    _s.cebk.banks[num2].cells[i].obj0.doubleSize = (byte) ((num5 >> 9) & 1);
                }
                _s.cebk.banks[num2].cells[i].obj0.objMode = (byte) ((num5 >> 10) & 3);
                _s.cebk.banks[num2].cells[i].obj0.mosaic_flag = (byte) ((num5 >> 12) & 1);
                _s.cebk.banks[num2].cells[i].obj0.depth = (byte) ((num5 >> 13) & 1);
                _s.cebk.banks[num2].cells[i].obj0.shape = (byte) ((num5 >> 14) & 3);
                _s.cebk.banks[num2].cells[i].obj1.xOffset = num6 & 0x1ff;
                if (_s.cebk.banks[num2].cells[i].obj1.xOffset >= 0x100)
                {
                    _s.cebk.banks[num2].cells[i].obj1.xOffset -= 0x200;
                }
                if (_s.cebk.banks[num2].cells[i].obj0.rs_flag == 0)
                {
                    _s.cebk.banks[num2].cells[i].obj1.unused = (byte) ((num6 >> 9) & 7);
                    _s.cebk.banks[num2].cells[i].obj1.flipX = (byte) ((num6 >> 12) & 1);
                    _s.cebk.banks[num2].cells[i].obj1.flipY = (byte) ((num6 >> 13) & 1);
                }
                else
                {
                    _s.cebk.banks[num2].cells[i].obj1.select_param = (byte) ((num6 >> 9) & 0x1f);
                }
                _s.cebk.banks[num2].cells[i].obj1.size = (byte) ((num6 >> 14) & 3);
                _s.cebk.banks[num2].cells[i].obj2.tileOffset = (uint) (num7 & 0x3ff);
                if (_s.cebk.unknown1 != 0)
                {
                    _s.cebk.banks[num2].cells[i].obj2.tileOffset += num;
                }
                _s.cebk.banks[num2].cells[i].obj2.priority = (byte) ((num7 >> 10) & 3);
                _s.cebk.banks[num2].cells[i].obj2.index_palette = (byte) ((num7 >> 12) & 15);
                Size size = Calculate_Size(_s.cebk.banks[num2].cells[i].obj0.shape, _s.cebk.banks[num2].cells[i].obj1.size);
                _s.cebk.banks[num2].cells[i].height = (ushort) size.Height;
                _s.cebk.banks[num2].cells[i].width = (ushort) size.Width;
                if (_s.cebk.banks[num2].cells[i].obj0.doubleSize == 1)
                {
                    _s.cebk.banks[num2].cells[i].width = (ushort) (_s.cebk.banks[num2].cells[i].width * 2);
                    _s.cebk.banks[num2].cells[i].height = (ushort) (_s.cebk.banks[num2].cells[i].height * 2);
                }
            }
            List<Cell> list = new List<Cell>();
            list.AddRange(_s.cebk.banks[num2].cells);
            list.Sort(new Comparison<Cell>(NCER.Comparision_Cell));
            _s.cebk.banks[num2].cells = list.ToArray();
            if ((_s.cebk.unknown1 != 0) && (_s.cebk.banks[num2].nCells != 0))
            {
                Cell cell = Get_LastCell(_s.cebk.banks[num2]);
                int num8 = cell.height * cell.width;
                num8 /= ((int) 0x40) << ((byte) _s.cebk.block_size);
                if (cell.obj0.depth == 1)
                {
                    num8 *= 2;
                }
                if (num8 == 0)
                {
                    num8 = 1;
                }
                num += (cell.obj2.tileOffset - num) + ((uint) num8);
            }
            reader.BaseStream.Position = position;
            Console.WriteLine("--------------");
        }
        reader.BaseStream.Position = _s.header.header_size + _s.cebk.section_size;
        List<uint> list2 = new List<uint>();
        List<string> list3 = new List<string>();
        _s.labl.names = new string[_s.cebk.nBanks];
        _s.labl.id = reader.ReadChars(4);
        if (new string(_s.labl.id) == "LBAL")
        {
            _s.labl.section_size = reader.ReadUInt32();
            for (num2 = 0; num2 < _s.cebk.nBanks; num2++)
            {
                uint item = reader.ReadUInt32();
                if (item >= (_s.labl.section_size - 8))
                {
                    Stream stream3 = reader.BaseStream;
                    stream3.Position -= 4L;
                    break;
                }
                list2.Add(item);
            }
            _s.labl.offset = list2.ToArray();
            for (num2 = 0; num2 < _s.labl.offset.Length; num2++)
            {
                list3.Add("");
                for (byte j = reader.ReadByte(); j != 0; j = reader.ReadByte())
                {
                    List<string> list4;
                    int num11;
                    (list4 = list3)[num11 = num2] = list4[num11] + ((char) j);
                }
            }
        }
        for (num2 = 0; num2 < _s.cebk.nBanks; num2++)
        {
            if (list3.Count > num2)
            {
                _s.labl.names[num2] = list3[num2];
            }
            else
            {
                _s.labl.names[num2] = num2.ToString();
            }
        }
        _s.uext.id = reader.ReadChars(4);
        if (!(new string(_s.uext.id) != "TXEU"))
        {
            _s.uext.section_size = reader.ReadUInt32();
            _s.uext.unknown = reader.ReadUInt32();
        }
        reader.Close();
        Console.WriteLine("</pre>EOF");
        return _s;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Bank
    {
        public ushort nCells;
        public ushort unknown1;
        public uint cell_offset;
        public NCER.Cell[] cells;
        public short xMax;
        public short yMax;
        public short xMin;
        public short yMin;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CEBK
    {
        public char[] id;
        public uint section_size;
        public ushort nBanks;
        public ushort tBank;
        public uint constant;
        public uint block_size;
        public uint unknown1;
        public ulong unknown2;
        public NCER.Bank[] banks;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Cell
    {
        public Obj0 obj0;
        public Obj1 obj1;
        public Obj2 obj2;
        public ushort width;
        public ushort height;
        public ushort num_cell;
        [StructLayout(LayoutKind.Sequential)]
        public struct Obj0
        {
            public int yOffset;
            public byte rs_flag;
            public byte objDisable;
            public byte doubleSize;
            public byte objMode;
            public byte mosaic_flag;
            public byte depth;
            public byte shape;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Obj1
        {
            public int xOffset;
            public byte unused;
            public byte flipX;
            public byte flipY;
            public byte select_param;
            public byte size;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Obj2
        {
            public uint tileOffset;
            public byte priority;
            public byte index_palette;
        }
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
    public struct NCER_s
    {
        public NCER.Header header;
        public NCER.CEBK cebk;
        public NCER.LABL labl;
        public NCER.UEXT uext;
        public object other;
        public uint id;
    }

    public enum TileOrder
    {
        NoTiled,
        Horizontal,
        Vertical
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct UEXT
    {
        public char[] id;
        public uint section_size;
        public uint unknown;
    }
}

