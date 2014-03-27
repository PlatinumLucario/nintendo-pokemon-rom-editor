using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using PG4Map.Formats;

namespace NPRE.Formats.Specific.Pokémon.Scripts
{
	public class HGSSCommandHandler
	{
        private int scriptType;
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
private  int newVar;
private  int newVar2;
private  int varLevel;
private  RichTextBox scriptBoxViewer;
private  string temp;


        public Scripts.Commands_s readCommandHGSS(BinaryReader reader, Scripts.Commands_s com, List<uint> movOffsetList, List<uint> functionOffsetList, List<uint> scriptOrder)
        {
            uint functionOffset = 0;
            switch (com.Id)
            {
                case 0x2:
                    com.Name = "End";
                    com.isEnd = 1;
                    functionOffset = (uint)reader.BaseStream.Position;
                    checkNextFunction(reader, functionOffset, scriptOrder);
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
                    checkNextFunction(reader, functionOffset, scriptOrder);
                    com.numJump++;
                    var isEndC = reader.ReadUInt16();
                    if (isEndC != 2)
                    {
                        com.isEnd = 1;
                        checkNextFunction(reader, (uint)reader.BaseStream.Position - 2, scriptOrder);
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
                    checkNextFunction(reader, functionOffset, scriptOrder);
                    break;

                case 0x1C:
                    com.Name = "If";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[1];
                    checkNextFunction(reader, functionOffset, scriptOrder);
                    break;
                case 0x1D:
                    com.Name = "If2";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[1];
                    checkNextFunction(reader, functionOffset, scriptOrder);
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

        public void writeCommandHGSS(BinaryWriter writer, string[] wordLine)
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

       public void getCommandSimplifiedHGSS(string[] scriptsLine, int lineCounter, string space, List<int> visitedLine)
        {
            var line = scriptsLine[lineCounter];
            var commandList = line.Split(' ');
            string movId;
            string tipe;
            int offset = Int32.Parse(commandList[1].Substring(0, commandList[1].Length - 1));
            var stringOffset = commandList[1];
            if (offset < 10)
                stringOffset = "000" + stringOffset;
            else if (offset > 10 && offset < 100)
                stringOffset = "00" + stringOffset;
            else if (offset > 100 && offset < 1000)
                stringOffset = "0" + stringOffset;
            switch (commandList[2])
            {
                //    case "1B8":
                //        newVar = checkStored(commandList, 3);
                //        var varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                //        scriptBoxEditor.AppendText(space + "" + commandList[2] + "( UNK " + commandList[3] + " , MESSAGE_ID " + commandList[4] + " ");
                //        var text = "";
                //        if (varString.Contains("M"))
                //        {
                //            var id = varString.Split('_')[1];
                //            text = textFile.textList[Int32.Parse(id)].text;
                //        }
                //        else
                //            text = textFile.textList[Int16.Parse(varString)].text;
                //        scriptBoxEditor.AppendText(" = " + text + " ");
                //        scriptBoxEditor.AppendText(");\n");
                //        break;
                case "1CD":
                    scriptBoxEditor.AppendText(space + commandList[2] + "( " + commandList[3] +
                                               " ," + commandList[4] + " , TRAINER " + commandList[5] + ", " + commandList[6] + ", " + commandList[7] + " );\n");
                    break;
                case "1E0":
                    var varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "1E0_STATUS", "BOL");
                    scriptBoxEditor.AppendText(space + "1E0_STATUS " + commandList[3] + " = " + commandList[2] + "( " + varString + " , " + commandList[5] + " );\n");
                    break;
                case "1E4":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "1E4_VALUE", "NOR");
                    scriptBoxEditor.AppendText(space + "1E4_VALUE " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "1EA":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "1EA_STATUS_" + commandList[3], "BOL");
                    scriptBoxEditor.AppendText(space + "1EA_STATUS " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "20A":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "20A_VALUE", "NOR");
                    scriptBoxEditor.AppendText(space + "20A_VALUE " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "22D":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 4), "22D_STATUS", "NOR");
                    scriptBoxEditor.AppendText(space + "22D_STATUS " + commandList[4] + " = " + commandList[2] + "( " + varString + " );\n");
                    break;
                case "238":
                    scriptBoxEditor.AppendText(space + "VAR " + commandList[4] + " = " + commandList[2] + "(P_1 " + commandList[3] + ");\n");
                    break;
                case "28C":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 5), "28C_STATUS", "NOR");
                    scriptBoxEditor.AppendText(space + "28C_STATUS " + commandList[5] + " = " + commandList[2] + "( " + varString + " , " + commandList[4] + " );\n");
                    break;
                case "28D":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 6), "28D_STATUS", "NOR");
                    scriptBoxEditor.AppendText(space + "28D_STATUS " + commandList[5] + " = " + commandList[2] + "( " + varString + " , " + commandList[4] + " );\n");
                    break;
                case "28F":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 4), "28F_STATUS", "NOR");
                    scriptBoxEditor.AppendText(space + "28F_STATUS " + commandList[4] + " = " + commandList[2] + "( " + varString + " );\n");
                    break;
                case "2AD":
                    newVar = Int32.Parse(commandList[3].Substring(2, commandList[3].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    newVar2 = Int32.Parse(commandList[4].Substring(2, commandList[4].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "OW_ID", "NOR");
                    scriptBoxEditor.AppendText(space + "UNK " + commandList[3] + ", OW_ID " + commandList[4] + " = " + commandList[2] + "();\n");
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
                case "CallMessageBox":
                    // scriptBoxEditor.AppendText(space + "BORDER " + commandList[5] + " = " + commandList[2] + "( MESSAGE_ID " + commandList[3] +
                    //           ", TYPE " + commandList[4] + " ");
                    var text = "";
                    //if (Int16.Parse(commandList[3]) < textFile.textList.Count)
                    //{
                    //    text = textFile.textList[Int16.Parse(commandList[3])].text;
                    //    scriptBoxEditor.AppendText(" = " + text + " ");
                    //}
                    scriptBoxEditor.AppendText(" );\n");
                    break;
                case "CallTextMessageBox":
                    scriptBoxEditor.AppendText(space + commandList[2] + "( MESSAGE_ID " + commandList[3] + ", " + commandList[4] +
                               " ");
                    text = "";
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

                case "CheckBadge":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BADGE_STATUS", "NOR");
                    scriptBoxEditor.AppendText(space + "BADGE_STATUS " + commandList[4] + "= " + commandList[2] + "( BADGE " + commandList[3] + ");\n");
                    break;
                case "CheckBike":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ON_BIKE", "NOR");
                    scriptBoxEditor.AppendText(space + "ON_BIKE " + commandList[4] + "= " + commandList[2] + "( BADGE " + commandList[3] + ");\n");
                    break;
                case "CheckBoxSpace":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BOX_SPACE", "NOR");
                    scriptBoxEditor.AppendText(space + "BOX_SPACE " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "CheckCasinoPrizeCoins":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "COINSBOX_SPACE", "NOR");
                    scriptBoxEditor.AppendText(space + "COINSBOX_SPACE " + commandList[3] + " = " + commandList[2] + "( PRIZE " + varString + " );\n");
                    break;
                case "CheckCoinsBoxSpace":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "COINSBOX_SPACE", "NOR");
                    scriptBoxEditor.AppendText(space + "COINSBOX_SPACE " + commandList[3] + " = " + commandList[2] + "( AMOUNT " + commandList[4] + " , " + commandList[5] + " );\n");
                    break;
                case "CheckContestPicture":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "CONTESTPICTURE_STATUS", "NOR");
                    scriptBoxEditor.AppendText(space + "CONTESTPICTURE_STATUS " + commandList[4] + " = " + commandList[2] + "( PICTURE " + commandList[3] + " );\n");
                    break;
                case "CheckHoney":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HONEY", "NOR");
                    scriptBoxEditor.AppendText(space + "HONEY " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "CheckItem":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEM_RECEIVED", "NOR");
                    scriptBoxEditor.AppendText(space + "ITEM_RECEIVED " + commandList[4] + " = " + commandList[2] + "( ITEM " + commandList[3] + " );\n");
                    break;
                case "CheckItemBagSpace":
                    newVar = checkStored(commandList, 5);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEMBAG_SPACE", "NOR");
                    scriptBoxEditor.AppendText(space + "ITEMBAG_SPACE " + commandList[5] + " = " + commandList[2] + "( ITEM " + commandList[3] + " , " + "NUMBER " + commandList[4] + " );\n");
                    break;
                case "CheckItemBagNumber":
                    newVar = checkStored(commandList, 5);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEMBAG_NUMBER", "NOR");
                    scriptBoxEditor.AppendText(space + "ITEMBAG_NUMBER " + commandList[5] + " = " + commandList[2] + "( ITEM " + commandList[3] + " , " + "NUMBER " + commandList[4] + " );\n");
                    break;
                case "CheckDressPicture":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "DRESSPICTURE_STATUS", "NOR");
                    scriptBoxEditor.AppendText(space + "DRESSPICTURE_STATUS " + commandList[4] + " = " + commandList[2] + "( PICTURE " + commandList[3] + " );\n");
                    break;
                case "CheckErrorSave":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ERROR", "NOR");
                    scriptBoxEditor.AppendText(space + "ERROR " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "CheckMoney":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MONEY_STATUS", "NOR");
                    scriptBoxEditor.AppendText(space + "MONEY_STATUS " + commandList[3] + " = " + commandList[2] + "( AMOUNT " + commandList[4] + ", " + commandList[5] + " );\n");
                    break;
                case "CheckPhotoSpace":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ALBUM_STATUS", "NOR");
                    scriptBoxEditor.AppendText(space + "ALBUM_STATUS " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "CheckPoisoned":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POISONED", "NOR");
                    newVar2 = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "POKEMON", "NOR");
                    scriptBoxEditor.AppendText(space + "POISONED " + commandList[3] + " = " + commandList[2] + "( POKEMON " + commandList[4] + " );\n");
                    break;
                case "CheckPokèKron":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PKRON_STATUS", "NOR");
                    scriptBoxEditor.AppendText(space + "PKRON_STATUS " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "CheckPokèKronApplication":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PKRONAPP_STATUS", "NOR");
                    scriptBoxEditor.AppendText(space + "PKRONAPP_STATUS " + commandList[4] + " = " + commandList[2] + "( PKRONAPP " + commandList[3] + ");\n");
                    break;
                case "CheckPokèmonCaught":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKE_CAUGHT", "NOR");
                    scriptBoxEditor.AppendText(space + "POKE_CAUGHT " + commandList[4] + " = " + commandList[2] + "( POKEMON " + commandList[3] + ");\n");
                    break;
                case "CheckPokèMoveTeacherCompatibility":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKE_COMPATIBLE", "NOR");
                    scriptBoxEditor.AppendText(space + "POKE_COMPATIBLE " + commandList[3] + " = " + commandList[2] + "( POKEMON " + commandList[4] + " );\n");
                    break;
                case "CheckPokèrus":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKERUS", "NOR");
                    scriptBoxEditor.AppendText(space + "POKERUS " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "CheckSpecialItem":
                    newVar = checkStored(commandList, 4);
                    newVar2 = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "SPECIAL_ITEM", "NOR");
                    scriptBoxEditor.AppendText(space + "SPECIAL_ITEM " + commandList[4] + " = " + commandList[2] + "( ITEM " + commandList[3] + " );\n");
                    break;
                case "ChooseUnion":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "UNION_DECISION", "NOR");
                    scriptBoxEditor.AppendText(space + "UNION_DECISION " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "ChooseWifiSprite":
                    newVar = checkStored(commandList, 4);
                    newVar2 = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "WIFI_SPRITE_CHOSEN", "NOR");
                    scriptBoxEditor.AppendText(space + "WIFI_SPRITE_CHOSEN " + commandList[4] + " = " + commandList[2] + "( " + varString + " );\n");
                    break;
                case "ClearFlag":
                    var statusFlag = "FALSE";
                    if (commandList[4] == "1")
                        statusFlag = "TRUE";
                    scriptBoxEditor.AppendText(space + "FLAG " + commandList[3] +
                                               " = " + statusFlag + ";\n");
                    break;
                case "ClearTrainerId":
                    scriptBoxEditor.AppendText(space + "TRAINER " + commandList[3] + " = INACTIVE;" + "\n");
                    break;
                case "Compare":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    var varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    var condition = getCondition(scriptsLine, lineCounter);
                    scriptBoxEditor.AppendText(space + "If( " + varString + " " + condition + " " + varString2 + ")\n");
                    break;
                case "Compare2":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
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
                case "Multi(2ED)":
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
                case "SetVar(28)":
                    newVar = checkStored(commandList, 4);
                    newVar2 = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    if (!Utils.IsNaturalNumber(commandList[4]))
                        addToVarNameDictionary(varNameDictionary, varLevel, newVar, varString, cond);
                    scriptBoxEditor.AppendText(space + "VAR " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVar(29)":
                    newVar = checkStored(commandList, 4);
                    newVar2 = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    if (!Utils.IsNaturalNumber(commandList[4]))
                        addToVarNameDictionary(varNameDictionary, varLevel, newVar, varString, cond);
                    scriptBoxEditor.AppendText(space + "VAR " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVar(2A)":
                    newVar = checkStored(commandList, 4);
                    newVar2 = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    if (!Utils.IsNaturalNumber(commandList[4]))
                        addToVarNameDictionary(varNameDictionary, varLevel, newVar, varString, cond);
                    scriptBoxEditor.AppendText(space + "VAR " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVar(2B)":
                    newVar = checkStored(commandList, 4);
                    newVar2 = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    if (!Utils.IsNaturalNumber(commandList[4]))
                        addToVarNameDictionary(varNameDictionary, varLevel, newVar, varString, cond);
                    scriptBoxEditor.AppendText(space + "VAR " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "Cry":
                    newVar = checkStored(commandList, 3);
                    scriptBoxEditor.AppendText(space + commandList[2] + "( POKE " + commandList[3] + " );\n");
                    break;
                case "DoubleTrainerBattle":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + commandList[2] + "( OTHER " + varString + ", OPPONENT " + commandList[4] + ", OPPONENT " + commandList[5] + " );\n");
                    break;
                case "Fanfare":
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MUSIC_ID " + commandList[3] + ");\n");
                    break;
                case "GiveCoins":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + commandList[2] + "( AMOUNT " + varString + " );\n");
                    break;
                case "GiveItem":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
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
                case "If2":
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
                    newVar2 = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + commandList[2] + "( POKEMON " + varString + " , AMOUNT " + commandList[3] + " );\n");
                    break;
                case "Jump":
                    scriptBoxEditor.AppendText(space + "Jump\n" + space + "{\n");
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
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + " ");
                    text = "";
                    text = getVarString(scriptBoxEditor, textFile, commandList, varString, text);
                    if (text != "")
                        scriptBoxEditor.AppendText(" = " + text + " ");
                    scriptBoxEditor.AppendText(");\n");
                    break;
                case "Message(2D)":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + " ");
                    text = "";
                    text = getVarString(scriptBoxEditor, textFile, commandList, varString, text);
                    if (text != "")
                        scriptBoxEditor.AppendText(" = " + text + " ");
                    scriptBoxEditor.AppendText(");\n");
                    break;
                case "Message(2E)":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + " ");
                    text = "";
                    text = getVarString(scriptBoxEditor, textFile, commandList, varString, text);
                    if (text != "")
                        scriptBoxEditor.AppendText(" = " + text + " ");
                    scriptBoxEditor.AppendText(");\n");
                    break;
                case "Message(2F)":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + " ");
                    text = "";
                    text = getVarString(scriptBoxEditor, textFile, commandList, varString, text);
                    if (text != "")
                        scriptBoxEditor.AppendText(" = " + text + " ");
                    scriptBoxEditor.AppendText(");\n");
                    break;
                case "Message3":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + " ");
                    text = "";
                    text = getVarString(scriptBoxEditor, textFile, commandList, varString, text);
                    if (text != "")
                        scriptBoxEditor.AppendText(" = " + text + " ");
                    scriptBoxEditor.AppendText(");\n");
                    break;
                case "MessageBattle":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( TRAINER_ID " + varString + ", MESSAGE_ID " + commandList[4] + ");\n");
                    break;
                case "Multi":
                    newVar = checkStored(commandList, 7);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MULTI_CHOSEN", "NOR");
                    scriptBoxEditor.AppendText(space + "MULTI_CHOSEN " + commandList[7] + " = " + commandList[2] + "(X " + commandList[3] + " , " +
                        "Y " + commandList[4] + " , " +
                        "CURSOR " + commandList[5] +
                        " , " + commandList[6] +
                        ");\n");
                    break;
                case "Multi2":
                    newVar = checkStored(commandList, 7);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MULTI_CHOSEN", "NOR");
                    scriptBoxEditor.AppendText(space + "MULTI_CHOSEN " + commandList[7] + " = " + commandList[2] + "(X " + commandList[3] + " , " +
                        "Y " + commandList[4] + " , " +
                        "CURSOR " + commandList[5] +
                        " , " + commandList[6] +
                        ");\n");
                    break;
                case "Multi3":
                    newVar = checkStored(commandList, 7);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MULTI_CHOSEN", "NOR");
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
                    scriptBoxEditor.AppendText(space + "FLAG " + commandList[3] + " = TRUE;" + "\n");
                    break;
                case "SetSafariGame":
                    if (commandList[3] == "1")
                        varString = "INACTIVE";
                    else
                        varString = "ACTIVE";
                    scriptBoxEditor.AppendText(space + "SAFARI_GAME = " + varString + " ;\n");
                    break;
                case "SetTextScriptMessageMulti(2EF)":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + ", " + commandList[4] + ", " + commandList[5] + " ");
                    text = "";
                    text = getTextFromVar(scriptBoxEditor, commandList, varString, text);
                    scriptBoxEditor.AppendText(");\n");
                    break;
                case "SetTextScriptMessageMulti":
                    scriptBoxEditor.AppendText(space + commandList[2] + "( BOX_TEXT " + commandList[3] + " , " +
                        "MESSAGEBOX_TEXT  " + commandList[4] + " , " +
                        "SCRIPT " + commandList[5] +
                        ");\n");
                    break;
                case "SetTrainerId":
                    scriptBoxEditor.AppendText(space + "TRAINER " + commandList[3] + " = ACTIVE;" + "\n");
                    break;
                case "SetValue":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    //addToVarNameDictionary(varNameDictionary, varLevel, newVar, "VALUE " + varString2);
                    scriptBoxEditor.AppendText(space + "VALUE " + varString2 + " = " + varString + "\n");
                    break;
                case "SetVar":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    if (!Utils.IsNaturalNumber(commandList[4]))
                        addToVarNameDictionary(varNameDictionary, varLevel, newVar, "NORM_VAR " + commandList[3], cond);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + varString2 + " = " + commandList[4] + "\n");
                    break;
                case "SetVarAccessories":
                    scriptBoxEditor.AppendText(space + "VAR ACC. " + commandList[3] + " = " + commandList[4] + ";\n");
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
                case "SetVarItemType":
                    newVar2 = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR ITEM_TYPE " + commandList[3] + " = " + varString + ";\n");
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
                case "SetVarNickPokémon":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR NICK " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarNumber":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "VAR NUM " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarPoffin":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR POFFIN ITEM " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarPokémon":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR POKE " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarPokèmon(C7)":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR POKE(C7) " + commandList[3] + " = " + varString + ";\n");
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
                case "StartDressPokèmon":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "DRESS_DECISION", "NOR");
                    scriptBoxEditor.AppendText(space + "DRESS_DECISION " + commandList[4] + " = " + commandList[2] + "(" + commandList[3] + " , " + commandList[5] + " );\n");
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
                case "StoreFlag":
                    scriptBoxEditor.AppendText(space + "If( FLAG " + commandList[3] + " == TRUE)\n");
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
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "GENDER", "NOR");
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
                    scriptBoxEditor.AppendText(space + "X,Y = " + commandList[2] + "();\n");
                    break;

                case "StoreItemType":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEM_TYPE", "NOR");
                    scriptBoxEditor.AppendText(space + "ITEM_TYPE " + commandList[4] + " = " + commandList[2] + "( ITEM " + commandList[3] + " );\n");
                    break;
                case "StoreMove":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 5);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MOVE_TEACHED", "NOR");
                    scriptBoxEditor.AppendText(space + "MOVE_TEACHED " + commandList[3] + " = " + commandList[2] + "( MOVE_ID " + commandList[4] + ", " + varString + " );\n");
                    break;
                case "StoreMoveDeleter":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MOVE_DELETE", "NOR");
                    scriptBoxEditor.AppendText(space + "MOVE_DELETE " + commandList[3] + " = " + commandList[2] + "();\n");
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
                case "StorePokèmonPartyNumber3":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PARTY_NUM", "NOR");
                    scriptBoxEditor.AppendText(space + "PARTY_NUM " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;


                case "StorePokèmonMoveNumber":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MOVE_NUM", "NOR");
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "MOVE_NUM " + commandList[3] + " = " + commandList[2] + "( " + varString + ");\n");
                    break;

                case "StorePokèmonSize":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "SIZE", "NOR");
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "SIZE " + commandList[3] + " = " + commandList[2] + "( " + varString + ");\n");
                    break;
                case "StorePokèmonType":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "POKE_TYPE", "NOR");
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + "POKE_TYPE " + commandList[4] + " = " + commandList[2] + "( POKE " + varString + " );\n");
                    break;

                case "StoreSpecificPokèmonParty":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HAVEPARTY_POKE", "NOR");
                    scriptBoxEditor.AppendText(space + "HAVEPARTY_POKE " + commandList[3] + " = " + commandList[2] + "( POKEMON " + commandList[4] + " );\n");
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
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "STARTER", "NOR");
                    scriptBoxEditor.AppendText(space + "STARTER  " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreStatusSave":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "SAVE_STATUS", "NOR");
                    scriptBoxEditor.AppendText(space + "SAVE_STATUS " + commandList[3] + "  = " + commandList[2] + "();\n");
                    break;
                case "StoreTrainerId":
                    scriptBoxEditor.AppendText(space + "If (TRAINER " + commandList[3] + " == INACTIVE);" + "\n");
                    break;
                case "StoreTextVarUnion":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MESSAGE_" + commandList[3], "NOR");
                    scriptBoxEditor.AppendText(space + "MESSAGE_ID " + commandList[4] + " = " + commandList[2] + "( ID " + commandList[3] + " );\n");
                    break;
                case "StoreTime":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "TIME", "NOR");
                    scriptBoxEditor.AppendText(space + "TIME " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreTypeBattle?":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BATTLE_TYPE", "NOR");
                    scriptBoxEditor.AppendText(space + "BATTLE_TYPE " + commandList[3] + "  = " + commandList[2] + "();\n");
                    break;
                case "StoreVersion":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "VERSION", "NOR");
                    scriptBoxEditor.AppendText(space + "VERSION " + commandList[3] + "  = " + commandList[2] + "();\n");
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
                        if (commandList[i] != "" || commandList[i] != "=")
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
                case "CheckFirstTimePokèmonLeague":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "VICTORY_LEAGUE", "NOR");
                    scriptBoxEditor.AppendText(space + "VICTORY_LEAGUE " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "CopyVar2":
                    newVar = checkStored(commandList, 4);
                    newVar2 = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    if (!(Utils.IsNaturalNumber(varString)))
                        addToVarNameDictionary(varNameDictionary, varLevel, newVar2, varString, cond);
                    scriptBoxEditor.AppendText(space + "VAR2 " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "DoubleMessage":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    text = "";
                    if (varString.Contains("M"))
                    {
                        var id = varString.Split('_')[1];
                        text = textFile.textList[Int32.Parse(id)].text;
                    }
                    else
                        text = textFile.textList[Int16.Parse(varString)].text;
                    scriptBoxEditor.AppendText(space + "MESSAGE_MALE = " + text + ";\n");
                    varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    text = "";
                    if (varString2.Contains("M"))
                    {
                        var id = varString2.Split('_')[1];
                        text = textFile.textList[Int32.Parse(id)].text;
                    }
                    else
                        text = textFile.textList[Int16.Parse(varString)].text;
                    scriptBoxEditor.AppendText(space + "MESSAGE_FEMALE = " + text + ";\n");

                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_MALE " + commandList[3] + ", MESSAGE_FEMALE " + commandList[4]);
                    scriptBoxEditor.AppendText(" );\n");
                    break;
                case "LockAll":
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( );\n");
                    break;
                case "SetVarPokèLottoNumber":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR L " + commandList[3] + " = " + commandList[2] + "( " + varString + " , " + commandList[5] + " );\n");
                    break;
                case "SetVarPokèmon":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR POKE " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "ShowYesNoLowScreen":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "YESNOLOW_RESULT", "YNO");
                    scriptBoxEditor.AppendText(space + "YESNOLOW_RESULT  " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreBoxNumber":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BOX_SPACE", "NOR");
                    scriptBoxEditor.AppendText(space + "BOX_SPACE " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StoredoublePhraseBoxInput":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "INSERT_CHECK", "NOR");
                    newVar2 = checkStored(commandList, 5);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "WORD", "NOR");
                    newVar3 = checkStored(commandList, 6);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "WORD_2", "NOR");
                    scriptBoxEditor.AppendText(space + "INSERT_CHECK " + commandList[4] + ", WORD " + commandList[5] + ", WORD_2 " + commandList[6] + " = " + commandList[2] + "( " + commandList[3] + " );\n");
                    break;
                case "StoredoublePhraseBoxInput2":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "INSERT_CHECK", "NOR");
                    newVar2 = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "WORD", "NOR");
                    newVar3 = checkStored(commandList, 5);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "WORD_2", "NOR");
                    newVar4 = checkStored(commandList, 6);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar4, "WORD_3", "NOR");
                    newVar5 = checkStored(commandList, 7);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar5, "WORD_4", "NOR");
                    scriptBoxEditor.AppendText(space + "INSERT_CHECK " + commandList[3] + ", WORD " + commandList[4] + ", WORD_2 " + commandList[5] + ", WORD_3 " + commandList[6] + ", WORD_4 " + commandList[7] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreItem":
                    newVar = checkStored(commandList, 5);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEM_NUMBER", "NOR");
                    scriptBoxEditor.AppendText(space + "ITEM_NUMBER " + commandList[5] + " = " + commandList[2] + "( ITEM " + commandList[3] + " , " + "NUMBER " + commandList[4] + " );\n");
                    break;
                case "StorePhotoName":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + commandList[2] + "( " + varString + " );\n");
                    break;
                case "StorePhotoSpace":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PHOTO_SPACE", "NOR");
                    scriptBoxEditor.AppendText(space + "PHOTO_SPACE " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StorePokèKronApplicationStatus":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PKRONAPP_STATUS", "NOR");
                    scriptBoxEditor.AppendText(space + "PKRONAPP_STATUS " + commandList[4] + " = " + commandList[2] + "( PKRONAPP " + commandList[3] + ");\n");
                    break;
                case "StorePokèmonTrade":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKEMON_TRADE", "NOR");
                    scriptBoxEditor.AppendText(space + "POKEMON_TRADE" + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StorePokèKronStatus":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PKRON_STATUS", "NOR");
                    scriptBoxEditor.AppendText(space + "PKRON_STATUS " + commandList[3] + " = " + commandList[2] + "();\n");
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

        private  void readFunction(string[] scriptsLine, int lineCounter, string space, ref int functionLineCounter, ref string line2, List<int> visitedLine)
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
                       getCommandSimplifiedHGSS(scriptsLine, functionLineCounter, space + "   ", visitedLine);
                    }
                    catch
                    {
                    }
                }
                functionLineCounter++;
                if (line2.Contains(" End ") || line2.Contains("KillScript") || (line2.Contains("Jump")))// && scriptsLine[functionLineCounter] == "")))
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

        //private string getTextFromCondition(string varString, string condition)
        //{
        //    //if (!IsNaturalNumber(varString))
        //    //    return varString;
        //    if (condition == "YNO")
        //    {
        //        if (varString == "0")
        //        {
        //            if (typeROM != 3 || typeROM != 4)
        //                return "YES";
        //            else
        //                return "NO";
        //        }
        //        if (varString == "1")
        //        {
        //            if (typeROM == 3 || typeROM == 4)
        //                return "YES";
        //            else
        //                return "NO";
        //        }
        //    }
        //    if (condition == "FLA")
        //    {
        //        int flag = Int32.Parse(varString);
        //        if (typeROM == DPSCRIPT)
        //        {
        //            switch (flag)
        //            {
        //                case 1:
        //                    return "'NPC_FIRST_TALK'";
        //                case 131:
        //                    return "'GIVE_ITEM(131)";
        //                case 241:
        //                    return "'TRAINER_SCHOOL'";
        //                case 243:
        //                    return "'POKEKRON_CAMPAIGN'";
        //                case 245:
        //                    return "'TRADE_POKEMON(245)'";
        //                case 246:
        //                    return "'FOREIGN_POKEDEX'";
        //                default:
        //                    return flag.ToString();
        //            }
        //        }
        //        else
        //        {
        //            switch (flag)
        //            {
        //                case 1:
        //                    return "NPC_FIRST_TALK";
        //                case 2753:
        //                    return "ITEM(DAY_EVENT)";
        //                case 2754:
        //                    return "ROYAL_UNOVA_TOUR(DAY_EVENT)";
        //                case 2756:
        //                    return "KOMOR_BATTLE(DAY_EVENT)";
        //                case 2757:
        //                    return "BELLE_FIRST_ARALIA_LAB";
        //                case 2758:
        //                    return "BELLE_BATTLE(DAY_EVENT)";
        //                default:
        //                    return flag.ToString();
        //            }
        //        }

        //    }

        //    if (condition == "GEN")
        //    {
        //        if (varString == "0")
        //            return "MALE";
        //        if (varString == "1")
        //            return "FEMALE";
        //    }
        //    if (condition == "DAY")
        //    {
        //        if (varString == "0")
        //            return "SUNDAY";
        //        if (varString == "1")
        //            return "MONDAY";
        //        if (varString == "2")
        //            return "TUESDAY";
        //        if (varString == "3")
        //            return "WEDNESDDAY";
        //        if (varString == "4")
        //            return "THURSDAY";
        //        if (varString == "5")
        //            return "FRIDAY";
        //        if (varString == "6")
        //            return "SATURDAY";
        //    }
        //    if (condition == "GEN")
        //    {
        //        if (varString == "0")
        //            return "MALE";
        //        if (varString == "1")
        //            return "FEMALE";
        //    }
        //    if (condition == "BOL")
        //    {
        //        if (varString == "0")
        //            return "TRUE";
        //        if (varString == "1")
        //            return "FALSE";
        //    }
        //    if (condition == "BAD")
        //    {
        //        if (typeROM == 0)
        //            varString = getText(varString, textNarc, 331) + " " + varString;
        //        else
        //            varString = getText((Int16.Parse(varString) + 11).ToString(), textNarc, 64) + " " + varString;
        //    }
        //    if (condition == "POK")
        //    {
        //        if (typeROM == DPSCRIPT)
        //            varString = getText(varString, textNarc, 362);
        //        else if (typeROM == PLSCRIPT)
        //            varString = getText(varString, textNarc, 412);
        //        else
        //            varString = getText(varString, bwTextNarc, 284);
        //    }
        //    if (condition == "ITE")
        //    {
        //        if (scriptType == 0)
        //            varString = getText(varString, textNarc, 347);
        //        else if (typeROM == PLSCRIPT)
        //            varString = getText(varString, textNarc, 392);
        //        else
        //            varString = getText(varString, bwTextNarc, 54);
        //    }
        //    if (condition == "MOV")
        //    {
        //        if (typeROM == DPSCRIPT)
        //            varString = getText(varString, textNarc, 347);
        //        else if (typeROM == PLSCRIPT)
        //            varString = getText(varString, textNarc, 647);
        //        else
        //            varString = getText(varString, bwTextNarc, 286);
        //    }

        //    if (condition == "ACC")
        //        varString = getText(varString, textNarc, 338);
        //    if (condition == "MUL")
        //    {
        //        short result = 0;
        //        Int16.TryParse(varString, out result);
        //        if (result != 0)
        //        {
        //            var id = Int16.Parse(varString);
        //            if (id < mulString.Count && id > 0)
        //                varString = mulString[id];
        //        }

        //    }
        //    return varString;
        //}

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
    }



}
