using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Constants;
using ScanFolderToFile.Core.Models;
using ScanFolderToFile.Core.Services;

namespace ScanFolderToFile.App;

public sealed partial class MainWindow : Window
{
    private readonly IScanService _scanService;
    private readonly IAppPaths _appPaths;
    private readonly IHistoryStore _historyStore;
    private readonly IExternalLauncher _externalLauncher;
    private readonly IFileOperationsService _fileOperationsService;
    private readonly IPrintService _printService;
    private readonly IRichTextEditorService _richTextEditorService;
    private readonly IPrintWorkflowService _printWorkflowService;
    private readonly IEditorWindowLauncher _editorWindowLauncher;
    private readonly MainWindowUiState _uiState;
    private readonly IReadOnlyList<OutputFormatChoice> _formatChoices;
    private readonly TextBox _sourceFolderTextBox;
    private readonly TextBox _outputFolderTextBox;
    private readonly ComboBox _outputFormatComboBox;
    private readonly CheckBox _onlyExtensionsCheckBox;
    private readonly CheckBox _createZipCheckBox;
    private readonly CheckBox _collectDuplicatesCheckBox;
    private readonly TextBlock _statusTextBlock;
    private readonly TextBlock _filterSummaryTextBlock;
    private readonly TextBlock _resultSummaryTextBlock;
    private readonly TextBox _previewTextBox;
    private readonly Button _browseSourceButton;
    private readonly Button _browseOutputButton;
    private readonly Button _generateButton;
    private readonly Button _openGeneratedFileButton;
    private readonly Button _openOutputFolderButton;
    private readonly Button _openFiltersButton;
    private readonly Button _openHistoryButton;
    private readonly Button _openDuplicatesButton;
    private readonly Button _openUtilitiesButton;
    private readonly Button _openEditorButton;
    private readonly Button _openPrintPreviewButton;
    private readonly NativeMenuItem _menuOpenGeneratedFileItem;
    private readonly NativeMenuItem _menuOpenOutputFolderItem;
    private readonly NativeMenuItem _menuOpenHistoryItem;
    private readonly NativeMenuItem _menuOpenDuplicatesItem;
    private readonly NativeMenuItem _menuConfigureFiltersItem;
    private readonly NativeMenuItem _menuClearFiltersItem;
    private readonly NativeMenuItem _menuGenerateItem;
    private readonly NativeMenuItem _menuCopyMoveItem;
    private readonly NativeMenuItem _menuReorderItem;
    private readonly NativeMenuItem _menuEditorItem;
    private readonly NativeMenuItem _menuPrintItem;
    private bool _isBusy;

    public MainWindow()
        : this(AppServiceFactory.Create())
    {
    }

    public MainWindow(
        IScanService scanService,
        IAppPaths appPaths,
        IHistoryStore historyStore,
        IExternalLauncher externalLauncher)
        : this(CreateNativeServices(scanService, appPaths, historyStore, externalLauncher, new FileOperationsService(), new MacPrintService()))
    {
    }

    public MainWindow(
        IScanService scanService,
        IAppPaths appPaths,
        IHistoryStore historyStore,
        IExternalLauncher externalLauncher,
        IFileOperationsService fileOperationsService)
        : this(CreateNativeServices(scanService, appPaths, historyStore, externalLauncher, fileOperationsService, new MacPrintService()))
    {
    }

    internal MainWindow(
        IScanService scanService,
        IAppPaths appPaths,
        IHistoryStore historyStore,
        IExternalLauncher externalLauncher,
        IFileOperationsService fileOperationsService,
        IPrintService printService)
        : this(CreateFallbackServices(scanService, appPaths, historyStore, externalLauncher, fileOperationsService, printService))
    {
    }

    internal MainWindow(
        IScanService scanService,
        IAppPaths appPaths,
        IHistoryStore historyStore,
        IExternalLauncher externalLauncher,
        IFileOperationsService fileOperationsService,
        IPrintService printService,
        IRichTextEditorService richTextEditorService,
        IPrintWorkflowService printWorkflowService,
        IEditorWindowLauncher editorWindowLauncher)
        : this(
            new DefaultAppServices(
                scanService,
                appPaths,
                historyStore,
                externalLauncher,
                fileOperationsService,
                printService,
                richTextEditorService,
                printWorkflowService,
                editorWindowLauncher))
    {
    }

    private MainWindow(DefaultAppServices services)
    {
        _scanService = services.ScanService;
        _appPaths = services.AppPaths;
        _historyStore = services.HistoryStore;
        _externalLauncher = services.ExternalLauncher;
        _fileOperationsService = services.FileOperationsService;
        _printService = services.PrintService;
        _richTextEditorService = services.RichTextEditorService;
        _printWorkflowService = services.PrintWorkflowService;
        _editorWindowLauncher = services.EditorWindowLauncher;
        _uiState = new MainWindowUiState();

        AvaloniaXamlLoader.Load(this);

        _sourceFolderTextBox = GetRequiredControl<TextBox>(AppStrings.Ui.SourceFolderTextBoxName);
        _outputFolderTextBox = GetRequiredControl<TextBox>(AppStrings.Ui.OutputFolderTextBoxName);
        _outputFormatComboBox = GetRequiredControl<ComboBox>(AppStrings.Ui.OutputFormatComboBoxName);
        _onlyExtensionsCheckBox = GetRequiredControl<CheckBox>(AppStrings.Ui.OnlyExtensionsCheckBoxName);
        _createZipCheckBox = GetRequiredControl<CheckBox>(AppStrings.Ui.CreateZipCheckBoxName);
        _collectDuplicatesCheckBox = GetRequiredControl<CheckBox>(AppStrings.Ui.CollectDuplicatesCheckBoxName);
        _statusTextBlock = GetRequiredControl<TextBlock>(AppStrings.Ui.StatusTextBlockName);
        _filterSummaryTextBlock = GetRequiredControl<TextBlock>(AppStrings.Ui.FilterSummaryTextBlockName);
        _resultSummaryTextBlock = GetRequiredControl<TextBlock>(AppStrings.Ui.ResultSummaryTextBlockName);
        _previewTextBox = GetRequiredControl<TextBox>(AppStrings.Ui.PreviewTextBoxName);
        _browseSourceButton = GetRequiredControl<Button>(AppStrings.Ui.BrowseSourceButtonName);
        _browseOutputButton = GetRequiredControl<Button>(AppStrings.Ui.BrowseOutputButtonName);
        _generateButton = GetRequiredControl<Button>(AppStrings.Ui.GenerateButtonName);
        _openGeneratedFileButton = GetRequiredControl<Button>(AppStrings.Ui.OpenGeneratedFileButtonName);
        _openOutputFolderButton = GetRequiredControl<Button>(AppStrings.Ui.OpenOutputFolderButtonName);
        _openFiltersButton = GetRequiredControl<Button>(AppStrings.Ui.OpenFiltersButtonName);
        _openHistoryButton = GetRequiredControl<Button>(AppStrings.Ui.OpenHistoryButtonName);
        _openDuplicatesButton = GetRequiredControl<Button>(AppStrings.Ui.OpenDuplicatesButtonName);
        _openUtilitiesButton = GetRequiredControl<Button>(AppStrings.Ui.OpenUtilitiesButtonName);
        _openEditorButton = GetRequiredControl<Button>(AppStrings.Ui.OpenEditorButtonName);
        _openPrintPreviewButton = GetRequiredControl<Button>(AppStrings.Ui.OpenPrintPreviewButtonName);
        _formatChoices = CreateOutputFormatChoices();

        _menuOpenGeneratedFileItem = CreateMenuItem(AppStrings.Ui.MenuOpenGeneratedFile, (_, _) => _ = OpenGeneratedFileAsync());
        _menuOpenOutputFolderItem = CreateMenuItem(AppStrings.Ui.MenuOpenOutputFolder, (_, _) => _ = OpenOutputFolderAsync());
        _menuOpenHistoryItem = CreateMenuItem(AppStrings.Ui.MenuOpenHistory, (_, _) => _ = OpenHistoryWindowAsync());
        _menuOpenDuplicatesItem = CreateMenuItem(AppStrings.Ui.MenuOpenDuplicates, (_, _) => _ = OpenDuplicatesWindowAsync());
        _menuConfigureFiltersItem = CreateMenuItem(AppStrings.Ui.MenuConfigureFilters, (_, _) => _ = OpenFiltersAsync());
        _menuClearFiltersItem = CreateMenuItem(AppStrings.Ui.MenuClearFilters, (_, _) => ClearFilter());
        _menuGenerateItem = CreateMenuItem(AppStrings.Ui.MenuGenerate, (_, _) => _ = GenerateAsync());
        _menuCopyMoveItem = CreateMenuItem(AppStrings.Ui.MenuCopyMove, (_, _) => _ = OpenUtilitiesWindowAsync(UtilityOperationMode.Copy));
        _menuReorderItem = CreateMenuItem(AppStrings.Ui.MenuReorder, (_, _) => _ = OpenUtilitiesWindowAsync(UtilityOperationMode.Reorder));
        _menuEditorItem = CreateMenuItem(AppStrings.Ui.MenuEditor, (_, _) => _ = OpenEditorWindowAsync());
        _menuPrintItem = CreateMenuItem(AppStrings.Ui.MenuPrint, (_, _) => _ = OpenPrintPreviewWindowAsync());

        Title = AppStrings.Ui.WindowTitle;
        InitializeText();
        InitializeControls();
        InitializeNativeMenu();
        InitializeInteractions();
        RefreshActionState();
    }

    internal string CurrentStatus => _statusTextBlock.Text ?? string.Empty;

    internal string CurrentFilterSummary => _filterSummaryTextBlock.Text ?? string.Empty;

    internal bool CanOpenDuplicates => _openDuplicatesButton.IsEnabled;

    internal bool CanOpenPrintPreview => _openPrintPreviewButton.IsEnabled;

    internal void ApplyFilterResult(FilterDialogResult filterResult)
    {
        _uiState.ActiveFilter = filterResult;
        _filterSummaryTextBlock.Text = filterResult.Summary;
        _statusTextBlock.Text = filterResult.Mode == FilterMode.None
            ? AppStrings.Ui.FilterClearedStatus
            : string.Concat(AppStrings.Ui.FilterAppliedStatusPrefix, filterResult.Summary);
        RefreshActionState();
    }

    internal void ApplyScanResult(ScanResult scanResult)
    {
        _uiState.LastResult = scanResult;
        _resultSummaryTextBlock.Text = ResultPreviewBuilder.BuildSummary(scanResult);
        _previewTextBox.Text = ResultPreviewBuilder.BuildPreview(scanResult);
        var generatedArtifactPath = GetCurrentGeneratedArtifactPath();
        _statusTextBlock.Text = scanResult.GeneratedFilePath is not null
            ? string.Concat(AppStrings.Ui.GenerateSuccessPrefix, generatedArtifactPath ?? AppStrings.Ui.ResultUnavailableValue)
            : string.Concat(AppStrings.Ui.ZipGenerateSuccessPrefix, generatedArtifactPath ?? AppStrings.Ui.ResultUnavailableValue);
        RefreshActionState();
    }

    private void InitializeText()
    {
        SetText(AppStrings.Ui.HeaderTitleTextBlockName, AppStrings.Ui.HeaderTitle);
        SetText(AppStrings.Ui.HeaderSubtitleTextBlockName, AppStrings.Ui.HeaderSubtitle);
        SetText(AppStrings.Ui.SourceFolderLabelTextBlockName, AppStrings.Ui.SourceFolderLabel);
        SetText(AppStrings.Ui.OutputFolderLabelTextBlockName, AppStrings.Ui.OutputFolderLabel);
        SetText(AppStrings.Ui.OutputFormatLabelTextBlockName, AppStrings.Ui.OutputFormatLabel);
        SetText(AppStrings.Ui.FilterSummaryTitleTextBlockName, AppStrings.Ui.FilterSummaryTitle);
        SetText(AppStrings.Ui.StatusTitleTextBlockName, AppStrings.Ui.StatusTitle);
        SetText(AppStrings.Ui.ResultTitleTextBlockName, AppStrings.Ui.ResultTitle);
        SetText(AppStrings.Ui.PreviewTitleTextBlockName, AppStrings.Ui.PreviewTitle);
        SetButtonContent(AppStrings.Ui.BrowseSourceButtonName, AppStrings.Ui.BrowseSourceButtonText);
        SetButtonContent(AppStrings.Ui.BrowseOutputButtonName, AppStrings.Ui.BrowseOutputButtonText);
        SetButtonContent(AppStrings.Ui.GenerateButtonName, AppStrings.Ui.GenerateButtonText);
        SetButtonContent(AppStrings.Ui.OpenGeneratedFileButtonName, AppStrings.Ui.OpenGeneratedFileButtonText);
        SetButtonContent(AppStrings.Ui.OpenOutputFolderButtonName, AppStrings.Ui.OpenOutputFolderButtonText);
        SetButtonContent(AppStrings.Ui.OpenFiltersButtonName, AppStrings.Ui.OpenFiltersButtonText);
        SetButtonContent(AppStrings.Ui.OpenHistoryButtonName, AppStrings.Ui.OpenHistoryButtonText);
        SetButtonContent(AppStrings.Ui.OpenDuplicatesButtonName, AppStrings.Ui.OpenDuplicatesButtonText);
        SetButtonContent(AppStrings.Ui.OpenUtilitiesButtonName, AppStrings.Ui.OpenUtilitiesButtonText);
        SetButtonContent(AppStrings.Ui.OpenEditorButtonName, AppStrings.Ui.OpenEditorButtonText);
        SetButtonContent(AppStrings.Ui.OpenPrintPreviewButtonName, AppStrings.Ui.OpenPrintPreviewButtonText);
        SetCheckBoxContent(AppStrings.Ui.OnlyExtensionsCheckBoxName, AppStrings.Ui.OnlyExtensionsCheckBoxText);
        SetCheckBoxContent(AppStrings.Ui.CreateZipCheckBoxName, AppStrings.Ui.CreateZipCheckBoxText);
        SetCheckBoxContent(AppStrings.Ui.CollectDuplicatesCheckBoxName, AppStrings.Ui.CollectDuplicatesCheckBoxText);
    }

    private void InitializeControls()
    {
        _outputFolderTextBox.Text = _appPaths.GetDefaultOutputFolder();
        _outputFormatComboBox.ItemsSource = _formatChoices;
        _outputFormatComboBox.SelectedItem = _formatChoices[0];
        _statusTextBlock.Text = AppStrings.Ui.ReadyStatus;
        _filterSummaryTextBlock.Text = _uiState.ActiveFilter.Summary;
        _resultSummaryTextBlock.Text = AppStrings.Ui.PreviewEmpty;
        _previewTextBox.Text = AppStrings.Ui.PreviewEmpty;
    }

    private void InitializeNativeMenu()
    {
        var rootMenu = new NativeMenu
        {
            new NativeMenuItem
            {
                Header = AppStrings.Ui.MenuFileHeader,
                Menu = new NativeMenu
                {
                    _menuOpenGeneratedFileItem,
                    _menuPrintItem,
                    _menuOpenOutputFolderItem,
                    _menuOpenHistoryItem,
                    _menuOpenDuplicatesItem,
                },
            },
            new NativeMenuItem
            {
                Header = AppStrings.Ui.MenuScanHeader,
                Menu = new NativeMenu
                {
                    _menuConfigureFiltersItem,
                    _menuClearFiltersItem,
                    _menuGenerateItem,
                },
            },
            new NativeMenuItem
            {
                Header = AppStrings.Ui.MenuOtherHeader,
                Menu = new NativeMenu
                {
                    _menuCopyMoveItem,
                    _menuReorderItem,
                    _menuEditorItem,
                },
            },
        };

        NativeMenu.SetMenu(this, rootMenu);
    }

    private void InitializeInteractions()
    {
        _browseSourceButton.Click += HandleBrowseSourceClick;
        _browseOutputButton.Click += HandleBrowseOutputClick;
        _generateButton.Click += HandleGenerateClick;
        _openGeneratedFileButton.Click += HandleOpenGeneratedFileClick;
        _openOutputFolderButton.Click += HandleOpenOutputFolderClick;
        _openFiltersButton.Click += HandleOpenFiltersClick;
        _openHistoryButton.Click += HandleOpenHistoryClick;
        _openDuplicatesButton.Click += HandleOpenDuplicatesClick;
        _openUtilitiesButton.Click += HandleOpenUtilitiesClick;
        _openEditorButton.Click += HandleOpenEditorClick;
        _openPrintPreviewButton.Click += HandleOpenPrintPreviewClick;
        _sourceFolderTextBox.TextChanged += HandleTextChanged;
        _outputFolderTextBox.TextChanged += HandleTextChanged;
        _outputFormatComboBox.SelectionChanged += HandleSelectionChanged;
    }

    private async void HandleBrowseSourceClick(object? sender, RoutedEventArgs e)
    {
        await SelectFolderAsync(_sourceFolderTextBox, AppStrings.Ui.BrowseSourceDialogTitle).ConfigureAwait(true);
    }

    private async void HandleBrowseOutputClick(object? sender, RoutedEventArgs e)
    {
        await SelectFolderAsync(_outputFolderTextBox, AppStrings.Ui.BrowseOutputDialogTitle).ConfigureAwait(true);
    }

    private async void HandleGenerateClick(object? sender, RoutedEventArgs e)
    {
        await GenerateAsync().ConfigureAwait(true);
    }

    private async void HandleOpenGeneratedFileClick(object? sender, RoutedEventArgs e)
    {
        await OpenGeneratedFileAsync().ConfigureAwait(true);
    }

    private async void HandleOpenOutputFolderClick(object? sender, RoutedEventArgs e)
    {
        await OpenOutputFolderAsync().ConfigureAwait(true);
    }

    private async void HandleOpenFiltersClick(object? sender, RoutedEventArgs e)
    {
        await OpenFiltersAsync().ConfigureAwait(true);
    }

    private async void HandleOpenHistoryClick(object? sender, RoutedEventArgs e)
    {
        await OpenHistoryWindowAsync().ConfigureAwait(true);
    }

    private async void HandleOpenDuplicatesClick(object? sender, RoutedEventArgs e)
    {
        await OpenDuplicatesWindowAsync().ConfigureAwait(true);
    }

    private async void HandleOpenUtilitiesClick(object? sender, RoutedEventArgs e)
    {
        await OpenUtilitiesWindowAsync(UtilityOperationMode.Copy).ConfigureAwait(true);
    }

    private async void HandleOpenEditorClick(object? sender, RoutedEventArgs e)
    {
        await OpenEditorWindowAsync().ConfigureAwait(true);
    }

    private async void HandleOpenPrintPreviewClick(object? sender, RoutedEventArgs e)
    {
        await OpenPrintPreviewWindowAsync().ConfigureAwait(true);
    }

    private void HandleTextChanged(object? sender, TextChangedEventArgs e)
    {
        RefreshActionState();
    }

    private void HandleSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        RefreshActionState();
    }

    private async Task GenerateAsync()
    {
        try
        {
            SetBusyState(true);
            _statusTextBlock.Text = AppStrings.Ui.RunningStatus;

            var request = ScanRequestFactory.Create(
                _sourceFolderTextBox.Text,
                _outputFolderTextBox.Text,
                GetSelectedOutputFormat(),
                _onlyExtensionsCheckBox.IsChecked == true,
                _createZipCheckBox.IsChecked == true,
                _collectDuplicatesCheckBox.IsChecked == true,
                _uiState,
                _appPaths);

            _outputFolderTextBox.Text = request.OutputFolder;
            var scanResult = await _scanService.ExecuteAsync(request).ConfigureAwait(true);
            ApplyScanResult(scanResult);

            var generatedFilePath = scanResult.GeneratedFilePath;
            if (!request.CreateZip
                && request.OutputFormat == OutputFormat.Txt
                && !string.IsNullOrWhiteSpace(generatedFilePath)
                && File.Exists(generatedFilePath))
            {
                await _editorWindowLauncher
                    .ShowEditorAsync(this, generatedFilePath, GetCurrentPreviewContent(), _externalLauncher, _printService)
                    .ConfigureAwait(true);
            }
        }
        catch (Exception exception)
        {
            _statusTextBlock.Text = string.Concat(AppStrings.Ui.ErrorPrefix, exception.Message);
            RefreshActionState();
        }
        finally
        {
            SetBusyState(false);
        }
    }

    private async Task OpenGeneratedFileAsync()
    {
        try
        {
            var generatedFilePath = GetCurrentGeneratedArtifactPath();
            if (string.IsNullOrWhiteSpace(generatedFilePath) || !File.Exists(generatedFilePath))
            {
                _statusTextBlock.Text = AppStrings.Ui.MissingGeneratedFileStatus;
                RefreshActionState();
                return;
            }

            await _externalLauncher.OpenFileAsync(generatedFilePath).ConfigureAwait(true);
        }
        catch (Exception exception)
        {
            _statusTextBlock.Text = string.Concat(AppStrings.Ui.ErrorPrefix, exception.Message);
        }
    }

    private async Task OpenOutputFolderAsync()
    {
        try
        {
            var outputFolder = GetOutputFolderOrDefault();
            if (string.IsNullOrWhiteSpace(outputFolder) || !Directory.Exists(outputFolder))
            {
                _statusTextBlock.Text = AppStrings.Ui.MissingOutputFolderStatus;
                RefreshActionState();
                return;
            }

            await _externalLauncher.OpenFolderAsync(outputFolder).ConfigureAwait(true);
        }
        catch (Exception exception)
        {
            _statusTextBlock.Text = string.Concat(AppStrings.Ui.ErrorPrefix, exception.Message);
        }
    }

    private async Task OpenFiltersAsync()
    {
        var filterWindow = new FilterWindow(_uiState.ActiveFilter);
        var result = await filterWindow.ShowDialog<FilterDialogResult?>(this).ConfigureAwait(true);
        if (result is null)
        {
            return;
        }

        ApplyFilterResult(result);
    }

    private async Task OpenHistoryWindowAsync()
    {
        try
        {
            var historyWindow = new HistoryWindow(
                await LoadHistoryItemsAsync().ConfigureAwait(true),
                _externalLauncher,
                LoadHistoryItemsAsync);

            await historyWindow.ShowDialog(this).ConfigureAwait(true);
        }
        catch (Exception exception)
        {
            _statusTextBlock.Text = string.Concat(AppStrings.Ui.ErrorPrefix, exception.Message);
        }
    }

    private async Task OpenDuplicatesWindowAsync()
    {
        try
        {
            var duplicateGroups = _uiState.LastResult?.DuplicateGroups ?? Array.Empty<DuplicateFileGroup>();
            if (duplicateGroups.Count == 0)
            {
                _statusTextBlock.Text = AppStrings.Ui.NoDuplicatesStatus;
                RefreshActionState();
                return;
            }

            var duplicatesWindow = new DuplicatesWindow(duplicateGroups, _externalLauncher);
            await duplicatesWindow.ShowDialog(this).ConfigureAwait(true);
        }
        catch (Exception exception)
        {
            _statusTextBlock.Text = string.Concat(AppStrings.Ui.ErrorPrefix, exception.Message);
        }
    }

    private async Task OpenUtilitiesWindowAsync(UtilityOperationMode initialMode)
    {
        try
        {
            var utilitiesWindow = new UtilitiesWindow(
                _sourceFolderTextBox.Text?.Trim() ?? string.Empty,
                GetOutputFolderOrDefault(),
                initialMode,
                _fileOperationsService);

            await utilitiesWindow.ShowDialog(this).ConfigureAwait(true);
        }
        catch (Exception exception)
        {
            _statusTextBlock.Text = string.Concat(AppStrings.Ui.ErrorPrefix, exception.Message);
        }
    }

    private async Task OpenEditorWindowAsync()
    {
        try
        {
            await _richTextEditorService
                .ShowAsync(this, GetCurrentEditableSourceFilePath(), GetCurrentPreviewContent())
                .ConfigureAwait(true);
        }
        catch (Exception exception)
        {
            _statusTextBlock.Text = string.Concat(AppStrings.Ui.ErrorPrefix, exception.Message);
        }
    }

    private async Task OpenPrintPreviewWindowAsync()
    {
        try
        {
            var previewContent = GetCurrentPreviewContent();
            var generatedFilePath = _uiState.LastResult?.GeneratedFilePath;
            if (string.IsNullOrWhiteSpace(previewContent) && string.IsNullOrWhiteSpace(generatedFilePath))
            {
                _statusTextBlock.Text = AppStrings.Ui.NoPrintPreviewStatus;
                RefreshActionState();
                return;
            }

            await _printWorkflowService
                .ShowPrintPreviewAsync(this, previewContent, generatedFilePath)
                .ConfigureAwait(true);
        }
        catch (Exception exception)
        {
            _statusTextBlock.Text = string.Concat(AppStrings.Ui.ErrorPrefix, exception.Message);
        }
    }

    private async Task SelectFolderAsync(TextBox targetTextBox, string title)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel?.StorageProvider is null)
        {
            _statusTextBlock.Text = AppStrings.Ui.PickerUnavailableStatus;
            return;
        }

        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(
            new FolderPickerOpenOptions
            {
                AllowMultiple = false,
                Title = title,
            }).ConfigureAwait(true);

        if (folders.Count == 0)
        {
            return;
        }

        var localPath = folders[0].TryGetLocalPath();
        if (string.IsNullOrWhiteSpace(localPath))
        {
            _statusTextBlock.Text = AppStrings.Ui.NonLocalFolderStatus;
            return;
        }

        targetTextBox.Text = localPath;
        RefreshActionState();
    }

    private async Task<IReadOnlyList<HistoryListItem>> LoadHistoryItemsAsync()
    {
        var outputFolder = GetOutputFolderOrDefault();
        var historyFilePath = _appPaths.GetHistoryFilePath(outputFolder);
        var entries = await _historyStore.ReadAsync(historyFilePath).ConfigureAwait(true);
        return HistoryPathResolver.Resolve(entries, outputFolder, _appPaths);
    }

    private void ClearFilter()
    {
        ApplyFilterResult(FilterDialogResult.CreateNone());
    }

    private void SetBusyState(bool isBusy)
    {
        _isBusy = isBusy;
        _sourceFolderTextBox.IsEnabled = !isBusy;
        _outputFolderTextBox.IsEnabled = !isBusy;
        _outputFormatComboBox.IsEnabled = !isBusy;
        _onlyExtensionsCheckBox.IsEnabled = !isBusy;
        _createZipCheckBox.IsEnabled = !isBusy;
        _collectDuplicatesCheckBox.IsEnabled = !isBusy;
        _browseSourceButton.IsEnabled = !isBusy;
        _browseOutputButton.IsEnabled = !isBusy;
        _generateButton.IsEnabled = !isBusy;
        _openFiltersButton.IsEnabled = !isBusy;
        _openHistoryButton.IsEnabled = !isBusy;
        _openDuplicatesButton.IsEnabled = !isBusy;
        _openUtilitiesButton.IsEnabled = !isBusy;
        _openEditorButton.IsEnabled = !isBusy;
        _openPrintPreviewButton.IsEnabled = !isBusy;
        RefreshActionState();
    }

    private void RefreshActionState()
    {
        var generatedFilePath = _uiState.LastResult?.GeneratedFilePath;
        var generatedArtifactPath = GetCurrentGeneratedArtifactPath();
        var outputFolder = GetOutputFolderOrDefault();
        var hasGeneratedFile = !string.IsNullOrWhiteSpace(generatedArtifactPath) && File.Exists(generatedArtifactPath);
        var hasOutputFolder = !string.IsNullOrWhiteSpace(outputFolder) && Directory.Exists(outputFolder);
        var hasFilter = _uiState.ActiveFilter.Mode != FilterMode.None;
        var hasDuplicateGroups = _uiState.LastResult?.DuplicateGroups.Count > 0;
        var hasPreviewContent = !string.IsNullOrWhiteSpace(GetCurrentPreviewContent());

        _generateButton.IsEnabled = !_isBusy;
        _openGeneratedFileButton.IsEnabled = !_isBusy && hasGeneratedFile;
        _openOutputFolderButton.IsEnabled = !_isBusy && hasOutputFolder;
        _openFiltersButton.IsEnabled = !_isBusy;
        _openHistoryButton.IsEnabled = !_isBusy;
        _openDuplicatesButton.IsEnabled = !_isBusy && hasDuplicateGroups;
        _openUtilitiesButton.IsEnabled = !_isBusy;
        _openEditorButton.IsEnabled = !_isBusy;
        _openPrintPreviewButton.IsEnabled = !_isBusy && (hasGeneratedFile || hasPreviewContent);

        _menuOpenGeneratedFileItem.IsEnabled = !_isBusy && hasGeneratedFile;
        _menuOpenOutputFolderItem.IsEnabled = !_isBusy && hasOutputFolder;
        _menuOpenHistoryItem.IsEnabled = !_isBusy;
        _menuOpenDuplicatesItem.IsEnabled = !_isBusy && hasDuplicateGroups;
        _menuConfigureFiltersItem.IsEnabled = !_isBusy;
        _menuClearFiltersItem.IsEnabled = !_isBusy && hasFilter;
        _menuGenerateItem.IsEnabled = !_isBusy;
        _menuCopyMoveItem.IsEnabled = !_isBusy;
        _menuReorderItem.IsEnabled = !_isBusy;
        _menuEditorItem.IsEnabled = !_isBusy;
        _menuPrintItem.IsEnabled = !_isBusy && (hasGeneratedFile || hasPreviewContent);
    }

    private OutputFormat GetSelectedOutputFormat()
    {
        return _outputFormatComboBox.SelectedItem is OutputFormatChoice selectedChoice
            ? selectedChoice.Value
            : OutputFormat.Txt;
    }

    private string GetOutputFolderOrDefault()
    {
        return string.IsNullOrWhiteSpace(_outputFolderTextBox.Text)
            ? _appPaths.GetDefaultOutputFolder()
            : _outputFolderTextBox.Text.Trim();
    }

    private string GetCurrentPreviewContent()
    {
        var content = _previewTextBox.Text ?? string.Empty;
        return string.Equals(content, AppStrings.Ui.PreviewEmpty, StringComparison.Ordinal)
            ? string.Empty
            : content;
    }

    private string? GetCurrentGeneratedArtifactPath()
    {
        if (!string.IsNullOrWhiteSpace(_uiState.LastResult?.GeneratedFilePath))
        {
            return _uiState.LastResult.GeneratedFilePath;
        }

        if (!string.IsNullOrWhiteSpace(_uiState.LastResult?.GeneratedZipPath))
        {
            return _uiState.LastResult.GeneratedZipPath;
        }

        return null;
    }

    private string? GetCurrentEditableSourceFilePath()
    {
        var generatedFilePath = _uiState.LastResult?.GeneratedFilePath;
        if (string.IsNullOrWhiteSpace(generatedFilePath) || !File.Exists(generatedFilePath))
        {
            return null;
        }

        var extension = Path.GetExtension(generatedFilePath);
        return string.Equals(extension, AppStrings.System.TextFileExtension, StringComparison.OrdinalIgnoreCase)
            || string.Equals(extension, AppStrings.System.MarkdownFileExtension, StringComparison.OrdinalIgnoreCase)
            || string.Equals(extension, AppStrings.System.JsonFileExtension, StringComparison.OrdinalIgnoreCase)
            || string.Equals(extension, AppStrings.System.CsvFileExtension, StringComparison.OrdinalIgnoreCase)
            || string.Equals(extension, AppStrings.System.XmlFileExtension, StringComparison.OrdinalIgnoreCase)
            || string.Equals(extension, AppStrings.System.LogFileExtension, StringComparison.OrdinalIgnoreCase)
            ? generatedFilePath
            : null;
    }

    private NativeMenuItem CreateMenuItem(string header, EventHandler onClick)
    {
        var menuItem = new NativeMenuItem
        {
            Header = header,
        };
        menuItem.Click += onClick;
        return menuItem;
    }

    private static NativeMenuItem CreateDisabledMenuItem(string header)
    {
        return new NativeMenuItem
        {
            Header = header,
            IsEnabled = false,
        };
    }

    private IReadOnlyList<OutputFormatChoice> CreateOutputFormatChoices()
    {
        return new[]
        {
            new OutputFormatChoice(OutputFormat.Txt, AppStrings.Ui.OutputFormatTxtLabel),
            new OutputFormatChoice(OutputFormat.Pdf, AppStrings.Ui.OutputFormatPdfLabel),
            new OutputFormatChoice(OutputFormat.Markdown, AppStrings.Ui.OutputFormatMarkdownLabel),
        };
    }

    private T GetRequiredControl<T>(string controlName)
        where T : Control
    {
        return this.FindControl<T>(controlName)
            ?? throw new InvalidOperationException(string.Concat(AppStrings.Ui.MissingControlPrefix, controlName));
    }

    private void SetText(string controlName, string value)
    {
        GetRequiredControl<TextBlock>(controlName).Text = value;
    }

    private void SetButtonContent(string controlName, string value)
    {
        GetRequiredControl<Button>(controlName).Content = value;
    }

    private void SetCheckBoxContent(string controlName, string value)
    {
        GetRequiredControl<CheckBox>(controlName).Content = value;
    }

    private static DefaultAppServices CreateNativeServices(
        IScanService scanService,
        IAppPaths appPaths,
        IHistoryStore historyStore,
        IExternalLauncher externalLauncher,
        IFileOperationsService fileOperationsService,
        IPrintService printService)
    {
        var fallbackPrintWorkflowService = new FallbackPrintWorkflowService(externalLauncher, printService);
        var printWorkflowService = new AppKitPrintWorkflowService(printService, fallbackPrintWorkflowService);
        var fallbackRichTextEditorService = new FallbackRichTextEditorService(externalLauncher, printService);
        var richTextEditorService = new AppKitRichTextEditorService(printWorkflowService, fallbackRichTextEditorService);
        return new DefaultAppServices(
            scanService,
            appPaths,
            historyStore,
            externalLauncher,
            fileOperationsService,
            printService,
            richTextEditorService,
            printWorkflowService,
            new DefaultEditorWindowLauncher(richTextEditorService));
    }

    private static DefaultAppServices CreateFallbackServices(
        IScanService scanService,
        IAppPaths appPaths,
        IHistoryStore historyStore,
        IExternalLauncher externalLauncher,
        IFileOperationsService fileOperationsService,
        IPrintService printService)
    {
        var printWorkflowService = new FallbackPrintWorkflowService(externalLauncher, printService);
        var richTextEditorService = new FallbackRichTextEditorService(externalLauncher, printService);
        return new DefaultAppServices(
            scanService,
            appPaths,
            historyStore,
            externalLauncher,
            fileOperationsService,
            printService,
            richTextEditorService,
            printWorkflowService,
            new PassiveEditorWindowLauncher());
    }

    private sealed record OutputFormatChoice(OutputFormat Value, string Label)
    {
        public override string ToString()
        {
            return Label;
        }
    }
}
