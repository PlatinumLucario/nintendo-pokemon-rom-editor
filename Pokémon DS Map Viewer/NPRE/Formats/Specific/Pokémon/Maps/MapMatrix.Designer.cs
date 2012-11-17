namespace NPRE.Formats.Specific.Pokémon.Maps
{
	partial class MapMatrix
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.matrixList = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.MatrixHeaderInfo = new System.Windows.Forms.DataGridView();
            this.MatrixBorderInfo = new System.Windows.Forms.DataGridView();
            this.MatrixTerrainInfo = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MatrixHeaderInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MatrixBorderInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MatrixTerrainInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // matrixList
            // 
            this.matrixList.FormattingEnabled = true;
            this.matrixList.Location = new System.Drawing.Point(27, 12);
            this.matrixList.Name = "matrixList";
            this.matrixList.Size = new System.Drawing.Size(121, 21);
            this.matrixList.TabIndex = 55;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(27, 39);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1160, 632);
            this.tabControl1.TabIndex = 56;
            // 
            // tabPage1
            // 
            this.tabPage1.AllowDrop = true;
            this.tabPage1.Controls.Add(this.MatrixHeaderInfo);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1152, 606);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Headers";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.MatrixBorderInfo);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1152, 606);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Borders";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.MatrixTerrainInfo);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1152, 606);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Terrain";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // MatrixHeaderInfo
            // 
            this.MatrixHeaderInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.MatrixHeaderInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.MatrixHeaderInfo.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.MatrixHeaderInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MatrixHeaderInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.MatrixHeaderInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MatrixHeaderInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.MatrixHeaderInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MatrixHeaderInfo.GridColor = System.Drawing.SystemColors.Control;
            this.MatrixHeaderInfo.Location = new System.Drawing.Point(151, 13);
            this.MatrixHeaderInfo.Name = "MatrixHeaderInfo";
            this.MatrixHeaderInfo.RowHeadersVisible = false;
            this.MatrixHeaderInfo.RowHeadersWidth = 4;
            this.MatrixHeaderInfo.RowTemplate.Height = 4;
            this.MatrixHeaderInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.MatrixHeaderInfo.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.MatrixHeaderInfo.Size = new System.Drawing.Size(850, 580);
            this.MatrixHeaderInfo.TabIndex = 58;
            // 
            // MatrixBorderInfo
            // 
            this.MatrixBorderInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.MatrixBorderInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.MatrixBorderInfo.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.MatrixBorderInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MatrixBorderInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.MatrixBorderInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MatrixBorderInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.MatrixBorderInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MatrixBorderInfo.GridColor = System.Drawing.SystemColors.Control;
            this.MatrixBorderInfo.Location = new System.Drawing.Point(151, 13);
            this.MatrixBorderInfo.Name = "MatrixBorderInfo";
            this.MatrixBorderInfo.RowHeadersVisible = false;
            this.MatrixBorderInfo.RowHeadersWidth = 4;
            this.MatrixBorderInfo.RowTemplate.Height = 4;
            this.MatrixBorderInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.MatrixBorderInfo.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.MatrixBorderInfo.Size = new System.Drawing.Size(850, 580);
            this.MatrixBorderInfo.TabIndex = 57;
            // 
            // MatrixTerrainInfo
            // 
            this.MatrixTerrainInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.MatrixTerrainInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.MatrixTerrainInfo.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.MatrixTerrainInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MatrixTerrainInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.MatrixTerrainInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MatrixTerrainInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.MatrixTerrainInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MatrixTerrainInfo.GridColor = System.Drawing.SystemColors.Control;
            this.MatrixTerrainInfo.Location = new System.Drawing.Point(151, 13);
            this.MatrixTerrainInfo.Name = "MatrixTerrainInfo";
            this.MatrixTerrainInfo.RowHeadersVisible = false;
            this.MatrixTerrainInfo.RowHeadersWidth = 4;
            this.MatrixTerrainInfo.RowTemplate.Height = 4;
            this.MatrixTerrainInfo.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.MatrixTerrainInfo.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.MatrixTerrainInfo.Size = new System.Drawing.Size(850, 580);
            this.MatrixTerrainInfo.TabIndex = 57;
            // 
            // MapMatrix
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1330, 683);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.matrixList);
            this.Name = "MapMatrix";
            this.Text = "MapMatrix";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MatrixHeaderInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MatrixBorderInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MatrixTerrainInfo)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.ComboBox matrixList;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        public System.Windows.Forms.DataGridView MatrixHeaderInfo;
        public System.Windows.Forms.DataGridView MatrixBorderInfo;
        public System.Windows.Forms.DataGridView MatrixTerrainInfo;
	}
}