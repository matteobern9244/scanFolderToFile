namespace ScanFolderToFile.Forms
{
    partial class FrmPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrincipal));
            this.txtSelectedPath = new System.Windows.Forms.TextBox();
            this.btnSfoglia = new System.Windows.Forms.Button();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.gbSelectFolder = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbOnlyExtensions = new System.Windows.Forms.CheckBox();
            this.cbPdf = new System.Windows.Forms.CheckBox();
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.cbZipFolder = new System.Windows.Forms.CheckBox();
            this.btnCopyMoveFiles = new System.Windows.Forms.Button();
            this.btnOpenFolderZip = new System.Windows.Forms.Button();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.btnStampaFile = new System.Windows.Forms.Button();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.gbSelectFolder.SuspendLayout();
            this.gbOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSelectedPath
            // 
            this.txtSelectedPath.Enabled = false;
            this.txtSelectedPath.Location = new System.Drawing.Point(8, 23);
            this.txtSelectedPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtSelectedPath.Name = "txtSelectedPath";
            this.txtSelectedPath.Size = new System.Drawing.Size(317, 34);
            this.txtSelectedPath.TabIndex = 1;
            // 
            // btnSfoglia
            // 
            this.btnSfoglia.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSfoglia.Location = new System.Drawing.Point(335, 23);
            this.btnSfoglia.Margin = new System.Windows.Forms.Padding(4);
            this.btnSfoglia.Name = "btnSfoglia";
            this.btnSfoglia.Size = new System.Drawing.Size(100, 28);
            this.btnSfoglia.TabIndex = 2;
            this.btnSfoglia.Text = "SFOGLIA";
            this.btnSfoglia.UseVisualStyleBackColor = true;
            this.btnSfoglia.Click += new System.EventHandler(this.btnSfoglia_Click);
            // 
            // gbSelectFolder
            // 
            this.gbSelectFolder.Controls.Add(this.txtSelectedPath);
            this.gbSelectFolder.Controls.Add(this.btnSfoglia);
            this.gbSelectFolder.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbSelectFolder.Location = new System.Drawing.Point(16, 15);
            this.gbSelectFolder.Margin = new System.Windows.Forms.Padding(4);
            this.gbSelectFolder.Name = "gbSelectFolder";
            this.gbSelectFolder.Padding = new System.Windows.Forms.Padding(4);
            this.gbSelectFolder.Size = new System.Drawing.Size(449, 63);
            this.gbSelectFolder.TabIndex = 4;
            this.gbSelectFolder.TabStop = false;
            this.gbSelectFolder.Text = "Seleziona cartella :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(635, 288);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "by Matteo Bernardini©";
            // 
            // cbOnlyExtensions
            // 
            this.cbOnlyExtensions.AutoSize = true;
            this.cbOnlyExtensions.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbOnlyExtensions.Location = new System.Drawing.Point(7, 68);
            this.cbOnlyExtensions.Margin = new System.Windows.Forms.Padding(4);
            this.cbOnlyExtensions.Name = "cbOnlyExtensions";
            this.cbOnlyExtensions.Size = new System.Drawing.Size(164, 30);
            this.cbOnlyExtensions.TabIndex = 10;
            this.cbOnlyExtensions.Text = "Solo Estensioni";
            this.cbOnlyExtensions.UseVisualStyleBackColor = true;
            this.cbOnlyExtensions.CheckedChanged += new System.EventHandler(this.cbOnlyExtensions_CheckedChanged);
            // 
            // cbPdf
            // 
            this.cbPdf.AutoSize = true;
            this.cbPdf.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPdf.Location = new System.Drawing.Point(7, 30);
            this.cbPdf.Margin = new System.Windows.Forms.Padding(4);
            this.cbPdf.Name = "cbPdf";
            this.cbPdf.Size = new System.Drawing.Size(107, 30);
            this.cbPdf.TabIndex = 11;
            this.cbPdf.Text = "File PDF";
            this.cbPdf.UseVisualStyleBackColor = true;
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.btnCopyMoveFiles);
            this.gbOptions.Controls.Add(this.cbZipFolder);
            this.gbOptions.Controls.Add(this.cbPdf);
            this.gbOptions.Controls.Add(this.cbOnlyExtensions);
            this.gbOptions.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.gbOptions.Location = new System.Drawing.Point(16, 90);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Size = new System.Drawing.Size(449, 150);
            this.gbOptions.TabIndex = 12;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Opzioni";
            // 
            // cbZipFolder
            // 
            this.cbZipFolder.AutoSize = true;
            this.cbZipFolder.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbZipFolder.Location = new System.Drawing.Point(7, 106);
            this.cbZipFolder.Margin = new System.Windows.Forms.Padding(4);
            this.cbZipFolder.Name = "cbZipFolder";
            this.cbZipFolder.Size = new System.Drawing.Size(135, 30);
            this.cbZipFolder.TabIndex = 12;
            this.cbZipFolder.Text = "Zip Cartella";
            this.cbZipFolder.UseVisualStyleBackColor = true;
            // 
            // btnCopyMoveFiles
            // 
            this.btnCopyMoveFiles.Location = new System.Drawing.Point(236, 18);
            this.btnCopyMoveFiles.Name = "btnCopyMoveFiles";
            this.btnCopyMoveFiles.Size = new System.Drawing.Size(207, 42);
            this.btnCopyMoveFiles.TabIndex = 13;
            this.btnCopyMoveFiles.Text = "Copia / Sposta Files";
            this.btnCopyMoveFiles.UseVisualStyleBackColor = true;
            this.btnCopyMoveFiles.Click += new System.EventHandler(this.btnCopyMoveFiles_Click);
            // 
            // btnOpenFolderZip
            // 
            this.btnOpenFolderZip.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnOpenFolderZip.BackgroundImage = global::ScanFolderToFile.Properties.Resources.open;
            this.btnOpenFolderZip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnOpenFolderZip.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFolderZip.ForeColor = System.Drawing.Color.Red;
            this.btnOpenFolderZip.Location = new System.Drawing.Point(281, 247);
            this.btnOpenFolderZip.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenFolderZip.Name = "btnOpenFolderZip";
            this.btnOpenFolderZip.Size = new System.Drawing.Size(295, 54);
            this.btnOpenFolderZip.TabIndex = 13;
            this.btnOpenFolderZip.Text = "APRI CARTELLA ZIP";
            this.btnOpenFolderZip.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOpenFolderZip.UseVisualStyleBackColor = false;
            this.btnOpenFolderZip.Click += new System.EventHandler(this.btnOpenFolderZip_Click);
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnOpenFolder.BackgroundImage = global::ScanFolderToFile.Properties.Resources.open;
            this.btnOpenFolder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnOpenFolder.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFolder.ForeColor = System.Drawing.Color.Red;
            this.btnOpenFolder.Location = new System.Drawing.Point(13, 247);
            this.btnOpenFolder.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(245, 54);
            this.btnOpenFolder.TabIndex = 8;
            this.btnOpenFolder.Text = "APRI CARTELLA";
            this.btnOpenFolder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOpenFolder.UseVisualStyleBackColor = false;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // btnStampaFile
            // 
            this.btnStampaFile.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnStampaFile.BackgroundImage = global::ScanFolderToFile.Properties.Resources.print;
            this.btnStampaFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnStampaFile.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStampaFile.ForeColor = System.Drawing.Color.Red;
            this.btnStampaFile.Location = new System.Drawing.Point(491, 85);
            this.btnStampaFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnStampaFile.Name = "btnStampaFile";
            this.btnStampaFile.Size = new System.Drawing.Size(287, 73);
            this.btnStampaFile.TabIndex = 7;
            this.btnStampaFile.Text = "STAMPA FILE";
            this.btnStampaFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStampaFile.UseVisualStyleBackColor = false;
            this.btnStampaFile.Click += new System.EventHandler(this.btnStampaFile_Click);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnOpenFile.BackgroundImage = global::ScanFolderToFile.Properties.Resources.file;
            this.btnOpenFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnOpenFile.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFile.ForeColor = System.Drawing.Color.Red;
            this.btnOpenFile.Location = new System.Drawing.Point(491, 6);
            this.btnOpenFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(287, 71);
            this.btnOpenFile.TabIndex = 6;
            this.btnOpenFile.Text = "APRI FILE";
            this.btnOpenFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOpenFile.UseVisualStyleBackColor = false;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 314);
            this.Controls.Add(this.btnOpenFolderZip);
            this.Controls.Add(this.gbOptions);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnOpenFolder);
            this.Controls.Add(this.btnStampaFile);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.gbSelectFolder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scan Folder To File";
            this.gbSelectFolder.ResumeLayout(false);
            this.gbSelectFolder.PerformLayout();
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSelectedPath;
        private System.Windows.Forms.Button btnSfoglia;
        private System.Windows.Forms.FolderBrowserDialog fbd;
        private System.Windows.Forms.GroupBox gbSelectFolder;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Button btnStampaFile;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbOnlyExtensions;
        private System.Windows.Forms.CheckBox cbPdf;
        private System.Windows.Forms.GroupBox gbOptions;
        private System.Windows.Forms.CheckBox cbZipFolder;
        private System.Windows.Forms.Button btnOpenFolderZip;
        private System.Windows.Forms.Button btnCopyMoveFiles;
    }
}

