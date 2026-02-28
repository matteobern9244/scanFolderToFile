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
        public const string ExternalOpenFailed = "L'apertura del percorso richiesto non e' riuscita.";
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
        public const string MacOpenCommandName = "open";
        public const string IsoDateFormat = "yyyy-MM-dd";
        public const string NameValueSeparator = ": ";
        public const string ListSeparator = ", ";
        public const string FileExistsLabel = "presente";
        public const string FileMissingLabel = "mancante";
        public const string DashPlaceholder = "-";
        public const string Space = " ";
        public const string OpenParenthesisWithLeadingSpace = " (";
        public const string CloseParenthesis = ")";
    }

    public static class Ui
    {
        public const string WindowTitle = "Scan Folder To File";

        public const string HeaderTitleTextBlockName = "HeaderTitleTextBlock";
        public const string HeaderSubtitleTextBlockName = "HeaderSubtitleTextBlock";
        public const string SourceFolderLabelTextBlockName = "SourceFolderLabelTextBlock";
        public const string SourceFolderTextBoxName = "SourceFolderTextBox";
        public const string BrowseSourceButtonName = "BrowseSourceButton";
        public const string OutputFolderLabelTextBlockName = "OutputFolderLabelTextBlock";
        public const string OutputFolderTextBoxName = "OutputFolderTextBox";
        public const string BrowseOutputButtonName = "BrowseOutputButton";
        public const string OutputFormatLabelTextBlockName = "OutputFormatLabelTextBlock";
        public const string OutputFormatComboBoxName = "OutputFormatComboBox";
        public const string OnlyExtensionsCheckBoxName = "OnlyExtensionsCheckBox";
        public const string CreateZipCheckBoxName = "CreateZipCheckBox";
        public const string CollectDuplicatesCheckBoxName = "CollectDuplicatesCheckBox";
        public const string FilterSummaryTitleTextBlockName = "FilterSummaryTitleTextBlock";
        public const string FilterSummaryTextBlockName = "FilterSummaryTextBlock";
        public const string GenerateButtonName = "GenerateButton";
        public const string OpenGeneratedFileButtonName = "OpenGeneratedFileButton";
        public const string OpenOutputFolderButtonName = "OpenOutputFolderButton";
        public const string OpenFiltersButtonName = "OpenFiltersButton";
        public const string OpenHistoryButtonName = "OpenHistoryButton";
        public const string StatusTitleTextBlockName = "StatusTitleTextBlock";
        public const string StatusTextBlockName = "StatusTextBlock";
        public const string ResultTitleTextBlockName = "ResultTitleTextBlock";
        public const string ResultSummaryTextBlockName = "ResultSummaryTextBlock";
        public const string PreviewTitleTextBlockName = "PreviewTitleTextBlock";
        public const string PreviewTextBoxName = "PreviewTextBox";

        public const string FilterWindowModeLabelTextBlockName = "FilterWindowModeLabelTextBlock";
        public const string FilterNoneRadioButtonName = "FilterNoneRadioButton";
        public const string FilterSizeRadioButtonName = "FilterSizeRadioButton";
        public const string FilterDateRadioButtonName = "FilterDateRadioButton";
        public const string FilterMinSizeLabelTextBlockName = "FilterMinSizeLabelTextBlock";
        public const string FilterMinSizeTextBoxName = "FilterMinSizeTextBox";
        public const string FilterMaxSizeLabelTextBlockName = "FilterMaxSizeLabelTextBlock";
        public const string FilterMaxSizeTextBoxName = "FilterMaxSizeTextBox";
        public const string FilterStartDateLabelTextBlockName = "FilterStartDateLabelTextBlock";
        public const string FilterStartDateTextBoxName = "FilterStartDateTextBox";
        public const string FilterEndDateLabelTextBlockName = "FilterEndDateLabelTextBlock";
        public const string FilterEndDateTextBoxName = "FilterEndDateTextBox";
        public const string FilterDateHintTextBlockName = "FilterDateHintTextBlock";
        public const string FilterStatusTextBlockName = "FilterStatusTextBlock";
        public const string FilterApplyButtonName = "FilterApplyButton";
        public const string FilterClearButtonName = "FilterClearButton";
        public const string FilterCancelButtonName = "FilterCancelButton";

        public const string HistoryStatusTextBlockName = "HistoryStatusTextBlock";
        public const string HistoryListBoxName = "HistoryListBox";
        public const string HistoryOpenButtonName = "HistoryOpenButton";
        public const string HistoryOpenFolderButtonName = "HistoryOpenFolderButton";
        public const string HistoryRefreshButtonName = "HistoryRefreshButton";
        public const string HistoryNameHeaderTextBlockName = "HistoryNameHeaderTextBlock";
        public const string HistoryExtensionHeaderTextBlockName = "HistoryExtensionHeaderTextBlock";
        public const string HistoryCreatedAtHeaderTextBlockName = "HistoryCreatedAtHeaderTextBlock";
        public const string HistoryStateHeaderTextBlockName = "HistoryStateHeaderTextBlock";

        public const string HeaderTitle = "Scansione cartelle e export";
        public const string HeaderSubtitle = "La UI macOS copre il flusso principale della vecchia WinForms: scegli la cartella, definisci output e filtri, genera il file e consulta lo storico.";
        public const string SourceFolderLabel = "Cartella sorgente";
        public const string OutputFolderLabel = "Cartella output";
        public const string OutputFormatLabel = "Formato output";
        public const string BrowseSourceButtonText = "Scegli...";
        public const string BrowseOutputButtonText = "Cambia...";
        public const string OnlyExtensionsCheckBoxText = "Solo estensioni";
        public const string CreateZipCheckBoxText = "Crea ZIP";
        public const string CollectDuplicatesCheckBoxText = "Rileva duplicati";
        public const string FilterSummaryTitle = "Filtro attivo";
        public const string GenerateButtonText = "Genera";
        public const string OpenGeneratedFileButtonText = "Apri file";
        public const string OpenOutputFolderButtonText = "Apri cartella";
        public const string OpenFiltersButtonText = "Filtri...";
        public const string OpenHistoryButtonText = "Storico...";
        public const string StatusTitle = "Stato";
        public const string ResultTitle = "Risultato";
        public const string PreviewTitle = "Preview";

        public const string ReadyStatus = "Pronto. Seleziona una cartella sorgente e avvia la generazione.";
        public const string RunningStatus = "Scansione in corso...";
        public const string FilterAppliedStatusPrefix = "Filtro applicato: ";
        public const string FilterClearedStatus = "Filtro rimosso.";
        public const string GenerateSuccessPrefix = "Generazione completata: ";
        public const string ErrorPrefix = "Errore: ";
        public const string MissingGeneratedFileStatus = "Nessun file generato disponibile da aprire.";
        public const string MissingOutputFolderStatus = "La cartella di output non e' disponibile.";
        public const string PickerUnavailableStatus = "Il picker di sistema non e' disponibile su questa finestra.";
        public const string NonLocalFolderStatus = "La cartella selezionata non e' un percorso locale utilizzabile.";
        public const string BrowseSourceDialogTitle = "Seleziona la cartella da analizzare";
        public const string BrowseOutputDialogTitle = "Seleziona la cartella di output";
        public const string PreviewEmpty = "Nessun risultato ancora disponibile.";
        public const string NoFilterSummary = "Nessun filtro";
        public const string InvalidDecimalPrefix = "Valore numerico non valido: ";
        public const string InvalidDatePrefix = "Data non valida: ";
        public const string MissingControlPrefix = "Missing control: ";

        public const string ResultItemsCountLabel = "Elementi";
        public const string ResultGeneratedFileLabel = "File";
        public const string ResultGeneratedZipLabel = "ZIP";
        public const string ResultDuplicatesCountLabel = "Duplicati";
        public const string ResultWarningsCountLabel = "Warning";
        public const string PreviewItemsTitle = "Dettaglio";
        public const string PreviewDuplicatesTitle = "Gruppi duplicati";
        public const string PreviewWarningsTitle = "Warning";
        public const string ResultUnavailableValue = "non disponibile";
        public const string HistoryDateFormat = "yyyy-MM-dd HH:mm:ss";
        public const string OutputFormatTxtLabel = "TXT";
        public const string OutputFormatPdfLabel = "PDF";
        public const string OutputFormatMarkdownLabel = "Markdown";
        public const string FilterSizeSummaryPrefix = "Dimensione MB";
        public const string FilterDateSummaryPrefix = "Date";
        public const string FilterFromLabel = "da";
        public const string FilterToLabel = "a";

        public const string FilterWindowTitle = "Filtri scansione";
        public const string FilterWindowModeLabel = "Scegli una sola modalita' di filtro";
        public const string FilterNoneOptionText = "Nessun filtro";
        public const string FilterSizeOptionText = "Filtro per dimensione";
        public const string FilterDateOptionText = "Filtro per date";
        public const string FilterMinSizeLabel = "Min MB";
        public const string FilterMaxSizeLabel = "Max MB";
        public const string FilterStartDateLabel = "Data inizio";
        public const string FilterEndDateLabel = "Data fine";
        public const string FilterDateHint = "Formato data: yyyy-MM-dd";
        public const string FilterStatusReady = "Configura il filtro e premi Applica.";
        public const string FilterStatusMissingSizeValue = "Inserisci almeno un limite di dimensione.";
        public const string FilterStatusMissingDateValue = "Inserisci almeno una data.";
        public const string FilterApplyButtonText = "Applica";
        public const string FilterClearButtonText = "Rimuovi filtro";
        public const string FilterCancelButtonText = "Annulla";

        public const string HistoryWindowTitle = "Storico file creati";
        public const string HistoryStatusReady = "Seleziona un elemento per aprire il file o la cartella.";
        public const string HistoryEmpty = "Nessun file generato nello storico corrente.";
        public const string HistoryReadFailedPrefix = "Storico non leggibile: ";
        public const string HistoryOpenButtonText = "Apri";
        public const string HistoryOpenFolderButtonText = "Apri cartella";
        public const string HistoryRefreshButtonText = "Aggiorna";
        public const string HistoryNameHeaderText = "Nome file";
        public const string HistoryExtensionHeaderText = "Ext";
        public const string HistoryCreatedAtHeaderText = "Creato il";
        public const string HistoryStateHeaderText = "Stato";
        public const string HistoryMissingFileStatus = "Il file selezionato non e' disponibile.";
        public const string HistoryMissingFolderStatus = "La cartella del file selezionato non e' disponibile.";
        public const string HistorySelectedLabelPrefix = "Selezionato: ";

        public const string MenuFileHeader = "Operazioni su File";
        public const string MenuOpenGeneratedFile = "Apri file generato";
        public const string MenuOpenOutputFolder = "Apri cartella file";
        public const string MenuOpenHistory = "Storico file creati";
        public const string MenuScanHeader = "Scansione";
        public const string MenuConfigureFilters = "Configura filtri";
        public const string MenuClearFilters = "Rimuovi filtri";
        public const string MenuGenerate = "Genera file";
        public const string MenuOtherHeader = "Altro";
        public const string MenuCopyMove = "Copia / Sposta Files";
        public const string MenuReorder = "Riordino per tipo";
        public const string MenuEditor = "Editor interno";
        public const string MenuPrint = "Stampa";
    }
}
