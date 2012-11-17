
namespace PG4Map
{
    using PG4Map.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using PG4Map.Formats;
    using PG4Map;

    public class MapCreator : Form
    {

        private Nsbmd actualModel;
        private Button Save;

        #region Gui

        private IContainer components = null;
        private string texture;
        private string command;
        public Panel polyInfo;
        public static NsbmdModel.ShapeInfoStruct shapeInfo;
        private int MAXSIZE;
        private List<Point_s> PointList;
        #endregion

        public MapCreator()
        {
              InitializeComponent();
        }

        public MapCreator(Maps actualMap)
        {
              InitializeComponent();
              actualModel = actualMap.actualModel;        
        }

        public MapCreator(string texture, string command, NsbmdModel.ShapeInfoStruct shape)
        {
            InitializeComponent();
            shapeInfo = shape;
            this.command = command;
            this.texture = texture;

            //MAXSIZE = getQuads(texture);

            //for (int movCounter = 0; movCounter < MAXSIZE; movCounter++)
            //{
            //    polyInfo.Columns.Add(movCounter.ToString(), movCounter.ToString());
            //    polyInfo.Rows.Add();
            //}

            //polyInfo.Rows[MAXSIZE / 2].Cells[MAXSIZE / 2].Style.BackColor = Color.Black;

            populateTable();
        }


        private void populateTable()
        {
            var commandList = shapeInfo.commandList;
            int shiftX = 0;
            int shiftY = 0;
            int realX = 0;
            int realY = 0;
            int num = 0;
            PointList = new List<Point_s>();
            var graph = polyInfo.CreateGraphics();
            var pointStruct = new Point_s();

            foreach (NsbmdModel.CommandStruct actualCommand in commandList)
            {
                if (actualCommand.id != 0x41 && actualCommand.id != 0x21 && actualCommand.id != 0x22)
                {
                    if (actualCommand.x != 1000)
                        realX = actualCommand.x >> 10;
                    if (actualCommand.z != 1000)
                        realY = actualCommand.z >> 10;
                    shiftX = (realX) * 12 + 256;
                    shiftY = (realY) * 12 + 256;
                    if (actualCommand.id == 0x23)
                        graph.FillRectangle(Brushes.Black, shiftX, shiftY, 2, 2);
                    else if (actualCommand.id == 0x24)
                    {
                        realX = actualCommand.x >> 4;
                        realY = actualCommand.z >> 4;
                        shiftX = (realX) * 12 + 256;
                        shiftY = (realY) * 12 + 256;
                        graph.FillRectangle(Brushes.Blue, shiftX, shiftY, 2, 2);
                    }
                    else if (actualCommand.id == 0x25)
                        graph.FillRectangle(Brushes.Red, shiftX, shiftY, 2, 2);
                    else if (actualCommand.id == 0x26)
                        graph.FillRectangle(Brushes.Yellow, shiftX, shiftY, 2, 2);
                    else if (actualCommand.id == 0x27)
                        graph.FillRectangle(Brushes.Green, shiftX, shiftY, 2, 2);
                    else if (actualCommand.id == 0x28)
                        graph.FillRectangle(Brushes.Brown, shiftX, shiftY, 2, 2);
                    pointStruct.commandInfo = actualCommand.id;
                    pointStruct.pointElement = new Point(shiftX, shiftY);
                    PointList.Add(pointStruct);
                    num++;

                }

            }


            PointList.RemoveAt(0);

            if (commandList[0].par == 0 || commandList[0].par == 1)
            {
                for (int i = 0; i < PointList.Count - 3; i += 4)
                {
                    graph.DrawLine(new Pen(Color.Red, 1), PointList[i].pointElement, PointList[i + 1].pointElement);
                    graph.DrawLine(new Pen(Color.Red, 1), PointList[i].pointElement, PointList[i + 3].pointElement);
                    graph.DrawLine(new Pen(Color.Red, 1), PointList[i + 1].pointElement, PointList[i + 2].pointElement);
                    graph.DrawLine(new Pen(Color.Red, 1), PointList[i + 2].pointElement, PointList[i + 3].pointElement);

                }
            }
            else
            {
                for (int i = 0; i < 4; i += 3)
                {
                    if ((i+4)<PointList.Count && PointList[i + 4].commandInfo == 0x40)
                    {
                        graph.DrawLine(new Pen(Color.Red, 1), PointList[i].pointElement, PointList[i + 1].pointElement);
                        graph.DrawLine(new Pen(Color.Red, 1), PointList[i].pointElement, PointList[i + 3].pointElement);
                        graph.DrawLine(new Pen(Color.Red, 1), PointList[i + 1].pointElement, PointList[i + 2].pointElement);
                        graph.DrawLine(new Pen(Color.Red, 1), PointList[i + 2].pointElement, PointList[i + 3].pointElement);
                        i+=2;
                    }
                    else
                    {
                        graph.DrawLine(new Pen(Color.Red, 1), PointList[i].pointElement, PointList[i + 1].pointElement);
                        graph.DrawLine(new Pen(Color.Red, 1), PointList[i].pointElement, PointList[i + 2].pointElement);
                        graph.DrawLine(new Pen(Color.Red, 1), PointList[i + 1].pointElement, PointList[i + 2].pointElement);
                    }
                }

            }
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            populateTable();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (  components != null))
            {
                  components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.Save = new System.Windows.Forms.Button();
            this.polyInfo = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(833, 78);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 1;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            // 
            // polyInfo
            // 
            this.polyInfo.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.polyInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.polyInfo.Location = new System.Drawing.Point(12, 12);
            this.polyInfo.Name = "polyInfo";
            this.polyInfo.Size = new System.Drawing.Size(768, 700);
            this.polyInfo.TabIndex = 48;
            this.polyInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_Paint);
            // 
            // MapCreator
            // 
            this.ClientSize = new System.Drawing.Size(1351, 742);
            this.Controls.Add(this.polyInfo);
            this.Controls.Add(this.Save);
            this.Name = "MapCreator";
            this.Text = "Map Creator";
            this.ResumeLayout(false);

        }

    }

    struct Point_s
    {
        public Point pointElement;
        public int commandInfo;
    }

}
