﻿using NPRE.Formats.Specific.Pokémon.Scripts;
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
            this.commandToSearch.Location = new System.Drawing.Point(332, 31);
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
            this.scriptToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.scriptToolStripMenuItem.Text = "Open";
            // 
            // scriptToolStripMenuItem1
            // 
            this.scriptToolStripMenuItem1.Name = "scriptToolStripMenuItem1";
            this.scriptToolStripMenuItem1.Size = new System.Drawing.Size(104, 22);
            this.scriptToolStripMenuItem1.Text = "Script";
            this.scriptToolStripMenuItem1.Click += new System.EventHandler(this.scriptToolStripMenuItem1_Click);
            // 
            // narcToolStripMenuItem
            // 
            this.narcToolStripMenuItem.Name = "narcToolStripMenuItem";
            this.narcToolStripMenuItem.Size = new System.Drawing.Size(104, 22);
            this.narcToolStripMenuItem.Text = "Narc";
            this.narcToolStripMenuItem.Click += new System.EventHandler(this.narcToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scriptToolStripMenuItem2,
            this.narcToolStripMenuItem1});
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
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
            this.scriptBoxViewer.Location = new System.Drawing.Point(12, 75);
            this.scriptBoxViewer.Name = "scriptBoxViewer";
            this.scriptBoxViewer.Size = new System.Drawing.Size(654, 473);
            this.scriptBoxViewer.TabIndex = 0;
            this.scriptBoxViewer.Text = "";
            // 
            // scriptBoxEditor
            // 
            this.scriptBoxEditor.Location = new System.Drawing.Point(672, 75);
            this.scriptBoxEditor.Name = "scriptBoxEditor";
            this.scriptBoxEditor.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.scriptBoxEditor.Size = new System.Drawing.Size(550, 473);
            this.scriptBoxEditor.TabIndex = 10;
            this.scriptBoxEditor.Text = "";
            this.scriptBoxEditor.WordWrap = false;
            // 
            // subScripts
            // 
            this.subScripts.FormattingEnabled = true;
            this.subScripts.Location = new System.Drawing.Point(646, 33);
            this.subScripts.Name = "subScripts";
            this.subScripts.Size = new System.Drawing.Size(121, 21);
            this.subScripts.TabIndex = 11;
            this.subScripts.SelectedIndexChanged += new System.EventHandler(this.subScripts_SelectedIndexChanged);
            // 
            // Scripts
            // 
            this.ClientSize = new System.Drawing.Size(1243, 742);
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
                reader.BaseStream.Position != scriptOffList[0] && reader.BaseStream.Position<reader.BaseStream.Length);

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
            uint scriptStart = scriptOffFinder + pad;

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
                        var next = reader.ReadByte();
                        scriptBoxViewer.AppendText("Offset " + (reader.BaseStream.Position - 1) + " :" + next + "\n");
                        actualScript.unknows.Add((int)reader.BaseStream.Position, next);
                    }
                     
                }
            }
            return actualScript;
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
            printCommand(actualFunction, scriptBoxViewer,false);
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
            printCommand(actualScript, scriptBoxViewer,true);
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
                scriptBoxViewer.AppendText("Offset: " + actualMov[i].offset.ToString() + " m" + actualMov[i].moveId.ToString() + " 0x" + actualMov[i].repeatTime.ToString() + "\n");

            var position = reader.BaseStream.Position;
            //reader.BaseStream.Position += 3;

            return actualMov;
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
                    var isEndC = reader.ReadUInt16();
                    if (isEndC != 2)
                    {
                        com.isEnd = 1;
                        checkNextFunction(reader, (uint)reader.BaseStream.Position - 2);
                    }
                    reader.BaseStream.Position -= 2;
                    break;

                case 0x17:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x1a:
                    com.Name = "Goto";
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[0];
                    checkNextFunction(reader, functionOffset);
                    com.numJump++;
                    break;

                case 0x1b:
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
                    com.Name = "SetValue";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x27:
                    com.Name = "CopyValue";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x28:
                    com.Name = "SetVar";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x29:
                    com.Name = "CopyVar";
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

                case 0x31:
                    com.Name = "WaitButton";
                    break;

                case 0x34:
                    com.Name = "CloseMessageKeyPress";
                    break;

                case 0x35:
                    com.Name = "FreezeMessageBox";
                    break;

                case 0x36:
                    com.Name = "CallMessageBox";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x37:
                    com.Name = "ColorMessageBox";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x38:
                    com.Name = "TypeMessageBox";
                    com.parameters.Add(reader.ReadByte());
                    break;

                case 0x39:
                    com.Name = "NoMapMessageBox";
                    break;

                case 0x3a:
                    com.Name = "CallTextMessageBox";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x3b:
                    com.Name = "StoreMenuStatus";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x3c:
                    com.Name = "ShowMenu";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x3e:
                    com.Name = "YesNoBox";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x3f:
                    com.Name = "WaitFor";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x40:
                    com.Name = "Multi";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x41:
                    com.Name = "Multi2";
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
                    com.Name = "CloseMulti";
                    break;

                case 0x44:
                    com.Name = "Multi3";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    //com.parameters.Add(reader.ReadByte());
                    //com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x45:
                    com.Name = "Multi4";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x46:
                    com.Name = "SetTextScriptMessageMulti";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x47:
                    com.Name = "CloseMulti4";
                    break;

                case 0x48:
                    com.Name = "SetTextRowMulti";
                    com.parameters.Add(reader.ReadByte());
                    break;

                case 0x49:
                    com.Name = "Fanfare";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x4a:
                    com.Name = "Fanfare2";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x4b:
                    com.Name = "WaitFanfare";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x4c:
                    com.Name = "Cry";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x4d:
                    com.Name = "WaitCry";
                    break;

                case 0x4e:
                    com.Name = "PlaySound";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x4f:
                    com.Name = "FadeDef";
                    break;

                case 80:
                    com.Name = "PlaySound2";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x51:
                    com.Name = "StopSound";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x52:
                    com.Name = "RestartSound";
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
                    com.Name = "PlaySound3";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x58:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x59:
                    com.Name = "StoreSoundMicrophone";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x5a:
                    com.Name = "SwitchSound2";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x5b:
                    com.Name = "ActivateMicrophone";
                    break;

                case 0x5c:
                    com.Name = "DisactivateMicrophone";
                    break;
                case 0x5d:
                    com.Name = "5D";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x5e:
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

                case 0x5f:
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

                case 0x6a:
                    com.Name = "StoreOWPosition";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x6b:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x6c:
                    com.Name = "ContinueFollow";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x6d:
                    com.Name = "FollowHero";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x6e:
                    com.Name = "StopFollowHero";
                    break;

                case 0x6f:
                    com.Name = "GiveMoney";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x70:
                    com.Name = "TakeMoney";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x71:
                    com.Name = "CheckMoney";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x72:
                    com.Name = "ShowMoney";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x73:
                    com.Name = "HideMoney";
                    break;

                case 0x74:
                    com.Name = "UpdateMoney";
                    break;

                case 0x75:
                    com.Name = "ShowCoins";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x76:
                    com.Name = "HideCoins";
                    break;

                case 0x77:
                    com.Name = "UpdateCoins";
                    break;

                case 0x78:
                    com.Name = "CompareCoins";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x79:
                    com.Name = "GiveCoins";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x7a:
                    com.Name = "TakeCoins";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x7b:
                    com.Name = "GiveItem";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x7c:
                    com.Name = "TakeItem";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x7d:
                    com.Name = "CheckItemBagSpace";
                    com.parameters.Add(reader.ReadUInt16()); //Item Id
                    com.parameters.Add(reader.ReadUInt16()); //Amount
                    com.parameters.Add(reader.ReadUInt16()); //RV = Item Bag Number
                    break;

                case 0x7e:
                    com.Name = "CheckItemBagNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x7f:
                    com.Name = "StoreItemTaken";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x80:
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

                case 0x85:
                    com.Name = "StoreUndergroundPcStatus";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x87:
                    com.Name = "StoreUndergroundItem";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x8f:
                    com.Name = "SendItemType";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x93:
                    com.Name = "StorePokèmonParty";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x94:
                    com.Name = "StorePokèmonParty2";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x95:
                    com.Name = "StorePokèmonParty3";
                    com.parameters.Add(reader.ReadUInt16());
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
                    com.Name = "StoreMove";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x9A:
                    com.Name = "StorePlace";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x9B:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0xA1:
                    com.Name = "CallEnd";
                    break;

                case 0xA3:
                    com.Name = "StartWFC";
                    break;

                case 0xA5:
                    com.Name = "StartInterview";
                    break;

                case 0xA6:
                    com.Name = "StartDressPokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0xA7:
                    com.Name = "DisplayDressedPokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0xA8:
                    com.Name = "DisplayContestPokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0xA9:
                    com.Name = "OpenBallCapsule";
                    break;

                case 0xAA:
                    com.Name = "OpenSinnohMaps";
                    break;

                case 0xAB:
                    com.Name = "OpenPc";
                    com.parameters.Add(reader.ReadByte());
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
                    com.Name = "StoreWFC";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
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
                    com.Name = "ChoosePok\x00e9monName";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
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
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xbf:
                    com.Name = "RockClimbAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC0:
                    com.Name = "SurfAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC1:
                    com.Name = "WaterfallAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC2:
                    com.Name = "FlyAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC5:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC6:
                    com.Name = "Tuxedo";
                    //com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC7:
                    com.Name = "StoreBike";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC8:
                    com.Name = "RideBike";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0xC9:
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
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0xCE:
                    com.Name = "SetVarRival";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0xCF:
                    com.Name = "SetVarAlter";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0xD0:
                    com.Name = "SetVarPokémon";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD1:
                    com.Name = "SetVarItem";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD2:
                    com.Name = "SetVarItem2";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD3:
                    com.Name = "SetVarBattleItem";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD4:
                    com.Name = "SetVarAttack";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD5:
                    com.Name = "SetVarNumber";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD6:
                    com.Name = "SetVarNickPokémon";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD7:
                    com.Name = "SetVarKeyItem";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD8:
                    com.Name = "SetVarTrainer";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD9:
                    com.Name = "SetVarTrainer2";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0xDA:
                    com.Name = "SetVarPokèmonStored";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0xDB:
                    com.Name = "SetVarHeroStarter";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0xDC:
                    com.Name = "SetVarRivalStarter";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0xDE:
                    com.Name = "StoreStarter";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xDF:
                    com.Name = "DF";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE0:
                    com.Name = "SetVarUndergroundItem";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE1:
                    com.Name = "E1";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE2:
                    com.Name = "SetVarSwarmPlace";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE3:
                    com.Name = "StoreSwarmInfo";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE4:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE5:
                    com.Name = "TrainerBattle";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE6:
                    com.Name = "MessageBattle";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE7:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE8:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE9:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xEA:
                    com.Name = "ActTrainer";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xEB:
                    com.Name = "TeleportPC";
                    break;
                case 0xEC:
                    com.Name = "StoreBattleResult";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xED:
                    com.Name = "StoreBattleResult2";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xEE:
                    com.Name = "CheckDoubleBattle";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF2:
                    com.Name = "F2";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF3:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF7:
                    com.Name = "StartPokèmonContest";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF8:
                    com.Name = "StartOvation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF9:
                    com.Name = "StopOvation";
                    com.parameters.Add(reader.ReadUInt16());
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
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xFE:
                    com.Name = "FE";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xFF:
                    com.Name = "SetVarPartIdMusical";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x101:
                    com.Name = "BlackScreenEffect";
                    break;
                case 0x102:
                    com.Name = "SetVarModeContest";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x103:
                    com.Name = "SetVarTypeContest";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x104:
                    com.Name = "SetVarNameWinnerContest";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x106:
                    com.Name = "SetVarPokèWinnerContest";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x107:
                    com.Name = "107";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x108:
                    com.Name = "StoreVarPeopleId";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x109:
                    com.Name = "StoreVarPartId";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x10A:
                    com.Name = "10A";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x10B:
                    com.Name = "ActPeopleContest";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x10C:
                    com.Name = "10C";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x10D:
                    com.Name = "CheckWinContest";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x10E:
                    com.Name = "SetVarItemWinnerContest";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x10F:
                    com.Name = "10F";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x110:
                    com.Name = "110";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x111:
                    com.Name = "StartFlashContest";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x112:
                    com.Name = "EndFlashContest";
                    break;
                case 0x113:
                    com.Name = "CarpetAnimationContest";
                    break;
                case 0x114:
                    com.Name = "SetFloor";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x115:
                    com.Name = "115";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x117:
                    com.Name = "117";
                    break;
                case 0x118:
                    com.Name = "118";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x119:
                    com.Name = "CheckPokèrus";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x11B:
                    com.Name = "WarpMapLift";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x11C:
                    com.Name = "StoreFloor";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x11D:
                    com.Name = "StartLift";
                    break;
                case 0x11E:
                    com.Name = "StorePokèmonNumberCaught";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x124:
                    com.Name = "WildPokèmonBattle";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x127:
                    com.Name = "HoneyEffect";
                    break;
                case 0x128:
                    com.Name = "CheckHoney";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x129:
                    com.Name = "StartHoneyBattle";
                    break;
                case 0x12A:
                    com.Name = "EndHoneyBattle";
                    break;
                case 0x12C:
                    com.Name = "StoreStatusSave";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x12D:
                    com.Name = "CheckErrorSave";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x12E:
                    com.Name = "CheckDressPicture";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x12F:
                    com.Name = "CheckContestPicture";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x130:
                    com.Name = "StorePictureName";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x131:
                    com.Name = "ActPokèKron";
                    break;
                case 0x132:
                    com.Name = "CheckPokèKron";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x133:
                    com.Name = "ActPokèKronApplication";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x134:
                    com.Name = "CheckPokèKronApplication";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x135:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x139:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x13C:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x13F:
                    com.Name = "StoreTextVarUnion";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x140:
                    com.Name = "ChooseUnion";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x141:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x142:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x143:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x144:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x145:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x146:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x147:
                    com.Name = "PokèMart";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x148:
                    com.Name = "PokèMart2";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x14A:
                    com.Name = "14A";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x14D:
                    com.Name = "StoreGender";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x14E:
                    com.Name = "HealPokèmon";
                    break;
                case 0x152:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x154:
                    com.Name = "StartChooseWifiSprite";
                    break;
                case 0x155:
                    com.Name = "ChooseWifiSprite";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x156:
                    com.Name = "ActWifiSprite";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x158:
                    com.Name = "ActSinnohPokèdex";
                    break;
                case 0x15A:
                    com.Name = "ActRunningShoes";
                    break;
                case 0x15B:
                    com.Name = "CheckBadge";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x15C:
                    com.Name = "SetBadge";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x15D:
                    com.Name = "StoreBadgeNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x15F:
                    com.Name = "ActRunningShoes2";
                    break;
                case 0x160:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x161:
                    com.Name = "StartFollowHero";
                    break;
                case 0x162:
                    com.Name = "StopFollowHero2";
                    break;
                case 0x163:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x164:
                    com.Name = "StartFollowHero2";
                    break;
                case 0x166:
                    com.parameters.Add(reader.ReadUInt16());
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
                case 0x16E:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x175:
                    com.Name = "175";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x176:
                    com.Name = "176";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x177:
                    com.Name = "StorePokèmonPartyNumber3";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x17C:
                    com.Name = "SetVarPokèmonNature";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x17D:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x17E:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x181:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x184:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x186:
                    com.Name = "ChangeOwPosition";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x187:
                    com.Name = "SetOwPosition";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x188:
                    com.Name = "ChangeOwPosition2";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x189:
                    com.Name = "ReleaseOw";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x18A:
                    com.Name = "SetTilePassable";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x18B:
                    com.Name = "SetTileLocked";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x18C:
                    com.Name = "18C";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    //com.parameters.Add(reader.ReadUInt16());
                    //com.parameters.Add(reader.ReadUInt16());
                    //com.parameters.Add(reader.ReadUInt16());
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
                    break;
                case 0x191:
                    com.Name = "ShowChoosePokèmonMenu";
                    break;
                case 0x193:
                    com.Name = "StoreChosenPokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x194:
                    com.Name = "ShowChoosePokèmonMusical";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x195:
                    com.Name = "StoreChosenPokèmonMusical";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x196:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x197:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x198:
                    com.Name = "StorePokèmonId";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19A:
                    com.Name = "StorePokèmonPartyNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19B:
                    com.Name = "StorePokèmonPartyNumber2";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19C:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19E:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19F:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1A0:
                    com.Name = "01A0";
                    break;
                case 0x1AE:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1AF:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1B0:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1B1:
                    com.Name = "0x1B1";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1B2:
                    com.Name = "0x1B2";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1B4:
                    com.Name = "CheckMail";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1B5:
                    com.Name = "ShowRecordList";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1B6:
                    com.Name = "StoreTime";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1B7:
                    com.Name = "StoreBoundedVariable";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1B8:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1B9:
                    com.Name = "StorePokèmonHappiness";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1BA:
                    com.Name = "IncPokèmonHappiness";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1BC:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1BD:
                    com.Name = "StoreHeroFaceOrientation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1BE:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1BF:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1C0:
                    com.Name = "StoreSpecificPokèmonParty";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1C1:
                    com.Name = "StorePokèmonSize";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
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
                case 0x1C6:
                    com.Name = "StorePokèmonDeleter";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1C7:
                    com.Name = "StoreMoveDeleter";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1C8:
                    com.Name = "StorePokèmonMoveNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1C9:
                    com.Name = "DeletePokèmonMove";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1CB:
                    com.Name = "SetVarMoveDeleter";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1CC:
                    com.Name = "ActJournal";
                    break;
                case 0x1CD:
                    com.Name = "1CD";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1CF:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1D0:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1D1:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1D2:
                    com.Name = "GiveAccessories";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1D3:
                    com.Name = "StoreAccessories";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1D5:
                    com.Name = "GiveAccessories2";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1D6:
                    com.Name = "StoreAccessories2";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1D7:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1D8:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E5:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E8:
                    com.Name = "StoreSinnohPokèdexCompleteStatus";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1E9:
                    com.Name = "StoreNationalPokèdexCompleteStatus";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1EA:
                    com.Name = "GiveSinnohDiploma";
                    break;
                case 0x1EB:
                    com.Name = "GiveNationalDiploma";
                    break;

                case 0x1ED:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F1:
                    com.Name = "StoreFossilNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F4:
                    com.Name = "StoreFossilPokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F5:
                    com.Name = "TakeFossil";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F6:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F7:
                    com.Name = "CheckPoisoned";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F8:
                    com.Name = "HealPoisoned";
                    break;
                case 0x1F9:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1FB:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x200:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x201:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x202:
                    com.Name = "SetSafariGame";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x203:
                    com.Name = "203";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x206:
                    com.Name = "ShowBinocole";
                    break;
                case 0x207:
                    com.Name = "207";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x208:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x20A:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x20D:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x20F:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x210:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x211:
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x212:
                    com.Name = "StorePokèmonNature";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x214:
                    com.Name = "StoreUGFriendNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x218:
                    com.Name = "StoreRandomPokèmonSearch";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x219:
                    com.Name = "ActRandomPokèmonSearch";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x21A:
                    com.Name = "CheckRandomPokèmonSearch";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x21D:
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
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x221:
                    com.Name = "StoreMoveTeacher";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x223:
                    com.Name = "StorePokèmonMoveTeacher";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x224:
                    com.Name = "TeachPokèmonMove";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x225:
                    com.Name = "StoreTeachingResult";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x226:
                    com.Name = "SetTradeId";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x228:
                    com.Name = "StoreChosenPokèmonTrade";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x229:
                    com.Name = "TradePokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22A:
                    com.Name = "EndTrade";
                    break;
                case 0x22C:
                    com.Name = "ActMFPokèdex";
                    break;
                case 0x22D:
                    com.Name = "StorePokèdex";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22F:
                    com.Name = "StoreRibbonNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x230:
                    com.Name = "StoreRibbonPokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x231:
                    com.Name = "GiveRibbonPokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x232:
                    com.Name = "SetVarRibbonPokèmon";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x234:
                    com.Name = "StoreDay";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x235:
                    com.Name = "StoreOtherFriendCode";
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
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x237:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x238:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x239:
                    com.Name = "239";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x23A:
                    com.Name = "StoreFootPokèmon";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x23B:
                    com.Name = "ShowPokèballPcAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x23C:
                    com.Name = "StoreLiftDirection";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x23D:
                    com.Name = "ShowShipAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x23E:
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
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x244:
                    com.Name = "StoreDoublePhraseBoxInput";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x245:
                    com.Name = "SetVarPhraseBoxInput";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x246:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x247:
                    com.Name = "StoreFirstPokèmonParty";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x248:
                    com.Name = "StoreDragonMovePokèmonParty";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x249:
                    com.Name = "StoreDoublePhraseBoxInput2";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24B:
                    com.Name = "24B";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x24C:
                    com.Name = "24C";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x24D:
                    com.Name = "24D";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x24E:
                    com.Name = "StorePokèLottoNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24F:
                    com.Name = "StorePokèLottoResults";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x251:
                    com.Name = "SetVarIdPokèmonBoxes";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x252:
                    com.Name = "CheckBoxSpace";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x253:
                    com.Name = "253";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x254:
                    com.Name = "StorePalParkNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x255:
                    com.Name = "SavePCPalParkPokèmon";
                    break;
                case 0x256:
                    com.Name = "StorePalParkPoint";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x25A:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x25D:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x260:
                    com.Name = "PlayAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x261:
                    com.Name = "SetVarAccessories";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x262:
                    com.Name = "CheckPokèmonCaught";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x263:
                    com.Name = "ChangePokèmonForm";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x264:
                    com.Name = "StoreBurmyFormsNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x26C:
                    com.Name = "StoreHappinessItem";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x26D:
                    com.Name = "26D";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x26E:
                    com.Name = "StorePalParkPokèmonCaught";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x267:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x268:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x270:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x273:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x276:
                    com.Name = "CheckCoinsBoxSpace";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x27A:
                    com.Name = "ShowPanoramicView";
                    break;
                case 0x27D:
                    com.Name = "SetVarTrendyWord";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x27F:
                    com.Name = "StoreTrendyWordStatus";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x280:
                    com.Name = "SetVarRandomPrize";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x281:
                    com.Name = "StorePokèContestFashion";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x282:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x284:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x285:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x289:
                    com.Name = "GivePoffinCaseItem";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x28A:
                    com.Name = "StorePoffinCaseNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x28B:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x28C:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x28E:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x28F:
                    com.Name = "StoreFirstTimePokèmonLeague";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x290:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x291:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x29C:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x29D:
                    com.Name = "MultiChoices";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x29E:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x29F:
                    com.Name = "BumpCameraAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2A0:
                    com.Name = "DoubleTrainerBattle";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2A3:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2A4:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2A5:
                    com.Name = "ShowPokèmonTradeMenu";
                    break;
                case 0x2A6:
                    com.Name = "StoreCasinoPrizeResult";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2A7:
                    com.Name = "CheckSpecialItem";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2A8:
                    com.Name = "2A8";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2A9:
                    com.Name = "CheckCasinoPrizeCoins";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2AA:
                    com.Name = "ComparePhraseBoxInput";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2AB:
                    com.Name = "CheckSeal";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2AC:
                    com.Name = "ActMisteryGift";
                    break;
                case 0x2AD:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2AF:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2B3:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2B5:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2B6:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2B7:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2BA:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2BC:
                    com.Name = "CheckLegendaryBattle";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2BD:
                    com.Name = "LegendaryBattle";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2BE:
                    com.Name = "StoreStarCardNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2C0:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2C1:
                    com.Name = "StartSavingGame";
                    break;
                case 0x2C2:
                    com.Name = "EndSavingGame";
                    break;
                case 0x2C4:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2C5:
                    com.Name = "2C5";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2C6:
                    com.Name = "SetVarUndergroundItem";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2CA:
                    com.Name = "SetVarStarterAlter";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x2CB:
                    com.Name = "SetVarStarterAccessories";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2CC:
                    com.Name = "SetVarWifiSprite";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2CD:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2CE:
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x2CF:
                    com.parameters.Add(reader.ReadByte());

                    break;
                case 0x2D1:
                    com.Name = "LiftAnimation";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x33A:
                    com.Name = "TexBoxVariable";
                    com.parameters.Add(reader.ReadByte());
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
                case 0x18:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
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
                    com.Name = "21";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22:
                    com.Name = "22";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x23:
                    com.Name = "23";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24:
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x25:
                    com.Name = "25";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x26:
                    com.Name = "26";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x27:
                    com.Name = "27";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x28:
                    com.Name = "28";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x29:
                    com.Name = "CopyVar";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2A:
                    com.Name = "CopyVar2";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x2B:
                    com.Name = "CopyVar3";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x2c:
                    com.Name = "Message";
                    com.parameters.Add(reader.ReadByte());
                    break;

                case 0x2D:
                    com.Name = "Message2";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x2e:
                    com.Name = "Message3";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x2f:
                    com.Name = "Message4";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x30:
                    com.Name = "Message5";
                    com.parameters.Add(reader.ReadByte());
                    break;

                case 0x31:
                    break;

                case 0x32:
                    com.Name = "WaitButton";
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

                //case 0x3D:
                //    com.Name = "ShowMenu";
                //    break;

                //case 0x3E:
                //    com.Name = "YesNoBox";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x3F:
                //    com.Name = "WaitFor";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x40:
                //    com.Name = "Multi";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x41:
                //    com.Name = "Multi2";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x42:
                //    com.Name = "SetTextScriptMulti";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadByte());
                //    break;

                //case 0x43:
                //    com.Name = "CloseMulti";
                //    break;

                //case 0x44:
                //    com.Name = "Multi3";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x45:
                //    com.Name = "SetTextScriptMessageMulti";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x46:
                //    com.Name = "CloseMulti2";
                //    break;
                //case 0x47:
                //    com.Name = "CloseMulti3";
                //    break;
                //case 0x48:
                //    com.Name = "SetTextRowMulti";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                case 0x49:
                    com.Name = "Fanfare";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x4A:
                    com.Name = "Fanfare2";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x4b:
                    com.Name = "WaitFanfare";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x4c:
                    com.Name = "Cry";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x4d:
                    com.Name = "WaitCry";
                    break;

                case 0x4e:
                    com.Name = "PlaySound";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x4f:
                    com.Name = "FadeDef";
                    break;

                case 0x50:
                    com.Name = "PlaySound2";
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

                //case 0x55:
                //    com.Name = "CheckSoundMicrophone";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x57:
                //    com.Name = "PlaySound3";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x58:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x59:
                //    com.Name = "CheckSoundMicrophone";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x5A:
                //    com.Name = "SwitchSound2";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x5b:
                //    com.Name = "ActivateMicrophone";
                //    break;

                //case 0x5c:
                //    com.Name = "DisactivateMicrophone";
                //    break;

                case 0x5e:
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

                case 0x5f:
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

                //case 0x6a:
                //    com.Name = "CheckOWPosition";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x6b:
                //    break;

                //case 0x6c:
                //    com.Name = "ContinueFollow";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x6d:
                //    com.Name = "FollowHero";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x6E:
                //    com.Name = "GiveMoney";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x6F:
                //    com.Name = "TakeMoney";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x70:
                //    com.Name = "CompareMoney";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x71:
                //    com.Name = "ShowMoney";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x72:
                //    com.Name = "HideMoney";
                //    break;
                //case 0x73:
                //    com.Name = "UpdateMoney";
                //    break;
                case 0x74:
                    com.Name = "74";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x75:
                    com.Name = "75";
                    break;

                case 0x76:
                    com.Name = "76";
                    com.parameters.Add(reader.ReadByte());
                    break;

                //case 0x77:
                //    com.Name = "CheckCoins";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x78:
                //    com.Name = "GiveCoins";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x79:
                //    com.Name = "TakeCoins";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x7b:
                //    com.Name = "7B";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x7c:
                //    com.Name = "7C";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                
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

                //case 0x85:
                //    com.Name = "CheckUndergroundPcStatus";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x89:
                //    com.Name = "GivePokèmon";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x8B:
                //    com.Name = "8B";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x8E:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x8f:
                //    com.Name = "SendItemType";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x90:
                    com.Name = "90";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x91:
                    com.Name = "ActPokèGearApplication";
                    com.parameters.Add(reader.ReadByte());
                    break;
                //case 0x92:
                //    com.Name = "RegisterPokèGearNumber";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x93:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                case 0x94:
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                //case 0x95:
                //    com.Name = "CheckPokèmonParty2";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                case 0x96:
                    com.Name = "CallEnd";
                    break;

                //case 0x97:
                //    break;

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

                //case 0xA1:
                //    com.Name = "CallEnd";
                //    break;

                //case 0xA3:
                //    com.Name = "StartWFC";
                //    break;

                //case 0xA5:
                //    com.Name = "StartInterview";
                //    break;

                //case 0xA6:
                //    com.Name = "StartDressPokèmon";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

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

                //case 0xAD:
                //    com.Name = "ChoosePokèmonName";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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

                //case 0xB8:
                //    com.Name = "CheckTypeBattle?";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0xB9:
                //    com.Name = "SetVarBattle2?";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0xBA:
                //    com.Name = "ChoosePlayerName";
                //    break;

                case 0xBB:
                    com.Name = "BB";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0xBC:
                    com.Name = "BC";
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0xBD:
                    com.Name = "BD";
                    break;
                case 0xBE:
                    com.Name = "SetVarHero";
                    com.parameters.Add(reader.ReadByte());
                    break;
                //case 0xBF:
                //    com.Name = "SetVarAlter";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0xC0:
                //    com.Name = "SurfAnimation";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0xC1:
                    com.Name = "C1";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC2:
                    com.Name = "SetVarItem";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC3:
                    com.Name = "SetVarItem2";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC5:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0xC6:
                //    com.Name = "SetVarNumber";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0xC7:
                    com.Name = "SetVarPokèmon";
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
                //case 0xCC:
                //    com.Name = "StopBerryHeroAnimation";
                //    break;
                //case 0xCD:
                //    com.Name = "SetVarHero";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0xCE:
                //    com.Name = "CE";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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
                //case 0xD5:
                //    com.Name = "TrainerBattle";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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
                //case 0xDB:
                //    com.Name = "TeleportPC";
                //    break; ;
                case 0xDC:
                    com.Name = "DC";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xDE:
                    com.Name = "DE";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0xE2:
                //    com.Name = "SetVarSwarmPlace";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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
                //case 0xF2:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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
                case 0x114:
                    com.Name = "114";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x119:
                //    com.Name = "119";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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
                //case 0x139:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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
                    com.Name = "CheckPokèmonPartyNumber";
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
                //case 0x15A:
                //    com.Name = "ActRunningShoes";
                //    break;
                //case 0x15B:
                //    com.Name = "CheckBadge";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x15C:
                //    com.Name = "SetBadge";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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
                //case 0x168:
                //    com.Name = "PrepareDoorAnimation";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0x169:
                //    com.Name = "WaitDoorOpeningAnimation";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
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
                //case 0x16E:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x176:
                    com.Name = "176";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x177:
                    com.Name = "177";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x17B:
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
                //case 0x17E:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x17F:
                //    com.Name = "17F";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x180:
                //    com.Name = "180";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0x181:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x182:
                    com.Name = "StoreHeroFaceOrientation";
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
                //case 0x193:
                //    com.Name = "CheckChosenPokèmon";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x196:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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
                //case 0x1AE:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1AF:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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
                    com.Name = "1B3";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1B5:
                    com.Name = "1B5";
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
                //case 0x1BF:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1C0:
                //    com.Name = "CheckSpecificPokèmonParty";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

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
                //case 0x1D6:
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0x1D7:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1D8:
                //    com.Name = "CheckChosenPokèmonTrade2";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1D9:
                //    com.Name = "TradePokèmon";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1DA:
                //    com.Name = "EndTrade";
                //    break;
                //case 0x1DF:
                //    com.Name = "1DF";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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
                //case 0x1EC:
                //    com.Name = "CheckSinglePhraseBoxInput";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;

                //case 0x1ED:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1EE:
                //    com.Name = "SetVarPhraseBoxInput";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1EF:
                //    com.Name = "1EF";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1F1:
                //    com.Name = "CheckFossilNumber";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1F4:
                //    com.Name = "CheckFossilPokèmon";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x1F5:
                //    com.Name = "TakeFossil";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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
                //case 0x1FC:
                //    com.Name = "1FC";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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
                //case 0x20A:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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
                    com.Name = "211";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x212:
                //    com.Name = "CheckPokèmonNature";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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
                //case 0x228:
                //    com.Name = "CheckChosenPokèmonTrade";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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
                //case 0x236:
                //    com.Name = "ShowPokèmonTradeMenu";
                //    break;
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
                //case 0x248:
                //    com.Name = "CheckDragonMovePokèmonParty";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x249:
                //    com.Name = "CheckDoublePhraseBoxInput2";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x24D:
                //    com.Name = "WildPokèmonBattle";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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
                //case 0x251:
                //    com.Name = "SetVarIdPokèmonBoxes";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x252:
                //    com.Name = "CheckBoxNumber";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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
                case 0x25A:
                    com.parameters.Add(reader.ReadUInt16());
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
                //case 0x282:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x284:
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x285:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x289:
                //    com.Name = "GivePoffinCaseItem";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x28A:
                //    com.Name = "CheckPoffinCaseNumber";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x28B:
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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
                //case 0x29C:
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x29D:
                //    com.Name = "MultiChoices";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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
                //    break;
                //case 0x2AA:
                //    com.Name = "ComparePhraseBoxInput";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2AB:
                //    com.Name = "2AB";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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
                //case 0x2CC:
                //    com.Name = "SetVarWifiSprite";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2CD:
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2CE:
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0x2CF:
                //    com.parameters.Add(reader.ReadByte());

                //    break;
                //case 0x2D1:
                //    com.Name = "LiftAnimation";
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadByte());
                //    break;
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
                    com.Name = "2ED";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2EF:
                    com.Name = "2EF";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x2F0:
                //    com.Name = "CloseMulti4";
                //    break;

                //case 0x2F6:
                //    com.Name = "2F6";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2F9:
                //    com.Name = "2F9";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x2FA:
                //    com.Name = "2FA";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x305:
                //    com.Name = "305";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                //case 0x306:
                //    com.Name = "306";
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x310:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                //case 0x316:
                //    com.Name = "316";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
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
                //case 0x33A:
                //    com.Name = "TexBoxVariable";
                //    com.parameters.Add(reader.ReadByte());
                //    break;
                //case 0x33C:
                //    com.Name = "33C";
                //    com.parameters.Add(reader.ReadUInt16());
                //    com.parameters.Add(reader.ReadByte());
                //    com.parameters.Add(reader.ReadUInt16());
                //    break;
                case 0x348:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
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
            if (functionOffset < reader.BaseStream.Length)
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
            switch (com.Id)
            {
                case 0x2:
                    com.Name = "End";
                    if (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        var next = reader.ReadUInt16();
                        if (next == 0)
                            com.isEnd = 1;
                        else
                            reader.BaseStream.Position -= 2;
                    }
                    break; 
                    //com.isEnd = 1;
                    //if (!functionOffsetList.Contains(functionOffset))
                    //{
                    //    functionOffsetList.Add(functionOffset);
                    //    Console.AppendText("\nA function is in: " + functionOffset.ToString());
                    //}
                    //break;
                case 0x03:
                    com.Name = "Return";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x04:
                    com.Name = "CallRoutine";
                    com.parameters.Add(reader.ReadUInt16());
                    var next2 = reader.ReadUInt16();
                    if (next2 == 0)
                        com.parameters.Add(next2);
                    else
                        reader.BaseStream.Position -= 2;
                    break;
                case 0x05:
                    com.Name = "EndFunction";
                    next2 = reader.ReadUInt16();
                    if (next2 == 0)
                        com.isEnd = 1;
                    else
                        reader.BaseStream.Position -= 2;
                    break;
                case 0x06:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x07:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x08:
                    com.Name = "CompareTo";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x09:
                    com.Name = "StoreVar";
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
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x11:
                    com.Name = "Condition";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x12:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x13:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x14:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x16:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x17:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19:
                    com.Name = "Compare";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1C:
                    com.Name = "CallStd";
                    com.parameters.Add(reader.ReadByte());//Standard Function Id.
                    com.parameters.Add(reader.ReadByte());
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
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22:
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
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x26:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x27:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x28:
                    com.Name = "SetVar";
                    com.parameters.Add(reader.ReadUInt16()); //Var as container
                    com.parameters.Add(reader.ReadUInt16()); //Value to store
                    break;
                case 0x29:
                    com.Name = "SetVar2";
                    com.parameters.Add(reader.ReadUInt16()); //Var as container
                    com.parameters.Add(reader.ReadUInt16()); //Value to store
                    break;
                case 0x2A:
                    com.Name = "SetVar3";
                    com.parameters.Add(reader.ReadUInt16()); //Var as container
                    com.parameters.Add(reader.ReadUInt16()); //Value to store
                    break;
                case 0x2B:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2D:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2E:
                    com.Name = "StartScript";
                    break;
                case 0x2F:
                    com.Name = "EndScript";
                    break;
                //case 0x30:
                //    com.Name = "WaitButton";
                //    break;
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
                    com.Name = "CloseEventMessage";
                    break;
                case 0x36:
                    com.Name = "CloseEventGreyMessage";
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
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x3C:
                    com.Name = "Message";
                    com.parameters.Add(reader.ReadByte()); //Costant
                    com.parameters.Add(reader.ReadByte()); //Costant
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id 
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    com.parameters.Add(reader.ReadUInt16()); //Message Type
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
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x4F:
                    com.Name = "SetVarItem2";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x50:
                    com.Name = "SetVarItem3";
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
                    com.Name = "SetVarPartyPokemon";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x54:
                    com.Name = "SetVarPartyPokemon2";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x55:
                    com.Name = "SetVar????";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x56:
                    com.Name = "SetVarType";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
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
                    com.Name = "SetVar????";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x5C:
                    com.Name = "SetVarNumber";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
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
                case 0x67:
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
                    break;
                case 0x6A:
                    com.Name = "6A";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16()); //Flag
                    break;
                case 0x6B:
                    com.Name = "AddPeople";
                    com.parameters.Add(reader.ReadUInt16()); //Npc Id
                    break;
                case 0x6C:
                    com.Name = "RemovePeople";
                    com.parameters.Add(reader.ReadUInt16()); //Npc Id
                    break;
                case 0x6D:
                    com.Name = "SetOwPosition";
                    com.parameters.Add(reader.ReadUInt16()); //Npc Id
                    com.parameters.Add(reader.ReadUInt16()); //X coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Face direction
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x6E:
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
                    com.Name = "Lock2";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    break;
                case 0x79:
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x7B:
                    com.Name = "MoveNpctoCoordinates";
                    com.parameters.Add(reader.ReadUInt16()); //Npc Id
                    com.parameters.Add(reader.ReadUInt16()); //X coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate
                    break;
                case 0x7E:
                    com.Name = "TeleportUpNPc";
                    com.parameters.Add(reader.ReadUInt16()); //Npc Id
                    break;
                case 0x80:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x82:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x83:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x84:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x85:
                    com.Name = "SingleTrainerBattle";
                    com.parameters.Add(reader.ReadUInt16()); //Trainer Id
                    com.parameters.Add(reader.ReadUInt16()); //2th Trainer Id (If 0x0 Single Battle)
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x86:
                    com.Name = "DoubleTrainerBattle";
                    com.parameters.Add(reader.ReadUInt16()); //If different from 0 start a strange Double
                    com.parameters.Add(reader.ReadUInt16()); //Trainer Id (If 0x0 2 vs 2 Battle)
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x8B:
                    com.Name = "PlayTrainerMusic";
                    break;
                case 0x8C:
                    com.Name = "EndBattle";
                    break;
                case 0x8D:
                    com.Name = "StoreBattleResult";
                    com.parameters.Add(reader.ReadUInt16()); //Variable as container.
                    break;
                case 0x8E:
                    com.Name = "LockBattle";
                    break;
                case 0x92:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x93:
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
                    com.parameters.Add(reader.ReadUInt16()); //Trainer ID
                    break;
                case 0x97:
                    com.Name = "StoreActiveTrainerId";
                    com.parameters.Add(reader.ReadUInt16()); //Trainer ID
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x98:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x9F:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xA2:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xA3:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xA5:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xA6:
                    com.Name = "Fanfare";
                    com.parameters.Add(reader.ReadUInt16()); //Sound Id
                    break;
                case 0xA8:
                    com.Name = "StopFanfare";
                    break;
                case 0xA9:
                    com.Name = "PlaySound";
                    com.parameters.Add(reader.ReadUInt16()); //Sound Id
                    break;
                case 0xAB:
                    com.Name = "Cry";
                    com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16());
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
                case 0xB2:
                    com.Name = "Multi2";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte());

                    break;
                case 0xB3:
                    com.Name = "FadeScreen";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xB4:
                    com.Name = "ResetScreen";
                    break;
                case 0xB5:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xB6:
                    break;

                case 0xB7:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xBA:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xBB:
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
                    com.Name = "UnionWarp";
                    break;
                case 0xC4:
                    com.Name = "TeleportWarp";
                    com.parameters.Add(reader.ReadUInt16()); //Map Id
                    com.parameters.Add(reader.ReadUInt16()); // X coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Y coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Z coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Hero's Facing
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC5:
                    com.Name = "SurfAnimation";
                    break;
                case 0xC6:
                    com.Name = "RockClimbAnimation";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC7:
                    com.Name = "CutAnimation";
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
                case 0xCF:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xCD:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD0:
                    com.Name = "StoreDay";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD2:
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
                case 0xDA:
                    com.Name = "DA";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE0:
                    com.Name = "StoreVersion";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE1:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE7:
                    com.Name = "ActivateRelocator";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF9:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xFA:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xFB:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xFC:
                    com.Name = "StorePokèmonPartyHappiness";
                    com.parameters.Add(reader.ReadUInt16()); //Return Value
                    com.parameters.Add(reader.ReadUInt16()); //Id Pokèmon Party
                    break;
                case 0xFE:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xFF:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x103:
                    com.Name = "StorePokémonPartyNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x106:
                    com.Name = "106";
                    com.parameters.Add(reader.ReadUInt16()); //Req
                    break;
                case 0x107:
                    com.Name = "ShowChoosePokèmonMenu";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Dialog Result
                    com.parameters.Add(reader.ReadUInt16()); //RV = Pokèmon Id
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x10A:
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
                    break;
                case 0x114:
                    com.Name = "114";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x118:
                    com.Name = "118";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x11E:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x11F:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x120:
                    com.Name = "120";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x121:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x127:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x128:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x129:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x12A:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x12B:
                    com.parameters.Add(reader.ReadUInt16()); //Req
                    com.parameters.Add(reader.ReadUInt16()); //Req
                    com.parameters.Add(reader.ReadUInt16()); //Req
                    break;
                case 0x136:
                    com.Name = "SetWeather";
                    com.parameters.Add(reader.ReadByte()); //Weather Id,
                    break;
                case 0x13B:
                    com.parameters.Add(reader.ReadByte());
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
                case 0x147:
                    com.Name = "ResetCamera";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x148:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x14B:
                    com.Name = "14B";
                    break;
                case 0x14E:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x15A:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x167:
                    com.Name = "ShowPokèmonMusicalData";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x168:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x169:
                    com.Name = "StoreChosenPokèmonMusical";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x16A:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte());
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
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x16D:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x16E:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x178:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x17B:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x17C:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x182:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x185:
                    com.Name = "NimbasaGymFunction";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x186:
                    com.Name = "NimbasaGymFunction2";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x187:
                    com.Name = "NimbasaGymFunction3";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x188:
                    com.Name = "NimbasaGymFunction4";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x189:
                    com.Name = "NimbasaGymFunction5";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x190:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x191:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x192:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19B:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19E:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19F:
                    com.Name = "CallScreenAnimation";
                    com.parameters.Add(reader.ReadUInt16()); //AnimationId
                    break;
                case 0x1A7:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1A8:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1A9:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1AA:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1BA:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1BD:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1BE:
                    com.Name = "1BE";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1BF:
                    com.Name = "CompareChosenPokemon";
                    com.parameters.Add(reader.ReadUInt16());//RV = True if is equal
                    com.parameters.Add(reader.ReadUInt16());//Id chosen Pokèmon
                    com.parameters.Add(reader.ReadUInt16());//Id requested Pokèmon
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

                case 0x1C6:
                    com.Name = "StorePokemonCaughtWF";
                    com.parameters.Add(reader.ReadUInt16()); //True if is Pokèmon searched
                    com.parameters.Add(reader.ReadUInt16()); //True if is caught the same day
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1C9:
                    com.Name = "StoreVarMessage";
                    com.parameters.Add(reader.ReadUInt16()); //Variable as Container
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    break;
                case 0x1CB:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1CD:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1CE:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1CF:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1D1:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1EA:
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
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1EE:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1EF:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F0:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F3:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F6:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F7:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F8:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1FB:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1FC:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x200:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x202:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x205:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x207:
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
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x20C:
                    com.Name = "StorePasswordClown";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //True if inserted password is exact.
                    break;
                case 0x20E:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x20F:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x215:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x227:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x229:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22A:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22B:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22C:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22D:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x22F:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x230:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x233:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x234:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x23A:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;

                case 0x23D:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //RV = Message Id
                    break;
                case 0x242:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x248:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x249:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24C:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24A:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24E:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24F:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x253:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x254:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x25C:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x276:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x287:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2A1:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2B2:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2D1:
                    com.parameters.Add(reader.ReadUInt16());
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
                            printCommand(actualFunc, scriptBox,false);
                            lastIndexFunction++;
                        }
                        else
                        {
                            goto End3;
                        }
                    }
                End3:
                    for (int movCounter = lastIndexMov; movCounter < movList.Count -1; movCounter++)
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

        public void printSimplifiedScript(RichTextBox scriptBoxEditor, RichTextBox scriptBoxViewer,int scriptType, ComboBox subScripts)
        {
            
            scriptBoxEditor.Clear();
            visitedLine.Clear();
            var scriptsLine = scriptBoxViewer.Lines;
            for (int lineCounter =0; lineCounter<scriptsLine.Length; lineCounter++)
            {
                var line = scriptsLine[lineCounter];
                if (line.Contains("= Script " + subScripts.SelectedItem + " ")){
                    scriptBoxEditor.AppendText("\n" + line + "\n\n");
                    varNameDictionaryList = new List<Dictionary<int, string>>();
                    varNameDictionaryList.Add(new Dictionary<int, string>());
                    lineCounter++;
                    getCommands(scriptsLine, ref lineCounter, ref line, " ", visitedLine,scriptBoxEditor,scriptType);
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
            switch (com.Id)
            {
                case 0x2:
                    com.Name = "End";
                    com.isEnd = 1;
                    //if (!functionOffsetList.Contains(functionOffset))
                    //{
                    //    functionOffsetList.Add(functionOffset);
                    //    Console.AppendText("\nA function is in: " + functionOffset.ToString());
                    //}
                    break;
                case 0x03:
                    com.Name = "Return";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x04:
                    com.Name = "CallRoutine";
                    com.parameters.Add(reader.ReadUInt16());
                    var next = reader.ReadUInt16();
                    if (next == 0)
                        com.parameters.Add(next);
                    else
                        reader.BaseStream.Position -= 2;
                    break;
                case 0x05:
                    com.Name = "End";
                    com.isEnd = 1;
                    //if (!functionOffsetList.Contains(functionOffset))
                    //{
                    //    functionOffsetList.Add(functionOffset);
                    //    Console.AppendText("\nA function is in: " + functionOffset.ToString());
                    //}
                    break;
                case 0x06:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x07:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x08:
                    com.Name = "CompareTo";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x09:
                    com.Name = "StoreVar";
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
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x11:
                    com.Name = "Condition";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x12:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x13:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x14:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x16:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x17:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19:
                    com.Name = "Compare";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1C:
                    com.Name = "CallStd";
                    com.parameters.Add(reader.ReadByte());//Standard Function Id.
                    com.parameters.Add(reader.ReadByte());
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
                case 0x26:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x28:
                    com.Name = "StoreVarValue";
                    com.parameters.Add(reader.ReadUInt16()); //Var as container
                    com.parameters.Add(reader.ReadUInt16()); //Value to store
                    break;
                case 0x29:
                    com.Name = "StoreVarValue";
                    com.parameters.Add(reader.ReadUInt16()); //Var as container
                    com.parameters.Add(reader.ReadUInt16()); //Value to store
                    break;
                case 0x2A:
                    com.Name = "StoreVarValue2";
                    com.parameters.Add(reader.ReadUInt16()); //Var as container
                    com.parameters.Add(reader.ReadUInt16()); //Value to store
                    break;
                case 0x2B:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2D:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2E:
                    com.Name = "StartScript";
                    break;
                case 0x2F:
                    com.Name = "EndScript";
                    break;
                //case 0x30:
                //    com.Name = "WaitButton";
                //    break;
                case 0x32:
                    com.Name = "WaitKeyPress";
                    break;
                case 0x34:
                    com.Name = "EventGreyMessage";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    break;
                case 0x35:
                    com.Name = "CloseEventMessage";
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
                    break;
                case 0x3B:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x3C:
                    com.Name = "Message";
                    com.parameters.Add(reader.ReadByte()); //Costant
                    com.parameters.Add(reader.ReadByte()); //Costant
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id 
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    com.parameters.Add(reader.ReadUInt16()); //Message Type
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
                    com.Name = "CloseMessage";
                    break;
                case 0x3F:
                    com.Name = "CloseMessage2";
                    break;
                case 0x40:
                    com.Name = "MoneyBox";
                    com.parameters.Add(reader.ReadUInt16()); //X coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate
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
                    com.parameters.Add(reader.ReadUInt16()); //Message Type
                    break;
                case 0x49:
                    com.Name = "Message4";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x4A:
                    com.Name = "AngryMessage";
                    com.parameters.Add(reader.ReadUInt16()); //MessageId
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    break;
                case 0x4C:
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x4D:
                    com.Name = "StoreTextItem2";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x4E:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x4F:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x50:
                    com.Name = "StoreTextItem2";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x52:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x56:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x57:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x5C:
                    com.Name = "StoreTextTrainerID";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x60:
                    com.Name = "ApplyMovement";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt32()); //Movement Offset
                    var movOffset = com.parameters[1] + (uint)reader.BaseStream.Position;
                    if (!movOffsetList.Contains(movOffset))
                    {
                        movOffsetList.Add(movOffset);
                        Console.AppendText("\nA movement is in: " + movOffset.ToString());
                    }
                    break;
                case 0x61:
                    com.Name = "WaitMovement";
                    break;
                case 0x67:
                    com.parameters.Add(reader.ReadUInt16());
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
                    break;
                case 0x6A:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x6B:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x6C:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x6D:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x6E:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x74:
                    com.Name = "FacePlayer";
                    break;
                case 0x77:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x79:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x85:
                    com.Name = "TrainerBattle";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x8C:
                    com.Name = "EndBattle";
                    break;
                case 0x8D:
                    com.Name = "StoreBattleResult";
                    com.parameters.Add(reader.ReadUInt16()); //Variable as container.
                    break;
                case 0x8E:
                    com.Name = "LockBattle";
                    break;
                case 0x98:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xA2:
                    com.Name = "PlaySound";
                    com.parameters.Add(reader.ReadUInt16()); //Sound Id
                    break;
                case 0xA8:
                    com.Name = "StopSound";
                    break;
                case 0xA9:
                    com.Name = "Fanfare";
                    com.parameters.Add(reader.ReadUInt16()); //Sound Id
                    break;
                case 0xAB:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xAF:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xB3:
                    com.Name = "FadeScreen";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xB4:
                    com.Name = "ResetScreen";
                    break;
                case 0xB5:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xB7:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xBA:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xBB:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xC1:
                    com.Name = "FallWarp";
                    com.parameters.Add(reader.ReadUInt16()); //Map Id
                    com.parameters.Add(reader.ReadUInt16()); // X coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Y coordinate
                    break;
                case 0xC2:
                    com.Name = "TeleportWarp";
                    com.parameters.Add(reader.ReadUInt16()); //Map Id
                    com.parameters.Add(reader.ReadUInt16()); // X coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Y coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Hero's Facing
                    break;
                case 0xC3:
                    com.Name = "UnionWarp";
                    break;
                case 0xC4:
                    com.Name = "TeleportWarp";
                    com.parameters.Add(reader.ReadUInt16()); //Map Id
                    com.parameters.Add(reader.ReadUInt16()); // X coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Y coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Z coordinate
                    com.parameters.Add(reader.ReadUInt16()); // Hero's Facing
                    break;
                case 0xCB:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xCC:
                    com.Name = "StoreVarItem";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xCF:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xCD:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xD2:
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
                case 0xE0:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE1:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xE7:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xF9:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0xFF:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x103:
                    com.Name = "StorePokémonPartyNumber";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x10A:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x10C:
                    com.Name = "ShowPokèmonParty";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x11E:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x11F:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x127:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x128:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x129:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x12A:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x136:
                    com.Name = "SetWeather";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x13F:
                    //com.parameters.Add(reader.ReadByte());
                    //com.parameters.Add(reader.ReadByte());
                    //com.parameters.Add(reader.ReadUInt16());
                    //com.parameters.Add(reader.ReadUInt16());
                    //com.parameters.Add(reader.ReadUInt16());
                    //com.parameters.Add(reader.ReadUInt16());
                    break;


                case 0x143:
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x147:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x14B:
                    com.Name = "RestartGame";
                    break;

                case 0x167:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x16B:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x17B:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x186:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x187:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x188:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x189:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x190:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x191:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x192:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x19F:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1A7:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1A8:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1A9:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1AA:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1C2:
                    com.Name = "StoreEventBC";
                    break;
                case 0x1C3:
                    com.Name = "EndEventBC";
                    break;
                case 0x1C4:
                    com.Name = "StoreTrainerIDBC";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1C9:
                    com.Name = "StoreVarMessageBC";
                    com.parameters.Add(reader.ReadUInt16()); //Variable as Container
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    break;
                case 0x1EA:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1EC:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1ED:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x1F0:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x209:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x20C:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x215:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x229:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x23D:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24C:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24E:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x24F:
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2CD:
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2D1:
                    com.parameters.Add(reader.ReadUInt16());
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
                    if (scriptType == DPSCRIPT)
                    {
                        try
                        {
                            Utils.getCommandSimplifiedDPP(scriptsLine, lineCounter, space, scriptBoxEditor, varNameDictionaryList, 0,visitedLine,scriptType, textFile);
                        }
                        catch
                        {
                        }
                    }
                    else if (scriptType == BW2SCRIPT || scriptType == BWSCRIPT)
                        Utils.getCommandSimplifiedBW(scriptsLine, lineCounter, space, scriptBoxEditor, varNameDictionaryList, 0, visitedLine,scriptType,textFile);
                    else if (scriptType == HGSSSCRIPT)
                        Utils.getCommandSimplifiedHGSS(scriptsLine, lineCounter, space, scriptBoxEditor, varNameDictionaryList, 0, visitedLine, scriptType, textFile);
                
                lineCounter++;
                if ((scriptType == BWSCRIPT || scriptType == BW2SCRIPT))
                {
                    if (line.Contains(" Jump ") || line.Contains("EndScript") || line.Contains("EndFunction"))
                        return;
                    if (line.Contains("End") && scriptsLine[lineCounter] == "")
                        return;
                }
                if ((scriptType == 0 || scriptType == 1 ) && (line.Contains(" End ") || line.Contains("KillScript") || (line.Contains("Jump") && scriptsLine[lineCounter] == "" )))
                    return;
                if ((scriptType == 2) && (line.Contains(" End ") || line.Contains("KillScript") || (line.Contains("Jump") && scriptsLine[lineCounter] == "")))
                    return;
            } while ( lineCounter < scriptsLine.Length);
        }




        private Script_s printCommand(Script_s script, RichTextBox scriptBox, bool isScript)
        {
            foreach (Commands_s command in script.commands)
            {
                //if (functionOffsetList.Contains((uint)command.offset) && isScript)
                //    return script;
                if (command.Name != null)
                    scriptBox.AppendText("Offset: " + command.offset.ToString() + " " + command.Name + " ");
                else
                    scriptBox.AppendText("Offset: " + command.offset.ToString() + " " + command.Id.ToString("X") + " ");
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


                ////else if ( scriptBox.AppendText(StoreConditionCommand(command.parameters[0]) + getFunction(StoreHex((int)command.parameters[1])));.Name == "Compare" || command.Name == "Compare2")
                ////    scriptBox.AppendText("0x" + command.parameters[0].ToString("X") + " 0x" + command.parameters[1].ToString());
                else if (command.Name == "Message2" || command.Name == "Message" || command.Name == "Message3")
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
                                if (command.Name == "Message")
                                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + StoreHex((int)command.parameters[5]));
                                else if (command.Name == "Message2")
                                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]));
                                else if (command.Name == "Message3")
                                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + StoreHex((int)command.parameters[5]) + StoreHex((int)command.parameters[6]));
                            }
                        }
                    }
                    else
                        scriptBox.AppendText(command.parameters[0].ToString());
                }
                //else if (command.Name == "DoubleMessage")
                //{

                //    if (scriptType == BWSCRIPT || scriptType == BW2SCRIPT)
                //    {
                //        idMessage = (int)command.parameters[2];
                //        int idMessage2 = (int)command.parameters[3];
                //        if (textFile != null)
                //        {
                //           //scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + StoreHex((int)command.parameters[5]) + " = ' " + textFile.messageList[idMessage] + " '\n");
                //           // scriptBox.AppendText("                                                              " + "= ' " + textFile.messageList[idMessage2] + " '");
                //        }
                //        else
                //            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + StoreHex((int)command.parameters[3]) + StoreHex((int)command.parameters[4]) + StoreHex((int)command.parameters[5]));
                //    }
                //    else
                //    {
                //        idMessage = (int)command.parameters[0];
                //        int idMessage2 = (int)command.parameters[1];
                //        if (textFile != null)
                //        {
                //            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + " = ' " + textFile.textList[idMessage].text + " '\n");
                //            scriptBox.AppendText("\n                                                " + StoreHex((int)command.parameters[1]) + "= ' " + textFile.textList[idMessage2].text + " '");
                //        }
                //        else
                //            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                //    }

                //}
                else if (command.Name == "BubbleMessage")
                {
                    idMessage = (int)command.parameters[0];
                    int idMessage2 = (int)command.parameters[1];
                    if (textFile != null)
                       // scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + textFile.messageList[idMessage] + " '");
                    //else
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                }
                else if (command.Name == "AngryMessage")
                {
                    idMessage = (int)command.parameters[0];
                    int idMessage2 = (int)command.parameters[1];
                    if (textFile != null)
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + "= ' " + textFile.messageList[idMessage] + " '");
                    else
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]));
                }
                else if (command.Name == "EventGreyMessage")
                {
                    idMessage = (int)command.parameters[0];
                    int idMessage2 = (int)command.parameters[1];
                    if (textFile != null)
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + textFile.messageList[idMessage] + " '");
                    else
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                }
                else if (command.Name == "BorderedMessage")
                {
                    idMessage = (int)command.parameters[0];
                    int idMessage2 = (int)command.parameters[1];
                    if (textFile != null)
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + textFile.messageList[idMessage] + " '");
                    else
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
                }
                else if (command.Name == "CallTextMessageBox")
                {
                    idMessage = (int)command.parameters[0];
                    if (scriptType != BWSCRIPT && scriptType != BW2SCRIPT)
                    {
                        //if (textFile != null)
                            //scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + "= ' " + textFile.textList[idMessage].text + " '");
                       // else
                         //   scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
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
                        if (textFile != null)
                            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + "= ' " + textFile.messageList[idMessage] + " '");
                        else
                            scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]));
                    }
                    else
                    {
                        idMessage = (int)command.parameters[1];
                        if (idMessage < 0x4000 && idMessage < textFile.textList.Count)
                        {
                            if (textFile != null)
                                scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + "= ' " + textFile.textList[idMessage].text + " '");
                            else
                                scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]));
                        }
                    }
                }
                else if (command.Name == "SetTextScriptMessage")
                {
                    idMessage = (int)command.parameters[0];
                    if (textFile != null)
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]) + "= ' " + textFile.messageList[idMessage] + " '");
                    else
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]) + StoreHex((int)command.parameters[2]));
                }
                else if (command.Name == "ShowMessageAt")
                {
                    idMessage = (int)command.parameters[0];
                    if (textFile != null)
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
                    idMessage = (int)command.parameters[0];
                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + StoreHex((int)command.parameters[1]));
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

            return parameter;
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
            return "UNK";
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
                com = readCommandBW(reader, com);
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
            textFile = null;
            scriptBoxViewer.Clear();
            int selectedIndex = listScript.SelectedIndex;
            int idScript = selectedIndex;
            int idMessage = -1;
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
                }
                else if (scriptType == 2)
                {
                    if (idScript == 3)
                        idMessage = 40;
                }

                idTextFile = idMessage;
                if (idMessage != -1)
                {
                    var textFiles = textNarc.figm.fileData[idMessage];
                    int textType = 0;
                    if (scriptType == BWSCRIPT || scriptType == BW2SCRIPT)
                        textType = 1;
                    var textHandler = new Texts(textFiles, textType);
                    textFile = textHandler;
                }
            }
            ClosableMemoryStream stream = actualNarc.figm.fileData[selectedIndex];
            actualFileId = selectedIndex;
            loadScript(stream);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileStream text = File.Create(Directory.GetCurrentDirectory() + "textScriptDump.txt");
            var writer = new BinaryWriter(text);
            int fileCounter = 0;
            foreach (ClosableMemoryStream stream in actualNarc.figm.fileData)
            {
                writer.Write("FILE: " + fileCounter + "\r\r");
                if (scriptType == DPSCRIPT)
                    if (fileCounter < 465 || fileCounter > 900)
                        loadScript(stream);
                writer.Write(scriptBoxViewer.Text);
                fileCounter++;
            }



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
            printSimplifiedScript(scriptBoxEditor,scriptBoxViewer,scriptType, subScripts);
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
