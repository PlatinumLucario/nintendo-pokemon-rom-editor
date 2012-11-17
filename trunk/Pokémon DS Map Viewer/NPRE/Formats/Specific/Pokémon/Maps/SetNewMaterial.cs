using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PG4Map.Formats;

namespace NPRE.Formats
{
	public partial class SetInfoNewPol: Form
	{

		public SetInfoNewPol(Nsbtx nsbtx)
		{
			InitializeComponent();
            LoadAvailableTextures(nsbtx);
		}

        private void LoadAvailableTextures(Nsbtx nsbtx)
        {
            int idTexture = 0;
            if (nsbtx == null)
                return;
            foreach (NsbmdModel.MatTexPalStruct texture in nsbtx.tex0.matTexPalList)
            {
                AvaTexList.Items.Add(texture.texName);
                idTexture++;
            }
            int idPalette = 0;
            foreach (NsbmdModel.MatTexPalStruct texture in nsbtx.tex0.matTexPalList)
            {
                AvaPalList.Items.Add(texture.palName);
                idPalette++;
            }
        }


        private void OkButton_Click(object sender, EventArgs e)
        {
                base.Close();
        }

        public string getTexture()
        {
            return AvaTexList.Items[AvaTexList.SelectedIndex].ToString();
        }

        public string getPalette()
        {
            
            return AvaPalList.Items[AvaPalList.SelectedIndex].ToString();
        }

        public int getSize()
        {
            return Int32.Parse(textBox1.Text);
        }
    }
}
