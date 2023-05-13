using System;
using System.Diagnostics;
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
            catch (Exception) { }
        }

        //PULSANTE APERTURA FILE
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Path.Combine(PathFolder, TxtFileFinal)))
                {
                    var psi = new ProcessStartInfo(Path.Combine(PathFolder, TxtFileFinal))
                    {
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                else
                {
                    MessageBox.Show(@"File non ancora creato.", @"FILE NON PRESENTE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception) { }
        }

        //PULSANTE APERTURA CARTELLA
        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(PathFolder))
                {
                    var psi = new ProcessStartInfo(PathFolder)
                    {
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                else
                {
                    MessageBox.Show(@"Cartella non ancora creata.", @"CARTELLA NON CREATA", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception) { }
        }

        //PULSANTE STAMPA FILE
        private void btnStampaFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Path.Combine(PathFolder, TxtFileFinal)))
                {
                    var psi = new ProcessStartInfo(Path.Combine(PathFolder, TxtFileFinal))
                    {
                        Verb = "print"
                    };
                    try
                    {
                        psi.Arguments = GetDefaultPrinterName();
                        if (psi.Arguments == "") return;
                        var print = new Process
                        {
                            StartInfo = psi
                        };
                        print.Start();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message != null)
                            MessageBox.Show($@"Si sono verificati problemi per la stampa : {ex.Message}");
                    }
                }
                else
                    MessageBox.Show(@"File non ancora creato.", @"FILE NON PRESENTE", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception) { }
        }

        private void cbOnlyExtensions_CheckedChanged(object sender, EventArgs e)
        {
            btnSfoglia.PerformClick();
        }
    }
}