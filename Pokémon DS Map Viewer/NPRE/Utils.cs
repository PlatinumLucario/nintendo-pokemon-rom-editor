using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using NPRE.Formats;
using PG4Map.Formats;
using System.Text.RegularExpressions;

public class Utils
{
    private static bool isVtx10 = false;
    private static string temp;
    private static int jumpBack;
    private static int newVar;
    private static int newVar2;
    private static int typeROM;
    private static string temp2;

    public static Stream loadStream(BinaryReader reader, uint size, Stream output)
    {
        BinaryWriter writer = new BinaryWriter(output);
        byte[] buffer = new byte[size];
        reader.Read(buffer, 0, buffer.Length);
        writer.Write(buffer);
        return output;
    }

    public static bool IsNaturalNumber(String strNumber)
    {
        Regex objNotNaturalPattern = new Regex("[^0-9]");
        Regex objNaturalPattern = new Regex("0*[1-9][0-9]*");
        return !objNotNaturalPattern.IsMatch(strNumber) &&
        objNaturalPattern.IsMatch(strNumber);
    } 

    public static ushort Read2BytesAsUInt16(byte[] bytes, int offset)
    {
        if (bytes.Length < 2)
            return 0;
        int num = 0;
        for (int i = 0; i < 2; i++)
        {
            num |= bytes[offset + i] << (8 * i);
        }
        return (ushort)num;
    }

    public static int Read4BytesAsInt32(byte[] bytes, int offset)
    {
        int num = 0;
        for (int i = 0; i < 4; i++)
        {
            num |= bytes[offset + i] << (8 * i);
        }
        return num;
    }

    public static uint Read4BytesAsUInt32(byte[] bytes, int offset)
    {
        uint num = 0;
        for (int i = 0; i < 4; i++)
        {
            num |= (uint)(bytes[offset + i] << (8 * i));
        }
        return num;
    }

    public static string readExtension(ClosableMemoryStream input)
    {
        BinaryReader reader = new BinaryReader(input);
        reader.BaseStream.Seek(0L, SeekOrigin.Begin);
        return Encoding.UTF8.GetString(reader.ReadBytes(4));
    }

    public static string ConvertToBits(int i)
    {
        string bits = Convert.ToString(i, 2);
        int len = bits.Length;
        if (len == 32)
        {
            return bits;
        }
        return new string('0', 32 - len) + bits;
    }

    public static int ConvertFromBits(string bits)
    {
        return Convert.ToInt32(bits, 2);
    }

    public static int GetBit(int i, int bitNum)
    {
        if (bitNum < 0 || bitNum > 31) return -1; // error value       
        return ConvertToBits(i)[31 - bitNum] - 48;
    }

    public static void SetBit(ref int i, int bitNum, int value)
    {
        if (bitNum < 0 || bitNum > 31 || value < 0 || value > 1) return; // i unchanged        
        char[] bitArray = ConvertToBits(i).ToCharArray();
        bitArray[31 - bitNum] = (char)(value + 48);
        i = Convert.ToInt32(new string(bitArray), 2);
    }

    public static void FlipBit(ref int i, int bitNum)
    {
        if (bitNum < 0 || bitNum > 31) return; // i unchanged        
        char[] bitArray = ConvertToBits(i).ToCharArray();
        bitArray[31 - bitNum] = (bitArray[31 - bitNum] == '1') ? '0' : '1';
        i = Convert.ToInt32(new string(bitArray), 2);
    }

    public static float GetSubBit(String bits, int start, int end)
    {
        var subBit = bits.Substring(start, end - start);
        return (float)Convert.ToDouble(subBit);
    }

    public static void CreateVoid(BinaryWriter b, uint offset)
    {
        for (long i = b.BaseStream.Position; i < offset; i += 1L)
            b.Write((byte)0xff);
    }

    public static string setInfoVertex(string textureName, int id)
    {
        return "";
    }



    private static int showTreeElement(DataGridView data, int numTree, int numNormal, int numVtx_16)
    {
        if (numNormal == 1)
        {
            if (numVtx_16 == 0)
                data.Rows.Add(new object[] { " ", "Top " + numTree });
            else if (numVtx_16 == 1)
                data.Rows.Add(new object[] { " ", "Center " + numTree });
            else if (numVtx_16 == 2)
                data.Rows.Add(new object[] { " ", "Bottom " + numTree });
            numVtx_16++;
            if (numVtx_16 == 3)
                numVtx_16 = 0;
        }
        return numVtx_16;
    }

    private static void showTreeNum(DataGridView data, ref int numTree, ref int numNormal)
    {
        if (numNormal == 0)
            data.Rows.Add(new object[] { " ", "Tree " + numTree });
        else
        {
            data.Rows.Add(new object[] { " ", "Radix " + numTree });
            numTree++;
        }


        numNormal++;
        if (numNormal == 2)
            numNormal = 0;
    }

    private static object[] popMtx_Restore(List<NsbmdModel.CommandStruct> command, DataGridView data, int commandCounter, String info)
    {
        object[] actualRow;
        actualRow = new object[8];
        actualRow[0] = command[commandCounter].startIdOffset.ToString();
        actualRow[1] = info;
        actualRow[2] = "Mtx_Restore";
        actualRow[3] = command[commandCounter].x.ToString("D6");
        actualRow[4] = " - ";
        actualRow[5] = " - ";
        actualRow[6] = " - ";
        actualRow[7] = " - ";
        data.Rows.Add(actualRow);
        return actualRow;
    }

    private static object[] popMtx_Scale(List<NsbmdModel.CommandStruct> command, DataGridView data, int commandCounter, String info)
    {
        object[] actualRow;
        actualRow = new object[8];
        actualRow[0] = command[commandCounter].startIdOffset.ToString();
        actualRow[1] = info;
        actualRow[2] = "Mtx_Scale";
        actualRow[3] = command[commandCounter].x.ToString();
        actualRow[4] = command[commandCounter].y.ToString("F2");
        actualRow[5] = command[commandCounter].y.ToString("F2");
        actualRow[6] = " - ";
        actualRow[7] = " - ";
        data.Rows.Add(actualRow);
        return actualRow;
    }

    private static object[] popColors(List<NsbmdModel.CommandStruct> command, DataGridView data, int commandCounter, String info)
    {
        object[] actualRow;
        actualRow = new object[8];
        actualRow[0] = command[commandCounter].startIdOffset.ToString();
        actualRow[1] = info;
        actualRow[2] = "Colors";
        actualRow[3] = command[commandCounter].x.ToString("D8");
        actualRow[4] = command[commandCounter].y.ToString("F2");
        actualRow[5] = " - ";
        actualRow[6] = command[commandCounter].par.ToString("F2");
        actualRow[7] = " - ";
        data.Rows.Add(actualRow);
        return actualRow;
    }

    private static object[] popDefault(List<NsbmdModel.CommandStruct> command, DataGridView data, ref int numVertex, int commandCounter, String info)
    {
        object[] actualRow;
        actualRow = new object[8];
        actualRow[0] = command[commandCounter].startIdOffset.ToString();
        actualRow[1] = info;
        actualRow[2] = command[commandCounter].id.ToString("F2");
        actualRow[3] = command[commandCounter].x.ToString("F2");
        actualRow[4] = command[commandCounter].y.ToString("F2");
        actualRow[5] = command[commandCounter].z.ToString("F2");
        actualRow[6] = command[commandCounter].par.ToString("F2");
        actualRow[7] = command[commandCounter].par2.ToString("F2");
        data.Rows.Add(actualRow);
        numVertex++;
        return actualRow;
    }

    private static object[] popEnd(List<NsbmdModel.CommandStruct> command, DataGridView data, ref int numVertex, int commandCounter)
    {
        object[] actualRow;
        actualRow = new object[8];
        actualRow[0] = command[commandCounter].startIdOffset.ToString();
        actualRow[1] = " ";
        actualRow[2] = "End";
        actualRow[3] = " - ";
        actualRow[4] = " - ";
        actualRow[5] = " - ";
        actualRow[6] = " - ";
        actualRow[7] = " - ";
        data.Rows.Add(actualRow);
        numVertex++;
        return actualRow;
    }

    private static object[] popStart(List<NsbmdModel.CommandStruct> command, DataGridView data, ref int numVertex, int commandCounter, String info)
    {
        object[] actualRow;
        actualRow = new object[8];
        actualRow[0] = command[commandCounter].startIdOffset.ToString();
        actualRow[1] = info;
        actualRow[2] = "Start";
        actualRow[3] = command[commandCounter].x.ToString("F2");
        actualRow[4] = " - ";
        actualRow[5] = " - ";
        actualRow[6] = command[commandCounter].par.ToString("F2");
        actualRow[7] = command[commandCounter].par2.ToString("F2");
        data.Rows.Add(actualRow);
        numVertex++;
        return actualRow;
    }

    private static object[] popVtx_Diff(List<NsbmdModel.CommandStruct> command, DataGridView data, ref int numVertex, int commandCounter, String info)
    {
        object[] actualRow;
        actualRow = new object[8];
        actualRow[0] = command[commandCounter].startIdOffset.ToString();
        actualRow[1] = info;
        actualRow[2] = "Vtx_Diff";
        actualRow[3] = ((command[commandCounter].x >> 4) * 4f).ToString("F2");
        actualRow[4] = ((command[commandCounter].z >> 4) * 4f).ToString("F2");
        actualRow[5] = ((command[commandCounter].y >> 4) * 4f).ToString("F2");
        actualRow[6] = command[commandCounter].par.ToString("F2");
        actualRow[7] = command[commandCounter].par2.ToString("F2");
        data.Rows.Add(actualRow);
        numVertex++;
        return actualRow;
    }

    private static object[] popVtx_YZ(List<NsbmdModel.CommandStruct> command, DataGridView data, ref int numVertex, int commandCounter, String info)
    {
        object[] actualRow;
        actualRow = new object[8];
        actualRow[0] = command[commandCounter].startIdOffset.ToString();
        actualRow[1] = info;
        actualRow[2] = "Vtx_YZ";

        if (!isVtx10)
            actualRow[3] = "(" + (command[commandCounter].x / 256f).ToString("F2") + ")";
        else
        {
            actualRow[3] = "(" + ((command[commandCounter].x >> 4) * 4f).ToString("F2") + ")";
            isVtx10 = false;
        }

        actualRow[4] = (command[commandCounter].z / 256f).ToString("F2");
        actualRow[5] = (command[commandCounter].y / 256f).ToString("F2");
        actualRow[6] = command[commandCounter].par.ToString("F2");
        actualRow[7] = command[commandCounter].par2.ToString("F2");
        data.Rows.Add(actualRow);
        numVertex++;
        return actualRow;
    }

    private static object[] popVtx_XZ(List<NsbmdModel.CommandStruct> command, DataGridView data, ref int numVertex, int commandCounter, String info)
    {
        object[] actualRow;
        actualRow = new object[8];
        actualRow[0] = command[commandCounter].startIdOffset.ToString();
        actualRow[1] = info;
        actualRow[2] = "Vtx_XZ";
        actualRow[3] = (command[commandCounter].x / 256f).ToString("F2");
        actualRow[4] = (command[commandCounter].z / 256f).ToString("F2");

        if (!isVtx10)
            actualRow[5] = "(" + (command[commandCounter].y / 256f).ToString("F2") + ")";
        else
        {
            actualRow[5] = "(" + ((command[commandCounter].y >> 4) * 4f).ToString("F2") + ")";
            isVtx10 = false;
        }

        actualRow[6] = command[commandCounter].par.ToString("F2");
        actualRow[7] = command[commandCounter].par2.ToString("F2");
        data.Rows.Add(actualRow);
        numVertex++;
        return actualRow;
    }

    private static object[] popVtx_XY(List<NsbmdModel.CommandStruct> command, DataGridView data, ref int numVertex, int commandCounter, String info)
    {
        object[] actualRow;
        actualRow = new object[8];
        actualRow[0] = command[commandCounter].startIdOffset.ToString();
        actualRow[1] = info;
        actualRow[2] = "Vtx_XY";
        actualRow[3] = (command[commandCounter].x / 256f).ToString("F2");

        if (!isVtx10)
            actualRow[4] = "(" + (command[commandCounter].z / 256f).ToString("F2") + ")";
        else
        {
            actualRow[4] = "(" + ((command[commandCounter].z >> 4) * 4f).ToString("F2") + ")";
            isVtx10 = false;
        }
        actualRow[5] = (command[commandCounter].y / 256f).ToString("F2");
        actualRow[6] = command[commandCounter].par.ToString("F2");
        actualRow[7] = command[commandCounter].par2.ToString("F2");
        data.Rows.Add(actualRow);
        numVertex++;
        return actualRow;
    }

    private static object[] popVtx_10(List<NsbmdModel.CommandStruct> command, DataGridView data, ref int numVertex, int commandCounter, String info)
    {
        object[] actualRow;
        actualRow = new object[8];
        actualRow[0] = command[commandCounter].startIdOffset.ToString();
        actualRow[1] = info;
        actualRow[2] = "Vtx_10";
        actualRow[3] = ((command[commandCounter].x >> 4) * 4f).ToString("F2");
        actualRow[4] = ((command[commandCounter].z >> 4) * 4f).ToString("F2");
        actualRow[5] = ((command[commandCounter].y >> 4) * 4f).ToString("F2");
        actualRow[6] = command[commandCounter].par.ToString("F2");
        actualRow[7] = command[commandCounter].par2.ToString("F2");
        data.Rows.Add(actualRow);
        isVtx10 = true;
        numVertex++;
        return actualRow;
    }

    private static object[] popTexture(List<NsbmdModel.CommandStruct> command, DataGridView data, int commandCounter, String info)
    {
        object[] actualRow;
        actualRow = new object[8];
        actualRow[0] = command[commandCounter].startIdOffset.ToString();
        actualRow[1] = info;
        actualRow[2] = "Texture";
        actualRow[3] = (command[commandCounter].x / 256f).ToString("F2");
        actualRow[4] = (command[commandCounter].y / 256f).ToString("F2");
        actualRow[5] = " - ";
        actualRow[6] = command[commandCounter].par.ToString("F2");
        actualRow[7] = command[commandCounter].par2.ToString("F2");
        data.Rows.Add(actualRow);
        return actualRow;
    }

    private static object[] popNormal(List<NsbmdModel.CommandStruct> command, DataGridView data, ref int numVertex, int commandCounter, String info)
    {
        object[] actualRow;
        numVertex = 0;
        actualRow = new object[8];
        actualRow[0] = command[commandCounter].startIdOffset.ToString();
        actualRow[1] = info;
        actualRow[2] = "Normal";
        actualRow[3] = ((command[commandCounter].x >> 4) * 4f).ToString("F2");
        actualRow[4] = ((command[commandCounter].z >> 4) * 4f).ToString("F2");
        actualRow[5] = ((command[commandCounter].y >> 4) * 4f).ToString("F2");
        actualRow[6] = command[commandCounter].par.ToString("F2");
        actualRow[7] = " - ";
        data.Rows.Add(actualRow);
        return actualRow;
    }

    private static object[] popVtx_16(List<NsbmdModel.CommandStruct> command, DataGridView data, ref int numVertex, int commandCounter, String info)
    {
        object[] actualRow;
        actualRow = new object[8];
        actualRow[0] = command[commandCounter].startIdOffset.ToString();
        actualRow[1] = info;
        actualRow[2] = "Vtx_16";
        actualRow[3] = (command[commandCounter].x / 256f).ToString("F2");
        actualRow[4] = (command[commandCounter].z / 256f).ToString("F2");
        actualRow[5] = (command[commandCounter].y / 256f).ToString("F2");
        actualRow[6] = command[commandCounter].par.ToString("F2");
        actualRow[7] = command[commandCounter].par2.ToString("F2");
        data.Rows.Add(actualRow);
        numVertex++;
        return actualRow;
    }


    public static ComboBox popFunctionList(string actualTextureName, ComboBox functionList)
    {
        //switch (actualTextureName)
        //{
        //    //case "tree01":
        //    {
        functionList.Items.Add("Add Tree");
        functionList.Items.Add("Remove Tree");
        //        }
        //        break;
        //}

        return functionList;
    }

    public static DataGridView showSpecificInfo(List<NsbmdModel.CommandStruct> command, DataGridView data, String textureName)
    {
        switch (textureName)
        {
            //case "tree01":
            //    {
            //        int numTree = 0;
            //        int numNormal = 0;
            //        int numVtx_16 = 0;
            //        data.Rows.Add(new object[] { "", textureName });
            //        if (command[0].par == 0)
            //            data.Rows.Add(new object[] { "Start", "Quads", "NULL ", " - " });
            //        else if (command[0].par == 1)
            //            data.Rows.Add(new object[] { "Start", "Quads_Strip", " - ", " - " });
            //        else if (command[0].par == 2)
            //            data.Rows.Add(new object[] { "Start", "Triangle", " - ", " - " });
            //        else if (command[0].par == 3)
            //            data.Rows.Add(new object[] { "Start", "Triangle_Strip", " - ", " - " });
            //        int numVertex = 4;
            //        int numObject = 0;
            //        for (int commandCounter = 1; commandCounter < command.Count; commandCounter++)
            //        {
            //            object[] actualRow;
            //            if (((numVertex == 4) && (commandCounter > 3)) && (commandCounter < (command.Count - 3)))
            //            {
            //                numObject++;
            //                numVertex = 0;
            //            }
            //            String info = Utils.setInfoVertex(textureName, command[commandCounter].id);
            //            switch (command[commandCounter].id)
            //            {
            //                case 0x14:
            //                    actualRow = popMtx_Restore(command, data, commandCounter, info);
            //                    break;

            //                case 0x1B:
            //                    actualRow = popMtx_Scale(command, data, commandCounter, info);
            //                    break;

            //                case 0x20:
            //                    actualRow = popColors(command, data, commandCounter, info);
            //                    break;

            //                case 0x21:
            //                    showTreeNum(data, ref numTree, ref numNormal);
            //                    actualRow = popNormal(command, data, ref numVertex, commandCounter, info);
            //                    break;

            //                case 0x22:
            //                    actualRow = popTexture(command, data, commandCounter, info);
            //                    break;

            //                case 0x23:
            //                    numVtx_16 = showTreeElement(data, numTree, numNormal, numVtx_16);
            //                    actualRow = popVtx_16(command, data, ref numVertex, commandCounter, info);
            //                    break;

            //                case 0x24:
            //                    actualRow = popVtx_10(command, data, ref numVertex, commandCounter, info);
            //                    break;

            //                case 0x25:
            //                    actualRow = popVtx_XY(command, data, ref numVertex, commandCounter, info);
            //                    break;

            //                case 0x26:
            //                    actualRow = popVtx_XZ(command, data, ref numVertex, commandCounter, info);
            //                    break;

            //                case 0x27:
            //                    actualRow = popVtx_YZ(command, data, ref numVertex, commandCounter, info);
            //                    break;

            //                case 0x28:
            //                    actualRow = popVtx_Diff(command, data, ref numVertex, commandCounter, info);
            //                    break;

            //                case 0x40:
            //                    actualRow = popStart(command, data, ref numVertex, commandCounter, info);
            //                    break;

            //                case 0x41:
            //                    actualRow = popEnd(command, data, ref numVertex, commandCounter);
            //                    break;

            //                default:
            //                    actualRow = popDefault(command, data, ref numVertex, commandCounter, info);
            //                    break;
            //            }
            //        }
            //        break;
            //    }
            default:
                {
                    data.Rows.Add(new object[] { "", textureName });
                    if (command[0].par == 0)
                        data.Rows.Add(new object[] { "Start", "Quads", "NULL ", " - " });
                    else if (command[0].par == 1)
                        data.Rows.Add(new object[] { "Start", "Quads_Strip", " - ", " - " });
                    else if (command[0].par == 2)
                        data.Rows.Add(new object[] { "Start", "Triangle", " - ", " - " });
                    else if (command[0].par == 3)
                        data.Rows.Add(new object[] { "Start", "Triangle_Strip", " - ", " - " });
                    int numVertex = 4;
                    int numObject = 0;
                    for (int commandCounter = 1; commandCounter < command.Count; commandCounter++)
                    {
                        object[] actualRow;
                        if (((numVertex == 4) && (commandCounter > 3)) && (commandCounter < (command.Count - 3)))
                        {
                            numObject++;
                            numVertex = 0;
                        }
                        String info = Utils.setInfoVertex(textureName, command[commandCounter].id);
                        switch (command[commandCounter].id)
                        {
                            case 0x14:
                                actualRow = popMtx_Restore(command, data, commandCounter, info);
                                break;

                            case 0x1B:
                                actualRow = popMtx_Scale(command, data, commandCounter, info);
                                break;

                            case 0x20:
                                actualRow = popColors(command, data, commandCounter, info);
                                break;

                            case 0x21:
                                actualRow = popNormal(command, data, ref numVertex, commandCounter, info);
                                break;

                            case 0x22:
                                actualRow = popTexture(command, data, commandCounter, info);
                                break;

                            case 0x23:
                                actualRow = popVtx_16(command, data, ref numVertex, commandCounter, info);
                                break;

                            case 0x24:
                                actualRow = popVtx_10(command, data, ref numVertex, commandCounter, info);
                                break;

                            case 0x25:
                                actualRow = popVtx_XY(command, data, ref numVertex, commandCounter, info);
                                break;
                            case 0x26:
                                actualRow = popVtx_XZ(command, data, ref numVertex, commandCounter, info);
                                break;

                            case 0x27:
                                actualRow = popVtx_YZ(command, data, ref numVertex, commandCounter, info);
                                break;

                            case 0x28:
                                actualRow = popVtx_Diff(command, data, ref numVertex, commandCounter, info);
                                break;

                            case 0x40:
                                actualRow = popStart(command, data, ref numVertex, commandCounter, info);
                                break;

                            case 0x41:
                                actualRow = popEnd(command, data, ref numVertex, commandCounter);
                                break;

                            default:
                                actualRow = popDefault(command, data, ref numVertex, commandCounter, info);
                                break;
                        }
                    }
                    break;
                }
        }
        return data;
    }


    internal static void getCommandSimplifiedDPP(string[] scriptsLine, int lineCounter, string space, RichTextBox scriptBoxEditor, List<Dictionary<int,string>> varNameDictionary, int varLevel, List<int> visitedLine, int typeROM, Texts textFile)
    {
        var line = scriptsLine[lineCounter];
        var commandList = line.Split(' ');
        string movId;
        string tipe;
        int offset = Int32.Parse(commandList[1].Substring(0,commandList[1].Length-1));
        var stringOffset = commandList[1];
        if (offset < 10)
            stringOffset = "000" + stringOffset;
        else if (offset>10 && offset<100)
            stringOffset = "00" + stringOffset;
        else if (offset > 100 && offset < 1000)
            stringOffset = "0" + stringOffset;

        //scriptBoxEditor.AppendText(stringOffset + " ");
        switch (commandList[2])
        {

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
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "OW_ID");
                scriptBoxEditor.AppendText(space + "UNK " + commandList[3] + ", OW_ID " + commandList[4] + " = " + commandList[2] + "();\n");
                break;
            case "ActPokèKronApplication":
                var varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
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
                    if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[7].TrimStart('(')))
                    {
                        functionLineCounter++;
                        do
                        {
                            line2 = scriptsLine[functionLineCounter];
                            var movList = line2.Split(' ');
                            if (line2.Length > 1)
                                scriptBoxEditor.AppendText(space + " MOV " + movList[2].ToString().TrimStart('m') + " TIMES " + movList[3] + "\n");
                            functionLineCounter++;
                        } while (!(line2.Contains("m254") || line2.Contains("FE")));
                        scriptBoxEditor.AppendText(space + "}\n");
                        return;
                    }
                }

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
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ACCESSORIES_TAKEN");
                scriptBoxEditor.AppendText(space + "ACCESSORIES_TAKEN " + commandList[5] + " = " + commandList[2] + "( ACCESSORIES " + commandList[3] + " , " + "NUMBER " + commandList[4] + " );\n");
                break;
            case "CheckBadge":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BADGE_STATUS");
                scriptBoxEditor.AppendText(space + "BADGE_STATUS " + commandList[4] + "= " + commandList[2] + "( BADGE " + commandList[3] + ");\n");
                break;
            case "CheckBoxSpace":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BOX_SPACE");
                scriptBoxEditor.AppendText(space + "BOX_SPACE " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "CheckCasinoPrizeCoins":
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "COINSBOX_SPACE");
                scriptBoxEditor.AppendText(space + "COINSBOX_SPACE " + commandList[3] + " = " + commandList[2] + "( PRIZE " + varString +  " );\n");
                break;
            case "CheckCoinsBoxSpace":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "COINSBOX_SPACE");
                scriptBoxEditor.AppendText(space + "COINSBOX_SPACE " + commandList[3] + " = " + commandList[2] + "( AMOUNT " + commandList[4] + " , " + commandList[5] + " );\n");
                break;
            case "CheckContestPicture":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "CONTESTPICTURE_STATUS");
                scriptBoxEditor.AppendText(space + "CONTESTPICTURE_STATUS " + commandList[4] + " = " + commandList[2] + "( PICTURE " + commandList[3] + " );\n");
                break;
            case "CheckHoney":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HONEY");
                scriptBoxEditor.AppendText(space + "HONEY " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "CheckItemBagSpace":
                newVar = checkStored(commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEMBAG_SPACE");
                scriptBoxEditor.AppendText(space + "ITEMBAG_SPACE " + commandList[5] + " = " + commandList[2] + "( ITEM " + commandList[3] + " , " + "NUMBER " + commandList[4] + " );\n");
                break;
            case "CheckItemBagNumber":
                newVar = checkStored(commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEMBAG_NUMBER");
                scriptBoxEditor.AppendText(space + "ITEMBAG_NUMBER " + commandList[5] + " = " + commandList[2] + "( ITEM " + commandList[3] + " , " + "NUMBER " + commandList[4] + " );\n");
                break;
            case "CheckDressPicture":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "DRESSPICTURE_STATUS");
                scriptBoxEditor.AppendText(space + "DRESSPICTURE_STATUS " + commandList[4] + " = " + commandList[2] + "( PICTURE " + commandList[3] + " );\n");
                break;
            case "CheckEgg":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "EGG_PARTY");
                scriptBoxEditor.AppendText(space + "EGG_PARTY " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "CheckErrorSave":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ERROR");
                scriptBoxEditor.AppendText(space + "ERROR " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "CheckMoney":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MONEY_STATUS");
                scriptBoxEditor.AppendText(space + "MONEY_STATUS " + commandList[3] + " = " + commandList[2] + "( AMOUNT " + commandList[4] + ", " + commandList[5] + " );\n");
                break;
            case "CheckPlantingLimit":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PLANTLIMIT_REACHED");
                scriptBoxEditor.AppendText(space + "PLANTLIMIT_REACHED " + commandList[4] + "  = " + commandList[2] + "( LIMIT" + commandList[3] + " );\n");
                break;
            case "CheckPoisoned":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POISONED");
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "POKEMON");
                scriptBoxEditor.AppendText(space + "POISONED " + commandList[3] + " = " + commandList[2] + "( POKEMON " + commandList[4] + " );\n");
                break;
            case "CheckPokèKron":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PKRON_STATUS");
                scriptBoxEditor.AppendText(space + "PKRON_STATUS " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "CheckPokèKronApplication":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PKRONAPP_STATUS");
                scriptBoxEditor.AppendText(space + "PKRONAPP_STATUS " + commandList[4] + " = " + commandList[2] + "( PKRONAPP " + commandList[3] + ");\n");
                break;
            case "CheckPokèmonCaught":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKE_CAUGHT");
                scriptBoxEditor.AppendText(space + "POKE_CAUGHT " + commandList[4] + " = " + commandList[2] + "( POKEMON " + commandList[3] + ");\n");
                break;
            case "CheckPokèMoveTeacherCompatibility":
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel,checkStored(commandList, 3), "POKE_COMPATIBLE");
                scriptBoxEditor.AppendText(space + "POKE_COMPATIBLE " + commandList[3] + " = " + commandList[2] + "( POKEMON " + commandList[4] + " );\n");
                break;
            case "CheckPokèrus":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKERUS");
                scriptBoxEditor.AppendText(space + "POKERUS " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "CheckRandomPokèmonSearch":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "SEARCH_RESULT");
                scriptBoxEditor.AppendText(space + "SEARCH_RESULT " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "CheckRibbonPokèmon":
                newVar = checkStored(commandList, 3);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "RIBBON_TAKEN");
                scriptBoxEditor.AppendText(space + "RIBBON_TAKEN " + commandList[3] + " = " + commandList[2] + "( POKE " + varString + ", RIBBON " + commandList[5] + " );\n");
                break;
            case "CheckSpecialItem":
                newVar = checkStored(commandList, 4);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "SPECIAL_ITEM");
                scriptBoxEditor.AppendText(space + "SPECIAL_ITEM " + commandList[4] + " = " + commandList[2] + "( ITEM " + commandList[3] + " );\n");
                break;
            case "ChooseUnion":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "UNION_DECISION");
                scriptBoxEditor.AppendText(space + "UNION_DECISION " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "ChooseWifiSprite":
                newVar = checkStored(commandList, 4);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "WIFI_SPRITE_CHOSEN");
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
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                var varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                var condition = getCondition(scriptsLine, lineCounter);
                scriptBoxEditor.AppendText(space + "If( " + varString + " " + condition + " " + varString2 + ")\n");
                break;
            case "Compare2":
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                condition = getCondition(scriptsLine, lineCounter);
                scriptBoxEditor.AppendText(space + "If( " + varString + " " + condition + " " + varString2 + ")\n");
                break;
            case "ComparePhraseBoxInput":
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "COMPARE_RESULT");
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
                if(!varString.Contains("0x"))
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar2, varString);
                scriptBoxEditor.AppendText(space + "VAR " + commandList[3] + " = " + varString  + ";\n");
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
                        readFunction(scriptsLine, lineCounter, space, scriptBoxEditor, varNameDictionary, varLevel, ref functionLineCounter, ref line2, visitedLine,typeROM, textFile);
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
                        readFunction(scriptsLine, lineCounter, space, scriptBoxEditor, varNameDictionary, varLevel, ref functionLineCounter, ref line2, visitedLine, typeROM, textFile);
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
                        readFunction(scriptsLine, lineCounter, space, scriptBoxEditor, varNameDictionary, varLevel, ref functionLineCounter, ref line2, visitedLine, typeROM, textFile);
                        return;
                    }
                }
                break;
            case "IncPokèmonHappiness":
                newVar = checkStored(commandList, 3);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                scriptBoxEditor.AppendText(space  + commandList[2] + "( POKEMON " + varString + " , AMOUNT " + commandList[3] + " );\n");
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
                        readFunction(scriptsLine, lineCounter, space, scriptBoxEditor, varNameDictionary, varLevel, ref functionLineCounter, ref line2, visitedLine, typeROM, textFile);
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
            case "Multi":
                newVar = checkStored(commandList, 7);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MULTI_CHOSEN");
                scriptBoxEditor.AppendText(space + "MULTI_CHOSEN " + commandList[7] + " = " + commandList[2] + "(X " + commandList[3] + " , " +
                    "Y " + commandList[4] + " , " +
                    "CURSOR " + commandList[5] +
                    " , " + commandList[6] +   
                    ");\n");
                break;
            case "Multi2":
                newVar = checkStored(commandList, 7);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MULTI_CHOSEN");
                scriptBoxEditor.AppendText(space + "MULTI_CHOSEN " + commandList[7] + " = " + commandList[2] + "(X " + commandList[3] + " , " +
                    "Y " + commandList[4] + " , " +
                    "CURSOR " + commandList[5] +
                    " , " + commandList[6] +
                    ");\n");
                break;
            case "Multi3":
                newVar = checkStored(commandList, 7);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MULTI_CHOSEN");
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
            case "SetTextScriptMessageMulti":
                scriptBoxEditor.AppendText(space  + commandList[2] + "( BOX_TEXT " + commandList[3] + " , " +
                    "MESSAGEBOX_TEXT  " + commandList[4] + " , " +
                    "SCRIPT " + commandList[5] +
                    ");\n");
                break;
            case "SetTextScriptMulti":
                newVar = checkStored(commandList, 3);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + ", SCRIPT " + commandList[4] + " ");
                text = "";
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
                //else 
                //    text = textFile.textList[Int16.Parse(varString)].text;
                //scriptBoxEditor.AppendText(" = " + text + " ");
                scriptBoxEditor.AppendText(");\n");
                break;
            case "SetTrainerId":
                scriptBoxEditor.AppendText(space + "TRAINER " + commandList[3] + " = ACTIVE;" + "\n");
                break;

            case "SetValue":
                newVar = checkStored(commandList, 3);
                newVar2 = checkStored(commandList, 4);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                //addToVarNameDictionary(varNameDictionary, varLevel, newVar, "VALUE " + varString2);
                scriptBoxEditor.AppendText(space + "VALUE " + varString2 + " = " + varString + "\n");
                break;
            case "SetVar":
                newVar = checkStored(commandList, 3);
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "VAR " + commandList[3]);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                scriptBoxEditor.AppendText(space + varString2 + " = " + varString + "\n");
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
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "DRESS_DECISION");
                scriptBoxEditor.AppendText(space + "DRESS_DECISION " + commandList[4] + " = " + commandList[2] + "(" + commandList[3] + " , " + commandList[5] + " );\n");
                break;
            case "ActPeopleContest":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "TRAINER_TYPE");
                scriptBoxEditor.AppendText(space + "TRAINER_TYPE " + commandList[4] + " = " + commandList[2] + "( PART_ID " + commandList[3] + " );\n");
                break;

            case "StoreBadgeNumber":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BADGE_NUMBER");
                scriptBoxEditor.AppendText(space + "BADGE_NUMBER " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreBattleResult":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BATTLE_RESULT");
                scriptBoxEditor.AppendText(space + "BATTLE_RESULT " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreBattleResult2":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BATTLE_RESULT2");
                scriptBoxEditor.AppendText(space + "BATTLE_RESULT2 " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreBerryGrownState":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "GROWN_STATE");
                scriptBoxEditor.AppendText(space + "GROWN_STATE " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreBerryMatureNumber":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BERRY_NUMBER");
                scriptBoxEditor.AppendText(space + "BERRY_NUMBER " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreBerryMatureType":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BERRY_TYPE");
                scriptBoxEditor.AppendText(space + "BERRY_TYPE " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreBerryPlanted":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BERRY");
                scriptBoxEditor.AppendText(space + "BERRY " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreBoundedVariable":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BOUNDED_VARIABLE");
                scriptBoxEditor.AppendText(space + "BOUNDED_VARIABLE " + commandList[3] + " = " + commandList[2] + "( BOUND " + commandList[4] + " );\n");
                break;
            case "StoreBurmyFormsNumber":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BURMYFORMS_NUMBER");
                scriptBoxEditor.AppendText(space + "BURMYFORMS_NUMBER " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreCasinoPrizeResult":
                newVar = checkStored(commandList, 4);
                newVar2 = checkStored(commandList, 5);
                var newVar3 = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PRIZE_ID");
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "PRIZE_NAME");
                addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "PRIZE_SCRIPT");
                scriptBoxEditor.AppendText(space + "PRIZE_SCRIPT " + commandList[3] + " , PRIZE_ID " + commandList[4] + ", PRIZE_NAME " + commandList[5] + " = " + commandList[2] + "();\n");
                break;
            case "StoreChosenPokèmon":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "CHOSEN_POKEMON");
                scriptBoxEditor.AppendText(space + "CHOSEN_POKEMON " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreChosenPokèmonTrade":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "TRADE_POKEMON");
                scriptBoxEditor.AppendText(space + "TRADE_POKEMON " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreDay":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "DAY");
                scriptBoxEditor.AppendText(space + "DAY " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreDoublePhraseBoxInput":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "INSERT_CHECK");
                newVar2 = checkStored(commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "WORD");
                newVar3 = checkStored(commandList, 6);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "WORD_2");
                scriptBoxEditor.AppendText(space + "INSERT_CHECK " + commandList[4] + ", WORD " + commandList[5] + ", WORD_2 " + commandList[6] + " = " + commandList[2] + "( " + commandList[3] + " );\n");
                break;
            case "StoreDoublePhraseBoxInput2":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "INSERT_CHECK");
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "WORD");
                newVar3 = checkStored(commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "WORD_2");
                var newVar4 = checkStored(commandList, 6);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar4, "WORD_3");
                var newVar5 = checkStored(commandList, 7);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar5, "WORD_4");
                scriptBoxEditor.AppendText(space + "INSERT_CHECK " + commandList[3] + ", WORD " + commandList[4] + ", WORD_2 " + commandList[5] + ", WORD_3 " + commandList[6] + ", WORD_4 " + commandList[7] + " = " + commandList[2] + "();\n");
                break;
            case "StoreFertilizer":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FERTILIZER");
                scriptBoxEditor.AppendText(space + "FERTILIZER " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreFlag":
                scriptBoxEditor.AppendText(space + "If( FLAG " + commandList[3] + " == TRUE)\n");
                break;
            case "StoreFirstPokèmonParty":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FIRST_POKEMON");
                scriptBoxEditor.AppendText(space + "FIRST_POKEMON " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreFirstTimePokèmonLeague":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "VICTORY_LEAGUE");
                scriptBoxEditor.AppendText(space + "VICTORY_LEAGUE " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreFloor":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FLOOR");
                scriptBoxEditor.AppendText(space + "FLOOR " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreGender":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "GENDER");
                scriptBoxEditor.AppendText(space + "GENDER " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreHappinessItem":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HAPPINESS_ITEM");
                scriptBoxEditor.AppendText(space + "HAPPINESS_ITEM " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreHeroFaceOrientation":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FACE_ORIENTATION");
                scriptBoxEditor.AppendText(space + "FACE_ORIENTATION " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreHeroFriendCode":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FRIEND_CODE");
                scriptBoxEditor.AppendText(space + "FRIEND_CODE " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreHeroPosition":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "X");
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "Y");
                scriptBoxEditor.AppendText(space  + "X " + commandList[3] + ",Y " + commandList[4] + " = " + commandList[2] + "();\n");
                break;

            case "StoreItemType":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEM_TYPE");
                scriptBoxEditor.AppendText(space + "ITEM_TYPE " + commandList[4] + " = " + commandList[2] + "( ITEM " + commandList[3] + " );\n");
                break;
            case "StoreMove":
                newVar = checkStored(commandList, 3);
                newVar2 = checkStored(commandList, 5);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MOVE_TEACHED");
                scriptBoxEditor.AppendText(space + "MOVE_TEACHED " + commandList[3] + " = " + commandList[2] + "( MOVE_ID " + commandList[4] + ", " + varString + " );\n");
                break;
            case "StoreMoveDeleter":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MOVE_DELETE");
                scriptBoxEditor.AppendText(space + "MOVE_DELETE " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StorePalParkPoint":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POINT");
                scriptBoxEditor.AppendText(space + "POINT " + commandList[4] + " = " + commandList[2] + "( TYPE " + commandList[3] + ");\n");
                break;
            case "StorePalParkPokèmonCaught":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKE_CAUGHT");
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
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "FASHION_LEVEL");
                scriptBoxEditor.AppendText(space + "FASHION_LEVEL " + commandList[5] + " = " + commandList[2] + "( POKEMON " + varString + ", FASHION " + commandList[4] + " );\n");
                break;
            case "StorePokèdex":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKEDEX_STATUS");
                scriptBoxEditor.AppendText(space + "POKEDEX_STATUS " + commandList[4] + " = " + commandList[2] + "( POKEDEX " + commandList[3] + ");\n");
                break;
            case "StorePokèLottoNumber":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "LOTTOTICKET_VALUE");
                scriptBoxEditor.AppendText(space + "LOTTOTICKET_VALUE " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StorePokèLottoResults":

                newVar = checkStored(commandList, 6);
                newVar2 = checkStored(commandList, 4);
                newVar3 = checkStored(commandList, 3);
                newVar4 = checkStored(commandList, 5);

                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 6);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "LOTTONUMBER_CHECK");
                addToVarNameDictionary(varNameDictionary, varLevel, newVar4, "LOTTOPOKE_CHECK");
                addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "LOTTOPOKE_ID");
                scriptBoxEditor.AppendText(space + "LOTTOPOKE_ID " + commandList[3] + ", LOTTONUMBER_CHECK " + commandList[4] + ", LOTTOPOKE_CHECK " + commandList[5] + " = " + commandList[2] + "();\n");
                break;
            case "StorePokèmonDeleter":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKE");
                scriptBoxEditor.AppendText(space + "POKE " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StorePokèmonHappiness":
                newVar = checkStored(commandList, 3);
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HAPPY_LEVEL");
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                scriptBoxEditor.AppendText(space + "HAPPY_LEVEL " + commandList[3] + " = " + commandList[2] + "( " + varString + ");\n");
                break;
            case "StorePokèmonId":

                newVar = checkStored(commandList, 3);
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "CHOSEN_POKEMON");
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "CHOSEN_POKEMON");
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                scriptBoxEditor.AppendText(space + "VAR " + commandList[4] + " = StorePokèmonId( "+ varString + " );\n");
                break;
            case "StorePokèmonLevel":
                newVar = checkStored(commandList, 3);
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "LEVEL");
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                scriptBoxEditor.AppendText(space + "LEVEL " + commandList[3] + " = " + commandList[2] + "( " + varString + ");\n");
                break;
            case "StorePokèmonPartyAtLevel":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PARTY_NUM");
                scriptBoxEditor.AppendText(space + "PARTY_NUM " + commandList[3] + " = " + commandList[2] + "( LEVEL " + commandList[4] + " );\n");
                break;
            case "StorePokèmonPartyNumber":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PARTY_NUM");
                scriptBoxEditor.AppendText(space + "PARTY_NUM " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StorePokèmonPartyNumber2":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PARTY_NUM");
                scriptBoxEditor.AppendText(space + "PARTY_NUM " + commandList[3] + " = " + commandList[2] + "( LIMIT " + commandList[4] + " );\n");
                break;
            case "StorePokèmonPartyNumber3":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PARTY_NUM");
                scriptBoxEditor.AppendText(space + "PARTY_NUM " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StorePokèmonMoveNumber":
                newVar = checkStored(commandList, 3);
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MOVE_NUM");
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                scriptBoxEditor.AppendText(space + "MOVE_NUM " + commandList[3] + " = " + commandList[2] + "( " + varString + " );\n");
                break;
            case "StorePokèmonNature":
                newVar = checkStored(commandList, 3);
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "NATURE");
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                scriptBoxEditor.AppendText(space + "NATURE " + commandList[3] + " = " + commandList[2] + "( POKE " + varString + " ) ;\n");
                break;
            case "StorePokèmonNumberCaught":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "CAUGHT_NUM");
                scriptBoxEditor.AppendText(space + "CAUGHT_NUM " + commandList[3] + " = " + commandList[2] + "();\n");
                break;

            case "StorePokèmonSize":
                newVar = checkStored(commandList, 3);
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "SIZE");
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                scriptBoxEditor.AppendText(space + "SIZE " + commandList[3] + " = " + commandList[2] + "( " + varString + ");\n");
                break;
            case "StoreRandomLevel":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "RANDOM_LEVEL");
                scriptBoxEditor.AppendText(space + "RANDOM_LEVEL " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreRandomPokèmonSearch":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "RANDOM_POKE");
                scriptBoxEditor.AppendText(space + "RANDOM_POKE " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreRibbonNumber":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "RIBBON");
                scriptBoxEditor.AppendText(space + "RIBBON " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreSpecificPokèmonParty":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HAVEPARTY_POKE");
                scriptBoxEditor.AppendText(space + "HAVEPARTY_POKE " + commandList[3] + " = " + commandList[2] + "( POKEMON " + commandList[4] + " );\n");
                break;
            case "StoreSinglePhraseBoxInput":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "INSERT_CHECK");
                newVar2 = checkStored(commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "WORD");
                scriptBoxEditor.AppendText(space + "INSERT_CHECK " + commandList[4] + ", WORD " + commandList[5] + " = " + commandList[2] + "( " + commandList[3] + " );\n");
                break;
            case "StoreStarCardNumber":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "STARS_NUM");
                scriptBoxEditor.AppendText(space + "STARS_NUM " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreStarter":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "STARTER");
                scriptBoxEditor.AppendText(space + "STARTER  " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreStatusSave":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "SAVE_STATUS");
                scriptBoxEditor.AppendText(space + "SAVE_STATUS " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreSwarmInfo":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PLACE");
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "POKE");
                scriptBoxEditor.AppendText(space + "PLACE " + commandList[3] + ", POKE " + commandList[4] + " = " + commandList[2] + "();\n");
                break;
            case "StoreTrainerId":
                scriptBoxEditor.AppendText(space + "If (TRAINER " + commandList[3] + " == INACTIVE);" + "\n");
                break;
            case "StoreTextVarUnion":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MESSAGE_" + commandList[3]);
                scriptBoxEditor.AppendText(space + "MESSAGE_ID " + commandList[4] + " = " + commandList[2] + "( ID " + commandList[3] + " );\n");
                break;
            case "StoreTime":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "TIME");
                scriptBoxEditor.AppendText(space + "TIME " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreVarPartId":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PART_ID");
                scriptBoxEditor.AppendText(space + "#DEFINE VAR " + commandList[3] + " AS PART_ID " + ";\n");
                break;
            case "StoreTypeBattle?":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BATTLE_TYPE");
                scriptBoxEditor.AppendText(space + "BATTLE_TYPE " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreUnionAlterChoice":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ALTER_CHOICE");
                scriptBoxEditor.AppendText(space + "ALTER_CHOICE " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "SetVarBattle2?":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "TRAINER_" + commandList[3]);
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
                scriptBoxEditor.AppendText(space + commandList[2] + "( MONEY " + varString +  ", " + commandList[4] + " );\n");
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
                scriptBoxEditor.AppendText(space  + commandList[2] + "( " + varString + " );\n");
                break;
            case "TrainerBattle":
                newVar = checkStored(commandList, 3);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                scriptBoxEditor.AppendText(space + commandList[2] + "( TRAINER " + varString + ", " + commandList[4] + " );\n");
                break;
            case "YesNoBox":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "YESNO_RESULT");
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
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, commandList.Length-2);
                    scriptBoxEditor.AppendText(" " + varString);
                }

                scriptBoxEditor.AppendText(");\n");
                break;
        }

    }

    internal static void getCommandSimplifiedHGSS(string[] scriptsLine, int lineCounter, string space, RichTextBox scriptBoxEditor, List<Dictionary<int, string>> varNameDictionary, int varLevel, List<int> visitedLine, int typeROM, Texts textFile)
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
            case "1EA":
                var varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "1EA_STATUS_" + commandList[3]);
                scriptBoxEditor.AppendText(space + "1EA_STATUS " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "22D":
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 4), "22D_STATUS");
                scriptBoxEditor.AppendText(space + "22D_STATUS " + commandList[4] + " = " + commandList[2] + "( " + varString + " );\n");
                break;
            case "238":
                scriptBoxEditor.AppendText(space + "VAR " + commandList[4] + " = " + commandList[2] + "(P_1 " + commandList[3] + ");\n");
                break;
            case "28C":
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 5), "28C_STATUS");
                scriptBoxEditor.AppendText(space + "28C_STATUS " + commandList[5] + " = " + commandList[2] + "( " + varString + " , " + commandList[4] + " );\n");
                break;
            case "28D":
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 6), "28D_STATUS");
                scriptBoxEditor.AppendText(space + "28D_STATUS " + commandList[5] + " = " + commandList[2] + "( " + varString + " , " + commandList[4] + " );\n");
                break;
            case "28F":
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 4), "28F_STATUS");
                scriptBoxEditor.AppendText(space + "28F_STATUS " + commandList[4] + " = " + commandList[2] + "( " + varString + " );\n");
                break;
            case "2AD":
                newVar = Int32.Parse(commandList[3].Substring(2, commandList[3].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                newVar2 = Int32.Parse(commandList[4].Substring(2, commandList[4].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "OW_ID");
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
                    if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[7].TrimStart('(')))
                    {
                        functionLineCounter++;
                        do
                        {
                            line2 = scriptsLine[functionLineCounter];
                            var movList = line2.Split(' ');
                            if (line2.Length > 1)
                                scriptBoxEditor.AppendText(space + " MOV " + movList[2].ToString().TrimStart('m') + " TIMES " + movList[3] + "\n");
                            functionLineCounter++;
                        } while (!line2.Contains("m254"));
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
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BADGE_STATUS");
                scriptBoxEditor.AppendText(space + "BADGE_STATUS " + commandList[4] + "= " + commandList[2] + "( BADGE " + commandList[3] + ");\n");
                break;
            case "CheckBoxSpace":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BOX_SPACE");
                scriptBoxEditor.AppendText(space + "BOX_SPACE " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "CheckCasinoPrizeCoins":
                newVar = checkStored(commandList, 3);
                newVar2 = checkStored(commandList, 4);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "COINSBOX_SPACE");
                scriptBoxEditor.AppendText(space + "COINSBOX_SPACE " + commandList[3] + " = " + commandList[2] + "( PRIZE " + varString + " );\n");
                break;
            case "CheckCoinsBoxSpace":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "COINSBOX_SPACE");
                scriptBoxEditor.AppendText(space + "COINSBOX_SPACE " + commandList[3] + " = " + commandList[2] + "( AMOUNT " + commandList[4] + " , " + commandList[5] + " );\n");
                break;
            case "CheckContestPicture":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "CONTESTPICTURE_STATUS");
                scriptBoxEditor.AppendText(space + "CONTESTPICTURE_STATUS " + commandList[4] + " = " + commandList[2] + "( PICTURE " + commandList[3] + " );\n");
                break;
            case "CheckHoney":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HONEY");
                scriptBoxEditor.AppendText(space + "HONEY " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "CheckItem":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEM_RECEIVED");
                scriptBoxEditor.AppendText(space + "ITEM_RECEIVED " + commandList[4] + " = " + commandList[2] + "( ITEM " + commandList[3] + " );\n");
                break;
            case "CheckItemBagSpace":
                newVar = checkStored(commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEMBAG_SPACE");
                scriptBoxEditor.AppendText(space + "ITEMBAG_SPACE " + commandList[5] + " = " + commandList[2] + "( ITEM " + commandList[3] + " , " + "NUMBER " + commandList[4] + " );\n");
                break;
            case "CheckItemBagNumber":
                newVar = checkStored(commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEMBAG_NUMBER");
                scriptBoxEditor.AppendText(space + "ITEMBAG_NUMBER " + commandList[5] + " = " + commandList[2] + "( ITEM " + commandList[3] + " , " + "NUMBER " + commandList[4] + " );\n");
                break;
            case "CheckDressPicture":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "DRESSPICTURE_STATUS");
                scriptBoxEditor.AppendText(space + "DRESSPICTURE_STATUS " + commandList[4] + " = " + commandList[2] + "( PICTURE " + commandList[3] + " );\n");
                break;
            case "CheckErrorSave":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ERROR");
                scriptBoxEditor.AppendText(space + "ERROR " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "CheckMoney":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MONEY_STATUS");
                scriptBoxEditor.AppendText(space + "MONEY_STATUS " + commandList[3] + " = " + commandList[2] + "( AMOUNT " + commandList[4] + ", " + commandList[5] + " );\n");
                break;
            case "CheckPoisoned":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POISONED");
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "POKEMON");
                scriptBoxEditor.AppendText(space + "POISONED " + commandList[3] + " = " + commandList[2] + "( POKEMON " + commandList[4] + " );\n");
                break;
            case "CheckPokèKron":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PKRON_STATUS");
                scriptBoxEditor.AppendText(space + "PKRON_STATUS " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "CheckPokèKronApplication":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PKRONAPP_STATUS");
                scriptBoxEditor.AppendText(space + "PKRONAPP_STATUS " + commandList[4] + " = " + commandList[2] + "( PKRONAPP " + commandList[3] + ");\n");
                break;
            case "CheckPokèmonCaught":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKE_CAUGHT");
                scriptBoxEditor.AppendText(space + "POKE_CAUGHT " + commandList[4] + " = " + commandList[2] + "( POKEMON " + commandList[3] + ");\n");
                break;
            case "CheckPokèMoveTeacherCompatibility":
                newVar = checkStored(commandList, 3);
                newVar2 = checkStored(commandList, 4);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKE_COMPATIBLE");
                scriptBoxEditor.AppendText(space + "POKE_COMPATIBLE " + commandList[3] + " = " + commandList[2] + "( POKEMON " + commandList[4] + " );\n");
                break;
            case "CheckPokèrus":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKERUS");
                scriptBoxEditor.AppendText(space + "POKERUS " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "CheckSpecialItem":
                newVar = checkStored(commandList, 4);
                newVar2 = checkStored(commandList, 3);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "SPECIAL_ITEM");
                scriptBoxEditor.AppendText(space + "SPECIAL_ITEM " + commandList[4] + " = " + commandList[2] + "( ITEM " + commandList[3] + " );\n");
                break;
            case "ChooseUnion":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "UNION_DECISION");
                scriptBoxEditor.AppendText(space + "UNION_DECISION " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "ChooseWifiSprite":
                newVar = checkStored(commandList, 4);
                newVar2 = checkStored(commandList, 3);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "WIFI_SPRITE_CHOSEN");
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
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "COMPARE_RESULT");
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                var varString3 = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
                var varString4 = getStoredMagic(varNameDictionary, varLevel, commandList, 6);
                var varString5 = getStoredMagic(varNameDictionary, varLevel, commandList, 7);
                scriptBoxEditor.AppendText(space + "COMPARE_RESULT " + commandList[3] + " = " + commandList[2] + "( " + varString + ", " + varString3 + ", " + varString4 + ", " + varString5 + " );\n");
                break;
            case "CopyVar":
                newVar = checkStored(commandList, 4);
                newVar2 = checkStored(commandList, 3);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                if (!IsNaturalNumber(commandList[4]))
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, varString);
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
                        readFunction(scriptsLine, lineCounter, space, scriptBoxEditor, varNameDictionary, varLevel, ref functionLineCounter, ref line2, visitedLine, typeROM, textFile);
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
                        readFunction(scriptsLine, lineCounter, space, scriptBoxEditor, varNameDictionary, varLevel, ref functionLineCounter, ref line2, visitedLine, typeROM, textFile);
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
                        readFunction(scriptsLine, lineCounter, space, scriptBoxEditor, varNameDictionary, varLevel, ref functionLineCounter, ref line2, visitedLine, typeROM, textFile);
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
                functionId = commandList[5];
                varNameDictionary.Add(new Dictionary<int, string>());
                for (var functionLineCounter = 0; functionLineCounter < scriptsLine.Length - 1; functionLineCounter++)
                {
                    var line2 = scriptsLine[functionLineCounter];
                    if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[6].TrimStart('(')))
                    {
                        readFunction(scriptsLine, lineCounter, space, scriptBoxEditor, varNameDictionary, varLevel, ref functionLineCounter, ref line2, visitedLine, typeROM, textFile);
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
            case "Message2":
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
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MULTI_CHOSEN");
                scriptBoxEditor.AppendText(space + "MULTI_CHOSEN " + commandList[7] + " = " + commandList[2] + "(X " + commandList[3] + " , " +
                    "Y " + commandList[4] + " , " +
                    "CURSOR " + commandList[5] +
                    " , " + commandList[6] +
                    ");\n");
                break;
            case "Multi2":
                newVar = checkStored(commandList, 7);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MULTI_CHOSEN");
                scriptBoxEditor.AppendText(space + "MULTI_CHOSEN " + commandList[7] + " = " + commandList[2] + "(X " + commandList[3] + " , " +
                    "Y " + commandList[4] + " , " +
                    "CURSOR " + commandList[5] +
                    " , " + commandList[6] +
                    ");\n");
                break;
            case "Multi3":
                newVar = checkStored(commandList, 7);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MULTI_CHOSEN");
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
                if (!IsNaturalNumber(commandList[4]))
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "NORM_VAR " + commandList[3]);
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
            case "SetVarPokémon":
                newVar = checkStored(commandList, 4);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                scriptBoxEditor.AppendText(space + "VAR POKE " + commandList[3] + " = " + varString + ";\n");
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
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "DRESS_DECISION");
                scriptBoxEditor.AppendText(space + "DRESS_DECISION " + commandList[4] + " = " + commandList[2] + "(" + commandList[3] + " , " + commandList[5] + " );\n");
                break;

            case "StoreBadgeNumber":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BADGE_NUMBER");
                scriptBoxEditor.AppendText(space + "BADGE_NUMBER " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreBattleResult":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BATTLE_RESULT");
                scriptBoxEditor.AppendText(space + "BATTLE_RESULT " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreBoundedVariable":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BOUNDED_VARIABLE");
                scriptBoxEditor.AppendText(space + "BOUNDED_VARIABLE " + commandList[3] + " = " + commandList[2] + "( BOUND " + commandList[4] + " );\n");
                break;
            case "StoreBurmyFormsNumber":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BURMYFORMS_NUMBER");
                scriptBoxEditor.AppendText(space + "BURMYFORMS_NUMBER " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreCasinoPrizeResult":
                newVar = checkStored(commandList, 4);
                newVar2 = checkStored(commandList, 5);
                var newVar3 = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PRIZE_ID");
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "PRIZE_NAME");
                addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "PRIZE_SCRIPT");
                scriptBoxEditor.AppendText(space + "PRIZE_SCRIPT " + commandList[3] + " , PRIZE_ID " + commandList[4] + ", PRIZE_NAME " + commandList[5] + " = " + commandList[2] + "();\n");
                break;
            case "StoreChosenPokèmon":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "CHOSEN_POKEMON");
                scriptBoxEditor.AppendText(space + "CHOSEN_POKEMON " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreChosenPokèmonTrade":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "TRADE_POKEMON");
                scriptBoxEditor.AppendText(space + "TRADE_POKEMON " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreDay":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "DAY");
                scriptBoxEditor.AppendText(space + "DAY " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreDoublePhraseBoxInput":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "INSERT_CHECK");
                newVar2 = checkStored(commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "WORD");
                newVar3 = checkStored(commandList, 6);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "WORD_2");
                scriptBoxEditor.AppendText(space + "INSERT_CHECK " + commandList[4] + ", WORD " + commandList[5] + ", WORD_2 " + commandList[6] + " = " + commandList[2] + "( " + commandList[3] + " );\n");
                break;
            case "StoreDoublePhraseBoxInput2":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "INSERT_CHECK");
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "WORD");
                newVar3 = checkStored(commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "WORD_2");
                var newVar4 = checkStored(commandList, 6);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar4, "WORD_3");
                var newVar5 = checkStored(commandList, 7);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar5, "WORD_4");
                scriptBoxEditor.AppendText(space + "INSERT_CHECK " + commandList[3] + ", WORD " + commandList[4] + ", WORD_2 " + commandList[5] + ", WORD_3 " + commandList[6] + ", WORD_4 " + commandList[7] + " = " + commandList[2] + "();\n");
                break;
            case "StoreFlag":
                scriptBoxEditor.AppendText(space + "If( FLAG " + commandList[3] + " == TRUE)\n");
                break;
            case "StoreFirstPokèmonParty":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FIRST_POKEMON");
                scriptBoxEditor.AppendText(space + "FIRST_POKEMON " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreFirstTimePokèmonLeague":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "VICTORY_LEAGUE");
                scriptBoxEditor.AppendText(space + "VICTORY_LEAGUE " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreFloor":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FLOOR");
                scriptBoxEditor.AppendText(space + "FLOOR " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreGender":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "GENDER");
                scriptBoxEditor.AppendText(space + "GENDER " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreHappinessItem":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HAPPINESS_ITEM");
                scriptBoxEditor.AppendText(space + "HAPPINESS_ITEM " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreHeroFaceOrientation":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FACE_ORIENTATION");
                scriptBoxEditor.AppendText(space + "FACE_ORIENTATION " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreHeroFriendCode":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FRIEND_CODE");
                scriptBoxEditor.AppendText(space + "FRIEND_CODE " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreHeroPosition":
                scriptBoxEditor.AppendText(space + "#DEFINE " + commandList[3] + " AS X ;\n");
                scriptBoxEditor.AppendText(space + "#DEFINE " + commandList[4] + " AS Y ;\n");
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "X");
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "Y");
                scriptBoxEditor.AppendText(space + "X,Y = " + commandList[2] + "();\n");
                break;

            case "StoreItemType":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEM_TYPE");
                scriptBoxEditor.AppendText(space + "ITEM_TYPE " + commandList[4] + " = " + commandList[2] + "( ITEM " + commandList[3] + " );\n");
                break;
            case "StoreMove":
                newVar = checkStored(commandList, 3);
                newVar2 = checkStored(commandList, 5);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MOVE_TEACHED");
                scriptBoxEditor.AppendText(space + "MOVE_TEACHED " + commandList[3] + " = " + commandList[2] + "( MOVE_ID " + commandList[4] + ", " + varString + " );\n");
                break;
            case "StoreMoveDeleter":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MOVE_DELETE");
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
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "FASHION_LEVEL");
                scriptBoxEditor.AppendText(space + "FASHION_LEVEL " + commandList[5] + " = " + commandList[2] + "( POKEMON " + varString + ", FASHION " + commandList[4] + " );\n");
                break;
            case "StorePokèdex":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKEDEX_STATUS");
                scriptBoxEditor.AppendText(space + "POKEDEX_STATUS " + commandList[4] + " = " + commandList[2] + "( POKEDEX " + commandList[3] + ");\n");
                break;
            case "StorePokèLottoNumber":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "LOTTOTICKET_VALUE");
                scriptBoxEditor.AppendText(space + "LOTTOTICKET_VALUE " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StorePokèLottoResults":

                newVar = checkStored(commandList, 6);
                newVar2 = checkStored(commandList, 4);
                newVar3 = checkStored(commandList, 3);
                newVar4 = checkStored(commandList, 5);

                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 6);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "LOTTONUMBER_CHECK");
                addToVarNameDictionary(varNameDictionary, varLevel, newVar4, "LOTTOPOKE_CHECK");
                addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "LOTTOPOKE_ID");
                scriptBoxEditor.AppendText(space + "LOTTOPOKE_ID " + commandList[3] + ", LOTTONUMBER_CHECK " + commandList[4] + ", LOTTOPOKE_CHECK " + commandList[5] + " = " + commandList[2] + "();\n");
                break;
            case "StorePokèmonDeleter":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKE");
                scriptBoxEditor.AppendText(space + "POKE " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StorePokèmonHappiness":
                newVar = checkStored(commandList, 3);
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HAPPY_LEVEL");
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                scriptBoxEditor.AppendText(space + "HAPPY_LEVEL " + commandList[3] + " = " + commandList[2] + "( " + varString + ");\n");
                break;
            case "StorePokèmonId":

                newVar = checkStored(commandList, 3);
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "CHOSEN_POKEMON");
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "CHOSEN_POKEMON");
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                scriptBoxEditor.AppendText(space + "VAR " + commandList[4] + " = StorePokèmonId( " + varString + " );\n");
                break;
            case "StorePokèmonPartyAtLevel":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PARTY_NUM");
                scriptBoxEditor.AppendText(space + "PARTY_NUM " + commandList[3] + " = " + commandList[2] + "( LEVEL " + commandList[4] + " );\n");
                break;
            case "StorePokèmonPartyNumber":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PARTY_NUM");
                scriptBoxEditor.AppendText(space + "PARTY_NUM " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StorePokèmonPartyNumber2":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PARTY_NUM");
                scriptBoxEditor.AppendText(space + "PARTY_NUM " + commandList[3] + " = " + commandList[2] + "( LIMIT " + commandList[4] + " );\n");
                break;
            case "StorePokèmonPartyNumber3":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PARTY_NUM");
                scriptBoxEditor.AppendText(space + "PARTY_NUM " + commandList[3] + " = " + commandList[2] + "();\n");
                break;


            case "StorePokèmonMoveNumber":
                newVar = checkStored(commandList, 3);
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MOVE_NUM");
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                scriptBoxEditor.AppendText(space + "MOVE_NUM " + commandList[3] + " = " + commandList[2] + "( " + varString + ");\n");
                break;

            case "StorePokèmonSize":
                newVar = checkStored(commandList, 3);
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "SIZE");
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                scriptBoxEditor.AppendText(space + "SIZE " + commandList[3] + " = " + commandList[2] + "( " + varString + ");\n");
                break;
            case "StorePokèmonType":
                newVar = checkStored(commandList, 3);
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "POKE_TYPE");
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3); 
                scriptBoxEditor.AppendText(space + "POKE_TYPE " + commandList[4] + " = " + commandList[2] + "( POKE " + varString + " );\n");
                break;

            case "StoreSpecificPokèmonParty":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HAVEPARTY_POKE");
                scriptBoxEditor.AppendText(space + "HAVEPARTY_POKE " + commandList[3] + " = " + commandList[2] + "( POKEMON " + commandList[4] + " );\n");
                break;
            case "StoreSinglePhraseBoxInput":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "INSERT_CHECK");
                newVar2 = checkStored(commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "WORD");
                scriptBoxEditor.AppendText(space + "INSERT_CHECK " + commandList[4] + ", WORD " + commandList[5] + " = " + commandList[2] + "( " + commandList[3] + " );\n");
                break;
            case "StoreStarCardNumber":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "STARS_NUM");
                scriptBoxEditor.AppendText(space + "STARS_NUM " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreStarter":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "STARTER");
                scriptBoxEditor.AppendText(space + "STARTER  " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreStatusSave":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "SAVE_STATUS");
                scriptBoxEditor.AppendText(space + "SAVE_STATUS " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreTrainerId":
                scriptBoxEditor.AppendText(space + "If (TRAINER " + commandList[3] + " == INACTIVE);" + "\n");
                break;
            case "StoreTextVarUnion":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MESSAGE_" + commandList[3]);
                scriptBoxEditor.AppendText(space + "MESSAGE_ID " + commandList[4] + " = " + commandList[2] + "( ID " + commandList[3] + " );\n");
                break;
            case "StoreTime":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "TIME");
                scriptBoxEditor.AppendText(space + "TIME " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreTypeBattle?":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BATTLE_TYPE");
                scriptBoxEditor.AppendText(space + "BATTLE_TYPE " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreVersion":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "VERSION");
                scriptBoxEditor.AppendText(space + "VERSION " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "SetVarBattle2?":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "TRAINER_" + commandList[3]);
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
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "YESNO_RESULT");
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
            case "CheckFirstTimePokèmonLeague":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "VICTORY_LEAGUE");
                scriptBoxEditor.AppendText(space + "VICTORY_LEAGUE " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "CopyVar2":
                newVar = checkStored(commandList, 4);
                newVar2 = checkStored(commandList, 3);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                if (!(IsNaturalNumber(varString)))
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar2, varString);
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
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "YESNOLOW_RESULT");
                scriptBoxEditor.AppendText(space + "YESNOLOW_RESULT  " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreBoxNumber":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BOX_SPACE");
                scriptBoxEditor.AppendText(space + "BOX_SPACE " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoredoublePhraseBoxInput":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "INSERT_CHECK");
                newVar2 = checkStored(commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "WORD");
                newVar3 = checkStored(commandList, 6);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "WORD_2");
                scriptBoxEditor.AppendText(space + "INSERT_CHECK " + commandList[4] + ", WORD " + commandList[5] + ", WORD_2 " + commandList[6] + " = " + commandList[2] + "( " + commandList[3] + " );\n");
                break;
            case "StoredoublePhraseBoxInput2":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "INSERT_CHECK");
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "WORD");
                newVar3 = checkStored(commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "WORD_2");
                newVar4 = checkStored(commandList, 6);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar4, "WORD_3");
                newVar5 = checkStored(commandList, 7);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar5, "WORD_4");
                scriptBoxEditor.AppendText(space + "INSERT_CHECK " + commandList[3] + ", WORD " + commandList[4] + ", WORD_2 " + commandList[5] + ", WORD_3 " + commandList[6] + ", WORD_4 " + commandList[7] + " = " + commandList[2] + "();\n");
                break;
            case "StoreItem":
                newVar = checkStored(commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEM_NUMBER");
                scriptBoxEditor.AppendText(space + "ITEM_NUMBER " + commandList[5] + " = " + commandList[2] + "( ITEM " + commandList[3] + " , " + "NUMBER " + commandList[4] + " );\n");
                break;
            case "StorePhotoName":
                newVar = checkStored(commandList, 3);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                scriptBoxEditor.AppendText(space + commandList[2] + "( " + varString + " );\n");
                break;
            case "StorePhotoSpace":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PHOTO_SPACE");
                scriptBoxEditor.AppendText(space + "PHOTO_SPACE " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StorePokèKronApplicationStatus":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PKRONAPP_STATUS");
                scriptBoxEditor.AppendText(space + "PKRONAPP_STATUS " + commandList[4] + " = " + commandList[2] + "( PKRONAPP " + commandList[3] + ");\n");
                break;
            case "StorePokèmonTrade":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKEMON_TRADE");
                scriptBoxEditor.AppendText(space + "POKEMON_TRADE" + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StorePokèKronStatus":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PKRON_STATUS");
                scriptBoxEditor.AppendText(space + "PKRON_STATUS " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
        }

    }

    private static string getVarString(RichTextBox scriptBoxEditor, Texts textFile, string[] commandList, string varString, string text)
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

    private static void getVarId(RichTextBox scriptBoxEditor, string[] commandList, ref int findValue, int pos, ref string id)
    {
        for (int i = pos - 1; i > 0 && findValue == 0; i--)
        {
            var line3 = scriptBoxEditor.Lines[i];
            if (line3.Contains("VAR " + commandList[3]))
            {
                findValue = 1;
                var lineVector = line3.Split(' ');
                var lineLength = lineVector.Length;
                id = lineVector[lineLength - 1].Substring(0, 2);
                if (id.ToCharArray()[id.Length - 1] == ';')
                    id = lineVector[lineLength - 1].Substring(0, 1);
            }
        }
    }

    internal static void getCommandSimplifiedBW(string[] scriptsLine, int lineCounter, string space, RichTextBox scriptBoxEditor, List<Dictionary<int, string>> varNameDictionary, int varLevel, List<int> visitedLine, int typeROM, Texts textFile)
    {
        var line = scriptsLine[lineCounter];
        var commandList = line.Split(' ');
        string movId;
        string tipe;
        //scriptBoxEditor.AppendText(commandList[1] + " ");
        switch (commandList[2])
        {

            case "79":
                var varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 5), "79_STATUS");
                scriptBoxEditor.AppendText(space + "79_STATUS " + commandList[5] + " = " + commandList[2] + "( OW_ID " + varString+ " , " + commandList[4] + " );\n");
                break;
            case "CE":
                addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "CE_VALUE");
                scriptBoxEditor.AppendText(space + "CE_VALUE " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "D2":
                addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "D2_VALUE");
                scriptBoxEditor.AppendText(space + "D2_VALUE " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "E1":
                addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "E1_VALUE");
                scriptBoxEditor.AppendText(space + "E1_VALUE " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "17B":
                addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "17B_STATUS");
                scriptBoxEditor.AppendText(space + "17B_STATUS " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "238":
                scriptBoxEditor.AppendText(space + "VAR " + commandList[4] + " = " + commandList[2] + "(P_1 " + commandList[3] + ");\n");
                break;
            case "23D":
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 4), "MESSAGE_ID");
                scriptBoxEditor.AppendText(space + "MESSAGE_ID " + commandList[4] + " = " + commandList[2] + "( " + varString + " );\n");
                break;
            case "2AD":
                addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "OW_ID");
                scriptBoxEditor.AppendText(space + "UNK " + commandList[3] + ", OW_ID " + commandList[4] + " = " + commandList[2] + "();\n");
                break;
            case "2D1":
                addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "2D1_STATUS");
                scriptBoxEditor.AppendText(space + "2D1_STATUS " + commandList[3] + " = " + commandList[2] + "();\n");
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
                    if (functionLineCounter + 1 < scriptsLine.Length && scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[7].TrimStart('(')))
                    {
                        functionLineCounter++;
                        do
                        {
                            line2 = scriptsLine[functionLineCounter];
                            var movList = line2.Split(' ');
                            if (line2.Length > 1)
                                scriptBoxEditor.AppendText(space + " MOV " + movList[2].ToString().TrimStart('m') + " TIMES " + movList[3] + "\n");
                            functionLineCounter++;
                        } while (!line2.Contains("m254") && functionLineCounter + 1 < scriptsLine.Length);
                        scriptBoxEditor.AppendText(space + "}\n");
                        return;
                    }
                }

                break;
            case "BorderedMessage":
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + " , COLOR " + commandList[4] + " ");
                readMessage(scriptBoxEditor, commandList, 5);
                scriptBoxEditor.AppendText(" );\n");
                break;
            case "BubbleMessage":
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + " , VIEW " + commandList[4] + " ");
                readMessage(scriptBoxEditor, commandList, 5);
                scriptBoxEditor.AppendText(" );\n");
                break;
            case "CallMessageBox":
                scriptBoxEditor.AppendText(space + "BORDER " + commandList[5] + " = " + commandList[2] + "( MESSAGE_ID " + commandList[3] +
                                           ", TYPE " + commandList[4] + " ");
                if (commandList.Length > 5)
                    for (int i = 6; i < commandList.Length; i++)
                        scriptBoxEditor.AppendText(commandList[i] + " ");
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
            case "CheckItemBagSpace":
                newVar = checkStored(commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEMBAG_SPACE");
                scriptBoxEditor.AppendText(space + "ITEMBAG_SPACE " + commandList[5] + " = " + commandList[2] + "( ITEM " + commandList[3] + " , " + "NUMBER " + commandList[4] + " );\n");
                break;
            case "CheckItemBagNumber":
                newVar = checkStored(commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEMBAG_NUMBER");
                scriptBoxEditor.AppendText(space + "ITEMBAG_NUMBER " + commandList[5] + " = " + commandList[2] + "( ITEM " + commandList[3] + " , " + "NUMBER " + commandList[4] + " );\n");
                break;
            case "CheckMoney":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MONEY_STATUS");
                scriptBoxEditor.AppendText(space + "MONEY_STATUS " + commandList[3] + " = " + commandList[2] + "( AMOUNT " + commandList[4] + ", " + commandList[5] + " );\n");
                break;
            case "ChooseWifiSprite":
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList,4), "WIFI_SPRITE_CHOSEN");
                scriptBoxEditor.AppendText(space + "WIFI_SPRITE_CHOSEN " + commandList[4] + " = " + commandList[2] + "( " + varString + " );\n");
                break;
            case "ClearFlag":
                var statusFlag = "FALSE";
                if (commandList[4] == "1")
                    statusFlag = "TRUE";
                scriptBoxEditor.AppendText(space + "FLAG " + commandList[3] +
                                           " = " + statusFlag + ";\n");
                break;
            case "ClearVar":
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                scriptBoxEditor.AppendText(space + varString + " = CLEARED;" + "\n");
                break;
            case "ClearTrainerId":
                scriptBoxEditor.AppendText(space + "TRAINER " + commandList[3] + " = FALSE;" + "\n");
                break;
            case "Compare":
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                var varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                var condition = getCondition(scriptsLine, lineCounter);
                scriptBoxEditor.AppendText(space + "If( " + varString + " " + condition + " " + varString2 + ")\n");
                break;
            case "Compare2":
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                condition = getCondition(scriptsLine, lineCounter);
                scriptBoxEditor.AppendText(space + "If( " + varString + " " + condition + " " + varString2 + ")\n");
                break;
            case "CompareTo":
                {
                    varString = getStoredMagic(varNameDictionary, varLevel, null, 0);
                    condition = getCondition(scriptsLine, lineCounter);
                    if (scriptsLine[lineCounter + 5].Contains("Condition") && scriptsLine[lineCounter + 4].Contains("Condition"))
                    {
                        scriptBoxEditor.AppendText(space + "If ( " + varString + " " + condition + " " + commandList[3] + ") ");
                        condition = getCondition(scriptsLine, lineCounter + 4);
                        scriptBoxEditor.AppendText(" " + condition + " ");
                    }
                    else
                        scriptBoxEditor.AppendText(space + "If( " + varString + " " + condition + " " + commandList[3] + ");\n");
                    break;

                }
            case "Condition":
                        break;
            case "CopyVar":
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), varString);
                scriptBoxEditor.AppendText(space + "VAR " + commandList[3] + " = " + varString + ";\n");
                break;
            case "EventGreyMessage":
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + " , TYPE " + commandList[4] + " ");
                readMessage(scriptBoxEditor, commandList, 5);
                scriptBoxEditor.AppendText(" );\n");
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
                    if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[6].TrimStart('(')))
                    {
                        readFunction(scriptsLine, lineCounter, space, scriptBoxEditor, varNameDictionary, varLevel, ref functionLineCounter, ref line2, visitedLine, typeROM, textFile);
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
                        readFunction(scriptsLine, lineCounter, space, scriptBoxEditor, varNameDictionary, varLevel, ref functionLineCounter, ref line2, visitedLine, typeROM, textFile);
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
                        readFunction(scriptsLine, lineCounter, space, scriptBoxEditor, varNameDictionary, varLevel, ref functionLineCounter, ref line2, visitedLine, typeROM, textFile);
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
                    if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[6].TrimStart('(')))
                    {
                        readFunction(scriptsLine, lineCounter, space, scriptBoxEditor, varNameDictionary, varLevel, ref functionLineCounter, ref line2, visitedLine, typeROM, textFile);
                        return;
                    }
                }
                break;
            case "Lock":
                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( OW_ID " + commandList[3] + ");\n");
                break;
            case "Message":
                if (commandList.Length < 5)
                {
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3]);
                }
                else
                {
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
                    varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 6);
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( COSTANT " + commandList[3] + " , COSTANT " + commandList[4] +
                                           " , MESSAGE_ID " + varString + " , OW_ID " + varString2 +
                                               " , VIEW " + commandList[7] + " , TYPE " + commandList[8] + " ");
                    if (varString.Contains("M"))
                        scriptBoxEditor.AppendText(" = " + textFile.messageList[varString.ToCharArray()[varString.Length - 2]] + " ");
                    else
                        scriptBoxEditor.AppendText(" = " + textFile.messageList[Int16.Parse(varString)] + " ");
                }
                scriptBoxEditor.AppendText(" );\n");
                break;
            case "Message2":
                if (commandList.Length < 5)
                {
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3]);
                    scriptBoxEditor.AppendText(" );\n");
                }
                else
                {
                    short index = 0;
                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
                    scriptBoxEditor.AppendText(space + "" + commandList[2] + "( COSTANT " + commandList[3] + " , COSTANT " + commandList[4] +
                                               " , MESSAGE_ID " + varString +
                                               " , VIEW " + commandList[6] + " , TYPE " + commandList[7] + " ");
                    if (varString.Contains("M"))
                        scriptBoxEditor.AppendText(" = " + textFile.messageList[varString.ToCharArray()[varString.Length - 2]] + " ");
                    else if (Int16.TryParse(varString,out index))
                        scriptBoxEditor.AppendText(" = " + textFile.messageList[index] + " ");

                    scriptBoxEditor.AppendText(" );\n");
                }
                break;      
            case "Message3":
                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3]);
                //scriptBoxEditor.AppendText(" = " + textFile.messageList[varString.ToCharArray()[varString.Length - 2]] + " ");
                
                scriptBoxEditor.AppendText(" );\n");
                break;
            case "Multi":
                addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 7), "MULTI_CHOSEN");
                scriptBoxEditor.AppendText(space + "MULTI_CHOSEN " + commandList[7] + " = " + commandList[2] + "(X " + commandList[4] + " , " +
                    "Y " + commandList[5] + " , " +
                    "CURSOR " + commandList[6] +
                    ");\n");
                break;
            case "Multi3":
                addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 7), "MULTI_CHOSEN");
                scriptBoxEditor.AppendText(space + "MULTI_CHOSEN " + commandList[7] + " = " + commandList[2] + "(X " + commandList[4] + " , " +
                    "Y " + commandList[5] + " , " +
                    "CURSOR " + commandList[6] + " , " +
                    "P_4 " + commandList[8] + " , " +
                    "P_5 " + commandList[9] + " , " +
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
            case "SetTextScriptMessage":
                scriptBoxEditor.AppendText(space + commandList[2] + "( BOX_TEXT " + commandList[3] + " , " +
                    "MESSAGEBOX_TEXT  " + commandList[4] + " , " +
                    "SCRIPT " + commandList[5] +
                    ");\n");
                break;
            case "SetTrainerId":
                scriptBoxEditor.AppendText(space + "TRAINER " + commandList[3] + " = TRUE;" + "\n");
                break;
            case "SetValue":
                addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "VALUE");
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                scriptBoxEditor.AppendText(space + "VALUE " + commandList[3] + " = " + varString + "\n");
                break;
            case "SetVar26":
                newVar = checkStored(commandList, 3);
                newVar2 = checkStored(commandList, 4);
                if (!IsNaturalNumber(commandList[4]))
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "NORM_VAR " + commandList[3]);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                scriptBoxEditor.AppendText(space + varString2 + " = " + commandList[4] + "\n");
                break;
            case "SetVarEqVal":
                newVar = checkStored(commandList, 3);
                newVar2 = checkStored(commandList, 4);
                if (!IsNaturalNumber(commandList[4]))
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "NORM_VAR " + commandList[3]);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                scriptBoxEditor.AppendText(space + varString2 + " = " + commandList[4] + "\n");
                break;
            case "SetVarRoutine":
                newVar = checkStored(commandList, 3);
                newVar2 = checkStored(commandList, 4);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ROUT_VAR " + commandList[3]);
                varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                scriptBoxEditor.AppendText(space + varString2 + " = " + varString + ";\n");
                if (IsNaturalNumber(varString))
                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ROUT_VAR " + varString);
                break;
            case "SetVarAlter":
                scriptBoxEditor.AppendText(space + "VAR NAME " + commandList[3] + " = [ALTER]" + ";\n");
                break;
            case "SetVarHero":
                scriptBoxEditor.AppendText(space + "VAR NAME " + commandList[3] + " = [HIRO]" + ";\n");
                break;
            case "SetVarItem":
                scriptBoxEditor.AppendText(space + "VAR ITEM " + commandList[3] + " = " + commandList[4] + ";\n");
                break;
            case "SetVarBattleItem":
                scriptBoxEditor.AppendText(space + "VAR BATTLE ITEM " + commandList[3] + " = " + commandList[4] + ";\n");
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
            case "StartDressPokèmon":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "DRESS_DECISION");
                scriptBoxEditor.AppendText(space + "DRESS_DECISION " + commandList[4] + " = " + commandList[2] + "(" + commandList[3] + " , " + commandList[5] + " );\n");
                break;
            case "StoreBadge":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BADGE_STATUS");
                scriptBoxEditor.AppendText(space + "BADGE_STATUS " + commandList[3] + "= " + commandList[2] + "( BADGE " + commandList[4] + ");\n");
                break;
            case "StoreBadgeNumber":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BADGE_NUMBER");
                scriptBoxEditor.AppendText(space + "BADGE_NUMBER " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreBattleResult":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BATTLE_RESULT");
                scriptBoxEditor.AppendText(space + "BATTLE_RESULT " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreBoxNumber":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "BOX_SPACE");
                scriptBoxEditor.AppendText(space + "BOX_SPACE " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreChosenPokèmon":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "CHOSEN_POKEMON");
                scriptBoxEditor.AppendText(space + "CHOSEN_POKEMON " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreChosenPokèmonTrade":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "TRADE_POKEMON");
                scriptBoxEditor.AppendText(space + "TRADE_POKEMON " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreDoublePhraseBoxInput":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "INSERT_CHECK");
                newVar2 = checkStored(commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "WORD");
                var newVar3 = checkStored(commandList, 6);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "WORD_2");
                scriptBoxEditor.AppendText(space + "INSERT_CHECK " + commandList[4] + ", WORD " + commandList[5] + ", WORD_2 " + commandList[6] + " = " + commandList[2] + "( " + commandList[3] + " );\n");
                break;
            case "StoreDoublePhraseBoxInput2":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "INSERT_CHECK");
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "WORD");
                newVar3 = checkStored(commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "WORD_2");
                var newVar4 = checkStored(commandList, 6);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar4, "WORD_3");
                var newVar5 = checkStored(commandList, 7);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar5, "WORD_4");
                scriptBoxEditor.AppendText(space + "INSERT_CHECK " + commandList[3] + ", WORD " + commandList[4] + ", WORD_2 " + commandList[5] + ", WORD_3 " + commandList[6] + ", WORD_4 " + commandList[7] + " = " + commandList[2] + "();\n");
                break;
            case "StoreFlag":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FLAG " + newVar);
                temp = newVar.ToString();
                break;
            case "StoreFirstTimePokèmonLeague":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "VICTORY_LEAGUE");
                scriptBoxEditor.AppendText(space + "VICTORY_LEAGUE " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreFloor":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FLOOR");
                scriptBoxEditor.AppendText(space + "FLOOR " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreGender":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "GENDER");
                scriptBoxEditor.AppendText(space + "GENDER " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreHeroFaceOrientation":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FACE_ORIENTATION");
                scriptBoxEditor.AppendText(space + "FACE_ORIENTATION " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreHeroFriendCode":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FRIEND_CODE");
                scriptBoxEditor.AppendText(space + "FRIEND_CODE " + commandList[3] + "  = " + commandList[2] + "();\n");
                break;
            case "StoreHeroPosition":
                scriptBoxEditor.AppendText(space + "#DEFINE " + commandList[3] + " AS X ;\n");
                scriptBoxEditor.AppendText(space + "#DEFINE " + commandList[4] + " AS Y ;\n");
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "X");
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "Y");
                scriptBoxEditor.AppendText(space + "X,Y = " + commandList[2] + "();\n");
                break;
            case "StoreItem":
                newVar = checkStored(commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEM_NUMBER");
                scriptBoxEditor.AppendText(space + "ITEM_NUMBER " + commandList[5] + " = " + commandList[2] + "( ITEM " + commandList[3] + " , " + "NUMBER " + commandList[4] + " );\n");
                break;
            case "StoreItemBagNumber":
                newVar = checkStored(commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEMBAG_SPACE");
                scriptBoxEditor.AppendText(space + "ITEM_BAG_SPACE " + commandList[5] + " = " + commandList[2] + "( ITEM " + commandList[3] + " , " + "NUMBER " + commandList[4] + " );\n");
                break;
            case "StorePartyCountMore":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKEMON_COUNT");
                scriptBoxEditor.AppendText(space + "POKEMON_COUNT " + commandList[3] + " = " + commandList[2] + "( LOWER_BOUND " + commandList[4] + " );\n");
                break;
            case "StorePhotoName":
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                scriptBoxEditor.AppendText(space + commandList[2] + "( " + varString + " );\n");
                break;
            case "StorePokèdex":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKEDEX_STATUS");
                scriptBoxEditor.AppendText(space + "POKEDEX_STATUS " + commandList[4] + " = " + commandList[2] + "( POKEDEX " + commandList[3] + ");\n");
                break;
            case "StorePokèLottoNumber":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "LOTTOTICKET_VALUE");
                scriptBoxEditor.AppendText(space + "LOTTOTICKET_VALUE " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StorePokèLottoResults":

                newVar2 = checkStored(commandList, 4);
                newVar3 = checkStored(commandList, 3);
                newVar4 = checkStored(commandList, 5);

                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 6);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "LOTTONUMBER_CHECK");
                addToVarNameDictionary(varNameDictionary, varLevel, newVar4, "LOTTOPOKE_CHECK");
                addToVarNameDictionary(varNameDictionary, varLevel, newVar3, "LOTTOPOKE_ID");
                scriptBoxEditor.AppendText(space + "LOTTOPOKE_ID " + commandList[3] + ", LOTTONUMBER_CHECK " + commandList[4] + ", LOTTOPOKE_CHECK " + commandList[5] + " = " + commandList[2] + "();\n");
                break;
            case "StorePokèmonId":

                newVar = checkStored(commandList, 3);
                newVar2 = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "CHOSEN_POKEMON");
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "CHOSEN_POKEMON");
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                scriptBoxEditor.AppendText(space + "VAR " + commandList[4] + " = StorePokèmonId( " + varString + " );\n");
                break;
            case "StorePokemonPartyAt":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "POKEMON");
                scriptBoxEditor.AppendText(space + "POKEMON " + commandList[3] + " = " + commandList[2] + "( POSITION " + commandList[4] + " );\n");
                break;

            case "StorePokèmonPartyNumber":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PARTY_NUM");
                scriptBoxEditor.AppendText(space + "PARTY_NUM " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StorePokèmonPartyNumber3":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PARTY_NUM");
                scriptBoxEditor.AppendText(space + "PARTY_NUM " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StorePokèmonSex":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "SEX");
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
                scriptBoxEditor.AppendText(space + "SEX " + commandList[3] + " = " + commandList[2] + "( POKE " + varString + ", " + commandList[5] + " );\n");
                break;
            case "StorePokèKronApplicationStatus":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PKRONAPP_STATUS");
                scriptBoxEditor.AppendText(space + "PKRONAPP_STATUS " + commandList[4] + " = " + commandList[2] + "( PKRONAPP " + commandList[3] + ");\n");
                break;
            case "StorePokèKronStatus":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "PKRON_STATUS");
                scriptBoxEditor.AppendText(space + "PKRON_STATUS " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreRandomNumber":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "NUMBER");
                scriptBoxEditor.AppendText(space + "NUMBER " + commandList[3] + " = " + commandList[2] + "( " + commandList[4] + " );\n");
                break;
            case "StoreSinglePhraseBoxInput":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "INSERT_CHECK");
                newVar2 = checkStored(commandList, 5);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, "WORD");
                scriptBoxEditor.AppendText(space + "INSERT_CHECK " + commandList[4] + ", WORD " + commandList[5] + " = " + commandList[2] + "( " + commandList[3] + " );\n");
                break;
            case "StoreStarter":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "STARTER");
                scriptBoxEditor.AppendText(space + "STARTER  " + commandList[3] + " = " + commandList[2] + "();\n");
                break;
            case "StoreTrainerID":
                newVar = checkStored(commandList, 4);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "TRAINER_" + commandList[3]);
                scriptBoxEditor.AppendText(space + "VAR " + commandList[4] + "  = TRAINER_" + commandList[3]+ ";\n");
                break;
            case "StoreVar":
                newVar = checkStored(commandList, 3);
                //addToVarNameDictionary(varNameDictionary, varLevel, newVar, "VAR 0x" + newVar.ToString("X"));
                var next = scriptsLine[lineCounter + 1];
                if (scriptsLine[lineCounter + 1].Contains("StoreVar") && scriptsLine[lineCounter + 1].Contains("CompareTo"))
                {
                    temp2 = next.Split(' ')[1];
                    lineCounter+=2;
                    condition = getCondition(scriptsLine, lineCounter);
                    scriptBoxEditor.AppendText(space + "If( " + temp + " " + condition + " " + temp2 + ")\n");
                }
                temp = newVar.ToString();
                break;
            case "StoreVarMessage":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MESSAGE_" + commandList[4]);
                scriptBoxEditor.AppendText(space + "VAR " + commandList[3] + " = MESSAGE_" + commandList[4] + ";\n");
                break;
            case "StoreVersion":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "VERSION");
                scriptBoxEditor.AppendText(space + "VERSION " + commandList[3] + "= " + commandList[2] + "();\n");
                break;
            case "TradePokèmon":
                newVar = checkStored(commandList, 3);
                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
                scriptBoxEditor.AppendText(space + commandList[2] + "( " + varString + " );\n");
                break;
            case "YesNoBox":
                newVar = checkStored(commandList, 3);
                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "YESNO_RESULT");
                scriptBoxEditor.AppendText(space + "YESNO_RESULT  " + commandList[3] + " = " + commandList[2] + "();\n");
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
                    scriptBoxEditor.AppendText(" " + commandList[commandList.Length - 2]);
                scriptBoxEditor.AppendText(");\n");
                break;
        }

    }

    private static void readMessage(RichTextBox scriptBoxEditor, string[] commandList, int start)
    {
        if (commandList.Length > start -1)
            for (int i = start; i < commandList.Length; i++)
                scriptBoxEditor.AppendText(commandList[i] + " ");
    }

    private static void readFunction(string[] scriptsLine, int lineCounter, string space, RichTextBox scriptBoxEditor, List<Dictionary<int, string>> varNameDictionary, int varLevel, ref int functionLineCounter, ref string line2, List<int> visitedLine,int typeROM, Texts textFile)
    {
        functionLineCounter++;
        if (visitedLine.Contains(functionLineCounter)) //&& functionLineCounter < lineCounter)
        {
            scriptBoxEditor.AppendText("       " + space + "Jump to Offset: " + scriptsLine[lineCounter].Split(' ')[6].TrimStart('(') + "\n");
            scriptBoxEditor.AppendText(space + "}\n");
            return;
        }
        visitedLine.Add(functionLineCounter);
        readFunctionSimplified(scriptsLine, space, scriptBoxEditor, varNameDictionary, varLevel, ref functionLineCounter, ref line2,visitedLine,typeROM, textFile);
        scriptBoxEditor.AppendText(space + "}\n");
        return;
    }

    private static void readFunctionSimplified(string[] scriptsLine, string space, RichTextBox scriptBoxEditor, List<Dictionary<int, string>> varNameDictionary, int varLevel, ref int functionLineCounter, ref string line2,List<int> visitedLine,int typeROM, Texts textFile)
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
                    if (typeROM == 3 || typeROM == 4)
                        Utils.getCommandSimplifiedBW(scriptsLine, functionLineCounter, space + "   ", scriptBoxEditor, varNameDictionary, varLevel + 1, visitedLine, 3, textFile);
                    else if (typeROM == 2)
                        Utils.getCommandSimplifiedHGSS(scriptsLine, functionLineCounter, space + "   ", scriptBoxEditor, varNameDictionary, varLevel + 1, visitedLine, 2, textFile);
                    else
                        Utils.getCommandSimplifiedDPP(scriptsLine, functionLineCounter, space + "   ", scriptBoxEditor, varNameDictionary, varLevel + 1, visitedLine, 0, textFile);
                }
                catch
                {
                }
            }
            functionLineCounter++;
            if ((typeROM == 3 || typeROM == 4))
            {
                if (line2.StartsWith("=") || line2.Contains(" Jump ") || line2.Contains("EndScript") || line2.Contains("EndFunction"))
                    return;
                if (line2.Contains("End") && (scriptsLine[functionLineCounter] == "" || scriptsLine[functionLineCounter].Contains("Offset ")))
                    return;
            }
            if ((typeROM == 0 || typeROM == 1) && (line2.Contains(" End ") || line2.Contains("KillScript") || (line2.Contains("Jump") && scriptsLine[functionLineCounter] == "")))
                return;
            if ((typeROM == 2) && (line2.Contains(" End ") || line2.Contains("KillScript") || (line2.Contains("Jump"))))// && scriptsLine[functionLineCounter] == "")))
                return;

        } while (functionLineCounter < scriptsLine.Length);

    }

    private static string getCondition(string[] scriptsLine, int lineCounter)
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

    private static string getStoredMagic(List<Dictionary<int, string>> varNameDictionary, int varLevel, string[] commandList, int id)
    {
        int var = 0;
        if (id == null)
            var = Int32.Parse(temp);
        else
            var = checkStored(commandList, id);
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
            {
                varString = nameDictionary[var];
                return varString;
            }
        }
        for (int i = varLevel; i >= 0; i--)
        {
            var nameDictionary = varNameDictionary[i];
            if (nameDictionary.ContainsKey(var))
            {
                varString = nameDictionary[var];
                return varString;
            }
        }
        return varString;
    }

    private static int checkStored(string[] commandList, int id)
    {
        if (commandList== null ||id>commandList.Length)
            return 0;
        var oldVar2 = 0;
        if (commandList[id].Contains("0x"))
            oldVar2 = Int32.Parse(commandList[id].Substring(2, commandList[id].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
        else
            oldVar2 = Int32.Parse(commandList[id]);
        return oldVar2;
    }

    private static void addToVarNameDictionary(List<Dictionary<int, string>> varNameDictionary, int varLevel, int newVar, string name)
    {
        if (varNameDictionary[varLevel].ContainsKey(newVar))
            varNameDictionary[varLevel].Remove(newVar);
        varNameDictionary[varLevel].Add(newVar, name);
    }

    //internal static void getCommandSimplifiedBW2(string[] scriptsLine, int lineCounter, string space, RichTextBox scriptBoxEditor, List<Dictionary<int, string>> varNameDictionary, int varLevel)
    //{
    //    var line = scriptsLine[lineCounter];
    //    var commandList = line.Split(' ');
    //    string movId;
    //    string tipe;
    //    string functionID;
    //    switch (commandList[2])
    //    {

    //        case "238":
    //            scriptBoxEditor.AppendText(space + "VAR " + commandList[4] + " = " + commandList[2] + "(P_1 " + commandList[3] + ");\n");
    //            break;
    //        case "2D1":
    //            scriptBoxEditor.AppendText(space + "VAR " + commandList[3] + " = " + commandList[2] + "();\n");
    //            break;
    //        case "AddPeople":
    //            scriptBoxEditor.AppendText(space + "" + commandList[2] + "( OW_ID " + commandList[3] + ");\n");
    //            break;
    //        case "ApplyMovement":
    //            scriptBoxEditor.AppendText(space + "" + commandList[2] + "( OW_ID " + commandList[3] + ");\n");
    //            scriptBoxEditor.AppendText(space + "{\n");
    //            movId = commandList[6];
    //            tipe = commandList[5];
    //            for (var functionLineCounter = 0; functionLineCounter < scriptsLine.Length; functionLineCounter++)
    //            {
    //                var line2 = scriptsLine[functionLineCounter];
    //                if (line2.Contains("= " + tipe + " " + movId + " "))
    //                {
    //                    functionLineCounter++;
    //                    do
    //                    {
    //                        line2 = scriptsLine[functionLineCounter];
    //                        var movList = line2.Split(' ');
    //                        if (line2.Length > 1)
    //                            scriptBoxEditor.AppendText(space + " MOV " + movList[2].ToString().TrimStart('m') + " TIMES " + movList[3] + "\n");
    //                        functionLineCounter++;
    //                    } while (!line2.Contains("m254"));
    //                }
    //                //if (functionLineCounter > scriptsLine.Length || line2.Contains("End") || line2.Contains("KillScript"))
    //                //    goto EndIf;
    //            }
    //            scriptBoxEditor.AppendText(space + "}\n");
    //            break;
    //        case "CallMessageBox":
    //            scriptBoxEditor.AppendText(space + "BORDER " + commandList[5] + " = " + commandList[2] + "( MESSAGE_ID " + commandList[3] +
    //                                       ", TYPE " + commandList[4] + " ");
    //            if (commandList.Length > 5)
    //                for (int i = 6; i < commandList.Length; i++)
    //                    scriptBoxEditor.AppendText(commandList[i] + " ");
    //            scriptBoxEditor.AppendText(" );\n");
    //            break;
    //        case "ChangeOwPosition":
    //            scriptBoxEditor.AppendText(space + "" + commandList[2] + "( OW_ID " + commandList[3] +
    //                                       ", X " + commandList[4] + ", Y " + commandList[5] + ");\n");
    //            break;
    //        case "ChangeOwPosition2":
    //            scriptBoxEditor.AppendText(space + "" + commandList[2] + "( OW_ID " + commandList[3] +
    //                                       ", P_2 " + commandList[4] + ");\n");
    //            break;
    //        case "ClearFlag":
    //            var statusFlag = "FALSE";
    //            if (commandList[4] == "1")
    //                statusFlag = "TRUE";
    //            scriptBoxEditor.AppendText(space + "FLAG " + commandList[3] +
    //                                       " = " + statusFlag + ";\n");
    //            break;
    //        case "Compare":
    //            var oldVar = Int32.Parse(commandList[3].Substring(2, commandList[3].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
    //            var varString = commandList[3];
    //            if (varNameDictionary.ContainsKey(oldVar))
    //                varString = varNameDictionary[oldVar];
    //            var condition = scriptsLine[lineCounter + 1].Split(' ')[3];
    //            if (condition == "EQUAL")
    //                condition = "==";
    //            if (condition == "BIGGER")
    //                condition = ">";
    //            if (condition == "LOWER")
    //                condition = "<";
    //            if (condition == "BIGGER/EQUAL")
    //                condition = ">=";
    //            if (condition == "LOWER/EQUAL")
    //                condition = "<=";
    //            if (condition == "DIFFERENT")
    //                condition = "!=";
    //            scriptBoxEditor.AppendText(space + "If( " + varString + " " + condition + " " + commandList[4] + ")\n");
    //            break;
    //        case "CompareTo":
    //            varString = temp;
    //            if (varNameDictionary.ContainsKey(Int16.Parse(temp)))
    //                varString = varNameDictionary[Int16.Parse(temp)];
    //            int conditionCounter = lineCounter;
    //            //while (scriptsLine[conditionCounter + 1].Split(' ')[2] == "Condition")
    //            //{
    //                condition = scriptsLine[conditionCounter + 1].Split(' ')[3];
    //                if (condition == "EQUAL")
    //                    condition = "==";
    //                if (condition == "BIGGER")
    //                    condition = ">";
    //                if (condition == "LOWER")
    //                    condition = "<";
    //                if (condition == "BIGGER/EQUAL")
    //                    condition = ">=";
    //                if (condition == "LOWER/EQUAL")
    //                    condition = "<=";
    //                if (condition == "DIFFERENT")
    //                    condition = "!=";
    //                scriptBoxEditor.AppendText(space + "If( " + varString + " " + condition + " " + commandList[3] + ") ");
    //            //    conditionCounter++;
    //            //    lineCounter++;
    //            //}
    //            scriptBoxEditor.AppendText("\n");
    //            break;
    //        case "Condition":
    //            break;
    //        case "CopyVar":
    //            oldVar = Int32.Parse(commandList[4].Substring(2, commandList[3].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
    //            var newVar = Int32.Parse(commandList[3].Substring(2, commandList[3].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
    //            varString = varNameDictionary[oldVar];
    //            varNameDictionary.Add(newVar, varString);
    //            scriptBoxEditor.AppendText(space + "VAR " + commandList[3] + " = " + varString + ";\n");
    //            break;
    //        case "Fanfare":
    //            scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MUSIC_ID " + commandList[3] + ");\n");
    //            break;
    //        case "Goto":
    //            scriptBoxEditor.AppendText(space + "Goto\n" + space + "{\n");
    //            var functionId = commandList[5];
    //            for (var functionLineCounter = 0; functionLineCounter < scriptsLine.Length; functionLineCounter++)
    //            {
    //                var line2 = scriptsLine[functionLineCounter];
    //                if (line2.Contains("= Function " + functionId + " "))
    //                {
    //                    functionLineCounter++;
    //                    do
    //                    {
    //                        line2 = scriptsLine[functionLineCounter];
    //                        if (line2.Length > 1)
    //                            Utils.getCommandSimplifiedBW2(scriptsLine, functionLineCounter, space + "   ", scriptBoxEditor, varNameDictionary);

    //                        functionLineCounter++;
    //                    } while (!line2.Contains("Goto") && !line2.Contains("Jump") && !line2.Contains("End") && !line2.Contains("KillScript") && functionLineCounter < scriptsLine.Length);
    //                }
    //                //if (functionLineCounter > scriptsLine.Length || line2.Contains("End") || line2.Contains("KillScript"))
    //                //    goto EndIf;
    //            }
    //            scriptBoxEditor.AppendText(space + "}\n");
    //            break;
    //        case "If":
    //            scriptBoxEditor.AppendText(space + "{\n");
    //            functionId = commandList[5];
    //            var type = commandList[4];
    //            for (var functionLineCounter = 0; functionLineCounter < scriptsLine.Length - 1; functionLineCounter++)
    //            {
    //                var line2 = scriptsLine[functionLineCounter];
    //                if (//line2.Contains("= " + type + " " + functionId + " ") || 
    //                    scriptsLine[functionLineCounter +1].Contains("Offset: " + commandList[6].TrimStart('(')))
    //                {
    //                    functionLineCounter++;
    //                    do
    //                    {
    //                        line2 = scriptsLine[functionLineCounter];
    //                        if (line2.Length > 1)
    //                            getCommandSimplifiedBW2(scriptsLine, functionLineCounter, space + "   ", scriptBoxEditor, varNameDictionary);

    //                        functionLineCounter++;
    //                    } while (!line2.Contains("Goto") && !line2.Contains("Jump") && !line2.Contains("End") && !line2.Contains("KillScript") && functionLineCounter < scriptsLine.Length);
    //                }
    //                //if (functionLineCounter > scriptsLine.Length || line2.Contains("End") || line2.Contains("KillScript"))
    //                //    goto EndIf;
    //            }
    //            scriptBoxEditor.AppendText(space + "}\n");
    //            break;
    //        case "If2":
    //            scriptBoxEditor.AppendText(space + "{\n");
    //            functionId = commandList[5];
    //            type = commandList[4];
    //            for (var functionLineCounter = 0; functionLineCounter < scriptsLine.Length; functionLineCounter++)
    //            {
    //                var line2 = scriptsLine[functionLineCounter];
    //                if (line2.Contains("= " + type + " " + functionId + " "))
    //                {
    //                    functionLineCounter++;
    //                    do
    //                    {
    //                        line2 = scriptsLine[functionLineCounter];
    //                        if (line2.Length > 1)
    //                            getCommandSimplifiedBW2(scriptsLine, functionLineCounter, space + "   ", scriptBoxEditor, varNameDictionary);

    //                        functionLineCounter++;
    //                    } while (!line2.Contains("End") && !line2.Contains("KillScript") && functionLineCounter < scriptsLine.Length);
    //                }
    //                //if (functionLineCounter > scriptsLine.Length || line2.Contains("End") || line2.Contains("KillScript"))
    //                //    goto EndIf;
    //            }
    //            scriptBoxEditor.AppendText(space + "}\n");
    //            break;
    //        case "Jump":
    //            scriptBoxEditor.AppendText(space + "Jump\n" + space + "{\n");
    //            functionId = commandList[5];
    //            for (var functionLineCounter = 0; functionLineCounter < scriptsLine.Length; functionLineCounter++)
    //            {
    //                var line2 = scriptsLine[functionLineCounter];
    //                if (line2.Contains("= Function " + functionId + " "))
    //                {
    //                    functionLineCounter++;
    //                    do
    //                    {
    //                        line2 = scriptsLine[functionLineCounter];
    //                        if (line2.Length > 1)
    //                            getCommandSimplifiedBW2(scriptsLine, functionLineCounter, space + "   ", scriptBoxEditor, varNameDictionary);

    //                        functionLineCounter++;
    //                    } while (!line2.Contains("Jump") && !line2.Contains("End") && !line2.Contains("KillScript") && functionLineCounter < scriptsLine.Length);
    //                }
    //                //if (functionLineCounter > scriptsLine.Length || line2.Contains("End") || line2.Contains("KillScript"))
    //                //    goto EndIf;
    //            }
    //            scriptBoxEditor.AppendText(space + "}\n");
    //            break;
    //        case "Lock":
    //            scriptBoxEditor.AppendText(space + "" + commandList[2] + "( OW_ID " + commandList[3] + ");\n");
    //            break;
    //        case "Message2":
    //            scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3]);
    //            if (commandList.Length > 4)
    //                for (int i = 5; i < commandList.Length; i++)
    //                    scriptBoxEditor.AppendText(commandList[i] + " ");
    //            scriptBoxEditor.AppendText(" );\n");
    //            break;
    //        case "Multi3":
    //            newVar = Int32.Parse(commandList[7].Substring(2, commandList[7].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
    //            if (varNameDictionary.ContainsKey(newVar))
    //                varNameDictionary.Remove(newVar);
    //            varNameDictionary.Add(newVar, "MULTI_CHOSEN ");
    //            scriptBoxEditor.AppendText(space + "MULTI_CHOSEN " + commandList[7] + " = " + commandList[2] + "(X " + commandList[4] + " , " +
    //                "Y " + commandList[5] + " , " +
    //                "CURSOR " + commandList[6] + " , " +
    //                "P_4 " + commandList[8] + " , " +
    //                "P_5 " + commandList[9] + " , " +
    //                ");\n");
    //            break;
    //        case "ReleaseOw":
    //            scriptBoxEditor.AppendText(space + commandList[2] + "( OW_ID " + commandList[3] +
    //                                       ", P_2 " + commandList[4] + ");\n");
    //            break;

    //        case "RemovePeople":
    //            scriptBoxEditor.AppendText(space + "RemovePeople( OW_ID " + commandList[3] + ");\n");
    //            break;
    //        case "SetFlag":
    //            scriptBoxEditor.AppendText(space + "FLAG " + commandList[3] + " = TRUE;" + "\n");
    //            break;
    //        case "SetTextScriptMessageMulti":
    //            scriptBoxEditor.AppendText(space + commandList[2] + "( BOX_TEXT " + commandList[3] + " , " +
    //                "MESSAGEBOX_TEXT  " + commandList[4] + " , " +
    //                "SCRIPT " + commandList[5] +
    //                ");\n");
    //            break;
    //        case "SetValue":
    //            newVar = Int32.Parse(commandList[3].Substring(2, commandList[3].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
    //            if (varNameDictionary.ContainsKey(newVar))
    //                varNameDictionary.Remove(newVar);
    //            varNameDictionary.Add(newVar, "VALUE");
    //            scriptBoxEditor.AppendText(space + "VALUE " + commandList[3] + " = " + commandList[4] + "\n");
    //            break;
    //        case "SetVarAlter":
    //            scriptBoxEditor.AppendText(space + "VAR NAME " + commandList[3] + " = [ALTER]" + ";\n");
    //            break;
    //        case "SetVarHero":
    //            scriptBoxEditor.AppendText(space + "VAR NAME " + commandList[3] + " = [HIRO]" + ";\n");
    //            break;
    //        case "SetVarNumber":
    //            oldVar = Int32.Parse(commandList[4].Substring(2, commandList[4].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
    //            scriptBoxEditor.AppendText(space + "VAR NUM " + commandList[3] + " = " + varNameDictionary[oldVar] + ";\n");
    //            break;
    //        case "SetVarRival":
    //            scriptBoxEditor.AppendText(space + "VAR NAME " + commandList[3] + " = [RIVAL]" + ";\n");
    //            break;
    //        case "StoreBadge":
    //            newVar = Int32.Parse(commandList[4].Substring(2, commandList[4].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
    //            if (varNameDictionary.ContainsKey(newVar))
    //                varNameDictionary.Remove(newVar);
    //            varNameDictionary.Add(newVar, "BADGE_STATUS");
    //            scriptBoxEditor.AppendText(space + "BADGE_STATUS " + commandList[4] + "= " + commandList[2] + "( BADGE " + commandList[3] + ");\n");
    //            break;
    //        case "StoreBadgeNumber":
    //            newVar = Int32.Parse(commandList[3].Substring(2, commandList[3].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
    //            if (varNameDictionary.ContainsKey(newVar))
    //                varNameDictionary.Remove(newVar);
    //            varNameDictionary.Add(newVar, "BADGE_NUMBER");
    //            scriptBoxEditor.AppendText(space + "BADGE_NUMBER " + commandList[3] + " = " + commandList[2] + "();\n");
    //            break;
    //        case "StoreFlag":
    //               newVar = Int32.Parse(commandList[3].Substring(2, commandList[3].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
    //            if (varNameDictionary.ContainsKey(newVar))
    //                varNameDictionary.Remove(newVar);
    //            varNameDictionary.Add(newVar, "FLAG " + commandList[3]);
    //            temp = commandList[3].Substring(2, commandList[3].Length - 2);
    //            break;
    //        case "StoreGender":
    //            newVar = Int32.Parse(commandList[3].Substring(2, commandList[3].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
    //            if (varNameDictionary.ContainsKey(newVar))
    //                varNameDictionary.Remove(newVar);
    //            varNameDictionary.Add(newVar, "GENDER");
    //            scriptBoxEditor.AppendText(space + "GENDER " + commandList[3] + "  = " + commandList[2] + "();\n");
    //            break;
    //        case "StoreHeroPosition":
    //            scriptBoxEditor.AppendText(space + "#DEFINE " + commandList[3] + " AS X ;\n");
    //            scriptBoxEditor.AppendText(space + "#DEFINE " + commandList[4] + " AS Y ;\n");
    //            newVar = Int32.Parse(commandList[3].Substring(2, commandList[3].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
    //            var newVar2 = Int32.Parse(commandList[4].Substring(2, commandList[4].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
    //            if (varNameDictionary.ContainsKey(newVar))
    //                varNameDictionary.Remove(newVar);
    //            varNameDictionary.Add(newVar, "X");
    //            if (varNameDictionary.ContainsKey(newVar2))
    //                varNameDictionary.Remove(newVar2);
    //            varNameDictionary.Add(newVar2, "Y");
    //            scriptBoxEditor.AppendText(space + "X,Y = " + commandList[2] + "();\n");
    //            break;
    //        case "StoreItem":
    //            newVar = Int32.Parse(commandList[5].Substring(2, commandList[5].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
    //            if (varNameDictionary.ContainsKey(newVar))
    //                varNameDictionary.Remove(newVar);
    //            varNameDictionary.Add(newVar, "ITEM_NUMBER");
    //            scriptBoxEditor.AppendText(space + "ITEM_NUMBER " + commandList[5] + " = " + commandList[2] + "( ITEM " + commandList[3] + " , " + "NUMBER " + commandList[4] + " );\n");
    //            break;
    //        case "StorePokèmonPartyNumber3":
    //            newVar = Int32.Parse(commandList[3].Substring(2, commandList[3].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
    //            if (varNameDictionary.ContainsKey(newVar))
    //                varNameDictionary.Remove(newVar);
    //            varNameDictionary.Add(newVar, "PARTY_NUM");
    //            scriptBoxEditor.AppendText(space + "PARTY_NUM " + commandList[3] + " = " + commandList[2] + "();\n");
    //            break;
    //        case "StorePokèKronApplicationStatus":
    //            newVar = Int32.Parse(commandList[4].Substring(2, commandList[4].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
    //            if (varNameDictionary.ContainsKey(newVar))
    //                varNameDictionary.Remove(newVar);
    //            varNameDictionary.Add(newVar, "PKRONAPP_STATUS");
    //            scriptBoxEditor.AppendText(space + "PKRONAPP__STATUS " + commandList[4] + " = " + commandList[2] + "( PKRONAPP " + commandList[3] + ");\n");
    //            break;
    //        case "StoreStarter":
    //            newVar = Int32.Parse(commandList[3].Substring(2, commandList[3].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
    //            if (varNameDictionary.ContainsKey(newVar))
    //                varNameDictionary.Remove(newVar);
    //            varNameDictionary.Add(newVar, "STARTER");
    //            scriptBoxEditor.AppendText(space + "STARTER  " + commandList[3] + " = " + commandList[2] + "();\n");
    //            break;
    //        case "StoreVar":
    //            newVar = Int32.Parse(commandList[3].Substring(2, commandList[3].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
    //            if (varNameDictionary.ContainsKey(newVar))
    //                varNameDictionary.Remove(newVar);
    //            varNameDictionary.Add(newVar, "VAR 0x" + commandList[3]);
    //            temp = commandList[3].Substring(2, commandList[3].Length - 2);
    //            //scriptBoxEditor.AppendText(space + "VAR TEMP = " + commandList[3] + ";\n");
    //            break;
    //        case "YesNoBox":
    //            newVar = Int32.Parse(commandList[3].Substring(2, commandList[3].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
    //            if (varNameDictionary.ContainsKey(newVar))
    //                varNameDictionary.Remove(newVar);
    //            varNameDictionary.Add(newVar, "YESNO_RESULT");
    //            scriptBoxEditor.AppendText(space + "YESNO_RESULT  " + commandList[3] + " = " + commandList[2] + "x();\n");
    //            break;

    //        default:
    //            //for (int i = 3; i < commandList.Length ; i++)
    //            //    if (commandList[i]!="")
    //            //        scriptBoxEditor.AppendText(space + "P_" + (i - 2) + " = " + commandList[i] + ";\n");
    //            scriptBoxEditor.AppendText(space + commandList[2] + "(");
    //            for (int i = 3; i < commandList.Length - 1; i++)
    //                if (commandList[i] != "")
    //                    scriptBoxEditor.AppendText(" P_" + (i - 2) + " " + commandList[i] + " ,");
    //            if (commandList[commandList.Length - 1] != "")
    //                scriptBoxEditor.AppendText(" P_" + (commandList.Length - 3) + " " + commandList[commandList.Length - 1]);
    //            scriptBoxEditor.AppendText(");\n");
    //            break;
    //    }

    //}
}
  

