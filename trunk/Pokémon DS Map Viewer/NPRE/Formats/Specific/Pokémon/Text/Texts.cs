namespace PG4Map.Formats
{
    using PG4Map.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Collections;
    using System.Text;

    public class Texts: Form
    {
        private IContainer components;
        private ComboBox listText;
        public RichTextBox textBox;
        public List<TextStruct> textList;
        private ClosableMemoryStream actualNode;
        private ArrayList messageOffset;
        private Narc textNarc;
        private int status;
        private bool isVar;
        private int type;
        private List<UInt16> unknowns = new List<UInt16>();
        private List<UInt16> keys = new List<UInt16>();
        private int count;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem textToolStripMenuItem;
        private ToolStripMenuItem narcToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem textToolStripMenuItem1;
        private ToolStripMenuItem narcToolStripMenuItem1;
        public List<string> messageList;
        private short numTextFile;
        private List<Int32> originalKey;
        private ArrayList originalOffsets;
        private List<Int32> originalSizes;


        public struct TextStruct
        {
            public int id;
            public int startOffset;
            public String text;
            public int key2;
            public int realKey;
            public int size;
            public int key;
        }

        public Texts()
        {
             components = null;
             InitializeComponent();
        }

        public Texts(ClosableMemoryStream actualNode, int type)
        {
            components = null;
            InitializeComponent();
            this.type = type;
            this.actualNode = actualNode;
            var reader = new BinaryReader(actualNode);
            reader.BaseStream.Position = 0;
            if (type == 1)
                readTextBW(reader,0,textBox);
            else
                readText(reader, textBox);
        }

        public Texts(Narc textNarc, int type)
        {
            components = null;
            InitializeComponent();
            this.textNarc = textNarc;
            this.type = type;
            for (int i = 0; i < textNarc.fatbNum; i++)
                listText.Items.Add("File : " + i);
        }


        public void saveText(BinaryWriter writer,  String[] messageLine)
        {

            //var messageLine = textBox.Text.Split("Message:".ToCharArray());
            numTextFile = (Int16)(textBox.Lines.Length - 1);
            writer.Write((Int16) numTextFile);
            writer.Write((Int16)originalKey[0]);
            int key = (originalKey[0] * 0x2FD) & 0xffff;
            int sum =0;
            for (int i = 0; i < numTextFile; i++ )
            {
                int value;
                int key2 = (key * (i + 1) & 0xffff);
                key2 = key2 | (key2 << 16);
                
                if (i == 0)
                {
                    value = ((Int16)8 * numTextFile + 4) ^ key2;
                    writer.Write(value);
                }
                else
                {
                    sum += getRealSize(messageLine[i-1])*2;
                    value = ((Int16)8 * numTextFile + 4 + sum) ^ key2;                   
                    writer.Write(value);
                }
                
                value = (Int16)(getRealSize(messageLine[i])) ^ key2;
                writer.Write(value);
            }

            for (int i = 0; i < numTextFile; i++){            
                key = (0x91BD3 * (i + 1)) & 0xffff;
                var words = messageLine[i].ToCharArray();
                bool skipFirst = false;
                for (int j = 0; j < words.Length - 1; j++)
                {
                    if (!skipFirst)
                    {
                        j += 8;
                        while (words[j] != ' ')
                            j++;
                        j += 2;
                        skipFirst = true;
                    }
                    else
                    {
                        var actualCar = words[j];
                        if (actualCar == '[')
                        {
                            int k = j;
                            while (words[k] != ']')
                                k++;
                            k++;
                            var varWords = messageLine[i].Substring(j, k + 1 - j);
                            var varArray = varWords.Split(' ');
                            writer.Write((Int16)(0xFFFE ^ key));
                            key += 0x493D;
                            key &= 0xffff;
                            writer.Write((Int16)(writeCharacter(varArray[1].ToString()) ^ key));
                            key += 0x493D;
                            key &= 0xffff;
                            writer.Write((Int16)(writeCharacter(varArray[2].ToString()) ^ key));
                            key += 0x493D;
                            key &= 0xffff;
                            writer.Write((Int16)(writeCharacter(varArray[3].ToString()) ^ key));
                            key += 0x493D;
                            key &= 0xffff;
                            j = k;

                        }
                        else if (actualCar == '/'){
                            writer.Write((Int16)(writeCharacter(actualCar.ToString() + words[j+1].ToString()) ^ key));
                            key += 0x493D;
                            key &= 0xffff;
                            j++;
                        }

                        else
                        {
                            writer.Write((Int16)(writeCharacter(actualCar.ToString()) ^ key));
                            key += 0x493D;
                            key &= 0xffff;
                        }
                    }
                }
            }
        }

        private int getRealSize(string p)
        {
            int size = 0;
            bool skipFirst = false;
            var charArray = p.ToCharArray();
            for (int i = 0; i < p.Length - 1; i++)
            {
                var actualChar = p[i];
                if (!skipFirst)
                {
                    i += 8;
                    while (p[i] != ' ')
                        i++;
                    i+= 2;
                    skipFirst = true;
                }
                else
                {
                    if (actualChar == '[')
                    {
                        while (p[i] != ']')
                            i++;
                        size += 3;
                    }
                    else if (actualChar == '/')
                    {
                        size++;
                        i++;
                    }
                    else
                        size++;
                }
            }
            return size;
        }


        private int writeCharacter(string actualCar)
        {
            switch (actualCar)
            {
                case ".0":
                    return 0;
                case ".1":
                    return 1;
                case ".2":
                    return 2;
                case "０":
                    return 0xA2;
                case "１":
                    return 0xA3;
                case "２":
                    return 0xA4;
                case "３":
                    return 0xA5;
                case "４":
                    return 0xA6;

                case "５":
                    return 0xA7;

                case "６":
                    return 0xA8;

                case "７":
                    return 0xA9;

                case "８":
                    return 0xAA;

                case "９":
                    return 0xAB;

                case "Ａ":
                    return 0xAC;

                case "Ｂ":
                    return 0xAD;

                case "Ｃ":
                    return 0xAE;

                case "Ｄ":
                    return 0xAF;

                case "Ｅ":
                    return 0xB0;

                case "Ｆ":
                    return 0xB1;

                case "Ｇ":
                    return 0xB2;

                case "Ｈ":
                    return 0xB3;

                case "Ｉ":
                    return 0xB4;

                case "Ｊ":
                    return 0xB5;

                case "Ｋ":
                    return 0xB6;

                case "Ｌ":
                    return 0xB7;

                case "Ｍ":
                    return 0xB8;

                case "Ｎ":
                    return 0xB9;

                case "Ｏ":
                    return 0xBA;

                case "Ｐ":
                    return 0xBB;

                case "Ｑ":
                    return 0xBC;

                case "Ｒ":
                    return 0xBD;

                case "Ｓ":
                    return 0xBE;

                case "Ｔ":
                    return 0xBF;
                case "Ｕ":
                    return 0xC0;
                case "Ｖ":
                    return 0xC1;
                case "Ｗ":
                    return 0xC2;
                case "Ｘ":
                    return 0xC3;
                case "Ｙ":
                    return 0xC4;
                case "Ｚ":
                    return 0xC5;
                case "ａ":
                    return 0xC6;
                case "ｂ":
                    return 0xC7;
                case "ｃ":
                    return 0xC8;
                case "ｄ":
                    return 0xC9;
                case "ｅ":
                    return 0xCA;
                case "ｆ":
                    return 0xCB;
                case "ｇ":
                    return 0xCC;
                case "ｈ":
                    return 0xCD;
                case "ｉ":
                    return 0xCE;
                case "ｊ":
                    return 0xCF;
                case "ｋ":
                    return 0xD0;
                case "ｌ":
                    return 0xD1;
                case "ｍ":
                    return 0xD2;
                case "ｎ":
                    return 0xD3;
                case "ｏ":
                    return 0xD4;
                case "ｐ":
                    return 0xD5;
                case "ｑ":
                    return 0xD6;
                case "ｒ":
                    return 0xD7;
                case "ｓ":
                    return 0xD8;
                case "ｔ":
                    return 0xD9;
                case "ｕ":
                    return 0xDA;
                case "ｖ":
                    return 0xDB;
                case "ｗ":
                    return 0xDC;
                case "ｘ":
                    return 0xDD;
                case "ｙ":
                    return 0xDE;
                case "ｚ":
                    return 0xDF;
                case "0":
                    return 0x121;
                case "1":
                    return 0x122;
                case "2":
                    return 0x123;
                case "3":
                    return 0x124;
                case "4":
                    return 0x125;
                case "5":
                    return 0x126;
                case "6":
                    return 0x127;
                case "7":
                    return 0x128;
                case "8":
                    return 0x129;
                case "9":
                    return 0x12A;
                case "A":
                    return 0x12B;
                case "B":
                    return 0x12C;
                case "C":
                    return 0x12D;
                case "D":
                    return 0x12E;
                case "E":
                    return 0x12F;
                case "F":
                    return 0x130;
                case "G":
                    return 0x131;
                case "H":
                    return 0x132;
                case "I":
                    return 0x133;
                case "J":
                    return 0x134;
                case "K":
                    return 0x135;
                case "L":
                    return 0x136;
                case "M":
                    return 0x137;
                case "N":
                    return 0x138;
                case "O":
                    return 0x139;
                case "P":
                    return 0x13A;
                case "Q":
                    return 0x13B;
                case "R":
                    return 0x13C;
                case "S":
                    return 0x13D;
                case "T":
                    return 0x13E;
                case "U":
                    return 0x13F;
                case "V":
                    return 0x140;
                case "W":
                    return 0x141;
                case "X":
                    return 0x142;
                case "Y":
                    return 0x143;
                case "Z":
                    return 0x144;
                case "a":
                    return 0x145;
                case "b":
                    return 0x146;
                case "c":
                    return 0x147;
                case "d":
                    return 0x148;
                case "e":
                    return 0x149;
                case "f":
                    return 0x14A;
                case "g":
                    return 0x14B;
                case "h":
                    return 0x14C;
                case "i":
                    return 0x14D;
                case "j":
                    return 0x14E;
                case "k":
                    return 0x14F;
                case "l":
                    return 0x150;
                case "m":
                    return 0x151;
                case "n":
                    return 0x152;
                case "o":
                    return 0x153;
                case "p":
                    return 0x154;
                case "q":
                    return 0x155;
                case "r":
                    return 0x156;
                case "s":
                    return 0x157;
                case "t":
                    return 0x158;
                case "u":
                    return 0x159;
                case "v":
                    return 0x15A;
                case "w":
                    return 0x15B;
                case "x":
                    return 0x15C;
                case "y":
                    return 0x15D;
                case "z":
                    return 0x15E;
                case "À":
                    return 0x15F;
                case "Á":
                    return 0x160;
                case "Â":
                    return 0x161;
                case "\x0162":
                    return 0x162;
                case "Ä":
                    return 0x163;
                case "\x0164":
                    return 0x164;
                case "\x0165":
                    return 0x165;
                case "Ç":
                    return 0x166;
                case "È":
                    return 0x167;
                case "É":
                    return 0x168;
                case "Ê":
                    return 0x169;
                case "Ë":
                    return 0x16A;
                case "Ì":
                    return 0x16B;
                case "Í":
                    return 0x16C;
                case "Î":
                    return 0x16D;
                case "Ï":
                    return 0x16E;
                case "\x016F":
                    return 0x16F;
                case "Ñ":
                    return 0x170;
                case "Ò":
                    return 0x171;
                case "Ó":
                    return 0x172;
                case "Ô":
                    return 0x173;
                case "\x0174":
                    return 0x174;
                case "Ö":
                    return 0x175;
                case "×":
                    return 0x176;
                case "\x0177":
                    return 0x177;
                case "Ù":
                    return 0x178;
                case "Ú":
                    return 0x179;
                case "Û":
                    return 0x17A;
                case "Ü":
                    return 0x17B;
                case "\x017C":
                    return 0x17C;
                case "\x017D":
                    return 0x17D;
                case "ß":
                    return 0x17E;
                case "à":
                    return 0x17F;
                case "á":
                    return 0x180;
                case "â":
                    return 0x181;
                case "\x0182":
                    return 0x182;
                case "ä":
                    return 0x183;
                case "\x0184":
                    return 0x184;
                case "\x0185":
                    return 0x185;
                case "ç":
                    return 0x186;
                case "è":
                    return 0x187;
                case "é":
                    return 0x188;
                case "ê":
                    return 0x189;
                case "ë":
                    return 0x18A;
                case "ì":
                    return 0x18B;
                case "í":
                    return 0x18C;
                case "î":
                    return 0x18D;
                case "ï":
                    return 0x18E;
                case "\x018F":
                    return 0x18F;
                case "ñ":
                    return 0x190;
                case "ò":
                    return 0x191;
                case "ó":
                    return 0x192;
                case "ô":
                    return 0x193;
                case "\x0194":
                    return 0x194;
                case "ö":
                    return 0x195;
                case "÷":
                    return 0x196;
                case "\x0197":
                    return 0x197;
                case "ù":
                    return 0x198;
                case "ú":
                    return 0x199;
                case "û":
                    return 0x19A;
                case "ü":
                    return 0x19B;
                case "\x019C":
                    return 0x19C;
                case "\x019D":
                    return 0x19D;
                case "\x019E":
                    return 0x19E;
                case "Œ":
                    return 0x19F;
                case "œ":
                    return 0x1A0;
                case "\x01A1":
                    return 0x1A1;
                case "\x01A2":
                    return 0x1A2;
                case "ª":
                    return 0x1A3;
                case "º":
                    return 0x1A4;
                case "ᵉʳ":
                    return 0x1A5;
                case "ʳᵉ":
                    return 0x1A6;
                case "ʳ":
                    return 0x1A7;
                case "¥":
                    return 0x1A8;
                case "¡":
                    return 0x1A9;
                case "¿":
                    return 0x1AA;
                case "!":
                    return 0x1AB;
                case "?":
                    return 0x1AC;
                case ",":
                    return 0x1AD;
                case ".":
                    return 0x1AE;
                case "...":
                    return 0x1AF;
                case "·":
                    return 0x1B0;
                case "/":
                    return 0x1B1;
                case "‘":
                    return 0x1B2;
                case "\'":
                    return 0x1B3;
                case "“":
                    return 0x1B4;
                case "”":
                    return 0x1B5;
                case "„":
                    return 0x1B6;
                case "«":
                    return 0x1B7;
                case "»":
                    return 0x1B8;
                case "(":
                    return 0x1B9;
                case ")":
                    return 0x1BA;
                case "♂":
                    return 0x1BB;
                case "♀":
                    return 0x1BC;
                case "+":
                    return 0x1BD;
                case "-":
                    return 0x1BE;
                case "*":
                    return 0x1BF;
                case "#":
                    return 0x1C0;
                case "=":
                    return 0x1C1;
                case "\and":
                    return 0x1C2;
                case "~":
                    return 0x1C3;
                case ":":
                    return 0x1C4;
                case ";":
                    return 0x1C5;
                case "♠":
                    return 0x1C6;
                case "♣":
                    return 0x1C7;
                case "♥":
                    return 0x1C8;
                case "♦":
                    return 0x1C9;
                case "★":
                    return 0x1CA;
                case "◎":
                    return 0x1CB;
                case "○":
                    return 0x1CC;
                case "□":
                    return 0x1CD;
                case "△":
                    return 0x1CE;
                case "◇":
                    return 0x1CF;
                case "@":
                    return 0x1D0;
                case "♪":
                    return 0x1D1;
                case "%":
                    return 0x1D2;
                case "☀":
                    return 0x1D3;
                case "☁":
                    return 0x1D4;
                case "☂":
                    return 0x1D5;
                case "☃":
                    return 0x1D6;
                case " ":
                    return 0x1DE;
                case "/n":
                    return 0xE000;
                case "/r":
                    return 0x25BC;
                case "/v":
                    return 0x25BD;
                case "[]":
                    return 0xFFFE;
                case "/0":
                    return 0xFFFF;
                case "POKE:":
                    return 0x100;
                case "POKE2:":
                    return 0x101;
                case "NAME:":
                    return 0x103;
                case "PLACE:":
                    return 0x104;
                case "MOVE:":
                    return 0x106;
                case "NAT:":
                    return 0x107;
                case "ITEM:":
                    return 0x108;
                case "POFFIN":
                    return 0x10A;
                case "TRAINER:":
                    return 0x10E;
                case "KEY":
                    return 0x118;
                case "ACC.:":
                    return 0x11F;
                case "NUM:":
                    return 0x132;
                case "LEVEL:":
                    return 0x133;
                case "NUM2:":
                    return 0x134;
                case "NUM3:":
                    return 0x135;
                case "MONEY:":
                    return 0x137;
                case "COLOR:":
                    return 0x203;
                default:
                    return 0;
            }
        }

        public void readText(BinaryReader reader, RichTextBox textBox)
        {
            numTextFile = reader.ReadInt16();
            //textBox.AppendText("Il file è lungo " + reader.BaseStream.Length + "\nVi sono: " + numTextFile.ToString());
            int key = reader.ReadInt16();
            originalKey = new List<Int32>();
            originalKey.Add(key);
            key = (key * 0x2FD) & 0xffff;
            //textBox.AppendText("\nKey: " + key.ToString("X"));

            textList = new List<TextStruct>();
            originalOffsets = new ArrayList();
            originalSizes = new List<Int32>();
            for (int i = 0; i < numTextFile; i++)
            {

                var actualTextFile = new TextStruct();
                actualTextFile.key2 = (key * (i + 1) & 0xffff);
                actualTextFile.realKey = actualTextFile.key2 | (actualTextFile.key2 << 16);
                var startOffset = reader.ReadInt32();
                actualTextFile.startOffset = startOffset ^ actualTextFile.realKey;
                originalOffsets.Add(startOffset);
                var size = reader.ReadInt32();
                actualTextFile.size = size ^ actualTextFile.realKey;
                originalSizes.Add(size);
                textList.Add(actualTextFile);

            }
            //textBox.AppendText("\nCi troviamo in posizione: " + reader.BaseStream.Position);
            
            for (int i = 0; i < numTextFile; i++)
            {
                var actualTextFile = textList[i];
                key = (0x91BD3*(i+1))&0xffff;
                int count = 0;
                isVar = false;
                StringBuilder str = new StringBuilder();
                for (int k=0; k<actualTextFile.size; k++){
                    if (count == 4)
                    {
                        str.Append("] ");
                        isVar = false;
                        count = 0;
                    }
                    var car = getCharacter(reader.ReadUInt16() ^ key);
                    if (car == "[]")
                    {
                        str.Append("[VAR ");
                        isVar = true;
                    }
                    else
                        str.Append(car);
                    
                    if (isVar)
                        count++;

                    key +=0x493D;
                    key &= 0xffff;
                }
                actualTextFile.text = str.ToString();
                textBox.AppendText("Message " + i + " :'" + actualTextFile.text.ToString());
                textBox.AppendText("'\n");

                textList[i] = actualTextFile;
            }

        }

        private string getCharacter(int p)
        {
            switch(p){
                case 0x0:
                    return ".0 ";
                case 0x1:
                    return ".1 ";
                case 0xA2:
                    return "０";
                case 0xA3:
                    return "１";
                case 0xA4:
                    return "２";
                case 0xA5:
                    return "３";
                case 0xA6:
                    return "４";
                case 0xA7:
                    return "５";
                case 0xA8:
                    return "６";
                case 0xA9:
                    return "７";
                case 0xAA:
                    return "８";
                case 0xAB:
                    return "９";
                case 0xAC:
                    return "Ａ";
                case 0xAD:
                    return "Ｂ";
                case 0xAE:
                    return "Ｃ";
                case 0xAF:
                    return "Ｄ";
                case 0xB0:
                    return "Ｅ";
                case 0xB1:
                    return "Ｆ";
                case 0xB2:
                    return "Ｇ";
                case 0xB3:
                    return "Ｈ";
                case 0xB4:
                    return "Ｉ";
                case 0xB5:
                    return "Ｊ";
                case 0xB6:
                    return "Ｋ";
                case 0xB7:
                    return "Ｌ";
                case 0xB8:
                    return "Ｍ";
                case 0xB9:
                    return "Ｎ";
                case 0xBA:
                    return "Ｏ";
                case 0xBB:
                    return "Ｐ";
                case 0xBC:
                    return "Ｑ";
                case 0xBD:
                    return "Ｒ";
                case 0xBE:
                    return "Ｓ";
                case 0xBF:
                    return "Ｔ";
                case 0xC0:
                    return "Ｕ";
                case 0xC1:
                    return "Ｖ";
                case 0xC2:
                    return "Ｗ";
                case 0xC3:
                    return "Ｘ";
                case 0xC4:
                    return "Ｙ";
                case 0xC5:
                    return "Ｚ";
                case 0xC6:
                    return "ａ";
                case 0xC7:
                    return "ｂ";
                case 0xC8:
                    return "c";
                case 0xC9:
                    return "d";
                case 0xCA:
                    return "e";
                case 0xCB:
                    return "f";;
                case 0xCC:
                    return "g";
                case 0xCD:
                    return "h";
                case 0xCE:
                    return "i";
                case 0xCF:
                    return "j";
                case 0xD0:
                    return "k";
                case 0xD1:
                    return "l";
                case 0xD2:
                    return "m";
                case 0xD3:
                    return "n";
                case 0xD4:
                    return "o";
                case 0xD5:
                    return "p";
                case 0xD6:
                    return "q";
                case 0xD7:
                    return "r";
                case 0xD8:
                    return "s";
                case 0xD9:
                    return "t";
                case 0xDA:
                    return "u";
                case 0xDB:
                    return "v";
                case 0xDC:
                    return "w";
                case 0xDD:
                    return "x";
                case 0xDE:
                    return "y";
                case 0xDF:
                    return "z";
                case 0x100:
                    if (isVar)
                        return "POKE: ";
                    break;
                case 0x101:
                    if (isVar)
                        return "POKE2: ";
                    break;
                case 0x103:
                    if (isVar)
                        return "NAME: ";
                    break;
                case 0x104:
                    if (isVar)
                        return "PLACE: ";
                    break;
                case 0x106:
                    if (isVar)
                        return "MOVE: ";
                    break;
                case 0x107:
                    if (isVar)
                        return "NAT: ";
                    break;
                case 0x108:
                    if (isVar)
                        return "ITEM: ";
                    break;
                case 0x10A:
                    if (isVar)
                        return "POFFIN ITEM: ";
                    break;
                case 0x10E:
                    if (isVar)
                        return "TRAINER: ";
                    break;
                case 0x118:
                    if (isVar)
                        return "KEY ITEM: ";
                    break;
                case 0x11F:
                    if (isVar)
                        return "ACC.: ";
                    break;
                case 0x121:
                    return "0";
                case 0x122:
                    return "1";
                case 0x123:
                    return "2";
                case 0x124:
                    return "3";
                case 0x125:
                    return "4";
                case 0x126:
                    return "5";
                case 0x127:
                    return "6";
                case 0x128:
                    return "7";
                case 0x129:
                    return "8";
                case 0x12A:
                    return "9";
                case 0x12B:
                    return "A"; ;
                case 0x12C:
                    return "B";
                case 0x12D:
                    return "C"; ;
                case 0x12E:
                    return "D";
                case 0x12F:
                    return "E";
                case 0x130:
                    return "F";
                case 0x131:
                    return "G";
                case 0x132:
                    if (isVar)
                        return "NUM: ";
                    return "H";
                case 0x133:
                    if (isVar)
                        return "LEVEL: ";
                    return "I";
                case 0x134:
                    if (isVar)
                        return "NUM2: ";
                    return "J";
                case 0x135:
                    if (isVar)
                        return "NUM3: ";
                    return "K";
                case 0x136:
                    return "L";
                case 0x137:
                    if (isVar)
                        return "MONEY: ";
                    return "M";
                case 0x138:
                    return "N";
                case 0x139:
                    return "O";
                case 0x13A:
                    return "P";
                case 0x13B:
                    return "Q";
                case 0x13C:
                    return "R";
                case 0x13D:
                    return "S";
                case 0x13E:
                    return "T";
                case 0x13F:
                    return "U";
                case 0x140:
                    return "V";
                case 0x141:
                    return "W";
                case 0x142:
                    return "X";
                case 0x143:
                    return "Y";
                case 0x144:
                    return "Z";
                case 0x145:
                    return "a";
                case 0x146:
                    return "b";
                case 0x147:
                    return "c";
                case 0x148:
                    return "d";
                case 0x149:
                    return "e";
                case 0x14A:
                    return "f";
                case 0x14B:
                    return "g";
                case 0x14C:
                    return "h";
                case 0x14D:
                    return "i";
                case 0x14E:
                    return "j";
                case 0x14F:
                    return "k";
                case 0x150:
                    return "l";
                case 0x151:
                    return "m";
                case 0x152:
                    return "n";
                case 0x153:
                    return "o";
                case 0x154:
                    return "p";
                case 0x155:
                    return "q";
                case 0x156:
                    return "r";
                case 0x157:
                    return "s";
                case 0x158:
                    return "t";
                case 0x159:
                    return "u";
                case 0x15A:
                    return "v";
                case 0x15B:
                    return "w";
                case 0x15C:
                    return "x";
                case 0x15D:
                    return "y";
                case 0x15E:
                    return "z";
                case 0x15F:
                    return "À";
                case 0x160:
                    return "Á";
                case 0x161:
                    return "Â";
                case 0x162:
                    return "\x0162";
                case 0x163:
                    return "Ä";
                case 0x164:
                    return "\x0164";
                case 0x165:
                    return "\x0165";
                case 0x166:
                    return "Ç";
                case 0x167:
                    return "È";
                case 0x168:
                    return "É";
                case 0x169:
                    return "Ê";
                case 0x16A:
                    return "Ë";
                case 0x16B:
                    return "Ì";
                case 0x16C:
                    return "Í";
                case 0x16D:
                    return "Î";
                case 0x16E:
                    return "Ï";
                case 0x16F:
                    return "\x016F";
                case 0x170:
                    return "Ñ";
                case 0x171:
                    return "Ò";
                case 0x172:
                    return "Ó";
                case 0x173:
                    return "Ô";
                case 0x174:
                    return "\x0174";
                case 0x175:
                    return "Ö";
                case 0x176:
                    return "×";
                case 0x177:
                    return "\x0177";
                case 0x178:
                    return "Ù";
                case 0x179:
                    return "Ú";
                case 0x17A:
                    return "Û";
                case 0x17B:
                    return "Ü";
                case 0x17C:
                    return "\x017C";
                case 0x17D:
                    return "\x017D";
                case 0x17E:
                    return "ß";
                case 0x17F:
                    return "à";
                case 0x180:
                    return "á";
                case 0x181:
                    return "â";
                case 0x182:
                    return "\x0182";
                case 0x183:
                    return "ä";
                case 0x184:
                    return "\x0184";
                case 0x185:
                    return "\x0185";
                case 0x186:
                    return "ç";
                case 0x187:
                    return "è";
                case 0x188:
                    return "é";
                case 0x189:
                    return "ê";
                case 0x18A:
                    return "ë";
                case 0x18B:
                    return "ì";
                case 0x18C:
                    return "í";
                case 0x18D:
                    return "î";
                case 0x18E:
                    return "ï";
                case 0x18F:
                    return "\x018F";
                case 0x190:
                    return "ñ";
                case 0x191:
                    return "ò";
                case 0x192:
                    return "ó";
                case 0x193:
                    return "ô";
                case 0x194:
                    return "\x0194";
                case 0x195:
                    return "ö";
                case 0x196:
                    return "÷";
                case 0x197:
                    return "\x0197";
                case 0x198:
                    return "ù";
                case 0x199:
                    return "ú";
                case 0x19A:
                    return "û";
                case 0x19B:
                    return "ü";
                case 0x19C:
                    return "\x019C";
                case 0x19D:
                    return "\x019D";
                case 0x19E:
                    return "\x019E";
                case 0x19F:
                    return "Œ";
                case 0x1A0:
                    return "œ";
                case 0x1A1:
                    return "\x01A1";
                case 0x1A2:
                    return "\x01A2";
                case 0x1A3:
                    return "ª";
                case 0x1A4:
                    return "º";
                case 0x1A5:
                    return "ᵉʳ";
                case 0x1A6:
                    return "ʳᵉ";
                case 0x1A7:
                    return "ʳ";
                case 0x1A8:
                    return "¥";
                case 0x1A9:
                    return "¡";
                case 0x1AA:
                    return "¿";
                case 0x1AB:
                    return "!";
                case 0x1AC:
                    return "?";
                case 0x1AD:
                    return ",";
                case 0x1AE:
                    return ".";
                case 0x1AF:
                    return "...";
                case 0x1B0:
                    return "·";
                case 0x1B1:
                    return "/";
                case 0x1B2:
                    return "‘";
                case 0x1B3:
                    return "'";
                case 0x1B4:
                    return "“";
                case 0x1B5:
                    return "”";
                case 0x1B6:
                    return "„";
                case 0x1B7:
                    return "«";
                case 0x1B8:
                    return "»";
                case 0x1B9:
                    return "(";
                case 0x1BA:
                    return ")";
                case 0x1BB:
                    return "♂";
                case 0x1BC:
                    return "♀";
                case 0x1BD:
                    return "+";
                case 0x1BE:
                    return "-";
                case 0x1BF:
                    return "*";
                case 0x1C0:
                    return "#";
                case 0x1C1:
                    return "=";
                case 0x1C2:
                    return "\and";
                case 0x1C3:
                    return "~";
                case 0x1C4:
                    return ":";
                case 0x1C5:
                    return ";";
                case 0x1C6:
                    return "„";
                case 0x1C7:
                    return "«";
                case 0x1C8:
                    return "»";
                case 0x1C9:
                    return "(";
                case 0x1CA:
                    return ")";
                case 0x1CB:
                    return "♂";
                case 0x1CC:
                    return "♀";
                case 0x1CD:
                    return "+";
                case 0x1CE:
                    return "-";
                case 0x1CF:
                    return "*";
                case 0x1D2:
                    return "%";
                case 0x1DE:
                    return " ";
                case 0x203:
                    if (isVar)
                        return "COLOR: ";
                    break;
                case 0xE000:
                    return "/n";
                case 0x25BC:
                    return "/r";
                case 0x25BD:
                    return "/v";
                case 0xFFFE:
                    return "[]";
                case 0xFFFF:
                    return "/0";

            }   
            return p.ToString() + " ";

        }

        public void readTextBW(BinaryReader reader, int i, RichTextBox textBox)
        {
            messageList  = new List<String>();
            if (i >= 0)
            {
                UInt16 numSections, numEntries, tmpCharCount, tmpUnknown, tmpChar;
                UInt32 unk1, tmpOffset;
                UInt32[] sizeSections = { 0, 0, 0 };
                UInt32[] sectionOffset = { 0, 0, 0 };
                Dictionary<int, List<UInt32>> tableOffsets = new Dictionary<int,List<UInt32>>();
                Dictionary<int, List<UInt16>> characterCount = new Dictionary<int,List<UInt16>>();
                Dictionary<int, List<UInt16>> unknown= new Dictionary<int,List<UInt16>>();
                Dictionary<int, List<List<UInt16>>> encText = new Dictionary<int,List<List<UInt16>>>();
                Dictionary<int, List<List<String>>> decText = new Dictionary<int,List<List<String>>>();
                String stringa;
                UInt16 key;

                numSections = reader.ReadUInt16();
                numEntries = reader.ReadUInt16();
                sizeSections[0] = reader.ReadUInt32();
                unk1 = reader.ReadUInt32();

                if (numSections > i)
                {
                    for (int z = 0; z < numSections; z++)
                        sectionOffset[z] = reader.ReadUInt32();
                    reader.BaseStream.Position = sectionOffset[i];
                    sizeSections[i] = reader.ReadUInt32();
                    for(int j=0;j<numEntries;j++)
                    {
                        tmpOffset = reader.ReadUInt32();
                        tmpCharCount = reader.ReadUInt16();
                        tmpUnknown = reader.ReadUInt16();
                        if (!tableOffsets.ContainsKey(i))
                            tableOffsets.Add(i, new List<uint>());
                        tableOffsets[i].Add(tmpOffset);
                        if (!characterCount.ContainsKey(i))
                            characterCount.Add(i, new List<UInt16>());
                        characterCount[i].Add(tmpCharCount);
                        if (!unknown.ContainsKey(i))
                            unknown.Add(i, new List<ushort>());
                        unknown[i].Add(tmpUnknown);
                        unknowns.Add(tmpUnknown);
                    }
                    for (int j = 0; j < numEntries; j++)
                    {
                        List<UInt16> tmpEncChars = new List<UInt16>();
                        reader.BaseStream.Position = sectionOffset[i] + tableOffsets[i][j];
                        for (int k = 0; k < characterCount[i][j]; k++)
                            tmpEncChars.Add(reader.ReadUInt16());
                        if (!encText.ContainsKey(i))
                            encText.Add(i, new List<List<ushort>>());
                        encText[i].Add(tmpEncChars);
                        key = (ushort)(encText[i][j][characterCount[i][j] - 1] ^ 0xFFFF);
                        for (int k = characterCount[i][j] - 1; k >= 0; k--)
                        {
                            encText[i][j][k] ^= key;
                            if (k == 0)
                                keys.Add(key);
                            key = (ushort)(((key >> 3) | (key << 13)) & 0xffff);
                        }

                        List<String> chars = new List<String>();
                        stringa = "";
                        count = 0;
                        for (int k = 0; k < characterCount[i][j]; k++)
                        {
                            var actualChar = encText[i][j][k];
                            if (encText[i][j][k] == 0xFFFF)
                            {
                                chars.Add("\\xffff");
                            }
                            else
                            {
                                //if((encText[i][j][k]<0xF000&&encText[i][j][k]>3) || (encText[i][j][k]>=0xFF00 && ((encText[i][j][k]&0xFF)>1&&(encText[i][j][k]&0xFF)<0xF0)))
                                //if (encText[i][j][k] > 20 && encText[i][j][k] <= 0xFFF0
                                //    && encText[i][j][k] != 0xF000 && encText[i][j][k] != 0xBE00 && encText[i][j][k] != 0xBE01)
                                ////&& Encoding.GetEncoding(encText[i][j][k])!=Encoding.Unicode)

                                //else
                                //{
                                String num = encText[i][j][k].ToString();
                                //int length = num.Length;
                                //for (int l = 0; l < 4 - length; l++)
                                //    num = "0" + num;
                                switch (encText[i][j][k])
                                {
                                    case 0xFFFE:
                                        chars.Add("\\n");
                                        stringa += "\\n";
                                        break;
                                    case 0xF000:
                                        chars.Add("[VAR ");
                                        stringa += "[VAR ";
                                        isVar = true;
                                        if (encText[i][j][k + 1] == 0xBE00)
                                        {
                                            stringa = stringa.Substring(0, stringa.Length - 5);
                                            chars.Add("\\c");
                                            stringa += "\\c";
                                            isVar = false;
                                            k += 2;
                                        }
                                        else if (encText[i][j][k + 1] == 0xBE01)
                                        {
                                            stringa = stringa.Substring(0, stringa.Length - 5);
                                            chars.Add("\\r");
                                            stringa += "\\r";
                                            isVar = false;
                                            k += 2;
                                        }
                                        else if (encText[i][j][k + 1].ToString() == "256")
                                        {
                                            count++;
                                            stringa += "NAME: ";
                                            isVar = true;
                                            k++;
                                        }
                                        else if (encText[i][j][k + 1].ToString() == "257")
                                        {
                                            count++;
                                            stringa += "POKE: ";
                                            isVar = true;
                                            k++;
                                        }
                                        else if (encText[i][j][k + 1].ToString() == "258")
                                        {
                                            count++;
                                            stringa += "NICK: ";
                                            isVar = true;
                                            k++;
                                        }
                                        else if (encText[i][j][k + 1].ToString() == "259")
                                        {
                                            count++;
                                            stringa += "TYPE: ";
                                            isVar = true;
                                            k++;
                                        }
                                        else if (encText[i][j][k + 1].ToString() == "265")
                                        {
                                            count++;
                                            stringa += "ITEM: ";
                                            isVar = true;
                                            k++;
                                        }
                                        else if (encText[i][j][k + 1].ToString() == "512")
                                        {
                                            count++;
                                            stringa += "SAGE NUM: ";
                                            isVar = true;
                                            k++;
                                        }
                                        else if (encText[i][j][k + 1].ToString() == "516")
                                        {
                                            count++;
                                            stringa += "NUM: ";
                                            isVar = true;
                                            k++;
                                        }
                                        else if (encText[i][j][k + 1].ToString() == "65280")
                                        {
                                            count++;
                                            stringa += "COLOR: ";
                                            isVar = true;
                                            k++;
                                        }
                                        break;
                                    default:
                                        if (isVar)
                                        {
                                            if (count < 2)
                                            {
                                                chars.Add(num.ToString() + " ");
                                                stringa += num;
                                                count++;
                                            }
                                            else if (count == 2)
                                            {
                                                chars.Add(" " + num.ToString() + "]");
                                                stringa += " " + num.ToString() + "]";
                                                count = 0;
                                                isVar = false;
                                            }
                                        }
                                        else
                                        {
                                            chars.Add((Convert.ToChar(encText[i][j][k])).ToString());
                                            stringa += Convert.ToChar(encText[i][j][k]).ToString();
                                        }
                                        break;
                                }
                            }
                        }
                        //}
                        messageList.Add(stringa);
                        if (!decText.ContainsKey(i))
                            decText.Add(i, new List<List<string>>());
                        decText[i].Add(chars);
                    }
            }
        }
            for (i = 0; i < messageList.Count; i++ )
                textBox.AppendText("\nMessage " + i + ":" + messageList[i]);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && ( components != null))
            {
                 components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.textBox = new System.Windows.Forms.RichTextBox();
            this.listText = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.narcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.narcToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(12, 63);
            this.textBox.Name = "textBox";
            this.textBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.textBox.Size = new System.Drawing.Size(973, 604);
            this.textBox.TabIndex = 0;
            this.textBox.Text = "";
            // 
            // listText
            // 
            this.listText.FormattingEnabled = true;
            this.listText.Location = new System.Drawing.Point(12, 36);
            this.listText.Name = "listText";
            this.listText.Size = new System.Drawing.Size(121, 21);
            this.listText.TabIndex = 2;
            this.listText.SelectedIndexChanged += new System.EventHandler(this.listText_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(997, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.textToolStripMenuItem,
            this.narcToolStripMenuItem});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // textToolStripMenuItem
            // 
            this.textToolStripMenuItem.Name = "textToolStripMenuItem";
            this.textToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.textToolStripMenuItem.Text = "Text";
            this.textToolStripMenuItem.Click += new System.EventHandler(this.textToolStripMenuItem_Click);
            // 
            // narcToolStripMenuItem
            // 
            this.narcToolStripMenuItem.Name = "narcToolStripMenuItem";
            this.narcToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.narcToolStripMenuItem.Text = "Narc";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.textToolStripMenuItem1,
            this.narcToolStripMenuItem1});
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // textToolStripMenuItem1
            // 
            this.textToolStripMenuItem1.Name = "textToolStripMenuItem1";
            this.textToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.textToolStripMenuItem1.Text = "E";
            this.textToolStripMenuItem1.Click += new System.EventHandler(this.textToolStripMenuItem1_Click);
            // 
            // narcToolStripMenuItem1
            // 
            this.narcToolStripMenuItem1.Name = "narcToolStripMenuItem1";
            this.narcToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.narcToolStripMenuItem1.Text = "Narc";
            // 
            // Texts
            // 
            this.ClientSize = new System.Drawing.Size(997, 688);
            this.Controls.Add(this.listText);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Texts";
            this.Text = "Texts";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void listText_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox.Clear();
            int idFile = listText.SelectedIndex;
            var actualText = textNarc.figm.fileData[idFile];
            var reader = new BinaryReader(actualText);
            reader.BaseStream.Position = 0;
            if (type == 0)
                readText(reader, textBox);
            else
                readTextBW(reader, 0, textBox);
        }

        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Text files (*.text)|*.text";
            if (fileDialog.ShowDialog() != DialogResult.Cancel)
            {
                var actualText = fileDialog.OpenFile();
                var checkOrigin = new IsBWDialog();
                checkOrigin.ShowDialog();
                var type = checkOrigin.CheckGame();
                var reader = new BinaryReader(actualText);
                reader.BaseStream.Position = 0;
                if (type == 0)
                    readText(reader,textBox);
                else
                    readTextBW(reader, 0,textBox);
                actualText.Close();
            }
        }

        private void textToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var fileDialog = new SaveFileDialog();
            fileDialog.Filter = "text files (*.text)|*.text";
            if (fileDialog.ShowDialog() != DialogResult.Cancel)
            {
                var file = fileDialog.OpenFile();
                saveText(new BinaryWriter(file), textBox.Lines);
                file.Close();
            }
        } 


    }
}

