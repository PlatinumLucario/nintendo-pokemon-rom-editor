using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NPRE.Commons;
using PG4Map.Formats;
using System.Windows.Forms;

namespace NPRE.Formats.Specific.Pokémon.Scripts
{
    public class DPPCommandsHandler
    {
        public int varLevel = 0;
        private short scriptType;
        public List<uint> movOffsetList;
        public List<uint> functionOffsetList;

        public List<String> mulString = new List<String>();
        public List<string> conditionList = new List<string>();
        public List<string> operatorList = new List<string>();
        public List<string> blockList = new List<string>();
        public List<Dictionary<int, string>> varNameDictionary;
        private string temp2;
        private string conditionType = "N";
        private string cond;
        private bool mulActive;

        public Texts multiFile;
        public Texts textFile;
        public PG4Map.Narc textNarc;
        public PG4Map.Narc bwTextNarc;
        public RichTextBox scriptAnalyzer;
        public RichTextBox scriptBoxEditor;
        private RichTextBox scriptBoxViewer;
        private int jumpBack;
        private string temp;



        public Scripts.Commands_s readCommandDPP(BinaryReader reader, Scripts.Commands_s com, List<uint> movOffsetList, List<uint> functionOffsetList, List<uint> scriptOrder)
        {
            uint functionOffset = 0;
            switch (com.Id)
            {
                case 0x2:
                    com.Name = "EndScript(02)";
                    com.isEnd = 1;
                    functionOffset = (uint)reader.BaseStream.Position;
                    checkNextFunction(reader, functionOffset, scriptOrder);
                    break;
                case 0x3:
                    com.Name = "PauseScriptFor(03)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x4:
                    com.Name = "StoreByteInGlobalVar(04)";
                    com.parameters.Add(reader.ReadByte()); // Global Var Id
                    com.parameters.Add(reader.ReadByte()); // Byte
                    break;
                case 0x5:
                    com.Name = "StoreLongInGlobalVar(05)";
                    com.parameters.Add(reader.ReadByte()); // Global Var Id
                    com.parameters.Add(reader.ReadUInt32()); // Int32
                    break;
                case 0x6:
                    com.Name = "StoreVarInGlobalVar(06)";
                    com.parameters.Add(reader.ReadByte()); // Global Var Id
                    com.parameters.Add(reader.ReadUInt32()); // Address
                    break;
                case 0x7:
                    com.Name = "StoreByteInVar(07)";
                    com.parameters.Add(reader.ReadUInt32()); //Address
                    com.parameters.Add(reader.ReadByte()); // Byte
                    break;
                case 0x8:
                    com.Name = "StoreGlobalVarInVar(08)";
                    com.parameters.Add(reader.ReadUInt32()); //Address
                    com.parameters.Add(reader.ReadByte()); // Global Var Id
                    break;
                case 0x9:
                    com.Name = "StoreGlobalVarInGlobalVar(09)";
                    com.parameters.Add(reader.ReadByte()); //Global Var Id
                    com.parameters.Add(reader.ReadByte()); // Global Var Id
                    break;
                case 0xA:
                    com.Name = "StoreVarInVar(0A)";
                    com.parameters.Add(reader.ReadUInt32()); //Var Address
                    com.parameters.Add(reader.ReadUInt32()); //Var Address
                    break;
                case 0xB:
                    com.Name = "CompareGlobalVars(0B)";
                    com.parameters.Add(reader.ReadByte()); //Global Var Id
                    com.parameters.Add(reader.ReadByte()); //Global Var Id
                    break;
                case 0xC:
                    com.Name = "CompareGlobalVarWithByte(0C)";
                    com.parameters.Add(reader.ReadByte()); //Global Var Id
                    com.parameters.Add(reader.ReadByte()); //Byte
                    break;
                case 0xD:
                    com.Name = "CompareGlobalVarWithVar(0D)";
                    com.parameters.Add(reader.ReadByte()); //Global Var Id
                    com.parameters.Add(reader.ReadUInt32()); //Var
                    break;
                case 0xE:
                    com.Name = "CompareVarWithGlobalVar(0E)";
                    com.parameters.Add(reader.ReadUInt32());//Var
                    com.parameters.Add(reader.ReadByte()); //Global Var Id
                    break;
                case 0xF:
                    com.Name = "CompareVarWithByte(0F)";
                    com.parameters.Add(reader.ReadUInt32());//Var
                    com.parameters.Add(reader.ReadByte()); //Byte
                    break;
                case 0x10:
                    com.Name = "CompareVarWithVar(10)";
                    com.parameters.Add(reader.ReadUInt32());//Var
                    com.parameters.Add(reader.ReadUInt32()); //Byte
                    break;
                case 0x11:
                    com.Name = "CompareDerefVarWithVar(11)";
                    com.parameters.Add(reader.ReadUInt16()); //Var Address
                    com.parameters.Add(reader.ReadUInt16()); //Var
                    break;
                case 0x12:
                    com.Name = "CompareDerefVarWithDerefVar(12)";
                    com.parameters.Add(reader.ReadUInt16()); //Var Address
                    com.parameters.Add(reader.ReadUInt16()); //Var Address
                    break;
                case 0x14:
                    com.Name = "CallScriptLib(14)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x15:
                    com.Name = "ExitScriptLib(15)";
                    break;
                case 0x16:
                    com.Name = "Jump(16)";
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[0];
                    checkNextFunction(reader, functionOffset, scriptOrder);
                    com.numJump++;
                    if (reader.BaseStream.Position < reader.BaseStream.Length - 1)
                    {
                        var isEndC = reader.ReadUInt16();
                        if (isEndC != 2)
                        {
                            com.isEnd = 1;
                            checkNextFunction(reader, (uint)reader.BaseStream.Position - 2, scriptOrder);
                        }
                        reader.BaseStream.Position -= 2;
                    }
                    break;
                case 0x17:
                    com.Name = "JumpIfNPC(17)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[0];
                    checkNextFunction(reader, functionOffset, scriptOrder);
                    com.numJump++;
                    if (reader.BaseStream.Position < reader.BaseStream.Length - 1)
                    {
                        var isEndC = reader.ReadUInt16();
                        if (isEndC != 2)
                        {
                            com.isEnd = 1;
                            checkNextFunction(reader, (uint)reader.BaseStream.Position - 2, scriptOrder);
                        }
                        reader.BaseStream.Position -= 2;
                    }
                    break;
                case 0x18:
                    com.Name = "JumpIfPar(18)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[0];
                    checkNextFunction(reader, functionOffset, scriptOrder);
                    com.numJump++;
                    if (reader.BaseStream.Position < reader.BaseStream.Length - 1)
                    {
                        var isEndC = reader.ReadUInt16();
                        if (isEndC != 2)
                        {
                            com.isEnd = 1;
                            checkNextFunction(reader, (uint)reader.BaseStream.Position - 2, scriptOrder);
                        }
                        reader.BaseStream.Position -= 2;
                    }
                    break;
                case 0x19:
                    com.Name = "JumpIfPar(19)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[0];
                    checkNextFunction(reader, functionOffset, scriptOrder);
                    com.numJump++;
                    if (reader.BaseStream.Position < reader.BaseStream.Length - 1)
                    {
                        var isEndC = reader.ReadUInt16();
                        if (isEndC != 2)
                        {
                            com.isEnd = 1;
                            checkNextFunction(reader, (uint)reader.BaseStream.Position - 2, scriptOrder);
                        }
                        reader.BaseStream.Position -= 2;
                    }
                    break;
                case 0x1A:
                    com.Name = "Goto(1A)";
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[0];
                    checkNextFunction(reader, functionOffset, scriptOrder);
                    com.numJump++;
                    break;
                case 0x1B:
                    com.Name = "KillScript(1B)";
                    com.isEnd = 1;
                    functionOffset = (uint)reader.BaseStream.Position;
                    checkNextFunction(reader, functionOffset, scriptOrder);
                    break;
                case 0x1C:
                    com.Name = "When(1C)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[1];
                    checkNextFunction(reader, functionOffset, scriptOrder);
                    break;
                case 0x1D:
                    com.Name = "If(1D)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[1];
                    checkNextFunction(reader, functionOffset, scriptOrder);
                    break;
                case 0x1E:
                    com.Name = "SetFlag(1E)";
                    com.parameters.Add(reader.ReadUInt16()); //Flag Id
                    break;
                case 0x1F:
                    com.Name = "ClearFlag(1F)";
                    com.parameters.Add(reader.ReadUInt16()); //Flag Id
                    break;
                case 0x20:
                    com.Name = "StoreFlag(20)";
                    com.parameters.Add(reader.ReadUInt16()); //Flag Id
                    break;
                case 0x21:
                    com.Name = "StoreFlagInVar(21)";
                    com.parameters.Add(reader.ReadUInt16()); //Var
                    com.parameters.Add(reader.ReadUInt16()); //Flag Id
                    break;
                case 0x22:
                    com.Name = "SetFlagFromAddress(22)";
                    com.parameters.Add(reader.ReadUInt16()); //Flag Address
                    break;
                case 0x23:
                    com.Name = "SetTrainerFlag(23)";
                    com.parameters.Add(reader.ReadUInt16()); //Flag Id
                    break;
                case 0x24:
                    com.Name = "ClearTrainerFlag(24)";
                    com.parameters.Add(reader.ReadUInt16()); //Flag Id
                    break;
                case 0x25:
                    com.Name = "StoreTrainerFlag(25)";
                    com.parameters.Add(reader.ReadUInt16()); //Flag Id
                    break;
                case 0x26:
                    com.Name = "AddVars";
                    com.parameters.Add(reader.ReadUInt16()); //First Var
                    com.parameters.Add(reader.ReadUInt16()); //Second Var
                    break;
                case 0x27:
                    com.Name = "SubVars";
                    com.parameters.Add(reader.ReadUInt16()); //First Var
                    com.parameters.Add(reader.ReadUInt16()); //Second Var
                    break;
                case 0x28:
                    com.Name = "StoreValueInDerefVar(28)";
                    com.parameters.Add(reader.ReadUInt16()); //First Var
                    com.parameters.Add(reader.ReadUInt16()); //Second Var
                    break;
                case 0x29:
                    com.Name = "StoreDerefVarInDerefVar(29)";
                    com.parameters.Add(reader.ReadUInt16()); //First Var
                    com.parameters.Add(reader.ReadUInt16()); //Second Var
                    break;
                case 0x2A:
                    com.Name = "StoreVarInDerefVar(2A)";
                    com.parameters.Add(reader.ReadUInt16()); //First Var
                    com.parameters.Add(reader.ReadUInt16()); //Second Var
                    break;
                case 0x2B:
                    com.Name = "Message(2B)";
                    com.parameters.Add(reader.ReadByte()); //Message Id
                    break;
                case 0x2c:
                    com.Name = "Message(2C)";
                    com.parameters.Add(reader.ReadByte()); //Message Id
                    break;
                case 0x2d:
                    com.Name = "Message(2D)";
                    com.parameters.Add(reader.ReadUInt16());  //Message Id
                    break;
                case 0x2E:
                    com.Name = "Message(2E)";
                    com.parameters.Add(reader.ReadUInt16());  //Message Id
                    break;
                case 0x2f:
                    com.Name = "Message(2F)";
                    com.parameters.Add(reader.ReadByte());
                    break;
                case 0x30:
                    com.Name = "ExecSpecialEvent(30)";
                    break;
                case 0x31:
                    com.Name = "WaitABPress(31)";
                    break;
                case 0x32:
                    com.Name = "WaitKeyPress(32)";
                    break;
                case 0x33:
                    com.Name = "33";
                    break;
                case 0x34:
                    com.Name = "CloseMessageKeyPress(34)";
                    break;
                case 0x35:
                    com.Name = "FreezeMessageBox(35)";
                    break;
                case 0x36:
                    com.Name = "CallMessageBox(36)";
                    com.parameters.Add(reader.ReadUInt16()); //Text Id
                    com.parameters.Add(reader.ReadUInt16()); //Type
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x37:
                    com.Name = "ColorMessageBox(37)";
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
                    if (scriptType == Constants.DPSCRIPT)
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
                    if (scriptType == Constants.DPSCRIPT)
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
                    if (scriptType == Constants.DPSCRIPT)
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
                    if (scriptType == Constants.DPSCRIPT)
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
                    if (scriptType == Constants.DPSCRIPT)
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
                    if (scriptType == Constants.DPSCRIPT)
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
                    if (scriptType == Constants.DPSCRIPT)
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

            this.functionOffsetList = functionOffsetList;
            this.movOffsetList = movOffsetList;
            return com;
        }

        private void checkNextFunction(BinaryReader reader, uint functionOffset, List<uint> scriptOrder)
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
                }
                reader.BaseStream.Position--;
            }
        }

        public void writeCommandDPP(BinaryWriter writer, string[] wordLine, Texts text)
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
                    saveTextString(wordLine, idMessage, 6, text);
                    break;
                case "Message2":
                    writer.Write((Int16)0x2C);
                    idMessage = Byte.Parse(retHex(wordLine[3]));
                    writer.Write((Byte)idMessage);
                    saveTextString(wordLine, idMessage, 6, text);
                    break;
                case "Message3":
                    writer.Write((Int16)0x2D);
                    idMessage = Byte.Parse(retHex(wordLine[3]));
                    writer.Write((Byte)idMessage);
                    saveTextString(wordLine, idMessage, 6, text);
                    break;
                case "Message4":
                    writer.Write((Int16)0x2E);
                    idMessage = Byte.Parse(retHex(wordLine[3]));
                    writer.Write((Byte)idMessage);
                    saveTextString(wordLine, idMessage, 6, text);
                    break;
                case "Message5":
                    writer.Write((Int16)0x2F);
                    idMessage = Byte.Parse(retHex(wordLine[3]));
                    writer.Write((Byte)idMessage);
                    saveTextString(wordLine, idMessage, 6, text);
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
                    saveTextString(wordLine, idMessage, 8, text);
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
                    saveTextString(wordLine, idMessage, 7, text);

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

        private void saveTextString(string[] wordLine, int idMessage, int skip, Texts textFile)
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

        private string getFunction(string parameter)
        {
            var id = Int16.Parse(parameter.Split(' ')[1]);
            if (parameter.Contains("Script"))
                return null;
            else if (parameter.Contains("Function"))
                return functionOffsetList[id].ToString();
            else if (parameter.Contains("Movement"))
                return movOffsetList[id].ToString();
            return parameter.ToString();

        }

        public void GetCommandSimplifiedDPP(string[] scriptsLine, int lineCounter, string space, List<int> visitedLine)
        {
            var line = scriptsLine[lineCounter];
            var commandList = line.Split(' ');
            string movId;
            string tipe;
            int newVar = 0;
            int newVar2 = 0;
            int offset = Int32.Parse(commandList[1].Substring(0, commandList[1].Length - 1));
            var stringOffset = commandList[1];
            if (offset < 10)
                stringOffset = "000" + stringOffset;
            else if (offset > 10 && offset < 100)
                stringOffset = "00" + stringOffset;
            else if (offset > 100 && offset < 1000)
                stringOffset = "0" + stringOffset;

            //scriptBoxEditor.AppendText(stringOffset + " ");
            switch (commandList[2].Split('(')[0])
            {
                case "141":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "141_VALUE", "NOR");
                    scriptBoxEditor.AppendText(space + "141_VALUE " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "144":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "144_VALUE", "NOR");
                    scriptBoxEditor.AppendText(space + "144_VALUE " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "146":
                    newVar = checkStored(commandList, 4);
                    var varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "146_VALUE", "NOR");
                    scriptBoxEditor.AppendText(space + "146_VALUE " + commandList[4] + " = " + commandList[2] + "( " + varString + " );\n");
                    break;
                case "1CD":
                    scriptBoxEditor.AppendText(space + commandList[2] + "( " + commandList[3] +
                                               " ," + commandList[4] + " , TRAINER " + commandList[5] + ", " + commandList[6] + ", " + commandList[7] + " );\n");
                    break;
                case "238":
                    scriptBoxEditor.AppendText(space + "VAR " + commandList[4] + " = " + commandList[2] + "(P_1 " + commandList[3] + ");\n");
                    break;
                case "2AD":
                    newVar = Int32.Parse(commandList[3].Substring(2, commandList[3].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    newVar2 = Int32.Parse(commandList[4].Substring(2, commandList[4].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "OW_ID", "NOR");
                    scriptBoxEditor.AppendText(space + "UNK " + commandList[3] + ", OW_ID " + commandList[4] + " = " + commandList[2] + "();\n");
                    break;
                case "2BA":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "2BA_VALUE", "NOR");
                    scriptBoxEditor.AppendText(space + "2BA_VALUE " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "ActPeopleContest":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "TRAINER_TYPE", "NOR");
                    scriptBoxEditor.AppendText(space + "TRAINER_TYPE " + commandList[4] + " = " + commandList[2] + "( PART_ID " + commandList[3] + " );\n");
                    break;
                case "ActPokèKronApplication":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + commandList[2] + "( PKRONAPP " + varString + " );\n");
                    break;
                case "AddPeople":
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( OW_ID " + commandList[3] + ");\n");
                    break;
                case "ApplyMovement":
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( OW_ID " + commandList[3] + ");\n");
                    scriptBoxEditor.AppendText(space + "{\n");
                    movId = commandList[6];
                    tipe = commandList[5];
                    for (var functionLineCounter = 0; functionLineCounter < scriptsLine.Length; functionLineCounter++)
                    {
                        var line2 = scriptsLine[functionLineCounter];
                        if (commandList.Length < 8)
                        {
                            var offset2 = commandList[5].TrimStart('0').TrimStart('x');
                            int offset3 = int.Parse(offset2, System.Globalization.NumberStyles.HexNumber);
                            if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + offset3))
                            {
                                functionLineCounter++;
                                do
                                {
                                    line2 = scriptsLine[functionLineCounter];
                                    var movList = line2.Split(' ');
                                    if (line2.Length > 1)
                                        scriptBoxEditor.AppendText(space + " " + movList[2].ToString().TrimStart('m') + " TIMES " + movList[3] + "\n");
                                    functionLineCounter++;
                                } while (!line2.Contains("End_Movement") && functionLineCounter + 1 < scriptsLine.Length);
                                scriptBoxEditor.AppendText(space + "}\n");
                                return;
                            }
                        }
                        else if (commandList.Length > 8 && functionLineCounter + 1 < scriptsLine.Length && scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[7].TrimStart('(')))
                        {
                            functionLineCounter++;
                            do
                            {
                                line2 = scriptsLine[functionLineCounter];
                                var movList = line2.Split(' ');
                                if (line2.Length > 1)
                                    scriptBoxEditor.AppendText(space + " " + movList[2].ToString().TrimStart('m') + " " + movList[3].TrimStart('0').TrimStart('x') + "\n");
                                functionLineCounter++;
                            } while (!line2.Contains("End_Movement") && functionLineCounter + 1 < scriptsLine.Length);
                            scriptBoxEditor.AppendText(space + "}\n");
                            return;
                        }


                    }

                    break;
                case "CallBattleParkFunction":
                    newVar = checkStored(commandList, 5);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FUNCT_VALUE", "NOR");
                    scriptBoxEditor.AppendText(space + "FUNC_VALUE " + commandList[5] + " = " + commandList[2] + "( " + varString + ", " + commandList[4] + " );\n");
                    break;
                case "CallMessageBox":
                    scriptBoxEditor.AppendText(space + "BORDER " + commandList[5] + " = " + commandList[2] + "( MESSAGE_ID " + commandList[3] +
                                               ", TYPE " + commandList[4] + " ");
                    string text = "";
                    if (Int16.Parse(commandList[3]) < textFile.textList.Count)
                    {
                        text = textFile.textList[Int16.Parse(commandList[3])].text;
                        scriptBoxEditor.AppendText(" = " + text + " ");
                    }
                    scriptBoxEditor.AppendText(" );\n");
                    break;
                case "ChangeOwPosition":
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( OW_ID " + commandList[3] +
                                               ", X " + commandList[4] + ", Y " + commandList[5] + ");\n");
                    break;
                case "ChangeOwPosition2":
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( OW_ID " + commandList[3] +
                                               ", P_2 " + commandList[4] + ");\n");
                    break;
                case "ChangePokèmonForm":
                    scriptBoxEditor.AppendText(space + commandList[2] + "( FORM " + commandList[3] + ", " + commandList[4] + ", ID " + commandList[5] + ", " + commandList[6] + " );\n");
                    break;
                case "CheckAccessories":
                    newVar = checkStored(commandList, 5);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ACCESSORIES_TAKEN", "BOL");
                    scriptBoxEditor.AppendText(space + "ACCESSORIES_TAKEN " + commandList[5] + " = " + commandList[2] + "( ACCESSORIES " + commandList[3] + " , " + "NUMBER " + commandList[4] + " );\n");
                    break;
                case "CheckBadge":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HAVE_BADGE", "BOL");
                    varString = getTextFromCondition(commandList[3].ToString(), "BAD");
                    scriptBoxEditor.AppendText(space + "HAVE_BADGE " + commandList[4] + "= " + commandList[2] + "( BADGE " + varString + ");\n");
                    break;
                case "CheckBoxSpace":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BOX_SPACE", "BOL");
                    scriptBoxEditor.AppendText(space + "BOX_SPACE " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "CheckCasinoPrizeCoins":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "COINSBOX_SPACE", "BOL");
                    scriptBoxEditor.AppendText(space + "COINSBOX_SPACE " + commandList[3] + " = " + commandList[2] + "( PRIZE " + varString + " );\n");
                    break;
                case "CheckCoinsBoxSpace":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "COINSBOX_SPACE", "BOL");
                    scriptBoxEditor.AppendText(space + "COINSBOX_SPACE " + commandList[3] + " = " + commandList[2] + "( AMOUNT " + commandList[4] + " , " + commandList[5] + " );\n");
                    break;
                case "CheckContestEnd":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "CONTEST_END", "BOL");
                    scriptBoxEditor.AppendText(space + "CONTEST_END " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "CheckContestPicture":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "CONTESTPICTURE_STATUS", "BOL");
                    scriptBoxEditor.AppendText(space + "CONTESTPICTURE_STATUS " + commandList[4] + " = " + commandList[2] + "( PICTURE " + commandList[3] + " );\n");
                    break;
                case "CheckHaveBackground":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HAVE_BACKGROUND", "BOL");
                    scriptBoxEditor.AppendText(space + "HAVE_BACKGROUND " + commandList[4] + " = " + commandList[2] + "( BACKGROUND " + commandList[3] + " );\n");
                    break;
                case "CheckHoney":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HONEY", "BOL");
                    scriptBoxEditor.AppendText(space + "HONEY " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "CheckItemBagSpace":
                    newVar = checkStored(commandList, 5);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEMBAG_SPACE", "BOL");
                    scriptBoxEditor.AppendText(space + "ITEMBAG_SPACE " + commandList[5] + " = " + commandList[2] + "( ITEM " + commandList[3] + " , " + "NUMBER " + commandList[4] + " );\n");
                    break;
                case "CheckItemBagNumber":
                    newVar = checkStored(commandList, 5);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEMBAG_NUMBER", "BOL");
                    scriptBoxEditor.AppendText(space + "ITEMBAG_NUMBER " + commandList[5] + " = " + commandList[2] + "( ITEM " + getTextFromCondition(commandList[3], "ITE") + " , " + "NUMBER " + commandList[4] + " );\n");
                    break;
                case "CheckDressPicture":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "DRESSPICTURE_STATUS", "BOL");
                    scriptBoxEditor.AppendText(space + "DRESSPICTURE_STATUS " + commandList[4] + " = " + commandList[2] + "( PICTURE " + commandList[3] + " );\n");
                    break;
                case "CheckEgg":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "EGG_PARTY", "BOL");
                    scriptBoxEditor.AppendText(space + "EGG_PARTY " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "CheckErrorSave":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ERROR", "BOL");
                    scriptBoxEditor.AppendText(space + "ERROR " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "CheckHavePartyPokèmon":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HAVEPARTY_POKE", "BOL");
                    scriptBoxEditor.AppendText(space + "HAVEPARTY_POKE " + commandList[3] + " = " + commandList[2] + "( POKEMON " + getTextFromCondition(commandList[4], "POK") + " );\n");
                    break;
                case "CheckMoney":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MONEY_STATUS", "BOL");
                    scriptBoxEditor.AppendText(space + "MONEY_STATUS " + commandList[3] + " = " + commandList[2] + "( AMOUNT " + commandList[4] + ", " + commandList[5] + " );\n");
                    break;
                case "CheckPlantingLimit":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PLANTLIMIT_REACHED", "BOL");
                    scriptBoxEditor.AppendText(space + "PLANTLIMIT_REACHED " + commandList[4] + "  = " + commandList[2] + "( LIMIT" + commandList[3] + " );\n");
                    break;
                case "CheckPoisoned":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POISONED", "BOL");
                    newVar2 = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "POKEMON", "NOR");
                    scriptBoxEditor.AppendText(space + "POISONED " + commandList[3] + " = " + commandList[2] + "( POKEMON " + commandList[4] + " );\n");
                    break;
                case "CheckPokèKron":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PKRON_STATUS", "BOL");
                    scriptBoxEditor.AppendText(space + "PKRON_STATUS " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "CheckPokèKronApplication":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PKRONAPP_STATUS", "BOL");
                    scriptBoxEditor.AppendText(space + "PKRONAPP_STATUS " + commandList[4] + " = " + commandList[2] + "( PKRONAPP " + commandList[3] + ");\n");
                    break;
                case "CheckPokèmonCaught":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKE_CAUGHT", "NOR");
                    scriptBoxEditor.AppendText(space + "POKE_CAUGHT " + commandList[4] + " = " + commandList[2] + "( POKEMON " + commandList[3] + ");\n");
                    break;
                case "CheckPokèMoveTeacherCompatibility":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "POKE_COMPATIBLE", "BOL");
                    scriptBoxEditor.AppendText(space + "POKE_COMPATIBLE " + commandList[3] + " = " + commandList[2] + "( POKEMON " + commandList[4] + " );\n");
                    break;
                case "CheckPokèrus":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKERUS", "BOL");
                    scriptBoxEditor.AppendText(space + "POKERUS " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "CheckRandomPokèmonSearch":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "SEARCH_RESULT", "BOL");
                    scriptBoxEditor.AppendText(space + "SEARCH_RESULT " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "CheckRibbonPokèmon":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "RIBBON_TAKEN", "BOL");
                    scriptBoxEditor.AppendText(space + "RIBBON_TAKEN " + commandList[3] + " = " + commandList[2] + "( POKE " + varString + ", RIBBON " + commandList[5] + " );\n");
                    break;
                case "CheckSpecialItem":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "SPECIAL_ITEM", "BOL");
                    scriptBoxEditor.AppendText(space + "SPECIAL_ITEM " + commandList[4] + " = " + commandList[2] + "( ITEM " + commandList[3] + " );\n");
                    break;
                case "ChooseUnion":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "UNION_DECISION", "NOR");
                    scriptBoxEditor.AppendText(space + "UNION_DECISION " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "ChooseWifiSprite":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "WIFI_SPRITE_CHOSEN", "NOR");
                    scriptBoxEditor.AppendText(space + "WIFI_SPRITE_CHOSEN " + commandList[4] + " = " + commandList[2] + "( " + varString + " );\n");
                    break;
                case "ClearFlag":
                    var statusFlag = "FALSE";
                    if (commandList[4] == "1")
                        statusFlag = "TRUE";
                    scriptBoxEditor.AppendText(space + "FLAG " + getTextFromCondition(commandList[3], "FLA") +
                                               " = " + statusFlag + ";\n");
                    break;
                case "ClearTrainerId":
                    scriptBoxEditor.AppendText(space + "TRAINER " + commandList[3] + " = INACTIVE;" + "\n");
                    break;
                case "CompareDerefVarWithVar":
                    cond = "NOR";

                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    var varString2 = getTextFromCondition(getStoredMagic(varNameDictionary, varLevel, commandList, 4), cond);
                    var condition = getCondition(scriptsLine, lineCounter);
                    scriptBoxEditor.AppendText(space + "If( " + varString + " " + condition + " " + varString2 + ")\n");
                    break;
                case "CompareVarWithVar":
                    cond = "NOR";
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    varString2 = getTextFromCondition(getStoredMagic(varNameDictionary, varLevel, commandList, 4), cond);
                    condition = getCondition(scriptsLine, lineCounter);
                    scriptBoxEditor.AppendText(space + "If( " + varString + " " + condition + " " + varString2 + ")\n");
                    break;
                case "ComparePhraseBoxInput":
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "COMPARE_RESULT", "NOR");
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    var varString3 = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
                    var varString4 = getStoredMagic(varNameDictionary, varLevel, commandList, 6);
                    var varString5 = getStoredMagic(varNameDictionary, varLevel, commandList, 7);
                    scriptBoxEditor.AppendText(space + "COMPARE_RESULT " + commandList[3] + " = " + commandList[2] + "( " + varString + ", " + varString3 + ", " + varString4 + ", " + varString5 + " );\n");
                    break;
                case "CopyVar":
                    newVar2 = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    if (!varString.Contains("0x"))
                        addToVarNameDictionary(varNameDictionary, varLevel, newVar2, varString, cond);
                    scriptBoxEditor.AppendText(space + "VAR " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "Cry":
                    scriptBoxEditor.AppendText(space + commandList[2] + "( POKE " + commandList[3] + " );\n");
                    break;
                case "DoubleTrainerBattle":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + commandList[2] + "( OTHER " + varString + ", OPPONENT " + commandList[4] + ", OPPONENT " + commandList[5] + " );\n");
                    break;
                case "Fanfare":
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MUSIC_ID " + commandList[3] + ");\n");
                    break;

                case "GiveCoins":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + commandList[2] + "( AMOUNT " + varString + " );\n");
                    break;
                case "GiveItem":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + commandList[2] + "( ITEM " + varString + " , NUMBER " + varString2 + ", " + commandList[5] + " );\n");
                    break;
                case "GivePokémon":
                    newVar = checkStored(commandList, 3);
                    scriptBoxEditor.AppendText(space + commandList[2] + "( POKEMON " + commandList[3] + " , LEVEL " + commandList[4] + " , ITEM " + commandList[5] + ", " + commandList[6] + " );\n");
                    break;
                case "Goto":
                    scriptBoxEditor.AppendText(space + "Goto\n" + space + "{\n");
                    var functionId = commandList[5];
                    varNameDictionary.Add(new Dictionary<int, string>());
                    for (var functionLineCounter = 0; functionLineCounter < scriptsLine.Length - 1; functionLineCounter++)
                    {
                        var line2 = scriptsLine[functionLineCounter];
                        if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[6].TrimStart('(')))
                        {
                            readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
                            return;
                        }
                    }
                    break;
                case "When":
                    scriptBoxEditor.AppendText(space + "{\n");
                    functionId = commandList[5];
                    var type = commandList[4];
                    varNameDictionary.Add(new Dictionary<int, string>());
                    for (var functionLineCounter = 0; functionLineCounter < scriptsLine.Length - 1; functionLineCounter++)
                    {
                        var line2 = scriptsLine[functionLineCounter];
                        if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[6].TrimStart('(')))
                        {
                            varLevel++;
                            readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
                            varLevel--;
                            return;
                        }
                    }
                    break;
                case "If":
                    scriptBoxEditor.AppendText(space + "{\n");
                    functionId = commandList[5];
                    type = commandList[4];
                    varNameDictionary.Add(new Dictionary<int, string>());
                    for (var functionLineCounter = 0; functionLineCounter < scriptsLine.Length - 1; functionLineCounter++)
                    {
                        var line2 = scriptsLine[functionLineCounter];
                        if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[6].TrimStart('(')))
                        {
                            varLevel++;
                            readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
                            varLevel--;
                            return;
                        }
                    }
                    break;
                case "IncPokèmonHappiness":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + commandList[2] + "( POKEMON " + varString + " , AMOUNT " + commandList[3] + " );\n");
                    break;
                case "Jump":
                    scriptBoxEditor.AppendText(space + "Jump\n" + space + "{\n");
                    functionId = commandList[5];
                    varNameDictionary.Add(new Dictionary<int, string>());
                    for (var functionLineCounter = 0; functionLineCounter < scriptsLine.Length - 1; functionLineCounter++)
                    {
                        var line2 = scriptsLine[functionLineCounter];
                        if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[6].TrimStart('(')))
                        {
                            readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
                            return;
                        }
                    }
                    break;
                case "LegendaryBattle":
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( POKE " + commandList[3] + " , LEVEL " + commandList[4] + " );\n");
                    break;
                case "Lock":
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( OW_ID " + commandList[3] + ");\n");
                    break;
                case "Message":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + " ");
                    text = "";
                    text = getVarString(scriptBoxEditor, textFile, commandList, varString, text);
                    if (text != "")
                        scriptBoxEditor.AppendText(" = " + text + " ");
                    scriptBoxEditor.AppendText(");\n");
                    break;
                case "Message2":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + " ");
                    text = "";
                    text = getVarString(scriptBoxEditor, textFile, commandList, varString, text);
                    if (text != "")
                        scriptBoxEditor.AppendText(" = " + text + " ");
                    scriptBoxEditor.AppendText(");\n");
                    break;
                case "Message3":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + " ");
                    text = "";
                    text = getVarString(scriptBoxEditor, textFile, commandList, varString, text);
                    if (text != "")
                        scriptBoxEditor.AppendText(" = " + text + " ");
                    scriptBoxEditor.AppendText(");\n");
                    break;
                case "Message5":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + " ");
                    text = "";
                    text = getVarString(scriptBoxEditor, textFile, commandList, varString, text);
                    if (text != "")
                        scriptBoxEditor.AppendText(" = " + text + " ");
                    scriptBoxEditor.AppendText(");\n");
                    break;
                case "MessageBattle":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( TRAINER_ID " + varString + ", MESSAGE_ID " + commandList[4] + ");\n");
                    break;
                case "MultiTextScriptMessage(40)":
                    newVar = checkStored(commandList, 7);
                    mulString = new List<String>();
                    mulActive = true;
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MULTI_CHOSEN", "MUL");
                    scriptBoxEditor.AppendText(space + "MULTI_CHOSEN " + commandList[7] + " = " + commandList[2] + "(X " + commandList[3] + " , " +
                        "Y " + commandList[4] + " , " +
                        "CURSOR " + commandList[5] +
                        " , " + commandList[6] +
                        ");\n");
                    break;
                case "MultiTextScriptMessage(41)":
                    newVar = checkStored(commandList, 7);
                    mulString = new List<String>();
                    mulActive = false;
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MULTI_CHOSEN", "MUL");
                    scriptBoxEditor.AppendText(space + "MULTI_CHOSEN " + commandList[7] + " = " + commandList[2] + "(X " + commandList[3] + " , " +
                        "Y " + commandList[4] + " , " +
                        "CURSOR " + commandList[5] +
                        " , " + commandList[6] +
                        ");\n");
                    break;
                case "MultiTextScriptMessage(44)":
                    newVar = checkStored(commandList, 7);
                    mulString = new List<String>();
                    mulActive = true;
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MULTI_CHOSEN", "MUL");
                    scriptBoxEditor.AppendText(space + "MULTI_CHOSEN " + commandList[7] + " = " + commandList[2] + "(X " + commandList[3] + " , " +
                        "Y " + commandList[4] + " , " +
                        "CURSOR " + commandList[5] +
                        " , " + commandList[6] +
                        ");\n");
                    break;
                case "MultiTextScriptMessage(45)":
                    newVar = checkStored(commandList, 7);
                    mulString = new List<String>();
                    mulActive = false;
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MULTI_CHOSEN", "MUL");
                    scriptBoxEditor.AppendText(space + "MULTI_CHOSEN " + commandList[7] + " = " + commandList[2] + "(X " + commandList[3] + " , " +
                        "Y " + commandList[4] + " , " +
                        "CURSOR " + commandList[5] +
                        " , " + commandList[6] +
                        ");\n");
                    break;
                case "Multi3":
                    newVar = checkStored(commandList, 7);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MULTI_CHOSEN", "MUL");
                    scriptBoxEditor.AppendText(space + "MULTI_CHOSEN " + commandList[7] + " = " + commandList[2] + "(X " + commandList[3] + " , " +
                        "Y " + commandList[4] + " , " +
                        "CURSOR " + commandList[5] +
                        " , " + commandList[6] +
                        ");\n");
                    break;
                case "PlaySound":
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( SOUND_ID " + commandList[3] + ");\n");
                    break;
                case "ReleaseOw":
                    scriptBoxEditor.AppendText(space + commandList[2] + "( OW_ID " + commandList[3] +
                                               ", P_2 " + commandList[4] + ");\n");
                    break;

                case "RemovePeople":
                    scriptBoxEditor.AppendText(space + commandList[2] + "( OW_ID " + commandList[3] + ");\n");
                    break;
                case "SetBadge":
                    scriptBoxEditor.AppendText(space + "BADGE " + commandList[3] + " = TRUE;" + "\n");
                    break;
                case "SetFlag":
                    scriptBoxEditor.AppendText(space + "FLAG " + getTextFromCondition(commandList[3], "FLA") + " = TRUE;" + "\n");
                    break;
                case "SetSafariGame":
                    if (commandList[3] == "1")
                        varString = "INACTIVE";
                    else
                        varString = "ACTIVE";
                    scriptBoxEditor.AppendText(space + "SAFARI_GAME = " + varString + " ;\n");
                    break;
                case "SetMultiBoxTextScriptMessage":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + commandList[2] + "( BOX_TEXT " + commandList[3] + " , " +
                        "MESSAGEBOX_TEXT  " + commandList[4] + " , " +
                        "SCRIPT " + commandList[5] + " ");
                    text = "";
                    text = getTextFromVar(scriptBoxEditor, commandList, varString, text);
                    scriptBoxEditor.AppendText(");\n");
                    break;
                case "SetMultiTextScriptMessage":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + commandList[2] + "( MESSAGEBOX_TEXT  " + commandList[3] + " , " +
                        "SCRIPT " + commandList[4] + " ");
                    text = "";
                    text = getTextFromVar(scriptBoxEditor, commandList, varString, text);
                    scriptBoxEditor.AppendText(");\n");
                    break;
                case "SetMultiTextScript":
                    newVar = checkStored(commandList, 3);
                    mulActive = false;
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + ", SCRIPT " + commandList[4] + " ");
                    text = "";
                    text = getTextFromVar(scriptBoxEditor, commandList, varString, text);
                    scriptBoxEditor.AppendText(");\n");
                    break;
                case "SetTrainerId":
                    scriptBoxEditor.AppendText(space + "TRAINER " + commandList[3] + " = ACTIVE;" + "\n");
                    break;
                case "SetVar":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "VAR " + commandList[3], "NOR");
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    if (scriptsLine[lineCounter + 2].Contains("CallStd 252"))
                        varString = getTextFromCondition(varString, "ITE");
                    if (scriptsLine[lineCounter + 2].Contains("GiveAccesso"))
                        varString = getTextFromCondition(varString, "ACC");
                    scriptBoxEditor.AppendText(space + varString2 + " = " + varString + "\n");
                    break;
                case "SetVarAccessories":
                    scriptBoxEditor.AppendText(space + "VAR ACC. " + commandList[3] + " = " + commandList[4] + ";\n");
                    break;
                case "SetVarAccPicture":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR PICTURE_ACC " + commandList[3] + " = " + commandList[2] + "( " + varString + " );\n");
                    break;
                case "SetVarStarterAccessories":
                    newVar2 = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR ACC. " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarAlter":
                    scriptBoxEditor.AppendText(space + "VAR NAME " + commandList[3] + " = [ALTER]" + ";\n");
                    break;
                case "SetVarHero":
                    scriptBoxEditor.AppendText(space + "VAR NAME " + commandList[3] + " = [HIRO]" + ";\n");
                    break;
                case "SetVarItem":
                    newVar2 = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR ITEM " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarItem2":
                    newVar2 = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR ITEM2 " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarBattleItem":
                    newVar2 = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR BATTLE ITEM " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarUndergroundItem":
                    newVar2 = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR ITEM " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarChosenPokèmonSize":
                    newVar = checkStored(commandList, 5);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
                    scriptBoxEditor.AppendText(space + "VAR " + commandList[3] + " , VAR " + commandList[4] + " = " + commandList[2] + "( " + varString + " );\n");
                    break;
                case "SetVarMoveDeleter":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR MOVE " + commandList[3] + " = " + commandList[2] + "( " + varString + " );\n");
                    break;
                case "SetVarNamePartContest":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "VAR NAME " + commandList[4] + " = " + commandList[2] + "(PART " + varString + " );\n");
                    break;
                case "SetVarModeContest":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "VAR MODE " + commandList[3] + " ;\n");
                    break;
                case "SetVarTypeContest":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "VAR TYPE " + commandList[3] + " ;\n");
                    break;
                case "SetVarNickPokémon":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR NICK " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarNumber":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR NUM " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarPartIdMusical":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "PART_ID " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarPokémon":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR POKE " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarPokèmonNature":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR NAT " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarPhraseBoxInput":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR STRING " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarRandomPrize":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR L " + commandList[3] + " = " + commandList[2] + "( " + varString + " , " + commandList[5] + " );\n");
                    break;
                case "SetVarRequestedPokèmonSize":
                    newVar = checkStored(commandList, 5);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
                    scriptBoxEditor.AppendText(space + "VAR " + commandList[3] + " , VAR " + commandList[4] + " = " + commandList[2] + "( POKEMON " + varString + " );\n");
                    break;
                case "SetVarRival":
                    scriptBoxEditor.AppendText(space + "VAR NAME " + commandList[3] + " = [RIVAL]" + ";\n");
                    break;
                case "SetVarSwarmPlace":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR SWARM_PLACE " + commandList[3] + " = " + commandList[2] + "( " + varString + " );\n");
                    break;
                case "SetVarUndergroundItem(P)":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR UNDERGROUND_ITEM " + commandList[3] + " = " + commandList[2] + "( " + varString + " );\n");
                    break;
                case "SetVarWifiSprite(P)":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR WIFI_SPRITE " + commandList[3] + " = " + commandList[2] + "( " + varString + " );\n");
                    break;
                case "SetVarTrainer":
                    scriptBoxEditor.AppendText(space + "VAR TRAINER " + commandList[3] + " = ACTIVE" + ";\n");
                    break;
                case "SetVarTrainer2":
                    scriptBoxEditor.AppendText(space + "VAR TRAINER_2 " + commandList[3] + " = ACTIVE" + ";\n");
                    break;
                case "ShowCoins":
                    scriptBoxEditor.AppendText(space + commandList[2] + "( X " + commandList[3] + " , Y " + commandList[4] + " );\n");
                    break;
                case "ShowMoney":
                    scriptBoxEditor.AppendText(space + commandList[2] + "( X " + commandList[3] + " , Y " + commandList[4] + " );\n");
                    break;
                case "ShowMoveCheckShards(P)":
                    newVar = checkStored(commandList, 5);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MOVE", "NOR");
                    scriptBoxEditor.AppendText(space + "MOVE " + commandList[5] + " = " + commandList[2] + "( " + commandList[3] + " , " + commandList[4] + ");\n");
                    break;
                case "StartDressPokèmon":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "DRESS_DECISION", "NOR");
                    scriptBoxEditor.AppendText(space + "DRESS_DECISION " + commandList[4] + " = " + commandList[2] + "(" + commandList[3] + " , " + commandList[5] + " );\n");
                    break;
                case "StartInterview(237)":
                    varString2 = "";
                    if (commandList.Length <= 8 && textFile == null)
                    {
                        scriptBoxEditor.AppendText(space + "" + commandList[2] + "( " + commandList[3] + " , MESSAGE " + commandList[4] + ", " + commandList[5] + ", " + commandList[6] + " );\n");
                    }
                    else
                    {
                        short index = 0;
                        varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                        if (varString.Contains("VAR"))
                        {
                            varString = varString.Split(' ')[1];
                        }
                        if (varString.Contains("M"))
                        {
                            index = Int16.Parse(varString.ToCharArray()[varString.Length - 1].ToString());
                            scriptBoxEditor.AppendText(space + varString + " = ' " + textFile.messageList[index] + " ';\n");
                        }
                        else if (varString.Length > 2 && Int16.Parse(varString.Substring(2, varString.Length - 2)) > 7999 && textFile != null)
                        {
                            var scriptAnLine = scriptBoxEditor.Lines;
                            int counterInverse = scriptAnLine.Length - 1;
                            while (counterInverse > 0)
                            {
                                if (!scriptAnLine[counterInverse].Contains("VAR " + varString + " = "))
                                    counterInverse--;
                                else
                                    break;
                            }
                            if (counterInverse >= 0)
                            {
                                var text2 = scriptAnLine[counterInverse].Split(' ');
                                varString = text2[text2.Length - 1];
                                scriptBoxEditor.AppendText(space + "INTERVIEW_MESSAGE " + commandList[4] + " = '" + textFile.textList[Int16.Parse(varString)].text + " ';\n");
                                varString = commandList[4].ToString();
                            }
                        }
                        else if (Int16.TryParse(varString, out index))
                            scriptBoxEditor.AppendText(space + "INTERVIEW_MESSAGE_" + varString + " = ' " + textFile.messageList[Int16.Parse(varString)] + " ';\n");
                        scriptBoxEditor.AppendText(space + "" + commandList[2] + "( " + commandList[3] + " , MESSAGE " + varString + ", " + commandList[5] + ", " + commandList[6] + " );\n");
                    }
                    break;
                case "StoreBadgeNumber":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BADGE_NUMBER", "NOR");
                    scriptBoxEditor.AppendText(space + "BADGE_NUMBER " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreBattleResult":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BATTLE_RESULT", "NOR");
                    scriptBoxEditor.AppendText(space + "BATTLE_RESULT " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreBattleResult2":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BATTLE_RESULT2", "NOR");
                    scriptBoxEditor.AppendText(space + "BATTLE_RESULT2 " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreBerryGrownState":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "GROWN_STATE", "NOR");
                    scriptBoxEditor.AppendText(space + "GROWN_STATE " + commandList[3] + "  = " + commandList[2] + "();\n");
                    break;
                case "StoreBerryMatureNumber":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BERRY_NUMBER", "NOR");
                    scriptBoxEditor.AppendText(space + "BERRY_NUMBER " + commandList[3] + "  = " + commandList[2] + "();\n");
                    break;
                case "StoreBerryMatureType":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BERRY_TYPE", "NOR");
                    scriptBoxEditor.AppendText(space + "BERRY_TYPE " + commandList[3] + "  = " + commandList[2] + "();\n");
                    break;
                case "StoreBerryPlanted":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BERRY", "NOR");
                    scriptBoxEditor.AppendText(space + "BERRY " + commandList[3] + "  = " + commandList[2] + "();\n");
                    break;
                case "StoreBoundedVariable":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BOUNDED_VARIABLE", "NOR");
                    scriptBoxEditor.AppendText(space + "BOUNDED_VARIABLE " + commandList[3] + " = " + commandList[2] + "( BOUND " + commandList[4] + " );\n");
                    break;
                case "StoreBurmyFormsNumber":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BURMYFORMS_NUMBER", "NOR");
                    scriptBoxEditor.AppendText(space + "BURMYFORMS_NUMBER " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreCasinoPrizeResult":
                    newVar = checkStored(commandList, 4);
                    newVar2 = checkStored(commandList, 5);
                    var newVar3 = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PRIZE_ID", "NOR");
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "PRIZE_NAME", "NOR");
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "PRIZE_SCRIPT", "NOR");
                    scriptBoxEditor.AppendText(space + "PRIZE_SCRIPT " + commandList[3] + " , PRIZE_ID " + commandList[4] + ", PRIZE_NAME " + commandList[5] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreChosenPokèmon":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "CHOSEN_POKEMON", "NOR");
                    scriptBoxEditor.AppendText(space + "CHOSEN_POKEMON " + commandList[3] + "  = " + commandList[2] + "();\n");
                    break;
                case "StoreChosenPokèmonBattlePark":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PARK_POKEMON", "NOR");
                    scriptBoxEditor.AppendText(space + "PARK_POKEMON " + commandList[3] + "  = " + commandList[2] + "(" + commandList[4] + " );\n");
                    break;
                case "StoreChosenPokèmonMusical":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MUSICAL_POKEMON", "NOR");
                    scriptBoxEditor.AppendText(space + "MUSICAL_POKEMON " + commandList[3] + "  = " + commandList[2] + "(" + commandList[4] + " );\n");
                    break;
                case "StoreChosenPokèmonTrade":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "TRADE_POKEMON", "NOR");
                    scriptBoxEditor.AppendText(space + "TRADE_POKEMON " + commandList[3] + "  = " + commandList[2] + "();\n");
                    break;
                case "StoreDay":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "DAY", "NOR");
                    scriptBoxEditor.AppendText(space + "DAY " + commandList[3] + "  = " + commandList[2] + "();\n");
                    break;
                case "StoreDoublePhraseBoxInput":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "INSERT_CHECK", "NOR");
                    newVar2 = checkStored(commandList, 5);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "WORD", "NOR");
                    newVar3 = checkStored(commandList, 6);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "WORD_2", "NOR");
                    scriptBoxEditor.AppendText(space + "INSERT_CHECK " + commandList[4] + ", WORD " + commandList[5] + ", WORD_2 " + commandList[6] + " = " + commandList[2] + "( " + commandList[3] + " );\n");
                    break;
                case "StoreDoublePhraseBoxInput2":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "INSERT_CHECK", "NOR");
                    newVar2 = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "WORD", "NOR");
                    newVar3 = checkStored(commandList, 5);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "WORD_2", "NOR");
                    var newVar4 = checkStored(commandList, 6);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar4, "WORD_3", "NOR");
                    var newVar5 = checkStored(commandList, 7);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar5, "WORD_4", "NOR");
                    scriptBoxEditor.AppendText(space + "INSERT_CHECK " + commandList[3] + ", WORD " + commandList[4] + ", WORD_2 " + commandList[5] + ", WORD_3 " + commandList[6] + ", WORD_4 " + commandList[7] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreItemBerry":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEM_BERRY", "ITE");
                    scriptBoxEditor.AppendText(space + "ITEM_BERRY " + commandList[3] + "  = " + commandList[2] + "();\n");
                    break;
                case "StoreFertilizer":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FERTILIZER", "NOR");
                    scriptBoxEditor.AppendText(space + "FERTILIZER " + commandList[3] + "  = " + commandList[2] + "();\n");
                    break;
                case "StoreFlag":
                    scriptBoxEditor.AppendText(space + "If( FLAG " + getTextFromCondition(commandList[3], "FLA") + " == TRUE)\n");
                    break;
                case "StoreFirstPokèmonParty":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FIRST_POKEMON", "NOR");
                    scriptBoxEditor.AppendText(space + "FIRST_POKEMON " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreFirstTimePokèmonLeague":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "VICTORY_LEAGUE", "NOR");
                    scriptBoxEditor.AppendText(space + "VICTORY_LEAGUE " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreFloor":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FLOOR", "NOR");
                    scriptBoxEditor.AppendText(space + "FLOOR " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreGender":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "GENDER", "GEN");
                    scriptBoxEditor.AppendText(space + "GENDER " + commandList[3] + "  = " + commandList[2] + "();\n");
                    break;
                case "StoreHappinessItem":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HAPPINESS_ITEM", "NOR");
                    scriptBoxEditor.AppendText(space + "HAPPINESS_ITEM " + commandList[3] + "  = " + commandList[2] + "();\n");
                    break;
                case "StoreHeroFaceOrientation":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FACE_ORIENTATION", "NOR");
                    scriptBoxEditor.AppendText(space + "FACE_ORIENTATION " + commandList[3] + "  = " + commandList[2] + "();\n");
                    break;
                case "StoreHeroFriendCode":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FRIEND_CODE", "NOR");
                    scriptBoxEditor.AppendText(space + "FRIEND_CODE " + commandList[3] + "  = " + commandList[2] + "();\n");
                    break;
                case "StoreHeroPosition":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "X", "NOR");
                    newVar2 = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "Y", "NOR");
                    scriptBoxEditor.AppendText(space + "X " + commandList[3] + ",Y " + commandList[4] + " = " + commandList[2] + "();\n");
                    break;

                case "StoreItemType":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEM_TYPE", "NOR");
                    scriptBoxEditor.AppendText(space + "ITEM_TYPE " + commandList[4] + " = " + commandList[2] + "( ITEM " + commandList[3] + " );\n");
                    break;
                case "StoreMNPokèmon":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKEMON", "NOR");
                    scriptBoxEditor.AppendText(space + "POKEMON " + commandList[3] + " = " + commandList[2] + "( MOVE_ID " + getTextFromCondition(commandList[4], "MOV") + " );\n");
                    break;
                case "StoreMoveDeleter":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MOVE_DELETE", "NOR");
                    scriptBoxEditor.AppendText(space + "MOVE_DELETE " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StorePalParkPoint":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POINT", "NOR");
                    scriptBoxEditor.AppendText(space + "POINT " + commandList[4] + " = " + commandList[2] + "( TYPE " + commandList[3] + ");\n");
                    break;
                case "StorePalParkPokèmonCaught":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKE_CAUGHT", "NOR");
                    scriptBoxEditor.AppendText(space + "POKE_CAUGHT " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StorePictureName":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + commandList[2] + "( " + varString + " );\n");
                    break;
                case "StorePokèContestFashion":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 5);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "FASHION_LEVEL", "NOR");
                    scriptBoxEditor.AppendText(space + "FASHION_LEVEL " + commandList[5] + " = " + commandList[2] + "( POKEMON " + varString + ", FASHION " + commandList[4] + " );\n");
                    break;
                case "StorePokèdex":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKEDEX_STATUS", "NOR");
                    scriptBoxEditor.AppendText(space + "POKEDEX_STATUS " + commandList[4] + " = " + commandList[2] + "( POKEDEX " + commandList[3] + ");\n");
                    break;
                case "StorePokèLottoNumber":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "LOTTOTICKET_VALUE", "NOR");
                    scriptBoxEditor.AppendText(space + "LOTTOTICKET_VALUE " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StorePokèLottoResults":

                    newVar = checkStored(commandList, 6);
                    newVar2 = checkStored(commandList, 4);
                    newVar3 = checkStored(commandList, 3);
                    newVar4 = checkStored(commandList, 5);

                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 6);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "LOTTONUMBER_CHECK", "NOR");
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar4, "LOTTOPOKE_CHECK", "NOR");
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "LOTTOPOKE_ID", "NOR");
                    scriptBoxEditor.AppendText(space + "LOTTOPOKE_ID " + commandList[3] + ", LOTTONUMBER_CHECK " + commandList[4] + ", LOTTOPOKE_CHECK " + commandList[5] + " = " + commandList[2] + "();\n");
                    break;
                case "StorePokèmonDeleter":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKE", "NOR");
                    scriptBoxEditor.AppendText(space + "POKE " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StorePokèmonHappiness":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HAPPY_LEVEL", "NOR");
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "HAPPY_LEVEL " + commandList[3] + " = " + commandList[2] + "( " + varString + ");\n");
                    break;
                case "StorePokèmonId":

                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "CHOSEN_POKEMON", "NOR");
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "CHOSEN_POKEMON", "NOR");
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "VAR " + commandList[4] + " = StorePokèmonId( " + varString + " );\n");
                    break;
                case "StorePokèmonLevel":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "LEVEL", "NOR");
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "LEVEL " + commandList[3] + " = " + commandList[2] + "( " + varString + ");\n");
                    break;
                case "StorePokèmonPartyAtLevel":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PARTY_NUM", "NOR");
                    scriptBoxEditor.AppendText(space + "PARTY_NUM " + commandList[3] + " = " + commandList[2] + "( LEVEL " + commandList[4] + " );\n");
                    break;
                case "StorePokèmonPartyNumber":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PARTY_NUM", "NOR");
                    scriptBoxEditor.AppendText(space + "PARTY_NUM " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StorePokèmonPartyNumber2":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PARTY_NUM", "NOR");
                    scriptBoxEditor.AppendText(space + "PARTY_NUM " + commandList[3] + " = " + commandList[2] + "( LIMIT " + commandList[4] + " );\n");
                    break;
                case "StorePokèmonPartyNumber(177)":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PARTY_NUM", "NOR");
                    scriptBoxEditor.AppendText(space + "PARTY_NUM " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StorePokèmonPartySpecieNumber":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PARTYSPECIE_NUM", "NOR");
                    scriptBoxEditor.AppendText(space + "PARTYSPECIE_NUM " + commandList[4] + " = " + commandList[2] + "( SPECIE " + commandList[3] + " );\n");
                    break;
                case "StorePokèmonPartySpeciePoffin":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POFFIN", "NOR");
                    scriptBoxEditor.AppendText(space + "POFFIN " + commandList[4] + " = " + commandList[2] + "( SPECIE " + commandList[3] + " );\n");
                    break;
                case "StorePokèmonMoveNumber":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MOVE_NUM", "NOR");
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "MOVE_NUM " + commandList[3] + " = " + commandList[2] + "( " + varString + " );\n");
                    break;
                case "StorePokèmonNature":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "NATURE", "NOR");
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "NATURE " + commandList[3] + " = " + commandList[2] + "( POKE " + varString + " ) ;\n");
                    break;
                case "StorePokèmonNumberCaught":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "CAUGHT_NUM", "NOR");
                    scriptBoxEditor.AppendText(space + "CAUGHT_NUM " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StorePokèmonSize":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "SIZE", "NOR");
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "SIZE " + commandList[3] + " = " + commandList[2] + "( " + varString + ");\n");
                    break;
                case "StorePokèmonSpecie(P)":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "SPECIE", "NOR");
                    scriptBoxEditor.AppendText(space + "SPECIE " + commandList[3] + " = " + commandList[2] + "( POKEMON " + getTextFromCondition(commandList[4], "POK") + " );\n");
                    break;
                case "StoreRandomLevel":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "RANDOM_LEVEL", "NOR");
                    scriptBoxEditor.AppendText(space + "RANDOM_LEVEL " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreRandomPokèmonSearch":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "RANDOM_POKE", "NOR");
                    scriptBoxEditor.AppendText(space + "RANDOM_POKE " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreRibbonNumber":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "RIBBON", "NOR");
                    scriptBoxEditor.AppendText(space + "RIBBON " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreSinglePhraseBoxInput":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "INSERT_CHECK", "NOR");
                    newVar2 = checkStored(commandList, 5);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "WORD", "NOR");
                    scriptBoxEditor.AppendText(space + "INSERT_CHECK " + commandList[4] + ", WORD " + commandList[5] + " = " + commandList[2] + "( " + commandList[3] + " );\n");
                    break;
                case "StoreStarCardNumber":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "STARS_NUM", "NOR");
                    scriptBoxEditor.AppendText(space + "STARS_NUM " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreStarter":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "STARTER", "POK");
                    scriptBoxEditor.AppendText(space + "STARTER  " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreStatusSave":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "SAVE_STATUS", "NOR");
                    scriptBoxEditor.AppendText(space + "SAVE_STATUS " + commandList[3] + "  = " + commandList[2] + "();\n");
                    break;
                case "StoreSwarmInfo":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PLACE", "NOR");
                    newVar2 = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "POKE", "NOR");
                    scriptBoxEditor.AppendText(space + "PLACE " + commandList[3] + ", POKE " + commandList[4] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreTrainerId":
                    scriptBoxEditor.AppendText(space + "If (TRAINER " + commandList[3] + " == INACTIVE);" + "\n");
                    break;
                case "StoreTextVarUnion":
                    newVar = checkStored(commandList, 4);
                    jumpBack = (Int16.Parse(commandList[3])) + 6;
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MESSAGE_" + jumpBack, "NOR");
                    scriptBoxEditor.AppendText(space + "MESSAGE_ID " + commandList[4] + " = " + commandList[2] + "( ID " + commandList[3] + " );\n");
                    break;
                case "StoreTime":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "TIME", "NOR");
                    scriptBoxEditor.AppendText(space + "TIME " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreAddVar":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "VAR " + varString2 + " = " + varString2 + " + " + varString + ";\n");
                    break;
                case "StoreSubVar":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "VAR " + varString2 + " = " + varString2 + " - " + varString + ";\n");
                    break;
                case "StoreVarValue":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    if (!Utils.IsNaturalNumber(varString))
                    {
                        if (cond == "FLA")
                            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FLAG_" + commandList[3], cond);
                        else
                            addToVarNameDictionary(varNameDictionary, varLevel, newVar, varString, cond);
                    }
                    varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    if (cond == "FLA")
                        scriptBoxEditor.AppendText(space + "VAR_FLAG " + varString2 + " = " + (Boolean.Parse(commandList[4])).ToString() + "\n");
                    else
                        scriptBoxEditor.AppendText(space + "VAR " + commandList[3] + " = " + varString + "\n");
                    break;
                case "StoreVarVariable":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    if (!Utils.IsNaturalNumber(commandList[4]))
                    {
                        if (cond == "FLA")
                            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FLAG_" + commandList[3], cond);
                        else
                            addToVarNameDictionary(varNameDictionary, varLevel, newVar, commandList[3], cond);
                    }
                    varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    if (cond == "FLA")
                        scriptBoxEditor.AppendText(space + "VAR_FLAG " + varString2 + " = " + (Boolean.Parse(commandList[4])).ToString() + "\n");
                    else
                        scriptBoxEditor.AppendText(space + "VAR " + varString2 + " = " + commandList[4] + "\n");
                    break;
                case "StoreVarPartId":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PART_ID", "NOR");
                    scriptBoxEditor.AppendText(space + "#DEFINE VAR " + commandList[3] + " AS PART_ID " + ";\n");
                    break;
                case "StoreTypeBattle?":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BATTLE_TYPE", "NOR");
                    scriptBoxEditor.AppendText(space + "BATTLE_TYPE " + commandList[3] + "  = " + commandList[2] + "();\n");
                    break;
                case "StoreUnionAlterChoice":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ALTER_CHOICE", "NOR");
                    scriptBoxEditor.AppendText(space + "ALTER_CHOICE " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreWirelessBattleType":
                    newVar = checkStored(commandList, 6);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BATTLE_TYPE", "NOR");
                    scriptBoxEditor.AppendText(space + "BATTLE_TYPE " + commandList[6] + " = " + commandList[2] + "( " + commandList[3] + ", " + commandList[4] + ", " + commandList[5] + " );\n");
                    break;
                case "SetVarBattle2?":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "TRAINER_" + commandList[3], "NOR");
                    scriptBoxEditor.AppendText(space + "TRAINER " + commandList[4] + " = " + commandList[2] + "( TRAINER_" + commandList[3] + " );\n");
                    break;
                case "TakeItem":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + commandList[2] + "( ITEM " + varString + " , NUMBER " + varString2 + ", " + commandList[5] + " );\n");
                    break;
                case "TakeMoney":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + commandList[2] + "( MONEY " + varString + ", " + commandList[4] + " );\n");
                    break;
                case "TeachPokèmonMove":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + commandList[2] + "( " + varString + ", MOVE " + varString2 + " );\n");
                    break;
                case "TradePokèmon":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + commandList[2] + "( " + varString + " );\n");
                    break;
                case "TrainerBattle":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + commandList[2] + "( TRAINER " + varString + ", " + commandList[4] + " );\n");
                    break;
                case "YesNoBox":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "YESNO_RESULT", "YNO");
                    scriptBoxEditor.AppendText(space + "YESNO_RESULT  " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "WaitFanfare":
                    newVar = checkStored(commandList, 3);
                    scriptBoxEditor.AppendText(space + commandList[2] + "( MUSIC_ID " + commandList[3] + " );\n");
                    break;

                default:
                    //for (int i = 3; i < commandList.Length ; i++)
                    //    if (commandList[i]!="")
                    //        scriptBoxEditor.AppendText(space + "P_" + (i - 2) + " = " + commandList[i] + ";\n");
                    scriptBoxEditor.AppendText(space + commandList[2] + "(");
                    for (int i = 3; i < commandList.Length - 2; i++)
                        if (commandList[i] != "")
                        {

                            newVar2 = checkStored(commandList, i);
                            varString = getStoredMagic(varNameDictionary, varLevel, commandList, i);
                            scriptBoxEditor.AppendText(" " + varString + " ,");
                        }
                    if (commandList[commandList.Length - 2] != "" && commandList.Length > 4)
                    {
                        newVar2 = checkStored(commandList, commandList.Length - 2);
                        varString = getStoredMagic(varNameDictionary, varLevel, commandList, commandList.Length - 2);
                        scriptBoxEditor.AppendText(" " + varString);
                    }

                    scriptBoxEditor.AppendText(");\n");
                    break;
            }

        }

        private string getTextFromVar(RichTextBox scriptBoxEditor, string[] commandList, string varString, string text)
        {
            if (varString.Contains("M"))
            {
                var id = varString.Split('_')[1];
                text = textFile.textList[Int32.Parse(id)].text;
            }
            else if (varString.Contains("VAR"))
            {
                for (int i = scriptBoxEditor.Lines.Length - 1; i > 0 && text == ""; i--)
                {
                    var line3 = scriptBoxEditor.Lines[i];
                    if (line3.Contains("VAR " + commandList[3]))
                    {
                        var id = line3.Split('=')[1];
                        var fixId = id.ToString().TrimStart(' ');
                        text = textFile.textList[Int16.Parse(fixId)].text;
                    }
                }
            }
            else
            {
                if (mulActive)
                    text = multiFile.textList[Int16.Parse(varString)].text;
                else
                    text = textFile.textList[Int16.Parse(varString)].text;
                mulString.Add(text);
                scriptBoxEditor.AppendText(" = " + text + " ");
            }
            return text;
        }

        private string getVarString(RichTextBox scriptBoxEditor, Texts textFile, string[] commandList, string varString, string text)
        {
            if (textFile == null)
                return "";
            try
            {
                var findValue = 0;
                if (varString.Contains("M"))
                {
                    var id = varString.Split('_')[1];
                    text = textFile.textList[Int32.Parse(id)].text;
                }
                else if (varString.Contains("VAR") || varString.Contains("0x"))
                {
                    var pos = scriptBoxEditor.GetLineFromCharIndex(scriptBoxEditor.GetFirstCharIndexOfCurrentLine());
                    var id = "";
                    getVarId(scriptBoxEditor, commandList, ref findValue, pos, ref id);
                    if (id != "")
                        scriptBoxEditor.AppendText(" (" + id + ") ");
                    if (textFile != null)
                        text = textFile.textList[Int32.Parse(id)].text;
                }
                else
                    text = textFile.textList[Int16.Parse(varString)].text;
            }
            catch (FormatException f)
            {
                return "";
            }
            return text;
        }

        private void getVarId(RichTextBox scriptBoxEditor, string[] commandList, ref int findValue, int pos, ref string id)
        {
            for (int i = pos - 1; i > 0 && findValue == 0; i--)
            {
                var line3 = scriptBoxEditor.Lines[i];
                if (line3.Contains("VAR " + commandList[3]))
                {
                    findValue = 1;
                    var lineVector = line3.Split(' ');
                    var lineLength = lineVector.Length;
                    id = lineVector[lineLength - 1];
                    if (id == ";")
                        id = lineVector[lineLength - 1].Substring(0, id.Length - 2);
                }
            }
        }

        private void readMessage(RichTextBox scriptBoxEditor, string[] commandList, int start)
        {
            if (commandList.Length > start - 1)
                for (int i = start; i < commandList.Length; i++)
                    scriptBoxEditor.AppendText(commandList[i] + " ");
        }

        private void readFunction(string[] scriptsLine, int lineCounter, string space, ref int functionLineCounter, ref string line2, List<int> visitedLine)
        {
            functionLineCounter++;
            var stringL = scriptsLine[lineCounter - 1];
            if (/*(space.Length>40 || scriptBoxEditor.Lines.Length>3000) &&*/ visitedLine.Contains(functionLineCounter) && !stringL.Contains("StoreVarValue"))
            {
                var offset = scriptsLine[lineCounter].Split(' ');
                if (offset.Length > 7)
                    scriptBoxEditor.AppendText("       " + space + "Jump to Offset: " + offset[6].TrimStart('(') + "\n");
                else if (offset.Length > 5)
                    scriptBoxEditor.AppendText("       " + space + "Jump to Offset: " + int.Parse(offset[4].TrimStart('0').TrimStart('x'), System.Globalization.NumberStyles.HexNumber) + "\n");
                else
                    scriptBoxEditor.AppendText("       " + space + "Jump to Offset: " + int.Parse(offset[3].TrimStart('0').TrimStart('x'), System.Globalization.NumberStyles.HexNumber) + "\n");
                scriptBoxEditor.AppendText(space + "}\n");
                return;
            }
            visitedLine.Add(functionLineCounter);
            readFunctionSimplified(scriptsLine, space, ref functionLineCounter, ref line2, visitedLine);
            scriptBoxEditor.AppendText(space + "}\n");
            return;
        }

        private void readFunctionSimplified(string[] scriptsLine, string space, ref int functionLineCounter, ref string line2, List<int> visitedLine)
        {
            do
            {
                line2 = scriptsLine[functionLineCounter];
                if (line2.Length > 1)
                {
                    if (line2.StartsWith("="))
                        return;
                    try
                    {
                        GetCommandSimplifiedDPP(scriptsLine, functionLineCounter, space + "   ", visitedLine);
                    }
                    catch
                    {
                    }
                }
                functionLineCounter++;
                if ((line2.Contains(" End ") || line2.Contains("KillScript") || (line2.Contains("Jump") && scriptsLine[functionLineCounter] == "")))
                    return;

            } while (functionLineCounter < scriptsLine.Length);

        }

        private string getCondition(string[] scriptsLine, int lineCounter)
        {
            var condition = scriptsLine[lineCounter + 1].Split(' ')[3];
            if (condition == "EQUAL")
                condition = "==";
            if (condition == "BIGGER")
                condition = ">";
            if (condition == "LOWER")
                condition = "<";
            if (condition == "BIGGER/EQUAL")
                condition = ">=";
            if (condition == "LOWER/EQUAL")
                condition = "<=";
            if (condition == "DIFFERENT")
                condition = "!=";
            return condition;
        }

        private string getStoredMagic(List<Dictionary<int, string>> varNameDictionary, int varLevel, string[] commandList, int id)
        {
            int var = checkStored(commandList, id);
            var varString = "";
            if (commandList != null)
                varString = commandList[id];
            else
            {
                var = (Int32.Parse(temp));
                varString = "0x" + var.ToString("X");
            }

            for (int i = varLevel; i < varNameDictionary.Count; i++)
            {
                var nameDictionary = varNameDictionary[i];
                if (nameDictionary.ContainsKey(var))
                    return getVarStringFromDict(var, ref varString, nameDictionary);
            }
            for (int i = varLevel; i >= 0; i--)
            {
                var nameDictionary = varNameDictionary[i];
                if (nameDictionary.ContainsKey(var))
                    return getVarStringFromDict(var, ref varString, nameDictionary);
            }
            return varString;
        }

        private string getVarStringFromDict(int var, ref string varString, Dictionary<int, string> nameDictionary)
        {
            varString = nameDictionary[var];
            conditionType = varString.Substring(varString.Length - 3, 3);
            cond = conditionType;
            varString = varString.Substring(0, varString.Length - 3);
            return varString;
        }

        private string getTextFromCondition(string varString, string condition)
        {
            //if (!IsNaturalNumber(varString))
            //    return varString;
            if (condition == "YNO")
            {
                if (varString == "0")
                {
                    return "YES";
                }
                if (varString == "1")
                {
                    return "NO";
                }
            }
            if (condition == "FLA")
            {
                int flag = Int32.Parse(varString);
                if (scriptType == Constants.DPSCRIPT)
                {
                    switch (flag)
                    {
                        case 1:
                            return "'NPC_FIRST_TALK'";
                        case 131:
                            return "'GIVE_ITEM(131)";
                        case 241:
                            return "'TRAINER_SCHOOL'";
                        case 243:
                            return "'POKEKRON_CAMPAIGN'";
                        case 245:
                            return "'TRADE_POKEMON(245)'";
                        case 246:
                            return "'FOREIGN_POKEDEX'";
                        default:
                            return flag.ToString();
                    }
                }

            }

            if (condition == "GEN")
            {
                if (varString == "0")
                    return "MALE";
                if (varString == "1")
                    return "FEMALE";
            }
            if (condition == "DAY")
            {
                if (varString == "0")
                    return "SUNDAY";
                if (varString == "1")
                    return "MONDAY";
                if (varString == "2")
                    return "TUESDAY";
                if (varString == "3")
                    return "WEDNESDDAY";
                if (varString == "4")
                    return "THURSDAY";
                if (varString == "5")
                    return "FRIDAY";
                if (varString == "6")
                    return "SATURDAY";
            }
            if (condition == "GEN")
            {
                if (varString == "0")
                    return "MALE";
                if (varString == "1")
                    return "FEMALE";
            }
            if (condition == "BOL")
            {
                if (varString == "0")
                    return "TRUE";
                if (varString == "1")
                    return "FALSE";
            }
            if (condition == "BAD")
            {
                if (scriptType == Constants.DPSCRIPT)
                    varString = getText(varString, textNarc, 331) + " " + varString;
            }
            if (condition == "POK")
            {
                if (scriptType == Constants.DPSCRIPT)
                    varString = getText(varString, textNarc, 362);
                else if (scriptType == Constants.PLSCRIPT)
                    varString = getText(varString, textNarc, 412);
            }
            if (condition == "ITE")
            {
                if (scriptType == 0)
                    varString = getText(varString, textNarc, 347);
                else if (scriptType == Constants.PLSCRIPT)
                    varString = getText(varString, textNarc, 392);
            }
            if (condition == "MOV")
            {
                if (scriptType == Constants.DPSCRIPT)
                    varString = getText(varString, textNarc, 347);
                else if (scriptType == Constants.PLSCRIPT)
                    varString = getText(varString, textNarc, 647);
                else
                    varString = getText(varString, bwTextNarc, 286);
            }

            if (condition == "ACC")
                varString = getText(varString, textNarc, 338);
            if (condition == "MUL")
            {
                short result = 0;
                Int16.TryParse(varString, out result);
                if (result != 0)
                {
                    var id = Int16.Parse(varString);
                    if (id < mulString.Count && id > 0)
                        varString = mulString[id];
                }

            }
            return varString;
        }

        private string getText(string varString, PG4Map.Narc textNarc, int index)
        {
            var textHandler = new Texts();
            var reader = new BinaryReader(textNarc.figm.fileData[index]);
            reader.BaseStream.Position = 0;
            textHandler.readText(reader, new RichTextBox());
            var id = Int16.Parse(varString);
            varString = "' " + textHandler.textList[id].text + " '";
            return varString;
        }

        private int checkStored(string[] commandList, int id)
        {
            if (commandList == null || id > commandList.Length)
                return 0;
            var intVar = 0;
            if (commandList[id].Contains("0x"))
            {
                var trimmedVar = commandList[id].Substring(2, commandList[id].Length - 2);
                intVar = Int32.Parse(trimmedVar, System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            else
                intVar = Int32.Parse(commandList[id]);
            return intVar;
        }

        private void addToVarNameDictionary(List<Dictionary<int, string>> varNameDictionary, int varLevel, int newVar, string name, string conditionType)
        {
            if (varNameDictionary[varLevel].ContainsKey(newVar))
                varNameDictionary[varLevel].Remove(newVar);
            varNameDictionary[varLevel].Add(newVar, name + conditionType);
        }

        public void SetSimplifierFile(Texts multiFile, List<Dictionary<int, string>> varNameDictionaryList, PG4Map.Narc textNarc, Texts textFile, RichTextBox scriptBoxViewer, RichTextBox scriptBoxEditor, int p)
        {
            this.multiFile = multiFile;
            this.varNameDictionary = varNameDictionaryList;
            this.textNarc = textNarc;
            this.textFile = textFile;
            this.scriptBoxViewer = scriptBoxViewer;
            this.scriptBoxEditor = scriptBoxEditor;
            this.varLevel = p;
        }

        public void PrintCommand(RichTextBox scriptBox, Scripts.Commands_s command, List<uint> scriptOrder)
        {
            int idMessage = 0;
            switch (command.Name.Split('(')[0])
            {
                case "ApplyMovement":
                    scriptBox.AppendText(StoreHex((int)command.parameters[0]) + SetFunction(StoreHex((int)command.parameters[1]), scriptOrder));
                    break;
                case "CompareStackCondition":
                    scriptBox.AppendText(StoreConditionCommand(command.parameters[0]));
                    break;
                case "When":
                case "If":
                    scriptBox.AppendText(StoreConditionCommand(command.parameters[0]) + SetFunction(StoreHex((int)command.parameters[1]), scriptOrder));
                    break;
                case "Jump":
                case "Goto":
                    scriptBox.AppendText(SetFunction(StoreHex((int)command.parameters[0]), scriptOrder));
                    break;
                case "Message":
                    if (command.parameters[0] < 0x4000)
                        idMessage = (int)command.parameters[0];
                    if (textFile != null && idMessage < textFile.messageList.Count)
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]) + "= ' " + textFile.messageList[idMessage] + " '");
                    else
                        scriptBox.AppendText(StoreHex((int)command.parameters[0]));
                    break;
                default:
                    if (command.parameters != null)
                    {
                        foreach (int parameter in command.parameters)
                            scriptBox.AppendText(StoreHex(parameter));
                    }
                    break;
            }
            scriptBox.AppendText("\n");
        }

        private string SetFunction(string parameter, List<uint> scriptOrder)
        {

            for (int i = 0; i < scriptOrder.Count; i++)
            {
                var actualOffset = scriptOrder[i].ToString() + " ";
                if (parameter == actualOffset)
                    return " Script " + i.ToString() + " (" + parameter + ")";
            }

            if (functionOffsetList != null)
            {
                for (int i = 0; i < functionOffsetList.Count; i++)
                {
                    var actualOffset = functionOffsetList[i].ToString() + " ";
                    if (parameter == actualOffset)
                        return " Function " + i.ToString() + " (" + parameter + ")";
                }
            }
            if (movOffsetList != null)
            {
                for (int i = 0; i < movOffsetList.Count; i++)
                {
                    var actualOffset = movOffsetList[i].ToString() + " ";
                    if (parameter == actualOffset)
                        return " Movement " + i.ToString() + " (" + parameter + ")";
                }
            }

            return " " + parameter;
        }

        private string StoreHex(int parameter)
        {
            if (parameter < 0x3000)
                return parameter.ToString() + " ";
            else
                return "0x" + parameter.ToString("X") + " ";
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
            if (p == 6) return "AND";
            if (p == 7) return "OR";
            return p.ToString();
        }


    }
}
