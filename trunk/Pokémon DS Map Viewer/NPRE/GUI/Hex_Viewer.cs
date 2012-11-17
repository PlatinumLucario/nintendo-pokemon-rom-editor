namespace PG4Map
{
    using PG4Map.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class Hex_Viewer : Form
    {
        private IContainer components;
        public RichTextBox HexBox;
        public static int num = 0;

        public Hex_Viewer()
        {
              components = null;
              InitializeComponent();
        }

        public Hex_Viewer(ClosableMemoryStream File)
        {
            int num2;
            string str;
            int num3;
            byte num4;
              components = null;
              InitializeComponent();
            BinaryReader reader = new BinaryReader(File);
            reader.BaseStream.Seek(0L, SeekOrigin.Begin);
            int num = 0;
            if (reader.BaseStream.Length > 0x10L)
            {
                for (num2 = 0; num2 < (File.Length / 0x10L); num2++)
                {
                    str = "";
                    if (num < 0x100)
                    {
                        if (num < 0x10)
                        {
                            str = "00" + num.ToString("X");
                        }
                        else
                        {
                            str = "0" + num.ToString("X");
                        }
                    }
                    else
                    {
                        str = num.ToString("X");
                    }
                      HexBox.AppendText("0x" + str + "        ");
                    num3 = 0;
                    while (num3 < 0x10)
                    {
                        num4 = reader.ReadByte();
                        if (num4 < 0x10)
                        {
                            str = "0" + num4.ToString("X");
                        }
                        else
                        {
                            str = num4.ToString("X");
                        }
                          HexBox.AppendText(str + " ");
                        num++;
                        num3++;
                    }
                    reader.BaseStream.Seek(-16L, SeekOrigin.Current);
                      HexBox.AppendText("               : ");
                    num3 = 0;
                    while (num3 < 0x10)
                    {
                        num4 = reader.ReadByte();
                        char ch = (char) num4;
                        if (num4 < 0x20)
                        {
                            ch = '.';
                        }
                          HexBox.AppendText(ch + " ");
                        num3++;
                    }
                      HexBox.AppendText("\n");
                }
            }
            else
            {
                  HexBox.AppendText("0x000            ");
                for (num2 = 0; num2 < File.Length; num2++)
                {
                    num4 = reader.ReadByte();
                    if (num4 < 0x10)
                    {
                        str = "0" + num4.ToString("X");
                    }
                    else
                    {
                        str = num4.ToString("X");
                    }
                      HexBox.AppendText(str + " ");
                    num++;
                }
                reader.BaseStream.Seek((long) -num, SeekOrigin.Current);
                  HexBox.AppendText("               : ");
                for (num3 = 0; num3 < num; num3++)
                {
                    num4 = reader.ReadByte();
                    char ch2 = (char) num4;
                    if (num4 < 0x20)
                    {
                        ch2 = '.';
                    }
                      HexBox.AppendText(ch2 + " ");
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
              HexBox = new RichTextBox();
            base.SuspendLayout();
              HexBox.Location = new Point(0x1b, 0x39);
              HexBox.Name = "HexBox";
              HexBox.Size = new Size(0x2be, 0x14f);
              HexBox.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
//            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(760, 0x1a3);
            base.Controls.Add(  HexBox);
            base.Name = "Hex_Viewer";
              Text = "Hex Viewer";
            base.ResumeLayout(false);
        }
    }
}

