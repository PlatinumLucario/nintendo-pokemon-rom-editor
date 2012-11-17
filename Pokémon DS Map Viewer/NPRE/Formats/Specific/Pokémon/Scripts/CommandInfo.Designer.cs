namespace NPRE.Formats.Specific.Pokémon.Scripts
{
	partial class CommandInfo
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
            this.scriptList = new System.Windows.Forms.ComboBox();
            this.commandDescription = new System.Windows.Forms.RichTextBox();
            this.commandName = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // scriptList
            // 
            this.scriptList.FormattingEnabled = true;
            this.scriptList.Location = new System.Drawing.Point(247, 23);
            this.scriptList.Name = "scriptList";
            this.scriptList.Size = new System.Drawing.Size(50, 21);
            this.scriptList.TabIndex = 0;
            this.scriptList.SelectedIndexChanged += new System.EventHandler(this.scriptList_SelectedIndexChanged);
            // 
            // commandDescription
            // 
            this.commandDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.commandDescription.Location = new System.Drawing.Point(6, 19);
            this.commandDescription.Name = "commandDescription";
            this.commandDescription.ReadOnly = true;
            this.commandDescription.Size = new System.Drawing.Size(270, 75);
            this.commandDescription.TabIndex = 3;
            this.commandDescription.Text = "";
            // 
            // commandName
            // 
            this.commandName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.commandName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.commandName.Location = new System.Drawing.Point(6, 19);
            this.commandName.Name = "commandName";
            this.commandName.ReadOnly = true;
            this.commandName.Size = new System.Drawing.Size(166, 13);
            this.commandName.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.commandName);
            this.panel1.Location = new System.Drawing.Point(15, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(226, 48);
            this.panel1.TabIndex = 5;
            this.panel1.TabStop = false;
            this.panel1.Text = "Name";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.commandDescription);
            this.groupBox1.Location = new System.Drawing.Point(15, 66);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(282, 100);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Description";
            // 
            // CommandInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(319, 181);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.scriptList);
            this.Name = "CommandInfo";
            this.Text = "CommandInfo";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.ComboBox scriptList;
        private System.Windows.Forms.RichTextBox commandDescription;
        private System.Windows.Forms.TextBox commandName;
        private System.Windows.Forms.GroupBox panel1;
        private System.Windows.Forms.GroupBox groupBox1;
	}
}