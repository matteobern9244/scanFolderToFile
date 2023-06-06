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
            Browse();
        }

        private void Browse()
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

                if (!PdfChecked() && !MarkdownChecked())
                    CreateFileTxt(content);

                if (checkNameFilesDuplicate)
                    CheckNameFilesDuplicate(content);

                MessageBox.Show(ElaborationConfirm, ElaborationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            {
                GenerateFile(cbZipFolder.Checked, cbNameFilesDuplicate.Checked);

                if (PdfChecked() || MarkdownChecked()) return;
                var frmEditFileTxt = new FrmEditorFileTxt();
                frmEditFileTxt.ShowDialog();
            }
            else
                MessageBox.Show(AlertMissingScanFolder, AlertTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnOpenFolderSelected_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSelectedPath.Text.Trim()))
                OpenFolder(txtSelectedPath.Text.Trim());
        }

        //Menù
        private void copiaSpostaFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmCopyMoveFiles = new FrmCopyMoveFiles();
            frmCopyMoveFiles.ShowDialog();
        }

        private void riordinamentoFilesInCartellePerTipoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSelectedPath.Text.Trim()))
                ReorderFilesInFolderByType(txtSelectedPath.Text.Trim());
        }

        private void storicoFileCreatiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenHistoryFileCreated();
        }

        private void apriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile(PdfChecked(), MarkdownChecked());
        }

        private void stampaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintFile(PdfChecked(), MarkdownChecked());
        }

        private void apriCartellaFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFolder(PathFolder);
        }
    }
}