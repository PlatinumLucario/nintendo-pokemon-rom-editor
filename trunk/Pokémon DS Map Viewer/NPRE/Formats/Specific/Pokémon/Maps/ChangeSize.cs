using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NPRE.Formats
{
	public partial class ChangeSize: Form
	{
        private int oldSize;

		public ChangeSize()
		{
			InitializeComponent();
		}

        public ChangeSize(int oldSize)
        {
            InitializeComponent();
            this.oldSize = oldSize;
            oldSizeT.AppendText(oldSize.ToString("X6"));
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
                base.Close();
        }

        public int getNewSize()
        {
            return Int32.Parse(newSizeT.Text, System.Globalization.NumberStyles.AllowHexSpecifier);
        }

	}
}
