namespace ScanFolderToFile.Core.Constants;

public static class AppStrings
{
    public static class Files
    {
        public const string ContentDirectoryName = "CONTENT";
        public const string OutputTitle = "CONTENUTO";
        public const string HistoryDirectoryName = "History";
        public const string ZipDirectoryName = "ZipFiles";
        public const string HistoryFileName = "HistoryFileCreated.json";
        public const string OutputTextFileName = "CONTENUTO.txt";
        public const string OutputMarkdownFileName = "CONTENUTO.md";
        public const string OutputPdfFileName = "CONTENUTO.pdf";
        public const string OutputZipFileName = "CONTENUTO.zip";
    }

    public static class Paths
    {
        public const string LegacyResourcesRoot = "scanFolderToFile/Resources";
    }

    public static class Json
    {
        public const string HistoryEntryFileNameProperty = "Nome File";
        public const string HistoryEntryExtensionProperty = "Estensione File";
        public const string HistoryEntryCreatedAtProperty = "Data Creazione";
    }

    public static class Messages
    {
        public const string MissingSourceFolder = "La cartella sorgente e' obbligatoria.";
        public const string MissingOutputFolder = "La cartella di output e' obbligatoria.";
        public const string SourceFolderNotFound = "La cartella sorgente non esiste.";
        public const string InvalidOutputFormat = "Il formato di output selezionato non e' valido.";
        public const string NegativeMinSize = "La dimensione minima non puo' essere negativa.";
        public const string NegativeMaxSize = "La dimensione massima non puo' essere negativa.";
        public const string InvalidSizeRange = "La dimensione minima non puo' superare la dimensione massima.";
        public const string InvalidDateRange = "La data iniziale non puo' essere successiva alla data finale.";
        public const string HistoryFileCorrupted = "Lo storico dei file creati non e' leggibile.";
        public const string UnsupportedExportFormat = "Nessun exporter supporta il formato richiesto.";
        public const string ExportFailed = "La generazione del file richiesto non e' riuscita.";
    }

    public static class Filters
    {
        public const string DesktopIni = "desktop.ini";
    }

    public static class System
    {
        public const string WildcardAllFiles = "*";
        public const string DotNetExecutableName = "dotnet";
        public const string CommandExtension = ".command";
    }

    public static class Ui
    {
        public const string WindowTitle = "Scan Folder To File";
        public const string HeadlineTextBlockName = "HeadlineTextBlock";
        public const string SubheadlineTextBlockName = "SubheadlineTextBlock";
        public const string CoreTitleTextBlockName = "CoreTitleTextBlock";
        public const string CoreBodyTextBlockName = "CoreBodyTextBlock";
        public const string LauncherTitleTextBlockName = "LauncherTitleTextBlock";
        public const string LauncherBodyTextBlockName = "LauncherBodyTextBlock";
        public const string CiTitleTextBlockName = "CiTitleTextBlock";
        public const string CiBodyTextBlockName = "CiBodyTextBlock";
        public const string FooterTextBlockName = "FooterTextBlock";
        public const string ShellHeadline = "Base moderna pronta";
        public const string ShellSubheadline = "La shell Avalonia e' attiva. Questo e' il punto di partenza del porting macOS: il core moderno, il launcher intelligente e la CI/CD sono gia' collegati.";
        public const string ShellCoreTitle = "Core cross-platform";
        public const string ShellCoreBody = "Scansione, filtri, export TXT/Markdown/PDF, ZIP, storico e duplicati sono gia' implementati nel progetto ScanFolderToFile.Core.";
        public const string ShellLauncherTitle = "Launcher macOS";
        public const string ShellLauncherBody = "Usa start_scanfolder.command per eseguire format, restore, build, test e avvio con fallback intelligente tra app-host e DLL.";
        public const string ShellCiTitle = "Quality gates";
        public const string ShellCiBody = "La pipeline moderna verifica dotnet format, build, test e coverage minima 90% line / 85% branch su runner macOS.";
        public const string ShellFooter = "Step successivo: sostituire questa shell con la UI funzionale completa del porting.";
        public const string MissingTextControlPrefix = "Missing text control: ";
    }
}
