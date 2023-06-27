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
            this.btnOpenFolderSelected = new System.Windows.Forms.Button();
            this.btnCreateFile = new System.Windows.Forms.Button();
            this.cbOnlyExtensions = new System.Windows.Forms.CheckBox();
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.cbNameFilesDuplicate = new System.Windows.Forms.CheckBox();
            this.cbZipFolder = new System.Windows.Forms.CheckBox();
            this.checkedListBoxFormatFile = new System.Windows.Forms.CheckedListBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.operazioniSuFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.apriToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stampaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.apriCartellaFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.altreOperazioniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copiaSpostaFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.riordinamentoFilesInCartellePerTipoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listaFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.storicoFileCreatiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbSelectFolder.SuspendLayout();
            this.gbOptions.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSelectedPath
            // 
            this.txtSelectedPath.Enabled = false;
            this.txtSelectedPath.Location = new System.Drawing.Point(10, 36);
            this.txtSelectedPath.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtSelectedPath.Name = "txtSelectedPath";
            this.txtSelectedPath.Size = new System.Drawing.Size(395, 34);
            this.txtSelectedPath.TabIndex = 1;
            // 
            // btnSfoglia
            // 
            this.btnSfoglia.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Italic);
            this.btnSfoglia.Location = new System.Drawing.Point(412, 31);
            this.btnSfoglia.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnSfoglia.Name = "btnSfoglia";
            this.btnSfoglia.Size = new System.Drawing.Size(125, 46);
            this.btnSfoglia.TabIndex = 2;
            this.btnSfoglia.Text = "Seleziona cartella...";
            this.btnSfoglia.UseVisualStyleBackColor = true;
            this.btnSfoglia.Click += new System.EventHandler(this.btnSfoglia_Click);
            // 
            // gbSelectFolder
            // 
            this.gbSelectFolder.Controls.Add(this.btnOpenFolderSelected);
            this.gbSelectFolder.Controls.Add(this.btnCreateFile);
            this.gbSelectFolder.Controls.Add(this.txtSelectedPath);
            this.gbSelectFolder.Controls.Add(this.btnSfoglia);
            this.gbSelectFolder.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbSelectFolder.Location = new System.Drawing.Point(14, 34);
            this.gbSelectFolder.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.gbSelectFolder.Name = "gbSelectFolder";
            this.gbSelectFolder.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.gbSelectFolder.Size = new System.Drawing.Size(705, 118);
            this.gbSelectFolder.TabIndex = 4;
            this.gbSelectFolder.TabStop = false;
            this.gbSelectFolder.Text = "Seleziona cartella da scansionare per creare il file :";
            // 
            // btnOpenFolderSelected
            // 
            this.btnOpenFolderSelected.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnOpenFolderSelected.BackgroundImage = global::ScanFolderToFile.Properties.Resources.open;
            this.btnOpenFolderSelected.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnOpenFolderSelected.Font = new System.Drawing.Font("Comic Sans MS", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFolderSelected.ForeColor = System.Drawing.Color.Red;
            this.btnOpenFolderSelected.Location = new System.Drawing.Point(9, 79);
            this.btnOpenFolderSelected.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnOpenFolderSelected.Name = "btnOpenFolderSelected";
            this.btnOpenFolderSelected.Size = new System.Drawing.Size(340, 33);
            this.btnOpenFolderSelected.TabIndex = 17;
            this.btnOpenFolderSelected.Text = "     APRI CARTELLA SELEZIONATA";
            this.btnOpenFolderSelected.UseVisualStyleBackColor = false;
            this.btnOpenFolderSelected.Click += new System.EventHandler(this.btnOpenFolderSelected_Click);
            // 
            // btnCreateFile
            // 
            this.btnCreateFile.Font = new System.Drawing.Font("Comic Sans MS", 10.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateFile.ForeColor = System.Drawing.Color.Blue;
            this.btnCreateFile.Location = new System.Drawing.Point(545, 31);
            this.btnCreateFile.Name = "btnCreateFile";
            this.btnCreateFile.Size = new System.Drawing.Size(149, 46);
            this.btnCreateFile.TabIndex = 3;
            this.btnCreateFile.Text = "GENERA FILE";
            this.btnCreateFile.UseVisualStyleBackColor = true;
            this.btnCreateFile.Click += new System.EventHandler(this.btnCreateFile_Click);
            // 
            // cbOnlyExtensions
            // 
            this.cbOnlyExtensions.AutoSize = true;
            this.cbOnlyExtensions.Font = new System.Drawing.Font("Comic Sans MS", 9F);
            this.cbOnlyExtensions.Location = new System.Drawing.Point(9, 29);
            this.cbOnlyExtensions.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cbOnlyExtensions.Name = "cbOnlyExtensions";
            this.cbOnlyExtensions.Size = new System.Drawing.Size(135, 24);
            this.cbOnlyExtensions.TabIndex = 10;
            this.cbOnlyExtensions.Text = "Solo Estensioni";
            this.cbOnlyExtensions.UseVisualStyleBackColor = true;
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.cbNameFilesDuplicate);
            this.gbOptions.Controls.Add(this.cbZipFolder);
            this.gbOptions.Controls.Add(this.cbOnlyExtensions);
            this.gbOptions.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.gbOptions.Location = new System.Drawing.Point(370, 154);
            this.gbOptions.Margin = new System.Windows.Forms.Padding(4);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Padding = new System.Windows.Forms.Padding(4);
            this.gbOptions.Size = new System.Drawing.Size(349, 112);
            this.gbOptions.TabIndex = 12;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Opzioni files :";
            // 
            // cbNameFilesDuplicate
            // 
            this.cbNameFilesDuplicate.AutoSize = true;
            this.cbNameFilesDuplicate.Font = new System.Drawing.Font("Comic Sans MS", 9F);
            this.cbNameFilesDuplicate.Location = new System.Drawing.Point(9, 79);
            this.cbNameFilesDuplicate.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cbNameFilesDuplicate.Name = "cbNameFilesDuplicate";
            this.cbNameFilesDuplicate.Size = new System.Drawing.Size(207, 24);
            this.cbNameFilesDuplicate.TabIndex = 13;
            this.cbNameFilesDuplicate.Text = "Check nomi files duplicati";
            this.cbNameFilesDuplicate.UseVisualStyleBackColor = true;
            // 
            // cbZipFolder
            // 
            this.cbZipFolder.AutoSize = true;
            this.cbZipFolder.Font = new System.Drawing.Font("Comic Sans MS", 9F);
            this.cbZipFolder.Location = new System.Drawing.Point(9, 54);
            this.cbZipFolder.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cbZipFolder.Name = "cbZipFolder";
            this.cbZipFolder.Size = new System.Drawing.Size(213, 24);
            this.cbZipFolder.TabIndex = 12;
            this.cbZipFolder.Text = "Creare Zip Cartella Scelta";
            this.cbZipFolder.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxFormatFile
            // 
            this.checkedListBoxFormatFile.CheckOnClick = true;
            this.checkedListBoxFormatFile.FormattingEnabled = true;
            this.checkedListBoxFormatFile.Location = new System.Drawing.Point(14, 161);
            this.checkedListBoxFormatFile.Name = "checkedListBoxFormatFile";
            this.checkedListBoxFormatFile.Size = new System.Drawing.Size(349, 108);
            this.checkedListBoxFormatFile.TabIndex = 14;
            this.checkedListBoxFormatFile.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxFormatFile_ItemCheck);
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.operazioniSuFileToolStripMenuItem,
            this.altreOperazioniToolStripMenuItem,
            this.listaFilesToolStripMenuItem,
            this.storicoFileCreatiToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(737, 30);
            this.menuStrip.TabIndex = 17;
            this.menuStrip.Text = "menuStrip1";
            // 
            // operazioniSuFileToolStripMenuItem
            // 
            this.operazioniSuFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.apriToolStripMenuItem,
            this.stampaToolStripMenuItem,
            this.apriCartellaFileToolStripMenuItem});
            this.operazioniSuFileToolStripMenuItem.Name = "operazioniSuFileToolStripMenuItem";
            this.operazioniSuFileToolStripMenuItem.Size = new System.Drawing.Size(141, 26);
            this.operazioniSuFileToolStripMenuItem.Text = "Operazioni su File";
            // 
            // apriToolStripMenuItem
            // 
            this.apriToolStripMenuItem.Name = "apriToolStripMenuItem";
            this.apriToolStripMenuItem.Size = new System.Drawing.Size(204, 26);
            this.apriToolStripMenuItem.Text = "Apri";
            this.apriToolStripMenuItem.Click += new System.EventHandler(this.apriToolStripMenuItem_Click);
            // 
            // stampaToolStripMenuItem
            // 
            this.stampaToolStripMenuItem.Name = "stampaToolStripMenuItem";
            this.stampaToolStripMenuItem.Size = new System.Drawing.Size(204, 26);
            this.stampaToolStripMenuItem.Text = "Stampa";
            this.stampaToolStripMenuItem.Click += new System.EventHandler(this.stampaToolStripMenuItem_Click);
            // 
            // apriCartellaFileToolStripMenuItem
            // 
            this.apriCartellaFileToolStripMenuItem.Name = "apriCartellaFileToolStripMenuItem";
            this.apriCartellaFileToolStripMenuItem.Size = new System.Drawing.Size(204, 26);
            this.apriCartellaFileToolStripMenuItem.Text = "Apri cartella files";
            this.apriCartellaFileToolStripMenuItem.Click += new System.EventHandler(this.apriCartellaFileToolStripMenuItem_Click);
            // 
            // altreOperazioniToolStripMenuItem
            // 
            this.altreOperazioniToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copiaSpostaFilesToolStripMenuItem,
            this.riordinamentoFilesInCartellePerTipoToolStripMenuItem});
            this.altreOperazioniToolStripMenuItem.Name = "altreOperazioniToolStripMenuItem";
            this.altreOperazioniToolStripMenuItem.Size = new System.Drawing.Size(56, 26);
            this.altreOperazioniToolStripMenuItem.Text = "Altro";
            // 
            // copiaSpostaFilesToolStripMenuItem
            // 
            this.copiaSpostaFilesToolStripMenuItem.Name = "copiaSpostaFilesToolStripMenuItem";
            this.copiaSpostaFilesToolStripMenuItem.Size = new System.Drawing.Size(348, 26);
            this.copiaSpostaFilesToolStripMenuItem.Text = "Copia / Sposta Files";
            this.copiaSpostaFilesToolStripMenuItem.Click += new System.EventHandler(this.copiaSpostaFilesToolStripMenuItem_Click);
            // 
            // riordinamentoFilesInCartellePerTipoToolStripMenuItem
            // 
            this.riordinamentoFilesInCartellePerTipoToolStripMenuItem.Name = "riordinamentoFilesInCartellePerTipoToolStripMenuItem";
            this.riordinamentoFilesInCartellePerTipoToolStripMenuItem.Size = new System.Drawing.Size(348, 26);
            this.riordinamentoFilesInCartellePerTipoToolStripMenuItem.Text = "Riordinamento files in cartelle per tipo";
            this.riordinamentoFilesInCartellePerTipoToolStripMenuItem.Click += new System.EventHandler(this.riordinamentoFilesInCartellePerTipoToolStripMenuItem_Click);
            // 
            // listaFilesToolStripMenuItem
            // 
            this.listaFilesToolStripMenuItem.Name = "listaFilesToolStripMenuItem";
            this.listaFilesToolStripMenuItem.Size = new System.Drawing.Size(177, 26);
            this.listaFilesToolStripMenuItem.Text = "Filtri Scansione Cartella";
            this.listaFilesToolStripMenuItem.Click += new System.EventHandler(this.listaFilesToolStripMenuItem_Click);
            // 
            // storicoFileCreatiToolStripMenuItem
            // 
            this.storicoFileCreatiToolStripMenuItem.Name = "storicoFileCreatiToolStripMenuItem";
            this.storicoFileCreatiToolStripMenuItem.Size = new System.Drawing.Size(140, 26);
            this.storicoFileCreatiToolStripMenuItem.Text = "Storico File Creati";
            this.storicoFileCreatiToolStripMenuItem.Click += new System.EventHandler(this.storicoFileCreatiToolStripMenuItem_Click);
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 276);
            this.Controls.Add(this.checkedListBoxFormatFile);
            this.Controls.Add(this.gbOptions);
            this.Controls.Add(this.gbSelectFolder);
            this.Controls.Add(this.menuStrip);
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
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSelectedPath;
        private System.Windows.Forms.Button btnSfoglia;
        private System.Windows.Forms.FolderBrowserDialog fbd;
        private System.Windows.Forms.GroupBox gbSelectFolder;
        private System.Windows.Forms.CheckBox cbOnlyExtensions;
        private System.Windows.Forms.GroupBox gbOptions;
        private System.Windows.Forms.CheckBox cbZipFolder;
        private System.Windows.Forms.CheckedListBox checkedListBoxFormatFile;
        private System.Windows.Forms.Button btnCreateFile;
        private System.Windows.Forms.CheckBox cbNameFilesDuplicate;
        private System.Windows.Forms.Button btnOpenFolderSelected;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem altreOperazioniToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copiaSpostaFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem riordinamentoFilesInCartellePerTipoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem storicoFileCreatiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem operazioniSuFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem apriToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stampaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem apriCartellaFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listaFilesToolStripMenuItem;
    }
}

