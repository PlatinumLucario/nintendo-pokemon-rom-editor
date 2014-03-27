using NPRE.Formats.Specific.Pokémon.Scripts;
using System.Runtime.InteropServices;

namespace NPRE.Formats.Specific.Pokémon.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using PG4Map.Formats;
    using PG4Map;
    using NPRE.Commons;

    public class Scripts : Form
    {
        #region GUI
        private Button button4;
        private ComboBox subScripts;
        private TextBox textBox1;
        private Button button5;
        private IContainer components;
        private ComboBox listScript;
        public RichTextBox Console;
        private Button button1;
        private TextBox commandToSearch;
        private Button button2;
        public RichTextBox SearchBox;
        private Button button3;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem scriptToolStripMenuItem;
        private ToolStripMenuItem scriptToolStripMenuItem1;
        private ToolStripMenuItem narcToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem scriptToolStripMenuItem2;
        private ToolStripMenuItem narcToolStripMenuItem1;
        public RichTextBox scriptBoxViewer;
        public RichTextBox scriptBoxEditor;

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
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScriptLoader scriptOffsetLoader;

        private DPPCommandsHandler commandHandlerDPP;
        private HGSSCommandHandler commandHandlerHGSS;
        private BWCommandHandler commandHandlerBW;
        private BW2CommandHandler commandHandlerBW2;

        public List<Script_s> scriptList;
        private List<Script_s> functionList;
        private List<uint> functionOffsetList = new List<uint>();
        private List<uint> scriptOrder;
        private List<List<Mov_s>> movList;
        private List<uint> movOffsetList = new List<uint>();
        private List<uint> scriptOffList = new List<uint>();
        private List<uint> scriptStartList = new List<uint>();

        private List<Dictionary<int, string>> varNameDictionaryList;
        private List<int> visitedLine = new List<int>();

        private Narc actualNarc;
        private Narc textNarc;

        public int scriptType;
        private int lastIndexFunction = 0;
        private int lastIndexMov;
        private int idMessage;
        private int idTextFile;
        public int actualFileId = 0;

        private Texts textFile;

        private AssTableHandler assTableHandler;

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
            LoadScript(actualNode);
        }

        public Scripts(ClosableMemoryStream actualNode, Texts textFile, int idTextFile)
        {
            this.textFile = textFile;
            this.idTextFile = idTextFile;
            components = null;
            InitializeComponent();
            UpdateType();
            LoadScript(actualNode);

        }

        public Scripts(Narc actualNarc, Narc textNarc, Main main)
        {
            this.actualNarc = actualNarc;
            this.textNarc = textNarc;
            this.assTableHandler = new AssTableHandler(main);
            InitializeComponent();
            actualTable = assTableHandler.InitAssTable(main.actualNds.getHeader().getTitle(), main.actualNds.getHeader().getCode());
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
        private List<tableRow> actualTable;


        private void LoadScript(Stream actualNode)
        {
            scriptBoxViewer.Clear();
            scriptBoxEditor.Clear();
            subScripts.Items.Clear();
            Console.Clear();

            if (actualNode.Length < 2)
                return;
            var reader = new BinaryReader(actualNode);
            reader.BaseStream.Position = 0;

            scriptOffsetLoader = new ScriptLoader(Console);

            scriptList = new List<Script_s>();
            functionList = new List<Script_s>();
            functionOffsetList = new List<uint>();
            movOffsetList = new List<uint>();
            movList = new List<List<Mov_s>>();
            scriptOffList = new List<uint>();
            scriptStartList = new List<uint>();

            InitCommandHandlers();

            lastIndexFunction = 0;
            lastIndexMov = 0;

            scriptOffsetLoader.ReadScriptsOffsets(reader, scriptList, scriptOffList);
            scriptOrder = scriptOffsetLoader.scriptOrder;
            scriptList = scriptOffsetLoader.scriptList;
            scriptOffList = scriptOffsetLoader.scriptOffList;
            scriptStartList = scriptOffsetLoader.scriptStartList;

            scriptBoxViewer.AppendText("\n");

            ReadScripts(reader, scriptList, scriptOffList);

        }

        private void InitCommandHandlers()
        {
            if (scriptType == Constants.DPSCRIPT || scriptType == Constants.PLSCRIPT)
                commandHandlerDPP = new DPPCommandsHandler();
            else if (scriptType == Constants.HGSSSCRIPT)
                commandHandlerHGSS = new HGSSCommandHandler();
            else if (scriptType == Constants.BWSCRIPT)
                commandHandlerBW = new BWCommandHandler();
            else
                commandHandlerBW2 = new BW2CommandHandler();
        }

        private void ReadScripts(BinaryReader reader, List<Script_s> scriptList, List<uint> scriptOffList)
        {
            for (int scriptCounter = 0; scriptCounter < scriptList.Count; scriptCounter++)
                ReadScriptFunctionMovements(reader, scriptList, scriptOffList, scriptCounter);
        }

        private void ReadScriptFunctionMovements(BinaryReader reader, List<Script_s> scriptList, List<uint> scriptOffList, int scriptCounter)
        {
            Script_s actualScript = ReadScript(reader, scriptList, scriptOffList, scriptCounter);

            actualScript.unknows = new Dictionary<int, byte>();

            actualScript = ReadFunctionsMovements(reader, scriptOffList, scriptCounter, actualScript);

            scriptList[scriptCounter] = actualScript;
        }

        private Script_s ReadFunctionsMovements(BinaryReader reader, List<uint> scriptOffList, int scriptCounter, Script_s actualScript)
        {
            while (reader.BaseStream.Position < actualScript.scriptEnd - 1 && reader.BaseStream.Position < reader.BaseStream.Length)
            {
                uint position = (uint)reader.BaseStream.Position;
                if (position != actualScript.scriptEnd)
                {
                    if (functionOffsetList.Contains(position))
                        actualScript = ReadFunctions(reader, scriptOffList, scriptCounter, actualScript);
                    else if (movOffsetList.Contains(position))
                        actualScript = ReadMovements(reader, scriptOffList, scriptCounter, actualScript);
                    else
                    {
                        var next = reader.ReadByte();
                        scriptBoxViewer.AppendText("Offset " + (reader.BaseStream.Position - 1) + " :" + next + "\n");
                        actualScript.unknows.Add((int)reader.BaseStream.Position, next);
                    }

                }
            }
            return actualScript;
        }

        private Script_s ReadMovements(BinaryReader reader, List<uint> scriptOffList, int scriptCounter, Script_s actualScript)
        {
            Console.AppendText("\rEnd of function group. We are in :  " + reader.BaseStream.Position.ToString() + "\r");
            actualScript = LoadMovements(reader, scriptOffList, scriptCounter, actualScript);
            lastIndexMov = movList.Count;
            return actualScript;
        }

        private Script_s ReadFunctions(BinaryReader reader, List<uint> scriptOffList, int scriptCounter, Script_s actualScript)
        {
            actualScript = LoadFunctions(reader, scriptOffList, scriptCounter, actualScript, lastIndexFunction);
            lastIndexFunction = functionList.Count;
            return actualScript;
        }

        private Script_s LoadMovements(BinaryReader reader, List<uint> scriptOffList, int scriptCounter, Script_s actualScript)
        {
            movList.Add(new List<Mov_s>());
            var actualMov = movList[movList.Count - 1];
            if (scriptCounter < scriptOffList.Count - 2)
                actualMov = LoadMovement(reader, actualMov, scriptStartList[scriptCounter + 1], movList.Count - 1);
            else
                actualMov = LoadMovement(reader, actualMov, (uint)reader.BaseStream.Length, movList.Count - 1);

            movList[movList.Count - 1] = actualMov;
            Console.AppendText("\nFinished movement. We are in : " + reader.BaseStream.Position.ToString());
            if (lastIndexMov != movList.Count)
                Console.AppendText("\r End of movement group. We are in :  " + reader.BaseStream.Position.ToString() + "\r");
            return actualScript;
        }

        private Script_s LoadFunctions(BinaryReader reader, List<uint> scriptOffList, int scriptCounter, Script_s actualScript, int functionCounter)
        {
            scriptBoxViewer.AppendText("\n=== Function" + functionCounter.ToString() + "=== \n\n");
            Console.AppendText("\r\rStart analysis function " + functionCounter.ToString() +
                               ". We are in " + (uint)reader.BaseStream.Position);
            Script_s actualFunction = new Script_s();
            if (scriptCounter < scriptOffList.Count - 1)
                actualFunction = ReadCommands(reader, actualFunction, scriptStartList[scriptCounter + 1]);
            else
                actualFunction = ReadCommands(reader, actualFunction, (uint)reader.BaseStream.Length);

            functionList.Add(actualFunction);
            printCommand(actualFunction, scriptBoxViewer, false);
            Console.AppendText("\nFinished function. We are in : " + reader.BaseStream.Position.ToString());
            return actualScript;
        }

        private Script_s ReadScript(BinaryReader reader, List<Script_s> scriptList, List<uint> scriptOffList, int scriptCounter)
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

        private List<Mov_s> LoadMovement(BinaryReader reader, List<Mov_s> actualMov, uint index, int movCounter)
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

        private Script_s ReadCommands(BinaryReader reader, Script_s actualFunction, uint index)
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
            if (scriptType == Constants.DPSCRIPT)
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
            if (scriptType == Constants.DPSCRIPT)
                commandHandlerDPP.writeCommandDPP(writer, wordLine, textFile);
            if (scriptType == Constants.HGSSSCRIPT)
                commandHandlerHGSS.writeCommandHGSS(writer, wordLine);

        }


        private void GenerateScriptList()
        {
            for (int narcCounter = 0; narcCounter < actualNarc.fatbNum; narcCounter++)
                listScript.Items.Add(narcCounter);
        }

        private void UpdateType()
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

                    if (scriptType == Constants.DPSCRIPT || scriptType == Constants.PLSCRIPT)
                    {
                        try
                        {
                            commandHandlerDPP.SetSimplifierFile(multiFile, varNameDictionaryList, textNarc, textFile, scriptBoxViewer, scriptBoxEditor, 0);
                            commandHandlerDPP.GetCommandSimplifiedDPP(scriptsLine, lineCounter, space, visitedLine);
                        }
                        catch
                        {
                        }
                    }
                    else if (scriptType == Constants.BWSCRIPT)
                    {
                        try
                        {
                            commandHandlerBW.SetSimplifierFile(multiFile, varNameDictionaryList, textNarc, textFile, scriptBoxViewer, scriptBoxEditor, 0);
                            commandHandlerBW.getCommandSimplifiedBW(scriptsLine, ref lineCounter, space, visitedLine);

                        }
                        catch
                        {
                        }
                    }
                    else if (scriptType == Constants.BW2SCRIPT)
                    {
                        try
                        {
                            commandHandlerBW2.SetSimplifierFile(multiFile, varNameDictionaryList, textNarc, textFile, scriptBoxViewer, scriptBoxEditor, 0);
                            commandHandlerBW2.getCommandSimplifiedBW2(scriptsLine, ref lineCounter, space, visitedLine);
                        }
                        catch
                        {
                        }
                    }
                    else if (scriptType == Constants.HGSSSCRIPT)
                    {
                        try
                        {
                            commandHandlerHGSS.SetSimplifierFile(multiFile, varNameDictionaryList, textNarc, textFile, scriptBoxViewer, scriptBoxEditor, 0);
                            commandHandlerHGSS.getCommandSimplifiedHGSS(scriptsLine, lineCounter, space, visitedLine);
                        }
                        catch
                        {
                        }
                    }
                }
                lineCounter++;
                if ((scriptType == Constants.BWSCRIPT || scriptType == Constants.BW2SCRIPT))
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
                if (command.Name != null)
                    scriptBox.AppendText("Offset: " + command.offset.ToString() + ": " + command.Name + " ");
                else
                    scriptBox.AppendText("Offset: " + command.offset.ToString() + ": " + command.Id.ToString("X") + " ");
                if (command.Name != null)
                {
                    if (scriptType == Constants.BWSCRIPT || scriptType == Constants.BW2SCRIPT)
                    {
                        commandHandlerBW.PrintCommand(scriptBox, command, scriptOrder);
                    }
                    else if (scriptType == Constants.DPSCRIPT || scriptType == Constants.PLSCRIPT)
                    {
                        commandHandlerDPP.PrintCommand(scriptBox, command, scriptOrder);
                    }

                    //if (functionOffsetList.Contains((uint)command.offset) && isScript)
                    //            //    return script;

                    //            //if (command.Name == "Jump")
                    //            //    scriptBox.AppendText("Function " + functionOffsetList.IndexOf((uint)command.parameters[0]).ToString() +
                    //            //                        "(" + command.parameters[0].ToString() + ")");
                    //            if (command.Name == "If" || command.Name == "If2")
                    //                scriptBox.AppendText(StoreConditionCommand(command.parameters[0]) + setFunction(StoreHex((int)command.parameters[1])));
                    //            else if (command.Name == "Jump" || command.Name == "Goto")
                    //                scriptBox.AppendText(setFunction(StoreHex((int)command.parameters[0])));
                    //            else if (command.Name == "ApplyMovement")
                    //                scriptBox.AppendText(StoreHex((int)command.parameters[0]) + setFunction(StoreHex((int)command.parameters[1])));
                    //            else if (command.Name == "Condition")
                    //                scriptBox.AppendText(StoreConditionCommand(command.parameters[0]));
                    //            else if (command.Name == "StoreTextVarUnion")
                    //            {
                    //                scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                    //                idMessage = (int)command.parameters[0] + 1;
                    //            }

                    //            ////else if ( scriptBox.AppendText(StoreConditionCommand(command.parameters[0]) + getFunction(StoreHex((int)command.parameters[1])));.Name == "Compare" || command.Name == "Compare2")
                    //            ////    scriptBox.AppendText("0x" + command.parameters[0].ToString("X") + " 0x" + command.parameters[1].ToString());
                    //            else if (command.Name == "Message" || command.Name == "Message2" || command.Name == "Message3" || command.Name == "Message(2C)" || command.Name == "Message(2D)" || command.Name == "Message(2E)" || command.Name == "Message(2F)")
                    //            {
                    //                if (textFile != null)
                    //                {
                    //                    if (scriptType != Constants.BWSCRIPT && scriptType != Constants.BW2SCRIPT)
                    //                    {
                    //                        if (command.parameters[0] < 0x4000)
                    //                            idMessage = (int)command.parameters[0];
                    //                        if (idMessage < textFile.textList.Count)
                    //                            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + "= ' " + textFile.textList[idMessage].text + " '");
                    //                        else
                    //                            scriptBox.AppendText(StoreHex((int)command.parameters[0]));
                    //                    }
                    //                    else
                    //                    {
                    //                        if (command.parameters[2] < 0x4000)
                    //                            idMessage = (int)command.parameters[2];
                    //                        if (idMessage < textFile.messageList.Count)
                    //                        {
                    //                            if (command.Name == "Message")
                    //                                scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + StoreHex((int)command.parameters[5]) + "= ' " + textFile.messageList[idMessage] + " '");
                    //                            else if (command.Name == "Message2")
                    //                                scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + "= ' " + textFile.messageList[idMessage] + " '");
                    //                            else if (command.Name == "Message3")
                    //                                scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + StoreHex((int)command.parameters[5]) + StoreHex((int)command.parameters[6]) + "= ' " + textFile.messageList[idMessage] + " '");

                    //                        }
                    //                        else
                    //                        {
                    //                            //if (command.Name == "Message")
                    //                            //scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + StoreHex((int)command.parameters[5]));
                    //                            if (command.Name == "Message2")
                    //                                scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]));
                    //                            else if (command.Name == "Message3")
                    //                                scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + StoreHex((int)command.parameters[5]) + StoreHex((int)command.parameters[6]));
                    //                        }
                    //                    }
                    //                }
                    //                else
                    //                    if (scriptType != Constants.BWSCRIPT && scriptType != Constants.BW2SCRIPT)
                    //                    {
                    //                        scriptBox.AppendText(StoreHex((int)command.parameters[0]));
                    //                    }
                    //                    else
                    //                    {
                    //                        if (command.Name == "Message")
                    //                            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + StoreHex((int)command.parameters[5]));
                    //                        else if (command.Name == "Message2")
                    //                            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]));
                    //                        else if (command.Name == "Message3")
                    //                            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + StoreHex((int)command.parameters[5]) + StoreHex((int)command.parameters[6]));
                    //                    }
                    //            }
                    //            else if (command.Name == "DoubleMessage")
                    //            {

                    //                if (scriptType == Constants.BWSCRIPT || scriptType == Constants.BW2SCRIPT)
                    //                {
                    //                    idMessage = (int)command.parameters[2];
                    //                    int idMessage2 = (int)command.parameters[3];
                    //                    if (textFile != null && idMessage < textFile.textList.Count && idMessage2 < textFile.textList.Count)
                    //                    {
                    //                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + StoreHex((int)command.parameters[5]) + " BLACK_MESSAGE = ' " + textFile.messageList[idMessage] + " ' ");
                    //                        scriptBox.AppendText("WHITE_MESSAGE = ' " + textFile.messageList[idMessage2] + " '");
                    //                    }
                    //                    else
                    //                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + StoreHex((int)command.parameters[5]));
                    //                }
                    //                else
                    //                {
                    //                    idMessage = (int)command.parameters[0];
                    //                    int idMessage2 = (int)command.parameters[1];
                    //                    if (textFile != null)
                    //                    {
                    //                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + " = ' " + textFile.textList[idMessage].text + " '\n");
                    //                        scriptBox.AppendText("\n                                                " + StoreHex((int)command.parameters[1]) + "= ' " + textFile.textList[idMessage2].text + " '");
                    //                    }
                    //                    else
                    //                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                    //                }

                    //            }
                    //            else if (command.Name == "BubbleMessage")
                    //            {
                    //                idMessage = (int)command.parameters[0];
                    //                int idMessage2 = (int)command.parameters[1];
                    //                if (textFile != null && idMessage < textFile.textList.Count)
                    //                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + textFile.messageList[idMessage] + " '");
                    //                else
                    //                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                    //            }
                    //            else if (command.Name == "MusicalMessage" || (command.Name == "MusicalMessage2"))
                    //            {
                    //                idMessage = (int)command.parameters[0];
                    //                if (textFile != null && idMessage < textFile.textList.Count)
                    //                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + "= ' " + textFile.messageList[idMessage] + " '");
                    //                else
                    //                    scriptBox.AppendText(StoreHex((int)command.parameters[0]));
                    //            }
                    //            else if (command.Name == "MessageBattle")
                    //            {
                    //                idMessage = (int)command.parameters[1];
                    //                if (textFile != null && idMessage < textFile.textList.Count)
                    //                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + "= ' " + textFile.messageList[idMessage] + " '");
                    //                else
                    //                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                    //            }
                    //            else if (command.Name == "AngryMessage")
                    //            {
                    //                idMessage = (int)command.parameters[0];
                    //                int idMessage2 = (int)command.parameters[1];
                    //                if (textFile != null && idMessage < textFile.textList.Count) if (textFile != null)
                    //                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + "= ' " + textFile.messageList[idMessage] + " '");
                    //                    else
                    //                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]));
                    //            }
                    //            else if (command.Name == "EventGreyMessage")
                    //            {
                    //                idMessage = (int)command.parameters[0];
                    //                int idMessage2 = (int)command.parameters[1];
                    //                if (textFile != null && idMessage < textFile.messageList.Count)
                    //                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + textFile.messageList[idMessage] + " '");
                    //                else
                    //                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                    //            }
                    //            else if (command.Name == "BorderedMessage")
                    //            {
                    //                idMessage = (int)command.parameters[0];
                    //                int idMessage2 = (int)command.parameters[1];
                    //                if (textFile != null && idMessage < textFile.textList.Count)
                    //                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + textFile.messageList[idMessage] + " '");
                    //                else
                    //                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                    //            }
                    //            else if (command.Name == "BorderedMessage2")
                    //            {
                    //                idMessage = (int)command.parameters[0];
                    //                int idMessage2 = (int)command.parameters[1];
                    //                if (textFile != null && idMessage < textFile.textList.Count)
                    //                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + textFile.messageList[idMessage] + " '");
                    //                else
                    //                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                    //            }
                    //            else if (command.Name == "CallTextMessageBox")
                    //            {
                    //                idMessage = (int)command.parameters[0];
                    //                if (scriptType != Constants.BWSCRIPT && scriptType != Constants.BW2SCRIPT)
                    //                {
                    //                    if (textFile != null && idMessage < textFile.textList.Count)
                    //                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + textFile.textList[idMessage].text + " '");
                    //                    else
                    //                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                    //                }
                    //                else
                    //                {
                    //                    if (textFile != null)
                    //                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + textFile.messageList[idMessage] + " '");
                    //                    else
                    //                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                    //                }
                    //            }
                    //            else if (command.Name == "CallMessageBox")
                    //            {
                    //                if (scriptType == Constants.BWSCRIPT || scriptType == Constants.BW2SCRIPT)
                    //                {
                    //                    idMessage = (int)command.parameters[1];
                    //                    if (textFile != null && idMessage < textFile.messageList.Count)
                    //                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + "= ' " + textFile.messageList[idMessage] + " '");
                    //                    else
                    //                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]));
                    //                }
                    //                else
                    //                {
                    //                    idMessage = (int)command.parameters[0];
                    //                    if (idMessage < 0x4000)
                    //                    {
                    //                        if (textFile != null && idMessage < textFile.textList.Count)
                    //                            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + "= ' " + textFile.textList[idMessage].text + " '");
                    //                        else
                    //                            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]));
                    //                    }
                    //                }
                    //            }
                    //            else if (command.Name == "SetMultiTextScript")
                    //            {
                    //                idMessage = (int)command.parameters[0];
                    //                if (textFile != null && idMessage < textFile.textList.Count)
                    //                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + textFile.textList[idMessage].text + " '");

                    //            }
                    //            else if (command.Name != null && command.Name.Contains("SetMultiTextScriptMessage"))
                    //            {
                    //                idMessage = (int)command.parameters[0];
                    //                if (multiFile != null && idMessage < multiFile.textList.Count)
                    //                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + multiFile.textList[idMessage].text + " '");
                    //                else
                    //                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                    //            }
                    //            else if (command.Name == "ShowMessageAt")
                    //            {
                    //                idMessage = (int)command.parameters[0];
                    //                if (textFile != null && idMessage < textFile.textList.Count)
                    //                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + textFile.messageList[idMessage] + " '");
                    //                else
                    //                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                    //            }
                    //            else if (command.Name == "StoreVarMessage")
                    //            {
                    //                idMessage = (int)command.parameters[1];
                    //                scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                    //            }

                    //            else if (command.Id == 0x13F)
                    //            {
                    //                //idMessage = (int)command.parameters[0];
                    //                //scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                    //            }
                    //            else
                    //            {
                    //                if (command.parameters != null)
                    //                {
                    //                    foreach (int parameter in command.parameters)
                    //                        scriptBox.AppendText(StoreHex(parameter));
                    //                }
                    //            }
                    //            scriptBox.AppendText("\n");
                    //        }
                    //    }
                    //}

                }
            }
            return script;
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
            if (scriptType == Constants.DPSCRIPT || scriptType == Constants.PLSCRIPT)
            {
                com = commandHandlerDPP.readCommandDPP(reader, com, movOffsetList, functionOffsetList, scriptOrder);
                movOffsetList = commandHandlerDPP.movOffsetList;
                functionOffsetList = commandHandlerDPP.functionOffsetList;
            }
            else if (scriptType == Constants.HGSSSCRIPT)
            {
                com = commandHandlerHGSS.readCommandHGSS(reader, com, movOffsetList, functionOffsetList, scriptOrder);
                movOffsetList = commandHandlerHGSS.movOffsetList;
                functionOffsetList = commandHandlerHGSS.functionOffsetList;
            }
            else if (scriptType == Constants.BWSCRIPT)
            {
                com = commandHandlerBW.readCommandBW(reader, com, movOffsetList, functionOffsetList, scriptStartList, scriptOffList, scriptOrder);
                movOffsetList = commandHandlerBW.movOffsetList;
                functionOffsetList = commandHandlerBW.functionOffsetList;
            }
            else if (scriptType == Constants.BW2SCRIPT)
            {
                com = commandHandlerBW2.readCommandBW2(reader, com, movOffsetList, functionOffsetList, scriptStartList, scriptOffList, scriptOrder);
                movOffsetList = commandHandlerBW2.movOffsetList;
                functionOffsetList = commandHandlerBW2.functionOffsetList;
            }
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
            if (scriptType == Constants.DPSCRIPT)
                if (actualFileId <= 976 && actualFileId >= 465)
                    normalScript = false;
            if (normalScript)
                LoadScript(stream);
            else
                loadUnknown(stream);
        }

        private int loadText(int selectedIndex)
        {
            textFile = null;
            int idScript = selectedIndex;
            int idMessage = -1;
            int idMulti = 321;
            if (scriptType == Constants.HGSSSCRIPT)
                idMulti = 191;
            if (scriptType == Constants.PLSCRIPT)
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
                else if (scriptType == Constants.PLSCRIPT)
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
                else if (scriptType == Constants.HGSSSCRIPT)
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
                        textFiles = commandHandlerBW.bwTextNarc.figm.fileData[idMessage];
                    var textMulti = textNarc.figm.fileData[idMulti];
                    int textType = 0;
                    if (scriptType == Constants.BWSCRIPT || scriptType == Constants.BW2SCRIPT)
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
                if (scriptType == Constants.DPSCRIPT)
                    if (fileCounter < 465 || fileCounter > 976)
                        LoadScript(stream);
                if (scriptType == Constants.PLSCRIPT)
                    if (fileCounter < 465 || fileCounter > 976)
                        LoadScript(stream);
                if (scriptType == Constants.HGSSSCRIPT)
                    if (fileCounter < 266 || fileCounter > 734)
                        LoadScript(stream);
                if (scriptType == Constants.BWSCRIPT && ((fileCounter % 2 == 0 && fileCounter < 854) || fileCounter > 854))
                    LoadScript(stream);
                if (scriptType == Constants.BW2SCRIPT && fileCounter != 90)
                    LoadScript(stream);
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
                    LoadScript(stream);
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
                LoadScript(file);
                file.Close();
            }
        }

        private void subScripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            printSimplifiedScript(scriptBoxEditor, scriptBoxViewer, scriptType, subScripts.SelectedItem.ToString());
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
                if (scriptType == Constants.DPSCRIPT)
                    if (fileCounter < 465 || fileCounter > 976)
                        LoadScript(stream);
                if (scriptType == Constants.PLSCRIPT)
                    if (fileCounter < 465 || fileCounter > 976)
                        LoadScript(stream);
                if (scriptType == Constants.HGSSSCRIPT)
                    if (fileCounter < 266 || fileCounter > 734)
                        LoadScript(stream);
                if (scriptType == Constants.BWSCRIPT && fileCounter != 90)
                    LoadScript(stream);
                foreach (int item in subScripts.Items)
                {
                    printSimplifiedScript(scriptBoxEditor, scriptBoxViewer, scriptType, item.ToString());
                    writer.Write(scriptBoxEditor.Text);
                }
                fileCounter++;
            }
            writer.Close();
        }

        public EventHandler scriptToolStripMenuItem_Click { get; set; }
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
