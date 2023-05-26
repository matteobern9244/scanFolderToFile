namespace ScanFolderToFile.Forms.OptionsForms
{
    partial class FrmCheckNameFilesDuplicate
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
            this.lblNameFilesDuplicate = new System.Windows.Forms.Label();
            this.lvNameFilesDuplicate = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // lblNameFilesDuplicate
            // 
            this.lblNameFilesDuplicate.AutoSize = true;
            this.lblNameFilesDuplicate.Location = new System.Drawing.Point(12, 9);
            this.lblNameFilesDuplicate.Name = "lblNameFilesDuplicate";
            this.lblNameFilesDuplicate.Size = new System.Drawing.Size(174, 24);
            this.lblNameFilesDuplicate.TabIndex = 0;
            this.lblNameFilesDuplicate.Text = "Nomi files duplicati :";
            // 
            // lvNameFilesDuplicate
            // 
            this.lvNameFilesDuplicate.FullRowSelect = true;
            this.lvNameFilesDuplicate.GridLines = true;
            this.lvNameFilesDuplicate.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvNameFilesDuplicate.HideSelection = false;
            this.lvNameFilesDuplicate.Location = new System.Drawing.Point(12, 36);
            this.lvNameFilesDuplicate.MultiSelect = false;
            this.lvNameFilesDuplicate.Name = "lvNameFilesDuplicate";
            this.lvNameFilesDuplicate.Size = new System.Drawing.Size(711, 232);
            this.lvNameFilesDuplicate.TabIndex = 1;
            this.lvNameFilesDuplicate.UseCompatibleStateImageBehavior = false;
            // 
            // FrmCheckNameFilesDuplicate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 280);
            this.Controls.Add(this.lvNameFilesDuplicate);
            this.Controls.Add(this.lblNameFilesDuplicate);
            this.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCheckNameFilesDuplicate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nomi files duplicati";
            this.Load += new System.EventHandler(this.FrmCheckNameFilesDuplicate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNameFilesDuplicate;
        private System.Windows.Forms.ListView lvNameFilesDuplicate;
    }
}