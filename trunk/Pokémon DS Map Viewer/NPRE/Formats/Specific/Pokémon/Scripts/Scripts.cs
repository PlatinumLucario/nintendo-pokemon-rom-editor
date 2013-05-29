using NPRE.Formats.Specific.Pokémon.Scripts;
using System.Runtime.InteropServices;

namespace PG4Map.Formats
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class Scripts : Form
    {
        private IContainer components;
        public List<Script_s> scriptList;
        private List<Script_s> functionList;
        private List<uint> functionOffsetList = new List<uint>();
        private List<List<Mov_s>> movList;
        private List<uint> movOffsetList = new List<uint>();
        private List<uint> scriptOffList = new List<uint>();
        private List<uint> scriptStartList = new List<uint>();
        private Narc actualNarc;
        private ComboBox listScript;
        public RichTextBox Console;
        public static int scriptType;
        private int lastIndexFunction = 0;
        private int lastIndexMov;
        public const short DPSCRIPT = 0;
        public const short HGSSSCRIPT = 2;
        public const short BWSCRIPT = 3;
        public const short PLSCRIPT = 1;
        public const short BW2SCRIPT = 4;
        private Button button1;
        private TextBox commandToSearch;
        private Button button2;
        public RichTextBox SearchBox;
        public const short BWJSCRIPT = 4;
        public int actualFileId = 0;
        private Texts textFile;
        private Narc textNarc;
        private Button button3;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem scriptToolStripMenuItem;
        private ToolStripMenuItem scriptToolStripMenuItem1;
        private ToolStripMenuItem narcToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem scriptToolStripMenuItem2;
        private ToolStripMenuItem narcToolStripMenuItem1;
        private int idMessage;
        private int idTextFile;
        private Button button4;
        private List<uint> scriptOrder;
        private Main main;
        private Texts textHandler;
        private ClosableMemoryStream mapMatrixStream;
        private ClosableMemoryStream scriptStream;
        private int romType;
        public RichTextBox scriptBoxViewer;
        public RichTextBox scriptBoxEditor;
        private List<tableRow> actualTable;
        private ComboBox subScripts;
        private List<Dictionary<int, string>> varNameDictionaryList;
        private TextBox textBox1;
        private Button button5;
        private List<int> visitedLine = new List<int>();

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.listScript = new System.Windows.Forms.ComboBox();
            this.Console = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.commandToSearch = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SearchBox = new System.Windows.Forms.RichTextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.narcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.narcToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.button4 = new System.Windows.Forms.Button();
            this.scriptBoxViewer = new System.Windows.Forms.RichTextBox();
            this.scriptBoxEditor = new System.Windows.Forms.RichTextBox();
            this.subScripts = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listScript
            // 
            this.listScript.FormattingEnabled = true;
            this.listScript.Location = new System.Drawing.Point(12, 28);
            this.listScript.Name = "listScript";
            this.listScript.Size = new System.Drawing.Size(121, 21);
            this.listScript.TabIndex = 1;
            this.listScript.SelectedIndexChanged += new System.EventHandler(this.listScript_SelectedIndexChanged);
            // 
            // Console
            // 
            this.Console.Location = new System.Drawing.Point(595, 569);
            this.Console.Name = "Console";
            this.Console.Size = new System.Drawing.Size(627, 161);
            this.Console.TabIndex = 2;
            this.Console.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(251, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Dump All";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // commandToSearch
            // 
            this.commandToSearch.Location = new System.Drawing.Point(543, 34);
            this.commandToSearch.Name = "commandToSearch";
            this.commandToSearch.Size = new System.Drawing.Size(100, 20);
            this.commandToSearch.TabIndex = 4;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(462, 31);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Search";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // SearchBox
            // 
            this.SearchBox.Location = new System.Drawing.Point(12, 569);
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.Size = new System.Drawing.Size(556, 161);
            this.SearchBox.TabIndex = 6;
            this.SearchBox.Text = "";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1147, 31);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "Info Script";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1243, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scriptToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.saveAsToolStripMenuItem.Text = "File";
            // 
            // scriptToolStripMenuItem
            // 
            this.scriptToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scriptToolStripMenuItem1,
            this.narcToolStripMenuItem});
            this.scriptToolStripMenuItem.Name = "scriptToolStripMenuItem";
            this.scriptToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.scriptToolStripMenuItem.Text = "Open";
            // 
            // scriptToolStripMenuItem1
            // 
            this.scriptToolStripMenuItem1.Name = "scriptToolStripMenuItem1";
            this.scriptToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.scriptToolStripMenuItem1.Text = "Script";
            this.scriptToolStripMenuItem1.Click += new System.EventHandler(this.scriptToolStripMenuItem1_Click);
            // 
            // narcToolStripMenuItem
            // 
            this.narcToolStripMenuItem.Name = "narcToolStripMenuItem";
            this.narcToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.narcToolStripMenuItem.Text = "Narc";
            this.narcToolStripMenuItem.Click += new System.EventHandler(this.narcToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scriptToolStripMenuItem2,
            this.narcToolStripMenuItem1});
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // scriptToolStripMenuItem2
            // 
            this.scriptToolStripMenuItem2.Name = "scriptToolStripMenuItem2";
            this.scriptToolStripMenuItem2.Size = new System.Drawing.Size(104, 22);
            this.scriptToolStripMenuItem2.Text = "Script";
            this.scriptToolStripMenuItem2.Click += new System.EventHandler(this.scriptToolStripMenuItem2_Click);
            // 
            // narcToolStripMenuItem1
            // 
            this.narcToolStripMenuItem1.Name = "narcToolStripMenuItem1";
            this.narcToolStripMenuItem1.Size = new System.Drawing.Size(104, 22);
            this.narcToolStripMenuItem1.Text = "Narc";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(155, 26);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 9;
            this.button4.Text = "Save";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // scriptBoxViewer
            // 
            this.scriptBoxViewer.Location = new System.Drawing.Point(12, 87);
            this.scriptBoxViewer.Name = "scriptBoxViewer";
            this.scriptBoxViewer.Size = new System.Drawing.Size(654, 461);
            this.scriptBoxViewer.TabIndex = 0;
            this.scriptBoxViewer.Text = "";
            // 
            // scriptBoxEditor
            // 
            this.scriptBoxEditor.Location = new System.Drawing.Point(672, 87);
            this.scriptBoxEditor.Name = "scriptBoxEditor";
            this.scriptBoxEditor.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.scriptBoxEditor.Size = new System.Drawing.Size(550, 461);
            this.scriptBoxEditor.TabIndex = 10;
            this.scriptBoxEditor.Text = "";
            this.scriptBoxEditor.WordWrap = false;
            // 
            // subScripts
            // 
            this.subScripts.FormattingEnabled = true;
            this.subScripts.Location = new System.Drawing.Point(672, 33);
            this.subScripts.Name = "subScripts";
            this.subScripts.Size = new System.Drawing.Size(121, 21);
            this.subScripts.TabIndex = 11;
            this.subScripts.SelectedIndexChanged += new System.EventHandler(this.subScripts_SelectedIndexChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 55);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 12;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(347, 26);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(94, 23);
            this.button5.TabIndex = 13;
            this.button5.Text = "Dump All (Ext)";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // Scripts
            // 
            this.ClientSize = new System.Drawing.Size(1243, 742);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.subScripts);
            this.Controls.Add(this.scriptBoxEditor);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.SearchBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.commandToSearch);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Console);
            this.Controls.Add(this.listScript);
            this.Controls.Add(this.scriptBoxViewer);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Scripts";
            this.Text = "Scripts";
            this.Load += new System.EventHandler(this.Scripts_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #region Constructors


        public Scripts()
        {
            components = null;
            InitializeComponent();
        }

        public Scripts(Stream actualNode)
        {
            components = null;
            InitializeComponent();
            loadScript(actualNode);
        }

        public Scripts(ClosableMemoryStream actualNode, Texts textFile, int idTextFile)
        {
            components = null;
            InitializeComponent();
            this.textFile = textFile;
            this.idTextFile = idTextFile;
            UpdateType();
            //initAssTable(main.actualNds.getHeader().getTitle(), main.actualNds.getHeader().getCode());
            loadScript(actualNode);

        }

        public Scripts(Narc actualNarc, Narc textNarc, Main main)
        {
            InitializeComponent();
            this.actualNarc = actualNarc;
            this.textNarc = textNarc;
            this.main = main;
            initAssTable(main.actualNds.getHeader().getTitle(), main.actualNds.getHeader().getCode());
            ReactionNarc();
        }



        private void ReactionNarc()
        {
            UpdateType();
            listScript.Enabled = true;
            GenerateScriptList();
        }

        #endregion

        Boolean normalScript;
        private Texts multiFile;

        private void loadScript(Stream actualNode)
        {

            scriptBoxViewer.Clear();
            scriptBoxEditor.Clear();
            Console.Clear();
            if (actualNode.Length < 2)
                return;
            var reader = new BinaryReader(actualNode);
            reader.BaseStream.Position = 0;

            scriptList = new List<Script_s>();
            functionList = new List<Script_s>();
            functionOffsetList = new List<uint>();
            movOffsetList = new List<uint>();
            movList = new List<List<Mov_s>>();
            scriptOffList = new List<uint>();
            scriptStartList = new List<uint>();
            subScripts.Items.Clear();
            lastIndexFunction = 0;
            lastIndexMov = 0;
            readScriptsOffsets(reader, scriptList, scriptOffList);
            scriptBoxViewer.AppendText("\n");

            readScripts(reader, scriptList, scriptOffList);
            //printScripts(scriptList, scriptBoxViewer,subScripts);

        }


        #region readScriptsOffset

        private void readScriptsOffsets(BinaryReader reader, List<Script_s> scriptList, List<uint> scriptOffList)
        {
            uint scriptOffFinder = 0;
            scriptOrder = new List<uint>();
            do
                scriptOffFinder = readScriptOffset(reader, scriptOffList, scriptOffFinder);
            while (scriptOffFinder != 0xFD13 &&
                //reader.BaseStream.Position != scriptOffList[0] - 2 && 
                reader.BaseStream.Position != scriptOffList[0] && reader.BaseStream.Position < reader.BaseStream.Length);

            fixScriptOffStartLists(scriptOffList);

            for (int scriptCounter = 0; scriptCounter < scriptOffList.Count; scriptCounter++)
                initScriptList(scriptList, scriptCounter);

            for (int scriptCounter = 0; scriptCounter < scriptOffList.Count; scriptCounter++)
                Console.AppendText("\nStored offset was " + scriptOffList[scriptCounter].ToString() +
                                   " .Real offset is " + scriptStartList[scriptCounter].ToString());
        }

        private void fixScriptOffStartLists(List<uint> scriptOffList)
        {
            if (scriptOffList[scriptOffList.Count - 1] == 0xFD13)
                scriptOffList.RemoveAt(scriptOffList.Count - 1);
            scriptOffList.Sort();
            //scriptStartList.RemoveAt(scriptStartList.Count - 1);
            scriptStartList.Sort();
        }

        private void initScriptList(List<Script_s> scriptList, int scriptCounter)
        {
            Script_s script = new Script_s
            {
                scriptStart = scriptStartList[scriptCounter]
            };
            scriptList.Add(script);
        }

        private uint readScriptOffset(BinaryReader reader, List<uint> scriptOffList, uint scriptOffFinder)
        {
            scriptOffFinder = reader.ReadUInt16();
            reader.ReadUInt16();
            uint pad = (uint)reader.BaseStream.Position;
            uint scriptStart = 0;
            scriptStart = scriptOffFinder + pad;
            scriptOffList.Add(scriptOffFinder);
            scriptStartList.Add(scriptStart);
            scriptOrder.Add(scriptStart);
            return scriptOffFinder;
        }
        #endregion

        private void readScripts(BinaryReader reader, List<Script_s> scriptList, List<uint> scriptOffList)
        {
            for (int scriptCounter = 0; scriptCounter < scriptList.Count; scriptCounter++)
                readScript_Function_Movements(reader, scriptList, scriptOffList, scriptCounter);
        }

        private void readScript_Function_Movements(BinaryReader reader, List<Script_s> scriptList, List<uint> scriptOffList, int scriptCounter)
        {
            Script_s actualScript = readScript(reader, scriptList, scriptOffList, scriptCounter);

            actualScript.unknows = new Dictionary<int, byte>();

            actualScript = readFunctions_Movements(reader, scriptOffList, scriptCounter, actualScript);

            scriptList[scriptCounter] = actualScript;
        }

        private Script_s readFunctions_Movements(BinaryReader reader, List<uint> scriptOffList, int scriptCounter, Script_s actualScript)
        {
            while (reader.BaseStream.Position < actualScript.scriptEnd - 1 && reader.BaseStream.Position < reader.BaseStream.Length)
            {
                //loadUnknownSection(reader);

                uint position = (uint)reader.BaseStream.Position;
                if (position != actualScript.scriptEnd)
                {
                    if (functionOffsetList.Contains(position))
                        actualScript = loadFunctions(reader, scriptOffList, scriptCounter, actualScript);
                    else if (movOffsetList.Contains(position))
                        actualScript = loadMovements(reader, scriptOffList, scriptCounter, actualScript);
                    else
                    {
                        //var next = reader.ReadByte();
                        //while (next == 0 && reader.BaseStream.Position<reader.BaseStream.Length)
                        //{
                        //    next = reader.ReadByte();
                        //    scriptBoxViewer.AppendText("Offset " + (reader.BaseStream.Position - 1) + " :" + next + "\n");
                        //}
                        //if (next != 0)
                        //    reader.BaseStream.Position -= 1;
                        //position = (uint)reader.BaseStream.Position;
                        //if (isPossibleMovement(reader))
                        //{
                        //    reader.BaseStream.Position = position;
                        //    actualScript = loadMovements(reader, scriptOffList, scriptCounter, actualScript);
                        //}
                        //else if (isPossibleFunctions(reader))
                        //{
                        //    reader.BaseStream.Position = position;
                        //    actualScript = loadFunctions(reader, scriptOffList, scriptCounter, actualScript);
                        //}

                        //else
                        //{
                        //    if (reader.BaseStream.Position < reader.BaseStream.Length)
                        //    {
                        var next = reader.ReadByte();
                        scriptBoxViewer.AppendText("Offset " + (reader.BaseStream.Position - 1) + " :" + next + "\n");
                        actualScript.unknows.Add((int)reader.BaseStream.Position, next);
                        // }
                        //}
                    }

                }
            }
            return actualScript;
        }

        private bool isPossibleFunctions(BinaryReader reader)
        {
            if (reader.BaseStream.Position < reader.BaseStream.Length - 1)
                return true;
            return false;
        }

        private bool isPossibleMovement(BinaryReader reader)
        {
            List<byte> skeleton = new List<byte>();
            if (reader.BaseStream.Position < reader.BaseStream.Length - 1)
            {
                var next = reader.ReadByte();
                while (next != 254 && reader.BaseStream.Position < reader.BaseStream.Length - 1)
                {
                    skeleton.Add(next);
                    next = reader.ReadByte();
                }
                skeleton.Add(next);
                for (int i = 3; i < skeleton.Count - 4; i += 4)
                    if (skeleton[i] > 5)
                        return false;
                return true;
            }
            return false;

        }

        private Script_s loadMovements(BinaryReader reader, List<uint> scriptOffList, int scriptCounter, Script_s actualScript)
        {
            Console.AppendText("\rEnd of function group. We are in :  " + reader.BaseStream.Position.ToString() + "\r");
            actualScript = readMovements(reader, scriptOffList, scriptCounter, actualScript);
            lastIndexMov = movList.Count;
            return actualScript;
        }

        private Script_s loadFunctions(BinaryReader reader, List<uint> scriptOffList, int scriptCounter, Script_s actualScript)
        {
            actualScript = readFunctions(reader, scriptOffList, scriptCounter, actualScript, lastIndexFunction);
            lastIndexFunction = functionList.Count;
            return actualScript;
        }

        private void loadUnknownSection(BinaryReader reader)
        {
            var next = 0;
            do
            {
                next = reader.ReadByte();
                if (!functionOffsetList.Contains((uint)reader.BaseStream.Position) && !movOffsetList.Contains((uint)reader.BaseStream.Position)
                    || next == 0)
                {
                    scriptBoxViewer.AppendText("\n=== Unknown ===\n\n");
                    scriptBoxViewer.AppendText("Offset " + (reader.BaseStream.Position - 1) + " :" + next + "\n");
                }
            }
            while (next == 0 && reader.BaseStream.Position < reader.BaseStream.Length);
            if (next != 0)
                reader.BaseStream.Position--;
        }

        private Script_s readMovements(BinaryReader reader, List<uint> scriptOffList, int scriptCounter, Script_s actualScript)
        {
            movList.Add(new List<Mov_s>());
            var actualMov = movList[movList.Count - 1];
            if (scriptCounter < scriptOffList.Count - 2)
                actualMov = readMovement(reader, actualMov, scriptStartList[scriptCounter + 1], movList.Count - 1);
            else
                actualMov = readMovement(reader, actualMov, (uint)reader.BaseStream.Length, movList.Count - 1);

            movList[movList.Count - 1] = actualMov;
            Console.AppendText("\nFinished movement. We are in : " + reader.BaseStream.Position.ToString());
            if (lastIndexMov != movList.Count)
                Console.AppendText("\r End of movement group. We are in :  " + reader.BaseStream.Position.ToString() + "\r");
            return actualScript;
        }

        private Script_s readFunctions(BinaryReader reader, List<uint> scriptOffList, int scriptCounter, Script_s actualScript, int functionCounter)
        {
            scriptBoxViewer.AppendText("\n=== Function" + functionCounter.ToString() + "=== \n\n");
            Console.AppendText("\r\rStart analysis function " + functionCounter.ToString() +
                               ". We are in " + (uint)reader.BaseStream.Position);
            Script_s actualFunction = new Script_s();
            if (scriptCounter < scriptOffList.Count - 1)
                actualFunction = readCommands(reader, actualFunction, scriptStartList[scriptCounter + 1]);
            else
                actualFunction = readCommands(reader, actualFunction, (uint)reader.BaseStream.Length);

            functionList.Add(actualFunction);
            printCommand(actualFunction, scriptBoxViewer, false);
            Console.AppendText("\nFinished function. We are in : " + reader.BaseStream.Position.ToString());
            return actualScript;
        }


        private Script_s readScript(BinaryReader reader, List<Script_s> scriptList, List<uint> scriptOffList, int scriptCounter)
        {
            Script_s actualScript = scriptList[scriptCounter];
            reader.BaseStream.Position = actualScript.scriptStart;

            actualScript.idScript = 0;
            for (int i = 0; i < scriptOrder.Count; i++)
                if (scriptOrder[i] == actualScript.scriptStart)
                    actualScript.idScript = i;

            scriptBoxViewer.AppendText("\n=== Script " + actualScript.idScript + " === \n\n");

            subScripts.Items.Add(actualScript.idScript);

            if (scriptCounter < scriptOffList.Count - 1)
                actualScript.scriptEnd = scriptStartList[scriptCounter + 1];
            else
                actualScript.scriptEnd = (uint)reader.BaseStream.Length;
            Console.AppendText("\rStart analysis script " + actualScript.idScript.ToString() +
                                 " .Script start in " + actualScript.scriptStart +
                                 " and end in " + actualScript.scriptEnd);
            //scriptBoxViewer.AppendText("- Script " + scriptCounter); 
            actualScript = readCommands(reader, actualScript);
            printCommand(actualScript, scriptBoxViewer, true);
            Console.AppendText("\nFinished script. We are in : " + reader.BaseStream.Position.ToString());
            return actualScript;
        }

        private List<Mov_s> readMovement(BinaryReader reader, List<Mov_s> actualMov, uint index, int movCounter)
        {
            Mov_s move = new Mov_s();
            do
            {
                move.offset = (uint)reader.BaseStream.Position;
                if (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    move.moveId = reader.ReadByte();
                    if (reader.BaseStream.Position < reader.BaseStream.Length)
                        move.repeatTime = reader.ReadByte();
                    actualMov.Add(move);
                }
            }
            while (move.moveId != 0xFE && reader.BaseStream.Position < reader.BaseStream.Length);

            scriptBoxViewer.AppendText("\n=== Movement " + movCounter + " === \n\n");
            for (int i = 0; i < actualMov.Count; i++)
                scriptBoxViewer.AppendText("Offset: " + actualMov[i].offset.ToString() + ": " + getMovement(actualMov[i].moveId) + " 0x" + actualMov[i].repeatTime.ToString() + "\n");

            var position = reader.BaseStream.Position;
            //reader.BaseStream.Position += 3;

            return actualMov;
        }

        private string getMovement(int p)
        {
            switch (p)
            {
                case 0x0:
                    return "Face_Up";
                case 0x1:
                    return "Face_Down";
                case 0x2:
                    return "Face_Left";
                case 0x3:
                    return "Face_Right";
                case 0x4:
                    return "Walk_Slow_Up";
                case 0x5:
                    return "Walk_Slow_Down";
                case 0x6:
                    return "Walk_Slow_Left";
                case 0x7:
                    return "Walk_Slow_Right";
                case 0x8:
                    return "Stroll_up";
                case 0x9:
                    return "Stroll_Down";
                case 0xA:
                    return "Stroll_up";
                case 0xB:
                    return "Stroll_right";
                case 0xC:
                    return "Walk_Fast_Up";
                case 0xD:
                    return "Walk_Fast_Down";
                case 0xE:
                    return "Walk_Fast_Left";
                case 0xF:
                    return "Walk_Fast_Right";
                case 0x10:
                    return "Jog_Up";
                case 0x11:
                    return "Jog_Down";
                case 0x12:
                    return "Jog_Left";
                case 0x13:
                    return "Jog_Right";
                case 0x14:
                    return "Bike_Up";
                case 0x15:
                    return "Bike_Down";
                case 0x16:
                    return "Bike_Left";
                case 0x17:
                    return "Bike_Right";
                case 0x18:
                    return "Wait_VSlow_Up";
                case 0x19:
                    return "Wait_VSlow_Down";
                case 0x1A:
                    return "Wait_VSlow_up";
                case 0x1B:
                    return "Wait_VSlow_right";
                case 0x1C:
                    return "Wait_Slow_Up";
                case 0x1D:
                    return "Wait_Slow_Down";
                case 0x1E:
                    return "Wait_Slow_Left";
                case 0x1F:
                    return "Wait_Slow_Right";
                case 0x20:
                    return "Wait_Normal_Up";
                case 0x21:
                    return "Wait_Normal_Down";
                case 0x22:
                    return "Wait_Normal_Left";
                case 0x23:
                    return "Wait_Normal_Right";
                case 0x24:
                    return "Wait_Fast_Up";
                case 0x25:
                    return "Wait_Fast_Down";
                case 0x26:
                    return "Wait_Fast_Left";
                case 0x27:
                    return "Wait_Fast_Right";
                case 0x28:
                    return "Wait_VFast_Up";
                case 0x29:
                    return "Wait_VFast_Down";
                case 0x2A:
                    return "Wait_VFast_up";
                case 0x2B:
                    return "Wait_VFast_right";
                case 0x2C:
                    return "HopPlace_Slow_Up";
                case 0x2D:
                    return "HopPlace_Slow_Down";
                case 0x2E:
                    return "HopPlace_Slow_Left";
                case 0x2F:
                    return "HopPlace_Slow_Right";
                case 0x30:
                    return "HopPlace_Fast_Up";
                case 0x31:
                    return "HopPlace_Fast_Down";
                case 0x32:
                    return "HopPlace_Fast_Left";
                case 0x33:
                    return "HopPlace_Fast_Right";
                case 0x34:
                    return "Hop_Up";
                case 0x35:
                    return "Hop_Down";
                case 0x36:
                    return "Hop_Left";
                case 0x37:
                    return "Hop_Right";
                case 0x38:
                    return "Hop_Bound_Up";
                case 0x39:
                    return "Hop_Bound_Down";
                case 0x3A:
                    return "Hop_Buond_up";
                case 0x3B:
                    return "Hop_Bound_right";
                case 0x3C:
                    return "3C";
                case 0x3D:
                    return "3D";
                case 0x3E:
                    return "3E";
                case 0x3F:
                    return "3F";
                case 0x40:
                    return "40";
                case 0x41:
                    return "41";
                case 0x42:
                    return "42";
                case 0x43:
                    return "Warp_Up";
                case 0x44:
                    return "Warp_Down";
                case 0x45:
                    return "Vanish";
                case 0x46:
                    return "Reappear";
                case 0x47:
                    return "47";
                case 0x48:
                    return "48";
                case 0x49:
                    return "49";
                case 0x4A:
                    return "4A";
                case 0x4B:
                    return "Exclaim";
                case 0x4C:
                    return "Move_Slow_Up";
                case 0x4D:
                    return "Move_Slow_Down";
                case 0x4E:
                    return "Move_Slow_Left";
                case 0x4F:
                    return "Move_Slow_Right";
                case 0x50:
                    return "Move_Normal_Up";
                case 0x51:
                    return "Move_Normal_Down";
                case 0x52:
                    return "Move_Normal_Left";
                case 0x53:
                    return "Move_Normal_Right";
                case 0x54:
                    return "Move_Fast_Up";
                case 0x55:
                    return "Move_Fast_Down";
                case 0x56:
                    return "Move_Fast_Left";
                case 0x57:
                    return "Move_Fast_Right";
                case 0xFE:
                    return "End_Movement";
                default:
                    return p.ToString();
            }










            //58	Spin Up	Walking speed.
            //59	Spin Down	
            //005A	Spin Left	
            //005B	Spin Right	
            //005C	Pounce Left	medium speed hop
            //005D	Pounce Right	
            //005E	Leap Left	2 panels
            //005F	Leap Right	
            //60	Walk Up	Walk speed movement
            //61	Walk Down	
            //62	Walk Left	
            //63	Walk Right	
            //64	Turn-90 Return	-90 turn, +90 turn
            //65	HopInPlace	..
            //66	Bounce	Bounce once
            //67	Exclaim	!
            //68	MoveInPlace	
            //69	RiseUp	Upwards *param
            //006A	RiseDown	Downwards *param
            //006B	90walk	*param
            //006C	90+up?	
            //006D	FRightRiseUp	
            //006E	FLeftRiseDown	
            //006F	Walk Up	..
            //70	Walk Down	
            //71	Walk Down	
            //72	Walk Up	
            //73	SlideMove Left	dont face, just moonwalk!
            //74	SlideMove Right	
            //75	Leap Up	Moon bounce *3param
            //76	Leap Down	3
            //77	Leap Left	3
            //78	Leap Right	3
            //79	T90FL-RU	This stuff below is stupid.
            //007A	Down of ^	.
            //007B	Slide Down, Face Down	.
            //007C	Slide Up, Face Up	.
            //007D	RiseUpLeftFaceRight	.
            //007E	RiseDownRightcrap	.
            //007F	SlideUpFaceUp	.
            //80	FU-SlideUp-FU	.
            //81	FU-SlideDown-FD	.
            //82	FU-SlideUp-FU	.
            //83	FU-SlideLeft-FR	.
            //84	FD-SlideRight-FL	.
            //85	FL-Hop-RiseUp	.
            //86	FR-Hop-RiseDown	.
            //87	TD-HopForward	.
            //88	TU-HopForward	.
            //89	TR-Hop-RiseUp	.
            //008A	TL-Hop-RiseDown	.
            //008B	Hop Up	.
            //008C	Hop Down	*param 
            //008D	Hop Down	.
            //008E	Hop Up	.
            //008F	HopSlide Left	.
            //90	HopSlide Right	.
            //91	FastWalkDown	.
            //92	FastWalkUp	.
            //93	Slide Left	.
            //94	Slide Right	.
            //95	RapidWalk Down	.
            //96	RapidWalk Up	.
            //97	QuickSlide Left	.
            //98	QuickSlide Right	.
            //99	Exclaim 	.
            //009A	Hopscotch in	.
            //009B	Shuffle Up	
            //009C	Shuffle Down	
            //009D	Shuffle Left	
            //009E	Shuffle Right	
            //009F	Question 	
            //00A0	MusicNote	
            //00A1	"..."
        }

        private Script_s readCommands(BinaryReader reader, Script_s actualFunction, uint index)
        {
            actualFunction.commands = new List<Commands_s>();
            Commands_s actualCommand = returnCommand(reader);
            while (actualCommand.isEnd == 0 && reader.BaseStream.Position < index - 1 && reader.BaseStream.Position + 2 < reader.BaseStream.Length)
            {
                actualFunction.commands.Add(actualCommand);
                actualCommand = returnCommand(reader);
            }
            actualFunction.commands.Add(actualCommand);
            return actualFunction;
        }

        private Script_s readCommands(BinaryReader reader, Script_s actualScript)
        {
            actualScript.commands = new List<Commands_s>();
            Commands_s actualCommand = returnCommand(reader);
            while (actualCommand.isEnd == 0 && reader.BaseStream.Position < actualScript.scriptEnd - 1 && reader.BaseStream.Position + 2 < reader.BaseStream.Length)
            {
                actualScript.commands.Add(actualCommand);
                actualCommand = returnCommand(reader);
            }
            actualScript.commands.Add(actualCommand);
            return actualScript;
        }


        private void saveScript(BinaryWriter writer)
        {
            saveScriptsOffset(writer);
            saveScriptCommand(writer);
        }

        private void saveScriptsOffset(BinaryWriter writer)
        {
            var scriptLine = scriptBoxViewer.Lines;
            var scriptNum = 0;
            var nextOffset = 0;
            for (int i = 0; i < scriptLine.Length; i++)
            {
                var line = scriptLine[i];
                if (line.Contains("= Script "))
                {
                    var number = Int16.Parse(line.Split(' ')[2]);
                    if (number > scriptNum)
                        scriptNum = number;
                }
            }

            var scriptOffsets = new Dictionary<int, int>();

            var startOffset = 2 + (scriptNum + 1) * 4;

            Console.Clear();


            for (int i = 0; i < scriptLine.Length - 1; i++)
            {
                var line = scriptLine[i];
                startOffset += getSize(scriptLine[i]);

                //Console.AppendText("\n" + startOffset + " Dopo la linea: " + scriptLine[i]);

                if (scriptLine[i + 1] != "" && !(scriptLine[i + 1].StartsWith("="))
                    && line != ""
                    && !(line.StartsWith("=")
                    && !(line.Contains("End"))
                    && !(line.Contains("Jump"))
                    && !(line.Contains("Goto"))
                    && !(line.Contains("KillScript"))))
                {
                    nextOffset = Int32.Parse(scriptLine[i + 1].Split(' ')[1]);
                    if (startOffset != nextOffset)
                        Console.AppendText("\nThere s a problem at " + nextOffset);
                }


                if (line.Contains("= Script "))
                {
                    var number = Int16.Parse(line.Split(' ')[2]);
                    scriptOffsets.Add(number, startOffset);
                }
                else if (line.Contains("= Function "))
                {
                    var number = Int16.Parse(line.Split(' ')[2]);
                    functionOffsetList[number] = (uint)startOffset;
                }
                else if (line.Contains("= Movement "))
                {
                    var number = Int16.Parse(line.Split(' ')[2]);
                    if (number < movOffsetList.Count)
                        movOffsetList[number] = (uint)startOffset;
                }
            }
            //scriptCounter++;
            //if (number != scriptCounter)
            //{
            //    for (int j = i; j < scriptLine.Length - 1 && number2 != scriptCounter; j++)
            //    {
            //        var line2 = scriptLine[j];
            //        startOffset += getSize(scriptLine[j]);
            //        if (scriptLine[j + 1] != "" || scriptLine[j + 1].StartsWith("="))
            //            nextOffset = scriptLine[j + 1].Split(' ')[1];
            //        else
            //            if (startOffset.ToString() != nextOffset)
            //                Console.AppendText("\nThere s a problem at " + nextOffset);
            //        if (line2.Contains(" Script "))
            //        {
            //            number2 = Int16.Parse(line2.Split(' ')[2]);
            //            if (number2 == scriptCounter)
            //            {
            //                var offsetLine = scriptLine[j + 2];
            //                //var startOffset = Int16.Parse(offsetLine.Split(' ')[1]);
            //                var writeOffset = (startOffset - 4 * number2 - 4);
            //                writer.Write((UInt16)writeOffset);
            //                writer.Write((UInt16)0);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            for (int i = 0; i < scriptOffsets.Keys.Count; i++)
            {
                startOffset = scriptOffsets[i];
                var writeOffset = (startOffset - 4 * i - 4);
                writer.Write((UInt16)writeOffset);
                writer.Write((UInt16)0);
            }
            //}
            //    }
            //}
            writer.Write((UInt16)0xFD13);
        }

        private int getSize(string p)
        {
            if (scriptType == DPSCRIPT)
                return getSizeDPP(p);
            return 0;
        }

        private int getSizeDPP(string p)
        {
            if (p == "" || p.StartsWith("="))
                return 0;
            var wordArray = p.Split(' ');
            switch (wordArray[2])
            {

                case ":0":
                    return 1;
                case "End":
                    return 2;
                case "Return":
                    return 6;
                case "Compare":
                    return 6;
                case "Compare2":
                    return 6;
                case "CallStd":
                    return 4;
                case "ExitStd":
                    return 2;
                case "Jump":
                    return 6;
                case "Goto":
                    return 6;
                case "KillScript":
                    return 2;
                case "If":
                    return 7;
                case "If2":
                    return 7;
                case "SetFlag":
                    return 4;
                case "ClearFlag":
                    return 4;
                case "StoreFlag":
                    return 4;
                case "22":
                    return 4;
                case "ClearTrainerId":
                    return 4;
                case "24":
                    return 4;
                case "StoreTrainerId":
                    return 4;
                case "SetValue":
                    return 6;
                case "CopyValue":
                    return 6;
                case "SetVar":
                    return 6;
                case "CopyVar":
                    return 6;
                case "Message":
                    return 3;
                case "Message2":
                    return 3;
                case "Message3":
                    return 3;
                case "Message4":
                    return 3;
                case "Message5":
                    return 3;
                case "30":
                    return 2;
                case "WaitButton":
                    return 2;
                case "CloseMessageKeyPress":
                    return 2;
                case "FreezeMessageBox":
                    return 2;
                case "CallMessageBox":
                    return 8;
                case "ColorMessageBox":
                    return 5;
                case "TypeMessageBox":
                    return 3;
                case "NoMapMessageBox":
                    return 2;
                case "CallTextMessageBox":
                    return 5;
                case "StoreMenuStatus":
                    return 4;
                case "ShowMenu":
                    return 6;
                case "YesNoBox":
                    return 4;
                case "WaitFor":
                    return 4;
                case "Multi":
                    return 8;
                case "Multi2":
                    return 8;
                case "SetTextScriptMulti":
                    return 4;
                case "CloseMulti":
                    return 2;
                case "Multi3":
                    return 11;
                case "Multi4":
                    return 8;
                case "SetTextScriptMessageMulti":
                    return 8;
                case "CloseMulti4":
                    return 2;
                case "SetTextRowMulti":
                    return 3;
                case "Fanfare":
                    return 4;
                case "Fanfare2":
                    return 4;
                case "WaitFanfare":
                    return 4;
                case "Cry":
                    return 6;
                case "WaitCry":
                    return 2;
                case "PlaySound":
                    return 4;
                case "FadeDef":
                    return 2;
                case "PlaySound2":
                    return 4;
                case "StopSound":
                    return 4;
                case "RestartSound":
                    return 2;
                case "SwitchSound":
                    return 6;
                case "StoreSoundMicrophone":
                    return 4;
                case "PlaySound3":
                    return 4;
                case "58":
                    return 6;
                case "StoreSoundMicrophone2":
                    return 4;
                case "SwitchSound2":
                    return 4;
                case "ActivateMicrophone":
                    return 2;
                case "DisactivateMicrophone":
                    return 2;
                case "ApplyMovement":
                    return 8;
                case "WaitMovement":
                    return 2;
                case "LockAll":
                    return 2;
                case "ReleaseAll":
                    return 2;
                case "Lock":
                    return 4;
                case "Release":
                    return 4;
                case "AddPeople":
                    return 4;
                case "RemovePeople":
                    return 4;
                case "MoveCam":
                    return 6;
                case "ZoomCam":
                    return 2;
                case "FacePlayer":
                    return 2;
                case "StoreHeroPosition":
                    return 6;
                case "StoreOWPosition":
                    return 8;
                case "6B":
                    return 8;
                case "ContinueFollowHero":
                    return 5;
                case "FollowHero":
                    return 6;
                case "StopFollowHero":
                    return 2;
                case "GiveMoney":
                    return 6;
                case "TakeMoney":
                    return 6;
                case "CompareMoney":
                    return 8;
                case "ShowMoney":
                    return 6;
                case "HideMoney":
                    return 2;
                case "UpdateMoney":
                    return 2;
                case "ShowCoins":
                    return 6;
                case "HideCoins":
                    return 2;
                case "UpdateCoins":
                    return 2;
                case "CompareCoins":
                    return 8;
                case "GiveCoins":
                    return 4;
                case "TakeCoins":
                    return 8;
                case "TakeItem":
                    return 8;
                case "GiveItem":
                    return 8;
                case "StoreItemBagNumber":
                    return 8;
                case "StoreItem":
                    return 8;
                case "StoreItemTaken":
                    return 6;
                case "StoreItemType":
                    return 6;
                case "SendItemType":
                    return 8;
                case "StoreUndergroundPcStatus":
                    return 8;
                case "SendItemType2":
                    return 8;
                case "SendItemType3":
                    return 8;
                case "StorePokèmonParty":
                    return 6;
                case "StorePokèmonParty2":
                    return 6;
                case "StorePokèmonParty3":
                    return 6;
                case "GivePokèmon":
                    return 10; ;
                case "GiveEgg":
                    return 10;
                case "StoreMove":
                    return 8;
                case "StorePlace":
                    return 6;
                case "9B":
                    return 6;
                case "CallEnd":
                    return 2;
                case "A2":
                    return 2;
                case "StartWFC":
                    return 2;
                case "StartInterview":
                    return 2;
                case "StartDressPokèmon":
                    return 8;
                case "DisplayDressedPokèmon":
                    return 6;
                case "DisplayContestPokèmon":
                    return 6;
                case "OpenBallCapsule":
                    return 2;
                case "OpenSinnohMaps":
                    return 2;
                case "OpenPc":
                    return 4;
                case "ShowDrawingUnion":
                    return 2;
                case "ShowTrainerCaseUnion":
                    return 2;
                case "ShowTradingUnion":
                    return 2;
                case "ShowRecordUnion":
                    return 2;
                case "EndGame":
                    return 2;
                case "ShowHallOfFame":
                    return 2;
                case "StoreWFC":
                    return 6;
                case "StartWFC2":
                    return 4;
                case "ChooseStarter":
                    return 2;
                case "BattleStarter":
                    return 2;
                case "StoreBattleID?":
                    return 4;
                case "SetVarBattle?":
                    return 6;
                case "StoreTypeBattle?":
                    return 4;
                case "SetVarBattle2?":
                    return 6;
                case "ChoosePlayerName":
                    return 2;
                case "ChoosePokèmonName":
                    return 2;
                case "FadeScreen":
                    return 10;
                case "ResetScreen":
                    return 2;
                case "Warp":
                    return 12;
                case "RockClimbAnimation":
                    return 4;
                case "SurfAnimation":
                    return 4;
                case "WaterFallAnimation":
                    return 4;
                case "FlyAnimation":
                    return 4;
                case "C5":
                    return 4;
                case "Tuxedo":
                    return 4;
                case "StoreBike":
                    return 3;
                case "RideBike":
                    return 3;
                case "C9":
                    return 3;
                case "BerryHeroAnimation":
                    return 4;
                case "StopBerryHeroAnimation":
                    return 2;
                case "SetVarHero":
                    return 3;
                case "SetVarRival":
                    return 3;
                case "SetVarAlter":
                    return 3;
                case "SetVarPokèmon":
                    return 5;
                case "SetVarItem":
                    return 5;
                case "SetVarItem2":
                    return 5;
                case "SetVarBattleItem":
                    return 5;
                case "SetVarAttack":
                    return 5;
                case "SetVarNumber":
                    return 5;
                case "SetVarNickPokèmon":
                    return 5;
                case "SetVarKeyItem":
                    return 5;
                case "SetVarTrainer":
                    return 5;
                case "SetVarTrainer2":
                    return 3;
                case "SetVarPokèmonStored":
                    return 8;
                case "StoreStarter":
                    return 4;
                case "SetVarSwarmPlace":
                    return 5;
                case "StoreSwarmInfo":
                    return 6;
                case "E4":
                    return 4;
                case "TrainerBattle":
                    return 6;
                case "E6":
                    return 6;
                case "E7":
                    return 8;
                case "E8":
                    return 8;
                case "E9":
                    return 4;
                case "ActTrainer":
                    return 4;
                case "TeleportPC":
                    return 2;
                case "StoreBattleResult":
                    return 4;
                case "EE":
                    return 4;
                case "SetFloor":
                    return 4;
                case "WarpMapLift":
                    return 12;
                case "StoreFloor":
                    return 4;
                case "StartLift":
                    return 4;
                case "StorePokèmonCaught":
                    return 4;
                case "WildPokèmonBattle":
                    return 6;
                case "StorePhotoImage":
                    return 6;
                case "StoreContestImage":
                    return 6;
                case "StorePhotoName":
                    return 4;
                case "ActPokèKron":
                    return 2;
                case "StorePokèKronStatus":
                    return 4;
                case "ActPokèKronApplication":
                    return 4;
                case "StorePokèKronApplicationStatus":
                    return 6;
                case "135":
                    return 4;
                case "139":
                    return 4;
                case "13C":
                    return 4;
                case "13F":
                    return 6;
                case "140":
                    return 4;
                case "141":
                    return 4;
                case "142":
                    return 4;
                case "143":
                    return 6;
                case "144":
                    return 4;
                case "145":
                    return 4;
                case "146":
                    return 6;
                case "147":
                    return 4;
                case "148":
                    return 4;
                case "StoreGender":
                    return 4;
                case "HealPokèmon":
                    return 2;
                case "152":
                    return 4;
                case "StartChooseWifiSprite":
                    return 2;
                case "ChooseWifiSprite":
                    return 6;
                case "ActWifiSprite":
                    return 4;
                case "ActSinnohPokèdex":
                    return 2;
                case "ActRunningShoes":
                    return 2;
                case "StoreBadge":
                    return 6;
                case "SetBadge":
                    return 4;
                case "StoreBadgeNumber":
                    return 4;
                case "ActRunningShoes2":
                    return 2;
                case "StartFollowHero":
                    return 2;
                case "StopFollowHero2":
                    return 2;
                case "StartFollowHero2":
                    return 2;
                case "166":
                    return 4;
                case "PrepareDoorAnimation":
                    return 11;
                case "WaitDoorOpeningAnimation":
                    return 3;
                case "WaitDoorClosingAnimation":
                    return 3;
                case "DoorOpeningAnimation":
                    return 3;
                case "DoorClosingAnimation":
                    return 3;
                case "StorePokèmonPartyNumber3":
                    return 4;
                case "SetVarPokèmonNature":
                    return 5;
                case "ChangeOwPosition":
                    return 8;
                case "SetOwPosition":
                    return 12;
                case "ChangeOwPosition2":
                    return 6;
                case "ReleaseOw":
                    return 6;
                case "SetTilePassable":
                    return 8;
                case "SetTileLocked":
                    return 8;
                case "ShowChoosePokèmonMenu":
                    return 2;
                case "StoreChosenPokèmon":
                    return 4;
                case "StorePokèmonId":
                    return 6;
                case "StorePokèmonPartyNumber":
                    return 4;
                case "StorePokèmonPartyNumber2":
                    return 6;
                case "19E":
                    return 6;
                case "19F":
                    return 4;
                case "1A0":
                    return 4;
                case "1B1":
                    return 4;
                case "1B2":
                    return 4;
                case "ShowRecordList":
                    return 4;
                case "StoreTime":
                    return 4;
                case "StorePlayerId":
                    return 6;
                case "StorePokèmonHappiness":
                    return 6;
                case "StoreHeroFaceOrientation":
                    return 4;
                case "StoreSpecificPokèmonParty":
                    return 6;
                case "StoreMoveDeleter":
                    return 4;
                case "StorePokèmonDeleter":
                    return 4;
                case "StorePokèmonMoveNumber":
                    return 6;
                case "DeletePokèmonMove":
                    return 6;
                case "SetVarMoveDeleter":
                    return 7;
                case "ActJournal":
                    return 2;
                case "DeActivateLeader":
                    return 12;
                case "GiveAccessories":
                    return 6;
                case "StoreAccessories":
                    return 8;
                case "GiveAccessories2":
                    return 4;
                case "StoreAccessories2":
                    return 6;
                case "StoreSinnohPokèdexStatus":
                    return 4;
                case "StoreNationalPokèdexStatus":
                    return 4;
                case "GiveSinnohDiploma":
                    return 2;
                case "GiveNationalDiploma":
                    return 2;
                case "StoreFossilNumber":
                    return 4;
                case "StoreFossilPokèmon":
                    return 6;
                case "TakeFossil":
                    return 8;
                case "1F9":
                    return 4;
                case "1FB":
                    return 6;
                case "SetSafariGame":
                    return 3;
                case "204":
                    return 2;
                case "StorePokèmonNature":
                    return 6;
                case "StoreUGFriendNumber":
                    return 4;
                case "GenerateRandomPokèmonSearch":
                    return 4;
                case "ActRandomPokèmonSearch":
                    return 4;
                case "StoreRandomPokèmonSearch":
                    return 4;
                case "StoreMoveUsableTeacher":
                    return 6;
                case "StoreMoveTeacher":
                    return 4;
                case "StorePokèmonMoveTeacher":
                    return 4;
                case "TeachPokèmonMove":
                    return 6;
                case "StoreTeachingResult":
                    return 4;
                case "SetTradeId":
                    return 3;
                case "StoreChosenPokèmonTrade":
                    return 6;
                case "ActMFPokèdex":
                    return 2;
                case "StoreNationalPokèdex":
                    return 5;
                case "StoreRibbonNumber":
                    return 4;
                case "StoreRibbonPokèmon":
                    return 8;
                case "GiveRibbonPokèmon":
                    return 6;
                case "StoreFootPokèmon":
                    return 8;
                case "StoreFirstPokèmonParty":
                    return 4;
                case "StorePalParkNumber":
                    return 4;
                case "DoubleTrainerBattle":
                    return 8;
                case "SavePCPalParkPokèmon":
                    return 2;
                case "StorePalParkPoint":
                    return 6;
                case "StorePalParkPokèmonCaught":
                    return 4;
                case "2BB":
                    return 2;
                case "SetVarStarterAlter":
                    return 3;
                default:
                    if (wordArray[2].StartsWith("m"))
                        return 2;
                    break;
            }
            return 0;
        }

        private void saveScriptCommand(BinaryWriter writer)
        {
            String[] commandLine = scriptBoxViewer.Lines;
            for (int i = 0; i < commandLine.Length; i++)
            {
                var line = commandLine[i];
                string[] wordLine = line.Split(' ');
                if (wordLine[0] == "Offset:")
                {

                    if (commandLine[i - 1].Split(' ')[0] != "Offset:")
                        while (writer.BaseStream.Position < UInt32.Parse(wordLine[1]))
                            writer.Write((Byte)0);
                    writeCommand(writer, wordLine);
                }

            }

        }

        private void writeCommand(BinaryWriter writer, string[] wordLine)
        {
            if (scriptType == DPSCRIPT)
                writeCommandDPP(writer, wordLine);
            if (scriptType == HGSSSCRIPT)
                writeCommandHGSS(writer, wordLine);

        }

        private void readCommandDPP(BinaryReader reader, ref Commands_s com)
        {
            uint functionOffset = 0;
            switch (com.Id)
            {
                case 0x2:
                    com.Name = "End";
                    com.isEnd = 1;
                    functionOffset = (uint)reader.BaseStream.Position;
                    checkNextFunction(reader, functionOffset);
                    break;
                case 0x3:
                    com.Name = "Return";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x11:
                    com.Name = "Compare";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x12:
                    com.Name = "Compare2";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x14:
                    com.Name = "CallStd";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x15:
                    com.Name = "ExitStd";
                    break;
                case 0x16:
                    com.Name = "Jump";
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[0];
                    checkNextFunction(reader, functionOffset);
                    com.numJump++;
                    if (reader.BaseStream.Position < reader.BaseStream.Length - 1)
                    {
                        var isEndC = reader.ReadUInt16();
                        if (isEndC != 2)
                        {
                            com.isEnd = 1;
                            checkNextFunction(reader, (uint)reader.BaseStream.Position - 2);
                        }
                        reader.BaseStream.Position -= 2;
                    }
                    break;
                case 0x1A:
                    com.Name = "Goto";
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[0];
                    checkNextFunction(reader, functionOffset);
                    com.numJump++;
                    break;

                case 0x1B:
                    com.Name = "KillScript";
                    com.isEnd = 1;
                    functionOffset = (uint)reader.BaseStream.Position;
                    checkNextFunction(reader, functionOffset);
                    break;

                case 0x1C:
                    com.Name = "If";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[1];
                    checkNextFunction(reader, functionOffset);
                    break;

                case 0x1D:
                    com.Name = "If2";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[1];
                    checkNextFunction(reader, functionOffset);
                    break;

                case 0x1E:
                    com.Name = "SetFlag";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F:
                    com.Name = "ClearFlag";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x20:
                    com.Name = "StoreFlag";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x23:
                    com.Name = "ClearTrainerId";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24:
                    com.Name = "SetTrainerId";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x25:
                    com.Name = "StoreTrainerId";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x26:
                    com.Name = "StoreAddVar";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x27:
                    com.Name = "StoreSubVar";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x28:
                    com.Name = "StoreVarVariable";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x29:
                    com.Name = "StoreVarValue";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2b:
                    com.Name = "Message";
                    com.parameters.Add(reader.ReadByte());
                    break;

                case 0x2c:
                    com.Name = "Message2";
                    com.parameters.Add(reader.ReadByte());
                    break;

                case 0x2d:
                    com.Name = "Message3";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x2e:
                    com.Name = "Message4";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2f:
                    com.Name = "Message5";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x30:
                    com.Name = "30";
                    break;
                case 0x31:
                    com.Name = "WaitButton";
                    break;
                case 0x33:
                    com.Name = "33";
                    break;
                case 0x34:
                    com.Name = "CloseMessageKeyPress";
                    break;
                case 0x35:
                    com.Name = "FreezeMessageBox";
                    break;
                case 0x36:
                    com.Name = "CallMessageBox";
                    com.parameters.Add(reader.ReadUInt16()); //Text Id
                    com.parameters.Add(reader.ReadUInt16()); //Type
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x37:
                    com.Name = "ColorMessageBox";
                    com.parameters.Add(reader.ReadByte()); //Color
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x38:
                    com.Name = "TypeMessageBox";
                    com.parameters.Add(reader.ReadByte()); //Type
                    break;
                case 0x39:
                    com.Name = "NoMapMessageBox";
                    break;
                case 0x3A:
                    com.Name = "CallTextMessageBox";
                    com.parameters.Add(reader.ReadByte()); //Text Id
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x3B:
                    com.Name = "StoreMenuStatus";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x3C:
                    com.Name = "ShowMenu";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x3E:
                    com.Name = "YesNoBox";
                    com.parameters.Add(reader.ReadUInt16()); //RV = YES/NO
                    break;
                case 0x3F:
                    com.Name = "WaitFor";
                    com.parameters.Add(reader.ReadUInt16()); //Time for waiting
                    break;
                case 0x40:
                    com.Name = "MultiTextScriptMessage(40)";
                    com.parameters.Add(reader.ReadByte()); //X Screen Coordinate
                    com.parameters.Add(reader.ReadByte()); //Y Screen Coordinate
                    com.parameters.Add(reader.ReadByte()); //Cursor Position
                    com.parameters.Add(reader.ReadByte()); //B Cancel Active
                    com.parameters.Add(reader.ReadUInt16()); //RV = Multi Condition Value
                    break;
                case 0x41:
                    com.Name = "MultiTextScriptMessage(41)";
                    com.parameters.Add(reader.ReadByte()); //X Screen Coordinate
                    com.parameters.Add(reader.ReadByte()); //Y Screen Coordinate
                    com.parameters.Add(reader.ReadByte()); //Cursor Position
                    com.parameters.Add(reader.ReadByte()); //B Cancel Active
                    com.parameters.Add(reader.ReadUInt16()); //RV = Multi Condition Id
                    break;
                case 0x42:
                    com.Name = "SetMultiTextScriptMessage";
                    com.parameters.Add(reader.ReadByte()); //Text Id
                    com.parameters.Add(reader.ReadByte()); //Multi Condition Id
                    break;
                case 0x43:
                    com.Name = "CloseMultiTextScriptMessage";
                    break;
                case 0x44:
                    com.Name = "MultiTextScriptMessage(44)";
                    com.parameters.Add(reader.ReadByte()); //X Screen Coordinate
                    com.parameters.Add(reader.ReadByte()); //Y Screen Coordinate
                    com.parameters.Add(reader.ReadByte()); //Cursor Position
                    com.parameters.Add(reader.ReadByte()); //B Cancel Active
                    com.parameters.Add(reader.ReadUInt16()); //RV = Multi Condition Value
                    break;
                case 0x45:
                    com.Name = "MultiTextScriptMessage(45)";
                    com.parameters.Add(reader.ReadByte()); //X Screen Coordinate
                    com.parameters.Add(reader.ReadByte()); //Y Screen Coordinate
                    com.parameters.Add(reader.ReadByte()); //Cursor Position
                    com.parameters.Add(reader.ReadByte()); //B Cancel Active
                    com.parameters.Add(reader.ReadUInt16()); //RV = Multi Condition Value
                    break;
                case 0x46:
                    com.Name = "SetMultiTextBoxScriptMessage";
                    com.parameters.Add(reader.ReadUInt16()); //Message Text Id 
                    com.parameters.Add(reader.ReadUInt16()); //Box Text Id
                    com.parameters.Add(reader.ReadUInt16()); //Multi Condition Id
                    break;
                case 0x47:
                    com.Name = "CloseMultiTextScriptMessage";
                    break;
                case 0x48:
                    com.Name = "SetTextRowMulti";
                    com.parameters.Add(reader.ReadByte()); //Row Number
                    break;
                case 0x49:
                    com.Name = "PlayFanfare(49)";
                    com.parameters.Add(reader.ReadUInt16()); //Fanfare Id
                    break;
                case 0x4A:
                    com.Name = "PlayFanfare(4A)";
                    com.parameters.Add(reader.ReadUInt16()); //Fanfare Id
                    break;
                case 0x4B:
                    com.Name = "WaitFanfare";
                    com.parameters.Add(reader.ReadUInt16()); //Fanfare Id
                    break;
                case 0x4C:
                    com.Name = "Cry";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x4D:
                    com.Name = "WaitCry";
                    break;
                case 0x4E:
                    com.Name = "PlayMusic(4E)";
                    com.parameters.Add(reader.ReadUInt16()); //Music Id
                    break;
                case 0x4F:
                    com.Name = "FadeDef";
                    break;
                case 80:
                    com.Name = "PlayMusic(50)";
                    com.parameters.Add(reader.ReadUInt16()); //Music Id
                    break;
                case 0x51:
                    com.Name = "StopMusic";
                    com.parameters.Add(reader.ReadUInt16()); //Music Id
                    break;
                case 0x52:
                    com.Name = "RestartMusic";
                    break;
                case 0x54:
                    com.Name = "ChangeVolume";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //Volume
                    break;
                case 0x55:
                    com.Name = "CheckVolume";
                    com.parameters.Add(reader.ReadUInt16()); //RV = T/F
                    break;
                case 0x57:
                    com.Name = "PlaySound3";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x58:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x59:
                    com.Name = "CheckOldTalkMicrophone";
                    com.parameters.Add(reader.ReadUInt16()); //RV = T/F
                    break;
                case 0x5A:
                    com.Name = "CheckTalkMicrophone";
                    com.parameters.Add(reader.ReadUInt16()); //RV = T/F
                    break;
                case 0x5B:
                    com.Name = "ActivateMicrophone";
                    break;
                case 0x5C:
                    com.Name = "DisactivateMicrophone";
                    break;
                case 0x5E:
                    com.Name = "ApplyMovement";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position); //Mov offset
                    var movOffset = com.parameters[1];
                    if (!movOffsetList.Contains(movOffset))
                    {
                        movOffsetList.Add(movOffset);
                        Console.AppendText("\nA movement is in: " + movOffset.ToString());
                    }
                    break;
                case 0x5F:
                    com.Name = "WaitMovement";
                    break;
                case 0x60:
                    com.Name = "LockAll";
                    break;
                case 0x61:
                    com.Name = "ReleaseAll";
                    break;
                case 0x62:
                    com.Name = "LockNPC";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    break;
                case 0x63:
                    com.Name = "ReleaseNPC";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    break;
                case 0x64:
                    com.Name = "AddNPC";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    break;

                case 0x65:
                    com.Name = "RemoveNPC";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    break;
                case 0x66:
                    com.Name = "MoveCam";
                    com.parameters.Add(reader.ReadUInt16()); //X
                    com.parameters.Add(reader.ReadUInt16()); //Y
                    break;
                case 0x67:
                    com.Name = "ZoomCam";
                    break;
                case 0x68:
                    com.Name = "FacePlayer";
                    break;
                case 0x69:
                    com.Name = "StoreHeroPosition";
                    com.parameters.Add(reader.ReadUInt16()); //RV = X
                    com.parameters.Add(reader.ReadUInt16()); //RV = Y
                    break;
                case 0x6A:
                    com.Name = "StoreNPCPosition";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = X
                    com.parameters.Add(reader.ReadUInt16()); //RV = Y
                    break;
                case 0x6B:
                    com.Name = "6B";
                    com.parameters.Add(reader.ReadUInt16()); //8
                    com.parameters.Add(reader.ReadUInt16()); //0
                    com.parameters.Add(reader.ReadUInt16()); //0
                    break;
                case 0x6C:
                    com.Name = "SetFollowHeroMovement?";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id  
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x6D:
                    com.Name = "SetFollowHeroSprite?";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16()); //Depends from Hero orientation
                    break;
                case 0x6E:
                    com.Name = "6E";
                    break;
                case 0x6F:
                    com.Name = "GiveMoney";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x70:
                    com.Name = "TakeMoney";
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x71:
                    com.Name = "CheckMoney";
                    com.parameters.Add(reader.ReadUInt16()); //RV = T/F
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x72:
                    com.Name = "ShowMoney";
                    com.parameters.Add(reader.ReadUInt16()); //X coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate
                    break;

                case 0x73:
                    com.Name = "HideMoney";
                    break;

                case 0x74:
                    com.Name = "UpdateMoney";
                    break;

                case 0x75:
                    com.Name = "ShowCoin";
                    com.parameters.Add(reader.ReadUInt16()); //X coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate
                    break;

                case 0x76:
                    com.Name = "HideCoin";
                    break;

                case 0x77:
                    com.Name = "UpdateCoin";
                    break;

                case 0x78:
                    com.Name = "CompareCoin";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x79:
                    com.Name = "GiveCoin";
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    break;

                case 0x7A:
                    com.Name = "TakeCoin";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x7B:
                    com.Name = "GiveItem";
                    com.parameters.Add(reader.ReadUInt16()); //Item Id
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    com.parameters.Add(reader.ReadUInt16()); //RV = T/F
                    break;
                case 0x7C:
                    com.Name = "TakeItem";
                    com.parameters.Add(reader.ReadUInt16()); //Item Id
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    com.parameters.Add(reader.ReadUInt16()); //RV = T(Taken)/F(Bag Full)
                    break;
                case 0x7D:
                    com.Name = "CheckItemBagSpace";
                    com.parameters.Add(reader.ReadUInt16()); //Item Id
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    com.parameters.Add(reader.ReadUInt16()); //RV = Item Bag Number
                    break;
                case 0x7E:
                    com.Name = "CheckItemBagNumber";
                    com.parameters.Add(reader.ReadUInt16()); //Item
                    com.parameters.Add(reader.ReadUInt16()); //Mininum Amount
                    com.parameters.Add(reader.ReadUInt16()); //RV = T/F
                    break;
                case 0x7F:
                    com.Name = "StoreItemAmount";
                    com.parameters.Add(reader.ReadUInt16()); //Item
                    com.parameters.Add(reader.ReadUInt16()); //RV = Amount
                    break;
                case 0x80:
                    com.Name = "StoreBag";
                    com.parameters.Add(reader.ReadUInt16()); //Item
                    com.parameters.Add(reader.ReadUInt16()); //RV = Bag
                    break;
                case 0x83:
                    com.Name = "GiveUGItem(83)";
                    com.parameters.Add(reader.ReadUInt16()); //Item
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x85:
                    com.Name = "CheckUGItemSpace";
                    com.parameters.Add(reader.ReadUInt16()); //Item
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x87:
                    com.Name = "GiveUGItem(87)";
                    com.parameters.Add(reader.ReadUInt16()); //Item
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x8F:
                    com.Name = "GiveUGGems";
                    com.parameters.Add(reader.ReadUInt16()); //Gem
                    com.parameters.Add(reader.ReadUInt16()); //Dimension
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x93:
                    com.Name = "StorePoffin";
                    com.parameters.Add(reader.ReadUInt16()); //Poffin
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x94:
                    com.Name = "GivePoffin";
                    com.parameters.Add(reader.ReadUInt16()); //Poffin Id
                    com.parameters.Add(reader.ReadUInt16()); //Number
                    break;
                case 0x95:
                    com.Name = "StorePokèmonPartySpecieNumber";
                    com.parameters.Add(reader.ReadUInt16()); //Specie
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x96:
                    com.Name = "GivePokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x97:
                    com.Name = "GiveEgg";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x99:
                    com.Name = "CheckMoveLearned";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16()); //Move
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon
                    break;
                case 0x9A:
                    com.Name = "StoreMNPokèmon";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Pokèmon
                    com.parameters.Add(reader.ReadUInt16()); //Move
                    break;
                case 0x9B:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xA1:
                    com.Name = "CallEnd";
                    break;
                case 0xA2:
                    com.Name = "A2";
                    break;
                case 0xA3:
                    com.Name = "StartWFC";
                    break;
                case 0xA5:
                    com.Name = "StartInterview";
                    break;
                case 0xA6:
                    com.Name = "StartDressPokèmon";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = Dress Decision
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xA7:
                    com.Name = "ShowDressedPokèmon";
                    com.parameters.Add(reader.ReadUInt16()); //Picture Id
                    com.parameters.Add(reader.ReadUInt16()); //Picture Pointer?
                    break;
                case 0xA8:
                    com.Name = "ShowContestPokèmon";
                    com.parameters.Add(reader.ReadUInt16()); //Picture Id
                    com.parameters.Add(reader.ReadUInt16()); //Picture Pointer?
                    break;
                case 0xA9:
                    com.Name = "OpenBallCapsule";
                    break;
                case 0xAA:
                    com.Name = "OpenSinnohMap";
                    break;
                case 0xAB:
                    com.Name = "OpenPC";
                    com.parameters.Add(reader.ReadByte()); //Pc Called Function Id
                    break;
                case 0xAC:
                    com.Name = "StartDrawingUnion";
                    break;
                case 0xAD:
                    com.Name = "ShowTrainerCaseUnion";
                    break;
                case 0xAE:
                    com.Name = "StartTradingUnion";
                    break;
                case 0xAF:
                    com.Name = "ShowRecordUnion";
                    break;
                case 0xB0:
                    com.Name = "EndGame";
                    break;
                case 0xB1:
                    com.Name = "ShowHallOfFame";
                    break;
                case 0xB2:
                    com.Name = "CheckWFC";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //RV = T/F
                    break;
                case 0xB3:
                    com.Name = "StartWFC2";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xB4:
                    com.Name = "ChooseStarter";
                    break;
                case 0xB5:
                    com.Name = "BattleStarter";
                    break;
                case 0xB6:
                    com.Name = "StoreBattleID?";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xB7:
                    com.Name = "SetVarBattle?";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xB8:
                    com.Name = "StoreTypeBattle?";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xB9:
                    com.Name = "SetVarBattle2?";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xBA:
                    com.Name = "ChoosePlayerName";
                    break;
                case 0xBB:
                    com.Name = "ChoosePokèmonName";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0xBC:
                    com.Name = "FadeScreen";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xBD:
                    com.Name = "ResetScreen";
                    break;
                case 0xBE:
                    com.Name = "Warp";
                    com.parameters.Add(reader.ReadUInt16()); //Map Id
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //X coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Orientation
                    break;
                case 0xBF:
                    com.Name = "RockClimbAnimation";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0xC0:
                    com.Name = "SurfAnimation";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0xC1:
                    com.Name = "WaterfallAnimation";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0xC2:
                    com.Name = "FlyAnimation";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0xC3:
                    com.Name = "FlashAnimation";
                    break;
                case 0xC4:
                    com.Name = "DefogAnimation";
                    break;
                case 0xC5:
                    com.Name = "SetPokèmonAnimation";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0xC6:
                    com.Name = "Tuxedo";
                    break;
                case 0xC7:
                    com.Name = "CheckBike";
                    com.parameters.Add(reader.ReadUInt16()); //RV = T/F
                    break;
                case 0xC8:
                    com.Name = "RideBike";
                    com.parameters.Add(reader.ReadByte()); //On/Off
                    break;
                case 0xC9:
                    com.Name = "C9";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0xCB:
                    com.Name = "StartHeroAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xCC:
                    com.Name = "StopHeroAnimation";
                    break;
                case 0xCD:
                    com.Name = "SetVarHero";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    break;
                case 0xCE:
                    com.Name = "SetVarRival";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    break;
                case 0xCF:
                    com.Name = "SetVarAlter";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    break;
                case 0xD0:
                    com.Name = "SetVarPokémon";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0xD1:
                    com.Name = "SetVarItem";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Item Id
                    break;
                case 0xD2:
                    com.Name = "SetVarBag";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Bag Id
                    break;
                case 0xD3:
                    com.Name = "SetVarMove";
                    com.parameters.Add(reader.ReadByte()); //Text Var 
                    com.parameters.Add(reader.ReadUInt16()); //Move Id
                    break;
                case 0xD4:
                    com.Name = "SetVarAttack";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Attack Id
                    break;
                case 0xD5:
                    com.Name = "SetVarNumber";
                    com.parameters.Add(reader.ReadByte());//Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Number
                    break;
                case 0xD6:
                    com.Name = "SetVarNickPokémon";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Nick
                    break;
                case 0xD7:
                    com.Name = "SetVarPokèKronApp";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Pokèkron App
                    break;
                case 0xD8:
                    com.Name = "SetVarTrainer";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Trainer Id
                    break;
                case 0xD9:
                    com.Name = "SetVarTrainerDefault";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    break;
                case 0xDA:
                    com.Name = "SetVarPokèmonStored";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0xDB:
                    com.Name = "SetVarHeroStarter";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    break;
                case 0xDC:
                    com.Name = "SetVarRivalStarter";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    break;
                case 0xDE:
                    com.Name = "StoreStarter";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Starter Id
                    break;
                case 0xDF:
                    com.Name = "DF";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Underground Item Id
                    break;
                case 0xE0:
                    com.Name = "SetVarUGItem";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Underground Item Id
                    break;
                case 0xE1:
                    com.Name = "SetVarGem";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Gem Id
                    break;
                case 0xE2:
                    com.Name = "SetVarSwarmPlace";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Swarm Place Id
                    break;
                case 0xE3:
                    com.Name = "StoreSwarmInfo";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Swarm Place
                    com.parameters.Add(reader.ReadUInt16()); //RV = Swarm Pokèmon
                    break;
                case 0xE4:
                    com.Name = "E4";
                    com.parameters.Add(reader.ReadUInt16()); //Trainer Id
                    break;
                case 0xE5:
                    com.Name = "TrainerBattle";
                    com.parameters.Add(reader.ReadUInt16()); //Trainer Id
                    com.parameters.Add(reader.ReadUInt16()); //Ally Trainer Id
                    break;
                case 0xE6:
                    com.Name = "MessageBattle";
                    com.parameters.Add(reader.ReadUInt16()); //Trainer Id
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    break;
                case 0xE7:
                    com.Name = "StoreMessageBattle(E7)";
                    com.parameters.Add(reader.ReadUInt16()); //Win Message Id
                    com.parameters.Add(reader.ReadUInt16()); //Normal Message Id
                    com.parameters.Add(reader.ReadUInt16()); //Lost Message Id
                    break;
                case 0xE8:
                    com.Name = "StoreMessageBattle(E8)";
                    com.parameters.Add(reader.ReadUInt16()); //Win Message Id
                    com.parameters.Add(reader.ReadUInt16()); //Normal Message Id
                    com.parameters.Add(reader.ReadUInt16()); //Lost Message Id
                    break;
                case 0xE9:
                    com.Name = "E9";
                    com.parameters.Add(reader.ReadUInt16()); //RV = T/F
                    break;
                case 0xEA:
                    com.Name = "ActTrainer";
                    com.parameters.Add(reader.ReadUInt16()); //Trainer Id
                    break;
                case 0xEB:
                    com.Name = "TeleportPC";
                    break;
                case 0xEC:
                    com.Name = "StoreBattleResult";
                    com.parameters.Add(reader.ReadUInt16()); //RV 
                    break;
                case 0xED:
                    com.Name = "StoreBattleResult2";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0xEE:
                    com.Name = "CheckDoubleBattle";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0xF2:
                    com.Name = "StoreWirelessResponse";
                    com.parameters.Add(reader.ReadUInt16()); //Function Id
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //RV = Response
                    break;
                case 0xF3:
                    com.Name = "StoreWirelessResponseHead";
                    com.parameters.Add(reader.ReadUInt16()); //Function Id
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //RV = Response
                    break;
                case 0xF6:
                    com.Name = "F6";
                    break;
                case 0xF7:
                    com.Name = "SetStatusPokèmonContest";
                    com.parameters.Add(reader.ReadUInt16()); //T/F
                    break;
                case 0xF8:
                    com.Name = "StartOvation";
                    com.parameters.Add(reader.ReadUInt16()); //Sound Id
                    break;
                case 0xF9:
                    com.Name = "StopOvation";
                    com.parameters.Add(reader.ReadUInt16()); //Sound Id
                    break;
                case 0xFA:
                    com.Name = "FA";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xFB:
                    com.Name = "FB";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xFD:
                    com.Name = "SetVarNamePartContest";
                    com.parameters.Add(reader.ReadUInt16()); //Part Id
                    com.parameters.Add(reader.ReadUInt16()); //Text Var
                    break;
                case 0xFE:
                    com.Name = "FE";
                    com.parameters.Add(reader.ReadUInt16()); //Part Id
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xFF:
                    com.Name = "SetVarPartIdContest";
                    com.parameters.Add(reader.ReadUInt16()); //Part Id
                    com.parameters.Add(reader.ReadUInt16()); //Text Var
                    break;
                case 0x101:
                    com.Name = "BlackScreenEffect";
                    break;
                case 0x102:
                    com.Name = "SetVarModeContest";
                    com.parameters.Add(reader.ReadUInt16()); //Text Var
                    break;
                case 0x103:
                    com.Name = "SetVarTypeContest";
                    com.parameters.Add(reader.ReadUInt16()); //Text Var
                    break;
                case 0x104:
                    com.Name = "SetVarNameWinnerContest";
                    com.parameters.Add(reader.ReadUInt16()); //Text Var
                    break;
                case 0x106:
                    com.Name = "SetVarPokèWinnerContest";
                    com.parameters.Add(reader.ReadUInt16()); //Text Var
                    break;
                case 0x107:
                    com.Name = "107";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x108:
                    com.Name = "StoreVarPeopleId";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x109:
                    com.Name = "StoreVarPartId";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x10A:
                    com.Name = "10A";
                    com.parameters.Add(reader.ReadUInt16()); //Part Id
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x10B:
                    com.Name = "StoreContestTrainerNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //RV = Trainer Number
                    break;
                case 0x10C:
                    com.Name = "StoreContestType";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Type Contest
                    break;
                case 0x10D:
                    com.Name = "CheckWinContest";
                    com.parameters.Add(reader.ReadUInt16()); //RV = T/F
                    break;
                case 0x10E:
                    com.Name = "SetVarItemWinContest";
                    com.parameters.Add(reader.ReadUInt16()); //Text Var
                    break;
                case 0x10F:
                    com.Name = "StoreItemWinContest";
                    com.parameters.Add(reader.ReadUInt16()); //Item win
                    break;
                case 0x110:
                    com.Name = "CallContestFunction";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //Contest Level
                    com.parameters.Add(reader.ReadUInt16()); //Contest Moment
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x111:
                    com.Name = "StartFlashContest";
                    com.parameters.Add(reader.ReadUInt16()); //Times
                    break;
                case 0x112:
                    com.Name = "EndFlashContest";
                    break;
                case 0x113:
                    com.Name = "CarpetAnimationContest";
                    break;
                case 0x114:
                    com.Name = "114";
                    break;
                case 0x115:
                    com.Name = "CheckContestEnd";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x116:
                    com.Name = "116";
                    break;
                case 0x117:
                    com.Name = "117";
                    break;
                case 0x118:
                    com.Name = "118";
                    break;
                case 0x119:
                    com.Name = "CheckPokèrus";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x11B:
                    com.Name = "WarpMapLift";
                    com.parameters.Add(reader.ReadUInt16()); //Map Id
                    com.parameters.Add(reader.ReadUInt16()); //X coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Z coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Orientation
                    break;
                case 0x11C:
                    com.Name = "StoreFloor";
                    com.parameters.Add(reader.ReadUInt16()); //Floor Id
                    break;
                case 0x11D:
                    com.Name = "StartLift";
                    break;
                case 0x11E:
                    com.Name = "StorePokèmonNumberCaught(11E)";
                    com.parameters.Add(reader.ReadUInt16()); //Total Pokèmon Caught
                    break;
                case 0x120:
                    com.Name = "StorePokèmonNumberCaught(120)";
                    com.parameters.Add(reader.ReadUInt16()); //Total Pokèmon Caught
                    break;
                case 0x121:
                    com.Name = "121";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x123:
                    com.Name = "123";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x124:
                    com.Name = "WildPokèmonBattle";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16()); //Level
                    break;
                case 0x125:
                    com.Name = "RivalBattle?";
                    com.parameters.Add(reader.ReadUInt16()); //Trainer Id
                    break;
                case 0x126:
                    com.Name = "ExampleCaughtBattle";
                    break;
                case 0x127:
                    com.Name = "HoneyEffect";
                    break;
                case 0x128:
                    com.Name = "StoreHoneyStatus";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Honey Status
                    break;
                case 0x129:
                    com.Name = "StartHoneyBattle";
                    break;
                case 0x12A:
                    com.Name = "EndHoneyBattle";
                    break;
                case 0x12B:
                    com.Name = "12B";
                    break;
                case 0x12C:
                    com.Name = "StoreStatusSave";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Save Status
                    break;
                case 0x12D:
                    com.Name = "CheckErrorSave";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x12E:
                    com.Name = "CheckDressPicture";
                    com.parameters.Add(reader.ReadUInt16()); //Picture Id
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x12F:
                    com.Name = "CheckContestPicture";
                    com.parameters.Add(reader.ReadUInt16()); //Picture Id
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x130:
                    com.Name = "StorePictureTitle";
                    com.parameters.Add(reader.ReadUInt16()); //Picture Title
                    break;
                case 0x131:
                    com.Name = "ActPokèKron";
                    break;
                case 0x132:
                    com.Name = "CheckPokèKron";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x133:
                    com.Name = "ActPokèKronApplication";
                    com.parameters.Add(reader.ReadUInt16()); //Application Id
                    break;
                case 0x134:
                    com.Name = "CheckPokèKronApplication";
                    com.parameters.Add(reader.ReadUInt16()); //Application Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = Status
                    break;
                case 0x135:
                    com.Name = "135";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x136:
                    com.Name = "136";
                    break;
                case 0x138:
                    com.Name = "138";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    break;
                case 0x139:
                    com.Name = "139";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x13A:
                    com.Name = "13A";
                    break;
                case 0x13B:
                    com.Name = "13B";
                    break;
                case 0x13C:
                    com.Name = "13C";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x13D:
                    com.Name = "13D";
                    break;
                case 0x13E:
                    com.Name = "13E";
                    break;
                case 0x13F:
                    com.Name = "13F";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x140:
                    com.Name = "140";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x141:
                    com.Name = "141";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x142:
                    com.Name = "142";
                    break;
                case 0x143:
                    com.Name = "SetStatusUnion";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x144:
                    com.Name = "144";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x145:
                    com.Name = "StoreUnionAlterChoice";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x146:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x147:
                    com.Name = "PokèMart(147)";
                    com.parameters.Add(reader.ReadUInt16()); //Mart Type
                    break;
                case 0x148:
                    com.Name = "PokèMart(148)";
                    com.parameters.Add(reader.ReadUInt16()); //Mart Type
                    break;
                case 0x149:
                    com.Name = "PokèMart(149)";
                    com.parameters.Add(reader.ReadUInt16()); //Mart Type
                    break;
                case 0x14A:
                    com.Name = "PokèMart(14A)";
                    com.parameters.Add(reader.ReadUInt16()); //Mart Type
                    break;
                case 0x14B:
                    com.Name = "LostBattle(14B)";
                    break;
                case 0x14C:
                    com.Name = "ActBike?";
                    com.parameters.Add(reader.ReadUInt16()); //#9 - Unknown
                    break;
                case 0x14D:
                    com.Name = "StoreGender";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Gender
                    break;
                case 0x14E:
                    com.Name = "HealPokèmon";
                    break;
                case 0x14F:
                    com.Name = "14F";
                    break;
                case 0x150:
                    com.Name = "EndWirelessFunction(150)";
                    break;
                case 0x151:
                    com.Name = "151";
                    break;
                case 0x152:
                    com.Name = "152";
                    com.parameters.Add(reader.ReadUInt16()); //#3
                    break;
                case 0x153:
                    com.Name = "153";
                    break;
                case 0x154:
                    com.Name = "StartChooseWifiSprite";
                    break;
                case 0x155:
                    com.Name = "ChooseWifiSprite";
                    com.parameters.Add(reader.ReadUInt16()); //Modality?
                    com.parameters.Add(reader.ReadUInt16()); //RV = Wifi Sprite
                    break;
                case 0x156:
                    com.Name = "ActWifiSprite";
                    com.parameters.Add(reader.ReadUInt16()); //Wifi Sprite
                    break;
                case 0x158:
                    com.Name = "ActSinnohPokèdex";
                    break;
                case 0x15A:
                    com.Name = "ActRunningShoe";
                    break;
                case 0x15B:
                    com.Name = "CheckBadge";
                    com.parameters.Add(reader.ReadUInt16()); //Badge Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = Have/Don't have
                    break;
                case 0x15C:
                    com.Name = "SetBadge";
                    com.parameters.Add(reader.ReadUInt16()); //Badge Id
                    break;
                case 0x15D:
                    com.Name = "StoreBadgeNumber";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Badge Number
                    break;
                case 0x15F:
                    com.Name = "ActRunningShoes2";
                    break;
                case 0x160:
                    com.Name = "CheckFollowHero";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Active/Not active
                    break;
                case 0x161:
                    com.Name = "StartFollowHero(161)";
                    break;
                case 0x162:
                    com.Name = "StopFollowHero(162)";
                    break;
                case 0x163:
                    com.Name = "163";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x164:
                    com.Name = "StartFollowHero(164)";
                    break;
                case 0x166:
                    com.Name = "166";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x168:
                    com.Name = "PrepareDoorAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x169:
                    com.Name = "WaitDoorOpeningAnimation";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x16A:
                    com.Name = "WaitDoorClosingAnimation";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x16B:
                    com.Name = "DoorOpeningAnimation";
                    com.parameters.Add(reader.ReadByte()); ;
                    break;
                case 0x16C:
                    com.Name = "DoorClosingAnimation";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x16D:
                    com.Name = "StartPokèmonDayCare";
                    break;
                case 0x16E:
                    com.Name = "StoreDayCareStatus";
                    com.parameters.Add(reader.ReadUInt16());//RV
                    break;
                case 0x16F:
                    com.Name = "PastoriaGym(16F)";
                    break;
                case 0x170:
                    com.Name = "PastoriaGym(170)";
                    break;
                case 0x171:
                    com.Name = "HearthomeGym(171)";
                    break;
                case 0x172:
                    com.Name = "HearthomeGym(172)";
                    break;
                case 0x173:
                    com.Name = "OreburghGym";
                    break;
                case 0x174:
                    com.Name = "VeilstoneGym";
                    break;
                case 0x175:
                    com.Name = "SunyShoreGym(175)";
                    com.parameters.Add(reader.ReadByte()); //9,1,2
                    break;
                case 0x176:
                    com.Name = "SunyShoreGym(176)";
                    com.parameters.Add(reader.ReadByte()); //0,1,2
                    break;
                case 0x177:
                    com.Name = "StorePokèmonPartyNumber(177)";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Pokèmon Number
                    break;
                case 0x178:
                    com.Name = "SetBerry";
                    com.parameters.Add(reader.ReadByte()); //RV
                    break;
                case 0x179:
                    com.Name = "StoreItemBerry";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Fertilizer/Berry
                    break;
                case 0x17A:
                    com.Name = "CheckPlantingLimit";
                    com.parameters.Add(reader.ReadUInt16()); //Limit(Quantity of Berry you can plant)
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 379:
                    com.Name = "SetVarGrowingBerry(P)";
                    com.parameters.Add(reader.ReadUInt16()); //Text Var
                    break;
                case 0x17C:
                    com.Name = "SetVarPokèmonNature";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Nature
                    break;
                case 0x17D:
                    com.Name = "StoreBerryGrownState";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Grown State
                    break;
                case 0x17E:
                    com.Name = "StoreBerryMatureType";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Berry Id
                    break;
                case 0x17F:
                    com.Name = "StoreFertilizer";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Fertilizer Id
                    break;
                case 384:
                    com.Name = "BerryGrownAnimation(P)";
                    com.parameters.Add(reader.ReadByte()); //Status
                    break;
                case 0x181:
                    com.Name = "StoreBerryMatureNumber";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Number Available Berries
                    break;
                case 0x182:
                    com.Name = "AddFertilizer";
                    com.parameters.Add(reader.ReadUInt16()); //Fertilizer Id
                    break;
                case 0x183:
                    com.Name = "PlantBerry";
                    com.parameters.Add(reader.ReadUInt16()); //Berry Id
                    break;
                case 0x184:
                    com.Name = "PlantingBerryAnimation";
                    com.parameters.Add(reader.ReadUInt16()); //Status
                    break;
                case 0x185:
                    com.Name = "RestoreSoil";
                    break;
                case 0x186:
                    com.Name = "SetNPCPosition";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16()); //X
                    com.parameters.Add(reader.ReadUInt16()); //Y
                    break;
                case 0x187:
                    com.Name = "RelocateNPC";
                    com.parameters.Add(reader.ReadUInt16()); // NPC Id
                    com.parameters.Add(reader.ReadUInt16()); // X container
                    com.parameters.Add(reader.ReadUInt16()); // 
                    com.parameters.Add(reader.ReadUInt16()); // Y container
                    com.parameters.Add(reader.ReadUInt16()); //
                    break;
                case 0x188:
                    com.Name = "SetNPCScript?";
                    com.parameters.Add(reader.ReadUInt16()); // NPC Id
                    com.parameters.Add(reader.ReadUInt16()); // Script?
                    break;
                case 0x189:
                    com.Name = "SetNPCMovement?";
                    com.parameters.Add(reader.ReadUInt16()); // NPC Id
                    com.parameters.Add(reader.ReadUInt16()); // Movement?
                    break;
                case 0x18A:
                    com.Name = "SetTilePassable";
                    com.parameters.Add(reader.ReadUInt16()); //Orientation
                    com.parameters.Add(reader.ReadUInt16()); //X
                    com.parameters.Add(reader.ReadUInt16()); //Y
                    break;
                case 0x18B:
                    com.Name = "SetTileLocked";
                    com.parameters.Add(reader.ReadUInt16()); //Orientation
                    com.parameters.Add(reader.ReadUInt16()); //X
                    com.parameters.Add(reader.ReadUInt16()); //Y
                    break;
                case 0x18C:
                    com.Name = "18C";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16()); //Depends from Orientation
                    break;
                case 0x18D:
                    com.Name = "StartErrorCheckSaving";
                    break;
                case 0x18E:
                    com.Name = "EndErrorCheckSaving";
                    break;
                case 0x18F:
                    com.Name = "UpdateSaveStatus";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x190:
                    com.Name = "SavingGame";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x191:
                    com.Name = "ShowChoosePokèmonMenu";
                    break;
                case 0x192:
                    com.Name = "ShowBattleUnion";
                    break;
                case 0x193:
                    com.Name = "StoreChosenPokèmon";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Pokèmon Id
                    break;
                case 0x194:
                    com.Name = "ChoosePokèmonContest";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //Match Level
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //Match Type
                    break;
                case 0x195:
                    com.Name = "StoreChosenPokèmonContest";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Chosen Status
                    com.parameters.Add(reader.ReadUInt16()); //RV = Pokèmon Id
                    break;
                case 0x196:
                    com.Name = "196";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x197:
                    com.Name = "197";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x198:
                    com.Name = "StorePokèmonSpecie";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = Specie
                    break;
                case 0x199:
                    com.Name = "CheckPokèmonNickChanged";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = Changed/Not Changed
                    break;
                case 0x19A:
                    com.Name = "StorePokèmonPartyNumber(19A)";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Number
                    break;
                case 0x19B:
                    com.Name = "CheckPokèmonPartyNumber(19B)";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Have/Don't have
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    break;
                case 0x19C:
                    com.Name = "StorePokèmonPartyNumber(19C)";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Number
                    break;
                case 0x19D:
                    com.Name = "CheckEgg";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x19E:
                    com.Name = "19E";
                    com.parameters.Add(reader.ReadUInt16()); //0,1,2
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x19F:
                    com.Name = "19F";
                    com.parameters.Add(reader.ReadUInt16()); //0
                    break;
                case 0x1A0:
                    com.Name = "1A0";
                    break;
                case 0x1A3:
                    com.Name = "TakeMoneyDayCare";
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    break;
                case 0x1A4:
                    com.Name = "TakePokèmonDayCare";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1A8:
                    com.Name = "TakeEggDayCare";
                    break;
                case 0x1A9:
                    com.Name = "GiveEggDayCare";
                    break;
                case 0x1AA:
                    com.Name = "StoreMoneyDayCare";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Amount
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1AB:
                    com.Name = "CheckEnoughMoney";
                    com.parameters.Add(reader.ReadUInt16()); //RV = T/F
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    break;
                case 0x1AC:
                    com.Name = "EggAnimation";
                    break;
                case 0x1AE:
                    com.Name = "CheckLevelDiffDayCare";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16()); //Level Difference?
                    break;
                case 0x1AF:
                    com.Name = "SetVarDataDayCare";
                    com.parameters.Add(reader.ReadUInt16()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Chosen Pokèmon
                    com.parameters.Add(reader.ReadUInt16()); //RV = Pokèmon Id
                    break;
                case 0x1B0:
                    com.Name = "GivePokèmonDayCare";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0x1B1:
                    com.Name = "1B1";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    break;
                case 0x1B2:
                    com.Name = "1B2";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    break;
                case 0x1B3:
                    com.Name = "OpenMail";
                    break;
                case 0x1B4:
                    com.Name = "CheckMail";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x1B5:
                    com.Name = "ShowRecordList";
                    com.parameters.Add(reader.ReadUInt16()); //List Id
                    break;
                case 0x1B6:
                    com.Name = "StoreTime";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Time
                    break;
                case 0x1B7:
                    com.Name = "StoreRandomVariable(1B7)";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16()); //Range
                    break;
                case 0x1B8:
                    com.Name = "StoreRandomVariable(1B8)";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16()); //Range
                    break;
                case 0x1B9:
                    com.Name = "StorePokèmonHappines";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0x1BA:
                    com.Name = "IncPokèmonHappines";
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0x1BC:
                    com.Name = "1BC";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1BD:
                    com.Name = "StoreHeroFaceOrientation";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x1BE:
                    com.Name = "StoreDayCareAffinity";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x1BF:
                    com.Name = "1BF";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1C0:
                    com.Name = "CheckHavePartyPokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0x1C1:
                    com.Name = "StorePokèmonSize";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0x1C2:
                    com.Name = "1C2";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1C3:
                    com.Name = "SetVarChosenPokèmonSize";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1C4:
                    com.Name = "SetVarRequestedPokèmonSize";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1C5:
                    com.Name = "1C5";
                    break;
                case 0x1C6:
                    com.Name = "StorePokèmonDeleter";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x1C7:
                    com.Name = "StoreMoveDeleter";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x1C8:
                    com.Name = "StorePokèmonMoveNumber";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Move Number
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0x1C9:
                    com.Name = "DeletePokèmonMove";
                    com.parameters.Add(reader.ReadUInt16()); //Move Id
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0x1CA:
                    com.Name = "StoreMoveForgotten";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16()); //Move Number
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0x1CB:
                    com.Name = "SetVarMoveDeleter";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Move Number
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0x1CC:
                    com.Name = "ActJournal";
                    break;
                case 0x1CD:
                    com.Name = "FunctionNPC?";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id?
                    com.parameters.Add(reader.ReadUInt16()); //X?
                    com.parameters.Add(reader.ReadUInt16()); //Y?
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //Orientation?
                    break;
                case 0x1CF:
                    com.Name = "CallStrenghtFunction";
                    com.parameters.Add(reader.ReadByte()); //Function Id
                    var next1CF = reader.ReadUInt16(); //RV = Strenght Active (If Function Id==2);
                    if (next1CF > 1000)
                        com.parameters.Add(next1CF);
                    else
                        reader.BaseStream.Position -= 2;
                    break;
                case 0x1D0:
                    com.Name = "SetFlash";
                    com.parameters.Add(reader.ReadByte()); //Status
                    break;
                case 0x1D1:
                    com.Name = "SetDefog";
                    com.parameters.Add(reader.ReadByte()); //Status
                    break;
                case 0x1D2:
                    com.Name = "GiveAccessories";
                    com.parameters.Add(reader.ReadUInt16()); //Accessories Id
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    break;
                case 0x1D3:
                    com.Name = "CheckAccessories";
                    com.parameters.Add(reader.ReadUInt16()); //Accessories Id
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x1D5:
                    com.Name = "GiveBackground";
                    com.parameters.Add(reader.ReadUInt16()); //Background Id
                    break;
                case 0x1D6:
                    com.Name = "CheckHaveBackground";
                    com.parameters.Add(reader.ReadUInt16()); //Background Id
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x1D7:
                    com.Name = "StartPoffinMixer";
                    com.parameters.Add(reader.ReadUInt16()); //Modality
                    break;
                case 0x1D8:
                    com.Name = "StoreStatusPoffinMixer";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x1D9:
                    com.Name = "1D9";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1DA:
                    com.Name = "StartBattleTower";
                    break;
                case 0x1DB:
                    com.Name = "SetBattleTowerStatus";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //Battle Type
                    break;
                case 0x1DC:
                    com.Name = "SavingBattleTower";
                    break;
                case 0x1DD:
                    com.Name = "CallRoutineBattleTower";
                    com.parameters.Add(reader.ReadUInt16()); //Routine Id
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x1DE:
                    com.Name = "StorePokèmonMoveBattleTower";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16()); //Slot?
                    com.parameters.Add(reader.ReadUInt16()); //RV = Move Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = Pokèmon Id
                    break;
                case 0x1DF:
                    com.Name = "StoreWinSerieBattleTower";
                    com.parameters.Add(reader.ReadUInt16()); //Win Number
                    break;
                case 0x1E0:
                    com.Name = "1E0";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x1E1:
                    com.Name = "CheckWirelessResponse?";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E2:
                    com.Name = "1E2";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E3:
                    com.Name = "StoreBestTrainerData";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Level
                    com.parameters.Add(reader.ReadUInt16()); //RV = Hal
                    break;
                case 0x1E4:
                    com.Name = "CheckTrainerData";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x1E5:
                    com.Name = "SpecialEvent?";
                    com.parameters.Add(reader.ReadUInt16()); //Event Id
                    break;
                case 0x1E8:
                    com.Name = "StoreSinnohPokèdexCompleteStatus";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x1E9:
                    com.Name = "StoreNationalPokèdexCompleteStatus";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x1EA:
                    com.Name = "GiveSinnohDiploma";
                    break;
                case 0x1EB:
                    com.Name = "GiveNationalDiploma";
                    break;
                case 0x1EC:
                    com.Name = "StartTrophyGarden";
                    break;
                case 0x1ED:
                    com.Name = "StoreRandomPokèmonTrophy";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0x1F1:
                    com.Name = "StoreFossilNumber";
                    com.parameters.Add(reader.ReadUInt16()); //Fossil Id
                    break;
                case 0x1F4:
                    com.Name = "StoreFossilPokèmon";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16()); //Fossil Id
                    break;
                case 0x1F5:
                    com.Name = "TakeFossil";
                    com.parameters.Add(reader.ReadUInt16()); //Fossil Id
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    break;
                case 0x1F6:
                    com.Name = "StorePokèmonPartyAtLevel";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16()); //Level
                    break;
                case 0x1F7:
                    com.Name = "CheckPoisoned";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0x1F8:
                    com.Name = "HealPoisoned";
                    break;
                case 0x1F9:
                    com.Name = "StoreStarCardNumber";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Star Number
                    break;
                case 0x1FB:
                    com.Name = "1FB";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1FE:
                    com.Name = "1FE";
                    com.parameters.Add(reader.ReadByte()); //0,1
                    break;
                case 0x1FF:
                    com.Name = "1FF";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x200:
                    com.Name = "200";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x201:
                    com.Name = "201";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x202:
                    com.Name = "SetSafariGame";
                    com.parameters.Add(reader.ReadByte()); //Status
                    break;
                case 0x203:
                    com.Name = "203";
                    com.parameters.Add(reader.ReadUInt16()); //332,333 
                    com.parameters.Add(reader.ReadUInt16()); //0
                    com.parameters.Add(reader.ReadUInt16()); //6,7
                    com.parameters.Add(reader.ReadUInt16()); //11
                    com.parameters.Add(reader.ReadUInt16()); //0
                    break;
                case 0x204:
                    com.Name = "WarpLastElevator";
                    break;
                case 0x205:
                    com.Name = "OpenGeoNet";
                    break;
                case 0x206:
                    com.Name = "ShowBinocole";
                    break;
                case 0x207:
                    com.Name = "207";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x208:
                    com.Name = "ShowPokèmonPicture";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16()); //X
                    com.parameters.Add(reader.ReadUInt16()); //Y
                    break;
                case 0x209:
                    com.Name = "ClosePokèmonPicture";
                    break;
                case 0x20A:
                    com.Name = "20A";
                    com.parameters.Add(reader.ReadUInt16());//Stored
                    break;
                case 0x20B:
                    com.Name = "20B";
                    break;
                case 0x20C:
                    com.Name = "20C";
                    break;
                case 0x20D:
                    com.Name = "20D"; //Recursive Command 
                    com.parameters.Add(reader.ReadByte()); //0..9 
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x20E:
                    com.Name = "StartGreatMarsh";
                    break;
                case 0x20F:
                    com.Name = "GreatMarsh(20F)";
                    com.parameters.Add(reader.ReadUInt16()); //2
                    com.parameters.Add(reader.ReadUInt16()); //3
                    break;
                case 0x210:
                    com.Name = "StoreGreatMarsh(210)";
                    com.parameters.Add(reader.ReadUInt16()); //0,1,2
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x211:
                    com.Name = "CallTramAnimationGreatMarsh";
                    com.parameters.Add(reader.ReadByte()); //Animation Id
                    break;
                case 0x212:
                    com.Name = "StorePokèmonNature";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Nature
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0x213:
                    com.Name = "CheckPokèmonPartyNature";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16()); //Nature
                    break;
                case 0x214:
                    com.Name = "StoreUGFriendNumber";
                    com.parameters.Add(reader.ReadUInt16()); //RV 
                    break;
                case 0x215:
                    com.Name = "215";
                    break;
                case 0x216:
                    com.Name = "216";
                    com.parameters.Add(reader.ReadUInt16()); //RV(200)
                    break;
                case 0x217:
                    com.Name = "217";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Accessories Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = Pokèmon Id
                    break;
                case 0x218:
                    com.Name = "StoreRandomPokèmonSearch";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Pokèmon Id
                    break;
                case 0x219:
                    com.Name = "ActRandomPokèmonSearch";
                    com.parameters.Add(reader.ReadUInt16()); //T/F
                    break;
                case 0x21A:
                    com.Name = "CheckCaughtRandomPokèmonSearch";
                    com.parameters.Add(reader.ReadUInt16()); //T/F
                    break;
                case 0x21B:
                    com.Name = "ActSwarming?";
                    break;
                case 0x21C:
                    com.Name = "21C";
                    com.parameters.Add(reader.ReadByte()); //NPC Id
                    break;
                case 0x21D:
                    com.Name = "Function(21D)";
                    com.parameters.Add(reader.ReadUInt16());
                    var next = reader.ReadUInt16();
                    while (next > 3000 || next < 10)
                    {
                        com.parameters.Add(next);
                        next = reader.ReadUInt16();
                    }

                    if (next < 3000 && next > 10)
                        reader.BaseStream.Position -= 2;
                    break;
                case 0x21F:
                    com.Name = "CheckPokèMoveTeacherCompatibility";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0x221:
                    com.Name = "StoreMoveTeacher";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x223:
                    com.Name = "StorePokèmonMoveTeacher";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x224:
                    com.Name = "TeachPokèmonMove";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16()); //Move Id
                    break;
                case 0x225:
                    com.Name = "StoreTeachingResult";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x226:
                    com.Name = "SetTradeId";
                    com.parameters.Add(reader.ReadByte()); //Trade Id
                    break;
                case 0x228:
                    com.Name = "StoreChosenPokèmonTrade";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x229:
                    com.Name = "TradePokèmon";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0x22A:
                    com.Name = "EndTrade";
                    break;
                case 0x22B:
                    com.Name = "ActForeignLanguagePokèdex";
                    break;
                case 0x22C:
                    com.Name = "ActMFPokèdex";
                    break;
                case 0x22D:
                    com.Name = "CheckPokèdexStatus";
                    com.parameters.Add(reader.ReadByte()); //Pokèdex Type
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x22F:
                    com.Name = "StoreRibbonNumber";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Ribbon
                    break;
                case 0x230:
                    com.Name = "CheckRibbonPokèmon";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16()); //Ribbon
                    break;
                case 0x231:
                    com.Name = "GiveRibbonPokèmon";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16()); //Ribbon
                    break;
                case 0x232:
                    com.Name = "SetVarRibbon";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Ribbon Id
                    break;
                case 0x233:
                    com.Name = "233";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0x234:
                    com.Name = "StoreDay";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x235:
                    com.Name = "Function(235)";
                    com.parameters.Add(reader.ReadUInt16());
                    next = reader.ReadUInt16();
                    while (next > 3000 || next < 10)
                    {
                        com.parameters.Add(next);
                        next = reader.ReadUInt16();
                    }
                    if (next < 3000 && next > 10)
                        reader.BaseStream.Position -= 2;
                    break;
                case 0x236:
                    com.Name = "StoreItem(236)";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x237:
                    com.Name = "StartInterview(237)";
                    com.parameters.Add(reader.ReadUInt16()); //Interview Status
                    com.parameters.Add(reader.ReadUInt16()); //Interview Id
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x238:
                    com.Name = "CheckInterviewDone";
                    com.parameters.Add(reader.ReadUInt16()); //Interview Id
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x239:
                    com.Name = "239";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x23A:
                    com.Name = "StoreFootPokèmon";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Classification
                    com.parameters.Add(reader.ReadUInt16()); //RV = Have/Not have footprints
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0x23B:
                    com.Name = "PokèballPcAnimation";
                    com.parameters.Add(reader.ReadUInt16()); //Number of Pokèball
                    break;
                case 0x23C:
                    com.Name = "LiftAnimation";
                    com.parameters.Add(reader.ReadUInt16()); //Up,Down
                    com.parameters.Add(reader.ReadUInt16()); //3,4
                    break;
                case 0x23D:
                    com.Name = "ShipAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x23E:
                    com.Name = "Function(23E)";
                    com.parameters.Add(reader.ReadUInt16());
                    next = reader.ReadUInt16();
                    while (next > 3000 || next < 2)
                    {
                        com.parameters.Add(next);
                        next = reader.ReadUInt16();
                    }

                    if (next < 3000 && next > 2)
                        reader.BaseStream.Position -= 2;
                    break;
                case 0x243:
                    com.Name = "StoreSinglePhraseBoxInput";
                    com.parameters.Add(reader.ReadUInt16()); //0
                    com.parameters.Add(reader.ReadUInt16()); //RV = Insert/Abort
                    com.parameters.Add(reader.ReadUInt16()); //RV = Word
                    break;
                case 0x244:
                    com.Name = "StoreDoublePhraseBoxInput";
                    com.parameters.Add(reader.ReadUInt16()); //0
                    com.parameters.Add(reader.ReadUInt16()); //RV = Insert/Abort
                    com.parameters.Add(reader.ReadUInt16()); //RV = Word
                    com.parameters.Add(reader.ReadUInt16()); //RV = Word
                    break;
                case 0x245:
                    com.Name = "SetVarPhraseBoxInput";
                    com.parameters.Add(reader.ReadUInt16()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Word
                    break;
                case 0x246:
                    com.Name = "246";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x247:
                    com.Name = "StoreFirstPokèmonParty";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Pokèmon Id
                    break;
                case 0x248:
                    com.Name = "StoreDragonMovePokèmonParty";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Happy?
                    com.parameters.Add(reader.ReadUInt16()); //RV = Type?
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0x249:
                    com.Name = "CheckPhraseBoxInputForPicture";
                    com.parameters.Add(reader.ReadUInt16()); //RV 
                    com.parameters.Add(reader.ReadUInt16()); //Your 1st Word
                    com.parameters.Add(reader.ReadUInt16()); //Your 2nd Word
                    com.parameters.Add(reader.ReadUInt16()); //Your 3rd Word
                    com.parameters.Add(reader.ReadUInt16()); //Your 4th Word
                    break;
                case 0x24A:
                    com.Name = "24A";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x24B:
                    com.Name = "PcAnimation(24B)";
                    com.parameters.Add(reader.ReadByte()); //Animation Id
                    break;
                case 0x24C:
                    com.Name = "PcAnimation(24C)";
                    com.parameters.Add(reader.ReadByte()); //Animation Id
                    break;
                case 0x24D:
                    com.Name = "PcAnimation(24D)";
                    com.parameters.Add(reader.ReadByte()); //Animation Id
                    break;
                case 0x24E:
                    com.Name = "StorePokèLottoTicketId";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Lotto Ticket Id
                    break;
                case 0x24F:
                    com.Name = "StorePokèLottoResult";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = Number of corr.numbers
                    com.parameters.Add(reader.ReadUInt16()); //RV = Is party pokèmon?
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x251:
                    com.Name = "SetVarLottoPokèmonBox";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id (From box PC)
                    break;
                case 0x252:
                    com.Name = "CheckBoxSpace";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x253:
                    com.Name = "StartPalPark";
                    com.parameters.Add(reader.ReadUInt16()); //T/F
                    break;
                case 0x254:
                    com.Name = "StorePalParkNumber";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x255:
                    com.Name = "SavePCPalParkPokèmon";
                    break;
                case 0x256:
                    com.Name = "StorePalParkPoint";
                    com.parameters.Add(reader.ReadUInt16()); //Type
                    com.parameters.Add(reader.ReadUInt16()); //RV = Point
                    break;
                case 0x257:
                    com.Name = "257";
                    break;
                case 0x258:
                    com.Name = "SavingLotData";
                    break;
                case 0x259:
                    com.Name = "EndSaveLotData";
                    break;
                case 0x25A:
                    com.Name = "HallOfFame";
                    com.parameters.Add(reader.ReadUInt16()); //Party Number
                    break;
                case 0x25B:
                    com.Name = "StartElevatorAnimation";
                    break;
                case 0x25C:
                    com.Name = "EndElevatorAnimation";
                    break;
                case 0x25D:
                    com.Name = "CheckElevatorStatus";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x25E:
                    com.Name = "StartGalaxyEvent(25E)";
                    break;
                case 0x25F:
                    com.Name = "EndGalaxyEvent(25F)";
                    break;
                case 0x260:
                    com.Name = "PlayAnimation";
                    com.parameters.Add(reader.ReadUInt16()); //Animation Id
                    break;
                case 0x261:
                    com.Name = "SetVarAccessories(261)";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Accessories Id
                    break;
                case 0x262:
                    com.Name = "CheckPokèmonCaught";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = Have in your PC/Party that Pokèmon?
                    break;
                case 0x263:
                    com.Name = "ChangePokèmonForm";
                    com.parameters.Add(reader.ReadUInt16()); //Form Id
                    com.parameters.Add(reader.ReadUInt16()); //76
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16()); //0
                    break;
                case 0x264:
                    com.Name = "StoreBurmyFormsNumber";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Burmy Forms Caught
                    break;
                case 0x265:
                    com.Name = "265";
                    break;
                case 0x266:
                    com.Name = "266";
                    break;
                case 0x267:
                    com.Name = "PlayCasino";
                    com.parameters.Add(reader.ReadUInt16()); //Casino Game?
                    break;
                case 0x268:
                    com.Name = "StoreHour";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Hour
                    break;
                case 0x269:
                    com.Name = "NPC(269)"; //Not Sure
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //NPC Sprite
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x26A:
                    com.Name = "NPC(26A)";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16()); //X coordinate?
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate?
                    break;
                case 0x26B:
                    com.Name = "CheckRegi";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Have Regi with you?
                    break;
                case 0x26C:
                    com.Name = "StoreHappinessItem";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x26D:
                    com.Name = "UnownMessage";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    break;
                case 0x26E:
                    com.Name = "StorePalParkPokèmonCaught";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Pokèmon Number
                    break;
                case 0x26F:
                    com.Name = "26F";
                    break;
                case 0x270:
                    com.Name = "270";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x271:
                    com.Name = "StartThanksInterviw";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Name
                    break;
                case 0x272:
                    com.Name = "SetVarThanks";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    break;
                case 0x273:
                    com.Name = "SetVarAccPicture";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Picture Id
                    break;
                case 0x275:
                    com.Name = "Casino(275)";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x276:
                    com.Name = "CheckCoinsBoxSpace";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    com.parameters.Add(reader.ReadUInt16()); //0
                    break;
                case 0x277:
                    com.Name = "StoreRandomLevel";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Level
                    break;
                case 0x278:
                    com.Name = "StorePokèmonLevel";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Level
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0x27A:
                    com.Name = "ShowPanoramicView";
                    break;
                case 0x27B:
                    com.Name = "27B";
                    break;
                case 0x27C:
                    com.Name = "SetVarAccessories(27C)";
                    com.parameters.Add(reader.ReadUInt16()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Accessories Id
                    break;
                case 0x27D:
                    com.Name = "StoreTrendyWord";
                    com.parameters.Add(reader.ReadUInt16()); //RV 
                    com.parameters.Add(reader.ReadUInt16()); //Text Var
                    break;
                case 0x27E:
                    com.Name = "CheckFirstTimeVisit(27E)";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x27F:
                    com.Name = "CheckTrendyWord";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x280:
                    com.Name = "SetVarRandomPrize";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x281:
                    com.Name = "StorePokèContestFashion";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16()); //Fashion Type
                    com.parameters.Add(reader.ReadUInt16()); //RV = Fashion Level
                    break;
                case 0x282:
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x283:
                    com.Name = "CallTVReportageRoutine";
                    com.parameters.Add(reader.ReadUInt16()); //Music?
                    com.parameters.Add(reader.ReadUInt16()); //Image?
                    break;
                case 0x284:
                    com.Name = "StoreUnownCaught";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x285:
                    com.Name = "285";
                    com.parameters.Add(reader.ReadUInt16()); //0,1
                    com.parameters.Add(reader.ReadUInt16()); //0,1
                    break;
                case 0x286:
                    com.Name = "StoreUGItemNumber";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x287:
                    com.Name = "StoreUGFoxilNumber";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x288:
                    com.Name = "StoreUGTrapsNumber";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x289:
                    com.Name = "GivePoffinCaseItem";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x28A:
                    com.Name = "CheckPoffinCaseSpace";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x28B:
                    com.Name = "28B";
                    com.parameters.Add(reader.ReadByte()); //0,1,2
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x28C:
                    com.Name = "ChatotFunction(28C)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x28D:
                    com.Name = "SaveChatotExpression";
                    break;
                case 0x28E:
                    com.Name = "PlayChatotExpression";
                    com.parameters.Add(reader.ReadUInt16()); //Id
                    break;
                case 0x28F:
                    com.Name = "StoreFirstTimePokèmonLeague";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x290:
                    com.Name = "ChoosePokèmonDaycare";
                    com.parameters.Add(reader.ReadUInt16()); //Mode?
                    break;
                case 0x291:
                    com.Name = "StoreChosenPokèmonDayCare";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Chosen Pokèmon
                    com.parameters.Add(reader.ReadUInt16()); //RV = Chosen Status
                    break;
                case 0x292:
                    com.Name = "292";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x293:
                    com.Name = "StoreUGPeopleNumber";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x294:
                    com.Name = "StartBPExchange";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x295:
                    com.Name = "DenyExchange";
                    break;
                case 0x296:
                    com.Name = "MakeExchange";
                    break;
                case 0x299:
                    com.Name = "TakeBP";
                    com.parameters.Add(reader.ReadUInt16()); //BP
                    break;
                case 0x29A:
                    com.Name = "CheckBP";
                    com.parameters.Add(reader.ReadUInt16()); //BP
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x29B:
                    com.Name = "StoreDataBPExchange";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Item Type
                    com.parameters.Add(reader.ReadUInt16()); //Prize Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = Prize
                    com.parameters.Add(reader.ReadUInt16()); //RV = PL
                    break;
                case 0x29C:
                    com.Name = "29C";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //WifiSprite
                    break;
                case 0x29D:
                    com.Name = "MultiChoice";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //Script Id
                    break;
                case 0x29E:
                    com.Name = "SetFunctionAnimation";
                    com.parameters.Add(reader.ReadUInt16()); //Function Id
                    com.parameters.Add(reader.ReadUInt16()); //PokèmonId
                    break;
                case 0x29F:
                    com.Name = "BumpCameraAnimation";
                    com.parameters.Add(reader.ReadUInt16()); //Animation Id?
                    break;
                case 0x2A0:
                    com.Name = "DoubleTrainerBattle";
                    com.parameters.Add(reader.ReadUInt16()); //Ally Trainer Id
                    com.parameters.Add(reader.ReadUInt16()); //1st Trainer Id
                    com.parameters.Add(reader.ReadUInt16()); //2nd Trainer Id
                    break;
                case 0x2A2:
                    com.Name = "2A2";
                    com.parameters.Add(reader.ReadUInt16()); //Item Id
                    break;
                case 0x2A3:
                    com.Name = "CheckHaveFriendCode";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Have your Friend Code?
                    break;
                case 0x2A4:
                    com.Name = "CheckHaveFriendRegistered";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x2A5:
                    com.Name = "ShowPokèmonTradeMenu";
                    break;
                case 0x2A6:
                    com.Name = "StoreCasinoPrizeResult";
                    com.parameters.Add(reader.ReadUInt16()); //Prize Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = Item Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = Prize Name
                    break;
                case 0x2A7:
                    com.Name = "CheckSpecialItem";
                    com.parameters.Add(reader.ReadUInt16()); //Item Id
                    com.parameters.Add(reader.ReadUInt16()); //RV 
                    break;
                case 0x2A8:
                    com.Name = "TakeCasinoCoin";
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    break;
                case 0x2A9:
                    com.Name = "CheckCasinoCoin";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    break;
                case 0x2AA:
                    com.Name = "CheckPhraseBoxMisteryGift";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16()); //1st word
                    com.parameters.Add(reader.ReadUInt16()); //2nd word
                    com.parameters.Add(reader.ReadUInt16()); //3rd word
                    com.parameters.Add(reader.ReadUInt16()); //4th word
                    break;
                case 0x2AB:
                    com.Name = "CheckSeal";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x2AC:
                    com.Name = "ActMisteryGift";
                    break;
                case 0x2AD:
                    com.Name = "StoreTrainerInfo?";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Position?
                    com.parameters.Add(reader.ReadUInt16()); //RV = Npc Id
                    break;
                case 0x2AF:
                    com.Name = "2AF";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Message Id
                    break;
                case 0x2B0:
                    com.Name = "ContestFunction(2B0)";
                    break;
                case 0x2B1:
                    com.Name = "ContestFunction(2B1)";
                    break;
                case 0x2B2:
                    com.Name = "StartGTS";
                    break;
                case 0x2B3:
                    com.Name = "SetVarPoffin(2B3)";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Poffin Id
                    break;
                case 0x2B5:
                    com.Name = "2B5";
                    com.parameters.Add(reader.ReadUInt16()); //266,33
                    com.parameters.Add(reader.ReadUInt16()); //762,58
                    com.parameters.Add(reader.ReadUInt16()); //714
                    break;
                case 0x2B6:
                    com.Name = "2B6";
                    com.parameters.Add(reader.ReadByte()); //4
                    com.parameters.Add(reader.ReadUInt16()); //256
                    break;
                case 0x2B7:
                    com.Name = "2B7";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x2B8:
                    com.Name = "SetPokèmonNickname";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x2B9:
                    com.Name = "UnionFunction(2B9)";
                    break;
                case 0x2BA:
                    com.Name = "StoreAlterDecision(2BA)";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x2BB:
                    com.Name = "ContestFunction(2BB)";
                    break;
                case 0x2BC:
                    com.Name = "CheckLegendaryBattle";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x2BD:
                    com.Name = "LegendaryBattle";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16()); //Level
                    break;
                case 0x2BE:
                    com.Name = "StoreStarCardNumber";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x2C0:
                    com.Name = "UnionFunction(2C0)";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    break;
                case 0x2C1:
                    com.Name = "StartSavingGame";
                    break;
                case 0x2C2:
                    com.Name = "EndSavingGame";
                    break;
                case 0x2C4:
                    com.Name = "2C4";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    next1CF = reader.ReadUInt16();
                    if (next1CF > 4000)
                        com.parameters.Add(next1CF);
                    else
                        reader.BaseStream.Position -= 2;
                    break;
                case 0x2C5:
                    if (scriptType == DPSCRIPT)
                    {
                        com.Name = "SetVarItem(2C5)";
                        com.parameters.Add(reader.ReadByte());
                        com.parameters.Add(reader.ReadUInt16());
                    }
                    else
                    {
                        com.Name = "ResetBattleFactory(2C5)";
                        com.parameters.Add(reader.ReadUInt16()); //RV 
                        com.parameters.Add(reader.ReadUInt16()); //RV
                    }
                    break;
                case 0x2C6:
                    com.Name = "StartEggUnion";
                    break;
                case 0x2C7:
                    com.Name = "CheckDiamondPearlUnion";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x2C9:
                    if (scriptType == DPSCRIPT)
                    {
                        com.Name = "SetVarPokèmon(DP)";
                        com.parameters.Add(reader.ReadByte()); //Text Var
                        com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                        com.parameters.Add(reader.ReadUInt16());
                        com.parameters.Add(reader.ReadByte());
                    }
                    else
                    {
                        com.Name = "EternaGym(P)";
                    }
                    break;
                case 0x2CA:
                    com.Name = "SetVarStarterAlter";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    break;
                case 0x2CB:
                    if (scriptType == DPSCRIPT)
                    {
                        com.Name = "SetVarStarterAccessories(DP)";
                        com.parameters.Add(reader.ReadByte()); //Text Var
                        com.parameters.Add(reader.ReadUInt16()); //Accessories Id
                    }
                    else
                    {
                        com.Name = "CheckPokèmonPartyMultiple(P)";
                        com.parameters.Add(reader.ReadUInt16()); //RV = Have at least 2 Pokèmon identical?
                        com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    }
                    break;
                case 0x2CC:
                    if (scriptType == DPSCRIPT)
                    {
                        com.Name = "SetVarWifiSprite(DP)";
                        com.parameters.Add(reader.ReadByte()); //Text Var
                        com.parameters.Add(reader.ReadUInt16()); //Wifi Sprite
                    }
                    else
                    {
                        com.Name = "CheckBattleHallFunction";
                        com.parameters.Add(reader.ReadUInt16());
                        com.parameters.Add(reader.ReadUInt16()); 
                        com.parameters.Add(reader.ReadUInt16()); //RV;
                    }
                    break;
                case 0x2CD:
                    if (scriptType == DPSCRIPT)
                    {
                        com.Name = "SetVarPoffin(2CD)";
                        com.parameters.Add(reader.ReadByte()); //Text Var
                        com.parameters.Add(reader.ReadUInt16()); //Poffin Id
                    }
                    else
                    {
                        com.Name = "GalaxyFunction(2CD)";
                    }
                    break;
                case 0x2CE:
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x2CF:
                    if (scriptType == DPSCRIPT)
                    {
                        com.Name = "ExternalMultiText(DP)";
                        com.parameters.Add(reader.ReadByte()); //Active/Not
                    }
                    else
                    {
                        com.Name = "CheckBattleHallCondition";
                        com.parameters.Add(reader.ReadUInt16());
                        com.parameters.Add(reader.ReadUInt16()); //RV = T/F
                    }
                    break;
                case 0x2D0:
                    com.Name = "StoreBattleHallChosenPokèmon";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Chosen Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2D1:
                    if (scriptType == DPSCRIPT)
                    {
                        com.Name = "LiftAnimation";
                        com.parameters.Add(reader.ReadUInt16()); //Animation
                    }
                    else
                    {
                        com.Name = "ResetBattleHall";
                        com.parameters.Add(reader.ReadUInt16()); //RV
                    }
                    break;
                case 0x2D2:
                    com.Name = "CallBattleCastleFunction(P)";
                    com.parameters.Add(reader.ReadUInt16()); 
                    com.parameters.Add(reader.ReadUInt16()); 
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x2D3:
                    com.Name = "CheckBattleCastleCondition(P)";
                    com.parameters.Add(reader.ReadUInt16()); //Specie 1
                    com.parameters.Add(reader.ReadUInt16()); //Specie 2
                    com.parameters.Add(reader.ReadUInt16()); //Rv
                    break;
                case 0x2D4:
                    com.Name = "StoreBattleCastleChosenPokèmon(P)";
                    com.parameters.Add(reader.ReadUInt16()); //RV = 1st Pokèmon Party Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = 2nd Pokèmon Party Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = 3rd Pokèmon Party Id
                    break;
                case 0x2D5:
                    com.Name = "ResetBattleCastle(P)";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x2D6:
                    com.Name = "2D6(P)";
                    break;
                case 0x2D7:
                    com.Name = "CheckLotOfDataSaving(P)";
                    com.parameters.Add(reader.ReadUInt16()); //RV;
                     break;
                case 0x2D8:
                    com.Name = "ShowPrizeExchange(P)";
                    com.parameters.Add(reader.ReadByte()); //List Id
                    break;
                case 0x2D9:
                    com.Name = "CallBattleArcadeFunction(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x2DA:
                    com.Name = "CheckBattleArcadeCondition(P)";
                    com.parameters.Add(reader.ReadUInt16()); //Specie 1
                    com.parameters.Add(reader.ReadUInt16()); //Specie 2
                    com.parameters.Add(reader.ReadUInt16()); //Rv
                    break;
                case 0x2DB:
                    com.Name = "StoreBattleArcadeChosenPokèmon(P)";
                    com.parameters.Add(reader.ReadUInt16()); //RV = 1st Pokèmon Party Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = 2nd Pokèmon Party Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = 3rd Pokèmon Party Id
                    break;
                case 0x2DC:
                    com.Name = "ResetBattleArcade(P)";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x2DD:
                    com.Name = "StorePokèmonSpecie(P)";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0x2DF:
                    com.Name = "StoreAmitySquareData(P)";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x2E0:
                    com.Name = "CheckAmitySquare(P)";
                    com.parameters.Add(reader.ReadUInt16()); //0x40AB
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x2E1:
                    com.Name = "StoreAmitySquareItem(P)";
                    com.parameters.Add(reader.ReadUInt16()); //0x40AB
                    com.parameters.Add(reader.ReadUInt16()); //RV = Item Id
                    break;
                case 0x2E2:
                    com.Name = "2E2(P)";
                    break;
                case 0x2E5:
                    com.Name = "CheckShardsMovePokèmonCompatible(P)";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Specie
                    com.parameters.Add(reader.ReadUInt16()); //Move Type?
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x2E6:
                    com.Name = "ChooseShardsMoveLearn(P)";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16()); //Move Type?
                    com.parameters.Add(reader.ReadUInt16()); //RV = Chosen Status
                    break;
                case 0x2E7:
                    com.Name = "ChooseShardsMoveForgot(P)";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = Move to forgot
                    break;
                case 0x2E8:
                    com.Name = "StorePokèmonMoveNumber(P)";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x2E9:
                    com.Name = "TeachShardsMove(P)";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon
                    com.parameters.Add(reader.ReadUInt16()); //Move Slot
                    com.parameters.Add(reader.ReadUInt16()); //Move Id
                    break;
                case 0x2EA:
                    com.Name = "CheckEnoughShards(P)";
                    com.parameters.Add(reader.ReadUInt16()); //Move Id
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x2EB:
                    com.Name = "2EB(P)";
                    com.parameters.Add(reader.ReadUInt16()); //Move
                    break;
                case 0x2EC:
                    com.Name = "ShowShardsBox(P)"; //Not sure
                    com.parameters.Add(reader.ReadUInt16()); //X Coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Move Id
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x2ED:
                    com.Name = "HideShardsBox(P)"; //Not sure
                    break;
                case 0x2EE:
                    com.Name = "StorePokèmonPotential(P)";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = IV Sum
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = Highest IV Value
                    break;
                case 0x2F0:
                    com.Name = "StartFurnitureHouse(P)";
                    break;
                case 0x2F2:
                    com.Name = "StartDistortionWorld(P)";
                    break;
                case 0x2F3:
                    com.Name = "SetVarTrainer(2F3)";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Trainer Id
                    break;
                case 0x2F4:
                    com.Name = "StoreBattleHouseTrainer(P)";
                    com.parameters.Add(reader.ReadUInt16()); //1st Trainer Id
                    com.parameters.Add(reader.ReadUInt16()); //2nd Trainer Id
                    com.parameters.Add(reader.ReadUInt16()); //3rd Trainer Id
                    com.parameters.Add(reader.ReadUInt16()); //4th Trainer Id
                    break;
                case 0x2F5:
                    com.Name = "ShowFurnitureBought(P)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16()); //Prize
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2F6:
                    com.Name = "2F6(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 759:
                    com.Name = "2F7(P)";
                    com.parameters.Add(reader.ReadUInt16()); //WFC value
                    break;
                case 760:
                    com.Name = "2F8(P)";
                    break;
                case 761:
                    com.Name = "2F9(P)";
                    com.parameters.Add(reader.ReadUInt16()); //2FA value
                    break;
                case 762:
                    com.Name = "2FA(P)";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 763:
                    com.Name = "2FB(P)";
                    break;
                case 764:
                    com.Name = "2FC(P)";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x2FD:
                    com.Name = "SetVarPokèmonHiddenPower(P)";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Hidden Power Type
                    break;
                case 0x2FE:
                    com.Name = "StoreItemNumber(2FE)";
                    com.parameters.Add(reader.ReadUInt16()); //Item Id
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x2FF:
                    com.Name = "StorePokèmoniddenPower(P)";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x300:
                    com.Name = "300(P)";
                    break;
                case 0x302:
                    com.Name = "302(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x303:
                    com.Name = "303(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x304:
                    com.Name = "304(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x305:
                    com.Name = "305(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 774:
                    com.Name = "306(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x307:
                    com.Name = "307(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x308:
                    com.Name = "308(P)";
                    break;
                case 0x309:
                    com.Name = "309(P)";
                    break;
                case 0x30A:
                    com.Name = "30A(P)";
                    break;
                case 0x30B:
                    com.Name = "JumpToPc(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x30C:
                    com.Name = "30C(P)";
                    break;
                case 781:
                    com.Name = "30D(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x30E:
                    com.Name = "30E(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x30F:
                    com.Name = "30F(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x310:
                    com.Name = "310(P)";
                    break;
                case 0x311:
                    com.Name = "311(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x312:
                    com.Name = "312(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x313:
                    com.Name = "SetBattleParkStatus(P)";
                    com.parameters.Add(reader.ReadUInt16()); //T/F
                    break;
                case 0x314:
                    com.Name = "314(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x315:
                    com.Name = "315(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x316:
                    com.Name = "316(P)";
                    break;
                case 0x317:
                    com.Name = "317(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x318:
                    com.Name = "318(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x319:
                    com.Name = "319(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x31A:
                    com.Name = "31A(P)";
                    break;
                case 795:
                    com.Name = "31B(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 796:
                    com.Name = "31C(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x31D:
                    com.Name = "31D(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x31E:
                    com.Name = "StorePokèmonId(31E)";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Party Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = Pokèmon Id
                    break;
                case 799:
                    com.Name = "31F(P)";
                    break;
                case 0x320:
                    com.Name = "320(P)";
                    break;
                case 0x321:
                    com.Name = "321(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x322:
                    com.Name = "322(P)";
                    break;
                case 0x323:
                    com.Name = "323(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 804:
                    com.Name = "324(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x325:
                    com.Name = "325(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x326:
                    com.Name = "326(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x327:
                    com.Name = "327(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x328:
                    com.Name = "328(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x329:
                    com.Name = "329(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x32A:
                    com.Name = "32A(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x32B:
                    com.Name = "32B(P)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x32C:
                    com.Name = "32C(P)";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //X
                    com.parameters.Add(reader.ReadUInt16()); //Y
                    break;
                case 0x32D:
                    com.Name = "32D(P)";
                    break;
                case 0x32E:
                    com.Name = "32E(P)";
                    break;
                case 0x32F:
                    com.Name = "32F(P)";
                    com.parameters.Add(reader.ReadUInt16()); //Item
                    com.parameters.Add(reader.ReadUInt16()); //RV = T/F
                    break;
                case 0x330:
                    com.Name = "330(P)";
                    break;
                case 0x331:
                    com.Name = "331(P)";
                    break;
                case 0x332:
                    com.Name = "332(P)";
                    break;
                case 0x334:
                    com.Name = "334(P)";
                    com.parameters.Add(reader.ReadUInt16()); //#35 - Unknown
                    com.parameters.Add(reader.ReadUInt16()); //Money Amount
                    break;
                case 0x335:
                    com.Name = "335(P)";
                    com.parameters.Add(reader.ReadUInt16()); //#35 - Unknown
                    com.parameters.Add(reader.ReadUInt16()); //Money Amount
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x336:
                    com.Name = "CheckHallofFameSaveStatus(P)";
                    com.parameters.Add(reader.ReadUInt16()); //RV = T(OK)/F(Corrupted)
                    break;
                case 0x337:
                    com.Name = "337(P)";
                    break;
                case 0x338:
                    com.Name = "338(P)";
                    break;
                case 0x339:
                    com.Name = "339(P)";
                    break;
                case 0x33A:
                    com.Name = "SetExternalTextMulti";
                    com.parameters.Add(reader.ReadByte()); //#1 - Unknown
                    break;
                case 0x33C:
                    com.Name = "SetVarColorItem(P)";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Item Id
                    break;
                case 0x33D:
                    com.Name = "SetVarColorItem2(P)";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Item Id
                    break;
                case 0x33E:
                    com.Name = "SetVarUGItem(P)";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Underground Id
                    break;
                case 0x341:
                    com.Name = "SetVarPokèmon(P341)";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x343:
                    com.Name = "SetVarStarterAccessories(P)";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Accessories Id
                    break;
                case 0x344:
                    com.Name = "SetVarWifiSprite(P)";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Category Id
                    break;
                case 0x345:
                    com.Name = "SetVarPoffin(P)";
                    com.parameters.Add(reader.ReadByte()); //Var Id
                    com.parameters.Add(reader.ReadUInt16()); //Poffin Id
                    break;
                case 838:
                    com.Name = "346(P)";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x347:
                    com.Name = "ShowFloor";
                    com.parameters.Add(reader.ReadUInt16()); //Floor Id
                    break;



                default:
                    com.Name = "0x" + com.Id;
                    break;
            }
        }

        private void readCommandHGSS(BinaryReader reader, ref Commands_s com)
        {
            uint functionOffset = 0;
            switch (com.Id)
            {
                case 0x2:
                    com.Name = "End";
                    com.isEnd = 1;
                    functionOffset = (uint)reader.BaseStream.Position;
                    checkNextFunction(reader, functionOffset);
                    break;
                case 0x3:
                    com.Name = "Return";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x11:
                    com.Name = "Compare";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x12:
                    com.Name = "Compare2";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x14:
                    com.Name = "CallStd";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    break;

                case 0x15:
                    com.Name = "ExitStd";
                    break;
                case 0x16:
                    com.Name = "Jump";
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[0];
                    checkNextFunction(reader, functionOffset);
                    com.numJump++;
                    var isEndC = reader.ReadUInt16();
                    if (isEndC != 2)
                    {
                        com.isEnd = 1;
                        checkNextFunction(reader, (uint)reader.BaseStream.Position - 2);
                    }
                    reader.BaseStream.Position -= 2;
                    break;
                case 0x1A:
                    com.Name = "Goto";
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[0];
                    if (!functionOffsetList.Contains(functionOffset))
                    {
                        functionOffsetList.Add(functionOffset);
                        Console.AppendText("\nA function is in: " + functionOffset.ToString());
                    }
                    com.numJump++;
                    //isEndC = reader.ReadUInt16();
                    //if (isEndC != 2)
                    //    com.isEnd = 1;
                    //else
                    //    Console.AppendText("\nFind End after " + com.Name);
                    //reader.BaseStream.Position -= 2;
                    break;

                case 0x1B:
                    com.Name = "KillScript";
                    com.isEnd = 1;
                    functionOffset = (uint)reader.BaseStream.Position;
                    checkNextFunction(reader, functionOffset);
                    break;

                case 0x1C:
                    com.Name = "If";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[1];
                    checkNextFunction(reader, functionOffset);
                    break;
                case 0x1D:
                    com.Name = "If2";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[1];
                    checkNextFunction(reader, functionOffset);
                    break;

                case 0x1E:
                    com.Name = "SetFlag";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x1F:
                    com.Name = "ClearFlag";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x20:
                    com.Name = "StoreFlag";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x21:
                    com.Name = "SetVar(21)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22:
                    com.Name = "SetVar(22)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x23:
                    com.Name = "StoreVar(23)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24:
                    com.Name = "SetVar(24)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x25:
                    com.Name = "SetVar(25)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x26:
                    com.Name = "SetVar(26)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x27:
                    com.Name = "StoreVar(27)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x28:
                    com.Name = "StoreVar(28)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x29:
                    com.Name = "StoreVarValue(29)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2A:
                    com.Name = "StoreVar(2A)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x2B:
                    com.Name = "StoreVar(2B)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x2c:
                    com.Name = "Message(2C)";
                    com.parameters.Add(reader.ReadByte());
                    break;

                case 0x2D:
                    com.Name = "Message";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x2e:
                    com.Name = "Message(2E)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x2f:
                    com.Name = "Message(2F)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x30:
                    com.Name = "Message(30)";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x31:
                    break;
                case 0x32:
                    com.Name = "WaitButton";
                    break;
                case 0x33:
                    com.Name = "33";
                    break;
                case 0x35:
                    com.Name = "CloseMessageKeyPress";
                    break;
                case 0x36:
                    com.Name = "FreezeMessageBox";
                    break;
                case 0x37:
                    com.Name = "CallMessageBox";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x38:
                    com.Name = "ColorMessageBox";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x39:
                    com.Name = "TypeMessageBox";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x3A:
                    com.Name = "NoMapMessageBox";
                    break;
                case 0x3B:
                    com.Name = "CallTextMessageBox";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x3C:
                    com.Name = "CheckMenuStatus";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x3D:
                    com.Name = "ShowMenu";
                    break;
                case 0x3F:
                    com.Name = "YesNoBox";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x40:
                    com.Name = "Multi(40)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x41:
                    com.Name = "Multi(41)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x42:
                    com.Name = "SetTextScriptMulti";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x43:
                    com.Name = "CloseMulti(43)";
                    break;
                case 0x46:
                    com.Name = "CloseMulti(46)";
                    break;
                case 0x47:
                    com.Name = "CloseMulti(47)";
                    break;
                case 0x49:
                    com.Name = "Fanfare(49)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x4A:
                    com.Name = "Fanfare(4A)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x4B:
                    com.Name = "WaitFanfare";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x4C:
                    com.Name = "Cry";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x4D:
                    com.Name = "WaitCry";
                    break;
                case 0x4E:
                    com.Name = "PlaySound";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x4F:
                    com.Name = "FadeDef";
                    break;
                case 0x50:
                    com.Name = "PlaySound(50)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x51:
                    com.Name = "StopSound";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x52:
                    com.Name = "RestartSound";
                    break;
                case 0x53:
                    com.Name = "53";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x54:
                    com.Name = "SwitchSound";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x55:
                    com.Name = "StoreSoundMicrophone";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x57:
                    com.Name = "PlaySound(57)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                //case 0x58:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                case 0x59:
                    com.Name = "CheckSoundMicrophone";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x5A:
                    com.Name = "SwitchSound(5A)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x5B:
                    com.Name = "ActivateMicrophone";
                    break;
                case 0x5C:
                    com.Name = "DisactivateMicrophone";
                    break;
                case 0x5E:
                    com.Name = "ApplyMovement";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    var movOffset = com.parameters[1];
                    if (!movOffsetList.Contains(movOffset))
                    {
                        movOffsetList.Add(movOffset);
                        Console.AppendText("\nA movement is in: " + movOffset.ToString());
                    }
                    break;
                case 0x5F:
                    com.Name = "WaitMovement";
                    break;
                case 0x60:
                    com.Name = "LockAll";
                    break;
                case 0x61:
                    com.Name = "ReleaseAll";
                    break;
                case 0x62:
                    com.Name = "Lock";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x63:
                    com.Name = "Release";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x64:
                    com.Name = "AddPeople";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x65:
                    com.Name = "RemovePeople";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x66:
                    com.Name = "MoveCam";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x67:
                    com.Name = "ZoomCam";
                    break;
                case 0x68:
                    com.Name = "FacePlayer";
                    break;
                case 0x69:
                    com.Name = "StoreHeroPosition";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x6A:
                    com.Name = "CheckOWPosition";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                //case 0x6b:
                //    break;

                //case 0x6c:
                //    com.Name = "ContinueFollow";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                case 0x6d:
                    com.Name = "FollowHero";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x6E:
                    com.Name = "GiveMoney";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x6F:
                    com.Name = "TakeMoney";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x70:
                    com.Name = "CheckMoney";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x71:
                    com.Name = "ShowMoney";
                    break;
                case 0x72:
                    com.Name = "HideMoney";
                    break;
                case 0x73:
                    com.Name = "UpdateMoney";
                    break;
                case 0x74:
                    com.Name = "ShowCoins";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x75:
                    com.Name = "HideCoins";
                    break;
                case 0x76:
                    com.Name = "UpdateCoins";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x77:
                    com.Name = "CheckCoins";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x78:
                    com.Name = "GiveCoins";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x79:
                    com.Name = "TakeCoins";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x7b:
                    com.Name = "7B";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x7c:
                    com.Name = "7C";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x7D:
                    com.Name = "GiveItem";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x7E:
                    com.Name = "TakeItem";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x7F:
                    com.Name = "CheckItemBagSpace";
                    com.parameters.Add(reader.ReadUInt16()); //Item Id
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    com.parameters.Add(reader.ReadUInt16()); //RV = Item Bag Number
                    break;
                case 0x80:
                    com.Name = "CheckItemBagNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x81:
                    com.Name = "StoreItemTaken";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x82:
                    com.Name = "StoreItemType";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x83:
                    com.Name = "SendItemType";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x84:
                    com.Name = "DoubleMessage";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x85:
                    com.Name = "85";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x86:
                    com.Name = "86";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x87:
                    com.Name = "StorePokèmonParty(87)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x88:
                    com.Name = "StorePokèmonParty(88)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x89:
                    com.Name = "GivePokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x8B:
                    com.Name = "GiveEgg";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x8E:
                    com.Name = "8E";
                    break;
                case 0x8f:
                    com.Name = "8F";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x90:
                    com.Name = "90";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x91:
                    com.Name = "ActPokèGearApplication";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x92:
                    com.Name = "RegisterPokèGearNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x93:
                    com.Name = "93";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x94:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x95:
                    com.Name = "95";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x96:
                    com.Name = "CallEnd";
                    break;
                case 0x97:
                    break;

                //case 0x99:
                //    com.Name = "CheckMove";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x9A:
                //    com.Name = "StartDressPokèmon";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x9B:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x9D:
                    com.Name = "ShowMap";
                    break;
                //case 0xA1:
                //    com.Name = "CallEnd";
                //    break;

                //case 0xA3:
                //    com.Name = "StartWFC";
                //    break;

                //case 0xA5:
                //    com.Name = "StartInterview";
                //    break;

                case 0xA6:
                    com.Name = "A6";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                //case 0xA7:
                //    com.Name = "DisplayDressedPokèmon";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0xA8:
                //    com.Name = "DisplayContestPokè9mon";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0xA9:
                //    com.Name = "OpenBallCapsule";
                //    break;

                //case 0xAA:
                //    com.Name = "OpenSinnohMaps";
                //    break;

                //case 0xAB:
                //    com.Name = "OpenPc";
                //    com.parameters.Add(reader.ReadByte());
                //    break;

                //case 0xAC:
                //    com.Name = "StartDrawingUnion";
                //    break;

                case 0xAD:
                    com.Name = "ChoosePokèmonName";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xAE:
                    com.Name = "FadeScreen";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0xAF:
                    com.Name = "ResetScreen";
                    break;

                case 0xB0:
                    com.Name = "Warp";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                //case 0xB1:
                //    com.Name = "ShowHallOfFame";
                //    break;

                //case 0xB2:
                //    com.Name = "CheckWFC";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0xB3:
                //    com.Name = "StartWFC2";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0xB4:
                //    com.Name = "ChooseStarter";
                //    break;

                //case 0xB5:
                //    com.Name = "BattleStarter";
                //    break;

                //case 0xB6:
                //    com.Name = "CheckBattleID?";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0xB7:
                //    com.Name = "SetVarBattle?";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                case 0xB8:
                    com.Name = "CheckBike";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0xB9:
                    com.Name = "RideBike";
                    com.parameters.Add(reader.ReadByte());
                    break;

                //case 0xBA:
                //    com.Name = "ChoosePlayerName";
                //    break;

                case 0xBB:
                    com.Name = "BB";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0xBC:
                    com.Name = "StartHeroAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0xBD:
                    com.Name = "StopHeroAnimation";
                    break;
                case 0xBE:
                    com.Name = "SetVarHero";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0xBF:
                    com.Name = "SetVarAlter";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0xC0:
                    com.Name = "SetVarHero(C0)";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0xC1:
                    com.Name = "SetVarPokèmon";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC2:
                    com.Name = "SetVarItem";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC3:
                    com.Name = "SetVarItemType";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC4:
                    com.Name = "C4";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC5:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC6:
                    com.Name = "SetVarNumber";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC7:
                    com.Name = "SetVarPokèmon(C7)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0xC8:
                //    com.Name = "RideBike";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0xC9:
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0xCB:
                //    com.Name = "BerryHeroAnimation";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 204:
                    com.Name = "SetVarAccessories";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0xCD:
                //    com.Name = "SetVarHero";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                case 0xCE:
                    com.Name = "StoreStarter";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0xCF:
                //    com.Name = "SetVarAlter";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0xD0:
                //    com.Name = "SetVarPokémon";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0xD1:
                //    com.Name = "SetVarItem";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0xD2:
                //    com.Name = "SetVarItem2";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0xD3:
                //    com.Name = "SetVarBattleItem";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0xD4:
                    com.Name = "D4";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD5:
                    com.Name = "TrainerBattle";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0xD6:
                //    com.Name = "SetVarNickPokémon";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0xD7:
                //    com.Name = "SetVarKeyItem";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0xD8:
                //    com.Name = "SetVarTrainer";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0xD9:
                    com.Name = "D9";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0xDA:
                //    com.Name = "SetVarPokèmonCheckd";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                case 0xDB:
                    com.Name = "TeleportPC";
                    break; ;
                case 0xDC:
                    com.Name = "StoreBattleResult";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xDE:
                    com.Name = "StoreBattleResult(DE)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE2:
                    com.Name = "E2";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0xE3:
                //    com.Name = "CheckSwarmInfo";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0xE4:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0xE5:
                //    com.Name = "TrainerBattle";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0xE6:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0xE7:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0xE8:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0xE9:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0xEA:
                //    com.Name = "ActTrainer";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0xEB:
                //    com.Name = "TeleportPC";
                //    break;
                //case 0xEC:
                //    com.Name = "CheckBattleResult";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0xEE:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF0:
                    com.Name = "F0";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF1:
                    com.Name = "F1";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF2:
                    com.Name = "F2";
                    break;
                //case 0xF3:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0xFD:
                    com.Name = "StoreStatusSave";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xFE:
                    com.Name = "CheckErrorSave";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x100:
                //    com.Name = "SetDressTitle";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x101:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x108:
                    com.Name = "108";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x10C:
                    com.Name = "10C";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x10D:
                    com.Name = "10D";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x112:
                    com.Name = "112";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x113:
                    com.Name = "113";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x114:
                    com.Name = "SetFloorStatus";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x117:
                    com.Name = "117";
                    break;
                case 0x119:
                    com.Name = "119";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x11A:
                    com.Name = "HealPokèmon";
                    break;
                //case 0x11B:
                //    com.Name = "WarpMapLift";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x11C:
                //    com.Name = "CheckFloor";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x11D:
                //    com.Name = "StartLift";
                //    break;
                //case 0x11E:
                //    com.Name = "CheckPokèmonCaught";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x124:
                //    com.Name = "WildPokèmonBattle";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x126:
                    com.Name = "CheckBadge";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x127:
                //    com.Name = "SetBadge";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x12E:
                //    com.Name = "CheckPhotoImage";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x12F:
                //    com.Name = "CheckContestImage";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x130:
                //    com.Name = "CheckPhotoName";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x131:
                //    com.Name = "ActPokèKron";
                //    break;
                //case 0x132:
                //    com.Name = "CheckPokèKronStatus";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x133:
                    com.Name = "PrepareDoorAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x134:
                    com.Name = "WaitDoorOpeningAnimation";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x135:
                    com.Name = "WaitDoorClosingAnimation";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x136:
                    com.Name = "DoorOpeningAnimation";
                    com.parameters.Add(reader.ReadByte()); ;
                    break;
                case 0x137:
                    com.Name = "DoorClosingAnimation";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x138:
                    com.Name = "138";
                    break;
                case 0x139:
                    com.Name = "139";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x13C:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x13F:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x140:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x141:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x142:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x143:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x144:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x145:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x146:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x147:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x148:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x14C:
                    com.Name = "StorePokèmonPartyNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x14D:
                //    com.Name = "CheckGender";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x14E:
                //    com.Name = "HealPokèmon";
                //    break;
                //case 0x152:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x153:
                    com.Name = "SetOWPosition";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x154:
                //    com.Name = "StartChooseWifiSprite";
                //    break;
                //case 0x155:
                //    com.Name = "ChooseWifiSprite";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x156:
                    com.Name = "156";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x158:
                //    com.Name = "ActSinnohPokèdex";
                //    break;
                case 0x159:
                    com.Name = "StartErrorCheckSaving";
                    break;
                case 0x15A:
                    com.Name = "EndErrorCheckSaving";
                    break;
                case 0x15B:
                    com.Name = "UpdateSaveStatus";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x15C:
                    com.Name = "SetBadge";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x15D:
                    com.Name = "ShowChoosePokèmonMenu";
                    break;
                case 0x15F:
                    com.Name = "StoreChosenPokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x160:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x161:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x162:
                    com.Name = "StorePokèmonType";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x163:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x164:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x165:
                    com.Name = "StorePokèmonPartyNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x166:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x168:
                    com.Name = "168";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x169:
                    com.Name = "169";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x16A:
                //    com.Name = "WaitDoorClosingAnimation";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0x16B:
                //    com.Name = "DoorOpeningAnimation";
                //    com.parameters.Add(reader.ReadByte()); ;
                //    break;
                //case 0x16C:
                //    com.Name = "DoorClosingAnimation";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                case 0x16D:
                    com.Name = "16D";
                    break;
                //case 0x16E:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x16F:
                    com.Name = "16F";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x170:
                    com.Name = "170";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x171:
                    com.Name = "EggAnimation";
                    break;
                case 0x173:
                    com.Name = "173";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x174:
                    com.Name = "174";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x175:
                    com.Name = "175";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x176:
                    com.Name = "176";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x177:
                    com.Name = "177";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x179:
                    com.Name = "179";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x17B:
                    com.Name = "StoreTime";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x17C:
                //    com.Name = "17C";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x17D:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x17E:
                    com.Name = "StorePokèmonHappiness";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x17F:
                //    com.Name = "17F";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x180:
                //    com.Name = "180";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                case 0x181:
                    com.Name = "181";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x182:
                    com.Name = "StoreHeroFaceOrientation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x183:
                    com.Name = "183";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                //case 0x184:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x186:
                //    com.Name = "ChangeOwPosition";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x187:
                //    com.Name = "SetOwPosition";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x188:
                //    com.Name = "ChangeOwPosition2";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x189:
                //    com.Name = "ReleaseOw";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x18A:
                //    com.Name = "SetTilePassable";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x18B:
                //    com.Name = "SetTileLocked";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x18C:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x18F:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x190:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x191:
                //    com.Name = "ShowChoosePokèmonMenu";
                //    break;
                case 0x193:
                    com.Name = "GiveAccessories";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x194:
                //    com.Name = "CheckAccessories";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x195:
                //    com.Name = "GiveAccPicture";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x196:
                    com.Name = "GiveAccPicture";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x197:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x198:
                //    com.Name = "CheckPokèmonId";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x19A:
                //    com.Name = "CheckPokèmonPartyNumber";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x19B:
                //    com.Name = "CheckPokèmonPartyNumber2";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x19C:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x19E:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x19F:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1A0:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x1AE:
                    com.Name = "1AE";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1AF:
                    com.Name = "1AF";
                    break;
                //case 0x1B0:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1B1:
                //    com.Name = "0x1B1";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1B2:
                //    com.Name = "0x1B2";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x1B3:
                    com.Name = "CheckPosoined";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 436:
                    com.Name = "HealPosoined";
                    break;
                case 0x1B5:
                    com.Name = "1B5"; //1F9 in Platinum
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1B6:
                    com.Name = "1B6";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x1B7:
                //    com.Name = "CheckPlayerId";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x1B8:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x1B9:
                //    com.Name = "CheckPokèmonHappiness";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1BA:
                //    com.Name = "SetPokèmonHappiness";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1BC:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1BD:
                //    com.Name = "CheckHeroFaceOrientation";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x1BE:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1BF:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x1C0:
                //    com.Name = "CheckSpecificPokèmonParty";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x1C1:
                    com.Name = "1C1";
                    break;

                //case 0x1C6:
                //    com.Name = "CheckMoveDeleter";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1C7:
                //    com.Name = "CheckPokèmonDeleter";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1C8:
                //    com.Name = "CheckPokèmonMoveNumber";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1C9:
                //    com.Name = "DeletePokèmonMove";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1CB:
                //    com.Name = "SetVarMoveDeleter";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1CC:
                //    com.Name = "ActJournal";
                //    break;
                //case 0x1CD:
                //    com.Name = "DeActivateLeader";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1CF:
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1D0:
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0x1D1:
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1D2:
                //    com.Name = "GiveAccessories";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1D3:
                //    com.Name = "CheckAccessories";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1D5:
                //    com.Name = "GiveAccessories2";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x1D6:
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x1D7:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1D8:
                    com.Name = "CheckChosenPokèmonTrade(1D8)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1D9:
                    com.Name = "TradePokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1DA:
                    com.Name = "EndTrade";
                    break;
                case 0x1DF:
                    com.Name = "1DF";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E0:
                    com.Name = "1E0";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E1:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E4:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x1E5:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x1E7:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x1E8:
                //    com.Name = "CheckSinnohPokèdexCompleteStatus";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1E9:
                //    com.Name = "CheckNationalPokèdexCompleteStatus";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x1EA:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x1EB:
                //    com.Name = "GiveNationalDiploma";
                //    break;
                case 0x1EC:
                    com.Name = "CheckSinglePhraseBoxInput";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                //case 0x1ED:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x1EE:
                    com.Name = "SetVarPhraseBoxInput";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1EF:
                    com.Name = "StoreVersion";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x1F1:
                //    com.Name = "CheckFossilNumber";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x1F4:
                    com.Name = "PcAnimation";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x1F5:
                    com.Name = "PcAnimation(1F5)sE";
                    com.parameters.Add(reader.ReadByte());
                    break;
                //case 0x1F6:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1F7:
                //    com.Name = "1F7";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1F8:
                //    com.Name = "1F8";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1F9:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1FA:
                //    com.Name = "1FA";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1FB:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x1FC:
                    com.Name = "1FC";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x200:
                //    //com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x201:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x202:
                //    com.Name = "SetSafariGame";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0x203:
                //    com.Name = "203";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x208:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x20A:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x20B:
                //    com.Name = "20B";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x20F:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x210:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x211:
                    com.Name = "StoreFirstPokèmonParty";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x213:
                    com.Name = "SetVarAccPicture";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x214:
                //    com.Name = "CheckUGFriendNumber";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x218:
                //    com.Name = "GenerateRandomPokèmonSearch";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x219:
                //    com.Name = "ActRandomPokèmonSearch";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x21A:
                //    com.Name = "CheckRandomPokèmonSearch";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x21D:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x21F:
                //    com.Name = "CheckMoveUsableTeacher";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x221:
                //    com.Name = "CheckMoveTeacher";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x223:
                //    com.Name = "CheckPokèmonMoveTeacher";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x224:
                //    com.Name = "TeachPokèmonMove";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x225:
                //    com.Name = "CheckTeachingResult";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x226:
                //    com.Name = "SetTradeId";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                case 0x227:
                    com.Name = "227";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x228:
                    com.Name = "228";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x229:
                //    com.Name = "TradePokèmon";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x22A:
                //    com.Name = "EndTrade";
                //    break;
                case 0x22C:
                    com.Name = "22C";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22D:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x22F:
                //    com.Name = "CheckRibbonNumber";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x230:
                //    com.Name = "CheckRibbonPokèmon";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x231:
                    com.Name = "231";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x232:
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x234:
                //    com.Name = "CheckHeroFriendCode";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x235:
                //    com.Name = "CheckOtherFriendCode";
                //    com.parameters.Add(reader.ReadUInt16());
                //    var next = reader.ReadUInt16();
                //    while (next > 3000 || next < 10)
                //    {
                //        com.parameters.Add(next);
                //        next = reader.ReadUInt16();
                //    }

                //    if (next < 3000 && next > 10)
                //        reader.BaseStream.Position -= 2;

                //    break;
                case 0x236:
                    com.Name = "ShowPokèmonTradeMenu";
                    break;
                //case 0x237:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x238:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x23A:
                //    com.Name = "CheckFootPokèmon";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x23C:
                //    com.Name = "CheckLiftDirection";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x23D:
                //    com.Name = "ShowShipAnimation";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x23E:
                //    com.parameters.Add(reader.ReadUInt16());
                //    next = reader.ReadUInt16();
                //    while (next > 3000 || next < 2)
                //    {
                //        com.parameters.Add(next);
                //        next = reader.ReadUInt16();
                //    }

                //    if (next < 3000 && next > 2)
                //        reader.BaseStream.Position -= 2;
                //    break;
                //case 0x243:
                //    com.Name = "CheckSinglePhraseBoxInput";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x244:
                //    com.Name = "CheckDoublePhraseBoxInput";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x245:
                //    com.Name = "SetVarPhraseBoxInput";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x246:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x247:
                //    com.Name = "CheckFirstPokèmonParty";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x248:
                    com.Name = "248";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x249:
                //    com.Name = "CheckDoublePhraseBoxInput2";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x24B:
                    com.Name = "24B";
                    break;
                case 0x24D:
                    com.Name = "WildPokèmonBattle";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24E:
                    com.Name = "StoreStarCardNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x24F:
                //    com.Name = "ComparePokèLottoNumber";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x251:
                    com.Name = "StartSavingGame";
                    break;
                case 0x252:
                    com.Name = "EndSavingGame";
                    break;
                case 0x254:
                    com.Name = "254";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x255:
                //    com.Name = "SavePCPalParkPokèmon";
                //    break;
                //case 0x256:
                //    com.Name = "CheckPalParkPoint";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 599:
                    com.Name = "257";
                    break;
                case 0x25A:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x25B:
                    com.Name = "25B";
                    break;
                case 0x25C:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x25D:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x260:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x261:
                    com.Name = "StartMecScript";
                    break;
                //case 0x264:
                //    com.Name = "CheckBurmyCaught";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x267:
                    com.Name = "MakePhoto";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x26A:
                    com.Name = "CheckPhotoSpace";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x26C:
                //    com.Name = "26C";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0x26E:
                //    com.Name = "26E";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x268:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x270:
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x277:
                    com.Name = "277";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x27A:
                //    com.Name = "ShowPanoramicView";
                //    break;
                //case 0x27D:
                //    com.Name = "SetVarTrendyWord";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x27F:
                //    com.Name = "CheckTrendyWordStatus";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x280:
                //    com.Name = "280";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                case 0x282:
                    com.Name = "282";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x284:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x285:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x289:
                    com.Name = "289";
                    break;
                //case 0x28A:
                //    com.Name = "CheckPoffinCaseNumber";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x28B:
                    com.Name = "28B";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x28C:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x28D:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x28E:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x28F:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x290:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x291:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x296:
                    com.Name = "296";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x29C:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x29D:
                    com.Name = "CheckItem";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x29E:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x29F:
                //    com.Name = "BumpCameraAnimation";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2A0:
                //    com.Name = "DoubleTrainerBattle";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2A3:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2A4:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2A5:
                //    com.Name = "ShowPokèmonTradeMenu";
                //    break;
                //case 0x2A7:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                case 0x2A8:
                    com.Name = "2A8";
                    break;
                //case 0x2AA:
                //    com.Name = "ComparePhraseBoxInput";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x2AB:
                    com.Name = "2AB";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x2AC:
                //    com.Name = "ActMisteryGift";
                //    break;
                //case 0x2AD:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2AF:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x2B2:
                    com.Name = "2B2";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x2B3:
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2B6:
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2B7:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2BA:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2C0:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2C4:
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x2C9:
                    com.Name = "ShowRuinsPuzzle";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x2CA:
                    com.Name = "ShowRuinsPhrases";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x2CB:
                    com.Name = "2CB";
                    break;
                //case 0x2CC:
                //    com.Name = "SetVarWifiSprite";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x2CD:
                    com.Name = "2CD";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2CE:
                    com.Name = "2CE";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2CF:
                    com.Name = "2CF";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2D0:
                    com.Name = "2D0";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2D1:
                    com.Name = "2D1";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2D7:
                    com.Name = "StoreShaymin";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2D9:
                    com.Name = "2D9";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2EA:
                    com.Name = "OpenLowScreen";
                    break;
                case 0x2EB:
                    com.Name = "CloseLowScreen";
                    break;
                case 0x2EC:
                    com.Name = "ShowYesNoLowScreen";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2ED:
                    com.Name = "Multi(2ED)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2EE:
                    com.Name = "Multi(2EE)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2EF:
                    com.Name = "SetTextScriptMessageMulti(2EF)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2F0:
                    com.Name = "CloseMulti(2F0)";
                    break;
                case 0x2F1:
                    com.Name = "2F1";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2F2:
                    com.Name = "2F2";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2F3:
                    com.Name = "2F3";
                    break;
                case 0x2F4:
                    com.Name = "2F4";
                    break;
                case 0x2F5:
                    com.Name = "2F5";
                    break;
                case 0x2F6:
                    com.Name = "2F6";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2F7:
                    com.Name = "2F7";
                    break;
                case 0x2F9:
                    com.Name = "2F9";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2FA:
                    com.Name = "2FA";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2FB:
                    com.Name = "2FB";
                    break;
                case 0x2FD:
                    com.Name = "2FD";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2FF:
                    com.Name = "2FF";
                    break;
                case 0x300:
                    com.Name = "300";
                    break;
                case 0x301:
                    com.Name = "301";
                    break;
                case 0x302:
                    com.Name = "302";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x303:
                    com.Name = "303";
                    break;
                case 0x304:
                    com.Name = "304";
                    break;
                case 0x305:
                    com.Name = "305";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x306:
                    com.Name = "306";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x307:
                    com.Name = "307";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x308:
                    com.Name = "308";
                    break;
                case 0x30A:
                    com.Name = "30A";
                    break;
                case 0x30B:
                    com.Name = "30B";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x30C:
                    com.Name = "30C";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x30D:
                    com.Name = "30D";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x30E:
                    com.Name = "30E";
                    break;
                case 0x30F:
                    com.Name = "30F";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x310:
                    com.Name = "310";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x311:
                    com.Name = "311";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x313:
                    com.Name = "313";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x314:
                    com.Name = "314";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x315:
                    com.Name = "315";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x316:
                    com.Name = "316";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x318:
                    com.Name = "318";
                    break;
                case 0x319:
                    com.Name = "319";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x31A:
                    com.Name = "31A";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x31B:
                    com.Name = "31B";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x31C:
                    com.Name = "31C";
                    break;
                case 0x31E:
                    com.Name = "31E";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x320:
                    com.Name = "320";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x324:
                    com.Name = "324";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x325:
                    com.Name = "325";
                    break;
                case 0x326:
                    com.Name = "ChangeShayminForm";
                    break;
                case 0x327:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x328:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x329:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x32A:
                    com.Name = "32A";
                    break;
                case 0x32E:
                    com.Name = "32E";
                    break;
                case 0x32F:
                    com.Name = "32F";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x330:
                    com.Name = "330";
                    break;
                case 0x331:
                    com.Name = "331";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x332:
                    com.Name = "332";
                    break;
                case 0x333:
                    com.Name = "333";
                    break;
                case 0x334:
                    com.Name = "334";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x335:
                    com.Name = "335";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x336:
                    com.Name = "336";
                    break;
                case 0x337:
                    com.Name = "337";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x338:
                    com.Name = "338";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x339:
                    com.Name = "339";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x33A:
                    com.Name = "33A";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x33B:
                    com.Name = "33B";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x33C:
                    com.Name = "33C";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x33D:
                    com.Name = "33D";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x33E:
                    com.Name = "33E";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x340:
                    com.Name = "340";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x345:
                    com.Name = "345";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x347:
                    com.Name = "347";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x348:
                    com.Name = "348";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x349:
                    com.Name = "349";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x34C:
                    com.Name = "SetVarMultipleItem";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x352:
                    com.Name = "SetVarPoffin";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x354:
                    com.Name = "354";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    break;
                default:
                    com.Name = "0x" + com.Id.ToString("X");
                    break;
            }
        }

        private void writeCommandHGSS(BinaryWriter writer, string[] wordLine)
        {
            switch (wordLine[2])
            {
                case "End":
                    writer.Write((Int16)0x2);
                    break;
                case "Return":
                    writer.Write((Int16)0x3);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "Compare":
                    writer.Write((Int16)0x11);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "Compare2":
                    writer.Write((Int16)0x12);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "CallStd":
                    writer.Write((Int16)0x14);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(Byte.Parse(wordLine[4]));
                    break;
                case "ExitStd":
                    writer.Write((Int16)0x15);
                    break;
                case "Jump":
                    writer.Write((Int16)0x16);
                    writer.Write(UInt32.Parse(wordLine[3]) - 4 - writer.BaseStream.Position);
                    break;
                case "Goto":
                    writer.Write((Int16)0x1A);
                    writer.Write(UInt32.Parse(wordLine[3]) - 4 - writer.BaseStream.Position);
                    break;
                case "KillScript":
                    writer.Write((Int16)0x1B);
                    break;
                case "If":
                    writer.Write((Int16)0x1C);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(UInt32.Parse(wordLine[4]) - 4 - writer.BaseStream.Position);
                    break;
                case "If2":
                    writer.Write((Int16)0x1D);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(UInt32.Parse(wordLine[4]) - 4 - writer.BaseStream.Position);
                    break;
                case "SetFlag":
                    writer.Write((Int16)0x1E);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "ClearFlag":
                    writer.Write((Int16)0x1F);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StoreFlag":
                    writer.Write((Int16)0x20);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "22":
                    writer.Write((Int16)0x22);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "ClearTrainerId":
                    writer.Write((Int16)0x23);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "24":
                    writer.Write((Int16)0x24);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StoreTrainerId":
                    writer.Write((Int16)0x25);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "SetValue":
                    writer.Write((Int16)0x26);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "CopyValue":
                    writer.Write((Int16)0x27);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "SetVar":
                    writer.Write((Int16)0x28);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "CopyVar":
                    writer.Write((Int16)0x29);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "Message":
                    writer.Write((Int16)0x2B);
                    writer.Write(Byte.Parse(wordLine[3]));
                    break;
                case "Message2":
                    writer.Write((Int16)0x2C);
                    writer.Write(Byte.Parse(wordLine[3]));
                    break;
                case "Message3":
                    writer.Write((Int16)0x2D);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "Message4":
                    writer.Write((Int16)0x2E);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "Message5":
                    writer.Write((Int16)0x2F);
                    writer.Write(Byte.Parse(wordLine[3]));
                    break;
                case "30":
                    writer.Write((Int16)0x30);
                    break;
                case "WaitButton":
                    writer.Write((Int16)0x31);
                    break;
                case "CloseMessageKeyPress":
                    writer.Write((Int16)0x34);
                    break;
                case "FreezeMessageBox":
                    writer.Write((Int16)0x35);
                    break;
                case "CallMessageBox":
                    writer.Write((Int16)0x36);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "ColorMessageBox":
                    writer.Write((Int16)0x37);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "TypeMessageBox":
                    writer.Write((Int16)0x38);
                    writer.Write(Byte.Parse(wordLine[3]));
                    break;
                case "NoMapMessageBox":
                    writer.Write((Int16)0x39);
                    break;
                case "CallTextMessageBox":
                    writer.Write((Int16)0x3A);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StoreMenuStatus":
                    writer.Write((Int16)0x3B);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "ShowMenu":
                    writer.Write((Int16)0x3C);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "YesNoBox":
                    writer.Write((Int16)0x3E);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "WaitFor":
                    writer.Write((Int16)0x3F);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "Multi":
                    writer.Write((Int16)0x40);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(Byte.Parse(wordLine[4]));
                    writer.Write(Byte.Parse(wordLine[5]));
                    writer.Write(Byte.Parse(wordLine[6]));
                    writer.Write(UInt16.Parse(wordLine[7]));
                    break;
                case "Multi2":
                    writer.Write((Int16)0x41);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(Byte.Parse(wordLine[4]));
                    writer.Write(Byte.Parse(wordLine[5]));
                    writer.Write(Byte.Parse(wordLine[6]));
                    writer.Write(UInt16.Parse(wordLine[7]));
                    break;
                case "SetTextScriptMulti":
                    writer.Write((Int16)0x42);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(Byte.Parse(wordLine[4]));
                    break;
                case "CloseMulti":
                    writer.Write((Int16)0x43);
                    break;
                case "Multi3":
                    writer.Write((Int16)0x44);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(Byte.Parse(wordLine[4]));
                    writer.Write(Byte.Parse(wordLine[5]));
                    writer.Write(Byte.Parse(wordLine[6]));
                    writer.Write(UInt16.Parse(wordLine[7]));
                    writer.Write(Byte.Parse(wordLine[8]));
                    writer.Write(UInt16.Parse(wordLine[9]));
                    break;
                case "Multi4":
                    writer.Write((Int16)0x45);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(Byte.Parse(wordLine[4]));
                    writer.Write(Byte.Parse(wordLine[5]));
                    writer.Write(Byte.Parse(wordLine[6]));
                    writer.Write(UInt16.Parse(wordLine[7]));
                    break;
                case "SetTextScriptMessageMulti":
                    writer.Write((Int16)0x46);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "CloseMulti4":
                    writer.Write((Int16)0x47);
                    break;
                case "SetTextRowMulti":
                    writer.Write((Int16)0x48);
                    writer.Write(Byte.Parse(wordLine[3]));
                    break;
                case "Fanfare":
                    writer.Write((Int16)0x49);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "Fanfare2":
                    writer.Write((Int16)0x4A);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "WaitFanfare":
                    writer.Write((Int16)0x4B);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "Cry":
                    writer.Write((Int16)0x4C);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "WaitCry":
                    writer.Write((Int16)0x4D);
                    break;
                case "PlaySound":
                    writer.Write((Int16)0x4E);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "FadeDef":
                    writer.Write((Int16)0x4F);
                    break;
                case "PlaySound2":
                    writer.Write((Int16)0x50);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StopSound":
                    writer.Write((Int16)0x51);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "RestartSound":
                    writer.Write((Int16)0x52);
                    break;
                case "SwitchSound":
                    writer.Write((Int16)0x54);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StoreSoundMicrophone":
                    writer.Write((Int16)0x55);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "PlaySound3":
                    writer.Write((Int16)0x57);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "58":
                    writer.Write((Int16)0x58);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StoreSoundMicrophone2":
                    writer.Write((Int16)0x59);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "SwitchSound2":
                    writer.Write((Int16)0x5A);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "ActivateMicrophone":
                    writer.Write((Int16)0x5B);
                    break;
                case "DisactivateMicrophone":
                    writer.Write((Int16)0x5C);
                    break;
                case "ApplyMovement":
                    writer.Write((Int16)0x5E);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt32.Parse(wordLine[4]) - 4 - writer.BaseStream.Position);
                    break;
                case "WaitMovement":
                    writer.Write((Int16)0x5F);
                    break;
                case "LockAll":
                    writer.Write((Int16)0x60);
                    break;
                case "ReleaseAll":
                    writer.Write((Int16)0x61);
                    break;
                case "Lock":
                    writer.Write((Int16)0x62);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "Release":
                    writer.Write((Int16)0x63);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "AddPeople":
                    writer.Write((Int16)0x64);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "RemovePeople":
                    writer.Write((Int16)0x65);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "MoveCam":
                    writer.Write((Int16)0x66);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "ZoomCam":
                    writer.Write((Int16)0x67);
                    break;
                case "FacePlayer":
                    writer.Write((Int16)0x68);
                    break;
                case "StoreHeroPosition":
                    writer.Write((Int16)0x69);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StoreOWPosition":
                    writer.Write((Int16)0x6A);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "6B":
                    writer.Write((Int16)0x6B);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "ContinueFollowHero":
                    writer.Write((Int16)0x6C);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "FollowHero":
                    writer.Write((Int16)0x6D);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StopFollowHero":
                    writer.Write((Int16)0x6E);
                    break;
                case "GiveMoney":
                    writer.Write((Int16)0x6F);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "TakeMoney":
                    writer.Write((Int16)0x70);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "CompareMoney":
                    writer.Write((Int16)0x71);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "ShowMoney":
                    writer.Write((Int16)0x72);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "HideMoney":
                    writer.Write((Int16)0x73);
                    break;
                case "UpdateMoney":
                    writer.Write((Int16)0x74);
                    break;
                case "ShowCoins":
                    writer.Write((Int16)0x75);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "HideCoins":
                    writer.Write((Int16)0x76);
                    break;
                case "UpdateCoins":
                    writer.Write((Int16)0x77);
                    break;
                case "CompareCoins":
                    writer.Write((Int16)0x78);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "GiveCoins":
                    writer.Write((Int16)0x79);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "TakeCoins":
                    writer.Write((Int16)0x7A);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "TakeItem":
                    writer.Write((Int16)0x7B);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "GiveItem":
                    writer.Write((Int16)0x7C);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "StoreItemBagNumber":
                    writer.Write((Int16)0x7D);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "StoreItem":
                    writer.Write((Int16)0x7E);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "StoreItemTaken":
                    writer.Write((Int16)0x7F);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StoreItemType":
                    writer.Write((Int16)0x80);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "SendItemType":
                    writer.Write((Int16)0x83);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "StoreUndergroundPcStatus":
                    writer.Write((Int16)0x85);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "SendItemType2":
                    writer.Write((Int16)0x87);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "SendItemType3":
                    writer.Write((Int16)0x8F);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "StorePokèmonParty":
                    writer.Write((Int16)0x93);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StorePokèmonParty2":
                    writer.Write((Int16)0x94);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StorePokèmonParty3":
                    writer.Write((Int16)0x95);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "GivePokèmon":
                    writer.Write((Int16)0x96);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    writer.Write(UInt16.Parse(wordLine[6]));
                    break;
                case "GiveEgg":
                    writer.Write((Int16)0x97);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    writer.Write(UInt16.Parse(wordLine[6]));
                    break;
                case "StoreMove":
                    writer.Write((Int16)0x99);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "StorePlace":
                    writer.Write((Int16)0x9A);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "9B":
                    writer.Write((Int16)0x9B);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "CallEnd":
                    writer.Write((Int16)0xA1);
                    break;
                case "StartWFC":
                    writer.Write((Int16)0xA3);
                    break;
                case "StartInterview":
                    writer.Write((Int16)0xA5);
                    break;
                case "StartDressPokèmon":
                    writer.Write((Int16)0xA6);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "DisplayDressedPokèmon":
                    writer.Write((Int16)0xA7);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "DisplayContestPokèmon":
                    writer.Write((Int16)0xA8);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "OpenBallCapsule":
                    writer.Write((Int16)0xA9);
                    break;
                case "OpenSinnohMaps":
                    writer.Write((Int16)0xAA);
                    break;
                case "OpenPc":
                    writer.Write((Int16)0xAB);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "ShowDrawingUnion":
                    writer.Write((Int16)0xAC);
                    break;
                case "ShowTrainerCaseUnion":
                    writer.Write((Int16)0xAD);
                    break;
                case "ShowTradingUnion":
                    writer.Write((Int16)0xAE);
                    break;
                case "ShowRecordUnion":
                    writer.Write((Int16)0xAF);
                    break;
                case "EndGame":
                    writer.Write((Int16)0xB0);
                    break;
                case "ShowHallOfFame":
                    writer.Write((Int16)0xB1);
                    break;
                case "StoreWFC":
                    writer.Write((Int16)0xB2);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StartWFC2":
                    writer.Write((Int16)0xB3);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "ChooseStarter":
                    writer.Write((Int16)0xB4);
                    break;
                case "BattleStarter":
                    writer.Write((Int16)0xB5);
                    break;
                case "StoreBattleID?":
                    writer.Write((Int16)0xB6);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "SetVarBattle?":
                    writer.Write((Int16)0xB7);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StoreTypeBattle?":
                    writer.Write((Int16)0xB8);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "SetVarBattle2?":
                    writer.Write((Int16)0xB9);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "ChoosePlayerName":
                    writer.Write((Int16)0xBA);
                    break;
                case "ChoosePokèmonName":
                    writer.Write((Int16)0xBB);
                    break;
                case "FadeScreen":
                    writer.Write((Int16)0xBC);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    writer.Write(UInt16.Parse(wordLine[6]));
                    break;
                case "ResetScreen":
                    writer.Write((Int16)0xBD);
                    break;
                case "Warp":
                    writer.Write((Int16)0xBE);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    writer.Write(UInt16.Parse(wordLine[6]));
                    writer.Write(UInt16.Parse(wordLine[7]));
                    break;
                case "RockClimbAnimation":
                    writer.Write((Int16)0xBF);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "SurfAnimation":
                    writer.Write((Int16)0xC0);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "WaterFallAnimation":
                    writer.Write((Int16)0xC1);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "FlyAnimation":
                    writer.Write((Int16)0xC2);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "C5":
                    writer.Write((Int16)0xC5);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "Tuxedo":
                    writer.Write((Int16)0xC6);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StoreBike":
                    writer.Write((Int16)0xC7);
                    writer.Write(Byte.Parse(wordLine[3]));
                    break;
                case "RideBike":
                    writer.Write((Int16)0xC8);
                    writer.Write(Byte.Parse(wordLine[3]));
                    break;
                case "C9":
                    writer.Write((Int16)0xC9);
                    writer.Write(Byte.Parse(wordLine[3]));
                    break;
                case "BerryHeroAnimation":
                    writer.Write((Int16)0xCB);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StopBerryHeroAnimation":
                    writer.Write((Int16)0xCC);
                    break;
                case "SetVarHero":
                    writer.Write((Int16)0xCD);
                    writer.Write(Byte.Parse(wordLine[3]));
                    break;
                case "SetVarRival":
                    writer.Write((Int16)0xCE);
                    writer.Write(Byte.Parse(wordLine[3]));
                    break;
                case "SetVarAlter":
                    writer.Write((Int16)0xCF);
                    writer.Write(Byte.Parse(wordLine[3]));
                    break;
                case "SetVarPokèmon":
                    writer.Write((Int16)0xD0);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "SetVarItem":
                    writer.Write((Int16)0xD1);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "SetVarItem2":
                    writer.Write((Int16)0xD2);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "SetVarBattleItem":
                    writer.Write((Int16)0xD3);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "SetVarAttack":
                    writer.Write((Int16)0xD4);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "SetVarNumber":
                    writer.Write((Int16)0xD5);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "SetVarNickPokèmon":
                    writer.Write((Int16)0xD6);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "SetVarKeyItem":
                    writer.Write((Int16)0xD7);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "SetVarTrainer":
                    writer.Write((Int16)0xD8);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "SetVarTrainer2":
                    writer.Write((Int16)0xD9);
                    writer.Write(Byte.Parse(wordLine[3]));
                    break;
                case "SetVarPokèmonStored":
                    writer.Write((Int16)0xDA);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    writer.Write(Byte.Parse(wordLine[6]));
                    break;
                case "StoreStarter":
                    writer.Write((Int16)0xDE);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "SetVarSwarmPlace":
                    writer.Write((Int16)0xE2);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StoreSwarmInfo":
                    writer.Write((Int16)0xE3);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "E4":
                    writer.Write((Int16)0xE4);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "TrainerBattle":
                    writer.Write((Int16)0xE5);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "E6":
                    writer.Write((Int16)0xE6);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "E7":
                    writer.Write((Int16)0xE7);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "E8":
                    writer.Write((Int16)0xE8);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "E9":
                    writer.Write((Int16)0xE9);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "ActTrainer":
                    writer.Write((Int16)0xEA);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "TeleportPC":
                    writer.Write((Int16)0xEB);
                    break;
                case "StoreBattleResult":
                    writer.Write((Int16)0xEC);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "EE":
                    writer.Write((Int16)0xEE);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "SetFloor":
                    writer.Write((Int16)0x114);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "WarpMapLift":
                    writer.Write((Int16)0x11B);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    writer.Write(UInt16.Parse(wordLine[6]));
                    writer.Write(UInt16.Parse(wordLine[7]));
                    break;
                case "StoreFloor":
                    writer.Write((Int16)0x11C);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StartLift":
                    writer.Write((Int16)0x11D);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StorePokèmonCaught":
                    writer.Write((Int16)0x11E);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "WildPokèmonBattle":
                    writer.Write((Int16)0x124);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StorePhotoImage":
                    writer.Write((Int16)0x12E);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StoreContestImage":
                    writer.Write((Int16)0x12F);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StorePhotoName":
                    writer.Write((Int16)0x130);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "ActPokèKron":
                    writer.Write((Int16)0x131);
                    break;
                case "StorePokèKronStatus":
                    writer.Write((Int16)0x132);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "ActPokèKronApplication":
                    writer.Write((Int16)0x133);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StorePokèKronApplicationStatus":
                    writer.Write((Int16)0x134);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;

                case "135":
                    writer.Write((Int16)0x135);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "139":
                    writer.Write((Int16)0x139);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "13C":
                    writer.Write((Int16)0x13C);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "13F":
                    writer.Write((Int16)0x13F);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "140":
                    writer.Write((Int16)0x140);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "141":
                    writer.Write((Int16)0x141);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "142":
                    writer.Write((Int16)0x142);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "143":
                    writer.Write((Int16)0x143);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "144":
                    writer.Write((Int16)0x144);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "145":
                    writer.Write((Int16)0x145);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "146":
                    writer.Write((Int16)0x146);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "147":
                    writer.Write((Int16)0x147);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "148":
                    writer.Write((Int16)0x148);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StoreGender":
                    writer.Write((Int16)0x14D);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "HealPokèmon":
                    writer.Write((Int16)0x14E);
                    break;
                case "152":
                    writer.Write((Int16)0x152);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StartChooseWifiSprite":
                    writer.Write((Int16)0x154);
                    break;
                case "ChooseWifiSprite":
                    writer.Write((Int16)0x155);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "ActWifiSprite":
                    writer.Write((Int16)0x156);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "ActSinnohPokèdex":
                    writer.Write((Int16)0x158);
                    break;
                case "ActRunningShoes":
                    writer.Write((Int16)0x15A);
                    break;
                case "StoreBadge":
                    writer.Write((Int16)0x15B);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "SetBadge":
                    writer.Write((Int16)0x15C);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StoreBadgeNumber":
                    writer.Write((Int16)0x15D);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "ActRunningShoes2":
                    writer.Write((Int16)0x15F);
                    break;
                case "StartFollowHero":
                    writer.Write((Int16)0x161);
                    break;
                case "StopFollowHero2":
                    writer.Write((Int16)0x162);
                    break;
                case "StartFollowHero2":
                    writer.Write((Int16)0x164);
                    break;
                case "166":
                    writer.Write((Int16)0x166);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "PrepareDoorAnimation":
                    writer.Write((Int16)0x168);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    writer.Write(UInt16.Parse(wordLine[6]));
                    writer.Write(Byte.Parse(wordLine[7]));
                    break;
                case "WaitDoorOpeningAnimation":
                    writer.Write((Int16)0x169);
                    writer.Write(Byte.Parse(wordLine[3]));
                    break;
                case "WaitDoorClosingAnimation":
                    writer.Write((Int16)0x16A);
                    writer.Write(Byte.Parse(wordLine[3]));
                    break;
                case "tDoorOpeningAnimation":
                    writer.Write((Int16)0x16B);
                    writer.Write(Byte.Parse(wordLine[3]));
                    break;
                case "DoorClosingAnimation":
                    writer.Write((Int16)0x16C);
                    writer.Write(Byte.Parse(wordLine[3]));
                    break;
                case "StorePokèmonPartyNumber3":
                    writer.Write((Int16)0x177);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "SetVarPokèmonNature":
                    writer.Write((Int16)0x17C);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "ChangeOwPosition":
                    writer.Write((Int16)0x186);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "SetOwPosition":
                    writer.Write((Int16)0x187);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    writer.Write(UInt16.Parse(wordLine[6]));
                    writer.Write(UInt16.Parse(wordLine[7]));
                    break;
                case "ChangeOwPosition2":
                    writer.Write((Int16)0x188);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "ReleaseOw":
                    writer.Write((Int16)0x189);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "SetTilePassable":
                    writer.Write((Int16)0x18A);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "SetTileLocked":
                    writer.Write((Int16)0x18B);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "ShowChoosePokèmonMenu":
                    writer.Write((Int16)0x191);
                    break;
                case "StoreChosenPokèmon":
                    writer.Write((Int16)0x193);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StorePokèmonId":
                    writer.Write((Int16)0x198);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StorePokèmonPartyNumber":
                    writer.Write((Int16)0x19A);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StorePokèmonPartyNumber2":
                    writer.Write((Int16)0x19B);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "19E":
                    writer.Write((Int16)0x19E);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "19F":
                    writer.Write((Int16)0x19F);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "1A0":
                    writer.Write((Int16)0x1A0);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "1B1":
                    writer.Write((Int16)0x1B1);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "1B2":
                    writer.Write((Int16)0x1B2);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "ShowRecordList":
                    writer.Write((Int16)0x1B5);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StoreTime":
                    writer.Write((Int16)0x1B6);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StorePlayerId":
                    writer.Write((Int16)0x1B7);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StorePokèmonHappiness":
                    writer.Write((Int16)0x1B9);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "SetPokèmonHappiness":
                    writer.Write((Int16)0x1BA);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StoreHeroFaceOrientation":
                    writer.Write((Int16)0x1BD);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StoreSpecificPokèmonParty":
                    writer.Write((Int16)0x1C0);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StoreMoveDeleter":
                    writer.Write((Int16)0x1C6);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StorePokèmonDeleter":
                    writer.Write((Int16)0x1C7);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StorePokèmonMoveNumber":
                    writer.Write((Int16)0x1C8);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "DeletePokèmonMove":
                    writer.Write((Int16)0x1C9);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "SetVarMoveDeleter":
                    writer.Write((Int16)0x1CB);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "ActJournal":
                    writer.Write((Int16)0x1CC);
                    break;
                case "DeActivateLeader":
                    writer.Write((Int16)0x1CD);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    writer.Write(UInt16.Parse(wordLine[6]));
                    writer.Write(UInt16.Parse(wordLine[7]));
                    break;
                case "GiveAccessories":
                    writer.Write((Int16)0x1D2);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StoreAccessories":
                    writer.Write((Int16)0x1D3);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "GiveAccessories2":
                    writer.Write((Int16)0x1D5);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StoreAccessories2":
                    writer.Write((Int16)0x1D6);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StoreSinnohPokèdexStatus":
                    writer.Write((Int16)0x1E8);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StoreNationalPokèdexStatus":
                    writer.Write((Int16)0x1E9);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "GiveSinnohDiploma":
                    writer.Write((Int16)0x1EA);
                    break;
                case "GiveNationalDiploma":
                    writer.Write((Int16)0x1EB);
                    break;
                case "StoreFossilNumber":
                    writer.Write((Int16)0x1F1);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StoreFossilPokèmon":
                    writer.Write((Int16)0x1F4);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "TakeFossil":
                    writer.Write((Int16)0x1F5);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "1F9":
                    writer.Write((Int16)0x1F9);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "1FB":
                    writer.Write((Int16)0x1FB);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "SetSafariGame":
                    writer.Write((Int16)0x202);
                    writer.Write(Byte.Parse(wordLine[3]));
                    break;
                case "StorePokèmonNature":
                    writer.Write((Int16)0x212);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StoreUGFriendNumber":
                    writer.Write((Int16)0x214);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "GenerateRandomPokèmonSearch":
                    writer.Write((Int16)0x218);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "ActRandomPokèmonSearch":
                    writer.Write((Int16)0x219);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StoreRandomPokèmonSearch":
                    writer.Write((Int16)0x21A);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StoreMoveUsableTeacher":
                    writer.Write((Int16)0x21F);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StoreMoveTeacher":
                    writer.Write((Int16)0x221);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StorePokèmonMoveTeacher":
                    writer.Write((Int16)0x223);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "TeachPokèmonMove":
                    writer.Write((Int16)0x224);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StoreTeachingResult":
                    writer.Write((Int16)0x225);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "SetTradeId":
                    writer.Write((Int16)0x226);
                    writer.Write(Byte.Parse(wordLine[3]));
                    break;
                case "StoreChosenPokèmonTrade":
                    writer.Write((Int16)0x228);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "ActMFPokèdex":
                    writer.Write((Int16)0x22C);
                    break;
                case "StoreNationalPokèdex":
                    writer.Write((Int16)0x22D);
                    writer.Write(Byte.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StoreRibbonNumber":
                    writer.Write((Int16)0x22F);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StoreRibbonPokèmon":
                    writer.Write((Int16)0x230);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "GiveRibbonPokèmon":
                    writer.Write((Int16)0x231);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StoreFootPokèmon":
                    writer.Write((Int16)0x23A);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "StoreFirstPokèmonParty":
                    writer.Write((Int16)0x247);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "StorePalParkNumber":
                    writer.Write((Int16)0x254);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "SavePCPalParkPokèmon":
                    writer.Write((Int16)0x255);
                    break;
                case "StorePalParkPoint":
                    writer.Write((Int16)0x256);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    break;
                case "StorePalParkPokèmonCaught":
                    writer.Write((Int16)0x26E);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    break;
                case "DoubleTrainerBattle":
                    writer.Write((Int16)0x2A0);
                    writer.Write(UInt16.Parse(wordLine[3]));
                    writer.Write(UInt16.Parse(wordLine[4]));
                    writer.Write(UInt16.Parse(wordLine[5]));
                    break;
                case "SetVarStarterAlter":
                    writer.Write((Int16)0x2CA);
                    writer.Write(Byte.Parse(wordLine[3]));
                    break;
                default:
                    MessageBox.Show("Unknown Command (Check Syntax of: " + wordLine[2]);
                    break;
            }
        }

        private void writeCommandDPP(BinaryWriter writer, string[] wordLine)
        {
            switch (wordLine[2])
            {
                case "End":
                    writer.Write((Int16)0x2);
                    break;
                case "Return":
                    writer.Write((Int16)0x3);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "Compare":
                    writer.Write((Int16)0x11);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "Compare2":
                    writer.Write((Int16)0x12);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "CallStd":
                    writer.Write((Int16)0x14);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(Byte.Parse(retHex(wordLine[4])));
                    break;
                case "ExitStd":
                    writer.Write((Int16)0x15);
                    break;
                case "Jump":
                    writer.Write((Int16)0x16);
                    writer.Write((UInt32)(UInt32.Parse(retHex(getFunction(wordLine[4] + " " + wordLine[5]))) - 4 - writer.BaseStream.Position));
                    break;
                case "Goto":
                    writer.Write((Int16)0x1A);
                    writer.Write((UInt32)(UInt32.Parse(retHex(getFunction(wordLine[4] + " " + wordLine[5]))) - 4 - writer.BaseStream.Position));
                    break;
                case "KillScript":
                    writer.Write((Int16)0x1B);
                    break;
                case "If":
                    writer.Write((Int16)0x1C);
                    writer.Write((Byte)retConditionCommand(wordLine[3]));
                    writer.Write((UInt32)(UInt32.Parse(retHex(getFunction(wordLine[4] + " " + wordLine[5]))) - 4 - writer.BaseStream.Position));
                    break;
                case "If2":
                    writer.Write((Int16)0x1D);
                    writer.Write((Byte)retConditionCommand(wordLine[3]));
                    writer.Write((UInt32)(UInt32.Parse(retHex(getFunction(wordLine[4] + " " + wordLine[5]))) - 4 - writer.BaseStream.Position));
                    break;
                case "SetFlag":
                    writer.Write((Int16)0x1E);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "ClearFlag":
                    writer.Write((Int16)0x1F);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StoreFlag":
                    writer.Write((Int16)0x20);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "22":
                    writer.Write((Int16)0x22);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "ClearTrainerId":
                    writer.Write((Int16)0x23);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "24":
                    writer.Write((Int16)0x24);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StoreTrainerId":
                    writer.Write((Int16)0x25);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "SetValue":
                    writer.Write((Int16)0x26);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "CopyValue":
                    writer.Write((Int16)0x27);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "SetVar":
                    writer.Write((Int16)0x28);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "CopyVar":
                    writer.Write((Int16)0x29);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "Message":
                    writer.Write((Int16)0x2B);
                    int idMessage = Byte.Parse(retHex(wordLine[3]));
                    writer.Write((Byte)idMessage);
                    saveTextString(wordLine, idMessage, 6);
                    break;
                case "Message2":
                    writer.Write((Int16)0x2C);
                    idMessage = Byte.Parse(retHex(wordLine[3]));
                    writer.Write((Byte)idMessage);
                    saveTextString(wordLine, idMessage, 6);
                    break;
                case "Message3":
                    writer.Write((Int16)0x2D);
                    idMessage = Byte.Parse(retHex(wordLine[3]));
                    writer.Write((Byte)idMessage);
                    saveTextString(wordLine, idMessage, 6);
                    break;
                case "Message4":
                    writer.Write((Int16)0x2E);
                    idMessage = Byte.Parse(retHex(wordLine[3]));
                    writer.Write((Byte)idMessage);
                    saveTextString(wordLine, idMessage, 6);
                    break;
                case "Message5":
                    writer.Write((Int16)0x2F);
                    idMessage = Byte.Parse(retHex(wordLine[3]));
                    writer.Write((Byte)idMessage);
                    saveTextString(wordLine, idMessage, 6);
                    break;
                case "30":
                    writer.Write((Int16)0x30);
                    break;
                case "WaitButton":
                    writer.Write((Int16)0x31);
                    break;
                case "CloseMessageKeyPress":
                    writer.Write((Int16)0x34);
                    break;
                case "FreezeMessageBox":
                    writer.Write((Int16)0x35);
                    break;
                case "CallMessageBox":
                    writer.Write((Int16)0x36);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    idMessage = UInt16.Parse(retHex(wordLine[4]));
                    saveTextString(wordLine, idMessage, 8);
                    break;
                case "ColorMessageBox":
                    writer.Write((Int16)0x37);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "TypeMessageBox":
                    writer.Write((Int16)0x38);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    break;
                case "NoMapMessageBox":
                    writer.Write((Int16)0x39);
                    break;
                case "CallTextMessageBox":
                    writer.Write((Int16)0x3A);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    idMessage = Byte.Parse(retHex(wordLine[3]));
                    saveTextString(wordLine, idMessage, 7);

                    break;
                case "StoreMenuStatus":
                    writer.Write((Int16)0x3B);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "ShowMenu":
                    writer.Write((Int16)0x3C);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "YesNoBox":
                    writer.Write((Int16)0x3E);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "WaitFor":
                    writer.Write((Int16)0x3F);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "Multi":
                    writer.Write((Int16)0x40);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(Byte.Parse(retHex(wordLine[4])));
                    writer.Write(Byte.Parse(retHex(wordLine[5])));
                    writer.Write(Byte.Parse(retHex(wordLine[6])));
                    writer.Write(UInt16.Parse(retHex(wordLine[7])));
                    break;
                case "Multi2":
                    writer.Write((Int16)0x41);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(Byte.Parse(retHex(wordLine[4])));
                    writer.Write(Byte.Parse(retHex(wordLine[5])));
                    writer.Write(Byte.Parse(retHex(wordLine[6])));
                    writer.Write(UInt16.Parse(retHex(wordLine[7])));
                    break;
                case "SetTextScriptMulti":
                    writer.Write((Int16)0x42);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(Byte.Parse(retHex(wordLine[4])));
                    break;
                case "CloseMulti":
                    writer.Write((Int16)0x43);
                    break;
                case "Multi3":
                    writer.Write((Int16)0x44);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(Byte.Parse(retHex(wordLine[4])));
                    writer.Write(Byte.Parse(retHex(wordLine[5])));
                    writer.Write(Byte.Parse(retHex(wordLine[6])));
                    writer.Write(UInt16.Parse(retHex(wordLine[7])));
                    writer.Write(Byte.Parse(retHex(wordLine[8])));
                    writer.Write(UInt16.Parse(retHex(wordLine[9])));
                    break;
                case "Multi4":
                    writer.Write((Int16)0x45);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(Byte.Parse(retHex(wordLine[4])));
                    writer.Write(Byte.Parse(retHex(wordLine[5])));
                    writer.Write(Byte.Parse(retHex(wordLine[6])));
                    writer.Write(UInt16.Parse(retHex(wordLine[7])));
                    break;
                case "SetTextScriptMessageMulti":
                    writer.Write((Int16)0x46);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "CloseMulti4":
                    writer.Write((Int16)0x47);
                    break;
                case "SetTextRowMulti":
                    writer.Write((Int16)0x48);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    break;
                case "Fanfare":
                    writer.Write((Int16)0x49);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "Fanfare2":
                    writer.Write((Int16)0x4A);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "WaitFanfare":
                    writer.Write((Int16)0x4B);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "Cry":
                    writer.Write((Int16)0x4C);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "WaitCry":
                    writer.Write((Int16)0x4D);
                    break;
                case "PlaySound":
                    writer.Write((Int16)0x4E);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "FadeDef":
                    writer.Write((Int16)0x4F);
                    break;
                case "PlaySound2":
                    writer.Write((Int16)0x50);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StopSound":
                    writer.Write((Int16)0x51);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "RestartSound":
                    writer.Write((Int16)0x52);
                    break;
                case "SwitchSound":
                    writer.Write((Int16)0x54);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StoreSoundMicrophone":
                    writer.Write((Int16)0x55);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "PlaySound3":
                    writer.Write((Int16)0x57);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "58":
                    writer.Write((Int16)0x58);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StoreSoundMicrophone2":
                    writer.Write((Int16)0x59);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "SwitchSound2":
                    writer.Write((Int16)0x5A);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "ActivateMicrophone":
                    writer.Write((Int16)0x5B);
                    break;
                case "DisactivateMicrophone":
                    writer.Write((Int16)0x5C);
                    break;
                case "ApplyMovement":
                    writer.Write((Int16)0x5E);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write((UInt32)(UInt32.Parse(retHex(getFunction(wordLine[6] + " " + wordLine[7]))) - 4 - writer.BaseStream.Position));
                    break;
                case "WaitMovement":
                    writer.Write((Int16)0x5F);
                    break;
                case "LockAll":
                    writer.Write((Int16)0x60);
                    break;
                case "ReleaseAll":
                    writer.Write((Int16)0x61);
                    break;
                case "Lock":
                    writer.Write((Int16)0x62);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "Release":
                    writer.Write((Int16)0x63);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "AddPeople":
                    writer.Write((Int16)0x64);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "RemovePeople":
                    writer.Write((Int16)0x65);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "MoveCam":
                    writer.Write((Int16)0x66);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "ZoomCam":
                    writer.Write((Int16)0x67);
                    break;
                case "FacePlayer":
                    writer.Write((Int16)0x68);
                    break;
                case "StoreHeroPosition":
                    writer.Write((Int16)0x69);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StoreOWPosition":
                    writer.Write((Int16)0x6A);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "6B":
                    writer.Write((Int16)0x6B);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "ContinueFollowHero":
                    writer.Write((Int16)0x6C);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "FollowHero":
                    writer.Write((Int16)0x6D);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StopFollowHero":
                    writer.Write((Int16)0x6E);
                    break;
                case "GiveMoney":
                    writer.Write((Int16)0x6F);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "TakeMoney":
                    writer.Write((Int16)0x70);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "CheckMoney":
                    writer.Write((Int16)0x71);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "ShowMoney":
                    writer.Write((Int16)0x72);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "HideMoney":
                    writer.Write((Int16)0x73);
                    break;
                case "UpdateMoney":
                    writer.Write((Int16)0x74);
                    break;
                case "ShowCoins":
                    writer.Write((Int16)0x75);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "HideCoins":
                    writer.Write((Int16)0x76);
                    break;
                case "UpdateCoins":
                    writer.Write((Int16)0x77);
                    break;
                case "CompareCoins":
                    writer.Write((Int16)0x78);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "GiveCoins":
                    writer.Write((Int16)0x79);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "TakeCoins":
                    writer.Write((Int16)0x7A);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "TakeItem":
                    writer.Write((Int16)0x7B);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "GiveItem":
                    writer.Write((Int16)0x7C);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "CheckItemBagSpace":
                    writer.Write((Int16)0x7D);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "CheckItemBagNumber":
                    writer.Write((Int16)0x7E);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "StoreItemTaken":
                    writer.Write((Int16)0x7F);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StoreItemType":
                    writer.Write((Int16)0x80);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "SendItemType":
                    writer.Write((Int16)0x83);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "StoreUndergroundPcStatus":
                    writer.Write((Int16)0x85);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "SendItemType2":
                    writer.Write((Int16)0x87);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "SendItemType3":
                    writer.Write((Int16)0x8F);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "StorePokèmonParty":
                    writer.Write((Int16)0x93);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StorePokèmonParty2":
                    writer.Write((Int16)0x94);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StorePokèmonParty3":
                    writer.Write((Int16)0x95);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "GivePokèmon":
                    writer.Write((Int16)0x96);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    writer.Write(UInt16.Parse(retHex(wordLine[6])));
                    break;
                case "GiveEgg":
                    writer.Write((Int16)0x97);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    writer.Write(UInt16.Parse(retHex(wordLine[6])));
                    break;
                case "StoreMove":
                    writer.Write((Int16)0x99);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "StorePlace":
                    writer.Write((Int16)0x9A);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "9B":
                    writer.Write((Int16)0x9B);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "CallEnd":
                    writer.Write((Int16)0xA1);
                    break;
                case "A2":
                    writer.Write((Int16)0xA2);
                    break;
                case "StartWFC":
                    writer.Write((Int16)0xA3);
                    break;
                case "StartInterview":
                    writer.Write((Int16)0xA5);
                    break;
                case "StartDressPokèmon":
                    writer.Write((Int16)0xA6);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "DisplayDressedPokèmon":
                    writer.Write((Int16)0xA7);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "DisplayContestPokèmon":
                    writer.Write((Int16)0xA8);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "OpenBallCapsule":
                    writer.Write((Int16)0xA9);
                    break;
                case "OpenSinnohMaps":
                    writer.Write((Int16)0xAA);
                    break;
                case "OpenPc":
                    writer.Write((Int16)0xAB);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "ShowDrawingUnion":
                    writer.Write((Int16)0xAC);
                    break;
                case "ShowTrainerCaseUnion":
                    writer.Write((Int16)0xAD);
                    break;
                case "ShowTradingUnion":
                    writer.Write((Int16)0xAE);
                    break;
                case "ShowRecordUnion":
                    writer.Write((Int16)0xAF);
                    break;
                case "EndGame":
                    writer.Write((Int16)0xB0);
                    break;
                case "ShowHallOfFame":
                    writer.Write((Int16)0xB1);
                    break;
                case "StoreWFC":
                    writer.Write((Int16)0xB2);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StartWFC2":
                    writer.Write((Int16)0xB3);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "ChooseStarter":
                    writer.Write((Int16)0xB4);
                    break;
                case "BattleStarter":
                    writer.Write((Int16)0xB5);
                    break;
                case "StoreBattleID?":
                    writer.Write((Int16)0xB6);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "SetVarBattle?":
                    writer.Write((Int16)0xB7);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StoreTypeBattle?":
                    writer.Write((Int16)0xB8);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "SetVarBattle2?":
                    writer.Write((Int16)0xB9);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "ChoosePlayerName":
                    writer.Write((Int16)0xBA);
                    break;
                case "ChoosePokèmonName":
                    writer.Write((Int16)0xBB);
                    break;
                case "FadeScreen":
                    writer.Write((Int16)0xBC);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    writer.Write(UInt16.Parse(retHex(wordLine[6])));
                    break;
                case "ResetScreen":
                    writer.Write((Int16)0xBD);
                    break;
                case "Warp":
                    writer.Write((Int16)0xBE);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    writer.Write(UInt16.Parse(retHex(wordLine[6])));
                    writer.Write(UInt16.Parse(retHex(wordLine[7])));
                    break;
                case "RockClimbAnimation":
                    writer.Write((Int16)0xBF);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "SurfAnimation":
                    writer.Write((Int16)0xC0);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "WaterFallAnimation":
                    writer.Write((Int16)0xC1);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "FlyAnimation":
                    writer.Write((Int16)0xC2);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "C5":
                    writer.Write((Int16)0xC5);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "Tuxedo":
                    writer.Write((Int16)0xC6);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StoreBike":
                    writer.Write((Int16)0xC7);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    break;
                case "RideBike":
                    writer.Write((Int16)0xC8);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    break;
                case "C9":
                    writer.Write((Int16)0xC9);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    break;
                case "BerryHeroAnimation":
                    writer.Write((Int16)0xCB);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StopBerryHeroAnimation":
                    writer.Write((Int16)0xCC);
                    break;
                case "SetVarHero":
                    writer.Write((Int16)0xCD);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    break;
                case "SetVarRival":
                    writer.Write((Int16)0xCE);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    break;
                case "SetVarAlter":
                    writer.Write((Int16)0xCF);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    break;
                case "SetVarPokèmon":
                    writer.Write((Int16)0xD0);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "SetVarItem":
                    writer.Write((Int16)0xD1);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "SetVarItem2":
                    writer.Write((Int16)0xD2);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "SetVarBattleItem":
                    writer.Write((Int16)0xD3);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "SetVarAttack":
                    writer.Write((Int16)0xD4);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "SetVarNumber":
                    writer.Write((Int16)0xD5);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "SetVarNickPokèmon":
                    writer.Write((Int16)0xD6);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "SetVarKeyItem":
                    writer.Write((Int16)0xD7);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "SetVarTrainer":
                    writer.Write((Int16)0xD8);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "SetVarTrainer2":
                    writer.Write((Int16)0xD9);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    break;
                case "SetVarPokèmonStored":
                    writer.Write((Int16)0xDA);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    writer.Write(Byte.Parse(retHex(wordLine[6])));
                    break;
                case "StoreStarter":
                    writer.Write((Int16)0xDE);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "SetVarSwarmPlace":
                    writer.Write((Int16)0xE2);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StoreSwarmInfo":
                    writer.Write((Int16)0xE3);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "E4":
                    writer.Write((Int16)0xE4);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "TrainerBattle":
                    writer.Write((Int16)0xE5);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "E6":
                    writer.Write((Int16)0xE6);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "E7":
                    writer.Write((Int16)0xE7);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "E8":
                    writer.Write((Int16)0xE8);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "E9":
                    writer.Write((Int16)0xE9);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "ActTrainer":
                    writer.Write((Int16)0xEA);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "TeleportPC":
                    writer.Write((Int16)0xEB);
                    break;
                case "StoreBattleResult":
                    writer.Write((Int16)0xEC);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "EE":
                    writer.Write((Int16)0xEE);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "SetFloor":
                    writer.Write((Int16)0x114);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "WarpMapLift":
                    writer.Write((Int16)0x11B);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    writer.Write(UInt16.Parse(retHex(wordLine[6])));
                    writer.Write(UInt16.Parse(retHex(wordLine[7])));
                    break;
                case "StoreFloor":
                    writer.Write((Int16)0x11C);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StartLift":
                    writer.Write((Int16)0x11D);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StorePokèmonNumberCaught":
                    writer.Write((Int16)0x11E);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "WildPokèmonBattle":
                    writer.Write((Int16)0x124);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "CheckDressPicture":
                    writer.Write((Int16)0x12E);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "CheckContestPicture":
                    writer.Write((Int16)0x12F);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StorePictureName":
                    writer.Write((Int16)0x130);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "ActPokèKron":
                    writer.Write((Int16)0x131);
                    break;
                case "CheckPokèKron":
                    writer.Write((Int16)0x132);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "ActPokèKronApplication":
                    writer.Write((Int16)0x133);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "CheckPokèKronApplication":
                    writer.Write((Int16)0x134);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;

                case "135":
                    writer.Write((Int16)0x135);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "139":
                    writer.Write((Int16)0x139);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "13C":
                    writer.Write((Int16)0x13C);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "13F":
                    writer.Write((Int16)0x13F);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "140":
                    writer.Write((Int16)0x140);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "141":
                    writer.Write((Int16)0x141);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "142":
                    writer.Write((Int16)0x142);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "143":
                    writer.Write((Int16)0x143);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "144":
                    writer.Write((Int16)0x144);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "145":
                    writer.Write((Int16)0x145);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "146":
                    writer.Write((Int16)0x146);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "147":
                    writer.Write((Int16)0x147);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "148":
                    writer.Write((Int16)0x148);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StoreGender":
                    writer.Write((Int16)0x14D);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "HealPokèmon":
                    writer.Write((Int16)0x14E);
                    break;
                case "152":
                    writer.Write((Int16)0x152);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StartChooseWifiSprite":
                    writer.Write((Int16)0x154);
                    break;
                case "ChooseWifiSprite":
                    writer.Write((Int16)0x155);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "ActWifiSprite":
                    writer.Write((Int16)0x156);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "ActSinnohPokèdex":
                    writer.Write((Int16)0x158);
                    break;
                case "ActRunningShoes":
                    writer.Write((Int16)0x15A);
                    break;
                case "CheckBadge":
                    writer.Write((Int16)0x15B);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "SetBadge":
                    writer.Write((Int16)0x15C);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StoreBadgeNumber":
                    writer.Write((Int16)0x15D);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "ActRunningShoes2":
                    writer.Write((Int16)0x15F);
                    break;
                case "StartFollowHero":
                    writer.Write((Int16)0x161);
                    break;
                case "StopFollowHero2":
                    writer.Write((Int16)0x162);
                    break;
                case "StartFollowHero2":
                    writer.Write((Int16)0x164);
                    break;
                case "166":
                    writer.Write((Int16)0x166);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "PrepareDoorAnimation":
                    writer.Write((Int16)0x168);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    writer.Write(UInt16.Parse(retHex(wordLine[6])));
                    writer.Write(Byte.Parse(retHex(wordLine[7])));
                    break;
                case "WaitDoorOpeningAnimation":
                    writer.Write((Int16)0x169);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    break;
                case "WaitDoorClosingAnimation":
                    writer.Write((Int16)0x16A);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    break;
                case "DoorOpeningAnimation":
                    writer.Write((Int16)0x16B);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    break;
                case "DoorClosingAnimation":
                    writer.Write((Int16)0x16C);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    break;
                case "StorePokèmonPartyNumber3":
                    writer.Write((Int16)0x177);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "SetVarPokèmonNature":
                    writer.Write((Int16)0x17C);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "ChangeOwPosition":
                    writer.Write((Int16)0x186);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "SetOwPosition":
                    writer.Write((Int16)0x187);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    writer.Write(UInt16.Parse(retHex(wordLine[6])));
                    writer.Write(UInt16.Parse(retHex(wordLine[7])));
                    break;
                case "ChangeOwPosition2":
                    writer.Write((Int16)0x188);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "ReleaseOw":
                    writer.Write((Int16)0x189);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "SetTilePassable":
                    writer.Write((Int16)0x18A);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "SetTileLocked":
                    writer.Write((Int16)0x18B);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "ShowChoosePokèmonMenu":
                    writer.Write((Int16)0x191);
                    break;
                case "StoreChosenPokèmon":
                    writer.Write((Int16)0x193);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StorePokèmonId":
                    writer.Write((Int16)0x198);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StorePokèmonPartyNumber":
                    writer.Write((Int16)0x19A);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StorePokèmonPartyNumber2":
                    writer.Write((Int16)0x19B);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "19E":
                    writer.Write((Int16)0x19E);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "19F":
                    writer.Write((Int16)0x19F);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "1A0":
                    writer.Write((Int16)0x1A0);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "1B1":
                    writer.Write((Int16)0x1B1);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "1B2":
                    writer.Write((Int16)0x1B2);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "ShowRecordList":
                    writer.Write((Int16)0x1B5);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StoreTime":
                    writer.Write((Int16)0x1B6);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StoreBoundedVariable":
                    writer.Write((Int16)0x1B7);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StorePokèmonHappiness":
                    writer.Write((Int16)0x1B9);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "IncPokèmonHappiness":
                    writer.Write((Int16)0x1BA);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StoreHeroFaceOrientation":
                    writer.Write((Int16)0x1BD);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StoreSpecificPokèmonParty":
                    writer.Write((Int16)0x1C0);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StoreMoveDeleter":
                    writer.Write((Int16)0x1C6);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StorePokèmonDeleter":
                    writer.Write((Int16)0x1C7);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StorePokèmonMoveNumber":
                    writer.Write((Int16)0x1C8);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "DeletePokèmonMove":
                    writer.Write((Int16)0x1C9);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "SetVarMoveDeleter":
                    writer.Write((Int16)0x1CB);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "ActJournal":
                    writer.Write((Int16)0x1CC);
                    break;
                case "DeActivateLeader":
                    writer.Write((Int16)0x1CD);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    writer.Write(UInt16.Parse(retHex(wordLine[6])));
                    writer.Write(UInt16.Parse(retHex(wordLine[7])));
                    break;
                case "GiveAccessories":
                    writer.Write((Int16)0x1D2);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StoreAccessories":
                    writer.Write((Int16)0x1D3);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "GiveAccessories2":
                    writer.Write((Int16)0x1D5);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StoreAccessories2":
                    writer.Write((Int16)0x1D6);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StoreSinnohPokèdexStatus":
                    writer.Write((Int16)0x1E8);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StoreNationalPokèdexStatus":
                    writer.Write((Int16)0x1E9);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "GiveSinnohDiploma":
                    writer.Write((Int16)0x1EA);
                    break;
                case "GiveNationalDiploma":
                    writer.Write((Int16)0x1EB);
                    break;
                case "StoreFossilNumber":
                    writer.Write((Int16)0x1F1);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StoreFossilPokèmon":
                    writer.Write((Int16)0x1F4);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "TakeFossil":
                    writer.Write((Int16)0x1F5);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "1F9":
                    writer.Write((Int16)0x1F9);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "1FB":
                    writer.Write((Int16)0x1FB);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "SetSafariGame":
                    writer.Write((Int16)0x202);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    break;
                case "204":
                    writer.Write((Int16)0x204);
                    break;
                case "StorePokèmonNature":
                    writer.Write((Int16)0x212);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StoreUGFriendNumber":
                    writer.Write((Int16)0x214);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "GenerateRandomPokèmonSearch":
                    writer.Write((Int16)0x218);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "ActRandomPokèmonSearch":
                    writer.Write((Int16)0x219);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StoreRandomPokèmonSearch":
                    writer.Write((Int16)0x21A);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StoreMoveUsableTeacher":
                    writer.Write((Int16)0x21F);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StoreMoveTeacher":
                    writer.Write((Int16)0x221);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StorePokèmonMoveTeacher":
                    writer.Write((Int16)0x223);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "TeachPokèmonMove":
                    writer.Write((Int16)0x224);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StoreTeachingResult":
                    writer.Write((Int16)0x225);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "SetTradeId":
                    writer.Write((Int16)0x226);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    break;
                case "StoreChosenPokèmonTrade":
                    writer.Write((Int16)0x228);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "ActMFPokèdex":
                    writer.Write((Int16)0x22C);
                    break;
                case "StoreNationalPokèdex":
                    writer.Write((Int16)0x22D);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StoreRibbonNumber":
                    writer.Write((Int16)0x22F);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StoreRibbonPokèmon":
                    writer.Write((Int16)0x230);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "GiveRibbonPokèmon":
                    writer.Write((Int16)0x231);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StoreFootPokèmon":
                    writer.Write((Int16)0x23A);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "StoreFirstPokèmonParty":
                    writer.Write((Int16)0x247);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "StorePalParkNumber":
                    writer.Write((Int16)0x254);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "DoubleTrainerBattle":
                    writer.Write((Int16)0x2A0);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    writer.Write(UInt16.Parse(retHex(wordLine[5])));
                    break;
                case "SavePCPalParkPokèmon":
                    writer.Write((Int16)0x255);
                    break;
                case "StorePalParkPoint":
                    writer.Write((Int16)0x256);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    writer.Write(UInt16.Parse(retHex(wordLine[4])));
                    break;
                case "StorePalParkPokèmonCaught":
                    writer.Write((Int16)0x26E);
                    writer.Write(UInt16.Parse(retHex(wordLine[3])));
                    break;
                case "2BB":
                    writer.Write((Int16)0x2BB);
                    break;
                case "SetVarStarterAlter":
                    writer.Write((Int16)0x2CA);
                    writer.Write(Byte.Parse(retHex(wordLine[3])));
                    break;
                default:
                    if (wordLine[2].StartsWith("m"))
                    {
                        var mov = wordLine[2].Split('m')[1];
                        writer.Write(Byte.Parse(mov));
                        writer.Write(Byte.Parse(retHex(wordLine[3])));
                    }
                    else
                    {
                        MessageBox.Show("Unknown Command (Check Syntax of: " + wordLine[2]);
                    }
                    break;
            }
        }

        private void saveTextString(string[] wordLine, int idMessage, int skip)
        {

            if (textFile == null)
                return;
            var actualText = textFile.textList[idMessage];
            actualText.text = "";
            actualText.text += "Message " + idMessage + " :'";
            for (int i = skip; i < wordLine.Length; i++)
                actualText.text += wordLine[i] + " ";
            textFile.textList[idMessage] = actualText;
        }

        private void checkNextFunction(BinaryReader reader, uint functionOffset)
        {
            if (functionOffset < reader.BaseStream.Length && reader.BaseStream.Position < reader.BaseStream.Length)
            {
                int check = reader.ReadByte();
                if (!functionOffsetList.Contains(functionOffset)
                    && !(check == 0)
                    && (!movOffsetList.Contains(functionOffset))
                    && (!scriptOrder.Contains(functionOffset)))
                {
                    functionOffsetList.Add(functionOffset);
                    Console.AppendText("\nA function is in: " + functionOffset.ToString());
                }
                reader.BaseStream.Position--;
            }
        }

        private Commands_s readCommandBW(BinaryReader reader, Commands_s com)
        {
            uint functionOffset = 0;
            reader.BaseStream.Position += 1;
            var actualPos = reader.BaseStream.Position;
            if (scriptStartList.Contains((uint)actualPos) || functionOffsetList.Contains((uint)actualPos))
            {
                com.Name = "WrongEnd";
                com.isEnd = 1;
                reader.BaseStream.Position -= 1;
                return com;
            }
            reader.BaseStream.Position -= 1;
            if (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                actualPos = reader.BaseStream.Position;
                if (actualPos + 1 == reader.BaseStream.Length || movOffsetList.Contains((uint)actualPos) || scriptStartList.Contains((uint)actualPos) || functionOffsetList.Contains((uint)actualPos))
                {
                    com.isEnd = 1;
                }
            }
            switch (com.Id)
            {
                case 0x0:
                    com.Name = "Nop";
                    com.isEnd = 1;
                    break;
                case 0x2:
                    com.Name = "End";
                    if (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        var next = reader.ReadByte();
                        if (next == 0)
                            com.isEnd = 1;
                        else
                            reader.BaseStream.Position -= 1;
                    }
                    break;
                case 0x03:
                    com.Name = "ReturnAfterDelay";
                    com.parameters.Add(reader.ReadUInt16()); //Delay
                    break;
                case 0x04:
                    com.Name = "CallRoutine";
                    if (reader.BaseStream.Position < reader.BaseStream.Length - 4)
                    {
                        com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                        functionOffset = com.parameters[0];
                        if (!scriptStartList.Contains(functionOffset) && !functionOffsetList.Contains(functionOffset) && !movOffsetList.Contains(functionOffset))
                        {
                            functionOffsetList.Add(functionOffset);
                            Console.AppendText("\nA function is in: " + functionOffset.ToString());
                        }
                    }
                    break;
                case 0x05:
                    com.Name = "EndFunction";
                    if (reader.BaseStream.Position + 1 > reader.BaseStream.Length) { com.isEnd = 1; break; }
                    var next5 = reader.ReadByte();
                    if (next5 == 0 || movOffsetList.Contains((uint)reader.BaseStream.Position - 1) || scriptOffList.Contains((uint)reader.BaseStream.Position - 1))
                    {
                        com.isEnd = 1;
                        reader.BaseStream.Position -= 1;
                        break;
                    }
                    else
                        reader.BaseStream.Position -= 1;
                    break;
                case 0x06:
                    com.Name = "SetVar(06)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x07:
                    com.Name = "SetVar(07)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x08:
                    com.Name = "CompareTo";
                    com.parameters.Add(reader.ReadUInt16()); //Value to Compare
                    break;
                case 0x09:
                    com.Name = "SetVar(09)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x0A:
                    com.Name = "ClearVar";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x0B:
                    com.Name = "0B";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x10:
                    com.Name = "StoreFlag";
                    com.parameters.Add(reader.ReadUInt16()); //Flag
                    break;
                case 0x11:
                    com.Name = "Condition";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x13:
                    com.Name = "StoreVar(13)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x14:
                    com.Name = "14";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x16:
                    com.Name = "16";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x17:
                    com.Name = "17";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19:
                    com.Name = "Compare";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1C:
                    com.Name = "CallStd";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());//Standard Function Id.
                    break;
                case 0x1D:
                    com.Name = "ReturnStd";
                    break;
                case 0x1E:
                    com.Name = "Jump";
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[0];
                    if (!scriptStartList.Contains(functionOffset) && !functionOffsetList.Contains(functionOffset) && !movOffsetList.Contains(functionOffset))
                    {
                        functionOffsetList.Add(functionOffset);
                        Console.AppendText("\nA function is in: " + functionOffset.ToString());
                    }
                    com.numJump++;
                    com.isEnd = 1;
                    break;
                case 0x1F:
                    com.Name = "If";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[1];
                    if (!scriptStartList.Contains(functionOffset) && !functionOffsetList.Contains(functionOffset) && !movOffsetList.Contains(functionOffset))
                    {
                        functionOffsetList.Add(functionOffset);
                        Console.AppendText("\nA function is in: " + functionOffset.ToString());
                    }
                    com.numJump++;
                    break;
                case 0x21:
                    com.Name = "21";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22:
                    com.Name = "22";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x23:
                    com.Name = "SetFlag";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24:
                    com.Name = "ClearFlag";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x25:
                    com.Name = "StoreVarFlag";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x26:
                    com.Name = "StoreAddVar";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x27:
                    com.Name = "StoreSubVar";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x28:
                    com.Name = "StoreVarValue";
                    com.parameters.Add(reader.ReadUInt16()); //Var as container
                    com.parameters.Add(reader.ReadUInt16()); //Value to store
                    break;
                case 0x29:
                    com.Name = "StoreVarVariable";
                    com.parameters.Add(reader.ReadUInt16()); //Var as container
                    com.parameters.Add(reader.ReadUInt16()); //Value to store
                    break;
                case 0x2A:
                    com.Name = "StoreVarCallable";
                    com.parameters.Add(reader.ReadUInt16()); //Var as container
                    com.parameters.Add(reader.ReadUInt16()); //Value to store
                    break;
                case 0x2B:
                    com.Name = "StoreVar(2B)";
                    com.parameters.Add(reader.ReadUInt16()); //Var as container
                    com.parameters.Add(reader.ReadUInt16()); //Value to store
                    break;
                case 0x2D:
                    com.Name = "ClearVar(2D)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2E:
                    com.Name = "LockAll";
                    break;
                case 0x2F:
                    com.Name = "UnlockAll";
                    break;
                case 0x30:
                    com.Name = "WaitMoment";
                    break;
                case 0x31:
                    com.Name = "31";
                    break;
                case 0x32:
                    com.Name = "WaitButton";
                    break;
                case 0x33:
                    com.Name = "MusicalMessage";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    break;
                case 0x34:
                    com.Name = "EventGreyMessage";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    break;
                case 0x35:
                    com.Name = "MusicalMessage2";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    break;
                case 0x36:
                    com.Name = "CloseEventGreyMessage";
                    break;
                case 0x37:
                    com.Name = "37";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x38:
                    com.Name = "BubbleMessage";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadByte()); //Bottom/Top View.
                    break;
                case 0x39:
                    com.Name = "CloseBubbleMessage";
                    break;
                case 0x3A:
                    com.Name = "ShowMessageAt";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //X coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate
                    break;
                case 0x3B:
                    com.Name = "CloseShowMessageAt";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x3C:
                    com.Name = "Message";
                    com.parameters.Add(reader.ReadByte()); //Costant
                    com.parameters.Add(reader.ReadByte()); //Costant
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    if (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                        com.parameters.Add(reader.ReadUInt16()); //Message Type
                    }
                    break;
                case 0x3D:
                    com.Name = "Message2";
                    com.parameters.Add(reader.ReadByte()); //Costant
                    com.parameters.Add(reader.ReadByte()); //Costant
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    com.parameters.Add(reader.ReadUInt16()); //Message Type
                    break;
                case 0x3E:
                    com.Name = "CloseMessageKeyPress";
                    break;
                case 0x3F:
                    com.Name = "CloseMessageKeyPress2";
                    break;
                case 0x40:
                    com.Name = "MoneyBox";
                    com.parameters.Add(reader.ReadUInt16()); //X coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate
                    break;
                case 0x41:
                    com.Name = "CloseMoneyBox";
                    break;
                case 0x42:
                    com.Name = "UpdateMoneyBox";
                    break;
                case 0x43:
                    com.Name = "BorderedMessage";
                    com.parameters.Add(reader.ReadUInt16()); //MessageId
                    com.parameters.Add(reader.ReadUInt16()); //Color
                    break;
                case 0x44:
                    com.Name = "CloseBorderedMessage";
                    break;
                case 0x45:
                    com.Name = "PaperMessage";
                    com.parameters.Add(reader.ReadUInt16()); //MessageId
                    com.parameters.Add(reader.ReadUInt16()); //Trans. Coordinate
                    break;
                case 0x46:
                    com.Name = "ClosePaperMessage";
                    break;
                case 0x47:
                    com.Name = "YesNoBox";
                    com.parameters.Add(reader.ReadUInt16()); //Variable: NO = 0, YES = 1
                    break;
                case 0x48:
                    com.Name = "Message3";
                    com.parameters.Add(reader.ReadByte()); //Costant
                    com.parameters.Add(reader.ReadByte()); //Costant
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id 
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    com.parameters.Add(reader.ReadUInt16()); //Message Type
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x49:
                    com.Name = "DoubleMessage";
                    com.parameters.Add(reader.ReadByte()); //Costant
                    com.parameters.Add(reader.ReadByte()); //Costant
                    com.parameters.Add(reader.ReadUInt16()); //Id Message Black
                    com.parameters.Add(reader.ReadUInt16()); //Id Message White
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id 
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    com.parameters.Add(reader.ReadUInt16()); //Message Type
                    break;
                case 0x4A:
                    com.Name = "AngryMessage";
                    com.parameters.Add(reader.ReadUInt16()); //MessageId
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    break;
                case 0x4B:
                    com.Name = "CloseAngryMessage";
                    break;
                case 0x4C:
                    com.Name = "SetVarHero";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x4D:
                    com.Name = "SetVarItem";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x4E:
                    com.Name = "SetVarItemNumber";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x4F:
                    com.Name = "SetVarItemContainter";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x50:
                    com.Name = "SetVarItemContained";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x51:
                    com.Name = "SetVarMove";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x52:
                    com.Name = "SetVarBag";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x53:
                    com.Name = "SetVarPartyPokèmon";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x54:
                    com.Name = "SetVarPartyPokèmonNick";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x55:
                    com.Name = "SetVar(55)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x56:
                    com.Name = "SetVarType";		     //382
                    com.parameters.Add(reader.ReadByte());   //text variable to set
                    com.parameters.Add(reader.ReadUInt16()); //type to set
                    break;
                case 0x57:
                    com.Name = "SetVarPokèmon";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x58:
                    com.Name = "SetVarPokèmon2";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x59:
                    com.Name = "SetVarLocation";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x5A:
                    com.Name = "SetVarPokèmonNick";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x5B:
                    com.Name = "SetVarPokèMansion";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x5C: // example 654
                    com.Name = "SetVarNumberCond";
                    com.parameters.Add(reader.ReadByte()); // 1 ?
                    com.parameters.Add(reader.ReadUInt16()); // Container to store to
                    com.parameters.Add(reader.ReadUInt16()); // Stat to Store [HP ATK DEF SPE SPA SPD, 0-5]
                    break;
                case 0x5D:
                    com.Name = "SetVarMusicalInfo";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x5E:
                    com.Name = "SetVarNations";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x5F:
                    com.Name = "SetVarActivities";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x60:
                    com.Name = "SetVarPower";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x61:
                    com.Name = "SetVarTrainerType";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x62:
                    com.Name = "SetVarTrainerType2";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x63:
                    com.Name = "SetVarGeneralWord";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x64:
                    com.Name = "ApplyMovement";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    var movOffset = com.parameters[1];
                    if (!movOffsetList.Contains(movOffset))
                    {
                        movOffsetList.Add(movOffset);
                        Console.AppendText("\nA movement is in: " + movOffset.ToString());
                    }
                    break;
                case 0x65:
                    com.Name = "WaitMovement";
                    break;
                case 0x66:
                    com.Name = "66";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x67:
                    com.Name = "StoreNPCPosition";
                    com.parameters.Add(reader.ReadUInt16()); //NPC
                    com.parameters.Add(reader.ReadUInt16()); //X container
                    com.parameters.Add(reader.ReadUInt16()); //Y container

                    break;
                case 0x68:
                    com.Name = "StoreHeroPosition";
                    com.parameters.Add(reader.ReadUInt16()); // X container.
                    com.parameters.Add(reader.ReadUInt16()); // Y container.
                    break;
                case 0x69:
                    com.Name = "CreateNPC";
                    com.parameters.Add(reader.ReadUInt16()); // X container
                    com.parameters.Add(reader.ReadUInt16()); // Y container.
                    com.parameters.Add(reader.ReadUInt16()); // Face Direction
                    com.parameters.Add(reader.ReadUInt16()); // NPC Id
                    com.parameters.Add(reader.ReadUInt16()); // Sprite
                    com.parameters.Add(reader.ReadUInt16()); // Movement Permission
                    break;
                case 0x6A:
                    com.Name = "StoreNPCFlag";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16()); //Flag
                    break;
                case 0x6B:
                    com.Name = "AddNPC";
                    com.parameters.Add(reader.ReadUInt16()); //Npc Id
                    break;
                case 0x6C:
                    com.Name = "RemoveNPC";
                    com.parameters.Add(reader.ReadUInt16()); //Npc Id
                    break;
                case 0x6D:
                    com.Name = "RelocateNPC";
                    com.parameters.Add(reader.ReadUInt16()); //Npc Id
                    com.parameters.Add(reader.ReadUInt16()); //X coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Z coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Face direction
                    break;
                case 0x6E:
                    com.Name = "StoreHeroNPCOrientation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x6F:
                    com.Name = "CheckNPCRemoved";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16()); //Return Value
                    break;
                case 0x70:
                    com.Name = "70";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16()); //Return Value
                    com.parameters.Add(reader.ReadUInt16()); //X
                    com.parameters.Add(reader.ReadUInt16()); //S_854 = 0, S_855 = 3
                    com.parameters.Add(reader.ReadUInt16()); //Y
                    break;
                case 0x73:
                    com.Name = "73";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x74:
                    com.Name = "FacePlayer";
                    break;
                case 0x75:
                    com.Name = "Release";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    break;
                case 0x76:
                    com.Name = "ReleaseAll";
                    break;
                case 0x77:
                    com.Name = "Lock";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    break;
                case 0x78:
                    com.Name = "CheckLock";
                    com.parameters.Add(reader.ReadUInt16()); //variable
                    break;
                case 0x79:
                    com.Name = "StoreNPCLevel";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x7B:
                    com.Name = "MoveNpctoCoordinates";
                    com.parameters.Add(reader.ReadUInt16()); //Npc Id
                    com.parameters.Add(reader.ReadUInt16()); //X coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Z coordinate
                    break;
                case 0x7C:
                    com.Name = "7C";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x7E:
                    com.Name = "TeleportUpNPc";
                    com.parameters.Add(reader.ReadUInt16()); //Npc Id
                    break;
                case 0x7F:
                    com.Name = "StoreTrainerIdNPCS";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16()); //Trainer Id
                    break;
                case 0x80:
                    com.Name = "80";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x81:
                    com.Name = "81";
                    break;
                case 0x82:
                    com.Name = "82";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x83:
                    com.Name = "SetVar(83)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x84:
                    com.Name = "SetVar(84)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x85:
                    com.Name = "SingleTrainerBattle";
                    com.parameters.Add(reader.ReadUInt16()); //Trainer Id
                    com.parameters.Add(reader.ReadUInt16()); //2th Trainer Id (If 0x0 Single Battle)
                    com.parameters.Add(reader.ReadUInt16()); //win loss logic (0 standard, 1 loss=>win)
                    break;
                case 0x86:
                    com.Name = "DoubleTrainerBattle";
                    com.parameters.Add(reader.ReadUInt16()); //Ally
                    com.parameters.Add(reader.ReadUInt16()); //Opp1 Trainer Id
                    com.parameters.Add(reader.ReadUInt16()); //Opp2 Trainer Id
                    com.parameters.Add(reader.ReadUInt16()); //win loss logic (0 standard, 1 loss=>win)
                    break;
                case 0x87:
                    com.Name = "MessageBattle";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //Message Id?
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x88:
                    com.Name = "MessageBattle2";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x8A:
                    com.Name = "8A";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x8B:
                    com.Name = "PlayTrainerMusic";
                    com.parameters.Add(reader.ReadUInt16()); // music to play
                    break;
                case 0x8C:
                    com.Name = "EndBattle";
                    break;
                case 0x8D:
                    com.Name = "StoreBattleResult";
                    com.parameters.Add(reader.ReadUInt16()); //Variable as container.
                    break;
                case 0x8E:
                    com.Name = "DisableTrainer";
                    break;
                case 0x92:
                    com.Name = "92";
                    com.parameters.Add(reader.ReadUInt16()); //Trainer Id
                    com.parameters.Add(reader.ReadUInt16()); //Variable as container
                    break;
                case 0x93:
                    com.Name = "93";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x94:
                    com.Name = "TrainerBattle";
                    com.parameters.Add(reader.ReadUInt16());//Trainer ID
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x95:
                    com.Name = "DeactiveTrainerId";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x96:
                    com.Name = "96";
                    com.parameters.Add(reader.ReadUInt16()); //Trainer ID
                    break;
                case 0x97:
                    com.Name = "StoreActiveTrainerId";
                    com.parameters.Add(reader.ReadUInt16()); //Trainer ID
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x98:
                    com.Name = "ChangeMusic";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x9E:
                    com.Name = "FadeToDefaultMusic";
                    break;
                case 0x9F:
                    com.Name = "PlayMusic";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xA1:
                    com.Name = "StopMusic";
                    com.parameters.Add(reader.ReadUInt16()); //sound?
                    break;
                case 0xA2:
                    com.Name = "A2";
                    com.parameters.Add(reader.ReadUInt16()); //sound?
                    com.parameters.Add(reader.ReadUInt16()); //strain
                    break;
                case 0xA3:
                    com.Name = "AddInstrument";
                    com.parameters.Add(reader.ReadUInt16()); //Instrument
                    break;
                case 0xA4:
                    com.Name = "RemoveInstrument";
                    com.parameters.Add(reader.ReadUInt16()); //Instrument
                    break;
                case 0xA5:
                    com.Name = "CheckInstrument";
                    com.parameters.Add(reader.ReadUInt16()); //Active/Not active(ret)
                    com.parameters.Add(reader.ReadUInt16()); //Instrument
                    break;
                case 0xA6:
                    com.Name = "PlaySound";
                    com.parameters.Add(reader.ReadUInt16()); //Sound Id
                    break;
                case 0xA7:
                    com.Name = "WaitSound(A7)";
                    break;
                case 0xA8:
                    com.Name = "WaitSound(A8)";
                    break;
                case 0xA9:
                    com.Name = "PlayFanfare";
                    com.parameters.Add(reader.ReadUInt16()); //Fanfare Id
                    break;
                case 0xAA:
                    com.Name = "WaitFanfare";
                    break;
                case 0xAB:
                    com.Name = "Cry";
                    com.parameters.Add(reader.ReadUInt16()); //Pokemon Index #
                    com.parameters.Add(reader.ReadUInt16()); //0 ~ unknown
                    break;
                case 0xAC:
                    com.Name = "WaitCry";
                    break;
                case 0xAF:
                    com.Name = "SetTextScriptMessage";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xB0:
                    com.Name = "CloseMulti";
                    break;
                case 0xB1:
                    com.Name = "B1";
                    break;
                case 0xB2:
                    com.Name = "Multi(B2)";			//88
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xB3:
                    com.Name = "FadeScreen";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xB4:
                    com.Name = "ResetScreen";
                    break;
                case 0xB5:
                    com.Name = "GiveItem";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xB6:
                    com.Name = "TakeItem";
                    com.parameters.Add(reader.ReadUInt16()); // Item Index Number
                    com.parameters.Add(reader.ReadUInt16()); // Quantity
                    com.parameters.Add(reader.ReadUInt16()); // Return Result (0=added successfully | 1=full bag)
                    break;
                case 0xB7:
                    com.Name = "CheckItemBagSpace";		//Store if it is possible to give an item.
                    com.parameters.Add(reader.ReadUInt16()); // Item Index Number
                    com.parameters.Add(reader.ReadUInt16()); // Quantity
                    com.parameters.Add(reader.ReadUInt16()); // Return Result (0=not full | 1=full)
                    break;
                case 0xB8:
                    com.Name = "CheckItemBagNumber";              //222
                    com.parameters.Add(reader.ReadUInt16()); // Item #
                    com.parameters.Add(reader.ReadUInt16()); // Minimum Quantity / Return X if has >=1
                    com.parameters.Add(reader.ReadUInt16()); // Result Storage variable/container
                    break;
                case 0xB9:
                    com.Name = "StoreItemCount";
                    com.parameters.Add(reader.ReadUInt16()); // item #
                    com.parameters.Add(reader.ReadUInt16()); // Return to storage
                    break;
                case 0xBA:
                    com.Name = "CheckItemContainer";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xBB:
                    com.Name = "StoreItemBag";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xBC:
                    com.Name = "BC";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xBD:
                    com.Name = "BD";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xBE:
                    com.Name = "Warp";
                    com.parameters.Add(reader.ReadUInt16()); //Map Id
                    com.parameters.Add(reader.ReadUInt16()); // X coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Y coordinate
                    break;
                case 0xBF:
                    com.Name = "TeleportWarp";
                    com.parameters.Add(reader.ReadUInt16()); //Map Id
                    com.parameters.Add(reader.ReadUInt16()); // X coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Y coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Z coordinate
                    break;
                case 0xC1:
                    com.Name = "FallWarp";
                    com.parameters.Add(reader.ReadUInt16()); //Map Id
                    com.parameters.Add(reader.ReadUInt16()); // X coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Y coordinate
                    break;
                case 0xC2:
                    com.Name = "FastWarp";
                    com.parameters.Add(reader.ReadUInt16()); //Map Id
                    com.parameters.Add(reader.ReadUInt16()); // X coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Y coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Hero's Facing
                    break;
                case 0xC3:
                    com.Name = "UnionWarp"; // warp to union room
                    break;
                case 0xC4:
                    com.Name = "TeleportWarp";
                    com.parameters.Add(reader.ReadUInt16()); //Map Id
                    com.parameters.Add(reader.ReadUInt16()); // X coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Y coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Z coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Hero's Facing
                    break;
                case 0xC5:
                    com.Name = "SurfAnimation";
                    break;
                case 0xC6:
                    com.Name = "SpecialAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC7:
                    com.Name = "SpecialAnimation2";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC8:
                    com.Name = "CallAnimation";
                    com.parameters.Add(reader.ReadUInt16()); //Animation Id
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xCB:
                    com.Name = "StoreRandomNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xCC:
                    com.Name = "StoreVarItem";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xCD:
                    com.Name = "StoreDayPart";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xCE:
                    com.Name = "StoreStarCardNumber";
                    com.parameters.Add(reader.ReadUInt16()); //Star on Trainer Card
                    break;
                case 0xCF:
                    com.Name = "StoreDay";
                    com.parameters.Add(reader.ReadUInt16()); //Day(Ret)
                    break;
                case 0xD0:
                    com.Name = "StoreDate";
                    com.parameters.Add(reader.ReadUInt16()); //Month Return to var/cont
                    com.parameters.Add(reader.ReadUInt16()); //Day Return to var/cont
                    break;
                case 0xD1:
                    com.Name = "StoreHour";
                    com.parameters.Add(reader.ReadUInt16()); //Hour(Ret)
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD2:
                    com.Name = "StoreSeason";
                    com.parameters.Add(reader.ReadUInt16()); //Season(Ret)
                    break;
                case 0xD3:
                    com.Name = "D3";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD4:
                    com.Name = "StoreBirthDay";
                    com.parameters.Add(reader.ReadUInt16()); //Month
                    com.parameters.Add(reader.ReadUInt16()); //Day
                    break;
                case 0xD5:
                    com.Name = "StoreBadge";
                    com.parameters.Add(reader.ReadUInt16()); //Variable to return
                    com.parameters.Add(reader.ReadUInt16()); //Badge Id
                    break;
                case 0xD6:
                    com.Name = "SetBadge";
                    com.parameters.Add(reader.ReadUInt16()); //Badge Id
                    break;
                case 0xD7:
                    com.Name = "StoreBadgeNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD8:
                    com.Name = "CheckChangeMapEvent";
                    com.parameters.Add(reader.ReadUInt16()); //Id event
                    com.parameters.Add(reader.ReadUInt16()); //Is changed? (ret)
                    break;
                case 0xD9:
                    com.Name = "D9";
                    com.parameters.Add(reader.ReadUInt16()); //Number
                    break;
                case 0xDA:
                    com.Name = "DA";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xDB:
                    com.Name = "ActUnionRoom";
                    break;
                case 0xDC:
                    com.Name = "DC";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xDD:
                    com.Name = "StorePokèmonCaught";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xDE:
                    com.Name = "ShowPokèmonSpecies"; // species display popup, Store
                    com.parameters.Add(reader.ReadUInt16()); //0
                    com.parameters.Add(reader.ReadUInt16()); //species
                    break;
                case 0xDF:
                    com.Name = "CheckPokèmonSeen";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16()); //True/False (Ret)
                    break;
                case 0xE0:
                    com.Name = "StoreVersion";
                    com.parameters.Add(reader.ReadUInt16()); //return result to this variable/cont
                    break;
                case 0xE1:
                    com.Name = "StoreGender";
                    com.parameters.Add(reader.ReadUInt16()); //return result to this variable/cont
                    break;
                case 226:
                    com.Name = "E2";
                    com.parameters.Add(reader.ReadUInt16()); //return result to this variable/cont
                    break;
                case 0xE4:
                    com.Name = "E4";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE5:
                    com.Name = "StoreTrainerType";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 230:
                    com.Name = "CheckKeyItem";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE7:
                    com.Name = "ActivateKeyItem";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 232:
                    com.Name = "CheckKeyItem2";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE9:
                    com.Name = "E9";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xEA:
                    com.Name = "EA";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xEB:
                    com.Name = "EB";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xEC:
                    com.Name = "TakeEggDayCare";
                    break;
                case 0xED:
                    com.Name = "KeepEggDayCare";
                    break;
                case 0xEE:
                    com.Name = "EE";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xEF:
                    com.Name = "StoreAffinityDayCare";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF0:
                    com.Name = "GivePokèDayCare";
                    com.parameters.Add(reader.ReadUInt16()); //POKE
                    break;
                case 0xF1:
                    com.Name = "TakePokèDayCare";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF2:
                    com.Name = "StorePokèDayCare";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF3:
                    com.Name = "StoreFormDayCare";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF4:
                    com.Name = "StoreLevelDayCare";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF5:
                    com.Name = "StoreLevelDiffDayCare";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF6:
                    com.Name = "StoreMoneyDayCare";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF7:
                    com.Name = "ChoosePokèDayCare";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF8:
                    com.Name = "StoreDayCare(F8)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF9:
                    com.Name = "F9";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xFA:
                    com.Name = "TakeMoney";			//66
                    com.parameters.Add(reader.ReadUInt16()); //Removes this amount of money from the player's $.
                    break;
                case 0xFB:
                    com.Name = "CheckEnoughMoney";			//66
                    com.parameters.Add(reader.ReadUInt16()); //Result storage container (0=enough $|1=not enough $)
                    com.parameters.Add(reader.ReadUInt16()); //Stores if current $ is >= [THIS ARGUMENT]
                    break;
                case 0xFC:
                    com.Name = "StorePokèmonHappiness";
                    com.parameters.Add(reader.ReadUInt16()); //Happiness storage container
                    com.parameters.Add(reader.ReadUInt16()); //Party member to Store
                    break;
                case 0xFD:
                    com.Name = "IncPokèmonHappiness";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xFE:
                    com.Name = "StorePartySpecies";
                    if (reader.BaseStream.Position == reader.BaseStream.Length) //this is temporary to catch movement related errors
                    {
                        com.isEnd = 1;
                        break;
                    }
                    com.parameters.Add(reader.ReadUInt16());    // Result Storage of Storeed species index #
                    if (reader.BaseStream.Position + 1 >= reader.BaseStream.Length)
                    {
                        com.isEnd = 1;
                        break;
                    }
                    else
                    {
                        com.parameters.Add(reader.ReadUInt16()); // PKM to Store
                        break;
                    }
                case 0xFF:
                    com.Name = "StorePokèmonFormNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    var nextFF = reader.ReadUInt16();
                    while (nextFF >= 0x4000) { com.parameters.Add(nextFF); nextFF = reader.ReadUInt16(); }
                    reader.BaseStream.Position -= 2;
                    break;
                case 0x100:
                    com.Name = "CheckPokèrus";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x101:
                    com.Name = "101";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x102:
                    com.Name = "CheckEgg";
                    com.parameters.Add(reader.ReadUInt16()); //Result storage container
                    com.parameters.Add(reader.ReadUInt16()); //Party member to Store
                    break;
                case 0x103:
                    com.Name = "StorePartyNumberMinimum";
                    com.parameters.Add(reader.ReadUInt16()); //Result Storage container
                    com.parameters.Add(reader.ReadUInt16()); //Does the player have more than [VALUE]? Return 0 if true.
                    break;
                case 0x104:
                    com.Name = "HealPokèmon";
                    break;
                case 0x105:
                    com.Name = "RenamePokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x106:
                    com.Name = "106";
                    com.parameters.Add(reader.ReadUInt16()); //Req
                    break;
                case 0x107:
                    com.Name = "StoreChosenPokèmon";
                    com.parameters.Add(reader.ReadUInt16()); //Dialog Result Logic (1 if PKM Chosen) default 0
                    com.parameters.Add(reader.ReadUInt16()); //    \->Variable Storage
                    com.parameters.Add(reader.ReadUInt16()); //Pokemon Choice Variable Storage
                    break;
                case 0x108:
                    com.Name = "StorePokèmonMoveLearned";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x109:
                    com.Name = "ChooseMoveForgot";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x10A:
                    com.Name = "StorePokèmonMoveForgot";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x10B:
                    com.Name = "10B";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x10C:
                    com.Name = "GivePokèmon";
                    com.parameters.Add(reader.ReadUInt16()); //Id Pokèmon
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //Item
                    com.parameters.Add(reader.ReadUInt16()); //Level
                    break;
                case 0x10D:
                    com.Name = "StorePokemonPartyAt";
                    com.parameters.Add(reader.ReadUInt16()); //Id Pokèmon Party
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x10E:
                    com.Name = "GivePokèmon(10E)";
                    com.parameters.Add(reader.ReadUInt16()); //Variable to return result to
                    com.parameters.Add(reader.ReadUInt16()); //Egg Pokemon to try to
                    com.parameters.Add(reader.ReadUInt16()); //Forme
                    com.parameters.Add(reader.ReadUInt16()); //Level
                    com.parameters.Add(reader.ReadUInt16()); //3
                    com.parameters.Add(reader.ReadUInt16()); //2
                    com.parameters.Add(reader.ReadUInt16()); //0
                    com.parameters.Add(reader.ReadUInt16()); //0
                    com.parameters.Add(reader.ReadUInt16()); //4
                    break;
                case 0x10F:
                    com.Name = "GiveEgg(10F)";
                    com.parameters.Add(reader.ReadUInt16()); //Variable to return result to
                    com.parameters.Add(reader.ReadUInt16()); //Egg Pokemon to try to
                    com.parameters.Add(reader.ReadUInt16()); //Response if Party is Full (~0=true or FORME?)
                    break;
                case 0x110:
                    com.Name = "StorePokèmonSex";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x113:
                    com.Name = "CheckPokèmonNickname";
                    com.parameters.Add(reader.ReadUInt16()); //Is different from default?(Ret)
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    break;
                case 0x114:
                    com.Name = "StorePartyHavePokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x115:
                    com.Name = "StorePartyCanLearnMove";
                    com.parameters.Add(reader.ReadUInt16()); //Variable to return result to
                    com.parameters.Add(reader.ReadUInt16()); //move to Store
                    com.parameters.Add(reader.ReadUInt16()); //Party member to Store
                    break;
                case 0x116:
                    com.Name = "StorePartyCanUseMove";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x117:
                    com.Name = "StorePokemonForm";
                    com.parameters.Add(reader.ReadUInt16()); //var
                    com.parameters.Add(reader.ReadUInt16()); //val
                    break;
                case 0x118:
                    com.Name = "CheckChosenSpecies";
                    com.parameters.Add(reader.ReadUInt16()); //Specie
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon
                    break;
                case 0x11A:
                    com.Name = "11A";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x11B:
                    com.Name = "StorePartyType";
                    com.parameters.Add(reader.ReadUInt16()); // Return Type 1
                    com.parameters.Add(reader.ReadUInt16()); // Return Type 2
                    com.parameters.Add(reader.ReadUInt16()); // Party member to Store
                    break;
                case 0x11C:
                    com.Name = "ForgotMove";
                    com.parameters.Add(reader.ReadUInt16()); //var
                    com.parameters.Add(reader.ReadUInt16()); //var
                    com.parameters.Add(reader.ReadUInt16()); //var
                    break;
                case 0x11D:
                    com.Name = "SetFavorite";			//82
                    com.parameters.Add(reader.ReadUInt16()); //Party member to set as favorite Pokemon
                    break;
                case 0x11E:
                    com.Name = "BadgeAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x11F:
                    com.Name = "StorePokèmonPartyNumberBadge";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x120:
                    com.Name = "SetVarPokèmonTrade";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x121:
                    com.Name = "CheckPartyAmount";
                    com.parameters.Add(reader.ReadUInt16()); //Result Storage Variable(Boolean)
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    break;
                case 0x122:
                    com.Name = "122";
                    break;
                case 0x123:
                    com.Name = "123";
                    break;
                case 0x124:
                    com.Name = "124";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x125:
                    com.Name = "125";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x126:
                    com.Name = "126";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x127:
                    com.Name = "127";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());//Ow
                    com.parameters.Add(reader.ReadUInt16());//X
                    com.parameters.Add(reader.ReadUInt16());//Y
                    break;
                case 0x128:
                    com.Name = "128";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x129:
                    com.Name = "129";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x12A:
                    com.Name = "12A";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x12B:
                    com.Name = "12B";
                    com.parameters.Add(reader.ReadUInt16()); //Req
                    com.parameters.Add(reader.ReadUInt16()); //Req
                    com.parameters.Add(reader.ReadUInt16()); //Req
                    break;
                case 0x12C:
                    com.Name = "12C";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x12D:
                    com.Name = "12D";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //0
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x12E:
                    com.Name = "12E";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x12F:
                    com.Name = "12F";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x130:
                    com.Name = "BootPCSound";
                    break;
                case 0x131:
                    com.Name = "PC-131";
                    break;
                case 0x132:
                    com.Name = "132";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x134:
                    com.Name = "134";
                    break;
                case 0x135:
                    com.Name = "135";
                    break;
                case 0x136:
                    com.Name = "136";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x137:
                    com.Name = "ShowClockSaving";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x138:
                    com.Name = "StoreSaveData";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x139:
                    com.Name = "SetComunication";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x13A:
                    com.Name = "StoreComunicationStatus";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x13B:
                    com.Name = "CheckWireless";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x13C:
                    com.Name = "13C";
                    break;
                case 0x13D:
                    com.Name = "13D";
                    break;
                case 0x13E:
                    com.Name = "13E";
                    break;
                case 0x13F:
                    com.Name = "StartCameraEvent";
                    break;
                case 0x140:
                    com.Name = "StopCameraEvent";
                    break;
                case 0x141:
                    com.Name = "LockCamera";
                    break;
                case 0x142:
                    com.Name = "ReleaseCamera";
                    break;
                case 0x143:
                    com.Name = "MoveCamera";
                    com.parameters.Add(reader.ReadUInt16()); //Elevation
                    com.parameters.Add(reader.ReadUInt16()); //Rotation
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //Zoom
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x144:
                    com.Name = "144";
                    com.parameters.Add(reader.ReadUInt16()); //Elevation
                    break;
                case 0x145:
                    com.Name = "EndCameraEvent";
                    break;
                case 0x146:
                    com.Name = "146";
                    break;
                case 0x147:
                    com.Name = "ResetCamera";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x148:
                    com.Name = "BumpingCamera";
                    com.parameters.Add(reader.ReadUInt16()); //Intensità X
                    com.parameters.Add(reader.ReadUInt16()); //Intensità Y
                    com.parameters.Add(reader.ReadUInt16()); //Speed
                    com.parameters.Add(reader.ReadUInt16()); //Degradation
                    com.parameters.Add(reader.ReadUInt16()); //Changing Speed (From Bumping X to Y)
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x149:
                    com.Name = "149";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x14A:
                    com.Name = "CallEnd";
                    break;
                case 0x14B:
                    com.Name = "CallStart";
                    break;
                case 0x14D:
                    com.Name = "14D";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x14E:
                    com.Name = "ChooseInterestingItem";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x14F:
                    com.Name = "CallPcFunction";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x150:
                    com.Name = "150";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x151:
                    com.Name = "ShowDiploma";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x152:
                    com.Name = "152";
                    break;
                case 0x153:
                    com.Name = "153";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x154:
                    com.Name = "LibertyShipAnm";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x155:
                    com.Name = "OpenInterpokè";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x156:
                    com.Name = "156";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x157:
                    com.Name = "157";
                    break;
                case 0x158:
                    com.Name = "158";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x159:
                    com.Name = "159";
                    break;
                case 0x15A:
                    com.Name = "15A";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x15B:
                    com.Name = "15B";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x15C:
                    com.Name = "15C";
                    break;
                case 0x15D:
                    com.Name = "15D";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x15E:
                    com.Name = "15E";
                    break;
                case 0x15F:
                    com.Name = "CheckFriend";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x160:
                    com.Name = "160";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x161:
                    com.Name = "161";
                    break;
                case 0x162:
                    com.Name = "162";
                    break;
                //case 0x163:
                //    com.Name = "163";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                case 0x164:
                    com.Name = "164";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x165:
                    com.Name = "165";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x166:
                    com.Name = "166";
                    break;
                case 0x167:
                    com.Name = "StartPokèmonMusical";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x168:
                    com.Name = "StartDressPokèmonMusical";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x169:
                    com.Name = "CheckPokèmonMusicalFunctions";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x16A:
                    com.Name = "StoreStatusPokèmonMusical";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x16B:
                    com.Name = "PokèmonMenuMusicalFunctions";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x16C:
                    com.Name = "16C";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x16D:
                    com.Name = "16D";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x16E:
                    com.Name = "ChoosePokèmonMusical";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x16F:
                    com.Name = "16F";
                    break;
                case 0x170:
                    com.Name = "170";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x171:
                    com.Name = "171";
                    break;
                case 0x172:
                    com.Name = "172";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x173:
                    com.Name = "173";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x174:
                    com.Name = "174";
                    com.parameters.Add(reader.ReadUInt16()); //Dex
                    com.parameters.Add(reader.ReadUInt16()); //Level
                    com.parameters.Add(reader.ReadUInt16()); //Unk
                    break;
                case 0x175:
                    com.Name = "175";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x176:
                    com.Name = "176";
                    com.parameters.Add(reader.ReadUInt16());
                    if (scriptType == BW2SCRIPT)
                        com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x177:
                    com.Name = "177";
                    if (scriptType == BW2SCRIPT)
                        com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x178:
                    com.Name = "WildPokèmonBattle";	     // 364	0=captured, might output 1 & 2 for something else
                    com.parameters.Add(reader.ReadUInt16()); //variable to store result to
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x179:
                    com.Name = "EndWildBattle";
                    break;
                case 0x17A:
                    com.Name = "LooseWildBattle";
                    break;
                case 0x17B:
                    com.Name = "StoreWildBattleResult";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x17C:
                    com.Name = "StoreWildBattlePokèmonStatus";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x17D:
                    com.Name = "17D";
                    break;
                case 0x17E:
                    com.Name = "17E";
                    break;
                case 0x17F:
                    com.Name = "17F";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x180:
                    com.Name = "NimbasaGymRailAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x181:
                    com.Name = "181";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x182:
                    com.Name = "182";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x185:
                    com.Name = "185";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x186:
                    com.Name = "186";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x187:
                    com.Name = "187";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x188:
                    com.Name = "188";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x189:
                    com.Name = "189";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x18A:
                    com.Name = "18A";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x18B:
                    com.Name = "18B";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x18C:
                    com.Name = "18C";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x18D:
                    com.Name = "18D";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x18E:
                    com.Name = "18E";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x18F:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x190:
                    com.Name = "DriftGymLiftAnmSecondRoom";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x191:
                    com.Name = "191";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x192:
                    com.Name = "DriftGymLiftAnmFirstRoom";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x193:
                    com.Name = "193";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x194:
                    com.Name = "194";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x196:
                    com.Name = "196";
                    break;
                case 0x197:
                    com.Name = "197";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x198:
                    com.Name = "198";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x199:
                    com.Name = "199";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19A:
                    com.Name = "19A";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19B:
                    com.Name = "SetStatusCG";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19C:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19D:
                    com.Name = "19D";
                    break;
                case 0x19E:
                    com.Name = "ShowCG";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19F:
                    com.Name = "CallScreenAnimation";
                    com.parameters.Add(reader.ReadUInt16()); //AnimationId
                    break;
                case 0x1A0:
                    com.Name = "1A0";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1A1:
                    com.Name = "OpenXtransciever(1A1)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1A2:
                    com.Name = "1A2";
                    break;
                case 0x1A3:
                    com.Name = "FlashBlackInstant";
                    break;
                case 0x1A4:
                    com.Name = "1A4";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1A5:
                    com.Name = "1A5";
                    break;
                case 0x1A6:
                    com.Name = "1A6";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1A7:
                    com.Name = "1A7";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1A8:
                    com.Name = "1A8";
                    break;
                case 0x1A9:
                    com.Name = "1A9";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1AA:
                    com.Name = "1AA";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1AB:
                    com.Name = "FadeFromBlack";
                    break;
                case 0x1AC:
                    com.Name = "FadeIntoBlack";
                    break;
                case 0x1AD:
                    com.Name = "FadeFromWhite";
                    break;
                case 0x1AE:
                    com.Name = "FadeIntoWhite";
                    break;
                case 0x1AF:
                    com.Name = "1AF";
                    break;
                case 0x1B1:
                    com.Name = "ScreenFunction";
                    break;
                case 0x1B2:
                    com.Name = "1B2";
                    break;
                case 0x1B4:
                    com.Name = "1B4";
                    break;
                case 0x1B5:
                    com.Name = "1B5";
                    break;
                case 0x1B6:
                    com.Name = "1B6";
                    break;
                case 0x1B9:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1BA:
                    com.Name = "1BA";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1BB:
                    com.Name = "1BB";
                    break;
                case 0x1BC:
                    com.Name = "1BC";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1BD:
                    com.Name = "1BD";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1BE:
                    com.Name = "TradePokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1BF:
                    com.Name = "CompareChosenPokemon";
                    com.parameters.Add(reader.ReadUInt16());//RV = True if is equal
                    com.parameters.Add(reader.ReadUInt16());//Id chosen Pokèmon
                    com.parameters.Add(reader.ReadUInt16());//Id requested Pokèmon
                    break;
                case 0x1C0:
                    com.Name = "1C0";
                    break;
                case 0x1C1:
                    com.Name = "1C1";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1C2:
                    com.Name = "StartEventBC";
                    break;
                case 0x1C3:
                    com.Name = "EndEventBC";
                    break;
                case 0x1C4:
                    com.Name = "StoreTrainerID";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1C5:
                    com.Name = "1C5";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1C6:
                    com.Name = "StorePokemonCaughtWF";
                    com.parameters.Add(reader.ReadUInt16()); //True if is Pokèmon searched
                    com.parameters.Add(reader.ReadUInt16()); //True if is caught the same day
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1C7:
                    com.Name = "1C7";
                    break;
                case 0x1C8:
                    com.Name = "1C8";
                    break;
                case 0x1C9:
                    com.Name = "StoreVarMessage";
                    com.parameters.Add(reader.ReadUInt16()); //Variable as Container
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    break;
                case 0x1CB:
                    com.Name = "1CB";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1CC:
                    com.Name = "1CC";
                    break;
                case 0x1CD:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1CE:
                    com.Name = "CheckPokèdexStatus";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1CF:
                    com.Name = "StorePokèdexCaught";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1D0:
                    com.Name = "1D0";
                    break;
                case 0x1D1:
                    com.Name = "1D1";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1D2:
                    com.Name = "1D2";
                    break;
                case 0x1D3:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1D4:
                    break;
                case 0x1D5:
                    com.Name = "1D5";
                    break;
                case 0x1D6:
                    com.Name = "AffinityCheck";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1D7:
                    com.Name = "SetVarAffinityCheck";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1D8:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1D9:
                    com.Name = "1D9";
                    com.parameters.Add(reader.ReadUInt16());
                    if (scriptType == BW2SCRIPT)
                        com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1DA:
                    com.Name = "1DA";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1DB:
                    com.Name = "1DB";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1DC:
                    com.Name = "1DC";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1DD:
                    com.Name = "1DD";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1DE:
                    com.Name = "StoreDataUnity";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1DF:
                    com.Name = "1DF";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E0:
                    com.Name = "ChooseUnityFloor";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E1:
                    com.Name = "StoreTrainerUnity";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E2:
                    com.Name = "1E2";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E3:
                    com.Name = "StoreUnityActivities";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E4:
                    com.Name = "StoreCanTeachDragonMove";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E5:
                    com.Name = "StorePokèmonStatusDragonMove";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E6:
                    com.Name = "1E6";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E7:
                    com.Name = "StoreChosenPokèmonDragonMove";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E8:
                    com.Name = "CheckRememberMove";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E9:
                    com.Name = "StoreRememberMove";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1EA:
                    com.Name = "1EA";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1EB:
                    com.Name = "1EB";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1EC:
                    com.Name = "SwitchOwPosition";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16()); //X Coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y Coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Z Coordinate
                    break;
                case 0x1ED:
                    com.Name = "DoublePhraseBoxInput";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1EE:
                    com.Name = "1EE";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1EF:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F0:
                    com.Name = "HMEffect";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F1:
                    com.Name = "1F1";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F2:
                    com.Name = "1F2";
                    break;
                case 0x1F3:
                    com.Name = "CreateStadiumTrainer?";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F4:
                    com.Name = "StartStadiumFunction?";
                    break;
                case 0x1F5:
                    com.Name = "EndStadiumFunction?";
                    break;
                case 0x1F6:
                    com.Name = "CreateStadiumOverworld?";
                    com.parameters.Add(reader.ReadUInt16()); // 0
                    com.parameters.Add(reader.ReadUInt16()); // 0
                    com.parameters.Add(reader.ReadUInt16()); // 0
                    break;
                case 0x1F7:
                    com.Name = "1F7";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F8:
                    com.Name = "SetStadiumBusy";
                    break;
                case 0x1F9:
                    com.Name = "StartBattleExam";
                    break;
                case 0x1FA:
                    com.Name = "SetBattleExamTrainerNumber";
                    com.parameters.Add(reader.ReadUInt16()); //Trainer Number
                    break;
                case 0x1FB:
                    com.Name = "SetBattleExamType";
                    com.parameters.Add(reader.ReadUInt16()); //Type
                    break;
                case 0x1FC:
                    com.Name = "StoreBattleExamModality";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1FD:
                    com.Name = "StoreBattleExamSprite";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16()); //Sprite(ret)
                    break;
                case 0x1FE:
                    com.Name = "ExamBattle";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    break;
                case 0x1FF:
                    com.Name = "EndExamBattle";
                    break;
                case 0x200:
                    com.Name = "CheckBattleExamStarted";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x202:
                    com.Name = "StoreBattleExamStarNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x203:
                    com.Name = "CheckBattleExamAvailable";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x204:
                    com.Name = "StoreBattleExamLevel";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x205:
                    com.Name = "StoreBattleExamType";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x206:
                    com.Name = "SavingBookAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x207:
                    com.Name = "ShowBattleExamResult";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x208:
                    com.Name = "StoreBattleExamWon";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x209:
                    com.Name = "DreamWorldFunction";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x20A:
                    com.Name = "DreamWorldFunction2";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x20B:
                    com.Name = "ShowDreamWorldFurniture";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x20C:
                    com.Name = "CheckRelocatorPassword";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x20D:
                    com.Name = "20D";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x20E:
                    com.Name = "CheckItemInterestingBag";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x20F:
                    com.Name = "CompareInterestingItem";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x210:
                    com.Name = "210";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x211:
                    com.Name = "211";
                    break;
                case 0x212:
                    com.Name = "StoreSurveyActive";
                    com.parameters.Add(reader.ReadUInt16()); //Survey Id
                    break;
                case 0x213:
                    com.Name = "213";
                    com.parameters.Add(reader.ReadUInt16()); //(ret)
                    com.parameters.Add(reader.ReadUInt16()); //0
                    com.parameters.Add(reader.ReadUInt16()); //0
                    break;
                case 0x214:
                    com.Name = "214";
                    com.parameters.Add(reader.ReadUInt16()); //
                    com.parameters.Add(reader.ReadUInt16()); //0
                    break;
                case 0x215:
                    com.Name = "215";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //(ret)
                    break;
                case 0x216:
                    com.Name = "216";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x217:
                    com.Name = "217";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x218:
                    com.Name = "StoreGreeting";
                    com.parameters.Add(reader.ReadUInt16());//val
                    break;
                case 0x219:
                    com.Name = "StoreThanks";
                    com.parameters.Add(reader.ReadUInt16());//var
                    break;
                case 0x21C:
                    com.Name = "21C";
                    com.parameters.Add(reader.ReadUInt16());//var
                    com.parameters.Add(reader.ReadUInt16());//val
                    break;
                case 0x21D:
                    com.Name = "21D";
                    break;
                case 0x21E:
                    com.Name = "StoreSurveyDone";
                    com.parameters.Add(reader.ReadUInt16()); //Survey Number
                    break;
                case 0x21F:
                    com.Name = "21F";
                    break;
                case 0x220:
                    com.Name = "CheckHavePokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x221:
                    com.Name = "221";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());//var
                    break;
                case 0x222:
                    com.Name = "222";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x223:
                    com.Name = "StoreHiddenPowerType";			// ex 382
                    com.parameters.Add(reader.ReadUInt16()); //Storage for result (0-17 move type)
                    nextFF = reader.ReadUInt16();
                    if (nextFF > 10)
                        reader.BaseStream.Position -= 2;
                    else
                        com.parameters.Add(reader.ReadUInt16()); //Party member to Store
                    break;
                case 0x224:
                    com.Name = "224";
                    com.parameters.Add(reader.ReadUInt16());//var
                    break;
                case 0x225:
                    com.Name = "225";
                    com.parameters.Add(reader.ReadUInt16()); //Storage for result (0-17 move type)
                    com.parameters.Add(reader.ReadUInt16()); //Party member to Store
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x226:
                    com.Name = "RotatingAnimation";
                    com.parameters.Add(reader.ReadUInt16()); //OW Id
                    break;
                case 0x227:
                    com.Name = "ShowPokèmonPicture";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x228:
                    com.Name = "228";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x229:
                    com.Name = "229";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22A:
                    com.Name = "TeleportDreamForest";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22B:
                    com.Name = "CheckDreamFunction";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22C:
                    com.Name = "CheckSpacePokèmonDream";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22D:
                    com.Name = "SetVarPokèmonDream";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22E:
                    com.Name = "StartDreamIsle";
                    break;
                case 0x22F:
                    com.Name = "DreamBattle";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x230:
                    com.Name = "GiveDreamPokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x231:
                    com.Name = "StoreFishPokèmon";
                    com.parameters.Add(reader.ReadUInt16()); // Pokèmon to catch(Ret)
                    break;
                case 0x232:
                    com.Name = "232";
                    com.parameters.Add(reader.ReadUInt16()); // Var
                    com.parameters.Add(reader.ReadUInt16()); // Var(Ret)
                    break;
                case 0x233:
                    com.Name = "StoreTrainerFromSeason"; //?
                    com.parameters.Add(reader.ReadUInt16()); // Season
                    com.parameters.Add(reader.ReadUInt16()); // Trainer Id
                    break;
                case 0x234:
                    com.Name = "234";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x235:
                    com.Name = "235";
                    com.parameters.Add(reader.ReadUInt16()); //var
                    break;
                case 0x236:
                    com.Name = "236";
                    com.parameters.Add(reader.ReadUInt16()); //var
                    com.parameters.Add(reader.ReadUInt16()); //val
                    com.parameters.Add(reader.ReadUInt16()); //val
                    com.parameters.Add(reader.ReadUInt16()); //val
                    break;
                case 0x237:
                    com.Name = "237";
                    com.parameters.Add(reader.ReadUInt16()); //var
                    com.parameters.Add(reader.ReadUInt16()); //val
                    com.parameters.Add(reader.ReadUInt16()); //val
                    break;
                case 0x238:
                    com.Name = "238";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x239:
                    com.Name = "239";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x23A:
                    com.Name = "Animation(23A)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x23B:
                    com.Name = "CheckSendSaveCG";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //RV = Message Id
                    break;
                case 0x23C:
                    com.Name = "23C";
                    break;
                case 0x23D:
                    com.Name = "23D";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //RV = Message Id
                    break;
                case 0x23E:
                    com.Name = "23E"; //Lock
                    com.parameters.Add(reader.ReadUInt16()); //var
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //var
                    com.parameters.Add(reader.ReadUInt16()); //var
                    break;
                case 0x23F:
                    com.Name = "23F"; //Freeze
                    break;
                case 0x240:
                    com.Name = "240";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x241:
                    com.Name = "241";
                    break;
                case 0x242:
                    com.Name = "242";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x243:
                    com.Name = "SpecialMessage";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x244:
                    com.Name = "OpenHelpSystem";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x245:
                    com.Name = "245";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x246:
                    com.Name = "246";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x247:
                    com.Name = "Warp(247)";
                    com.parameters.Add(reader.ReadUInt16()); //Ow
                    com.parameters.Add(reader.ReadUInt16()); //X
                    com.parameters.Add(reader.ReadUInt16()); //Y
                    com.parameters.Add(reader.ReadUInt16()); //Z
                    com.parameters.Add(reader.ReadUInt16()); //Orientation
                    break;
                case 0x248:
                    com.Name = "248";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x249:
                    com.Name = "StoreInterestingItemData";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24A:
                    com.Name = "TakeInterestingItem";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24B:
                    com.Name = "24B";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24C:
                    com.Name = "24C";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24D:
                    com.Name = "Cry(24D)";
                    com.parameters.Add(reader.ReadUInt16()); //Cry
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24E:
                    com.Name = "24E"; //Lock
                    break;
                case 0x24F:
                    com.Name = "24F"; //Lock
                    break;
                case 0x250:
                    com.Name = "250"; //Freeze
                    break;
                case 0x251:
                    com.Name = "251";
                    com.parameters.Add(reader.ReadUInt16()); //(Ret)
                    com.parameters.Add(reader.ReadUInt16()); //0
                    com.parameters.Add(reader.ReadUInt16()); //Var
                    break;
                case 0x252:
                    com.Name = "ShowMapName";
                    break;
                case 0x253:
                    com.Name = "ChangeMusicVolume(253)";
                    com.parameters.Add(reader.ReadUInt16()); //Volume
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x254:
                    com.Name = "254";
                    com.parameters.Add(reader.ReadUInt16()); //1 60
                    break;
                case 0x255: //Freeze
                    com.Name = "255";
                    com.parameters.Add(reader.ReadUInt16()); //0
                    break;
                case 0x257: //Freeze
                    com.Name = "257";
                    break;
                case 0x259:
                    com.Name = "StopMusic(259)";
                    break;
                case 0x25A:
                    com.Name = "ShipAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x25B:
                    com.Name = "25B";
                    com.parameters.Add(reader.ReadUInt16()); //0 1
                    break;
                case 0x25C:
                    com.Name = "25C"; //Used in stadium
                    com.parameters.Add(reader.ReadUInt16()); //var (0x4XXX)
                    com.parameters.Add(reader.ReadUInt16()); //var (0x4XXX)
                    com.parameters.Add(reader.ReadUInt16()); //var (0x4XXX)
                    com.parameters.Add(reader.ReadUInt16()); //var (0x4XXX)
                    com.parameters.Add(reader.ReadUInt16()); //var (0x4XXX)
                    com.parameters.Add(reader.ReadUInt16()); //var (0x4XXX)
                    break;
                case 0x25D:
                    com.Name = "CheckCGearActive";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x25E:
                    com.Name = "25E";
                    break;
                case 0x25F:
                    com.Name = "PlayTheatralMusic";
                    break;
                case 0x260: //126
                    com.Name = "260";
                    break;
                default:
                    com.Name = "0x" + com.Id.ToString("X");
                    break;

            }
            return com;
        }

        private void GenerateScriptList()
        {
            for (int narcCounter = 0; narcCounter < actualNarc.fatbNum; narcCounter++)
                listScript.Items.Add(narcCounter);
        }

        private static void UpdateType()
        {
            IsBWDialog checkGame = new IsBWDialog();
            checkGame.ShowDialog();
            scriptType = checkGame.CheckGame();
        }

        public void printScripts(List<Script_s> scriptList, RichTextBox scriptBox, ComboBox subScripts)
        {
            scriptBox.Clear();
            subScripts.Items.Clear();
            functionOffsetList.Sort();
            movOffsetList.Sort();
            Console.AppendText("\nThere are " + scriptList.Count + " script and " + functionList.Count + " function and " + movList.Count + " movement");
            scriptBox.AppendText("=== File: " + actualFileId);
            lastIndexFunction = 0;
            lastIndexMov = 0;
            int scriptNum = 0;

            foreach (Script_s script in scriptList)
            {

                scriptBox.AppendText("\n=== Script " + script.idScript + " === \n\n");
                subScripts.Items.Add(script.idScript);
                if (script.commands != null)
                {
                    Console.AppendText("\nPrinting script: " + (script.idScript + 1));
                    printCommand(script, scriptBox, true);
                    int functionCounter = 0;
                    for (functionCounter = lastIndexFunction; functionCounter < functionList.Count; functionCounter++)
                    {
                        var actualFunc = functionList[functionCounter];
                        if (actualFunc.commands[0].offset >= script.scriptStart && actualFunc.commands[0].offset <= script.scriptEnd)
                        {
                            Console.AppendText("\nPrinting function: " + functionCounter);
                            scriptBox.AppendText("\n=== Function " + functionCounter + " === \n\n");
                            printCommand(actualFunc, scriptBox, false);
                            lastIndexFunction++;
                        }
                        else
                        {
                            goto End3;
                        }
                    }
                End3:
                    for (int movCounter = lastIndexMov; movCounter < movList.Count - 1; movCounter++)
                    {
                        var actualMov = movList[movCounter];
                        var position = actualMov[0].offset;
                        if (actualMov[0].offset >= script.scriptStart && actualMov[0].offset <= script.scriptEnd)
                        {
                            Console.AppendText("\nPrinting mov: " + movCounter);
                            scriptBox.AppendText("\n=== Movement " + movCounter + " === \n\n");
                            for (int i = 0; i < actualMov.Count; i++)
                                scriptBox.AppendText("Offset: " + actualMov[i].offset.ToString() + " m" + actualMov[i].moveId.ToString() + " 0x" + actualMov[i].repeatTime.ToString() + "\n");
                            lastIndexMov++;
                        }
                        else
                        {
                            goto End2;
                        }
                    }
                End2: ;
                    scriptNum++;

                }
            }
        }

        public void printSimplifiedScript(RichTextBox scriptBoxEditor, RichTextBox scriptBoxViewer, int scriptType, string item)
        {

            scriptBoxEditor.Clear();

            visitedLine.Clear();

            var scriptsLine = scriptBoxViewer.Lines;
            for (int lineCounter = 0; lineCounter < scriptsLine.Length; lineCounter++)
            {
                var line = scriptsLine[lineCounter];
                if (line.Contains("= Script " + item + " "))
                {
                    scriptBoxEditor.AppendText("\n" + line + "\n\n");
                    varNameDictionaryList = new List<Dictionary<int, string>>();
                    varNameDictionaryList.Add(new Dictionary<int, string>());
                    lineCounter++;
                    getCommands(scriptsLine, ref lineCounter, ref line, " ", visitedLine, scriptBoxEditor, scriptType);
                    scriptBoxEditor.AppendText("\n");
                }
            }
        }

        private Commands_s readCommandBW2(BinaryReader reader, Commands_s com)
        {
            uint functionOffset = 0;
            reader.BaseStream.Position += 1;
            var actualPos = reader.BaseStream.Position;
            if (scriptStartList.Contains((uint)actualPos) || functionOffsetList.Contains((uint)actualPos))
            {
                com.Name = "WrongEnd";
                com.isEnd = 1;
                reader.BaseStream.Position -= 1;
                return com;
            }
            reader.BaseStream.Position -= 1;
            if (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                actualPos = reader.BaseStream.Position;
                if (actualPos == 368)
                {
                }
                if (actualPos + 1 == reader.BaseStream.Length || movOffsetList.Contains((uint)actualPos) || scriptStartList.Contains((uint)actualPos) || functionOffsetList.Contains((uint)actualPos))
                {
                    com.isEnd = 1;
                }
            }
            switch (com.Id)
            {
                case 0x2:
                    com.Name = "End";
                    if (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        var next = reader.ReadByte();
                        if (next == 0)
                            com.isEnd = 1;
                        else
                            reader.BaseStream.Position -= 1;
                    }
                    break;
                case 0x03:
                    com.Name = "ReturnAfterDelay";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x04:
                    com.Name = "CallRoutine";
                    if (reader.BaseStream.Position < reader.BaseStream.Length - 4)
                    {
                        com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                        functionOffset = com.parameters[0];
                        if (!scriptStartList.Contains(functionOffset) && !functionOffsetList.Contains(functionOffset) && !movOffsetList.Contains(functionOffset))
                        {
                            functionOffsetList.Add(functionOffset);
                            Console.AppendText("\nA function is in: " + functionOffset.ToString());
                        }
                    }
                    //com.numJump++;
                    ////com.isEnd = 1;
                    break;
                case 0x05:
                    com.Name = "EndFunction";
                    if (reader.BaseStream.Position + 1 > reader.BaseStream.Length) { com.isEnd = 1; break; }
                    var next5 = reader.ReadByte();
                    if (next5 == 0 || movOffsetList.Contains((uint)reader.BaseStream.Position - 1) || scriptOffList.Contains((uint)reader.BaseStream.Position - 1))
                    {
                        com.isEnd = 1;
                        reader.BaseStream.Position -= 1;
                        break;
                    }
                    else
                        reader.BaseStream.Position -= 1;
                    break;
                case 0x06:
                    com.Name = "SetVar(06)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x07:
                    com.Name = "SetVar(07)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x08:
                    com.Name = "CompareTo";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x09:
                    com.Name = "SetVar(09)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x0A:
                    com.Name = "ClearVar";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x0B:
                    com.Name = "0B";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x0C:
                //    com.Name = "0C";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x0D:
                //    com.Name = "0D";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x0E:
                //    com.Name = "0E";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x0F:
                //    com.Name = "0F";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x10:
                    com.Name = "StoreFlag";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x11:
                    com.Name = "Condition";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x13:
                    com.Name = "StoreVar(13)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x14:
                    com.Name = "14";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x16:
                    com.Name = "16";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x17:
                    com.Name = "17";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19:
                    com.Name = "Compare";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1C:
                    com.Name = "CallStd";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());//Standard Function Id.
                    break;
                case 0x1D:
                    com.Name = "ReturnStd";
                    break;
                case 0x1E:
                    com.Name = "Jump";
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[0];
                    if (!scriptStartList.Contains(functionOffset) && !functionOffsetList.Contains(functionOffset) && !movOffsetList.Contains(functionOffset))
                    {
                        functionOffsetList.Add(functionOffset);
                        Console.AppendText("\nA function is in: " + functionOffset.ToString());
                    }
                    com.numJump++;
                    com.isEnd = 1;
                    break;
                case 0x1F:
                    com.Name = "If";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[1];
                    if (!scriptStartList.Contains(functionOffset) && !functionOffsetList.Contains(functionOffset) && !movOffsetList.Contains(functionOffset))
                    {
                        functionOffsetList.Add(functionOffset);
                        Console.AppendText("\nA function is in: " + functionOffset.ToString());
                    }
                    com.numJump++;
                    break;
                case 0x21:
                    com.Name = "21";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22:
                    com.Name = "22";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x23:
                    com.Name = "SetFlag";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24:
                    com.Name = "ClearFlag";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x25:
                    com.Name = "StoreVarFlag";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x26:
                    com.Name = "StoreAddVar";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x27:
                    com.Name = "StoreSubVar";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x28:
                    com.Name = "StoreVarValue";
                    com.parameters.Add(reader.ReadUInt16()); //Var as container
                    com.parameters.Add(reader.ReadUInt16()); //Value to store
                    break;
                case 0x29:
                    com.Name = "StoreVarVariable";
                    com.parameters.Add(reader.ReadUInt16()); //Var as container
                    com.parameters.Add(reader.ReadUInt16()); //Value to store
                    break;
                case 0x2A:
                    com.Name = "StoreVarCallable";
                    com.parameters.Add(reader.ReadUInt16()); //Var as container
                    com.parameters.Add(reader.ReadUInt16()); //Value to store
                    break;
                case 0x2B:
                    com.Name = "StoreVar(2B)";
                    com.parameters.Add(reader.ReadUInt16()); //Var as container
                    com.parameters.Add(reader.ReadUInt16()); //Value to store
                    break;
                case 0x2D:
                    com.Name = "ClearVar(2D)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2E:
                    com.Name = "LockAll";
                    break;
                case 0x2F:
                    com.Name = "UnlockAll";
                    break;
                case 0x30:
                    com.Name = "WaitMoment";
                    break;
                case 0x31:
                    com.Name = "31";
                    break;
                case 0x32:
                    com.Name = "WaitButton";
                    break;
                case 0x33:
                    com.Name = "MusicalMessage";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    break;
                case 0x34:
                    com.Name = "EventGreyMessage";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    break;
                case 0x35:
                    com.Name = "MusicalMessage2";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    break;
                case 0x36:
                    com.Name = "CloseEventGreyMessage";
                    break;
                case 0x37:
                    com.Name = "37";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x38:
                    com.Name = "BubbleMessage";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadByte()); //Bottom/Top View.
                    break;
                case 0x39:
                    com.Name = "CloseBubbleMessage";
                    break;
                case 0x3A:
                    com.Name = "ShowMessageAt";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //X coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate
                    break;
                case 0x3B:
                    com.Name = "CloseShowMessageAt";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x3C:
                    com.Name = "Message";
                    com.parameters.Add(reader.ReadByte()); //Costant
                    com.parameters.Add(reader.ReadByte()); //Costant
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    if (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                        com.parameters.Add(reader.ReadUInt16()); //Message Type
                    }
                    break;
                case 0x3D:
                    com.Name = "Message2";
                    com.parameters.Add(reader.ReadByte()); //Costant
                    com.parameters.Add(reader.ReadByte()); //Costant
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    com.parameters.Add(reader.ReadUInt16()); //Message Type
                    break;
                case 0x3E:
                    com.Name = "CloseMessageKeyPress";
                    break;
                case 0x3F:
                    com.Name = "CloseMessageKeyPress2";
                    break;
                case 0x40:
                    com.Name = "MoneyBox";
                    com.parameters.Add(reader.ReadUInt16()); //X coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate
                    break;
                case 0x41:
                    com.Name = "CloseMoneyBox";
                    break;
                case 0x42:
                    com.Name = "UpdateMoneyBox";
                    break;
                case 0x43:
                    com.Name = "BorderedMessage";
                    com.parameters.Add(reader.ReadUInt16()); //MessageId
                    com.parameters.Add(reader.ReadUInt16()); //Color
                    break;
                case 0x44:
                    com.Name = "CloseBorderedMessage";
                    break;
                case 0x45:
                    com.Name = "PaperMessage";
                    com.parameters.Add(reader.ReadUInt16()); //MessageId
                    com.parameters.Add(reader.ReadUInt16()); //Trans. Coordinate
                    break;
                case 0x46:
                    com.Name = "ClosePaperMessage";
                    break;
                case 0x47:
                    com.Name = "YesNoBox";
                    com.parameters.Add(reader.ReadUInt16()); //Variable: NO = 0, YES = 1
                    break;
                case 0x48:
                    com.Name = "Message3";
                    com.parameters.Add(reader.ReadByte()); //Costant
                    com.parameters.Add(reader.ReadByte()); //Costant
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id 
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    com.parameters.Add(reader.ReadUInt16()); //Message Type
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x49:
                    com.Name = "DoubleMessage";
                    com.parameters.Add(reader.ReadByte()); //Costant
                    com.parameters.Add(reader.ReadByte()); //Costant
                    com.parameters.Add(reader.ReadUInt16()); //Id Message Black
                    com.parameters.Add(reader.ReadUInt16()); //Id Message White
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id 
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    com.parameters.Add(reader.ReadUInt16()); //Message Type
                    break;
                case 0x4A:
                    com.Name = "AngryMessage";
                    com.parameters.Add(reader.ReadUInt16()); //MessageId
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    break;
                case 0x4B:
                    com.Name = "CloseAngryMessage";
                    break;
                case 0x4C:
                    com.Name = "SetVarHero";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x4D:
                    com.Name = "SetVarItem";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x4E:
                    com.Name = "SetVarItemNumber";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x4F:
                    com.Name = "SetVarItemContainter";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x50:
                    com.Name = "SetVarItemContained";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x51:
                    com.Name = "SetVarMove";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x52:
                    com.Name = "SetVarBag";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x53:
                    com.Name = "SetVarPartyPokèmon";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x54:
                    com.Name = "SetVarPartyPokèmonNick";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x55:
                    com.Name = "SetVar(55)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x56:
                    com.Name = "SetVarType";		     //382
                    com.parameters.Add(reader.ReadByte());   //text variable to set
                    com.parameters.Add(reader.ReadUInt16()); //type to set
                    break;
                case 0x57:
                    com.Name = "SetVarPokèmon";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x58:
                    com.Name = "SetVarPokèmon2";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x59:
                    com.Name = "SetVarLocation";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x5A:
                    com.Name = "SetVarPokèmonNick";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x5B:
                    com.Name = "SetVar(58)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x5C: // example 654
                    com.Name = "SetVarNumberCond";
                    com.parameters.Add(reader.ReadByte()); // 1 ?
                    com.parameters.Add(reader.ReadUInt16()); // Container to store to
                    com.parameters.Add(reader.ReadUInt16()); // Stat to Store [HP ATK DEF SPE SPA SPD, 0-5]
                    break;
                case 0x5D:
                    com.Name = "SetVarMusicalInfo";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x5E:
                    com.Name = "SetVarNations";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x5F:
                    com.Name = "SetVarActivities";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x60:
                    com.Name = "SetVarPower";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x61:
                    com.Name = "SetVarTrainerType";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x62:
                    com.Name = "SetVarTrainerType2";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x63:
                    com.Name = "SetVarGeneralWord";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x64:
                    com.Name = "ApplyMovement";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    var movOffset = com.parameters[1];
                    if (!movOffsetList.Contains(movOffset))
                    {
                        movOffsetList.Add(movOffset);
                        Console.AppendText("\nA movement is in: " + movOffset.ToString());
                    }
                    break;
                case 0x65:
                    com.Name = "WaitMovement";
                    break;
                case 0x66:
                    com.Name = "66";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x67:
                    com.Name = "67";
                    com.parameters.Add(reader.ReadUInt16());
                    uint variable = 0;
                    do
                    {
                        variable = reader.ReadUInt16();
                        if (variable < 0x8000)
                            reader.BaseStream.Position -= 2;
                        else
                            com.parameters.Add(variable);
                    }
                    while (variable > 0x8000);

                    break;
                case 0x68:
                    com.Name = "StoreHeroPosition";
                    com.parameters.Add(reader.ReadUInt16()); // Variable as X container.
                    com.parameters.Add(reader.ReadUInt16()); // Variable as Y container.
                    break;
                case 0x69:
                    com.Name = "StoreNPCPosition";
                    com.parameters.Add(reader.ReadUInt16()); // NPC Id
                    com.parameters.Add(reader.ReadUInt16()); // Variable as X container.
                    com.parameters.Add(reader.ReadUInt16()); // Variable as Y container.
                    com.parameters.Add(reader.ReadUInt16()); // NPC Id
                    com.parameters.Add(reader.ReadUInt16()); // Variable as X container.
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x6A:
                    com.Name = "StoreNPCFlag";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16()); //Flag
                    break;
                case 0x6B:
                    com.Name = "AddNPC";
                    com.parameters.Add(reader.ReadUInt16()); //Npc Id
                    break;
                case 0x6C:
                    com.Name = "RemoveNPC";
                    com.parameters.Add(reader.ReadUInt16()); //Npc Id
                    break;
                case 0x6D:
                    com.Name = "SetOWPosition";
                    com.parameters.Add(reader.ReadUInt16()); //Npc Id
                    com.parameters.Add(reader.ReadUInt16()); //X coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Z coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Face direction
                    break;
                case 0x6E:
                    com.Name = "StoreHeroNPCOrientation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x6F:
                    com.Name = "6F";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x70:
                    com.Name = "70";
                    com.parameters.Add(reader.ReadUInt16()); //Ow_Id (Variable)
                    com.parameters.Add(reader.ReadUInt16()); //Return Value
                    com.parameters.Add(reader.ReadUInt16()); //X
                    com.parameters.Add(reader.ReadUInt16()); //S_854 = 0, S_855 = 3
                    com.parameters.Add(reader.ReadUInt16()); //Y
                    break;
                //case 0x71:
                //    com.Name = "71";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x72:
                //    com.Name = "72";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x73:
                    com.Name = "73";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x74:
                    com.Name = "FacePlayer";
                    break;
                case 0x75:
                    com.Name = "Release";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    break;
                case 0x76:
                    com.Name = "ReleaseAll";
                    break;
                case 0x77:
                    com.Name = "Lock";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    break;
                case 0x78:
                    com.Name = "CheckLock";
                    com.parameters.Add(reader.ReadUInt16()); //variable
                    break;
                case 0x79:
                    com.Name = "StoreNPCLevel";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x7B:
                    com.Name = "MoveNpctoCoordinates";
                    com.parameters.Add(reader.ReadUInt16()); //Npc Id
                    com.parameters.Add(reader.ReadUInt16()); //X coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Z coordinate
                    break;
                case 0x7C:
                    com.Name = "7C";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x7D:
                //    com.Name = "7D";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x7E:
                    com.Name = "TeleportUpNPc";
                    com.parameters.Add(reader.ReadUInt16()); //Npc Id
                    break;
                case 0x7F:
                    com.Name = "7F";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x80:
                    com.Name = "80";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x81:
                    com.Name = "81";
                    break;
                case 0x82:
                    com.Name = "82";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x83:
                    com.Name = "SetVar(83)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x84:
                    com.Name = "SetVar(84)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x85:
                    com.Name = "SingleTrainerBattle";
                    com.parameters.Add(reader.ReadUInt16()); //Trainer Id
                    com.parameters.Add(reader.ReadUInt16()); //2th Trainer Id (If 0x0 Single Battle)
                    com.parameters.Add(reader.ReadUInt16()); //win loss logic (0 standard, 1 loss=>win)
                    break;
                case 0x86:
                    com.Name = "DoubleTrainerBattle";
                    com.parameters.Add(reader.ReadUInt16()); //Ally
                    com.parameters.Add(reader.ReadUInt16()); //Opp1 Trainer Id
                    com.parameters.Add(reader.ReadUInt16()); //Opp2 Trainer Id
                    com.parameters.Add(reader.ReadUInt16()); //win loss logic (0 standard, 1 loss=>win)
                    break;
                case 0x87:
                    com.Name = "MessageBattle";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //Message Id?
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x88:
                    com.Name = "MessageBattle2";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x8A:
                    com.Name = "8A";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x8B:
                    com.Name = "PlayTrainerMusic";
                    com.parameters.Add(reader.ReadUInt16()); // music to play
                    break;
                case 0x8C:
                    com.Name = "EndBattle";
                    break;
                case 0x8D:
                    com.Name = "StoreBattleResult";
                    com.parameters.Add(reader.ReadUInt16()); //Variable as container.
                    break;
                case 0x8E:
                    com.Name = "DisableTrainer";
                    break;
                case 0x90:
                    com.Name = "90";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x92:
                    com.Name = "92";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x93:
                    com.Name = "93";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x94:
                    com.Name = "TrainerBattle";
                    com.parameters.Add(reader.ReadUInt16());//Trainer ID
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x95:
                    com.Name = "DeactiveTrainerId";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x96:
                    com.Name = "96";
                    com.parameters.Add(reader.ReadUInt16()); //Trainer ID
                    break;
                case 0x97:
                    com.Name = "StoreActiveTrainerId";
                    com.parameters.Add(reader.ReadUInt16()); //Trainer ID
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x98:
                    com.Name = "ChangeMusic";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x9E:
                    com.Name = "FadeToDefaultMusic";
                    break;
                case 0x9F:
                    com.Name = "PlayMusic";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xA1:
                    com.Name = "StopMusic";
                    com.parameters.Add(reader.ReadUInt16()); //sound?
                    break;
                case 0xA2:
                    com.Name = "A2";
                    com.parameters.Add(reader.ReadUInt16()); //sound?
                    com.parameters.Add(reader.ReadUInt16()); //strain
                    break;
                case 0xA3:
                    com.Name = "AddInstrument";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xA4:
                    com.Name = "A4";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xA5:
                    com.Name = "CheckInstrument";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xA6:
                    com.Name = "PlaySound";
                    com.parameters.Add(reader.ReadUInt16()); //Sound Id
                    break;
                case 0xA7:
                    com.Name = "WaitSound(A7)";
                    break;
                case 0xA8:
                    com.Name = "WaitSound(A8)";
                    break;
                case 0xA9:
                    com.Name = "PlayFanfare";
                    com.parameters.Add(reader.ReadUInt16()); //Fanfare Id
                    break;
                case 0xAA:
                    com.Name = "WaitFanfare";
                    break;
                case 0xAB:
                    com.Name = "Cry";
                    com.parameters.Add(reader.ReadUInt16()); //Pokemon Index #
                    com.parameters.Add(reader.ReadUInt16()); //0 ~ unknown
                    break;
                case 0xAC:
                    com.Name = "WaitCry";
                    break;
                case 0xAF:
                    com.Name = "SetTextScriptMessage";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xB0:
                    com.Name = "CloseMulti";
                    break;
                case 0xB1:
                    com.Name = "B1";
                    break;
                case 0xB2:
                    com.Name = "Multi(B2)";			//88
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xB3:
                    com.Name = "FadeScreen";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xB4:
                    com.Name = "ResetScreen";
                    break;
                case 0xB5:
                    com.Name = "GiveItem";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xB6:
                    com.Name = "TakeItem";
                    com.parameters.Add(reader.ReadUInt16()); // Item Index Number
                    com.parameters.Add(reader.ReadUInt16()); // Quantity
                    com.parameters.Add(reader.ReadUInt16()); // Return Result (0=added successfully | 1=full bag)
                    break;
                case 0xB7:
                    com.Name = "CheckItemBagSpace";		//Store if it is possible to give an item.
                    com.parameters.Add(reader.ReadUInt16()); // Item Index Number
                    com.parameters.Add(reader.ReadUInt16()); // Quantity
                    com.parameters.Add(reader.ReadUInt16()); // Return Result (0=not full | 1=full)
                    break;
                case 0xB8:
                    com.Name = "CheckItemBagNumber";              //222
                    com.parameters.Add(reader.ReadUInt16()); // Item #
                    com.parameters.Add(reader.ReadUInt16()); // Minimum Quantity / Return X if has >=1
                    com.parameters.Add(reader.ReadUInt16()); // Result Storage variable/container
                    break;
                case 0xB9:
                    com.Name = "StoreItemCount";
                    com.parameters.Add(reader.ReadUInt16()); // item #
                    com.parameters.Add(reader.ReadUInt16()); // Return to storage
                    break;
                case 0xBA:
                    com.Name = "CheckItemContainer";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xBB:
                    com.Name = "StoreItemBag";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xBC:
                    com.Name = "BC";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xBD:
                    com.Name = "BD";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xBE:
                    com.Name = "Warp";
                    com.parameters.Add(reader.ReadUInt16()); //Map Id
                    com.parameters.Add(reader.ReadUInt16()); // X coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Y coordinate
                    break;
                case 0xBF:
                    com.Name = "TeleportWarp";
                    com.parameters.Add(reader.ReadUInt16()); //Map Id
                    com.parameters.Add(reader.ReadUInt16()); // X coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Y coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Z coordinate
                    break;
                case 0xC1:
                    com.Name = "FallWarp";
                    com.parameters.Add(reader.ReadUInt16()); //Map Id
                    com.parameters.Add(reader.ReadUInt16()); // X coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Y coordinate
                    break;
                case 0xC2:
                    com.Name = "FastWarp";
                    com.parameters.Add(reader.ReadUInt16()); //Map Id
                    com.parameters.Add(reader.ReadUInt16()); // X coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Y coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Hero's Facing
                    break;
                case 0xC3:
                    com.Name = "UnionWarp"; // warp to union room
                    break;
                case 0xC4:
                    com.Name = "TeleportWarp";
                    com.parameters.Add(reader.ReadUInt16()); //Map Id
                    com.parameters.Add(reader.ReadUInt16()); // X coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Y coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Z coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Hero's Facing
                    break;
                case 0xC5:
                    com.Name = "SurfAnimation";
                    break;
                case 0xC6:
                    com.Name = "SpecialAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC7:
                    com.Name = "SpecialAnimation2";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC8:
                    com.Name = "CallAnimation";
                    com.parameters.Add(reader.ReadUInt16()); //Animation Id
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xCB:
                    com.Name = "StoreRandomNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xCC:
                    com.Name = "StoreVarItem";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xCD:
                    com.Name = "StoreDayPart";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xCE:
                    com.Name = "CE";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xCF:
                    com.Name = "StoreDay";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD0:
                    com.Name = "StoreDate";
                    com.parameters.Add(reader.ReadUInt16()); //Month Return to var/cont
                    com.parameters.Add(reader.ReadUInt16()); //Day Return to var/cont
                    break;
                case 0xD1:
                    com.Name = "D1";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD2:
                    com.Name = "StoreSeason";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD3:
                    com.Name = "D3";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD4:
                    com.Name = "StoreBirthDay";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD5:
                    com.Name = "StoreBadge";
                    com.parameters.Add(reader.ReadUInt16()); //Variable to return
                    com.parameters.Add(reader.ReadUInt16()); //Badge Id
                    break;
                case 0xD6:
                    com.Name = "SetBadge";
                    com.parameters.Add(reader.ReadUInt16()); //Badge Id
                    break;
                case 0xD7:
                    com.Name = "StoreBadgeNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD8:
                    com.Name = "D8";
                    com.parameters.Add(reader.ReadUInt16()); //Variable to return
                    com.parameters.Add(reader.ReadUInt16()); //Badge Id
                    break;
                case 0xD9:
                    com.Name = "D9";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xDA:
                    com.Name = "DA";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xDB:
                    com.Name = "DB";
                    break;
                case 0xDC:
                    com.Name = "DC";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xDD:
                    com.Name = "StorePokèmonCaught";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0xDE:
                    com.Name = "SpeciesDisplayDE"; // species display popup, Store
                    com.parameters.Add(reader.ReadUInt16()); //0
                    com.parameters.Add(reader.ReadUInt16()); //species
                    break;
                case 0xDF:
                    com.Name = "DF"; // species display popup, Store
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE0:
                    com.Name = "StoreVersion";
                    com.parameters.Add(reader.ReadUInt16()); //return result to this variable/cont
                    break;
                case 0xE1:
                    com.Name = "StoreGender";
                    com.parameters.Add(reader.ReadUInt16()); //return result to this variable/cont
                    break;
                case 226:
                    com.Name = "StoreE2";
                    com.parameters.Add(reader.ReadUInt16()); //return result to this variable/cont
                    break;
                case 0xE4:
                    com.Name = "E4";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE5:
                    com.Name = "StoreE5";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 230:
                    com.Name = "CheckKeyItem";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE7:
                    com.Name = "ActivateKeyItem";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 232:
                    com.Name = "CheckKeyItem2";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE9:
                    com.Name = "E9";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xEA:
                    com.Name = "EA";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xEB:
                    com.Name = "EB";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xEC:
                    com.Name = "EC";
                    break;
                case 0xED:
                    com.Name = "ED";
                    break;
                case 0xEE:
                    com.Name = "EE";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xEF:
                    com.Name = "EF";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF0:
                    com.Name = "GivePokèmonMansion";
                    com.parameters.Add(reader.ReadUInt16()); //POKE
                    break;
                case 0xF1:
                    com.Name = "F1";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF2:
                    com.Name = "F2";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF3:
                    com.Name = "F3";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF4:
                    com.Name = "F4";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF5:
                    com.Name = "F5";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF6:
                    com.Name = "F6";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF7:
                    com.Name = "F7";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF8:
                    com.Name = "F8";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF9:
                    com.Name = "F9";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xFA:
                    com.Name = "TakeMoney";			//66
                    com.parameters.Add(reader.ReadUInt16()); //Removes this amount of money from the player's $.
                    break;
                case 0xFB:
                    com.Name = "CheckEnoughMoney";			//66
                    com.parameters.Add(reader.ReadUInt16()); //Result storage container (0=enough $|1=not enough $)
                    com.parameters.Add(reader.ReadUInt16()); //Stores if current $ is >= [THIS ARGUMENT]
                    break;
                case 0xFC:
                    com.Name = "StorePokèmonHappiness";
                    com.parameters.Add(reader.ReadUInt16()); //Happiness storage container
                    com.parameters.Add(reader.ReadUInt16()); //Party member to Store
                    break;
                case 0xFD:
                    com.Name = "IncPokèmonHappiness";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xFE:
                    com.Name = "StorePartySpecies";
                    if (reader.BaseStream.Position == reader.BaseStream.Length) //this is temporary to catch movement related errors
                    {
                        com.isEnd = 1;
                        break;
                    }
                    com.parameters.Add(reader.ReadUInt16());    // Result Storage of Storeed species index #
                    if (reader.BaseStream.Position + 1 >= reader.BaseStream.Length)
                    {
                        com.isEnd = 1;
                        break;
                    }
                    else
                    {
                        com.parameters.Add(reader.ReadUInt16()); // PKM to Store
                        break;
                    }
                case 0xFF:
                    com.Name = "StorePokèmonFormNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    var nextFF = reader.ReadUInt16();
                    while (nextFF >= 0x4000) { com.parameters.Add(nextFF); nextFF = reader.ReadUInt16(); }
                    reader.BaseStream.Position -= 2;
                    break;
                case 0x101:
                    com.Name = "101";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x102:
                    com.Name = "CheckEgg";
                    com.parameters.Add(reader.ReadUInt16()); //Result storage container
                    com.parameters.Add(reader.ReadUInt16()); //Party member to Store
                    break;
                case 0x103:
                    com.Name = "StorePartyNumberMinimum";
                    com.parameters.Add(reader.ReadUInt16()); //Result Storage container
                    com.parameters.Add(reader.ReadUInt16()); //Does the player have more than [VALUE]? Return 0 if true.
                    break;
                case 0x104:
                    com.Name = "HealPokèmon";
                    break;
                case 0x105:
                    com.Name = "RenamePokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x106:
                    com.Name = "106";
                    com.parameters.Add(reader.ReadUInt16()); //Req
                    break;
                case 0x107:
                    com.Name = "StoreChosenPokèmon";
                    com.parameters.Add(reader.ReadUInt16()); //Dialog Result Logic (1 if PKM Chosen) default 0
                    com.parameters.Add(reader.ReadUInt16()); //    \->Variable Storage
                    com.parameters.Add(reader.ReadUInt16()); //Pokemon Choice Variable Storage
                    break;
                case 0x108:
                    com.Name = "StorePokèmonMoveLearned";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x109:
                    com.Name = "ChooseMoveForgot";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x10A:
                    com.Name = "StorePokèmonMoveForgot";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x10B:
                    com.Name = "10B";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x10C:
                    com.Name = "GivePokèmon";
                    com.parameters.Add(reader.ReadUInt16()); //Id Pokèmon
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //Item
                    com.parameters.Add(reader.ReadUInt16()); //Level
                    break;
                case 0x10D:
                    com.Name = "StorePokemonPartyAt";
                    com.parameters.Add(reader.ReadUInt16()); //Id Pokèmon Party
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x10E:
                    com.Name = "GivePokèmon(10E)";
                    com.parameters.Add(reader.ReadUInt16()); //Variable to return result to
                    com.parameters.Add(reader.ReadUInt16()); //Egg Pokemon to try to
                    com.parameters.Add(reader.ReadUInt16()); //Forme
                    com.parameters.Add(reader.ReadUInt16()); //Level
                    com.parameters.Add(reader.ReadUInt16()); //3
                    com.parameters.Add(reader.ReadUInt16()); //2
                    com.parameters.Add(reader.ReadUInt16()); //0
                    com.parameters.Add(reader.ReadUInt16()); //0
                    com.parameters.Add(reader.ReadUInt16()); //4
                    break;
                case 0x10F:
                    com.Name = "GiveEgg(10F)";
                    com.parameters.Add(reader.ReadUInt16()); //Variable to return result to
                    com.parameters.Add(reader.ReadUInt16()); //Egg Pokemon to try to
                    com.parameters.Add(reader.ReadUInt16()); //Response if Party is Full (~0=true or FORME?)
                    break;
                case 0x110:
                    com.Name = "StorePokèmonSex";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x113:
                    com.Name = "113";
                    com.parameters.Add(reader.ReadUInt16()); //var
                    com.parameters.Add(reader.ReadUInt16()); //var
                    break;
                case 0x114:
                    com.Name = "StorePartyHavePokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x115:
                    com.Name = "StorePartyCanLearnMove";
                    com.parameters.Add(reader.ReadUInt16()); //Variable to return result to
                    com.parameters.Add(reader.ReadUInt16()); //move to Store
                    com.parameters.Add(reader.ReadUInt16()); //Party member to Store
                    break;
                case 0x116:
                    com.Name = "StorePartyCanUseMove";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x117:
                    com.Name = "StorePokemonForm";
                    com.parameters.Add(reader.ReadUInt16()); //var
                    com.parameters.Add(reader.ReadUInt16()); //val
                    break;
                case 0x118:
                    com.Name = "CheckChosenSpecies";
                    com.parameters.Add(reader.ReadUInt16()); //Specie
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon
                    break;
                //case 0x11A:
                //    com.Name = "11A";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x11B:
                    com.Name = "StorePartyType";
                    com.parameters.Add(reader.ReadUInt16()); // Return Type 1
                    com.parameters.Add(reader.ReadUInt16()); // Return Type 2
                    com.parameters.Add(reader.ReadUInt16()); // Party member to Store
                    break;
                case 0x11C:
                    com.Name = "ForgotMove";
                    com.parameters.Add(reader.ReadUInt16()); //var
                    com.parameters.Add(reader.ReadUInt16()); //var
                    com.parameters.Add(reader.ReadUInt16()); //var
                    break;
                //case 0x11D:
                //    com.Name = "SetFavorite";			//82
                //    com.parameters.Add(reader.ReadUInt16()); //Party member to set as favorite Pokemon
                //    break;
                case 0x11E:
                    com.Name = "BadgeAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x11F:
                    com.Name = "StorePokèmonPartyNumberBadge";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x120:
                    com.Name = "SetVarPokèmonTrade";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x121:
                    com.Name = "CheckPartyAmount";
                    com.parameters.Add(reader.ReadUInt16());//Bool(Ret)
                    com.parameters.Add(reader.ReadUInt16());//Amount
                    break;
                case 0x124:
                    com.Name = "124";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x126:
                    com.Name = "126";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x127:
                    com.Name = "127";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());//Ow
                    com.parameters.Add(reader.ReadUInt16());//X
                    com.parameters.Add(reader.ReadUInt16());//Y
                    break;
                case 0x128:
                    com.Name = "128";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x129:
                    com.Name = "129";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x12A:
                    com.Name = "12A";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x12B:
                //    com.Name = "12B";
                //    com.parameters.Add(reader.ReadUInt16()); //Req
                //    com.parameters.Add(reader.ReadUInt16()); //Req
                //    com.parameters.Add(reader.ReadUInt16()); //Req
                //    break;
                case 0x12C:
                    com.Name = "12C";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x12D:
                    com.Name = "12D";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //0
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x12E:
                    com.Name = "12E";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x12F:
                    com.Name = "12F";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x130:
                    com.Name = "BootPCSound";
                    break;
                case 0x131:
                    com.Name = "PC-131";
                    break;
                case 0x132:
                    com.Name = "132";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x134:
                    com.Name = "134";
                    break;
                //case 0x136:
                //    com.Name = "136";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                case 0x137:
                    com.Name = "ShowClockSaving";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x138:
                    com.Name = "138";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x139:
                    com.Name = "SaveData(139)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x13A:
                    com.Name = "13A";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x13B:
                    com.Name = "CheckWireless";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x13C:
                //    com.Name = "13C";
                //    break;
                //case 0x13D:
                //    com.Name = "13D";
                //    com.parameters.Add(reader.ReadByte()); //Elevation
                //    com.parameters.Add(reader.ReadUInt16()); //Rotation
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16()); //Zoom
                //    //com.parameters.Add(reader.ReadUInt16());
                //    //com.parameters.Add(reader.ReadUInt16());
                //    //com.parameters.Add(reader.ReadUInt16());
                //    //com.parameters.Add(reader.ReadUInt16());
                //    //com.parameters.Add(reader.ReadUInt16());
                //    //com.parameters.Add(reader.ReadUInt16());
                //    //com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x13F:
                    com.Name = "StartCameraEvent";
                    break;
                case 0x140:
                    com.Name = "StopCameraEvent";
                    break;
                case 0x141:
                    com.Name = "LockCamera";
                    break;
                case 0x142:
                    com.Name = "ReleaseCamera";
                    break;
                case 0x143:
                    com.Name = "MoveCamera";
                    com.parameters.Add(reader.ReadUInt16()); //Elevation
                    com.parameters.Add(reader.ReadUInt16()); //Rotation
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //Zoom
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x144:
                    com.Name = "144";
                    com.parameters.Add(reader.ReadUInt16()); //Elevation
                    break;
                case 0x145:
                    com.Name = "EndCameraEvent";
                    break;
                case 0x147:
                    com.Name = "ResetCamera";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x148:
                    com.Name = "BumpingCamera";
                    com.parameters.Add(reader.ReadUInt16()); //Intensità X
                    com.parameters.Add(reader.ReadUInt16()); //Intensità Y
                    com.parameters.Add(reader.ReadUInt16()); //Speed
                    com.parameters.Add(reader.ReadUInt16()); //Degradation
                    com.parameters.Add(reader.ReadUInt16()); //Changing Speed (From Bumping X to Y)
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x149:
                    com.Name = "149";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x14B:
                    com.Name = "CallStart";
                    break;
                case 0x14A:
                    com.Name = "CallEnd";
                    break;
                case 0x14D:
                    com.Name = "14D";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x14E:
                    com.Name = "ChooseInterestingItem";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x14F:
                    com.Name = "14F";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x150:
                    com.Name = "150";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x151:
                    com.Name = "ShowDiploma";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x154:
                    com.Name = "LibertyShipAnm";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x155:
                    com.Name = "OpenInterpokè";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x156:
                    com.Name = "156";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x158:
                    com.Name = "158";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x159:
                //    com.Name = "159";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x15A:
                    com.Name = "15A";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x15B:
                //    com.Name = "15B";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0x15C:
                //    com.Name = "15C";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                case 0x15F:
                    com.Name = "CheckFriend";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x160:
                    com.Name = "160";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x15C:
                //    com.Name = "15C";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0x163:
                //    com.Name = "163";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                case 0x164:
                    com.Name = "164";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x165:
                    com.Name = "165";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x166:
                //    com.Name = "166";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x167:
                    com.Name = "StartPokèmonMusical";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    //com.parameters.Add(reader.ReadUInt16());
                    //com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x168:
                    com.Name = "StartDressPokèmonMusical";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x169:
                    com.Name = "CheckPokèmonMusicalFunctions";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x16A:
                    com.Name = "16A";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x16B:
                    com.Name = "PokèmonMenuMusicalFunctions";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x16C:
                    com.Name = "16C";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x16D:
                    com.Name = "16D";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x16E:
                    com.Name = "ChoosePokèmonMusical";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x16F:
                    com.Name = "16F";
                    break;
                case 0x170:
                    com.Name = "170";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x171:
                    com.Name = "171";
                    break;
                case 0x172:
                    com.Name = "172";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x173:
                    com.Name = "173";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x174:
                    com.Name = "174";
                    com.parameters.Add(reader.ReadUInt16()); //Dex
                    com.parameters.Add(reader.ReadUInt16()); //Level
                    com.parameters.Add(reader.ReadUInt16()); //Unk
                    break;
                case 0x175:
                    com.Name = "175";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x176:
                    com.Name = "176";
                    com.parameters.Add(reader.ReadUInt16());
                    if (scriptType == BW2SCRIPT)
                        com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x177:
                    com.Name = "177";
                    if (scriptType == BW2SCRIPT)
                        com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x178:
                    com.Name = "WildPokèmonBattle";	     // 364	0=captured, might output 1 & 2 for something else
                    com.parameters.Add(reader.ReadUInt16()); //variable to store result to
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x179:
                    com.Name = "EndWildBattle";
                    break;
                case 0x17A:
                    com.Name = "LooseWildBattle";
                    break;
                case 0x17B:
                    com.Name = "StoreWildBattleResult";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x17C:
                    com.Name = "StoreWildBattlePokèmonStatus";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x17D:
                //    com.Name = "17D";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x17E:
                //    com.Name = "17E";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x17F:
                    com.Name = "17F";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x180:
                    com.Name = "NimbasaGymRailAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x181:
                //    com.Name = "181";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x182:
                    com.Name = "182";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x183:
                //    com.Name = "183";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x184:
                //    com.Name = "184";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x185:
                //    com.Name = "185";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x186:
                    com.Name = "186";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x187:
                    com.Name = "187";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x188:
                    com.Name = "188";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x189:
                    com.Name = "189";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x18A:
                //    com.Name = "18A";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x18B:
                //    com.Name = "18B";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x18C:
                    com.Name = "18C";
                    com.parameters.Add(reader.ReadUInt16());
                    //com.parameters.Add(reader.ReadUInt16());
                    //com.parameters.Add(reader.ReadUInt16());
                    //com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x18D:
                    com.Name = "18D";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x18E:
                    com.Name = "18E";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x18F:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x190:
                    com.Name = "DriftGymLiftAnmSecondRoom";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x191:
                    com.Name = "191";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x192:
                    com.Name = "DriftGymLiftAnmFirstRoom";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x193:
                //    com.Name = "193";
                //    break;
                //case 0x195:
                //    com.Name = "195";
                //    break;
                //case 0x199:
                //    com.Name = "199";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x19B:
                    com.Name = "SetStatusCG";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19C:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19D:
                    com.Name = "19D";
                    break;
                case 0x19E:
                    com.Name = "ShowCG";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19F:
                    com.Name = "CallScreenAnimation";
                    com.parameters.Add(reader.ReadUInt16()); //AnimationId
                    break;
                case 0x1A0:
                    com.Name = "1A0";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1A1:
                    com.Name = "OpenXtransciever(1A1)";
                    com.parameters.Add(reader.ReadUInt16());
                    //com.parameters.Add(reader.ReadUInt16());
                    //com.parameters.Add(reader.ReadUInt16());
                    //com.parameters.Add(reader.ReadUInt16()); // container
                    break;
                case 0x1A3:
                    com.Name = "FlashBlackInstant";
                    break;
                case 0x1A4:
                    com.Name = "1A4";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x1A5:
                //    com.Name = "Xtransciever5 (0x1A5)";
                //    break;
                //case 0x1A6:
                //    com.Name = "Xtransciever6 (0x1A6)";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x1A7:
                    com.Name = "1A7";
                    //com.parameters.Add(reader.ReadUInt16());
                    //com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x1A8:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x1A9:
                    com.Name = "1A9";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1AA:
                    com.Name = "1AA";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1AB:
                    com.Name = "FadeFromBlack";
                    break;
                case 0x1AC:
                    com.Name = "FadeIntoBlack";
                    break;
                case 0x1AD:
                    com.Name = "FadeFromWhite";
                    break;
                case 0x1AE:
                    com.Name = "FadeIntoWhite";
                    break;
                //case 0x1AF:
                //    com.Name = "1AF";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x1B1:
                    com.Name = "ScreenFunction";
                    break;
                //case 0x1B4:
                //    com.Name = "TradeNPCStart";
                //    com.parameters.Add(reader.ReadUInt16()); //Trade Entry to Trade For
                //    com.parameters.Add(reader.ReadUInt16()); //PKM Slot that gets traded away FOREVER!
                //    break;
                //case 0x1B5:
                //    com.Name = "TradeNPCQualify";
                //    com.parameters.Add(reader.ReadUInt16()); //Logic Criterion (usually set to 0~true)
                //    com.parameters.Add(reader.ReadUInt16()); //Trade Criterion
                //    com.parameters.Add(reader.ReadUInt16()); //Offered PKM
                //    break;
                case 0x1B5:
                    com.Name = "1B5";
                    break;
                case 0x1B6:
                    com.Name = "1B6";
                    break;
                case 0x1B9:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1BA:
                    com.Name = "1BA";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1BB:
                    com.Name = "1BB";
                    break;
                //case 0x1BD:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x1BE:
                    com.Name = "TradePokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1BF:
                    com.Name = "CompareChosenPokemon";
                    com.parameters.Add(reader.ReadUInt16());//RV = True if is equal
                    com.parameters.Add(reader.ReadUInt16());//Id chosen Pokèmon
                    com.parameters.Add(reader.ReadUInt16());//Id requested Pokèmon
                    break;
                case 0x1C0:
                    com.Name = "1C0";
                    break;
                case 0x1C1:
                    com.Name = "1C1";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1C2:
                    com.Name = "StartEventBC";
                    break;
                case 0x1C3:
                    com.Name = "EndEventBC";
                    break;
                case 0x1C4:
                    com.Name = "StoreTrainerID";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x1C5:
                //    com.Name = "1C5";
                //    com.parameters.Add(reader.ReadUInt16());
                //    var next1C5 = reader.ReadUInt16();
                //    while (next1C5 >= 0x4000) { com.parameters.Add(next1C5); next1C5 = reader.ReadUInt16(); }
                //    reader.BaseStream.Position -= 2;
                //    break;
                //case 0x1C6:
                //    com.Name = "StorePokemonCaughtWF";
                //    com.parameters.Add(reader.ReadUInt16()); //True if is Pokèmon searched
                //    com.parameters.Add(reader.ReadUInt16()); //True if is caught the same day
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1C7:
                //    com.Name = "1C7";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x1C9:
                    com.Name = "StoreVarMessage";
                    com.parameters.Add(reader.ReadUInt16()); //Variable as Container
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    break;
                case 0x1CB:
                    com.Name = "1CB";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x1CC:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1CD:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x1CE:
                    com.Name = "CheckPokèdexStatus";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1CF:
                    com.Name = "StorePokèdexCaught";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x1D0:
                //    com.Name = "1D0";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1D1:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1D2:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x1D3:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1D4:
                    break;
                //case 0x1D5:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x1D6:
                    com.Name = "AffinityCheck";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1D7:
                    com.Name = "SetVarAffinityCheck";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1D8:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    //com.parameters.Add(reader.ReadUInt16());
                    //com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1D9:
                    com.Name = "1D9";
                    com.parameters.Add(reader.ReadUInt16());
                    if (scriptType == BW2SCRIPT)
                        com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1DA:
                    com.Name = "1DA";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1DB:
                    com.Name = "1DB";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1DC:
                    com.Name = "1DC";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1DD:
                    com.Name = "1DD";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1DE:
                    com.Name = "StoreDataUnity";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1DF:
                    com.Name = "1DF";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E0:
                    com.Name = "ChooseUnityFloor";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E1:
                    com.Name = "StoreTrainerUnity";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x1E2:
                //    com.Name = "1E3";
                //    break;
                case 0x1E3:
                    com.Name = "StoreUnityActivities";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E4:
                    com.Name = "StoreCanTeachDragonMove";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E5:
                    com.Name = "StorePokèmonStatusDragonMove";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E6:
                    com.Name = "1E6";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E7:
                    com.Name = "StoreChosenPokèmonDragonMove";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E8:
                    com.Name = "CheckRememberMove";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E9:
                    com.Name = "StoreRememberMove";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1EA:
                    com.Name = "1EA";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1EB:
                    com.Name = "1EB";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1EC:
                    com.Name = "SwitchOwPosition";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16()); //X Coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y Coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Z Coordinate
                    break;
                case 0x1ED:
                    com.Name = "DoublePhraseBoxInput";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1EE:
                    com.Name = "1EE";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1EF:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F0:
                    com.Name = "HMEffect";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F1:
                    com.Name = "1F1";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F2:
                    com.Name = "1F2";
                    break;
                //case 0x1F4:
                //    com.Name = "1F4";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x1F3:
                    com.Name = "1F3";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F6:
                    com.Name = "1F6";
                    com.parameters.Add(reader.ReadUInt16()); // 0
                    com.parameters.Add(reader.ReadUInt16()); // 0
                    com.parameters.Add(reader.ReadUInt16()); // 0
                    break;
                case 0x1F7:
                    com.Name = "1F7";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x1F8:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x1F9:
                    com.Name = "StartBattleExam";
                    break;
                case 0x1FA:
                    com.Name = "1FA";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1FB:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1FC:
                    com.Name = "StoreBattleExamModality";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1FD:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    if (scriptType == BW2SCRIPT)
                    {
                        com.parameters.Add(reader.ReadUInt16());
                        com.parameters.Add(reader.ReadUInt16());
                    }
                    break;
                case 0x1FE:
                    if (scriptType == BWSCRIPT)
                    {
                        com.parameters.Add(reader.ReadUInt16());
                        com.parameters.Add(reader.ReadUInt16());
                        com.parameters.Add(reader.ReadUInt16());
                        com.parameters.Add(reader.ReadUInt16());
                        com.parameters.Add(reader.ReadUInt16());
                        com.parameters.Add(reader.ReadUInt16());
                        com.parameters.Add(reader.ReadUInt16());
                    }
                    break;
                case 0x200:
                    com.Name = "CheckBattleExamStarted";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x202:
                    if (scriptType == BW2SCRIPT)
                    {
                        com.Name = "202";
                        com.parameters.Add(reader.ReadUInt16());
                        com.parameters.Add(reader.ReadUInt16());
                    }
                    else
                    {
                        com.Name = "StoreBattleExamStarNumber";
                        com.parameters.Add(reader.ReadUInt16());
                    }
                    break;
                case 0x203:
                    com.Name = "CheckBattleExamAvailable";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x204:
                    com.Name = "StoreBattleExamLevel";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x205:
                    com.Name = "StoreBattleExamType";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x206:
                    com.Name = "SavingBookAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x207:
                    com.Name = "ShowBattleExamResult";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x208:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x209:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x20A:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x20B:
                    if (scriptType == BW2SCRIPT)
                    {
                        com.Name = "20B";
                        com.parameters.Add(reader.ReadUInt16());
                    }
                    else
                    {
                        com.parameters.Add(reader.ReadUInt16());
                        com.parameters.Add(reader.ReadUInt16());
                    }
                    break;
                case 0x20C:
                    if (scriptType == BW2SCRIPT)
                    {
                        com.Name = "20C";
                    }
                    else
                    {
                        com.Name = "CheckRelocatorPassword";
                        com.parameters.Add(reader.ReadUInt16());
                        com.parameters.Add(reader.ReadUInt16());
                        com.parameters.Add(reader.ReadUInt16());
                    }//True if inserted password is exact.
                    break;
                //case 0x20D:
                //    com.Name = "20D";
                //    break;
                case 0x20E:
                    com.Name = "CheckItemInterestingBag";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x20F:
                    com.Name = "CompareInterestingItem";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x212:
                    com.Name = "212";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x214:
                    com.Name = "214";
                    com.parameters.Add(reader.ReadUInt16()); // species
                    com.parameters.Add(reader.ReadUInt16()); //0
                    com.parameters.Add(reader.ReadUInt16()); //0
                    com.parameters.Add(reader.ReadUInt16()); //0
                    break;
                case 0x215:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x217:
                    com.Name = "217";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x218:
                    com.Name = "StoreGreeting";
                    com.parameters.Add(reader.ReadUInt16());//val
                    break;
                case 0x219:
                    com.Name = "StoreThanks";
                    com.parameters.Add(reader.ReadUInt16());//var
                    break;
                //case 0x21A:
                //    com.Name = "21A";
                //    com.parameters.Add(reader.ReadUInt16());//var
                //    com.parameters.Add(reader.ReadUInt16());//val
                //    break;
                case 0x21C:
                    com.Name = "21C";
                    com.parameters.Add(reader.ReadUInt16());//var
                    com.parameters.Add(reader.ReadUInt16());//val
                    break;
                //case 0x21D:
                //    com.Name = "21D";
                //    com.parameters.Add(reader.ReadUInt16());//var
                //    break;
                case 0x21E:
                    com.Name = "21E";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x21F:
                    com.Name = "21F";
                    break;
                case 0x220:
                    com.Name = "CheckHavePokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x221:
                    com.Name = "221";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());//var
                    break;
                case 0x222:
                    com.Name = "222";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());//var
                    break;
                case 0x223:
                    com.Name = "StoreHiddenPowerType";			// ex 382
                    com.parameters.Add(reader.ReadUInt16()); //Storage for result (0-17 move type)
                    com.parameters.Add(reader.ReadUInt16()); //Party member to Store
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x224:
                //    com.Name = "224";
                //    com.parameters.Add(reader.ReadUInt16());//var
                //    com.parameters.Add(reader.ReadUInt16());//val
                //    com.parameters.Add(reader.ReadUInt16());//var
                //    break;
                case 0x225:
                    com.Name = "225";
                    com.parameters.Add(reader.ReadUInt16()); //Storage for result (0-17 move type)
                    com.parameters.Add(reader.ReadUInt16()); //Party member to Store
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x226:
                    com.Name = "RotatingAnimation";
                    com.parameters.Add(reader.ReadUInt16()); //OW Id
                    break;
                case 0x227:
                    com.Name = "ShowPokèmonPicture";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x228:
                    com.Name = "228";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x229:
                    com.Name = "229";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22A:
                    com.Name = "TeleportDreamForest";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22B:
                    com.Name = "CheckDreamFunction";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22C:
                    com.Name = "CheckSpacePokèmonDream";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22D:
                    com.Name = "SetVarPokèmonDream";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22E:
                    com.Name = "StartDreamIsle";
                    break;
                case 0x22F:
                    com.Name = "DreamBattle";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x230:
                    com.Name = "GiveDreamPokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x231:
                //    com.Name = "231";
                //    com.parameters.Add(reader.ReadUInt16()); // Var
                //    var next231 = reader.ReadUInt16();
                //    while (next231 >= 0x4000) { com.parameters.Add(next231); next231 = reader.ReadUInt16(); }
                //    reader.BaseStream.Position -= 2;
                //    break;
                case 0x232:
                    com.Name = "232";
                    com.parameters.Add(reader.ReadUInt16()); // Var
                    com.parameters.Add(reader.ReadUInt16()); // Var
                    break;
                case 0x233:
                    if (scriptType == BWSCRIPT)
                    {
                        com.Name = "233";
                        com.parameters.Add(reader.ReadUInt16()); // Var
                        com.parameters.Add(reader.ReadUInt16()); // Var
                    }
                    else
                    {
                        com.Name = "233";
                        com.parameters.Add(reader.ReadUInt16()); // Var
                    }
                    break;
                case 0x234:
                    com.Name = "233";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x236:
                    com.Name = "236";
                    com.parameters.Add(reader.ReadUInt16()); //var
                    com.parameters.Add(reader.ReadUInt16()); //val
                    com.parameters.Add(reader.ReadUInt16()); //val
                    com.parameters.Add(reader.ReadUInt16()); //val
                    break;
                case 0x237:
                    com.Name = "237";
                    com.parameters.Add(reader.ReadUInt16()); //var
                    com.parameters.Add(reader.ReadUInt16()); //val
                    com.parameters.Add(reader.ReadUInt16()); //val
                    break;
                //case 0x239:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x23A:
                    com.Name = "23A";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x23B:
                    com.Name = "CheckSendSaveCG";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //RV = Message Id
                    break;
                case 0x23D:
                    com.Name = "23D";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //RV = Message Id
                    break;
                case 0x23E:
                    com.Name = "23E";
                    com.parameters.Add(reader.ReadUInt16()); //var
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //var
                    break;
                //case 0x23F:
                //    com.Name = "Close23F";
                //    break;
                case 0x240:
                    com.Name = "240";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x242:
                    com.Name = "242";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x243:
                    com.Name = "BorderedMessage2";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x244:
                    com.Name = "OpenHelpSystem";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x245:
                //    com.Name = "245";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x246:
                    com.Name = "246";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x247:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x248:
                    com.Name = "248";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x249:
                    com.Name = "StoreInterestingItemData";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    var next249 = reader.ReadUInt16();
                    while (next249 >= 0x4000) { com.parameters.Add(next249); next249 = reader.ReadUInt16(); }
                    reader.BaseStream.Position -= 2;
                    break;
                case 0x24A:
                    if (scriptType == BWSCRIPT)
                    {
                        com.Name = "TakeInterestingItem";
                        com.parameters.Add(reader.ReadUInt16());
                        com.parameters.Add(reader.ReadUInt16());
                        break;
                    }
                    else
                    {
                        com.Name = "CheckCGearActive";
                        com.parameters.Add(reader.ReadUInt16());
                        break;
                    }
                case 0x24B:
                    com.Name = "24B";
                    com.parameters.Add(reader.ReadUInt16());
                    //com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24C:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24D:
                    com.Name = "24D";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24E:
                    com.Name = "24E";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24F:
                    com.Name = "24F";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x251:
                    com.Name = "251";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x252:
                //    com.Name = "252";
                //    com.parameters.Add(reader.ReadUInt16());
                //    var next252 = reader.ReadUInt16();
                //    while (next252 >= 0x4000) { com.parameters.Add(next252); next252 = reader.ReadUInt16(); }
                //    reader.BaseStream.Position -= 2;
                //    break;
                case 0x253:
                    com.Name = "253";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x254:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x25A:
                    com.Name = "25A";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x25C:
                    com.Name = "25C";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x25D:
                    com.Name = "CheckCGearActive";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x25F:
                //    com.Name = "25F";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x262:
                    com.Name = "262(BW2)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x263:
                    com.Name = "263(BW2)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x266:
                //    com.Name = "266";
                //    com.parameters.Add(reader.ReadUInt16()); // var
                //    break;
                //case 0x26C:
                //    com.Name = "StoreMedals26C";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x26D:
                //    com.Name = "StoreMedals26D";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x26E:
                    com.Name = "StoreMedalsNumber";
                    com.parameters.Add(reader.ReadByte()); // 3 For medals, command type?
                    com.parameters.Add(reader.ReadUInt16()); // Variable to Store To
                    break;
                //case 0x271:
                //    com.Name = "271";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x272:
                //    com.Name = "272";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x273:
                    com.Name = "ActHome(BW2)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x275:
                    com.Name = "SetVarMission(BW2)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x276:
                    com.Name = "276(BW2)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x279:
                //    com.Name = "279";
                //    break;
                //case 0x283:
                //    com.Name = "283";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0x284:
                //    com.Name = "284";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadByte());
                //    break;

                //case 0x285:
                //    com.Name = "285";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x287:
                //    com.Name = "287";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x288:
                //    com.Name = "288";
                //    com.parameters.Add(reader.ReadUInt16()); // value
                //    com.parameters.Add(reader.ReadUInt16()); // Variable
                //    com.parameters.Add(reader.ReadUInt16()); // Variable
                //    break;
                //case 0x289:
                //    com.Name = "289";
                //    com.parameters.Add(reader.ReadUInt16());    //might just be 3 tot
                //    var next289 = reader.ReadUInt16();
                //    while (next289 >= 0x4000) { com.parameters.Add(next289); next289 = reader.ReadUInt16(); }
                //    reader.BaseStream.Position -= 2;
                //    break;
                //case 0x28B:
                //    com.Name = "28B";
                //    break;
                case 0x290:
                    com.Name = "290(BW2)";
                    com.parameters.Add(reader.ReadByte());
                    break;
                //case 0x292:
                //    com.Name = "292";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0x293:
                //    com.Name = "293";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0x294:
                //    com.Name = "294";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0x295:
                //    com.Name = "290";
                //    break;
                //case 0x297:
                //    com.Name = "297";
                //    com.parameters.Add(reader.ReadUInt16());
                //    var next297 = reader.ReadUInt16();
                //    while (next297 >= 0x4000) { com.parameters.Add(next297); next297 = reader.ReadUInt16(); }
                //    reader.BaseStream.Position -= 2;
                //    break;
                //case 0x29A:
                //    com.Name = "29A";
                //    com.parameters.Add(reader.ReadByte()); // opt
                //    com.parameters.Add(reader.ReadUInt16()); //var
                //    break;
                //case 0x29B:
                //    com.Name = "29B";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0x29D:
                //    com.Name = "29D";
                //    break;
                //case 0x29E:
                //    com.Name = "29E";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x29F:
                //    com.Name = "29F";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2A0:
                //    com.Name = "StoreHasMedal";
                //    com.parameters.Add(reader.ReadUInt16()); //
                //    com.parameters.Add(reader.ReadUInt16()); //
                //    break;
                case 0x2A1:
                    com.Name = "StoreMedalsType(BW2)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x2A5:
                //    com.Name = "2A5";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2A6:
                //    com.Name = "2A6";
                //    break;
                //case 0x2A7:
                //    com.Name = "2A7";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2A8:
                //    com.Name = "2A8";
                //    break;
                //case 0x2A9:
                //    com.Name = "2A9";
                //    break;
                //case 0x2AF:
                //    com.Name = "StoreDifficulty";
                //    com.parameters.Add(reader.ReadUInt16()); // Store result to var/cont of Easy 0 | Normal 1 | Hard 2
                //    break;
                //case 0x2B1:
                //    com.Name = "2B1";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x2B2:
                    com.Name = "2B2(BW2)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x2B3:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16()); // var
                //    break;
                //case 0x2B4:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16()); // var
                //    break;
                //case 0x2B5:
                //    com.Name = "2B5";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2B6:
                //    com.Name = "2B6";
                //    com.parameters.Add(reader.ReadUInt16()); //
                //    com.parameters.Add(reader.ReadUInt16()); //var
                //    break;
                //case 0x2B7:
                //    com.Name = "2B7";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2B8:
                //    com.Name = "FollowMeStart (0x2B8)"; // seen at the start of a follow me, maybe tracksteps
                //    break;
                //case 0x2B9:
                //    com.Name = "FollowMeEnd (0x2B9)"; // maybe endtracksteps
                //    break;
                //case 0x2BA:
                //    com.Name = "FollowMeCopyStepsTo (0x2BA)"; // copy steps taken with follow me to CONT
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2BC:
                //    com.Name = "2BC";
                //    com.parameters.Add(reader.ReadUInt16());//var
                //    com.parameters.Add(reader.ReadUInt16());//var
                //    break;
                //case 0x2BD:
                //    com.Name = "2BD";
                //    com.parameters.Add(reader.ReadUInt16());//var
                //    break;
                case 0x2BE:
                    com.Name = "2BE(BW2)";
                    com.parameters.Add(reader.ReadUInt16()); //val
                    com.parameters.Add(reader.ReadUInt16()); //var
                    com.parameters.Add(reader.ReadUInt16()); //var
                    com.parameters.Add(reader.ReadUInt16()); //var
                    break;
                //case 0x2C0:
                //    com.Name = "2C0";
                //    com.parameters.Add(reader.ReadUInt16());//var
                //    com.parameters.Add(reader.ReadUInt16());//var
                //    break;
                //case 0x2C3:
                //    com.Name = "2C3";
                //    com.parameters.Add(reader.ReadUInt16());//var
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2C4:
                //    com.Name = "2C4";
                //    break;
                //case 0x2C5:
                //    com.Name = "2C5";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2CB:
                //    com.Name = "2CB";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2CD:
                //    com.Name = "2CD";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2CF:
                //    com.Name = "2CF";
                //    com.parameters.Add(reader.ReadUInt16()); //val
                //    com.parameters.Add(reader.ReadUInt16()); //val
                //    com.parameters.Add(reader.ReadUInt16()); //val
                //    com.parameters.Add(reader.ReadUInt16()); //var
                //    break;
                //case 0x2D0:
                //    com.Name = "***HABITATLISTENABLE***";
                //    break;
                case 0x2D1:
                    com.Name = "StoreBCSkyscraperTrainerNumber(BW2)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2D3:
                    com.Name = "2D3(BW2)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x2D4:
                //    com.Name = "2D4";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2D5:
                //    com.Name = "2D5";
                //    com.parameters.Add(reader.ReadUInt16()); // value
                //    com.parameters.Add(reader.ReadUInt16()); // variable
                //    break;
                case 0x2D7:
                    com.Name = "2D7(BW2)";
                    com.parameters.Add(reader.ReadUInt16()); // value
                    com.parameters.Add(reader.ReadUInt16()); // variable
                    break;
                //case 0x2D9:
                //    com.Name = "2D9";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2DA:
                //    com.Name = "2DA";
                //    com.parameters.Add(reader.ReadUInt16()); // low value
                //    break;
                //case 0x2DB:
                //    com.Name = "2DB";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2DC:
                //    com.Name = "2DC";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2DD:
                //    com.Name = "StoreUnityVisitors";
                //    com.parameters.Add(reader.ReadUInt16()); // Variable to return to?
                //    break;
                //case 0x2DF:
                //    com.Name = "StoreMyActivities"; // activity
                //    com.parameters.Add(reader.ReadUInt16()); // Variable to return to?
                //    break;
                //case 0x2E1:
                //    com.Name = "2E1";
                //    break;
                //case 0x2E8:
                //    com.Name = "2E8";
                //    com.parameters.Add(reader.ReadUInt16()); // 
                //    com.parameters.Add(reader.ReadUInt16()); // 
                //    break;
                //case 0x2ED:
                //    com.Name = "2ED";
                //    com.parameters.Add(reader.ReadUInt16()); // 
                //    com.parameters.Add(reader.ReadUInt16()); // 
                //    break;
                //case 0x2EE:
                //    com.Name = "Prop2EE";
                //    com.parameters.Add(reader.ReadUInt16()); // value ~ prop# to give?
                //    com.parameters.Add(reader.ReadUInt16()); // variable/container
                //    break;
                //case 0x2EF:
                //    com.Name = "2EF";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2F1:
                //    com.Name = "2F1";
                //    com.parameters.Add(reader.ReadUInt16()); //
                //    break;
                //case 0x2F2:
                //    com.Name = "2F2";
                //    break;

                ////commands set 2 (1000)

                case 0x3E8:
                    com.Name = "3E8(BW2)";
                    var next3E9 = reader.ReadUInt16();
                    while (next3E9 < 0xA || next3E9 > 0x400)
                    {
                        if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
                        {
                            reader.BaseStream.Position -= 2;
                            goto End;
                        }
                        com.parameters.Add(next3E9);
                        next3E9 = reader.ReadUInt16();

                    }
                End: reader.BaseStream.Position -= 2;
                    break;
                case 0x3E9:
                    com.Name = "3E9(BW2)";
                    next3E9 = reader.ReadUInt16();
                    while (next3E9 < 0xA || next3E9 > 0x400)
                    {
                        if ((next3E9 == 0x9 && reader.ReadUInt16() > 0x400) || next3E9 == 3)
                        {
                            reader.BaseStream.Position -= 2;
                            goto End;
                        }
                        com.parameters.Add(next3E9);
                        next3E9 = reader.ReadUInt16();

                    }
                    reader.BaseStream.Position -= 2;
                    break;
                case 0x3EA:
                    com.Name = "3EA(BW2)";
                    next3E9 = reader.ReadUInt16();
                    while (next3E9 < 0xA || next3E9 > 0x400)
                    {
                        if ((next3E9 == 0x9 && reader.ReadUInt16() > 0x400) || next3E9 == 3)
                        {
                            reader.BaseStream.Position -= 2;
                            goto End;
                        }
                        com.parameters.Add(next3E9);
                        next3E9 = reader.ReadUInt16();

                    }
                    reader.BaseStream.Position -= 2;
                    break;
                case 0x3EB:
                    com.Name = "3EB(BW2)";
                    next3E9 = reader.ReadUInt16();
                    while (next3E9 < 0xA || next3E9 > 0x400)
                    {
                        if ((next3E9 == 0x9 && reader.ReadUInt16() > 0x400) || next3E9 == 3)
                        {
                            reader.BaseStream.Position -= 2;
                            goto End;
                        }
                        com.parameters.Add(next3E9);
                        next3E9 = reader.ReadUInt16();

                    }
                    reader.BaseStream.Position -= 2;
                    break;
                case 0x3EC:
                    com.Name = "3EC(BW2)";
                    next3E9 = reader.ReadUInt16();
                    while (next3E9 < 0xA || next3E9 > 0x400)
                    {
                        if ((next3E9 == 0x9 && reader.ReadUInt16() > 0x400) || next3E9 == 3)
                        {
                            reader.BaseStream.Position -= 2;
                            goto End;
                        }
                        com.parameters.Add(next3E9);
                        next3E9 = reader.ReadUInt16();

                    }
                    reader.BaseStream.Position -= 2;
                    break;
                case 0x3ED:
                    com.Name = "3ED(BW2)";
                    next3E9 = reader.ReadUInt16();
                    com.parameters.Add(next3E9);
                    //while (next3E9 < 0xA || next3E9 > 0x400)
                    //{
                    //    if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
                    //    {
                    //        reader.BaseStream.Position -= 2;
                    //        goto End;
                    //    }
                    //    com.parameters.Add(next3E9);
                    //    next3E9 = reader.ReadUInt16();

                    //}
                    //reader.BaseStream.Position -= 2;
                    break;
                case 0x3EE:
                    com.Name = "3EE(BW2)";
                    var next3EE = reader.ReadUInt16();
                    while (next3EE <= 40) { com.parameters.Add(next3EE); next3EE = reader.ReadUInt16(); }
                    while (next3EE >= 0x4000) { com.parameters.Add(next3EE); next3EE = reader.ReadUInt16(); }
                    reader.BaseStream.Position -= 2;
                    break;
                case 0x3EF:
                    com.Name = "3EF(BW2)";
                    next3E9 = reader.ReadUInt16();
                    while (next3E9 < 0x40 || next3E9 > 0x400)
                    {
                        if ((next3E9 == 0x9 && reader.ReadUInt16() > 0x400) || next3E9 == 3)
                        {
                            reader.BaseStream.Position -= 2;
                            goto End;
                        }
                        com.parameters.Add(next3E9);
                        next3E9 = reader.ReadUInt16();

                    }
                    reader.BaseStream.Position -= 2;
                    break;
                case 0x3F0:
                    com.Name = "3F0";
                    next3E9 = reader.ReadUInt16();
                    while (next3E9 < 0x40 || next3E9 > 0x400)
                    {
                        if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
                        {
                            reader.BaseStream.Position -= 2;
                            goto End;
                        }
                        com.parameters.Add(next3E9);
                        next3E9 = reader.ReadUInt16();

                    }
                    reader.BaseStream.Position -= 2;
                    break;
                case 0x3F1:
                    com.Name = "3F1";
                    next3E9 = reader.ReadUInt16();
                    while (next3E9 < 0x40 || next3E9 > 0x400)
                    {
                        if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
                        {
                            reader.BaseStream.Position -= 2;
                            goto End;
                        }
                        com.parameters.Add(next3E9);
                        next3E9 = reader.ReadUInt16();

                    }
                    reader.BaseStream.Position -= 2;
                    break;
                case 0x3F2:
                    com.Name = "3F2";
                    next3E9 = reader.ReadUInt16();
                    while (next3E9 < 0xA || next3E9 > 0x400)
                    {
                        if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
                        {
                            reader.BaseStream.Position -= 2;
                            goto End;
                        }
                        com.parameters.Add(next3E9);
                        next3E9 = reader.ReadUInt16();

                    }
                    reader.BaseStream.Position -= 2;
                    break;
                case 0x3F3:
                    com.Name = "3F3";
                    next3E9 = reader.ReadUInt16();
                    while (next3E9 < 0xA || next3E9 > 0x400)
                    {
                        if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
                        {
                            reader.BaseStream.Position -= 2;
                            goto End;
                        }
                        com.parameters.Add(next3E9);
                        next3E9 = reader.ReadUInt16();

                    }
                    reader.BaseStream.Position -= 2;
                    break;
                case 0x3F4:
                    com.Name = "3F4";
                    next3E9 = reader.ReadUInt16();
                    while (next3E9 < 0xA || next3E9 > 0x400)
                    {
                        if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
                        {
                            reader.BaseStream.Position -= 2;
                            goto End;
                        }
                        com.parameters.Add(next3E9);
                        next3E9 = reader.ReadUInt16();

                    }
                    reader.BaseStream.Position -= 2;
                    break;
                case 0x3F5:
                    com.Name = "3F5";
                    next3E9 = reader.ReadUInt16();
                    while (next3E9 < 0xA || next3E9 > 0x400)
                    {
                        if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
                        {
                            reader.BaseStream.Position -= 2;
                            goto End;
                        }
                        com.parameters.Add(next3E9);
                        next3E9 = reader.ReadUInt16();

                    }
                    reader.BaseStream.Position -= 2;
                    break;
                case 0x3F6:
                    com.Name = "3F6";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x3F8:
                    com.Name = "3F8";
                    next3E9 = reader.ReadUInt16();
                    while (next3E9 < 0xA || next3E9 > 0x400)
                    {
                        if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
                        {
                            reader.BaseStream.Position -= 2;
                            goto End;
                        }
                        com.parameters.Add(next3E9);
                        next3E9 = reader.ReadUInt16();
                    }
                    reader.BaseStream.Position -= 2;
                    break;
                case 0x3F9:
                    com.Name = "3F9";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());

                    break;
                case 0x3FA:
                    com.Name = "3FA";
                    next3E9 = reader.ReadUInt16();
                    while (next3E9 < 0xA || next3E9 > 0x400)
                    {
                        if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
                        {
                            reader.BaseStream.Position -= 2;
                            goto End;
                        }
                        com.parameters.Add(next3E9);
                        next3E9 = reader.ReadUInt16();

                    }
                    reader.BaseStream.Position -= 2;
                    break;
                case 0x3FB:         //iris?
                    com.parameters.Add(reader.ReadUInt16()); //not sure if below is needed
                    next3E9 = reader.ReadUInt16();
                    while (next3E9 < 0xA || next3E9 > 0x400)
                    {
                        if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
                        {
                            reader.BaseStream.Position -= 2;
                            goto End;
                        }
                        com.parameters.Add(next3E9);
                        next3E9 = reader.ReadUInt16();
                    }
                    reader.BaseStream.Position -= 2;
                    break;
                case 0x3FC:
                    com.Name = "3FC";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x3FD:
                    com.Name = "3FD";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x3FE:
                    com.Name = "3FE";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x3FF:
                    com.Name = "3FF";
                    com.parameters.Add(reader.ReadUInt16());
                    var next3FF = reader.ReadUInt16();
                    while (next3FF >= 0x4000) { com.parameters.Add(next3FF); next3FF = reader.ReadUInt16(); }
                    reader.BaseStream.Position -= 2;
                    break;
                case 0x401:
                    com.Name = "401";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x402:
                //    com.Name = "402";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x403:
                //    com.Name = "403";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x404:
                //    com.Name = "404";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x406:
                //    com.Name = "406";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x407:
                //    com.Name = "407";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x40D:
                //    com.Name = "40D";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x40E:
                //    com.Name = "40E";
                //    com.parameters.Add(reader.ReadUInt16()); //var
                //    break;
                //case 0x410:
                //    com.Name = "410";
                //    com.parameters.Add(reader.ReadUInt16()); //var
                //    break;
                //case 0x411:
                //    com.Name = "411";
                //    com.parameters.Add(reader.ReadUInt16()); //var
                //    break;
                //case 0x412:
                //    com.Name = "412";
                //    com.parameters.Add(reader.ReadUInt16()); //var
                //    break;
                //case 0x414:
                //    com.Name = "414";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16()); //var
                //    break;
                //case 0x415:
                //    com.Name = "415";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0x416:
                //    com.Name = "416";
                //    com.parameters.Add(reader.ReadUInt16()); //var
                //    break;
                //case 0x417:
                //    com.Name = "417";
                //    com.parameters.Add(reader.ReadUInt16()); //val
                //    break;
                //case 0x418:
                //    com.Name = "418";
                //    com.parameters.Add(reader.ReadUInt16()); //var
                //    break;
                //case 0x419:
                //    com.Name = "419";
                //    com.parameters.Add(reader.ReadUInt16()); //var
                //    com.parameters.Add(reader.ReadUInt16()); //var
                //    break;
                //case 0x41A:
                //    com.Name = "41A";
                //    com.parameters.Add(reader.ReadUInt16()); //var
                //    break;
                //case 0x41B:
                //    com.Name = "40B";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16()); //var
                //    break;
                //case 0x41C:
                //    com.Name = "41C";
                //    com.parameters.Add(reader.ReadUInt16()); //var
                //    break;
                //case 0x421:
                //    com.Name = "420";
                //    com.parameters.Add(reader.ReadUInt16()); //var
                //    com.parameters.Add(reader.ReadUInt16()); //var
                //    break;24
                //case 0x411F:
                //    com.Name = "41F";
                //    com.paerameters.Add(reader.ReadUInt16()); //var
                //    com.parameters.Add(reader.ReadUInt16()); //var
                //    break;
                //case 0x420:
                //    com.Name = "420";
                //    com.parameters.Add(reader.ReadUInt16()); //var
                //    break;
                //case 0x422:
                //    com.Name = "422";
                //    com.parameters.Add(reader.ReadUInt16()); //var
                //    break;
                default:
                    com.Name = "0x" + com.Id.ToString("X");
                    break;

            }
            return com;
        }

        private void getCommands(string[] scriptsLine, ref int lineCounter, ref string line, string space, List<int> visitedLine, RichTextBox scriptBoxEditor, int scriptType)
        {
            do
            {
                line = scriptsLine[lineCounter];
                if (line.Length > 1)
                {
                    if (line.StartsWith("="))
                    {
                        lineCounter++;
                        return;
                    }
                    Utils.multiFile = multiFile;
                    Utils.varNameDictionary = varNameDictionaryList;
                    Utils.textNarc = textNarc;
                    Utils.textFile = textFile;
                    Utils.typeROM = scriptType;
                    Utils.scriptAnalyzer = scriptBoxViewer;
                    Utils.scriptBoxEditor = scriptBoxEditor;
                    Utils.varLevel = 0;
                    if (scriptType == DPSCRIPT || scriptType == PLSCRIPT)
                    {
                        try
                        {
                            Utils.getCommandSimplifiedDPP(scriptsLine, lineCounter, space, visitedLine);
                        }
                        catch
                        {
                        }
                    }
                    else if (scriptType == BWSCRIPT)
                    {
                        try
                        {
                            Utils.getCommandSimplifiedBW(scriptsLine, ref lineCounter, space, visitedLine);
                        }
                        catch
                        {
                        }
                    }
                    else if (scriptType == BW2SCRIPT)
                    {
                        try
                        {
                            Utils.getCommandSimplifiedBW2(scriptsLine, ref lineCounter, space, visitedLine);
                        }
                        catch
                        {
                        }
                    }
                    else if (scriptType == HGSSSCRIPT)
                    {
                        try
                        {
                            Utils.getCommandSimplifiedHGSS(scriptsLine, lineCounter, space, visitedLine);
                        }
                        catch
                        {
                        }
                    }
                }
                lineCounter++;
                if ((scriptType == BWSCRIPT || scriptType == BW2SCRIPT))
                {
                    if (line.Contains(" Jump ") || line.Contains("EndScript") || line.Contains("EndFunction"))
                        return;
                    if (line.Contains("End") && scriptsLine[lineCounter] == "")
                        return;
                }
                if ((scriptType == 0 || scriptType == 1) && (line.Contains(" End ") || line.Contains("KillScript") || (line.Contains("Jump") && scriptsLine[lineCounter] == "")))
                    return;
                if ((scriptType == 2) && (line.Contains(" End ") || line.Contains("KillScript") || (line.Contains("Jump") && scriptsLine[lineCounter] == "")))
                    return;
            } while (lineCounter < scriptsLine.Length);
        }

        private Script_s printCommand(Script_s script, RichTextBox scriptBox, bool isScript)
        {
            foreach (Commands_s command in script.commands)
            {
                //if (functionOffsetList.Contains((uint)command.offset) && isScript)
                //    return script;
                if (command.Name != null)
                    scriptBox.AppendText("Offset: " + command.offset.ToString() + ": " + command.Name + " ");
                else
                    scriptBox.AppendText("Offset: " + command.offset.ToString() + ": " + command.Id.ToString("X") + " ");
                //if (command.Name == "Jump")
                //    scriptBox.AppendText("Function " + functionOffsetList.IndexOf((uint)command.parameters[0]).ToString() +
                //                        "(" + command.parameters[0].ToString() + ")");
                if (command.Name == "If" || command.Name == "If2")
                    scriptBox.AppendText(StoreConditionCommand(command.parameters[0]) + setFunction(StoreHex((int)command.parameters[1])));
                else if (command.Name == "Jump" || command.Name == "Goto")
                    scriptBox.AppendText(setFunction(StoreHex((int)command.parameters[0])));
                else if (command.Name == "ApplyMovement")
                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + setFunction(StoreHex((int)command.parameters[1])));
                else if (command.Name == "Condition")
                    scriptBox.AppendText(StoreConditionCommand(command.parameters[0]));
                else if (command.Name == "StoreTextVarUnion")
                {
                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                    idMessage = (int)command.parameters[0] + 1;
                }

                ////else if ( scriptBox.AppendText(StoreConditionCommand(command.parameters[0]) + getFunction(StoreHex((int)command.parameters[1])));.Name == "Compare" || command.Name == "Compare2")
                ////    scriptBox.AppendText("0x" + command.parameters[0].ToString("X") + " 0x" + command.parameters[1].ToString());
                else if (command.Name == "Message" || command.Name == "Message2" || command.Name == "Message3" || command.Name == "Message(2C)" || command.Name == "Message(2D)" || command.Name == "Message(2E)" || command.Name == "Message(2F)")
                {
                    if (textFile != null)
                    {
                        if (scriptType != BWSCRIPT && scriptType != BW2SCRIPT)
                        {
                            if (command.parameters[0] < 0x4000)
                                idMessage = (int)command.parameters[0];
                            if (idMessage < textFile.textList.Count)
                                scriptBox.AppendText(StoreHex((int)command.parameters[0]) + "= ' " + textFile.textList[idMessage].text + " '");
                            else
                                scriptBox.AppendText(StoreHex((int)command.parameters[0]));
                        }
                        else
                        {
                            if (command.parameters[2] < 0x4000)
                                idMessage = (int)command.parameters[2];
                            if (idMessage < textFile.messageList.Count)
                            {
                                if (command.Name == "Message")
                                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + StoreHex((int)command.parameters[5]) + "= ' " + textFile.messageList[idMessage] + " '");
                                else if (command.Name == "Message2")
                                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + "= ' " + textFile.messageList[idMessage] + " '");
                                else if (command.Name == "Message3")
                                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + StoreHex((int)command.parameters[5]) + StoreHex((int)command.parameters[6]) + "= ' " + textFile.messageList[idMessage] + " '");

                            }
                            else
                            {
                                //if (command.Name == "Message")
                                //scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + StoreHex((int)command.parameters[5]));
                                if (command.Name == "Message2")
                                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]));
                                else if (command.Name == "Message3")
                                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + StoreHex((int)command.parameters[5]) + StoreHex((int)command.parameters[6]));
                            }
                        }
                    }
                    else
                        if (scriptType != BWSCRIPT && scriptType != BW2SCRIPT)
                        {
                            scriptBox.AppendText(StoreHex((int)command.parameters[0]));
                        }
                        else
                        {
                            if (command.Name == "Message")
                                scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + StoreHex((int)command.parameters[5]));
                            else if (command.Name == "Message2")
                                scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]));
                            else if (command.Name == "Message3")
                                scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + StoreHex((int)command.parameters[5]) + StoreHex((int)command.parameters[6]));
                        }
                }
                else if (command.Name == "DoubleMessage")
                {

                    if (scriptType == BWSCRIPT || scriptType == BW2SCRIPT)
                    {
                        idMessage = (int)command.parameters[2];
                        int idMessage2 = (int)command.parameters[3];
                        if (textFile != null && idMessage < textFile.textList.Count && idMessage2 < textFile.textList.Count)
                        {
                            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + StoreHex((int)command.parameters[5]) + " BLACK_MESSAGE = ' " + textFile.messageList[idMessage] + " ' ");
                            scriptBox.AppendText("WHITE_MESSAGE = ' " + textFile.messageList[idMessage2] + " '");
                        }
                        else
                            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + StoreHex((int)command.parameters[5]));
                    }
                    else
                    {
                        idMessage = (int)command.parameters[0];
                        int idMessage2 = (int)command.parameters[1];
                        if (textFile != null)
                        {
                            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + " = ' " + textFile.textList[idMessage].text + " '\n");
                            scriptBox.AppendText("\n                                                " + StoreHex((int)command.parameters[1]) + "= ' " + textFile.textList[idMessage2].text + " '");
                        }
                        else
                            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                    }

                }
                else if (command.Name == "BubbleMessage")
                {
                    idMessage = (int)command.parameters[0];
                    int idMessage2 = (int)command.parameters[1];
                    if (textFile != null && idMessage < textFile.textList.Count)
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + textFile.messageList[idMessage] + " '");
                    else
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                }
                else if (command.Name == "MusicalMessage" || (command.Name == "MusicalMessage2"))
                {
                    idMessage = (int)command.parameters[0];
                    if (textFile != null && idMessage < textFile.textList.Count)
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + "= ' " + textFile.messageList[idMessage] + " '");
                    else
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]));
                }
                else if (command.Name == "MessageBattle")
                {
                    idMessage = (int)command.parameters[1];
                    if (textFile != null && idMessage < textFile.textList.Count)
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + "= ' " + textFile.messageList[idMessage] + " '");
                    else
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                }
                else if (command.Name == "AngryMessage")
                {
                    idMessage = (int)command.parameters[0];
                    int idMessage2 = (int)command.parameters[1];
                    if (textFile != null && idMessage < textFile.textList.Count) if (textFile != null)
                            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + "= ' " + textFile.messageList[idMessage] + " '");
                        else
                            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]));
                }
                else if (command.Name == "EventGreyMessage")
                {
                    idMessage = (int)command.parameters[0];
                    int idMessage2 = (int)command.parameters[1];
                    if (textFile != null && idMessage < textFile.messageList.Count)
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + textFile.messageList[idMessage] + " '");
                    else
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                }
                else if (command.Name == "BorderedMessage")
                {
                    idMessage = (int)command.parameters[0];
                    int idMessage2 = (int)command.parameters[1];
                    if (textFile != null && idMessage < textFile.textList.Count)
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + textFile.messageList[idMessage] + " '");
                    else
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                }
                else if (command.Name == "BorderedMessage2")
                {
                    idMessage = (int)command.parameters[0];
                    int idMessage2 = (int)command.parameters[1];
                    if (textFile != null && idMessage < textFile.textList.Count)
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + textFile.messageList[idMessage] + " '");
                    else
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                }
                else if (command.Name == "CallTextMessageBox")
                {
                    idMessage = (int)command.parameters[0];
                    if (scriptType != BWSCRIPT && scriptType != BW2SCRIPT)
                    {
                        if (textFile != null && idMessage < textFile.textList.Count)
                            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + textFile.textList[idMessage].text + " '");
                        else
                            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                    }
                    else
                    {
                        if (textFile != null)
                            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + textFile.messageList[idMessage] + " '");
                        else
                            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                    }
                }
                else if (command.Name == "CallMessageBox")
                {
                    if (scriptType == BWSCRIPT || scriptType == BW2SCRIPT)
                    {
                        idMessage = (int)command.parameters[1];
                        if (textFile != null && idMessage < textFile.messageList.Count)
                            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + "= ' " + textFile.messageList[idMessage] + " '");
                        else
                            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]));
                    }
                    else
                    {
                        idMessage = (int)command.parameters[0];
                        if (idMessage < 0x4000)
                        {
                            if (textFile != null && idMessage < textFile.textList.Count)
                                scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + "= ' " + textFile.textList[idMessage].text + " '");
                            else
                                scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]));
                        }
                    }
                }
                else if (command.Name == "SetMultiTextScript")
                {
                    idMessage = (int)command.parameters[0];
                    if (textFile != null && idMessage < textFile.textList.Count)
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + textFile.textList[idMessage].text + " '");

                }
                else if (command.Name != null && command.Name.Contains("SetMultiTextScriptMessage"))
                {
                    idMessage = (int)command.parameters[0];
                    if (multiFile != null && idMessage < multiFile.textList.Count)
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + multiFile.textList[idMessage].text + " '");
                    else
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                }
                else if (command.Name == "ShowMessageAt")
                {
                    idMessage = (int)command.parameters[0];
                    if (textFile != null && idMessage < textFile.textList.Count)
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + textFile.messageList[idMessage] + " '");
                    else
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                }
                else if (command.Name == "StoreVarMessage")
                {
                    idMessage = (int)command.parameters[1];
                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                }

                else if (command.Id == 0x13F)
                {
                    //idMessage = (int)command.parameters[0];
                    //scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                }
                else
                {
                    if (command.parameters != null)
                    {
                        foreach (int parameter in command.parameters)
                            scriptBox.AppendText(StoreHex(parameter));
                    }
                }
                scriptBox.AppendText("\n");
            }
            return script;
        }

        private string setFunction(string parameter)
        {

            for (int i = 0; i < scriptOrder.Count; i++)
            {
                var actualOffset = scriptOrder[i].ToString() + " ";
                if (parameter == actualOffset)
                    return " Script " + i.ToString() + " (" + parameter + ")";
            }

            for (int i = 0; i < functionOffsetList.Count; i++)
            {
                var actualOffset = functionOffsetList[i].ToString() + " ";
                if (parameter == actualOffset)
                    return " Function " + i.ToString() + " (" + parameter + ")";
            }

            for (int i = 0; i < movOffsetList.Count; i++)
            {
                var actualOffset = movOffsetList[i].ToString() + " ";
                if (parameter == actualOffset)
                    return " Movement " + i.ToString() + " (" + parameter + ")";
            }

            return " " + parameter;
        }

        private string getFunction(string parameter)
        {
            var id = Int16.Parse(parameter.Split(' ')[1]);
            if (parameter.Contains("Script"))
                return scriptOrder[id].ToString();
            else if (parameter.Contains("Function"))
                return functionOffsetList[id].ToString();
            else if (parameter.Contains("Movement"))
                return movOffsetList[id].ToString();
            return parameter.ToString();

        }

        private string StoreHex(int parameter)
        {
            if (parameter < 0x3000)
                return parameter.ToString() + " ";
            else
                return "0x" + parameter.ToString("X") + " ";
        }

        private string retHex(string parameter)
        {
            int value = 0;
            if (parameter == "")
                return value.ToString();
            if (parameter.Length > 1 && parameter[1] == 'x')
                value = Convert.ToInt32(parameter.Substring(2, parameter.Length - 2), 16);
            else
                value = Convert.ToInt32(parameter, 10);
            return value.ToString();
        }

        private string StoreConditionCommand(uint p)
        {
            if (p == 255) return "TRUEUP";
            if (p == 0) return "LOWER";
            if (p == 1) return "EQUAL";
            if (p == 2) return "BIGGER";
            if (p == 3) return "LOWER/EQUAL";
            if (p == 4) return "BIGGER/EQUAL";
            if (p == 5) return "DIFFERENT";
            if (p == 6) return "OR";
            if (p == 7) return "AND";
            return p.ToString();
        }

        private int retConditionCommand(string p)
        {
            if (p == "TRUEUP") return 255;
            if (p == "LOWER") return 0;
            if (p == "EQUAL") return 1;
            if (p == "BIGGER") return 2;
            if (p == "LOWER/EQUAL") return 3;
            if (p == "BIGGER/EQUAL") return 4;
            if (p == "DIFFERENT") return 5;
            return -1;
        }

        public Commands_s returnCommand(BinaryReader reader)
        {

            if (reader.BaseStream.Position + 2 > reader.BaseStream.Length)
                return new Commands_s() { offset = reader.BaseStream.Position };

            Commands_s com = new Commands_s
            {
                offset = reader.BaseStream.Position,
                Id = reader.ReadUInt16(),
                parameters = new List<uint>()
            };
            if (scriptType == DPSCRIPT || scriptType == PLSCRIPT)
                readCommandDPP(reader, ref com);
            else if (scriptType == HGSSSCRIPT)
                readCommandHGSS(reader, ref com);
            else if (scriptType == BWSCRIPT)
                com = readCommandBW(reader, com);
            else if (scriptType == BW2SCRIPT)
                com = readCommandBW2(reader, com);
            return com;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Commands_s
        {
            public long offset;
            public ushort Id;
            public string Name;
            public int isEnd;
            public int numJump;
            public int isMov;
            public List<uint> parameters;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Script_s
        {
            public uint scriptStart;
            public uint scriptEnd;
            public List<Scripts.Commands_s> commands;
            public int numFunction;
            public int idScript;
            public Dictionary<int, byte> unknows;
        }

        private void listScript_SelectedIndexChanged(object sender, EventArgs e)
        {
            scriptBoxViewer.Clear();
            loadText(listScript.SelectedIndex);
            ClosableMemoryStream stream = actualNarc.figm.fileData[listScript.SelectedIndex];
            actualFileId = listScript.SelectedIndex;
            normalScript = true;
            if (scriptType == DPSCRIPT)
                if (actualFileId <= 976 && actualFileId >= 465)
                    normalScript = false;
            if (normalScript)
                loadScript(stream);
            else
                loadUnknown(stream);
        }

        private int loadText(int selectedIndex)
        {
            textFile = null;
            int idScript = selectedIndex;
            int idMessage = -1;
            int idMulti = 321;
            if (scriptType == HGSSSCRIPT)
                idMulti = 191;
            if (scriptType == PLSCRIPT)
                idMulti = 361;
            bool isBWText = false;
            if (actualTable != null)
            {
                foreach (tableRow tableRow in actualTable)
                    if (idScript == tableRow.scripts)
                        idMessage = tableRow.texts;
                if (scriptType == 0)
                {
                    if (idScript == 205)
                        idMessage = 199;
                    if (idScript == 206)
                        idMessage = 203;
                    if (idScript == 207)
                        idMessage = 207;
                    if (idScript == 462)
                        idMessage = 485;
                    if (idScript == 464)
                        idMessage = 492;
                    if (idScript == 1042)
                        idMessage = 562;
                }
                else if (scriptType == PLSCRIPT)
                {
                    if (idScript == 211)
                        idMessage = 213;
                    if (idScript == 212)
                        idMessage = 217;
                    if (idScript == 409)
                        idMessage = 381;
                    if (idScript == 413)
                        idMessage = 397;
                    if (idScript == 423)
                        idMessage = 430;
                    if (idScript == 424)
                        idMessage = 431;
                    if (idScript == 426)
                        idMessage = 432;
                }
                else if (scriptType == HGSSSCRIPT)
                {
                    if (idScript == 2)
                        idMessage = 748;
                    if (idScript == 3)
                        idMessage = 40;
                }
                else if (scriptType == 3)
                {
                    if (idScript == 854)
                        idMessage = 158;
                    if (idScript == 855)
                        idMessage = 346;
                    if (idScript == 856)
                        idMessage = 426;
                    if (idScript == 857)
                        idMessage = 302;
                    if (idScript == 859)
                        idMessage = 347;
                    if (idScript == 860)
                        idMessage = 425;
                    if (idScript == 862)
                        idMessage = 283;
                    if (idScript == 864)
                        idMessage = 283;
                    if (idScript == 867)
                        idMessage = 280;
                    if (idScript == 868)
                        idMessage = 314;
                    if (idScript == 869)
                        idMessage = 470;
                    if (idScript == 870)
                        idMessage = 471;
                    if (idScript == 876)
                        idMessage = 257;
                    if (idScript == 877)
                        idMessage = 285;
                    if (idScript == 880)
                        idMessage = 421;
                    if (idScript == 884)
                        idMessage = 281;
                    if (idScript == 890)
                        idMessage = 461;
                    if (idScript == 891)
                        idMessage = 316;
                    if (idScript == 892)
                        idMessage = 286;
                    if (idScript == 894)
                        idMessage = 0;
                    if (idScript == 895)
                        idMessage = 427;
                }
                textBox1.Text = idMessage.ToString();
                idTextFile = idMessage;
                if (idMessage != -1)
                {
                    ClosableMemoryStream textFiles = null;
                    if (!isBWText)
                        textFiles = textNarc.figm.fileData[idMessage];
                    else
                        textFiles = Utils.bwTextNarc.figm.fileData[idMessage];
                    var textMulti = textNarc.figm.fileData[idMulti];
                    int textType = 0;
                    if (scriptType == BWSCRIPT || scriptType == BW2SCRIPT)
                        textType = 1;
                    var textHandler = new Texts(textFiles, textType);
                    multiFile = new Texts(textMulti, textType);
                    textFile = textHandler;
                }
            }
            return idMessage;
        }

        private void loadUnknown(ClosableMemoryStream stream)
        {
            var reader = new BinaryReader(stream);
            reader.BaseStream.Position = 0;
            while (reader.BaseStream.Position < reader.BaseStream.Length)
                scriptBoxViewer.AppendText(reader.ReadInt16().ToString("X") + " ");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileStream text = File.Create(Directory.GetCurrentDirectory() + "textScriptDump.txt");
            var writer = new BinaryWriter(text);
            int fileCounter = 0;
            foreach (ClosableMemoryStream stream in actualNarc.figm.fileData)
            {
                int selectedText = loadText(fileCounter);
                writer.Write("FILE: " + fileCounter + "\r\r");
                scriptBoxEditor.Clear();
                if (scriptType == DPSCRIPT)
                    if (fileCounter < 465 || fileCounter > 976)
                        loadScript(stream);
                if (scriptType == PLSCRIPT)
                    if (fileCounter < 465 || fileCounter > 976)
                        loadScript(stream);
                if (scriptType == HGSSSCRIPT)
                    if (fileCounter < 266 || fileCounter > 734)
                        loadScript(stream);
                if (scriptType == BWSCRIPT && ((fileCounter % 2 == 0 && fileCounter < 854) || fileCounter > 854))
                    loadScript(stream);
                if (scriptType == BW2SCRIPT && fileCounter != 90)
                    loadScript(stream);
                writer.Write(scriptBoxViewer.Text);
                fileCounter++;
            }
            writer.Close();



        }

        private void button2_Click(object sender, EventArgs e)
        {
            SearchBox.Clear();
            if (commandToSearch.Text == "")
                return;
            int idCommand = Int32.Parse(commandToSearch.Text, System.Globalization.NumberStyles.AllowHexSpecifier);
            int fileCounter = 0;
            foreach (ClosableMemoryStream stream in actualNarc.figm.fileData)
            {
                actualFileId = fileCounter;
                try
                {
                    loadScript(stream);
                }
                catch (EndOfStreamException ex)
                {
                }
                foreach (Script_s script in scriptList)
                    if (script.commands != null)
                    {
                        foreach (Commands_s command in script.commands)
                        {
                            if (command.Id == idCommand)
                            {
                                SearchBox.AppendText("\nTrovato nel file" + fileCounter);
                                SearchBox.AppendText("\nOffset: " + command.offset + " " + command.Id + " ");
                                foreach (uint parameter in command.parameters)
                                    SearchBox.AppendText("0x" + parameter + " ");
                                goto End3;
                            }
                        }
                    }
            End3: fileCounter++;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var commandInfo = new CommandInfo(scriptType);
            commandInfo.Show();
        }

        private void narcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Narc files (*.narc)|*.narc";
            if (fileDialog.ShowDialog() != DialogResult.Cancel)
            {
                var file = fileDialog.OpenFile();
                actualNarc = new Narc();
                actualNarc.LoadNarc(new BinaryReader(file));
                ReactionNarc();
                file.Close();
            }

        }

        private void scriptToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Script files (*.script)|*.script";
            if (fileDialog.ShowDialog() != DialogResult.Cancel)
            {
                var file = fileDialog.OpenFile();
                saveScript(new BinaryWriter(file));
                const string message = "Do you wanna save texts?";
                const string caption = "Saving text";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    var textfileDialog = new SaveFileDialog();
                    textfileDialog.Filter = "Text files (*.text)|*.text";
                    textfileDialog.FileName = idTextFile + ".text";
                    if (textfileDialog.ShowDialog() != DialogResult.Cancel)
                    {
                        var file2 = textfileDialog.OpenFile();
                        string[] messageList = new string[textFile.textList.Count];
                        for (int i = 0; i < textFile.textList.Count; i++)
                            messageList[i] = textFile.textList[i].text;
                        textFile.saveText(new BinaryWriter(file2), messageList);
                        file2.Close();

                    }

                }
                file.Close();

            }
        }

        private void scriptToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Script files (*.script)|*.script";
            if (fileDialog.ShowDialog() != DialogResult.Cancel)
            {
                var file = fileDialog.OpenFile();
                UpdateType();
                loadScript(file);
                file.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        public void initAssTable(string Title, string Code)
        {
            if (Title == "POKEMON P\0\0\0")
                initAssTablePearl(Code);
            else if (Title == "POKEMON D\0\0\0")
                initAssTableDiamond(Code);
            else if (Title == "POKEMON PL\0\0")
                initAssTablePlatinum(Code);
            else if ((Title == "POKEMON HG\0\0") || (Title == "POKEMON SS\0\0"))
                LoadMapAss(ref actualTable, 0xf6be2, 0x21b, 2);
            else if ((Title == "POKEMON W\0\0\0") || (Title == "POKEMON B\0\0\0") || (Title == "POKEMON W2\0\0") || (Title == "POKEMON B2\0\0"))
                LoadMapAss(ref actualTable, 0x0, 0x0, 3);
        }

        private void initAssTablePlatinum(string Code)
        {
            if (Code == "CPUD")
                LoadMapAss(ref actualTable, 0xe6074, 0x251, 1);
            else if (Code == "CPUE")
                LoadMapAss(ref actualTable, 0xe601c, 0x251, 1);
            else if (Code == "CPUF")
                LoadMapAss(ref actualTable, 0xe60a4, 0x251, 1);
            else if (Code == "CPUI")
                LoadMapAss(ref actualTable, 0xe6038, 0x251, 1);
            else if (Code == "CPUJ")
                LoadMapAss(ref actualTable, 0xe56f0, 0x251, 1);
            else if (Code == "CPUK")
                LoadMapAss(ref actualTable, 0xe6aa4, 0x251, 1);
            else if (Code == "CPUS")
                LoadMapAss(ref actualTable, 0xe60b0, 0x251, 1);
        }

        private void initAssTableDiamond(string Code)
        {
            if (Code == "ADAD")
                LoadMapAss(ref actualTable, 0xeedcc, 0x22f, 0);
            else if (Code == "ADAE")
                LoadMapAss(ref actualTable, 0xeedbc, 0x22f, 0);
            else if (Code == "ADAF")
                LoadMapAss(ref actualTable, 0xeedfc, 0x22f, 0);
            else if (Code == "ADAI")
                LoadMapAss(ref actualTable, 0xeed70, 0x22f, 0);
            else if (Code == "ADAJ")
                LoadMapAss(ref actualTable, 0xf0c28, 0x22f, 0);
            else if (Code == "ADAK")
                LoadMapAss(ref actualTable, 0xea408, 0x22f, 0);
            else if (Code == "ADAS")
                LoadMapAss(ref actualTable, 0xeee08, 0x22f, 0);
        }

        private void initAssTablePearl(string Code)
        {
            if (Code == "APAD")
                LoadMapAss(ref actualTable, 0xeedcc, 0x22f, 0);
            else if (Code == "APAE")
                LoadMapAss(ref actualTable, 0xeedbc, 0x22f, 0);
            else if (Code == "APAF")
                LoadMapAss(ref actualTable, 0xeedfc, 0x22f, 0);
            else if (Code == "APAI")
                LoadMapAss(ref actualTable, 0xeed70, 0x22f, 0);
            else if (Code == "APAJ")
                LoadMapAss(ref actualTable, 0xf0c2c, 0x22f, 0);
            else if (Code == "APAK")
                LoadMapAss(ref actualTable, 0xea408, 0x22f, 0);
            else if (Code == "APAS")
                LoadMapAss(ref actualTable, 0xeee08, 0x22f, 0);
        }

        private void LoadMapAss(ref List<tableRow> MapTex, int offset, int num, int type)
        {
            Stream ARM9 = getARM9(type);
            var reader = new BinaryReader(ARM9);
            MapTex = new List<tableRow>();
            romType = type;
            if (type != 3)
                LoadMapAssDPPHGSS(MapTex, offset, num, type, reader);
            else
                LoadMapAssBW(MapTex, reader);
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

        private void subScripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            printSimplifiedScript(scriptBoxEditor,scriptBoxViewer,scriptType, subScripts.SelectedItem.ToString());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FileStream text = File.Create(Directory.GetCurrentDirectory() + "textScriptDumpExt.txt");
            var writer = new BinaryWriter(text);
            int fileCounter = 0;
            foreach (ClosableMemoryStream stream in actualNarc.figm.fileData)
            {
                int selectedText = loadText(fileCounter);
                writer.Write("FILE: " + fileCounter + "\r\r");
                if (scriptType == DPSCRIPT)
                    if (fileCounter < 465 || fileCounter > 976)
                        loadScript(stream);
                if (scriptType == PLSCRIPT)
                    if (fileCounter < 465 || fileCounter > 976)
                        loadScript(stream);
                if (scriptType == HGSSSCRIPT)
                    if (fileCounter < 266 || fileCounter > 734)
                        loadScript(stream);
                if (scriptType == BWSCRIPT && fileCounter != 90)
                    loadScript(stream);
                foreach (int item in subScripts.Items)
                {
                    printSimplifiedScript(scriptBoxEditor, scriptBoxViewer, scriptType, item.ToString());
                    writer.Write(scriptBoxEditor.Text);
                }
                fileCounter++;
            }
            writer.Close();
        }

        private void Scripts_Load(object sender, EventArgs e)
        {

        }


    }
}

namespace NPRE.Formats.Specific.Pokémon.Scripts
{
    struct Mov_s
    {
        public uint offset;
        public int moveId;
        public int repeatTime;
    }


    #region Structs
    [StructLayout(LayoutKind.Sequential)]
    public struct tableRow
    {
        public int map;
        public int unk;
        public int unk2;
        public int tex_int;
        public int events;
        public int scripts;
        public int texts;
        public int map_matrix;
        public int trainers;
        public int encount;
        public int tex_ext;
        public uint tileset;
        public string mapTerrainId;
        public ushort musicA;
        public ushort scriptB;
        public ushort musicB;
        public ushort musicD;
        public ushort zoneId;
        public ushort musicC;
        public ushort mapId;
        public ushort flag;
        public ushort cameraAngle;
        public uint flytoX;
        public ushort unk3;
        public uint flytoZ;
        public uint flytoY;
        public ushort cameraAngle2;
        public byte name;
        public ushort weather;
        public byte nameStyle;
        public int headerId;
    }
    #endregion

}
