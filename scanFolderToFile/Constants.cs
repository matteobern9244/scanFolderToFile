﻿namespace ScanFolderToFile
{
    public class Constants
    {
        #region static readonly
        public static readonly string PathFolder = $@"C:\{Folder}";
        public static readonly string PathHistoryFileCreated = $@"C:\{HistoryFileCreated}";
        public static readonly string PathFolderZip = $@"{PathFolder}\ZipFiles";

        public static readonly string AlertMessage = $"Verrà creata la cartella '{Folder}' in C: con il file '{Content}'";
        public static readonly string AlertMissingScanFolder = @"Selezionare la cartella da scansionare";

        public static readonly string TxtFileFinal = $"{Content}.{ExtTxt}";
        public static readonly string PdfFileFinal = $"{Content}.{ExtPdf}";
        public static readonly string MarkdownFileFinal = $"{Content}.{ExtMd}";
        public static readonly string ZipFileFinal = $"{Content}.{ExtZip}";
        #endregion

        #region const

        public const string HistoryFileCreated = "HistoryFileCreated.json";

        public const string Folder = "CONTENT";

        public const string AlertTitle = "ATTENZIONE";

        public const string ElaborationConfirm = "Elaborazione completata senza errori";

        public const string ElaborationTitle = "ELABORAZIONE";

        public const string DesktopIni = "desktop.ini";

        public const string Content = "CONTENUTO";

        public const string Error = "Error";
        public const string Fatal = "Fatal";
        public const string Warn = "Warn";

        public const string Copy = "Copia";
        public const string Move = "Sposta";

        public const string FileTxt = "File TXT";
        public const string FilePdf = "File PDF";
        public const string FileMarkdown = "File Markdown";

        public const string ExtMd = "md";
        public const string ExtZip = "zip";
        public const string ExtPdf = "pdf";
        public const string ExtTxt = "txt";

        public const string OldFilesFromDates = "Più VECCHI di una certa data/e";
        public const string NewFilesFromDates = "Più GIOVANI di una certa data/e";
        #endregion
    }
}