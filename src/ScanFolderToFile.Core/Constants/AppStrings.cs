namespace ScanFolderToFile.Core.Constants;

public static class AppStrings
{
    public static class Files
    {
        public const string ContentDirectoryName = "CONTENT";
        public const string OutputTitle = "CONTENUTO";
        public const string HistoryDirectoryName = "History";
        public const string ZipDirectoryName = "ZipFiles";
        public const string NoExtensionDirectoryName = "no_extension";
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
        public const string MissingDestinationFolder = "La cartella di destinazione e' obbligatoria.";
        public const string DestinationFolderMatchesSource = "La cartella di destinazione deve essere diversa dalla cartella sorgente.";
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
        public const string MacPrintCommandName = "lp";
        public const string IsoDateFormat = "yyyy-MM-dd";
        public const string NameValueSeparator = ": ";
        public const string ListSeparator = ", ";
        public const string NewLine = "\n";
        public const string FileExistsLabel = "presente";
        public const string FileMissingLabel = "mancante";
        public const string DashPlaceholder = "-";
        public const string Space = " ";
        public const string OpenParenthesisWithLeadingSpace = " (";
        public const string CloseParenthesis = ")";
        public const string PathSeparator = "/";
        public const string TextFileExtension = ".txt";
        public const string MarkdownFileExtension = ".md";
        public const string PdfFileExtension = ".pdf";
        public const string JsonFileExtension = ".json";
        public const string CsvFileExtension = ".csv";
        public const string XmlFileExtension = ".xml";
        public const string LogFileExtension = ".log";
        public const string TempPrintFilePrefix = "scanfolder-print-";
        public const string GuidCompactFormat = "N";
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
        public const string OpenDuplicatesButtonName = "OpenDuplicatesButton";
        public const string OpenUtilitiesButtonName = "OpenUtilitiesButton";
        public const string OpenEditorButtonName = "OpenEditorButton";
        public const string OpenPrintPreviewButtonName = "OpenPrintPreviewButton";
        public const string StatusTitleTextBlockName = "StatusTitleTextBlock";
        public const string StatusTextBlockName = "StatusTextBlock";
        public const string ResultTitleTextBlockName = "ResultTitleTextBlock";
        public const string ResultSummaryTextBlockName = "ResultSummaryTextBlock";
        public const string PreviewTitleTextBlockName = "PreviewTitleTextBlock";
        public const string PreviewTextBoxName = "PreviewTextBox";

        public const string UtilitiesModeLabelTextBlockName = "UtilitiesModeLabelTextBlock";
        public const string UtilityCopyRadioButtonName = "UtilityCopyRadioButton";
        public const string UtilityMoveRadioButtonName = "UtilityMoveRadioButton";
        public const string UtilityReorderRadioButtonName = "UtilityReorderRadioButton";
        public const string UtilitySourceLabelTextBlockName = "UtilitySourceLabelTextBlock";
        public const string UtilitySourceTextBoxName = "UtilitySourceTextBox";
        public const string UtilityBrowseSourceButtonName = "UtilityBrowseSourceButton";
        public const string UtilityDestinationLabelTextBlockName = "UtilityDestinationLabelTextBlock";
        public const string UtilityDestinationTextBoxName = "UtilityDestinationTextBox";
        public const string UtilityBrowseDestinationButtonName = "UtilityBrowseDestinationButton";
        public const string UtilityStatusTextBlockName = "UtilityStatusTextBlock";
        public const string UtilityResultTextBoxName = "UtilityResultTextBox";
        public const string UtilityExecuteButtonName = "UtilityExecuteButton";
        public const string UtilityCloseButtonName = "UtilityCloseButton";

        public const string DuplicatesStatusTextBlockName = "DuplicatesStatusTextBlock";
        public const string DuplicatesSummaryTextBlockName = "DuplicatesSummaryTextBlock";
        public const string DuplicatesListBoxName = "DuplicatesListBox";
        public const string DuplicatesOpenFileButtonName = "DuplicatesOpenFileButton";
        public const string DuplicatesOpenFolderButtonName = "DuplicatesOpenFolderButton";
        public const string DuplicatesEmptyTextBlockName = "DuplicatesEmptyTextBlock";

        public const string EditorPathTextBlockName = "EditorPathTextBlock";
        public const string EditorStatusTextBlockName = "EditorStatusTextBlock";
        public const string EditorTextBoxName = "EditorTextBox";
        public const string EditorOpenButtonName = "EditorOpenButton";
        public const string EditorSaveButtonName = "EditorSaveButton";
        public const string EditorSaveAsButtonName = "EditorSaveAsButton";
        public const string EditorPrintPreviewButtonName = "EditorPrintPreviewButton";
        public const string EditorCloseButtonName = "EditorCloseButton";

        public const string PrintPreviewSummaryTextBlockName = "PrintPreviewSummaryTextBlock";
        public const string PrintPreviewStatusTextBlockName = "PrintPreviewStatusTextBlock";
        public const string PrintPreviewTextBoxName = "PrintPreviewTextBox";
        public const string PrintPreviewPrintButtonName = "PrintPreviewPrintButton";
        public const string PrintPreviewOpenFileButtonName = "PrintPreviewOpenFileButton";
        public const string PrintPreviewCloseButtonName = "PrintPreviewCloseButton";

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

        public const string HeaderTitle = "Seleziona cartella da scansionare";
        public const string HeaderSubtitle = "Genera il file in TXT, PDF o Markdown, applica i filtri, crea lo ZIP e consulta lo storico dei file creati.";
        public const string SourceFolderLabel = "Cartella da scansionare";
        public const string OutputFolderLabel = "Cartella file creati";
        public const string OutputFormatLabel = "Formato output";
        public const string BrowseSourceButtonText = "Seleziona cartella...";
        public const string BrowseOutputButtonText = "Seleziona cartella...";
        public const string OnlyExtensionsCheckBoxText = "Solo Estensioni";
        public const string CreateZipCheckBoxText = "Creare Zip Cartella Scelta";
        public const string CollectDuplicatesCheckBoxText = "Check nomi files duplicati";
        public const string FilterSummaryTitle = "Filtri Scansione Cartella";
        public const string GenerateButtonText = "GENERA FILE";
        public const string OpenGeneratedFileButtonText = "Apri";
        public const string OpenOutputFolderButtonText = "Apri cartella files";
        public const string OpenFiltersButtonText = "Filtri...";
        public const string OpenHistoryButtonText = "Storico File Creati";
        public const string OpenDuplicatesButtonText = "Nomi files duplicati";
        public const string OpenUtilitiesButtonText = "Altre operazioni...";
        public const string OpenEditorButtonText = "Editor File TXT";
        public const string OpenPrintPreviewButtonText = "Stampa";
        public const string StatusTitle = "Stato";
        public const string ResultTitle = "Risultato";
        public const string PreviewTitle = "Anteprima";

        public const string ReadyStatus = "Pronto. Seleziona la cartella da scansionare e genera il file.";
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
        public const string NoDuplicatesStatus = "Nessun gruppo duplicato disponibile per questa esecuzione.";
        public const string NoEditorSourceStatus = "Nessun file testuale disponibile: usa Apri per caricare un documento.";
        public const string NoPrintPreviewStatus = "Nessun contenuto disponibile da inviare alla stampa.";
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

        public const string FilterWindowTitle = "Filtri Scansione Cartella";
        public const string FilterWindowModeLabel = "Filtra i files per data o per dimensione";
        public const string FilterNoneOptionText = "Nessun filtro";
        public const string FilterSizeOptionText = "Intervallo di dimensioni";
        public const string FilterDateOptionText = "Intervallo di data/e";
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

        public const string HistoryWindowTitle = "Storico File Creati";
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
        public const string MenuOpenGeneratedFile = "Apri";
        public const string MenuOpenOutputFolder = "Apri cartella files";
        public const string MenuOpenHistory = "Storico File Creati";
        public const string MenuOpenDuplicates = "Nomi files duplicati";
        public const string MenuScanHeader = "Filtri Scansione Cartella";
        public const string MenuConfigureFilters = "Configura";
        public const string MenuClearFilters = "Rimuovi filtri";
        public const string MenuGenerate = "Genera File";
        public const string MenuOtherHeader = "Altro";
        public const string MenuCopyMove = "Copia / Sposta Files";
        public const string MenuReorder = "Riordinamento files in cartelle per tipo";
        public const string MenuEditor = "Editor interno";
        public const string MenuPrint = "Stampa";

        public const string UtilitiesWindowTitle = "Copia / Sposta Files";
        public const string UtilitiesModeLabel = "Scegli l'operazione da eseguire";
        public const string UtilityCopyText = "Copia";
        public const string UtilityMoveText = "Sposta";
        public const string UtilityReorderText = "Riordinamento per tipo";
        public const string UtilitySourceLabel = "Cartella di partenza";
        public const string UtilityDestinationLabel = "Cartella di destinazione";
        public const string UtilityBrowseSourceText = "SFOGLIA";
        public const string UtilityBrowseDestinationText = "SFOGLIA";
        public const string UtilityExecuteButtonText = "Esegui";
        public const string UtilityCloseButtonText = "Chiudi";
        public const string UtilityResultEmpty = "Nessuna operazione eseguita.";
        public const string UtilityReadyStatus = "Configura l'operazione e premi Esegui.";
        public const string UtilityDestinationDisabledStatus = "Per il riordino per estensione la destinazione coincide con la cartella sorgente.";
        public const string UtilityCopySuccessPrefix = "Copia completata. File coinvolti: ";
        public const string UtilityMoveSuccessPrefix = "Spostamento completato. File coinvolti: ";
        public const string UtilityReorderSuccessPrefix = "Riordino completato. File coinvolti: ";
        public const string UtilityNoSourceStatus = "Seleziona una cartella sorgente valida.";
        public const string UtilityNoDestinationStatus = "Seleziona una cartella di destinazione valida.";
        public const string UtilityBrowseSourceDialogTitle = "Seleziona la cartella sorgente per l'operazione";
        public const string UtilityBrowseDestinationDialogTitle = "Seleziona la cartella di destinazione";
        public const string UtilityAffectedTitle = "File coinvolti";
        public const string UtilitySkippedTitle = "File saltati";

        public const string DuplicatesWindowTitle = "Nomi files duplicati";
        public const string DuplicatesEmpty = "Nessun duplicato disponibile.";
        public const string DuplicatesStatusReady = "Seleziona un file duplicato per aprirlo o aprirne la cartella.";
        public const string DuplicatesOpenFileButtonText = "Apri file";
        public const string DuplicatesOpenFolderButtonText = "Apri cartella";
        public const string DuplicatesMissingFileStatus = "Il file duplicato selezionato non e' disponibile.";
        public const string DuplicatesMissingFolderStatus = "La cartella del duplicato selezionato non e' disponibile.";
        public const string DuplicatesSummaryPrefix = "Gruppi duplicati trovati: ";
        public const string DuplicatesSelectedPrefix = "Duplicato selezionato: ";

        public const string EditorWindowTitle = "Editor File TXT";
        public const string EditorNoFileLoaded = "Nessun file caricato.";
        public const string EditorLoadedFromFilePrefix = "File caricato: ";
        public const string EditorLoadedFromPreviewStatus = "Preview caricata come documento modificabile.";
        public const string EditorUnsupportedFileStatus = "Il file selezionato non e' un formato testuale modificabile. Usa Salva come per esportare il contenuto.";
        public const string EditorOpenButtonText = "Apri...";
        public const string EditorSaveButtonText = "Salva";
        public const string EditorSaveAsButtonText = "Salva come...";
        public const string EditorPrintPreviewButtonText = "Anteprima di stampa";
        public const string EditorCloseButtonText = "Chiudi";
        public const string EditorSavedStatus = "Documento salvato.";
        public const string EditorOpenDialogTitle = "Seleziona un file testuale da modificare";
        public const string EditorSaveDialogTitle = "Salva il documento modificato";
        public const string EditorSuggestedSaveName = "documento.txt";
        public const string EditorUntitledLabel = "Documento non salvato";

        public const string PrintPreviewWindowTitle = "Anteprima di stampa";
        public const string PrintPreviewSummaryPrefix = "Documento";
        public const string PrintPreviewFromFileSuffix = "file reale";
        public const string PrintPreviewFromBufferSuffix = "buffer interno";
        public const string PrintPreviewReadyStatus = "Controlla l'anteprima e premi Stampa per inviare il documento alla stampante di default.";
        public const string PrintPreviewPrintedStatus = "Documento inviato alla stampante di default.";
        public const string PrintPreviewOpenFileButtonText = "Apri file";
        public const string PrintPreviewPrintButtonText = "Stampa";
        public const string PrintPreviewCloseButtonText = "Chiudi";
        public const string PrintPreviewMissingFileStatus = "Il file associato all'anteprima non e' disponibile.";
        public const string PrintPreviewNoContent = "Nessun contenuto disponibile per l'anteprima.";
    }
}
