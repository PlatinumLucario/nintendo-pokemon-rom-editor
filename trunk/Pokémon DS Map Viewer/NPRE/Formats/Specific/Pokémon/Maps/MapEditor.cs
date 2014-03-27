namespace PG4Map
{
    using PG4Map.Formats;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using Tao.OpenGl;
    using Tao.Platform.Windows;
    using NPRE.Formats;
    using System.Runtime.InteropServices;
    using NPRE.Formats.Specific.Pokémon.Maps;
    using NPRE.Formats.Specific.Pokémon.Scripts;

    public class MapEditor : Form
    {


        #region Constants
        public const short GENERAL_DIRECTORY = 0;
        public const short NARC_FILE = 1;
        public const short AB_FILE = 2;
        public const short DPMAP = 0;
        public const short HGSSMAP = 2;
        public const short BWMAP = 3;
        public const short BW2MAP = 4;
        public const short NSBMD_MODEL = 5;
        public const short PLMAP = 1;
        public const short MAXMOVSIZE = 32;
        #endregion

        #region Structured
        private Nsbmd actualModel;
        private Maps actualMap;
        private Nsbtx actualTex;
        private Narc actualNarc;
        private Renderer renderer;
        private ClosableMemoryStream scriptStream;
        private Stream textStream;
        private Stream mapStream;

        #endregion

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
            public string name;
            public ushort weather;
            public byte nameStyle;
            public int headerId;
        }
        #endregion

        public static int textureShowActive;
        public static int modelType;
        public static int actualPolygonId = 0;
        public static string textureName;
        public static int headerId;


        public Boolean isNarc = false;
        public Boolean isMap = false;
        public Boolean isModel = false;

        #region GUI

        private IContainer components = null;

        public ToolStripMenuItem Grass;
        public ToolStripMenuItem High_Grass;
        public ToolStripMenuItem mapToolStripMenuItem;
        public ToolStripMenuItem mapToolStripMenuItem1;
        public ContextMenuStrip MovMenu;
        public ToolStripMenuItem narcToolStripMenuItem;
        public ToolStripMenuItem narcToolStripMenuItem1;
        public ToolStripMenuItem nsbmdToolStripMenuItem;
        public ToolStripMenuItem openNarcToolStripMenuItem;
        public ToolStripMenuItem saveNarcToolStripMenuItem;
        public ToolStripMenuItem Surf;
        public ToolStrip toolStrip;
        public ToolStripDropDownButton toolStripComboBox1;
        public ToolStripSeparator toolStripDropDownButton1;
        public ToolStripMenuItem toolStripMenuItem1;
        public ToolStripMenuItem toolStripMenuItem2;
        public ToolStripMenuItem toolStripMenuItem3;
        public ToolStripMenuItem toolStripMenuItem4;
        public ToolStripMenuItem toolStripMenuItem5;
        public ToolStripMenuItem toolStripMenuItem6;
        public ToolStripMenuItem toolStripMenuItem7;
        public ToolStripMenuItem toolStripMenuItem8;
        public ToolStripSeparator toolStripSeparator;
        public ToolStripMenuItem Waterfall;
        public ToolStripMenuItem x101DoorDownToolStripMenuItem;
        public ToolStripMenuItem x105DoorToolStripMenuItem;
        public ToolStripMenuItem x108RightToolStripMenuItem;
        public ToolStripMenuItem x109LeftToolStripMenuItem;
        public ToolStripMenuItem x109StairsLeftToolStripMenuItem;
        public ToolStripMenuItem x110UpToolStripMenuItem;
        public ToolStripMenuItem x111DownToolStripMenuItem;
        public ToolStripMenuItem x160WoodEntranceToolStripMenuItem;
        public ToolStripMenuItem x219BikeRoutineToolStripMenuItem;
        public ToolStripMenuItem x23LowWaterEffectToolStripMenuItem;
        public ToolStripMenuItem x33SandToolStripMenuItem;
        public ToolStripMenuItem x56JumpLeftToolStripMenuItem;
        public ToolStripMenuItem x56LeftToolStripMenuItem;
        public ToolStripMenuItem x57RightToolStripMenuItem;
        public ToolStripMenuItem x58UpToolStripMenuItem;
        public ToolStripMenuItem x59DownToolStripMenuItem;
        public ToolStripMenuItem x63WSVerticlesToolStripMenuItem;
        public ToolStripMenuItem x6LockToolStripMenuItem;
        public ToolStripMenuItem x8CaveToolStripMenuItem;
        public ToolStripMenuItem x94InternalStairsDownToolStripMenuItem;
        public ToolStripMenuItem x94RightToolStripMenuItem;
        public ToolStripMenuItem x95LeftToolStripMenuItem;
        public ToolStripMenuItem x96UpToolStripMenuItem;
        public ToolStripMenuItem x97DownToolStripMenuItem;
        public ToolStripMenuItem x99DoorLeftToolStripMenuItem;
        private ToolStripMenuItem modelToolStripMenuItem;
        private TabPage tabPage3;
        private DataGridView AssMap_T;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn s;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn Texture_2;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Script;
        private DataGridViewTextBoxColumn Trainer;
        private DataGridViewTextBoxColumn Texts;
        private DataGridViewTextBoxColumn Encount;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Event;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Par;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Flag;
        private DataGridViewTextBoxColumn Terrain;
        public TabPage Objects;
        public DataGridView objShowInfo;
        public Button SaveObj;
        public Button DelObj;
        public Button AddObj;
        public DataGridView ObjInfo;
        private DataGridViewTextBoxColumn Count;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn Xf;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn Yf;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn Zf;
        private DataGridViewTextBoxColumn H;
        private DataGridViewTextBoxColumn L;
        private DataGridViewTextBoxColumn W;
        public TabPage Movements;
        private TabControl tabControl3;
        private TabPage Layer_0;
        public Button saveMovInfoL0;
        public DataGridView MovInfo;
        private TabPage Layer_1;
        public Button saveMovInfoL1;
        public DataGridView MovInfoL1;
        private TabPage Layer_2;
        public Button button1;
        public DataGridView MovInfoL2;
        private TabPage Layer_3;
        public Button button2;
        public DataGridView MovInfoL3;
        public TabPage Nsbmd;
        private Panel panel4;
        public SimpleOpenGlControl OpenGlControl;
        private TabControl tabControl2;
        private TabPage tabPage1;
        private Label label14;
        public ComboBox AvaPalList;
        private Label label13;
        public ComboBox AvaTexList;
        public Button SaveMat;
        public RichTextBox Console;
        private Label label7;
        public DataGridView AssTable;
        private DataGridViewTextBoxColumn Polygon;
        private DataGridViewTextBoxColumn Material;
        private DataGridViewTextBoxColumn idMaterial;
        private DataGridViewTextBoxColumn Texture;
        private DataGridViewTextBoxColumn idTexture;
        private DataGridViewTextBoxColumn Palette;
        private DataGridViewTextBoxColumn idPal;
        internal Label label23;
        private TabPage tabPage2;
        private ComboBox functionList;
        private Button AddPol;
        private Button ResizePol;
        private Button ResetPoly;
        private Button SavePol;
        public DataGridView VerValue;
        private DataGridViewTextBoxColumn Offset;
        private DataGridViewTextBoxColumn Info;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn Par3;
        private DataGridViewTextBoxColumn Par2;
        private DataGridViewTextBoxColumn Cost;
        private DataGridViewTextBoxColumn Cost2;
        public Button Rem_Poly;
        public ComboBox MapList;
        internal Label label12;
        internal TextBox Tex_File;
        public GroupBox groupBox1;
        public Label label11;
        public Label label2;
        public TrackBar trackBarTransZ;
        public Label lblDistance;
        public Label label24;
        public Label label22;
        public Label label21;
        public TrackBar trackBarTransY;
        public Label label20;
        public Label label19;
        public Label label18;
        public TrackBar trackBarRotZ;
        public CheckBox showSingular;
        public CheckBox SaveType;
        public CheckBox CheckText;
        public Label label15;
        public Label label16;
        public TrackBar trackBarTransX;
        public Label label17;
        public Label label10;
        public NumericUpDown PolyNumMax;
        public Label label5;
        public Label label3;
        public TrackBar trackBarRotX;
        public TrackBar trackBarRotY;
        public Label label6;
        public Label lblElevation;
        public Label label4;
        public Label lblAngle;
        public Label label1;
        public Label label9;
        public Label label8;
        public TabControl tabControl1;
        private TabControl tabControl4;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TabPage tabPage6;
        private TabPage tabPage7;
        public DataGridView PeopleMapInfo;
        private TabControl tabControl5;
        private TabPage People;
        private TabPage Warps;
        public DataGridView WarpInfo;
        public DataGridView WarpsMapInfo;
        private TabPage tabPage10;
        public DataGridView FurnInfo;
        public DataGridView FurnMapInfo;
        private TabPage tabPage11;
        public DataGridView TriggerInfo;
        public DataGridView TriggerMapInfo;
        private TabControl tabControl6;
        private TabPage Headers;
        public DataGridView matrixHeaderInfo;
        private TabPage Borders;
        public DataGridView matrixBorderInfo;
        private TabPage Terrains;
        public DataGridView matrixTerrainInfo;
        private RichTextBox scriptBoxViewer;
        private RichTextBox textBox;
        private DataGridView PeopleInfo;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn22;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn24;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn26;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn23;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn33;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn35;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn37;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn38;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn42;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn44;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn46;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn52;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn51;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn50;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private DataGridViewTextBoxColumn Mov;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private DataGridViewTextBoxColumn LineOfSight;
        private DataGridViewTextBoxColumn X;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private TabPage tabPage8;
        private Button button3;
        private GroupBox groupBox5;
        private Label label34;
        private Label label35;
        private Label label36;
        private TextBox actualTerrainBox;
        private TextBox actualBorderBox;
        private TextBox actualHeaderBox;
        private GroupBox groupBox4;
        private Label label31;
        private Label label32;
        private Label label33;
        private TextBox westTerrainBox;
        private TextBox westBorderBox;
        private TextBox westHeaderMap;
        private GroupBox groupBox3;
        private Label label28;
        private Label label29;
        private Label label30;
        private TextBox eastTerrainBox;
        private TextBox eastBorderBox;
        private TextBox eastHeaderBox;
        private GroupBox groupBox2;
        private Label label27;
        private Label label26;
        private Label label25;
        private TextBox northTerrainBox;
        private TextBox northBorderBox;
        private TextBox northHeaderBox;
        private GroupBox groupBox6;
        private Label label37;
        private Label label38;
        private Label label39;
        private TextBox southTerrainBox;
        private TextBox southBorderBox;
        private TextBox southHeaderBox;
        private Panel SEPrev;
        private Panel SPrev;
        private Panel EPrev;
        private Panel CPrev;
        private Panel NEPrev;
        private Panel NPrev;
        private Panel SWPrev;
        private Panel WPrev;
        private Panel NWPrev;
        private GroupBox groupBox10;
        private Label label49;
        private Label label50;
        private Label label51;
        private TextBox southWestTerrainBox;
        private TextBox southWestBorderBox;
        private TextBox southWestHeaderBox;
        private GroupBox groupBox9;
        private Label label46;
        private Label label47;
        private Label label48;
        private TextBox southEastTerrainBox;
        private TextBox southEastBorderBox;
        private TextBox southEastHeaderBox;
        private GroupBox groupBox8;
        private Label label43;
        private Label label44;
        private Label label45;
        private TextBox northEastTerrainBox;
        private TextBox northEastBorderBox;
        private TextBox northEastHeaderBox;
        private GroupBox groupBox7;
        private Label label40;
        private Label label41;
        private Label label42;
        private TextBox northWestTerrainBox;
        private TextBox northWestBorderBox;
        private TextBox northWestHeaderBox;
        private RichTextBox scriptBoxEditor;
        private ComboBox subScripts;
        private CheckBox OrthoView;
        private ToolStripMenuItem modelIMDToolStripMenuItem;



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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle36 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle37 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle38 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle39 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle40 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Grass = new System.Windows.Forms.ToolStripMenuItem();
            this.High_Grass = new System.Windows.Forms.ToolStripMenuItem();
            this.x6LockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x8CaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Surf = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.Waterfall = new System.Windows.Forms.ToolStripMenuItem();
            this.x23LowWaterEffectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x33SandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x56JumpLeftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x56LeftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x57RightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x58UpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x59DownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x63WSVerticlesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x94InternalStairsDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x94RightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x95LeftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x96UpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x97DownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x99DoorLeftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x105DoorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x109StairsLeftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x108RightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x109LeftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x110UpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x111DownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.x160WoodEntranceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x219BikeRoutineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x101DoorDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.openNarcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.narcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nsbmdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveNarcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.narcToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mapToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.modelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modelIMDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripSeparator();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabControl4 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabControl6 = new System.Windows.Forms.TabControl();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.label49 = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.southWestTerrainBox = new System.Windows.Forms.TextBox();
            this.southWestBorderBox = new System.Windows.Forms.TextBox();
            this.southWestHeaderBox = new System.Windows.Forms.TextBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.label46 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.southEastTerrainBox = new System.Windows.Forms.TextBox();
            this.southEastBorderBox = new System.Windows.Forms.TextBox();
            this.southEastHeaderBox = new System.Windows.Forms.TextBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label43 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.northEastTerrainBox = new System.Windows.Forms.TextBox();
            this.northEastBorderBox = new System.Windows.Forms.TextBox();
            this.northEastHeaderBox = new System.Windows.Forms.TextBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label40 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.northWestTerrainBox = new System.Windows.Forms.TextBox();
            this.northWestBorderBox = new System.Windows.Forms.TextBox();
            this.northWestHeaderBox = new System.Windows.Forms.TextBox();
            this.SEPrev = new System.Windows.Forms.Panel();
            this.SPrev = new System.Windows.Forms.Panel();
            this.EPrev = new System.Windows.Forms.Panel();
            this.CPrev = new System.Windows.Forms.Panel();
            this.NEPrev = new System.Windows.Forms.Panel();
            this.NPrev = new System.Windows.Forms.Panel();
            this.SWPrev = new System.Windows.Forms.Panel();
            this.WPrev = new System.Windows.Forms.Panel();
            this.NWPrev = new System.Windows.Forms.Panel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label37 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.southTerrainBox = new System.Windows.Forms.TextBox();
            this.southBorderBox = new System.Windows.Forms.TextBox();
            this.southHeaderBox = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.actualTerrainBox = new System.Windows.Forms.TextBox();
            this.actualBorderBox = new System.Windows.Forms.TextBox();
            this.actualHeaderBox = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.westTerrainBox = new System.Windows.Forms.TextBox();
            this.westBorderBox = new System.Windows.Forms.TextBox();
            this.westHeaderMap = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.eastTerrainBox = new System.Windows.Forms.TextBox();
            this.eastBorderBox = new System.Windows.Forms.TextBox();
            this.eastHeaderBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.northTerrainBox = new System.Windows.Forms.TextBox();
            this.northBorderBox = new System.Windows.Forms.TextBox();
            this.northHeaderBox = new System.Windows.Forms.TextBox();
            this.Headers = new System.Windows.Forms.TabPage();
            this.matrixHeaderInfo = new System.Windows.Forms.DataGridView();
            this.Borders = new System.Windows.Forms.TabPage();
            this.matrixBorderInfo = new System.Windows.Forms.DataGridView();
            this.Terrains = new System.Windows.Forms.TabPage();
            this.matrixTerrainInfo = new System.Windows.Forms.DataGridView();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.subScripts = new System.Windows.Forms.ComboBox();
            this.scriptBoxEditor = new System.Windows.Forms.RichTextBox();
            this.scriptBoxViewer = new System.Windows.Forms.RichTextBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.textBox = new System.Windows.Forms.RichTextBox();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.tabControl5 = new System.Windows.Forms.TabControl();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.FurnInfo = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn22 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn24 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn26 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FurnMapInfo = new System.Windows.Forms.DataGridView();
            this.People = new System.Windows.Forms.TabPage();
            this.PeopleInfo = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mov = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LineOfSight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.X = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PeopleMapInfo = new System.Windows.Forms.DataGridView();
            this.Warps = new System.Windows.Forms.TabPage();
            this.WarpInfo = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn33 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn35 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn37 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn38 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WarpsMapInfo = new System.Windows.Forms.DataGridView();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.TriggerInfo = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn42 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn44 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn46 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn52 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn51 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn50 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TriggerMapInfo = new System.Windows.Forms.DataGridView();
            this.AssMap_T = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.s = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Texture_2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Script = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Trainer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Texts = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Encount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Event = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Par = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Flag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Terrain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Objects = new System.Windows.Forms.TabPage();
            this.objShowInfo = new System.Windows.Forms.DataGridView();
            this.SaveObj = new System.Windows.Forms.Button();
            this.DelObj = new System.Windows.Forms.Button();
            this.AddObj = new System.Windows.Forms.Button();
            this.ObjInfo = new System.Windows.Forms.DataGridView();
            this.Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Xf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Yf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Zf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.H = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.L = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.W = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Movements = new System.Windows.Forms.TabPage();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.Layer_0 = new System.Windows.Forms.TabPage();
            this.saveMovInfoL0 = new System.Windows.Forms.Button();
            this.MovInfo = new System.Windows.Forms.DataGridView();
            this.Layer_1 = new System.Windows.Forms.TabPage();
            this.saveMovInfoL1 = new System.Windows.Forms.Button();
            this.MovInfoL1 = new System.Windows.Forms.DataGridView();
            this.Layer_2 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.MovInfoL2 = new System.Windows.Forms.DataGridView();
            this.Layer_3 = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.MovInfoL3 = new System.Windows.Forms.DataGridView();
            this.Nsbmd = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.OpenGlControl = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.AvaPalList = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.AvaTexList = new System.Windows.Forms.ComboBox();
            this.SaveMat = new System.Windows.Forms.Button();
            this.Console = new System.Windows.Forms.RichTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.AssTable = new System.Windows.Forms.DataGridView();
            this.Polygon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Material = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idMaterial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Texture = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idTexture = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Palette = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idPal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label23 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.functionList = new System.Windows.Forms.ComboBox();
            this.AddPol = new System.Windows.Forms.Button();
            this.ResizePol = new System.Windows.Forms.Button();
            this.ResetPoly = new System.Windows.Forms.Button();
            this.SavePol = new System.Windows.Forms.Button();
            this.VerValue = new System.Windows.Forms.DataGridView();
            this.Offset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Info = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Par3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Par2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Rem_Poly = new System.Windows.Forms.Button();
            this.MapList = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.Tex_File = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBarTransZ = new System.Windows.Forms.TrackBar();
            this.lblDistance = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.trackBarTransY = new System.Windows.Forms.TrackBar();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.trackBarRotZ = new System.Windows.Forms.TrackBar();
            this.showSingular = new System.Windows.Forms.CheckBox();
            this.SaveType = new System.Windows.Forms.CheckBox();
            this.CheckText = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.trackBarTransX = new System.Windows.Forms.TrackBar();
            this.label17 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.PolyNumMax = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBarRotX = new System.Windows.Forms.TrackBar();
            this.trackBarRotY = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.lblElevation = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblAngle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.button3 = new System.Windows.Forms.Button();
            this.OrthoView = new System.Windows.Forms.CheckBox();
            this.toolStrip.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabControl4.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabControl6.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.Headers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.matrixHeaderInfo)).BeginInit();
            this.Borders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.matrixBorderInfo)).BeginInit();
            this.Terrains.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.matrixTerrainInfo)).BeginInit();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabControl5.SuspendLayout();
            this.tabPage10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FurnInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FurnMapInfo)).BeginInit();
            this.People.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PeopleInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PeopleMapInfo)).BeginInit();
            this.Warps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WarpInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarpsMapInfo)).BeginInit();
            this.tabPage11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TriggerInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TriggerMapInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AssMap_T)).BeginInit();
            this.Objects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objShowInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ObjInfo)).BeginInit();
            this.Movements.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.Layer_0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MovInfo)).BeginInit();
            this.Layer_1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MovInfoL1)).BeginInit();
            this.Layer_2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MovInfoL2)).BeginInit();
            this.Layer_3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MovInfoL3)).BeginInit();
            this.Nsbmd.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AssTable)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VerValue)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTransZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTransY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRotZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTransX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PolyNumMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRotX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRotY)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Grass
            // 
            this.Grass.Name = "Grass";
            this.Grass.Size = new System.Drawing.Size(205, 22);
            this.Grass.Text = "0x2 Grass";
            this.Grass.Click += new System.EventHandler(this.Grass_Click);
            // 
            // High_Grass
            // 
            this.High_Grass.Name = "High_Grass";
            this.High_Grass.Size = new System.Drawing.Size(205, 22);
            this.High_Grass.Text = "0x3 High Grass";
            this.High_Grass.Click += new System.EventHandler(this.High_Grass_Click);
            // 
            // x6LockToolStripMenuItem
            // 
            this.x6LockToolStripMenuItem.Name = "x6LockToolStripMenuItem";
            this.x6LockToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.x6LockToolStripMenuItem.Text = "0x6 HeadButt (HG)";
            this.x6LockToolStripMenuItem.Click += new System.EventHandler(this.x6LockToolStripMenuItem_Click);
            // 
            // x8CaveToolStripMenuItem
            // 
            this.x8CaveToolStripMenuItem.Name = "x8CaveToolStripMenuItem";
            this.x8CaveToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.x8CaveToolStripMenuItem.Text = "0x8 Cave";
            this.x8CaveToolStripMenuItem.Click += new System.EventHandler(this.x8CaveToolStripMenuItem_Click);
            // 
            // Surf
            // 
            this.Surf.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem7,
            this.toolStripMenuItem8,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6});
            this.Surf.Name = "Surf";
            this.Surf.Size = new System.Drawing.Size(205, 22);
            this.Surf.Text = "0x10 Surf";
            this.Surf.Click += new System.EventHandler(this.Surf_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(121, 22);
            this.toolStripMenuItem2.Text = "0x11 Surf";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(121, 22);
            this.toolStripMenuItem3.Text = "0x12 Surf";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(121, 22);
            this.toolStripMenuItem4.Text = "0x14 Surf";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(121, 22);
            this.toolStripMenuItem7.Text = "0x15 Surf";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(121, 22);
            this.toolStripMenuItem8.Text = "0x16 Surf";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(121, 22);
            this.toolStripMenuItem5.Text = "0x21 Surf";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(121, 22);
            this.toolStripMenuItem6.Text = "0x22 Surf";
            // 
            // Waterfall
            // 
            this.Waterfall.Name = "Waterfall";
            this.Waterfall.Size = new System.Drawing.Size(205, 22);
            this.Waterfall.Text = "0x13 Waterfall";
            // 
            // x23LowWaterEffectToolStripMenuItem
            // 
            this.x23LowWaterEffectToolStripMenuItem.Name = "x23LowWaterEffectToolStripMenuItem";
            this.x23LowWaterEffectToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.x23LowWaterEffectToolStripMenuItem.Text = "0x23 Low Water Effect";
            // 
            // x33SandToolStripMenuItem
            // 
            this.x33SandToolStripMenuItem.Name = "x33SandToolStripMenuItem";
            this.x33SandToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.x33SandToolStripMenuItem.Text = "0x33 Sand";
            // 
            // x56JumpLeftToolStripMenuItem
            // 
            this.x56JumpLeftToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.x56LeftToolStripMenuItem,
            this.x57RightToolStripMenuItem,
            this.x58UpToolStripMenuItem,
            this.x59DownToolStripMenuItem,
            this.x63WSVerticlesToolStripMenuItem});
            this.x56JumpLeftToolStripMenuItem.Name = "x56JumpLeftToolStripMenuItem";
            this.x56JumpLeftToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.x56JumpLeftToolStripMenuItem.Text = "0x5- Jump";
            // 
            // x56LeftToolStripMenuItem
            // 
            this.x56LeftToolStripMenuItem.Name = "x56LeftToolStripMenuItem";
            this.x56LeftToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.x56LeftToolStripMenuItem.Text = "0x56 Right";
            // 
            // x57RightToolStripMenuItem
            // 
            this.x57RightToolStripMenuItem.Name = "x57RightToolStripMenuItem";
            this.x57RightToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.x57RightToolStripMenuItem.Text = "0x57 Left";
            // 
            // x58UpToolStripMenuItem
            // 
            this.x58UpToolStripMenuItem.Name = "x58UpToolStripMenuItem";
            this.x58UpToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.x58UpToolStripMenuItem.Text = "0x58 Up";
            // 
            // x59DownToolStripMenuItem
            // 
            this.x59DownToolStripMenuItem.Name = "x59DownToolStripMenuItem";
            this.x59DownToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.x59DownToolStripMenuItem.Text = "0x59 Down";
            // 
            // x63WSVerticlesToolStripMenuItem
            // 
            this.x63WSVerticlesToolStripMenuItem.Name = "x63WSVerticlesToolStripMenuItem";
            this.x63WSVerticlesToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.x63WSVerticlesToolStripMenuItem.Text = "0x63 WS Verticles";
            // 
            // x94InternalStairsDownToolStripMenuItem
            // 
            this.x94InternalStairsDownToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.x94RightToolStripMenuItem,
            this.x95LeftToolStripMenuItem,
            this.x96UpToolStripMenuItem,
            this.x97DownToolStripMenuItem});
            this.x94InternalStairsDownToolStripMenuItem.Name = "x94InternalStairsDownToolStripMenuItem";
            this.x94InternalStairsDownToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.x94InternalStairsDownToolStripMenuItem.Text = "0x9- Internal Entrance";
            // 
            // x94RightToolStripMenuItem
            // 
            this.x94RightToolStripMenuItem.Name = "x94RightToolStripMenuItem";
            this.x94RightToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.x94RightToolStripMenuItem.Text = "0x94 Right";
            // 
            // x95LeftToolStripMenuItem
            // 
            this.x95LeftToolStripMenuItem.Name = "x95LeftToolStripMenuItem";
            this.x95LeftToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.x95LeftToolStripMenuItem.Text = "0x95 Left";
            // 
            // x96UpToolStripMenuItem
            // 
            this.x96UpToolStripMenuItem.Name = "x96UpToolStripMenuItem";
            this.x96UpToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.x96UpToolStripMenuItem.Text = "0x96 Up";
            // 
            // x97DownToolStripMenuItem
            // 
            this.x97DownToolStripMenuItem.Name = "x97DownToolStripMenuItem";
            this.x97DownToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.x97DownToolStripMenuItem.Text = "0x97 Down";
            // 
            // x99DoorLeftToolStripMenuItem
            // 
            this.x99DoorLeftToolStripMenuItem.Name = "x99DoorLeftToolStripMenuItem";
            this.x99DoorLeftToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.x99DoorLeftToolStripMenuItem.Text = "0x99 Door (Left)";
            // 
            // x105DoorToolStripMenuItem
            // 
            this.x105DoorToolStripMenuItem.Name = "x105DoorToolStripMenuItem";
            this.x105DoorToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.x105DoorToolStripMenuItem.Text = "0x105 Door (Up)";
            // 
            // x109StairsLeftToolStripMenuItem
            // 
            this.x109StairsLeftToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.x108RightToolStripMenuItem,
            this.x109LeftToolStripMenuItem,
            this.x110UpToolStripMenuItem,
            this.x111DownToolStripMenuItem});
            this.x109StairsLeftToolStripMenuItem.Name = "x109StairsLeftToolStripMenuItem";
            this.x109StairsLeftToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.x109StairsLeftToolStripMenuItem.Text = "0x10- External Entrance";
            // 
            // x108RightToolStripMenuItem
            // 
            this.x108RightToolStripMenuItem.Name = "x108RightToolStripMenuItem";
            this.x108RightToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.x108RightToolStripMenuItem.Text = "0x108 Right";
            // 
            // x109LeftToolStripMenuItem
            // 
            this.x109LeftToolStripMenuItem.Name = "x109LeftToolStripMenuItem";
            this.x109LeftToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.x109LeftToolStripMenuItem.Text = "0x109 Left";
            // 
            // x110UpToolStripMenuItem
            // 
            this.x110UpToolStripMenuItem.Name = "x110UpToolStripMenuItem";
            this.x110UpToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.x110UpToolStripMenuItem.Text = "0x110 Up";
            // 
            // x111DownToolStripMenuItem
            // 
            this.x111DownToolStripMenuItem.Name = "x111DownToolStripMenuItem";
            this.x111DownToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.x111DownToolStripMenuItem.Text = "0x111 Down";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(205, 22);
            this.toolStripMenuItem1.Text = "0x159 Snow";
            // 
            // x160WoodEntranceToolStripMenuItem
            // 
            this.x160WoodEntranceToolStripMenuItem.Name = "x160WoodEntranceToolStripMenuItem";
            this.x160WoodEntranceToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.x160WoodEntranceToolStripMenuItem.Text = "0x160 Wood Entrance";
            // 
            // x219BikeRoutineToolStripMenuItem
            // 
            this.x219BikeRoutineToolStripMenuItem.Name = "x219BikeRoutineToolStripMenuItem";
            this.x219BikeRoutineToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.x219BikeRoutineToolStripMenuItem.Text = "0x219 Bike Ride Question";
            // 
            // x101DoorDownToolStripMenuItem
            // 
            this.x101DoorDownToolStripMenuItem.Name = "x101DoorDownToolStripMenuItem";
            this.x101DoorDownToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.x101DoorDownToolStripMenuItem.Text = "0x101 Door (Down) ";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBox1,
            this.toolStripSeparator,
            this.toolStripDropDownButton1});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1362, 25);
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
            this.openNarcToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.narcToolStripMenuItem,
            this.mapToolStripMenuItem,
            this.nsbmdToolStripMenuItem});
            this.openNarcToolStripMenuItem.Name = "openNarcToolStripMenuItem";
            this.openNarcToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openNarcToolStripMenuItem.Text = "Open";
            // 
            // narcToolStripMenuItem
            // 
            this.narcToolStripMenuItem.Enabled = false;
            this.narcToolStripMenuItem.Name = "narcToolStripMenuItem";
            this.narcToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.narcToolStripMenuItem.Text = "Narc";
            this.narcToolStripMenuItem.Click += new System.EventHandler(this.narcToolStripMenuItem_Click);
            // 
            // mapToolStripMenuItem
            // 
            this.mapToolStripMenuItem.Name = "mapToolStripMenuItem";
            this.mapToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.mapToolStripMenuItem.Text = "Map";
            this.mapToolStripMenuItem.Click += new System.EventHandler(this.mapToolStripMenuItem_Click);
            // 
            // nsbmdToolStripMenuItem
            // 
            this.nsbmdToolStripMenuItem.Name = "nsbmdToolStripMenuItem";
            this.nsbmdToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.nsbmdToolStripMenuItem.Text = "Model";
            this.nsbmdToolStripMenuItem.Click += new System.EventHandler(this.nsbmdToolStripMenuItem_Click);
            // 
            // saveNarcToolStripMenuItem
            // 
            this.saveNarcToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.narcToolStripMenuItem1,
            this.mapToolStripMenuItem1,
            this.modelToolStripMenuItem,
            this.modelIMDToolStripMenuItem});
            this.saveNarcToolStripMenuItem.Name = "saveNarcToolStripMenuItem";
            this.saveNarcToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveNarcToolStripMenuItem.Text = "Save";
            // 
            // narcToolStripMenuItem1
            // 
            this.narcToolStripMenuItem1.Enabled = false;
            this.narcToolStripMenuItem1.Name = "narcToolStripMenuItem1";
            this.narcToolStripMenuItem1.Size = new System.Drawing.Size(163, 22);
            this.narcToolStripMenuItem1.Text = "Narc";
            // 
            // mapToolStripMenuItem1
            // 
            this.mapToolStripMenuItem1.Name = "mapToolStripMenuItem1";
            this.mapToolStripMenuItem1.Size = new System.Drawing.Size(163, 22);
            this.mapToolStripMenuItem1.Text = "Map";
            this.mapToolStripMenuItem1.Click += new System.EventHandler(this.mapToolStripMenuItem1_Click);
            // 
            // modelToolStripMenuItem
            // 
            this.modelToolStripMenuItem.Name = "modelToolStripMenuItem";
            this.modelToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.modelToolStripMenuItem.Text = "Model (.NSBMD)";
            this.modelToolStripMenuItem.Click += new System.EventHandler(this.modelToolStripMenuItem_Click_1);
            // 
            // modelIMDToolStripMenuItem
            // 
            this.modelIMDToolStripMenuItem.Name = "modelIMDToolStripMenuItem";
            this.modelIMDToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.modelIMDToolStripMenuItem.Text = "Model (.IMD)";
            this.modelIMDToolStripMenuItem.Visible = false;
            this.modelIMDToolStripMenuItem.Click += new System.EventHandler(this.modelIMDToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(6, 25);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tabControl4);
            this.tabPage3.Controls.Add(this.AssMap_T);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1339, 683);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "Map Header";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabControl4
            // 
            this.tabControl4.Controls.Add(this.tabPage4);
            this.tabControl4.Controls.Add(this.tabPage5);
            this.tabControl4.Controls.Add(this.tabPage6);
            this.tabControl4.Controls.Add(this.tabPage7);
            this.tabControl4.Location = new System.Drawing.Point(16, 117);
            this.tabControl4.Name = "tabControl4";
            this.tabControl4.SelectedIndex = 0;
            this.tabControl4.Size = new System.Drawing.Size(1304, 558);
            this.tabControl4.TabIndex = 10;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tabControl6);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1296, 532);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Matrix";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabControl6
            // 
            this.tabControl6.Controls.Add(this.tabPage8);
            this.tabControl6.Controls.Add(this.Headers);
            this.tabControl6.Controls.Add(this.Borders);
            this.tabControl6.Controls.Add(this.Terrains);
            this.tabControl6.Location = new System.Drawing.Point(19, 17);
            this.tabControl6.Name = "tabControl6";
            this.tabControl6.SelectedIndex = 0;
            this.tabControl6.Size = new System.Drawing.Size(1255, 509);
            this.tabControl6.TabIndex = 0;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.groupBox10);
            this.tabPage8.Controls.Add(this.groupBox9);
            this.tabPage8.Controls.Add(this.groupBox8);
            this.tabPage8.Controls.Add(this.groupBox7);
            this.tabPage8.Controls.Add(this.SEPrev);
            this.tabPage8.Controls.Add(this.SPrev);
            this.tabPage8.Controls.Add(this.EPrev);
            this.tabPage8.Controls.Add(this.CPrev);
            this.tabPage8.Controls.Add(this.NEPrev);
            this.tabPage8.Controls.Add(this.NPrev);
            this.tabPage8.Controls.Add(this.SWPrev);
            this.tabPage8.Controls.Add(this.WPrev);
            this.tabPage8.Controls.Add(this.NWPrev);
            this.tabPage8.Controls.Add(this.groupBox6);
            this.tabPage8.Controls.Add(this.groupBox5);
            this.tabPage8.Controls.Add(this.groupBox4);
            this.tabPage8.Controls.Add(this.groupBox3);
            this.tabPage8.Controls.Add(this.groupBox2);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(1247, 483);
            this.tabPage8.TabIndex = 3;
            this.tabPage8.Text = "MatrixInfo";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.label49);
            this.groupBox10.Controls.Add(this.label50);
            this.groupBox10.Controls.Add(this.label51);
            this.groupBox10.Controls.Add(this.southWestTerrainBox);
            this.groupBox10.Controls.Add(this.southWestBorderBox);
            this.groupBox10.Controls.Add(this.southWestHeaderBox);
            this.groupBox10.Location = new System.Drawing.Point(10, 287);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(173, 100);
            this.groupBox10.TabIndex = 81;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "South-West";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(9, 75);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(40, 13);
            this.label49.TabIndex = 5;
            this.label49.Text = "Terrain";
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(9, 53);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(43, 13);
            this.label50.TabIndex = 4;
            this.label50.Text = "Borders";
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(9, 27);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(42, 13);
            this.label51.TabIndex = 3;
            this.label51.Text = "Header";
            // 
            // southWestTerrainBox
            // 
            this.southWestTerrainBox.Location = new System.Drawing.Point(56, 72);
            this.southWestTerrainBox.Name = "southWestTerrainBox";
            this.southWestTerrainBox.Size = new System.Drawing.Size(100, 20);
            this.southWestTerrainBox.TabIndex = 2;
            // 
            // southWestBorderBox
            // 
            this.southWestBorderBox.Location = new System.Drawing.Point(56, 46);
            this.southWestBorderBox.Name = "southWestBorderBox";
            this.southWestBorderBox.Size = new System.Drawing.Size(100, 20);
            this.southWestBorderBox.TabIndex = 1;
            // 
            // southWestHeaderBox
            // 
            this.southWestHeaderBox.Location = new System.Drawing.Point(56, 20);
            this.southWestHeaderBox.Name = "southWestHeaderBox";
            this.southWestHeaderBox.Size = new System.Drawing.Size(100, 20);
            this.southWestHeaderBox.TabIndex = 0;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.label46);
            this.groupBox9.Controls.Add(this.label47);
            this.groupBox9.Controls.Add(this.label48);
            this.groupBox9.Controls.Add(this.southEastTerrainBox);
            this.groupBox9.Controls.Add(this.southEastBorderBox);
            this.groupBox9.Controls.Add(this.southEastHeaderBox);
            this.groupBox9.Location = new System.Drawing.Point(434, 287);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(173, 100);
            this.groupBox9.TabIndex = 80;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "South-East";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(9, 75);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(40, 13);
            this.label46.TabIndex = 5;
            this.label46.Text = "Terrain";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(9, 53);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(43, 13);
            this.label47.TabIndex = 4;
            this.label47.Text = "Borders";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(9, 27);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(42, 13);
            this.label48.TabIndex = 3;
            this.label48.Text = "Header";
            // 
            // southEastTerrainBox
            // 
            this.southEastTerrainBox.Location = new System.Drawing.Point(56, 72);
            this.southEastTerrainBox.Name = "southEastTerrainBox";
            this.southEastTerrainBox.Size = new System.Drawing.Size(100, 20);
            this.southEastTerrainBox.TabIndex = 2;
            // 
            // southEastBorderBox
            // 
            this.southEastBorderBox.Location = new System.Drawing.Point(56, 46);
            this.southEastBorderBox.Name = "southEastBorderBox";
            this.southEastBorderBox.Size = new System.Drawing.Size(100, 20);
            this.southEastBorderBox.TabIndex = 1;
            // 
            // southEastHeaderBox
            // 
            this.southEastHeaderBox.Location = new System.Drawing.Point(56, 20);
            this.southEastHeaderBox.Name = "southEastHeaderBox";
            this.southEastHeaderBox.Size = new System.Drawing.Size(100, 20);
            this.southEastHeaderBox.TabIndex = 0;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.label43);
            this.groupBox8.Controls.Add(this.label44);
            this.groupBox8.Controls.Add(this.label45);
            this.groupBox8.Controls.Add(this.northEastTerrainBox);
            this.groupBox8.Controls.Add(this.northEastBorderBox);
            this.groupBox8.Controls.Add(this.northEastHeaderBox);
            this.groupBox8.Location = new System.Drawing.Point(434, 19);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(173, 100);
            this.groupBox8.TabIndex = 79;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "North-East";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(9, 75);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(40, 13);
            this.label43.TabIndex = 5;
            this.label43.Text = "Terrain";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(9, 53);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(43, 13);
            this.label44.TabIndex = 4;
            this.label44.Text = "Borders";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(9, 27);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(42, 13);
            this.label45.TabIndex = 3;
            this.label45.Text = "Header";
            // 
            // northEastTerrainBox
            // 
            this.northEastTerrainBox.Location = new System.Drawing.Point(56, 72);
            this.northEastTerrainBox.Name = "northEastTerrainBox";
            this.northEastTerrainBox.Size = new System.Drawing.Size(100, 20);
            this.northEastTerrainBox.TabIndex = 2;
            // 
            // northEastBorderBox
            // 
            this.northEastBorderBox.Location = new System.Drawing.Point(56, 46);
            this.northEastBorderBox.Name = "northEastBorderBox";
            this.northEastBorderBox.Size = new System.Drawing.Size(100, 20);
            this.northEastBorderBox.TabIndex = 1;
            // 
            // northEastHeaderBox
            // 
            this.northEastHeaderBox.Location = new System.Drawing.Point(56, 20);
            this.northEastHeaderBox.Name = "northEastHeaderBox";
            this.northEastHeaderBox.Size = new System.Drawing.Size(100, 20);
            this.northEastHeaderBox.TabIndex = 0;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label40);
            this.groupBox7.Controls.Add(this.label41);
            this.groupBox7.Controls.Add(this.label42);
            this.groupBox7.Controls.Add(this.northWestTerrainBox);
            this.groupBox7.Controls.Add(this.northWestBorderBox);
            this.groupBox7.Controls.Add(this.northWestHeaderBox);
            this.groupBox7.Location = new System.Drawing.Point(10, 19);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(173, 100);
            this.groupBox7.TabIndex = 78;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "North-West";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(9, 75);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(40, 13);
            this.label40.TabIndex = 5;
            this.label40.Text = "Terrain";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(9, 53);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(43, 13);
            this.label41.TabIndex = 4;
            this.label41.Text = "Borders";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(9, 27);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(42, 13);
            this.label42.TabIndex = 3;
            this.label42.Text = "Header";
            // 
            // northWestTerrainBox
            // 
            this.northWestTerrainBox.Location = new System.Drawing.Point(56, 72);
            this.northWestTerrainBox.Name = "northWestTerrainBox";
            this.northWestTerrainBox.Size = new System.Drawing.Size(100, 20);
            this.northWestTerrainBox.TabIndex = 2;
            // 
            // northWestBorderBox
            // 
            this.northWestBorderBox.Location = new System.Drawing.Point(56, 46);
            this.northWestBorderBox.Name = "northWestBorderBox";
            this.northWestBorderBox.Size = new System.Drawing.Size(100, 20);
            this.northWestBorderBox.TabIndex = 1;
            // 
            // northWestHeaderBox
            // 
            this.northWestHeaderBox.Location = new System.Drawing.Point(56, 20);
            this.northWestHeaderBox.Name = "northWestHeaderBox";
            this.northWestHeaderBox.Size = new System.Drawing.Size(100, 20);
            this.northWestHeaderBox.TabIndex = 0;
            // 
            // SEPrev
            // 
            this.SEPrev.Location = new System.Drawing.Point(1001, 311);
            this.SEPrev.Name = "SEPrev";
            this.SEPrev.Size = new System.Drawing.Size(140, 140);
            this.SEPrev.TabIndex = 77;
            this.SEPrev.Paint += new System.Windows.Forms.PaintEventHandler(this.SEPrev_Paint);
            // 
            // SPrev
            // 
            this.SPrev.Location = new System.Drawing.Point(855, 311);
            this.SPrev.Name = "SPrev";
            this.SPrev.Size = new System.Drawing.Size(140, 140);
            this.SPrev.TabIndex = 76;
            this.SPrev.Paint += new System.Windows.Forms.PaintEventHandler(this.SPrev_Paint);
            // 
            // EPrev
            // 
            this.EPrev.Location = new System.Drawing.Point(1001, 165);
            this.EPrev.Name = "EPrev";
            this.EPrev.Size = new System.Drawing.Size(140, 140);
            this.EPrev.TabIndex = 75;
            this.EPrev.Paint += new System.Windows.Forms.PaintEventHandler(this.EPrev_Paint);
            // 
            // CPrev
            // 
            this.CPrev.Location = new System.Drawing.Point(855, 165);
            this.CPrev.Name = "CPrev";
            this.CPrev.Size = new System.Drawing.Size(140, 140);
            this.CPrev.TabIndex = 74;
            this.CPrev.Paint += new System.Windows.Forms.PaintEventHandler(this.CPrev_Paint);
            // 
            // NEPrev
            // 
            this.NEPrev.Location = new System.Drawing.Point(1001, 19);
            this.NEPrev.Name = "NEPrev";
            this.NEPrev.Size = new System.Drawing.Size(140, 140);
            this.NEPrev.TabIndex = 73;
            this.NEPrev.Paint += new System.Windows.Forms.PaintEventHandler(this.NEPrev_Paint);
            // 
            // NPrev
            // 
            this.NPrev.Location = new System.Drawing.Point(855, 19);
            this.NPrev.Name = "NPrev";
            this.NPrev.Size = new System.Drawing.Size(140, 140);
            this.NPrev.TabIndex = 72;
            this.NPrev.Paint += new System.Windows.Forms.PaintEventHandler(this.NPrev_Paint);
            // 
            // SWPrev
            // 
            this.SWPrev.Location = new System.Drawing.Point(709, 311);
            this.SWPrev.Name = "SWPrev";
            this.SWPrev.Size = new System.Drawing.Size(140, 140);
            this.SWPrev.TabIndex = 69;
            this.SWPrev.Paint += new System.Windows.Forms.PaintEventHandler(this.SWPrev_Paint);
            // 
            // WPrev
            // 
            this.WPrev.Location = new System.Drawing.Point(709, 165);
            this.WPrev.Name = "WPrev";
            this.WPrev.Size = new System.Drawing.Size(140, 140);
            this.WPrev.TabIndex = 71;
            this.WPrev.Paint += new System.Windows.Forms.PaintEventHandler(this.WPrev_Paint);
            // 
            // NWPrev
            // 
            this.NWPrev.Location = new System.Drawing.Point(709, 19);
            this.NWPrev.Name = "NWPrev";
            this.NWPrev.Size = new System.Drawing.Size(140, 140);
            this.NWPrev.TabIndex = 68;
            this.NWPrev.Paint += new System.Windows.Forms.PaintEventHandler(this.NWPrev_Paint);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label37);
            this.groupBox6.Controls.Add(this.label38);
            this.groupBox6.Controls.Add(this.label39);
            this.groupBox6.Controls.Add(this.southTerrainBox);
            this.groupBox6.Controls.Add(this.southBorderBox);
            this.groupBox6.Controls.Add(this.southHeaderBox);
            this.groupBox6.Location = new System.Drawing.Point(222, 287);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(173, 100);
            this.groupBox6.TabIndex = 66;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Southern";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(9, 75);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(40, 13);
            this.label37.TabIndex = 5;
            this.label37.Text = "Terrain";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(9, 53);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(43, 13);
            this.label38.TabIndex = 4;
            this.label38.Text = "Borders";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(9, 27);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(42, 13);
            this.label39.TabIndex = 3;
            this.label39.Text = "Header";
            // 
            // southTerrainBox
            // 
            this.southTerrainBox.Location = new System.Drawing.Point(56, 72);
            this.southTerrainBox.Name = "southTerrainBox";
            this.southTerrainBox.Size = new System.Drawing.Size(100, 20);
            this.southTerrainBox.TabIndex = 2;
            // 
            // southBorderBox
            // 
            this.southBorderBox.Location = new System.Drawing.Point(56, 46);
            this.southBorderBox.Name = "southBorderBox";
            this.southBorderBox.Size = new System.Drawing.Size(100, 20);
            this.southBorderBox.TabIndex = 1;
            // 
            // southHeaderBox
            // 
            this.southHeaderBox.Location = new System.Drawing.Point(57, 19);
            this.southHeaderBox.Name = "southHeaderBox";
            this.southHeaderBox.Size = new System.Drawing.Size(100, 20);
            this.southHeaderBox.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label34);
            this.groupBox5.Controls.Add(this.label35);
            this.groupBox5.Controls.Add(this.label36);
            this.groupBox5.Controls.Add(this.actualTerrainBox);
            this.groupBox5.Controls.Add(this.actualBorderBox);
            this.groupBox5.Controls.Add(this.actualHeaderBox);
            this.groupBox5.Location = new System.Drawing.Point(222, 154);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(173, 100);
            this.groupBox5.TabIndex = 65;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Actual";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(9, 75);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(40, 13);
            this.label34.TabIndex = 5;
            this.label34.Text = "Terrain";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(9, 53);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(43, 13);
            this.label35.TabIndex = 4;
            this.label35.Text = "Borders";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(9, 27);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(42, 13);
            this.label36.TabIndex = 3;
            this.label36.Text = "Header";
            // 
            // actualTerrainBox
            // 
            this.actualTerrainBox.Location = new System.Drawing.Point(56, 72);
            this.actualTerrainBox.Name = "actualTerrainBox";
            this.actualTerrainBox.Size = new System.Drawing.Size(100, 20);
            this.actualTerrainBox.TabIndex = 2;
            // 
            // actualBorderBox
            // 
            this.actualBorderBox.Location = new System.Drawing.Point(56, 46);
            this.actualBorderBox.Name = "actualBorderBox";
            this.actualBorderBox.Size = new System.Drawing.Size(100, 20);
            this.actualBorderBox.TabIndex = 1;
            // 
            // actualHeaderBox
            // 
            this.actualHeaderBox.Location = new System.Drawing.Point(56, 20);
            this.actualHeaderBox.Name = "actualHeaderBox";
            this.actualHeaderBox.Size = new System.Drawing.Size(100, 20);
            this.actualHeaderBox.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label31);
            this.groupBox4.Controls.Add(this.label32);
            this.groupBox4.Controls.Add(this.label33);
            this.groupBox4.Controls.Add(this.westTerrainBox);
            this.groupBox4.Controls.Add(this.westBorderBox);
            this.groupBox4.Controls.Add(this.westHeaderMap);
            this.groupBox4.Location = new System.Drawing.Point(10, 152);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(173, 100);
            this.groupBox4.TabIndex = 64;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Western";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(9, 75);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(40, 13);
            this.label31.TabIndex = 5;
            this.label31.Text = "Terrain";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(9, 53);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(43, 13);
            this.label32.TabIndex = 4;
            this.label32.Text = "Borders";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(9, 27);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(42, 13);
            this.label33.TabIndex = 3;
            this.label33.Text = "Header";
            // 
            // westTerrainBox
            // 
            this.westTerrainBox.Location = new System.Drawing.Point(56, 72);
            this.westTerrainBox.Name = "westTerrainBox";
            this.westTerrainBox.Size = new System.Drawing.Size(100, 20);
            this.westTerrainBox.TabIndex = 2;
            // 
            // westBorderBox
            // 
            this.westBorderBox.Location = new System.Drawing.Point(56, 46);
            this.westBorderBox.Name = "westBorderBox";
            this.westBorderBox.Size = new System.Drawing.Size(100, 20);
            this.westBorderBox.TabIndex = 1;
            // 
            // westHeaderMap
            // 
            this.westHeaderMap.Location = new System.Drawing.Point(56, 20);
            this.westHeaderMap.Name = "westHeaderMap";
            this.westHeaderMap.Size = new System.Drawing.Size(100, 20);
            this.westHeaderMap.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label28);
            this.groupBox3.Controls.Add(this.label29);
            this.groupBox3.Controls.Add(this.label30);
            this.groupBox3.Controls.Add(this.eastTerrainBox);
            this.groupBox3.Controls.Add(this.eastBorderBox);
            this.groupBox3.Controls.Add(this.eastHeaderBox);
            this.groupBox3.Location = new System.Drawing.Point(434, 154);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(173, 100);
            this.groupBox3.TabIndex = 63;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Eastern";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(9, 75);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(40, 13);
            this.label28.TabIndex = 5;
            this.label28.Text = "Terrain";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(9, 53);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(43, 13);
            this.label29.TabIndex = 4;
            this.label29.Text = "Borders";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(9, 27);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(42, 13);
            this.label30.TabIndex = 3;
            this.label30.Text = "Header";
            // 
            // eastTerrainBox
            // 
            this.eastTerrainBox.Location = new System.Drawing.Point(56, 72);
            this.eastTerrainBox.Name = "eastTerrainBox";
            this.eastTerrainBox.Size = new System.Drawing.Size(100, 20);
            this.eastTerrainBox.TabIndex = 2;
            // 
            // eastBorderBox
            // 
            this.eastBorderBox.Location = new System.Drawing.Point(56, 46);
            this.eastBorderBox.Name = "eastBorderBox";
            this.eastBorderBox.Size = new System.Drawing.Size(100, 20);
            this.eastBorderBox.TabIndex = 1;
            // 
            // eastHeaderBox
            // 
            this.eastHeaderBox.Location = new System.Drawing.Point(57, 19);
            this.eastHeaderBox.Name = "eastHeaderBox";
            this.eastHeaderBox.Size = new System.Drawing.Size(100, 20);
            this.eastHeaderBox.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label27);
            this.groupBox2.Controls.Add(this.label26);
            this.groupBox2.Controls.Add(this.label25);
            this.groupBox2.Controls.Add(this.northTerrainBox);
            this.groupBox2.Controls.Add(this.northBorderBox);
            this.groupBox2.Controls.Add(this.northHeaderBox);
            this.groupBox2.Location = new System.Drawing.Point(222, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(173, 100);
            this.groupBox2.TabIndex = 59;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Northern";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(9, 75);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(40, 13);
            this.label27.TabIndex = 5;
            this.label27.Text = "Terrain";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(9, 53);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(43, 13);
            this.label26.TabIndex = 4;
            this.label26.Text = "Borders";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(9, 27);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(42, 13);
            this.label25.TabIndex = 3;
            this.label25.Text = "Header";
            // 
            // northTerrainBox
            // 
            this.northTerrainBox.Location = new System.Drawing.Point(56, 72);
            this.northTerrainBox.Name = "northTerrainBox";
            this.northTerrainBox.Size = new System.Drawing.Size(100, 20);
            this.northTerrainBox.TabIndex = 2;
            // 
            // northBorderBox
            // 
            this.northBorderBox.Location = new System.Drawing.Point(56, 46);
            this.northBorderBox.Name = "northBorderBox";
            this.northBorderBox.Size = new System.Drawing.Size(100, 20);
            this.northBorderBox.TabIndex = 1;
            // 
            // northHeaderBox
            // 
            this.northHeaderBox.Location = new System.Drawing.Point(56, 20);
            this.northHeaderBox.Name = "northHeaderBox";
            this.northHeaderBox.Size = new System.Drawing.Size(100, 20);
            this.northHeaderBox.TabIndex = 0;
            // 
            // Headers
            // 
            this.Headers.Controls.Add(this.matrixHeaderInfo);
            this.Headers.Location = new System.Drawing.Point(4, 22);
            this.Headers.Name = "Headers";
            this.Headers.Padding = new System.Windows.Forms.Padding(3);
            this.Headers.Size = new System.Drawing.Size(1247, 483);
            this.Headers.TabIndex = 0;
            this.Headers.Text = "Headers";
            this.Headers.UseVisualStyleBackColor = true;
            // 
            // matrixHeaderInfo
            // 
            this.matrixHeaderInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.matrixHeaderInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.matrixHeaderInfo.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.matrixHeaderInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.matrixHeaderInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.matrixHeaderInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle21.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("Microsoft Sans Serif", 4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle21.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle21.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.matrixHeaderInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle21;
            this.matrixHeaderInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.matrixHeaderInfo.GridColor = System.Drawing.SystemColors.Control;
            this.matrixHeaderInfo.Location = new System.Drawing.Point(27, 25);
            this.matrixHeaderInfo.Name = "matrixHeaderInfo";
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle22.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle22.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle22.Format = "N2";
            dataGridViewCellStyle22.NullValue = null;
            dataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle22.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.matrixHeaderInfo.RowHeadersDefaultCellStyle = dataGridViewCellStyle22;
            this.matrixHeaderInfo.RowHeadersWidth = 4;
            this.matrixHeaderInfo.RowTemplate.Height = 4;
            this.matrixHeaderInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.matrixHeaderInfo.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.matrixHeaderInfo.Size = new System.Drawing.Size(897, 423);
            this.matrixHeaderInfo.TabIndex = 54;
            // 
            // Borders
            // 
            this.Borders.Controls.Add(this.matrixBorderInfo);
            this.Borders.Location = new System.Drawing.Point(4, 22);
            this.Borders.Name = "Borders";
            this.Borders.Padding = new System.Windows.Forms.Padding(3);
            this.Borders.Size = new System.Drawing.Size(1247, 483);
            this.Borders.TabIndex = 1;
            this.Borders.Text = "Borders";
            this.Borders.UseVisualStyleBackColor = true;
            // 
            // matrixBorderInfo
            // 
            this.matrixBorderInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.matrixBorderInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.matrixBorderInfo.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.matrixBorderInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.matrixBorderInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.matrixBorderInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle23.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle23.Font = new System.Drawing.Font("Microsoft Sans Serif", 4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle23.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle23.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle23.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle23.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.matrixBorderInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle23;
            this.matrixBorderInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.matrixBorderInfo.GridColor = System.Drawing.SystemColors.Control;
            this.matrixBorderInfo.Location = new System.Drawing.Point(29, 17);
            this.matrixBorderInfo.Name = "matrixBorderInfo";
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle24.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle24.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle24.Format = "N2";
            dataGridViewCellStyle24.NullValue = null;
            dataGridViewCellStyle24.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle24.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle24.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.matrixBorderInfo.RowHeadersDefaultCellStyle = dataGridViewCellStyle24;
            this.matrixBorderInfo.RowHeadersWidth = 4;
            this.matrixBorderInfo.RowTemplate.Height = 4;
            this.matrixBorderInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.matrixBorderInfo.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.matrixBorderInfo.Size = new System.Drawing.Size(897, 423);
            this.matrixBorderInfo.TabIndex = 55;
            // 
            // Terrains
            // 
            this.Terrains.Controls.Add(this.matrixTerrainInfo);
            this.Terrains.Location = new System.Drawing.Point(4, 22);
            this.Terrains.Name = "Terrains";
            this.Terrains.Padding = new System.Windows.Forms.Padding(3);
            this.Terrains.Size = new System.Drawing.Size(1247, 483);
            this.Terrains.TabIndex = 2;
            this.Terrains.Text = "Terrain";
            this.Terrains.UseVisualStyleBackColor = true;
            // 
            // matrixTerrainInfo
            // 
            this.matrixTerrainInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.matrixTerrainInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.matrixTerrainInfo.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.matrixTerrainInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.matrixTerrainInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.matrixTerrainInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle25.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle25.Font = new System.Drawing.Font("Microsoft Sans Serif", 4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle25.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle25.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle25.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle25.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.matrixTerrainInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle25;
            this.matrixTerrainInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.matrixTerrainInfo.GridColor = System.Drawing.SystemColors.Control;
            this.matrixTerrainInfo.Location = new System.Drawing.Point(28, 26);
            this.matrixTerrainInfo.Name = "matrixTerrainInfo";
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle26.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle26.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle26.Format = "N2";
            dataGridViewCellStyle26.NullValue = null;
            dataGridViewCellStyle26.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle26.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.matrixTerrainInfo.RowHeadersDefaultCellStyle = dataGridViewCellStyle26;
            this.matrixTerrainInfo.RowHeadersWidth = 4;
            this.matrixTerrainInfo.RowTemplate.Height = 4;
            this.matrixTerrainInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.matrixTerrainInfo.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.matrixTerrainInfo.Size = new System.Drawing.Size(821, 423);
            this.matrixTerrainInfo.TabIndex = 55;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.subScripts);
            this.tabPage5.Controls.Add(this.scriptBoxEditor);
            this.tabPage5.Controls.Add(this.scriptBoxViewer);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1296, 532);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "Scripts";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // subScripts
            // 
            this.subScripts.FormattingEnabled = true;
            this.subScripts.Location = new System.Drawing.Point(570, 19);
            this.subScripts.Name = "subScripts";
            this.subScripts.Size = new System.Drawing.Size(121, 21);
            this.subScripts.TabIndex = 2;
            this.subScripts.SelectedIndexChanged += new System.EventHandler(this.subScripts_SelectedIndexChanged);
            // 
            // scriptBoxEditor
            // 
            this.scriptBoxEditor.Location = new System.Drawing.Point(570, 51);
            this.scriptBoxEditor.Name = "scriptBoxEditor";
            this.scriptBoxEditor.Size = new System.Drawing.Size(531, 452);
            this.scriptBoxEditor.TabIndex = 1;
            this.scriptBoxEditor.Text = "";
            // 
            // scriptBoxViewer
            // 
            this.scriptBoxViewer.Location = new System.Drawing.Point(25, 51);
            this.scriptBoxViewer.Name = "scriptBoxViewer";
            this.scriptBoxViewer.Size = new System.Drawing.Size(526, 452);
            this.scriptBoxViewer.TabIndex = 0;
            this.scriptBoxViewer.Text = "";
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.textBox);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(1296, 532);
            this.tabPage6.TabIndex = 2;
            this.tabPage6.Text = "Texts";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(31, 22);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(859, 488);
            this.textBox.TabIndex = 21;
            this.textBox.Text = "";
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.tabControl5);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(1296, 532);
            this.tabPage7.TabIndex = 3;
            this.tabPage7.Text = "Events";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // tabControl5
            // 
            this.tabControl5.Controls.Add(this.tabPage10);
            this.tabControl5.Controls.Add(this.People);
            this.tabControl5.Controls.Add(this.Warps);
            this.tabControl5.Controls.Add(this.tabPage11);
            this.tabControl5.Location = new System.Drawing.Point(7, 7);
            this.tabControl5.Name = "tabControl5";
            this.tabControl5.SelectedIndex = 0;
            this.tabControl5.Size = new System.Drawing.Size(1283, 508);
            this.tabControl5.TabIndex = 57;
            // 
            // tabPage10
            // 
            this.tabPage10.Controls.Add(this.FurnInfo);
            this.tabPage10.Controls.Add(this.FurnMapInfo);
            this.tabPage10.Location = new System.Drawing.Point(4, 22);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage10.Size = new System.Drawing.Size(1275, 482);
            this.tabPage10.TabIndex = 2;
            this.tabPage10.Text = "Furniture";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // FurnInfo
            // 
            this.FurnInfo.AllowUserToAddRows = false;
            this.FurnInfo.AllowUserToDeleteRows = false;
            this.FurnInfo.AllowUserToResizeColumns = false;
            this.FurnInfo.AllowUserToResizeRows = false;
            this.FurnInfo.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.FurnInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FurnInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedHorizontal;
            this.FurnInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.FurnInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FurnInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn20,
            this.dataGridViewTextBoxColumn22,
            this.dataGridViewTextBoxColumn24,
            this.dataGridViewTextBoxColumn26,
            this.dataGridViewTextBoxColumn23});
            this.FurnInfo.GridColor = System.Drawing.SystemColors.Control;
            this.FurnInfo.Location = new System.Drawing.Point(692, 11);
            this.FurnInfo.Name = "FurnInfo";
            this.FurnInfo.RowHeadersVisible = false;
            this.FurnInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.FurnInfo.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.FurnInfo.Size = new System.Drawing.Size(572, 237);
            this.FurnInfo.TabIndex = 59;
            // 
            // dataGridViewTextBoxColumn20
            // 
            this.dataGridViewTextBoxColumn20.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn20.HeaderText = "Script";
            this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
            // 
            // dataGridViewTextBoxColumn22
            // 
            this.dataGridViewTextBoxColumn22.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn22.HeaderText = "X";
            this.dataGridViewTextBoxColumn22.Name = "dataGridViewTextBoxColumn22";
            // 
            // dataGridViewTextBoxColumn24
            // 
            this.dataGridViewTextBoxColumn24.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn24.HeaderText = "Y";
            this.dataGridViewTextBoxColumn24.Name = "dataGridViewTextBoxColumn24";
            // 
            // dataGridViewTextBoxColumn26
            // 
            this.dataGridViewTextBoxColumn26.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn26.HeaderText = "Z";
            this.dataGridViewTextBoxColumn26.Name = "dataGridViewTextBoxColumn26";
            // 
            // dataGridViewTextBoxColumn23
            // 
            this.dataGridViewTextBoxColumn23.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn23.HeaderText = "Flag";
            this.dataGridViewTextBoxColumn23.Name = "dataGridViewTextBoxColumn23";
            // 
            // FurnMapInfo
            // 
            this.FurnMapInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.FurnMapInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.FurnMapInfo.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.FurnMapInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.FurnMapInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.FurnMapInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle27.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle27.Font = new System.Drawing.Font("Microsoft Sans Serif", 4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle27.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle27.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle27.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle27.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.FurnMapInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle27;
            this.FurnMapInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FurnMapInfo.GridColor = System.Drawing.SystemColors.Control;
            this.FurnMapInfo.Location = new System.Drawing.Point(11, 11);
            this.FurnMapInfo.Name = "FurnMapInfo";
            dataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle28.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle28.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle28.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle28.Format = "N2";
            dataGridViewCellStyle28.NullValue = null;
            dataGridViewCellStyle28.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle28.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle28.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.FurnMapInfo.RowHeadersDefaultCellStyle = dataGridViewCellStyle28;
            this.FurnMapInfo.RowHeadersWidth = 4;
            this.FurnMapInfo.RowTemplate.Height = 4;
            this.FurnMapInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.FurnMapInfo.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.FurnMapInfo.Size = new System.Drawing.Size(666, 460);
            this.FurnMapInfo.TabIndex = 58;
            // 
            // People
            // 
            this.People.Controls.Add(this.PeopleInfo);
            this.People.Controls.Add(this.PeopleMapInfo);
            this.People.Location = new System.Drawing.Point(4, 22);
            this.People.Name = "People";
            this.People.Padding = new System.Windows.Forms.Padding(3);
            this.People.Size = new System.Drawing.Size(1275, 482);
            this.People.TabIndex = 0;
            this.People.Text = "People";
            this.People.UseVisualStyleBackColor = true;
            // 
            // PeopleInfo
            // 
            this.PeopleInfo.AllowUserToAddRows = false;
            this.PeopleInfo.AllowUserToDeleteRows = false;
            this.PeopleInfo.AllowUserToResizeColumns = false;
            this.PeopleInfo.AllowUserToResizeRows = false;
            this.PeopleInfo.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.PeopleInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PeopleInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedHorizontal;
            this.PeopleInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.PeopleInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PeopleInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10,
            this.Mov,
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12,
            this.LineOfSight,
            this.X,
            this.dataGridViewTextBoxColumn14,
            this.dataGridViewTextBoxColumn16});
            this.PeopleInfo.GridColor = System.Drawing.SystemColors.Control;
            this.PeopleInfo.Location = new System.Drawing.Point(705, 16);
            this.PeopleInfo.Name = "PeopleInfo";
            this.PeopleInfo.RowHeadersVisible = false;
            this.PeopleInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PeopleInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.PeopleInfo.Size = new System.Drawing.Size(564, 294);
            this.PeopleInfo.TabIndex = 56;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn9.HeaderText = "Id";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn10.HeaderText = "Sprite";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // Mov
            // 
            this.Mov.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Mov.HeaderText = "Mov";
            this.Mov.Name = "Mov";
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn11.HeaderText = "Flag";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn12.HeaderText = "Script";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            // 
            // LineOfSight
            // 
            this.LineOfSight.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LineOfSight.HeaderText = "Line Of Sight";
            this.LineOfSight.Name = "LineOfSight";
            // 
            // X
            // 
            this.X.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.X.HeaderText = "X";
            this.X.Name = "X";
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn14.HeaderText = "Y";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn16.HeaderText = "Z";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            // 
            // PeopleMapInfo
            // 
            this.PeopleMapInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.PeopleMapInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.PeopleMapInfo.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.PeopleMapInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PeopleMapInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.PeopleMapInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle29.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle29.Font = new System.Drawing.Font("Microsoft Sans Serif", 4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle29.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle29.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle29.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle29.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.PeopleMapInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle29;
            this.PeopleMapInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PeopleMapInfo.GridColor = System.Drawing.SystemColors.Control;
            this.PeopleMapInfo.Location = new System.Drawing.Point(16, 16);
            this.PeopleMapInfo.Name = "PeopleMapInfo";
            dataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle30.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle30.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle30.Format = "N2";
            dataGridViewCellStyle30.NullValue = null;
            dataGridViewCellStyle30.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle30.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle30.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.PeopleMapInfo.RowHeadersDefaultCellStyle = dataGridViewCellStyle30;
            this.PeopleMapInfo.RowHeadersWidth = 4;
            this.PeopleMapInfo.RowTemplate.Height = 4;
            this.PeopleMapInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PeopleMapInfo.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.PeopleMapInfo.Size = new System.Drawing.Size(666, 460);
            this.PeopleMapInfo.TabIndex = 55;
            // 
            // Warps
            // 
            this.Warps.Controls.Add(this.WarpInfo);
            this.Warps.Controls.Add(this.WarpsMapInfo);
            this.Warps.Location = new System.Drawing.Point(4, 22);
            this.Warps.Name = "Warps";
            this.Warps.Padding = new System.Windows.Forms.Padding(3);
            this.Warps.Size = new System.Drawing.Size(1275, 482);
            this.Warps.TabIndex = 1;
            this.Warps.Text = "Warps";
            this.Warps.UseVisualStyleBackColor = true;
            // 
            // WarpInfo
            // 
            this.WarpInfo.AllowUserToAddRows = false;
            this.WarpInfo.AllowUserToDeleteRows = false;
            this.WarpInfo.AllowUserToResizeColumns = false;
            this.WarpInfo.AllowUserToResizeRows = false;
            this.WarpInfo.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.WarpInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.WarpInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedHorizontal;
            this.WarpInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.WarpInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.WarpInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn33,
            this.dataGridViewTextBoxColumn35,
            this.dataGridViewTextBoxColumn37,
            this.dataGridViewTextBoxColumn38});
            this.WarpInfo.GridColor = System.Drawing.SystemColors.Control;
            this.WarpInfo.Location = new System.Drawing.Point(687, 11);
            this.WarpInfo.Name = "WarpInfo";
            this.WarpInfo.RowHeadersVisible = false;
            this.WarpInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.WarpInfo.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.WarpInfo.Size = new System.Drawing.Size(572, 237);
            this.WarpInfo.TabIndex = 61;
            // 
            // dataGridViewTextBoxColumn33
            // 
            this.dataGridViewTextBoxColumn33.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn33.HeaderText = "X";
            this.dataGridViewTextBoxColumn33.Name = "dataGridViewTextBoxColumn33";
            // 
            // dataGridViewTextBoxColumn35
            // 
            this.dataGridViewTextBoxColumn35.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn35.HeaderText = "Y";
            this.dataGridViewTextBoxColumn35.Name = "dataGridViewTextBoxColumn35";
            // 
            // dataGridViewTextBoxColumn37
            // 
            this.dataGridViewTextBoxColumn37.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn37.HeaderText = "Map Id";
            this.dataGridViewTextBoxColumn37.Name = "dataGridViewTextBoxColumn37";
            // 
            // dataGridViewTextBoxColumn38
            // 
            this.dataGridViewTextBoxColumn38.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn38.HeaderText = "Map Anchor";
            this.dataGridViewTextBoxColumn38.Name = "dataGridViewTextBoxColumn38";
            // 
            // WarpsMapInfo
            // 
            this.WarpsMapInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.WarpsMapInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.WarpsMapInfo.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.WarpsMapInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.WarpsMapInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.WarpsMapInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle31.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle31.Font = new System.Drawing.Font("Microsoft Sans Serif", 4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle31.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle31.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle31.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle31.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.WarpsMapInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle31;
            this.WarpsMapInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.WarpsMapInfo.GridColor = System.Drawing.SystemColors.Control;
            this.WarpsMapInfo.Location = new System.Drawing.Point(15, 11);
            this.WarpsMapInfo.Name = "WarpsMapInfo";
            dataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle32.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle32.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle32.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle32.Format = "N2";
            dataGridViewCellStyle32.NullValue = null;
            dataGridViewCellStyle32.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle32.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle32.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.WarpsMapInfo.RowHeadersDefaultCellStyle = dataGridViewCellStyle32;
            this.WarpsMapInfo.RowHeadersWidth = 4;
            this.WarpsMapInfo.RowTemplate.Height = 4;
            this.WarpsMapInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.WarpsMapInfo.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.WarpsMapInfo.Size = new System.Drawing.Size(666, 460);
            this.WarpsMapInfo.TabIndex = 60;
            // 
            // tabPage11
            // 
            this.tabPage11.Controls.Add(this.TriggerInfo);
            this.tabPage11.Controls.Add(this.TriggerMapInfo);
            this.tabPage11.Location = new System.Drawing.Point(4, 22);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage11.Size = new System.Drawing.Size(1275, 482);
            this.tabPage11.TabIndex = 3;
            this.tabPage11.Text = "Triggers";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // TriggerInfo
            // 
            this.TriggerInfo.AllowUserToAddRows = false;
            this.TriggerInfo.AllowUserToDeleteRows = false;
            this.TriggerInfo.AllowUserToResizeColumns = false;
            this.TriggerInfo.AllowUserToResizeRows = false;
            this.TriggerInfo.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.TriggerInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TriggerInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedHorizontal;
            this.TriggerInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.TriggerInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TriggerInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn42,
            this.dataGridViewTextBoxColumn44,
            this.dataGridViewTextBoxColumn46,
            this.dataGridViewTextBoxColumn52,
            this.dataGridViewTextBoxColumn51,
            this.dataGridViewTextBoxColumn50});
            this.TriggerInfo.GridColor = System.Drawing.SystemColors.Control;
            this.TriggerInfo.Location = new System.Drawing.Point(692, 11);
            this.TriggerInfo.Name = "TriggerInfo";
            this.TriggerInfo.RowHeadersVisible = false;
            this.TriggerInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TriggerInfo.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.TriggerInfo.Size = new System.Drawing.Size(572, 237);
            this.TriggerInfo.TabIndex = 59;
            // 
            // dataGridViewTextBoxColumn42
            // 
            this.dataGridViewTextBoxColumn42.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn42.HeaderText = "Script";
            this.dataGridViewTextBoxColumn42.Name = "dataGridViewTextBoxColumn42";
            // 
            // dataGridViewTextBoxColumn44
            // 
            this.dataGridViewTextBoxColumn44.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn44.HeaderText = "X";
            this.dataGridViewTextBoxColumn44.Name = "dataGridViewTextBoxColumn44";
            // 
            // dataGridViewTextBoxColumn46
            // 
            this.dataGridViewTextBoxColumn46.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn46.HeaderText = "Y";
            this.dataGridViewTextBoxColumn46.Name = "dataGridViewTextBoxColumn46";
            // 
            // dataGridViewTextBoxColumn52
            // 
            this.dataGridViewTextBoxColumn52.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn52.HeaderText = "Width";
            this.dataGridViewTextBoxColumn52.Name = "dataGridViewTextBoxColumn52";
            // 
            // dataGridViewTextBoxColumn51
            // 
            this.dataGridViewTextBoxColumn51.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn51.HeaderText = "Length";
            this.dataGridViewTextBoxColumn51.Name = "dataGridViewTextBoxColumn51";
            // 
            // dataGridViewTextBoxColumn50
            // 
            this.dataGridViewTextBoxColumn50.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn50.HeaderText = "Flag";
            this.dataGridViewTextBoxColumn50.Name = "dataGridViewTextBoxColumn50";
            // 
            // TriggerMapInfo
            // 
            this.TriggerMapInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.TriggerMapInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.TriggerMapInfo.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.TriggerMapInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.TriggerMapInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.TriggerMapInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle33.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle33.Font = new System.Drawing.Font("Microsoft Sans Serif", 4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle33.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle33.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle33.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle33.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.TriggerMapInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle33;
            this.TriggerMapInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TriggerMapInfo.GridColor = System.Drawing.SystemColors.Control;
            this.TriggerMapInfo.Location = new System.Drawing.Point(11, 11);
            this.TriggerMapInfo.Name = "TriggerMapInfo";
            dataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle34.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle34.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle34.Format = "N2";
            dataGridViewCellStyle34.NullValue = null;
            dataGridViewCellStyle34.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle34.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle34.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.TriggerMapInfo.RowHeadersDefaultCellStyle = dataGridViewCellStyle34;
            this.TriggerMapInfo.RowHeadersWidth = 4;
            this.TriggerMapInfo.RowTemplate.Height = 4;
            this.TriggerMapInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TriggerMapInfo.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.TriggerMapInfo.Size = new System.Drawing.Size(666, 460);
            this.TriggerMapInfo.TabIndex = 58;
            // 
            // AssMap_T
            // 
            this.AssMap_T.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.AssMap_T.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AssMap_T.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.s,
            this.dataGridViewTextBoxColumn7,
            this.Texture_2,
            this.Column2,
            this.Script,
            this.Trainer,
            this.Texts,
            this.Encount,
            this.Column3,
            this.Column4,
            this.Event,
            this.dataGridViewTextBoxColumn8,
            this.Column1,
            this.Par,
            this.Column5,
            this.Flag,
            this.Terrain});
            this.AssMap_T.GridColor = System.Drawing.SystemColors.ButtonHighlight;
            this.AssMap_T.Location = new System.Drawing.Point(16, 20);
            this.AssMap_T.Name = "AssMap_T";
            this.AssMap_T.RowHeadersVisible = false;
            this.AssMap_T.Size = new System.Drawing.Size(1317, 78);
            this.AssMap_T.TabIndex = 9;
            this.AssMap_T.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.AssMap_T_CellContentClick);
            // 
            // Id
            // 
            this.Id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.Width = 41;
            // 
            // s
            // 
            this.s.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.s.HeaderText = "Internal Name";
            this.s.Name = "s";
            this.s.Width = 90;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn7.HeaderText = "Main Texture";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Width = 87;
            // 
            // Texture_2
            // 
            this.Texture_2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Texture_2.HeaderText = "Building Textures";
            this.Texture_2.Name = "Texture_2";
            this.Texture_2.Width = 104;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column2.HeaderText = "Matrix";
            this.Column2.Name = "Column2";
            this.Column2.Width = 60;
            // 
            // Script
            // 
            this.Script.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Script.HeaderText = "Scripts";
            this.Script.Name = "Script";
            // 
            // Trainer
            // 
            this.Trainer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Trainer.HeaderText = "Trainer Data";
            this.Trainer.Name = "Trainer";
            // 
            // Texts
            // 
            this.Texts.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Texts.HeaderText = "Texts";
            this.Texts.Name = "Texts";
            // 
            // Encount
            // 
            this.Encount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Encount.HeaderText = "Music(Day)";
            this.Encount.Name = "Encount";
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column3.HeaderText = "Music(Night)";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column4.HeaderText = "Wild Pokèmon";
            this.Column4.Name = "Column4";
            // 
            // Event
            // 
            this.Event.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Event.HeaderText = "Events";
            this.Event.Name = "Event";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn8.HeaderText = "Name";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Width = 60;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.HeaderText = "Weather";
            this.Column1.Name = "Column1";
            // 
            // Par
            // 
            this.Par.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Par.HeaderText = "Camera";
            this.Par.Name = "Par";
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column5.HeaderText = "Name Style";
            this.Column5.Name = "Column5";
            // 
            // Flag
            // 
            this.Flag.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Flag.HeaderText = "Flags";
            this.Flag.Name = "Flag";
            // 
            // Terrain
            // 
            this.Terrain.HeaderText = "Terrain(Der.)";
            this.Terrain.Name = "Terrain";
            // 
            // Objects
            // 
            this.Objects.Controls.Add(this.objShowInfo);
            this.Objects.Controls.Add(this.SaveObj);
            this.Objects.Controls.Add(this.DelObj);
            this.Objects.Controls.Add(this.AddObj);
            this.Objects.Controls.Add(this.ObjInfo);
            this.Objects.Location = new System.Drawing.Point(4, 22);
            this.Objects.Name = "Objects";
            this.Objects.Size = new System.Drawing.Size(1339, 683);
            this.Objects.TabIndex = 2;
            this.Objects.Text = "Objects";
            this.Objects.UseVisualStyleBackColor = true;
            // 
            // objShowInfo
            // 
            this.objShowInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.objShowInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.objShowInfo.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.objShowInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.objShowInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.objShowInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle35.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle35.Font = new System.Drawing.Font("Microsoft Sans Serif", 4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle35.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle35.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle35.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle35.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.objShowInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle35;
            this.objShowInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.objShowInfo.GridColor = System.Drawing.SystemColors.Control;
            this.objShowInfo.Location = new System.Drawing.Point(32, 18);
            this.objShowInfo.Name = "objShowInfo";
            dataGridViewCellStyle36.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle36.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle36.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle36.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle36.Format = "N2";
            dataGridViewCellStyle36.NullValue = null;
            dataGridViewCellStyle36.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle36.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle36.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.objShowInfo.RowHeadersDefaultCellStyle = dataGridViewCellStyle36;
            this.objShowInfo.RowHeadersWidth = 4;
            this.objShowInfo.RowTemplate.Height = 4;
            this.objShowInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.objShowInfo.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.objShowInfo.Size = new System.Drawing.Size(666, 639);
            this.objShowInfo.TabIndex = 52;
            // 
            // SaveObj
            // 
            this.SaveObj.Location = new System.Drawing.Point(925, 351);
            this.SaveObj.Name = "SaveObj";
            this.SaveObj.Size = new System.Drawing.Size(75, 23);
            this.SaveObj.TabIndex = 51;
            this.SaveObj.Text = "Save";
            this.SaveObj.UseVisualStyleBackColor = true;
            this.SaveObj.Click += new System.EventHandler(this.SavObj_Click);
            // 
            // DelObj
            // 
            this.DelObj.Location = new System.Drawing.Point(844, 351);
            this.DelObj.Name = "DelObj";
            this.DelObj.Size = new System.Drawing.Size(75, 23);
            this.DelObj.TabIndex = 50;
            this.DelObj.Text = "Delete";
            this.DelObj.UseVisualStyleBackColor = true;
            this.DelObj.Click += new System.EventHandler(this.DelObj_Click);
            // 
            // AddObj
            // 
            this.AddObj.Location = new System.Drawing.Point(763, 351);
            this.AddObj.Name = "AddObj";
            this.AddObj.Size = new System.Drawing.Size(75, 23);
            this.AddObj.TabIndex = 49;
            this.AddObj.Text = "Add";
            this.AddObj.UseVisualStyleBackColor = true;
            this.AddObj.Click += new System.EventHandler(this.AddObj_Click);
            // 
            // ObjInfo
            // 
            this.ObjInfo.AllowUserToAddRows = false;
            this.ObjInfo.AllowUserToDeleteRows = false;
            this.ObjInfo.AllowUserToResizeColumns = false;
            this.ObjInfo.AllowUserToResizeRows = false;
            this.ObjInfo.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.ObjInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ObjInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedHorizontal;
            this.ObjInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.ObjInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ObjInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Count,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.Xf,
            this.dataGridViewTextBoxColumn5,
            this.Yf,
            this.dataGridViewTextBoxColumn6,
            this.Zf,
            this.H,
            this.L,
            this.W});
            this.ObjInfo.GridColor = System.Drawing.SystemColors.Control;
            this.ObjInfo.Location = new System.Drawing.Point(704, 18);
            this.ObjInfo.Name = "ObjInfo";
            this.ObjInfo.RowHeadersVisible = false;
            this.ObjInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ObjInfo.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.ObjInfo.Size = new System.Drawing.Size(622, 327);
            this.ObjInfo.TabIndex = 48;
            // 
            // Count
            // 
            this.Count.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Count.HeaderText = "Count";
            this.Count.Name = "Count";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.HeaderText = "Id";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.HeaderText = "X8";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // Xf
            // 
            this.Xf.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Xf.HeaderText = "X-Flag";
            this.Xf.Name = "Xf";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.HeaderText = "Y";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // Yf
            // 
            this.Yf.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Yf.HeaderText = "Y-Flag";
            this.Yf.Name = "Yf";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.HeaderText = "Z";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // Zf
            // 
            this.Zf.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Zf.HeaderText = "Z-Flag";
            this.Zf.Name = "Zf";
            // 
            // H
            // 
            this.H.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.H.HeaderText = "Height";
            this.H.Name = "H";
            // 
            // L
            // 
            this.L.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.L.HeaderText = "Length";
            this.L.Name = "L";
            // 
            // W
            // 
            this.W.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.W.HeaderText = "Width";
            this.W.Name = "W";
            // 
            // Movements
            // 
            this.Movements.Controls.Add(this.tabControl3);
            this.Movements.Location = new System.Drawing.Point(4, 22);
            this.Movements.Name = "Movements";
            this.Movements.Padding = new System.Windows.Forms.Padding(3);
            this.Movements.Size = new System.Drawing.Size(1339, 683);
            this.Movements.TabIndex = 1;
            this.Movements.Text = "Movements";
            this.Movements.UseVisualStyleBackColor = true;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.Layer_0);
            this.tabControl3.Controls.Add(this.Layer_1);
            this.tabControl3.Controls.Add(this.Layer_2);
            this.tabControl3.Controls.Add(this.Layer_3);
            this.tabControl3.Location = new System.Drawing.Point(6, 0);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(1310, 687);
            this.tabControl3.TabIndex = 53;
            // 
            // Layer_0
            // 
            this.Layer_0.Controls.Add(this.saveMovInfoL0);
            this.Layer_0.Controls.Add(this.MovInfo);
            this.Layer_0.Location = new System.Drawing.Point(4, 22);
            this.Layer_0.Name = "Layer_0";
            this.Layer_0.Padding = new System.Windows.Forms.Padding(3);
            this.Layer_0.Size = new System.Drawing.Size(1302, 661);
            this.Layer_0.TabIndex = 0;
            this.Layer_0.Text = "Layer 0";
            this.Layer_0.UseVisualStyleBackColor = true;
            // 
            // saveMovInfoL0
            // 
            this.saveMovInfoL0.Location = new System.Drawing.Point(1221, 6);
            this.saveMovInfoL0.Name = "saveMovInfoL0";
            this.saveMovInfoL0.Size = new System.Drawing.Size(75, 23);
            this.saveMovInfoL0.TabIndex = 54;
            this.saveMovInfoL0.Text = "Save";
            this.saveMovInfoL0.UseVisualStyleBackColor = true;
            this.saveMovInfoL0.Click += new System.EventHandler(this.saveMov_Click);
            // 
            // MovInfo
            // 
            this.MovInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.MovInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.MovInfo.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.MovInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MovInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.MovInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle37.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle37.Font = new System.Drawing.Font("Microsoft Sans Serif", 4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle37.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle37.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle37.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle37.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MovInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle37;
            this.MovInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MovInfo.GridColor = System.Drawing.SystemColors.Control;
            this.MovInfo.Location = new System.Drawing.Point(14, 11);
            this.MovInfo.Name = "MovInfo";
            this.MovInfo.RowHeadersVisible = false;
            this.MovInfo.RowHeadersWidth = 4;
            this.MovInfo.RowTemplate.Height = 4;
            this.MovInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.MovInfo.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.MovInfo.Size = new System.Drawing.Size(1197, 639);
            this.MovInfo.TabIndex = 53;
            // 
            // Layer_1
            // 
            this.Layer_1.Controls.Add(this.saveMovInfoL1);
            this.Layer_1.Controls.Add(this.MovInfoL1);
            this.Layer_1.Location = new System.Drawing.Point(4, 22);
            this.Layer_1.Name = "Layer_1";
            this.Layer_1.Padding = new System.Windows.Forms.Padding(3);
            this.Layer_1.Size = new System.Drawing.Size(1302, 661);
            this.Layer_1.TabIndex = 1;
            this.Layer_1.Text = "Layer 1";
            this.Layer_1.UseVisualStyleBackColor = true;
            this.Layer_1.Click += new System.EventHandler(this.Layer_1_Click);
            // 
            // saveMovInfoL1
            // 
            this.saveMovInfoL1.Location = new System.Drawing.Point(1221, 11);
            this.saveMovInfoL1.Name = "saveMovInfoL1";
            this.saveMovInfoL1.Size = new System.Drawing.Size(75, 23);
            this.saveMovInfoL1.TabIndex = 54;
            this.saveMovInfoL1.Text = "Save";
            this.saveMovInfoL1.UseVisualStyleBackColor = true;
            this.saveMovInfoL1.Click += new System.EventHandler(this.saveMovInfoL1_Click);
            // 
            // MovInfoL1
            // 
            this.MovInfoL1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.MovInfoL1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.MovInfoL1.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.MovInfoL1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MovInfoL1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.MovInfoL1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle38.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle38.Font = new System.Drawing.Font("Microsoft Sans Serif", 4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle38.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle38.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle38.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle38.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MovInfoL1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle38;
            this.MovInfoL1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MovInfoL1.GridColor = System.Drawing.SystemColors.Control;
            this.MovInfoL1.Location = new System.Drawing.Point(14, 11);
            this.MovInfoL1.Name = "MovInfoL1";
            this.MovInfoL1.RowHeadersVisible = false;
            this.MovInfoL1.RowHeadersWidth = 4;
            this.MovInfoL1.RowTemplate.Height = 4;
            this.MovInfoL1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.MovInfoL1.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.MovInfoL1.Size = new System.Drawing.Size(1194, 639);
            this.MovInfoL1.TabIndex = 53;
            // 
            // Layer_2
            // 
            this.Layer_2.Controls.Add(this.button1);
            this.Layer_2.Controls.Add(this.MovInfoL2);
            this.Layer_2.Location = new System.Drawing.Point(4, 22);
            this.Layer_2.Name = "Layer_2";
            this.Layer_2.Size = new System.Drawing.Size(1302, 661);
            this.Layer_2.TabIndex = 2;
            this.Layer_2.Text = "Layer 2";
            this.Layer_2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1214, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 56;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // MovInfoL2
            // 
            this.MovInfoL2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.MovInfoL2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.MovInfoL2.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.MovInfoL2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MovInfoL2.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.MovInfoL2.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle39.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle39.Font = new System.Drawing.Font("Microsoft Sans Serif", 4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle39.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle39.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle39.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle39.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MovInfoL2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle39;
            this.MovInfoL2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MovInfoL2.GridColor = System.Drawing.SystemColors.Control;
            this.MovInfoL2.Location = new System.Drawing.Point(14, 11);
            this.MovInfoL2.Name = "MovInfoL2";
            this.MovInfoL2.RowHeadersVisible = false;
            this.MovInfoL2.RowHeadersWidth = 4;
            this.MovInfoL2.RowTemplate.Height = 4;
            this.MovInfoL2.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.MovInfoL2.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.MovInfoL2.Size = new System.Drawing.Size(1194, 639);
            this.MovInfoL2.TabIndex = 55;
            // 
            // Layer_3
            // 
            this.Layer_3.Controls.Add(this.button2);
            this.Layer_3.Controls.Add(this.MovInfoL3);
            this.Layer_3.Location = new System.Drawing.Point(4, 22);
            this.Layer_3.Name = "Layer_3";
            this.Layer_3.Size = new System.Drawing.Size(1302, 661);
            this.Layer_3.TabIndex = 3;
            this.Layer_3.Text = "Layer 3";
            this.Layer_3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1214, 11);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 56;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // MovInfoL3
            // 
            this.MovInfoL3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.MovInfoL3.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.MovInfoL3.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.MovInfoL3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MovInfoL3.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.MovInfoL3.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle40.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle40.Font = new System.Drawing.Font("Microsoft Sans Serif", 4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle40.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle40.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle40.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle40.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MovInfoL3.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle40;
            this.MovInfoL3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MovInfoL3.GridColor = System.Drawing.SystemColors.Control;
            this.MovInfoL3.Location = new System.Drawing.Point(14, 11);
            this.MovInfoL3.Name = "MovInfoL3";
            this.MovInfoL3.RowHeadersVisible = false;
            this.MovInfoL3.RowHeadersWidth = 4;
            this.MovInfoL3.RowTemplate.Height = 4;
            this.MovInfoL3.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.MovInfoL3.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.MovInfoL3.Size = new System.Drawing.Size(1194, 639);
            this.MovInfoL3.TabIndex = 55;
            // 
            // Nsbmd
            // 
            this.Nsbmd.Controls.Add(this.panel4);
            this.Nsbmd.Controls.Add(this.tabControl2);
            this.Nsbmd.Controls.Add(this.MapList);
            this.Nsbmd.Controls.Add(this.label12);
            this.Nsbmd.Controls.Add(this.Tex_File);
            this.Nsbmd.Controls.Add(this.groupBox1);
            this.Nsbmd.Controls.Add(this.label1);
            this.Nsbmd.Controls.Add(this.label9);
            this.Nsbmd.Controls.Add(this.label8);
            this.Nsbmd.Location = new System.Drawing.Point(4, 22);
            this.Nsbmd.Name = "Nsbmd";
            this.Nsbmd.Padding = new System.Windows.Forms.Padding(3);
            this.Nsbmd.Size = new System.Drawing.Size(1339, 683);
            this.Nsbmd.TabIndex = 0;
            this.Nsbmd.Text = "Nsbmd";
            this.Nsbmd.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.OpenGlControl);
            this.panel4.Location = new System.Drawing.Point(695, 6);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(638, 655);
            this.panel4.TabIndex = 65;
            // 
            // OpenGlControl
            // 
            this.OpenGlControl.AccumBits = ((byte)(0));
            this.OpenGlControl.AutoCheckErrors = false;
            this.OpenGlControl.AutoFinish = false;
            this.OpenGlControl.AutoMakeCurrent = true;
            this.OpenGlControl.AutoSize = true;
            this.OpenGlControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.OpenGlControl.AutoSwapBuffers = true;
            this.OpenGlControl.BackColor = System.Drawing.Color.Black;
            this.OpenGlControl.ColorBits = ((byte)(32));
            this.OpenGlControl.DepthBits = ((byte)(16));
            this.OpenGlControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OpenGlControl.Location = new System.Drawing.Point(0, 0);
            this.OpenGlControl.Name = "OpenGlControl";
            this.OpenGlControl.Size = new System.Drawing.Size(638, 655);
            this.OpenGlControl.StencilBits = ((byte)(0));
            this.OpenGlControl.TabIndex = 2;
            this.OpenGlControl.Paint += new System.Windows.Forms.PaintEventHandler(this.simpleOpenGlControl1_Paint);
            this.OpenGlControl.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.simpleOpenGlControl1_Select);
            this.OpenGlControl.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.simpleOpenGlControl1_PreviewKeyDown);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Location = new System.Drawing.Point(13, 54);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(482, 580);
            this.tabControl2.TabIndex = 63;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label14);
            this.tabPage1.Controls.Add(this.AvaPalList);
            this.tabPage1.Controls.Add(this.label13);
            this.tabPage1.Controls.Add(this.AvaTexList);
            this.tabPage1.Controls.Add(this.SaveMat);
            this.tabPage1.Controls.Add(this.Console);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.AssTable);
            this.tabPage1.Controls.Add(this.label23);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(474, 554);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Materials";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(233, 29);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(86, 13);
            this.label14.TabIndex = 69;
            this.label14.Text = "Available Palette";
            // 
            // AvaPalList
            // 
            this.AvaPalList.Enabled = false;
            this.AvaPalList.FormattingEnabled = true;
            this.AvaPalList.Location = new System.Drawing.Point(233, 48);
            this.AvaPalList.Name = "AvaPalList";
            this.AvaPalList.Size = new System.Drawing.Size(181, 21);
            this.AvaPalList.TabIndex = 68;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 29);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(94, 13);
            this.label13.TabIndex = 67;
            this.label13.Text = "Available Textures";
            // 
            // AvaTexList
            // 
            this.AvaTexList.Enabled = false;
            this.AvaTexList.FormattingEnabled = true;
            this.AvaTexList.Location = new System.Drawing.Point(9, 48);
            this.AvaTexList.Name = "AvaTexList";
            this.AvaTexList.Size = new System.Drawing.Size(181, 21);
            this.AvaTexList.TabIndex = 66;
            // 
            // SaveMat
            // 
            this.SaveMat.Location = new System.Drawing.Point(9, 316);
            this.SaveMat.Name = "SaveMat";
            this.SaveMat.Size = new System.Drawing.Size(61, 23);
            this.SaveMat.TabIndex = 60;
            this.SaveMat.Text = "Save";
            this.SaveMat.UseVisualStyleBackColor = true;
            this.SaveMat.Click += new System.EventHandler(this.SaveMat_Click);
            // 
            // Console
            // 
            this.Console.Location = new System.Drawing.Point(9, 358);
            this.Console.Name = "Console";
            this.Console.Size = new System.Drawing.Size(429, 190);
            this.Console.TabIndex = 64;
            this.Console.Text = "";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 342);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 65;
            this.label7.Text = "Console Log";
            // 
            // AssTable
            // 
            this.AssTable.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.AssTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AssTable.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.AssTable.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.AssTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AssTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Polygon,
            this.Material,
            this.idMaterial,
            this.Texture,
            this.idTexture,
            this.Palette,
            this.idPal});
            this.AssTable.GridColor = System.Drawing.SystemColors.Control;
            this.AssTable.Location = new System.Drawing.Point(9, 84);
            this.AssTable.Name = "AssTable";
            this.AssTable.RowHeadersVisible = false;
            this.AssTable.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.AssTable.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.AssTable.Size = new System.Drawing.Size(443, 213);
            this.AssTable.TabIndex = 47;
            // 
            // Polygon
            // 
            this.Polygon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Polygon.HeaderText = "Polygon";
            this.Polygon.Name = "Polygon";
            // 
            // Material
            // 
            this.Material.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Material.HeaderText = "Material";
            this.Material.Name = "Material";
            // 
            // idMaterial
            // 
            this.idMaterial.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.idMaterial.HeaderText = "idMat";
            this.idMaterial.Name = "idMaterial";
            // 
            // Texture
            // 
            this.Texture.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Texture.HeaderText = "Texture";
            this.Texture.Name = "Texture";
            // 
            // idTexture
            // 
            this.idTexture.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.idTexture.HeaderText = "idTex";
            this.idTexture.Name = "idTexture";
            // 
            // Palette
            // 
            this.Palette.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Palette.HeaderText = "Palette";
            this.Palette.Name = "Palette";
            // 
            // idPal
            // 
            this.idPal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.idPal.HeaderText = "idPal";
            this.idPal.Name = "idPal";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(-88, -44);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(54, 13);
            this.label23.TabIndex = 46;
            this.label23.Text = "Ass.Table";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.functionList);
            this.tabPage2.Controls.Add(this.AddPol);
            this.tabPage2.Controls.Add(this.ResizePol);
            this.tabPage2.Controls.Add(this.ResetPoly);
            this.tabPage2.Controls.Add(this.SavePol);
            this.tabPage2.Controls.Add(this.VerValue);
            this.tabPage2.Controls.Add(this.Rem_Poly);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(474, 554);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Polygon";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // functionList
            // 
            this.functionList.FormattingEnabled = true;
            this.functionList.Location = new System.Drawing.Point(33, 379);
            this.functionList.Name = "functionList";
            this.functionList.Size = new System.Drawing.Size(239, 21);
            this.functionList.TabIndex = 61;
            this.functionList.Text = "Pre-packed Functions";
            this.functionList.SelectedIndexChanged += new System.EventHandler(this.functionList_SelectedIndexChanged);
            // 
            // AddPol
            // 
            this.AddPol.Location = new System.Drawing.Point(119, 326);
            this.AddPol.Name = "AddPol";
            this.AddPol.Size = new System.Drawing.Size(75, 23);
            this.AddPol.TabIndex = 60;
            this.AddPol.Text = "Add";
            this.AddPol.UseVisualStyleBackColor = true;
            this.AddPol.Click += new System.EventHandler(this.AddPol_Click);
            // 
            // ResizePol
            // 
            this.ResizePol.Location = new System.Drawing.Point(365, 326);
            this.ResizePol.Name = "ResizePol";
            this.ResizePol.Size = new System.Drawing.Size(75, 23);
            this.ResizePol.TabIndex = 50;
            this.ResizePol.Text = "Resize(B)";
            this.ResizePol.UseVisualStyleBackColor = true;
            this.ResizePol.Click += new System.EventHandler(this.ResizePol_Click);
            // 
            // ResetPoly
            // 
            this.ResetPoly.Location = new System.Drawing.Point(278, 326);
            this.ResetPoly.Name = "ResetPoly";
            this.ResetPoly.Size = new System.Drawing.Size(75, 23);
            this.ResetPoly.TabIndex = 49;
            this.ResetPoly.Text = "Reset";
            this.ResetPoly.UseVisualStyleBackColor = true;
            this.ResetPoly.Click += new System.EventHandler(this.ResetPoly_Click);
            // 
            // SavePol
            // 
            this.SavePol.Location = new System.Drawing.Point(33, 326);
            this.SavePol.Name = "SavePol";
            this.SavePol.Size = new System.Drawing.Size(75, 23);
            this.SavePol.TabIndex = 48;
            this.SavePol.Text = "Save";
            this.SavePol.UseVisualStyleBackColor = true;
            this.SavePol.Click += new System.EventHandler(this.SavePol_Click);
            // 
            // VerValue
            // 
            this.VerValue.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.VerValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.VerValue.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.VerValue.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.VerValue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.VerValue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Offset,
            this.Info,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.Par3,
            this.Par2,
            this.Cost,
            this.Cost2});
            this.VerValue.GridColor = System.Drawing.SystemColors.Control;
            this.VerValue.Location = new System.Drawing.Point(6, 48);
            this.VerValue.Name = "VerValue";
            this.VerValue.RowHeadersVisible = false;
            this.VerValue.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.VerValue.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.VerValue.Size = new System.Drawing.Size(462, 272);
            this.VerValue.TabIndex = 47;
            // 
            // Offset
            // 
            this.Offset.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Offset.HeaderText = "Offset";
            this.Offset.Name = "Offset";
            // 
            // Info
            // 
            this.Info.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Info.HeaderText = "Info";
            this.Info.Name = "Info";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "Part";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "X";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // Par3
            // 
            this.Par3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Par3.HeaderText = "Z";
            this.Par3.Name = "Par3";
            // 
            // Par2
            // 
            this.Par2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Par2.HeaderText = "Y";
            this.Par2.Name = "Par2";
            // 
            // Cost
            // 
            this.Cost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Cost.HeaderText = "Cost";
            this.Cost.Name = "Cost";
            this.Cost.Visible = false;
            // 
            // Cost2
            // 
            this.Cost2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Cost2.HeaderText = "Cost2";
            this.Cost2.Name = "Cost2";
            this.Cost2.Visible = false;
            // 
            // Rem_Poly
            // 
            this.Rem_Poly.Enabled = false;
            this.Rem_Poly.Location = new System.Drawing.Point(200, 326);
            this.Rem_Poly.Name = "Rem_Poly";
            this.Rem_Poly.Size = new System.Drawing.Size(72, 23);
            this.Rem_Poly.TabIndex = 59;
            this.Rem_Poly.Text = "Remove(B)";
            this.Rem_Poly.UseVisualStyleBackColor = true;
            this.Rem_Poly.Click += new System.EventHandler(this.Rem_Poly_Click);
            // 
            // MapList
            // 
            this.MapList.Enabled = false;
            this.MapList.FormattingEnabled = true;
            this.MapList.Location = new System.Drawing.Point(13, 27);
            this.MapList.Name = "MapList";
            this.MapList.Size = new System.Drawing.Size(300, 21);
            this.MapList.TabIndex = 50;
            this.MapList.SelectedIndexChanged += new System.EventHandler(this.MapList_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(332, 11);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 13);
            this.label12.TabIndex = 48;
            this.label12.Text = "Tex File";
            this.label12.Visible = false;
            // 
            // Tex_File
            // 
            this.Tex_File.Location = new System.Drawing.Point(335, 27);
            this.Tex_File.Name = "Tex_File";
            this.Tex_File.Size = new System.Drawing.Size(100, 20);
            this.Tex_File.TabIndex = 47;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.OrthoView);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.trackBarTransZ);
            this.groupBox1.Controls.Add(this.lblDistance);
            this.groupBox1.Controls.Add(this.label24);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.trackBarTransY);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.trackBarRotZ);
            this.groupBox1.Controls.Add(this.showSingular);
            this.groupBox1.Controls.Add(this.SaveType);
            this.groupBox1.Controls.Add(this.CheckText);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.trackBarTransX);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.PolyNumMax);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.trackBarRotX);
            this.groupBox1.Controls.Add(this.trackBarRotY);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lblElevation);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lblAngle);
            this.groupBox1.Location = new System.Drawing.Point(501, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(188, 601);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "3d Command";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 363);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(16, 13);
            this.label11.TabIndex = 74;
            this.label11.Text = "In";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(136, 360);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 13);
            this.label2.TabIndex = 71;
            this.label2.Text = "Out";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trackBarTransZ
            // 
            this.trackBarTransZ.BackColor = System.Drawing.SystemColors.Control;
            this.trackBarTransZ.Location = new System.Drawing.Point(13, 375);
            this.trackBarTransZ.Maximum = 600;
            this.trackBarTransZ.Minimum = -600;
            this.trackBarTransZ.Name = "trackBarTransZ";
            this.trackBarTransZ.Size = new System.Drawing.Size(147, 45);
            this.trackBarTransZ.TabIndex = 72;
            this.trackBarTransZ.Value = 135;
            this.trackBarTransZ.Scroll += new System.EventHandler(this.trackBarDist_Scroll);
            // 
            // lblDistance
            // 
            this.lblDistance.AutoSize = true;
            this.lblDistance.Location = new System.Drawing.Point(69, 360);
            this.lblDistance.Name = "lblDistance";
            this.lblDistance.Size = new System.Drawing.Size(61, 13);
            this.lblDistance.TabIndex = 73;
            this.lblDistance.Text = "Translate Z";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(139, 299);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(21, 13);
            this.label24.TabIndex = 70;
            this.label24.Text = "Up";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(14, 299);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(35, 13);
            this.label22.TabIndex = 69;
            this.label22.Text = "Down";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(70, 299);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(61, 13);
            this.label21.TabIndex = 68;
            this.label21.Text = "Translate Y";
            // 
            // trackBarTransY
            // 
            this.trackBarTransY.BackColor = System.Drawing.SystemColors.Control;
            this.trackBarTransY.Location = new System.Drawing.Point(15, 312);
            this.trackBarTransY.Maximum = 300;
            this.trackBarTransY.Minimum = -100;
            this.trackBarTransY.Name = "trackBarTransY";
            this.trackBarTransY.Size = new System.Drawing.Size(147, 45);
            this.trackBarTransY.TabIndex = 67;
            this.trackBarTransY.Value = 3;
            this.trackBarTransY.Scroll += new System.EventHandler(this.trackBarTrY_Scroll);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(17, 161);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(25, 13);
            this.label20.TabIndex = 66;
            this.label20.Text = "Left";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(135, 161);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(32, 13);
            this.label19.TabIndex = 65;
            this.label19.Text = "Right";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(70, 161);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(49, 13);
            this.label18.TabIndex = 64;
            this.label18.Text = "Rotate Z";
            // 
            // trackBarRotZ
            // 
            this.trackBarRotZ.BackColor = System.Drawing.SystemColors.Control;
            this.trackBarRotZ.Location = new System.Drawing.Point(15, 113);
            this.trackBarRotZ.Maximum = 600;
            this.trackBarRotZ.Minimum = -600;
            this.trackBarRotZ.Name = "trackBarRotZ";
            this.trackBarRotZ.Size = new System.Drawing.Size(147, 45);
            this.trackBarRotZ.TabIndex = 63;
            this.trackBarRotZ.Scroll += new System.EventHandler(this.trackBarRotZ_Scroll);
            // 
            // showSingular
            // 
            this.showSingular.AutoSize = true;
            this.showSingular.Checked = true;
            this.showSingular.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showSingular.Location = new System.Drawing.Point(16, 480);
            this.showSingular.Name = "showSingular";
            this.showSingular.Size = new System.Drawing.Size(71, 17);
            this.showSingular.TabIndex = 62;
            this.showSingular.Text = "Show Inc";
            this.showSingular.UseVisualStyleBackColor = true;
            // 
            // SaveType
            // 
            this.SaveType.AutoSize = true;
            this.SaveType.Location = new System.Drawing.Point(17, 503);
            this.SaveType.Name = "SaveType";
            this.SaveType.Size = new System.Drawing.Size(95, 17);
            this.SaveType.TabIndex = 61;
            this.SaveType.Text = "SaveasNsbmd";
            this.SaveType.UseVisualStyleBackColor = true;
            // 
            // CheckText
            // 
            this.CheckText.AutoSize = true;
            this.CheckText.Checked = true;
            this.CheckText.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckText.Location = new System.Drawing.Point(94, 480);
            this.CheckText.Name = "CheckText";
            this.CheckText.Size = new System.Drawing.Size(68, 17);
            this.CheckText.TabIndex = 58;
            this.CheckText.Text = "Textured";
            this.CheckText.UseVisualStyleBackColor = true;
            this.CheckText.CheckedChanged += new System.EventHandler(this.CheckText_CheckedChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(17, 225);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(25, 13);
            this.label15.TabIndex = 57;
            this.label15.Text = "Left";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(137, 225);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(32, 13);
            this.label16.TabIndex = 54;
            this.label16.Text = "Right";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trackBarTransX
            // 
            this.trackBarTransX.BackColor = System.Drawing.SystemColors.Control;
            this.trackBarTransX.Location = new System.Drawing.Point(13, 241);
            this.trackBarTransX.Maximum = 300;
            this.trackBarTransX.Minimum = -100;
            this.trackBarTransX.Name = "trackBarTransX";
            this.trackBarTransX.Size = new System.Drawing.Size(147, 45);
            this.trackBarTransX.TabIndex = 55;
            this.trackBarTransX.Value = 53;
            this.trackBarTransX.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(70, 225);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(61, 13);
            this.label17.TabIndex = 56;
            this.label17.Text = "Translate X";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(13, 429);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 13);
            this.label10.TabIndex = 46;
            this.label10.Text = "PolygonVisible";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PolyNumMax
            // 
            this.PolyNumMax.Location = new System.Drawing.Point(15, 445);
            this.PolyNumMax.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.PolyNumMax.Name = "PolyNumMax";
            this.PolyNumMax.Size = new System.Drawing.Size(120, 20);
            this.PolyNumMax.TabIndex = 45;
            this.PolyNumMax.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.PolyNumMax.ValueChanged += new System.EventHandler(this.PolyNumMax_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(145, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 13);
            this.label5.TabIndex = 38;
            this.label5.Text = "Up";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(144, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 36;
            this.label3.Text = "CW";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackBarRotX
            // 
            this.trackBarRotX.BackColor = System.Drawing.SystemColors.Control;
            this.trackBarRotX.Location = new System.Drawing.Point(13, 48);
            this.trackBarRotX.Maximum = 600;
            this.trackBarRotX.Minimum = -600;
            this.trackBarRotX.Name = "trackBarRotX";
            this.trackBarRotX.Size = new System.Drawing.Size(147, 45);
            this.trackBarRotX.TabIndex = 31;
            this.trackBarRotX.Scroll += new System.EventHandler(this.trackBarAng_Scroll);
            // 
            // trackBarRotY
            // 
            this.trackBarRotY.BackColor = System.Drawing.SystemColors.Control;
            this.trackBarRotY.Location = new System.Drawing.Point(13, 177);
            this.trackBarRotY.Maximum = 0;
            this.trackBarRotY.Minimum = -1200;
            this.trackBarRotY.Name = "trackBarRotY";
            this.trackBarRotY.Size = new System.Drawing.Size(147, 45);
            this.trackBarRotY.TabIndex = 32;
            this.trackBarRotY.Value = -900;
            this.trackBarRotY.Scroll += new System.EventHandler(this.trackBarElev_Scroll);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 39;
            this.label6.Text = "Down";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblElevation
            // 
            this.lblElevation.AutoSize = true;
            this.lblElevation.Location = new System.Drawing.Point(70, 96);
            this.lblElevation.Name = "lblElevation";
            this.lblElevation.Size = new System.Drawing.Size(49, 13);
            this.lblElevation.TabIndex = 33;
            this.lblElevation.Text = "Rotate Y";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 37;
            this.label4.Text = "CCW";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAngle
            // 
            this.lblAngle.AutoSize = true;
            this.lblAngle.Location = new System.Drawing.Point(70, 32);
            this.lblAngle.Name = "lblAngle";
            this.lblAngle.Size = new System.Drawing.Size(49, 13);
            this.lblAngle.TabIndex = 34;
            this.lblAngle.Text = "Rotate X";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(246, 264);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Pol Info";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(513, 648);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "SentryAlphaOmega.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(513, 635);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(111, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Based on PG4Map by";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.Nsbmd);
            this.tabControl1.Controls.Add(this.Movements);
            this.tabControl1.Controls.Add(this.Objects);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(3, 33);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1347, 709);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 20;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(74, 45);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(204, 23);
            this.button3.TabIndex = 0;
            this.button3.Text = "NORTH";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // OrthoView
            // 
            this.OrthoView.AutoSize = true;
            this.OrthoView.Location = new System.Drawing.Point(16, 526);
            this.OrthoView.Name = "OrthoView";
            this.OrthoView.Size = new System.Drawing.Size(130, 17);
            this.OrthoView.TabIndex = 75;
            this.OrthoView.Text = "Orthogonal Rendering";
            this.OrthoView.UseVisualStyleBackColor = true;
            this.OrthoView.CheckedChanged += new System.EventHandler(this.OrthoView_CheckedChanged);
            // 
            // MapEditor
            // 
            this.ClientSize = new System.Drawing.Size(1362, 742);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip);
            this.Name = "MapEditor";
            this.Text = "3D Viewer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabControl4.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabControl6.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.Headers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.matrixHeaderInfo)).EndInit();
            this.Borders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.matrixBorderInfo)).EndInit();
            this.Terrains.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.matrixTerrainInfo)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabControl5.ResumeLayout(false);
            this.tabPage10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FurnInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FurnMapInfo)).EndInit();
            this.People.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PeopleInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PeopleMapInfo)).EndInit();
            this.Warps.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.WarpInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarpsMapInfo)).EndInit();
            this.tabPage11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TriggerInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TriggerMapInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AssMap_T)).EndInit();
            this.Objects.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objShowInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ObjInfo)).EndInit();
            this.Movements.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.Layer_0.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MovInfo)).EndInit();
            this.Layer_1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MovInfoL1)).EndInit();
            this.Layer_2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MovInfoL2)).EndInit();
            this.Layer_3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MovInfoL3)).EndInit();
            this.Nsbmd.ResumeLayout(false);
            this.Nsbmd.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AssTable)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.VerValue)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTransZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTransY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRotZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTransX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PolyNumMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRotX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRotY)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        public event Action renderScene;
        public static Stream file;
        private int hits;
        private string actualTextureName;
        private List<tableRow> actualTable;
        private Main main;
        private Texts textHandler;
        private ClosableMemoryStream mapMatrixStream;
        private int romType;
        private Dictionary<int, string> mapListData;
        private NPRE.Formats.Specific.Pokémon.Maps.MapMatrix mapMatrixViewer;
        private PG4Map.Events eventViewer;
        private int yEventShift;
        private int xEventShift;
        private MapMatrix.MapMat_s MapMat;
        private short matrixValue = -1;
        private short newMatrixValue;
        private int yshiftFromStart;
        private int xshiftFromStart;
        private List<tableRow> headerListData;
        private List<int> terrainList;
        private List<Maps> terrainModelList;
        private Narc matrixNarc;
        private Scripts scriptViewer;

        #region Constructors

        public MapEditor(Main main)
        {
            InitializeComponent();
            OpenGlControl.InitializeContexts();
            renderer = new Renderer(this);
            renderScene += renderer.renderMultipleAction;
            Render();
            reactionNormal(main);
        }

        public MapEditor(Nsbmd model)
        {
            InitializeComponent();
            OpenGlControl.InitializeContexts();
            tabControl1.TabPages.RemoveAt(1);
            tabControl1.TabPages.RemoveAt(1);
            renderer = new Renderer(this);
            actualModel = model;
            reactionModel();
        }


        public MapEditor(Maps actualMap, int Id, Main main)
        {

            InitializeComponent();
            OpenGlControl.InitializeContexts();
            renderer = new Renderer(this);
            this.actualMap = actualMap;
            actualModel = actualMap.actualModel;
            headerId = Id;
            reactionMap(actualMap);
            reactionModel();
            reactionNormal(main);

        }

        private void reactionMap(Maps actualMap)
        {
            Tex_File.Text = MapEditor.textureName;
            
            resetInterfaceMap();
            
            actualMap.associationTable = actualModel.assTableList;
            
            PopObjTable(actualMap);
            PopMovTable(actualMap);

            renderer.modelList.Clear();
        }

        private void resetInterfaceMap()
        {
            MapList.Enabled = false;
            SaveObj.Enabled = true;
            AddObj.Enabled = true;
            DelObj.Enabled = true;
            MapList.Items.Clear();
            AssTable.Rows.Clear();
            MovInfo.Rows.Clear();
            MovInfo.Columns.Clear();
            ObjInfo.Rows.Clear();
            PolyNumMax.Value = -1M;
        }

        private void reactionModel()
        {
            resetInterfaceModel();
            
            PopAssTable(actualModel);

            printInfoMap();
            
            renderer.modelList.Clear();
            Render();

            if (showSingular.Checked)
                renderMultiple();
            else
                renderSingular();

            loadAvailableTextures(actualModel.actualTex);

            showItem();
        }

        private void renderMultiple()
        {
            renderer.modelList.Add(actualModel);
            renderScene += new Action(renderer.renderMultipleAction);
        }

        private void renderSingular()
        {
            renderer.modelList.Add(actualModel);
            renderScene += new Action(renderer.renderSingularAction);
        }

        private void resetInterfaceModel()
        {
            AssTable.Rows.Clear();
            AvaTexList.Items.Clear();
            AvaPalList.Items.Clear();
            Rem_Poly.Enabled = true;
            PolyNumMax.Enabled = true;
            AvaTexList.Enabled = true;
            AvaPalList.Enabled = true;
        }

        private void loadAvailableTextures(Nsbtx nsbtx)
        {
            int idTexture = 0;
            if (actualModel.actualTex == null)
                return;
            foreach (NsbmdModel.MatTexPalStruct texture in actualModel.actualTex.tex0.matTexPalList)
            {
                AvaTexList.Items.Add(idTexture + ": " + texture.texName);
                idTexture++;
            }
            int idPalette = 0;
            foreach (NsbmdModel.MatTexPalStruct texture in actualModel.actualTex.tex0.matTexPalList)
            {
                AvaPalList.Items.Add(idPalette + ": " + texture.palName);
                idPalette++;
            }
        }

        public MapEditor(Narc actualNarc, Main main)
        {

            InitializeComponent();
            OpenGlControl.InitializeContexts();
            this.actualNarc = actualNarc;
            renderer = new Renderer(this);
            headerId = -2;
            reactionNormal(main);
            ReactionNarc();
        }

        private void reactionNormal(Main main)
        {
            this.main = main;
            if (main.actualNds != null)
            {
                initAssTable(main.actualNds.getHeader().getTitle(), main.actualNds.getHeader().getCode());
                Console.AppendText("\nAssociation Table loaded!");
                showMatrixInfo(main.actualMap);
                //showMatrixMap();
                Console.AppendText("\nMatrix loaded!\nStart loading script...");
                showScript();
                Console.AppendText("\nScript and text loaded!\nStart loading events...");
                resetEventTables();
                showEvent();
            }
        }

        private void showMatrixInfo(Maps maps)
        {
            if (actualMap == null)
            {
                terrainModelList = null;
                updateMapMatrixBoxes();
                return;
            }

            getMapMat();

            resetMapMatrixBoxes();

            terrainList = new List<int>();

            if (romType == 3)
                showMatrixInfoBW();
            else
                showMatrixInfoOther();

            updateMapMatrixBoxes();
        }

        private void showMatrixInfoOther()
        {
            long cellValue;
            int header;
            getMapMatCellOther(out cellValue, out header);

            popMatrixBoxes((int)cellValue, header);

            getTerrainModelList();
        }

        private void showMatrixInfoBW()
        {
            long cellValue;
            int header;
            getMapMatCellBW(out cellValue, out header);

            popMatrixBoxes((int)cellValue, header);

            getTerrainModelList();
        }

        private void getMapMatCellOther(out long cellValue, out int header)
        {
            long width = actualMap.matrixCells[0];
            long height = actualMap.matrixCells[0];
            if (actualMap.matrixCells.Count > 1)
                height = actualMap.matrixCells[1];
            cellValue = (height) * MapMat.Width + (width);
            header = MapMat.Headers[(int)cellValue];
        }

        private void getTerrainModelList()
        {
            terrainModelList = new List<Maps>();

            foreach (var terrain in terrainList)
            {
                if (terrain == -1)
                    terrainModelList.Add(null);
                else
                {
                    var mapReader = new BinaryReader(actualNarc.figm.fileData[terrain]);
                    var map = new Maps();
                    map.LoadMap(mapReader, false);
                    terrainModelList.Add(map);
                }
            }
        }

        private void getMapMatCellBW(out long cellValue, out int header)
        {
            long width = actualMap.matrixCells[0];
            long height = actualMap.matrixCells[0];
            if (actualMap.matrixCells.Count > 1)
                height = actualMap.matrixCells[1];
            cellValue = (height) * MapMat.Width + (width);
            header = MapMat.Headers[(int)cellValue];
        }

        private void getMapMat()
        {
            var mapMatrixId = getMapMatrixPosition(romType);
            var mapMatrixStream = getStream(mapMatrixId);
            matrixNarc = new Narc().LoadNarc(new BinaryReader(mapMatrixStream));
            var mapMatrixValue = getMatrixValue(actualMap.matrixName);
            var actualMatrix = matrixNarc.figm.fileData[mapMatrixValue];
            if (romType == 3)
                mapMatrixViewer = new MapMatrix(actualMatrix, 1);
            else
                mapMatrixViewer = new MapMatrix(actualMatrix, 0);
            //mapMatrixViewer.showMatrix(matrixHeaderInfo, matrixBorderInfo, matrixTerrainInfo);
            MapMat = mapMatrixViewer.MapMat;
        }

        private void updateMapMatrixBoxes()
        {
            CPrev.Invalidate();
            NPrev.Invalidate();
            SPrev.Invalidate();
            WPrev.Invalidate();
            EPrev.Invalidate();
            NWPrev.Invalidate();
            NEPrev.Invalidate();
            SWPrev.Invalidate();
            SEPrev.Invalidate();
        }

        private int getMatrixValue(string p)
        {
            int matrixValue = -1;
            var matrixName = new List<string>();
            for (int matrixCounter = 0; matrixCounter < matrixNarc.figm.fileData.Count; matrixCounter++)
            {
                getActualMapMat(matrixCounter);

                if (mapMatrixViewer.MapMat.Name == p)
                {
                    matrixValue = matrixCounter;
                    return matrixValue;
                }
            }
            return matrixValue;
        }

        private void getActualMapMat(int matrixCounter)
        {
            var actualMatrix = matrixNarc.figm.fileData[matrixCounter];

            if (romType == 3)
            {
                mapMatrixViewer = new MapMatrix(actualMatrix, 1);
                mapMatrixViewer.MapMat.Name = "map";
            }
            else
                mapMatrixViewer = new MapMatrix(actualMatrix, 0);
        }

        private void CPrev_Paint(object sender, PaintEventArgs e)
        {
            if (terrainModelList!= null)
                popPanel(terrainModelList[0], CPrev);
        }

        private void NPrev_Paint(object sender, PaintEventArgs e)
        {
            if (terrainModelList != null)
            popPanel(terrainModelList[1], NPrev);
        }

        private void WPrev_Paint(object sender, PaintEventArgs e)
        {
            if (terrainModelList != null)
            popPanel(terrainModelList[3], WPrev);
        }

        private void EPrev_Paint(object sender, PaintEventArgs e)
        {
            if (terrainModelList != null)
            popPanel(terrainModelList[4], EPrev);
        }

        private void SPrev_Paint(object sender, PaintEventArgs e)
        {
            if (terrainModelList != null)
            popPanel(terrainModelList[2], SPrev);
        }

        private void NWPrev_Paint(object sender, PaintEventArgs e)
        {
            if (terrainModelList != null)
            popPanel(terrainModelList[5], NWPrev);
        }

        private void NEPrev_Paint(object sender, PaintEventArgs e)
        {
            if (terrainModelList != null)
            popPanel(terrainModelList[6], NEPrev);
        }

        private void SWPrev_Paint(object sender, PaintEventArgs e)
        {
            if (terrainModelList != null)
            popPanel(terrainModelList[7], SWPrev);
        }

        private void SEPrev_Paint(object sender, PaintEventArgs e)
        {
            if (terrainModelList != null)
            popPanel(terrainModelList[8], SEPrev);
        }


        private void popPanel(Maps actualMap, Panel actualPanel)
        {
            int movCounter = 0;
            var g = actualPanel.CreateGraphics();
            Pen pen;
            if (actualMap == null)
            {
                for (int rowCounter = 0; rowCounter < MAXMOVSIZE; rowCounter++)
                {
                    for (int columnCounter = 0; columnCounter < MAXMOVSIZE; columnCounter++)
                    {
                        pen = new Pen(Color.Black, 1);
                        g.DrawRectangle(pen, 4 * columnCounter, 4 * rowCounter, 4 * columnCounter + 4, 4 * rowCounter + 4);
                        g.FillRectangle(pen.Brush, 4 * columnCounter, 4 * rowCounter, 4 * columnCounter + 4, 4 * rowCounter + 4);
                    }
                }
            }
            else
            {
                if (romType != 3)
                {
                    while (movCounter < actualMap.arrayMovement.Length)
                    {
                        for (int rowCounter = 0; rowCounter < MAXMOVSIZE; rowCounter++)
                        {
                            for (int columnCounter = 0; columnCounter < MAXMOVSIZE; columnCounter++)
                            {
                                if (movCounter < actualMap.arrayMovement.Length)
                                {
                                    Maps.Move_s actualMovement = actualMap.arrayMovement[movCounter];
                                    if (actualMovement.actualFlag == 0x80)
                                    {
                                        pen = new Pen(Color.Red, 1);
                                    }
                                    else if (actualMovement.actualMov == 0)
                                        pen = new Pen(Color.LightGreen, 1);
                                    else if (actualMovement.actualMov == 2)
                                        pen = new Pen(Color.LawnGreen, 1);
                                    else if (actualMovement.actualMov == 3)
                                        pen = new Pen(Color.ForestGreen, 1);
                                    else if (actualMovement.actualMov == 16)
                                        pen = new Pen(Color.CornflowerBlue, 1);
                                    else if (actualMovement.actualMov == 21)
                                        pen = new Pen(Color.CornflowerBlue, 1);
                                    else if (actualMovement.actualMov == 33)
                                        pen = new Pen(Color.Yellow, 1);
                                    else if (actualMovement.actualMov == 0x3b)
                                        pen = new Pen(Color.Red, 1);
                                    else if (actualMovement.actualMov == 0x69)
                                        pen = new Pen(Color.Brown, 1);
                                    else if (actualMovement.actualMov == 0xa9)
                                        pen = new Pen(Color.Snow, 1);
                                    else
                                        pen = new Pen(Color.Green, 1);

                                    g.DrawRectangle(pen, 4 * columnCounter, 4 * rowCounter, 4 * columnCounter + 4, 4 * rowCounter + 4);
                                    g.FillRectangle(pen.Brush, 4 * columnCounter, 4 * rowCounter, 4 * columnCounter + 4, 4 * rowCounter + 4);
                                    movCounter++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    while (movCounter < actualMap.arrayMovementBW.Length)
                    {
                        for (int rowCounter = 0; rowCounter < MAXMOVSIZE; rowCounter++)
                        {
                            for (int columnCounter = 0; columnCounter < MAXMOVSIZE; columnCounter++)
                            {
                                if (movCounter < actualMap.arrayMovementBW.Length)
                                {
                                    Maps.Move_s_bw actualMovement = actualMap.arrayMovementBW[movCounter];
                                    if (actualMovement.par2 == 0x81)
                                    {
                                        pen = new Pen(Color.Red, 1);
                                    }
                                                 
                                    else if (actualMovement.actualMov == 0 || actualMovement.actualMov == 31)
                                        pen = new Pen(Color.LightGreen, 1);
                                    else if (actualMovement.actualMov == 1)
                                        pen = new Pen(Color.LightGray, 1);
                                    else if (actualMovement.actualMov == 3)
                                        pen = new Pen(Color.SandyBrown, 1);
                                          else if (actualMovement.actualMov == 4 || actualMovement.actualMov == 6)
                                        pen = new Pen(Color.ForestGreen,1);
                                    else if (actualMovement.actualMov == 63)
                                        pen = new Pen(Color.CornflowerBlue, 1);
                                    else
                                        pen = new Pen(Color.Green, 1);

                                    g.DrawRectangle(pen, 4 * columnCounter, 4 * rowCounter, 4 * columnCounter + 4, 4 * rowCounter + 4);
                                    g.FillRectangle(pen.Brush, 4 * columnCounter, 4 * rowCounter, 4 * columnCounter + 4, 4 * rowCounter + 4);
                                    movCounter++;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void popMatrixBoxes(int cellValue, int header)
        {
            if (romType != 3)
            {
                if (MapMat.Borders == null && MapMat.Terrain == null)
                {
                    actualHeaderBox.AppendText("NULL");
                    actualBorderBox.AppendText("NULL");
                    actualTerrainBox.AppendText(header.ToString());
                    terrainList.Add(header);

                    northHeaderBox.AppendText("NULL");
                    northBorderBox.AppendText("NULL");
                    var id = cellValue - (int)MapMat.Width;
                    if (id < 0 || id > MapMat.Headers.Count - 1)
                        northTerrainBox.AppendText("-1");
                    else
                        northTerrainBox.AppendText(MapMat.Headers[id].ToString());
                    terrainList.Add(Int16.Parse(northTerrainBox.Text));

                    southHeaderBox.AppendText("NULL");
                    southBorderBox.AppendText("NULL");
                    id = cellValue + (int)MapMat.Width;
                    if (id < 0 || id > MapMat.Headers.Count - 1)
                        southTerrainBox.AppendText("-1");
                    else
                        southTerrainBox.AppendText(MapMat.Headers[id].ToString());
                    terrainList.Add(Int16.Parse(southTerrainBox.Text));

                    westHeaderMap.AppendText("NULL");
                    westBorderBox.AppendText("NULL");
                    id = cellValue - 1;
                    if (id < 0 || id > MapMat.Headers.Count - 1)
                        westTerrainBox.AppendText("-1");
                    else
                        westTerrainBox.AppendText(MapMat.Headers[id].ToString());
                    terrainList.Add(Int16.Parse(westTerrainBox.Text));

                    eastHeaderBox.AppendText("NULL");
                    eastBorderBox.AppendText("NULL");
                    id = cellValue + 1;
                    if (id < 0 || id > MapMat.Headers.Count - 1)
                        eastTerrainBox.AppendText("-1");
                    else
                        eastTerrainBox.AppendText(MapMat.Headers[id].ToString());
                    terrainList.Add(Int16.Parse(eastTerrainBox.Text));

                    northWestHeaderBox.AppendText("NULL");
                    northWestBorderBox.AppendText("NULL");
                    id = cellValue - (int)MapMat.Width - 1;
                    if (id < 0 || id > MapMat.Headers.Count - 1)
                        northWestTerrainBox.AppendText("-1");
                    else
                        northWestTerrainBox.AppendText(MapMat.Headers[id].ToString());
                    terrainList.Add(Int16.Parse(northWestTerrainBox.Text));

                    northEastHeaderBox.AppendText("NULL");
                    northEastBorderBox.AppendText("NULL");
                    id = cellValue - (int)MapMat.Width + 1;
                    if (id < 0 || id > MapMat.Headers.Count - 1)
                        northEastTerrainBox.AppendText("-1");
                    else
                        northEastTerrainBox.AppendText(MapMat.Headers[id].ToString());
                    terrainList.Add(Int16.Parse(northEastTerrainBox.Text));

                    id = cellValue + (int)MapMat.Width - 1;
                    if (id < 0 || id > MapMat.Headers.Count - 1)
                        terrainList.Add(-1);
                    else
                        terrainList.Add(MapMat.Headers[id]);

                    southWestHeaderBox.AppendText("NULL");
                    southWestBorderBox.AppendText("NULL");
                    id = cellValue - (int)MapMat.Width - 1;
                    if (id < 0 || id > MapMat.Headers.Count - 1)
                        southWestTerrainBox.AppendText("-1");
                    else
                        southWestTerrainBox.AppendText(MapMat.Headers[id].ToString());
                    terrainList.Add(Int16.Parse(southWestTerrainBox.Text));

                    southEastHeaderBox.AppendText("NULL");
                    southEastBorderBox.AppendText("NULL");
                    id = cellValue + (int)MapMat.Width + 1;
                    if (id < 0 || id > MapMat.Headers.Count - 1)
                        southEastTerrainBox.AppendText("-1");
                    else
                        southEastTerrainBox.AppendText(MapMat.Headers[id].ToString());
                    terrainList.Add(Int16.Parse(southEastTerrainBox.Text));
                }
                else
                {
                    actualHeaderBox.AppendText(header.ToString());
                    actualBorderBox.AppendText(MapMat.Borders[cellValue].ToString());
                    actualTerrainBox.AppendText(MapMat.Terrain[cellValue].ToString());
                    terrainList.Add(MapMat.Terrain[cellValue]);

                    northHeaderBox.AppendText(MapMat.Headers[cellValue - (int)MapMat.Width].ToString());
                    northBorderBox.AppendText(MapMat.Borders[cellValue - (int)MapMat.Width].ToString());
                    northTerrainBox.AppendText(MapMat.Terrain[cellValue - (int)MapMat.Width].ToString());
                    terrainList.Add(Int16.Parse(northTerrainBox.Text));

                    southHeaderBox.AppendText(MapMat.Headers[cellValue + (int)MapMat.Width].ToString());
                    southBorderBox.AppendText(MapMat.Borders[cellValue + (int)MapMat.Width].ToString());
                    southTerrainBox.AppendText(MapMat.Terrain[cellValue + (int)MapMat.Width].ToString());
                    terrainList.Add(Int16.Parse(southTerrainBox.Text));

                    westHeaderMap.AppendText(MapMat.Headers[cellValue - 1].ToString());
                    westBorderBox.AppendText(MapMat.Borders[cellValue - 1].ToString());
                    westTerrainBox.AppendText(MapMat.Terrain[cellValue - 1].ToString());
                    terrainList.Add(Int16.Parse(westTerrainBox.Text));

                    eastHeaderBox.AppendText(MapMat.Headers[cellValue + 1].ToString());
                    eastBorderBox.AppendText(MapMat.Borders[cellValue + 1].ToString());
                    eastTerrainBox.AppendText(MapMat.Terrain[cellValue + 1].ToString());
                    terrainList.Add(Int16.Parse(eastTerrainBox.Text));

                    northWestHeaderBox.AppendText(MapMat.Headers[cellValue - (int)MapMat.Width - 1].ToString());
                    northWestBorderBox.AppendText(MapMat.Borders[cellValue - (int)MapMat.Width - 1].ToString());
                    northWestTerrainBox.AppendText(MapMat.Terrain[cellValue - (int)MapMat.Width - 1].ToString());
                    terrainList.Add(Int16.Parse(northWestTerrainBox.Text));

                    northEastHeaderBox.AppendText(MapMat.Headers[cellValue - (int)MapMat.Width + 1].ToString());
                    northEastBorderBox.AppendText(MapMat.Borders[cellValue - (int)MapMat.Width + 1].ToString());
                    northEastTerrainBox.AppendText(MapMat.Terrain[cellValue - (int)MapMat.Width + 1].ToString());
                    terrainList.Add(Int16.Parse(northEastTerrainBox.Text));

                    southWestHeaderBox.AppendText(MapMat.Headers[cellValue + (int)MapMat.Width - 1].ToString());
                    southWestBorderBox.AppendText(MapMat.Borders[cellValue + (int)MapMat.Width - 1].ToString());
                    southWestTerrainBox.AppendText(MapMat.Terrain[cellValue + (int)MapMat.Width - 1].ToString());
                    terrainList.Add(Int16.Parse(southWestTerrainBox.Text));

                    southEastHeaderBox.AppendText(MapMat.Headers[cellValue + (int)MapMat.Width + 1].ToString());
                    southEastBorderBox.AppendText(MapMat.Borders[cellValue + (int)MapMat.Width + 1].ToString());
                    southEastTerrainBox.AppendText(MapMat.Terrain[cellValue + (int)MapMat.Width + 1].ToString());
                    terrainList.Add(Int16.Parse(southEastTerrainBox.Text));

                }
            }
            else
            {
                actualHeaderBox.AppendText("NULL");
                actualBorderBox.AppendText(MapMat.Borders[cellValue].ToString());
                actualTerrainBox.AppendText(MapMat.Headers[cellValue].ToString());
                terrainList.Add(MapMat.Headers[cellValue]);

                northHeaderBox.AppendText("NULL");
                northBorderBox.AppendText(MapMat.Borders[cellValue - (int)MapMat.Width].ToString());
                northTerrainBox.AppendText(MapMat.Headers[cellValue - (int)MapMat.Width].ToString());
                terrainList.Add(Int16.Parse(northTerrainBox.Text));

                southHeaderBox.AppendText("NULL");
                southBorderBox.AppendText(MapMat.Borders[cellValue + (int)MapMat.Width].ToString());
                southTerrainBox.AppendText(MapMat.Headers[cellValue + (int)MapMat.Width].ToString());
                terrainList.Add(Int16.Parse(southTerrainBox.Text));

                westHeaderMap.AppendText("NULL");
                westBorderBox.AppendText(MapMat.Borders[cellValue - 1].ToString());
                westTerrainBox.AppendText(MapMat.Headers[cellValue - 1].ToString());
                terrainList.Add(Int16.Parse(westTerrainBox.Text));

                eastHeaderBox.AppendText("NULL");
                eastBorderBox.AppendText(MapMat.Borders[cellValue + 1].ToString());
                eastTerrainBox.AppendText(MapMat.Headers[cellValue + 1].ToString());
                terrainList.Add(Int16.Parse(eastTerrainBox.Text));

                northWestHeaderBox.AppendText("NULL");
                northWestBorderBox.AppendText(MapMat.Borders[cellValue - (int)MapMat.Width - 1].ToString());
                northWestTerrainBox.AppendText(MapMat.Headers[cellValue - (int)MapMat.Width - 1].ToString());
                terrainList.Add(Int16.Parse(northWestTerrainBox.Text));

                northEastHeaderBox.AppendText("NULL");
                northEastBorderBox.AppendText(MapMat.Borders[cellValue - (int)MapMat.Width + 1].ToString());
                northEastTerrainBox.AppendText(MapMat.Headers[cellValue - (int)MapMat.Width + 1].ToString());
                terrainList.Add(Int16.Parse(northEastTerrainBox.Text));

                southWestHeaderBox.AppendText("NULL");
                southWestBorderBox.AppendText(MapMat.Borders[cellValue + (int)MapMat.Width - 1].ToString());
                southWestTerrainBox.AppendText(MapMat.Headers[cellValue + (int)MapMat.Width - 1].ToString());
                terrainList.Add(Int16.Parse(southWestTerrainBox.Text));

                southEastHeaderBox.AppendText("NULL");
                southEastBorderBox.AppendText(MapMat.Borders[cellValue + (int)MapMat.Width + 1].ToString());
                southEastTerrainBox.AppendText(MapMat.Headers[cellValue + (int)MapMat.Width + 1].ToString());
                terrainList.Add(Int16.Parse(southEastTerrainBox.Text));
            }
        }

        private void resetMapMatrixBoxes()
        {
            actualHeaderBox.Clear();
            actualBorderBox.Clear();
            actualTerrainBox.Clear();
            northHeaderBox.Clear();
            northBorderBox.Clear();
            northTerrainBox.Clear();
            southHeaderBox.Clear();
            southBorderBox.Clear();
            southTerrainBox.Clear();
            westHeaderMap.Clear();
            westBorderBox.Clear();
            westTerrainBox.Clear();
            eastHeaderBox.Clear();
            eastBorderBox.Clear();
            eastTerrainBox.Clear();
            southWestHeaderBox.Clear();
            southWestBorderBox.Clear();
            southWestTerrainBox.Clear();
            southEastHeaderBox.Clear();
            southEastBorderBox.Clear();
            southEastTerrainBox.Clear();
            northWestHeaderBox.Clear();
            northWestBorderBox.Clear();
            northWestTerrainBox.Clear();
            northEastHeaderBox.Clear();
            northEastBorderBox.Clear();
            northEastTerrainBox.Clear();
        }

        private void resetEventTables()
        {
            FurnInfo.Rows.Clear();
            FurnMapInfo.Rows.Clear();
            PeopleInfo.Rows.Clear();
            PeopleMapInfo.Rows.Clear();
            WarpInfo.Rows.Clear();
            WarpsMapInfo.Rows.Clear();
            TriggerInfo.Rows.Clear();
            TriggerMapInfo.Rows.Clear();
        }

        private void showEvent()
        {

            if (AssMap_T.Rows[0].Cells[17].Value == null)
                return;
            var eventId = getEventPosition(romType);
            var eventStream = getStream(eventId);
            var eventValue = Int16.Parse(AssMap_T.Rows[0].Cells[11].Value.ToString());
            var reader2 = new BinaryReader(eventStream);
            var actualEvent = new Narc().LoadNarc(reader2).figm.fileData[eventValue];
            eventViewer = new Events(actualEvent);
            var reader = new BinaryReader(actualEvent);

            eventViewer.loadEvent(reader, FurnInfo, PeopleInfo, WarpInfo, TriggerInfo);

            //var tempTerrainArray = AssMap_T.Rows[0].Cells[17].Value.ToString().Split(' ');
            //var terrainList = new List<int>();
            //foreach (var terrain in tempTerrainArray)
            //    if (terrain != "")
            //        terrainList.Add(Byte.Parse(terrain));

            //var terrainModelList = new List<Maps>();

            //foreach (var terrain in terrainList)
            //{
            //    var mapReader = new BinaryReader(actualNarc.figm.fileData[terrain]);
            //    var map = new Maps();
            //    map.LoadMap(mapReader, false);
            //    terrainModelList.Add(map);
            //}

            //for (int i=0; i< terrainModelList.Count; i++){
            //    var map = terrainModelList[i];
                popPeopleMapInfo(eventViewer, actualMap, 0);
                popWarpMapInfo(eventViewer,actualMap, 0);
                popFurnMapInfo(eventViewer,actualMap, 0);
                popTriggerMapInfo(eventViewer, actualMap, 0);
            //}
        }

        private void popPeopleMapInfo(PG4Map.Events eventViewer, Maps map, int location)
        {
            PeopleMapInfo = InitTable(ref PeopleMapInfo, location);
            PeopleMapInfo = PopTableOther(actualMap, false, ref PeopleMapInfo, 0, location);
            if (eventViewer.eventStruct.listPeople == null)
                return;
            int peopleCounter = 0;
            foreach (var actualPeople in eventViewer.eventStruct.listPeople)
            {
                var yshift = 0;
                var xshift = 0;
                getYXShift(ref yshift, ref xshift);
                int yCoord = (actualPeople.y - 32 * yshift);
                int xCoord = (actualPeople.x - 32 * xshift);
                if (xCoord > 0 && xCoord < MAXMOVSIZE && yCoord > 0 && yCoord < MAXMOVSIZE)
                    PeopleMapInfo[xCoord, yCoord].Value = peopleCounter;
                peopleCounter++;
            }
        }

        private void popWarpMapInfo(PG4Map.Events eventViewer, Maps map, int location)
        {
            WarpsMapInfo = InitTable(ref WarpsMapInfo, location);
            WarpsMapInfo = PopTableOther(actualMap, false, ref WarpsMapInfo, 0,location);
            int warpCounter = 0;
            if (eventViewer.eventStruct.listWarps == null)
                return;
            foreach (var actualWarp in eventViewer.eventStruct.listWarps)
            {
                var yshift = 0;
                var xshift = 0;
                getYXShift(ref yshift, ref xshift);
                int yCoord = (actualWarp.y - 32 * (yshift));
                int xCoord = (actualWarp.x - 32 * (xshift));
                if (xCoord > 0 && xCoord < MAXMOVSIZE && yCoord > 0 && yCoord < MAXMOVSIZE)
                    WarpsMapInfo[xCoord, yCoord].Value = warpCounter;
                warpCounter++;
            }
        }

        private void getYXShift(ref int yshift, ref int xshift)
        {
            if (yEventShift == 27 && xEventShift == 3)
            {
                yshift = yEventShift;
                xshift = xEventShift;
            }
            else if (yEventShift == 26 && xEventShift == 4)
            {
                yshift = yEventShift;
                xshift = xEventShift - 1;
            }
            else if (yEventShift == 27 && xEventShift == 4)
            {
                yshift = yEventShift - 1;
                xshift = xEventShift - 1;
            }
            else if (yEventShift == 28 && xEventShift == 4)
            {
                yshift = yEventShift - 2;
                xshift = xEventShift - 2;
            }

            else if (yEventShift == 22 && xEventShift == 7)
            {
                yshift = yEventShift + 1;
                xshift = xEventShift + 1;
            }
            else if (yEventShift == 25 && xEventShift == 11)
            {
                yshift = yEventShift - 5;
                xshift = xEventShift - 6;
            }
        }

        private void popTriggerMapInfo(PG4Map.Events eventViewer, Maps map, int location)
        {
            TriggerMapInfo = InitTable(ref TriggerMapInfo, location);
            TriggerMapInfo = PopTableOther(actualMap, false, ref TriggerMapInfo, 0, location);
            int warpCounter = 0;
            if (eventViewer.eventStruct.listTriggers == null)
                return;
            foreach (var actualWarp in eventViewer.eventStruct.listTriggers)
            {
                var yshift = 0;
                var xshift = 0;
                getYXShift(ref yshift, ref xshift);

                int yCoord = (actualWarp.y - 32 * yshift);
                int xCoord = (actualWarp.x - 32 * xshift);
                if (xCoord > 0 && xCoord < MAXMOVSIZE && yCoord > 0 && yCoord < MAXMOVSIZE)
                {
                    TriggerMapInfo[xCoord, yCoord].Value = warpCounter;
                    if (actualWarp.width>1)
                        for (int i=xCoord;i<xCoord + actualWarp.width;i++)
                            TriggerMapInfo[i, yCoord].Value = warpCounter;
                    if (actualWarp.length > 1)
                        for (int i = yCoord; i < yCoord + actualWarp.length; i++)
                            TriggerMapInfo[xCoord, i].Value = warpCounter;
                }
                warpCounter++;
            }
        }

        private void popFurnMapInfo(PG4Map.Events eventViewer, Maps map, int location)
        {
            FurnMapInfo = InitTable(ref  FurnMapInfo, location);
            FurnMapInfo = PopTableOther(actualMap, false, ref FurnMapInfo, 0, location);
            int warpCounter = 0;
            if (eventViewer.eventStruct.listFurniture == null)
                return;
            foreach (var actualWarp in eventViewer.eventStruct.listFurniture)
            {
                var yshift = 0;
                var xshift = 0;
                getYXShift(ref yshift, ref xshift);
                int yCoord = (actualWarp.Y - 32 * (yshift));
                int xCoord = (actualWarp.X - 32 * (xshift));
                if (xCoord > 0 && xCoord < MAXMOVSIZE && yCoord > 0 && yCoord < MAXMOVSIZE)
                    FurnMapInfo[xCoord, yCoord].Value = warpCounter;
                warpCounter++;
            }
        }

        private short getEventPosition(int type)
        {
            short eventPosition = 0;

            if (type == 0)
                eventPosition = (short)main.Sys.Nodes[0].Nodes[7].Nodes[3].Nodes[0].Tag;
            else if (type == 1)
                eventPosition = (short)main.Sys.Nodes[0].Nodes[8].Nodes[3].Nodes[0].Tag;
            else if (type == 2)
                eventPosition = (short)main.Sys.Nodes[0].Nodes[0].Nodes[0].Nodes[3].Nodes[2].Tag;
            else if (type == 3)
                eventPosition = (short)main.Sys.Nodes[0].Nodes[0].Nodes[0].Nodes[9].Nodes[0].Tag;
            return eventPosition;
        }

        private void showScript()
        {

            if (AssMap_T.Rows[0].Cells[7].Value == null)
                return;
            var scriptValue = Int16.Parse(AssMap_T.Rows[0].Cells[5].Value.ToString());
            var textValue = Int16.Parse(AssMap_T.Rows[0].Cells[7].Value.ToString());
            var scriptId = getScriptPosition(romType);
            var scriptStream = getStream(scriptId);
            var scriptNarc = new Narc();
            scriptNarc.LoadNarc(new BinaryReader(scriptStream));
            var actualScript = (ClosableMemoryStream)scriptNarc.figm.fileData[scriptValue];
            var textHandler = getTextHandler(textValue);

            scriptViewer = new Scripts(actualScript, textHandler, textValue);
            scriptViewer.scriptType = romType;
            scriptViewer.printScripts(scriptViewer.scriptList, scriptBoxViewer,subScripts);
           
        }

        private void showMatrixMap()
        {
            if (AssMap_T.Rows[0].Cells[4].Value == null)
                return;
            var mapMatrixId = getMapMatrixPosition(romType);
            var mapMatrixStream = getStream(mapMatrixId);
            newMatrixValue = Int16.Parse(AssMap_T.Rows[0].Cells[4].Value.ToString());
            if (matrixValue != newMatrixValue)
            {
                var actualMatrix = new Narc().LoadNarc(new BinaryReader(mapMatrixStream)).figm.fileData[newMatrixValue];
                mapMatrixViewer = new MapMatrix(actualMatrix, 0);
                matrixHeaderInfo.Rows.Clear();
                matrixBorderInfo.Rows.Clear();
                matrixTerrainInfo.Rows.Clear();
                MapMat = mapMatrixViewer.MapMat;
                showMatrix(matrixHeaderInfo, matrixBorderInfo, matrixTerrainInfo);
                matrixValue = newMatrixValue;
            }

            for (int i=0; i<matrixTerrainInfo.Rows.Count;i++)
            {
                var actualRow = matrixTerrainInfo.Rows[i];
                for (int j = 0; j < actualRow.Cells.Count; j++)
                {
                    var actualCell = actualRow.Cells[j];
                    if (actualCell.Value.ToString() == headerId.ToString())
                    {
                        yEventShift = 30 - j;
                        xEventShift = 30 - i;
                        if (headerId == 0)
                        {
                            xshiftFromStart = xEventShift;
                            yshiftFromStart = yEventShift;
                        }
                            
                        goto End;
                    }
                }
            }
        End: ;
        }

        public void showMatrix(DataGridView MatrixHeaderInfo, DataGridView MatrixBorderInfo, DataGridView MatrixTerrainInfo)
        {
            initMatrix(MatrixHeaderInfo, MatrixBorderInfo, MatrixTerrainInfo);

            int dataCounter = 0;
            for (int rowCounter = 0; rowCounter < MapMat.Height; rowCounter++)
            {
                for (int columnCounter = 0; columnCounter < MapMat.Width; columnCounter++)
                {
                    MatrixHeaderInfo.Rows[rowCounter].Cells[columnCounter].Value = MapMat.Headers[dataCounter];
                    dataCounter++;
                }
            }
            dataCounter = 0;
            if (MapMat.Borders != null)
            {
                for (int rowCounter = 0; rowCounter < MapMat.Height; rowCounter++)
                {
                    for (int columnCounter = 0; columnCounter < MapMat.Width; columnCounter++)
                    {
                        MatrixBorderInfo.Rows[rowCounter].Cells[columnCounter].Value = MapMat.Borders[dataCounter];
                        dataCounter++;
                    }
                }
            }
            dataCounter = 0;
            if (MapMat.Terrain != null)
            {
                for (int rowCounter = 0; rowCounter < MapMat.Height; rowCounter++)
                {
                    for (int columnCounter = 0; columnCounter < MapMat.Width; columnCounter++)
                    {
                        MatrixTerrainInfo.Rows[rowCounter].Cells[columnCounter].Value = MapMat.Terrain[dataCounter];
                        dataCounter++;
                    }
                }
            }
        }

        private void initMatrix(DataGridView MatrixHeaderInfo, DataGridView MatrixBorderInfo, DataGridView MatrixTerrainInfo)
        {
            for (int dataCounter = 0; dataCounter < Math.Max(MapMat.Width, MapMat.Height); dataCounter++)
            {
                MatrixHeaderInfo.Columns.Add(dataCounter.ToString(), dataCounter.ToString());
                MatrixHeaderInfo.Rows.Add();
                MatrixBorderInfo.Columns.Add(dataCounter.ToString(), dataCounter.ToString());
                MatrixBorderInfo.Rows.Add();
                MatrixTerrainInfo.Columns.Add(dataCounter.ToString(), dataCounter.ToString());
                MatrixTerrainInfo.Rows.Add();
            }


        }

        private string getTerrainData(int matrixValue, int headerId, string mapName)
        {
            string terrainValue = "";
            var mapMatrixId = getMapMatrixPosition(romType);
            var mapMatrixStream = getStream(mapMatrixId);
            var actualMatrix = new Narc().LoadNarc(new BinaryReader(mapMatrixStream)).figm.fileData[matrixValue];
            mapMatrixViewer = new MapMatrix(actualMatrix, 0);
            var MapMat = mapMatrixViewer.MapMat;
            var headersList = MapMat.Headers;
            var cellList = new List<int>();
            for (int headersCounter = 0; headersCounter < headersList.Count; headersCounter++)
            {
                if (headersList[headersCounter] == headerId)
                    cellList.Add(headersCounter);
            }
            if (cellList.Count == 0 && MapMat.Terrain == null && MapMat.Borders == null)
            {
                for (int i = 0; i < MapMat.Headers.Count; i++)
                {
                    int actualTerrainId = MapMat.Headers[i];
                    if (!mapListData.ContainsKey(actualTerrainId))
                        if (i > 0)
                            mapListData.Add(actualTerrainId, mapName.Remove(mapName.Length - 2) + " " + i);
                        else
                            mapListData.Add(actualTerrainId, mapName.Remove(mapName.Length - 2));

                    terrainValue = " " + actualTerrainId.ToString();
                }
            }
            else
            {
                var terrainList = new List<int>();
                for (int cellCounter = 0; cellCounter < cellList.Count; cellCounter++)
                {
                    var actualCell = cellList[cellCounter];
                    terrainList.Add(MapMat.Terrain[actualCell]);
                    if (!mapListData.ContainsKey(MapMat.Terrain[actualCell]))
                        if (cellCounter > 0)
                            mapListData.Add(MapMat.Terrain[actualCell], mapName.Remove(mapName.Length - 2) + " " + cellCounter);

                        else
                            mapListData.Add(MapMat.Terrain[actualCell], mapName.Remove(mapName.Length - 2));
                    terrainValue += " " + MapMat.Terrain[actualCell];
                }
            }
            terrainValue += " ";
            return terrainValue;
        }

        private short getMapMatrixPosition(int type)
        {
            short mapMatrixPosition = 0;

            if (type == 0)
                mapMatrixPosition = (short)main.Sys.Nodes[0].Nodes[7].Nodes[5].Nodes[0].Tag;
            else if (type == 1)
                mapMatrixPosition = (short)main.Sys.Nodes[0].Nodes[8].Nodes[5].Nodes[0].Tag;
            else if (type == 2)
                mapMatrixPosition = (short)main.Sys.Nodes[0].Nodes[0].Nodes[0].Nodes[4].Nodes[1].Tag;
            else if (type == 3)
                mapMatrixPosition = (short)main.Sys.Nodes[0].Nodes[0].Nodes[0].Nodes[0].Nodes[9].Tag;
            return mapMatrixPosition;
        }

        private short getScriptPosition(int type)
        {
            short scriptPosition = 0;

            if (type == 0)
                scriptPosition = (short)main.Sys.Nodes[0].Nodes[7].Nodes[8].Nodes[0].Tag;
            else if (type == 1)
                scriptPosition = (short)main.Sys.Nodes[0].Nodes[8].Nodes[8].Nodes[0].Tag;
            else if (type == 2)
                scriptPosition = (short)main.Sys.Nodes[0].Nodes[0].Nodes[0].Nodes[1].Nodes[2].Tag;
            else if (type == 3)
                scriptPosition = (short)main.Sys.Nodes[0].Nodes[0].Nodes[0].Nodes[5].Nodes[6].Tag;
            return scriptPosition;
        }

        private ClosableMemoryStream getTextStream(string Title)
        {
            ClosableMemoryStream textStream = null;
            if (Title == "POKEMON P\0\0\0" || Title == "POKEMON D\0\0\0")
                textStream = main.actualNds.getFat().getFileStreamAt(Int16.Parse(main.Sys.Nodes[0].Nodes[10].Nodes[1].Tag.ToString()));
            else if (Title == "POKEMON PL\0\0")
                textStream = main.actualNds.getFat().getFileStreamAt(Int16.Parse(main.Sys.Nodes[0].Nodes[12].Nodes[2].Tag.ToString()));
            else if (Title == "POKEMON HG\0\0" || Title == "POKEMON SS\0\0")
                textStream = main.actualNds.getFat().getFileStreamAt(Int16.Parse(main.Sys.Nodes[0].Nodes[0].Nodes[0].Nodes[2].Nodes[7].Tag.ToString()));
            else if (Title == "POKEMON B\0\0\0" || Title == "POKEMON W\0\0\0" || Title == "POKEMON B2\0\0" || Title == "POKEMON W2\0\0")
                textStream = main.actualNds.getFat().getFileStreamAt(Int16.Parse(main.Sys.Nodes[0].Nodes[0].Nodes[0].Nodes[0].Nodes[3].Tag.ToString()));
            return textStream;
        }

        private Formats.Texts getTextHandler(int idMessage)
        {
            var textStream = getTextStream(main.actualNds.getHeader().getTitle());
            var textNarc = new Narc().LoadNarc(new BinaryReader(textStream));
            var textFile = textNarc.figm.fileData[idMessage];
            int textType = 0;
            if (main.actualNds.getHeader().getTitle() == "POKEMON B\0\0\0" || main.actualNds.getHeader().getTitle() == "POKEMON W\0\0\0" ||
                main.actualNds.getHeader().getTitle() == "POKEMON B2\0\0" || main.actualNds.getHeader().getTitle() == "POKEMON W2\0\0")
                textType = 1;
            var textHandler = new Texts(textFile, textType);
            var reader = new BinaryReader(textFile);
            reader.BaseStream.Position = 0;
            if (romType != 4)
                textHandler.readText(reader, textBox);
            else
                textHandler.readTextBW(reader, 0, textBox);
            return textHandler;
        }


        private void ReactionNarc()
        {
            renderScene += renderer.renderMultipleAction;
            Render();
            UpdateType();
            MapList.Enabled = true;
            GenerateMapList();
        }

        private static void UpdateType()
        {
            IsBWDialog checkGame = new IsBWDialog();
            checkGame.ShowDialog();
            modelType = checkGame.CheckGame();
            Maps.type = modelType;
        }
        #endregion

        #region Objects_Section

        private void AddObj_Click(object sender, EventArgs e)
        {
            ObjInfo.Rows.Add();
            if (Maps.type == DPMAP || Maps.type == PLMAP)
            {
                actualMap.listObjects.Add(new Maps.Obj_S());
                actualMap.mapHeader.ObjSize += 0x30;
            }
            else if (Maps.type == HGSSMAP)
            {
                actualMap.listObjects.Add(new Maps.Obj_S());
                actualMap.mapHeaderHG.ObjSize += 0x30;
            }
            else if (Maps.type == BWMAP || Maps.type == BW2MAP)
            {
                actualMap.listObjectsBW.Add(new Maps.Obj_S());
                if (actualMap.listObjectsBW.Count == 1)
                    actualMap.mapHeaderBW.ObjSize += 20;
                else
                    actualMap.mapHeaderBW.ObjSize += 16;
            }

        }

        private void DelObj_Click(object sender, EventArgs e)
        {
            if (ObjInfo.Rows.Count == 0)
                MessageBox.Show("There aren't objetct on map to delete.");
            else
            {
                ObjInfo.Rows.Remove(ObjInfo.CurrentRow);
                if (Maps.type == DPMAP || Maps.type == PLMAP)
                {
                    actualMap.listObjects.RemoveAt(ObjInfo.CurrentRow.Index);
                    actualMap.mapHeader.ObjSize -= 0x30;
                }
                else if (Maps.type == HGSSMAP)
                {
                    actualMap.listObjects.RemoveAt(ObjInfo.CurrentRow.Index);
                    actualMap.mapHeaderHG.ObjSize -= 0x30;
                }
                else
                {
                    actualMap.listObjectsBW.RemoveAt(ObjInfo.CurrentRow.Index);
                    actualMap.mapHeaderBW.ObjSize -= 0x30;
                }
            }
        }

        private void SavObj_Click(object sender, EventArgs e)
        {
            if (Maps.type != BWMAP && Maps.type != BW2MAP)
            {
                actualMap.listObjects.Clear();
                foreach (DataGridViewRow row in ObjInfo.Rows)
                {
                    var obj = new Maps.Obj_S
                    {
                        idObject = Int32.Parse(row.Cells[1].Value.ToString()),
                        ySmall = Int16.Parse(row.Cells[5].Value.ToString()),
                        yBig = Int16.Parse(row.Cells[4].Value.ToString()),
                        zSmall = Int16.Parse(row.Cells[7].Value.ToString()),
                        zBig = Int16.Parse(row.Cells[6].Value.ToString()),
                        xSmall = Int16.Parse(row.Cells[3].Value.ToString()),
                        xBig = Int16.Parse(row.Cells[2].Value.ToString()),
                        heightObject = Int16.Parse(row.Cells[8].Value.ToString()),
                        lengthObject = Int16.Parse(row.Cells[9].Value.ToString()),
                        widthObject = Int16.Parse(row.Cells[10].Value.ToString()),
                    };
                    actualMap.listObjects.Add(obj);

                }
            }
            else
            {
                actualMap.listObjectsBW.Clear();
                foreach (DataGridViewRow row in ObjInfo.Rows)
                {
                    var obj = new Maps.Obj_S
                    {
                        idObject = Int32.Parse(row.Cells[1].Value.ToString()),
                        ySmall = Int16.Parse(row.Cells[5].Value.ToString()),
                        yBig = Int16.Parse(row.Cells[4].Value.ToString()),
                        zSmall = Int16.Parse(row.Cells[7].Value.ToString()),
                        zBig = Int16.Parse(row.Cells[6].Value.ToString()),
                        xSmall = Int16.Parse(row.Cells[3].Value.ToString()),
                        xBig = Int16.Parse(row.Cells[2].Value.ToString()),
                        heightObject = Int16.Parse(row.Cells[8].Value.ToString()),
                        lengthObject = Int16.Parse(row.Cells[9].Value.ToString()),
                        widthObject = Int16.Parse(row.Cells[10].Value.ToString()),
                    };
                    actualMap.listObjectsBW.Add(obj);
                }
            }
        }

        #endregion

        #region Polygon Section

        #endregion

        #region Movements

        private void saveMov_Click(object sender, EventArgs e)
        {
            int moveCounter;
            int rowCounter;
            int columnCounter;
            if (Maps.type == BWMAP || Maps.type == BW2MAP)
            {
                moveCounter = 0;
                for (rowCounter = 0; rowCounter < (MovInfo.Rows.Count - 1); rowCounter++)
                {
                    columnCounter = 0;
                    while (columnCounter < MovInfo.Rows[rowCounter].Cells.Count)
                    {
                        actualMap.arrayMovementBW[moveCounter].actualMov = Convert.ToInt32(MovInfo.Rows[rowCounter].Cells[columnCounter].Value);
                        moveCounter++;
                        columnCounter++;
                    }
                }
            }
            else
            {
                moveCounter = 0;
                for (rowCounter = 0; rowCounter < (MovInfo.Rows.Count - 1); rowCounter++)
                {
                    for (columnCounter = 0; columnCounter < MovInfo.Rows[rowCounter].Cells.Count; columnCounter++)
                    {
                        actualMap.arrayMovement[moveCounter].actualMov = Convert.ToInt32(MovInfo.Rows[rowCounter].Cells[columnCounter].Value);
                        moveCounter++;
                    }
                }
            }
        }


        private void saveMovInfoL1_Click(object sender, EventArgs e)
        {
            int moveCounter;
            int rowCounter;
            int columnCounter;
            if (Maps.type == BWMAP || Maps.type == BW2MAP)
            {
                moveCounter = 0;
                for (rowCounter = 0; rowCounter < (MovInfoL1.Rows.Count - 1); rowCounter++)
                {
                    columnCounter = 0;
                    while (columnCounter < MovInfoL1.Rows[rowCounter].Cells.Count)
                    {
                        actualMap.arrayMovementBW[moveCounter].par = Convert.ToInt32(MovInfoL1.Rows[rowCounter].Cells[columnCounter].Value);
                        moveCounter++;
                        columnCounter++;
                    }
                }
            }
            else
            {
                moveCounter = 0;
                for (rowCounter = 0; rowCounter < (MovInfoL1.Rows.Count - 1); rowCounter++)
                {
                    for (columnCounter = 0; columnCounter < MovInfoL1.Rows[rowCounter].Cells.Count; columnCounter++)
                    {
                        actualMap.arrayMovement[moveCounter].actualFlag = Convert.ToInt32(MovInfoL1.Rows[rowCounter].Cells[columnCounter].Value);
                        moveCounter++;
                    }
                }
            }

        }


        private void saveMovInfoL2_Click(object sender, EventArgs e)
        {
            int moveCounter;
            int rowCounter;
            int columnCounter;
            if (Maps.type == BWMAP || Maps.type == BW2MAP)
            {
                moveCounter = 0;
                for (rowCounter = 0; rowCounter < (MovInfoL2.Rows.Count - 1); rowCounter++)
                {
                    columnCounter = 0;
                    while (columnCounter < MovInfoL2.Rows[rowCounter].Cells.Count)
                    {
                        actualMap.arrayMovementBW[moveCounter].par2 = Convert.ToInt32(MovInfoL2.Rows[rowCounter].Cells[columnCounter].Value);
                        moveCounter++;
                        columnCounter++;
                    }
                }
            }

        }


        private void saveMovInfoL3_Click(object sender, EventArgs e)
        {
            int moveCounter;
            int rowCounter;
            int columnCounter;
            if (Maps.type == BWMAP || Maps.type == BW2MAP)
            {
                moveCounter = 0;
                for (rowCounter = 0; rowCounter < (MovInfoL3.Rows.Count - 1); rowCounter++)
                {
                    columnCounter = 0;
                    while (columnCounter < MovInfoL3.Rows[rowCounter].Cells.Count)
                    {
                        actualMap.arrayMovementBW[moveCounter].actualFlag = Convert.ToInt32(MovInfoL3.Rows[rowCounter].Cells[columnCounter].Value);
                        moveCounter++;
                        columnCounter++;
                    }
                }
            }

        }

        private void Grass_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MovInfo.Rows.Count; i++)
            {
                for (int j = 0; j < MovInfo.Rows[i].Cells.Count; j++)
                {
                    if (MovInfo.Rows[i].Cells[j].Selected)
                    {
                        MovInfo.Rows[i].Cells[j].Value = 2;
                        MovInfo.Rows[i].Cells[j].Style.BackColor = Color.LawnGreen;
                    }
                }
            }
        }

        private void High_Grass_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MovInfo.Rows.Count; i++)
            {
                for (int j = 0; j < MovInfo.Rows[i].Cells.Count; j++)
                {
                    if (MovInfo.Rows[i].Cells[j].Selected)
                    {
                        MovInfo.Rows[i].Cells[j].Value = 3;
                        MovInfo.Rows[i].Cells[j].Style.BackColor = Color.DarkGreen;
                    }
                }
            }
        }

        private void Surf_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MovInfo.Rows.Count; i++)
            {
                for (int j = 0; j < MovInfo.Rows[i].Cells.Count; j++)
                {
                    if (MovInfo.Rows[i].Cells[j].Selected)
                    {
                        MovInfo.Rows[i].Cells[j].Value = 10;
                        MovInfo.Rows[i].Cells[j].Style.BackColor = Color.Blue;
                    }
                }
            }
        }

        #endregion

        #region Various

        private bool inScrollRangeLoop(ref TrackBar aScroll, int aToAdd)
        {
            if (aScroll.Maximum < (aScroll.Value + aToAdd))
            {
                aScroll.Value = aScroll.Minimum + aToAdd;
            }
            else if (aScroll.Minimum > (aScroll.Value + aToAdd))
            {
                aScroll.Value = aScroll.Maximum + aToAdd;
            }
            else
            {
                aScroll.Value += aToAdd;
                return false;
            }
            return true;
        }

        #endregion

        #region Maps

        private void GenerateMapList()
        {
            if (modelType == DPMAP)
                GenerateDPPMapList();
            else if (modelType == HGSSMAP)
                GenerateHGSSMapList();
            else if (modelType == BWMAP)
                GenerateBWMapList();
            else if (modelType == PLMAP)
                GeneratePlMapList();
            else if (modelType == BW2MAP)
                GenerateBW2MapList();
            else
                GenerateGenericList();
        }

        private void GenerateHGSSMapList()
        {
            for (int i = 0; i < 675; i++)
            {
                if (mapListData.ContainsKey(i))
                    MapList.Items.Add(i + " - " + mapListData[i].Split('-')[1]);
                else
                    MapList.Items.Add(i);
            }
            MapList.Text = "HG Maps";
        }

        private void GeneratePlMapList()
        {
            for (int i = 0; i < 665; i++)
            {
                if (mapListData.ContainsKey(i))
                    MapList.Items.Add(i + " - " + mapListData[i].Split('-')[1]);
                else
                    MapList.Items.Add(i);
            }
          MapList.Text = "PL Maps";
        }

        private void GenerateGenericList()
        {
            for (int narcCounter = 0; narcCounter < actualNarc.fatbNum; narcCounter++)
                MapList.Items.Add("Model: " + narcCounter);
        }

        private void GenerateBWMapList()
        {
            MapList.Items.AddRange(new object[] { 
                        "000 -  Nuvema Town Ext", "001 - Route 1 (1)", "002 - Route 1 (2)", "003- Route 1 (3)", "004- Route 1 (4)", "005- Accumula Town Outside", "006- Route 2 (1)", "007- Route 2 (2)", "008- Route 2 (3)", "009- Route 2 (4)", "010- Striaton City Outside (1)", "011- Striaton City Outside (2)", "012- Route 3 (1)", "013- Route 3 (2)", "014- Route 3 (3)", "015- Route 3 (4)", 
                        "016- Route 3 (5)", "017- Nacrene City Outside (1)", "018- Nacrene City Outside (2)", "019- Pinwheel Forest entrance (1)", "020- Pinwheel Forest entrance (2)", "021- Pinwheel Forest entrance (3)", "022- Route 17 (1)", "023- Route 17 (2)", "024- Route 17 (3)", "025- Route 17 (4)", "026- Route 18 (1)", "027- Route 18 (2)", "028- Route 18 (3)", "029- Route 4 (1)", "030- Route 4 (2)", "031- Route 4 (3)", 
                        "032- Route 4 (4)", "033- Route 4 (5)", "034- Route 4 (6)", "035- Route 4 (7)", "036- Route 4 (8)", "037- Route 4 (9)", "038- Nimbasa City Outside (1)", "039- Nimbasa City Outside (2)", "040- Nimbasa City Outside (3)", "041- Nimbasa City Outside (4)", "042- Route 5 (1)", "043- Route 5 (2)", "044- Route 16 (1)", "045- Route 16 (2)", "046- Driftveil City Outside (1)", "047- Driftveil City Outside (2)", 
                        "048- Driftveil City Outside (3)", "049- Driftveil City Outside (4)", "050- Driftveil City Outside (5)", "051- Cold Storage Outside (1)", "052- Route 6 (1)", "053- Route 6 (2)", "054- Route 6 (3)", "055- Route 6 (4)", "056- Mistralton City (B) Outside (1)", "057- Mistralton City (B) Outside (2)", "058- Mistralton City (B) Outside (3)", "059- Mistralton City (B) Outside (4)", "060- Route 7 (1)", "061- Route 7 (2)", "062- Route 7 (3)", "063- Route 7 (4)", 
                        "064- Icirrus City Outside (1)", "065- Icirrus City Outside (2)", "066- Icirrus City Outside (3)", "067- Dragonspiral Tower Outside (1)", "068- Route 8 (1)", "069- Route 8 (2)", "070- Route 9 (1)", "071- Route 9 (2)", "072- Route 11 (1)", "073- Route 11 (2)", "074- Opelucid City (B) Outside (1)", "075- Opelucid City (B) Outside (2)", "076- Opelucid City (B) Outside (3)", "077- Opelucid City (B) Outside (4)", "078- Route 10 (1)", "079- Route 10 (2)", 
                        "080- Route 10 (3)", "081- Route 12 (1)", "082- Route 12 (2)", "083- Lacunosa Town Outside (1)", "084- Giant Chasm Outside", "085- Route 13 (1)", "086- Route 13 (2)", "087- Route 13 (3)", "088- Route 13 (4)", "089- Route 13 (5)", "090- Route 13 (6)", "091- Route 13 (7)", "092- Route 13 (8)", "093- Undella Town Outside (1)", "094- Undella Town Outside (2)", "095- Route 14 (1)", 
                        "096- Route 14 (2)", "097- Route 14 (3)", "098- Route 14 (4)", "099- Route 15 (1)", "100- Route 15 (2)", "101- Liberty Garden Outside", "102- Unity Tower Outside", "103- Plain Field", "104- Plain Trees", "105- Plain Rock", "106- Plain Sand", "107- Unknown Object (Breaking)", "108- Castelia City - Central Plaza", "109- Castelia City - Gym Street (Entrance)", "110- Castelia City - North Street", "111- Castelia City - Castelia Street (Entrance)", 
                        "112- Castelia City - Mode Street (Entrance)", "113- Castelia City - Narrow Street (Entrance)", "114- Liberty Pier", "115- Unity Pier", "116- Prime Pier", "117- Cruise Dock", "118- Sightseeing Pier", "119- Skyarrow Bridge (Forest View)", "120- Pok\x00e9mon League Building", "121- Nimbasa City Outside (5)", "122- Nimbasa City Outside (6)", "123- Unknown Object", "124- Unknown Object", "125- Unknown Object", "126- Unknown Object", "127- Unknown Object", 
                        "128- Unknown Object", "129- Unknown Object", "130- Unknown Object", "131- Sand with Water", "132- Driftveil Drawbridge (1)", "133- Driftveil Drawbridge (2)", "134- Driftveil Drawbridge (3)", "135- Driftveil Drawbridge (4)", "136- Driftveil Drawbridge (5)", "137- Driftveil Drawbridge (6)", "138- Unknown Object with Water (1)", "139- Unknown Object with Water (2)", "140- Sand with Block", "141- Water", "142- Water (2)", "143- Water (3)", 
                        "144- Water (4)", "145- Water with Block", "146- Water (5)", "147- Cold Storage Outside (2)", "148- Entralink (1)", "149- Entralink (2)", "150- Entralink (3)", "151- Entralink (4)", "152- Friend's Entralink (1)", "153- Friend's Entralink (2)", "154- Friend's Entralink (3)", "155- Friend's Entralink (4)", "156- Entree Forest (1)", "157- Anville Town Outside (1)", "158- Anville Town Outside (2)", "159- Anville Town Outside (3)", 
                        "160- Anville Town Outside (4)", "161- Marvelous Bridge (1)", "162- Marvelous Bridge (2)", "163- Marvelous Bridge (3)", "164- Marvelous Bridge (4)", "165- Marvelous Bridge (5)", "166- Marvelous Bridge (6)", "167- Marvelous Bridge (7)", "168- Marvelous Bridge (8)", "169- Marvelous Bridge (9)", "170- Marvelous Bridge (10)", "171- Marvelous Bridge (11)", "172- Marvelous Bridge (12)", "173- Pok\x00e9mon League Gate 1", "174- Pok\x00e9mon League Gate 2", "175- Pok\x00e9mon League Gate 3", 
                        "176- Pok\x00e9mon League Gate 4", "177- Pok\x00e9mon League Gate 5", "178- Pok\x00e9mon League Gate 6", "179- Pok\x00e9mon League Gate 7", "180- Pok\x00e9mon League Gate 8", "181- Pok\x00e9mon League Gate 9", "182- Tubeline Bridge (1)", "183- Tubeline Bridge (2)", "184- Tubeline Bridge (3)", "185- Tubeline Bridge (4)", "186- Tubeline Bridge (5)", "187- Tubeline Bridge (6)", "188- Village Bridge Outside (1)", "189- Village Bridge Outside (2)", "190- Village Bridge Outside (3)", "191- Village Bridge Outside (4)", 
                        "192- Village Bridge Outside (5)", "193- Village Bridge Outside (6)", "194- Icirrus City (Winter) (1)", "195- Icirrus City (Winter) (2)", "196- Icirrus City (Winter) (3)", "197- Dragonspiral Tower Outside (2)", "198- Route 8 (Winter) (1)", "199- Route 8 (Winter) (2)", "200- Opelucid City (W) (1)", "201- Opelucid City (W) (2)", "202- Opelucid City (W) (3)", "203- Opelucid City (W) (4)", "204- Bunch of Trees", "205- Road to Champ's room (1)", "206- Pok\x00e9mon League Selection Room (1)", "207- Road to Champ's room (2)", 
                        "208- Pok\x00e9mon League Selection Room (2)", "209- Road to Champ's room (3)", "210- Road to Champ's room (4)", "211- Road to Champ's room (5)", "212- Road to Champ's room (6)", "213- Road to Champ's room (7)", "214- Road to Champ's room (8)", "215- Mistralton City (W) (1)", "216- Mistralton City (W) (2)", "217- Mistralton City (W) (3)", "218- Mistralton City (W) (4)", "219- N's Castle Outside (1)", "220- Model of Pok\x00e9mon League top", "221- N's Castle Outside (2)", "222- Plain Rock", "223- Castelia Street", 
                        "224- Pokèmon Gym Street", "225- Mode Street", "226- Castelia Street", "227- Narrow Street", "228- White Forest (1)", "229- White Forest (2)", "230- White Forest (3)", "231- White Forest (4)", "232- Black City (1)", "233- Black City (2)", "234- Black City (3)", "235- Black City (4)", "236- Entree Forest (2)", "237- Entree Forest (3)", "238- Entree Forest (4)", "239- Entree Forest (5)", 
                        "240- Entree Forest (6)", "241- Entree Forest (7)", "242- Entree Forest (8)", "243- Entree Forest (9)", "244- Trees with River", "245- Trees and Mountain", "246- Undella Bay (1)", "247- Undella Bay (2)", "248- Route*", "249- Fence, Trees and water*", "250- Green Platform*", "251- Water*", "252- Trees, cliff and water*", "253- River and Mountains*", "254- Valley*", "255- Valley with trees and river*", 
                        "256- Cliff with rivers*", "257- Cliff with rivers*", "258- Brown Platform*", "259- Brown Platform*", "260- Brown Platform with water*", "261- Brown Platform*", "262- Brown Platform*", "263- Cliff by water*", "264- Water with Rock*", "265- Brown Platform*", "266- Brown Platform*", "267- Brown Platform*", "268- Brown Platform*", "269- Brown Platform*", "270- Brown Platform*", "271- Brown Platform*", 
                        "272- Cliff by sand*", "273- Route 1 (Copy?) (1)", "274- Route 1 (Copy?) (2)", "275- Route 1 (Copy?) (3)", "276- Route 1 (Copy?) (4)", "277- Route 1 (Copy?) (5)", "278- Route 1 (Copy?) (6)", "279- Dreamyard (1)", "280- Dreamyard (2)", "281- Dreamyard (3)", "282- Dreamyard (4)", "283- Trees", "284- Pinwheel Forest (1)", "285- Pinwheel Forest (2)", "286- Pinwheel Forest (3)", "287- Pinwheel Forest (4)", 
                        "288- Pinwheel Forest (5)", "289- Pinwheel Forest (6)", "290- Pinwheel Forest (7)", "291- Pinwheel Forest (8)", "292- Pinwheel Forest (9)", "293- Pinwheel Forest (10)", "294- Trees", "295- Relic Castle (1)", "296- Relic Castle (2)", "297- Relic Castle (3)", "298- Relic Castle (4)", "299- Relic Castle (5)", "300- Chargestone Cave (1)", "301- Chargestone Cave (2)", "302- Chargestone Cave (3)", "303- Chargestone Cave (4)", 
                        "304- Chargestone Cave (5)", "305- Chargestone Cave (6)", "306- Chargestone Cave Texture?", "307- Chargestone Cave (7)", "308- Chargestone Cave (8)(Bug)", "309- Chargestone Cave (9)", "310- Chargestone Cave (10)", "311- Chargestone Cave (11)", "312- Relic Castle (6)", "313- Relic Castle (7)", "314- Relic Castle (8)", "315- Relic Castle (9)", "316- Relic Castle (10)", "317- Relic Castle (11)", "318- Relic Castle (12)", "319- Relic Castle (13)", 
                        "320- Relic Castle (14)", "321- Relic Castle (15)", "322- Relic Castle (16)", "323- Relic Castle (17)", "324- Relic Castle (18)", "325- Relic Castle (19)", "326- Relic Castle (20)", "327- Relic Castle (21)", "328- Relic Castle (22)", "329- Cold Storage Inside (1)", "330- Twist Mountain (1)", "331- Twist Mountain (2)", "332- Twist Mountain (3)", "333- Twist Mountain (4)", "334- Twist Mountain (5)", "335- Twist Mountain (6)", 
                        "336- Twist Mountain (7)", "337- Twist Mountain (8)", "338- Twist Mountain (9)", "339- Twist Mountain (10)", "340- Twist Mountain (11)", "341- Twist Mountain (12)", "342- Twist Mountain (13)", "343- Twist Mountain (14)", "344- Twist Mountain (15)", "345- Twist Mountain (16)", "346- Twist Mountain (17)", "347- Desert Resort (1)", "348- Desert Resort (2)", "349- Desert Resort (3)", "350- Desert Resort (4)", "351- Desert Resort (5)", 
                        "352- Desert Resort (6)", "353- Desert Resort (7)", "354- Desert Resort (8)", "355- Desert Resort (9)", "356- Wellspring Cave (1)", "357- Wellspring Cave (2)", "358- Wellspring Cave (3)", "359- Mistralton Cave (1)", "360- Mistralton Cave (2)", "361- Mistralton Cave (3)", "362- Challenger's Cave (1)", "363- Challenger's Cave (2)", "364- Challenger's Cave (3)", "365- Challenger's Cave (4)", "366- Challenger's Cave (5)*", "367- Challenger's Cave (6)*", 
                        "368- Challenger's Cave (7)*", "369- Challenger's Cave (8)*", "370- Challenger's Cave (9)*", "371- Challenger's Cave (10)*", "372- Challenger's Cave (11)*", "373- Challenger's Cave (12)*", "374- Challenger's Cave (13)*", "375- Challenger's Cave (14)*", "376- Challenger's Cave (15)*", "377- Challenger's Cave (16)*", "378- Dragonspiral Tower (Inside) (1)", "379- Giant Chasm (Inside) (1)", "380- Giant Chasm (Inside) (2)", "381- Giant Chasm (Inside) (3)", "382- Giant Chasm (Inside) (4)", "383- Moor of Icirrus (1)", 
                        "384- Moor of Icirrus (2)", "385- Moor of Icirrus (3)", "386- Moor of Icirrus (4)", "387- Celestial Tower (1)", "388- Celestial Tower (2)", "389- Celestial Tower (3)", "390- Celestial Tower (4)", "391- Celestial Tower (5)", "392- Abundant Shrine (1)", "393- Abundant Shrine (2)", "394- Abundant Shrine (3)", "395- Abundant Shrine (4)", "396- Twist Mountain (Winter) (1)", "397- Twist Mountain (Winter) (2)", "398- Twist Mountain (Winter) (3)", "399- Twist Mountain (Winter) (4)", 
                        "400- Victory Road (1)", "401- Victory Road (2)", "402- Victory Road (3)", "403- Victory Road (4)", "404- Victory Road (5)", "405- Victory Road (6)", "406- Victory Road (7)", "407- Dragonspiral Tower (Inside) (2)", "408- Dragonspiral Tower (Inside) (3)", "409- Dragonspiral Tower (Inside) (4)", "410- Dragonspiral Tower (Inside) (5)", "411- Dragonspiral Tower (Inside) (6)", "412- Dragonspiral Tower (Inside) (7)", "413- Dragonspiral Tower (Winter) (1)", "414- Dragonspiral Tower (Winter, Pre-Plasma)", "415- Dragonspiral Tower (Winter) (2)", 
                        "416- Dragonspiral Tower (Winter) (3)", "417- Dragonspiral Tower (Winter) (4)", "418- Dragonspiral Tower (Winter, Post-Plasma)", "419- Dragonspiral Tower (Winter) (5)", "420- Dragonspiral Tower (Winter) (6)", "421- Lostlorn Forest (1)", "422- Lostlorn Forest (2)", "423- Lostlorn Forest (3)", "424- Lostlorn Forest (4)", "425- Abyssal Ruins (Dive) (1) (Bug)", "426- Lostlorn Forest (5)", "427- Abyssal Ruins (Dive) (2)", "428- Abyssal Ruins (Dive) (3)", "429- Abyssal Ruins (Dive) (4)", "430- Blank Map?", "431- Abyssal Ruins Puzzle (1)", 
                        "432- Abyssal Ruins Puzzle (2)", "433- Moor of Icirrus (Winter) (1)", "434- Moor of Icirrus (Winter) (2)", "435- Moor of Icirrus (Winter) (3)", "436- Moor of Icirrus (Winter) (4)", "437- Abyssal Ruins (Dive) (5)", "438- Blank Map?", "439- Abyssal Ruins (Dive) (6)", "440- Blank Map?", "441- Abyssal Ruins (Dive) (7)", "442- Abyssal Ruins (Dive) (8)", "443- Abyssal Ruins (Dive) (9)", "444- Abyssal Ruins (Dive) (10)", "445- Abyssal Ruins (Dive) (11)", "446- Giant Chasm (Kyurem Cave)", "447- Giant Chasm (Puzzle) (1)", 
                        "448- Giant Chasm (Puzzle) (2)", "449- Giant Chasm (Puzzle) (3)", "450- Giant Chasm (Puzzle) (4)", "451- Giant Chasm (Puzzle) (5)", "452- Giant Chasm (Puzzle) (6)", "453- Giant Chasm (Puzzle) (7)", "454- Giant Chasm (Puzzle) (8)", "455- Giant Chasm (Puzzle) (9)", "456- Giant Chasm (Puzzle) (10)", "457- Giant Chasm (Puzzle) (11)", "458- Giant Chasm (Puzzle) (12)", "459- Giant Chasm (Puzzle) (13)", "460- Giant Chasm (Puzzle) (14)", "461- Giant Chasm (Puzzle) (15)", "462- Giant Chasm (Puzzle) (16)", "463- Giant Chasm (Puzzle) (17)", 
                        "464- Giant Chasm (Puzzle) (18)", "465- Blank Map?", "466- Blank Map?", "467- Abyssal Ruins (Dive) (12)", "468- Abyssal Ruins-Lambert (0)", "469- Abyssal Ruins (Dive) (13)", "470- Abyssal Ruins (Dive) (14)", "471- Black Object?", "472- Abyssal Ruins (Dive) (15)", "473- Abyssal Ruins (Dive) (16)", "474- Abyssal Ruins (Dive) (17)", "475- Abyssal Ruins-Lambert (2)", "476- Abyssal Ruins-Lambert (3)", "477- Abyssal Ruins-Lambert (4)", "478- Abyssal Ruins (Dive) (18)", "479- Abyssal Ruins-Lambert (5)", 
                        "480- Abyssal Ruins-Lambert (6)", "481- Abyssal Ruins (Dive) (19)", "482- Abyssal Ruins (Dive) (20)", "483- Abyssal Ruins (Dive) (21)", "484- Abyssal Ruins (Dive) (22)", "485- Abyssal Ruins (Dive) (23)", "486- Abyssal Ruins (Dive) (24)", "487- Dreamyard Underground", "488- Vertical Gate", "489- Unloadable Map", "490- Accumula House (1)", "491- Accumula House (2)", "492- Shop?", "493- Striaton School", "494- Inside House (1)", "495- Nacrene Cafe", 
                        "496- Nacrene House (1)", "497- Inside House (2)", "498- Unloadable Map", "499- Castelia Office (1)", "500- Unloadable Map", "501- Unloadable Map", "502- Unloadable Map", "503- Inside House (3)", "504- Unloadable Map", "505- House Inside (4)", "506- Unloadable Map", "507- Castelia Office (2)", "508- House Inside (5)", "509- House Inside (6)", "510- House Inside (7)", "511- House Inside (8)", 
                        "512- Unloadable Map", "513- Nacrene House (2)", "514- Horizontal Gate", "515- Striaton Gym (1)", "516- Nacrene Museum", "517- Unloadable Map", "518- Nacrene Gym (1)", "519- Castelia Gym (1)", "520- Nimbasa Gym (1)", "521- Musical Hall", "522- House Inside (9)", "523- Driftveil Market", "524- Driftveil Gym (1)", "525- Mistralton Gym", "526- Mistralton Airport", "527- Icirrus Gym (1)", 
                        "528- Trailer", "529- Opelucid Gym (1)", "530- Opelucid House (White) (1)", "531- Opelucid House (Black) (1)", "532- Opelucid House (White) (2)", "533- Unloadable Map", "534- Unloadable Map", "535- Friendly Shop (Mall 9)", "536- Unloadable Map", "537- Unloadable Map", "538- Castelia Gym (2)", "539- Castelia Gym (3)", "540- Blank Map?", "541- Large Sports Dome Entrance", "542- Small Sports Dome Entrance", "543- Tennis Court", 
                        "544- Gear Station", "545- Unloadable Map", "546- Undella Mansion", "547- Football Court (1)", "548- Nimbasa Gym (2)", "549- Nimbasa Gym (3)", "550- Nimbasa Gym (4)", "551- Musical Dressing Room", "552- Unloadable Map", "553- Elite Four Room 1 (1)", "554- Elite Four Room 1 (2)", "555- Elite Four Room 2", "556- Elite Four Room 3", "557- Champ's Room (1)", "558- Champ's Room (2)", "559- N's Castle (1)", 
                        "560- Tubeline Bridge Gate (1)", "561- Marvelous Bridge Gate (1)", "562- Marvelous Bridge Gate (2)", "563- Fennel's Lab", "564- Striaton Gym (2)", "565- Miatralton Gym (2)", "566- Skyarrow Bridge Gate (1)", "567- Skyarrow Bridge Gate (2)", "568- Vertical Gate (2)", "569- Pok\x00e9Transfer Lab", "570- Storage Room (Friendly Shop)", "571- Opelucid City House (White) (3)", "572- Opelucid City House (Black) (2)", "573- N's Castle - Room (Freeze)", "574- N's Castle (2)", "575- Battle Subway- Departure Room", 
                        "576- Battle Subway Waiting Platform (1)", "577- Inside the Subway", "578- Internal Building", "579- Nacrene Dream World Shop", "580- N's Castle (3)", "581- N's Castle (4)", "582- N's Castle (5)", "583- N's Castle (6)", "584- N's Castle (7)", "585- N's Castle (8)", "586- N's Castle (9)", "587- N's Castle (10)", "588- N's Castle (11)", "589- N's Castle (12)", "590- Soccer Staduim (1)", "591- C-Gear Surveror's Building", 
                        "592- Beta Map?", "593- Royal Unova (1)", "594- Royal Unova (2)", "595- Driftveil Gym (2)", "596- Nacrene Gym (Beta?)", "597- Some Lab?", "598- Castelia Cafe", "599- Black City House (1)", "600- Black City Shop", "601- Driftveil Market 2", "602- Driftveil Market 3", "603- Driftveil Market 4", "604- Driftveil Market 5", "605- Unloadable Map", "606- Icirrus Gym (2)", "607- Icirrus Gym (3)", 
                        "608- N's Castle (13)", "609- N's Castle (14)", "610- Royal Unova (3)", "611- Royal Unova (4)", "612- Royal Unova (5)", "613- Battle Subway Waiting Platform (2)", "614- Opelucid Gym (2)", "615- Opelucid Gym (3)", "616- Opelucid Gym (4)", "617- Champ's Room (To N's Castle)", "618- Football Court (2)", "619- Basketball Court", "620- Large Staduim Stands", "621- Small Stadium Stands", "622- Soccer Court (2)", "623- Baseball Court (1)", 
                        "624- Baseball Court (2)", "625- Baseball Court (3)", "626- Baseball Court (4)", "627- Union Tower (1)", "628- Liberty Tower (Inside) (1)", "629- Liberty Tower (Inside) (2)", "630- Nimbasa Battle House", "631- Battle Subway Waiting Platform (3)", "632 - Nuvema Town (Copy?)", "633 - Nuvema Town - Hiro's Camera Messed Up", "634 - Multi Object Map", "635 - Multi Object Map", "636 - Castelia City Main Area (Buildings)", "637- Blank Map?", "638 - Castelia City Main Area (Terrain)", "639 - Skyarrow Bridge - North (Left)", 
                        "640 - Skyarrow Bridge - Center (Left)", "641 - Skyarrow Bridge - South (Left)", "642 - Skyarrow Bridge - North (Right)", "643 - Skyarrow Bridge - Center (Right)", "644 - Skyarrow Bridge - South (Right)", "645 - Victory Road Ext. (West)", "646 - Victory Road Ext. (Center-West)", "647 - Victory Road Ext. (Center-East)", "648 - Victory Road Ext. (East)"
                     });
            MapList.Text = "BW Maps";
        }

        private void GenerateBW2MapList()
        {
            MapList.Items.AddRange(new object[] { 
                        "000 -  Nuvema Town Ext", 
                        "001 - Route 1 (1)", 
                        "002 - Route 1 (2)", 
                        "003 - Route 1 (3)", 
                        "004 - Route 1 (4)", 
                        "005 - Accumula Town Outside", 
                        "006 - Route 2 (1)", 
                        "007 - Route 2 (2)", 
                        "008 - Route 2 (3)", 
                        "009 - Route 2 (4)", 
                        "010 - Striaton City Outside (1)", 
                        "011 - Striaton City Outside (2)", 
                        "012 - Route 3 (1)", 
                        "013 - Route 3 (2)", 
                        "014 - Route 3 (3)", 
                        "015 - Route 3 (4)", 
                        "016 - Route 3 (5)", 
                        "017 - Error", 
                        "018 - Nacrene City Outside (2)", 
                        "019 - Pinwheel Forest entrance (1)", 
                        "020 - Pinwheel Forest entrance (2)", 
                        "021 - Pinwheel Forest entrance (3)", 
                        "022 - Route 17 (1)", 
                        "023 - Route 17 (2)", 
                        "024 - Route 17 (3)", 
                        "025 - Route 17 (4)", 
                        "026 - Route 18 (1)", 
                        "027 - Route 18 (2)", 
                        "028 - Route 18 (3)", 
                        "029 - Route 4 (1)", 
                        "030 - Route 4 (2)", 
                        "031 - Route 4 (3)", 
                        "032 - Route 4 (4)", 
                        "033 - Route 4 (5)", 
                        "034 - Route 4 (6)", 
                        "035 - Route 4 (7)", 
                        "036 - Route 4 (8)", 
                        "037 - Route 4 (9)", 
                        "038 - Nimbasa City Outside (1)", 
                        "039 - Nimbasa City Outside (2)", 
                        "040 - Nimbasa City Outside (3)", 
                        "041 - Nimbasa City Outside (4)", 
                        "042 - Route 5 (1)", 
                        "043 - Route 5 (2)", 
                        "044 - Route 16 (1)", 
                        "045 - Route 16 (2)", 
                        "046 - Driftveil City Outside (1)", 
                        "047 - Error", 
                        "048 - Error", 
                        "049 - Error", 
                        "050 - Error (5)", 
                        "051 - Error", 
                        "052 - Route 6 (1)", 
                        "053 - Route 6 (2)", 
                        "054 - Route 6 (3)", 
                        "055 - Route 6 (4)", 
                        "056 - Mistralton City (B) Outside (1)", 
                        "057 - Mistralton City (B) Outside (2)", 
                        "058 - Mistralton City (B) Outside (3)", 
                        "059 - Mistralton City (B) Outside (4)", 
                        "060 - Route 7 (1)", 
                        "061 - Route 7 (2)", 
                        "062 - Route 7 (3)", 
                        "063 - Route 7 (4)", 
                        "064 - Icirrus City Outside (1)", 
                        "065 - Icirrus City Outside (2)", 
                        "066 - Icirrus City Outside (3)", 
                        "067 - Dragonspiral Tower Outside (1)", 
                        "068 - Route 8 (1)", 
                        "069 - Route 8 (2)", 
                        "070 - Route 9 (1)", 
                        "071 - Route 9 (2)", 
                        "072 - Route 11 (1)", 
                        "073 - Route 11 (2)", 
                        "074 - Opelucid City (B) Outside (1)", 
                        "075 - Opelucid City (B) Outside (2)", 
                        "076 - Opelucid City (B) Outside (3)", 
                        "077 - Opelucid City (B) Outside (4)", 
                        "078 - Route 10 (1)", 
                        "079 - Route 10 (2)", 
                        "080 - Route 10 (3)", 
                        "081 - Route 12 (1)", 
                        "082 - Route 12 (2)", 
                        "083 - Lacunosa Town Outside (1)", 
                        "084 - Giant Chasm Outside", 
                        "085 - Route 13 (1)", 
                        "086 - Route 13 (2)", 
                        "087 - Route 13 (3)", 
                        "088 - Route 13 (4)", 
                        "089 - Route 13 (5)", 
                        "090 - Route 13 (6)", 
                        "091 - Route 13 (7)", 
                        "092 - Route 13 (8)", 
                        "093 - Undella Town Outside (1)", 
                        "094 - Undella Town Outside (2)", 
                        "095 - Route 14 (1)", 
                        "096 - Route 14 (2)", 
                        "097 - Route 14 (3)", 
                        "098 - Route 14 (4)", 
                        "099 - Route 15 (1)", 
                        "100 - Route 15 (2)", 
                        "101 - Liberty Garden Outside", 
                        "102 - Unity Tower Outside", 
                        "103 - Plain Field", 
                        "104 - Plain Trees",
                        "105 - Plain Rock", 
                        "106 - Plain Sand", 
                        "107 - Unknown Object (Breaking)", 
                        "108 - Castelia City  - Central Plaza", 
                        "109 - Castelia City  - Gym Street (Entrance)", 
                        "110 - Castelia City  - North Street", 
                        "111 - Castelia City  - Castelia Street (Entrance)", 
                        "112 - Castelia City  - Mode Street (Entrance)", 
                        "113 - Castelia City  - Narrow Street (Entrance)", 
                        "114 - Liberty Pier", 
                        "115 - Unity Pier", 
                        "116 - Prime Pier", 
                        "117 - Cruise Dock", 
                        "118 - Sightseeing Pier", 
                        "119 - Skyarrow Bridge (Forest View)", 
                        "120 - Pok\x00e9mon League Building", 
                        "121 - Nimbasa City Outside (5)", 
                        "122 - Nimbasa City Outside (6)", 
                        "123 - Unknown Object", 
                        "124 - Unknown Object", 
                        "125 - Unknown Object", 
                        "126 - Unknown Object", 
                        "127 - Unknown Object", 
                        "128 - Unknown Object", 
                        "129 - Unknown Object", 
                        "130 - Unknown Object", 
                        "131 - Sand with Water", 
                        "132 - Driftveil Drawbridge (1)", 
                        "133 - Driftveil Drawbridge (2)", 
                        "134 - Driftveil Drawbridge (3)", 
                        "135 - Driftveil Drawbridge (4)", 
                        "136 - Driftveil Drawbridge (5)", 
                        "137 - Driftveil Drawbridge (6)", 
                        "138 - Unknown Object with Water (1)", 
                        "139 - Unknown Object with Water (2)", 
                        "140 - Sand with Block", 
                        "141 - Water", 
                        "142 - Water (2)", 
                        "143 - Water (3)", 
                        "144 - Water (4)", 
                        "145 - Water with Block", 
                        "146 - Water (5)", 
                        "147 - ???? City (Replace Cold Storage)", 
                        "148 - Entralink (1)", 
                        "149 - Entralink (2)", 
                        "150 - Entralink (3)", 
                        "151 - Entralink (4)", 
                        "152 - Friend's Entralink (1)", 
                        "153 - Friend's Entralink (2)", 
                        "154 - Friend's Entralink (3)", 
                        "155 - Friend's Entralink (4)", 
                        "156 - Entree Forest (1)", 
                        "157 - Anville Town Outside (1)", 
                        "158 - Anville Town Outside (2)", 
                        "159 - Anville Town Outside (3)", 
                        "160 - Anville Town Outside (4)", 
                        "161 - Marvelous Bridge (1)", 
                        "162 - Marvelous Bridge (2)", 
                        "163 - Marvelous Bridge (3)", 
                        "164 - Marvelous Bridge (4)", 
                        "165 - Marvelous Bridge (5)", 
                        "166 - Marvelous Bridge (6)", 
                        "167 - Marvelous Bridge (7)", 
                        "168 - Marvelous Bridge (8)", 
                        "169 - Marvelous Bridge (9)", 
                        "170 - Marvelous Bridge (10)", 
                        "171 - Marvelous Bridge (11)", 
                        "172 - Marvelous Bridge (12)", 
                        "173 - Pok\x00e9mon League Gate 1", 
                        "174 - Pok\x00e9mon League Gate 2", 
                        "175 - Pok\x00e9mon League Gate 3", 
                        "176 - Pok\x00e9mon League Gate 4", 
                        "177 - Pok\x00e9mon League Gate 5", 
                        "178 - Pok\x00e9mon League Gate 6", 
                        "179 - Pok\x00e9mon League Gate 7", 
                        "180 - Pok\x00e9mon League Gate 8", 
                        "181 - Pok\x00e9mon League Gate 9", 
                        "182 - Tubeline Bridge (1)", 
                        "183 - Tubeline Bridge (2)", 
                        "184 - Tubeline Bridge (3)", 
                        "185 - Tubeline Bridge (4)", 
                        "186 - Tubeline Bridge (5)", 
                        "187 - Tubeline Bridge (6)", 
                        "188 - Village Bridge Outside (1)", 
                        "189 - Village Bridge Outside (2)", 
                        "190 - Village Bridge Outside (3)", 
                        "191 - Village Bridge Outside (4)", 
                        "192 - Village Bridge Outside (5)", 
                        "193 - Village Bridge Outside (6)", 
                        "194 - Icirrus City (Winter) (1)", 
                        "195 - Icirrus City (Winter) (2)", 
                        "196 - Icirrus City (Winter) (3)", 
                        "197 - Dragonspiral Tower Outside (2)", 
                        "198 - Route 8 (Winter) (1)", 
                        "199 - Route 8 (Winter) (2)", 
                        "200 - Opelucid City (W) (1)", 
                        "201 - Opelucid City (W) (2)", 
                        "202 - Opelucid City (W) (3)", 
                        "203 - Opelucid City (W) (4)", 
                        "204 - Bunch of Trees", 
                        "205 - Road to Champ's room (1)", 
                        "206 - Pok\x00e9mon League Selection Room (1)", 
                        "207 - Road to Champ's room (2)", 
                        "208 - Pok\x00e9mon League Selection Room (2)", 
                        "209 - Road to Champ's room (3)", 
                        "210 - Road to Champ's room (4)", 
                        "211 - Road to Champ's room (5)", 
                        "212 - Road to Champ's room (6)", 
                        "213 - Road to Champ's room (7)", 
                        "214 - Road to Champ's room (8)", 
                        "215 - Mistralton City (W) (1)", 
                        "216 - Mistralton City (W) (2)", 
                        "217 - Mistralton City (W) (3)", 
                        "218 - Mistralton City (W) (4)", 
                        "219 - N's Castle Outside (1)", 
                        "220 - Model of Pok\x00e9mon League top", 
                        "221 - N's Castle Outside (2)", 
                        "222 - Plain Rock", 
                        "223 - Castelia Street", 
                        "224 - Pokèmon Gym Street", 
                        "225 - Mode Street", 
                        "226 - Castelia Street", 
                        "227 - Narrow Street", 
                        "228 - White Forest (1)", 
                        "229 - White Forest (2)", 
                        "230 - White Forest (3)", 
                        "231 - White Forest (4)", 
                        "232 - Black City (1)", 
                        "233 - Black City (2)", 
                        "234 - Black City (3)", 
                        "235 - Black City (4)", 
                        "236 - Entree Forest (2)", 
                        "237 - Entree Forest (3)", 
                        "238 - Entree Forest (4)", 
                        "239 - Entree Forest (5)", 
                        "240 - Entree Forest (6)", 
                        "241 - Entree Forest (7)", 
                        "242 - Entree Forest (8)", 
                        "243 - Entree Forest (9)", 
                        "244 - Trees with River", 
                        "245 - Trees and Mountain", 
                        "246 - Undella Bay (1)", 
                        "247 - Undella Bay (2)", 
                        "248 - Route*", 
                        "249 - Fence, Trees and water*", 
                        "250 - Green Platform*", 
                        "251 - Water*", 
                        "252 - Trees, cliff and water*", 
                        "253 - River and Mountains*", 
                        "254 - Valley*", 
                        "255 - Valley with trees and river*", 
                        "256 - Cliff with rivers*", 
                        "257 - Cliff with rivers*", 
                        "258 - Brown Platform*", 
                        "259 - Brown Platform*", 
                        "260 - Brown Platform with water*", 
                        "261 - Brown Platform*", 
                        "262 - Brown Platform*", 
                        "263 - Cliff by water*", 
                        "264 - Water with Rock*", 
                        "265 - Brown Platform*", 
                        "266 - Brown Platform*", 
                        "267 - Brown Platform*",
                        "268 - Brown Platform*", 
                        "269 - Brown Platform*", 
                        "270 - Brown Platform*", 
                        "271 - Brown Platform*", 
                        "272 - Cliff by sand*", 
                        "273 - Route 1 (Copy?) (1)", 
                        "274 - Route 1 (Copy?) (2)", 
                        "275 - Route 1 (Copy?) (3)", 
                        "276 - Route 1 (Copy?) (4)", 
                        "277 - Route 1 (Copy?) (5)", 
                        "278 - Route 1 (Copy?) (6)",                         
                        "279 - Aspertia City (South)", 
                        "280 - Aspertia City (North)",
                        "281 - Route 19 (West)", 
                        "282 - Route 19 (East)",
                        "283 - Sangi Town (South)", 
                        "284 - Sangi Town (North)",
                        "285 - Sangi Town - Industrial Complex (East)", 
                        "286 - Route 20 (North-West)", 
                        "287 - Route 20 (North-East)",
                        "288 - Route 20 - Surfing (South-West)", 
                        "289 - Route 20 - Surfing (South-East)",
                        "290 - Route 20 - Autumn (North-West)", 
                        "291 - Route 20 - Autumn (North-East)",
                        "292 - Route 20 - Surfing in Autumn (South-West)", 
                        "293 - Route 20 - Surfing in Autumn (South-East)", 
                        "294 - Virbank City (North-West)", 
                        "295 - Virbank City (North-East)",
                        "296 - Virbank City (South)", 
                        "297 - Yamagi Town (West)", 
                        "298 - Yamagi Town (East)",
                        "299 - Seigaha City (South)", 
                        "300 - Seigaha City (Center)",
                        "301 - Seigaha City (North)", 
                        "302 - Route 22 (East",
                        "303 - Route 22 (West)",
                        "304 - Route 23 (North)",
                        "305 - Route 23 (Center)",
                        "306 - Route 23 (South-East)",
                        "307 - Route 23 (South-West)",
                        "308 - Route 23 (East)",
                        "309 - Route 23 (West)",
                        "310 - Underwater Bridge (North)",
                        "311 - Underwater Bridge (Center-North)",
                        "312 - Underwater Bridge (Center)",
                        "313 - Underwater Bridge (Center-South)",
                        "314 - Underwater Bridge (South)",
                        "315 - Underwater Bridge (Far South)",
                        "316 - Beta Map",
                        "317 - Beta Map",
                        "318 - Beta Map",
                        "319 - Beta Map",
                        "320 - Beta Map",
                        "321 - Beta Map",
                        "322 - Beta Map",
                        "323 - Beta Map",
                        "324 - PokèStar Studios (North-West) ",
                        "325 - Route 4 (Center-West: White 2)",
                        "326 - Route 4 (Center-East: White 2)",
                        "327 - Route 4 (North: White 2)",
                        "328 - Route 4 (South-West: White 2)",
                        "329 - Route 4 (South-East: White 2)",
                        "330 - Route 4 (White 2)",
                        "331 - Route 4 (White 2)",
                        "332 - Castelia City - Underground Exit",
                        "333 - Castelia City - Underground Exit 2",
                        "334 - PokèStar Studios (North-East) ",
                        "335 - PokèStar Studios (South-West) ",
                        "336 - PokèStar Studios (South-East) ",
                        "337 - Error",
                        "338 - Error ",
                        "339 - Trees ",
                        "340 - Trees ",
                        "341 - Trees ",
                        "342 - Water ",
                        "343 - Opelucid City (Freezed: White 2) (1)", 
                        "344 - Opelucid City (Freezed: White 2) (2)", 
                        "345 - Opelucid City (Freezed: White 2) (3)", 
                        "346 - Opelucid City (Freezed: White 2) (4)", 
                        "347 - Crash", 
                        "348 - Crash", 
                        "349 - Opelucid City (Freezed: Black 2) (3)", 
                        "350 - Opelucid City (Freezed: Black 2) (4)",
                        "351 - Tree ",
                        "352 - Opelucid City (Snow: White 2) (1)", 
                        "353 - Opelucid City (Snow: White 2) (2)", 
                        "354 - Opelucid City (Snow: White 2) (3)", 
                        "355 - Opelucid City (Snow: White 2) (4)", 
                        "356 - Opelucid City (Snow: Black 2) (1)", 
                        "357 - Opelucid City (Snow: Black 2) (2)", 
                        "358 - Opelucid City (Snow: Black 2) (3)", 
                        "359 - Opelucid City (Snow: Black 2) (4)", 
                        "360 - Victory Road - External (North-West) ",
                        "361 - Victory Road - External (North-Center) ",
                        "362 - Victory Road - External (North-East) ",
                        "363 - Victory Road - External (South-West) ",
                        "364 - Victory Road - External (South-Center) ",
                        "365 - Victory Road - External (South-East) ",
                        "366 - White Forest (North-West)",
                        "367 - White Forest (North-East)",
                        "368 - White Forest (South-West)",
                        "369 - White Forest (South-East)",
                        "370 - Black City (North-West)",
                        "371 - Black City (North-East)",
                        "372 - Black City (South-West)",
                        "373 - Black City (South-East)",
                        "374 - Nimbasa City (Far South-East) ",
                        "375 - Terrain Map ",
                        "376 - Terrain Map ",
                        "377 - Terrain Map ",
                        "378 - Terrain Map ",
                        "379 - Terrain Map ",
                        "380 - Victory Road - External (South-West)",
                        "381 - Victory Road - External (South-Center) ",
                        "382 - Victory Road - External (South-East)  ",
                        "383 - Terrain Map ",
                        "384 - Terrain Map  ",
                        "385 - Terrain Map",
                        "386 - Terrain Map ",
                        "387 - Terrain Map ",
                        "388 - Terrain Map ",
                        "389 - Terrain Map ",
                        "390 - Terrain Map ",
                        "391 - Terrain Map ",
                        "392 - Terrain Map ",
                        "393 - Terrain Map ",
                        "394 - Terrain Map ",
                        "395 - Terrain Map ",
                        "396 - Terrain Map ",
                        "397 - PokèStar Studios (North-West)",
                        "398 - PokèStar Studios (North-East)",
                        "399 - PokèStar Studios (South-West)",
                        "400 - PokèStar Studios (South-East)",
                        "400 - Route 11 (West:Freezed)",
                        "401 - Route 11 (Center-West:Freezed)",
                        "402 - Route 11 (Center-East:Freezed)",
                        "403 - Route 11 (East:Freezed)",
                        "404 - Castelia City - Prime Pier ",
                        "405 - Error",
                        "406 - Terrain Map",
                        "407 - Terrain Map",
                        "408 - Dreamyard - External (West)",
                        "409 - Dreamyard - External (East)",
                        "410 - Dreamyard - External 2",
                        "411 - Dreamyard - External 3",
                        "412 - Terrain Map",
                        "413 - Pinwheel Forest",
                        "414 - Pinwheel Forest 2",
                        "415 - Pinwheel Forest 3",
                        "416 - Pinwheel Forest 4",
                        "417 - Pinwheel Forest 5",
                        "418 - Pinwheel Forest 6",
                        "419 - Pinwheel Forest 7",
                        "420 - Pinwheel Forest 8",
                        "421 - Pinwheel Forest 9",
                        "422 - Pinwheel Forest 10",
                        "423 - Pinwheel Forest 11",
                        "424 - Relic Castle ",
                        "425 - Relic Castle 2",
                        "426 - Relic Castle 3",
                        "427 - Relic Castle 4",
                        "428 - Relic Castle 5",
                        "429 - Chargestone Cave ",
                        "430 - Chargestone Cave 2 ",
                        "431 - Chargestone Cave 3",
                        "432 - Chargestone Cave 4",
                        "433 - Chargestone Cave 5",
                        "434 - Chargestone Cave 6",
                        "435 - Chargestone Cave 7",
                        "436 - Chargestone Cave 8",
                        "437 - Chargestone Cave 9",
                        "438 - Chargestone Cave 10",
                        "439 - Crash ",
                        "440 - Chargestone Cave 11",
                        "441 - Chargestone Cave 12",
                        "442 - Chargestone Cave 13",
                        "443 - Relic Castle 6",
                        "444 - Relic Castle 7",
                        "445 - Relic Castle 8",
                        "446 - Relic Castle 9",
                        "447 - Relic Castle 10",
                        "448 - Relic Castle 11",
                        "449 - Relic Castle 12",
                        "450 - Relic Castle 13",
                        "451 - Relic Castle 14",
                        "452 - Relic Castle 15",
                        "453 - Relic Castle 16",
                        "454 - Relic Castle 17",
                        "455 - Relic Castle 18",
                        "456 - Relic Castle 19",
                        "457 - Relic Castle 20",
                        "458 - Relic Castle 21",
                        "459 - Relic Castle 21",
                        "460 - Beta Map",
                        "461 - Twist Mountain",
                        "462 - Twist Mountain",
                        "463 - Twist Mountain",
                        "464 - Twist Mountain",
                        "465 - Twist Mountain",
                        "466 - Twist Mountain",
                        "467 - Twist Mountain",
                        "469 - Twist Mountain",
                        "470 - Twist Mountain",
                        "471 - Twist Mountain",
                        "472 - Twist Mountain",
                        "473 - Twist Mountain",
                        "474 - Twist Mountain",
                        "475 - Twist Mountain",
                        "476 - Twist Mountain",
                        "477 - Twist Mountain",
                        "478 - Desert Resort",
                        "479 - Desert Resort",
                        "480 - Desert Resort",
                        "481 - Desert Resort",
                        "482 - Desert Resort",
                        "483 - Desert Resort",
                        "484 - Desert Resort",
                        "485 - Desert Resort",
                        "486 - Desert Resort",
                        "487 - Wellspring Cave",
                        "488 - Wellspring Cave",
                        "489 - Wellspring Cave",
                        "490 - Mistralton Cave",
                        "491 - Mistralton Cave",
                        "492 - Mistralton Cave",
                        "493 - Challenger's Cave",
                        "494 - Challenger's Cave", 
                        "495 - Challenger's Cave", 
                        "496 - Challenger's Cave", 
                        "497 - Challenger's Cave", 
                        "498 - Challenger's Cave", 
                        "499 - Challenger's Cave", 
                        "500 - Challenger's Cave", 
                        "501 - Challenger's Cave", 
                        "502 - Challenger's Cave", 
                        "503 - Challenger's Cave", 
                        "504 - Challenger's Cave", 
                        "505 - Challenger's Cave", 
                        "506 - Challenger's Cave", 
                        "507 - Challenger's Cave", 
                        "508 - Challenger's Cave", 
                        "509 - Dragonspiral Tower - Inside", 
                        "510 - Giant Chasm - Inside", 
                        "511 - Giant Chasm - Inside 2", 
                        "512 - Giant Chasm - Inside 3", 
                        "513 - Giant Chasm - Inside 4", 
                        "514 - Moor of Icirrus", 
                        "515 - Moor of Icirrus 2", 
                        "516 - Moor of Icirrus 3", 
                        "517 - Moor of Icirrus 4", 
                        "518 - Celestial Tower ", 
                        "519 - Celestial Tower 2", 
                        "520 - Celestial Tower 3", 
                        "521 - Celestial Tower 4",
                        "522 - Celestial Tower 5",
                        "523 - Abundant Shrine", 
                        "524 - Abundant Shrine 2", 
                        "525 - Abundant Shrine 3", 
                        "526 - Abundant Shrine 4", 
                        "527 - Twist Mountain (Winter)", 
                        "528 - Twist Mountain 2 (Winter)", 
                        "529 - Twist Mountain 3 (Winter)", 
                        "530 - Twist Mountain 4 (Winter)", 
                        "531 - Victory Road ", 
                        "532 - Victory Road 2", 
                        "533 - Victory Road 3", 
                        "534 - Victory Road 4", 
                        "535 - Victory Road 5", 
                        "536 - Victory Road 6", 
                        "537 - Victory Road 7", 
                        "538 - Dragonspiral Tower (Inside) (2)", 
                        "539 - Dragonspiral Tower (Inside) (3)", 
                        "540 - Dragonspiral Tower (Inside) (4)", 
                        "541 - Dragonspiral Tower (Inside) (5)", 
                        "542 - Dragonspiral Tower (Inside) (6)", 
                        "543 - Dragonspiral Tower (Inside) (7)", 
                        "544 - Dragonspiral Tower (Winter) (1)", 
                        "545 - Dragonspiral Tower (Winter, Pre-Plasma)", 
                        "546 - Dragonspiral Tower (Winter) (2)", 
                        "547 - Dragonspiral Tower (Winter) (3)", 
                        "548 - Dragonspiral Tower (Winter) (4)", 
                        "549 - Dragonspiral Tower (Winter, Post-Plasma)",
                        "550 - Dragonspiral Tower (Winter, Post-Plasma) (2)", 
                        "551 - Dragonspiral Tower (Winter, Post-Plasma) (3)", 
                        "552 - Dragonspiral Tower (Winter, Post-Plasma) (4)",
                        "553 - Giant Chasm (Puzzle) (18)",
                        "554 -  ",
                        "555 -  ",
                        "556 -  ",
                        "557 -  ",
                        "558 -  ",
                        "559 -  ",
                        "560 -  ",
                        "561 -  ",
                        "562 -  ",
                        "563 -  ",
                        "564 -  ",
                        "565 -  ",
                        "566 -  ",
                        "567 -  ",
                        "568 -  ",
                        "569 -  ",
                        "570 -  ",
                        "571 -  ",
                        "572 -  ",
                        "573 -  ",
                        "574 -  ",
                        "575 -  ",
                        "576 -  ",
                        "577 -  ",
                        "578 -  ",
                        "579 -  ",
                        "580 -  ",
                        "581 -  ",
                        "582 -  ",
                        "583 -  ",
                        "584 -  ",
                        "585 -  ",
                        "586 -  ",
                        "587 -  ",
                        "588 -  ",
                        "589 -  ",
                        "590 -  ",
                        "591 -  ",
                        "592 -  ",
                        "593 -  ",
                        "594 -  ",
                        "595 -  ",
                        "596 -  ",
                        "597 -  ",
                        "598 -  ",
                        "599 -  ",
                        "600 -  ",
                        "601 -  ",
                        "602 -  ",
                        "603 -  ",
                        "604 -  ",
                        "605 -  ",
                        "606 -  ",
                        "607 -  ",
                        "608 -  ",
                        "609 -  ",
                        "610 -  ",
                        "611 -  ",
                        "612 -  ",
                        "613 -  ",
                        "614 -  ",
                        "615 -  ",
                        "616 -  ",
                        "617 -  ",
                        "618 -  ",
                        "619 -  ",
                        "620 -  ",
                        "621 -  ",
                        "622 -  ",
                        "623 -  ",
                        "624 -  ",
                        "625 -  ",
                        "626 -  ",
                        "627 -  ",
                        "628 -  ",
                        "629 -  ",
                        "630 -  ",
                        "631 -  ",
                        "632 -  ",
                        "633 -  ",
                        "634 -  ",
                        "635 -  ",
                        "636 -  ",
                        "637 -  ",
                        "638 -  ",
                        "639 -  ",
                        "640 -  ",
                        "641 -  ",
                        "642 -  ",
                        "643 -  ",
                        "644 -  ",
                        "645 -  ",
                        "646 -  ",
                        "647 -  ",
                        "648 -  ",
                        "649 -  ",
                        "650 -  ",
                        "651 -  ",
                        "652 -  ",
                        "653 -  ",
                        "654 -  ",
                        "655 -  ",
                        "656 -  ",
                        "657 -  ",
                        "658 -  ",
                        "659 -  ",
                        "660 -  ",
                        "661 -  ",
                        "662 -  ",
                        "663 -  ",
                        "664 -  ",
                        "665 -  ",
                        "666 -  ",
                        "667 -  ",
                        "668 -  ",
                        "669 -  ",
                        "670 -  ",
                        "671 -  ",
                        "672 -  ",
                        "673 -  ",
                        "674 -  ",
                        "675 -  ",
                        "676 -  ",
                        "677 -  ",
                        "678 -  ",
                        "679 -  ",
                        "680 -  ",
                        "681 -  ",
                        "682 -  ",
                        "683 -  ",
                        "684 -  ",
                        "685 -  ",
                        "686 -  ",
                        "687 -  ",
                        "688 -  ",
                        "689 -  ",
                        "690 -  ",
                        "691 -  ",
                        "692 -  ",
                        "693 -  ",
                        "694 -  ",
                        "695 -  ",
                        "696 -  ",
                        "697 -  ",
                        "698 -  ",
                        "699 -  ",
                        "700 -  ",
                        "701 -  ",
                        "702 -  ",
                        "703 -  ",
                        "704 -  ",
                        "705 -  ",
                        "706 -  ",
                        "707 -  ",
                        "708 -  ",
                        "709 -  ",
                        "710 -  ",
                        "711 -  ",
                        "712 -  ",
                        "713 -  ",
                        "714 -  ",
                        "715 -  ",
                        "716 -  ",
                        "717 -  ",
                        "718 -  ",
                        "719 -  ",
                        "720 -  ",
                        "721 -  ",
                        "722 -  ",
                        "723 -  ",
                        "724 -  ",
                        "725 -  ",
                        "726 -  ",
                        "727 -  ",
                        "728 -  ",
                        "729 -  ",
                        "730 -  ",
                        "731 -  ",
                        "732 -  ",
                        "733 -  ",
                        "734 -  ",
                        "735 -  ",
                        "736 -  ",
                        "737 -  ",
                        "738 -  ",
                        "739 -  ",
                        "740 -  ",
                        "741 -  ",
                        "742 -  ",
                        "743 -  ",
                        "744 -  ",
                        "745 -  ",
                        "746 -  ",
                        "747 -  ",
                        "748 -  ",
                        "749 -  ",
                        "750 -  ",
                        "751 -  ",
                        "752 -  ",
                        "753 -  ",
                        "754 -  ",
                        "755 -  ",
                        "756 -  ",
                        "757 -  ",
                        "758 -  ",
                        "759 -  ",
                        "760 -  ",
                        "761 -  ",
                        "762 -  ",
                        "763 -  ",
                        "764 -  ",
                        "765 -  ",
                        "766 -  ",
                        "767 -  ",
                        "768 -  ",
                        "769 -  ",
                        "770 -  ",
                        "771 -  ",
                        "772 -  ",
                        "773 -  ",
                        "774 -  ",
                        "775 -  ",
                        "776 -  ",
                        "777 -  ",
                        "778 -  ",
                        "779 -  ",
                        "780 -  ",
                        "781 -  ",
                        "782 -  ",
                        "783 -  ",
                        "784 -  ",
                        "785 -  ",
                        "786 -  ",
                        "787 -  ",
                        "788 -  ",
                        "789 -  ",
                        "790 -  ",
                        "791 -  ",
                        "792 -  ",
                        "793 -  ",
                        "794 -  ",
                        "795 -  ",
                        "796 -  ",
                        "797 -  ",
                        "798 -  ",
                        "799 -  ",
                        "800 -  ",
                        "801 -  ",
                        "802 -  ",
                        "803 -  ",
                        "804 -  ",
                        "805 -  ",
                        "806 -  ",
                        "807 -  ",
                        "808 -  ",
                        "809 -  ",
                        "810 -  ",
                        "811 -  ",
                        "812 -  ",
                        "813 -  ",
                        "814 -  ",
                        "815 -  ",
                        "816 -  ",
                        "817 -  ",
                        "818 -  ",
                        "819 -  ",
                        "820 -  ",
                        "821 -  ",
                        "822 -  ",
                        "823 -  ",
                        "824 -  ",
                        "825 -  ",
                        "826 -  ",
                        "827 -  ",
                        "828 -  ",
                        "829 -  ",
                        "830 -  ",
                        "831 -  ",
                        "832 -  ",
                        "833 -  ",
                        "834 -  ",
                        "835 -  ",
                        "836 -  ",
                        "837 -  ",
                        "838 -  ",
                        "839 -  ",
                        "840 -  ",
                        "841 -  ",
                        "842 -  ",
                        "843 -  ",
                        "844 -  ",
                        "845 -  ",
                        "846 -  ",
                        "847 -  ",
                        "848 -  ",
                        "849 -  ",
                        "850 -  ",
                        "851 -  ",
                        "852 -  ",
                        "853 -  ",
                        "854 -  ",
                        "855 -  ",
                        "856 -  ",
                        "857 -  ",
                        "858 -  ",
                        "859 -  ",
                        "860 -  ",
                        "861 -  ",
                        "862 -  ",
                        "863 -  ",
                        "864 -  ",
                        "865 -  ",
                        "866 -  ",
                        "867 -  ",
                        "868 -  ",
                        "869 -  ",
                        "870 -  ",
                        "871 -  ",
                        "872 -  ",
                        "873 -  ",
                        "874 -  ",
                        "875 -  ",
                        "876 -  ",
                        "877 -  ",
                        "878 -  ",
                        "879 -  ",
                        "880 -  ",
                        "881 -  ",
                        "882 -  ",
                        "883 -  ",
                        "884 -  ",
                        "885 -  ",
                        "886 -  ",
                        "887 -  ",
                        "888 -  ",
                        "889 -  ",
                        "890 -  ",
                        "891 -  ",
                        "892 -  ",
                        "893 -  ",
                        "894 -  ",
                        "895 -  ",
                        "896 -  ",
                        "897 -  ",
                        "898 -  ",
                        "899 -  ",
                        "900 -  ",
                        "901 -  ",
                        "902 -  ",
                        "903 -  ",
                        "904 -  ",
                        "905 -  ",
                        "906 -  ",
                        "907 -  ",
                        "908 -  ",
                        "909 -  ",
                        "910 -  ",
                        "911 -  ",
                        "912 -  ",
                        "913 -  ",
                        "914 -  ",
                        "915 -  ",
                        "916 -  ",
                        "917 -  ",
                        "918 -  ",
                        "919 -  ",
                        "920 -  ",
                        "921 -  ",
                        "922 -  ",
                        "923 -  ",
                        "924 -  ",
                        "925 -  ",
                        "926 -  ",
                        "927 -  ",
                        "928 -  ",
                        "929 -  ",
                        "930 -  ",
                        "931 -  ",
                        "932 -  ",
                        "933 -  ",
                        "934 -  ",
                        "935 -  ",
                        "936 -  ",
                        "937 -  ",
                        "938 -  ",
                        "939 -  ",
                        "940 -  ",
                        "941 -  ",
                        "942 -  ",
                        "943 -  ",
                        "944 -  ",
                        "945 -  ",
                        "946 -  ",
                        "947 -  ",
                        "948 -  ",
                        "949 -  ",
                        "950 -  ",
                        "951 -  ",
                        "952 -  ",
                        "953 -  ",
                        "954 -  ",
                        "955 -  ",
                        "956 -  ",
                        "957 -  ",
                        "958 -  ",
                        "959 -  ",
                        "960 -  ",
                        "961 -  ",
                        "962 -  ",
                        "963 -  ",
                        "964 -  ",
                        "965 -  ",
                        "966 -  ",
                        "967 -  ",
                        "968 -  ",
                        "969 -  ",
                        "970 -  ",
                        "971 -  ",
                        "972 -  ",
                        "973 -  ",
                        "974 -  ",
                        "975 -  ",
                        "976 -  ",
                        "977 -  ",
                        "978 -  ",
                        "979 -  ",
                        "980 -  ",
                        "981 -  ",
                        "982 -  ",
                        "983 -  ",
                        "984 -  ",
                        "985 -  ",
                        "986 -  ",
                        "987 -  ",
                        "988 -  ",
                        "989 -  ",
                        "990 -  ",
                        "991 -  ",
                        "992 -  ",
                        "993 -  ",
                        "994 -  ",
                        "995 -  ",
                        "996 -  ",
                        "997 -  ",
                        "998 -  ",
                        "999 -  ",
                        "1000 -  ",
                        "1001 -  ",
                        "1002 -  ",
                        "1003 -  ",
                        "1004 -  ",
                        "1005 -  ",
                        "1006 -  ",
                        "1007 -  ",
                        "1008 -  ",
                        "1009 -  ",
                        "1010 -  ",
                        "1011 -  ",
                        "1012 -  ",
                        "1013 -  ",
                        "1014 -  ",
                        "1015 -  ",
                        "1016 -  ",
                        "1017 -  ",
                        "1018 -  ",
                        "1019 -  ",
                        "1020 -  ",
                        "1021 -  ",
                        "1022 -  ",
                        "1023 -  ",
                        "1024 -  ",
                        "1025 -  ",
                        "1026 -  ",
                        "1027 -  ",
                        "1028 -  ",
                        "1029 -  ",
                        "1030 -  ",
                        "1031 -  ",
                        "1032 -  ",
                        "1033 -  ",
                        "1034 -  ",
                        "1035 -  ",
                        "1036 -  ",
                        "1037 -  ",
                        "1038 -  ",
                        "1039 -  ",
                        "1040 -  ",
                        "1041 -  ",
                        "1042 -  ",
                        "1043 -  ",
                        "1044 -  ",
                        "1045 -  ",
                        "1046 -  ",
                        "1047 -  ",
                        "1048 -  ",
                        "1049 -  ",
                        "1050 -  ",
                        "1051 -  ",
                        "1052 -  ",
                        "1053 -  ",
                        "1054 -  ",
                        "1055 -  ",
                        "1056 -  ",
                        "1057 -  ",
                        "1058 -  ",
                        "1059 -  ",
                        "1060 -  ",
                        "1061 -  ",
                        "1062 -  ",
                        "1063 -  ",
                        "1064 -  ",
            });
            MapList.Text = "BW2 Maps";
        }

        //private void GenerateHGSSMapList()
        //{
        //    MapList.Items.AddRange(new object[] { 
        //                "000 - New Bark Town.", 
        //                "001 - Route 29 (West)", 
        //                "002 - Route 29 (Center)", 
        //                "003 - Route 29 (East)", 
        //                "004 - Cherrygrove City (West)", 
        //                "005 - Cherrygrove City (East)", 
        //                "006 - Route 30 (North)", 
        //                "007 - Route 30 (Center)", 
        //                "008 - Route 30 (South)", 
        //                "009 - Route 31 (West)", 
        //                "010 - Route 31 (East)", 
        //                "011 - Route 27 (West)", 
        //                "012 - Route 27 (Center - West)", 
        //                "013 - Route 27 (Center)", 
        //                "014 - Route 27 (Center - East)", 
        //                "015 - Route 27 (East)", 
        //                "016 - Route 27 (Far East)",
        //                "017 - Route 26 (North)",
        //                "018 - Route 26 (Center - North)",
        //                "019 - Route 26 (Center - South)",
        //                "020 - Route 26 (South)",
        //                "021 - Mauville (Not Visible (To Fix))",
        //                "022 - Mauville (Not Visible (To Fix))",
        //                "023 - Mauville (Not Visible (To Fix))",
        //                "024 - Mauville (Not Visible (To Fix))",
        //                "025 - Route 26 (North)",
        //                "026 - Route 26 (Center - North)",
        //                "027 - Not Visible (To Fix)",
        //                "028 - Route 26 (Center - South)",
        //                "029 - Route 26 (South)",
        //                "030 - Alpha's Ruins (North)", 
        //                "031 - Alpha's Ruins (South)", 
        //                "032 - Route 33", 
        //                "033 - Azalea Town (West)", 
        //                "034 - Azalea Town (East)", 
        //                "035 - Route 34 (North)", 
        //                "036 - Route 34 (Center)",
        //                "037 - Route 34 (South)",
        //                "038 - Goldenrod City (North-West)",
        //                "039 - Goldenrod City (North-East)",
        //                "040 - Goldenrod City (South-West)",
        //                "041 - Goldenrod City (South-East)",
        //                "042 - Goldenrod City - GTS (North)",
        //                "043 - Goldenrod City - GTS (South)",
        //                "044 - Route 35 (North)",
        //                "045 - Route 35 (South)",
        //                "046 - Route 36 (West)",
        //                "047 - Route 36 (Center)",
        //                "048 - Route 36 (East)",
        //                "049 - Route 37 ",
        //                "050 - Ecruteak City (North-West)",
        //                "051 - Ecruteak City (North-East)",
        //                "052 - Ecruteak City (South-West)",
        //                "053 - Ecruteak City (South-East)",
        //                "054 - Route 38 (West)",
        //                "055 - Route 38 (East)",
        //                "056 - Route 39 (North)",
        //                "057 - Route 39 (South)",
        //                "058 - Olivine City (North-West)",
        //                "059 - Olivine City (North-East)",
        //                "060 - Olivine City (South-West)",
        //                "061 - Olivine City (South-East)",
        //                "062 - Route 40 (North)",
        //                "063 - Route 40 (South)",
        //                "064 - Route 41 (North-West)",
        //                "065 - Route 41 (North-East)",
        //                "066 - Route 41 (South-West)",
        //                "067 - Route 41 (South-East)",
        //                "068 - Cianwood City (North)",
        //                "069 - Cianwood City (South)",
        //                "070 - Void Green Map",
        //                "071 - Void Green Map 2",
        //                "072 - Saphari Zone Gate (West)",
        //                "073 - Saphari Zone Gate (East)",
        //                "074 - Route 47-48 (North-West)",
        //                "075 - Route 47-48 (North-East)",
        //                "076 - Route 47-48 (Center-West)",
        //                "077 - Route 47-48 (Center)",
        //                "078 - Route 47-48 (Center-East)",
        //                "079 - Route 47-48 (South-West)",
        //                "080 - Route 47-48 (South-Center)",
        //                "081 - Route 47-48 (South-East)",
        //                "082 - Route 42 (West)",
        //                "083 - Route 42 (Center)",
        //                "084 - Not Visible (To Fix)",
        //                "085 - Not Visible (To Fix)",
        //                "086 - Not Visible (To Fix)",
        //                "087 - Not Visible (To Fix)",
        //                "088 - Not Visible (To Fix)",
        //                "089 - Lake of Rage (North-West)",
        //                "090 - Lake of Rage (North-Center)",
        //                "091 - Lake of Rage (North-East)",
        //                "092 - Lake of Rage (South-West)",
        //                "093 - Lake of Rage (South-Center)",
        //                "094 - Lake of Rage (South-East)",
        //                "095 - Lake of Rage (North-West : After TR)",
        //                "096 - Lake of Rage (North-Center : After TR)",
        //                "097 - Lake of Rage (North-East : After TR)",
        //                "098 - Lake of Rage (South-West : After TR)",
        //                "099 - Lake of Rage (South-Center : After TR)",
        //                "100 - Lake of Rage (South-East) : After TR",
        //                "101 - Route 43 (West)",
        //                "102 - Route 43 (Center)",
        //                "103 - Route 43 (East)",
        //                "104 - Blacktorn City (North-West)",
        //                "105 - Blacktorn City  (North-East)",
        //                "106 - Blacktorn City (South-West)",
        //                "107 - Blacktorn City (South-East)",
        //                "108 - Route 45 (North)",
        //                "109 - Route 45 (Center - North)",
        //                "110 - Route 45 (Center)",
        //                "111 - Route 45 (Center - South)",
        //                "112 - Route 45 (South)",
        //                "113 - Route 46 (North)",
        //                "114 - Route 46 (South)",
        //                "115 - Victory Road - Outside",
        //                "116 - Route 2",
        //                "117 - Viridian City (North-West)",
        //                "118 - Viridian City (North-East)",
        //                "119 - Viridian City (South-West)",
        //                "120 - Viridian City (South-East)",
        //                "121 - Route 22 (West)",
        //                "122 - Route 22 (East)",
        //                "123 - Route 28 (West)",
        //                "124 - Route 28 (East)",
        //                "125 - Not Visible (To Fix)",
        //                "126 - Vermilion City (North-West)",
        //                "127 - Viridian City (North-East)",
        //                "128 - Viridian City (South-West)",
        //                "129 - Viridian City (South-East)",
        //                "130 - Route 6",
        //                "131 - Route 11 (West)",
        //                "132 - Route 11 (East)",                                    
        //                "133 - Saffron City (North-West)",
        //                "134 - Saffron City (North-East)",
        //                "135 - Saffron City (South-West)",
        //                "136 - Saffrom City (South-East)",
        //                "137 - Route 9 (West)",
        //                "138 - Route 9 (Center-West)",
        //                "139 - Route 9 (East)",
        //                "140 - Route 9 (South-East)",
        //                "141 - Azuria City (North-West)",
        //                "142 - Azuria City (North-East)",
        //                "143 - Azuria City (South-West)",
        //                "144 - Azuria City (South-East)",
        //                "145 - Route 24",
        //                "146 - Route 25 (West)",
        //                "147 - Route 25 (Center-West)",
        //                "148 - Route 25 (Center-East)",
        //                "149 - Route 25 (East)",
        //                "150 - Not Visible (To Fix)",
        //                "151 - Route 4 (Center)",
        //                "152 - Route 4 (East)",
        //                "153 - Route 5 ",
        //                "154 - Route 8 (West)",
        //                "155 - Route 8 (East)",
        //                "156 - Lavander Town",
        //                "157 - Route 10",
        //                "158 - Route 12 (North)", 
        //                "159 - Route 12 (Center)",
        //                "160 - Route 12 (South)",
        //                "161 - Route 13 (West)",
        //                "162 - Route 13 (Center)",
        //                "163 - Route 13 (East)",
        //                "164 - Route 14 (North) ",
        //                "165 - Route 15 (West)",
        //                "166 - Route 15 (Center)",
        //                "167 - Route 15 (East)",
        //                "168 - Route 14 (South) ",
        //                "169 - Route 7 ",
        //                "170 - Celadon City (North-West)",
        //                "171 - Celadon City (North-East)",
        //                "172 - Celadon City (South-West)",
        //                "173 - Celadon City (South-East)",                     
        //                "174 - Route 6 (East)", 
        //                "175 - Not Visible (To Fix)", 
        //                "176 - Route 17 (North)",
        //                "177 - Route 17 (Center - North)",
        //                "178 - Route 17 (Center - South)",
        //                "179 - Route 17 (South)",
        //                "180 - Route 18 (West)",
        //                "181 - Route 18 (East)",
        //                "182 - Fuchsia City (North-West)",
        //                "183 - Fuchsia City (North-East)",
        //                "184 - Fuchsia City (South-West)",
        //                "185 - Fuchsia City (South-East)",
        //                "186 - Route 19 (North)",
        //                "187 - Route 19 (South)",
        //                "188 - Route 20 (West)",
        //                "189 - Route 20 (East)",
        //                "190 - Route 2 (North)",
        //                "191 - Route 2 (South)",
        //                "192 - Pewter City (North-West)",
        //                "193 - Pewter City (North-East)",
        //                "194 - Pewter City (South-West)",
        //                "195 - Pewter City (South-East)",
        //                "196 - Route 3 (West)",
        //                "197 - Not Visible (To Fix)",
        //                "198 - Route 3 (East)",
        //                "199 - Route 1 (North)",
        //                "200 - Route 1 (South)",
        //                "201 - Pallet Town",
        //                "202 - Route 21 (North)",
        //                "203 - Route 21 (Center",
        //                "204 - Route 21 (South)",
        //                "205 - Cinnabar Island",
        //                "206 - Route 22 (West)",
        //                "207 - Route 22 (East)",
        //                "208 - Tree Block",
        //                "209 - Sea Block",
        //                "210 - Rock Block",
        //                "211 - Tree Block",
        //                "212 - Generic House",
        //                "213 - Generic House",
        //                "214 - Generic House",
        //                "215 - Generic House",
        //                "216 - Hiro's House (1st Floor)",
        //                "217 - Hiro's House (2st Floor)",
        //                "218 - Beta Map",
        //                "219 - Beta Map",
        //                "220 - Pokémon Market",
        //                "221 - Pokémon Center",
        //                "222 - Pokémon Center - Union Room",
        //                "223 - Pokémon Center - Colosseum (Single Battle)",
        //                "224 - Pokémon Center - Colosseum (Double Battle)",
        //                "225 - Pokémon DayCare",
        //                "226 - Bike Shop",
        //                "227 - Game Corner",
        //                "228 - Kimono Teather",
        //                "229 - Barrier Station (First Floor)",
        //                "230 - Barrier Station (Underground Floor)",
        //                "231 - Beta Map",
        //                "232 - Potion Shop",
        //                "233 - Ship Port House",
        //                "234 - Cianwood City - Boutique",
        //                "235 - Mohagany Town - Suovenir Shop",
        //                "236 - Dragon's Den - Sages'Temple",
        //                "237 - Pokémon League - Reception Gate",
        //                "238 - Lab's House",
        //                "239 - Bill's House?",
        //                "240 - Beta Map",
        //                "241 - Not Visible (To Fix)",
        //                "242 - Gate (North-South)",
        //                "243 - Gate (West-East)",
        //                "244 - Generic House",
        //                "245 - Pokémon Center - Wi-fi Room",
        //                "246 - Violet City - Gym",
        //                "247 - Generic House",
        //                "248 - Azalea Town - Gym",
        //                "249 - Beta Map",
        //                "250 - Goldenrod City - Gym",
        //                "251 - Ecruteak City - Gym (North)",
        //                "252 - Ecruteak City - Gym (South)",
        //                "253 - Olivine City - Gym",
        //                "254 - Cianwood City - Gym",
        //                "255 - Mahogany Town - Gym (Entrance)",
        //                "256 - Blacktorn City - Gym (North)",
        //                "257 - Elm's Lab (Second Floor)",
        //                "258 - Trainer School",
        //                "259 - Generic House",
        //                "260 - Blacktorn City - Gym (Center)",
        //                "261 - Blacktorn City - Gym (South)",
        //                "262 - Flower Shop",
        //                "263 - Train Station (First Floor)",
        //                "264 - Train Station (Second Floor)",
        //                "265 - Department Store (First Floor)",
        //                "266 - Department Store (Second Floor)",
        //                "267 - Department Store (Third Floor)",
        //                "268 - Department Store (Forth Floor)",
        //                "269 - Department Store (Fifth Floor)",
        //                "270 - Department Store (Sixth Floor)",
        //                "271 - Generic Building (First Floor)",
        //                "272 - Generic Building (Second Floor)",
        //                "273 - Generic Building (Third Floor)",
        //                "274 - Generic Building (Forth Floor)",
        //                "275 - Generic Building (Fifth Floor)",
        //                "276 - Generic Building (Sixth Floor)",
        //                "277 - Charcoal's Men House",
        //                "278 - Generic House",
        //                "279 - ", "280 - ", "281 - ", "282 - ", "283 - ", "284 - ", "285 - ", "286 - ", "287 - ", 
        //                "288 - ", "289 - ", "290 - ", "291 - ", "292 - ", "293 - ", "294 - ", "295 - ", "296 - ", "297 - ", "298 - ", "299 - ", "300 - Tree Block", "301 - Tree Block", "302 - ", "303 - ", 
        //                "304 - ", "305 - ", "306 - ", "307 - ", "308 - ", "309 - ", "310 - ", "311 - ", "312 - ", "313 - ", "314 - ", "315 - ", "316 - ", "317 - ", "318 - ", "319 - ", 
        //                "320 - ", "321 - ", "322 - ", "323 - ", "324 - ", "325 - ", "326 - ", "327 - ", "328 - ", "329 - ", "330 - ", "331 - ", "332 - ", "333 - ", "334 - ", "335 - ", 
        //                "336 - ", "337 - ", "338 - ", "339 - ", "340 - ", "341 - ", "342 - ", "343 - ", "344 - ", "345 - ", "346 - ", "347 - ", "348 - ", "349 - ", "350 - ", "351 - ", 
        //                "352 - ", "353 - ", "354 - ", "355 - ", "356 - ", "357 - ", "358 - ", "359 - ", "360 - ", "361 - ", "362 - ", "363 - ", "364 - ", "365 - ", "366 - ", "367 - ", 
        //                "368 - ", "369 - ", "370 - ", "371 - ", "372 - ", "373 - ", "374 - ", "375 - ", "376 - ", "377 - ", "378 - ", "379 - ", "380 - ", "381 - ", "382 - ", "383 - ", 
        //                "384 - ", "385 - ", "386 - ", "387 - ", "388 - ", "389 - ", "390 - ", "391 - ", "392 - ", "393 - ", "394 - ", "395 - ", "396 - ", "397 - ", "398 - ", "399 - ", 
        //                "400 - ", "401 - ", "402 - ", "403 - ", "404 - ", "405 - ", "406 - ", "407 - ", "408 - ", "409 - ", "410 - ", "411 - ", "412 - ", "413 - ", "414 - ", "415 - ", 
        //                "416 - ", "417 - ", "418 - ", "419 - ", "420 - ", "421 - ", "422 - ", "423 - ", "424 - ", "425 - ", "426 - ", "427 - ", "428 - ", "429 - ", "430 - ", "431 - ", 
        //                "432 - ", "433 - ", "434 - ", "435 - ", "436 - ", "437 - ", "438 - ", "439 - ", "440 - ", "441 - ", "442 - ", "443 - ", "444 - ", "445 - ", "446 - ", "447 - ", 
        //                "448 - ", "449 - ", "450 - ", "451 - ", "452 - ", "453 - ", "454 - ", "455 - ", "456 - ", "457 - ", "458 - ", "459 - ", "460 - ", "461 - ", "462 - ", "463 - ", 
        //                "464 - ", "465 - ", "466 - ", "467 - ", "468 - ", "469 - ", "470 - ", "471 - ", "472 - ", "473 - ", "474 - ", "475 - ", "476 - ", "477 - ", "478 - ", "479 - ", 
        //                "480 - ", "481 - ", "482 - ", "483 - ", "484 - ", "485 - ", "486 - ", "487 - ", "488 - ", "489 - ", "490 - ", "491 - ", "492 - ", "493 - ", "494 - ", "495 - ", 
        //                "496 - ", "497 - ", "498 - ", "499 - ", "500 - ", "501 - ", "502 - ", "503 - ", "504 - ", "505 - ", "506 - ", "507 - ", "508 - ", "509 - ", "510 - ", "511 - ", 
        //                "512 - ", "513 - ", "514 - ", "515 - ", "516 - ", "517 - ", "518 - ", "519 - ", "520 - ", "521 - ", "522 - ", "523 - ", "524 - ", "525 - ", "526 - ", "527 - ", 
        //                "528 - ", "529 - ", "530 - ", "531 - ", "532 - ", "533 - ", "534 - ", "535 - ", "536 - ", "537 - ", "538 - ", "539 - ", "540 - ", "541 - ", "542 - ", "543 - ", 
        //                "544 - ", "545 - ", "546 - ", "547 - ", "548 - ", "549 - ", "550 - ", "551 - ", "552 - ", "553 - ", "554 - ", "555 - ", "556 - ", "557 - ", "558 - ", "559 - ", 
        //                "560 - ", "561 - ", "562 - ", "563 - ", "564 - ", "565 - ", "566 - ", "567 - ", "568 - ", "569 - ", "570 - ", "571 - ", "572 - ", "573 - ", "574 - ", "575 - ", 
        //                "576 - ", "577 - ", "578 - ", "579 - ",                       
        //                "580 - ", "581 - ", "582 - ", "583 - ", "584 - ", "585 - ", "586 - ", "587 - ", "588 - ", "589 - ", "590 - ", "591 - ", "592 - ", "593 - ", "594 - ", "595 - ", 
        //                "596 - ", "597 - ", "598 - ", "599 - ", "600 - ", "601 - ", "602 - ", "603 - ", "604 - ", "605 - ", "606 - ", "607 - ", "608 - ", "609 - ", "610 - ", "611 - ", 
        //                "612 - ", "613 - ", "614 - ", "615 - ", "616 - ", "617 - ", "618 - ", "619 - ", "620 - ", "621 - ", "622 - ", "623 - ", "624 - ", "625 - ", "626 - ", "627 - ", 
        //                "628 - ", "629 - ", "630 - ", "631 - ", "632 - ", "633 - ", "634 - ", "635 - ", "636 - ", "637 - ", "638 - ", "639 - ", "640 - ", "641 - ", "642 - ", "643 - ", 
        //                "644 - ", "645 - ", "646 - ", "647 - ", "648 - ", "649 - ", "650 - ", "651 - ", "652 - ", "653 - ", "654 - ", "655 - ", "656 - ", "657 - ", "658 - ", "659 - ", 
        //                "660 - ", "661 - ", "662 - ", "663 - ", "664 - ", "665 - ", "666 - ", "667 - ", "668 - ", "669 - ", "670 - ", "671 - ", "672 - ", "673 - ", "674 - ", "675 - ", 
        //             });
        //    MapList.Text = "HGSS Maps";
        //}

        private void GenerateDPPMapList()
        {
            #region Old List
            //MapList.Items.AddRange(new object[] { 
            //            "000 - Twinleaf Town Ext.",
            //            "001 - Verity Lakefront (North-West)", 
            //            "002 - Verity Lakefront (North-East)", 
            //            "003 - Verity Lakefront (South-West)", 
            //            "004 - Verity Lakefront (South-East)", 
            //            "005 - Route 201 (West)", 
            //            "006 - Route 201 (East)", 
            //            "007 - Sandgem Town", 
            //            "008 - Route 219", 
            //            "009 - Route 220 (West)", 
            //            "010 - Route 220 (Center)", 
            //            "011 - Route 220 (East)", 
            //            "012 - Route 221", 
            //            "013 - Pal Park", 
            //            "014 - Route 202", 
            //            "015 - Jubilife City Ext.(South-East)", 
            //            "016 - Jubilife City Ext.(North-East)", 
            //            "017 - Jubilife City Ext.(South-West)", 
            //            "018 - Jubilife City Ext.(North-West)", 
            //            "019 - Route 203 (West)", 
            //            "020 - Route 203 (East)", 
            //            "021 - Route 204 (South)", 
            //            "022 - Oreburgh City (West)", 
            //            "023 - Oreburgh City (East)", 
            //            "024 - Oreburgh Mine Ext.", 
            //            "025 - Route 207 (West)", 
            //            "026 - Route 207 (East)", 
            //            "027 - Route 206 (South)", 
            //            "028 - Route 206 (Center-South)", 
            //            "029 - Route 206 (Center-North)", 
            //            "030 - Route 206 (North)",
            //            "031 - Route 204 (North)", 
            //            "032 - Floaroma Town", 
            //            "033 - Floaroma Meadow", 
            //            "034 - Route 205 (South)", 
            //            "035 - Route 205 (Center)", 
            //            "036 - Valley Windworks", 
            //            "037 - Route 205 (North)", 
            //            "038 - Eterna Forest Ext.(South-West)", 
            //            "039 - Eterna Forest Ext.(North-West)", 
            //            "040 - Eterna Forest Ext.(North-East)", 
            //            "041 - Eterna Forest Ext.(South-East)", 
            //            "042 - Fuego IronWorks", 
            //            "043 - Route 205 (Eterna Exit)", 
            //            "044 - Eterna City Ext.(North-West)", 
            //            "045 - Eterna City Ext.(South)", 
            //            "046 - Eterna City Ext.(North-East)", 
            //            "047 - Route 211 (West)", 
            //            "048 - Canalave City Ext. (North)", 
            //            "049 - Canalave City Ext. (South)", 
            //            "050 - Route 218 (West)", 
            //            "051 - Route 218 (East)", 
            //            "052 - New Moon Island Ext.", 
            //            "053 - Full Mooon Island Ext. ", 
            //            "054 - Iron Island Ext", 
            //            "055 - Route 208 (West)", 
            //            "056 - Route 208 (East)", 
            //            "057 - Hearthome City (North-West)", 
            //            "058 - Hearthome City (North-East)", 
            //            "059 - Hearthome City (South-West)", 
            //            "060 - Hearthome City (South-East)", 
            //            "061 - Route 211 (East)", 
            //            "062 - Celestic Town", 
            //            "063 - Route 210 (North-West)", 
            //            "064 - Route 210 (North-Center)", 
            //            "065 - Route 210 (North-East)", 
            //            "066 - Route 210 (Center-North)", 
            //            "067 - Route 210 (Center-South)", 
            //            "068 - Route 210 (South)", 
            //            "069 - Route 215 (West)", 
            //            "070 - Route 215 (Center)", 
            //            "071 - Route 215 (East)", 
            //            "072 - Solaceon Town Ext.", 
            //            "073 - Solaceon Ruins Ext.", 
            //            "074 - Route 209 (North)", 
            //            "075 - Route 209 (South-East)", 
            //            "076 - Route 209 (South-Center)", 
            //            "077 - Route 209 (South-West)", 
            //            "078 - Veilstone City Ext.(North-East)", 
            //            "079 - Veilstone City Ext.(South-West)", 
            //            "080 - Veilstone City Ext.(South-East)", 
            //            "081 - Veilstone City Ext.(North-West)", 
            //            "082 - Pastoria City Ext.(North-East)", 
            //            "083 - Pastoria City Ext.(South-West)", 
            //            "084 - Pastoria City Ext.(South-East)", 
            //            "085 - Pastoria City Ext.(North-West)", 
            //            "086 - Route 212 (North)", 
            //            "087 - Route 212 (Center)", 
            //            "088 - Route 212 (South-West)", 
            //            "089 - Route 212 (South-Center.1)", 
            //            "090 - Route 212 (South-Center.2)", 
            //            "091 - Route 212 (South-East)", 
            //            "092 - Route 223 (North)", 
            //            "093 - Route 223 (Center-North)", 
            //            "094 - Route 223 (Center-South)", 
            //            "095 - Route 223 (South)", 
            //            "096 - Sunishore City Ext.(North-West)", 
            //            "097 - Sunishore City Ext.(North-East)", 
            //            "098 - Sunishore City Ext.(South-East)", 
            //            "099 - Sunishore City Ext.(South-West)", 
            //            "100 - Acuity Lakefront(North-West)", 
            //            "101 - Acuity Lakefront(North-East)", 
            //            "102 - Acuity Lakefront(South-East)", 
            //            "103 - Acuity Lakefront(South-West)",
            //            "104 - Snowpoint City (North)",
            //            "105 - Snowpoint City (South)",
            //            "106 - Route 217 (North)",
            //            "107 - Route 217 (Center-North)",
            //            "108 - Route 217 (Center-South)",
            //            "109 - Route 217 (Sud)",
            //            "110 - Route 216 (West)", 
            //            "111 - Route 216 (Center)",
            //            "112 - Route 216 (East)",
            //            "113 - Pokemon League (South)", 
            //            "114 - Pokemon League (North)", 
            //            "115 - Route 224 (North)", 
            //            "116 - Route 224 (Center-West)", 
            //            "117 - Route 224 (Center-East)",
            //            "118 - Route 224 (South)", 
            //            "119 - Route 224 (North : Shaymin Event)", 
            //            "120 - Route 224 (Center-West : Shaymin Event)", 
            //            "121 - Route 224 (Center-East : Shaymin Event)", 
            //            "122 - Route 224 (South : Shaymin Event)", 
            //            "123 - Flower Paradise", 
            //            "124 - Seabreak Path", 
            //            "125 - Seabreak Path 2", 
            //            "126 - Seabreak Path 3", 
            //            "127 - Seabreak Path 4", 
            //            "128 - Seabreak Path 5", 
            //            "129 - Seabreak Path 6", 
            //            "130 - Seabreak Path 7", 
            //            "131 - Seabreak Path 8", 
            //            "132 - Fight Area (West)", 
            //            "133 - Fight Area (East)", 
            //            "134 - Route 229 (West)", 
            //            "135 - Route 229 (East)", 
            //            "136 - Route 230 (West)", 
            //            "137 - Route 230 (Center)", 
            //            "138 - Route 230 (East)", 
            //            "139 - Route 228 (North)", 
            //            "140 - Route 228 (Center)", 
            //            "141 - Route 228 (South)", 
            //            "142 - Resort Area", 
            //            "143 - Route 213 (North-West)", 
            //            "144 - Route 213 (North-Center)",
            //            "145 - Route 213 (North-East)", 
            //            "146 - Route 213 (South-West)", 
            //            "147 - Route 213 (South-Center)", 
            //            "148 - Route 213 (South-East)",                        
            //            "149 - Route 222 (West)", 
            //            "150 - Route 222 (Center)", 
            //            "151 - Route 222 (East)",
            //            "152 - Route 214 (North)", 
            //            "153 - Route 214 (Center)", 
            //            "154 - Route 214 (South)",
            //            "155 - Valor LakeFront (North-West)", 
            //            "156 - Valor LakeFront (North-East)", 
            //            "157 - Valor LakeFront (South-West", 
            //            "158 - Valor LakeFront (South-East)",         
            //            "159 - Spring Path (North-West)", 
            //            "160 - Spring Path (North-East)", 
            //            "161 - Spring Path (South-West", 
            //            "162 - Spring Path (South-East)",
            //            "163 - Route 225 (North)", 
            //            "164 - Route 225 (Center)", 
            //            "165 - Route 225 (South)",
            //            "166 - Survival Area", 
            //            "167 - Route 226 (West)", 
            //            "168 - Route 226 (Center)", 
            //            "169 - Route 226 (East)",
            //            "170 - Route 227 (North)", 
            //            "171 - Route 228 (South)", 
            //            "172 - Stark Mountain (Outside)", 
            //            "173 - Tree Block Map", 
            //            "174 - Sea Block (NV) ", 
            //            "175 - Rock Block (Not Viewable (To fix)", 
            //            "176 - Tree Block Map", 
            //            "177 - Tree Block Map", 
            //            "178 - Grass Block (Not Viewable (To fix)", 
            //            "179 - Generic House", 
            //            "180 - Generic House 2", 
            //            "181 - Generic House 3", 
            //            "182 - Generic House 4", 
            //            "183 - Twinleaf Town - Rival's House (First Floor)", 
            //            "184 - Twinleaf Town - Rival's House (Second Floor)", 
            //            "185 - Twinleaf Town - Player's House (First Floor)", 
            //            "186 - Twinleaf Town - Player's House (Second Floor)", 
            //            "187 - Sandgen Town - Alter's House (First Floor)", 
            //            "188 - Sandgem Town - Alter's House (Second Floor)", 
            //            "189 - Market", 
            //            "190 - Pokemon Center (First Floor)", 
            //            "191 - Pokemon Center (Second Floor)", 
            //            "192 - Pokemon Center (Colosseum)", 
            //            "193 - Pokemon Center (Exchange)", 
            //            "194 - Pokemon DayCare", 
            //            "195 - Bike Shop ", 
            //            "196 - Game Corner", 
            //            "197 - Trainer School", 
            //            "198 - Generic Building (First Floor)", 
            //            "199 - Generic Building (Second Floor)",  
            //            "200 - Generic Building Beta (Third Floor)", 
            //            "201 - Generic Building Beta (Fourth Floor)",
            //            "202 - Generic Building Beta (Fifth Floor)", 
            //            "203 - Jubilife City - PokeKron (Second Floor)", 
            //            "204 - Jubilife City - PokeKron (Third Floor)",  
            //            "205 - Union Room",                       
            //            "206 - Jubilife City - PokeKron (First Floor)",
            //            "207 - Pokemon League - Pokemon Center", 
            //            "208 - Hotel",
            //            "209 - Grand Lake Hotel - Seven Stars Hotel",
            //            "210 - Grand Lake Hotel - Generic House",
            //            "211 - Jubilife City - TV Station (First Floor)",                        
            //            "212 - Jubilife City - TV Station (Second Floor)",                                                
            //            "213 - Jubilife City - TV Station (Third Floor)",                                                                        
            //            "214 - Jubilife City - TV Station (Fourth Floor)",
            //            "215 - Jubilife City - TV Station (DressUp Room)",
            //            "216 - Oreburgh City - Museum",
            //            "217 - Generic House 5", 
            //            "218 - Gate", 
            //            "219 - Gate (West - East)",
            //            "220 - Gate (Entrance)", 
            //            "221 - Sandgem Town - Rowan's Laboratory", 
            //            "222 - Pastoria City - Gym (North)", 
            //            "223 - Pastoria City - Gym (South)", 
            //            "224 - Canalave City - Gym", 
            //            "225 - Not Viewable (To fix)", 
            //            "226 - Not Viewable (To fix)", 
            //            "227 - Not Viewable (To fix)", 
            //            "228 - Oreburgh City - Gym", 
            //            "229 - Hearthome City - Gym (First Choose Room)", 
            //            "230 - Hearthome City - Gym (Room)",
            //            "231 - Hearthome City - Gym (Second Choose Room)",  
            //            "232 - Hearthome City - Gym (Leader Room)", 
            //            "233 - Snowpoint City - Gym", 
            //            "234 - Veilston City - Gym", 
            //            "235 - Colosseum 2", 
            //            "236 - Not Viewable (To fix)", 
            //            "237 - Not Viewable (To fix)",                         
            //            "238 - Not Viewable (To fix)", 
            //            "239 - Not Viewable (To fix)", 
            //            "240 - Hearthome City - Contest (Entrance)", 
            //            "241 - Hearthome City - Contest Room (West)", 
            //            "242 - Hearthome City - Contest Room (East)", 
            //            "243 - Hearthome City - Church", 
            //            "244 - Generic House 6", 
            //            "245 - Celestic Town - Ruins", 
            //            "246 - Celestic Town - Herb Shop", 
            //            "247 - Generic House Shop", 
            //            "248 - Department Store (First Floor)", 
            //            "249 - Department Store (Second Floor)", 
            //            "250 - Department Store (Third Floor) ", 
            //            "251 - Department Store (Fourth Floor) ", 
            //            "252 - Department Store (Fifth Floor) ", 
            //            "253 - Generic House 7", 
            //            "254 - Pokemon Mansion (West)", 
            //            "255 - Pokemon Mansion (East)", 
            //            "256 - Pokemon Mansion - Rooms",
            //            "257 - Pokemon Mansion - Master Room", 
            //            "258 - ???? Room", 
            //            "259 - Pokemon League - Aaron's Room", 
            //            "260 - Pokemon League - Bertha's Room", 
            //            "261 - Pokemon League - Flint's Room", 
            //            "262 - Pokemon League - Lucian's Room", 
            //            "263 - Pokemon League - Champion's Room (Not Viewable (To fix)", 
            //            "264 - Pokemon League - Hall of Fame ", 
            //            "265 - Battle Tower - Entrance", 
            //            "266 - Battle Tower - Corridor", 
            //            "267 - Battle Tower - Corridor 2", 
            //            "268 - Battle Tower - Room", 
            //            "269 - Battle Tower - Room 2", 
            //            "270 - Battle Tower - Room 3", 
            //            "271 - Ribbon Syndacate (First Floor)", 
            //            "272 - Ribbon Syndacate (Second Floor)", 
            //            "273 - Pal Park Building", 
            //            "274 - Not Viewable (To fix)", 
            //            "275 - Flower's Shop", 
            //            "276 - Game Corner's Exchange House", 
            //            "277 - GTS", 
            //            "278 - Market", 
            //            "279 - Lift", 
            //            "280 - Lifting Building (First Floor)", 
            //            "281 - Lifting Building (Second Floor)", 
            //            "282 - Lifting Building (Third Floor)", 
            //            "283 - Lifting Building (Fourth Floor)", 
            //            "284 - Lost Tower (Not Viewable (To fix))", 
            //            "285 - Lost Tower (Not Viewable (To fix))", 
            //            "286 - Lost Tower (Not Viewable (To fix))", 
            //            "287 - Lost Tower (Not Viewable (To fix))", 
            //            "288 - Lost Tower (Not Viewable (To fix))", 
            //            "289 - Generic Floor", 
            //            "290 - Canalave City - Library (Not Viewable (To fix))", 
            //            "291 - Canalave City - Library (Second Floor)", 
            //            "292 - Canalave City - Library (Third Floor)", 
            //            "293 - Eterna City - Gym (Entrance)", 
            //            "294 - Eterna City - Gym (Room)", 
            //            "295 - Sunyshore City - Gym (Entrance)", 
            //            "296 - Sunyshore City - Gym (First Room)", 
            //            "297 - Sunyshore City - Gym (Second Room)",
            //            "298 - Lift Room", 
            //            "299 - Battle Tower - Exchange Points Room", 
            //            "300 - Not Viewable (To fix)", 
            //            "301 - Not Viewable (To fix)", 
            //            "302 - TV Station - Wireless Room", 
            //            "303 - TV Station - Wi-fi Room", 
            //            "304 - Picture House?", 
            //            "305 - Pokemon League - Corridor", 
            //            "306 - Pokemon League - Corridor 2", 
            //            "307 - Pokemon League - Corridor 3", 
            //            "308 - Pokemon League - Hall of Fame Corridor", 
            //            "309 - Pokemon Center - Wi-fi Floor", 
            //            "310 - Flower Shop 2 ", 
            //            "311 - Generic House 8", 
            //            "312 - Generic House 9", 
            //            "313 - ", 
            //            "314 - ", 
            //            "315 - ", 
            //            "316 - ", "317 - ", "318 - ", "319 - ", 
            //            "320 - ", "321 - ", "322 - ", "323 - ", "324 - ", "325 - ", "326 - ", "327 - ", "328 - ", "329 - ", "330 - ", "331 - ", "332 - ", "333 - ", "334 - ", "335 - ", 
            //            "336 - ", "337 - ", "338 - ", "339 - ", "340 - ", "341 - ", "342 - ", "343 - ", "344 - ", "345 - ", "346 - ", "347 - ", "348 - ", "349 - ", "350 - ", "351 - ", 
            //            "352 - ", "353 - ", "354 - ", "355 - ", "356 - ", "357 - ", "358 - ", "359 - ", "360 - ", "361 - ", "362 - ", "363 - ", "364 - ", "365 - ", "366 - ", "367 - ", 
            //            "368 - ", "369 - ", "370 - ", "371 - ", "372 - ", "373 - ", "374 - ", "375 - ", "376 - ", "377 - ", "378 - ", "379 - ", "380 - ", "381 - ", "382 - ", "383 - ", 
            //            "384 - ", "385 - ", "386 - ", "387 - ", "388 - ", "389 - ", "390 - ", "391 - Tree Block", "392 - ", "393 - ", "394 - ", "395 - ", "396 - ", "397 - ", "398 - ", "399 - ", 
            //            "400 - Tree Block", "401 - Tree Block", "402 - ", "403 - ", "404 - ", "405 - ", "406 - ", "407 - ", "408 - ", "409 - ", "410 - ", "411 - ", "412 - ", "413 - ", "414 - ", "415 - ", 
            //            "416 - ", "417 - ", "418 - ", "419 - ", "420 - ", "421 - ", "422 - ", "423 - ", "424 - ", "425 - ", "426 - ", "427 - ", "428 - ", "429 - ", "430 - ", "431 - ", 
            //            "432 - ", "433 - ", "434 - ", "435 - ", "436 - ", "437 - ", "438 - ", "439 - ", "440 - ", "441 - ", "442 - ", "443 - ", "444 - ", "445 - ", "446 - ", "447 - ", 
            //            "448 - ", "449 - ", "450 - ", "451 - ", "452 - ", "453 - ", "454 - ", "455 - ", "456 - ", "457 - ", "458 - ", "459 - ", "460 - ", "461 - ", "462 - ", "463 - ", 
            //            "464 - ", "465 - ", "466 - ", "467 - ", "468 - ", "469 - ", "470 - ", "471 - ", "472 - ", "473 - ", "474 - ", "475 - ", "476 - ", "477 - ", "478 - ", "479 - ", 
            //            "480 - ", "481 - ", "482 - ", "483 - ", "484 - ", "485 - ", "486 - ", "487 - ", "488 - ", "489 - ", "490 - ", "491 - Tree Block", "492 - ", "493 - ", "494 - ", "495 - ", 
            //            "496 - ", "497 - ", "498 - ", "499 - ", "500 - Tree Block", "501 - Tree Block", "502 - ", "503 - ", "504 - ", "505 - ", "506 - ", "507 - ", "508 - ", "509 - ", "510 - ", "511 - ", 
            //            "512 - ", "513 - ", "514 - ", "515 - ", "516 - ", "517 - ", "518 - ", "519 - ", "520 - ", "521 - ", "522 - ", "523 - ", "524 - ", "525 - ", "526 - ", "527 - ", 
            //            "528 - ", "529 - ", "530 - ", "531 - ", "532 - ", "533 - ", "534 - ", "535 - ", "536 - ", "537 - ", "538 - ", "539 - ", "540 - ", "541 - ", "542 - ", "543 - ", 
            //            "544 - ", 
            //            "545 - ", 
            //            "546 - ", 
            //            "547 - ", 
            //            "548 - ", 
            //            "549 - ", 
            //            "550 - ", 
            //            "551 - ", 
            //            "552 - ", 
            //            "553 - Spear Pillar", 
            //            "554 - Spear Pillar 2", 
            //            "555 - Spear Pillar 3", 
            //            "556 - Spear Pillar 4", 
            //            "557 - Spear Pillar (After Arceus?)", 
            //            "558 - Spear Pillar 2 (After Arceus?)", 
            //            "559 - Spear Pillar 3 (After Arceus?)", 
            //            "560 - Spear Pillar 4 (After Arceus?)", 
            //            "561 - Underground (Up Map)", 
            //            "562 - Underground", 
            //            "563 - Underground 2 ", 
            //            "564 - Underground 3 ", 
            //            "565 - Underground 4", 
            //            "566 - Underground 5", 
            //            "567 - Underground 6 ", 
            //            "568 - Underground 7 ", 
            //            "569 - Underground 8", 
            //            "570 - Underground 9", 
            //            "571 - Underground 10", 
            //            "572 - Underground 11", 
            //            "573 - Underground 12", 
            //            "574 - Underground 13", 
            //            "575 - Underground 14", 
            //            "576 - Underground 15", 
            //            "577 - Underground 16 (Ex) "
            //         });
            #endregion
            for (int i = 0; i < 577; i++)
            {
                if (mapListData.ContainsKey(i))
                    MapList.Items.Add(i + " - " + mapListData[i].Split('-')[1]);
                else
                    MapList.Items.Add(i);
            }
          MapList.Text = "DP Maps";
        }

        //private void GeneratePlMapList()
        //{
        //    MapList.Items.AddRange(new object[] { 
        //                "000 - Twinleaf Town Ext.",
        //                "001 - Tree Block Map", 
        //                "002 - Tree Block Map", 
        //                "003 - Tree Block Map", 
        //                "004 - Verity Lakefront", 
        //                "005 - Route 201 (West)", 
        //                "006 - Route 201 (East)", 
        //                "007 - Sandgem Town", 
        //                "008 - Route 219", 
        //                "009 - Route 220 (West)", 
        //                "010 - Route 220 (Center)", 
        //                "011 - Route 220 (East)", 
        //                "012 - Route 221", 
        //                "013 - Pal Park", 
        //                "014 - Route 202", 
        //                "015 - Jubilife City Ext.(South-East)", 
        //                "016 - Jubilife City Ext.(North-East)", 
        //                "017 - Jubilife City Ext.(South-West)", 
        //                "018 - Jubilife City Ext.(North-West)", 
        //                "019 - Route 203 (West)", 
        //                "020 - Route 203 (East)", 
        //                "021 - Route 204 (South)", 
        //                "022 - Oreburgh City (West)", 
        //                "023 - Oreburgh City (East)", 
        //                "024 - Oreburgh Mine Ext.", 
        //                "025 - Route 207 (West)", 
        //                "026 - Route 207 (East)", 
        //                "027 - Route 206 (South)", 
        //                "028 - Route 206 (Center-South)", 
        //                "029 - Route 206 (Center-North)", 
        //                "030 - Route 206 (North)",
        //                "031 - Route 204 (North)", 
        //                "032 - Floaroma Town", 
        //                "033 - Floaroma Meadow", 
        //                "034 - Route 205 (South)", 
        //                "035 - Route 205 (Center-East)", 
        //                "036 - Route 205 (Center-West)", 
        //                "037 - Route 205 (Eterna Forest South Exit)", 
        //                "038 - Route 205 (Eterna Forest Center-West Exit)", 
        //                "039 - Tree Block Map", 
        //                "040 - Route 205 (Eterna Forest Center-East Exit)", 
        //                "041 - Route 205 (Eterna Forest Border)", 
        //                "042 - Route 205 (North-East)", 
        //                "043 - Route 205 (North-West)", 
        //                "044 - Eterna City Ext.(North-West)", 
        //                "045 - Eterna City Ext.(South)", 
        //                "046 - Eterna City Ext.(North-East)", 
        //                "047 - Route 211 (West)", 
        //                "048 - Canalave City Ext. (North)", 
        //                "049 - Canalave City Ext. (South)", 
        //                "050 - Route 218 (West)", 
        //                "051 - Route 218 (East)", 
        //                "052 - New Moon Island Ext.", 
        //                "053 - Full Mooon Island Ext. ", 
        //                "054 - Iron Island Ext", 
        //                "055 - Route 208 (West)", 
        //                "056 - Route 208 (East)", 
        //                "057 - Hearthome City (North-West)", 
        //                "058 - Hearthome City (North-East)", 
        //                "059 - Hearthome City (South-West)", 
        //                "060 - Hearthome City (South-East)", 
        //                "061 - Route 211 (East)", 
        //                "062 - Celestic Town", 
        //                "063 - Route 210 (North-West)", 
        //                "064 - Route 210 (North-Center)", 
        //                "065 - Route 210 (North-East)", 
        //                "066 - Route 210 (Center-North)", 
        //                "067 - Route 210 (Center-South)", 
        //                "068 - Route 210 (South)", 
        //                "069 - Route 215 (West)", 
        //                "070 - Route 215 (Center)", 
        //                "071 - Route 215 (East)", 
        //                "072 - Solaceon Town Ext.", 
        //                "073 - Solaceon Ruins Ext.", 
        //                "074 - Route 209 (North)", 
        //                "075 - Route 209 (South-East)", 
        //                "076 - Route 209 (South-Center)", 
        //                "077 - Route 209 (South-West)", 
        //                "078 - Veilstone City Ext.(North-East)", 
        //                "079 - Veilstone City Ext.(South-West)", 
        //                "080 - Veilstone City Ext.(South-East)", 
        //                "081 - Veilstone City Ext.(North-West)", 
        //                "082 - Pastoria City Ext.(North-East)", 
        //                "083 - Pastoria City Ext.(South-West)", 
        //                "084 - Pastoria City Ext.(South-East)", 
        //                "085 - Pastoria City Ext.(North-West)", 
        //                "086 - Route 212 (North)", 
        //                "087 - Route 212 (Center)", 
        //                "088 - Route 212 (South-West)", 
        //                "089 - Route 212 (South-Center.1)", 
        //                "090 - Route 212 (South-Center.2)", 
        //                "091 - Route 212 (South-East)", 
        //                "092 - Route 223 (North)", 
        //                "093 - Route 223 (Center-North)", 
        //                "094 - Route 223 (Center-South)", 
        //                "095 - Route 223 (South)", 
        //                "096 - Sunishore City Ext.(North-West)", 
        //                "097 - Sunishore City Ext.(North-East)", 
        //                "098 - Sunishore City Ext.(South-East)", 
        //                "099 - Sunishore City Ext.(South-West)", 
        //                "100 - Tree Block", 
        //                "101 - Tree Block", 
        //                "102 - Route 217 (North)",
        //                "103 - Route 217 (North-East)",
        //                "104 - Snowpoint City (North)",
        //                "105 - Snowpoint City (South)",
        //                "106 - Route 217 (North)",
        //                "107 - Route 217 (Center-North)",
        //                "108 - Route 217 (Center-South)",
        //                "109 - Route 217 (Sud)",
        //                "110 - Route 216 (West)", 
        //                "111 - Route 216 (Center)",
        //                "112 - Route 216 (East)",
        //                "113 - Pokemon League (South)", 
        //                "114 - Pokemon League (North)", 
        //                "115 - Route 224 (North)", 
        //                "116 - Route 224 (Center-West)", 
        //                "117 - Route 224 (Center-East)",
        //                "118 - Route 224 (South)", 
        //                "119 - Route 224 (North : Shaymin Event)", 
        //                "120 - Route 224 (Center-West : Shaymin Event)", 
        //                "121 - Route 224 (Center-East : Shaymin Event)", 
        //                "122 - Route 224 (South : Shaymin Event)", 
        //                "123 - Flower Paradise", 
        //                "124 - Seabreak Path", 
        //                "125 - Seabreak Path 2", 
        //                "126 - Seabreak Path 3", 
        //                "127 - Seabreak Path 4", 
        //                "128 - Seabreak Path 5", 
        //                "129 - Seabreak Path 6", 
        //                "130 - Seabreak Path 7", 
        //                "131 - Seabreak Path 8", 
        //                "132 - Fight Area (West)", 
        //                "133 - Fight Area (East)", 
        //                "134 - Route 229 (West)", 
        //                "135 - Route 229 (East)", 
        //                "136 - Route 230 (West)", 
        //                "137 - Route 230 (Center)", 
        //                "138 - Route 230 (East)", 
        //                "139 - Route 228 (North)", 
        //                "140 - Route 228 (Center)", 
        //                "141 - Route 228 (South)", 
        //                "142 - Resort Area", 
        //                "143 - Route 213 (North-West)", 
        //                "144 - Route 213 (North-Center)",
        //                "145 - Route 213 (North-East)", 
        //                "146 - Route 213 (South-West)", 
        //                "147 - Route 213 (South-Center)", 
        //                "148 - Route 213 (South-East)",                        
        //                "149 - Route 222 (West)", 
        //                "150 - Route 222 (Center)", 
        //                "151 - Route 222 (East)",
        //                "152 - Route 214 (North)", 
        //                "153 - Route 214 (Center)", 
        //                "154 - Route 214 (South)",
        //                "155 - Valor LakeFront (North-West)", 
        //                "156 - Valor LakeFront (North-East)", 
        //                "157 - Valor LakeFront (South-West", 
        //                "158 - Valor LakeFront (South-East)",         
        //                "159 - Route ??? (North-West)", 
        //                "160 - Route ??? (North-East)", 
        //                "161 - Route ??? (South-West", 
        //                "162 - Route ??? (South-East)",
        //                "163 - Route 225 (North)", 
        //                "164 - Route 225 (Center)", 
        //                "165 - Route 225 (South)",
        //                "166 - Survival Area", 
        //                "167 - Route 226 (West)", 
        //                "168 - Route 226 (Center)", 
        //                "169 - Route 226 (East)",
        //                "170 - Route 227 (North)", 
        //                "171 - Route 228 (South)", 
        //                "172 - Stark Mountain (Outside)", 
        //                "173 - Tree Block Map", 
        //                "174 - Sea Block (NV) ", 
        //                "175 - Rock Block (Not Viewable (To fix)", 
        //                "176 - Tree Block Map", 
        //                "177 - Tree Block Map", 
        //                "178 - Grass Block (Not Viewable (To fix)", 
        //                "179 - Tree Block Map",
        //                "180 - Generic House", 
        //                "181 - Generic House 2", 
        //                "182 - Generic House 3", 
        //                "183 - Generic House 4", 
        //                "184 - Twinleaf Town - Rival's House (First Floor)", 
        //                "185 - Twinleaf Town - Rival's House (Second Floor)", 
        //                "186 - Twinleaf Town - Player's House (First Floor)", 
        //                "187 - Twinleaf Town - Player's House (Second Floor)", 
        //                "188 - Sandgen Town - Alter's House (First Floor)", 
        //                "189 - Sandgem Town - Alter's House (Second Floor)", 
        //                "190 - Market", 
        //                "191 - Pokemon Center (First Floor)", 
        //                "192 - Pokemon Center (Second Floor)", 
        //                "193 - Pokemon Center (Colosseum)", 
        //                "194 - Pokemon Center (Exchange)", 
        //                "195 - Pokemon DayCare", 
        //                "196 - Bike Shop ", 
        //                "197 - Game Corner", 
        //                "198 - Trainer School", 
        //                "199 - Generic Building (First Floor)", 
        //                "200 - Generic Building (Second Floor)",  
        //                "201 - Generic Building Beta (Third Floor)", 
        //                "202 - Generic Building Beta (Fourth Floor)",
        //                "203 - Generic Building Beta (Fifth Floor)", 
        //                "204 - Jubilife City - PokeKron (Second Floor)", 
        //                "204 - Jubilife City - PokeKron (Third Floor)",  
        //                "205 - Union Room",                       
        //                "206 - Jubilife City - PokeKron (First Floor)",
        //                "207 - Pokemon League - Pokemon Center", 
        //                "208 - Hotel",
        //                "209 - Grand Lake Hotel - Seven Stars Hotel",
        //                "210 - Grand Lake Hotel - Generic House",
        //                "211 - Jubilife City - TV Station (First Floor)",                        
        //                "212 - Jubilife City - TV Station (Second Floor)",                                                
        //                "213 - Jubilife City - TV Station (Third Floor)",                                                                        
        //                "214 - Jubilife City - TV Station (Fourth Floor)",
        //                "215 - Jubilife City - TV Station (DressUp Room)",
        //                "216 - Oreburgh City - Museum",
        //                "217 - Generic House 5", 
        //                "218 - Gate", 
        //                "219 - Gate (West - East)",
        //                "220 - Gate (Entrance)", 
        //                "221 - Sandgem Town - Rowan's Laboratory", 
        //                "222 - Pastoria City - Gym (North)", 
        //                "223 - Pastoria City - Gym (South)", 
        //                "224 - Canalave City - Gym", 
        //                "225 - Not Viewable (To fix)", 
        //                "226 - Not Viewable (To fix)", 
        //                "227 - Not Viewable (To fix)", 
        //                "228 - Oreburgh City - Gym", 
        //                "229 - Hearthome City - Gym (First Choose Room)", 
        //                "230 - Hearthome City - Gym (Room)",
        //                "231 - Hearthome City - Gym (Second Choose Room)",  
        //                "232 - Hearthome City - Gym (Leader Room)", 
        //                "233 - Snowpoint City - Gym", 
        //                "234 - Veilston City - Gym", 
        //                "235 - Colosseum 2", 
        //                "236 - Not Viewable (To fix)", 
        //                "237 - Not Viewable (To fix)",                         
        //                "238 - Not Viewable (To fix)", 
        //                "239 - Not Viewable (To fix)", 
        //                "240 - Hearthome City - Contest (Entrance)", 
        //                "241 - Hearthome City - Contest Room (West)", 
        //                "242 - Hearthome City - Contest Room (East)", 
        //                "243 - Hearthome City - Church", 
        //                "244 - Generic House 6", 
        //                "245 - Celestic Town - Ruins", 
        //                "246 - Celestic Town - Herb Shop", 
        //                "247 - Generic House Shop", 
        //                "248 - Department Store (First Floor)", 
        //                "249 - Department Store (Second Floor)", 
        //                "250 - Department Store (Third Floor) ", 
        //                "251 - Department Store (Fourth Floor) ", 
        //                "252 - Department Store (Fifth Floor) ", 
        //                "253 - Generic House 7", 
        //                "254 - Pokemon Mansion (West)", 
        //                "255 - Pokemon Mansion (East)", 
        //                "256 - Pokemon Mansion - Rooms",
        //                "257 - Pokemon Mansion - Master Room", 
        //                "258 - ???? Room", 
        //                "259 - Pokemon League - Aaron's Room", 
        //                "260 - Pokemon League - Bertha's Room", 
        //                "261 - Pokemon League - Flint's Room", 
        //                "262 - Pokemon League - Lucian's Room", 
        //                "263 - Pokemon League - Champion's Room (Not Viewable (To fix)", 
        //                "264 - Pokemon League - Hall of Fame ", 
        //                "265 - Battle Tower - Entrance", 
        //                "266 - Battle Tower - Corridor", 
        //                "267 - Battle Tower - Corridor 2", 
        //                "268 - Battle Tower - Room", 
        //                "269 - Battle Tower - Room 2", 
        //                "270 - Battle Tower - Room 3", 
        //                "271 - Ribbon Syndacate (First Floor)", 
        //                "272 - Ribbon Syndacate (Second Floor)", 
        //                "273 - Pal Park Building", 
        //                "274 - Not Viewable (To fix)", 
        //                "275 - Flower's Shop", 
        //                "276 - Game Corner's Exchange House", 
        //                "277 - GTS", 
        //                "278 - Market", 
        //                "279 - Lift", 
        //                "280 - Lifting Building (First Floor)", 
        //                "281 - Lifting Building (Second Floor)", 
        //                "282 - Lifting Building (Third Floor)", 
        //                "283 - Lifting Building (Fourth Floor)", 
        //                "284 - Lost Tower (Not Viewable (To fix))", 
        //                "285 - Lost Tower (Not Viewable (To fix))", 
        //                "286 - Lost Tower (Not Viewable (To fix))", 
        //                "287 - Lost Tower (Not Viewable (To fix))", 
        //                "288 - Lost Tower (Not Viewable (To fix))", 
        //                "289 - Generic Floor", 
        //                "290 - Canalave City - Library (Not Viewable (To fix))", 
        //                "291 - Canalave City - Library (Second Floor)", 
        //                "292 - Canalave City - Library (Third Floor)", 
        //                "293 - Eterna City - Gym (Entrance)", 
        //                "294 - Eterna City - Gym (Room)", 
        //                "295 - Sunyshore City - Gym (Entrance)", 
        //                "296 - Sunyshore City - Gym (First Room)", 
        //                "297 - Sunyshore City - Gym (Second Room)",
        //                "298 - Lift Room", 
        //                "299 - Battle Tower - Exchange Points Room", 
        //                "300 - Not Viewable (To fix)", 
        //                "301 - Not Viewable (To fix)", 
        //                "302 - TV Station - Wireless Room", 
        //                "303 - TV Station - Wi-fi Room", 
        //                "304 - Picture House?", 
        //                "305 - Pokemon League - Corridor", 
        //                "306 - Pokemon League - Corridor 2", 
        //                "307 - Pokemon League - Corridor 3", 
        //                "308 - Pokemon League - Hall of Fame Corridor", 
        //                "309 - Pokemon Center - Wi-fi Floor", 
        //                "310 - Flower Shop 2 ", 
        //                "311 - Generic House 8", 
        //                "312 - Generic House 9", 
        //                "313 - ", 
        //                "314 - ", 
        //                "315 - ", 
        //                "316 - ", "317 - ", "318 - ", "319 - ", 
        //                "320 - ", "321 - ", "322 - ", "323 - ", "324 - ", "325 - ", "326 - ", "327 - ", "328 - ", "329 - ", "330 - ", "331 - ", "332 - ", "333 - ", "334 - ", "335 - ", 
        //                "336 - ", "337 - ", "338 - ", "339 - ", "340 - ", "341 - ", "342 - ", "343 - ", "344 - ", "345 - ", "346 - ", "347 - ", "348 - ", "349 - ", "350 - ", "351 - ", 
        //                "352 - ", "353 - ", "354 - ", "355 - ", "356 - ", "357 - ", "358 - ", "359 - ", "360 - ", "361 - ", "362 - ", "363 - ", "364 - ", "365 - ", "366 - ", "367 - ", 
        //                "368 - ", "369 - ", "370 - ", "371 - ", "372 - ", "373 - ", "374 - ", "375 - ", "376 - ", "377 - ", "378 - ", "379 - ", "380 - ", "381 - ", "382 - ", "383 - ", 
        //                "384 - ", "385 - ", "386 - ", "387 - ", "388 - ", "389 - ", "390 - ", "391 - Tree Block", "392 - ", "393 - ", "394 - ", "395 - ", "396 - ", "397 - ", "398 - ", "399 - ", 
        //                "400 - Tree Block", "401 - Tree Block", "402 - ", "403 - ", "404 - ", "405 - ", "406 - ", "407 - ", "408 - ", "409 - ", "410 - ", "411 - ", "412 - ", "413 - ", "414 - ", "415 - ", 
        //                "416 - ", "417 - ", "418 - ", "419 - ", "420 - ", "421 - ", "422 - ", "423 - ", "424 - ", "425 - ", "426 - ", "427 - ", "428 - ", "429 - ", "430 - ", "431 - ", 
        //                "432 - ", "433 - ", "434 - ", "435 - ", "436 - ", "437 - ", "438 - ", "439 - ", "440 - ", "441 - ", "442 - ", "443 - ", "444 - ", "445 - ", "446 - ", "447 - ", 
        //                "448 - ", "449 - ", "450 - ", "451 - ", "452 - ", "453 - ", "454 - ", "455 - ", "456 - ", "457 - ", "458 - ", "459 - ", "460 - ", "461 - ", "462 - ", "463 - ", 
        //                "464 - ", "465 - ", "466 - ", "467 - ", "468 - ", "469 - ", "470 - ", "471 - ", "472 - ", "473 - ", "474 - ", "475 - ", "476 - ", "477 - ", "478 - ", "479 - ", 
        //                "480 - ", "481 - ", "482 - ", "483 - ", "484 - ", "485 - ", "486 - ", "487 - ", "488 - ", "489 - ", "490 - ", "491 - Tree Block", "492 - ", "493 - ", "494 - ", "495 - ", 
        //                "496 - ", "497 - ", "498 - ", "499 - ", "500 - Tree Block", "501 - Tree Block", "502 - ", "503 - ", "504 - ", "505 - ", "506 - ", "507 - ", "508 - ", "509 - ", "510 - ", "511 - ", 
        //                "512 - ", "513 - ", "514 - ", "515 - ", "516 - ", "517 - ", "518 - ", "519 - ", "520 - ", "521 - ", "522 - ", "523 - ", "524 - ", "525 - ", "526 - ", "527 - ", 
        //                "528 - ", "529 - ", "530 - ", "531 - ", "532 - ", "533 - ", "534 - ", "535 - ", "536 - ", "537 - ", "538 - ", "539 - ", "540 - ", "541 - ", "542 - ", "543 - ", 
        //                "544 - ", 
        //                "545 - ", 
        //                "546 - ", 
        //                "547 - ", 
        //                "548 - ", 
        //                "549 - ", 
        //                "550 - ", 
        //                "551 - ", 
        //                "552 - ", 
        //                "553 - Spear Pillar", 
        //                "554 - Spear Pillar 2", 
        //                "555 - Spear Pillar 3", 
        //                "556 - Spear Pillar 4", 
        //                "557 - Spear Pillar (After Arceus?)", 
        //                "558 - Spear Pillar 2 (After Arceus?)", 
        //                "559 - Spear Pillar 3 (After Arceus?)", 
        //                "560 - Spear Pillar 4 (After Arceus?)", 
        //                "561 - Underground (Up Map)", 
        //                "562 - Underground", 
        //                "563 - Underground 2 ", 
        //                "564 - Underground 3 ", 
        //                "565 - Underground 4", 
        //                "566 - Underground 5", 
        //                "567 - Underground 6 ", 
        //                "568 - Underground 7 ", 
        //                "569 - Underground 8", 
        //                "570 - Underground 9", 
        //                "571 - Underground 10", 
        //                "572 - Underground 11", 
        //                "573 - Underground 12", 
        //                "574 - Underground 13", 
        //                "575 - Underground 14", 
        //                "576 - Underground 15", 
        //                "577 - Underground 16 (Ex) "
        //             });
        //    MapList.Text = "PL Maps";
        //}

        private void MapList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetForm();
            Console.Clear();

            Console.AppendText("\nFinish to reset form. Start Loading part...");

            LoadSelectedItem();

            Console.AppendText("\nFinish Loading part succesfully.");

            OpenGlControl.Invalidate();
        }

        private void showItem()
        {
            Console.AppendText("\nStart Showing method");
            var shapeNum = actualModel.getMDL0at(0).modelInfo.shapeNum;

            for (int shapeCounter = 0; shapeCounter < shapeNum; shapeCounter++)
            {
                Console.AppendText("\nShowing shape " + shapeCounter);
                showShape(shapeCounter);
            }
            Console.AppendText("\nStop Showing method");
        }

        private void LoadSelectedItem()
        {
            int selectedIndex = MapList.SelectedIndex;

            Console.AppendText("\nSelected item is " + selectedIndex);

            renderer.modelList.Clear();


            ClosableMemoryStream stream = actualNarc.figm.fileData[selectedIndex];
            actualMap = new Maps();

            Console.AppendText("\nStarting loading map");
            actualMap.LoadMap(new BinaryReader(stream), false);

            Console.AppendText("\nFinish loading map.");
            Console.AppendText("\nMap is a " + Maps.type + " map");


            actualModel = actualMap.actualModel;

            //string name = MapList.SelectedItem.ToString();
            ////if (!name.Contains("("))
            ////{
            ////    MapList.Items.RemoveAt(selectedIndex);
            ////    name += " (" + actualModel.getMDL0at(0).model.modelNameList[0].name.TrimEnd('\0') + ") ";
            ////    MapList.Items.Insert(selectedIndex, name);
            ////    MapList.Text = name;
            ////}

            printInfoMap();



            if (actualModel == null)
            {
                var reader = new BinaryReader(actualNarc.figm.fileData[selectedIndex]);
                actualModel = new Nsbmd();
                actualModel.LoadBMD0(reader, 0);
            }

            PolyNumMax.Enabled = true;
            Tex_File.Text = MapEditor.textureName;

            PopAssTable(actualModel);

            Console.AppendText("\nTry to show...");
            showItem();
            OpenGlControl.Invalidate();
            if (Maps.type != NSBMD_MODEL)
            {
                if (actualMap.arrayMovement !=null || actualMap.arrayMovementBW != null)
                    PopMovTable(actualMap);
                PopObjTable(actualMap);
                headerId = selectedIndex;
                reactionNormal(main);
            }

            OpenGlControl.Invalidate();
        }

        private void printInfoMap()
        {
            Console.AppendText("\nThere are " + actualModel.models.Length + " models.");
            for (int i = 0; i < actualModel.models.Length; i++)
            {
                var model = actualModel.models[i];

                Console.AppendText("\nModel " + i + " is called " + model.model.modelNameList[i].name + " .");
                Console.AppendText("\nModel " + i + " has " + model.modelInfo.shapeNum + " polygon.");
                Console.AppendText("\nModel " + i + " has " + model.material.dictMat.matNum + " material.");
                Console.AppendText("\nModel " + i + " has " + model.material.dictTex.texNum + " texture.");
            }
        }

        private void ResetForm()
        {
            PolyNumMax.Value = -1M;
            AssTable.Rows.Clear();
            MovInfo.Rows.Clear();
            MovInfo.Columns.Clear();
            ObjInfo.Rows.Clear();
        }

        private void PopObjTable(Maps actualMap)
        {
            if (Maps.type != BWMAP && Maps.type != BW2MAP)
                PopObjTableOther(actualMap);
            else
                PopObjTableBW(actualMap);
        }

        private void PopObjTableBW(Maps actualMap)
        {
            objShowInfo.Rows.Clear();
            objShowInfo.Columns.Clear();
            objShowInfo = InitTable(ref objShowInfo,0);
            objShowInfo = PopMovTableBW(actualMap, false, ref objShowInfo, 0);

            int objCounter = 0;
            foreach (Maps.Obj_S actualObject in actualMap.listObjectsBW)
            {
                int yCoord = -actualObject.xBig + 15;
                int xCoord = actualObject.yBig + 17;
                Console.AppendText("\nX vale: " + xCoord + " Y vale: " + yCoord);
                if (xCoord > 0 && xCoord < MAXMOVSIZE && yCoord > 0 && yCoord < MAXMOVSIZE)
                    objShowInfo[xCoord, yCoord].Value = objCounter;

                ObjInfo.Rows.Add(new object[] { objCounter, 
                                                actualObject.idObject.ToString(), 
                                                                actualObject.xBig.ToString(), 
                                                               actualObject.xSmall.ToString(), 
                                                               actualObject.yBig.ToString(), 
                                                               actualObject.ySmall.ToString(),
                                                               actualObject.zBig.ToString(), 
                                                               actualObject.zSmall.ToString(),
                                                               actualObject.heightObject.ToString(), 
                                                               actualObject.lengthObject.ToString(),
                                                               actualObject.widthObject.ToString() 
                });
                objCounter++;
            }
        }

        private void PopObjTableOther(Maps actualMap)
        {
            objShowInfo.Rows.Clear();
            objShowInfo.Columns.Clear();
            objShowInfo = InitTable(ref objShowInfo, 0);
            objShowInfo = PopTableOther(actualMap, false, ref objShowInfo, 0, 0);

            int objCounter = 0;
            foreach (Maps.Obj_S actualObject in actualMap.listObjects)
            {
                int yCoord = actualObject.xBig + 16;
                int xCoord = actualObject.yBig + 16;
                if (xCoord > 0 && xCoord < MAXMOVSIZE && yCoord > 0 && yCoord < MAXMOVSIZE)
                    objShowInfo[xCoord, yCoord].Value = objCounter;

                ObjInfo.Rows.Add(new object[] { objCounter,
                                                 actualObject.idObject.ToString(), 
                                                               actualObject.xBig.ToString(), 
                                                               actualObject.xSmall.ToString(), 
                                                               actualObject.yBig.ToString(), 
                                                               actualObject.ySmall.ToString(),
                                                               actualObject.zBig.ToString(), 
                                                               actualObject.zSmall.ToString(),
                                                               actualObject.heightObject.ToString(), 
                                                               actualObject.lengthObject.ToString(),
                                                               actualObject.widthObject.ToString() 
                                                              });
                objCounter++;


            }





        }

        private void PopMovTable(Maps actualMap)
        {
            MovInfo = InitTable(ref MovInfo,0);
            MovInfoL1 = InitTable(ref MovInfoL1,0);

            if (Maps.type != BWMAP && Maps.type != BW2MAP)
            {
                if (actualMap.arrayMovement == null)
                    return;
                MovInfo = PopTableOther(actualMap, true, ref MovInfo, 0,0);
                MovInfoL1 = PopTableOther(actualMap, true, ref MovInfoL1, 1,0);
            }

            else
            {
                if (actualMap.arrayMovementBW == null)
                    return;
                MovInfoL2 = InitTable(ref MovInfoL2,0);
                MovInfoL3 = InitTable(ref MovInfoL3,0);
                MovInfo = PopMovTableBW(actualMap, true, ref MovInfo, 0);
                MovInfoL1 = PopMovTableBW(actualMap, true, ref MovInfoL1, 1);
                MovInfoL2 = PopMovTableBW(actualMap, true, ref MovInfoL2, 2);
                MovInfoL3 = PopMovTableBW(actualMap, true, ref MovInfoL3, 3);
            }
        }

        private DataGridView PopMovTableBW(Maps actualMap, bool number, ref DataGridView actualTable, int layer)
        {
            int movCounter = 0;
            while (movCounter < actualMap.arrayMovementBW.Length)
            {
                for (int rowCounter = 0; rowCounter < MAXMOVSIZE; rowCounter++)
                {
                    for (int columnCounter = 0; columnCounter < MAXMOVSIZE; columnCounter++)
                    {
                        if (movCounter < actualMap.arrayMovementBW.Length)
                        {
                            Maps.Move_s_bw actualMovement = actualMap.arrayMovementBW[movCounter];
                            if (actualMovement.par2 == 0x81)
                                actualTable.Rows[rowCounter].Cells[columnCounter].Style.BackColor = Color.Black;
                            else if (actualMovement.actualMov == 0 && actualMovement.par == 0)
                                actualTable.Rows[rowCounter].Cells[columnCounter].Value = " ";
                            else if (actualMovement.actualMov == 0 || actualMovement.actualMov == 31)
                                actualTable.Rows[rowCounter].Cells[columnCounter].Style.BackColor = Color.LightGreen;
                            else if (actualMovement.actualMov == 1)
                                actualTable.Rows[rowCounter].Cells[columnCounter].Style.BackColor = Color.LightGray;
                            else if (actualMovement.actualMov == 3)
                                actualTable.Rows[rowCounter].Cells[columnCounter].Style.BackColor = Color.SandyBrown;
                            else if (actualMovement.actualMov == 4 || actualMovement.actualMov == 6)
                                actualTable.Rows[rowCounter].Cells[columnCounter].Style.BackColor = Color.ForestGreen;
                            else if (actualMovement.actualMov == 63)
                                actualTable.Rows[rowCounter].Cells[columnCounter].Style.BackColor = Color.CornflowerBlue;
                            if (number)
                                if (layer == 0)
                                    actualTable.Rows[rowCounter].Cells[columnCounter].Value = actualMovement.actualMov;
                                else if (layer == 1)
                                    actualTable.Rows[rowCounter].Cells[columnCounter].Value = actualMovement.par;
                                else if (layer == 2)
                                    actualTable.Rows[rowCounter].Cells[columnCounter].Value = actualMovement.par2;
                                else if (layer == 3)
                                    actualTable.Rows[rowCounter].Cells[columnCounter].Value = actualMovement.actualFlag;

                            movCounter++;
                        }
                    }
                }
            }
            return actualTable;
        }

        private DataGridView PopTableOther(Maps actualMap, bool number, ref DataGridView actualTable, int layer, int location)
        {
            int movCounter = 0;
            while (movCounter < actualMap.arrayMovement.Length)
            {
                for (int rowCounter = 0; rowCounter < MAXMOVSIZE; rowCounter++)
                {
                    for (int columnCounter = location; columnCounter < MAXMOVSIZE *(location + 1); columnCounter++)
                    {
                        if (movCounter < actualMap.arrayMovement.Length)
                        {
                            Maps.Move_s actualMovement = actualMap.arrayMovement[movCounter];
                            if (actualMovement.actualFlag == 0x80)
                                actualTable.Rows[rowCounter].Cells[columnCounter].Style.BackColor = Color.LightGray;
                            else if (actualMovement.actualMov == 0)
                                actualTable.Rows[rowCounter].Cells[columnCounter].Style.BackColor = Color.LightGreen;
                            else if (actualMovement.actualMov == 2)
                                actualTable.Rows[rowCounter].Cells[columnCounter].Style.BackColor = Color.LawnGreen;
                            else if (actualMovement.actualMov == 3)
                                actualTable.Rows[rowCounter].Cells[columnCounter].Style.BackColor = Color.ForestGreen;
                            else if (actualMovement.actualMov == 16)
                                actualTable.Rows[rowCounter].Cells[columnCounter].Style.BackColor = Color.CornflowerBlue;
                            else if (actualMovement.actualMov == 21)
                                actualTable.Rows[rowCounter].Cells[columnCounter].Style.BackColor = Color.CornflowerBlue;
                            else if (actualMovement.actualMov == 33)
                                actualTable.Rows[rowCounter].Cells[columnCounter].Style.BackColor = Color.Yellow;
                            else if (actualMovement.actualMov == 59)
                                actualTable.Rows[rowCounter].Cells[columnCounter].Style.BackColor = Color.Red;
                            else if (actualMovement.actualMov == 0x69)
                                actualTable.Rows[rowCounter].Cells[columnCounter].Style.BackColor = Color.Brown;
                            else if (actualMovement.actualMov == 0xa9)
                                actualTable.Rows[rowCounter].Cells[columnCounter].Style.BackColor = Color.Snow;
                            else
                                actualTable.Rows[rowCounter].Cells[columnCounter].Style.BackColor = Color.White;
                            if (number)
                                if (layer == 0)
                                    actualTable.Rows[rowCounter].Cells[columnCounter].Value = actualMovement.actualMov;
                                else
                                    actualTable.Rows[rowCounter].Cells[columnCounter].Value = actualMovement.actualFlag;
                            movCounter++;
                        }
                    }
                }
            }
            return actualTable;
        }

        private DataGridView InitTable(ref DataGridView MovInfo, int location)
        {
            for (int movCounter = 0; movCounter < MAXMOVSIZE * (location + 1); movCounter++)
            {
                MovInfo.Columns.Add(movCounter.ToString(), movCounter.ToString());
                MovInfo.Rows.Add();
            }
            return MovInfo;
        }

        private void PopAssTable(Nsbmd model)
        {
            var dictShp = model.getMDL0at(0).shapeInfo.dictShp;
            var dictMat = model.getMDL0at(0).material.dictMat;
            for (int i = 0; i < model.assTableList.Count; i++)
            {

                var idActualPol = (byte)model.assTableList[i].pol;
                var idActualMat = (byte)model.assTableList[i].mat;
                if (idActualPol > dictShp.shapeNameList.Count || idActualMat > dictMat.matNameList.Count)
                    AssTable.Rows.Add(new object[] { 
                    "Wrong Id", "Wrong Id" });
                else
                    AssTable.Rows.Add(new object[] { 
                    //dictShp.shapeNameList[idActualPol].name + " " + 
                    idActualPol, 
                    dictMat.matNameList[idActualMat].name,
                    idActualMat });
            }
        }

        private void PopAssTableFromSbc()
        {
            NsbmdModel model = actualModel.getMDL0at(0);
            actualMap.associationTable = new List<Nsbmd.Table_s>();
            for (int i = 0; i < model.sequenceBoneCommandList.Count; i++)
            {
                if (model.sequenceBoneCommandList[i].Equals((byte)1))
                {
                    i++;
                }
                if (model.sequenceBoneCommandList[i].Equals((byte)2))
                {
                    i += 2;
                }
                else if (model.sequenceBoneCommandList[i].Equals((byte)0x4))
                {
                    Nsbmd.Table_s item = new Nsbmd.Table_s
                    {
                        pol = (byte)model.sequenceBoneCommandList[i + 3],
                        mat = (byte)model.sequenceBoneCommandList[i + 1]
                    };
                    object[] values = new object[] { //model.shapeInfo.dictShp.shapeNameList[(byte)model.sequenceBoneCommandList[i + 3]].name + " " + 
                        ((byte)model.sequenceBoneCommandList[i + 3]).ToString(), 
                        model.material.dictMat.matNameList[(byte)model.sequenceBoneCommandList[i + 1]].name,
                        ((byte)model.sequenceBoneCommandList[i + 1]).ToString() };
                    AssTable.Rows.Add(values);
                    actualMap.associationTable.Add(item);
                    i += 1;
                }
                else if (model.sequenceBoneCommandList[i].Equals((byte)0x26))
                {
                    i += 4;
                }
                else if (model.sequenceBoneCommandList[i].Equals((byte)0x24))
                {
                    i += 4;
                }
                else if (model.sequenceBoneCommandList[i].Equals((byte)0x44))
                {
                    i += 4;
                }
                else if (model.sequenceBoneCommandList[i].Equals((byte)0x66))
                {
                    i += 4;
                }
                else if (model.sequenceBoneCommandList[i].Equals((byte)6))
                {
                    i += 4;
                }
            }
        }

        public void PolyNumMax_ValueChanged(object sender, EventArgs e)
        {
            Console.Clear();
            if (actualModel == null)
                actualModel = actualMap.actualModel;
            PolyNumMax.Maximum = actualModel.getMDL0at(0).shapeInfo.shapeList.Count - 1;
            if (PolyNumMax.Value > -1)
            {
                showShape((int)PolyNumMax.Value);
                actualTextureName = VerValue.Rows[0].Cells[1].Value.ToString();
                functionList = Utils.popFunctionList(actualTextureName, functionList);

            }
            OpenGlControl.Invalidate();
        }

        private void showShape(int polyval)
        {
            renderer.polyval = polyval;


            VerValue.Rows.Clear();

            if (isPolygonInAssTable())
            {
                Console.AppendText("\nTexture is active: " + CheckText.Checked);

                setTextureVisibility();


                if (actualModel.actualTex != null)
                {
                    Console.AppendText("\nTexture file is loaded.");
                    fixMaterialAss();
                    updateAssTable();
                }

                if (showSingular.Checked)
                {
                    Console.AppendText("\nStart rendering(Full mode)");
                    renderer.setModel(actualModel, false);
                    renderer.renderMultipleAction();
                   // renderScene += new Action(renderer.renderMultipleAction);
                }
                else
                {
                    Console.AppendText("\nStart rendering(Singular mode)");
                    OpenGlControl.Invalidate();
                    renderer.setModel(actualModel, true);
                    renderer.renderSingularAction();
                    //renderScene += new Action(renderer.renderSingularAction);
                }

                popVerTable();
            }
        }

        private void setTextureVisibility()
        {
            if (CheckText.Checked)
                renderer.text_on = 1;
            else
                renderer.text_on = 0;
        }

        private Boolean isPolygonInAssTable()
        {
            Console.AppendText("\nSearching polygon in association Table...");
            Boolean findPol = false;
            if (actualModel.assTableList == null)
                actualModel.assTableList = actualMap.associationTable;
            for (int i = 0; i < actualModel.assTableList.Count && findPol == false; i++)
                if (renderer.polyval == actualModel.assTableList[i].pol)
                {
                    Console.AppendText("\nFinding polygon at " + i + " position.");
                    findPol = true;
                }
            return findPol;
        }

        #region PolyNumMax private

        private void popVerTable()
        {
            NsbmdModel.ShapeInfoStruct info = actualModel.getMDL0at(0).shapeInfo.shapeList[renderer.polyval];
            try
            {
                if (actualModel.matTexPalList != null && actualModel.matTexPalList.Count > 0)
                    VerValue = ShowModInfo(actualModel.matTexPalList[renderer.idActualMaterial].texName, info.commandList, VerValue);
                else
                    VerValue = ShowModInfo(null, info.commandList, VerValue);
            }
            catch (Exception e)
            {
            }
        }

        private void updateAssTable()
        {
            Boolean inserted = false;
            for (int listCounter = 0; listCounter < AssTable.Rows.Count && inserted == false; listCounter++)
            {
                if (((listCounter < actualModel.assTableList.Count) &&
                    (renderer.polyval == actualModel.assTableList[listCounter].pol))
                    && ((listCounter < AssTable.Rows.Count)
                    && (actualModel.assTableList[listCounter].mat < actualModel.matTexPalList.Count)))
                {
                    string textureName = actualModel.matTexPalList[actualModel.assTableList[listCounter].mat].texName.ToString();
                    string palName = actualModel.matTexPalList[actualModel.assTableList[listCounter].mat].palName.ToString();

                    int idTexture = -1;

                    StringBuilder strTexture = new StringBuilder();
                    strTexture.Append(textureName);
                    for (int i = textureName.Length; i < 16; i++)
                        strTexture.Append('\0');

                    for (int k = 0; k < actualModel.models[0].material.dictTex.texNameList.Count && idTexture == -1; k++)
                    {
                        if (strTexture.ToString() == actualModel.models[0].material.dictTex.texNameList[k].name)
                            idTexture = k;
                    }

                    int idPalette = -1;

                    StringBuilder strPalette = new StringBuilder();
                    strPalette.Append(palName);
                    for (int i = palName.Length; i < 16; i++)
                        strPalette.Append('\0');

                    for (int k = 0; k < actualModel.models[0].material.dictPal.palNameList.Count && idPalette == -1; k++)
                    {
                        if (strPalette.ToString() == actualModel.models[0].material.dictPal.palNameList[k].name)
                            idPalette = k;
                    }

                    AssTable.Rows[listCounter].Cells[3].Value = textureName;
                    AssTable.Rows[listCounter].Cells[4].Value = idTexture;
                    AssTable.Rows[listCounter].Cells[5].Value = palName;
                    AssTable.Rows[listCounter].Cells[6].Value = idPalette;


                    int polyval = renderer.polyval;
                    Array.Resize<int>(ref renderer.polMatList, renderer.polyval + 1);
                    renderer.idActualMaterial = actualModel.assTableList[listCounter].mat;
                    renderer.idActualPolygon = renderer.polyval;
                    renderer.polMatList[polyval] = renderer.idActualMaterial;
                    inserted = true;
                }
            }
        }

        private void fixMaterialAss()
        {
            int listCounter = 0;
            actualModel.matTexPalList = new List<NsbmdModel.MatTexPalStruct>();
            actualTex = actualModel.actualTex;
            NsbmdModel.MatTexPalStruct item = new NsbmdModel.MatTexPalStruct();
            List<NsbmdModel.MatTexPalStruct> list = actualModel.getMDL0at(0).matTexPalList;
            List<NsbmdModel.MatTexPalStruct> mat = actualTex.tex0.matTexPalList;
            for (listCounter = 0; listCounter < list.Count; listCounter++)
            {

                int findTexture = 0;
                int findPalette = 0;
                if (mat != null)
                {
                    int matCounter = 0;
                    while (matCounter < mat.Count)
                    {
                        string[] strArray = list[listCounter].texName.Split(new char[1]);
                        if ((findTexture == 0) && (mat[matCounter].texName == strArray[0]))
                        {
                            item = copyMaterials(item, mat, matCounter);
                            findTexture = 1;
                            break;
                        }
                        matCounter++;
                    }
                    for (matCounter = 0; matCounter < mat.Count; matCounter++)
                    {
                        if ((findPalette == 0) && (list[listCounter].palName.Length > 0))
                        {
                            string[] strArray2 = list[listCounter].palName.Split(new char[1]);
                            string str = strArray2[0].Substring(0, strArray2[0].Length - 2);
                            string str2 = "";
                            if (mat[matCounter].palName.Length > 4)
                                str2 = mat[matCounter].palName.Substring(0, mat[matCounter].palName.Length - 3);
                            if ((mat[matCounter].palName == strArray2[0]) || (str2 == strArray2[0]))
                            {
                                item = copyPalette(item, mat, matCounter);
                                findPalette = 1;
                                break;
                            }
                        }
                    }
                    if (item.palName == null)
                        item.palName = "";
                    if (item.texName == null)
                        item.texName = "";
                    actualModel.matTexPalList.Add(item);
                    listCounter = actualModel.getMaterials().Count - 1;
                }
            }
        }

        private static NsbmdModel.MatTexPalStruct copyPalette(NsbmdModel.MatTexPalStruct item, List<NsbmdModel.MatTexPalStruct> mat, int matCounter)
        {
            item.palName = mat[matCounter].palName;
            item.palData = mat[matCounter].palData;
            item.palOffset = mat[matCounter].palOffset;
            item.palSize = mat[matCounter].palSize;
            item.palMatId = mat[matCounter].palMatId;
            return item;
        }

        private static NsbmdModel.MatTexPalStruct copyMaterials(NsbmdModel.MatTexPalStruct item, List<NsbmdModel.MatTexPalStruct> mat, int matCounter)
        {
            item.format = mat[matCounter].format;
            item.colorBase = mat[matCounter].colorBase;
            item.heigth = mat[matCounter].heigth;
            item.width = mat[matCounter].width;
            item.flipS = mat[matCounter].flipS;
            item.flipT = mat[matCounter].flipT;
            item.repeatS = mat[matCounter].repeatS;
            item.repeatT = mat[matCounter].repeatT;
            item.repeat = mat[matCounter].repeat;
            item.texName = mat[matCounter].texName;
            item.texData = mat[matCounter].texData;
            item.texDataOffset = mat[matCounter].texDataOffset;
            item.texSize = mat[matCounter].texSize;
            item.textMatId = mat[matCounter].textMatId;
            item.spData = mat[matCounter].spData;
            return item;
        }

        #endregion

        #region Menu

        private void mapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != DialogResult.Cancel)
            {
                BinaryReader reader = new BinaryReader(dialog.OpenFile());
                actualMap = new Maps();
                actualMap.LoadMap(reader, true);
                actualModel = actualMap.actualModel;


                reactionMap(actualMap);
                reactionModel();


                showItem();

                OpenGLControl.Invalidate();
            }
        }

        private void mapToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (SaveType.Checked)
            {
                Save(0);
            }
            else
            {
                Save(1);
            }
        }

        public void Save(int t)
        {
            FileStream stream;
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if ((dialog.ShowDialog() == DialogResult.OK) && ((stream = (FileStream)dialog.OpenFile()) != null))
                SaveMap(t, stream);
        }

        private void SaveMap(int asNsbmd, FileStream stream)
        {
            int num;
            BinaryReader reader3;
            BinaryWriter writer = new BinaryWriter(stream);
            if (asNsbmd == 1)
            {
                BinaryReader reader4;
                if (Maps.type == DPMAP || Maps.type == PLMAP)
                {
                    writer.Write((int)actualMap.mapHeader.MovSize);
                    writer.Write((int)actualMap.mapHeader.ObjSize);
                    writer.Write((int)actualMap.mapHeader.BMDSize);
                    writer.Write((int)actualMap.mapHeader.BDHCSize);
                    actualMap.SaveMov(writer);
                    actualMap.SaveObj(writer);
                    actualMap.actualModel.SaveBMD0(writer);
                    reader4 = new BinaryReader(actualMap.streamBDHC);
                    reader4.BaseStream.Seek(0L, SeekOrigin.Begin);
                    for (num = 0; num < actualMap.streamBDHC.Length; num++)
                    {
                        writer.Write(reader4.ReadByte());
                    }
                }
                else if (Maps.type == HGSSMAP)
                {
                    writer.Write((int)actualMap.mapHeaderHG.MovSize);
                    writer.Write((int)actualMap.mapHeaderHG.ObjSize);
                    writer.Write((int)actualMap.mapHeaderHG.BMDSize);
                    writer.Write((int)actualMap.mapHeaderHG.BDHCSize);
                    BinaryReader reader5 = new BinaryReader(actualMap.streamUnknownSection);
                    reader5.BaseStream.Seek(0L, SeekOrigin.Begin);
                    for (num = 0; num < actualMap.streamUnknownSection.Length; num++)
                    {
                        writer.Write(reader5.ReadByte());
                    }
                    actualMap.SaveMov(writer);
                    actualMap.SaveObj(writer);
                    actualMap.actualModel.SaveBMD0(writer);
                    reader4 = new BinaryReader(actualMap.streamBDHC);
                    reader4.BaseStream.Seek(0L, SeekOrigin.Begin);
                    for (num = 0; num < actualMap.streamBDHC.Length; num++)
                    {
                        writer.Write(reader4.ReadByte());
                    }
                }
                else if (Maps.type == BWMAP)
                {
                    writer.Write((byte)0x57);
                    writer.Write((byte)0x42);
                    writer.Write((byte)3);
                    writer.Write((byte)0);
                    writer.Write((int)actualMap.mapHeaderBW.BMDOffset);
                    writer.Write((int)actualMap.mapHeaderBW.MovOffset);
                    writer.Write((int)actualMap.mapHeaderBW.ObjOffset);
                    writer.Write((int)actualMap.mapHeaderBW.BdhcOffset);
                    actualMap.actualModel.SaveBMD0(writer);
                    actualMap.SaveMov_Bw(writer);
                    actualMap.SaveObj_bw(writer);
                }
            }
            else if (asNsbmd == 0)
            {
                reader3 = new BinaryReader(actualMap.streamNSBMD);
                reader3.BaseStream.Seek(0L, SeekOrigin.Begin);
                for (num = 0; num < actualMap.streamNSBMD.Length; num++)
                {
                    writer.Write(reader3.ReadByte());
                }
            }
            writer.Flush();
            writer.Close();
            stream.Close();
        }

        private void modelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MapCreator().Show();
        }

        private void narcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Narc File (*.narc)|*.narc"
            };
            if (dialog.ShowDialog() != DialogResult.Cancel)
            {
                MapList.Items.Clear();
                MapList.Enabled = true;
                BinaryReader reader = new BinaryReader(dialog.OpenFile());
                if (Encoding.UTF8.GetString(reader.ReadBytes(4)) == "NARC")
                {
                    actualNarc = new Narc();
                    actualNarc.LoadNarc(reader);
                    IsBWDialog dialog2 = new IsBWDialog();
                    dialog2.ShowDialog();
                    modelType = dialog2.CheckGame();
                }
                GenerateMapList();
            }
        }

        private void nsbmdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() != DialogResult.Cancel)
            {
                var file = openDialog.OpenFile();
                BinaryReader reader = new BinaryReader(file);
                actualMap = new Maps();
                actualModel = new Nsbmd();
                actualModel.LoadBMD0(reader, 0);
                PolyNumMax.Enabled = true;
                reactionModel();
                PopAssTable(actualModel);
                showItem();
            }
        }

        #endregion

        private void Render()
        {
            Gl.glViewport(0, 0, Width, Height);
            var vp = new[] { 0f, 0f, 0f, 0f };
            Gl.glGetFloatv(Gl.GL_VIEWPORT, vp);
            float aspect = ((vp[2] - vp[0])) / ((vp[3] - vp[1]));
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
 
            var height = Math.Tan(45.0f / 2) * (0.12f + 32.0f) / 4;
            if (OrthoView.Checked)
                Gl.glOrtho(-height * aspect, height * aspect, -height, height, 0.12f, 32.0f);
            else
                Glu.gluPerspective(45.0f, aspect, 0.12f, 32.0f);
            if (renderScene != null)
            {
                renderScene();
            }
        }

        private void SaveObj_Click_3(object sender, EventArgs e)
        {
            int num;
            Maps.Obj_S j_s;
            if (Maps.type == BWMAP)
            {
                for (num = 1; num < ObjInfo.Rows.Count; num++)
                {
                    j_s = new Maps.Obj_S
                    {
                        idObject = Convert.ToInt32(ObjInfo.Rows[num].Cells[0].Value),
                        xBig = Convert.ToInt32(ObjInfo.Rows[num].Cells[1].Value),
                        xSmall = Convert.ToInt32(ObjInfo.Rows[num].Cells[2].Value),
                        yBig = Convert.ToInt32(ObjInfo.Rows[num].Cells[3].Value),
                        ySmall = Convert.ToInt32(ObjInfo.Rows[num].Cells[4].Value),
                        zBig = Convert.ToInt32(ObjInfo.Rows[num].Cells[5].Value),
                        zSmall = Convert.ToInt32(ObjInfo.Rows[num].Cells[6].Value),
                        heightObject = Convert.ToInt32(ObjInfo.Rows[num].Cells[7].Value),
                        lengthObject = Convert.ToInt32(ObjInfo.Rows[num].Cells[8].Value),
                        widthObject = Convert.ToInt32(ObjInfo.Rows[num].Cells[9].Value)
                    };
                    actualMap.listObjectsBW.RemoveAt(num);
                    actualMap.listObjectsBW.Insert(num, j_s);
                }
            }
            else
            {
                for (num = 0; num < ObjInfo.Rows.Count; num++)
                {
                    j_s = new Maps.Obj_S
                    {
                        idObject = Convert.ToInt32(ObjInfo.Rows[num].Cells[0].Value),
                        xBig = Convert.ToInt32(ObjInfo.Rows[num].Cells[1].Value),
                        xSmall = Convert.ToInt32(ObjInfo.Rows[num].Cells[2].Value),
                        yBig = Convert.ToInt32(ObjInfo.Rows[num].Cells[3].Value),
                        ySmall = Convert.ToInt32(ObjInfo.Rows[num].Cells[4].Value),
                        zBig = Convert.ToInt32(ObjInfo.Rows[num].Cells[5].Value),
                        zSmall = Convert.ToInt32(ObjInfo.Rows[num].Cells[6].Value),
                        heightObject = Convert.ToInt32(ObjInfo.Rows[num].Cells[7].Value),
                        lengthObject = Convert.ToInt32(ObjInfo.Rows[num].Cells[8].Value),
                        widthObject = Convert.ToInt32(ObjInfo.Rows[num].Cells[9].Value)
                    };
                    actualMap.listObjects.RemoveAt(num);
                    actualMap.listObjects.Insert(num, j_s);
                }
            }
        }

        private void Show_Inc_CheckedChanged(object sender, EventArgs e)
        {
            if (showSingular.Checked)
            {
                PolyNumMax.Value = -1M;
            }
        }

        public static DataGridView ShowModInfo(string texture, List<NsbmdModel.CommandStruct> commandList, DataGridView outputTable)
        {
            String actualTextureName = texture;
            if ((actualTextureName != null))
                outputTable = Utils.showSpecificInfo(commandList, outputTable, actualTextureName);
            return outputTable;
        }

        private void SavePol_Click(object sender, EventArgs e)
        {
            if (PolyNumMax.Value < 0)
                return;
            var shapeInfo = actualModel.getMDL0at(0).shapeInfo.shapeList[(int)PolyNumMax.Value];
            var originalLength = shapeInfo.commandList.Count;
            var commandList = new List<NsbmdModel.CommandStruct>();
            int rowCounter = 0;
            addFirstCommand(commandList);
            rowCounter = 2;
            while (rowCounter < VerValue.Rows.Count && VerValue.Rows[rowCounter].Cells[2].Value != null)
            {
                var actualRow = VerValue.Rows[rowCounter];
                var nameCommand = actualRow.Cells[2].Value.ToString();
                var actualCommand = new NsbmdModel.CommandStruct();
                Double x, y, z, xReal, yReal, zReal;
                Console.AppendText("\n" + nameCommand);
                switch (nameCommand)
                {
                    case "Mtx_Restore":
                        {
                            actualCommand.id = (Byte)0x14;
                            actualCommand.x = Int32.Parse(actualRow.Cells[3].Value.ToString(), System.Globalization.NumberStyles.Float);
                            commandList.Add(actualCommand);
                            break;
                        }
                    case "Mtx_Scale":
                        {
                            actualCommand.id = (Byte)0x1B;
                            actualCommand.x = Int32.Parse(actualRow.Cells[3].Value.ToString(), System.Globalization.NumberStyles.Float);
                            actualCommand.z = Int32.Parse(actualRow.Cells[4].Value.ToString(), System.Globalization.NumberStyles.Float);
                            actualCommand.y = Int32.Parse(actualRow.Cells[5].Value.ToString(), System.Globalization.NumberStyles.Float);
                            commandList.Add(actualCommand);
                            break;
                        }
                    case "Colors":
                        {
                            actualCommand.id = (Byte)0x20;

                            x = Double.Parse(actualRow.Cells[3].Value.ToString(), System.Globalization.NumberStyles.Float);
                            z = Double.Parse(actualRow.Cells[4].Value.ToString(), System.Globalization.NumberStyles.Float);
                            y = Double.Parse(actualRow.Cells[5].Value.ToString(), System.Globalization.NumberStyles.Float);
                            xReal = x * 256f;
                            yReal = y * 256f;
                            zReal = z * 256f;
                            actualCommand.par = (int)(xReal + yReal + zReal);
                            actualCommand.x = (int)xReal;
                            actualCommand.y = (int)yReal;
                            actualCommand.z = (int)zReal;
                            //actualCommand.par = Int32.Parse(actualRow.Cells[5].Value.ToString(),System.Globalization.NumberStyles.Float);
                            commandList.Add(actualCommand);
                            break;
                        }
                    case "Normal":
                        {
                            actualCommand.id = (Byte)0x21;

                            x = Double.Parse(actualRow.Cells[3].Value.ToString(), System.Globalization.NumberStyles.Float);
                            z = Double.Parse(actualRow.Cells[4].Value.ToString(), System.Globalization.NumberStyles.Float);
                            y = Double.Parse(actualRow.Cells[5].Value.ToString(), System.Globalization.NumberStyles.Float);
                            xReal = ((int)(x / 4) << 4);
                            yReal = ((int)(y / 4) << 4) * 1024;
                            zReal = ((int)(z / 4) << 4) * 1024 * 1024;
                            actualCommand.par = (int)(xReal + yReal + zReal);
                            actualCommand.x = (int)xReal;
                            actualCommand.y = (int)yReal;
                            actualCommand.z = (int)zReal;
                            //actualCommand.par= Int32.Parse(actualRow.Cells[5].Value.ToString(),System.Globalization.NumberStyles.Float);
                            commandList.Add(actualCommand);
                            break;
                        }
                    case "Texture":
                        {
                            actualCommand.id = (Byte)0x22;

                            x = Double.Parse(actualRow.Cells[3].Value.ToString(), System.Globalization.NumberStyles.Float);
                            y = Double.Parse(actualRow.Cells[4].Value.ToString(), System.Globalization.NumberStyles.Float);
                            xReal = x * 256;
                            yReal = y * 256 * 65536;
                            actualCommand.par = (int)(xReal + yReal);
                            actualCommand.x = (int)xReal;
                            actualCommand.y = (int)yReal;
                            //actualCommand.par = Int32.Parse(actualRow.Cells[5].Value.ToString(),System.Globalization.NumberStyles.Float);
                            commandList.Add(actualCommand);
                            break;
                        }
                    case "Vtx_16":
                        {
                            actualCommand.id = (Byte)0x23;

                            x = Double.Parse(actualRow.Cells[3].Value.ToString(), System.Globalization.NumberStyles.Float);
                            z = Double.Parse(actualRow.Cells[4].Value.ToString(), System.Globalization.NumberStyles.Float);
                            y = Double.Parse(actualRow.Cells[5].Value.ToString(), System.Globalization.NumberStyles.Float);
                            xReal = x * 256;
                            yReal = y * 256;
                            zReal = z * 256;
                            actualCommand.par = (int)(xReal + ((int)yReal << 16));
                            actualCommand.par2 = (int)zReal;
                            actualCommand.x = (int)xReal;
                            actualCommand.y = (int)yReal;
                            actualCommand.z = (int)zReal;
                            //actualCommand.par = Int32.Parse(actualRow.Cells[5].Value.ToString(), System.Globalization.NumberStyles.Float);
                            //actualCommand.par2 = Int32.Parse(actualRow.Cells[6].Value.ToString(), System.Globalization.NumberStyles.Float);
                            commandList.Add(actualCommand);
                            break;
                        }
                    case "Vtx_10":
                        {
                            actualCommand.id = (Byte)0x24;
                            x = Double.Parse(actualRow.Cells[3].Value.ToString(), System.Globalization.NumberStyles.Float);
                            z = Double.Parse(actualRow.Cells[4].Value.ToString(), System.Globalization.NumberStyles.Float);
                            y = Double.Parse(actualRow.Cells[5].Value.ToString(), System.Globalization.NumberStyles.Float);
                            xReal = ((int)(x / 4f) << 4);
                            yReal = ((int)(y / 4f) << 14);
                            zReal = ((int)(z / 4f) << 4) * 1024 * 1024;
                            actualCommand.par = (int)(xReal + yReal + zReal);
                            actualCommand.x = (int)xReal;
                            actualCommand.y = (int)yReal;
                            actualCommand.z = (int)zReal;
                            //actualCommand.par = Int32.Parse(actualRow.Cells[5].Value.ToString(), System.Globalization.NumberStyles.Float);
                            commandList.Add(actualCommand);
                            break;
                        }
                    case "Vtx_XY":
                        {
                            actualCommand.id = (Byte)0x25;

                            x = (int)Double.Parse(actualRow.Cells[3].Value.ToString(), System.Globalization.NumberStyles.Float);
                            y = (int)Double.Parse(actualRow.Cells[5].Value.ToString(), System.Globalization.NumberStyles.Float);
                            xReal = x * 256;
                            yReal = y * 256;
                            actualCommand.par = (int)(xReal + ((int)yReal << 16));
                            actualCommand.x = (int)xReal;
                            actualCommand.y = (int)yReal;
                            //   actualCommand.par= (int).Parse(actualRow.Cells[5].Value.ToString(), System.Globalization.NumberStyles.Float);
                            commandList.Add(actualCommand);
                            break;
                        }
                    case "Vtx_XZ":
                        {
                            actualCommand.id = (Byte)0x26;

                            x = (int)Double.Parse(actualRow.Cells[3].Value.ToString(), System.Globalization.NumberStyles.Float);
                            z = (int)Double.Parse(actualRow.Cells[4].Value.ToString(), System.Globalization.NumberStyles.Float);
                            xReal = x * 256;
                            zReal = z * 256;
                            actualCommand.par = (int)(xReal + ((int)zReal << 16));
                            actualCommand.x = (int)xReal;
                            actualCommand.z = (int)zReal;
                            //actualCommand.par = Int32.Parse(actualRow.Cells[5].Value.ToString(), System.Globalization.NumberStyles.Float);
                            commandList.Add(actualCommand);
                            break;
                        }
                    case "Vtx_YZ":
                        {
                            actualCommand.id = (Byte)0x27;

                            y = (int)Double.Parse(actualRow.Cells[5].Value.ToString(), System.Globalization.NumberStyles.Float);
                            z = (int)Double.Parse(actualRow.Cells[4].Value.ToString(), System.Globalization.NumberStyles.Float);
                            yReal = y * 256;
                            zReal = z * 256;
                            actualCommand.par = (int)(yReal + ((int)zReal << 16));
                            actualCommand.y = (int)yReal;
                            actualCommand.z = (int)zReal;
                            //actualCommand.par = Int32.Parse(actualRow.Cells[5].Value.ToString(), System.Globalization.NumberStyles.Float);
                            commandList.Add(actualCommand);
                            break;
                        }
                    case "Vtx_Diff":
                        {
                            actualCommand.id = (Byte)0x28;

                            x = Double.Parse(actualRow.Cells[3].Value.ToString(), System.Globalization.NumberStyles.Float);
                            z = Double.Parse(actualRow.Cells[4].Value.ToString(), System.Globalization.NumberStyles.Float);
                            y = Double.Parse(actualRow.Cells[5].Value.ToString(), System.Globalization.NumberStyles.Float);
                            xReal = ((int)(x / 4f) << 4);
                            yReal = ((int)(y / 4f) << 14);
                            zReal = ((int)(z / 4f) << 4) * 1024 * 1024;
                            actualCommand.par = (int)(xReal + yReal + zReal);
                            actualCommand.x = (int)xReal;
                            actualCommand.y = (int)yReal;
                            actualCommand.z = (int)zReal;
                            // actualCommand.par = Int32.Parse(actualRow.Cells[6].Value.ToString(), System.Globalization.NumberStyles.Float);
                            commandList.Add(actualCommand);
                            break;
                        }
                    case "Start":
                        {
                            actualCommand.id = (Byte)0x40;
                            actualCommand.par = Int32.Parse(actualRow.Cells[6].Value.ToString(), System.Globalization.NumberStyles.Float);
                            commandList.Add(actualCommand);
                            break;
                        }
                    case "End":
                        {
                            actualCommand.id = (Byte)0x41;
                            commandList.Add(actualCommand);
                            break;
                        }
                    case "0":
                        {
                            actualCommand.id = (Byte)0x0;
                            commandList.Add(actualCommand);
                        }
                        break;
                    case "0,00":
                        {
                            actualCommand.id = (Byte)0x0;
                            commandList.Add(actualCommand);
                        }
                        break;
                }
                rowCounter++;
            }


            shapeInfo.commandList = commandList;
            actualModel.models[0].shapeInfo.shapeList.RemoveAt((int)PolyNumMax.Value);
            actualModel.models[0].shapeInfo.shapeList.Insert((int)PolyNumMax.Value, shapeInfo);
            actualMap.actualModel = actualModel;

        }

        private void addFirstCommand(List<NsbmdModel.CommandStruct> commandList)
        {
            var actualCommand = new NsbmdModel.CommandStruct();
            actualCommand.id = 0x40;
            var firstCell = VerValue.Rows[1].Cells[1].Value.ToString();
            if (firstCell == "Quads")
                actualCommand.par = (Int32)0x0;
            else if (firstCell == "Quads_Strip")
                actualCommand.par = (Int32)0x1;
            else if (firstCell == "Triangle_Strip")
                actualCommand.par = (Int32)0x2;
            else if (firstCell == "Triangle")
                actualCommand.par = (Int32)0x3;
            commandList.Add(actualCommand);
        }

        //private void ShowObj_ValueChanged(object sender, EventArgs e)
        //{
        //    int num = (int)  ShowObj.Value;
        //    object obj2 =  ObjInfo.Rows[num].Cells[0].Value;
        //    Mod_Narc = new Narc();
        //    BinaryReader reader = new BinaryReader(file);
        //    reader.BaseStream.Seek(0L, SeekOrigin.Begin);
        //    if (Mod_Narc.Con == 0)
        //    {
        //        reader.ReadInt32();
        //        Mod_Narc.LoadNarc(reader);
        //         Render_Scene += new Action(renderer.RenderFunc2);
        //    }
        //}

        private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        private void simpleOpenGlControl1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D:
                    inScrollRangeLoop(ref  trackBarRotX, renderer.cKeyPressInc);
                    trackBarAng_Scroll(sender, null);
                    break;

                case Keys.S:
                    inScrollRangeLoop(ref  trackBarRotY, renderer.cKeyPressInc);
                    trackBarElev_Scroll(sender, null);
                    break;

                case Keys.W:
                    inScrollRangeLoop(ref  trackBarRotY, renderer.cKeyPressInc);
                    trackBarElev_Scroll(sender, null);
                    break;

                case Keys.Up:
                    inScrollRangeLoop(ref  trackBarTransZ, renderer.cKeyPressInc);
                    trackBarDist_Scroll(sender, null);
                    break;

                case Keys.Down:
                    inScrollRangeLoop(ref  trackBarTransZ, -renderer.cKeyPressInc);
                    trackBarDist_Scroll(sender, null);
                    break;

                case Keys.A:
                    inScrollRangeLoop(ref  trackBarRotX, -renderer.cKeyPressInc);
                    trackBarAng_Scroll(sender, null);
                    break;
            }
        }

        //public void startPicking(int cursorX, int cursorY)
        //{
        //    int[] buffer = new int[0x200];
        //    int[] params = new int[4];
        //    Gl.glSelectBuffer(0x200, buffer);
        //    Gl.glRenderMode(0x1c02);
        //    Gl.glMatrixMode(0x1701);
        //    Gl.glPushMatrix();
        //    Gl.glLoadIdentity();
        //    Gl.glGetIntegerv(0xba2, params);
        //    Glu.gluPickMatrix((double) cursorX, (double) (params[3] - cursorY), 5.0, 5.0, params);
        //    Glu.gluPerspective(45.0, 1.0, 0.1, 1000.0);
        //    Gl.glMatrixMode(0x1700);
        //    Gl.glInitNames();
        //}



        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Console.AppendText("\nScroll = " + trackBarTransX.Value.ToString());
            OpenGlControl.Invalidate();
        }


        private void trackBarTrY_Scroll(object sender, EventArgs e)
        {
            OpenGlControl.Invalidate();
        }

        private void trackBarAng_Scroll(object sender, EventArgs e)
        {
            OpenGlControl.Invalidate();
        }

        private void trackBarDist_Scroll(object sender, EventArgs e)
        {
            Console.AppendText("\nZoom = " + trackBarTransZ.Value.ToString());
            OpenGlControl.Invalidate();
        }

        private void trackBarElev_Scroll(object sender, EventArgs e)
        {
            OpenGlControl.Invalidate();
        }

        private void trackBarRotZ_Scroll(object sender, EventArgs e)
        {
            OpenGlControl.Invalidate();
        }



        private void x6LockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MovInfo.Rows.Count; i++)
            {
                for (int j = 0; j < MovInfo.Rows[i].Cells.Count; j++)
                {
                    if (MovInfo.Rows[i].Cells[j].Selected)
                    {
                        MovInfo.Rows[i].Cells[j].Value = 6;
                        MovInfo.Rows[i].Cells[j].Style.BackColor = Color.Yellow;
                    }
                }
            }
        }

        private void x8CaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MovInfo.Rows.Count; i++)
            {
                for (int j = 0; j < MovInfo.Rows[i].Cells.Count; j++)
                {
                    if (MovInfo.Rows[i].Cells[j].Selected)
                    {
                        MovInfo.Rows[i].Cells[j].Value = 8;
                        MovInfo.Rows[i].Cells[j].Style.BackColor = Color.Brown;
                    }
                }
            }
        }

        public SimpleOpenGlControl OpenGLControl
        {
            get
            {
                return OpenGlControl;
            }
        }

        private void Editor_Click(object sender, EventArgs e)
        {
            var mapCreator = new MapCreator(actualMap);
            mapCreator.Show();

        }

        private void modelToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            if (saveDialog.ShowDialog() != DialogResult.Cancel)
            {
                var file = saveDialog.OpenFile();
                var writer = new BinaryWriter(file);
                actualModel.padOffset = 0;
                actualModel.SaveBMD0(writer);
                writer.Close();
            }


        }

        private void Rem_Poly_Click(object sender, EventArgs e)
        {
            Int32 idPolygon = (Int32)PolyNumMax.Value;
            if (idPolygon == -1)
            {
                MessageBox.Show("You can't remove negative polygon!");
                return;
            }

            actualModel = actualModel.removePolygon(idPolygon);

            if (actualMap != null)
            {
                if (Maps.type == DPMAP || Maps.type == PLMAP)
                {
                    //actualMap.size -= actualModel.removedSize;
                    ////actualMap.mapHeader.BdhcOffset -= (uint)actualModel.removedSize;
                    actualMap.mapHeader.BMDSize = (uint)actualModel.sizeBMD0;
                    //actualMap.mapHeader.BdhcOffset = actualMap.mapHeader.BMDOffset + actualMap.mapHeader.BMDSize;  
                    actualMap.actualModel = actualModel;
                }
                else if (Maps.type == NSBMD_MODEL)
                    if (actualModel.sectionNum == 2)
                        actualModel.sectionData[1] = actualModel.models[0].blockSize + 0x18;
            }

        }


        private void AddPol_Click(object sender, EventArgs e)
        {
            int newIdPolygon = actualModel.models[0].shapeInfo.dictShp.shapeNum;
            SetInfoNewPol infoNewPol = new SetInfoNewPol(actualModel.actualTex);
            infoNewPol.ShowDialog();
            actualModel = actualModel.addPolygon(newIdPolygon, infoNewPol.getSize(), infoNewPol.getTexture(), infoNewPol.getPalette());
            if (actualMap.size != 0)
            {
                actualMap.size += actualModel.addedSize;
                if (Maps.type == DPMAP || Maps.type == PLMAP)
                {
                    actualMap.mapHeader.BdhcOffset += (uint)actualModel.addedSize;
                    actualMap.mapHeader.BMDSize = (uint)actualModel.sizeBMD0;
                }
                else if (Maps.type == BWMAP)
                {
                    actualMap.mapHeaderBW.MovOffset += (uint)actualModel.addedSize;
                    actualMap.mapHeaderBW.ObjOffset += (uint)actualModel.addedSize;
                    actualMap.mapHeader.BMDSize = (uint)actualModel.sizeBMD0;
                }

                actualMap.actualModel = actualModel;
            }



        }

        private void SaveMat_Click(object sender, EventArgs e)
        {
            int rowCounter = 0;
            var sbc = new List<Nsbmd.Sbc_s>();
            foreach (Nsbmd.Sbc_s oldSbcCommand in actualModel.sbc)
            {
                if (oldSbcCommand.id != 0x4 && oldSbcCommand.id != 0x5)
                    sbc.Add(oldSbcCommand);
                else if (oldSbcCommand.id == 0x4)
                {
                    var newSbcCommand = new Nsbmd.Sbc_s();
                    var actualRow = AssTable.Rows[rowCounter];
                    var newTexture = actualRow.Cells[3].Value.ToString();
                    var newMaterial = actualRow.Cells[1].Value.ToString();


                    //foreach( NsbmdModel.MatTexPalStruct matTex in actualTex.tex0.matTexPalList)
                    //    if (matTex.texName == newTexture)
                    //        actualRow.Cells[5].Value = matTex.palName;

                    var newPalette = actualRow.Cells[5].Value.ToString();

                    newSbcCommand.id = 0x4;
                    newSbcCommand.parameters = new List<Byte>();

                    //if (DuplicateName(newString)){
                    //    MessageBox.Show("Polygon Name already present!" + rowCounter);
                    //    return;
                    //}
                    //var newTexture = newTextureString.ToString().Split(' ')[0];
                    //var newMaterial = newMaterialString.ToString().Split(' ')[0];

                    StringBuilder strTexture = new StringBuilder();
                    strTexture.Append(newTexture);
                    for (int i = newTexture.Length; i < 16; i++)
                        strTexture.Append('\0');

                    StringBuilder strMaterial = new StringBuilder();
                    strMaterial.Append(newMaterial);
                    for (int i = newMaterial.Length; i < 16; i++)
                        strMaterial.Append('\0');


                    StringBuilder strPalette = new StringBuilder();
                    strPalette.Append(newPalette);
                    for (int i = newPalette.Length; i < 16; i++)
                        strPalette.Append('\0');

                    var newTextureValue = Byte.Parse(actualRow.Cells[4].Value.ToString());
                    var newMaterialValue = Byte.Parse(actualRow.Cells[2].Value.ToString());
                    var newPaletteValue = Byte.Parse(actualRow.Cells[6].Value.ToString());

                    var oldTexture = actualModel.models[0].material.dictTex.texNameList[newTextureValue];
                    oldTexture.name = strTexture.ToString();
                    actualModel.models[0].material.dictTex.texNameList[newTextureValue] = oldTexture;

                    var oldMaterial = actualModel.models[0].material.dictMat.matNameList[newMaterialValue];
                    oldMaterial.name = strMaterial.ToString();
                    actualModel.models[0].material.dictMat.matNameList[newMaterialValue] = oldMaterial;

                    var oldPalette = actualModel.models[0].material.dictPal.palNameList[newPaletteValue];
                    oldPalette.name = strPalette.ToString();
                    actualModel.models[0].material.dictPal.palNameList[newPaletteValue] = oldPalette;

                    //UpdateShapeInfo(newString);

                    newSbcCommand.parameters.Add(newMaterialValue);
                    sbc.Add(newSbcCommand);

                }
                else if (oldSbcCommand.id == 0x5)
                {
                    var newSbcCommand = new Nsbmd.Sbc_s();
                    var actualRow = AssTable.Rows[rowCounter];
                    var newString = actualRow.Cells[0].Value;

                    newSbcCommand.id = 0x5;
                    newSbcCommand.parameters = new List<Byte>();

                    //if (DuplicateName(newString)){
                    //    MessageBox.Show("Polygon Name already present!" + rowCounter);
                    //    return;
                    //}

                    var newValue = Byte.Parse(newString.ToString());

                    //UpdateShapeInfo(newString);

                    newSbcCommand.parameters.Add(newValue);
                    sbc.Add(newSbcCommand);

                    rowCounter++;

                }
            }
            actualModel.sbc = sbc;
        }

        private void ResetPoly_Click(object sender, EventArgs e)
        {
            Int32 idPolygon = (Int32)PolyNumMax.Value;
            if (idPolygon == -1)
            {
                MessageBox.Show("You can't reset negative polygon!");
                return;
            }

            actualModel = actualModel.resetPolygon(idPolygon);

            VerValue.Rows.Clear();
            popVerTable();
        }

        private void ResizePol_Click(object sender, EventArgs e)
        {

            Int32 idPolygon = (Int32)PolyNumMax.Value;
            if (idPolygon == -1)
            {
                MessageBox.Show("You can't resize negative polygon!");
                return;
            }

            var model = actualModel.models[0];
            var shapeInfo = model.shapeInfo;
            int oldSize = shapeInfo.shapeList[idPolygon].sizeDL;

            var changeSize = new ChangeSize(oldSize);
            changeSize.ShowDialog();
            if (changeSize.newSizeT.Text == "")
                return;
            int size = changeSize.getNewSize();
            actualModel = actualModel.resizePolygon(idPolygon, size);
            if (Maps.type != NSBMD_MODEL)
            {
                actualMap.actualModel = actualModel;
                if (Maps.type == DPMAP || Maps.type == PLMAP)
                    actualMap.mapHeader.BMDSize = (uint)actualModel.sizeBMD0;
            }


            VerValue.Rows.Clear();
            popVerTable();
        }


        private void simpleOpenGlControl1_Select(object sender, EventArgs e)
        {
            //int[] viewport = new int[4];   
            //uint[] selectBuf = new uint[512];

            //Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);
            //Gl.glSelectBuffer(512, selectBuf); 
            ////Gl.glRenderMode(Gl.GL_SELECT);
            //Gl.glInitNames();
            //Gl.glPushName(0);
            //Gl.glPopName();

            //Gl.glMatrixMode(Gl.GL_PROJECTION);
            //Gl.glPushMatrix();                             // Push The Projection Matrix
            //Gl.glLoadIdentity();
            //Glu.gluPickMatrix((double)MousePosition.X, (double)(viewport[3] - MousePosition.Y), 1.0f, 1.0f, viewport);

            //Glu.gluPerspective(45.0f, (float)(viewport[2] - viewport[0]) / (float)(viewport[3] - viewport[1]), 0.1f, 100.0f);

            //Gl.glMatrixMode(Gl.GL_MODELVIEW);                          
            //Gl.glMatrixMode(Gl.GL_PROJECTION);  
            //Gl.glPopMatrix();                              // Pop The Projection Matrix

            //Gl.glMatrixMode(Gl.GL_MODELVIEW);                     // Select The Modelview Matrix

            //hits = Gl.glRenderMode(Gl.GL_RENDER);
            //Console.Clear();
            //Console.AppendText("\n" + hits);
            ////ShowItem();
        }

        private void CheckText_CheckedChanged(object sender, EventArgs e)
        {
            if (PolyNumMax.Value != -1)
            {
                OpenGlControl.Invalidate();
                showShape((int)PolyNumMax.Value);
            }
        }

        private void functionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var texture = VerValue.Rows[0].Cells[1].Value.ToString();
            var mapCreator = new MapCreator(texture, functionList.SelectedItem.ToString(), actualMap.actualModel.getMDL0at(0).shapeInfo.shapeList[(int)PolyNumMax.Value]);
            mapCreator.Show();
        }

        private void modelIMDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            if (saveDialog.ShowDialog() != DialogResult.Cancel)
            {
                var file = saveDialog.OpenFile();
                var writer = new StreamWriter(file);
                actualModel.padOffset = 0;
                actualModel.SaveIMD0(writer);
                writer.Close();
            }
        }

        private void Layer_1_Click(object sender, EventArgs e)
        {

        }






        #endregion

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
            else if ((Title == "POKEMON W\0\0\0") || (Title == "POKEMON B\0\0\0")|| (Title == "POKEMON W2\0\0") || (Title == "POKEMON B2\0\0"))
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
            AssMap_T.Rows.Clear();

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
            short mapMatrixPosition = getMapNamePosition(type);
            mapMatrixStream = getStream(mapMatrixPosition);
            short scriptPosition = getScriptPosition(type);
            scriptStream = getStream(scriptPosition);

            //Temp_File = new List<ClosableMemoryStream>();
            //Temp_File.Add(mapMatrixStream);

            var reader3 = new BinaryReader(mapMatrixStream);
            reader3.BaseStream.Seek(0L, SeekOrigin.Begin);
            reader.BaseStream.Seek((long)offset, SeekOrigin.Begin);

            AssMap_T.Rows.Clear();

            if (type == 0 || type == 1)
                readTableDPP(MapTex, num, reader, reader3);
            else if (type == 2)
                readTableHGSS(MapTex, num, reader, reader3);
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
            mapListData = new Dictionary<int, string>();
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
            if (item.mapTerrainId == MapList.SelectedIndex.ToString())
                AssMap_T.Rows.Add(new object[] { item.mapId, "-", item.tex_ext, item.tex_int, item.map_matrix, item.scripts, item.trainers, item.texts, item.musicA, item.musicB, item.encount, item.events, item.name, item.weather, item.cameraAngle, item.nameStyle, item.flag, item.mapTerrainId });
            MapTex.Add(item);
        }

        private void readTableDPP(List<tableRow> MapTex, int num, BinaryReader reader, BinaryReader reader3)
        {
            mapListData = new Dictionary<int, string>();
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
            item.name = getLocationName(reader.ReadByte());
            reader.ReadByte();
            item.weather = reader.ReadByte();
            item.cameraAngle = reader.ReadByte();
            item.nameStyle = reader.ReadByte();
            item.flag = reader.ReadByte();
            item.mapTerrainId = getTerrainData(item.map_matrix, id, item.name);


            string str = Encoding.UTF8.GetString(reader3.ReadBytes(0x10));
            if (item.mapTerrainId.Contains(" " + headerId + " "))
                AssMap_T.Rows.Add(new object[] { id, str, item.tex_ext, item.tex_int, item.map_matrix,item.scripts, item.trainers, item.texts, item.musicA, item.musicB, item.encount, item.events, item.name, item.weather, item.cameraAngle, item.nameStyle, item.flag, item.mapTerrainId });
            MapTex.Add(item);
        }


        private string getLocationName(byte id)
        {
            if (textHandler == null)
            {
                var Title = main.actualNds.getHeader().getTitle();
                Stream textStream = null;
                int textId = 0;
                if (Title == "POKEMON P\0\0\0" || Title == "POKEMON D\0\0\0")
                {
                    textStream = main.actualNds.getFat().getFileStreamAt(Int16.Parse(main.Sys.Nodes[0].Nodes[10].Nodes[1].Tag.ToString()));
                    textId = 382;
                }
                else if (Title == "POKEMON PL\0\0")
                {
                    textStream = main.actualNds.getFat().getFileStreamAt(Int16.Parse(main.Sys.Nodes[0].Nodes[12].Nodes[2].Tag.ToString()));
                    textId = 433;
                }
                else if (Title == "POKEMON HG\0\0" || Title == "POKEMON SS\0\0")
                {
                    textStream = main.actualNds.getFat().getFileStreamAt(Int16.Parse(main.Sys.Nodes[0].Nodes[0].Nodes[0].Nodes[2].Nodes[7].Tag.ToString()));
                    textId = 279;
                }
                else if (Title == "POKEMON B\0\0\0" || Title == "POKEMON W\0\0\0" || Title == "POKEMON B2\0\0" || Title == "POKEMON W2\0\0")
                {
                    textStream = main.actualNds.getFat().getFileStreamAt(Int16.Parse(main.Sys.Nodes[0].Nodes[0].Nodes[0].Nodes[0].Nodes[3].Tag.ToString()));
                    textId = 382;
                }
                var textNarc = new Narc().LoadNarc(new BinaryReader(textStream));
                var textFile = textNarc.figm.fileData[textId];
                int textType = 0;
                if (main.actualNds.getHeader().getTitle() == "POKEMON B\0\0\0" || main.actualNds.getHeader().getTitle() == "POKEMON W\0\0\0" || 
                    main.actualNds.getHeader().getTitle() == "POKEMON B2\0\0" || main.actualNds.getHeader().getTitle() == "POKEMON W2\0\0")
                    textType = 1;
                textHandler = new Texts(textFile, textType);
            }
            return id + " - " + textHandler.textList[id].text;

        }

        private void readTableHGSS(List<tableRow> MapTex, int num, BinaryReader reader, BinaryReader reader3)
        {
            mapListData = new Dictionary<int, string>();
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
            item.name = getLocationName(reader.ReadByte());
            item.encount = reader.ReadInt16();
            reader.ReadByte();
            item.weather = reader.ReadByte();
            item.cameraAngle = reader.ReadByte();
            item.nameStyle = reader.ReadByte();
            item.flag = reader.ReadByte();
            item.mapTerrainId = getTerrainData(item.map_matrix, id,item.name);
            string str = Encoding.UTF8.GetString(reader3.ReadBytes(0x10));

            if (item.mapTerrainId.Contains(" " + headerId + " "))
                AssMap_T.Rows.Add(new object[] { id, str, item.tex_ext, item.tex_int, item.map_matrix, item.scripts, item.trainers, item.texts, item.musicA, item.musicB, item.encount, item.events, item.name, item.weather, item.cameraAngle, item.nameStyle, item.flag, item.mapTerrainId });
            MapTex.Add(item);
        }

        private void AssMap_T_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var columnIndex = AssMap_T.SelectedCells[0].ColumnIndex;
            var cellValue = AssMap_T.SelectedCells[0].Value;
            var headerName = AssMap_T.Columns[columnIndex].HeaderText;
            if (headerName == "Scripts")
                showScript();
            else if (headerName == "Matrix")
                showMatrix(matrixHeaderInfo, matrixBorderInfo, matrixTerrainInfo);
            else if (headerName == "Events")
                showEvent();
            else if (headerName == "Texts")
                showScript();


        }


        private void subScripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            scriptViewer.printSimplifiedScript(scriptBoxEditor, scriptBoxViewer, romType, subScripts.SelectedItem.ToString());
        }

        private void OrthoView_CheckedChanged(object sender, EventArgs e)
        {
            if (PolyNumMax.Value != -1)
            {
                OpenGlControl.Invalidate();
                showShape((int)PolyNumMax.Value);
            }
        }

      
    


    }
}