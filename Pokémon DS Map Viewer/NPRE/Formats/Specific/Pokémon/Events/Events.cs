namespace PG4Map
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class Events : Form
    {
        private IContainer components;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private TextBox EventNum;
        private DataGridViewTextBoxColumn Flag;
        private TabPage Furn;
        private TextBox FurnNum;
        private MenuStrip menuStrip2;
        private DataGridViewTextBoxColumn Mov;
        private ToolStripMenuItem openEventToolStripMenuItem;
        private DataGridView OwInfo;
        private DataGridViewTextBoxColumn Par3;
        private DataGridViewTextBoxColumn Par4;
        private DataGridViewTextBoxColumn Par5;
        private DataGridView S0Info;
        private DataGridViewTextBoxColumn Script;
        private TabPage Sign;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private DataGridView WarpInfo;
        private TextBox WarpNum;
        private DataGridViewTextBoxColumn X;
        private DataGridViewTextBoxColumn Xf;
        private DataGridViewTextBoxColumn Yf;
        private DataGridViewTextBoxColumn Zf;
        public Ev_S eventStruct;
        private DataGridView TriggerInfo;

        public Events()
        {
              components = null;
              InitializeComponent();
        }

        public Events(Stream actualEvent)
        {
              components = null;
              InitializeComponent();
              BinaryReader reader = new BinaryReader(actualEvent);
              loadEvent(reader, S0Info,OwInfo,WarpInfo,TriggerInfo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (  components != null))
            {
                  components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
              menuStrip2 = new MenuStrip();
              openEventToolStripMenuItem = new ToolStripMenuItem();
              tabControl1 = new TabControl();
              Furn = new TabPage();
              FurnNum = new TextBox();
              S0Info = new DataGridView();
              dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
              dataGridViewTextBoxColumn10 = new DataGridViewTextBoxColumn();
              dataGridViewTextBoxColumn11 = new DataGridViewTextBoxColumn();
              dataGridViewTextBoxColumn12 = new DataGridViewTextBoxColumn();
              Par5 = new DataGridViewTextBoxColumn();
              tabPage2 = new TabPage();
              EventNum = new TextBox();
              OwInfo = new DataGridView();
              dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
              dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
              Mov = new DataGridViewTextBoxColumn();
              Flag = new DataGridViewTextBoxColumn();
              Script = new DataGridViewTextBoxColumn();
              X = new DataGridViewTextBoxColumn();
              Xf = new DataGridViewTextBoxColumn();
              dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
              Yf = new DataGridViewTextBoxColumn();
              dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
              Zf = new DataGridViewTextBoxColumn();
              tabPage1 = new TabPage();
              WarpNum = new TextBox();
              WarpInfo = new DataGridView();
              dataGridViewTextBoxColumn14 = new DataGridViewTextBoxColumn();
              dataGridViewTextBoxColumn15 = new DataGridViewTextBoxColumn();
              Par3 = new DataGridViewTextBoxColumn();
              Par4 = new DataGridViewTextBoxColumn();
              Sign = new TabPage();
              menuStrip2.SuspendLayout();
              tabControl1.SuspendLayout();
              Furn.SuspendLayout();
            ((ISupportInitialize)   S0Info).BeginInit();
              tabPage2.SuspendLayout();
            ((ISupportInitialize)   OwInfo).BeginInit();
              tabPage1.SuspendLayout();
            ((ISupportInitialize)   WarpInfo).BeginInit();
            base.SuspendLayout();
              menuStrip2.Items.AddRange(new ToolStripItem[] {   openEventToolStripMenuItem });
              menuStrip2.Location = new Point(0, 0);
              menuStrip2.Name = "menuStrip1";
              menuStrip2.Size = new Size(0x27a, 0x18);
              menuStrip2.TabIndex = 50;
              menuStrip2.Text = "menuStrip1";
              openEventToolStripMenuItem.Name = "openEventToolStripMenuItem";
              openEventToolStripMenuItem.Size = new Size(0x59, 20);
              openEventToolStripMenuItem.Text = "Open Event...";
              openEventToolStripMenuItem.Click += new EventHandler(  openEventToolStripMenuItem_Click);
              tabControl1.Controls.Add(  Furn);
              tabControl1.Controls.Add(  tabPage2);
              tabControl1.Controls.Add(  tabPage1);
              tabControl1.Controls.Add(  Sign);
              tabControl1.Location = new Point(12, 0x1b);
              tabControl1.Name = "tabControl1";
              tabControl1.SelectedIndex = 0;
              tabControl1.Size = new Size(0x261, 0x1a1);
              tabControl1.TabIndex = 0x37;
              Furn.Controls.Add(  FurnNum);
              Furn.Controls.Add(  S0Info);
              Furn.Location = new Point(4, 0x16);
              Furn.Name = "Furn";
              Furn.Padding = new Padding(3);
              Furn.Size = new Size(0x259, 0x187);
              Furn.TabIndex = 0;
              Furn.Text = "Furniture";
              Furn.UseVisualStyleBackColor = true;
              FurnNum.Location = new Point(0x11, 0x11);
              FurnNum.Name = "FurnNum";
              FurnNum.Size = new Size(0x69, 20);
              FurnNum.TabIndex = 0x38;
              S0Info.AllowUserToAddRows = false;
              S0Info.AllowUserToDeleteRows = false;
              S0Info.AllowUserToResizeColumns = false;
              S0Info.AllowUserToResizeRows = false;
              S0Info.BackgroundColor = SystemColors.ControlLightLight;
              S0Info.BorderStyle = BorderStyle.None;
              S0Info.CellBorderStyle = DataGridViewCellBorderStyle.RaisedHorizontal;
              S0Info.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
              S0Info.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
              S0Info.Columns.AddRange(new DataGridViewColumn[] {   dataGridViewTextBoxColumn9,   dataGridViewTextBoxColumn10,   dataGridViewTextBoxColumn11,   dataGridViewTextBoxColumn12,   Par5 });
              S0Info.GridColor = SystemColors.Control;
              S0Info.Location = new Point(0x11, 60);
              S0Info.Name = "S0Info";
              S0Info.RowHeadersVisible = false;
              S0Info.RowTemplate.Resizable = DataGridViewTriState.False;
              S0Info.ScrollBars = ScrollBars.Vertical;
              S0Info.Size = new Size(0x228, 0x134);
              S0Info.TabIndex = 0x37;
              dataGridViewTextBoxColumn9.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              dataGridViewTextBoxColumn9.HeaderText = "Par1";
              dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
              dataGridViewTextBoxColumn10.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              dataGridViewTextBoxColumn10.HeaderText = "X";
              dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
              dataGridViewTextBoxColumn11.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              dataGridViewTextBoxColumn11.HeaderText = "Y";
              dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
              dataGridViewTextBoxColumn12.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              dataGridViewTextBoxColumn12.HeaderText = "Z";
              dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
              Par5.HeaderText = "Par5";
              Par5.Name = "Par5";
              tabPage2.Controls.Add(  EventNum);
              tabPage2.Controls.Add(  OwInfo);
              tabPage2.Location = new Point(4, 0x16);
              tabPage2.Name = "tabPage2";
              tabPage2.Padding = new Padding(3);
              tabPage2.Size = new Size(0x259, 0x187);
              tabPage2.TabIndex = 1;
              tabPage2.Text = "People";
              tabPage2.UseVisualStyleBackColor = true;
              EventNum.Location = new Point(0x11, 0x11);
              EventNum.Name = "EventNum";
              EventNum.Size = new Size(100, 20);
              EventNum.TabIndex = 0x34;
              OwInfo.AllowUserToAddRows = false;
              OwInfo.AllowUserToDeleteRows = false;
              OwInfo.AllowUserToResizeColumns = false;
              OwInfo.AllowUserToResizeRows = false;
              OwInfo.BackgroundColor = SystemColors.ControlLightLight;
              OwInfo.BorderStyle = BorderStyle.None;
              OwInfo.CellBorderStyle = DataGridViewCellBorderStyle.RaisedHorizontal;
              OwInfo.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
              OwInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
              OwInfo.Columns.AddRange(new DataGridViewColumn[] {   dataGridViewTextBoxColumn3,   dataGridViewTextBoxColumn4,   Mov,   Flag,   Script,   X,   Xf,   dataGridViewTextBoxColumn5,   Yf,   dataGridViewTextBoxColumn6,   Zf });
              OwInfo.GridColor = SystemColors.Control;
              OwInfo.Location = new Point(0x11, 0x3d);
              OwInfo.Name = "OwInfo";
              OwInfo.RowHeadersVisible = false;
              OwInfo.RowTemplate.Resizable = DataGridViewTriState.False;
              OwInfo.ScrollBars = ScrollBars.Vertical;
              OwInfo.Size = new Size(0x234, 0x126);
              OwInfo.TabIndex = 50;
              dataGridViewTextBoxColumn3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              dataGridViewTextBoxColumn3.HeaderText = "Id";
              dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
              dataGridViewTextBoxColumn4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              dataGridViewTextBoxColumn4.HeaderText = "Sprite";
              dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
              Mov.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              Mov.HeaderText = "Mov";
              Mov.Name = "Mov";
              Flag.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              Flag.HeaderText = "Flag";
              Flag.Name = "Flag";
              Script.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              Script.HeaderText = "Script";
              Script.Name = "Script";
              X.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              X.HeaderText = "X";
              X.Name = "X";
              Xf.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              Xf.HeaderText = "X-Flag";
              Xf.Name = "Xf";
              dataGridViewTextBoxColumn5.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              dataGridViewTextBoxColumn5.HeaderText = "Y";
              dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
              Yf.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              Yf.HeaderText = "Y-Flag";
              Yf.Name = "Yf";
              dataGridViewTextBoxColumn6.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              dataGridViewTextBoxColumn6.HeaderText = "Z";
              dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
              Zf.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              Zf.HeaderText = "Z-Flag";
              Zf.Name = "Zf";
              tabPage1.Controls.Add(  WarpNum);
              tabPage1.Controls.Add(  WarpInfo);
              tabPage1.Location = new Point(4, 0x16);
              tabPage1.Name = "tabPage1";
              tabPage1.Padding = new Padding(3);
              tabPage1.Size = new Size(0x259, 0x187);
              tabPage1.TabIndex = 2;
              tabPage1.Text = "Warps";
              tabPage1.UseVisualStyleBackColor = true;
              WarpNum.Location = new Point(0x11, 0x11);
              WarpNum.Name = "WarpNum";
              WarpNum.Size = new Size(100, 20);
              WarpNum.TabIndex = 0x36;
              WarpInfo.AllowUserToAddRows = false;
              WarpInfo.AllowUserToDeleteRows = false;
              WarpInfo.AllowUserToResizeColumns = false;
              WarpInfo.AllowUserToResizeRows = false;
              WarpInfo.BackgroundColor = SystemColors.ControlLightLight;
              WarpInfo.BorderStyle = BorderStyle.None;
              WarpInfo.CellBorderStyle = DataGridViewCellBorderStyle.RaisedHorizontal;
              WarpInfo.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
              WarpInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
              WarpInfo.Columns.AddRange(new DataGridViewColumn[] {   dataGridViewTextBoxColumn14,   dataGridViewTextBoxColumn15,   Par3,   Par4 });
              WarpInfo.GridColor = SystemColors.Control;
              WarpInfo.Location = new Point(0x11, 0x39);
              WarpInfo.Name = "WarpInfo";
              WarpInfo.RowHeadersVisible = false;
              WarpInfo.RowTemplate.Resizable = DataGridViewTriState.False;
              WarpInfo.ScrollBars = ScrollBars.Vertical;
              WarpInfo.Size = new Size(0x22a, 0x125);
              WarpInfo.TabIndex = 0x35;
              dataGridViewTextBoxColumn14.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              dataGridViewTextBoxColumn14.HeaderText = "X";
              dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
              dataGridViewTextBoxColumn15.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              dataGridViewTextBoxColumn15.HeaderText = "Y";
              dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
              Par3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              Par3.HeaderText = "Map ID";
              Par3.Name = "Par3";
              Par4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
              Par4.HeaderText = "Map Anchor";
              Par4.Name = "Par4";
              Sign.Location = new Point(4, 0x16);
              Sign.Name = "Sign";
              Sign.Padding = new Padding(3);
              Sign.Size = new Size(0x259, 0x187);
              Sign.TabIndex = 3;
              Sign.Text = "Sign";
              Sign.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.ClientSize = new Size(0x27a, 0x1e3);
            base.Controls.Add(  tabControl1);
            base.Controls.Add(  menuStrip2);
            base.MainMenuStrip =   menuStrip2;
            base.Name = "Events";
              Text = "EventsEditor";
              menuStrip2.ResumeLayout(false);
              menuStrip2.PerformLayout();
              tabControl1.ResumeLayout(false);
              Furn.ResumeLayout(false);
              Furn.PerformLayout();
            ((ISupportInitialize)   S0Info).EndInit();
              tabPage2.ResumeLayout(false);
              tabPage2.PerformLayout();
            ((ISupportInitialize)   OwInfo).EndInit();
              tabPage1.ResumeLayout(false);
              tabPage1.PerformLayout();
            ((ISupportInitialize)   WarpInfo).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void loadEvent(BinaryReader reader, DataGridView S0Info, DataGridView OwInfo, DataGridView WarpInfo, DataGridView TriggerInfo)
        {
            eventStruct = new Ev_S();
            reader.BaseStream.Position = 0;
            loadFurniture(reader, S0Info);
            loadPeople(reader, OwInfo);
            loadWarps(reader, WarpInfo);
            loadTriggers(reader, TriggerInfo);
        }

        private void loadTriggers(BinaryReader reader, DataGridView TriggerInfo)
        {
            eventStruct.numTriggers = reader.ReadInt32();
            WarpNum.Text = eventStruct.numTriggers.ToString();
            if (eventStruct.numTriggers > 0)
            {
                eventStruct.listTriggers = new List<Triggers_s>();
                for (int num = 0; num < eventStruct.numTriggers; num++)
                {
                    var actualTrigger = new Triggers_s
                    {
                        script = reader.ReadInt16(),
                        x = reader.ReadInt16(),
                        y = reader.ReadInt16(),
                        width = reader.ReadInt16(),
                        length = reader.ReadInt16(),
                        par = reader.ReadInt16(),
                        par2 = reader.ReadInt16(),
                        flag = reader.ReadInt16()
                    };
                    eventStruct.listTriggers.Add(actualTrigger);
                }
                for (int num = 0; num < eventStruct.listTriggers.Count; num++)
                {
                    var actualTriggers = eventStruct.listTriggers[num];
                    if (TriggerInfo!=null)
                    TriggerInfo.Rows.Add(new object[] { 
                        actualTriggers.script.ToString(),
                        actualTriggers.x.ToString(), 
                        actualTriggers.y.ToString(), 
                        actualTriggers.width.ToString(), 
                        actualTriggers.length.ToString(),
                        actualTriggers.flag.ToString()});
                }
            }
        }

        private void loadWarps(BinaryReader reader, DataGridView WarpInfo)
        {
            eventStruct.numWarp = reader.ReadInt32();
            WarpNum.Text = eventStruct.numWarp.ToString();
            if (eventStruct.numWarp > 0)
            {
                eventStruct.listWarps = new List<Warp_s>();
                for (int num = 0; num < eventStruct.numWarp; num++)
                {
                    Warp_s _s3 = new Warp_s
                    {
                        x = reader.ReadInt16(),
                        y = reader.ReadInt16(),
                        mapId = reader.ReadInt16(),
                        anchor = reader.ReadInt16(),
                        par = reader.ReadInt16(),
                        par2 = reader.ReadInt16()
                    };
                    eventStruct.listWarps.Add(_s3);
                }
                for (int num = 0; num < eventStruct.listWarps.Count; num++)
                {
                    var actualWarp = eventStruct.listWarps[num];
                    WarpInfo.Rows.Add(new object[] { actualWarp.x.ToString(), 
                        actualWarp.y.ToString(), 
                        actualWarp.mapId.ToString(), 
                        actualWarp.anchor.ToString() });
                }
            }
        }

        private void loadPeople(BinaryReader reader, DataGridView OwInfo)
        {
            eventStruct.numPeople = reader.ReadInt32();
            EventNum.Text = eventStruct.numPeople.ToString();
            if (eventStruct.numPeople > 0)
            {
                eventStruct.listPeople = new List<Ow_s>();
                for (int num = 0; num < eventStruct.numPeople; num++)
                {
                    Ow_s peopleStruct = new Ow_s
                    {
                        id = reader.ReadInt16(),
                        sprite = reader.ReadInt16(),
                        mov = reader.ReadInt16(),
                        par = reader.ReadInt16(),
                        flag = reader.ReadInt16(),
                        script = reader.ReadInt16(),
                        par2 = reader.ReadInt16(),
                        sight = reader.ReadInt16(),
                        par3 = reader.ReadInt16(),
                        par4 = reader.ReadInt16(),
                        par5 = reader.ReadInt16(),
                        par6 = reader.ReadInt16(),
                        x = reader.ReadInt16(),
                        y = reader.ReadInt16(),
                        z = reader.ReadInt16(),
                        par7 = reader.ReadInt16(),

                    };
                    eventStruct.listPeople.Add(peopleStruct);
                }
                for (int num = 0; num < eventStruct.listPeople.Count; num++)
                    OwInfo.Rows.Add(new object[] { 
                        eventStruct.listPeople[num].id.ToString(), 
                        eventStruct.listPeople[num].sprite.ToString(), 
                        eventStruct.listPeople[num].mov.ToString(), 
                        eventStruct.listPeople[num].flag.ToString(), 
                        eventStruct.listPeople[num].script.ToString(), 
                        eventStruct.listPeople[num].sight.ToString(),
                        eventStruct.listPeople[num].x.ToString(),
                        eventStruct.listPeople[num].y.ToString(),
                        eventStruct.listPeople[num].z.ToString() });
            }
        }

        private void loadFurniture(BinaryReader reader, DataGridView S0Info)
        {
            eventStruct.numFurniture = reader.ReadInt32();
            FurnNum.Text = eventStruct.numFurniture.ToString();
            if (eventStruct.numFurniture > 0)
            {
                eventStruct.listFurniture = new List<Furn_S>();
                for (int num = 0; num < eventStruct.numFurniture; num++)
                {
                    Furn_S actualFurniture = new Furn_S
                    {
                        script = reader.ReadInt16(),
                        par = reader.ReadInt16(),
                        X = reader.ReadInt16(),
                        Z = reader.ReadInt16(),
                        Y = reader.ReadInt16(),
                        par2 = reader.ReadInt16(),
                        par3 = reader.ReadInt16(),
                        par4 = reader.ReadInt16(),
                        Flag = reader.ReadInt16(),
                        par5 = reader.ReadInt16(),
                    };
                    eventStruct.listFurniture.Add(actualFurniture);
                }
                for (int num = 0; num < eventStruct.listFurniture.Count; num++)
                {
                    S0Info.Rows.Add(new object[] { 
                        eventStruct.listFurniture[num].script.ToString(), 
                        eventStruct.listFurniture[num].X.ToString(), 
                        eventStruct.listFurniture[num].Y.ToString(), 
                        eventStruct.listFurniture[num].Z.ToString(), 
                        eventStruct.listFurniture[num].Flag.ToString() });
                }
            }
        }

        private void openEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            FileStream input = new FileStream(dialog.FileName, FileMode.Open);
            BinaryReader reader = new BinaryReader(input);
            if (reader != null)
            {
                  loadEvent(reader, S0Info,OwInfo,WarpInfo,TriggerInfo);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Ev_S
        {
            public int numFurniture;
            public List<Events.Furn_S> listFurniture;
            public int numPeople;
            public List<Events.Ow_s> listPeople;
            public int numWarp;
            public List<Events.Warp_s> listWarps;
            public List<Events.Triggers_s> listTriggers;
            public int[] Sign;
            public int numTriggers;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Ow_s
        {
            public short id;
            public short sprite;
            public short mov;
            public int flag;
            public short script;
            public short x;
            public short x_f;
            public short y;
            public short y_f;
            public short z;
            public short z_f;
            public short par;
            public short par2;
            public short sight;
            public short par3;
            public short par4;
            public short par5;
            public short par6;
            public short par7;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Furn_S
        {
            public int script;
            public int X;
            public int Y;
            public int Z;
            public int Flag;
            public short par;
            public short par2;
            public short par3;
            public short par4;
            public short par5;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Warp_s
        {
            public int x;
            public int y;
            public int mapId;
            public int anchor;
            public short par;
            public short par2;
        }

        public struct Triggers_s
      {
            public int x;
            public int y;
            public int width;
            public int length;
            public short par;
            public short par2;
            public short script;
            public short flag;
        }
    }
}

