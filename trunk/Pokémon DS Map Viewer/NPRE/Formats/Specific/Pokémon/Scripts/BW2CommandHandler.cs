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
    public class BW2CommandHandler
    {
        private int scriptType;
        public List<uint> movOffsetList = new List<uint>();
        public List<uint> functionOffsetList = new List<uint>();

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
        private int varLevel;
        private RichTextBox scriptBoxViewer;
        private int newVar;
        private int newVar2;
        private string temp;

        public Scripts.Commands_s readCommandBW2(BinaryReader reader, Scripts.Commands_s com, List<uint> movOffsetList, List<uint> functionOffsetList, List<uint> scriptStartList, List<uint> scriptOffList, List<uint> scriptOrder)
        {
            uint functionOffset = 0;
            reader.BaseStream.Position += 1;
            var actualPos = reader.BaseStream.Position;
            if (scriptStartList.Contains((uint)actualPos) || (functionOffsetList!=null &&  functionOffsetList.Contains((uint)actualPos)))
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
                if (actualPos + 1 == reader.BaseStream.Length || (movOffsetList!=null && movOffsetList.Contains((uint)actualPos)) || scriptStartList.Contains((uint)actualPos) || (functionOffsetList!= null && functionOffsetList.Contains((uint)actualPos)))
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
                case 0x1:
                    com.Name = "Nop2";
                    com.isEnd = 1;
                    break;
                case 0x2:
                    com.Name = "EndScript";
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
                    com.Name = "PauseScriptFor";
                    com.parameters.Add(reader.ReadUInt16()); //Delay
                    break;
                case 0x04:
                    com.Name = "CallRoutine(04)";
                    if (reader.BaseStream.Position < reader.BaseStream.Length - 4)
                    {
                        com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                        functionOffset = com.parameters[0];
                        if (!scriptStartList.Contains(functionOffset) && !functionOffsetList.Contains(functionOffset) && !movOffsetList.Contains(functionOffset))
                        {
                            functionOffsetList.Add(functionOffset);
                        }
                    }
                    break;
                case 0x05:
                    com.Name = "EndRoutine(05)";
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
                    com.Name = "GetDerefVar(06)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x07:
                    com.Name = "GetDerefVar(07)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x08:
                    com.Name = "SetStackVar(08)";
                    com.parameters.Add(reader.ReadUInt16()); //Value to push
                    break;
                case 0x09:
                    com.Name = "SetStackDerefVar(09)";
                    com.parameters.Add(reader.ReadUInt16()); //Containter to push
                    break;
                case 0x0A:
                    com.Name = "GetStackVar(0A)";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Container 
                    break;
                case 0x0B:
                    com.Name = "PopStack(0B)";
                    break;
                case 0x0C:
                    com.Name = "AddStackVar(0C)";
                    break;
                case 0x0D:
                    com.Name = "SubStackVar(0D)";
                    break;
                case 0x0E:
                    com.Name = "MulStackVar(0E)";
                    break;
                case 0x0F:
                    com.Name = "DivStackVar(0F)";
                    break;
                case 0x10:
                    com.Name = "SetStackFlag(10)";
                    com.parameters.Add(reader.ReadUInt16()); //Flag
                    break;
                case 0x11:
                    com.Name = "CompareStackCondition(11)";
                    com.parameters.Add(reader.ReadUInt16()); //Condition
                    break;
                case 0x12:
                    com.Name = "AndDerefVar(12)";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Container/First Deref Var
                    com.parameters.Add(reader.ReadUInt16()); //Second Deref Var
                    break;
                case 0x13:
                    com.Name = "OrDerefVar(13)";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Container/First Deref Var
                    com.parameters.Add(reader.ReadUInt16()); //Second Deref Var
                    break;
                case 0x14:
                    com.Name = "StoreByteToGlobalVar(14)";
                    com.parameters.Add(reader.ReadByte()); //Global Var Id
                    com.parameters.Add(reader.ReadByte()); //Value
                    break;
                case 0x15:
                    com.Name = "StoreLongToGlobalVar(15)";
                    com.parameters.Add(reader.ReadByte()); //Global Var Id
                    com.parameters.Add(reader.ReadUInt32()); //Value
                    break;
                case 0x16:
                    com.Name = "MoveGlobalVars(16)";
                    com.parameters.Add(reader.ReadByte()); //Destination Id
                    com.parameters.Add(reader.ReadByte()); //Source Id
                    break;
                case 0x17:
                    com.Name = "CompareGlobalVars(17)";
                    com.parameters.Add(reader.ReadByte()); //First Global Var Id
                    com.parameters.Add(reader.ReadByte()); //Second Global Var Id
                    break;
                case 0x18:
                    com.Name = "CompareValueWithGlobalVar(17)";
                    com.parameters.Add(reader.ReadByte()); //Global Var Id
                    com.parameters.Add(reader.ReadByte()); //Value
                    break;
                case 0x19:
                    com.Name = "CompareValueWithDerefVar(19)";
                    com.parameters.Add(reader.ReadUInt16()); //Deref Var
                    com.parameters.Add(reader.ReadUInt16()); //Value
                    break;
                case 0x1A:
                    com.Name = "CompareDerefVars(1A)";
                    com.parameters.Add(reader.ReadUInt16()); //First Deref Var
                    com.parameters.Add(reader.ReadUInt16()); //Second Deref Var
                    break;
                case 0x1B:
                    com.Name = "AddScriptVM(1B)";
                    com.parameters.Add(reader.ReadUInt16()); //Param
                    break;
                case 0x1C:
                    com.Name = "CallSubScriptVM(1C)";
                    com.parameters.Add(reader.ReadUInt16());//Standard Table Offset
                    break;
                case 0x1D:
                    com.Name = "EndSubScriptVM(1D)";
                    break;
                case 0x1E:
                    com.Name = "Jump";
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[0];
                    if (!scriptStartList.Contains(functionOffset) && (functionOffsetList != null && !functionOffsetList.Contains(functionOffset)) && (movOffsetList != null && !movOffsetList.Contains(functionOffset)))
                    {
                        functionOffsetList.Add(functionOffset);
                    }
                    com.numJump++;
                    com.isEnd = 1;
                    break;
                case 0x1F:
                    com.Name = "When(1F)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[1];
                    if (!scriptStartList.Contains(functionOffset) && (functionOffsetList!= null && !functionOffsetList.Contains(functionOffset)) && (movOffsetList!=null && !movOffsetList.Contains(functionOffset)))
                    {
                        functionOffsetList.Add(functionOffset);
                    }
                    com.numJump++;
                    break;
                case 0x20:
                    com.Name = "If(20)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    functionOffset = com.parameters[1];
                    if (!scriptStartList.Contains(functionOffset) && !functionOffsetList.Contains(functionOffset) && !movOffsetList.Contains(functionOffset))
                    {
                        functionOffsetList.Add(functionOffset);
                    }
                    com.numJump++;
                    break;
                case 0x21:
                    com.Name = "SetMapEventStatusFlag(21)";
                    com.parameters.Add(reader.ReadUInt16()); //Flag
                    break;
                case 0x22:
                    com.Name = "CheckMapTypeChange(22)";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Container
                    break;
                case 0x23:
                    com.Name = "SetFlag(23)";
                    com.parameters.Add(reader.ReadUInt16()); //Flag
                    break;
                case 0x24:
                    com.Name = "ClearFlag(24)";
                    com.parameters.Add(reader.ReadUInt16()); //Flag
                    break;
                case 0x25:
                    com.Name = "StoreVarFlag(25)";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Container
                    com.parameters.Add(reader.ReadUInt16()); //Flag
                    break;
                case 0x26:
                    com.Name = "AddVars(26)";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Container
                    com.parameters.Add(reader.ReadUInt16()); //Second Var
                    break;
                case 0x27:
                    com.Name = "SubVars(27)";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Container
                    com.parameters.Add(reader.ReadUInt16()); //Second Var
                    break;
                case 0x28:
                    com.Name = "StoreValueInVar(28)";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Container
                    com.parameters.Add(reader.ReadUInt16()); //Value
                    break;
                case 0x29:
                    com.Name = "StoreVarInVar(29)";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Container
                    com.parameters.Add(reader.ReadUInt16()); //Var
                    break;
                case 0x2A:
                    com.Name = "StoreDerefVarInVar(2A)";
                    com.parameters.Add(reader.ReadUInt16()); //Var as container
                    com.parameters.Add(reader.ReadUInt16()); //Deref Var
                    break;
                case 0x2B:
                    com.Name = "MulVars(2B)";
                    com.parameters.Add(reader.ReadUInt16()); //Var as container
                    com.parameters.Add(reader.ReadUInt16()); //Deref Var
                    break;
                case 0x2C:
                    com.Name = "DivVars(2C)";
                    com.parameters.Add(reader.ReadUInt16()); //Var as container
                    com.parameters.Add(reader.ReadUInt16()); //Deref Var
                    break;
                case 0x2D:
                    com.Name = "ModDivVars(2D)";
                    com.parameters.Add(reader.ReadUInt16()); //Var as container
                    com.parameters.Add(reader.ReadUInt16()); //Deref Var
                    break;
                case 0x2E:
                    com.Name = "LockAll(2E)";
                    break;
                case 0x2F:
                    com.Name = "ReleaseAll(2F)";
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
                    com.Name = "MusicalMessage(33)";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    break;
                case 0x34:
                    com.Name = "EventGreyMessage(34)";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    break;
                case 0x35:
                    com.Name = "MusicalMessage(35)";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    break;
                case 0x36:
                    com.Name = "CloseEventGreyMessage(36)";
                    break;
                case 0x37:
                    com.Name = "37";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x38:
                    com.Name = "BubbleMessage(38)";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadByte()); //Bottom/Top View.
                    break;
                case 0x39:
                    com.Name = "CloseBubbleMessage(39)";
                    break;
                case 0x3A:
                    com.Name = "ShowMessageAt(3A)";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //X coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate
                    break;
                case 0x3B:
                    com.Name = "CloseShowMessageAt(3B)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x3C:
                    com.Name = "NPCMessage(3C)";
                    com.parameters.Add(reader.ReadUInt16()); //Internal Narc File Id
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    com.parameters.Add(reader.ReadUInt16()); //Message Type
                    break;
                case 0x3D:
                    com.Name = "GenericMessage(3D)";
                    com.parameters.Add(reader.ReadUInt16()); //Internal Narc File Id
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    com.parameters.Add(reader.ReadUInt16()); //Message Type
                    break;
                case 0x3E:
                    com.Name = "CloseSpecificMessageKeyPress(3E)";
                    break;
                case 0x3F:
                    com.Name = "CloseGenericMessageKeyPress(3F)";
                    break;
                case 0x40:
                    com.Name = "ShowMoneyBox(40)";
                    com.parameters.Add(reader.ReadUInt16()); //X coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate
                    break;
                case 0x41:
                    com.Name = "CloseMoneyBox(41)";
                    break;
                case 0x42:
                    com.Name = "UpdateMoneyBox(42)";
                    break;
                case 0x43:
                    com.Name = "BorderedMessage(43)";
                    com.parameters.Add(reader.ReadUInt16()); //MessageId
                    com.parameters.Add(reader.ReadUInt16()); //Color
                    break;
                case 0x44:
                    com.Name = "CloseBorderedMessage(44)";
                    break;
                case 0x45:
                    com.Name = "PaperMessage(45)";
                    com.parameters.Add(reader.ReadUInt16()); //MessageId
                    com.parameters.Add(reader.ReadUInt16()); //Trans. Coordinate
                    break;
                case 0x46:
                    com.Name = "ClosePaperMessage(46)";
                    break;
                case 0x47:
                    com.Name = "YesNoBox(47)";
                    com.parameters.Add(reader.ReadUInt16()); //Variable: NO = 0, YES = 1
                    break;
                case 0x48:
                    com.Name = "GenderMessage(48)";
                    com.parameters.Add(reader.ReadUInt16()); //Internal Narc File Id
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id 
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    com.parameters.Add(reader.ReadUInt16()); //Message Type
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x49:
                    com.Name = "VersionMessage(49)";
                    com.parameters.Add(reader.ReadUInt16()); //Internal Narc File Id
                    com.parameters.Add(reader.ReadUInt16()); //Id Message Black
                    com.parameters.Add(reader.ReadUInt16()); //Id Message White
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id 
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    com.parameters.Add(reader.ReadUInt16()); //Message Type
                    break;
                case 0x4A:
                    com.Name = "AngryMessage(4A)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
                    break;
                case 0x4B:
                    com.Name = "CloseAngryMessage(4B)";
                    break;
                case 0x4C:
                    com.Name = "SetVarHero(4C)";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    break;
                case 0x4D:
                    com.Name = "SetVarItem(4D)";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Item Id
                    break;
                case 0x4E:
                    com.Name = "SetVarItemMultiple(4E)";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Item Id
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadByte()); //Single/Multiple Items
                    break;
                case 0x4F:
                    com.Name = "SetVarColoredItem(4F)";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Item Id
                    break;
                case 0x50:
                    com.Name = "SetVarTM(50)";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //TM Id
                    break;
                case 0x51:
                    com.Name = "SetVarMove(51)";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Move Id
                    break;
                case 0x52:
                    com.Name = "SetVarBag(52)";
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16()); //Bag Id
                    break;
                case 0x53:
                    com.Name = "SetVarPartyPokèmon(53)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x54:
                    com.Name = "SetVarPartyPokèmonNick(54)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x55:
                    com.Name = "SetVarDayCarePokèmon(55)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x56:
                    com.Name = "SetVarType(56)";		     //382
                    com.parameters.Add(reader.ReadByte());   //text variable to set
                    com.parameters.Add(reader.ReadUInt16()); //type to set
                    break;
                case 0x57:
                    com.Name = "SetVarPokèmon(57)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x58:
                    com.Name = "SetVarColoredPokèmon(58)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x59:
                    com.Name = "SetVarLocation(59)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x5A:
                    com.Name = "SetVarSaying(5A)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x5B:
                    com.Name = "SetVarDayCarePokemonNick(5B)";
                    com.parameters.Add(reader.ReadByte());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x5C: // example 654
                    com.Name = "SetVarNumberBound(5C)";
                    com.parameters.Add(reader.ReadByte()); // Text Var
                    com.parameters.Add(reader.ReadUInt16()); // Number
                    com.parameters.Add(reader.ReadUInt16()); // Max ID
                    break;
                case 0x5D:
                    com.Name = "SetVarMusicalInfo(5D)";
                    com.parameters.Add(reader.ReadByte()); //Info ID
                    com.parameters.Add(reader.ReadByte()); //Text Var
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x5E:
                    com.Name = "SetVarNations(5E)";
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
                    com.Name = "ApplyMovement(64)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
                    var movOffset = com.parameters[1];
                    if (!movOffsetList.Contains(movOffset))
                    {
                        movOffsetList.Add(movOffset);
                    }
                    break;
                case 0x65:
                    com.Name = "WaitMovement(65)";
                    break;
                case 0x66:
                    com.Name = "66";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    com.parameters.Add(reader.ReadUInt16()); //NPC ID
                    break;
                case 0x67:
                    com.Name = "StoreNPCPosition(67)";
                    com.parameters.Add(reader.ReadUInt16()); //NPC ID
                    com.parameters.Add(reader.ReadUInt16()); //RV = X
                    com.parameters.Add(reader.ReadUInt16()); //RV = Y
                    break;
                case 0x68:
                    com.Name = "StoreHeroPosition(68)";
                    com.parameters.Add(reader.ReadUInt16()); //RV = X
                    com.parameters.Add(reader.ReadUInt16()); //RV = Y
                    break;
                case 0x69:
                    com.Name = "MakeNPC(69)";
                    com.parameters.Add(reader.ReadUInt16()); // X container
                    com.parameters.Add(reader.ReadUInt16()); // Y container.
                    com.parameters.Add(reader.ReadUInt16()); // Face Direction
                    com.parameters.Add(reader.ReadUInt16()); // NPC Id
                    com.parameters.Add(reader.ReadUInt16()); // Sprite
                    com.parameters.Add(reader.ReadUInt16()); // Movement Permission
                    break;
                case 0x6A:
                    com.Name = "StoreNPCFlag(6A)";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    com.parameters.Add(reader.ReadUInt16()); //Flag
                    break;
                case 0x6B:
                    com.Name = "AddNPC(6B)";
                    com.parameters.Add(reader.ReadUInt16()); //Npc Id
                    break;
                case 0x6C:
                    com.Name = "RemoveNPC(6C)";
                    com.parameters.Add(reader.ReadUInt16()); //Npc Id
                    break;
                case 0x6D:
                    com.Name = "RelocateNPC(6D)";
                    com.parameters.Add(reader.ReadUInt16()); //Npc Id
                    com.parameters.Add(reader.ReadUInt16()); //X coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Y coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Z coordinate
                    com.parameters.Add(reader.ReadUInt16()); //Face direction
                    break;
                case 0x6E:
                    com.Name = "StoreHeroOrientation(6E)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x6F:
                    com.Name = "(6F)";
                    com.parameters.Add(reader.ReadUInt16()); //RV = NPC Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = Bool
                    break;
                case 0x70:
                    com.Name = "(70)";
                    com.parameters.Add(reader.ReadUInt16()); //RV = NPC Id
                    com.parameters.Add(reader.ReadUInt16()); //RV = Bool
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
                    com.Name = "FacePlayer(74)";
                    break;
                case 0x75:
                    com.Name = "Release(75)";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    break;
                case 0x76:
                    com.Name = "76";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x77:
                    com.Name = "Lock(77)";
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
                    break;
                case 0x78:
                    com.Name = "CheckLock(78)";
                    com.parameters.Add(reader.ReadUInt16()); //RV
                    break;
                case 0x79:
                    com.Name = "StoreNPCLevel(79)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16()); //NPC Id
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
                    com.Name = "PlaySSeq(A6)";
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
                    com.Name = "SetTextScriptMessage(AF)";
                    com.parameters.Add(reader.ReadUInt16()); //Message Id
                    com.parameters.Add(reader.ReadUInt16()); //Box Message Id
                    com.parameters.Add(reader.ReadUInt16()); //Script Id
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
                    com.Name = "StoreDay(CF)";
                    com.parameters.Add(reader.ReadUInt16()); //RV = Day
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
                    com.Name = "MoveCamera(143)";
                    com.parameters.Add(reader.ReadUInt16()); //Elevation
                    com.parameters.Add(reader.ReadUInt16()); //Rotation
                    com.parameters.Add(reader.ReadUInt32()); //Zoom
                    com.parameters.Add(reader.ReadUInt32());
                    com.parameters.Add(reader.ReadUInt32());
                    com.parameters.Add(reader.ReadUInt32());
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
                    if (scriptType == Constants.BW2SCRIPT)
                        com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x177:
                    com.Name = "177";
                    if (scriptType == Constants.BW2SCRIPT)
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
                    com.Name = "StoreVarMessage(1C9)";
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
                    if (scriptType == Constants.BW2SCRIPT)
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
                    com.Name = "227";
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
                    com.Name = "230";
                    com.parameters.Add(reader.ReadUInt16());
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
                case 0x276:
                    com.Name = "(276)";
                    com.parameters.Add(reader.ReadUInt16());
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                case 0x2D1:
                    com.Name = "StoreBCSkyscraperTrainerNumber(2D1)";
                    com.parameters.Add(reader.ReadUInt16());
                    break;
                default:
                    com.Name = "0x" + com.Id.ToString("X");
                    break;

            }
            return com;
        }

        //private Commands_s readCommandBW2(BinaryReader reader, Commands_s com)
        //{
        //    uint functionOffset = 0;
        //    reader.BaseStream.Position += 1;
        //    var actualPos = reader.BaseStream.Position;
        //    if (scriptStartList.Contains((uint)actualPos) || functionOffsetList.Contains((uint)actualPos))
        //    {
        //        com.Name = "WrongEnd";
        //        com.isEnd = 1;
        //        reader.BaseStream.Position -= 1;
        //        return com;
        //    }
        //    reader.BaseStream.Position -= 1;
        //    if (reader.BaseStream.Position < reader.BaseStream.Length)
        //    {
        //        actualPos = reader.BaseStream.Position;
        //        if (actualPos == 368)
        //        {
        //        }
        //        if (actualPos + 1 == reader.BaseStream.Length || movOffsetList.Contains((uint)actualPos) || scriptStartList.Contains((uint)actualPos) || functionOffsetList.Contains((uint)actualPos))
        //        {
        //            com.isEnd = 1;
        //        }
        //    }
        //    switch (com.Id)
        //    {
        //        case 0x2:
        //            com.Name = "End";
        //            if (reader.BaseStream.Position < reader.BaseStream.Length)
        //            {
        //                var next = reader.ReadByte();
        //                if (next == 0)
        //                    com.isEnd = 1;
        //                else
        //                    reader.BaseStream.Position -= 1;
        //            }
        //            break;
        //        case 0x03:
        //            com.Name = "ReturnAfterDelay";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x04:
        //            com.Name = "CallRoutine";
        //            if (reader.BaseStream.Position < reader.BaseStream.Length - 4)
        //            {
        //                com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
        //                functionOffset = com.parameters[0];
        //                if (!scriptStartList.Contains(functionOffset) && !functionOffsetList.Contains(functionOffset) && !movOffsetList.Contains(functionOffset))
        //                {
        //                    functionOffsetList.Add(functionOffset);
        //                    Console.AppendText("\nA function is in: " + functionOffset.ToString());
        //                }
        //            }
        //            //com.numJump++;
        //            ////com.isEnd = 1;
        //            break;
        //        case 0x05:
        //            com.Name = "EndFunction";
        //            if (reader.BaseStream.Position + 1 > reader.BaseStream.Length) { com.isEnd = 1; break; }
        //            var next5 = reader.ReadByte();
        //            if (next5 == 0 || movOffsetList.Contains((uint)reader.BaseStream.Position - 1) || scriptOffList.Contains((uint)reader.BaseStream.Position - 1))
        //            {
        //                com.isEnd = 1;
        //                reader.BaseStream.Position -= 1;
        //                break;
        //            }
        //            else
        //                reader.BaseStream.Position -= 1;
        //            break;
        //        case 0x06:
        //            com.Name = "SetVar(06)";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x07:
        //            com.Name = "SetVar(07)";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x08:
        //            com.Name = "CompareTo";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x09:
        //            com.Name = "SetVar(09)";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x0A:
        //            com.Name = "ClearVar";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x0B:
        //            com.Name = "0B";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x0C:
        //        //    com.Name = "0C";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x0D:
        //        //    com.Name = "0D";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x0E:
        //        //    com.Name = "0E";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x0F:
        //        //    com.Name = "0F";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x10:
        //            com.Name = "StoreFlag";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x11:
        //            com.Name = "Condition";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x13:
        //            com.Name = "StoreVar(13)";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x14:
        //            com.Name = "14";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x16:
        //            com.Name = "16";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x17:
        //            com.Name = "17";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x19:
        //            com.Name = "Compare";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1C:
        //            com.Name = "CallStd";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadByte());//Standard Function Id.
        //            break;
        //        case 0x1D:
        //            com.Name = "ReturnStd";
        //            break;
        //        case 0x1E:
        //            com.Name = "Jump";
        //            com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
        //            functionOffset = com.parameters[0];
        //            if (!scriptStartList.Contains(functionOffset) && !functionOffsetList.Contains(functionOffset) && !movOffsetList.Contains(functionOffset))
        //            {
        //                functionOffsetList.Add(functionOffset);
        //                Console.AppendText("\nA function is in: " + functionOffset.ToString());
        //            }
        //            com.numJump++;
        //            com.isEnd = 1;
        //            break;
        //        case 0x1F:
        //            com.Name = "If";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
        //            functionOffset = com.parameters[1];
        //            if (!scriptStartList.Contains(functionOffset) && !functionOffsetList.Contains(functionOffset) && !movOffsetList.Contains(functionOffset))
        //            {
        //                functionOffsetList.Add(functionOffset);
        //                Console.AppendText("\nA function is in: " + functionOffset.ToString());
        //            }
        //            com.numJump++;
        //            break;
        //        case 0x21:
        //            com.Name = "21";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x22:
        //            com.Name = "22";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x23:
        //            com.Name = "SetFlag";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x24:
        //            com.Name = "ClearFlag";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x25:
        //            com.Name = "StoreVarFlag";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x26:
        //            com.Name = "StoreAddVar";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x27:
        //            com.Name = "StoreSubVar";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x28:
        //            com.Name = "StoreVarValue";
        //            com.parameters.Add(reader.ReadUInt16()); //Var as container
        //            com.parameters.Add(reader.ReadUInt16()); //Value to store
        //            break;
        //        case 0x29:
        //            com.Name = "StoreVarVariable";
        //            com.parameters.Add(reader.ReadUInt16()); //Var as container
        //            com.parameters.Add(reader.ReadUInt16()); //Value to store
        //            break;
        //        case 0x2A:
        //            com.Name = "StoreVarCallable";
        //            com.parameters.Add(reader.ReadUInt16()); //Var as container
        //            com.parameters.Add(reader.ReadUInt16()); //Value to store
        //            break;
        //        case 0x2B:
        //            com.Name = "StoreVar(2B)";
        //            com.parameters.Add(reader.ReadUInt16()); //Var as container
        //            com.parameters.Add(reader.ReadUInt16()); //Value to store
        //            break;
        //        case 0x2D:
        //            com.Name = "ClearVar(2D)";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x2E:
        //            com.Name = "LockAll";
        //            break;
        //        case 0x2F:
        //            com.Name = "UnlockAll";
        //            break;
        //        case 0x30:
        //            com.Name = "WaitMoment";
        //            break;
        //        case 0x31:
        //            com.Name = "31";
        //            break;
        //        case 0x32:
        //            com.Name = "WaitButton";
        //            break;
        //        case 0x33:
        //            com.Name = "MusicalMessage";
        //            com.parameters.Add(reader.ReadUInt16()); //Message Id
        //            break;
        //        case 0x34:
        //            com.Name = "EventGreyMessage";
        //            com.parameters.Add(reader.ReadUInt16()); //Message Id
        //            com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
        //            break;
        //        case 0x35:
        //            com.Name = "MusicalMessage2";
        //            com.parameters.Add(reader.ReadUInt16()); //Message Id
        //            com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
        //            break;
        //        case 0x36:
        //            com.Name = "CloseEventGreyMessage";
        //            break;
        //        case 0x37:
        //            com.Name = "37";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x38:
        //            com.Name = "BubbleMessage";
        //            com.parameters.Add(reader.ReadUInt16()); //Message Id
        //            com.parameters.Add(reader.ReadByte()); //Bottom/Top View.
        //            break;
        //        case 0x39:
        //            com.Name = "CloseBubbleMessage";
        //            break;
        //        case 0x3A:
        //            com.Name = "ShowMessageAt";
        //            com.parameters.Add(reader.ReadUInt16()); //Message Id
        //            com.parameters.Add(reader.ReadUInt16()); //X coordinate
        //            com.parameters.Add(reader.ReadUInt16()); //Y coordinate
        //            com.parameters.Add(reader.ReadUInt16()); //Y coordinate
        //            break;
        //        case 0x3B:
        //            com.Name = "CloseShowMessageAt";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x3C:
        //            com.Name = "Message";
        //            com.parameters.Add(reader.ReadByte()); //Costant
        //            com.parameters.Add(reader.ReadByte()); //Costant
        //            com.parameters.Add(reader.ReadUInt16()); //Message Id
        //            com.parameters.Add(reader.ReadUInt16()); //NPC Id
        //            if (reader.BaseStream.Position < reader.BaseStream.Length)
        //            {
        //                com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
        //                com.parameters.Add(reader.ReadUInt16()); //Message Type
        //            }
        //            break;
        //        case 0x3D:
        //            com.Name = "Message2";
        //            com.parameters.Add(reader.ReadByte()); //Costant
        //            com.parameters.Add(reader.ReadByte()); //Costant
        //            com.parameters.Add(reader.ReadUInt16()); //Message Id
        //            com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
        //            com.parameters.Add(reader.ReadUInt16()); //Message Type
        //            break;
        //        case 0x3E:
        //            com.Name = "CloseMessageKeyPress";
        //            break;
        //        case 0x3F:
        //            com.Name = "CloseMessageKeyPress2";
        //            break;
        //        case 0x40:
        //            com.Name = "MoneyBox";
        //            com.parameters.Add(reader.ReadUInt16()); //X coordinate
        //            com.parameters.Add(reader.ReadUInt16()); //Y coordinate
        //            break;
        //        case 0x41:
        //            com.Name = "CloseMoneyBox";
        //            break;
        //        case 0x42:
        //            com.Name = "UpdateMoneyBox";
        //            break;
        //        case 0x43:
        //            com.Name = "BorderedMessage";
        //            com.parameters.Add(reader.ReadUInt16()); //MessageId
        //            com.parameters.Add(reader.ReadUInt16()); //Color
        //            break;
        //        case 0x44:
        //            com.Name = "CloseBorderedMessage";
        //            break;
        //        case 0x45:
        //            com.Name = "PaperMessage";
        //            com.parameters.Add(reader.ReadUInt16()); //MessageId
        //            com.parameters.Add(reader.ReadUInt16()); //Trans. Coordinate
        //            break;
        //        case 0x46:
        //            com.Name = "ClosePaperMessage";
        //            break;
        //        case 0x47:
        //            com.Name = "YesNoBox";
        //            com.parameters.Add(reader.ReadUInt16()); //Variable: NO = 0, YES = 1
        //            break;
        //        case 0x48:
        //            com.Name = "Message3";
        //            com.parameters.Add(reader.ReadByte()); //Costant
        //            com.parameters.Add(reader.ReadByte()); //Costant
        //            com.parameters.Add(reader.ReadUInt16()); //Message Id
        //            com.parameters.Add(reader.ReadUInt16()); //NPC Id 
        //            com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
        //            com.parameters.Add(reader.ReadUInt16()); //Message Type
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x49:
        //            com.Name = "DoubleMessage";
        //            com.parameters.Add(reader.ReadByte()); //Costant
        //            com.parameters.Add(reader.ReadByte()); //Costant
        //            com.parameters.Add(reader.ReadUInt16()); //Id Message Black
        //            com.parameters.Add(reader.ReadUInt16()); //Id Message White
        //            com.parameters.Add(reader.ReadUInt16()); //NPC Id 
        //            com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
        //            com.parameters.Add(reader.ReadUInt16()); //Message Type
        //            break;
        //        case 0x4A:
        //            com.Name = "AngryMessage";
        //            com.parameters.Add(reader.ReadUInt16()); //MessageId
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16()); //Bottom/Top View.
        //            break;
        //        case 0x4B:
        //            com.Name = "CloseAngryMessage";
        //            break;
        //        case 0x4C:
        //            com.Name = "SetVarHero";
        //            com.parameters.Add(reader.ReadByte());
        //            break;
        //        case 0x4D:
        //            com.Name = "SetVarItem";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x4E:
        //            com.Name = "SetVarItemNumber";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadByte());
        //            break;
        //        case 0x4F:
        //            com.Name = "SetVarItemContainter";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x50:
        //            com.Name = "SetVarItemContained";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x51:
        //            com.Name = "SetVarMove";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x52:
        //            com.Name = "SetVarBag";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x53:
        //            com.Name = "SetVarPartyPokèmon";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x54:
        //            com.Name = "SetVarPartyPokèmonNick";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x55:
        //            com.Name = "SetVar(55)";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x56:
        //            com.Name = "SetVarType";		     //382
        //            com.parameters.Add(reader.ReadByte());   //text variable to set
        //            com.parameters.Add(reader.ReadUInt16()); //type to set
        //            break;
        //        case 0x57:
        //            com.Name = "SetVarPokèmon";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x58:
        //            com.Name = "SetVarPokèmon2";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x59:
        //            com.Name = "SetVarLocation";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x5A:
        //            com.Name = "SetVarPokèmonNick";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x5B:
        //            com.Name = "SetVar(58)";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x5C: // example 654
        //            com.Name = "SetVarNumberCond";
        //            com.parameters.Add(reader.ReadByte()); // 1 ?
        //            com.parameters.Add(reader.ReadUInt16()); // Container to store to
        //            com.parameters.Add(reader.ReadUInt16()); // Stat to Store [HP ATK DEF SPE SPA SPD, 0-5]
        //            break;
        //        case 0x5D:
        //            com.Name = "SetVarMusicalInfo";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x5E:
        //            com.Name = "SetVarNations";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x5F:
        //            com.Name = "SetVarActivities";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x60:
        //            com.Name = "SetVarPower";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x61:
        //            com.Name = "SetVarTrainerType";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x62:
        //            com.Name = "SetVarTrainerType2";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x63:
        //            com.Name = "SetVarGeneralWord";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x64:
        //            com.Name = "ApplyMovement";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt32() + (uint)reader.BaseStream.Position);
        //            var movOffset = com.parameters[1];
        //            if (!movOffsetList.Contains(movOffset))
        //            {
        //                movOffsetList.Add(movOffset);
        //                Console.AppendText("\nA movement is in: " + movOffset.ToString());
        //            }
        //            break;
        //        case 0x65:
        //            com.Name = "WaitMovement";
        //            break;
        //        case 0x66:
        //            com.Name = "66";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x67:
        //            com.Name = "67";
        //            com.parameters.Add(reader.ReadUInt16());
        //            uint variable = 0;
        //            do
        //            {
        //                variable = reader.ReadUInt16();
        //                if (variable < 0x8000)
        //                    reader.BaseStream.Position -= 2;
        //                else
        //                    com.parameters.Add(variable);
        //            }
        //            while (variable > 0x8000);

        //            break;
        //        case 0x68:
        //            com.Name = "StoreHeroPosition";
        //            com.parameters.Add(reader.ReadUInt16()); // Variable as X container.
        //            com.parameters.Add(reader.ReadUInt16()); // Variable as Y container.
        //            break;
        //        case 0x69:
        //            com.Name = "StoreNPCPosition";
        //            com.parameters.Add(reader.ReadUInt16()); // NPC Id
        //            com.parameters.Add(reader.ReadUInt16()); // Variable as X container.
        //            com.parameters.Add(reader.ReadUInt16()); // Variable as Y container.
        //            com.parameters.Add(reader.ReadUInt16()); // NPC Id
        //            com.parameters.Add(reader.ReadUInt16()); // Variable as X container.
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x6A:
        //            com.Name = "StoreNPCFlag";
        //            com.parameters.Add(reader.ReadUInt16()); //NPC Id
        //            com.parameters.Add(reader.ReadUInt16()); //Flag
        //            break;
        //        case 0x6B:
        //            com.Name = "AddNPC";
        //            com.parameters.Add(reader.ReadUInt16()); //Npc Id
        //            break;
        //        case 0x6C:
        //            com.Name = "RemoveNPC";
        //            com.parameters.Add(reader.ReadUInt16()); //Npc Id
        //            break;
        //        case 0x6D:
        //            com.Name = "SetOWPosition";
        //            com.parameters.Add(reader.ReadUInt16()); //Npc Id
        //            com.parameters.Add(reader.ReadUInt16()); //X coordinate
        //            com.parameters.Add(reader.ReadUInt16()); //Y coordinate
        //            com.parameters.Add(reader.ReadUInt16()); //Z coordinate
        //            com.parameters.Add(reader.ReadUInt16()); //Face direction
        //            break;
        //        case 0x6E:
        //            com.Name = "StoreHeroNPCOrientation";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x6F:
        //            com.Name = "6F";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x70:
        //            com.Name = "70";
        //            com.parameters.Add(reader.ReadUInt16()); //Ow_Id (Variable)
        //            com.parameters.Add(reader.ReadUInt16()); //Return Value
        //            com.parameters.Add(reader.ReadUInt16()); //X
        //            com.parameters.Add(reader.ReadUInt16()); //S_854 = 0, S_855 = 3
        //            com.parameters.Add(reader.ReadUInt16()); //Y
        //            break;
        //        //case 0x71:
        //        //    com.Name = "71";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x72:
        //        //    com.Name = "72";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x73:
        //            com.Name = "73";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x74:
        //            com.Name = "FacePlayer";
        //            break;
        //        case 0x75:
        //            com.Name = "Release";
        //            com.parameters.Add(reader.ReadUInt16()); //NPC Id
        //            break;
        //        case 0x76:
        //            com.Name = "ReleaseAll";
        //            break;
        //        case 0x77:
        //            com.Name = "Lock";
        //            com.parameters.Add(reader.ReadUInt16()); //NPC Id
        //            break;
        //        case 0x78:
        //            com.Name = "CheckLock";
        //            com.parameters.Add(reader.ReadUInt16()); //variable
        //            break;
        //        case 0x79:
        //            com.Name = "StoreNPCLevel";
        //            com.parameters.Add(reader.ReadUInt16()); //NPC Id
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x7B:
        //            com.Name = "MoveNpctoCoordinates";
        //            com.parameters.Add(reader.ReadUInt16()); //Npc Id
        //            com.parameters.Add(reader.ReadUInt16()); //X coordinate
        //            com.parameters.Add(reader.ReadUInt16()); //Y coordinate
        //            com.parameters.Add(reader.ReadUInt16()); //Z coordinate
        //            break;
        //        case 0x7C:
        //            com.Name = "7C";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x7D:
        //        //    com.Name = "7D";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x7E:
        //            com.Name = "TeleportUpNPc";
        //            com.parameters.Add(reader.ReadUInt16()); //Npc Id
        //            break;
        //        case 0x7F:
        //            com.Name = "7F";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x80:
        //            com.Name = "80";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x81:
        //            com.Name = "81";
        //            break;
        //        case 0x82:
        //            com.Name = "82";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x83:
        //            com.Name = "SetVar(83)";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x84:
        //            com.Name = "SetVar(84)";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x85:
        //            com.Name = "SingleTrainerBattle";
        //            com.parameters.Add(reader.ReadUInt16()); //Trainer Id
        //            com.parameters.Add(reader.ReadUInt16()); //2th Trainer Id (If 0x0 Single Battle)
        //            com.parameters.Add(reader.ReadUInt16()); //win loss logic (0 standard, 1 loss=>win)
        //            break;
        //        case 0x86:
        //            com.Name = "DoubleTrainerBattle";
        //            com.parameters.Add(reader.ReadUInt16()); //Ally
        //            com.parameters.Add(reader.ReadUInt16()); //Opp1 Trainer Id
        //            com.parameters.Add(reader.ReadUInt16()); //Opp2 Trainer Id
        //            com.parameters.Add(reader.ReadUInt16()); //win loss logic (0 standard, 1 loss=>win)
        //            break;
        //        case 0x87:
        //            com.Name = "MessageBattle";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16()); //Message Id?
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x88:
        //            com.Name = "MessageBattle2";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x8A:
        //            com.Name = "8A";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x8B:
        //            com.Name = "PlayTrainerMusic";
        //            com.parameters.Add(reader.ReadUInt16()); // music to play
        //            break;
        //        case 0x8C:
        //            com.Name = "EndBattle";
        //            break;
        //        case 0x8D:
        //            com.Name = "StoreBattleResult";
        //            com.parameters.Add(reader.ReadUInt16()); //Variable as container.
        //            break;
        //        case 0x8E:
        //            com.Name = "DisableTrainer";
        //            break;
        //        case 0x90:
        //            com.Name = "90";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x92:
        //            com.Name = "92";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x93:
        //            com.Name = "93";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x94:
        //            com.Name = "TrainerBattle";
        //            com.parameters.Add(reader.ReadUInt16());//Trainer ID
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x95:
        //            com.Name = "DeactiveTrainerId";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x96:
        //            com.Name = "96";
        //            com.parameters.Add(reader.ReadUInt16()); //Trainer ID
        //            break;
        //        case 0x97:
        //            com.Name = "StoreActiveTrainerId";
        //            com.parameters.Add(reader.ReadUInt16()); //Trainer ID
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x98:
        //            com.Name = "ChangeMusic";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x9E:
        //            com.Name = "FadeToDefaultMusic";
        //            break;
        //        case 0x9F:
        //            com.Name = "PlayMusic";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xA1:
        //            com.Name = "StopMusic";
        //            com.parameters.Add(reader.ReadUInt16()); //sound?
        //            break;
        //        case 0xA2:
        //            com.Name = "A2";
        //            com.parameters.Add(reader.ReadUInt16()); //sound?
        //            com.parameters.Add(reader.ReadUInt16()); //strain
        //            break;
        //        case 0xA3:
        //            com.Name = "AddInstrument";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xA4:
        //            com.Name = "A4";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xA5:
        //            com.Name = "CheckInstrument";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xA6:
        //            com.Name = "PlaySound";
        //            com.parameters.Add(reader.ReadUInt16()); //Sound Id
        //            break;
        //        case 0xA7:
        //            com.Name = "WaitSound(A7)";
        //            break;
        //        case 0xA8:
        //            com.Name = "WaitSound(A8)";
        //            break;
        //        case 0xA9:
        //            com.Name = "PlayFanfare";
        //            com.parameters.Add(reader.ReadUInt16()); //Fanfare Id
        //            break;
        //        case 0xAA:
        //            com.Name = "WaitFanfare";
        //            break;
        //        case 0xAB:
        //            com.Name = "Cry";
        //            com.parameters.Add(reader.ReadUInt16()); //Pokemon Index #
        //            com.parameters.Add(reader.ReadUInt16()); //0 ~ unknown
        //            break;
        //        case 0xAC:
        //            com.Name = "WaitCry";
        //            break;
        //        case 0xAF:
        //            com.Name = "SetTextScriptMessage";
        //            com.parameters.Add(reader.ReadUInt16()); //Message Id
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xB0:
        //            com.Name = "CloseMulti";
        //            break;
        //        case 0xB1:
        //            com.Name = "B1";
        //            break;
        //        case 0xB2:
        //            com.Name = "Multi(B2)";			//88
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xB3:
        //            com.Name = "FadeScreen";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xB4:
        //            com.Name = "ResetScreen";
        //            break;
        //        case 0xB5:
        //            com.Name = "GiveItem";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xB6:
        //            com.Name = "TakeItem";
        //            com.parameters.Add(reader.ReadUInt16()); // Item Index Number
        //            com.parameters.Add(reader.ReadUInt16()); // Quantity
        //            com.parameters.Add(reader.ReadUInt16()); // Return Result (0=added successfully | 1=full bag)
        //            break;
        //        case 0xB7:
        //            com.Name = "CheckItemBagSpace";		//Store if it is possible to give an item.
        //            com.parameters.Add(reader.ReadUInt16()); // Item Index Number
        //            com.parameters.Add(reader.ReadUInt16()); // Quantity
        //            com.parameters.Add(reader.ReadUInt16()); // Return Result (0=not full | 1=full)
        //            break;
        //        case 0xB8:
        //            com.Name = "CheckItemBagNumber";              //222
        //            com.parameters.Add(reader.ReadUInt16()); // Item #
        //            com.parameters.Add(reader.ReadUInt16()); // Minimum Quantity / Return X if has >=1
        //            com.parameters.Add(reader.ReadUInt16()); // Result Storage variable/container
        //            break;
        //        case 0xB9:
        //            com.Name = "StoreItemCount";
        //            com.parameters.Add(reader.ReadUInt16()); // item #
        //            com.parameters.Add(reader.ReadUInt16()); // Return to storage
        //            break;
        //        case 0xBA:
        //            com.Name = "CheckItemContainer";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xBB:
        //            com.Name = "StoreItemBag";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xBC:
        //            com.Name = "BC";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xBD:
        //            com.Name = "BD";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xBE:
        //            com.Name = "Warp";
        //            com.parameters.Add(reader.ReadUInt16()); //Map Id
        //            com.parameters.Add(reader.ReadUInt16()); // X coordinate
        //            com.parameters.Add(reader.ReadUInt16()); // Y coordinate
        //            break;
        //        case 0xBF:
        //            com.Name = "TeleportWarp";
        //            com.parameters.Add(reader.ReadUInt16()); //Map Id
        //            com.parameters.Add(reader.ReadUInt16()); // X coordinate
        //            com.parameters.Add(reader.ReadUInt16()); // Y coordinate
        //            com.parameters.Add(reader.ReadUInt16()); // Z coordinate
        //            break;
        //        case 0xC1:
        //            com.Name = "FallWarp";
        //            com.parameters.Add(reader.ReadUInt16()); //Map Id
        //            com.parameters.Add(reader.ReadUInt16()); // X coordinate
        //            com.parameters.Add(reader.ReadUInt16()); // Y coordinate
        //            break;
        //        case 0xC2:
        //            com.Name = "FastWarp";
        //            com.parameters.Add(reader.ReadUInt16()); //Map Id
        //            com.parameters.Add(reader.ReadUInt16()); // X coordinate
        //            com.parameters.Add(reader.ReadUInt16()); // Y coordinate
        //            com.parameters.Add(reader.ReadUInt16()); // Hero's Facing
        //            break;
        //        case 0xC3:
        //            com.Name = "UnionWarp"; // warp to union room
        //            break;
        //        case 0xC4:
        //            com.Name = "TeleportWarp";
        //            com.parameters.Add(reader.ReadUInt16()); //Map Id
        //            com.parameters.Add(reader.ReadUInt16()); // X coordinate
        //            com.parameters.Add(reader.ReadUInt16()); // Y coordinate
        //            com.parameters.Add(reader.ReadUInt16()); // Z coordinate
        //            com.parameters.Add(reader.ReadUInt16()); // Hero's Facing
        //            break;
        //        case 0xC5:
        //            com.Name = "SurfAnimation";
        //            break;
        //        case 0xC6:
        //            com.Name = "SpecialAnimation";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xC7:
        //            com.Name = "SpecialAnimation2";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xC8:
        //            com.Name = "CallAnimation";
        //            com.parameters.Add(reader.ReadUInt16()); //Animation Id
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xCB:
        //            com.Name = "StoreRandomNumber";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xCC:
        //            com.Name = "StoreVarItem";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xCD:
        //            com.Name = "StoreDayPart";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xCE:
        //            com.Name = "CE";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xCF:
        //            com.Name = "StoreDay";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xD0:
        //            com.Name = "StoreDate";
        //            com.parameters.Add(reader.ReadUInt16()); //Month Return to var/cont
        //            com.parameters.Add(reader.ReadUInt16()); //Day Return to var/cont
        //            break;
        //        case 0xD1:
        //            com.Name = "D1";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xD2:
        //            com.Name = "StoreSeason";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xD3:
        //            com.Name = "D3";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xD4:
        //            com.Name = "StoreBirthDay";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xD5:
        //            com.Name = "StoreBadge";
        //            com.parameters.Add(reader.ReadUInt16()); //Variable to return
        //            com.parameters.Add(reader.ReadUInt16()); //Badge Id
        //            break;
        //        case 0xD6:
        //            com.Name = "SetBadge";
        //            com.parameters.Add(reader.ReadUInt16()); //Badge Id
        //            break;
        //        case 0xD7:
        //            com.Name = "StoreBadgeNumber";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xD8:
        //            com.Name = "D8";
        //            com.parameters.Add(reader.ReadUInt16()); //Variable to return
        //            com.parameters.Add(reader.ReadUInt16()); //Badge Id
        //            break;
        //        case 0xD9:
        //            com.Name = "D9";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xDA:
        //            com.Name = "DA";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xDB:
        //            com.Name = "DB";
        //            break;
        //        case 0xDC:
        //            com.Name = "DC";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xDD:
        //            com.Name = "StorePokèmonCaught";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;

        //        case 0xDE:
        //            com.Name = "SpeciesDisplayDE"; // species display popup, Store
        //            com.parameters.Add(reader.ReadUInt16()); //0
        //            com.parameters.Add(reader.ReadUInt16()); //species
        //            break;
        //        case 0xDF:
        //            com.Name = "DF"; // species display popup, Store
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xE0:
        //            com.Name = "StoreVersion";
        //            com.parameters.Add(reader.ReadUInt16()); //return result to this variable/cont
        //            break;
        //        case 0xE1:
        //            com.Name = "StoreGender";
        //            com.parameters.Add(reader.ReadUInt16()); //return result to this variable/cont
        //            break;
        //        case 226:
        //            com.Name = "StoreE2";
        //            com.parameters.Add(reader.ReadUInt16()); //return result to this variable/cont
        //            break;
        //        case 0xE4:
        //            com.Name = "E4";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xE5:
        //            com.Name = "StoreE5";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 230:
        //            com.Name = "CheckKeyItem";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xE7:
        //            com.Name = "ActivateKeyItem";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 232:
        //            com.Name = "CheckKeyItem2";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xE9:
        //            com.Name = "E9";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xEA:
        //            com.Name = "EA";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xEB:
        //            com.Name = "EB";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xEC:
        //            com.Name = "EC";
        //            break;
        //        case 0xED:
        //            com.Name = "ED";
        //            break;
        //        case 0xEE:
        //            com.Name = "EE";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xEF:
        //            com.Name = "EF";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xF0:
        //            com.Name = "GivePokèmonMansion";
        //            com.parameters.Add(reader.ReadUInt16()); //POKE
        //            break;
        //        case 0xF1:
        //            com.Name = "F1";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xF2:
        //            com.Name = "F2";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xF3:
        //            com.Name = "F3";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xF4:
        //            com.Name = "F4";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xF5:
        //            com.Name = "F5";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xF6:
        //            com.Name = "F6";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xF7:
        //            com.Name = "F7";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xF8:
        //            com.Name = "F8";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xF9:
        //            com.Name = "F9";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xFA:
        //            com.Name = "TakeMoney";			//66
        //            com.parameters.Add(reader.ReadUInt16()); //Removes this amount of money from the player's $.
        //            break;
        //        case 0xFB:
        //            com.Name = "CheckEnoughMoney";			//66
        //            com.parameters.Add(reader.ReadUInt16()); //Result storage container (0=enough $|1=not enough $)
        //            com.parameters.Add(reader.ReadUInt16()); //Stores if current $ is >= [THIS ARGUMENT]
        //            break;
        //        case 0xFC:
        //            com.Name = "StorePokèmonHappiness";
        //            com.parameters.Add(reader.ReadUInt16()); //Happiness storage container
        //            com.parameters.Add(reader.ReadUInt16()); //Party member to Store
        //            break;
        //        case 0xFD:
        //            com.Name = "IncPokèmonHappiness";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0xFE:
        //            com.Name = "StorePartySpecies";
        //            if (reader.BaseStream.Position == reader.BaseStream.Length) //this is temporary to catch movement related errors
        //            {
        //                com.isEnd = 1;
        //                break;
        //            }
        //            com.parameters.Add(reader.ReadUInt16());    // Result Storage of Storeed species index #
        //            if (reader.BaseStream.Position + 1 >= reader.BaseStream.Length)
        //            {
        //                com.isEnd = 1;
        //                break;
        //            }
        //            else
        //            {
        //                com.parameters.Add(reader.ReadUInt16()); // PKM to Store
        //                break;
        //            }
        //        case 0xFF:
        //            com.Name = "StorePokèmonFormNumber";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            var nextFF = reader.ReadUInt16();
        //            while (nextFF >= 0x4000) { com.parameters.Add(nextFF); nextFF = reader.ReadUInt16(); }
        //            reader.BaseStream.Position -= 2;
        //            break;
        //        case 0x101:
        //            com.Name = "101";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x102:
        //            com.Name = "CheckEgg";
        //            com.parameters.Add(reader.ReadUInt16()); //Result storage container
        //            com.parameters.Add(reader.ReadUInt16()); //Party member to Store
        //            break;
        //        case 0x103:
        //            com.Name = "StorePartyNumberMinimum";
        //            com.parameters.Add(reader.ReadUInt16()); //Result Storage container
        //            com.parameters.Add(reader.ReadUInt16()); //Does the player have more than [VALUE]? Return 0 if true.
        //            break;
        //        case 0x104:
        //            com.Name = "HealPokèmon";
        //            break;
        //        case 0x105:
        //            com.Name = "RenamePokèmon";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x106:
        //            com.Name = "106";
        //            com.parameters.Add(reader.ReadUInt16()); //Req
        //            break;
        //        case 0x107:
        //            com.Name = "StoreChosenPokèmon";
        //            com.parameters.Add(reader.ReadUInt16()); //Dialog Result Logic (1 if PKM Chosen) default 0
        //            com.parameters.Add(reader.ReadUInt16()); //    \->Variable Storage
        //            com.parameters.Add(reader.ReadUInt16()); //Pokemon Choice Variable Storage
        //            break;
        //        case 0x108:
        //            com.Name = "StorePokèmonMoveLearned";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x109:
        //            com.Name = "ChooseMoveForgot";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x10A:
        //            com.Name = "StorePokèmonMoveForgot";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x10B:
        //            com.Name = "10B";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x10C:
        //            com.Name = "GivePokèmon";
        //            com.parameters.Add(reader.ReadUInt16()); //Id Pokèmon
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16()); //Item
        //            com.parameters.Add(reader.ReadUInt16()); //Level
        //            break;
        //        case 0x10D:
        //            com.Name = "StorePokemonPartyAt";
        //            com.parameters.Add(reader.ReadUInt16()); //Id Pokèmon Party
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x10E:
        //            com.Name = "GivePokèmon(10E)";
        //            com.parameters.Add(reader.ReadUInt16()); //Variable to return result to
        //            com.parameters.Add(reader.ReadUInt16()); //Egg Pokemon to try to
        //            com.parameters.Add(reader.ReadUInt16()); //Forme
        //            com.parameters.Add(reader.ReadUInt16()); //Level
        //            com.parameters.Add(reader.ReadUInt16()); //3
        //            com.parameters.Add(reader.ReadUInt16()); //2
        //            com.parameters.Add(reader.ReadUInt16()); //0
        //            com.parameters.Add(reader.ReadUInt16()); //0
        //            com.parameters.Add(reader.ReadUInt16()); //4
        //            break;
        //        case 0x10F:
        //            com.Name = "GiveEgg(10F)";
        //            com.parameters.Add(reader.ReadUInt16()); //Variable to return result to
        //            com.parameters.Add(reader.ReadUInt16()); //Egg Pokemon to try to
        //            com.parameters.Add(reader.ReadUInt16()); //Response if Party is Full (~0=true or FORME?)
        //            break;
        //        case 0x110:
        //            com.Name = "StorePokèmonSex";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x113:
        //            com.Name = "113";
        //            com.parameters.Add(reader.ReadUInt16()); //var
        //            com.parameters.Add(reader.ReadUInt16()); //var
        //            break;
        //        case 0x114:
        //            com.Name = "StorePartyHavePokèmon";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16()); //RV
        //            break;
        //        case 0x115:
        //            com.Name = "StorePartyCanLearnMove";
        //            com.parameters.Add(reader.ReadUInt16()); //Variable to return result to
        //            com.parameters.Add(reader.ReadUInt16()); //move to Store
        //            com.parameters.Add(reader.ReadUInt16()); //Party member to Store
        //            break;
        //        case 0x116:
        //            com.Name = "StorePartyCanUseMove";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16()); //RV
        //            break;
        //        case 0x117:
        //            com.Name = "StorePokemonForm";
        //            com.parameters.Add(reader.ReadUInt16()); //var
        //            com.parameters.Add(reader.ReadUInt16()); //val
        //            break;
        //        case 0x118:
        //            com.Name = "CheckChosenSpecies";
        //            com.parameters.Add(reader.ReadUInt16()); //Specie
        //            com.parameters.Add(reader.ReadUInt16()); //RV
        //            com.parameters.Add(reader.ReadUInt16()); //Pokèmon
        //            break;
        //        //case 0x11A:
        //        //    com.Name = "11A";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x11B:
        //            com.Name = "StorePartyType";
        //            com.parameters.Add(reader.ReadUInt16()); // Return Type 1
        //            com.parameters.Add(reader.ReadUInt16()); // Return Type 2
        //            com.parameters.Add(reader.ReadUInt16()); // Party member to Store
        //            break;
        //        case 0x11C:
        //            com.Name = "ForgotMove";
        //            com.parameters.Add(reader.ReadUInt16()); //var
        //            com.parameters.Add(reader.ReadUInt16()); //var
        //            com.parameters.Add(reader.ReadUInt16()); //var
        //            break;
        //        //case 0x11D:
        //        //    com.Name = "SetFavorite";			//82
        //        //    com.parameters.Add(reader.ReadUInt16()); //Party member to set as favorite Pokemon
        //        //    break;
        //        case 0x11E:
        //            com.Name = "BadgeAnimation";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x11F:
        //            com.Name = "StorePokèmonPartyNumberBadge";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x120:
        //            com.Name = "SetVarPokèmonTrade";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x121:
        //            com.Name = "CheckPartyAmount";
        //            com.parameters.Add(reader.ReadUInt16());//Bool(Ret)
        //            com.parameters.Add(reader.ReadUInt16());//Amount
        //            break;
        //        case 0x124:
        //            com.Name = "124";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x126:
        //            com.Name = "126";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x127:
        //            com.Name = "127";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());//Ow
        //            com.parameters.Add(reader.ReadUInt16());//X
        //            com.parameters.Add(reader.ReadUInt16());//Y
        //            break;
        //        case 0x128:
        //            com.Name = "128";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x129:
        //            com.Name = "129";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x12A:
        //            com.Name = "12A";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x12B:
        //        //    com.Name = "12B";
        //        //    com.parameters.Add(reader.ReadUInt16()); //Req
        //        //    com.parameters.Add(reader.ReadUInt16()); //Req
        //        //    com.parameters.Add(reader.ReadUInt16()); //Req
        //        //    break;
        //        case 0x12C:
        //            com.Name = "12C";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x12D:
        //            com.Name = "12D";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16()); //0
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x12E:
        //            com.Name = "12E";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x12F:
        //            com.Name = "12F";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x130:
        //            com.Name = "BootPCSound";
        //            break;
        //        case 0x131:
        //            com.Name = "PC-131";
        //            break;
        //        case 0x132:
        //            com.Name = "132";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x134:
        //            com.Name = "134";
        //            break;
        //        //case 0x136:
        //        //    com.Name = "136";
        //        //    com.parameters.Add(reader.ReadByte());
        //        //    break;
        //        case 0x137:
        //            com.Name = "ShowClockSaving";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x138:
        //            com.Name = "138";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x139:
        //            com.Name = "SaveData(139)";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x13A:
        //            com.Name = "13A";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x13B:
        //            com.Name = "CheckWireless";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x13C:
        //        //    com.Name = "13C";
        //        //    break;
        //        //case 0x13D:
        //        //    com.Name = "13D";
        //        //    com.parameters.Add(reader.ReadByte()); //Elevation
        //        //    com.parameters.Add(reader.ReadUInt16()); //Rotation
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16()); //Zoom
        //        //    //com.parameters.Add(reader.ReadUInt16());
        //        //    //com.parameters.Add(reader.ReadUInt16());
        //        //    //com.parameters.Add(reader.ReadUInt16());
        //        //    //com.parameters.Add(reader.ReadUInt16());
        //        //    //com.parameters.Add(reader.ReadUInt16());
        //        //    //com.parameters.Add(reader.ReadUInt16());
        //        //    //com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x13F:
        //            com.Name = "StartCameraEvent";
        //            break;
        //        case 0x140:
        //            com.Name = "StopCameraEvent";
        //            break;
        //        case 0x141:
        //            com.Name = "LockCamera";
        //            break;
        //        case 0x142:
        //            com.Name = "ReleaseCamera";
        //            break;
        //        case 0x143:
        //            com.Name = "MoveCamera";
        //            com.parameters.Add(reader.ReadUInt16()); //Elevation
        //            com.parameters.Add(reader.ReadUInt16()); //Rotation
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16()); //Zoom
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x144:
        //            com.Name = "144";
        //            com.parameters.Add(reader.ReadUInt16()); //Elevation
        //            break;
        //        case 0x145:
        //            com.Name = "EndCameraEvent";
        //            break;
        //        case 0x147:
        //            com.Name = "ResetCamera";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x148:
        //            com.Name = "BumpingCamera";
        //            com.parameters.Add(reader.ReadUInt16()); //Intensità X
        //            com.parameters.Add(reader.ReadUInt16()); //Intensità Y
        //            com.parameters.Add(reader.ReadUInt16()); //Speed
        //            com.parameters.Add(reader.ReadUInt16()); //Degradation
        //            com.parameters.Add(reader.ReadUInt16()); //Changing Speed (From Bumping X to Y)
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x149:
        //            com.Name = "149";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x14B:
        //            com.Name = "CallStart";
        //            break;
        //        case 0x14A:
        //            com.Name = "CallEnd";
        //            break;
        //        case 0x14D:
        //            com.Name = "14D";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x14E:
        //            com.Name = "ChooseInterestingItem";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x14F:
        //            com.Name = "14F";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x150:
        //            com.Name = "150";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x151:
        //            com.Name = "ShowDiploma";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x154:
        //            com.Name = "LibertyShipAnm";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x155:
        //            com.Name = "OpenInterpokè";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x156:
        //            com.Name = "156";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x158:
        //            com.Name = "158";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x159:
        //        //    com.Name = "159";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x15A:
        //            com.Name = "15A";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x15B:
        //        //    com.Name = "15B";
        //        //    com.parameters.Add(reader.ReadByte());
        //        //    break;
        //        //case 0x15C:
        //        //    com.Name = "15C";
        //        //    com.parameters.Add(reader.ReadByte());
        //        //    break;
        //        case 0x15F:
        //            com.Name = "CheckFriend";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x160:
        //            com.Name = "160";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x15C:
        //        //    com.Name = "15C";
        //        //    com.parameters.Add(reader.ReadByte());
        //        //    break;
        //        //case 0x163:
        //        //    com.Name = "163";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadByte());
        //        //    break;
        //        case 0x164:
        //            com.Name = "164";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x165:
        //            com.Name = "165";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x166:
        //        //    com.Name = "166";
        //        //    com.parameters.Add(reader.ReadByte());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x167:
        //            com.Name = "StartPokèmonMusical";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            //com.parameters.Add(reader.ReadUInt16());
        //            //com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x168:
        //            com.Name = "StartDressPokèmonMusical";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x169:
        //            com.Name = "CheckPokèmonMusicalFunctions";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x16A:
        //            com.Name = "16A";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x16B:
        //            com.Name = "PokèmonMenuMusicalFunctions";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x16C:
        //            com.Name = "16C";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x16D:
        //            com.Name = "16D";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x16E:
        //            com.Name = "ChoosePokèmonMusical";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x16F:
        //            com.Name = "16F";
        //            break;
        //        case 0x170:
        //            com.Name = "170";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x171:
        //            com.Name = "171";
        //            break;
        //        case 0x172:
        //            com.Name = "172";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x173:
        //            com.Name = "173";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x174:
        //            com.Name = "174";
        //            com.parameters.Add(reader.ReadUInt16()); //Dex
        //            com.parameters.Add(reader.ReadUInt16()); //Level
        //            com.parameters.Add(reader.ReadUInt16()); //Unk
        //            break;
        //        case 0x175:
        //            com.Name = "175";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x176:
        //            com.Name = "176";
        //            com.parameters.Add(reader.ReadUInt16());
        //            if (scriptType == Constants.BW2SCRIPT)
        //                com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x177:
        //            com.Name = "177";
        //            if (scriptType == Constants.BW2SCRIPT)
        //                com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x178:
        //            com.Name = "WildPokèmonBattle";	     // 364	0=captured, might output 1 & 2 for something else
        //            com.parameters.Add(reader.ReadUInt16()); //variable to store result to
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x179:
        //            com.Name = "EndWildBattle";
        //            break;
        //        case 0x17A:
        //            com.Name = "LooseWildBattle";
        //            break;
        //        case 0x17B:
        //            com.Name = "StoreWildBattleResult";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x17C:
        //            com.Name = "StoreWildBattlePokèmonStatus";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x17D:
        //        //    com.Name = "17D";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x17E:
        //        //    com.Name = "17E";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x17F:
        //            com.Name = "17F";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x180:
        //            com.Name = "NimbasaGymRailAnimation";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x181:
        //        //    com.Name = "181";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x182:
        //            com.Name = "182";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x183:
        //        //    com.Name = "183";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x184:
        //        //    com.Name = "184";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x185:
        //        //    com.Name = "185";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x186:
        //            com.Name = "186";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x187:
        //            com.Name = "187";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x188:
        //            com.Name = "188";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x189:
        //            com.Name = "189";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x18A:
        //        //    com.Name = "18A";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x18B:
        //        //    com.Name = "18B";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x18C:
        //            com.Name = "18C";
        //            com.parameters.Add(reader.ReadUInt16());
        //            //com.parameters.Add(reader.ReadUInt16());
        //            //com.parameters.Add(reader.ReadUInt16());
        //            //com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x18D:
        //            com.Name = "18D";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x18E:
        //            com.Name = "18E";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x18F:
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x190:
        //            com.Name = "DriftGymLiftAnmSecondRoom";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x191:
        //            com.Name = "191";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x192:
        //            com.Name = "DriftGymLiftAnmFirstRoom";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x193:
        //        //    com.Name = "193";
        //        //    break;
        //        //case 0x195:
        //        //    com.Name = "195";
        //        //    break;
        //        //case 0x199:
        //        //    com.Name = "199";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x19B:
        //            com.Name = "SetStatusCG";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x19C:
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x19D:
        //            com.Name = "19D";
        //            break;
        //        case 0x19E:
        //            com.Name = "ShowCG";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x19F:
        //            com.Name = "CallScreenAnimation";
        //            com.parameters.Add(reader.ReadUInt16()); //AnimationId
        //            break;
        //        case 0x1A0:
        //            com.Name = "1A0";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1A1:
        //            com.Name = "OpenXtransciever(1A1)";
        //            com.parameters.Add(reader.ReadUInt16());
        //            //com.parameters.Add(reader.ReadUInt16());
        //            //com.parameters.Add(reader.ReadUInt16());
        //            //com.parameters.Add(reader.ReadUInt16()); // container
        //            break;
        //        case 0x1A3:
        //            com.Name = "FlashBlackInstant";
        //            break;
        //        case 0x1A4:
        //            com.Name = "1A4";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x1A5:
        //        //    com.Name = "Xtransciever5 (0x1A5)";
        //        //    break;
        //        //case 0x1A6:
        //        //    com.Name = "Xtransciever6 (0x1A6)";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x1A7:
        //            com.Name = "1A7";
        //            //com.parameters.Add(reader.ReadUInt16());
        //            //com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x1A8:
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x1A9:
        //            com.Name = "1A9";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1AA:
        //            com.Name = "1AA";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1AB:
        //            com.Name = "FadeFromBlack";
        //            break;
        //        case 0x1AC:
        //            com.Name = "FadeIntoBlack";
        //            break;
        //        case 0x1AD:
        //            com.Name = "FadeFromWhite";
        //            break;
        //        case 0x1AE:
        //            com.Name = "FadeIntoWhite";
        //            break;
        //        //case 0x1AF:
        //        //    com.Name = "1AF";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x1B1:
        //            com.Name = "ScreenFunction";
        //            break;
        //        //case 0x1B4:
        //        //    com.Name = "TradeNPCStart";
        //        //    com.parameters.Add(reader.ReadUInt16()); //Trade Entry to Trade For
        //        //    com.parameters.Add(reader.ReadUInt16()); //PKM Slot that gets traded away FOREVER!
        //        //    break;
        //        //case 0x1B5:
        //        //    com.Name = "TradeNPCQualify";
        //        //    com.parameters.Add(reader.ReadUInt16()); //Logic Criterion (usually set to 0~true)
        //        //    com.parameters.Add(reader.ReadUInt16()); //Trade Criterion
        //        //    com.parameters.Add(reader.ReadUInt16()); //Offered PKM
        //        //    break;
        //        case 0x1B5:
        //            com.Name = "1B5";
        //            break;
        //        case 0x1B6:
        //            com.Name = "1B6";
        //            break;
        //        case 0x1B9:
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1BA:
        //            com.Name = "1BA";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1BB:
        //            com.Name = "1BB";
        //            break;
        //        //case 0x1BD:
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x1BE:
        //            com.Name = "TradePokèmon";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1BF:
        //            com.Name = "CompareChosenPokemon";
        //            com.parameters.Add(reader.ReadUInt16());//RV = True if is equal
        //            com.parameters.Add(reader.ReadUInt16());//Id chosen Pokèmon
        //            com.parameters.Add(reader.ReadUInt16());//Id requested Pokèmon
        //            break;
        //        case 0x1C0:
        //            com.Name = "1C0";
        //            break;
        //        case 0x1C1:
        //            com.Name = "1C1";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1C2:
        //            com.Name = "StartEventBC";
        //            break;
        //        case 0x1C3:
        //            com.Name = "EndEventBC";
        //            break;
        //        case 0x1C4:
        //            com.Name = "StoreTrainerID";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x1C5:
        //        //    com.Name = "1C5";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    var next1C5 = reader.ReadUInt16();
        //        //    while (next1C5 >= 0x4000) { com.parameters.Add(next1C5); next1C5 = reader.ReadUInt16(); }
        //        //    reader.BaseStream.Position -= 2;
        //        //    break;
        //        //case 0x1C6:
        //        //    com.Name = "StorePokemonCaughtWF";
        //        //    com.parameters.Add(reader.ReadUInt16()); //True if is Pokèmon searched
        //        //    com.parameters.Add(reader.ReadUInt16()); //True if is caught the same day
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x1C7:
        //        //    com.Name = "1C7";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x1C9:
        //            com.Name = "StoreVarMessage";
        //            com.parameters.Add(reader.ReadUInt16()); //Variable as Container
        //            com.parameters.Add(reader.ReadUInt16()); //Message Id
        //            break;
        //        case 0x1CB:
        //            com.Name = "1CB";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x1CC:
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x1CD:
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x1CE:
        //            com.Name = "CheckPokèdexStatus";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1CF:
        //            com.Name = "StorePokèdexCaught";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x1D0:
        //        //    com.Name = "1D0";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x1D1:
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x1D2:
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x1D3:
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1D4:
        //            break;
        //        //case 0x1D5:
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x1D6:
        //            com.Name = "AffinityCheck";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1D7:
        //            com.Name = "SetVarAffinityCheck";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1D8:
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            //com.parameters.Add(reader.ReadUInt16());
        //            //com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1D9:
        //            com.Name = "1D9";
        //            com.parameters.Add(reader.ReadUInt16());
        //            if (scriptType == Constants.BW2SCRIPT)
        //                com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1DA:
        //            com.Name = "1DA";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1DB:
        //            com.Name = "1DB";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1DC:
        //            com.Name = "1DC";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1DD:
        //            com.Name = "1DD";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1DE:
        //            com.Name = "StoreDataUnity";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1DF:
        //            com.Name = "1DF";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1E0:
        //            com.Name = "ChooseUnityFloor";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1E1:
        //            com.Name = "StoreTrainerUnity";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x1E2:
        //        //    com.Name = "1E3";
        //        //    break;
        //        case 0x1E3:
        //            com.Name = "StoreUnityActivities";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1E4:
        //            com.Name = "StoreCanTeachDragonMove";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1E5:
        //            com.Name = "StorePokèmonStatusDragonMove";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1E6:
        //            com.Name = "1E6";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1E7:
        //            com.Name = "StoreChosenPokèmonDragonMove";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1E8:
        //            com.Name = "CheckRememberMove";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1E9:
        //            com.Name = "StoreRememberMove";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1EA:
        //            com.Name = "1EA";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1EB:
        //            com.Name = "1EB";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1EC:
        //            com.Name = "SwitchOwPosition";
        //            com.parameters.Add(reader.ReadUInt16()); //NPC Id
        //            com.parameters.Add(reader.ReadUInt16()); //NPC Id
        //            com.parameters.Add(reader.ReadUInt16()); //X Coordinate
        //            com.parameters.Add(reader.ReadUInt16()); //Y Coordinate
        //            com.parameters.Add(reader.ReadUInt16()); //Z Coordinate
        //            break;
        //        case 0x1ED:
        //            com.Name = "DoublePhraseBoxInput";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1EE:
        //            com.Name = "1EE";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1EF:
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1F0:
        //            com.Name = "HMEffect";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1F1:
        //            com.Name = "1F1";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1F2:
        //            com.Name = "1F2";
        //            break;
        //        //case 0x1F4:
        //        //    com.Name = "1F4";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x1F3:
        //            com.Name = "1F3";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1F6:
        //            com.Name = "1F6";
        //            com.parameters.Add(reader.ReadUInt16()); // 0
        //            com.parameters.Add(reader.ReadUInt16()); // 0
        //            com.parameters.Add(reader.ReadUInt16()); // 0
        //            break;
        //        case 0x1F7:
        //            com.Name = "1F7";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x1F8:
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x1F9:
        //            com.Name = "StartBattleExam";
        //            break;
        //        case 0x1FA:
        //            com.Name = "1FA";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1FB:
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1FC:
        //            com.Name = "StoreBattleExamModality";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x1FD:
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            if (scriptType == Constants.BW2SCRIPT)
        //            {
        //                com.parameters.Add(reader.ReadUInt16());
        //                com.parameters.Add(reader.ReadUInt16());
        //            }
        //            break;
        //        case 0x1FE:
        //            if (scriptType == Constants.BWSCRIPT)
        //            {
        //                com.parameters.Add(reader.ReadUInt16());
        //                com.parameters.Add(reader.ReadUInt16());
        //                com.parameters.Add(reader.ReadUInt16());
        //                com.parameters.Add(reader.ReadUInt16());
        //                com.parameters.Add(reader.ReadUInt16());
        //                com.parameters.Add(reader.ReadUInt16());
        //                com.parameters.Add(reader.ReadUInt16());
        //            }
        //            break;
        //        case 0x200:
        //            com.Name = "CheckBattleExamStarted";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x202:
        //            if (scriptType == Constants.BW2SCRIPT)
        //            {
        //                com.Name = "202";
        //                com.parameters.Add(reader.ReadUInt16());
        //                com.parameters.Add(reader.ReadUInt16());
        //            }
        //            else
        //            {
        //                com.Name = "StoreBattleExamStarNumber";
        //                com.parameters.Add(reader.ReadUInt16());
        //            }
        //            break;
        //        case 0x203:
        //            com.Name = "CheckBattleExamAvailable";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x204:
        //            com.Name = "StoreBattleExamLevel";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x205:
        //            com.Name = "StoreBattleExamType";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x206:
        //            com.Name = "SavingBookAnimation";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x207:
        //            com.Name = "ShowBattleExamResult";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x208:
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x209:
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x20A:
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x20B:
        //            if (scriptType == Constants.BW2SCRIPT)
        //            {
        //                com.Name = "20B";
        //                com.parameters.Add(reader.ReadUInt16());
        //            }
        //            else
        //            {
        //                com.parameters.Add(reader.ReadUInt16());
        //                com.parameters.Add(reader.ReadUInt16());
        //            }
        //            break;
        //        case 0x20C:
        //            if (scriptType == Constants.BW2SCRIPT)
        //            {
        //                com.Name = "20C";
        //            }
        //            else
        //            {
        //                com.Name = "CheckRelocatorPassword";
        //                com.parameters.Add(reader.ReadUInt16());
        //                com.parameters.Add(reader.ReadUInt16());
        //                com.parameters.Add(reader.ReadUInt16());
        //            }//True if inserted password is exact.
        //            break;
        //        //case 0x20D:
        //        //    com.Name = "20D";
        //        //    break;
        //        case 0x20E:
        //            com.Name = "CheckItemInterestingBag";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x20F:
        //            com.Name = "CompareInterestingItem";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x212:
        //            com.Name = "212";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x214:
        //            com.Name = "214";
        //            com.parameters.Add(reader.ReadUInt16()); // species
        //            com.parameters.Add(reader.ReadUInt16()); //0
        //            com.parameters.Add(reader.ReadUInt16()); //0
        //            com.parameters.Add(reader.ReadUInt16()); //0
        //            break;
        //        case 0x215:
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x217:
        //            com.Name = "217";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x218:
        //            com.Name = "StoreGreeting";
        //            com.parameters.Add(reader.ReadUInt16());//val
        //            break;
        //        case 0x219:
        //            com.Name = "StoreThanks";
        //            com.parameters.Add(reader.ReadUInt16());//var
        //            break;
        //        //case 0x21A:
        //        //    com.Name = "21A";
        //        //    com.parameters.Add(reader.ReadUInt16());//var
        //        //    com.parameters.Add(reader.ReadUInt16());//val
        //        //    break;
        //        case 0x21C:
        //            com.Name = "21C";
        //            com.parameters.Add(reader.ReadUInt16());//var
        //            com.parameters.Add(reader.ReadUInt16());//val
        //            break;
        //        //case 0x21D:
        //        //    com.Name = "21D";
        //        //    com.parameters.Add(reader.ReadUInt16());//var
        //        //    break;
        //        case 0x21E:
        //            com.Name = "21E";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x21F:
        //            com.Name = "21F";
        //            break;
        //        case 0x220:
        //            com.Name = "CheckHavePokèmon";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x221:
        //            com.Name = "221";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());//var
        //            break;
        //        case 0x222:
        //            com.Name = "222";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());//var
        //            break;
        //        case 0x223:
        //            com.Name = "StoreHiddenPowerType";			// ex 382
        //            com.parameters.Add(reader.ReadUInt16()); //Storage for result (0-17 move type)
        //            com.parameters.Add(reader.ReadUInt16()); //Party member to Store
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x224:
        //        //    com.Name = "224";
        //        //    com.parameters.Add(reader.ReadUInt16());//var
        //        //    com.parameters.Add(reader.ReadUInt16());//val
        //        //    com.parameters.Add(reader.ReadUInt16());//var
        //        //    break;
        //        case 0x225:
        //            com.Name = "225";
        //            com.parameters.Add(reader.ReadUInt16()); //Storage for result (0-17 move type)
        //            com.parameters.Add(reader.ReadUInt16()); //Party member to Store
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x226:
        //            com.Name = "RotatingAnimation";
        //            com.parameters.Add(reader.ReadUInt16()); //OW Id
        //            break;
        //        case 0x227:
        //            com.Name = "ShowPokèmonPicture";
        //            com.parameters.Add(reader.ReadUInt16()); //Pokèmon Id
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x228:
        //            com.Name = "228";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x229:
        //            com.Name = "229";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x22A:
        //            com.Name = "TeleportDreamForest";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x22B:
        //            com.Name = "CheckDreamFunction";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x22C:
        //            com.Name = "CheckSpacePokèmonDream";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x22D:
        //            com.Name = "SetVarPokèmonDream";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x22E:
        //            com.Name = "StartDreamIsle";
        //            break;
        //        case 0x22F:
        //            com.Name = "DreamBattle";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x230:
        //            com.Name = "GiveDreamPokèmon";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x231:
        //        //    com.Name = "231";
        //        //    com.parameters.Add(reader.ReadUInt16()); // Var
        //        //    var next231 = reader.ReadUInt16();
        //        //    while (next231 >= 0x4000) { com.parameters.Add(next231); next231 = reader.ReadUInt16(); }
        //        //    reader.BaseStream.Position -= 2;
        //        //    break;
        //        case 0x232:
        //            com.Name = "232";
        //            com.parameters.Add(reader.ReadUInt16()); // Var
        //            com.parameters.Add(reader.ReadUInt16()); // Var
        //            break;
        //        case 0x233:
        //            if (scriptType == Constants.BWSCRIPT)
        //            {
        //                com.Name = "233";
        //                com.parameters.Add(reader.ReadUInt16()); // Var
        //                com.parameters.Add(reader.ReadUInt16()); // Var
        //            }
        //            else
        //            {
        //                com.Name = "233";
        //                com.parameters.Add(reader.ReadUInt16()); // Var
        //            }
        //            break;
        //        case 0x234:
        //            com.Name = "233";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x236:
        //            com.Name = "236";
        //            com.parameters.Add(reader.ReadUInt16()); //var
        //            com.parameters.Add(reader.ReadUInt16()); //val
        //            com.parameters.Add(reader.ReadUInt16()); //val
        //            com.parameters.Add(reader.ReadUInt16()); //val
        //            break;
        //        case 0x237:
        //            com.Name = "237";
        //            com.parameters.Add(reader.ReadUInt16()); //var
        //            com.parameters.Add(reader.ReadUInt16()); //val
        //            com.parameters.Add(reader.ReadUInt16()); //val
        //            break;
        //        //case 0x239:
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x23A:
        //            com.Name = "23A";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x23B:
        //            com.Name = "CheckSendSaveCG";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16()); //RV = Message Id
        //            break;
        //        case 0x23D:
        //            com.Name = "23D";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16()); //RV = Message Id
        //            break;
        //        case 0x23E:
        //            com.Name = "23E";
        //            com.parameters.Add(reader.ReadUInt16()); //var
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16()); //var
        //            break;
        //        //case 0x23F:
        //        //    com.Name = "Close23F";
        //        //    break;
        //        case 0x240:
        //            com.Name = "240";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x242:
        //            com.Name = "242";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x243:
        //            com.Name = "BorderedMessage2";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x244:
        //            com.Name = "OpenHelpSystem";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x245:
        //        //    com.Name = "245";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x246:
        //            com.Name = "246";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x247:
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x248:
        //            com.Name = "248";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x249:
        //            com.Name = "StoreInterestingItemData";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            var next249 = reader.ReadUInt16();
        //            while (next249 >= 0x4000) { com.parameters.Add(next249); next249 = reader.ReadUInt16(); }
        //            reader.BaseStream.Position -= 2;
        //            break;
        //        case 0x24A:
        //            if (scriptType == Constants.BWSCRIPT)
        //            {
        //                com.Name = "TakeInterestingItem";
        //                com.parameters.Add(reader.ReadUInt16());
        //                com.parameters.Add(reader.ReadUInt16());
        //                break;
        //            }
        //            else
        //            {
        //                com.Name = "CheckCGearActive";
        //                com.parameters.Add(reader.ReadUInt16());
        //                break;
        //            }
        //        case 0x24B:
        //            com.Name = "24B";
        //            com.parameters.Add(reader.ReadUInt16());
        //            //com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x24C:
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x24D:
        //            com.Name = "24D";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x24E:
        //            com.Name = "24E";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x24F:
        //            com.Name = "24F";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x251:
        //            com.Name = "251";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x252:
        //        //    com.Name = "252";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    var next252 = reader.ReadUInt16();
        //        //    while (next252 >= 0x4000) { com.parameters.Add(next252); next252 = reader.ReadUInt16(); }
        //        //    reader.BaseStream.Position -= 2;
        //        //    break;
        //        case 0x253:
        //            com.Name = "253";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x254:
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x25A:
        //            com.Name = "25A";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x25C:
        //            com.Name = "25C";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x25D:
        //            com.Name = "CheckCGearActive";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x25F:
        //        //    com.Name = "25F";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x262:
        //            com.Name = "262(BW2)";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x263:
        //            com.Name = "263(BW2)";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x266:
        //        //    com.Name = "266";
        //        //    com.parameters.Add(reader.ReadUInt16()); // var
        //        //    break;
        //        //case 0x26C:
        //        //    com.Name = "StoreMedals26C";
        //        //    com.parameters.Add(reader.ReadByte());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x26D:
        //        //    com.Name = "StoreMedals26D";
        //        //    com.parameters.Add(reader.ReadByte());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x26E:
        //            com.Name = "StoreMedalsNumber";
        //            com.parameters.Add(reader.ReadByte()); // 3 For medals, command type?
        //            com.parameters.Add(reader.ReadUInt16()); // Variable to Store To
        //            break;
        //        //case 0x271:
        //        //    com.Name = "271";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x272:
        //        //    com.Name = "272";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x273:
        //            com.Name = "ActHome(BW2)";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x275:
        //            com.Name = "SetVarMission(BW2)";
        //            com.parameters.Add(reader.ReadByte());
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x276:
        //            com.Name = "276(BW2)";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x279:
        //        //    com.Name = "279";
        //        //    break;
        //        //case 0x283:
        //        //    com.Name = "283";
        //        //    com.parameters.Add(reader.ReadByte());
        //        //    com.parameters.Add(reader.ReadByte());
        //        //    break;
        //        //case 0x284:
        //        //    com.Name = "284";
        //        //    com.parameters.Add(reader.ReadByte());
        //        //    com.parameters.Add(reader.ReadByte());
        //        //    break;

        //        //case 0x285:
        //        //    com.Name = "285";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x287:
        //        //    com.Name = "287";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x288:
        //        //    com.Name = "288";
        //        //    com.parameters.Add(reader.ReadUInt16()); // value
        //        //    com.parameters.Add(reader.ReadUInt16()); // Variable
        //        //    com.parameters.Add(reader.ReadUInt16()); // Variable
        //        //    break;
        //        //case 0x289:
        //        //    com.Name = "289";
        //        //    com.parameters.Add(reader.ReadUInt16());    //might just be 3 tot
        //        //    var next289 = reader.ReadUInt16();
        //        //    while (next289 >= 0x4000) { com.parameters.Add(next289); next289 = reader.ReadUInt16(); }
        //        //    reader.BaseStream.Position -= 2;
        //        //    break;
        //        //case 0x28B:
        //        //    com.Name = "28B";
        //        //    break;
        //        case 0x290:
        //            com.Name = "290(BW2)";
        //            com.parameters.Add(reader.ReadByte());
        //            break;
        //        //case 0x292:
        //        //    com.Name = "292";
        //        //    com.parameters.Add(reader.ReadByte());
        //        //    break;
        //        //case 0x293:
        //        //    com.Name = "293";
        //        //    com.parameters.Add(reader.ReadByte());
        //        //    break;
        //        //case 0x294:
        //        //    com.Name = "294";
        //        //    com.parameters.Add(reader.ReadByte());
        //        //    com.parameters.Add(reader.ReadByte());
        //        //    break;
        //        //case 0x295:
        //        //    com.Name = "290";
        //        //    break;
        //        //case 0x297:
        //        //    com.Name = "297";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    var next297 = reader.ReadUInt16();
        //        //    while (next297 >= 0x4000) { com.parameters.Add(next297); next297 = reader.ReadUInt16(); }
        //        //    reader.BaseStream.Position -= 2;
        //        //    break;
        //        //case 0x29A:
        //        //    com.Name = "29A";
        //        //    com.parameters.Add(reader.ReadByte()); // opt
        //        //    com.parameters.Add(reader.ReadUInt16()); //var
        //        //    break;
        //        //case 0x29B:
        //        //    com.Name = "29B";
        //        //    com.parameters.Add(reader.ReadByte());
        //        //    break;
        //        //case 0x29D:
        //        //    com.Name = "29D";
        //        //    break;
        //        //case 0x29E:
        //        //    com.Name = "29E";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x29F:
        //        //    com.Name = "29F";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x2A0:
        //        //    com.Name = "StoreHasMedal";
        //        //    com.parameters.Add(reader.ReadUInt16()); //
        //        //    com.parameters.Add(reader.ReadUInt16()); //
        //        //    break;
        //        case 0x2A1:
        //            com.Name = "StoreMedalsType(BW2)";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x2A5:
        //        //    com.Name = "2A5";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x2A6:
        //        //    com.Name = "2A6";
        //        //    break;
        //        //case 0x2A7:
        //        //    com.Name = "2A7";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x2A8:
        //        //    com.Name = "2A8";
        //        //    break;
        //        //case 0x2A9:
        //        //    com.Name = "2A9";
        //        //    break;
        //        //case 0x2AF:
        //        //    com.Name = "StoreDifficulty";
        //        //    com.parameters.Add(reader.ReadUInt16()); // Store result to var/cont of Easy 0 | Normal 1 | Hard 2
        //        //    break;
        //        //case 0x2B1:
        //        //    com.Name = "2B1";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        case 0x2B2:
        //            com.Name = "2B2(BW2)";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x2B3:
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16()); // var
        //        //    break;
        //        //case 0x2B4:
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16()); // var
        //        //    break;
        //        //case 0x2B5:
        //        //    com.Name = "2B5";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x2B6:
        //        //    com.Name = "2B6";
        //        //    com.parameters.Add(reader.ReadUInt16()); //
        //        //    com.parameters.Add(reader.ReadUInt16()); //var
        //        //    break;
        //        //case 0x2B7:
        //        //    com.Name = "2B7";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x2B8:
        //        //    com.Name = "FollowMeStart (0x2B8)"; // seen at the start of a follow me, maybe tracksteps
        //        //    break;
        //        //case 0x2B9:
        //        //    com.Name = "FollowMeEnd (0x2B9)"; // maybe endtracksteps
        //        //    break;
        //        //case 0x2BA:
        //        //    com.Name = "FollowMeCopyStepsTo (0x2BA)"; // copy steps taken with follow me to CONT
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x2BC:
        //        //    com.Name = "2BC";
        //        //    com.parameters.Add(reader.ReadUInt16());//var
        //        //    com.parameters.Add(reader.ReadUInt16());//var
        //        //    break;
        //        //case 0x2BD:
        //        //    com.Name = "2BD";
        //        //    com.parameters.Add(reader.ReadUInt16());//var
        //        //    break;
        //        case 0x2BE:
        //            com.Name = "2BE(BW2)";
        //            com.parameters.Add(reader.ReadUInt16()); //val
        //            com.parameters.Add(reader.ReadUInt16()); //var
        //            com.parameters.Add(reader.ReadUInt16()); //var
        //            com.parameters.Add(reader.ReadUInt16()); //var
        //            break;
        //        //case 0x2C0:
        //        //    com.Name = "2C0";
        //        //    com.parameters.Add(reader.ReadUInt16());//var
        //        //    com.parameters.Add(reader.ReadUInt16());//var
        //        //    break;
        //        //case 0x2C3:
        //        //    com.Name = "2C3";
        //        //    com.parameters.Add(reader.ReadUInt16());//var
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x2C4:
        //        //    com.Name = "2C4";
        //        //    break;
        //        //case 0x2C5:
        //        //    com.Name = "2C5";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x2CB:
        //        //    com.Name = "2CB";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x2CD:
        //        //    com.Name = "2CD";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x2CF:
        //        //    com.Name = "2CF";
        //        //    com.parameters.Add(reader.ReadUInt16()); //val
        //        //    com.parameters.Add(reader.ReadUInt16()); //val
        //        //    com.parameters.Add(reader.ReadUInt16()); //val
        //        //    com.parameters.Add(reader.ReadUInt16()); //var
        //        //    break;
        //        //case 0x2D0:
        //        //    com.Name = "***HABITATLISTENABLE***";
        //        //    break;
        //        case 0x2D1:
        //            com.Name = "StoreBCSkyscraperTrainerNumber(BW2)";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x2D3:
        //            com.Name = "2D3(BW2)";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x2D4:
        //        //    com.Name = "2D4";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x2D5:
        //        //    com.Name = "2D5";
        //        //    com.parameters.Add(reader.ReadUInt16()); // value
        //        //    com.parameters.Add(reader.ReadUInt16()); // variable
        //        //    break;
        //        case 0x2D7:
        //            com.Name = "2D7(BW2)";
        //            com.parameters.Add(reader.ReadUInt16()); // value
        //            com.parameters.Add(reader.ReadUInt16()); // variable
        //            break;
        //        //case 0x2D9:
        //        //    com.Name = "2D9";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x2DA:
        //        //    com.Name = "2DA";
        //        //    com.parameters.Add(reader.ReadUInt16()); // low value
        //        //    break;
        //        //case 0x2DB:
        //        //    com.Name = "2DB";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x2DC:
        //        //    com.Name = "2DC";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x2DD:
        //        //    com.Name = "StoreUnityVisitors";
        //        //    com.parameters.Add(reader.ReadUInt16()); // Variable to return to?
        //        //    break;
        //        //case 0x2DF:
        //        //    com.Name = "StoreMyActivities"; // activity
        //        //    com.parameters.Add(reader.ReadUInt16()); // Variable to return to?
        //        //    break;
        //        //case 0x2E1:
        //        //    com.Name = "2E1";
        //        //    break;
        //        //case 0x2E8:
        //        //    com.Name = "2E8";
        //        //    com.parameters.Add(reader.ReadUInt16()); // 
        //        //    com.parameters.Add(reader.ReadUInt16()); // 
        //        //    break;
        //        //case 0x2ED:
        //        //    com.Name = "2ED";
        //        //    com.parameters.Add(reader.ReadUInt16()); // 
        //        //    com.parameters.Add(reader.ReadUInt16()); // 
        //        //    break;
        //        //case 0x2EE:
        //        //    com.Name = "Prop2EE";
        //        //    com.parameters.Add(reader.ReadUInt16()); // value ~ prop# to give?
        //        //    com.parameters.Add(reader.ReadUInt16()); // variable/container
        //        //    break;
        //        //case 0x2EF:
        //        //    com.Name = "2EF";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x2F1:
        //        //    com.Name = "2F1";
        //        //    com.parameters.Add(reader.ReadUInt16()); //
        //        //    break;
        //        //case 0x2F2:
        //        //    com.Name = "2F2";
        //        //    break;

        //        ////commands set 2 (1000)

        //        case 0x3E8:
        //            com.Name = "3E8(BW2)";
        //            var next3E9 = reader.ReadUInt16();
        //            while (next3E9 < 0xA || next3E9 > 0x400)
        //            {
        //                if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
        //                {
        //                    reader.BaseStream.Position -= 2;
        //                    goto End;
        //                }
        //                com.parameters.Add(next3E9);
        //                next3E9 = reader.ReadUInt16();

        //            }
        //        End: reader.BaseStream.Position -= 2;
        //            break;
        //        case 0x3E9:
        //            com.Name = "3E9(BW2)";
        //            next3E9 = reader.ReadUInt16();
        //            while (next3E9 < 0xA || next3E9 > 0x400)
        //            {
        //                if ((next3E9 == 0x9 && reader.ReadUInt16() > 0x400) || next3E9 == 3)
        //                {
        //                    reader.BaseStream.Position -= 2;
        //                    goto End;
        //                }
        //                com.parameters.Add(next3E9);
        //                next3E9 = reader.ReadUInt16();

        //            }
        //            reader.BaseStream.Position -= 2;
        //            break;
        //        case 0x3EA:
        //            com.Name = "3EA(BW2)";
        //            next3E9 = reader.ReadUInt16();
        //            while (next3E9 < 0xA || next3E9 > 0x400)
        //            {
        //                if ((next3E9 == 0x9 && reader.ReadUInt16() > 0x400) || next3E9 == 3)
        //                {
        //                    reader.BaseStream.Position -= 2;
        //                    goto End;
        //                }
        //                com.parameters.Add(next3E9);
        //                next3E9 = reader.ReadUInt16();

        //            }
        //            reader.BaseStream.Position -= 2;
        //            break;
        //        case 0x3EB:
        //            com.Name = "3EB(BW2)";
        //            next3E9 = reader.ReadUInt16();
        //            while (next3E9 < 0xA || next3E9 > 0x400)
        //            {
        //                if ((next3E9 == 0x9 && reader.ReadUInt16() > 0x400) || next3E9 == 3)
        //                {
        //                    reader.BaseStream.Position -= 2;
        //                    goto End;
        //                }
        //                com.parameters.Add(next3E9);
        //                next3E9 = reader.ReadUInt16();

        //            }
        //            reader.BaseStream.Position -= 2;
        //            break;
        //        case 0x3EC:
        //            com.Name = "3EC(BW2)";
        //            next3E9 = reader.ReadUInt16();
        //            while (next3E9 < 0xA || next3E9 > 0x400)
        //            {
        //                if ((next3E9 == 0x9 && reader.ReadUInt16() > 0x400) || next3E9 == 3)
        //                {
        //                    reader.BaseStream.Position -= 2;
        //                    goto End;
        //                }
        //                com.parameters.Add(next3E9);
        //                next3E9 = reader.ReadUInt16();

        //            }
        //            reader.BaseStream.Position -= 2;
        //            break;
        //        case 0x3ED:
        //            com.Name = "3ED(BW2)";
        //            next3E9 = reader.ReadUInt16();
        //            com.parameters.Add(next3E9);
        //            //while (next3E9 < 0xA || next3E9 > 0x400)
        //            //{
        //            //    if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
        //            //    {
        //            //        reader.BaseStream.Position -= 2;
        //            //        goto End;
        //            //    }
        //            //    com.parameters.Add(next3E9);
        //            //    next3E9 = reader.ReadUInt16();

        //            //}
        //            //reader.BaseStream.Position -= 2;
        //            break;
        //        case 0x3EE:
        //            com.Name = "3EE(BW2)";
        //            var next3EE = reader.ReadUInt16();
        //            while (next3EE <= 40) { com.parameters.Add(next3EE); next3EE = reader.ReadUInt16(); }
        //            while (next3EE >= 0x4000) { com.parameters.Add(next3EE); next3EE = reader.ReadUInt16(); }
        //            reader.BaseStream.Position -= 2;
        //            break;
        //        case 0x3EF:
        //            com.Name = "3EF(BW2)";
        //            next3E9 = reader.ReadUInt16();
        //            while (next3E9 < 0x40 || next3E9 > 0x400)
        //            {
        //                if ((next3E9 == 0x9 && reader.ReadUInt16() > 0x400) || next3E9 == 3)
        //                {
        //                    reader.BaseStream.Position -= 2;
        //                    goto End;
        //                }
        //                com.parameters.Add(next3E9);
        //                next3E9 = reader.ReadUInt16();

        //            }
        //            reader.BaseStream.Position -= 2;
        //            break;
        //        case 0x3F0:
        //            com.Name = "3F0";
        //            next3E9 = reader.ReadUInt16();
        //            while (next3E9 < 0x40 || next3E9 > 0x400)
        //            {
        //                if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
        //                {
        //                    reader.BaseStream.Position -= 2;
        //                    goto End;
        //                }
        //                com.parameters.Add(next3E9);
        //                next3E9 = reader.ReadUInt16();

        //            }
        //            reader.BaseStream.Position -= 2;
        //            break;
        //        case 0x3F1:
        //            com.Name = "3F1";
        //            next3E9 = reader.ReadUInt16();
        //            while (next3E9 < 0x40 || next3E9 > 0x400)
        //            {
        //                if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
        //                {
        //                    reader.BaseStream.Position -= 2;
        //                    goto End;
        //                }
        //                com.parameters.Add(next3E9);
        //                next3E9 = reader.ReadUInt16();

        //            }
        //            reader.BaseStream.Position -= 2;
        //            break;
        //        case 0x3F2:
        //            com.Name = "3F2";
        //            next3E9 = reader.ReadUInt16();
        //            while (next3E9 < 0xA || next3E9 > 0x400)
        //            {
        //                if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
        //                {
        //                    reader.BaseStream.Position -= 2;
        //                    goto End;
        //                }
        //                com.parameters.Add(next3E9);
        //                next3E9 = reader.ReadUInt16();

        //            }
        //            reader.BaseStream.Position -= 2;
        //            break;
        //        case 0x3F3:
        //            com.Name = "3F3";
        //            next3E9 = reader.ReadUInt16();
        //            while (next3E9 < 0xA || next3E9 > 0x400)
        //            {
        //                if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
        //                {
        //                    reader.BaseStream.Position -= 2;
        //                    goto End;
        //                }
        //                com.parameters.Add(next3E9);
        //                next3E9 = reader.ReadUInt16();

        //            }
        //            reader.BaseStream.Position -= 2;
        //            break;
        //        case 0x3F4:
        //            com.Name = "3F4";
        //            next3E9 = reader.ReadUInt16();
        //            while (next3E9 < 0xA || next3E9 > 0x400)
        //            {
        //                if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
        //                {
        //                    reader.BaseStream.Position -= 2;
        //                    goto End;
        //                }
        //                com.parameters.Add(next3E9);
        //                next3E9 = reader.ReadUInt16();

        //            }
        //            reader.BaseStream.Position -= 2;
        //            break;
        //        case 0x3F5:
        //            com.Name = "3F5";
        //            next3E9 = reader.ReadUInt16();
        //            while (next3E9 < 0xA || next3E9 > 0x400)
        //            {
        //                if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
        //                {
        //                    reader.BaseStream.Position -= 2;
        //                    goto End;
        //                }
        //                com.parameters.Add(next3E9);
        //                next3E9 = reader.ReadUInt16();

        //            }
        //            reader.BaseStream.Position -= 2;
        //            break;
        //        case 0x3F6:
        //            com.Name = "3F6";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x3F8:
        //            com.Name = "3F8";
        //            next3E9 = reader.ReadUInt16();
        //            while (next3E9 < 0xA || next3E9 > 0x400)
        //            {
        //                if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
        //                {
        //                    reader.BaseStream.Position -= 2;
        //                    goto End;
        //                }
        //                com.parameters.Add(next3E9);
        //                next3E9 = reader.ReadUInt16();
        //            }
        //            reader.BaseStream.Position -= 2;
        //            break;
        //        case 0x3F9:
        //            com.Name = "3F9";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());

        //            break;
        //        case 0x3FA:
        //            com.Name = "3FA";
        //            next3E9 = reader.ReadUInt16();
        //            while (next3E9 < 0xA || next3E9 > 0x400)
        //            {
        //                if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
        //                {
        //                    reader.BaseStream.Position -= 2;
        //                    goto End;
        //                }
        //                com.parameters.Add(next3E9);
        //                next3E9 = reader.ReadUInt16();

        //            }
        //            reader.BaseStream.Position -= 2;
        //            break;
        //        case 0x3FB:         //iris?
        //            com.parameters.Add(reader.ReadUInt16()); //not sure if below is needed
        //            next3E9 = reader.ReadUInt16();
        //            while (next3E9 < 0xA || next3E9 > 0x400)
        //            {
        //                if (next3E9 == 0x9 && reader.ReadUInt16() > 0x400)
        //                {
        //                    reader.BaseStream.Position -= 2;
        //                    goto End;
        //                }
        //                com.parameters.Add(next3E9);
        //                next3E9 = reader.ReadUInt16();
        //            }
        //            reader.BaseStream.Position -= 2;
        //            break;
        //        case 0x3FC:
        //            com.Name = "3FC";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x3FD:
        //            com.Name = "3FD";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x3FE:
        //            com.Name = "3FE";
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        case 0x3FF:
        //            com.Name = "3FF";
        //            com.parameters.Add(reader.ReadUInt16());
        //            var next3FF = reader.ReadUInt16();
        //            while (next3FF >= 0x4000) { com.parameters.Add(next3FF); next3FF = reader.ReadUInt16(); }
        //            reader.BaseStream.Position -= 2;
        //            break;
        //        case 0x401:
        //            com.Name = "401";
        //            com.parameters.Add(reader.ReadUInt16());
        //            com.parameters.Add(reader.ReadUInt16());
        //            break;
        //        //case 0x402:
        //        //    com.Name = "402";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x403:
        //        //    com.Name = "403";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x404:
        //        //    com.Name = "404";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x406:
        //        //    com.Name = "406";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x407:
        //        //    com.Name = "407";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x40D:
        //        //    com.Name = "40D";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    break;
        //        //case 0x40E:
        //        //    com.Name = "40E";
        //        //    com.parameters.Add(reader.ReadUInt16()); //var
        //        //    break;
        //        //case 0x410:
        //        //    com.Name = "410";
        //        //    com.parameters.Add(reader.ReadUInt16()); //var
        //        //    break;
        //        //case 0x411:
        //        //    com.Name = "411";
        //        //    com.parameters.Add(reader.ReadUInt16()); //var
        //        //    break;
        //        //case 0x412:
        //        //    com.Name = "412";
        //        //    com.parameters.Add(reader.ReadUInt16()); //var
        //        //    break;
        //        //case 0x414:
        //        //    com.Name = "414";
        //        //    com.parameters.Add(reader.ReadByte());
        //        //    com.parameters.Add(reader.ReadUInt16()); //var
        //        //    break;
        //        //case 0x415:
        //        //    com.Name = "415";
        //        //    com.parameters.Add(reader.ReadByte());
        //        //    break;
        //        //case 0x416:
        //        //    com.Name = "416";
        //        //    com.parameters.Add(reader.ReadUInt16()); //var
        //        //    break;
        //        //case 0x417:
        //        //    com.Name = "417";
        //        //    com.parameters.Add(reader.ReadUInt16()); //val
        //        //    break;
        //        //case 0x418:
        //        //    com.Name = "418";
        //        //    com.parameters.Add(reader.ReadUInt16()); //var
        //        //    break;
        //        //case 0x419:
        //        //    com.Name = "419";
        //        //    com.parameters.Add(reader.ReadUInt16()); //var
        //        //    com.parameters.Add(reader.ReadUInt16()); //var
        //        //    break;
        //        //case 0x41A:
        //        //    com.Name = "41A";
        //        //    com.parameters.Add(reader.ReadUInt16()); //var
        //        //    break;
        //        //case 0x41B:
        //        //    com.Name = "40B";
        //        //    com.parameters.Add(reader.ReadUInt16());
        //        //    com.parameters.Add(reader.ReadUInt16()); //var
        //        //    break;
        //        //case 0x41C:
        //        //    com.Name = "41C";
        //        //    com.parameters.Add(reader.ReadUInt16()); //var
        //        //    break;
        //        //case 0x421:
        //        //    com.Name = "420";
        //        //    com.parameters.Add(reader.ReadUInt16()); //var
        //        //    com.parameters.Add(reader.ReadUInt16()); //var
        //        //    break;24
        //        //case 0x411F:
        //        //    com.Name = "41F";
        //        //    com.paerameters.Add(reader.ReadUInt16()); //var
        //        //    com.parameters.Add(reader.ReadUInt16()); //var
        //        //    break;
        //        //case 0x420:
        //        //    com.Name = "420";
        //        //    com.parameters.Add(reader.ReadUInt16()); //var
        //        //    break;
        //        //case 0x422:
        //        //    com.Name = "422";
        //        //    com.parameters.Add(reader.ReadUInt16()); //var
        //        //    break;
        //        default:
        //            com.Name = "0x" + com.Id.ToString("X");
        //            break;

        //    }
        //    return com;
        //}

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

        public void getCommandSimplifiedBW2(string[] scriptsLine, ref int lineCounter, string space, List<int> visitedLine)
        {
            var line = scriptsLine[lineCounter];
            var commandList = line.Split(' ');
            string movId;
            string tipe;
            string varString, varString2, varString3, condition;
            //scriptBoxEditor.AppendText(commandList[1] + " ");
            switch (commandList[2].Split('(')[0])
            {
                case "ApplyMovement":
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( NPC_ID " + commandList[3] + ");\n");
                    scriptBoxEditor.AppendText(space + "{\n");
                    movId = commandList[6];
                    tipe = commandList[5];
                    for (var functionLineCounter = 0; functionLineCounter < scriptsLine.Length; functionLineCounter++)
                    {
                        var line2 = scriptsLine[functionLineCounter];
                        if (commandList.Length < 8)
                        {
                            var offset2 = commandList[5].TrimStart('0').TrimStart('x');
                            int offset = int.Parse(offset2, System.Globalization.NumberStyles.HexNumber);
                            if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + offset))
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
                case "AngryMessage":
                    if (commandList.Length <= 5 && textFile == null)
                    {
                        scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + " , COLOR " + commandList[4] + " );\n");
                    }
                    else
                    {
                        short index = 0;
                        varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                        if (varString.Contains("M"))
                        {
                            index = Int16.Parse(varString.ToCharArray()[varString.Length - 1].ToString());
                            scriptBoxEditor.AppendText(space + varString + " = ' " + textFile.messageList[index] + " ';\n");
                        }
                        else if (varString.Length > 2 && Int16.Parse(varString.Substring(2, varString.Length - 2)) > 8000 && textFile != null)
                        {
                            var scriptAnLine = scriptBoxEditor.Lines;
                            int counterInverse = scriptAnLine.Length - 1;
                            while (!scriptAnLine[counterInverse].Contains("VAR") && !scriptAnLine[counterInverse].Contains(" = ") && !scriptAnLine[counterInverse].Contains(varString) || scriptAnLine[counterInverse].Contains("+") && counterInverse > 0)
                                counterInverse--;
                            if (counterInverse > 0)
                            {
                                var text = scriptAnLine[counterInverse].Split(' ');
                                varString = text[text.Length - 1];
                                scriptBoxEditor.AppendText(space + "ANGRY_MESSAGE " + commandList[3] + " = '" + textFile.messageList[Int16.Parse(varString)] + " ';\n");
                                varString = commandList[3].ToString();
                            }
                        }
                        else if (Int16.TryParse(varString, out index))
                            scriptBoxEditor.AppendText(space + "ANGRY_MESSAGE_" + varString + " = ' " + textFile.messageList[Int16.Parse(varString)] + " ';\n");
                        scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + varString + " , COLOR " + commandList[4] + ", " + commandList[5] + " );\n");
                    }
                    break;
                case "BorderedMessage":
                case "EventGreyMessage":
                    if (textFile == null)
                    {
                        scriptBoxEditor.AppendText(space + "" + commandList[2] + "(  " +
                                               " , MESSAGE_ID " + commandList[3] + " , COLOR " + commandList[4] + " );\n");
                    }
                    else
                    {
                        short index = 0;
                        varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                        if (varString.Contains("M"))
                        {
                            index = Int16.Parse(varString.ToCharArray()[varString.Length - 1].ToString());
                            scriptBoxEditor.AppendText(space + varString + " = ' " + textFile.messageList[index] + " ';\n");
                        }
                        else if (varString.Length > 2 && Int16.Parse(varString.Substring(2, varString.Length - 2)) > 8000 && textFile != null)
                        {
                            var scriptAnLine = scriptBoxEditor.Lines;
                            int counterInverse = scriptAnLine.Length - 1;
                            while (!scriptAnLine[counterInverse].Contains("VAR") && !scriptAnLine[counterInverse].Contains(" = ") && !scriptAnLine[counterInverse].Contains(varString)
                                 || (scriptAnLine[counterInverse].Contains("+") || scriptAnLine[counterInverse].Contains(")") || scriptAnLine[counterInverse].Contains("-") || scriptAnLine[counterInverse].Contains("[")) && counterInverse > 0)
                                counterInverse--;
                            if (counterInverse > 0)
                            {
                                var text = scriptAnLine[counterInverse].Split(' ');
                                varString = text[text.Length - 1];
                                scriptBoxEditor.AppendText(space + "MESSAGE " + commandList[3] + " = '" + textFile.messageList[Int16.Parse(varString)] + " ';\n");
                                varString = commandList[3].ToString();
                            }
                        }
                        else if (Int16.TryParse(varString, out index))
                            scriptBoxEditor.AppendText(space + "MESSAGE_" + varString + " = ' " + textFile.messageList[Int16.Parse(varString)] + " ';\n");
                        scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + varString + " , COLOR " + commandList[4] + " );\n");
                    }
                    break;
                case "BubbleMessage":
                    varString2 = "";
                    if (commandList.Length <= 5 && textFile == null)
                    {
                        scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + " , COLOR " + commandList[4] + " );\n");
                    }
                    else
                    {
                        short index = 0;
                        varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                        if (varString.Contains("M"))
                        {
                            index = Int16.Parse(varString.ToCharArray()[varString.Length - 1].ToString());
                            scriptBoxEditor.AppendText(space + varString + " = ' " + textFile.messageList[index] + " ';\n");
                        }
                        else if (varString.Length > 2 && Int16.Parse(varString.Substring(2, varString.Length - 2)) > 8000 && textFile != null)
                        {
                            var scriptAnLine = scriptBoxEditor.Lines;
                            int counterInverse = scriptAnLine.Length - 1;
                            while (!scriptAnLine[counterInverse].Contains("VAR") && !scriptAnLine[counterInverse].Contains(" = ") && !scriptAnLine[counterInverse].Contains(varString) || scriptAnLine[counterInverse].Contains("+") && counterInverse > 0)
                                counterInverse--;
                            if (counterInverse > 0)
                            {
                                var text = scriptAnLine[counterInverse].Split(' ');
                                varString = text[text.Length - 1];
                                scriptBoxEditor.AppendText(space + "BUBBLE_MESSAGE " + commandList[3] + " = '" + textFile.messageList[Int16.Parse(varString)] + " ';\n");
                                varString = commandList[3].ToString();
                            }
                        }
                        else if (Int16.TryParse(varString, out index))
                            scriptBoxEditor.AppendText(space + "BUBBLE_MESSAGE_" + varString + " = ' " + textFile.messageList[Int16.Parse(varString)] + " ';\n");
                        scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + varString + " , COLOR " + commandList[4] + " );\n");
                    }
                    break;
                case "CallMessageBox":
                    scriptBoxEditor.AppendText(space + "BORDER " + commandList[5] + " = " + commandList[2] + "( MESSAGE_ID " + commandList[3] +
                                               ", TYPE " + commandList[4] + " ");
                    if (commandList.Length > 5)
                        for (int i = 6; i < commandList.Length; i++)
                            scriptBoxEditor.AppendText(commandList[i] + " ");
                    scriptBoxEditor.AppendText(" );\n");
                    break;
                case "CallRoutine":
                    scriptBoxEditor.AppendText(space + "CallRoutine( " + commandList[3] + ") \n" + space + "{\n");
                    //functionId = commandList[5];
                    varNameDictionary.Add(new Dictionary<int, string>());
                    for (var functionLineCounter = 0; functionLineCounter < scriptsLine.Length - 1; functionLineCounter++)
                    {
                        var line2 = scriptsLine[functionLineCounter];
                        if (commandList.Length < 7)
                        {
                            int offset = 0;
                            if (commandList[3].Contains("0x"))
                            {
                                var offset2 = commandList[3].TrimStart('0').TrimStart('x');
                                offset = int.Parse(offset2, System.Globalization.NumberStyles.HexNumber);
                            }
                            else
                            {
                                offset = int.Parse(commandList[3]);
                            }
                            if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + offset))
                            {
                                varLevel++;
                                readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
                                varLevel--;
                                return;
                            }
                        }
                        else if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[6].TrimStart('(')))
                        {
                            varLevel++;
                            readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
                            varLevel--;
                            return;
                        }
                    }
                    break;
                case "ClearFlag":
                    var statusFlag = "FALSE";
                    if (commandList[4] == "1")
                        statusFlag = "TRUE";
                    scriptBoxEditor.AppendText(space + "FLAG " + getTextFromCondition(commandList[3], "FLA") +
                                               " = " + statusFlag + ";\n");
                    break;
                case "CheckEnoughMoney":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HAVE_MONEY", "BOL");
                    scriptBoxEditor.AppendText(space + "HAVE_MONEY " + commandList[3] + " = " + commandList[2] + "( AMOUNT " + commandList[4] + " );\n");
                    break;
                case "CheckItemBagSpace":
                    newVar = checkStored(commandList, 5);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEMBAG_SPACE", "BOL");
                    newVar2 = checkStored(commandList, 3);
                    varString = commandList[3];
                    if (Utils.IsNaturalNumber(varString))
                        varString = getTextFromCondition(commandList[3], "ITE");
                    else
                        addToVarNameDictionary(varNameDictionary, varLevel, newVar2, commandList[3], "ITE");
                    scriptBoxEditor.AppendText(space + "ITEMBAG_SPACE " + commandList[5] + " = " + commandList[2] + "( ITEM " + varString + " , " + "NUMBER " + commandList[4] + " );\n");
                    break;
                case "CheckItemBagNumber":
                    newVar = checkStored(commandList, 5);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEMBAG_NUMBER", "BOL");
                    varString = commandList[3];
                    if (Utils.IsNaturalNumber(varString))
                        varString = getTextFromCondition(commandList[3], "ITE");
                    else
                        addToVarNameDictionary(varNameDictionary, varLevel, newVar2, commandList[3], "ITE");
                    scriptBoxEditor.AppendText(space + "ITEMBAG_NUMBER " + commandList[5] + " = " + commandList[2] + "( ITEM " + varString + " , " + "NUMBER " + commandList[4] + " );\n");
                    break;
                case "CheckItemContainer":
                    newVar = checkStored(commandList, 4);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "IS_CONTAINER", "BOL");
                    varString = commandList[3];
                    if (Utils.IsNaturalNumber(varString))
                        varString = getTextFromCondition(commandList[3], "ITE");
                    else
                        addToVarNameDictionary(varNameDictionary, varLevel, newVar2, commandList[3], "ITE");
                    scriptBoxEditor.AppendText(space + "IS_CONTAINER " + commandList[4] + " = " + commandList[2] + "( ITEM " + varString + " );\n");
                    break;
                case "CompareStackCondition":
                    break;
                case "CompareValueWithDerefVar":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    varString2 = getTextFromCondition(getStoredMagic(varNameDictionary, varLevel, commandList, 4), cond);
                    condition = getCondition(scriptsLine, lineCounter);
                    scriptBoxEditor.AppendText(space + "If( " + varString + " " + condition + " " + varString2 + " )\n");
                    break;
                case "CompareDerefVars":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    varString2 = getTextFromCondition(getStoredMagic(varNameDictionary, varLevel, commandList, 4), cond);
                    condition = getCondition(scriptsLine, lineCounter);
                    scriptBoxEditor.AppendText(space + "If( " + varString + " " + condition + " " + varString2 + ")\n");
                    break;
                case "Fanfare":
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MUSIC_ID " + commandList[3] + ");\n");
                    break;
                case "Goto":
                    scriptBoxEditor.AppendText(space + "Goto\n" + space + "{\n");
                    var functionId = commandList[5];
                    varNameDictionary.Add(new Dictionary<int, string>());
                    for (var functionLineCounter = 0; functionLineCounter < scriptsLine.Length - 1; functionLineCounter++)
                    {
                        var line2 = scriptsLine[functionLineCounter];
                        if (commandList.Length < 6)
                        {
                            var offset2 = commandList[4].TrimStart('0').TrimStart('x');
                            int offset = int.Parse(offset2, System.Globalization.NumberStyles.HexNumber);
                            if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + offset))
                            {
                                varLevel++;
                                readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
                                varLevel--;
                                return;
                            }
                        }
                        else if (commandList.Length > 6 && scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[6].TrimStart('(')))
                        {
                            varLevel++;
                            readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
                            varLevel--;
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
                        if (commandList.Length < 8)
                        {
                            var offset2 = commandList[4].TrimStart('0').TrimStart('x');
                            int offset = int.Parse(offset2, System.Globalization.NumberStyles.HexNumber);
                            if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + offset))
                            {
                                varLevel++;
                                readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
                                varLevel--;
                                return;
                            }
                        }
                        else if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[6].TrimStart('(')))
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
                        if (commandList.Length < 8)
                        {
                            var offset2 = commandList[4].TrimStart('0').TrimStart('x');
                            int offset = int.Parse(offset2, System.Globalization.NumberStyles.HexNumber);
                            if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + offset))
                            {
                                varLevel++;
                                readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
                                varLevel--;
                                return;
                            }
                        }
                        else if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[6].TrimStart('(')))
                        {
                            varLevel++;
                            readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
                            varLevel--;
                            return;
                        }
                    }
                    break;
                case "Jump":
                    scriptBoxEditor.AppendText(space + "Jump\n" + space + "{\n");
                    functionId = commandList[5];
                    varNameDictionary.Add(new Dictionary<int, string>());
                    for (var functionLineCounter = 0; functionLineCounter < scriptsLine.Length - 1; functionLineCounter++)
                    {
                        var line2 = scriptsLine[functionLineCounter];
                        if (commandList.Length < 7)
                        {
                            var offset2 = commandList[4].TrimStart('0').TrimStart('x');
                            int offset = int.Parse(offset2, System.Globalization.NumberStyles.HexNumber);
                            if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + offset))
                            {
                                readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
                                return;
                            }
                        }
                        else if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[6].TrimStart('(')))
                        {
                            readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
                            return;
                        }
                    }
                    break;
                case "GenericMessage":
                    if (commandList.Length < 10)
                    {
                        scriptBoxEditor.AppendText(space + "" + commandList[2] + "( INTERNAL NARC ID  " + commandList[3] +
                                                   " , MESSAGE_ID " + commandList[4] +
                                                   " , VIEW " + commandList[5] + " , TYPE " + commandList[6] + " );\n");
                    }
                    else
                    {
                        short index = 0;
                        varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                        if (varString.Contains("M"))
                        {
                            index = Int16.Parse(varString.ToCharArray()[varString.Length - 1].ToString());
                            scriptBoxEditor.AppendText(space + varString + " = ' " + textFile.messageList[index] + " ';\n");
                        }
                        else if (varString.Length > 2 && Int16.Parse(varString.Substring(2, varString.Length - 2)) > 8000 && textFile != null)
                        {
                            var scriptAnLine = scriptBoxEditor.Lines;
                            int counterInverse = scriptAnLine.Length - 1;
                            while (!scriptAnLine[counterInverse].Contains("VAR") && !scriptAnLine[counterInverse].Contains(" = ") && !scriptAnLine[counterInverse].Contains(varString)
                                || scriptAnLine[counterInverse].Contains("+") || scriptAnLine[counterInverse].Contains(")") || scriptAnLine[counterInverse].Contains("-") & counterInverse > 0)
                                counterInverse--;
                            if (counterInverse > 0)
                            {
                                var text = scriptAnLine[counterInverse].Split(' ');
                                varString = text[text.Length - 1];
                                scriptBoxEditor.AppendText(space + "MESSAGE " + commandList[4] + " = '" + textFile.messageList[Int16.Parse(varString)] + " ';\n");
                                varString = commandList[4].ToString();
                            }
                        }
                        else if (Int16.TryParse(varString, out index))
                            scriptBoxEditor.AppendText(space + "MESSAGE_" + varString + " = ' " + textFile.messageList[Int16.Parse(varString)] + " ';\n");
                        scriptBoxEditor.AppendText(space + "" + commandList[2] + "( INTERNAL NARC ID " + commandList[3] +
                                                   " , MESSAGE_ID " + varString +
                                                   " , VIEW " + commandList[5] + " , TYPE " + commandList[6] + " );\n");
                    }
                    break;
                case "Multi":
                    addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 8), "MULTI_CHOSEN", "MUL");
                    mulString = new List<string>();
                    scriptBoxEditor.AppendText(space + "MULTI_CHOSEN " + commandList[8] + " = " + commandList[2] + "(X " + commandList[4] + " , " +
                        "Y " + commandList[5] + " , " +
                        "CURSOR " + commandList[6] + " , " +
                        "P_4 " + commandList[7] +
                        ");\n");
                    break;
                case "NPCMessage":
                    if (textFile == null)
                    {
                        scriptBoxEditor.AppendText(space + "" + commandList[2] + "( INTERNAL NARC ID " + commandList[3] +
                                               " , MESSAGE_ID " + commandList[4] + " , NPC_ID " + commandList[5] +
                                                   " , VIEW " + commandList[6] + " , TYPE " + commandList[7] + " );\n");
                    }
                    else
                    {
                        short index = 0;
                        varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                        varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
                        if (varString.Contains("M"))
                        {
                            index = Int16.Parse(varString.ToCharArray()[varString.Length - 1].ToString());
                            scriptBoxEditor.AppendText(space + varString + " = ' " + textFile.messageList[index] + " ';\n");
                        }
                        else if (varString.Length > 2 && Int16.Parse(varString.Substring(2, varString.Length - 2)) > 8000 && textFile != null)
                        {
                            var scriptAnLine = scriptBoxEditor.Lines;
                            int counterInverse = scriptAnLine.Length - 1;
                            while (!scriptAnLine[counterInverse].Contains("VAR") && !scriptAnLine[counterInverse].Contains(" = ") && !scriptAnLine[counterInverse].Contains(varString)
                                 || (scriptAnLine[counterInverse].Contains("+") || scriptAnLine[counterInverse].Contains(")") || scriptAnLine[counterInverse].Contains("-") || scriptAnLine[counterInverse].Contains("[")) && counterInverse > 0)
                                counterInverse--;
                            if (counterInverse > 0)
                            {
                                var text = scriptAnLine[counterInverse].Split(' ');
                                varString = text[text.Length - 1];
                                scriptBoxEditor.AppendText(space + "MESSAGE " + commandList[4] + " = '" + textFile.messageList[Int16.Parse(varString)] + " ';\n");
                                varString = commandList[4].ToString();
                            }
                        }
                        else if (Int16.TryParse(varString, out index))
                            scriptBoxEditor.AppendText(space + "MESSAGE_" + varString + " = ' " + textFile.messageList[Int16.Parse(varString)] + " ';\n");
                        scriptBoxEditor.AppendText(space + "" + commandList[2] + "( INTERNAL NARC ID  " + commandList[3] +
                                               " , MESSAGE_ID " + varString + " , OW_ID " + varString2 +
                                                   " , VIEW " + commandList[6] + " , TYPE " + commandList[7] + " );\n");
                    }
                    break;
                case "PlayCry":
                    varString = getTextFromCondition(commandList[3], "POK");
                    scriptBoxEditor.AppendText(space + commandList[2] + "( POKE " + varString + ", " + commandList[4] + " );\n");
                    break;
                case "PlaySSeq":
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( SOUND_ID " + commandList[3] + ");\n");
                    break;
                case "SetFlag":
                    scriptBoxEditor.AppendText(space + "FLAG " + getTextFromCondition(commandList[3], "FLA") + " = TRUE;" + "\n");
                    break;
                case "SetStackVar":
                    {
                        varString = getStoredMagic(varNameDictionary, varLevel, null, 0);
                        condition = getCondition(scriptsLine, lineCounter);
                        var text = getTextFromCondition(commandList[3], cond);
                        if (scriptsLine[lineCounter + 5].Contains("CompareStackCondition") && scriptsLine[lineCounter + 4].Contains("CompareStackCondition"))
                        {
                            scriptBoxEditor.AppendText(space + "If( " + varString + " " + condition + " " + text + ") ");
                            condition = getCondition(scriptsLine, lineCounter + 4);
                            scriptBoxEditor.AppendText(" " + condition + " ");
                        }
                        else
                            scriptBoxEditor.AppendText(space + "If( " + varString + " " + condition + " " + text + ");\n");
                        break;
                    }
                case "SetStackDerefVar":
                    newVar = checkStored(commandList, 3);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    conditionList = new List<string>();
                    operatorList = new List<string>();
                    blockList = new List<string>();
                    int tempLineCounter = lineCounter;
                    List<string> firstMembers = new List<string>();
                    List<string> conditions = new List<string>();
                    List<string> secondMembers = new List<string>();
                    int numSetVar = 0;
                    bool setVarCompareConditionBlock = (scriptsLine[tempLineCounter].Contains("SetStackDerefVar(09)") || scriptsLine[tempLineCounter].Contains("StoreFlag")) && scriptsLine[tempLineCounter + 1].Contains("SetStackVar(08)");
                    bool setVarsetVarConditionBlock = scriptsLine[tempLineCounter].Contains("SetStackDerefVar(09)") && scriptsLine[tempLineCounter + 1].Contains("SetStackDerefVar(09)") && scriptsLine[tempLineCounter + 2].Contains("CompareStackCondition");
                    if (setVarCompareConditionBlock || setVarsetVarConditionBlock)
                    {
                        while ((scriptsLine[tempLineCounter].Contains("SetStackDerefVar(09)") || scriptsLine[tempLineCounter].Contains("StoreFlag")) && scriptsLine[tempLineCounter + 1].Contains("SetStackVar(08)")
                            || scriptsLine[tempLineCounter].Contains("SetStackDerefVar(09)") && scriptsLine[tempLineCounter + 1].Contains("SetStackDerefVar(09)") && scriptsLine[tempLineCounter + 2].Contains("CompareStackCondition"))
                        {
                            numSetVar++;

                            //First Member
                            var next = scriptsLine[tempLineCounter];
                            commandList = next.Split(' ');
                            newVar = checkStored(commandList, 3);
                            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                            if (next.Contains("Flag"))
                                firstMembers.Add("FLAG " + varString);
                            else
                                firstMembers.Add(varString);
                            tempLineCounter++;

                            //Second Member
                            next = scriptsLine[tempLineCounter];
                            commandList = next.Split(' ');
                            newVar = checkStored(commandList, 3);
                            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                            //scriptBoxEditor.AppendText(space + "SET VAR " + varString + ";\n");
                            temp2 = varString;

                            secondMembers.Add(getTextFromCondition(temp2, cond));
                            tempLineCounter++;

                            //Condition
                            next = scriptsLine[tempLineCounter];
                            conditions.Add(getCondition(scriptsLine, tempLineCounter - 1));
                            tempLineCounter++;
                        }
                        scriptBoxEditor.AppendText(space + "If( ");
                        for (int i = 0; i < numSetVar; i++)
                        {
                            string extCondition = "";
                            if (i != 0)
                            {
                                extCondition = getCondition(scriptsLine, tempLineCounter - 1);
                                scriptBoxEditor.AppendText(" " + extCondition + " ");
                            }
                            if (numSetVar > 1)
                                scriptBoxEditor.AppendText("( " + firstMembers[i] + " " + conditions[i] + " " + secondMembers[i] + " )");
                            else
                                scriptBoxEditor.AppendText(firstMembers[i] + " " + conditions[i] + " " + secondMembers[i]);
                        }
                        scriptBoxEditor.AppendText("); \n");
                    }
                    else
                    {
                        scriptBoxEditor.AppendText(space + "SET VAR " + varString + ";\n");
                        break;
                    }
                    lineCounter = tempLineCounter - 1;
                    break;
                case "SetStackFlag":
                    newVar = checkStored(commandList, 3);
                    varString = getTextFromCondition(commandList[3], "FLA");
                    if (Utils.IsNaturalNumber(varString))
                        addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FLAG " + newVar, "BOL");
                    else
                        addToVarNameDictionary(varNameDictionary, varLevel, newVar, varString, "BOL");
                    temp = newVar.ToString();
                    break;
                case "SetTextScriptMessage":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    scriptBoxEditor.AppendText(space + commandList[2] + "( MESSAGE_TEXT " + commandList[3] + " , " +
                        "BOX_TEXT " + commandList[4] + " , " +
                        "SCRIPT " + commandList[5] + " ");
                    if (textFile != null)
                    {
                        var text2 = "";
                        text2 = getTextFromVar(scriptBoxEditor, commandList, varString, text2);
                    }
                    scriptBoxEditor.AppendText(");\n");
                    break;
                case "SetTrainerId":
                    scriptBoxEditor.AppendText(space + "TRAINER " + commandList[3] + " = TRUE;" + "\n");
                    break;
                case "SetVarAlter":
                    scriptBoxEditor.AppendText(space + "VAR NAME " + commandList[3] + " = [ALTER]" + ";\n");
                    break;
                case "SetVarBag":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR BAG " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarColoredItem":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    if (Utils.IsNaturalNumber(varString))
                        varString = getTextFromCondition(varString, "ITE");
                    scriptBoxEditor.AppendText(space + "VAR COLORED ITEM " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarHero":
                    scriptBoxEditor.AppendText(space + "VAR NAME " + commandList[3] + " = [HIRO]" + ";\n");
                    break;
                case "SetVarItem":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    if (Utils.IsNaturalNumber(varString))
                        varString = getTextFromCondition(varString, "ITE");
                    scriptBoxEditor.AppendText(space + "VAR ITEM " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarItemNumber":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
                    if (Utils.IsNaturalNumber(varString))
                        varString = getTextFromCondition(varString, "ITE");
                    scriptBoxEditor.AppendText(space + "VAR ITEM " + commandList[3] + " = " + commandList[2] + "( " + varString + " , " + commandList[5] + " , " + commandList[6] + " );\n");
                    break;
                case "SetVarBattleItem":
                    scriptBoxEditor.AppendText(space + "VAR BATTLE ITEM " + commandList[3] + " = " + commandList[4] + ";\n");
                    break;
                case "SetVarMove":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR MOVE " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarNations":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR NATION " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarNumberBound":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR NUM " + commandList[3] + " = " + commandList[2] + "( " + varString + " , BOUND " + commandList[5] + " );\n");
                    break;
                case "SetVarNickPokémon":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR NICK " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarNumber":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR NUM " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarPartyPokemon":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR POKE " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarPartyPokèmonNick":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR NICK " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarPokèmon":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    if (Utils.IsNaturalNumber(varString))
                        varString = getTextFromCondition(commandList[4], "POK");
                    scriptBoxEditor.AppendText(space + "VAR POKE " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarPokèmonDream":
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    if (Utils.IsNaturalNumber(varString))
                        varString = getTextFromCondition(commandList[3], "POK");
                    scriptBoxEditor.AppendText(space + "VAR POKE " + commandList[4] + " = " + varString + ";\n");
                    break;
                case "SetVarPokèLottoNumber":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR L " + commandList[3] + " = " + commandList[2] + "( " + varString + " , " + commandList[5] + " );\n");
                    break;
                case "SetVarPhraseBoxInput":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR STRING " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "SetVarRival":
                    scriptBoxEditor.AppendText(space + "VAR NAME " + commandList[3] + " = [RIVAL]" + ";\n");
                    break;
                case "SetVarTrainer":
                    scriptBoxEditor.AppendText(space + "VAR TRAINER " + commandList[3] + " = TRUE" + ";\n");
                    break;
                case "SetVarTrainer2":
                    scriptBoxEditor.AppendText(space + "VAR TRAINER_2 " + commandList[3] + " = TRUE" + ";\n");
                    break;
                case "SetVarType":
                    newVar = checkStored(commandList, 4);
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    scriptBoxEditor.AppendText(space + "VAR TYPE " + commandList[3] + " = " + varString + ";\n");
                    break;
                case "ShowMessageAt":
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + " , TYPE " + commandList[4] + " ");
                    readMessage(scriptBoxEditor, commandList, 5);
                    scriptBoxEditor.AppendText(" );\n");
                    break;
                case "ShowMoneyBox":
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( X " + commandList[3] + " , Y " + commandList[4] + " ");
                    scriptBoxEditor.AppendText(" );\n");
                    break;
                case "StoreBCSkyscraperTrainerNumber":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "TRAINER_NUMBER", "NOR");
                    scriptBoxEditor.AppendText(space + "TRAINER_NUMBER " + commandList[3] + "  = " + commandList[2] + "();\n");
                    break;
                case "StoreDayPart":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "DAY_PART", "NOR");
                    scriptBoxEditor.AppendText(space + "DAY_PART " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreHeroOrientation":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FACE_ORIENTATION", "NOR");
                    scriptBoxEditor.AppendText(space + "FACE_ORIENTATION " + commandList[3] + "  = " + commandList[2] + "();\n");
                    break;
                case "StoreSeason":
                    addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "SEASON", "NOR");
                    scriptBoxEditor.AppendText(space + "SEASON " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreValueInVar":
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
                case "StoreVarInVar":
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
                case "StoreDay":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "DAY", "DAY");
                    scriptBoxEditor.AppendText(space + "DAY " + commandList[3] + " = " + commandList[2] + "();\n");
                    break;
                case "StoreDerefVarInVar":
                    newVar = checkStored(commandList, 3);
                    newVar2 = checkStored(commandList, 4);
                    if (!Utils.IsNaturalNumber(commandList[4]))
                        addToVarNameDictionary(varNameDictionary, varLevel, newVar, commandList[3], "NOR");
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                    varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                    if (scriptsLine[lineCounter + 2].Contains("CallStd"))
                        varString = getTextFromCondition(commandList[4], "ITE");
                    scriptBoxEditor.AppendText(space + "VAR_CALL " + varString2 + " = " + varString + "\n");
                    break;
                case "StoreVarMessage":
                    newVar = checkStored(commandList, 3);
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MESSAGE_" + commandList[4], "NOR");
                    scriptBoxEditor.AppendText(space + "VAR " + commandList[3] + " = MESSAGE_" + commandList[4] + ";\n");
                    break;

                //        case "66":
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "66_VALUE", "NOR");
                //            scriptBoxEditor.AppendText(space + "66_VALUE " + commandList[3] + " = " + commandList[2] + "( NPC " + commandList[4] + " );\n");
                //            break;
                //        case "SetVar(83)":
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "83_VALUE", "NOR");
                //            scriptBoxEditor.AppendText(space + "83_VALUE " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "8A":
                //            var varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 4), "8A_VALUE", "NOR");
                //            scriptBoxEditor.AppendText(space + "8A_VALUE " + commandList[4] + " = " + commandList[2] + "( TRAINER_ID " + varString + " );\n");
                //            break;
                //        case "92":
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 4), "92_VALUE", "NOR");
                //            scriptBoxEditor.AppendText(space + "92_VALUE " + commandList[4] + " = " + commandList[2] + "( TRAINER_ID " + varString + " );\n");
                //            break;
                //        case "E1":
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "E1_VALUE", "NOR");
                //            scriptBoxEditor.AppendText(space + "E1_VALUE " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "17B":
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "17B_STATUS", "NOR");
                //            scriptBoxEditor.AppendText(space + "17B_STATUS " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "1BA":
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 4), "1BA_VALUE", "NOR");
                //            scriptBoxEditor.AppendText(space + "1BA_VALUE " + commandList[4] + " = " + commandList[2] + "( " + varString + " );\n");
                //            break;
                //        case "237":
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 5), "237_STATUS", "NOR");
                //            scriptBoxEditor.AppendText(space + "237_STATUS " + commandList[5] + " = " + commandList[2] + "( POKE " + commandList[3] + " , " + commandList[4] + " );\n");
                //            break;
                //        case "238":
                //            scriptBoxEditor.AppendText(space + "VAR " + commandList[4] + " = " + commandList[2] + "(P_1 " + commandList[3] + ");\n");
                //            break;
                //        case "23D":
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 4), "MESSAGE_ID", "NOR");
                //            scriptBoxEditor.AppendText(space + "MESSAGE_ID " + commandList[4] + " = " + commandList[2] + "( " + varString + " );\n");
                //            break;
                //        case "2AD":
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "OW_ID", "NOR");
                //            scriptBoxEditor.AppendText(space + "UNK " + commandList[3] + ", OW_ID " + commandList[4] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "2D1":
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "2D1_STATUS", "NOR");
                //            scriptBoxEditor.AppendText(space + "2D1_STATUS " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "AddPeople":
                //            scriptBoxEditor.AppendText(space + "" + commandList[2] + "( OW_ID " + commandList[3] + ");\n");
                //            break;
                //        case "AffinityCheck":
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "PEOPLE_NUM", "NOR");
                //            scriptBoxEditor.AppendText(space + "PEOPLE_NUM " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;

                //
                //        case "ChangeOwPosition":
                //            scriptBoxEditor.AppendText(space + "" + commandList[2] + "( OW_ID " + commandList[3] +
                //                                       ", X " + commandList[4] + ", Y " + commandList[5] + ");\n");
                //            break;
                //        case "ChangeOwPosition2":
                //            scriptBoxEditor.AppendText(space + "" + commandList[2] + "( OW_ID " + commandList[3] +
                //                                       ", P_2 " + commandList[4] + ");\n");
                //            break;
                //        case "CheckKeyItem":
                //            newVar = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HAVEKEY_ITEM", "BOL");
                //            scriptBoxEditor.AppendText(space + "HAVEKEY_ITEM " + commandList[4] + " = " + commandList[2] + "( ITEM " + commandList[3] + " );\n");
                //            break;
                //        case "CheckBattleExamAvailable":
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "IS_AVAILABLE", "BOL");
                //            scriptBoxEditor.AppendText(space + "IS_AVAILABLE " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "CheckBattleExamStarted":
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "IS_STARTED", "BOL");
                //            scriptBoxEditor.AppendText(space + "IS_STARTED " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "CheckDreamFunction":
                //            newVar = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FUNC_STATUS", "BOL");
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //            scriptBoxEditor.AppendText(space + "FUNC_STATUS " + commandList[4] + " = " + commandList[2] + "( FUNC " + varString + " );\n");
                //            break;
                //        case "CheckEgg":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "IS_EGG", "BOL");
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                //            scriptBoxEditor.AppendText(space + "IS_EGG " + commandList[3] + " = " + commandList[2] + "( POKE " + varString + " );\n");
                //            break;

                //        case "CheckFriend":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HAVE_FRIEND", "BOL");
                //            scriptBoxEditor.AppendText(space + "HAVE_FRIEND " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "CheckHavePokèmon":
                //            newVar = checkStored(commandList, 6);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HAVE_POKEMON", "BOL");
                //            varString = getTextFromCondition(commandList[3], "POK");
                //            varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
                //            scriptBoxEditor.AppendText(space + "HAVE_POKEMON " + commandList[6] + " = " + commandList[2] + "( POKE " + varString + " , " + commandList[4] + " , " + varString2 + " );\n");
                //            break;

                //        case "CheckItemInterestingBag":
                //            newVar = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HASINT_ITEM", "BOL");
                //            scriptBoxEditor.AppendText(space + "HASINT_ITEM " + commandList[4] + " = " + commandList[2] + "( INTEREST " + commandList[3] + " );\n");
                //            break;
                //        case "CheckLock":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "IS_LOCKED", "BOL");
                //            scriptBoxEditor.AppendText(space + "IS_LOCKED " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "CheckMoney":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MONEY_STATUS", "BOL");
                //            scriptBoxEditor.AppendText(space + "MONEY_STATUS " + commandList[3] + " = " + commandList[2] + "( AMOUNT " + commandList[4] + ", " + commandList[5] + " );\n");
                //            break;
                //        case "CheckPokèdexStatus":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "IS_ACTIVE", "BOL");
                //            scriptBoxEditor.AppendText(space + "IS_ACTIVE " + commandList[3] + " = " + commandList[2] + "( POKEDEX " + commandList[4] + " );\n");
                //            break;
                //        case "CheckPokèmonSeen":
                //            newVar = checkStored(commandList, 5);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HAVE_SEEN", "BOL");
                //            scriptBoxEditor.AppendText(space + "HAVE_SEEN " + commandList[5] + " = " + commandList[2] + "( " + commandList[3] + " POKEMON " + commandList[4] + " );\n");
                //            break;
                //        case "CheckPokèmonNickname":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "IS_DEFAULT", "BOL");
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                //            scriptBoxEditor.AppendText(space + "IS_DEFAULT " + commandList[3] + " = " + commandList[2] + "( POKEMON " + varString + " );\n");
                //            break;
                //        case "CheckPokèrus":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HAVE_POKERUS", "BOL");
                //            scriptBoxEditor.AppendText(space + "HAVE_POKERUS " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "CheckRelocatorPassword":
                //            newVar = checkStored(commandList, 6);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PASSWORD_STATUS", "BOL");
                //            scriptBoxEditor.AppendText(space + "PASSWORD_STATUS " + commandList[6] + " = " + commandList[2] + "( ITEM " + commandList[3] + " , " + "WORD_1 " + commandList[4] + " , WORD_2 " + commandList[5] + " );\n");
                //            break;
                //        case "CheckSendSaveCG":
                //            newVar = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "SEND_STATUS", "BOL");
                //            scriptBoxEditor.AppendText(space + "SEND_STATUS " + commandList[4] + " = " + commandList[2] + "( " + commandList[3] + " );\n");
                //            break;
                //        case "CheckSpacePokèmonDream":
                //            newVar = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HAVEDREAM_SPACE", "BOL");
                //            scriptBoxEditor.AppendText(space + "HAVEDREAM_SPACE " + commandList[4] + " = " + commandList[2] + "( POKE " + commandList[3] + " );\n");
                //            break;
                //        case "CheckChosenSpecies":
                //            newVar = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "RESULT", "BOL");
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
                //            scriptBoxEditor.AppendText(space + "RESULT " + commandList[4] + " = " + commandList[2] + "( SPECIE " + commandList[3] + " , POKE " + varString + " );\n");
                //            break;
                //        case "CheckWireless":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "WIRELESS_ACTIVATED", "BOL");
                //            scriptBoxEditor.AppendText(space + "WIRELESS_ACTIVATED " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "ChooseWifiSprite":
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 4), "WIFI_SPRITE_CHOSEN", "NOR");
                //            scriptBoxEditor.AppendText(space + "WIFI_SPRITE_CHOSEN " + commandList[4] + " = " + commandList[2] + "( " + varString + " );\n");
                //            break;

                //        case "ChooseInterestingItem":
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 5), "HAS_CHOSEN", "BOL");
                //            scriptBoxEditor.AppendText(space + "HAS_CHOSEN " + commandList[5] + " = " + commandList[2] + "( " + commandList[3] + " , " + commandList[4] + " );\n");
                //            break;
                //        case "ChooseMoveForgot":
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "HAS_CHOSEN", "BOL");
                //            scriptBoxEditor.AppendText(space + "HAS_CHOSEN " + commandList[3] + " = " + commandList[2] + "( " + commandList[4] + " , " + commandList[5] + " , " + commandList[6] + " );\n");
                //            break;

                //        case "ChooseUnityFloor":
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 6);
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 6), "HAS_CHOSEN", "BOL");
                //            scriptBoxEditor.AppendText(space + "HAS_CHOSEN " + commandList[6] + " = " + commandList[2] + "( " + commandList[3] + " , " + commandList[4] + " , " + commandList[5] + " );\n");
                //            break;

                //        case "ClearTrainerId":
                //            scriptBoxEditor.AppendText(space + "TRAINER " + commandList[3] + " = FALSE;" + "\n");
                //            break;


                //        case "Condition":
                //            break;
                //        case "CopyVar":
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), varString, "NOR");
                //            scriptBoxEditor.AppendText(space + "VAR " + commandList[3] + " = " + varString + ";\n");
                //            break;
                
                //        case "DreamBattle":
                //            newVar = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BATTLE_STATUS", "BOL");
                //            scriptBoxEditor.AppendText(space + "BATTLE_STATUS " + commandList[4] + " = " + commandList[2] + "( POKE " + commandList[3] + " );\n");
                //            break;
                //        case "DoubleMessage":
                //            if (commandList.Length <= 10)
                //            {
                //                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( COSTANT " + commandList[3] + " , COSTANT " + commandList[4] +
                //                                       " , BLACK_MESSAGE " + commandList[5] + " , WHITE_MESSAGE " + commandList[6] +
                //                                           " , OW_ID " + commandList[7] + " , VIEW " + commandList[8] + " );\n");
                //            }
                //            else
                //            {
                //                short index = 0;
                //                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
                //                varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 6);
                //                if (varString.Contains("M") && varString2.Contains("M"))
                //                {
                //                    index = Int16.Parse(varString.ToCharArray()[varString.Length - 1].ToString());
                //                    int index2 = Int16.Parse(varString2.ToCharArray()[varString2.Length - 1].ToString());
                //                    scriptBoxEditor.AppendText(space + "BLACK_MESSAGE" + varString + " = ' " + textFile.messageList[index] + " ';\n");
                //                    scriptBoxEditor.AppendText(space + "WHITE_MESSAGE" + varString2 + " = ' " + textFile.messageList[index] + " ';\n");
                //                }
                //                else if (Int16.TryParse(varString, out index) && (Int16.TryParse(varString2, out index)))
                //                {
                //                    scriptBoxEditor.AppendText(space + "BLACK_MESSAGE" + "MESSAGE_" + varString + " = ' " + textFile.messageList[Int16.Parse(varString)] + " ';\n");
                //                    scriptBoxEditor.AppendText(space + "WHITE_MESSAGE" + "MESSAGE_" + varString2 + " = ' " + textFile.messageList[Int16.Parse(varString)] + " ';\n");
                //                }
                //                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( COSTANT " + commandList[3] + " , COSTANT " + commandList[4] +
                //                                       " , BLACK_MESSAGE " + commandList[5] + " , WHITE_MESSAGE " + commandList[6] +
                //                                           " , OW_ID " + commandList[7] + " , VIEW " + commandList[8] + " );\n");
                //            }
                //            break;
                //        case "EventGreyMessage":
                //            varString2 = "";
                //            if (commandList.Length <= 6 && textFile == null)
                //            {
                //                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + " , TYPE " + commandList[4] + " );\n");
                //            }
                //            else
                //            {
                //                short index = 0;
                //                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //                if (varString.Contains("M"))
                //                {
                //                    index = Int16.Parse(varString.ToCharArray()[varString.Length - 1].ToString());
                //                    scriptBoxEditor.AppendText(space + varString + " = ' " + textFile.messageList[index] + " ';\n");
                //                }
                //                else if (varString.Length > 2 && Int16.Parse(varString.Substring(2, varString.Length - 2)) > 8000 && textFile != null)
                //                {
                //                    var scriptAnLine = scriptBoxEditor.Lines;
                //                    int counterInverse = scriptAnLine.Length - 1;
                //                    while (!scriptAnLine[counterInverse].Contains("VAR") && !scriptAnLine[counterInverse].Contains(" = ") && !scriptAnLine[counterInverse].Contains(varString) || scriptAnLine[counterInverse].Contains("+") && counterInverse > 0)
                //                        counterInverse--;
                //                    if (counterInverse > 0)
                //                    {
                //                        var text = scriptAnLine[counterInverse].Split(' ');
                //                        varString = text[text.Length - 1];
                //                        scriptBoxEditor.AppendText(space + "GREY_MESSAGE " + commandList[3] + " = '" + textFile.messageList[Int16.Parse(varString)] + " ';\n");
                //                        varString = commandList[3].ToString();
                //                    }
                //                }
                //                else if (Int16.TryParse(varString, out index))
                //                    scriptBoxEditor.AppendText(space + "GREY_MESSAGE_" + varString + " = ' " + textFile.messageList[Int16.Parse(varString)] + " ';\n");
                //                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + varString + " , TYPE " + commandList[4] + " );\n");
                //            }
                //            break;

                //        case "Lock":
                //            scriptBoxEditor.AppendText(space + "" + commandList[2] + "( OW_ID " + commandList[3] + ");\n");
                //            break;


                //  
                //        case "Message3":
                //            scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3]);
                //            scriptBoxEditor.AppendText(" = " + textFile.messageList[commandList[3].ToCharArray()[commandList[3].Length - 2]] + " ");

                //            scriptBoxEditor.AppendText(" );\n");
                //            break;
                //        case "MessageBattle":
                //            varString2 = "";
                //            if (commandList.Length <= 5 && textFile == null)
                //            {
                //                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( " + commandList[3] + ", MESSAGE_ID " + commandList[4] + " , " + commandList[5] + " );\n");
                //            }
                //            else
                //            {
                //                short index = 0;
                //                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                //                if (varString.Contains("M"))
                //                {
                //                    index = Int16.Parse(varString.ToCharArray()[varString.Length - 1].ToString());
                //                    scriptBoxEditor.AppendText(space + varString + " = ' " + textFile.messageList[index] + " ';\n");
                //                }
                //                else if (varString.Length > 2 && Int16.Parse(varString.Substring(2, varString.Length - 2)) > 8000 && textFile != null)
                //                {
                //                    var scriptAnLine = scriptBoxEditor.Lines;
                //                    int counterInverse = scriptAnLine.Length - 1;
                //                    while (!scriptAnLine[counterInverse].Contains("VAR") && !scriptAnLine[counterInverse].Contains(" = ") && !scriptAnLine[counterInverse].Contains(varString) || scriptAnLine[counterInverse].Contains("+") && counterInverse > 0)
                //                        counterInverse--;
                //                    if (counterInverse > 0)
                //                    {
                //                        var text = scriptAnLine[counterInverse].Split(' ');
                //                        varString = text[text.Length - 1];
                //                        scriptBoxEditor.AppendText(space + "BATTLE_MESSAGE " + commandList[4] + " = '" + textFile.messageList[Int16.Parse(varString)] + " ';\n");
                //                        varString = commandList[4].ToString();
                //                    }
                //                }
                //                else if (Int16.TryParse(varString, out index))
                //                    scriptBoxEditor.AppendText(space + "BATTLE_MESSAGE" + varString + " = ' " + textFile.messageList[Int16.Parse(varString)] + " ';\n");
                //                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( " + commandList[3] + ", MESSAGE_ID " + varString + " , " + commandList[5] + " );\n");
                //            }
                //            break;
                //        case "Multi":
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 7), "MULTI_CHOSEN", "MUL");
                //            mulString = new List<string>();
                //            scriptBoxEditor.AppendText(space + "MULTI_CHOSEN " + commandList[7] + " = " + commandList[2] + "(X " + commandList[4] + " , " +
                //                "Y " + commandList[5] + " , " +
                //                "CURSOR " + commandList[6] +
                //                ");\n");
                //            break;

                //        case "Multi3":
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 7), "MULTI_CHOSEN", "MUL");
                //            mulString = new List<string>();
                //            scriptBoxEditor.AppendText(space + "MULTI_CHOSEN " + commandList[7] + " = " + commandList[2] + "(X " + commandList[4] + " , " +
                //                "Y " + commandList[5] + " , " +
                //                "CURSOR " + commandList[6] + " , " +
                //                "P_4 " + commandList[8] + " , " +
                //                "P_5 " + commandList[9] +
                //                ");\n");
                //            break;
                //        case "MusicalMessage":
                //            varString2 = "";
                //            if (commandList.Length <= 5 && textFile == null)
                //            {
                //                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + " , COLOR " + commandList[4] + " );\n");
                //            }
                //            else
                //            {
                //                short index = 0;
                //                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //                if (varString.Contains("M"))
                //                {
                //                    index = Int16.Parse(varString.ToCharArray()[varString.Length - 1].ToString());
                //                    scriptBoxEditor.AppendText(space + varString + " = ' " + textFile.messageList[index] + " ';\n");
                //                }
                //                else if (varString.Length > 2 && Int16.Parse(varString.Substring(2, varString.Length - 2)) > 8000 && textFile != null)
                //                {
                //                    var scriptAnLine = scriptBoxEditor.Lines;
                //                    int counterInverse = scriptAnLine.Length - 1;
                //                    while (!scriptAnLine[counterInverse].Contains("VAR") && !scriptAnLine[counterInverse].Contains(" = ") && !scriptAnLine[counterInverse].Contains(varString) || scriptAnLine[counterInverse].Contains("+") && counterInverse > 0)
                //                        counterInverse--;
                //                    if (counterInverse > 0)
                //                    {
                //                        var text = scriptAnLine[counterInverse].Split(' ');
                //                        varString = text[text.Length - 1];
                //                        scriptBoxEditor.AppendText(space + "MUSICAL_MESSAGE " + commandList[3] + " = '" + textFile.messageList[Int16.Parse(varString)] + " ';\n");
                //                        varString = commandList[3].ToString();
                //                    }
                //                }
                //                else if (Int16.TryParse(varString, out index))
                //                    scriptBoxEditor.AppendText(space + "MUSICAL_MESSAGE_" + varString + " = ' " + textFile.messageList[Int16.Parse(varString)] + " ';\n");
                //                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + varString + " , COLOR " + commandList[4] + " );\n");
                //            }
                //            break;
                //        case "MusicalMessage2":
                //            varString2 = "";
                //            if (commandList.Length <= 5 && textFile == null)
                //            {
                //                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + " , COLOR " + commandList[4] + " );\n");
                //            }
                //            else
                //            {
                //                short index = 0;
                //                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //                if (varString.Contains("M"))
                //                {
                //                    index = Int16.Parse(varString.ToCharArray()[varString.Length - 1].ToString());
                //                    scriptBoxEditor.AppendText(space + varString + " = ' " + textFile.messageList[index] + " ';\n");
                //                }
                //                else if (varString.Length > 2 && Int16.Parse(varString.Substring(2, varString.Length - 2)) > 8000 && textFile != null)
                //                {
                //                    var scriptAnLine = scriptBoxEditor.Lines;
                //                    int counterInverse = scriptAnLine.Length - 1;
                //                    while (!scriptAnLine[counterInverse].Contains("VAR") && !scriptAnLine[counterInverse].Contains(" = ") && !scriptAnLine[counterInverse].Contains(varString) || scriptAnLine[counterInverse].Contains("+") && counterInverse > 0)
                //                        counterInverse--;
                //                    if (counterInverse > 0)
                //                    {
                //                        var text = scriptAnLine[counterInverse].Split(' ');
                //                        varString = text[text.Length - 1];
                //                        scriptBoxEditor.AppendText(space + "MUSICAL_MESSAGE " + commandList[3] + " = '" + textFile.messageList[Int16.Parse(varString)] + " ';\n");
                //                        varString = commandList[3].ToString();
                //                    }
                //                }
                //                else if (Int16.TryParse(varString, out index))
                //                    scriptBoxEditor.AppendText(space + "MUSICAL_MESSAGE_" + varString + " = ' " + textFile.messageList[Int16.Parse(varString)] + " ';\n");
                //                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + varString + " , COLOR " + commandList[4] + " );\n");
                //            }
                //            break;

                //        case "ReleaseOw":
                //            scriptBoxEditor.AppendText(space + commandList[2] + "( OW_ID " + commandList[3] +
                //                                       ", P_2 " + commandList[4] + ");\n");
                //            break;

                //        case "RemovePeople":
                //            scriptBoxEditor.AppendText(space + commandList[2] + "( OW_ID " + commandList[3] + ");\n");
                //            break;
                //        case "SetBadge":
                //            scriptBoxEditor.AppendText(space + "BADGE " + commandList[3] + " = TRUE;" + "\n");
                //            break;




                //        case "SetVarRoutine":
                //            newVar = checkStored(commandList, 3);
                //            newVar2 = checkStored(commandList, 4);
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ROUT_VAR " + commandList[3], "NOR");
                //            varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //            scriptBoxEditor.AppendText(space + varString2 + " = " + varString + ";\n");
                //            if (IsNaturalNumber(varString))
                //                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ROUT_VAR " + varString, "NOR");
                //            break;
                //        case "SetVarAffinityCheck":
                //            scriptBoxEditor.AppendText(space + "VAR NAME " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;

                //        case "StartDressPokèmon":
                //            newVar = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "DRESS_DECISION", "NOR");
                //            scriptBoxEditor.AppendText(space + "DRESS_DECISION " + commandList[4] + " = " + commandList[2] + "(" + commandList[3] + " , " + commandList[5] + " );\n");
                //            break;
                //        case "StoreActiveTrainerId":
                //            newVar = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "IS_ACTIVE", "BOL");
                //            scriptBoxEditor.AppendText(space + "IS_ACTIVE " + commandList[4] + "= " + commandList[2] + "( TRAINER_ID " + commandList[3] + ");\n");
                //            break;
                //        case "StoreBadge":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BADGE_STATUS", "BOL");
                //            varString = getTextFromCondition(commandList[4], "BAD");
                //            scriptBoxEditor.AppendText(space + "BADGE_STATUS " + commandList[3] + "= " + commandList[2] + "( BADGE " + varString + ");\n");
                //            break;
                //        case "StoreBadgeNumber":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BADGE_NUMBER", "NOR");
                //            scriptBoxEditor.AppendText(space + "BADGE_NUMBER " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "StoreBattleExamLevel":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "EXAM_LEVEL", "NOR");
                //            scriptBoxEditor.AppendText(space + "EXAM_LEVEL " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "StoreBattleExamModality":
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 5), "MODALITY", "NOR");
                //            scriptBoxEditor.AppendText(space + "MODALITY " + commandList[5] + " = " + commandList[2] + "( " + commandList[3] + " , " + varString + " );\n");
                //            break;
                //        case "StoreBattleExamStarNumber":
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "STAR_NUMBER", "NOR");
                //            scriptBoxEditor.AppendText(space + "STAR_NUMBER " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "StoreBattleExamType":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "EXAM_TYPE", "NOR");
                //            scriptBoxEditor.AppendText(space + "EXAM_TYPE " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "StoreBattleResult":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BATTLE_RESULT", "NOR");
                //            scriptBoxEditor.AppendText(space + "BATTLE_RESULT " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "StoreBirthDay":
                //            newVar = checkStored(commandList, 3);
                //            newVar2 = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BIRTH_MONTH", "NOR");
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "BIRTH_DAY", "NOR");
                //            scriptBoxEditor.AppendText(space + "BIRTH_MONTH " + commandList[3] + " , BIRTH_DAY " + commandList[4] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "StoreBoxNumber":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BOX_SPACE", "NOR");
                //            scriptBoxEditor.AppendText(space + "BOX_SPACE " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "StoreCanTeachDragonMove":
                //            newVar = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "STATUS", "NOR");
                //            scriptBoxEditor.AppendText(space + "STATUS " + commandList[4] + "  = " + commandList[2] + "( " + commandList[3] + " );\n");
                //            break;
                //        case "StoreChosenPokèmon":
                //            newVar = checkStored(commandList, 3);
                //            newVar2 = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "CHOSEN_STATUS", "BOL");
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "CHOSEN_POKEMON", "POK");
                //            scriptBoxEditor.AppendText(space + "CHOSEN_STATUS " + commandList[3] + ", CHOSEN_POKEMON " + commandList[4] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "StoreChosenPokèmonDragonMove":
                //            newVar = checkStored(commandList, 5);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "CHOSEN_POKEMON", "NOR");
                //            newVar2 = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "CHOSEN_STATUS", "BOL");
                //            scriptBoxEditor.AppendText(space + "CHOSEN_STATUS  " + commandList[4] + ", CHOSEN_POKEMON " + commandList[5] + "  = " + commandList[2] + "( " + commandList[3] + " );\n");
                //            break;
                //        case "StoreChosenPokèmonTrade":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "TRADE_POKEMON", "NOR");
                //            scriptBoxEditor.AppendText(space + "TRADE_POKEMON " + commandList[3] + "  = " + commandList[2] + "();\n");
                //            break;


                //        case "StoreDate":
                //            newVar = checkStored(commandList, 3);
                //            newVar2 = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MONTH", "NOR");
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "DAY", "NOR");
                //            scriptBoxEditor.AppendText(space + "MONTH " + commandList[3] + " , DAY " + commandList[4] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "StoreDataUnity":
                //            newVar = checkStored(commandList, 4);
                //            varString = "";
                //            if (commandList[3] == "0")
                //                varString = "TRAINER_NUMBER";
                //            else if (commandList[3] == "1")
                //                varString = "NATION";
                //            else if (commandList[3] == "2")
                //                varString = "FLOOR";
                //            else
                //                varString = "DATA";
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, varString, "NOR");
                //            scriptBoxEditor.AppendText(space + varString + " " + commandList[4] + " = " + commandList[2] + "( TYPE " + commandList[3] + " );\n");
                //            break;
                //        case "StoreDoublePhraseBoxInput":
                //            newVar = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "INSERT_CHECK", "NOR");
                //            newVar2 = checkStored(commandList, 5);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "WORD", "NOR");
                //            var newVar3 = checkStored(commandList, 6);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "WORD_2", "NOR");
                //            scriptBoxEditor.AppendText(space + "INSERT_CHECK " + commandList[4] + ", WORD " + commandList[5] + ", WORD_2 " + commandList[6] + " = " + commandList[2] + "( " + commandList[3] + " );\n");
                //            break;
                //        case "StoreDoublePhraseBoxInput2":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "INSERT_CHECK", "NOR");
                //            newVar2 = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "WORD", "NOR");
                //            newVar3 = checkStored(commandList, 5);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "WORD_2", "NOR");
                //            var newVar4 = checkStored(commandList, 6);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar4, "WORD_3", "NOR");
                //            var newVar5 = checkStored(commandList, 7);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar5, "WORD_4", "NOR");
                //            scriptBoxEditor.AppendText(space + "INSERT_CHECK " + commandList[3] + ", WORD " + commandList[4] + ", WORD_2 " + commandList[5] + ", WORD_3 " + commandList[6] + ", WORD_4 " + commandList[7] + " = " + commandList[2] + "();\n");
                //            break;
                //   
                //        case "StoreFirstTimePokèmonLeague":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "VICTORY_LEAGUE", "NOR");
                //            scriptBoxEditor.AppendText(space + "VICTORY_LEAGUE " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "StoreFloor":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FLOOR", "NOR");
                //            scriptBoxEditor.AppendText(space + "FLOOR " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "StoreGender":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "GENDER", "NOR");
                //            scriptBoxEditor.AppendText(space + "GENDER " + commandList[3] + "  = " + commandList[2] + "();\n");
                //            break;

                //        case "StoreHeroFriendCode":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FRIEND_CODE", "NOR");
                //            scriptBoxEditor.AppendText(space + "FRIEND_CODE " + commandList[3] + "  = " + commandList[2] + "();\n");
                //            break;
                //        case "StoreHeroNPCOrientation":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ORIENTATION", "NOR");
                //            scriptBoxEditor.AppendText(space + "ORIENTATION " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "StoreHeroPosition":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "X", "NOR");
                //            newVar2 = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "Y", "NOR");
                //            scriptBoxEditor.AppendText(space + "X " + commandList[3] + " , Y " + commandList[4] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "StoreHour":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HOUR", "NOR");
                //            scriptBoxEditor.AppendText(space + "HOUR " + commandList[3] + " ,  " + commandList[4] + " = " + commandList[2] + "();\n");
                //            break;

                //        case "StoreInterestingItemData":
                //            newVar = checkStored(commandList, 3);
                //            newVar2 = checkStored(commandList, 5);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEM", "NOR");
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "PRIZE", "NOR");
                //            scriptBoxEditor.AppendText(space + "ITEM " + commandList[3] + ", PRIZE " + commandList[5] + " = " + commandList[2] + "( " + commandList[4] + " , " + commandList[6] + " );\n");
                //            break;
                //        case "StoreItem":
                //            newVar = checkStored(commandList, 5);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEM_NUMBER", "NOR");
                //            scriptBoxEditor.AppendText(space + "ITEM_NUMBER " + commandList[5] + " = " + commandList[2] + "( ITEM " + commandList[3] + " , " + "NUMBER " + commandList[4] + " );\n");
                //            break;
                //        case "StoreItemBag":
                //            newVar = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEM_BAG", "NOR");
                //            scriptBoxEditor.AppendText(space + "ITEM_BAG " + commandList[4] + " = " + commandList[2] + "( ITEM " + commandList[3] + " );\n");
                //            break;
                //        case "StoreItemBagNumber":
                //            newVar = checkStored(commandList, 5);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEMBAG_SPACE", "NOR");
                //            scriptBoxEditor.AppendText(space + "ITEM_BAG_SPACE " + commandList[5] + " = " + commandList[2] + "( ITEM " + commandList[3] + " , " + "NUMBER " + commandList[4] + " );\n");
                //            break;
                //        case "StoreNPCFlag":
                //            newVar = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "NPC_FLAG", "NOR");
                //            scriptBoxEditor.AppendText(space + "NPC_FLAG " + commandList[4] + " = " + commandList[2] + "( NPC " + commandList[3] + " );\n");
                //            break;
                //        case "StoreNPCLevel":
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 5), "NPC_LEVEL", "NOR");
                //            scriptBoxEditor.AppendText(space + "NPC_LEVEL " + commandList[5] + " = " + commandList[2] + "( NPC " + varString + " , " + commandList[4] + " );\n");
                //            break;
                //        case "StorePartyCanUseMove":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKE_CAN", "NOR");
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                //            if (IsNaturalNumber(varString))
                //                varString = getTextFromCondition(commandList[4], "MOV");
                //            scriptBoxEditor.AppendText(space + "POKE_CAN " + commandList[3] + " = " + commandList[2] + "( MOVE " + varString + " );\n");
                //            break;
                //        case "StorePartyHavePokèmon":
                //            newVar = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKE_NUM", "NOR");
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //            if (IsNaturalNumber(varString))
                //                varString = getTextFromCondition(commandList[3], "POK");
                //            scriptBoxEditor.AppendText(space + "POKE_NUM " + commandList[4] + " = " + commandList[2] + "( POKE " + varString + " );\n");
                //            break;
                //        case "StorePartyNumberMinimum":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKEMON_NUMBER", "NOR");
                //            scriptBoxEditor.AppendText(space + "POKEMON_NUMBER " + commandList[3] + " = " + commandList[2] + "( LOWER_BOUND " + commandList[4] + " );\n");
                //            break;
                //        case "StorePartySpecies":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "SPECIE", "POK");
                //            scriptBoxEditor.AppendText(space + "SPECIE " + commandList[3] + " = " + commandList[2] + "( POKE " + commandList[4] + " );\n");
                //            break;
                //        case "StorePhotoName":
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //            scriptBoxEditor.AppendText(space + commandList[2] + "( " + varString + " );\n");
                //            break;
                //        case "StorePokèdex":
                //            newVar = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKEDEX_STATUS", "NOR");
                //            scriptBoxEditor.AppendText(space + "POKEDEX_STATUS " + commandList[4] + " = " + commandList[2] + "( POKEDEX " + commandList[3] + ");\n");
                //            break;
                //        case "StorePokèdexCaught":
                //            newVar = checkStored(commandList, 5);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKE_CAUGHT", "NOR");
                //            scriptBoxEditor.AppendText(space + "POKE_CAUGHT " + commandList[5] + " = " + commandList[2] + "( POKEDEX " + commandList[3] + " , " + commandList[4] + " );\n");
                //            break;
                //        case "StorePokèLottoResults":

                //            newVar2 = checkStored(commandList, 4);
                //            newVar3 = checkStored(commandList, 3);
                //            newVar4 = checkStored(commandList, 5);

                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 6);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "LOTTONUMBER_CHECK", "NOR");
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar4, "LOTTOPOKE_CHECK", "NOR");
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "LOTTOPOKE_ID", "NOR");
                //            scriptBoxEditor.AppendText(space + "LOTTOPOKE_ID " + commandList[3] + ", LOTTONUMBER_CHECK " + commandList[4] + ", LOTTOPOKE_CHECK " + commandList[5] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "StorePokemonCaughtWF":
                //            newVar = checkStored(commandList, 3);
                //            newVar2 = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "IS_CAUGHT", "BOL");
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "IS_TODAY", "BOL");
                //            scriptBoxEditor.AppendText(space + "IS_CAUGHT " + commandList[3] + ", IS_TODAY " + commandList[4] + " = " + commandList[2] + "( " + commandList[5] + " );\n");
                //            break;
                //        case "StorePokemonForm":
                //            newVar = checkStored(commandList, 4);
                //            newVar2 = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FORM", "NOR");
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //            scriptBoxEditor.AppendText(space + "FORM " + commandList[4] + " = " + commandList[2] + "( POKE " + varString + ");\n");
                //            break;
                //        case "StorePokèmonFormNumber":
                //            newVar = checkStored(commandList, 3);
                //            newVar2 = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FORM_NUMBER", "NOR");
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                //            scriptBoxEditor.AppendText(space + "FORM_NUMBER " + commandList[3] + " = " + commandList[2] + "( POKE " + varString + ");\n");
                //            break;
                //        case "StorePokèmonHappiness":
                //            newVar = checkStored(commandList, 3);
                //            newVar2 = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HAPPY_LEVEL", "NOR");
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                //            scriptBoxEditor.AppendText(space + "HAPPY_LEVEL " + commandList[3] + " = " + commandList[2] + "( " + varString + ");\n");
                //            break;
                //        case "StorePokèmonId":

                //            newVar = checkStored(commandList, 3);
                //            newVar2 = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "CHOSEN_POKEMON", "NOR");
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "CHOSEN_POKEMON", "NOR");
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //            scriptBoxEditor.AppendText(space + "VAR " + commandList[4] + " = StorePokèmonId( " + varString + " );\n");
                //            break;
                //        case "StorePokèmonMoveLearned":
                //            newVar = checkStored(commandList, 3);
                //            newVar2 = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MOVE_NUMBER", "NOR");
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                //            scriptBoxEditor.AppendText(space + "MOVE_NUMBER " + commandList[3] + " = " + commandList[2] + "( " + varString + ");\n");
                //            break;
                //        case "StorePokemonPartyAt":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKEMON", "NOR");
                //            scriptBoxEditor.AppendText(space + "POKEMON " + commandList[3] + " = " + commandList[2] + "( POSITION " + commandList[4] + " );\n");
                //            break;

                //        case "StorePokèmonPartyNumber":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PARTY_NUM", "NOR");
                //            scriptBoxEditor.AppendText(space + "PARTY_NUM " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "StorePokèmonPartyNumber3":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PARTY_NUM", "NOR");
                //            scriptBoxEditor.AppendText(space + "PARTY_NUM " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "StorePokèmonPartyNumberBadge":
                //            newVar = checkStored(commandList, 4);
                //            newVar2 = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PARTY_NUM", "NOR");
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "BADGE", "BAD");
                //            scriptBoxEditor.AppendText(space + "BADGE " + commandList[3] + ", PARTY_NUM " + commandList[4] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "StorePokèmonSex":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "SEX", "NOR");
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                //            scriptBoxEditor.AppendText(space + "SEX " + commandList[3] + " = " + commandList[2] + "( POKE " + varString + ", " + commandList[5] + " );\n");
                //            break;
                //        case "StorePokèmonStatusDragonMove":
                //            newVar = checkStored(commandList, 4);
                //            newVar2 = checkStored(commandList, 5);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "POKE_STATUS", "NOR");
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                //            scriptBoxEditor.AppendText(space + "POKE_STATUS " + commandList[5] + " = " + commandList[2] + "( " + commandList[3] + ", POKE " + varString + ");\n");
                //            break;
                //        case "StorePokèKronApplicationStatus":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PKRONAPP_STATUS", "NOR");
                //            scriptBoxEditor.AppendText(space + "PKRONAPP_STATUS " + commandList[4] + " = " + commandList[2] + "( PKRONAPP " + commandList[3] + ");\n");
                //            break;
                //        case "StorePokèKronStatus":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PKRON_STATUS", "NOR");
                //            scriptBoxEditor.AppendText(space + "PKRON_STATUS " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "StoreRandomNumber":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "NUMBER", "NOR");
                //            scriptBoxEditor.AppendText(space + "NUMBER " + commandList[3] + " = " + commandList[2] + "( " + commandList[4] + " );\n");
                //            break;
                //        case "StoreSaveData":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "SAVE_PAR", "NOR");
                //            newVar2 = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "SAVE_PAR2", "NOR");
                //            newVar3 = checkStored(commandList, 5);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "AMOUNT", "NOR");
                //            scriptBoxEditor.AppendText(space + "SAVE_PAR " + commandList[3] + ", SAVE_PAR2 " + commandList[4] + ", AMOUNT " + commandList[5] + " = " + commandList[2] + "();\n");
                //            break;
                
                //        case "StoreSinglePhraseBoxInput":
                //            newVar = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "INSERT_CHECK", "NOR");
                //            newVar2 = checkStored(commandList, 5);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "WORD", "NOR");
                //            scriptBoxEditor.AppendText(space + "INSERT_CHECK " + commandList[4] + ", WORD " + commandList[5] + " = " + commandList[2] + "( " + commandList[3] + " );\n");
                //            break;
                //        case "StoreStarter":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "STARTER", "NOR");
                //            scriptBoxEditor.AppendText(space + "STARTER  " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "StoreTrainerID":
                //            newVar = checkStored(commandList, 4);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "TRAINER_" + commandList[3], "NOR");
                //            scriptBoxEditor.AppendText(space + "VAR " + commandList[4] + "  = TRAINER_" + commandList[3] + ";\n");
                //            break;
                //        case "StoreWildBattleResult":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "WILDBATTLE_RESULT", "BOL");
                //            scriptBoxEditor.AppendText(space + "WILDBATTLE_RESULT " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "StoreWildBattlePokèmonStatus":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKEMON_STATUS", "NOR");
                //            scriptBoxEditor.AppendText(space + "POKEMON_STATUS " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "StoreVar(13)":
                //            newVar = checkStored(commandList, 3);
                //            newVar2 = checkStored(commandList, 4);
                //            if (!IsNaturalNumber(commandList[4]))
                //                addToVarNameDictionary(varNameDictionary, varLevel, newVar, commandList[3], "NOR");
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                //            varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //            if (IsNaturalNumber(varString))
                //                varString = getTextFromCondition(commandList[4], "ITE");
                //            scriptBoxEditor.AppendText(space + "VAR_13 " + varString2 + " = " + varString + "\n");
                //            break;
                //        case "StoreVarFlag":
                //            newVar = checkStored(commandList, 3);
                //            newVar2 = checkStored(commandList, 4);
                //            if (!IsNaturalNumber(commandList[4]))
                //                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FLAG_" + commandList[3], "NOR");
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //            varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                //            scriptBoxEditor.AppendText(space + "VAR_FLAG " + varString2 + " = " + commandList[3] + "\n");
                //            break;
                //        case "StoreAddVar":
                //            newVar = checkStored(commandList, 3);
                //            newVar2 = checkStored(commandList, 4);
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                //            varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //            scriptBoxEditor.AppendText(space + "VAR " + varString2 + " = " + varString2 + " + " + varString + ";\n");
                //            break;
                //        case "StoreSubVar":
                //            newVar = checkStored(commandList, 3);
                //            newVar2 = checkStored(commandList, 4);
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                //            varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //            scriptBoxEditor.AppendText(space + "VAR " + varString2 + " = " + varString2 + " - " + varString + ";\n");
                //            break;
                //    
                //        case "StoreVersion":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "VERSION", "NOR");
                //            scriptBoxEditor.AppendText(space + "VERSION " + commandList[3] + "= " + commandList[2] + "();\n");
                //            break;
                //        case "TakeItem":
                //            newVar = checkStored(commandList, 3);
                //            newVar2 = checkStored(commandList, 4);
                //            newVar3 = checkStored(commandList, 5);
                //            varString = getTextFromCondition(getStoredMagic(varNameDictionary, varLevel, commandList, 3), "ITE");
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "HAS_TAKEN", "BOL");
                //            varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                //            scriptBoxEditor.AppendText(space + "HAS_TAKEN " + commandList[5] + "= " + commandList[2] + "( ITEM " + varString + " , NUMBER " + varString2 + " );\n");
                //            break;
                //        case "TradePokèmon":
                //            newVar = checkStored(commandList, 3);
                //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //            scriptBoxEditor.AppendText(space + commandList[2] + "( " + varString + " );\n");
                //            break;
                //        case "YesNoBox":
                //            newVar = checkStored(commandList, 3);
                //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "YESNO_RESULT", "YNO");
                //            scriptBoxEditor.AppendText(space + "YESNO_RESULT  " + commandList[3] + " = " + commandList[2] + "();\n");
                //            break;
                //        case "WildPokèmonBattle":
                //            newVar = checkStored(commandList, 3);
                //            varString = getTextFromCondition(commandList[3], "POK");
                //            varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
                //            if (IsNaturalNumber(varString))
                //                varString2 = getTextFromCondition(commandList[5], "ITE");
                //            var varString3 = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                //            scriptBoxEditor.AppendText(space + commandList[2] + "( POKE " + varString + " , LEVEL " + varString3 + ", ITEM " + varString2 + " );\n");
                //            break;
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
                    scriptBoxEditor.AppendText(" );\n");
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
                        getCommandSimplifiedBW2(scriptsLine, ref functionLineCounter, space + "   ", visitedLine);
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
                    return "NO";
                }
                if (varString == "1")
                {
                    return "YES";
                }
            }
            if (condition == "FLA")
            {
                int flag = Int32.Parse(varString);

                switch (flag)
                {
                    case 1:
                        return "NPC_FIRST_TALK";
                    case 2753:
                        return "ITEM(DAY_EVENT)";
                    case 2754:
                        return "ROYAL_UNOVA_TOUR(DAY_EVENT)";
                    case 2756:
                        return "KOMOR_BATTLE(DAY_EVENT)";
                    case 2757:
                        return "BELLE_FIRST_ARALIA_LAB";
                    case 2758:
                        return "BELLE_BATTLE(DAY_EVENT)";
                    default:
                        return flag.ToString();
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
                    varString = getText((Int16.Parse(varString) + 11).ToString(), textNarc, 64) + " " + varString;
            }
            if (condition == "POK")
            {
                    varString = getText(varString, bwTextNarc, 284);
            }
            if (condition == "ITE")
            {
                    varString = getText(varString, bwTextNarc, 54);
            }
            if (condition == "MOV")
            {
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
            textHandler.readTextBW(reader, 0, new RichTextBox());
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
