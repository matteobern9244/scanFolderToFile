namespace ScanFolderToFile
{
    public class Constants
    {
        #region static readonly
        public static readonly string PathFolder = $@"C:\{Folder}";
        public static readonly string PathFolderZip = $@"C:\{FolderZip}";
        public static readonly string AlertMessage = $"Verrà creata la cartella '{Folder}' in C: con il file '{Content}'";
        public static readonly string TxtFileFinal = $"{Content}.txt";
        public static readonly string PdfFileFinal = $"{Content}.pdf";
        public static readonly string ZipFileFinal = $"{Content}.zip";
        #endregion

        #region const
        public const string Folder = "CONTENT";
        public const string FolderZip = "CONTENT_ZIPPED";
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
        #endregion
    }
}