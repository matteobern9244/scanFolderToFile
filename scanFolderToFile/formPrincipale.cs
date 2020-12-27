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

        private void creaCartella()
        {
            if (!(Directory.Exists(@"C:\CONTENT SELECTED")))
            {
                Directory.CreateDirectory(@"C:\CONTENT SELECTED");
            }

        }

        //PULSANTE SFOGLIA
        private void button1_Click(object sender, EventArgs e)
        {
           // MessageBox.Show("Non selezionare una cartella dove al suo interno sono presenti sottocartelle!","ATTENZIONE",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            MessageBox.Show("Verra' creata la cartella 'CONTENT SELECTED' in C: con il file 'CONTENUTO.TXT'", "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                creaCartella();
                string[] pathFiles = Directory.GetFiles(folderBrowserDialog1.SelectedPath);
                textBox1.Text = folderBrowserDialog1.SelectedPath;

                MessageBox.Show("Files trovati: " + pathFiles.Length.ToString(), "FILES TROVATI", MessageBoxButtons.OK, MessageBoxIcon.Information);

                StreamWriter file = new StreamWriter(@"C:\\CONTENT SELECTED\\CONTENUTO.txt");

                foreach (string nomeFile in Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.*", SearchOption.AllDirectories))
                {
                    file.WriteLine(Path.GetFileName(nomeFile));
                }

                file.Close();

            }
        }

        //PULSANTE APERTURA FILE
        private void button2_Click(object sender, EventArgs e)
        {
            string path = "C:\\CONTENT SELECTED\\CONTENUTO.txt";

            if (File.Exists(path))
            {
                ProcessStartInfo psi;
                psi = new ProcessStartInfo(@"C:\\CONTENT SELECTED\\CONTENUTO.txt");
                psi.UseShellExecute = true;
                Process.Start(psi);
            }
            else
            {
                MessageBox.Show("File non ancora creato.", "FILE NON PRESENTE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //PULSANTE APERTURA CARTELLA
        private void button4_Click(object sender, EventArgs e)
        {
            string path = "C:\\CONTENT SELECTED";
            if (Directory.Exists(path))
            {
                ProcessStartInfo psi;
                psi = new ProcessStartInfo(@"C:\\CONTENT SELECTED");
                psi.UseShellExecute = true;
                Process.Start(psi);
            }
            else
            {
                MessageBox.Show("Cartella non ancora creata.","CARTELLA NON PRESENTE",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        //CARICA STAMPANTE DI DEFAULT
        private string GetDefaultPrinterName()
        {
            String pkInstalledPrinters;
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
                PrinterSettings settings = new PrinterSettings();
                settings.PrinterName = pkInstalledPrinters;
                if (settings.IsDefaultPrinter) return pkInstalledPrinters;
            }
            return "";
        }

        //PULSANTE STAMPA FILE
        private void button3_Click(object sender, EventArgs e)
        {
            string path = "C:\\CONTENT SELECTED\\CONTENUTO.txt";
            if (File.Exists(path))
            {
                ProcessStartInfo psi = new ProcessStartInfo(@"C:\\CONTENT SELECTED\\CONTENUTO.txt");
                psi.Verb = "print";
                try
                {
                    psi.Arguments = GetDefaultPrinterName();
                    if (!(psi.Arguments == ""))
                    {
                        Process print = new Process();
                        print.StartInfo = psi;
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
    }
}
  