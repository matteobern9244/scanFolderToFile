using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Pdf.Lists;
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

        private void CreateFilePdf(bool onlyExtension, List<string> content)
        {
            var doc = new PdfDocument();

            //Set the margins
            var margins = new PdfMargins(30);

            //Add a page
            var page = doc.Pages.Add(PdfPageSize.A4, margins);

            //Create a brush
            var brush = PdfBrushes.Black;

            //Create two fonts
            var titleFont = new PdfFont(PdfFontFamily.TimesRoman, 12f, PdfFontStyle.Bold);
            var listFont = new PdfFont(PdfFontFamily.TimesRoman, 12f, PdfFontStyle.Regular);

            //Specify the initial coordinate
            float x = 0;
            float y = 0;

            //Draw title
            var title = "CONTENT";
            page.Canvas.DrawString(title, titleFont, brush, x, y);
            y = y + (float)titleFont.MeasureString(title).Height;
            y = y + 5;

            var list = new PdfSortedList(string.Join(Environment.NewLine, content))
            {
                //Set the font, indent, text indent, brush of the list
                Font = listFont,
                Indent = 2,
                TextIndent = 4,
                Brush = brush
            };

            //Draw list on the page at the specified location
            list.Draw(page, 0, y);

            //Save to file
            doc.SaveToFile(Path.Combine(PathFolder, PdfFileFinal));
        }

        private void CreateFileTxt(bool onlyExtension, List<string> content)
        {
            var file = new StreamWriter(Path.Combine(PathFolder, TxtFileFinal));

            var listElement = new List<string>();

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

        //CARICA STAMPANTE DI DEFAULT
        private static string GetDefaultPrinterName()
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