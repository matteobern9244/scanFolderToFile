using System;
using System.IO;
using System.Linq;
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
            try
            {
                MessageBox.Show(AlertMessage, AlertTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (fbd.ShowDialog() != DialogResult.OK) return;

                CheckFolder();

                txtSelectedPath.Text = fbd.SelectedPath;

                var onlyExtension = cbOnlyExtensions.Checked;

                var content = Directory.GetFiles(fbd.SelectedPath, "*.*", SearchOption.AllDirectories)
                    .Where(files => !files.ToLower().Contains(DesktopIni))
                    .OrderBy(i => i)
                    .ToList();

                if (cbPdf.Checked)
                    CreateFilePdf(onlyExtension, content);
                else
                    CreateFileTxt(onlyExtension, content);

                MessageBox.Show(ElaborationConfirm, ElaborationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
            }
        }

        //PULSANTE APERTURA FILE
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        //PULSANTE APERTURA CARTELLA
        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            OpenFolder();
        }

        //PULSANTE STAMPA FILE
        private void btnStampaFile_Click(object sender, EventArgs e)
        {
            PrintFile();
        }

        private void cbOnlyExtensions_CheckedChanged(object sender, EventArgs e)
        {
            btnSfoglia.PerformClick();
        }
    }
}