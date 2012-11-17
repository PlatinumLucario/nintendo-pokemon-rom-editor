namespace PG4Map
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Windows.Forms;

    public class TileView : Form
    {
        private Button btnBgd;
        private Button btnBgdTrans;
        internal Button btnImport;
        private Button btnSave;
        private Button btnSetTrans;
        private CheckBox checkTransparency;
        private ComboBox comboBox1;
        private ComboBox comboDepth;
        private IContainer components;
        private bool isMap;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label lblZoom;
        private DataGridView listInfo;
        private NSCR.NSCR_s map;
        private NumericUpDown numericHeight;
        private NumericUpDown numericStart;
        private NumericUpDown numericWidth;
        private string oldDepth;
        private int oldTiles;
        private NCLR.NCLR_s paleta;
        private Panel panel1;
        private PictureBox pic;
        private PictureBox pictureBgd;
        private DataGridViewTextBoxColumn Property;
        private bool selectColor;
        private bool stopUpdating;
        private NCGR.NCGR_s tile;
        private TrackBar trackZoom;
        private DataGridViewTextBoxColumn Value;

        public TileView()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public TileView(NCGR.NCGR_s tile, NCLR.NCLR_s paleta)
        {
            this.components = null;
            this.InitializeComponent();
            this.isMap = false;
            this.paleta = paleta;
            this.tile = tile;
            if (!(this.tile.other is int))
            {
                this.tile.other = 0;
            }
            this.pic.Image = NCGR.Get_Image(tile, paleta, (int) this.tile.other);
            this.numericWidth.Value = this.pic.Image.Width;
            this.numericHeight.Value = this.pic.Image.Height;
            this.comboDepth.Text = (tile.rahc.depth == ColorDepth.Depth4Bit) ? "4 bpp" : "8 bpp";
            this.oldDepth = this.comboDepth.Text;
            switch (tile.order)
            {
                case NCGR.TileOrder.NoTiled:
                    this.oldTiles = 0;
                    this.comboBox1.SelectedIndex = 0;
                    break;

                case NCGR.TileOrder.Horizontal:
                    this.oldTiles = 1;
                    this.comboBox1.SelectedIndex = 1;
                    break;
            }
            this.comboDepth.SelectedIndexChanged += new EventHandler(this.comboDepth_SelectedIndexChanged);
            this.numericWidth.ValueChanged += new EventHandler(this.numericSize_ValueChanged);
            this.numericHeight.ValueChanged += new EventHandler(this.numericSize_ValueChanged);
            this.numericStart.ValueChanged += new EventHandler(this.numericStart_ValueChanged);
            this.Info();
            if ((new string(paleta.header.id) != "NCLR.NCLR_s") && (new string(paleta.header.id) != "RLCN"))
            {
                this.btnSetTrans.Enabled = false;
            }
        }

        public TileView(NCGR.NCGR_s tile, NCLR.NCLR_s paleta, NSCR.NSCR_s map)
        {
            this.components = null;
            this.InitializeComponent();
            this.isMap = true;
            this.paleta = paleta;
            this.tile = tile;
            this.map = map;
            if (!(this.tile.other is int))
            {
                this.tile.other = 0;
            }
            if (!(this.map.other is int))
            {
                this.map.other = 0;
            }
            this.numericWidth.Value = map.section.width;
            this.numericHeight.Value = map.section.height;
            this.comboDepth.Text = (tile.rahc.depth == ColorDepth.Depth4Bit) ? "4 bpp" : "8 bpp";
            this.oldDepth = this.comboDepth.Text;
            switch (tile.order)
            {
                case NCGR.TileOrder.NoTiled:
                    this.oldTiles = 0;
                    this.comboBox1.SelectedIndex = 0;
                    break;

                case NCGR.TileOrder.Horizontal:
                    this.oldTiles = 1;
                    this.comboBox1.SelectedIndex = 1;
                    break;
            }
            this.comboDepth.SelectedIndexChanged += new EventHandler(this.comboDepth_SelectedIndexChanged);
            this.numericWidth.ValueChanged += new EventHandler(this.numericSize_ValueChanged);
            this.numericHeight.ValueChanged += new EventHandler(this.numericSize_ValueChanged);
            this.numericStart.ValueChanged += new EventHandler(this.numericStart_ValueChanged);
            this.Info();
            if ((new string(paleta.header.id) != "NCLR.NCLR_s") && (new string(paleta.header.id) != "RLCN"))
            {
                this.btnSetTrans.Enabled = false;
            }
            this.UpdateImage();
        }

        private void Add_TransparencyColor()
        {
            int index = Convertir.Remove_DuplicatedColors(ref this.paleta.pltt.palettes[0], ref this.tile.rahc.tileData.tiles);
            if (index == -1)
            {
                index = Convertir.Remove_NotUsedColors(ref this.paleta.pltt.palettes[0], ref this.tile.rahc.tileData.tiles);
            }
            this.paleta.pltt.palettes[0].colors[index] = this.paleta.pltt.palettes[0].colors[0];
            this.paleta.pltt.palettes[0].colors[0] = Color.FromArgb(0xf8, 0, 0xf8);
            Convertir.Change_Color(ref this.tile.rahc.tileData.tiles, index, 0);
            string tempFileName = Path.GetTempFileName();
            NCLR.Escribir(this.paleta, tempFileName);
            string fileout = Path.GetTempFileName();
            NCGR.Write(this.tile, fileout);
            this.UpdateImage();
            this.checkTransparency.Checked = true;
        }

        private void btnBgd_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog {
                AllowFullOpen = true,
                AnyColor = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.pictureBgd.BackColor = dialog.Color;
                this.pic.BackColor = dialog.Color;
                this.btnBgdTrans.Enabled = true;
            }
        }

        private void btnBgdTrans_Click(object sender, EventArgs e)
        {
            this.btnBgdTrans.Enabled = false;
            this.pictureBgd.BackColor = Color.Transparent;
            this.pic.BackColor = Color.Transparent;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog {
                AddExtension = true,
                DefaultExt = "bmp",
                Filter = "BitMaP (*.bmp)|*.bmp|Portable Network Graphic (*.png)|*.png|JPEG (*.jpg)|*.jpg;*.jpeg|Tagged Image File Format (*.tiff)|*.tiff;*.tif|Graphic Interchange Format (*.gif)|*.gif|Icon (*.ico)|*.ico;*.icon",
                OverwritePrompt = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap bitmap = (Bitmap) this.UpdateImage();
                if (dialog.FilterIndex == 1)
                {
                    bitmap.Save(dialog.FileName, ImageFormat.Bmp);
                }
                else if (dialog.FilterIndex == 2)
                {
                    this.UpdateImage().Save(dialog.FileName, ImageFormat.Png);
                }
                else if (dialog.FilterIndex == 3)
                {
                    this.UpdateImage().Save(dialog.FileName, ImageFormat.Jpeg);
                }
                else if (dialog.FilterIndex == 4)
                {
                    this.UpdateImage().Save(dialog.FileName, ImageFormat.Tiff);
                }
                else if (dialog.FilterIndex == 5)
                {
                    this.UpdateImage().Save(dialog.FileName, ImageFormat.Gif);
                }
                else if (dialog.FilterIndex == 6)
                {
                    this.UpdateImage().Save(dialog.FileName, ImageFormat.Icon);
                }
            }
            dialog.Dispose();
        }

        private void Change_TransparencyColor(Color color)
        {
            int oldIndex = 0;
            for (int i = 0; i < this.paleta.pltt.palettes[0].colors.Length; i++)
            {
                if (this.paleta.pltt.palettes[0].colors[i] == color)
                {
                    this.paleta.pltt.palettes[0].colors[i] = this.paleta.pltt.palettes[0].colors[0];
                    this.paleta.pltt.palettes[0].colors[0] = color;
                    oldIndex = i;
                    break;
                }
            }
            string tempFileName = Path.GetTempFileName();
            NCLR.Escribir(this.paleta, tempFileName);
            Convertir.Change_Color(ref this.tile.rahc.tileData.tiles, oldIndex, 0);
            string fileout = Path.GetTempFileName();
            NCGR.Write(this.tile, fileout);
            this.UpdateImage();
            this.checkTransparency.Checked = true;
        }

        private void checkTransparency_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkTransparency.Checked)
            {
                Bitmap image = (Bitmap) this.pic.Image;
                image.MakeTransparent(this.paleta.pltt.palettes[this.tile.rahc.tileData.nPalette[0]].colors[0]);
                this.pic.Image = image;
            }
            else
            {
                this.UpdateImage();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.oldTiles != this.comboBox1.SelectedIndex)
            {
                switch (this.comboBox1.SelectedIndex)
                {
                    case 0:
                        this.tile.order = NCGR.TileOrder.NoTiled;
                        this.tile.rahc.tileData.tiles[0] = Convertir.TilesToBytes(this.tile.rahc.tileData.tiles, 0);
                        this.numericHeight.Minimum = 0M;
                        this.numericWidth.Minimum = 0M;
                        this.numericWidth.Increment = 1M;
                        this.numericHeight.Increment = 1M;
                        break;

                    case 1:
                        this.tile.order = NCGR.TileOrder.Horizontal;
                        this.tile.rahc.tileData.tiles = Convertir.BytesToTiles(this.tile.rahc.tileData.tiles[0]);
                        this.numericHeight.Minimum = 8M;
                        this.numericWidth.Minimum = 8M;
                        this.numericWidth.Increment = 8M;
                        this.numericHeight.Increment = 8M;
                        break;

                    case 2:
                        this.tile.order = NCGR.TileOrder.Vertical;
                        break;
                }
                this.oldTiles = this.comboBox1.SelectedIndex;
                this.UpdateImage();
            }
        }

        private void comboDepth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.comboDepth.Text != this.oldDepth) && !this.stopUpdating)
            {
                byte[] buffer;
                this.oldDepth = this.comboDepth.Text;
                this.tile.rahc.depth = (this.comboDepth.Text == "4 bpp") ? ColorDepth.Depth4Bit : ColorDepth.Depth8Bit;
                if (this.comboDepth.Text == "4 bpp")
                {
                    buffer = Convertir.Bit8ToBit4(Convertir.TilesToBytes(this.tile.rahc.tileData.tiles, 0));
                    if (this.tile.order != NCGR.TileOrder.NoTiled)
                    {
                        this.tile.rahc.tileData.nPalette = new byte[this.tile.rahc.tileData.tiles.Length];
                    }
                    else
                    {
                        this.tile.rahc.tileData.tiles = new byte[][] { buffer };
                        this.tile.rahc.tileData.nPalette = new byte[this.tile.rahc.tileData.tiles[0].Length];
                    }
                }
                else
                {
                    buffer = Convertir.Bit4ToBit8(Convertir.TilesToBytes(this.tile.rahc.tileData.tiles, 0));
                    if (this.tile.order != NCGR.TileOrder.NoTiled)
                    {
                        this.tile.rahc.tileData.nPalette = new byte[this.tile.rahc.tileData.tiles.Length];
                    }
                    else
                    {
                        this.tile.rahc.tileData.tiles = new byte[][] { buffer };
                        this.tile.rahc.tileData.nPalette = new byte[this.tile.rahc.tileData.tiles[0].Length];
                    }
                }
                this.UpdateImage();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Info()
        {
            this.listInfo.Rows.Add(new object[] { "Costant", "0x" + string.Format("{0:X}", this.tile.header.constant) });
            this.listInfo.Rows.Add(new object[] { "NumSec", this.tile.header.nSection.ToString() });
            this.listInfo.Rows.Add(new object[] { "Magic", new string(this.tile.rahc.id) });
            this.listInfo.Rows.Add(new object[] { "Section", this.tile.rahc.size_section });
            this.listInfo.Rows.Add(new object[] { "Tiles_Y", string.Concat(new object[] { this.tile.rahc.nTilesY.ToString(), " (0x", string.Format("{0:X}", this.tile.rahc.nTilesY), ')' }) });
            this.listInfo.Rows.Add(new object[] { "Tiles_X", string.Concat(new object[] { this.tile.rahc.nTilesX.ToString(), " (0x", string.Format("{0:X}", this.tile.rahc.nTilesX), ')' }) });
            this.listInfo.Rows.Add(new object[] { "Format", Enum.GetName(this.tile.rahc.depth.GetType(), this.tile.rahc.depth) });
            this.listInfo.Rows.Add(new object[] { "Unknown", "0x" + string.Format("{0:X}", this.tile.rahc.unknown1) });
            this.listInfo.Rows.Add(new object[] { "Flag", "0x" + string.Format("{0:X}", this.tile.rahc.tiledFlag) });
            this.listInfo.Rows.Add(new object[] { "Size", "0x" + string.Format("{0:X}", this.tile.rahc.size_tiledata) });
            this.listInfo.Rows.Add(new object[] { "Unknown", "0x" + string.Format("{0:X}", this.tile.rahc.unknown3) });
        }

        private void InitializeComponent()
        {
            this.btnSetTrans = new Button();
            this.btnImport = new Button();
            this.btnBgdTrans = new Button();
            this.pictureBgd = new PictureBox();
            this.btnBgd = new Button();
            this.checkTransparency = new CheckBox();
            this.label8 = new Label();
            this.label7 = new Label();
            this.lblZoom = new Label();
            this.trackZoom = new TrackBar();
            this.label6 = new Label();
            this.comboBox1 = new ComboBox();
            this.label4 = new Label();
            this.btnSave = new Button();
            this.comboDepth = new ComboBox();
            this.label3 = new Label();
            this.numericStart = new NumericUpDown();
            this.numericHeight = new NumericUpDown();
            this.numericWidth = new NumericUpDown();
            this.label2 = new Label();
            this.label1 = new Label();
            this.pic = new PictureBox();
            this.panel1 = new Panel();
            this.listInfo = new DataGridView();
            this.Property = new DataGridViewTextBoxColumn();
            this.Value = new DataGridViewTextBoxColumn();
            ((ISupportInitialize) this.pictureBgd).BeginInit();
            this.trackZoom.BeginInit();
            this.numericStart.BeginInit();
            this.numericHeight.BeginInit();
            this.numericWidth.BeginInit();
            ((ISupportInitialize) this.pic).BeginInit();
            this.panel1.SuspendLayout();
            ((ISupportInitialize) this.listInfo).BeginInit();
            base.SuspendLayout();
            this.btnSetTrans.Location = new Point(0x243, 0x152);
            this.btnSetTrans.Name = "btnSetTrans";
            this.btnSetTrans.Size = new Size(0xe3, 30);
            this.btnSetTrans.TabIndex = 0x18;
            this.btnSetTrans.Text = "S22";
            this.btnSetTrans.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.btnSetTrans.UseVisualStyleBackColor = true;
            this.btnImport.Location = new Point(0x2d7, 0x1d9);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new Size(0x4f, 0x20);
            this.btnImport.TabIndex = 0x17;
            this.btnImport.Text = "S21";
            this.btnImport.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnBgdTrans.Enabled = false;
            this.btnBgdTrans.Location = new Point(0x2d5, 0x112);
            this.btnBgdTrans.Name = "btnBgdTrans";
            this.btnBgdTrans.Size = new Size(0x4e, 0x23);
            this.btnBgdTrans.TabIndex = 0x16;
            this.btnBgdTrans.Text = "S20";
            this.btnBgdTrans.UseVisualStyleBackColor = true;
            this.pictureBgd.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBgd.Location = new Point(0x2ac, 0x112);
            this.pictureBgd.Name = "pictureBgd";
            this.pictureBgd.Size = new Size(0x23, 0x23);
            this.pictureBgd.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBgd.TabIndex = 0x15;
            this.pictureBgd.TabStop = false;
            this.btnBgd.Location = new Point(0x240, 0x112);
            this.btnBgd.Name = "btnBgd";
            this.btnBgd.Size = new Size(0x66, 0x23);
            this.btnBgd.TabIndex = 20;
            this.btnBgd.Text = "S1F";
            this.btnBgd.UseVisualStyleBackColor = true;
            this.checkTransparency.AutoSize = true;
            this.checkTransparency.Location = new Point(0x243, 0x13b);
            this.checkTransparency.Name = "checkTransparency";
            this.checkTransparency.Size = new Size(0x2e, 0x11);
            this.checkTransparency.TabIndex = 0x13;
            this.checkTransparency.Text = "S1C";
            this.checkTransparency.UseVisualStyleBackColor = true;
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0x30d, 0x185);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x13, 13);
            this.label8.TabIndex = 0x12;
            this.label8.Text = "20";
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x240, 0x185);
            this.label7.Name = "label7";
            this.label7.Size = new Size(13, 13);
            this.label7.TabIndex = 0x11;
            this.label7.Text = "1";
            this.lblZoom.AutoSize = true;
            this.lblZoom.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblZoom.Location = new Point(0x296, 0x17a);
            this.lblZoom.Name = "lblZoom";
            this.lblZoom.Size = new Size(0x22, 0x11);
            this.lblZoom.TabIndex = 0x10;
            this.lblZoom.Text = "S1E";
            this.trackZoom.BackColor = SystemColors.GradientInactiveCaption;
            this.trackZoom.LargeChange = 2;
            this.trackZoom.Location = new Point(0x243, 0x195);
            this.trackZoom.Maximum = 20;
            this.trackZoom.Minimum = 1;
            this.trackZoom.Name = "trackZoom";
            this.trackZoom.Size = new Size(0xe2, 0x2d);
            this.trackZoom.SmallChange = 50;
            this.trackZoom.TabIndex = 15;
            this.trackZoom.Value = 1;
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x240, 0xf7);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x1a, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "S14";
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] { "S16", "S17" });
            this.comboBox1.Location = new Point(0x2ae, 0xf4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0x77, 0x15);
            this.comboBox1.TabIndex = 10;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x2ba, 0xc2);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x1f, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "BPP:";
            this.btnSave.Location = new Point(0x243, 0x1d9);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(0x60, 0x20);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "S15";
            this.btnSave.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.comboDepth.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboDepth.FormattingEnabled = true;
            this.comboDepth.Items.AddRange(new object[] { "4 bpp", "8 bpp" });
            this.comboDepth.Location = new Point(0x2ef, 0xbf);
            this.comboDepth.Name = "comboDepth";
            this.comboDepth.Size = new Size(0x36, 0x15);
            this.comboDepth.TabIndex = 8;
            this.comboDepth.SelectedIndexChanged += new EventHandler(this.comboDepth_SelectedIndexChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x240, 0xc2);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x1a, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "S11";
            this.numericStart.Location = new Point(0x27a, 0xc0);
            int[] bits = new int[4];
            bits[0] = 0xffff;
            this.numericStart.Maximum = new decimal(bits);
            this.numericStart.Name = "numericStart";
            this.numericStart.Size = new Size(0x37, 20);
            this.numericStart.TabIndex = 6;
            bits = new int[4];
            bits[0] = 8;
            this.numericHeight.Increment = new decimal(bits);
            this.numericHeight.Location = new Point(0x2ef, 0xda);
            bits = new int[4];
            bits[0] = 0x10000;
            this.numericHeight.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 8;
            this.numericHeight.Minimum = new decimal(bits);
            this.numericHeight.Name = "numericHeight";
            this.numericHeight.Size = new Size(0x37, 20);
            this.numericHeight.TabIndex = 4;
            bits = new int[4];
            bits[0] = 8;
            this.numericHeight.Value = new decimal(bits);
            bits = new int[4];
            bits[0] = 8;
            this.numericWidth.Increment = new decimal(bits);
            this.numericWidth.Location = new Point(0x27a, 0xda);
            bits = new int[4];
            bits[0] = 0x10000;
            this.numericWidth.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 8;
            this.numericWidth.Minimum = new decimal(bits);
            this.numericWidth.Name = "numericWidth";
            this.numericWidth.Size = new Size(0x37, 20);
            this.numericWidth.TabIndex = 1;
            bits = new int[4];
            bits[0] = 8;
            this.numericWidth.Value = new decimal(bits);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x2ba, 220);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x1a, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "S13";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x240, 220);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1a, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "S12";
            this.pic.BackColor = Color.Transparent;
            this.pic.BorderStyle = BorderStyle.FixedSingle;
            this.pic.Location = new Point(0x12, 7);
            this.pic.Name = "pic";
            this.pic.Size = new Size(100, 100);
            this.pic.SizeMode = PictureBoxSizeMode.AutoSize;
            this.pic.TabIndex = 11;
            this.pic.TabStop = false;
            this.panel1.Controls.Add(this.pic);
            this.panel1.Location = new Point(12, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x21d, 0x199);
            this.panel1.TabIndex = 0x19;
            this.listInfo.BackgroundColor = SystemColors.ButtonHighlight;
            this.listInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.listInfo.Columns.AddRange(new DataGridViewColumn[] { this.Property, this.Value });
            this.listInfo.GridColor = SystemColors.ButtonHighlight;
            this.listInfo.Location = new Point(0x236, 12);
            this.listInfo.Name = "listInfo";
            this.listInfo.RowHeadersVisible = false;
            this.listInfo.RowHeadersWidth = 8;
            this.listInfo.Size = new Size(240, 150);
            this.listInfo.TabIndex = 0x1a;
            this.Property.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Property.HeaderText = "Property";
            this.Property.Name = "Property";
            this.Value.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x331, 0x21d);
            base.Controls.Add(this.listInfo);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.btnSave);
            base.Controls.Add(this.btnImport);
            base.Controls.Add(this.btnSetTrans);
            base.Controls.Add(this.label8);
            base.Controls.Add(this.checkTransparency);
            base.Controls.Add(this.label7);
            base.Controls.Add(this.lblZoom);
            base.Controls.Add(this.trackZoom);
            base.Controls.Add(this.btnBgdTrans);
            base.Controls.Add(this.pictureBgd);
            base.Controls.Add(this.comboBox1);
            base.Controls.Add(this.btnBgd);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.numericWidth);
            base.Controls.Add(this.numericHeight);
            base.Controls.Add(this.numericStart);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.comboDepth);
            base.Controls.Add(this.label4);
            base.Name = "TileView";
            this.Text = "Tile Viewer";
            ((ISupportInitialize) this.pictureBgd).EndInit();
            this.trackZoom.EndInit();
            this.numericStart.EndInit();
            this.numericHeight.EndInit();
            this.numericWidth.EndInit();
            ((ISupportInitialize) this.pic).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((ISupportInitialize) this.listInfo).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void numericSize_ValueChanged(object sender, EventArgs e)
        {
            this.UpdateImage();
        }

        private void numericStart_ValueChanged(object sender, EventArgs e)
        {
            if (this.isMap)
            {
                this.map.other = (int) this.numericStart.Value;
            }
            else
            {
                this.tile.other = (int) this.numericStart.Value;
            }
            this.UpdateImage();
        }

        private void pic_DoubleClick(object sender, EventArgs e)
        {
            Form form = new Form();
            PictureBox box = new PictureBox {
                Image = this.pic.Image,
                SizeMode = PictureBoxSizeMode.AutoSize,
                BackColor = this.pictureBgd.BackColor
            };
            form.Controls.Add(box);
            form.BackColor = SystemColors.GradientInactiveCaption;
            form.AutoScroll = true;
            form.MaximumSize = new Size(0x400, 700);
            form.ShowIcon = false;
            form.AutoSize = true;
            form.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            form.MaximizeBox = false;
            form.Show();
        }

        private void pic_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.selectColor && (this.pic.Image != null))
            {
                Color pixel = ((Bitmap) this.pic.Image).GetPixel(e.X, e.Y);
                this.Change_TransparencyColor(pixel);
            }
        }

        private void trackZoom_Scroll(object sender, EventArgs e)
        {
            this.UpdateImage();
        }

        private Image UpdateImage()
        {
            Image image;
            if (this.stopUpdating)
            {
                return null;
            }
            if (this.tile.order != NCGR.TileOrder.NoTiled)
            {
                this.tile.rahc.nTilesX = (ushort) (this.numericWidth.Value / 8M);
                this.tile.rahc.nTilesY = (ushort) (this.numericHeight.Value / 8M);
            }
            else
            {
                this.tile.rahc.nTilesX = (ushort) this.numericWidth.Value;
                this.tile.rahc.nTilesY = (ushort) this.numericHeight.Value;
            }
            if (this.isMap)
            {
                NCGR.NCGR_s tile = this.tile;
                tile.rahc.tileData.tiles = Convertir.BytesToTiles(Convertir.TilesToBytes(tile.rahc.tileData.tiles, (int) this.tile.other));
                image = NCGR.Get_Image(tile, this.paleta, 0, this.trackZoom.Value);
            }
            else
            {
                image = NCGR.Get_Image(this.tile, this.paleta, (int) this.tile.other, this.trackZoom.Value);
            }
            this.pic.Image = image;
            return image;
        }
    }
}

