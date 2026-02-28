using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Constants;
using ScanFolderToFile.Core.Models;

namespace ScanFolderToFile.App;

public sealed partial class MainWindow : Window
{
    private readonly IScanService _scanService;
    private readonly IAppPaths _appPaths;
    private readonly IHistoryStore _historyStore;
    private readonly IExternalLauncher _externalLauncher;
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
    private readonly NativeMenuItem _menuOpenGeneratedFileItem;
    private readonly NativeMenuItem _menuOpenOutputFolderItem;
    private readonly NativeMenuItem _menuOpenHistoryItem;
    private readonly NativeMenuItem _menuConfigureFiltersItem;
    private readonly NativeMenuItem _menuClearFiltersItem;
    private readonly NativeMenuItem _menuGenerateItem;
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
        : this(new DefaultAppServices(scanService, appPaths, historyStore, externalLauncher))
    {
    }

    private MainWindow(DefaultAppServices services)
    {
        _scanService = services.ScanService;
        _appPaths = services.AppPaths;
        _historyStore = services.HistoryStore;
        _externalLauncher = services.ExternalLauncher;
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
        _formatChoices = CreateOutputFormatChoices();

        _menuOpenGeneratedFileItem = CreateMenuItem(AppStrings.Ui.MenuOpenGeneratedFile, (_, _) => _ = OpenGeneratedFileAsync());
        _menuOpenOutputFolderItem = CreateMenuItem(AppStrings.Ui.MenuOpenOutputFolder, (_, _) => _ = OpenOutputFolderAsync());
        _menuOpenHistoryItem = CreateMenuItem(AppStrings.Ui.MenuOpenHistory, (_, _) => _ = OpenHistoryWindowAsync());
        _menuConfigureFiltersItem = CreateMenuItem(AppStrings.Ui.MenuConfigureFilters, (_, _) => _ = OpenFiltersAsync());
        _menuClearFiltersItem = CreateMenuItem(AppStrings.Ui.MenuClearFilters, (_, _) => ClearFilter());
        _menuGenerateItem = CreateMenuItem(AppStrings.Ui.MenuGenerate, (_, _) => _ = GenerateAsync());

        Title = AppStrings.Ui.WindowTitle;
        InitializeText();
        InitializeControls();
        InitializeNativeMenu();
        InitializeInteractions();
        RefreshActionState();
    }

    internal string CurrentStatus => _statusTextBlock.Text ?? string.Empty;

    internal string CurrentFilterSummary => _filterSummaryTextBlock.Text ?? string.Empty;

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
        _statusTextBlock.Text = string.Concat(
            AppStrings.Ui.GenerateSuccessPrefix,
            scanResult.GeneratedFilePath ?? AppStrings.Ui.ResultUnavailableValue);
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
                    _menuOpenOutputFolderItem,
                    _menuOpenHistoryItem,
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
                    CreateDisabledMenuItem(AppStrings.Ui.MenuCopyMove),
                    CreateDisabledMenuItem(AppStrings.Ui.MenuReorder),
                    CreateDisabledMenuItem(AppStrings.Ui.MenuEditor),
                    CreateDisabledMenuItem(AppStrings.Ui.MenuPrint),
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
            ApplyScanResult(await _scanService.ExecuteAsync(request).ConfigureAwait(true));
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
            var generatedFilePath = _uiState.LastResult?.GeneratedFilePath;
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
        RefreshActionState();
    }

    private void RefreshActionState()
    {
        var generatedFilePath = _uiState.LastResult?.GeneratedFilePath;
        var outputFolder = GetOutputFolderOrDefault();
        var hasGeneratedFile = !string.IsNullOrWhiteSpace(generatedFilePath) && File.Exists(generatedFilePath);
        var hasOutputFolder = !string.IsNullOrWhiteSpace(outputFolder) && Directory.Exists(outputFolder);
        var hasFilter = _uiState.ActiveFilter.Mode != FilterMode.None;

        _generateButton.IsEnabled = !_isBusy;
        _openGeneratedFileButton.IsEnabled = !_isBusy && hasGeneratedFile;
        _openOutputFolderButton.IsEnabled = !_isBusy && hasOutputFolder;
        _openFiltersButton.IsEnabled = !_isBusy;
        _openHistoryButton.IsEnabled = !_isBusy;

        _menuOpenGeneratedFileItem.IsEnabled = !_isBusy && hasGeneratedFile;
        _menuOpenOutputFolderItem.IsEnabled = !_isBusy && hasOutputFolder;
        _menuOpenHistoryItem.IsEnabled = !_isBusy;
        _menuConfigureFiltersItem.IsEnabled = !_isBusy;
        _menuClearFiltersItem.IsEnabled = !_isBusy && hasFilter;
        _menuGenerateItem.IsEnabled = !_isBusy;
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

    private sealed record OutputFormatChoice(OutputFormat Value, string Label)
    {
        public override string ToString()
        {
            return Label;
        }
    }
}
