namespace PG4Map
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using NPRE.Commons;

    public class IsBWDialog : Form
    {
        private Button button1;
        private RadioButton Bw;
        private RadioButton checkBox1;
        public IContainer components = null;
        private RadioButton Dpp;
        private RadioButton Hgss;
        private RadioButton Pl;
        private RadioButton BW2;
        public Label label1;

        public IsBWDialog()
        {
              InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (  CheckGame() != -1)
            {
                base.Close();
            }
        }

        public int CheckGame()
        {
            if ( Dpp.Checked)
            {
                return Constants.DPSCRIPT;
            }
            if (  Hgss.Checked)
            {
                return Constants.HGSSSCRIPT;
            }
            if (  Bw.Checked)
            {
                return Constants.BWSCRIPT;
            }
            if (  checkBox1.Checked)
            {
                return 5;
            }
            if (  Pl.Checked)
                return Constants.PLSCRIPT;
            if (  BW2.Checked)
                return Constants.BW2SCRIPT;
            return -1;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (  components != null))
            {
                  components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void InitializeComponent()
        {
              label1 = new System.Windows.Forms.Label();
              Dpp = new System.Windows.Forms.RadioButton();
              Hgss = new System.Windows.Forms.RadioButton();
              Bw = new System.Windows.Forms.RadioButton();
              checkBox1 = new System.Windows.Forms.RadioButton();
              button1 = new System.Windows.Forms.Button();
              Pl = new System.Windows.Forms.RadioButton();
              BW2 = new System.Windows.Forms.RadioButton();
              SuspendLayout();
            // 
            // label1
            // 
              label1.AutoSize = true;
              label1.Location = new System.Drawing.Point(13, 9);
              label1.Name = "label1";
              label1.Size = new System.Drawing.Size(99, 13);
              label1.TabIndex = 2;
              label1.Text = "Choose map Origin.";
            // 
            // Dpp
            // 
              Dpp.AutoSize = true;
              Dpp.Location = new System.Drawing.Point(16, 36);
              Dpp.Name = "Dpp";
              Dpp.Size = new System.Drawing.Size(40, 17);
              Dpp.TabIndex = 4;
              Dpp.Text = "DP";
              Dpp.UseVisualStyleBackColor = true;
            // 
            // Hgss
            // 
              Hgss.AutoSize = true;
              Hgss.Location = new System.Drawing.Point(16, 84);
              Hgss.Name = "Hgss";
              Hgss.Size = new System.Drawing.Size(55, 17);
              Hgss.TabIndex = 5;
              Hgss.Text = "HGSS";
              Hgss.UseVisualStyleBackColor = true;
            // 
            // Bw
            // 
              Bw.AutoSize = true;
              Bw.Location = new System.Drawing.Point(16, 107);
              Bw.Name = "Bw";
              Bw.Size = new System.Drawing.Size(43, 17);
              Bw.TabIndex = 6;
              Bw.Text = "BW";
              Bw.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
              checkBox1.AutoSize = true;
              checkBox1.Location = new System.Drawing.Point(16, 153);
              checkBox1.Name = "checkBox1";
              checkBox1.Size = new System.Drawing.Size(51, 17);
              checkBox1.TabIndex = 7;
              checkBox1.Text = "Other";
              checkBox1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
              button1.Location = new System.Drawing.Point(12, 186);
              button1.Name = "button1";
              button1.Size = new System.Drawing.Size(75, 23);
              button1.TabIndex = 8;
              button1.Text = "OK";
              button1.UseVisualStyleBackColor = true;
              button1.Click += new System.EventHandler(  button1_Click);
            // 
            // Pl
            // 
              Pl.AutoSize = true;
              Pl.Location = new System.Drawing.Point(16, 61);
              Pl.Name = "Pl";
              Pl.Size = new System.Drawing.Size(38, 17);
              Pl.TabIndex = 9;
              Pl.Text = "PL";
              Pl.UseVisualStyleBackColor = true;
            // 
            // BW2
            // 
              BW2.AutoSize = true;
              BW2.Location = new System.Drawing.Point(16, 130);
              BW2.Name = "BW2";
              BW2.Size = new System.Drawing.Size(49, 17);
              BW2.TabIndex = 10;
              BW2.Text = "BW2";
              BW2.UseVisualStyleBackColor = true;
            // 
            // IsBWDialog
            // 
              ClientSize = new System.Drawing.Size(124, 276);
              Controls.Add(  BW2);
              Controls.Add(  Pl);
              Controls.Add(  button1);
              Controls.Add(  checkBox1);
              Controls.Add(  Bw);
              Controls.Add(  Hgss);
              Controls.Add(  Dpp);
              Controls.Add(  label1);
              MaximizeBox = false;
              MinimizeBox = false;
              Name = "IsBWDialog";
              Text = "Game";
              Load += new System.EventHandler(  IsBWDialog_Load);
              ResumeLayout(false);
              PerformLayout();

        }

        private void IsBWDialog_Load(object sender, EventArgs e)
        {

        }


    }
}

