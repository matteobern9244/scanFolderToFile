using Spire.Pdf.Graphics;
using Spire.Pdf.Lists;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing.Printing;
using System.Diagnostics;
using System.Windows.Forms;
using static ScanFolderToFile.Constants;

namespace ScanFolderToFile.Utils
{
    public static class Utils
    {
        public static void CheckFolder()
        {
            try
            {
                if (Directory.Exists(PathFolder)) return;
                Directory.CreateDirectory(PathFolder);
                ScanFolderToFileLogger.Info(nameof(CheckFolder),
                    $"Non esiste la cartella di destinazione, creata cartella {PathFolder}");
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(CheckFolder), $"Errore in creazione cartella {PathFolder}");
            }
        }

        public static void CreateFilePdf(List<string> content)
        {
            try
            {
                //Create the pdf document
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
                y = y + titleFont.MeasureString(title).Height;
                y = y + 5;

                var list = new PdfSortedList(string.Join(Environment.NewLine, content.Select(Path.GetFileName).ToList()))
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

                ScanFolderToFileLogger.Info(nameof(CreateFilePdf),
                    $"Creato file {PdfFileFinal} in {PathFolder}");
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(CreateFilePdf), "Errore in creazione del file PDF.");
            }
        }

        public static List<string> GetContentFromFolderSelected(string pathFolderSelected, bool onlyExtension)
        {
            var content = new List<string>();
            try
            {
                content = Directory.GetFiles(pathFolderSelected, "*.*", SearchOption.AllDirectories)
                    .Where(files => !files.ToLower().Contains(DesktopIni))
                    .OrderBy(i => i)
                    .ToList();

                if (onlyExtension)
                {
                    var contentOnlyExtension = new List<string>();
                    foreach (var extension in content
                                 .Select(sFile => Path.GetExtension(sFile).Trim())
                                 .Where(extension => !contentOnlyExtension.Contains(extension)))
                    {
                        contentOnlyExtension.Add(extension);
                    }

                    return contentOnlyExtension;
                }
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(GetContentFromFolderSelected),
                    "Errore in estrapolazione contenuto cartella.");
            }

            return content;
        }

        public static void CreateFileTxt(List<string> content)
        {
            try
            {
                using (var file = new StreamWriter(Path.Combine(PathFolder, TxtFileFinal)))
                {
                    if (!content.Any()) return;
                    content.ForEach(x => file.WriteLine(Path.GetFileName(x)));
                    ScanFolderToFileLogger.Info(nameof(CreateFileTxt),
                        $"Creato file {TxtFileFinal} in {PathFolder}");
                }
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(CreateFileTxt), "Errore in creazione del file TXT.");
            }
        }

        public static string GetDefaultPrinterName()
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
                    if (!settings.IsDefaultPrinter) continue;
                    ScanFolderToFileLogger.Info(nameof(GetDefaultPrinterName),
                        $"Recuperato stampante di default {pkInstalledPrinters}");
                    return pkInstalledPrinters;

                }
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(GetDefaultPrinterName),
                    "Errore in recupero stampante di default.");
            }
            return string.Empty;
        }

        public static void OpenFile(bool pdf)
        {
            try
            {
                var file = pdf ? PdfFileFinal : TxtFileFinal;
                if (File.Exists(Path.Combine(PathFolder, file)))
                {
                    var psi = new ProcessStartInfo(Path.Combine(PathFolder, file))
                    {
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                else
                {
                    MessageBox.Show(@"File non ancora creato.", @"FILE NON PRESENTE", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(OpenFile), "Errore in apertura file.");
            }
        }

        public static void OpenFolder()
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
                    MessageBox.Show(@"Cartella non ancora creata.", @"CARTELLA NON CREATA", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(OpenFolder), "Errore in apertura cartella.");
            }
        }

        public static void PrintFile(bool pdf)
        {
            try
            {
                var file = pdf ? PdfFileFinal : TxtFileFinal;
                if (File.Exists(Path.Combine(PathFolder, file)))
                {
                    var psi = new ProcessStartInfo(Path.Combine(PathFolder, file))
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
                    MessageBox.Show(@"File non ancora creato.", @"FILE NON PRESENTE", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(PrintFile), "Errore in stampa file.");
            }
        }
    }
}