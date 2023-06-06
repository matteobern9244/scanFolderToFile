using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Pdf.Lists;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using ScanFolderToFile.Forms.OptionsForms;
using ScanFolderToFile.MarkdownOut;
using ScanFolderToFile.Model;
using Path = System.IO.Path;
using static ScanFolderToFile.Constants;

namespace ScanFolderToFile.Utils
{
    public static class Utils
    {
        public static void CheckFolder(string pathFolder)
        {
            try
            {
                if (Directory.Exists(pathFolder)) return;
                Directory.CreateDirectory(pathFolder);
                ScanFolderToFileLogger.Info(nameof(CheckFolder),
                    $"Non esiste la cartella di destinazione, creata cartella {pathFolder}");
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(CheckFolder), $"Errore in creazione cartella {pathFolder}");
            }
        }

        public static void CreateFileMarkdown(List<string> content)
        {
            try
            {
                using (var mdWriter = new MdWriter(Path.Combine(PathFolder, MarkdownFileFinal)))
                {
                    mdWriter.WriteLineSingle(Content, MdStyle.Bold, MdFormat.Heading1);
                    mdWriter.WriteLine(string.Join(Environment.NewLine, content.Select(Path.GetFileName).ToList()));
                }

                AddFileInHistory(MarkdownFileFinal, ExtMd);
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(CreateFileMarkdown), "Errore in creazione del file Markdown.");
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
                const string title = "CONTENT";
                page.Canvas.DrawString(title, titleFont, brush, x, y);
                y += titleFont.MeasureString(title).Height;
                y += 5;

                var list = new PdfSortedList(
                    string.Join(Environment.NewLine, content.Select(Path.GetFileName).ToList()))
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

                ScanFolderToFileLogger.Info(nameof(CreateFilePdf), $"Creato file {PdfFileFinal} in {PathFolder}", true,
                    "FILE PDF CREATO");

                AddFileInHistory(PdfFileFinal, ExtPdf);
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(CreateFilePdf), "Errore in creazione del file PDF.");
            }
        }

        public static void CreateZipFolder(string pathFolderSelected)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                CheckFolder(PathFolderZip);
                var zipPath = Path.Combine(PathFolderZip, ZipFileFinal);
                ZipFile.CreateFromDirectory(pathFolderSelected, zipPath, CompressionLevel.Optimal,
                    false);
                ScanFolderToFileLogger.Info(nameof(CreateZipFolder), $"Creato file {ZipFileFinal} in {PathFolderZip}",
                    true, "FILE ZIP CREATO");
                AddFileInHistory(ZipFileFinal, ExtZip);
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(CreateZipFolder), "Errore in creazione file zip.");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        public static List<string> GetContentFromFolderSelected(string pathFolderSelected, bool onlyExtension = false)
        {
            var content = new List<string>();
            try
            {
                content = Directory.GetFiles(pathFolderSelected, "*.*", SearchOption.AllDirectories)
                    .Where(files => !files.ToLower().Contains(DesktopIni))
                    .OrderBy(i => i)
                    .ToList();
                if (content.Any())
                {
                    if (onlyExtension)
                        return GetExtensionFromContentFromFolderSelected(content);
                }
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(GetContentFromFolderSelected),
                    "Errore in estrapolazione contenuto cartella.");
            }

            return content;
        }

        public static List<string> GetExtensionFromContentFromFolderSelected(List<string> content)
        {
            return content
                .Select(sFile => Path.GetExtension(sFile).Trim())
                .Distinct()
                .ToList();
        }

        public static void CheckNameFilesDuplicate(List<string> content)
        {
            try
            {
                var listKeyValuePairs =
                    (from pathFile in content
                     let onlyNameFile = Path.GetFileNameWithoutExtension(pathFile)
                     let ext = Path.GetExtension(pathFile)
                     select new KeyValuePair<string, string>(onlyNameFile, ext))
                    .ToList();

                var duplicates = listKeyValuePairs
                    .GroupBy(i => i.Key)
                    .Where(g => g.Count() > 1)
                    .Select(i => i)
                    .ToList();

                if (!duplicates.Any()) return;

                var frmCheckNameFilesDuplicate = new FrmCheckNameFilesDuplicate(duplicates);
                frmCheckNameFilesDuplicate.ShowDialog();
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(CheckNameFilesDuplicate), "Errore in check dei nomi dei files duplicati.");
            }
        }

        private static void AddFileInHistory(string fileName, string extensionFile)
        {
            try
            {
                var history = DeserializeHistoryFilesCreated() ?? new List<HistoryFilesCreated>();
                history.Add(new HistoryFilesCreated()
                {
                    NameFile = fileName,
                    ExtensionFile = extensionFile,
                    CreatedAt = DateTime.Now
                });
                File.WriteAllText(PathHistoryFileCreated, JsonConvert.SerializeObject(history, Formatting.Indented));
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(AddFileInHistory), "Errore in creazione file json.");
            }
        }

        public static void OpenHistoryFileCreated()
        {
            try
            {
                using (var frmHistoryFileCreated = new FrmHistoryFileCreated(DeserializeHistoryFilesCreated()))
                {
                    frmHistoryFileCreated.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(OpenHistoryFileCreated), "Errore in apertura storico file creati.");
            }
        }


        private static List<HistoryFilesCreated> DeserializeHistoryFilesCreated()
        {
            try
            {
                if (File.Exists(PathHistoryFileCreated))
                {
                    return JsonConvert.DeserializeObject<List<HistoryFilesCreated>>(File.ReadAllText(PathHistoryFileCreated))
                        .OrderByDescending(x => x.CreatedAt)
                        .ToList();
                }

            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(DeserializeHistoryFilesCreated), "Errore in deserializzazione file json.");
            }

            return null;
        }

        public static void CreateFileTxt(List<string> content)
        {
            try
            {
                using (var file = new StreamWriter(Path.Combine(PathFolder, TxtFileFinal)))
                {
                    if (!content.Any()) return;
                    content.ForEach(x => file.WriteLine(Path.GetFileName(x)));
                    ScanFolderToFileLogger.Info(nameof(CreateFileTxt), $"Creato file {TxtFileFinal} in {PathFolder}",
                        true, "FILE TXT CREATO");
                    AddFileInHistory(TxtFileFinal, ExtTxt);
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

        public static void OpenFile(bool pdf, bool markdown)
        {
            try
            {
                var file = TxtFileFinal;
                if (pdf)
                    file = PdfFileFinal;
                if (markdown)
                    file = MarkdownFileFinal;

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
                ScanFolderToFileLogger.Error(ex, nameof(OpenFile), @"Errore in apertura file.");
            }
        }

        public static void OpenFolder(string pathFolder)
        {
            try
            {
                if (Directory.Exists(pathFolder))
                {
                    var psi = new ProcessStartInfo(pathFolder)
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

        public static void PrintFile(bool pdf, bool markdown)
        {
            try
            {
                var file = TxtFileFinal;
                if (pdf)
                    file = PdfFileFinal;
                if (markdown)
                    file = MarkdownFileFinal;
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

        public static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                //Create tutte le directory
                foreach (var dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));

                //Copia tutti i file e sostituisce tutti i file con lo stesso nome
                foreach (var newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);

                ScanFolderToFileLogger.Info(nameof(CopyFilesRecursively), @"File copiati correttamente.", true,
                    @"FILE COPIATI CORRETTAMENTE");
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(CopyFilesRecursively), "Errore in copia dei file.");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        public static void MoveAllContentFolders(string src, string dest, bool showConfirm = true)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                var dirInfoDest = new DirectoryInfo(dest);
                var content = Directory.GetFiles(src, "*.*", SearchOption.AllDirectories).ToList();
                if (!content.Any()) return;
                foreach (var mFile in content
                             .Select(file => new FileInfo(file))
                             .Where(mFile => new FileInfo(dirInfoDest + "\\" + mFile.Name).Exists == false))
                {
                    mFile.MoveTo(dirInfoDest + "\\" + mFile.Name);
                }

                if (showConfirm)
                    ScanFolderToFileLogger.Info(nameof(MoveAllContentFolders), @"File spostati correttamente.", true,
                    @"FILE SPOSTATI CORRETTAMENTE");
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(MoveAllContentFolders), @"Errore in spostamento dei file.");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        public static void CheckOnlyOneElements(CheckedListBox clb, ItemCheckEventArgs e)
        {
            for (var ix = 0; ix < clb.Items.Count; ++ix)
                if (ix != e.Index)
                    clb.SetItemChecked(ix, false);
        }

        public static string GetChoiceCheckedListBox(CheckedListBox clb)
        {
            return clb.SelectedItem?.ToString();
        }

        //Reorganizing files into folders by extension
        public static void ReorderFilesInFolderByType(string pathFolderSelected)
        {
            try
            {
                var content = GetContentFromFolderSelected(pathFolderSelected);

                if (!content.Any()) return;

                //Si creano tutte le cartelle delle estensioni scansionate
                foreach (var pathFolderToCreate in GetExtensionFromContentFromFolderSelected(content)
                             .Select(extension => Path.Combine(pathFolderSelected, extension.Replace(".", ""))))
                    Directory.CreateDirectory(pathFolderToCreate);

                foreach (var file in content)
                {
                    var ext = Path.GetExtension(file).Replace(".", "");
                    var fileName = Path.GetFileName(file);
                    var pathToMoveFile = Path.Combine(pathFolderSelected, Path.Combine(ext, fileName));
                    File.Move(file, pathToMoveFile);
                }

                ScanFolderToFileLogger.Info(nameof(ReorderFilesInFolderByType), @"File spostati correttamente.", true,
                    @"FILE SPOSTATI CORRETTAMENTE");

            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(ReorderFilesInFolderByType), @"Errore in riordinamento dei file.");
            }
        }

        public static string GetPathFileTxt()
        {
            return File.Exists(Path.Combine(PathFolder, TxtFileFinal)) ? Path.Combine(PathFolder, TxtFileFinal) : string.Empty;
        }
    }
}