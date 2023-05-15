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

        public static void CreateFilePdf(bool onlyExtension, List<string> content)
        {
            try
            {
                //TODO: gestire onlyExtension


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

                ScanFolderToFileLogger.Info(nameof(CreateFilePdf),
                    $"Creato file {PdfFileFinal} in {PathFolder}");
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(CreateFilePdf), "Errore in creazione del file PDF.");
            }
        }

        public static void CreateFileTxt(bool onlyExtension, List<string> content)
        {
            try
            {
                using (var file = new StreamWriter(Path.Combine(PathFolder, TxtFileFinal)))
                {
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

        public static void OpenFile()
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

        public static void PrintFile()
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