namespace ScanFolderToFile
{
    public class Constants
    {
        #region static readonly
        public static readonly string PathFolder = $@"C:\{Folder}";
        public static readonly string AlertMessage = $"Verrà creata la cartella '{Folder}' in C: con il file '{Content}'";
        public static readonly string TxtFileFinal = $"{Content}.txt";
        public static readonly string PdfFileFinal = $"{Content}.pdf";
        #endregion

        #region const
        public const string Folder = "CONTENT";
        public const string AlertTitle = "ATTENZIONE";
        public const string ElaborationConfirm = "Elaborazione completata senza errori";
        public const string ElaborationTitle = "ELABORAZIONE";
        public const string DesktopIni = "desktop.ini";
        public const string Content = "CONTENUTO";
        public const string Error = "Error";
        public const string Fatal = "Fatal";
        public const string Warn = "Warn";
        #endregion
    }
}