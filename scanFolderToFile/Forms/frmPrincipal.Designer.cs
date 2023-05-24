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
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.cbZipFolder = new System.Windows.Forms.CheckBox();
            this.btnCopyMoveFiles = new System.Windows.Forms.Button();
            this.btnOpenFolderZip = new System.Windows.Forms.Button();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.btnStampaFile = new System.Windows.Forms.Button();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.checkedListBoxFormatFile = new System.Windows.Forms.CheckedListBox();
            this.gbSelectFolder.SuspendLayout();
            this.gbOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSelectedPath
            // 
            this.txtSelectedPath.Enabled = false;
            this.txtSelectedPath.Location = new System.Drawing.Point(10, 34);
            this.txtSelectedPath.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtSelectedPath.Name = "txtSelectedPath";
            this.txtSelectedPath.Size = new System.Drawing.Size(395, 34);
            this.txtSelectedPath.TabIndex = 1;
            // 
            // btnSfoglia
            // 
            this.btnSfoglia.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSfoglia.Location = new System.Drawing.Point(419, 34);
            this.btnSfoglia.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnSfoglia.Name = "btnSfoglia";
            this.btnSfoglia.Size = new System.Drawing.Size(125, 42);
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
            this.gbSelectFolder.Location = new System.Drawing.Point(20, 22);
            this.gbSelectFolder.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.gbSelectFolder.Name = "gbSelectFolder";
            this.gbSelectFolder.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.gbSelectFolder.Size = new System.Drawing.Size(679, 94);
            this.gbSelectFolder.TabIndex = 4;
            this.gbSelectFolder.TabStop = false;
            this.gbSelectFolder.Text = "Seleziona cartella :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(827, 434);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "by Matteo Bernardini©";
            // 
            // cbOnlyExtensions
            // 
            this.cbOnlyExtensions.AutoSize = true;
            this.cbOnlyExtensions.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbOnlyExtensions.Location = new System.Drawing.Point(10, 34);
            this.cbOnlyExtensions.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cbOnlyExtensions.Name = "cbOnlyExtensions";
            this.cbOnlyExtensions.Size = new System.Drawing.Size(164, 30);
            this.cbOnlyExtensions.TabIndex = 10;
            this.cbOnlyExtensions.Text = "Solo Estensioni";
            this.cbOnlyExtensions.UseVisualStyleBackColor = true;
            this.cbOnlyExtensions.CheckedChanged += new System.EventHandler(this.cbOnlyExtensions_CheckedChanged);
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.cbZipFolder);
            this.gbOptions.Controls.Add(this.cbOnlyExtensions);
            this.gbOptions.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.gbOptions.Location = new System.Drawing.Point(20, 214);
            this.gbOptions.Margin = new System.Windows.Forms.Padding(4);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Padding = new System.Windows.Forms.Padding(4);
            this.gbOptions.Size = new System.Drawing.Size(289, 117);
            this.gbOptions.TabIndex = 12;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Opzioni";
            // 
            // cbZipFolder
            // 
            this.cbZipFolder.AutoSize = true;
            this.cbZipFolder.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbZipFolder.Location = new System.Drawing.Point(9, 76);
            this.cbZipFolder.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cbZipFolder.Name = "cbZipFolder";
            this.cbZipFolder.Size = new System.Drawing.Size(262, 30);
            this.cbZipFolder.TabIndex = 12;
            this.cbZipFolder.Text = "Creare Zip Cartella Scelta";
            this.cbZipFolder.UseVisualStyleBackColor = true;
            // 
            // btnCopyMoveFiles
            // 
            this.btnCopyMoveFiles.BackColor = System.Drawing.Color.White;
            this.btnCopyMoveFiles.ForeColor = System.Drawing.Color.Red;
            this.btnCopyMoveFiles.Location = new System.Drawing.Point(709, 215);
            this.btnCopyMoveFiles.Margin = new System.Windows.Forms.Padding(4);
            this.btnCopyMoveFiles.Name = "btnCopyMoveFiles";
            this.btnCopyMoveFiles.Size = new System.Drawing.Size(259, 63);
            this.btnCopyMoveFiles.TabIndex = 13;
            this.btnCopyMoveFiles.Text = "Copia / Sposta Files";
            this.btnCopyMoveFiles.UseVisualStyleBackColor = false;
            this.btnCopyMoveFiles.Click += new System.EventHandler(this.btnCopyMoveFiles_Click);
            // 
            // btnOpenFolderZip
            // 
            this.btnOpenFolderZip.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnOpenFolderZip.BackgroundImage = global::ScanFolderToFile.Properties.Resources.open;
            this.btnOpenFolderZip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnOpenFolderZip.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFolderZip.ForeColor = System.Drawing.Color.Red;
            this.btnOpenFolderZip.Location = new System.Drawing.Point(351, 370);
            this.btnOpenFolderZip.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnOpenFolderZip.Name = "btnOpenFolderZip";
            this.btnOpenFolderZip.Size = new System.Drawing.Size(369, 81);
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
            this.btnOpenFolder.Location = new System.Drawing.Point(16, 370);
            this.btnOpenFolder.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(306, 81);
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
            this.btnStampaFile.Location = new System.Drawing.Point(709, 86);
            this.btnStampaFile.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnStampaFile.Name = "btnStampaFile";
            this.btnStampaFile.Size = new System.Drawing.Size(264, 61);
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
            this.btnOpenFile.Location = new System.Drawing.Point(709, 9);
            this.btnOpenFile.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(264, 65);
            this.btnOpenFile.TabIndex = 6;
            this.btnOpenFile.Text = "APRI FILE";
            this.btnOpenFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOpenFile.UseVisualStyleBackColor = false;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // checkedListBoxFormatFile
            // 
            this.checkedListBoxFormatFile.CheckOnClick = true;
            this.checkedListBoxFormatFile.FormattingEnabled = true;
            this.checkedListBoxFormatFile.Location = new System.Drawing.Point(20, 125);
            this.checkedListBoxFormatFile.Name = "checkedListBoxFormatFile";
            this.checkedListBoxFormatFile.Size = new System.Drawing.Size(405, 82);
            this.checkedListBoxFormatFile.TabIndex = 14;
            this.checkedListBoxFormatFile.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxFormatFile_ItemCheck);
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 471);
            this.Controls.Add(this.checkedListBoxFormatFile);
            this.Controls.Add(this.btnCopyMoveFiles);
            this.Controls.Add(this.btnOpenFolderZip);
            this.Controls.Add(this.gbOptions);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnOpenFolder);
            this.Controls.Add(this.btnStampaFile);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.gbSelectFolder);
            this.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.MaximizeBox = false;
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scan Folder To File";
            this.Load += new System.EventHandler(this.FrmPrincipal_Load);
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
        private System.Windows.Forms.GroupBox gbOptions;
        private System.Windows.Forms.CheckBox cbZipFolder;
        private System.Windows.Forms.Button btnOpenFolderZip;
        private System.Windows.Forms.Button btnCopyMoveFiles;
        private System.Windows.Forms.CheckedListBox checkedListBoxFormatFile;
    }
}

