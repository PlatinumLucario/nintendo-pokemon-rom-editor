using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NPRE.Formats.Specific.Pokémon.Scripts
{
    public partial class CommandInfo : Form
    {
        private int scriptType;
        public const short DPSCRIPT = 0;
        public const short HGSSSCRIPT = 1;
        public const short BWSCRIPT = 2;

        public CommandInfo()
        {
            InitializeComponent();
        }

        public CommandInfo(int scriptType)
        {
            InitializeComponent();
            this.scriptType = scriptType;
            if (scriptType == DPSCRIPT)
            {
                for (int i = 0; i < 0x33A; i++)
                    scriptList.Items.Add("0x" + i.ToString("X3") + "");
            }
        }

        private void scriptList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idScript = scriptList.SelectedIndex;
            commandName.Clear();
            commandDescription.Clear();
            switch (idScript)
            {
                case 0x2:
                    commandName.Text = "End";
                    break;
                case 0x3:
                    commandName.Text = "Return";

                    commandDescription.AppendText("\n1st Par (Short):  ");
                    commandDescription.AppendText("Unknown");
                    commandDescription.AppendText("\n2nd Par (Short): "); 
                    commandDescription.AppendText("Unknown");
                    break;
                case 0x11:
                    commandName.Text = "If";

                    commandDescription.AppendText("Set two variable to compare in a if condition. \n (Comparer's type is set following by GetLR (or GetLR2) command.");
                    commandDescription.AppendText("\n1st Par (Short):  ");
                    commandDescription.AppendText("Variable that contains value to check.");
                    commandDescription.AppendText("\n2nd Par (Short): "); 
                    commandDescription.AppendText("Value that make if condition true.");
                     
                    break;
                case 0x12:
                    commandName.Text = "If2";

                    commandDescription.AppendText("Implements an if condition. \nUsually followed by GetLR (or GetLR2) command.");
                    commandDescription.AppendText("\n1st Par (Short):  ");
                    commandDescription.AppendText("Variable that contains value to check.");
                    commandDescription.AppendText("\n2nd Par (Short): "); 
                    commandDescription.AppendText("Value that make if condition true."); 
                     
                    break;
                case 20:
                    commandName.Text = "CallStd";
                     
                     
                    break;

                case 0x15:
                    commandName.Text = "ExitStd";
                    break;

                case 0x16:
                    commandName.Text = "Jump";
                    break;

                case 0x17:
                     
                     
                     
                     
                    break;

                case 0x1a:
                    commandName.Text = "Goto";
                    break;

                case 0x1b:
                    commandName.Text = "KillScript";
                    break;

                case 0x1C:
                    commandName.Text = "GetLR";
                    
                    commandDescription.AppendText("Check a previous if condition.");
                    commandDescription.AppendText("\n1st Par (Short):  ");
                    commandDescription.AppendText("Variable that contains check type: \n0x0 Lower \n0x1 Equal \n0x2 Bigger \n0x3 Lower or Equals \n0x4 Bigger or Equals \n0x5 Different");
                    commandDescription.AppendText("\n2nd Par (Short): "); 
                    commandDescription.AppendText("Offset to jump if check is true");

                    break;

                case 0x1D:
                    commandName.Text = "GetLR2";

                    commandDescription.AppendText("Check a previous if condition.");
                    commandDescription.AppendText("\n1st Par (Short):  ");
                    commandDescription.AppendText("Variable that contains check type: \n0x0 Lower \n0x1 Equal \n0x2 Bigger \n0x3 Lower or Equals \n0x4 Bigger or Equals \n0x5 Different");
                    commandDescription.AppendText("\n2nd Par (Short): "); 
                    commandDescription.AppendText("Offset to jump if check is true");
                     
                    break;

                case 0x1E:
                    commandName.Text = "SetFlag";
                     
                    break;

                case 0x1F:
                    commandName.Text = "ClearFlag";
                     
                    break;

                case 0x20:
                    commandName.Text = "GetFlag";
                     
                    break;

                case 0x22:
                     
                    break;

                case 0x23:
                    commandName.Text = "SetTrainerId";
                     
                    break;

                case 0x24:
                     
                    break;

                case 0x25:
                    commandName.Text = "ClearTrainerId";
                     
                    break;

                case 0x26:
                    commandName.Text = "SetValue";
                     
                     
                    break;

                case 0x27:
                    commandName.Text = "CopyValue";
                     
                     
                    break;

                case 0x28:
                    commandName.Text = "SetVar";
                     
                     
                    break;

                case 0x29:
                    commandName.Text = "CopyVar";
                     
                     
                    break;

                case 0x2b:
                    commandName.Text = "Message";
                     
                    break;

                case 0x2c:
                    commandName.Text = "Message2";
                     
                    break;

                case 0x2d:
                    commandName.Text = "Message3";
                     
                    break;

                case 0x2e:
                    commandName.Text = "Message4";
                     
                    break;

                case 0x2f:
                    commandName.Text = "Message5";
                     
                    break;

                case 0x31:
                    commandName.Text = "WaitButton";
                    break;

                case 0x34:
                    commandName.Text = "CloseMessageKeyPress";
                    break;

                case 0x35:
                    commandName.Text = "FreezeMessageBox";
                    break;

                case 0x36:
                    commandName.Text = "CallMessageBox";
                     
                     
                     
                    break;

                case 0x37:
                    commandName.Text = "ColorMessageBox";
                     
                     
                    break;

                case 0x38:
                    commandName.Text = "TypeMessageBox";
                     
                    break;

                case 0x39:
                    commandName.Text = "NoMapMessageBox";
                    break;

                case 0x3a:
                    commandName.Text = "CallTextMessageBox";
                     
                     
                    break;

                case 0x3b:
                    commandName.Text = "StoreMenuStatus";
                     
                    break;

                case 0x3c:
                    commandName.Text = "ShowMenu";
                     
                     
                    break;

                case 0x3e:
                    commandName.Text = "YesNoBox";
                     
                    break;

                case 0x3f:
                    commandName.Text = "WaitFor";
                     
                    break;

                case 0x40:
                    commandName.Text = "Multi";
                     
                     
                     
                     
                     
                    break;

                case 0x41:
                    commandName.Text = "Multi2";
                     
                     
                     
                     
                     
                    break;

                case 0x42:
                    commandName.Text = "SetTextScriptMulti";
                     
                     
                    break;

                case 0x43:
                    commandName.Text = "CloseMulti";
                    break;

                case 0x44:
                    commandName.Text = "Multi3";
                     
                     
                     
                     
                     
                     
                     
                    break;

                case 0x45:
                    commandName.Text = "Multi4";
                     
                     
                     
                     
                     
                    break;

                case 0x46:
                    commandName.Text = "SetTextScriptMessageMulti";
                     
                     
                     
                    break;

                case 0x47:
                    commandName.Text = "CloseMulti4";
                    break;

                case 0x48:
                    commandName.Text = "SetTextRowMulti";
                     
                    break;

                case 0x49:
                    commandName.Text = "Fanfare";
                     
                    break;

                case 0x4a:
                    commandName.Text = "Fanfare2";
                     
                    break;

                case 0x4b:
                    commandName.Text = "WaitFanfare";
                     
                    break;

                case 0x4c:
                    commandName.Text = "Cry";
                     
                     
                    break;

                case 0x4d:
                    commandName.Text = "WaitCry";
                    break;

                case 0x4e:
                    commandName.Text = "PlaySound";
                     
                    break;

                case 0x4f:
                    commandName.Text = "FadeDef";
                    break;

                case 80:
                    commandName.Text = "PlaySound2";
                     
                    break;

                case 0x51:
                    commandName.Text = "StopSound";
                     
                    break;

                case 0x52:
                    commandName.Text = "RestartSound";
                    break;

                case 0x54:
                    commandName.Text = "SwitchSound";
                     
                     
                    break;

                case 0x55:
                    commandName.Text = "StoreSoundMicrophone";
                     
                    break;

                case 0x57:
                    commandName.Text = "PlaySound3";
                     
                    break;

                case 0x58:
                     
                     
                    break;

                case 0x59:
                    commandName.Text = "GetSoundMicrophone";
                     
                    break;

                case 0x5a:
                    commandName.Text = "SwitchSound2";
                     
                    break;

                case 0x5b:
                    commandName.Text = "ActivateMicrophone";
                    break;

                case 0x5c:
                    commandName.Text = "DisactivateMicrophone";
                    break;

                case 0x5e:
                    commandName.Text = "ApplyMovement";

                    break;

                case 0x5f:
                    commandName.Text = "WaitMovement";
                    break;

                case 0x60:
                    commandName.Text = "LockAll";
                    break;

                case 0x61:
                    commandName.Text = "ReleaseAll";
                    break;

                case 0x62:
                    commandName.Text = "Lock";
                     
                    break;

                case 0x63:
                    commandName.Text = "Release";
                     
                    break;

                case 0x64:
                    commandName.Text = "AddPeople";
                     
                    break;

                case 0x65:
                    commandName.Text = "RemovePeople";
                     
                    break;

                case 0x66:
                    commandName.Text = "MoveCam";
                     
                     
                    break;

                case 0x67:
                    commandName.Text = "ZoomCam";
                    break;

                case 0x68:
                    commandName.Text = "FacePlayer";
                    break;

                case 0x69:
                    commandName.Text = "GetHiroPosition";
                     
                     
                    break;

                case 0x6a:
                    commandName.Text = "GetIDPosition";
                     
                     
                     
                    break;

                case 0x6b:
                     
                     
                     
                    break;

                case 0x6c:
                    commandName.Text = "ContinueFollow";
                     
                     
                    break;

                case 0x6d:
                    commandName.Text = "FollowHero";
                     
                     
                    break;

                case 0x6e:
                    commandName.Text = "StopFollowHero";
                    break;

                case 0x6f:
                    commandName.Text = "GiveMoney";
                     
                     
                    break;
                case 0x71:
                    commandName.Text = "GetMoney";
                     
                     
                     
                    break;

                case 0x72:
                    commandName.Text = "ShowMoney";
                     
                     
                    break;

                case 0x73:
                    commandName.Text = "HideMoney";
                    break;

                case 0x74:
                    commandName.Text = "UpdateMoney";
                    break;

                case 0x75:
                    commandName.Text = "ShowCoins";
                     
                     
                    break;

                case 0x76:
                    commandName.Text = "HideCoins";
                    break;

                case 0x77:
                    commandName.Text = "UpdateCoins";
                    break;

                case 0x78:
                    commandName.Text = "CompareCoins";
                     
                     
                     
                    break;

                case 0x79:
                    commandName.Text = "GiveCoins";
                     
                    break;

                case 0x7a:
                    commandName.Text = "TakeCoins";
                     
                     
                     
                    break;

                case 0x7b:
                    commandName.Text = "TakeItem";
                     
                     
                     
                    break;

                case 0x7c:
                    commandName.Text = "GiveItem";
                     
                     
                     
                    break;

                case 0x7d:
                    commandName.Text = "GetStoreItem";
                     
                     
                     
                    break;

                case 0x7e:
                    commandName.Text = "GetItem";
                     
                     
                     
                    break;

                case 0x7f:
                    commandName.Text = "StoreItemTaken";
                     
                     
                    break;

                case 0x80:
                    commandName.Text = "StoreItemType";
                     
                     
                    break;

                case 0x83:
                    commandName.Text = "SendItemType";
                     
                     
                     
                    break;

                case 0x85:
                    commandName.Text = "GetUndergroundPcStatus";
                     
                     
                     
                    break;

                case 0x87:
                    commandName.Text = "SendItemType2";
                     
                     
                     
                    break;

                case 0x8f:
                    commandName.Text = "SendItemType";
                     
                     
                     
                    break;

                case 0x93:
                    commandName.Text = "GetPok\x00e9monParty";
                     
                     
                    break;

                case 0x94:
                    commandName.Text = "StorePok\x00e9monParty";
                     
                     
                    break;

                case 0x95:
                    commandName.Text = "StorePok\x00e9monParty2";
                     
                     
                    break;

                case 0x96:
                    commandName.Text = "GivePok\x00e9mon";
                     
                     
                     
                     
                    break;

                case 0x97:
                    commandName.Text = "GiveEgg";
                     
                     
                    break;

                case 0x99:
                    commandName.Text = "GetMove";
                     
                     
                     
                    break;

                case 0x9A:
                    commandName.Text = "GetPlace";
                     
                     
                    break;

                case 0x9B:
                     
                     
                    break;

                case 0xA1:
                    commandName.Text = "CallEnd";
                    break;

                case 0xA3:
                    commandName.Text = "StartWFC";
                    break;

                case 0xA5:
                    commandName.Text = "StartInterview";
                    break;

                case 0xA6:
                    commandName.Text = "StartDressPokèmon";
                     
                     
                     
                    break;

                case 0xA7:
                    commandName.Text = "DisplayDressedPokèmon";
                     
                     
                    break;

                case 0xA8:
                    commandName.Text = "DisplayContestPok\x00e9mon";
                     
                     
                    break;

                case 0xA9:
                    commandName.Text = "OpenBallCapsule";
                    break;

                case 0xAA:
                    commandName.Text = "OpenSinnohMaps";
                    break;

                case 0xAB:
                    commandName.Text = "OpenPc";
                     
                    break;

                case 0xAC:
                    commandName.Text = "StartDrawingUnion";
                    break;

                case 0xAD:
                    commandName.Text = "ShowTrainerCaseUnion";
                    break;

                case 0xAE:
                    commandName.Text = "StartTradingUnion";
                    break;

                case 0xAF:
                    commandName.Text = "ShowRecordUnion";
                    break;

                case 0xB0:
                    commandName.Text = "EndGame";
                    break;

                case 0xB1:
                    commandName.Text = "ShowHallOfFame";
                    break;

                case 0xB2:
                    commandName.Text = "GetWFC";
                     
                     
                    break;

                case 0xB3:
                    commandName.Text = "StartWFC";
                     
                    break;

                case 0xB4:
                    commandName.Text = "ChooseStarter";
                    break;

                case 0xB5:
                    commandName.Text = "BattleStarter";
                    break;

                case 0xB6:
                    commandName.Text = "GetBattleID?";
                     
                    break;

                case 0xB7:
                    commandName.Text = "SetVarBattle?";
                     
                     
                    break;

                case 0xB8:
                    commandName.Text = "GetTypeBattle?";
                     
                    break;

                case 0xB9:
                    commandName.Text = "SetVarBattle2?";
                     
                     
                    break;

                case 0xBA:
                    commandName.Text = "ChoosePlayerName";
                    break;

                case 0xBB:
                    commandName.Text = "ChoosePok\x00e9monName";
                     
                     
                    break;

                case 0xBC:
                    commandName.Text = "FadeScreen";
                     
                     
                     
                     
                    break;

                case 0xBD:
                    commandName.Text = "ResetScreen";
                    break;
                case 0xBE:
                    commandName.Text = "Warp";
                     
                     
                     
                     
                     
                    break;
                case 0xbf:
                    commandName.Text = "RockClimbAnimation";
                     
                    break;
                case 0xC0:
                    commandName.Text = "SurfAnimation";
                     
                    break;
                case 0xC1:
                    commandName.Text = "WaterfallAnimation";
                     
                    break;
                case 0xC2:
                    commandName.Text = "FlyAnimation";
                     
                    break;
                case 0xC5:
                     
                    break;
                case 0xC6:
                    commandName.Text = "Tuxedo";
                     
                    break;
                case 0xC7:
                    commandName.Text = "GetBike";
                     
                    break;
                case 0xC8:
                    commandName.Text = "RideBike";
                     
                    break;
                case 0xC9:
                     
                    break;
                case 0xCB:
                    commandName.Text = "BerryHiroAnimation";
                     
                    break;
                case 0xCC:
                    commandName.Text = "StopBerryHiroAnimation";
                    break;
                case 0xCD:
                    commandName.Text = "SetVarHiro";
                     
                    break;
                case 0xCE:
                    commandName.Text = "SetVarRival";
                     
                    break;
                case 0xCF:
                    commandName.Text = "SetVarAlter";
                     
                    break;
                case 0xD0:
                    commandName.Text = "SetVarPokémon";
                     
                     
                    break;
                case 0xD1:
                    commandName.Text = "SetVarItem";
                     
                     
                    break;
                case 0xD2:
                    commandName.Text = "SetVarItem2";
                     
                     
                    break;
                case 0xD3:
                    commandName.Text = "SetVarBattleItem";
                     
                     
                    break;
                case 0xD4:
                    commandName.Text = "SetVarAttack";
                     
                     
                    break;
                case 0xD5:
                    commandName.Text = "SetVarNumber";
                     
                     
                    break;
                case 0xD6:
                    commandName.Text = "SetVarNickPokémon";
                     
                     
                    break;
                case 0xD7:
                    commandName.Text = "SetVarObject";
                     
                     
                    break;
                case 0xD8:
                    commandName.Text = "SetVarTrainer";
                     
                     
                    break;
                case 0xD9:
                     
                    break;
                case 0xDA:
                    commandName.Text = "SetVarPokèmonStored";
                     
                     
                     
                     
                    break;
                case 0xDE:
                    commandName.Text = "GetStarter";
                     
                    break;
                case 0xE4:
                     
                    break;
                case 0xE5:
                    commandName.Text = "TrainerBattle";
                     
                     
                    break;
                case 0xE6:
                     
                     
                    break;
                case 0xE7:
                     
                     
                     
                    break;
                case 0xE8:
                     
                     
                     
                    break;
                case 0xE9:
                     
                    break;
                case 0xEA:
                     
                    break;
                case 0xEB:
                    commandName.Text = "TeleportPC";
                    break;
                case 0xEC:
                    commandName.Text = "GetBattleResult";
                     
                    break;
                case 0xEE:
                     
                    break;
                case 0x114:
                    commandName.Text = "SetFloor";
                     
                    break;
                case 0x11B:
                    commandName.Text = "WarpMapLift";
                     
                     
                     
                     
                     
                    break;
                case 0x11C:
                    commandName.Text = "GetFloor";
                     
                    break;
                case 0x11D:
                    commandName.Text = "StartLift";
                    break;
                case 0x12E:
                    commandName.Text = "GetPhotoImage";
                     
                     
                    break;
                case 0x130:
                    commandName.Text = "GetPhotoName";
                     
                    break;
                case 0x131:
                    commandName.Text = "ActPokèKron";
                    break;
                case 0x132:
                    commandName.Text = "GetPokèKronApplicationStatus";
                     
                    break;
                case 0x133:
                    commandName.Text = "ActPokèKronApplication";
                     
                    break;
                case 0x134:
                    commandName.Text = "ActPokèKronApplication";
                     
                     
                    break;
                case 0x135:
                     
                    break;
                case 0x139:
                     
                    break;
                case 0x13C:
                     
                    break;
                case 0x13F:
                     
                     
                    break;

                case 0x140:
                     
                    break;
                case 0x141:
                     
                    break;
                case 0x142:
                     
                    break;
                case 0x143:
                     
                     
                    break;
                case 0x144:
                     
                    break;
                case 0x145:
                     
                    break;
                case 0x146:
                     
                     
                    break;

                case 0x14D:
                    commandName.Text = "GetGender";
                     
                    break;
                case 0x152:
                     
                    break;
                case 0x15B:
                    commandName.Text = "GetBadge";
                     
                     
                    break;
                case 0x15D:
                    commandName.Text = "GetBadgeNumber";
                     
                    break;

                case 0x168:
                    commandName.Text = "PrepareDoorAnimation";
                     
                     
                     
                     
                     
                    break;
                case 0x169:
                    commandName.Text = "WaitDoorOpeningAnimation";
                     
                    break;
                case 0x16A:
                    commandName.Text = "WaitDoorClosingAnimation";
                     
                    break;
                case 0x16B:
                    commandName.Text = "DoorOpeningAnimation";
                      ;
                    break;
                case 0x16C:
                    commandName.Text = "DoorClosingAnimation";
                     
                    break;
                case 0x177:
                    commandName.Text = "GetPokémonCaught";
                     
                    break;
                case 0x186:
                    commandName.Text = "ChangeOwPosition";
                     
                     
                     
                    break;
                case 0x187:
                    commandName.Text = "SetOwPosition";
                     
                     
                     
                     
                     
                    break;
                case 0x188:
                    commandName.Text = "ChangeOwPosition";
                     
                     
                    break;
                case 0x189:
                    commandName.Text = "ReleaseOw";
                     
                     
                    break;
                case 0x191:
                    commandName.Text = "ShowChoosePokèmonMenu";
                    break;
                case 0x193:
                    commandName.Text = "GetChosenPokèmon";
                     
                    break;
                case 0x198:
                    commandName.Text = "GetPokèmonId";
                     
                     
                    break;
                case 0x19A:
                    commandName.Text = "GetPokèmonPartyNumber";
                     
                    break;
                case 0x19B:
                    commandName.Text = "GetPokèmonPartyNumber2";
                     
                     
                    break;
                case 0x19E:
                     
                     
                    break;
                case 0x19F:
                     
                    break;
                case 0x1A0:
                     
                    break;
                case 0x1B1:
                    commandName.Text = "0x1B1";
                     
                    break;
                case 0x1B2:
                    commandName.Text = "0x1B2";
                     
                    break;
                case 0x1B5:
                    commandName.Text = "ShowRecordList";
                     
                    break;
                case 0x1B6:
                    commandName.Text = "GetTime";
                     
                    break;
                case 0x1B7:
                    commandName.Text = "GetPlayerId";
                     
                     
                    break;
                case 0x1BD:
                    commandName.Text = "GetHeroFaceOrientation";
                     
                    break;
                case 0x1D2:
                    commandName.Text = "GiveAccessories";
                     
                     
                    break;
                case 0x1D3:
                    commandName.Text = "GetAccessories";
                     
                     
                     
                    break;
                case 0x1D5:
                    commandName.Text = "GiveAccessories2";
                     
                    break;
                case 0x1D6:
                    commandName.Text = "GetAccessories2";
                     
                     
                    break;
                case 0x1F9:
                     

                    break;
                case 0x1FB:
                     
                     
                    break;
                case 0x226:
                    commandName.Text = "SetTradeId";
                     
                    break;
                case 0x228:
                    commandName.Text = "GetChosenPokèmonTrade";
                     
                    break;
                case 0x229:
                    commandName.Text = "TradePokèmon";
                     
                    break;
                case 0x22A:
                    commandName.Text = "EndTrade";
                    break;
                case 0x234:
                    commandName.Text = "GetHeroFriendCode";
                     
                    break;
                case 0x235:
                    commandName.Text = "GetOtherFriendCode";
                     
                    break;
                case 0x237:
                     
                     
                     
                     
                    break;
                case 0x238:
                     
                     
                    break;
                case 0x23C:
                    commandName.Text = "GetLiftDirection";
                     
                     
                    break;
                case 0x243:
                    commandName.Text = "GetSinglePhraseBoxInput";
                     
                     
                     
                    break;
                case 0x244:
                    commandName.Text = "GetDoublePhraseBoxInput";
                     
                     
                     
                     
                    break;
                case 0x245:
                    commandName.Text = "SetVarPhraseBoxInput";
                     
                     
                    break;
                case 0x249:
                    commandName.Text = "GetDoublePhraseBoxInput2";
                     
                     
                     
                     
                     
                    break;
                case 0x24E:
                    commandName.Text = "GetPokèLottoNumber";
                     
                    break;
                case 0x24F:
                    commandName.Text = "ComparePokèLottoNumber";
                     
                     
                     
                     
                    break;
                case 0x251:
                    commandName.Text = "SetVarIdPokèmonBoxes";
                     
                     
                    break;
                case 0x252:
                    commandName.Text = "GetBoxNumber";
                     
                    break;
                case 0x280:
                    commandName.Text = "ComparePokèLottoNumber";
                     
                     
                     
                    break;
                case 0x29D:
                    commandName.Text = "MultiChoices";
                     
                     
                    break;
                case 0x2A0:
                    commandName.Text = "DoubleTrainerBattle";
                     
                     
                     
                    break;
                case 0x2A5:
                    commandName.Text = "ShowPokèmonTradeMenu";
                    break;
                case 0x2AA:
                    commandName.Text = "ComparePhraseBoxInput";
                     
                     
                     
                     
                     
                    break;
                case 0x2AC:
                    commandName.Text = "ActMisteryGift";
                    break;
                case 0x2AD:
                     
                     
                    break;
                case 0x2B7:
                     
                    break;
                case 0x2CF:
                     
                     
                    break;
                case 0x2D1:
                    commandName.Text = "LiftAnimation";
                     
                     
                    break;
                case 0x33A:
                    commandName.Text = "TexBoxVariable";
                     
                    break;

                default:
                    commandName.Text = "Unknown";
                    commandDescription.Text = "Unknown";
                    break;


            }
        }
    }
}
