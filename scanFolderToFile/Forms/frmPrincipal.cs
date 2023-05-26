using System;
using System.Windows.Forms;
using ScanFolderToFile.Forms.OptionsForms;
using System.Collections.Generic;
using ScanFolderToFile.Utils;
using static ScanFolderToFile.Constants;
using static ScanFolderToFile.Utils.Utils;

namespace ScanFolderToFile.Forms
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        //PULSANTE SFOGLIA
        private void btnSfoglia_Click(object sender, EventArgs e)
        {
            Browse(cbZipFolder.Checked);
        }

        //PULSANTE APERTURA FILE
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFile(PdfChecked(), MarkdownChecked());
        }

        //PULSANTE APERTURA CARTELLA
        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            OpenFolder(PathFolder);
        }

        //PULSANTE STAMPA FILE
        private void btnStampaFile_Click(object sender, EventArgs e)
        {
            PrintFile(PdfChecked(), MarkdownChecked());
        }

        private void cbOnlyExtensions_CheckedChanged(object sender, EventArgs e)
        {
            btnSfoglia.PerformClick();
        }

        private void Browse(bool zipFolder)
        {
            try
            {
                if (fbd.ShowDialog() != DialogResult.OK) return;

                txtSelectedPath.Text = fbd.SelectedPath;
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(Browse), "Errore in sfoglia contenuto.");
            }
        }

        private void GenerateFile(bool zipFolder, bool checkNameFilesDuplicate)
        {
            if (zipFolder)
                CreateZipFolder(txtSelectedPath.Text);
            else
            {
                CheckFolder(PathFolder);
                MessageBox.Show(AlertMessage, AlertTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                var content = GetContentFromFolderSelected(txtSelectedPath.Text, cbOnlyExtensions.Checked);

                if (PdfChecked())
                    CreateFilePdf(content);
                if (MarkdownChecked())
                    CreateFileMarkdown(content);
                else
                    CreateFileTxt(content);

                if (checkNameFilesDuplicate)
                    CheckNameFilesDuplicate(content);

                MessageBox.Show(ElaborationConfirm, ElaborationTitle, MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void FillAndCheckFirstElementCbLb()
        {
            checkedListBoxFormatFile.DataSource = new List<string>
            {
                FileTxt,
                FilePdf,
                FileMarkdown
            };

            //Set default create file TXT
            checkedListBoxFormatFile.SetItemChecked(0, true);
        }

        private void btnOpenFolderZip_Click(object sender, EventArgs e)
        {
            OpenFolder(PathFolderZip);
        }

        private void btnCopyMoveFiles_Click(object sender, EventArgs e)
        {
            var frmCopyMoveFiles = new FrmCopyMoveFiles();
            frmCopyMoveFiles.ShowDialog();
        }

        private void checkedListBoxFormatFile_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckOnlyOneElements(checkedListBoxFormatFile, e);
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            FillAndCheckFirstElementCbLb();
        }

        private bool PdfChecked()
        {
            return GetChoiceCheckedListBox(checkedListBoxFormatFile).Equals(FilePdf);
        }
        private bool MarkdownChecked()
        {
            return GetChoiceCheckedListBox(checkedListBoxFormatFile).Equals(FileMarkdown);
        }

        private void btnCreateFile_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSelectedPath.Text.Trim()))
                GenerateFile(cbZipFolder.Checked, cbNameFilesDuplicate.Checked);
            else
                MessageBox.Show(AlertMissingScanFolder, AlertTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void buttonReorderFilesInFolderByType_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSelectedPath.Text.Trim()))
                ReorderFilesInFolderByType(txtSelectedPath.Text.Trim());
        }

        private void btnOpenFolderSelected_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSelectedPath.Text.Trim()))
                OpenFolder(txtSelectedPath.Text.Trim());
        }
    }
}