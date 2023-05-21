namespace ScanFolderToFile.Forms.OptionsForms
{
    partial class FrmCopyMoveFiles
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
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialogSource = new System.Windows.Forms.FolderBrowserDialog();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.txtDestination = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkedListBoxOptions = new System.Windows.Forms.CheckedListBox();
            this.btnExec = new System.Windows.Forms.Button();
            this.folderBrowserDialogDestination = new System.Windows.Forms.FolderBrowserDialog();
            this.btnSfogliaSrc = new System.Windows.Forms.Button();
            this.btnSfogliaDest = new System.Windows.Forms.Button();
            this.btnOpenFolderDestionation = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cartella di partenza :";
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(242, 8);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(552, 34);
            this.txtSource.TabIndex = 1;
            // 
            // txtDestination
            // 
            this.txtDestination.Location = new System.Drawing.Point(242, 53);
            this.txtDestination.Name = "txtDestination";
            this.txtDestination.Size = new System.Drawing.Size(552, 34);
            this.txtDestination.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 56);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(226, 26);
            this.label2.TabIndex = 2;
            this.label2.Text = "Cartella di destinazione :";
            // 
            // checkedListBoxOptions
            // 
            this.checkedListBoxOptions.CheckOnClick = true;
            this.checkedListBoxOptions.FormattingEnabled = true;
            this.checkedListBoxOptions.Location = new System.Drawing.Point(412, 93);
            this.checkedListBoxOptions.Name = "checkedListBoxOptions";
            this.checkedListBoxOptions.Size = new System.Drawing.Size(95, 62);
            this.checkedListBoxOptions.TabIndex = 4;
            this.checkedListBoxOptions.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxOptions_ItemCheck);
            // 
            // btnExec
            // 
            this.btnExec.Location = new System.Drawing.Point(400, 161);
            this.btnExec.Name = "btnExec";
            this.btnExec.Size = new System.Drawing.Size(119, 33);
            this.btnExec.TabIndex = 5;
            this.btnExec.Text = "Esegui";
            this.btnExec.UseVisualStyleBackColor = true;
            this.btnExec.Click += new System.EventHandler(this.btnExec_Click);
            // 
            // btnSfogliaSrc
            // 
            this.btnSfogliaSrc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSfogliaSrc.Location = new System.Drawing.Point(801, 14);
            this.btnSfogliaSrc.Margin = new System.Windows.Forms.Padding(4);
            this.btnSfogliaSrc.Name = "btnSfogliaSrc";
            this.btnSfogliaSrc.Size = new System.Drawing.Size(100, 28);
            this.btnSfogliaSrc.TabIndex = 6;
            this.btnSfogliaSrc.Text = "SFOGLIA";
            this.btnSfogliaSrc.UseVisualStyleBackColor = true;
            this.btnSfogliaSrc.Click += new System.EventHandler(this.btnSfogliaSrc_Click);
            // 
            // btnSfogliaDest
            // 
            this.btnSfogliaDest.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSfogliaDest.Location = new System.Drawing.Point(801, 59);
            this.btnSfogliaDest.Margin = new System.Windows.Forms.Padding(4);
            this.btnSfogliaDest.Name = "btnSfogliaDest";
            this.btnSfogliaDest.Size = new System.Drawing.Size(100, 28);
            this.btnSfogliaDest.TabIndex = 7;
            this.btnSfogliaDest.Text = "SFOGLIA";
            this.btnSfogliaDest.UseVisualStyleBackColor = true;
            this.btnSfogliaDest.Click += new System.EventHandler(this.btnSfogliaDest_Click);
            // 
            // btnOpenFolderDestionation
            // 
            this.btnOpenFolderDestionation.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnOpenFolderDestionation.BackgroundImage = global::ScanFolderToFile.Properties.Resources.open;
            this.btnOpenFolderDestionation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnOpenFolderDestionation.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFolderDestionation.ForeColor = System.Drawing.Color.Red;
            this.btnOpenFolderDestionation.Location = new System.Drawing.Point(656, 95);
            this.btnOpenFolderDestionation.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenFolderDestionation.Name = "btnOpenFolderDestionation";
            this.btnOpenFolderDestionation.Size = new System.Drawing.Size(245, 95);
            this.btnOpenFolderDestionation.TabIndex = 9;
            this.btnOpenFolderDestionation.Text = "APRI CARTELLA DI DESTINAZIONE";
            this.btnOpenFolderDestionation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOpenFolderDestionation.UseVisualStyleBackColor = false;
            this.btnOpenFolderDestionation.Click += new System.EventHandler(this.btnOpenFolderDestionation_Click);
            // 
            // FrmCopyMoveFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 26F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 203);
            this.Controls.Add(this.btnOpenFolderDestionation);
            this.Controls.Add(this.btnSfogliaDest);
            this.Controls.Add(this.btnSfogliaSrc);
            this.Controls.Add(this.btnExec);
            this.Controls.Add(this.checkedListBoxOptions);
            this.Controls.Add(this.txtDestination);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Comic Sans MS", 11.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCopyMoveFiles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Copia / Sposta Files";
            this.Load += new System.EventHandler(this.FrmCopyMoveFiles_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogSource;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.TextBox txtDestination;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox checkedListBoxOptions;
        private System.Windows.Forms.Button btnExec;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogDestination;
        private System.Windows.Forms.Button btnSfogliaSrc;
        private System.Windows.Forms.Button btnSfogliaDest;
        private System.Windows.Forms.Button btnOpenFolderDestionation;
    }
}