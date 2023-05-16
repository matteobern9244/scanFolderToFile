using ScanFolderToFile.Utils;
using System;
using System.Windows.Forms;
using static ScanFolderToFile.Constants;
using static ScanFolderToFile.Utils.Utils;

namespace ScanFolderToFile
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

        //PULSANTE APERTURA FILE
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFile(cbPdf.Checked);
        }

        //PULSANTE APERTURA CARTELLA
        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            OpenFolder();
        }

        //PULSANTE STAMPA FILE
        private void btnStampaFile_Click(object sender, EventArgs e)
        {
            PrintFile(cbPdf.Checked);
        }

        private void cbOnlyExtensions_CheckedChanged(object sender, EventArgs e)
        {
            btnSfoglia.PerformClick();
        }

        private void Browse()
        {
            try
            {
                MessageBox.Show(AlertMessage, AlertTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (fbd.ShowDialog() != DialogResult.OK) return;

                CheckFolder();

                txtSelectedPath.Text = fbd.SelectedPath;

                var content = GetContentFromFolderSelected(fbd.SelectedPath, cbOnlyExtensions.Checked);

                if (cbPdf.Checked)
                    CreateFilePdf(content);
                else
                    CreateFileTxt(content);

                MessageBox.Show(ElaborationConfirm, ElaborationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(Browse), "Errore in sfoglia contenuto.");
            }
        }
    }
}