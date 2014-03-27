using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace NPRE.Formats.Specific.Pokémon.Scripts
{
	public class ScriptLoader
	{
        public RichTextBox Console;

        public List<Scripts.Script_s> scriptList;
        public List<uint> scriptOffList = new List<uint>();
        public List<uint> scriptOrder;
        public List<uint> scriptStartList = new List<uint>();

        public ScriptLoader(RichTextBox Console)
        {
            scriptList = new List<Scripts.Script_s>();
            scriptOffList = new List<uint>();
            scriptStartList = new List<uint>();
            this.Console = Console;
        }

        public void ReadScriptsOffsets(BinaryReader reader, List<Scripts.Script_s> scriptList, List<uint> scriptOffList)
        {
            uint scriptOffFinder = 0;
            scriptOrder = new List<uint>();

            do
                scriptOffFinder = ReadScriptOffset(reader, scriptOffList, scriptOffFinder);
            while (scriptOffFinder != 0xFD13 &&
                reader.BaseStream.Position != scriptOffList[0] &&
                reader.BaseStream.Position < reader.BaseStream.Length);

            FixScriptOffStartLists(scriptOffList);

            for (int scriptCounter = 0; scriptCounter < scriptOffList.Count; scriptCounter++)
                InitScriptList(scriptList, scriptCounter);

            for (int scriptCounter = 0; scriptCounter < scriptOffList.Count; scriptCounter++)
                Console.AppendText("\nStored offset was " + scriptOffList[scriptCounter].ToString() +
                                   " .Real offset is " + scriptStartList[scriptCounter].ToString());
            this.scriptList = scriptList;
            this.scriptOffList = scriptOffList;
        }

        private void FixScriptOffStartLists(List<uint> scriptOffList)
        {
            if (scriptOffList[scriptOffList.Count - 1] == 0xFD13)
                scriptOffList.RemoveAt(scriptOffList.Count - 1);
            scriptOffList.Sort();
            //scriptStartList.RemoveAt(scriptStartList.Count - 1);
            scriptStartList.Sort();
        }

        private void InitScriptList(List<Scripts.Script_s> scriptList, int scriptCounter)
        {
            Scripts.Script_s script = new Scripts.Script_s
            {
                scriptStart = scriptStartList[scriptCounter]
            };
            scriptList.Add(script);
        }

        private uint ReadScriptOffset(BinaryReader reader, List<uint> scriptOffList, uint scriptOffFinder)
        {
            scriptOffFinder = reader.ReadUInt16();
            reader.ReadUInt16();
            uint pad = (uint)reader.BaseStream.Position;
            uint scriptStart = 0;
            scriptStart = scriptOffFinder + pad;
            scriptOffList.Add(scriptOffFinder);
            scriptStartList.Add(scriptStart);
            scriptOrder.Add(scriptStart);
            return scriptOffFinder;
        }






	}
}
