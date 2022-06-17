using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace scanFolderToFile
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private static void CreateFolder()
        {
            try
            {
                if (!(Directory.Exists(Constants.PathFolder)))
                    Directory.CreateDirectory(Constants.PathFolder);
            }
            catch (Exception) { }
        }

        //PULSANTE SFOGLIA
        private void btnSfoglia_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(Constants.AlertMessage, Constants.AlertTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (fbd.ShowDialog() != DialogResult.OK) return;

                CreateFolder();

                var pathFiles = Directory.GetFiles(fbd.SelectedPath);
                txtSelectedPath.Text = fbd.SelectedPath;

                MessageBox.Show(Constants.ElaborationConfirm, Constants.ElaborationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

                var file = new StreamWriter(Path.Combine(Constants.PathFolder, Constants.FileFinal));

                var onlyExtension = cbOnlyExtensions.Checked;

                var listElement = new List<string>();

                var content = Directory.GetFiles(fbd.SelectedPath, "*.*", SearchOption.AllDirectories)
                                            .Where(files => !files.ToLower().Contains("desktop.ini"))
                                            .OrderBy(i => i)
                                            .ToList();
                if (content.Any())
                {
                    foreach (var sFile in content)
                    {
                        if (onlyExtension)
                        {
                            var extension = Path.GetExtension(sFile).Trim();
                            if (!listElement.Contains(extension))
                                listElement.Add(extension);
                        }
                        else
                            file.WriteLine(Path.GetFileName(sFile));
                    }

                    if (listElement.Any())
                    {
                        foreach (var extension in listElement)
                        {
                            file.WriteLine(extension);
                        }
                    }
                }

                file.Close();
            }
            catch (Exception) { }
        }

        //PULSANTE APERTURA FILE
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Path.Combine(Constants.PathFolder, Constants.FileFinal)))
                {
                    var psi = new ProcessStartInfo(Path.Combine(Constants.PathFolder, Constants.FileFinal))
                    {
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                else
                {
                    MessageBox.Show("File non ancora creato.", "FILE NON PRESENTE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception) { }
        }

        //PULSANTE APERTURA CARTELLA
        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(Constants.PathFolder))
                {
                    var psi = new ProcessStartInfo(Constants.PathFolder)
                    {
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                else
                {
                    MessageBox.Show("Cartella non ancora creata.", "CARTELLA NON PRESENTE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception) { }
        }

        //CARICA STAMPANTE DI DEFAULT
        private string GetDefaultPrinterName()
        {
            try
            {
                for (var i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
                {
                    var pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
                    var settings = new PrinterSettings
                    {
                        PrinterName = pkInstalledPrinters
                    };
                    if (settings.IsDefaultPrinter) 
                        return pkInstalledPrinters;
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        //PULSANTE STAMPA FILE
        private void btnStampaFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Path.Combine(Constants.PathFolder, Constants.FileFinal)))
                {
                    var psi = new ProcessStartInfo(Path.Combine(Constants.PathFolder, Constants.FileFinal))
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
                            MessageBox.Show($"Si sono verificati problemi per la stampa : {ex.Message}");
                    }
                }
                else
                    MessageBox.Show("File non ancora creato.", "FILE NON PRESENTE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
            catch (Exception) { }
        }

        private void cbOnlyExtensions_CheckedChanged(object sender, EventArgs e)
        {
            btnSfoglia.PerformClick();
        }
    }
}