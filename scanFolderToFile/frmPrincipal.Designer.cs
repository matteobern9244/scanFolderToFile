namespace scanFolderToFile
{
    partial class frmPrincipal
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrincipal));
            this.txtSelectedPath = new System.Windows.Forms.TextBox();
            this.btnSfoglia = new System.Windows.Forms.Button();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.btnStampaFile = new System.Windows.Forms.Button();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cbOnlyExtensions = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSelectedPath
            // 
            this.txtSelectedPath.Enabled = false;
            this.txtSelectedPath.Location = new System.Drawing.Point(6, 19);
            this.txtSelectedPath.Name = "txtSelectedPath";
            this.txtSelectedPath.Size = new System.Drawing.Size(239, 28);
            this.txtSelectedPath.TabIndex = 1;
            // 
            // btnSfoglia
            // 
            this.btnSfoglia.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSfoglia.Location = new System.Drawing.Point(251, 19);
            this.btnSfoglia.Name = "btnSfoglia";
            this.btnSfoglia.Size = new System.Drawing.Size(75, 23);
            this.btnSfoglia.TabIndex = 2;
            this.btnSfoglia.Text = "SFOGLIA";
            this.toolTip1.SetToolTip(this.btnSfoglia, "seleziona la cartella");
            this.btnSfoglia.UseVisualStyleBackColor = true;
            this.btnSfoglia.Click += new System.EventHandler(this.btnSfoglia_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSelectedPath);
            this.groupBox1.Controls.Add(this.btnSfoglia);
            this.groupBox1.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(337, 51);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Seleziona cartella :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(15, 179);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "by Matteo Bernardini©";
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnOpenFolder.BackgroundImage = global::scanFolderToFile.Properties.Resources.open;
            this.btnOpenFolder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnOpenFolder.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFolder.ForeColor = System.Drawing.Color.Red;
            this.btnOpenFolder.Location = new System.Drawing.Point(12, 119);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(174, 44);
            this.btnOpenFolder.TabIndex = 8;
            this.btnOpenFolder.Text = "APRI CARTELLA";
            this.btnOpenFolder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.btnOpenFolder, "Apri cartella");
            this.btnOpenFolder.UseVisualStyleBackColor = false;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // btnStampaFile
            // 
            this.btnStampaFile.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnStampaFile.BackgroundImage = global::scanFolderToFile.Properties.Resources.print;
            this.btnStampaFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnStampaFile.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStampaFile.ForeColor = System.Drawing.Color.Red;
            this.btnStampaFile.Location = new System.Drawing.Point(355, 69);
            this.btnStampaFile.Name = "btnStampaFile";
            this.btnStampaFile.Size = new System.Drawing.Size(211, 59);
            this.btnStampaFile.TabIndex = 7;
            this.btnStampaFile.Text = "STAMPA FILE";
            this.btnStampaFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.btnStampaFile, "Stampa file");
            this.btnStampaFile.UseVisualStyleBackColor = false;
            this.btnStampaFile.Click += new System.EventHandler(this.btnStampaFile_Click);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnOpenFile.BackgroundImage = global::scanFolderToFile.Properties.Resources.file;
            this.btnOpenFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnOpenFile.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFile.ForeColor = System.Drawing.Color.Red;
            this.btnOpenFile.Location = new System.Drawing.Point(355, 5);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(211, 58);
            this.btnOpenFile.TabIndex = 6;
            this.btnOpenFile.Text = "APRI FILE";
            this.btnOpenFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.btnOpenFile, "Apri file");
            this.btnOpenFile.UseVisualStyleBackColor = false;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // cbOnlyExtensions
            // 
            this.cbOnlyExtensions.AutoSize = true;
            this.cbOnlyExtensions.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbOnlyExtensions.Location = new System.Drawing.Point(12, 78);
            this.cbOnlyExtensions.Name = "cbOnlyExtensions";
            this.cbOnlyExtensions.Size = new System.Drawing.Size(174, 24);
            this.cbOnlyExtensions.TabIndex = 10;
            this.cbOnlyExtensions.Text = "SOLO ESTENSIONI";
            this.cbOnlyExtensions.UseVisualStyleBackColor = true;
            this.cbOnlyExtensions.CheckedChanged += new System.EventHandler(this.cbOnlyExtensions_CheckedChanged);
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 203);
            this.Controls.Add(this.cbOnlyExtensions);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnOpenFolder);
            this.Controls.Add(this.btnStampaFile);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scan Folder To File";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSelectedPath;
        private System.Windows.Forms.Button btnSfoglia;
        private System.Windows.Forms.FolderBrowserDialog fbd;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Button btnStampaFile;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox cbOnlyExtensions;
    }
}

