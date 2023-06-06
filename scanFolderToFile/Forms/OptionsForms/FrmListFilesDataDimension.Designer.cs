namespace ScanFolderToFile.Forms.OptionsForms
{
    partial class FrmListFilesDataDimension
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblFiles = new System.Windows.Forms.Label();
            this.checkedListBoxOptionsDates = new System.Windows.Forms.CheckedListBox();
            this.monthCalendar = new System.Windows.Forms.MonthCalendar();
            this.cbDimension = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownFrom = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownTo = new System.Windows.Forms.NumericUpDown();
            this.btnSave = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFiles
            // 
            this.lblFiles.AutoSize = true;
            this.lblFiles.Location = new System.Drawing.Point(12, 9);
            this.lblFiles.Name = "lblFiles";
            this.lblFiles.Size = new System.Drawing.Size(138, 24);
            this.lblFiles.TabIndex = 0;
            this.lblFiles.Text = "Lista solo i file :";
            // 
            // checkedListBoxOptionsDates
            // 
            this.checkedListBoxOptionsDates.CheckOnClick = true;
            this.checkedListBoxOptionsDates.FormattingEnabled = true;
            this.checkedListBoxOptionsDates.Items.AddRange(new object[] {
            "Più VECCHI di una certa data/e",
            "Più GIOVANI di una certa data/e"});
            this.checkedListBoxOptionsDates.Location = new System.Drawing.Point(12, 41);
            this.checkedListBoxOptionsDates.Name = "checkedListBoxOptionsDates";
            this.checkedListBoxOptionsDates.Size = new System.Drawing.Size(316, 56);
            this.checkedListBoxOptionsDates.TabIndex = 1;
            this.checkedListBoxOptionsDates.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxOptionsDates_ItemCheck);
            // 
            // monthCalendar
            // 
            this.monthCalendar.Location = new System.Drawing.Point(339, 41);
            this.monthCalendar.Name = "monthCalendar";
            this.monthCalendar.TabIndex = 3;
            // 
            // cbDimension
            // 
            this.cbDimension.AutoSize = true;
            this.cbDimension.Location = new System.Drawing.Point(16, 244);
            this.cbDimension.Name = "cbDimension";
            this.cbDimension.Size = new System.Drawing.Size(218, 28);
            this.cbDimension.TabIndex = 4;
            this.cbDimension.Text = "Intervallo di dimensioni";
            this.cbDimension.UseVisualStyleBackColor = true;
            this.cbDimension.CheckedChanged += new System.EventHandler(this.cbDimension_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 279);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 24);
            this.label1.TabIndex = 5;
            this.label1.Text = "Da :";
            // 
            // numericUpDownFrom
            // 
            this.numericUpDownFrom.Enabled = false;
            this.numericUpDownFrom.Location = new System.Drawing.Point(60, 277);
            this.numericUpDownFrom.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownFrom.Name = "numericUpDownFrom";
            this.numericUpDownFrom.Size = new System.Drawing.Size(74, 31);
            this.numericUpDownFrom.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(140, 279);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 24);
            this.label2.TabIndex = 7;
            this.label2.Text = "A :";
            // 
            // numericUpDownTo
            // 
            this.numericUpDownTo.Enabled = false;
            this.numericUpDownTo.Location = new System.Drawing.Point(178, 277);
            this.numericUpDownTo.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownTo.Name = "numericUpDownTo";
            this.numericUpDownTo.Size = new System.Drawing.Size(74, 31);
            this.numericUpDownTo.TabIndex = 8;
            this.numericUpDownTo.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Comic Sans MS", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.Red;
            this.btnSave.Location = new System.Drawing.Point(488, 269);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(121, 39);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "SALVA";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(258, 280);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 24);
            this.label3.TabIndex = 10;
            this.label3.Text = "Mb";
            // 
            // FrmListFilesDataDimension
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 313);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.numericUpDownTo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDownFrom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbDimension);
            this.Controls.Add(this.monthCalendar);
            this.Controls.Add(this.checkedListBoxOptionsDates);
            this.Controls.Add(this.lblFiles);
            this.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmListFilesDataDimension";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lista dei files scansionati in relazione a Data/e e Dimensione/i";
            this.Load += new System.EventHandler(this.FrmListFilesDataDimension_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFiles;
        private System.Windows.Forms.CheckedListBox checkedListBoxOptionsDates;
        private System.Windows.Forms.MonthCalendar monthCalendar;
        private System.Windows.Forms.CheckBox cbDimension;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownTo;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label3;
    }
}