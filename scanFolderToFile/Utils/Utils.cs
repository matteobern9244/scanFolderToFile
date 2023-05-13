using Spire.Pdf.Graphics;
using Spire.Pdf.Lists;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing.Printing;
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

        public static void CreateFileTxt(bool onlyExtension, List<string> content)
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

        //CARICA STAMPANTE DI DEFAULT
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
    }
}