namespace PG4Map
{
    using PG4Map.Formats;
    using PG4Map.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    public class Texture : Form
    {
        public Nsbtx actualTex;
        private IContainer components = null;
        private ToolStripMenuItem fileToolStripMenuItem;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem openNarcToolStripMenuItem;
        public static List<Tex_List> tex;
        private RichTextBox Texture_Out;

        public Texture()
        {
              InitializeComponent();
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
              fileToolStripMenuItem = new ToolStripMenuItem();
              openNarcToolStripMenuItem = new ToolStripMenuItem();
              menuStrip1.SuspendLayout();
            base.SuspendLayout();
              Texture_Out = new RichTextBox();
              Texture_Out.Location = new Point(0x2a, 0x3f);
              Texture_Out.Name = "Texture_Out";
              Texture_Out.Size = new Size(0x28c, 0x171);
              Texture_Out.TabIndex = 11;
              Texture_Out.TextChanged += new EventHandler(  Texture_Out_TextChanged);
              menuStrip1.Items.AddRange(new ToolStripItem[] {   fileToolStripMenuItem });
              menuStrip1.Location = new Point(0, 0);
              menuStrip1.Name = "menuStrip1";
              menuStrip1.Size = new Size(0x2d6, 0x18);
              menuStrip1.TabIndex = 12;
              menuStrip1.Text = "menuStrip1";
              fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {   openNarcToolStripMenuItem });
              fileToolStripMenuItem.Name = "fileToolStripMenuItem";
              fileToolStripMenuItem.Size = new Size(0x25, 20);
              fileToolStripMenuItem.Text = "File";
              openNarcToolStripMenuItem.Name = "openNarcToolStripMenuItem";
              openNarcToolStripMenuItem.Size = new Size(0x83, 0x16);
              openNarcToolStripMenuItem.Text = "Open Narc";
              openNarcToolStripMenuItem.Click += new EventHandler(  openNarcToolStripMenuItem_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.ClientSize = new Size(0x2d6, 0x189);
            base.Controls.Add(  Texture_Out);
            base.Controls.Add(  menuStrip1);
            base.MainMenuStrip =   menuStrip1;
            base.Name = "Texture";
              Text = "Texture Editor";
              menuStrip1.ResumeLayout(false);
              menuStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void openNarcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog {
                Filter = "Narc File (*.narc)|*.narc|Texture File(*.btx)|*.btx",
            };
            if (dialog.ShowDialog() != DialogResult.Cancel)
            {
                BinaryReader reader = new BinaryReader(dialog.OpenFile());
                var extension = Encoding.UTF8.GetString(reader.ReadBytes(4));
                if (extension == "NARC")
                {
                    Narc narc = new Narc();
                    narc.LoadNarc(reader);
                    tex = new List<Tex_List>();
                      Texture_Out.Clear();
                    for (int i = 0; i < narc.fatbNum; i++)
                    {
                        Tex_List list = new Tex_List();
                        Nsbtx _c = new Nsbtx();
                        ClosableMemoryStream stream = narc.figm.fileData[i];
                        actualTex = new Nsbtx();
                          actualTex.LoadBTX0(stream);
                        _c =   actualTex;
                        list.File_Name = "Tex_" + i.ToString();
                          Texture_Out.AppendText(list.File_Name + "=");
                        list.Texture = new List<string>();
                        for (int j = 0; j <   actualTex.tex0.texNum; j++)
                        {
                            list.Texture.Add(  actualTex.tex0.texNameArray[j]);
                              Texture_Out.AppendText(  actualTex.tex0.texNameArray[j] + ";");
                        }
                          Texture_Out.AppendText("\n");
                    }
                }
                else if (extension == "BTX0")
                {
                    Tex_List list = new Tex_List();
                    Nsbtx _c = new Nsbtx();
                    actualTex = new Nsbtx();
                      actualTex.LoadBTX0(reader);
                    _c =   actualTex;
                    list.File_Name = dialog.SafeFileName;
                      Texture_Out.AppendText(list.File_Name + "=");
                    list.Texture = new List<string>();
                    for (int j = 0; j <   actualTex.tex0.texNum; j++)
                    {
                        list.Texture.Add(  actualTex.tex0.texNameArray[j]);
                          Texture_Out.AppendText(  actualTex.tex0.texNameArray[j] + ";");
                    }
                      Texture_Out.AppendText("\n");
                }
            }
        }

        private void Texture_Out_TextChanged(object sender, EventArgs e)
        {
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Tex_List
        {
            public string File_Name;
            public List<string> Texture;
        }
    }
}

