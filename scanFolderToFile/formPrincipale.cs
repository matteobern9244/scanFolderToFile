using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace scanFolderToFile
{
    public partial class formPrincipale : Form
    {
        public formPrincipale()
        {
            InitializeComponent();
        }

        private void createFolder()
        {
            try
            {
                if (!(Directory.Exists(Constants.PATH_FOLDER)))
                    Directory.CreateDirectory(Constants.PATH_FOLDER);
            }
            catch (Exception) { }
        }

        //PULSANTE SFOGLIA
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(Constants.ALERT_MESSAGE, Constants.ALERT_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    createFolder();
                    var pathFiles = Directory.GetFiles(folderBrowserDialog1.SelectedPath);
                    textBox1.Text = folderBrowserDialog1.SelectedPath;

                    MessageBox.Show(Constants.ELABORATION_CONFIRM, Constants.ELABORATION_TITLE , MessageBoxButtons.OK, MessageBoxIcon.Information);

                    var file = new StreamWriter(Path.Combine(Constants.PATH_FOLDER, Constants.FILE_FINAL));

                    var onlyExtension = cbOnlyExtensions.Checked;
                    
                    var listExtensions = new List<string>();

                    foreach (var sFile in Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.*", SearchOption.AllDirectories))
                    {
                        if(onlyExtension)
                        {
                            var extension = Path.GetExtension(sFile).Trim();
                            if(!listExtensions.Contains(extension))
                                listExtensions.Add(extension);
                        }
                        else
                            file.WriteLine(Path.GetFileName(sFile));
                    }
                    
                    if(listExtensions != null && listExtensions.Any())
                    {
                        foreach (var extension in listExtensions)
                        {
                            file.WriteLine(extension);
                        }
                    }

                    file.Close();

                }
            }
            catch (Exception) { }
        }

        //PULSANTE APERTURA FILE
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Path.Combine(Constants.PATH_FOLDER, Constants.FILE_FINAL)))
                {
                    ProcessStartInfo psi;
                    psi = new ProcessStartInfo(Path.Combine(Constants.PATH_FOLDER, Constants.FILE_FINAL))
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
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(Constants.PATH_FOLDER))
                {
                    ProcessStartInfo psi;
                    psi = new ProcessStartInfo(Constants.PATH_FOLDER)
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
                String pkInstalledPrinters;
                for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
                {
                    pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
                    PrinterSettings settings = new PrinterSettings
                    {
                        PrinterName = pkInstalledPrinters
                    };
                    if (settings.IsDefaultPrinter) return pkInstalledPrinters;
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        //PULSANTE STAMPA FILE
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Path.Combine(Constants.PATH_FOLDER, Constants.FILE_FINAL)))
                {
                    var psi = new ProcessStartInfo(Path.Combine(Constants.PATH_FOLDER, Constants.FILE_FINAL))
                    {
                        Verb = "print"
                    };
                    try
                    {
                        psi.Arguments = GetDefaultPrinterName();
                        if (!(psi.Arguments == ""))
                        {
                            var print = new Process
                            {
                                StartInfo = psi
                            };
                            print.Start();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Si sono verificati problemi per la stampa : " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("File non ancora creato.", "FILE NON PRESENTE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception) { }
        }

        private void cbOnlyExtensions_CheckedChanged(object sender, EventArgs e)
        {
            button1.PerformClick();
        }
    }
}
