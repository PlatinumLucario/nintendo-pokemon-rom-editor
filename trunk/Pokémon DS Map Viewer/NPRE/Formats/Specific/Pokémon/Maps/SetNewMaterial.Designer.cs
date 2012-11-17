namespace NPRE.Formats
{
	partial class SetInfoNewPol
	{
		/// <summary>
		/// Variabile di progettazione necessaria.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Liberare le risorse in uso.
		/// </summary>
		/// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Codice generato da Progettazione Windows Form

		/// <summary>
		/// Metodo necessario per il supporto della finestra di progettazione. Non modificare
		/// il contenuto del metodo con l'editor di codice.
		/// </summary>
		private void InitializeComponent()
		{
            this.OkButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.AvaPalList = new System.Windows.Forms.ComboBox();
            this.AvaTexList = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(18, 161);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 0;
            this.OkButton.Text = "Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Material";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Palette";
            // 
            // AvaPalList
            // 
            this.AvaPalList.FormattingEnabled = true;
            this.AvaPalList.Location = new System.Drawing.Point(18, 84);
            this.AvaPalList.Name = "AvaPalList";
            this.AvaPalList.Size = new System.Drawing.Size(181, 21);
            this.AvaPalList.TabIndex = 70;
            // 
            // AvaTexList
            // 
            this.AvaTexList.FormattingEnabled = true;
            this.AvaTexList.Location = new System.Drawing.Point(18, 41);
            this.AvaTexList.Name = "AvaTexList";
            this.AvaTexList.Size = new System.Drawing.Size(181, 21);
            this.AvaTexList.TabIndex = 69;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(18, 124);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 71;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 72;
            this.label3.Text = "Size";
            // 
            // SetInfoNewPol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 196);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.AvaPalList);
            this.Controls.Add(this.AvaTexList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OkButton);
            this.Name = "SetInfoNewPol";
            this.Text = "Info New Polygon";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox AvaPalList;
        public System.Windows.Forms.ComboBox AvaTexList;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
	}
}