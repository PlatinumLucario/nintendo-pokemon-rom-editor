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




    //internal static void getCommandSimplifiedBW2(string[] scriptsLine, ref int lineCounter, string space, List<int> visitedLine)
    //{
    //    var line = scriptsLine[lineCounter];
    //    var commandList = line.Split(' ');
    //    string movId;
    //    string tipe;
    //    //scriptBoxEditor.AppendText(commandList[1] + " ");
    //    switch (commandList[2])
    //    {

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
    //        case "AngryMessage":
    //            var varString2 = "";
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
    //                        scriptBoxEditor.AppendText(space + "ANGRY_MESSAGE " + commandList[3] + " = '" + textFile.messageList[Int16.Parse(varString)] + " ';\n");
    //                        varString = commandList[3].ToString();
    //                    }
    //                }
    //                else if (Int16.TryParse(varString, out index))
    //                    scriptBoxEditor.AppendText(space + "ANGRY_MESSAGE_" + varString + " = ' " + textFile.messageList[Int16.Parse(varString)] + " ';\n");
    //                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + varString + " , COLOR " + commandList[4] + ", " + commandList[5] + " );\n");
    //            }
    //            break;
    //        case "ApplyMovement":
    //            scriptBoxEditor.AppendText(space + "" + commandList[2] + "( OW_ID " + commandList[3] + ");\n");
    //            scriptBoxEditor.AppendText(space + "{\n");
    //            movId = commandList[6];
    //            tipe = commandList[5];
    //            for (var functionLineCounter = 0; functionLineCounter < scriptsLine.Length; functionLineCounter++)
    //            {
    //                var line2 = scriptsLine[functionLineCounter];
    //                if (commandList.Length < 8)
    //                {
    //                    var offset2 = commandList[5].TrimStart('0').TrimStart('x');
    //                    int offset = int.Parse(offset2, System.Globalization.NumberStyles.HexNumber);
    //                    if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + offset))
    //                    {
    //                        functionLineCounter++;
    //                        do
    //                        {
    //                            line2 = scriptsLine[functionLineCounter];
    //                            var movList = line2.Split(' ');
    //                            if (line2.Length > 1)
    //                                scriptBoxEditor.AppendText(space + " " + movList[2].ToString().TrimStart('m') + " TIMES " + movList[3] + "\n");
    //                            functionLineCounter++;
    //                        } while (!line2.Contains("End_Movement") && functionLineCounter + 1 < scriptsLine.Length);
    //                        scriptBoxEditor.AppendText(space + "}\n");
    //                        return;
    //                    }
    //                }
    //                else if (commandList.Length > 8 && functionLineCounter + 1 < scriptsLine.Length && scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[7].TrimStart('(')))
    //                {
    //                    functionLineCounter++;
    //                    do
    //                    {
    //                        line2 = scriptsLine[functionLineCounter];
    //                        var movList = line2.Split(' ');
    //                        if (line2.Length > 1)
    //                            scriptBoxEditor.AppendText(space + " " + movList[2].ToString().TrimStart('m') + " " + movList[3].TrimStart('0').TrimStart('x') + "\n");
    //                        functionLineCounter++;
    //                    } while (!line2.Contains("End_Movement") && functionLineCounter + 1 < scriptsLine.Length);
    //                    scriptBoxEditor.AppendText(space + "}\n");
    //                    return;
    //                }


    //            }

    //            break;
    //        case "BorderedMessage":
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
    //            scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + " , COLOR " + commandList[4] + " ");
    //            readMessage(scriptBoxEditor, commandList, 5);
    //            scriptBoxEditor.AppendText(" );\n");
    //            break;
    //        case "BorderedMessage2":
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
    //            scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + " , COLOR " + commandList[4] + " ");
    //            readMessage(scriptBoxEditor, commandList, 5);
    //            scriptBoxEditor.AppendText(" );\n");
    //            break;
    //        case "BubbleMessage":
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
    //                        scriptBoxEditor.AppendText(space + "BUBBLE_MESSAGE " + commandList[3] + " = '" + textFile.messageList[Int16.Parse(varString)] + " ';\n");
    //                        varString = commandList[3].ToString();
    //                    }
    //                }
    //                else if (Int16.TryParse(varString, out index))
    //                    scriptBoxEditor.AppendText(space + "BUBBLE_MESSAGE_" + varString + " = ' " + textFile.messageList[Int16.Parse(varString)] + " ';\n");
    //                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + varString + " , COLOR " + commandList[4] + " );\n");
    //            }
    //            break;
    //        case "CallMessageBox":
    //            scriptBoxEditor.AppendText(space + "BORDER " + commandList[5] + " = " + commandList[2] + "( MESSAGE_ID " + commandList[3] +
    //                                       ", TYPE " + commandList[4] + " ");
    //            if (commandList.Length > 5)
    //                for (int i = 6; i < commandList.Length; i++)
    //                    scriptBoxEditor.AppendText(commandList[i] + " ");
    //            scriptBoxEditor.AppendText(" );\n");
    //            break;
    //        case "CallRoutine":
    //            scriptBoxEditor.AppendText(space + "CallRoutine( " + commandList[3] + ") \n" + space + "{\n");
    //            //functionId = commandList[5];
    //            varNameDictionary.Add(new Dictionary<int, string>());
    //            for (var functionLineCounter = 0; functionLineCounter < scriptsLine.Length - 1; functionLineCounter++)
    //            {
    //                var line2 = scriptsLine[functionLineCounter];
    //                if (commandList.Length < 7)
    //                {
    //                    int offset = 0;
    //                    if (commandList[3].Contains("0x"))
    //                    {
    //                        var offset2 = commandList[3].TrimStart('0').TrimStart('x');
    //                        offset = int.Parse(offset2, System.Globalization.NumberStyles.HexNumber);
    //                    }
    //                    else
    //                    {
    //                        offset = int.Parse(commandList[3]);
    //                    }
    //                    if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + offset))
    //                    {
    //                        varLevel++;
    //                        readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
    //                        varLevel--;
    //                        return;
    //                    }
    //                }
    //                else if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[6].TrimStart('(')))
    //                {
    //                    varLevel++;
    //                    readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
    //                    varLevel--;
    //                    return;
    //                }
    //            }
    //            break;
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
    //        case "CheckEnoughMoney":
    //            newVar = checkStored(commandList, 3);
    //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "HAVE_MONEY", "BOL");
    //            scriptBoxEditor.AppendText(space + "HAVE_MONEY " + commandList[3] + " = " + commandList[2] + "( AMOUNT " + commandList[4] + " );\n");
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
    //        case "CheckItemBagSpace":
    //            newVar = checkStored(commandList, 5);
    //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEMBAG_SPACE", "BOL");
    //            newVar2 = checkStored(commandList, 3);
    //            varString = commandList[3];
    //            if (IsNaturalNumber(varString))
    //                varString = getTextFromCondition(commandList[3], "ITE");
    //            else
    //                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, commandList[3], "ITE");
    //            scriptBoxEditor.AppendText(space + "ITEMBAG_SPACE " + commandList[5] + " = " + commandList[2] + "( ITEM " + varString + " , " + "NUMBER " + commandList[4] + " );\n");
    //            break;
    //        case "CheckItemBagNumber":
    //            newVar = checkStored(commandList, 5);
    //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "ITEMBAG_NUMBER", "BOL");
    //            varString = commandList[3];
    //            if (IsNaturalNumber(varString))
    //                varString = getTextFromCondition(commandList[3], "ITE");
    //            else
    //                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, commandList[3], "ITE");
    //            scriptBoxEditor.AppendText(space + "ITEMBAG_NUMBER " + commandList[5] + " = " + commandList[2] + "( ITEM " + varString + " , " + "NUMBER " + commandList[4] + " );\n");
    //            break;
    //        case "CheckItemContainer":
    //            newVar = checkStored(commandList, 4);
    //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "IS_CONTAINER", "BOL");
    //            varString = commandList[3];
    //            if (IsNaturalNumber(varString))
    //                varString = getTextFromCondition(commandList[3], "ITE");
    //            else
    //                addToVarNameDictionary(varNameDictionary, varLevel, newVar2, commandList[3], "ITE");
    //            scriptBoxEditor.AppendText(space + "IS_CONTAINER " + commandList[4] + " = " + commandList[2] + "( ITEM " + varString + " );\n");
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
    //        case "ClearFlag":
    //            var statusFlag = "FALSE";
    //            if (commandList[4] == "1")
    //                statusFlag = "TRUE";
    //            scriptBoxEditor.AppendText(space + "FLAG " + getTextFromCondition(commandList[3], "FLA") +
    //                                       " = " + statusFlag + ";\n");
    //            break;
    //        case "ClearVar":
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
    //            scriptBoxEditor.AppendText(space + "VAR " + varString + " = CLEARED;" + "\n");
    //            break;
    //        case "ClearTrainerId":
    //            scriptBoxEditor.AppendText(space + "TRAINER " + commandList[3] + " = FALSE;" + "\n");
    //            break;
    //        case "Compare":
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
    //            varString2 = getTextFromCondition(getStoredMagic(varNameDictionary, varLevel, commandList, 4), cond);
    //            var condition = getCondition(scriptsLine, lineCounter);
    //            scriptBoxEditor.AppendText(space + "If( " + varString + " " + condition + " " + varString2 + " )\n");
    //            break;
    //        case "Compare2":
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
    //            varString2 = getTextFromCondition(getStoredMagic(varNameDictionary, varLevel, commandList, 4), cond);
    //            condition = getCondition(scriptsLine, lineCounter);
    //            scriptBoxEditor.AppendText(space + "If( " + varString + " " + condition + " " + varString2 + ")\n");
    //            break;
    //        case "CompareTo":
    //            {
    //                varString = getStoredMagic(varNameDictionary, varLevel, null, 0);
    //                condition = getCondition(scriptsLine, lineCounter);
    //                var text = getTextFromCondition(commandList[3], cond);
    //                if (scriptsLine[lineCounter + 5].Contains("Condition") && scriptsLine[lineCounter + 4].Contains("Condition"))
    //                {
    //                    scriptBoxEditor.AppendText(space + "If ( " + varString + " " + condition + " " + text + ") ");
    //                    condition = getCondition(scriptsLine, lineCounter + 4);
    //                    scriptBoxEditor.AppendText(" " + condition + " ");
    //                }
    //                else
    //                    scriptBoxEditor.AppendText(space + "If( " + varString + " " + condition + " " + text + ");\n");
    //                break;

    //            }
    //        case "Condition":
    //            break;
    //        case "CopyVar":
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
    //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), varString, "NOR");
    //            scriptBoxEditor.AppendText(space + "VAR " + commandList[3] + " = " + varString + ";\n");
    //            break;
    //        case "Cry":
    //            varString = getTextFromCondition(commandList[3], "POK");
    //            scriptBoxEditor.AppendText(space + commandList[2] + "( POKE " + varString + ", " + commandList[4] + " );\n");
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
    //        case "Fanfare":
    //            scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MUSIC_ID " + commandList[3] + ");\n");
    //            break;
    //        case "Goto":
    //            scriptBoxEditor.AppendText(space + "Goto\n" + space + "{\n");
    //            var functionId = commandList[5];
    //            varNameDictionary.Add(new Dictionary<int, string>());
    //            for (var functionLineCounter = 0; functionLineCounter < scriptsLine.Length - 1; functionLineCounter++)
    //            {
    //                var line2 = scriptsLine[functionLineCounter];
    //                if (commandList.Length < 6)
    //                {
    //                    var offset2 = commandList[4].TrimStart('0').TrimStart('x');
    //                    int offset = int.Parse(offset2, System.Globalization.NumberStyles.HexNumber);
    //                    if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + offset))
    //                    {
    //                        varLevel++;
    //                        readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
    //                        varLevel--;
    //                        return;
    //                    }
    //                }
    //                else if (commandList.Length > 6 && scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[6].TrimStart('(')))
    //                {
    //                    varLevel++;
    //                    readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
    //                    varLevel--;
    //                    return;
    //                }
    //            }
    //            break;
    //        case "If":
    //            scriptBoxEditor.AppendText(space + "{\n");
    //            functionId = commandList[5];
    //            var type = commandList[4];
    //            varNameDictionary.Add(new Dictionary<int, string>());
    //            for (var functionLineCounter = 0; functionLineCounter < scriptsLine.Length - 1; functionLineCounter++)
    //            {
    //                var line2 = scriptsLine[functionLineCounter];
    //                if (commandList.Length < 8)
    //                {
    //                    var offset2 = commandList[4].TrimStart('0').TrimStart('x');
    //                    int offset = int.Parse(offset2, System.Globalization.NumberStyles.HexNumber);
    //                    if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + offset))
    //                    {
    //                        varLevel++;
    //                        readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
    //                        varLevel--;
    //                        return;
    //                    }
    //                }
    //                else if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[6].TrimStart('(')))
    //                {
    //                    varLevel++;
    //                    readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
    //                    varLevel--;
    //                    return;
    //                }

    //            }
    //            break;
    //        case "If2":
    //            scriptBoxEditor.AppendText(space + "{\n");
    //            functionId = commandList[5];
    //            type = commandList[4];
    //            varNameDictionary.Add(new Dictionary<int, string>());
    //            for (var functionLineCounter = 0; functionLineCounter < scriptsLine.Length - 1; functionLineCounter++)
    //            {
    //                var line2 = scriptsLine[functionLineCounter];
    //                if (commandList.Length < 8)
    //                {
    //                    var offset2 = commandList[4].TrimStart('0').TrimStart('x');
    //                    int offset = int.Parse(offset2, System.Globalization.NumberStyles.HexNumber);
    //                    if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + offset))
    //                    {
    //                        varLevel++;
    //                        readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
    //                        varLevel--;
    //                        return;
    //                    }
    //                }
    //                else if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[6].TrimStart('(')))
    //                {
    //                    varLevel++;
    //                    readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
    //                    varLevel--;
    //                    return;
    //                }
    //            }
    //            break;
    //        case "Jump":
    //            scriptBoxEditor.AppendText(space + "Jump\n" + space + "{\n");
    //            functionId = commandList[5];
    //            varNameDictionary.Add(new Dictionary<int, string>());
    //            for (var functionLineCounter = 0; functionLineCounter < scriptsLine.Length - 1; functionLineCounter++)
    //            {
    //                var line2 = scriptsLine[functionLineCounter];
    //                if (commandList.Length < 7)
    //                {
    //                    var offset2 = commandList[4].TrimStart('0').TrimStart('x');
    //                    int offset = int.Parse(offset2, System.Globalization.NumberStyles.HexNumber);
    //                    if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + offset))
    //                    {
    //                        readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
    //                        return;
    //                    }
    //                }
    //                else if (scriptsLine[functionLineCounter + 1].Contains("Offset: " + commandList[6].TrimStart('(')))
    //                {
    //                    readFunction(scriptsLine, lineCounter, space, ref functionLineCounter, ref line2, visitedLine);
    //                    return;
    //                }
    //            }
    //            break;
    //        case "Lock":
    //            scriptBoxEditor.AppendText(space + "" + commandList[2] + "( OW_ID " + commandList[3] + ");\n");
    //            break;

    //        case "Message":
    //            if (commandList.Length <= 10)
    //            {
    //                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( COSTANT " + commandList[3] + " , COSTANT " + commandList[4] +
    //                                       " , MESSAGE_ID " + commandList[5] + " , OW_ID " + commandList[6] +
    //                                           " , VIEW " + commandList[7] + " , TYPE " + commandList[8] + " );\n");
    //            }
    //            else
    //            {
    //                short index = 0;
    //                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
    //                varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 6);
    //                if (varString.Contains("M"))
    //                {
    //                    index = Int16.Parse(varString.ToCharArray()[varString.Length - 1].ToString());
    //                    scriptBoxEditor.AppendText(space + varString + " = ' " + textFile.messageList[index] + " ';\n");
    //                }
    //                else if (varString.Length > 2 && Int16.Parse(varString.Substring(2, varString.Length - 2)) > 8000 && textFile != null)
    //                {
    //                    var scriptAnLine = scriptBoxEditor.Lines;
    //                    int counterInverse = scriptAnLine.Length - 1;
    //                    while (!scriptAnLine[counterInverse].Contains("VAR") && !scriptAnLine[counterInverse].Contains(" = ") && !scriptAnLine[counterInverse].Contains(varString)
    //                         || (scriptAnLine[counterInverse].Contains("+") || scriptAnLine[counterInverse].Contains(")") || scriptAnLine[counterInverse].Contains("-") || scriptAnLine[counterInverse].Contains("[")) && counterInverse > 0)
    //                        counterInverse--;
    //                    if (counterInverse > 0)
    //                    {
    //                        var text = scriptAnLine[counterInverse].Split(' ');
    //                        varString = text[text.Length - 1];
    //                        scriptBoxEditor.AppendText(space + "MESSAGE " + commandList[5] + " = '" + textFile.messageList[Int16.Parse(varString)] + " ';\n");
    //                        varString = commandList[5].ToString();
    //                    }
    //                }
    //                else if (Int16.TryParse(varString, out index))
    //                    scriptBoxEditor.AppendText(space + "MESSAGE_" + varString + " = ' " + textFile.messageList[Int16.Parse(varString)] + " ';\n");
    //                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( COSTANT " + commandList[3] + " , COSTANT " + commandList[4] +
    //                                       " , MESSAGE_ID " + varString + " , OW_ID " + varString2 +
    //                                           " , VIEW " + commandList[7] + " , TYPE " + commandList[8] + " );\n");
    //            }
    //            break;
    //        case "Message2":
    //            if (commandList.Length < 10)
    //            {
    //                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( COSTANT " + commandList[3] + " , COSTANT " + commandList[4] +
    //                                           " , MESSAGE_ID " + commandList[5] +
    //                                           " , VIEW " + commandList[6] + " , TYPE " + commandList[7] + " );\n");
    //            }
    //            else
    //            {
    //                short index = 0;
    //                varString = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
    //                if (varString.Contains("M"))
    //                {
    //                    index = Int16.Parse(varString.ToCharArray()[varString.Length - 1].ToString());
    //                    scriptBoxEditor.AppendText(space + varString + " = ' " + textFile.messageList[index] + " ';\n");
    //                }
    //                else if (varString.Length > 2 && Int16.Parse(varString.Substring(2, varString.Length - 2)) > 8000 && textFile != null)
    //                {
    //                    var scriptAnLine = scriptBoxEditor.Lines;
    //                    int counterInverse = scriptAnLine.Length - 1;
    //                    while (!scriptAnLine[counterInverse].Contains("VAR") && !scriptAnLine[counterInverse].Contains(" = ") && !scriptAnLine[counterInverse].Contains(varString)
    //                        || scriptAnLine[counterInverse].Contains("+") || scriptAnLine[counterInverse].Contains(")") || scriptAnLine[counterInverse].Contains("-") & counterInverse > 0)
    //                        counterInverse--;
    //                    if (counterInverse > 0)
    //                    {
    //                        var text = scriptAnLine[counterInverse].Split(' ');
    //                        varString = text[text.Length - 1];
    //                        scriptBoxEditor.AppendText(space + "MESSAGE " + commandList[5] + " = '" + textFile.messageList[Int16.Parse(varString)] + " ';\n");
    //                        varString = commandList[5].ToString();
    //                    }
    //                }
    //                else if (Int16.TryParse(varString, out index))
    //                    scriptBoxEditor.AppendText(space + "MESSAGE_" + varString + " = ' " + textFile.messageList[Int16.Parse(varString)] + " ';\n");
    //                scriptBoxEditor.AppendText(space + "" + commandList[2] + "( COSTANT " + commandList[3] + " , COSTANT " + commandList[4] +
    //                                           " , MESSAGE_ID " + varString +
    //                                           " , VIEW " + commandList[6] + " , TYPE " + commandList[7] + " );\n");
    //            }
    //            break;
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
    //        case "Multi(B2)":
    //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 8), "MULTI_CHOSEN", "MUL");
    //            mulString = new List<string>();
    //            scriptBoxEditor.AppendText(space + "MULTI_CHOSEN " + commandList[8] + " = " + commandList[2] + "(X " + commandList[4] + " , " +
    //                "Y " + commandList[5] + " , " +
    //                "CURSOR " + commandList[6] + " , " +
    //                "P_4 " + commandList[7] +
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
    //        case "PlaySound":
    //            scriptBoxEditor.AppendText(space + "" + commandList[2] + "( SOUND_ID " + commandList[3] + ");\n");
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
    //        case "SetFlag":
    //            scriptBoxEditor.AppendText(space + "FLAG " + getTextFromCondition(commandList[3], "FLA") + " = TRUE;" + "\n");
    //            break;
    //        case "SetTextScriptMessage":
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
    //            scriptBoxEditor.AppendText(space + commandList[2] + "( BOX_TEXT " + commandList[3] + " , " +
    //                "MESSAGEBOX_TEXT  " + commandList[4] + " , " +
    //                "SCRIPT " + commandList[5] + " ");
    //            if (textFile != null)
    //            {
    //                var text2 = "";
    //                text2 = getTextFromVar(scriptBoxEditor, commandList, varString, text2);
    //            }
    //            scriptBoxEditor.AppendText(");\n");
    //            break;
    //        case "SetTrainerId":
    //            scriptBoxEditor.AppendText(space + "TRAINER " + commandList[3] + " = TRUE;" + "\n");
    //            break;
    //        case "SetValue":
    //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "VALUE", "NOR");
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
    //            scriptBoxEditor.AppendText(space + "VALUE " + commandList[3] + " = " + varString + "\n");
    //            break;
    //        case "SetVar(09)":
    //            newVar = checkStored(commandList, 3);
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
    //            //if (!scriptsLine[lineCounter + 1].Contains("CompareTo")) 
    //            //   //scriptBoxEditor.AppendText(space + "SET VAR " +  varString + ";\n");
    //            //addToVarNameDictionary(varNameDictionary, varLevel, newVar, "VAR 0x" + newVar.ToString("X"));
    //            conditionList = new List<string>();
    //            operatorList = new List<string>();
    //            blockList = new List<string>();
    //            int tempLineCounter = lineCounter;
    //            List<string> firstMembers = new List<string>();
    //            List<string> conditions = new List<string>();
    //            List<string> secondMembers = new List<string>();
    //            int numSetVar = 0;
    //            bool setVarCompareConditionBlock = (scriptsLine[tempLineCounter].Contains("SetVar(09)") || scriptsLine[tempLineCounter].Contains("StoreFlag")) && scriptsLine[tempLineCounter + 1].Contains("CompareTo");
    //            bool setVarsetVarConditionBlock = scriptsLine[tempLineCounter].Contains("SetVar(09)") && scriptsLine[tempLineCounter + 1].Contains("SetVar(09)") && scriptsLine[tempLineCounter + 2].Contains("Condition");
    //            if (setVarCompareConditionBlock || setVarsetVarConditionBlock)
    //            {
    //                while ((scriptsLine[tempLineCounter].Contains("SetVar(09)") || scriptsLine[tempLineCounter].Contains("StoreFlag")) && scriptsLine[tempLineCounter + 1].Contains("CompareTo")
    //                    || scriptsLine[tempLineCounter].Contains("SetVar(09)") && scriptsLine[tempLineCounter + 1].Contains("SetVar(09)") && scriptsLine[tempLineCounter + 2].Contains("Condition"))
    //                {
    //                    numSetVar++;

    //                    //First Member
    //                    var next = scriptsLine[tempLineCounter];
    //                    commandList = next.Split(' ');
    //                    newVar = checkStored(commandList, 3);
    //                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
    //                    if (next.Contains("Flag"))
    //                        firstMembers.Add("FLAG " + varString);
    //                    else
    //                        firstMembers.Add(varString);
    //                    tempLineCounter++;

    //                    //Second Member
    //                    next = scriptsLine[tempLineCounter];
    //                    commandList = next.Split(' ');
    //                    newVar = checkStored(commandList, 3);
    //                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
    //                    //scriptBoxEditor.AppendText(space + "SET VAR " + varString + ";\n");
    //                    temp2 = varString;

    //                    secondMembers.Add(getTextFromCondition(temp2, cond));
    //                    tempLineCounter++;

    //                    //Condition
    //                    next = scriptsLine[tempLineCounter];
    //                    conditions.Add(getCondition(scriptsLine, tempLineCounter - 1));
    //                    tempLineCounter++;
    //                }
    //                scriptBoxEditor.AppendText(space + "If( ");
    //                for (int i = 0; i < numSetVar; i++)
    //                {
    //                    string extCondition = "";
    //                    if (i != 0)
    //                    {
    //                        extCondition = getCondition(scriptsLine, tempLineCounter - 1);
    //                        scriptBoxEditor.AppendText(" " + extCondition + " ");
    //                    }
    //                    if (numSetVar > 1)
    //                        scriptBoxEditor.AppendText("( " + firstMembers[i] + " " + conditions[i] + " " + secondMembers[i] + " )");
    //                    else
    //                        scriptBoxEditor.AppendText(firstMembers[i] + " " + conditions[i] + " " + secondMembers[i]);
    //                }
    //                scriptBoxEditor.AppendText("); \n");
    //            }
    //            else
    //            {
    //                scriptBoxEditor.AppendText(space + "SET VAR " + varString + ";\n");
    //                break;
    //            }
    //            lineCounter = tempLineCounter - 1;
    //            //if (scriptsLine[lineCounter + 1].Contains("SetVar(09)") && scriptsLine[lineCounter + 2].Contains("CompareTo"))
    //            //{
    //            //    temp2 = next.Split(' ')[1];
    //            //    var text = getTextFromCondition(temp2, cond);
    //            //    lineCounter += 2;
    //            //    condition = getCondition(scriptsLine, lineCounter);
    //            //    scriptBoxEditor.AppendText(space + "If( " + temp + " " + condition + " " + text + ")");
    //            //}
    //            //else if (scriptsLine[lineCounter + 1].Contains("SetVar(09)") && scriptsLine[lineCounter + 2].Contains("Condition"))
    //            //{
    //            //    var temp3 = next.Split(' ');
    //            //    varString2 = getStoredMagic(varNameDictionary, varLevel, temp3, 3);
    //            //    scriptBoxEditor.AppendText(space + "SET VAR " + varString2 + ";\n");
    //            //    lineCounter += 1;
    //            //    condition = getCondition(scriptsLine, lineCounter);
    //            //    scriptBoxEditor.AppendText(space + "If( " + varString + " " + condition + " " + varString2 + " )\n");
    //            //    lineCounter += 1;
    //            //}
    //            //else
    //            //{
    //            //    int c = lineCounter;
    //            //    while (scriptsLine[c].Contains("SetVar(09)"))
    //            //        c++;
    //            //    if (scriptsLine[c].Contains("CompareTo"))
    //            //    {
    //            //        while (scriptsLine[lineCounter].Contains("SetVar(09)"))
    //            //        {
    //            //            line = scriptsLine[lineCounter];
    //            //            commandList = line.Split(' ');
    //            //            newVar = checkStored(commandList, 3);
    //            //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
    //            //            next = scriptsLine[lineCounter + 1];
    //            //            temp2 = next.Split(' ')[3];
    //            //            var text = getTextFromCondition(temp2, cond);
    //            //            condition = getCondition(scriptsLine, lineCounter + 1);
    //            //            operatorList.Add(space + "If( " + varString + " " + condition + " " + text + ")");
    //            //            //isFirst = false;
    //            //            lineCounter += 3;
    //            //            next = scriptsLine[lineCounter];
    //            //            var counter = lineCounter;
    //            //            while (next.Contains("Condition"))
    //            //            {
    //            //                conditionList.Add(next.Split(' ')[3]);
    //            //                counter++;
    //            //                next = scriptsLine[counter];
    //            //            }
    //            //            if (counter > lineCounter + 1)
    //            //            {
    //            //                if (!(blockList[blockList.Count - 1].Contains("AND")) && !(blockList[blockList.Count - 1].Contains("OR")))
    //            //                    blockList.RemoveAt(blockList.Count - 1);
    //            //                blockList.Add(" (" + operatorList[operatorList.Count - 1] + " " + next.Split(' ')[3] + " " + operatorList[operatorList.Count - 2] + ") ");
    //            //                var length = blockList.Count;
    //            //                counter = length - 1;
    //            //                //if (scriptsLine[lineCounter + 2].Contains("Condition"))
    //            //                //{
    //            //                //    while (scriptsLine[lineCounter + 2].Contains("Condition") && counter > 0)
    //            //                //    {
    //            //                //        conditionList.Add(blockList[blockList.Count - 1 - counter] + " " + scriptsLine[lineCounter + 1].Split(' ')[3]);
    //            //                //        lineCounter++;
    //            //                //        counter--;
    //            //                //    }
    //            //                //    blockList = conditionList;
    //            //                //}
    //            //            }
    //            //            else
    //            //                blockList.Add(operatorList[operatorList.Count - 1]);

    //            //        }
    //            //        for (int i = 0; i < blockList.Count; i++)
    //            //        {
    //            //            scriptBoxEditor.AppendText(blockList[i]);
    //            //            blockList.RemoveAt(i);
    //            //        }
    //            //        scriptBoxEditor.AppendText("\n");
    //            //        lineCounter -= 1;

    //            //    }
    //            //}
    //            //temp = newVar.ToString();
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
    //        case "SetVarAlter":
    //            scriptBoxEditor.AppendText(space + "VAR NAME " + commandList[3] + " = [ALTER]" + ";\n");
    //            break;
    //        case "SetVarBag":
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
    //            scriptBoxEditor.AppendText(space + "VAR BAG " + commandList[3] + " = " + varString + ";\n");
    //            break;
    //        case "SetVarHero":
    //            scriptBoxEditor.AppendText(space + "VAR NAME " + commandList[3] + " = [HIRO]" + ";\n");
    //            break;
    //        case "SetVarItem":
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
    //            if (IsNaturalNumber(varString))
    //                varString = getTextFromCondition(varString, "ITE");
    //            scriptBoxEditor.AppendText(space + "VAR ITEM " + commandList[3] + " = " + varString + ";\n");
    //            break;
    //        case "SetVarItemNumber":
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
    //            varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 5);
    //            if (IsNaturalNumber(varString))
    //                varString = getTextFromCondition(varString, "ITE");
    //            scriptBoxEditor.AppendText(space + "VAR ITEM " + commandList[3] + " = " + commandList[2] + "( " + varString + " , " + commandList[5] + " , " + commandList[6] + " );\n");
    //            break;
    //        case "SetVarBattleItem":
    //            scriptBoxEditor.AppendText(space + "VAR BATTLE ITEM " + commandList[3] + " = " + commandList[4] + ";\n");
    //            break;
    //        case "SetVarMove":
    //            newVar = checkStored(commandList, 4);
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
    //            scriptBoxEditor.AppendText(space + "VAR MOVE " + commandList[3] + " = " + varString + ";\n");
    //            break;
    //        case "SetVarNations":
    //            newVar = checkStored(commandList, 4);
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
    //            scriptBoxEditor.AppendText(space + "VAR NATION " + commandList[3] + " = " + varString + ";\n");
    //            break;
    //        case "SetVarNumberCond":
    //            newVar = checkStored(commandList, 4);
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
    //            scriptBoxEditor.AppendText(space + "VAR NUM " + commandList[3] + " = " + commandList[2] + "( " + varString + " , " + commandList[5] + " );\n");
    //            break;
    //        case "SetVarNickPokémon":
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
    //            scriptBoxEditor.AppendText(space + "VAR NICK " + commandList[3] + " = " + varString + ";\n");
    //            break;

    //        case "SetVarNumber":
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
    //            scriptBoxEditor.AppendText(space + "VAR NUM " + commandList[3] + " = " + varString + ";\n");
    //            break;
    //        case "SetVarPartyPokemon":
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
    //            scriptBoxEditor.AppendText(space + "VAR POKE " + commandList[3] + " = " + varString + ";\n");
    //            break;
    //        case "SetVarPartyPokèmonNick":
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
    //            scriptBoxEditor.AppendText(space + "VAR NICK " + commandList[3] + " = " + varString + ";\n");
    //            break;
    //        case "SetVarPokèmon":
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
    //            if (IsNaturalNumber(varString))
    //                varString = getTextFromCondition(commandList[4], "POK");
    //            scriptBoxEditor.AppendText(space + "VAR POKE " + commandList[3] + " = " + varString + ";\n");
    //            break;
    //        case "SetVarPokèmonDream":
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
    //            if (IsNaturalNumber(varString))
    //                varString = getTextFromCondition(commandList[3], "POK");
    //            scriptBoxEditor.AppendText(space + "VAR POKE " + commandList[4] + " = " + varString + ";\n");
    //            break;
    //        case "SetVarPokèLottoNumber":
    //            newVar = checkStored(commandList, 4);
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
    //            scriptBoxEditor.AppendText(space + "VAR L " + commandList[3] + " = " + commandList[2] + "( " + varString + " , " + commandList[5] + " );\n");
    //            break;
    //        case "SetVarPhraseBoxInput":
    //            newVar = checkStored(commandList, 4);
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
    //            scriptBoxEditor.AppendText(space + "VAR STRING " + commandList[3] + " = " + varString + ";\n");
    //            break;
    //        case "SetVarRival":
    //            scriptBoxEditor.AppendText(space + "VAR NAME " + commandList[3] + " = [RIVAL]" + ";\n");
    //            break;
    //        case "SetVarTrainer":
    //            scriptBoxEditor.AppendText(space + "VAR TRAINER " + commandList[3] + " = TRUE" + ";\n");
    //            break;
    //        case "SetVarTrainer2":
    //            scriptBoxEditor.AppendText(space + "VAR TRAINER_2 " + commandList[3] + " = TRUE" + ";\n");
    //            break;
    //        case "SetVarType":
    //            newVar = checkStored(commandList, 4);
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
    //            scriptBoxEditor.AppendText(space + "VAR TYPE " + commandList[3] + " = " + varString + ";\n");
    //            break;
    //        case "ShowMessageAt":
    //            scriptBoxEditor.AppendText(space + "" + commandList[2] + "( MESSAGE_ID " + commandList[3] + " , TYPE " + commandList[4] + " ");
    //            readMessage(scriptBoxEditor, commandList, 5);
    //            scriptBoxEditor.AppendText(" );\n");
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
    //        case "StoreDay":
    //            newVar = checkStored(commandList, 3);
    //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "DAY", "DAY");
    //            scriptBoxEditor.AppendText(space + "DAY " + commandList[3] + " = " + commandList[2] + "();\n");
    //            break;
    //        case "StoreDayPart":
    //            newVar = checkStored(commandList, 3);
    //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "DAY_PART", "NOR");
    //            scriptBoxEditor.AppendText(space + "DAY_PART " + commandList[3] + " = " + commandList[2] + "();\n");
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
    //        case "StoreFlag":
    //            newVar = checkStored(commandList, 3);
    //            varString = getTextFromCondition(commandList[3], "FLA");
    //            if (IsNaturalNumber(varString))
    //                addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FLAG " + newVar, "BOL");
    //            else
    //                addToVarNameDictionary(varNameDictionary, varLevel, newVar, varString, "BOL");
    //            temp = newVar.ToString();
    //            break;
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
    //        case "StoreHeroFaceOrientation":
    //            newVar = checkStored(commandList, 3);
    //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FACE_ORIENTATION", "NOR");
    //            scriptBoxEditor.AppendText(space + "FACE_ORIENTATION " + commandList[3] + "  = " + commandList[2] + "();\n");
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
    //        case "StoreSeason":
    //            addToVarNameDictionary(varNameDictionary, varLevel, checkStored(commandList, 3), "SEASON", "NOR");
    //            scriptBoxEditor.AppendText(space + "SEASON " + commandList[3] + " = " + commandList[2] + "();\n");
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
    //        case "StoreVarValue":
    //            newVar = checkStored(commandList, 3);
    //            newVar2 = checkStored(commandList, 4);
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
    //            if (!IsNaturalNumber(commandList[4]))
    //            {
    //                if (cond == "FLA")
    //                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FLAG_" + commandList[3], cond);
    //                else
    //                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, commandList[3], cond);
    //            }
    //            varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
    //            if (cond == "FLA")
    //                scriptBoxEditor.AppendText(space + "VAR_FLAG " + varString2 + " = " + (Boolean.Parse(commandList[4])).ToString() + "\n");
    //            else
    //                scriptBoxEditor.AppendText(space + "VAR " + varString2 + " = " + commandList[4] + "\n");
    //            break;
    //        case "StoreVarVariable":
    //            newVar = checkStored(commandList, 3);
    //            newVar2 = checkStored(commandList, 4);
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
    //            if (!IsNaturalNumber(commandList[4]))
    //            {
    //                if (cond == "FLA")
    //                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, "FLAG_" + commandList[3], cond);
    //                else
    //                    addToVarNameDictionary(varNameDictionary, varLevel, newVar, commandList[3], cond);
    //            }
    //            varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
    //            if (cond == "FLA")
    //                scriptBoxEditor.AppendText(space + "VAR_FLAG " + varString2 + " = " + (Boolean.Parse(commandList[4])).ToString() + "\n");
    //            else
    //                scriptBoxEditor.AppendText(space + "VAR " + varString2 + " = " + commandList[4] + "\n");
    //            break;
    //        case "StoreVarCallable":
    //            newVar = checkStored(commandList, 3);
    //            newVar2 = checkStored(commandList, 4);
    //            if (!IsNaturalNumber(commandList[4]))
    //                addToVarNameDictionary(varNameDictionary, varLevel, newVar, commandList[3], "NOR");
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
    //            varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
    //            if (scriptsLine[lineCounter + 2].Contains("CallStd"))
    //                varString = getTextFromCondition(commandList[4], "ITE");
    //            scriptBoxEditor.AppendText(space + "VAR_CALL " + varString2 + " = " + varString + "\n");
    //            break;
    //        case "StoreVar(2B)":
    //            newVar = checkStored(commandList, 3);
    //            newVar2 = checkStored(commandList, 4);
    //            if (!IsNaturalNumber(commandList[4]))
    //                addToVarNameDictionary(varNameDictionary, varLevel, newVar, commandList[3], "NOR");
    //            varString = getStoredMagic(varNameDictionary, varLevel, commandList, 4);
    //            varString2 = getStoredMagic(varNameDictionary, varLevel, commandList, 3);
    //            if (scriptsLine[lineCounter + 2].Contains("CallStd"))
    //                varString = getTextFromCondition(commandList[4], "ITE");
    //            scriptBoxEditor.AppendText(space + "VAR_2B " + varString2 + " = " + varString + "\n");
    //            break;
    //        case "StoreVarMessage":
    //            newVar = checkStored(commandList, 3);
    //            addToVarNameDictionary(varNameDictionary, varLevel, newVar, "MESSAGE_" + commandList[4], "NOR");
    //            scriptBoxEditor.AppendText(space + "VAR " + commandList[3] + " = MESSAGE_" + commandList[4] + ";\n");
    //            break;
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
    //        default:
    //            //for (int i = 3; i < commandList.Length ; i++)
    //            //    if (commandList[i]!="")
    //            //        scriptBoxEditor.AppendText(space + "P_" + (i - 2) + " = " + commandList[i] + ";\n");
    //            scriptBoxEditor.AppendText(space + commandList[2] + "(");
    //            for (int i = 3; i < commandList.Length - 2; i++)
    //                if (commandList[i] != "")
    //                {
    //                    newVar2 = checkStored(commandList, i);
    //                    varString = getStoredMagic(varNameDictionary, varLevel, commandList, i);
    //                    scriptBoxEditor.AppendText(" " + varString + " ,");
    //                }
    //            if (commandList[commandList.Length - 2] != "" && commandList.Length > 4)
    //            {
    //                newVar2 = checkStored(commandList, commandList.Length - 2);
    //                varString = getStoredMagic(varNameDictionary, varLevel, commandList, commandList.Length - 2);
    //                scriptBoxEditor.AppendText(" " + varString);
    //            }
    //            scriptBoxEditor.AppendText(" );\n");
    //            break;
    //    }

    //}
}



