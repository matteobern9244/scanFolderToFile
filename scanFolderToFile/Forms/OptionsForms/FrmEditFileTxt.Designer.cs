namespace ScanFolderToFile.Forms.OptionsForms
{
    partial class FrmEditFileTxt
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
            this.richTextBoxFileTxt = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBoxFileTxt
            // 
            this.richTextBoxFileTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxFileTxt.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxFileTxt.Name = "richTextBoxFileTxt";
            this.richTextBoxFileTxt.Size = new System.Drawing.Size(782, 553);
            this.richTextBoxFileTxt.TabIndex = 0;
            this.richTextBoxFileTxt.Text = "";
            // 
            // FrmEditFileTxt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.richTextBoxFileTxt);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmEditFileTxt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edita file txt";
            this.Load += new System.EventHandler(this.FrmEditFileTxt_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxFileTxt;
    }
}