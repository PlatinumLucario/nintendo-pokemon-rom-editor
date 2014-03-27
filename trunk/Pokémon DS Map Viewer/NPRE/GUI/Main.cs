
namespace PG4Map
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;
    using PG4Map.Formats;
    using NPRE.Archive;
    using NPRE.Formats.Specific.Pokémon.Maps;
    using NPRE.Formats.Specific.Pokémon.Scripts;

    public class Main : Form
    {
        #region Fields

        #region Stream
        public Stream ROM;
        public ClosableMemoryStream actualDec;
        public ClosableMemoryStream actualNode;
        public ClosableMemoryStream actualSubNode;
        #endregion

        #region Elements
        public Maps actualMap;
        public Nsbmd actualModel;
        public Narc actualNarc;
        public Nds actualNds;
        public abEditor.AB_s actualAb;
        public NCLR.NCLR_s actualPalette;
        public NCGR.NCGR_s actualTile;
        public List<PG4Map.MapEditor.tableRow> actualTable;
        #endregion

        #region Other GUIs
        public abEditor abEditor;
        public MapEditor mapEditor;
        #endregion

        #region Checkers
        public int AreinDec = 0;
        public int AreinNamp = 0;
        public int AreinNarc = 0;
        public int AreinNormal = 0;
        #endregion

        #region Constants
        public const short GENERAL_DIRECTORY = 0;
        public const short NARC_FILE = 255;
        public const short AB_FILE = 2;
        public const short DPPMAP = 0;
        public const short HGSSMAP = 1;
        public const short BWMAP = 2;
        public const short NSBMD_MODEL = 3;
        public const short MAXMOVSIZE = 32;

        #endregion





        #endregion

        #region GUI

        private Button button2;
        private Button scriptButton;
        private ContextMenuStrip fileSystemContextMenu;
        private IContainer components;
        private ToolStripMenuItem decompressToolStripMenuItem;
        private ToolStripMenuItem eventsToolStripMenuItem;
        private ToolStripMenuItem extractToolStripMenuItem;
        private GroupBox groupBox1;
        private Button HexButton;
        private ToolStripMenuItem hexToolStripMenuItem;
        public static Hex_Viewer HexViewer;
        private ToolStripMenuItem huffmanToolStripMenuItem;
        private ToolStripMenuItem importToolStripMenuItem;
        private DataGridView InfoFile;
        private ToolStripMenuItem lZ77ToolStripMenuItem;
        private ToolStripMenuItem lZToolStripMenuItem;
        private Button MapButton;

        private ToolStripMenuItem mapToolStripMenuItem;
        private ToolStripMenuItem mapToolStripMenuItem2;
        public static MapEditor MapViewer;
        private ToolStripMenuItem openAsToolStripMenuItem;
        private ToolStripMenuItem openNarcToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem paletteToolStripMenuItem1;
        private DataGridViewTextBoxColumn Property;
        private ToolStripMenuItem rLEToolStripMenuItem;
        private ToolStripMenuItem saveNarcToolStripMenuItem;
        private ToolStripMenuItem scriptToolStripMenuItem;
        private ToolStripMenuItem setAsToolStripMenuItem;
        public static ClosableMemoryStream Sub_File;
        public TreeView Sys;
        public static List<ClosableMemoryStream> Temp_File;
        private ToolStripMenuItem tileToolStripMenuItem;
        public static TileView TileViewer;
        public ToolStrip toolStrip;
        private ToolStripDropDownButton toolStripComboBox1;
        public ToolStripSeparator toolStripSeparator;
        private ToolStripMenuItem listOfMapsToolStripMenuItem;
        private DataGridViewTextBoxColumn Value;
        private ToolStripMenuItem textureToolStripMenuItem;
        private Label label7;
        private RichTextBox ConsoleLog;
        private ToolStripMenuItem narcScriptToolStripMenuItem;
        private Button button1;
        private ToolStripMenuItem textToolStripMenuItem;
        private ToolStripMenuItem narcTextToolStripMenuItem;
        private Button button3;
        private Nsbtx selectedTex;
        private ToolStripMenuItem matrixToolStripMenuItem;
        private ToolStripMenuItem narcMatrixToolStripMenuItem;
        private int idTextFile;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.openNarcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveNarcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.Sys = new System.Windows.Forms.TreeView();
            this.fileSystemContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.matrixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eventsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listOfMapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.narcScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.narcTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.narcMatrixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decompressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lZ77ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lZToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rLEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.huffmanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.paletteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mapToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.textureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InfoFile = new System.Windows.Forms.DataGridView();
            this.Property = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HexButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.scriptButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.MapButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.ConsoleLog = new System.Windows.Forms.RichTextBox();
            this.toolStrip.SuspendLayout();
            this.fileSystemContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InfoFile)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBox1,
            this.toolStripSeparator});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(727, 25);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openNarcToolStripMenuItem,
            this.saveNarcToolStripMenuItem});
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(38, 22);
            this.toolStripComboBox1.Text = "File";
            // 
            // openNarcToolStripMenuItem
            // 
            this.openNarcToolStripMenuItem.Name = "openNarcToolStripMenuItem";
            this.openNarcToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.openNarcToolStripMenuItem.Text = "Open....";
            this.openNarcToolStripMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // saveNarcToolStripMenuItem
            // 
            this.saveNarcToolStripMenuItem.Name = "saveNarcToolStripMenuItem";
            this.saveNarcToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.saveNarcToolStripMenuItem.Text = "Save...";
            this.saveNarcToolStripMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // Sys
            // 
            this.Sys.Location = new System.Drawing.Point(12, 38);
            this.Sys.Name = "Sys";
            this.Sys.Size = new System.Drawing.Size(462, 567);
            this.Sys.TabIndex = 3;
            this.Sys.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FileSystemNode_AfterSelect);
            // 
            // fileSystemContextMenu
            // 
            this.fileSystemContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.openAsToolStripMenuItem,
            this.decompressToolStripMenuItem,
            this.extractToolStripMenuItem,
            this.importToolStripMenuItem,
            this.setAsToolStripMenuItem});
            this.fileSystemContextMenu.Name = "contextMenuStrip1";
            this.fileSystemContextMenu.Size = new System.Drawing.Size(154, 136);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Enabled = false;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // openAsToolStripMenuItem
            // 
            this.openAsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapToolStripMenuItem,
            this.matrixToolStripMenuItem,
            this.hexToolStripMenuItem,
            this.eventsToolStripMenuItem,
            this.scriptToolStripMenuItem,
            this.textToolStripMenuItem,
            this.listOfMapsToolStripMenuItem,
            this.narcScriptToolStripMenuItem,
            this.narcTextToolStripMenuItem,
            this.narcMatrixToolStripMenuItem});
            this.openAsToolStripMenuItem.Name = "openAsToolStripMenuItem";
            this.openAsToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.openAsToolStripMenuItem.Text = "Open as";
            // 
            // mapToolStripMenuItem
            // 
            this.mapToolStripMenuItem.Name = "mapToolStripMenuItem";
            this.mapToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.mapToolStripMenuItem.Text = "Map";
            this.mapToolStripMenuItem.Click += new System.EventHandler(this.mapToolStripMenuItem_Click);
            // 
            // matrixToolStripMenuItem
            // 
            this.matrixToolStripMenuItem.Name = "matrixToolStripMenuItem";
            this.matrixToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.matrixToolStripMenuItem.Text = "Matrix";
            this.matrixToolStripMenuItem.Click += new System.EventHandler(this.matrixToolStripMenuItem_Click);
            // 
            // hexToolStripMenuItem
            // 
            this.hexToolStripMenuItem.Name = "hexToolStripMenuItem";
            this.hexToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.hexToolStripMenuItem.Text = "Hex";
            this.hexToolStripMenuItem.Click += new System.EventHandler(this.hexToolStripMenuItem_Click);
            // 
            // eventsToolStripMenuItem
            // 
            this.eventsToolStripMenuItem.Name = "eventsToolStripMenuItem";
            this.eventsToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.eventsToolStripMenuItem.Text = "Events";
            this.eventsToolStripMenuItem.Click += new System.EventHandler(this.eventsToolStripMenuItem_Click);
            // 
            // scriptToolStripMenuItem
            // 
            this.scriptToolStripMenuItem.Name = "scriptToolStripMenuItem";
            this.scriptToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.scriptToolStripMenuItem.Text = "Script";
            this.scriptToolStripMenuItem.Click += new System.EventHandler(this.scriptToolStripMenuItem_Click);
            // 
            // textToolStripMenuItem
            // 
            this.textToolStripMenuItem.Name = "textToolStripMenuItem";
            this.textToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.textToolStripMenuItem.Text = "Text";
            this.textToolStripMenuItem.Click += new System.EventHandler(this.textToolStripMenuItem_Click);
            // 
            // listOfMapsToolStripMenuItem
            // 
            this.listOfMapsToolStripMenuItem.Name = "listOfMapsToolStripMenuItem";
            this.listOfMapsToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.listOfMapsToolStripMenuItem.Text = "Narc (3D)";
            this.listOfMapsToolStripMenuItem.Click += new System.EventHandler(this.listOfMapsToolStripMenuItem_Click);
            // 
            // narcScriptToolStripMenuItem
            // 
            this.narcScriptToolStripMenuItem.Name = "narcScriptToolStripMenuItem";
            this.narcScriptToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.narcScriptToolStripMenuItem.Text = "Narc (Script)";
            this.narcScriptToolStripMenuItem.Click += new System.EventHandler(this.narcScriptToolStripMenuItem_Click);
            // 
            // narcTextToolStripMenuItem
            // 
            this.narcTextToolStripMenuItem.Name = "narcTextToolStripMenuItem";
            this.narcTextToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.narcTextToolStripMenuItem.Text = "Narc (Text)";
            this.narcTextToolStripMenuItem.Click += new System.EventHandler(this.narcTextToolStripMenuItem_Click);
            // 
            // narcMatrixToolStripMenuItem
            // 
            this.narcMatrixToolStripMenuItem.Name = "narcMatrixToolStripMenuItem";
            this.narcMatrixToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.narcMatrixToolStripMenuItem.Text = "Narc (Matrix)";
            // 
            // decompressToolStripMenuItem
            // 
            this.decompressToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lZ77ToolStripMenuItem,
            this.lZToolStripMenuItem,
            this.rLEToolStripMenuItem,
            this.huffmanToolStripMenuItem});
            this.decompressToolStripMenuItem.Name = "decompressToolStripMenuItem";
            this.decompressToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.decompressToolStripMenuItem.Text = "Decompress as";
            // 
            // lZ77ToolStripMenuItem
            // 
            this.lZ77ToolStripMenuItem.Name = "lZ77ToolStripMenuItem";
            this.lZ77ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.lZ77ToolStripMenuItem.Text = "LZ77";
            this.lZ77ToolStripMenuItem.Click += new System.EventHandler(this.lZ77ToolStripMenuItem_Click);
            // 
            // lZToolStripMenuItem
            // 
            this.lZToolStripMenuItem.Enabled = false;
            this.lZToolStripMenuItem.Name = "lZToolStripMenuItem";
            this.lZToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.lZToolStripMenuItem.Text = "LZ";
            // 
            // rLEToolStripMenuItem
            // 
            this.rLEToolStripMenuItem.Enabled = false;
            this.rLEToolStripMenuItem.Name = "rLEToolStripMenuItem";
            this.rLEToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.rLEToolStripMenuItem.Text = "RLE";
            // 
            // huffmanToolStripMenuItem
            // 
            this.huffmanToolStripMenuItem.Enabled = false;
            this.huffmanToolStripMenuItem.Name = "huffmanToolStripMenuItem";
            this.huffmanToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.huffmanToolStripMenuItem.Text = "Huffman";
            // 
            // extractToolStripMenuItem
            // 
            this.extractToolStripMenuItem.Name = "extractToolStripMenuItem";
            this.extractToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.extractToolStripMenuItem.Text = "Extract";
            this.extractToolStripMenuItem.Click += new System.EventHandler(this.extractToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Enabled = false;
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.importToolStripMenuItem.Text = "Import";
            // 
            // setAsToolStripMenuItem
            // 
            this.setAsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tileToolStripMenuItem,
            this.paletteToolStripMenuItem1,
            this.mapToolStripMenuItem2,
            this.textureToolStripMenuItem});
            this.setAsToolStripMenuItem.Name = "setAsToolStripMenuItem";
            this.setAsToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.setAsToolStripMenuItem.Text = "Set as";
            // 
            // tileToolStripMenuItem
            // 
            this.tileToolStripMenuItem.Name = "tileToolStripMenuItem";
            this.tileToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.tileToolStripMenuItem.Text = "Tile";
            this.tileToolStripMenuItem.Visible = false;
            this.tileToolStripMenuItem.Click += new System.EventHandler(this.tileToolStripMenuItem_Click);
            // 
            // paletteToolStripMenuItem1
            // 
            this.paletteToolStripMenuItem1.Name = "paletteToolStripMenuItem1";
            this.paletteToolStripMenuItem1.Size = new System.Drawing.Size(113, 22);
            this.paletteToolStripMenuItem1.Text = "Palette";
            this.paletteToolStripMenuItem1.Visible = false;
            this.paletteToolStripMenuItem1.Click += new System.EventHandler(this.paletteToolStripMenuItem1_Click);
            // 
            // mapToolStripMenuItem2
            // 
            this.mapToolStripMenuItem2.Name = "mapToolStripMenuItem2";
            this.mapToolStripMenuItem2.Size = new System.Drawing.Size(113, 22);
            this.mapToolStripMenuItem2.Text = "Map";
            this.mapToolStripMenuItem2.Visible = false;
            // 
            // textureToolStripMenuItem
            // 
            this.textureToolStripMenuItem.Name = "textureToolStripMenuItem";
            this.textureToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.textureToolStripMenuItem.Text = "Texture";
            this.textureToolStripMenuItem.Click += new System.EventHandler(this.textureToolStripMenuItem_Click);
            // 
            // InfoFile
            // 
            this.InfoFile.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.InfoFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InfoFile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Property,
            this.Value});
            this.InfoFile.GridColor = System.Drawing.SystemColors.ButtonHighlight;
            this.InfoFile.Location = new System.Drawing.Point(492, 38);
            this.InfoFile.Name = "InfoFile";
            this.InfoFile.RowHeadersVisible = false;
            this.InfoFile.Size = new System.Drawing.Size(215, 136);
            this.InfoFile.TabIndex = 5;
            // 
            // Property
            // 
            this.Property.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Property.HeaderText = "Property";
            this.Property.Name = "Property";
            // 
            // Value
            // 
            this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            // 
            // HexButton
            // 
            this.HexButton.Enabled = false;
            this.HexButton.Location = new System.Drawing.Point(6, 25);
            this.HexButton.Name = "HexButton";
            this.HexButton.Size = new System.Drawing.Size(75, 23);
            this.HexButton.TabIndex = 6;
            this.HexButton.Text = "Hex Editor";
            this.HexButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.scriptButton);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.MapButton);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.HexButton);
            this.groupBox1.Location = new System.Drawing.Point(486, 200);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(225, 136);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tools";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(6, 83);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 69;
            this.button3.Text = "Text Editor";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // scriptButton
            // 
            this.scriptButton.Location = new System.Drawing.Point(127, 54);
            this.scriptButton.Name = "scriptButton";
            this.scriptButton.Size = new System.Drawing.Size(75, 23);
            this.scriptButton.TabIndex = 9;
            this.scriptButton.Text = "Script Editor";
            this.scriptButton.UseVisualStyleBackColor = true;
            this.scriptButton.Click += new System.EventHandler(this.button4_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(127, 83);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 68;
            this.button1.Text = "Texture List Creator";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // MapButton
            // 
            this.MapButton.Location = new System.Drawing.Point(127, 25);
            this.MapButton.Name = "MapButton";
            this.MapButton.Size = new System.Drawing.Size(75, 23);
            this.MapButton.TabIndex = 8;
            this.MapButton.Text = "Map Editor";
            this.MapButton.UseVisualStyleBackColor = true;
            this.MapButton.Click += new System.EventHandler(this.MapButton_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 54);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Event Editor";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(489, 352);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 67;
            this.label7.Text = "Console Log";
            // 
            // ConsoleLog
            // 
            this.ConsoleLog.Location = new System.Drawing.Point(492, 368);
            this.ConsoleLog.Name = "ConsoleLog";
            this.ConsoleLog.Size = new System.Drawing.Size(219, 237);
            this.ConsoleLog.TabIndex = 66;
            this.ConsoleLog.Text = "";
            // 
            // Main
            // 
            this.ClientSize = new System.Drawing.Size(727, 656);
            this.Controls.Add(this.Sys);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.ConsoleLog);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.InfoFile);
            this.Name = "Main";
            this.Text = "Nintendo Pokemon Rom Editor Beta 4.0";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.fileSystemContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.InfoFile)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Constructors

        public Main()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        #region FileSystem

        #region FileSystem Field
        
        private int subElementsCounter;
        private bool isNdsMainTree;
        private uint nodeStart, nodeLength;
        private string nodeExtension;

        #endregion

        #region Updating

        private TreeNode AddNodes(Common.Folder_s Folder, TreeNode Node, Boolean isNdsMainTree)
        {
            //Init counters
            int subElementsCounter = 0;

            //Check if actual folder contains other internal folders.
            if (Folder.Folders != null)
            {

                //Iterate on internal folders.
                for (int folderCounter = 0; folderCounter < Folder.Folders.Count; folderCounter++)
                {

                    //Store variables
                    Common.Folder_s actualFolder = Folder.Folders[folderCounter];

                    //Add actualFolder to File System
                    Node.Nodes.Add(actualFolder.Name);

                    //Recursive call of method for actualFolder
                    AddNodes(actualFolder, Node.Nodes[folderCounter], true);

                    //Set node tag to generic directory
                    Node.Nodes[folderCounter].Tag = GENERAL_DIRECTORY;

                    //Increments counter of Sub Elements.
                    if (Folder.Files != null)
                        subElementsCounter++;
                }
            }

            //Check if actual folder contains internal files.
            if (Folder.Files != null)
            {

                //Iterate on internal files.
                for (int fileCounter = 0; fileCounter < Folder.Files.Count; fileCounter++)
                {

                    //Add actualFile to File System
                    Node.Nodes.Add(Folder.Files[fileCounter].Name);

                    //Check if is a NDS archive or Narc archive
                    if (isNdsMainTree)
                    {

                        //Check if there were other elements into folder and update tag.
                        if (fileCounter > subElementsCounter)
                            Node.Nodes[fileCounter].Tag = (short)Folder.Files[fileCounter - subElementsCounter].ID;
                        else
                            Node.Nodes[fileCounter + subElementsCounter].Tag = (short)Folder.Files[fileCounter].ID;
                    }
                    else
                    {
                        Node.Nodes[fileCounter].Tag = NARC_FILE;
                    }
                }
                subElementsCounter = 0;
            }
            return Node;
        }

        private void InitTree(string rootName)
        {
            //Add root node to File System, taking the name from file onto your PC.
            Sys.Nodes.Add(rootName);

            //Populate File System.
            AddNodes(actualNds.Tree, Sys.Nodes[0], true);

            //Assign Context Menu to File System
            Sys.ContextMenuStrip = fileSystemContextMenu;

            //Disable all Context Menu item.
            DisableContextMenu();
        }

        private TreeNode MakeTree(Common.Folder_s Folder, TreeNode Node)
        {
            //Init counters
            subElementsCounter = 0;

            //Check if actual folder contains other internal folders.
            if (Folder.Folders != null)
                Folder = CheckInternalFolders(Folder, Node);

            if (Folder.Files != null)
                Folder = CheckInternalFiles(Folder, Node);

            return Node;
        }

        private Common.Folder_s CheckInternalFolders(Common.Folder_s Folder, TreeNode Node)
        {
            //Iterate on internal folders.
            for (int folderCounter = 0; folderCounter < Folder.Folders.Count; folderCounter++)
                Folder = CheckInternalFolder(Folder, Node, ref folderCounter);
            return Folder;
        }

        private Common.Folder_s CheckInternalFolder(Common.Folder_s Folder, TreeNode Node, ref int folderCounter)
        {
            //Store variables
            var actualFolder = Folder.Folders[folderCounter];

            //Add actualFolder to File System
            Node.Nodes.Add(actualFolder.Name);

            var actualNode = Node.Nodes[folderCounter];

            //Recursive call of method for actualFolder
            MakeTree(actualFolder, actualNode);

            //Set node tag to generic directory
            actualNode.Tag = GENERAL_DIRECTORY;

            //Increments counter of Sub Elements.
            if (Folder.Files != null)
                subElementsCounter++;

            Node.Nodes[folderCounter] = actualNode;

            return Folder;
        }

        private Common.Folder_s CheckInternalFiles(Common.Folder_s Folder, TreeNode Node)
        {
            //Iterate on internal files.
            for (int fileCounter = 0; fileCounter < Folder.Files.Count; fileCounter++)
                Folder = CheckInternalFile(Folder, Node, fileCounter);
            subElementsCounter = 0;
            return Folder;
        }

        private Common.Folder_s CheckInternalFile(Common.Folder_s Folder, TreeNode Node, int fileCounter)
        {
            //Add actualFile to File System
            var nodeName = Folder.Files[fileCounter].Name;
            Node.Nodes.Add(nodeName);
            Folder = UpdateNodeTag(Folder, Node, fileCounter);
            return Folder;
        }

        private Common.Folder_s UpdateNodeTag(Common.Folder_s Folder, TreeNode Node, int fileCounter)
        {
            //Check if is a NDS archive or Narc archive
            if (isNdsMainTree)
                Folder = UpdateNodeTagNds(Folder, Node, fileCounter);
            else
                Node.Nodes[fileCounter].Tag = NARC_FILE;
            return Folder;
        }

        private Common.Folder_s UpdateNodeTagNds(Common.Folder_s Folder, TreeNode Node, int fileCounter)
        {
            //Check if there were other elements into folder and update tag.
            if (fileCounter > subElementsCounter)
                Node.Nodes[fileCounter].Tag = (short)Folder.Files[fileCounter - subElementsCounter].ID;
            else
                Node.Nodes[fileCounter + subElementsCounter].Tag = (short)Folder.Files[fileCounter].ID;
            return Folder;
        }

        private void DisableContextMenu()
        {
            foreach (ToolStripMenuItem item in fileSystemContextMenu.Items)
                item.Enabled = false;
        }

        private void ResetTree()
        {
            actualNode = null;
            DisableContextMenu();
        }

        private void ResetForm()
        {
            Sys.Nodes.Clear();
        }

        #endregion

        #region Selection and Menu


        private void FileSystemNode_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ResetTree();

            //Save Tag
            var tag = Convert.ToInt16(Sys.SelectedNode.Tag);

            if (isFileTag(tag))
            {
                ActiveContextMenu();
                LoadFileData(tag);
            }
        }

        private void LoadFileData(int tag)
        {
            //Variable declaration;
            nodeStart = 0;
            nodeLength = 0;

            //Load file stream into actualNode, saving nodeStart and length
            actualNode = LoadFileStream(tag);

            //Load extension
            nodeExtension = Utils.readExtension(actualNode);
            ConsoleLog.AppendText("\nSelected file " + Sys.SelectedNode.Text);
            ConsoleLog.AppendText("\nExtensione is: " + nodeExtension);

            //Update Info Table
            PopInfoTable();

            LoadFileStructure(nodeExtension, actualNode);
        }

        private void ActiveContextMenu()
        {
            //active Context Menu
            for (int i = 0; i < 4; i++)
                fileSystemContextMenu.Items[i].Enabled = true;

            fileSystemContextMenu.Items[5].Enabled = true;
        }

        private static bool isFileTag(object tag)
        {
            return tag != null && !tag.Equals(GENERAL_DIRECTORY);
        }

        private void LoadFileStructure(string nodeExtension, ClosableMemoryStream actualNode)
        {

            var reader = new BinaryReader(actualNode);
            reader.BaseStream.Seek(0L, SeekOrigin.Begin);

            switch (nodeExtension)
            {
                case "NARC":
                    actualNarc = new Narc();
                    actualNarc.LoadNarc(reader);
                    break;
                case "BMD0":
                    actualModel = new Nsbmd();
                    actualModel.LoadBMD0(reader,0);
                    break;

                case "RGCN":
                    actualTile = NCGR.Read(reader, 0);
                    break;
            }
            if (nodeExtension.Substring(0, 2) == "AB")
                HandleABArchive(reader);
        }

        private void HandleABArchive(BinaryReader reader)
        {
            InfoFile.Rows[1].Cells[1].Value = "AB";
            abEditor = new abEditor();
            abEditor.Ab = new List<abEditor.AB_s>();
            var abArchive = new abEditor.AB_s();
            abArchive = abEditor.LoadAb(reader, abArchive);
            abEditor.Ab.Add(abArchive);
        }

        private void PopInfoTable()
        {
            InfoFile.Rows.Clear();
            InfoFile.Rows.Add(new object[] { "ActualFile:", Sys.SelectedNode.Text.Split('.')[0] });
            InfoFile.Rows.Add(new object[] { "Real Type: ", nodeExtension });
            InfoFile.Rows.Add(new object[] { "Offset: ", "0x" + nodeStart.ToString("X")});
            InfoFile.Rows.Add(new object[] { "Size: ", nodeLength + " byte" });
        }

        private ClosableMemoryStream LoadFileStream(int tag)
        {
            switch (tag)
            {
                case NARC_FILE:
                    actualNode = actualNarc.figm.fileData[Sys.SelectedNode.Index];
                    nodeStart = actualNarc.fatb.getFileStartAt(Sys.SelectedNode.Index);
                    nodeLength = actualNarc.fatb.getFileEndAt(Sys.SelectedNode.Index) - nodeStart;
                    break;

                case AB_FILE:
                    actualNode = abEditor.Ab[0].BlockData[Sys.SelectedNode.Index];
                    nodeStart = (uint)abEditor.Ab[0].BlockStartOff[Sys.SelectedNode.Index];
                    nodeLength = (uint)abEditor.Ab[0].BlockData[Sys.SelectedNode.Index].Length;
                    break;

                default:
                    actualNode = actualNds.getFat().getFileStreamAt(tag);
                    nodeStart = actualNds.getFat().getFileStartAt(tag);
                    nodeLength = actualNds.getFat().getFileEndAt(tag) - nodeStart;
                    break;
            }
            return actualNode;
        }

        #endregion

        #endregion

        #region Main Menu

        public void openMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Nds File (*.nds)|*.nds"
            };

            if (dialog.ShowDialog() != DialogResult.Cancel)
            {
                ResetForm();
                ROM = dialog.OpenFile();
                var reader = new BinaryReader(ROM);
                actualNds = new Nds(reader);
                actualNds.LoadNds();
                InitTree(dialog.SafeFileName);
                //initAssTable(actualNds.getHeader().getTitle(), actualNds.getHeader().getCode());
            }
        }

        public void saveMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() != DialogResult.Cancel)
            {
                BinaryWriter writer = new BinaryWriter(dialog.OpenFile());
                actualNds.SaveNds(writer);
            }
        }

        #endregion

        #region Context Menu

        private void mapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actualMap = new Maps();
            var reader = new BinaryReader(actualNode);
            actualMap.LoadMap(reader, true);
            mapEditor = new MapEditor(actualMap,Sys.SelectedNode.Index,this);
            mapEditor.Show();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string extension = InfoFile.Rows[1].Cells[1].Value.ToString();
            if (extension != null)
            {
                if (extension == "BMD0")
                    openClickBMD0();
                else if (extension == "RGCN")
                    openClickRGCN();
                else if (extension == "AB")
                    openClickAB();
                else if (extension == "NARC")
                    openClickNarc();
            }
        }

        private void openClickNarc()
        {
            if (isSimpleNarc())
                addInternalFileNarc();
            else
            {
                isNdsMainTree = false;
                MakeTree(actualNarc.treeSystem, Sys.SelectedNode);
            }
        }

        private void addInternalFileNarc()
        {
            for (int fileNumber = 0; fileNumber < actualNarc.fatbNum; fileNumber++)
            {
                string actualFileExt = Utils.readExtension(actualNarc.figm.fileData[fileNumber]);
                Sys.SelectedNode.Nodes.Add(InfoFile.Rows[0].Cells[1].Value.ToString() + "-" + fileNumber.ToString() + "." + actualFileExt);
                Sys.SelectedNode.Nodes[fileNumber].Tag = 0xff;
            }
        }

        private bool isSimpleNarc()
        {
            return (actualNarc.treeSystem.Files == null) && (actualNarc.treeSystem.Folders == null);
        }

        private void openClickAB()
        {
            for (int i = 0; i < abEditor.Ab.Count; i++)
            {
                Sys.SelectedNode.Nodes.Add("Block" + i);
                for (int num = 0; num < abEditor.Ab[i].BlockData.Count; num++)
                {
                    string str3 = Utils.readExtension(abEditor.Ab[i].BlockData[num]);
                    Sys.SelectedNode.Nodes[i].Nodes.Add(string.Concat(new object[] { "File ", num, ".", str3 }));
                    Sys.SelectedNode.Nodes[i].Nodes[num].Tag = AB_FILE;
                }
            }
        }

        private void openClickRGCN()
        {
            if (actualPalette.id != 0)
            {
                TileViewer = new TileView(actualTile, actualPalette);
                TileViewer.Show();
            }
        }

        private void openClickBMD0()
        {
            if (selectedTex != null)
            {
                ConsoleLog.AppendText("Texture selezionata");
                actualModel.actualTex = selectedTex;
            }
            MapViewer = new MapEditor(actualModel);
            MapViewer.Show();
        }

        private void paletteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Utils.readExtension(actualNode) == "RLCN")
            {
                BinaryReader br = new BinaryReader(actualNode);
                actualPalette = NCLR.Leer(br, 0);
                actualNode.ForceClose();
            }
            else
                MessageBox.Show("It isn't a palette!");
        }

        private void eventsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Events(actualNode).Show();
        }

        private void hexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HexViewer = new Hex_Viewer(actualNode);
            HexViewer.Show();
        }

        private void imageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((actualTile.id != 1) && (actualPalette.id != 1))
            {
                TileViewer = new TileView(actualTile, actualPalette);
                TileViewer.Show();
            }
        }

        private void lZ77ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LZ10.Decompress(actualNode, actualNode.Length, ref actualDec);
            string str = Utils.readExtension(actualDec);
            Sys.SelectedNode.Nodes.Add(Sys.SelectedNode.Text + "." + str);
            Sys.SelectedNode.Nodes[0].Tag = 300;
        }

        private void scriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int idScript = Int16.Parse((Sys.SelectedNode.Text.Split('-'))[1].ToCharArray()[0].ToString());

            int idMessage = getIdMessage(idScript);

            if (idMessage != -1)
            {
                var textHandler = getTextHandler(idMessage);
                new Scripts(actualNode, textHandler,idTextFile).Show();
            }
            else
                new Scripts(actualNode).Show();

            
        }

        private Formats.Texts getTextHandler(int idMessage)
        {
            var textStream = actualNds.getFat().getFileStreamAt(Int16.Parse(Sys.Nodes[0].Nodes[10].Nodes[1].Tag.ToString()));
            var textNarc = new Narc().LoadNarc(new BinaryReader(textStream));
            var textFile = textNarc.figm.fileData[idMessage];
            int textType = 0;
            if (actualNds.getHeader().getTitle() == "POKEMON B\0\0\0" || actualNds.getHeader().getTitle() == "POKEMON W\0\0\0")
                textType = 1;
            var textHandler = new Texts(textFile, textType);
            return textHandler;
        }

        private int getIdMessage(int idScript)
        {
            int idMessage = -1;

            foreach (MapEditor.tableRow tableRow in actualTable)
                if (idScript == tableRow.scripts)
                    idMessage = tableRow.texts;
            idTextFile = idMessage;

            return idMessage;
        }

        private void tileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Utils.readExtension(actualNode) == "RGCN")
            {
                BinaryReader br = new BinaryReader(actualNode);
                actualTile = NCGR.Read(br, 0);
                actualNode.ForceClose();
            }
            else
            {
                MessageBox.Show("Non \x00e8 un immagine!");
            }
        }

        #endregion

        #region Other Element

        private void HexButton_Click(object sender, EventArgs e)
        {
            if (AreinDec == 1)
                HexViewer = new Hex_Viewer(actualDec);
            else if (actualNode != null)
                HexViewer = new Hex_Viewer(actualNode);
            else if (AreinNarc == 1)
                HexViewer = new Hex_Viewer(actualSubNode);
            HexViewer.Show();
            Sub_File = null;
        }


        private void MapButton_Click(object sender, EventArgs e)
        {
            mapEditor = new MapEditor(this);
            mapEditor.PolyNumMax.Enabled = false;
            mapEditor.Rem_Poly.Enabled = false;
            mapEditor.AddObj.Enabled = false;
            mapEditor.DelObj.Enabled = false;
            mapEditor.SaveObj.Enabled = false;
            mapEditor.Tex_File.Text = Program.Tex_File;
            mapEditor.Show();
        }

        #endregion

        private void listOfMapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapEditor = new MapEditor(actualNarc, this);
            mapEditor.Show();
        }


        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            Texture texture = new Texture();
            texture.Show();
        }

        private void textureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedTex = new Nsbtx();
            selectedTex.LoadBTX0(actualNode);
        }

        private void narcScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Stream textStream = getTextStream(actualNds.getHeader().getTitle());
            Narc textNarc = new Narc();
            if (textStream != null)
                textNarc = new Narc().LoadNarc(new BinaryReader(textStream));

            Narc scriptNarc = new Narc();
            scriptNarc.LoadNarc(new BinaryReader(actualNode));
            var scriptEditor = new Scripts(scriptNarc, textNarc,this);
            //try
            //{
            //    Utils.bwTextNarc = new Narc().LoadNarc(new BinaryReader(actualNds.getFat().getFileStreamAt(Int16.Parse(Sys.Nodes[0].Nodes[0].Nodes[0].Nodes[0].Nodes[2].Tag.ToString()))));
            //}
            //catch
            //{
            //}

                scriptEditor.Show();
        }

        private Stream getTextStream(string Title)
        {
            Stream textStream = null;
            if (Title == "POKEMON P\0\0\0" || Title == "POKEMON D\0\0\0")
                textStream = actualNds.getFat().getFileStreamAt(Int16.Parse(Sys.Nodes[0].Nodes[10].Nodes[1].Tag.ToString()));
            else if (Title == "POKEMON PL\0\0")
                textStream = actualNds.getFat().getFileStreamAt(Int16.Parse(Sys.Nodes[0].Nodes[12].Nodes[2].Tag.ToString()));
            else if (Title == "POKEMON HG\0\0" || Title == "POKEMON SS\0\0")
                textStream = actualNds.getFat().getFileStreamAt(Int16.Parse(Sys.Nodes[0].Nodes[0].Nodes[0].Nodes[2].Nodes[7].Tag.ToString()));
            else if (Title == "POKEMON B\0\0\0" || Title == "POKEMON W\0\0\0" || (Title == "POKEMON B2\0\0" || Title == "POKEMON W2\0\0"))
                textStream = actualNds.getFat().getFileStreamAt(Int16.Parse(Sys.Nodes[0].Nodes[0].Nodes[0].Nodes[0].Nodes[3].Tag.ToString()));
            return textStream;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Texture tex = new Texture();
            tex.Show();

        }

        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            if (save.ShowDialog() != DialogResult.Cancel)
            {
                BinaryWriter writer = new BinaryWriter(save.OpenFile());
                writer.BaseStream.Position = 0;
                var reader = new BinaryReader(actualNode);
                reader.BaseStream.Position = 0;
                var buffer = reader.ReadBytes((int)actualNode.Length);
                writer.Write(buffer);
                writer.Close();
            }
        }

        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int textType = 0;
            if (actualNds.getHeader().getTitle() == "POKEMON B\0\0\0" || actualNds.getHeader().getTitle() == "POKEMON W\0\0\0" ||
                actualNds.getHeader().getTitle() == "POKEMON B2\0\0" || actualNds.getHeader().getTitle() == "POKEMON W2\0\0")
                textType = 1;
            var textEditor = new Texts(actualNode, textType);
            textEditor.Show(); if (actualNds.getHeader().getTitle() == "POKEMON B\0\0\0" || actualNds.getHeader().getTitle() == "POKEMON W\0\0\0" ||
     actualNds.getHeader().getTitle() == "POKEMON B2\0\0" || actualNds.getHeader().getTitle() == "POKEMON W2\0\0")
                textType = 1;
        }

        private void narcTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var textNarc = new Narc();
            textNarc.LoadNarc(new BinaryReader(actualNode));
            int textType = 0;
            if (actualNds.getHeader().getTitle() == "POKEMON B\0\0\0" || actualNds.getHeader().getTitle() == "POKEMON W\0\0\0" ||
                actualNds.getHeader().getTitle() == "POKEMON B2\0\0" || actualNds.getHeader().getTitle() == "POKEMON W2\0\0")
                textType = 1;
            var textEditor = new Texts(textNarc,textType);
            textEditor.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var scriptEditor = new Scripts();
            scriptEditor.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var textEditor = new Texts();
            textEditor.Show();
        }

        private void matrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int matrixType =0;
            if (actualNds.getHeader().getTitle() == "POKEMON B\0\0\0" || actualNds.getHeader().getTitle() == "POKEMON W\0\0\0"
                || actualNds.getHeader().getTitle() == "POKEMON B2\0\0" || actualNds.getHeader().getTitle() == "POKEMON W2\0\0")
                matrixType = 1;
            var matrixEditor = new MapMatrix(actualNode,matrixType);
            matrixEditor.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var scriptRoutine = new NPRE.GUI.ScriptRoutineViewer(actualNds.getARM9());
            scriptRoutine.Show();
        }

    }
}

