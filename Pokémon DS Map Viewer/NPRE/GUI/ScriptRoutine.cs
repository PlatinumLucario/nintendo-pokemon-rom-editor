using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace NPRE.GUI
{
	public partial class ScriptRoutineViewer: Form
	{
        private ClosableMemoryStream ARM9;
        private List<scriptRoutine_S> scriptRoutineList;
        private List<int> scriptRoutineOffsetList;
        private bool isCommand;
        private bool isEnd;

        private struct scriptRoutine_S
        {
            public int offset;
            public int id;
            public List<Cmd_s> command;
        }

        public struct Cmd_s
        {
            public int id;
            public List<int> parameter;
        }

		public ScriptRoutineViewer()
		{
			InitializeComponent();
		}

        public ScriptRoutineViewer(ClosableMemoryStream ARM9)
        {
            InitializeComponent();
            this.ARM9 = ARM9;
            var reader = new BinaryReader(ARM9);
            reader.BaseStream.Position= 0xEAC58;
            scriptRoutineList = new List<scriptRoutine_S>();
            scriptRoutineOffsetList = new List<int>();
            for (int i = 0; i < 839; i++)
            {
                var item = new scriptRoutine_S();
                item.id = i;
                item.offset = reader.ReadInt32();
                scriptRoutineOffsetList.Add(item.offset);
                commandList.Items.Add(i.ToString("X"));
                scriptRoutineBox.AppendText("\nCommand " + item.id.ToString("X") + " = 0x" + item.offset.ToString("X"));
                scriptRoutineList.Add(item);
            }

            scriptRoutineOffsetList.Sort();

        }

        private void commandList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var commandId = commandList.SelectedIndex;
            var actualRoutine = scriptRoutineList[commandId];
            scriptRoutineBox.Clear();
            var nextOffset = -1;
            for (int i = 0; i < scriptRoutineOffsetList.Count; i++)
                if (scriptRoutineOffsetList[i] == actualRoutine.offset)
                    nextOffset = scriptRoutineOffsetList[i + 1] - 0x02000000;
            scriptRoutineBox.AppendText("\nEnding offset: " + nextOffset.ToString("X"));
            var reader = new BinaryReader(ARM9);
            reader.BaseStream.Position = actualRoutine.offset - 0x02000000;
            scriptRoutineBox.AppendText("\nStart offset: " + reader.BaseStream.Position.ToString("X"));
            actualRoutine.command = new List<Cmd_s>();
            var start = (int)reader.BaseStream.Position;
            int j = start;
            isEnd = false;
            while (j <= nextOffset - 1 && !isEnd)
            {
                var actualCommand = new Cmd_s();
                scriptRoutineBox.AppendText("\nOffset:" + reader.BaseStream.Position.ToString("X"));
                actualCommand.id = (int)reader.ReadByte();
                j++;
                scriptRoutineBox.AppendText(" " + actualCommand.id.ToString("X"));
                actualCommand.parameter = new List<int>();
                switch (actualCommand.id)
                {
                    case 0x4D:
                        actualCommand.parameter.Add(reader.ReadByte());
                        actualCommand.parameter.Add(reader.ReadByte());
                        actualCommand.parameter.Add(reader.ReadByte());
                        actualCommand.parameter.Add(reader.ReadByte());
                        j += 4;
                        break;
                    case 0xB5:
                        actualCommand.parameter.Add(reader.ReadByte());
                        actualCommand.parameter.Add(reader.ReadByte());
                        j += 2;
                        break;
                    case 0xBD:
                        actualCommand.parameter.Add(reader.ReadByte());
                        actualCommand.parameter.Add(reader.ReadByte());
                        actualCommand.parameter.Add(reader.ReadByte());
                        j += 3;
                        break;

                }
                //j++;

                
                //isCommand = true;
                //while (isCommand && !isEnd)
                //    actualCommand = checkParameter(reader, actualCommand, nextOffset, ref j);

                actualRoutine.command.Add(actualCommand);
                foreach (var parameter in actualCommand.parameter)
                    scriptRoutineBox.AppendText(" 0x" + parameter.ToString("X"));


            }
        }
      

        private Cmd_s checkParameter(BinaryReader reader, Cmd_s actualCommand, int offset, ref int i)
        {
            if (reader.BaseStream.Position <= offset - 1)
            {
                var next = reader.ReadByte();
                if (next > 0x9F)
                {
                    i--;
                    reader.BaseStream.Position--;
                    isCommand = false;
                }
                else
                {
                    actualCommand.parameter.Add(next);
                    i++;
                    scriptRoutineBox.AppendText(" 0x" + next.ToString("X"));
                }
            }
            else
                isEnd= true;

            return actualCommand;
        }
	}

}
