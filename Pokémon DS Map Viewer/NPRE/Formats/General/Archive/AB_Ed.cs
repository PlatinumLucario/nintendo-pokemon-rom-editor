namespace PG4Map
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    public class abEditor : Form
    {
        public List<AB_s> Ab;
        private ComboBox ABList;
        private Button button1;
        private Button button2;
        private IContainer components = null;
        private Label label1;
        private Label label2;
        private Label label3;
        private MenuStrip menuStrip1;
        private ComboBox NarcList;
        private ToolStripMenuItem openABToolStripMenuItem;
        private ToolStripMenuItem openABToolStripMenuItem1;
        private ToolStripMenuItem openNarcToolStripMenuItem;
        private ToolStripMenuItem saveABToolStripMenuItem;
        private ComboBox SubList;

        public abEditor()
        {
              InitializeComponent();
        }

        private void ABList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BinaryReader reader;
            int num3;
            int selectedIndex =   ABList.SelectedIndex;
            int num2 =   NarcList.SelectedIndex;
            ClosableMemoryStream input = new ClosableMemoryStream();
            BlockInfo_s _s = new BlockInfo_s();
            if (num2 > -1)
            {
                input =   Ab[num2].BlockData[selectedIndex];
                _s =   Ab[num2].BlockInfo[selectedIndex];
                reader = new BinaryReader(input);
            }
            else
            {
                input =   Ab[0].BlockData[selectedIndex];
                _s =   Ab[0].BlockInfo[selectedIndex];
                reader = new BinaryReader(input);
                num2 = 0;
            }
              SubList.Items.Clear();
              SubList.Text = "";
            reader.BaseStream.Seek(0x13L, SeekOrigin.Begin);
            _s.File_Num = reader.ReadByte();
            _s.FileOff = new ArrayList();
            for (num3 = 0; num3 < _s.File_Num; num3++)
            {
                uint num4 = reader.ReadUInt32();
                _s.FileOff.Add(num4);
            }
            _s.FileData = new List<ClosableMemoryStream>();
            reader.ReadInt32();
              SubList.Text = "Loaded";
            if (_s.File_Num == 0)
            {
                ClosableMemoryStream item =   Ab[num2].BlockData[selectedIndex];
                _s.FileData.Add(item);
                  SubList.Items.Add("Unique File");
            }
            for (num3 = 0; num3 < _s.File_Num; num3++)
            {
                ClosableMemoryStream output = new ClosableMemoryStream();
                byte[] buffer = new byte[0];
                if (num3 == (_s.File_Num - 1))
                {
                    buffer = new byte[((uint) input.Length) - ((uint) _s.FileOff[num3])];
                }
                else
                {
                    buffer = new byte[((uint) _s.FileOff[num3 + 1]) - ((uint) _s.FileOff[num3])];
                }
                reader.Read(buffer, 0, buffer.Length);
                new BinaryWriter(output).Write(buffer);
                _s.FileData.Add(output);
                  SubList.Items.Add("Elem:" + num3);
            }
              Ab[num2].BlockInfo[selectedIndex] = _s;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() != DialogResult.Cancel)
            {
                BinaryWriter writer = new BinaryWriter(dialog.OpenFile());
                ClosableMemoryStream input =   Ab[  NarcList.SelectedIndex].BlockInfo[  ABList.SelectedIndex].FileData[  SubList.SelectedIndex];
                BinaryReader reader = new BinaryReader(input);
                reader.BaseStream.Seek(0L, SeekOrigin.Begin);
                for (int i = 0; i < input.Length; i++)
                {
                    writer.Write(reader.ReadByte());
                }
                writer.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <   Ab.Count; i++)
            {
                for (int j = 0; j <   Ab[i].BlockInfo.Count; j++)
                {
                    for (int k = 0; k <   Ab[i].BlockInfo[j].FileData.Count; k++)
                    {
                        ClosableMemoryStream input =   Ab[i].BlockInfo[j].FileData[k];
                        BinaryReader reader = new BinaryReader(input);
                        reader.BaseStream.Seek(0L, SeekOrigin.Begin);
                        Encoding encoding = Encoding.UTF8;
                        byte[] bytes = reader.ReadBytes(4);
                        int num4 = 0;
                        string str = "";
                        int index = 0;
                        while (index < 4)
                        {
                            char[] invalidPathChars = Path.GetInvalidPathChars();
                            foreach (char ch in invalidPathChars)
                            {
                                if (bytes[index] == ch)
                                {
                                    num4 = 1;
                                }
                            }
                            index++;
                        }
                        if (num4 == 0)
                        {
                            str = encoding.GetString(bytes);
                        }
                        Stream output = new FileStream(i.ToString() + "-" + k.ToString() + "-" + j.ToString() + "." + str, FileMode.Create);
                        BinaryWriter writer = new BinaryWriter(output);
                        reader.BaseStream.Seek(0L, SeekOrigin.Begin);
                        for (index = 0; index < input.Length; index++)
                        {
                            writer.Write(reader.ReadByte());
                        }
                        writer.Close();
                    }
                }
            }
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
              menuStrip1 = new MenuStrip();
              openABToolStripMenuItem = new ToolStripMenuItem();
              openNarcToolStripMenuItem = new ToolStripMenuItem();
              openABToolStripMenuItem1 = new ToolStripMenuItem();
              saveABToolStripMenuItem = new ToolStripMenuItem();
              ABList = new ComboBox();
              button1 = new Button();
              SubList = new ComboBox();
              button2 = new Button();
              NarcList = new ComboBox();
              label1 = new Label();
              label2 = new Label();
              label3 = new Label();
              menuStrip1.SuspendLayout();
            base.SuspendLayout();
              menuStrip1.Items.AddRange(new ToolStripItem[] {   openABToolStripMenuItem });
              menuStrip1.Location = new Point(0, 0);
              menuStrip1.Name = "menuStrip1";
              menuStrip1.Size = new Size(0x15c, 0x18);
              menuStrip1.TabIndex = 0;
              menuStrip1.Text = "menuStrip1";
              openABToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {   openNarcToolStripMenuItem,   openABToolStripMenuItem1,   saveABToolStripMenuItem });
              openABToolStripMenuItem.Name = "openABToolStripMenuItem";
              openABToolStripMenuItem.Size = new Size(0x25, 20);
              openABToolStripMenuItem.Text = "File";
              openNarcToolStripMenuItem.Name = "openNarcToolStripMenuItem";
              openNarcToolStripMenuItem.Size = new Size(0x83, 0x16);
              openNarcToolStripMenuItem.Text = "Open Narc";
              openABToolStripMenuItem1.Name = "openABToolStripMenuItem1";
              openABToolStripMenuItem1.Size = new Size(0x83, 0x16);
              openABToolStripMenuItem1.Text = "Open AB...";
              openABToolStripMenuItem1.Click += new EventHandler(  openABToolStripMenuItem1_Click);
              saveABToolStripMenuItem.Name = "saveABToolStripMenuItem";
              saveABToolStripMenuItem.Size = new Size(0x83, 0x16);
              saveABToolStripMenuItem.Text = "Save AB...";
              ABList.FormattingEnabled = true;
              ABList.Location = new Point(0x10, 0x66);
              ABList.Name = "ABList";
              ABList.Size = new Size(0x79, 0x15);
              ABList.TabIndex = 1;
              ABList.SelectedIndexChanged += new EventHandler(  ABList_SelectedIndexChanged);
              button1.Location = new Point(0xa8, 0x33);
              button1.Name = "button1";
              button1.Size = new Size(0x5c, 0x17);
              button1.TabIndex = 3;
              button1.Text = "Extract Singular";
              button1.UseVisualStyleBackColor = true;
              button1.Click += new EventHandler(  button1_Click);
              SubList.FormattingEnabled = true;
              SubList.Location = new Point(0x10, 0xa3);
              SubList.Name = "SubList";
              SubList.Size = new Size(0x79, 0x15);
              SubList.TabIndex = 4;
              button2.Location = new Point(0xa8, 0x66);
              button2.Name = "button2";
              button2.Size = new Size(0x5c, 0x17);
              button2.TabIndex = 5;
              button2.Text = "Extract All";
              button2.UseVisualStyleBackColor = true;
              button2.Click += new EventHandler(  button2_Click);
              NarcList.FormattingEnabled = true;
              NarcList.Location = new Point(0x10, 0x33);
              NarcList.Name = "NarcList";
              NarcList.Size = new Size(0x79, 0x15);
              NarcList.TabIndex = 6;
              label1.AutoSize = true;
              label1.Location = new Point(13, 0x23);
              label1.Name = "label1";
              label1.Size = new Size(0x31, 13);
              label1.TabIndex = 7;
              label1.Text = "Narc File";
              label2.AutoSize = true;
              label2.Location = new Point(13, 0x58);
              label2.Name = "label2";
              label2.Size = new Size(60, 13);
              label2.TabIndex = 8;
              label2.Text = "AB Archive";
              label3.AutoSize = true;
              label3.Location = new Point(13, 0x93);
              label3.Name = "label3";
              label3.Size = new Size(0x40, 13);
              label3.TabIndex = 9;
              label3.Text = "Singular File";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x15c, 0xc6);
            base.Controls.Add(  label3);
            base.Controls.Add(  label2);
            base.Controls.Add(  label1);
            base.Controls.Add(  NarcList);
            base.Controls.Add(  button2);
            base.Controls.Add(  SubList);
            base.Controls.Add(  button1);
            base.Controls.Add(  ABList);
            base.Controls.Add(  menuStrip1);
            base.MainMenuStrip =   menuStrip1;
            base.Name = "AB_Ed";
              Text = "AB Archive Editor";
              menuStrip1.ResumeLayout(false);
              menuStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public static AB_s LoadAb(BinaryReader fp, AB_s Ab)
        {
            fp.BaseStream.Seek(0L, SeekOrigin.Begin);
            int num = 0;
            fp.ReadInt16();
            Ab.Block_Num = fp.ReadInt16();
            Ab.BlockStartOff = new ArrayList();
            for (num = 0; num <= Ab.Block_Num; num++)
            {
                uint num2 = fp.ReadUInt32();
                Ab.BlockStartOff.Add(num2);
            }
            Ab.BlockData = new List<ClosableMemoryStream>();
            Ab.BlockInfo = new List<BlockInfo_s>();
            num = 0;
            for (num = 0; num < Ab.Block_Num; num++)
            {
                ClosableMemoryStream output = new ClosableMemoryStream();
                byte[] buffer = new byte[((uint) Ab.BlockStartOff[num + 1]) - ((uint) Ab.BlockStartOff[num])];
                fp.Read(buffer, 0, buffer.Length);
                new BinaryWriter(output).Write(buffer);
                Ab.BlockData.Add(output);
                BlockInfo_s item = new BlockInfo_s();
                BinaryReader reader = new BinaryReader(Ab.BlockData[num]);
                reader.BaseStream.Seek(0x13L, SeekOrigin.Begin);
                item.File_Num = reader.ReadByte();
                item.FileOff = new ArrayList();
                int num3 = 0;
                while (num3 < item.File_Num)
                {
                    uint num4 = reader.ReadUInt32();
                    item.FileOff.Add(num4);
                    num3++;
                }
                item.FileData = new List<ClosableMemoryStream>();
                reader.ReadInt32();
                if (item.File_Num == 0)
                {
                    ClosableMemoryStream stream2 = Ab.BlockData[num];
                    item.FileData.Add(stream2);
                }
                else
                {
                    for (num3 = 0; num3 < item.File_Num; num3++)
                    {
                        ClosableMemoryStream stream3 = new ClosableMemoryStream();
                        byte[] buffer2 = new byte[0];
                        if (num3 == (item.File_Num - 1))
                        {
                            buffer2 = new byte[((uint) Ab.BlockData[num].Length) - ((uint) item.FileOff[num3])];
                        }
                        else
                        {
                            buffer2 = new byte[((uint) item.FileOff[num3 + 1]) - ((uint) item.FileOff[num3])];
                        }
                        reader.Read(buffer, 0, buffer.Length);
                        new BinaryWriter(stream3).Write(buffer);
                        item.FileData.Add(stream3);
                    }
                }
                Ab.BlockInfo.Add(item);
            }
            return Ab;
        }

        private void openABToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog {
                Filter = "AB File (*.AB)|*.AB"
            };
            if (dialog.ShowDialog() != DialogResult.Cancel)
            {
                  ABList.Items.Clear();
                BinaryReader fp = new BinaryReader(dialog.OpenFile());
                string str = Encoding.UTF8.GetString(fp.ReadBytes(2));
                  Ab = new List<AB_s>();
                if (str == "AB")
                {
                    AB_s ab = new AB_s();
                    ab = LoadAb(fp, ab);
                      Ab.Add(ab);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct AB_s
        {
            public string Id;
            public int Block_Num;
            public ArrayList BlockStartOff;
            public List<abEditor.BlockInfo_s> BlockInfo;
            public List<ClosableMemoryStream> BlockData;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BlockInfo_s
        {
            public byte File_Num;
            public ArrayList FileOff;
            public List<ClosableMemoryStream> FileData;
        }
    }
}

