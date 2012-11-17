namespace NPRE.GUI
{
	partial class ScriptRoutineViewer
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
            this.commandList = new System.Windows.Forms.ComboBox();
            this.scriptRoutineBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // commandList
            // 
            this.commandList.FormattingEnabled = true;
            this.commandList.Location = new System.Drawing.Point(13, 29);
            this.commandList.Name = "commandList";
            this.commandList.Size = new System.Drawing.Size(121, 21);
            this.commandList.TabIndex = 0;
            this.commandList.SelectedIndexChanged += new System.EventHandler(this.commandList_SelectedIndexChanged);
            // 
            // scriptRoutineBox
            // 
            this.scriptRoutineBox.Location = new System.Drawing.Point(13, 72);
            this.scriptRoutineBox.Name = "scriptRoutineBox";
            this.scriptRoutineBox.Size = new System.Drawing.Size(587, 447);
            this.scriptRoutineBox.TabIndex = 1;
            this.scriptRoutineBox.Text = "";
            // 
            // ScriptRoutineViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 548);
            this.Controls.Add(this.scriptRoutineBox);
            this.Controls.Add(this.commandList);
            this.Name = "ScriptRoutineViewer";
            this.Text = "Form1";
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.ComboBox commandList;
        private System.Windows.Forms.RichTextBox scriptRoutineBox;
	}
}