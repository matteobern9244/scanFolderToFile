using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Constants;

namespace ScanFolderToFile.App;

public sealed partial class EditorWindow : Window
{
    private readonly IExternalLauncher _externalLauncher;
    private readonly IPrintService _printService;
    private readonly TextBlock _pathTextBlock;
    private readonly TextBlock _statusTextBlock;
    private readonly TextBox _editorTextBox;
    private readonly Button _openButton;
    private readonly Button _saveButton;
    private readonly Button _saveAsButton;
    private readonly Button _printPreviewButton;
    private readonly Button _closeButton;
    private string? _currentFilePath;
    private bool _canSaveDirectly;

    public EditorWindow()
        : this(null, string.Empty, new MacExternalLauncher(), new MacPrintService())
    {
    }

    internal EditorWindow(
        string? initialFilePath,
        string initialText,
        IExternalLauncher externalLauncher,
        IPrintService printService)
    {
        _externalLauncher = externalLauncher;
        _printService = printService;

        AvaloniaXamlLoader.Load(this);

        _pathTextBlock = GetRequiredControl<TextBlock>(AppStrings.Ui.EditorPathTextBlockName);
        _statusTextBlock = GetRequiredControl<TextBlock>(AppStrings.Ui.EditorStatusTextBlockName);
        _editorTextBox = GetRequiredControl<TextBox>(AppStrings.Ui.EditorTextBoxName);
        _openButton = GetRequiredControl<Button>(AppStrings.Ui.EditorOpenButtonName);
        _saveButton = GetRequiredControl<Button>(AppStrings.Ui.EditorSaveButtonName);
        _saveAsButton = GetRequiredControl<Button>(AppStrings.Ui.EditorSaveAsButtonName);
        _printPreviewButton = GetRequiredControl<Button>(AppStrings.Ui.EditorPrintPreviewButtonName);
        _closeButton = GetRequiredControl<Button>(AppStrings.Ui.EditorCloseButtonName);

        Title = AppStrings.Ui.EditorWindowTitle;
        _openButton.Content = AppStrings.Ui.EditorOpenButtonText;
        _saveButton.Content = AppStrings.Ui.EditorSaveButtonText;
        _saveAsButton.Content = AppStrings.Ui.EditorSaveAsButtonText;
        _printPreviewButton.Content = AppStrings.Ui.EditorPrintPreviewButtonText;
        _closeButton.Content = AppStrings.Ui.EditorCloseButtonText;

        InitializeDocument(initialFilePath, initialText);
        RefreshState();

        _openButton.Click += HandleOpenClick;
        _saveButton.Click += HandleSaveClick;
        _saveAsButton.Click += HandleSaveAsClick;
        _printPreviewButton.Click += HandlePrintPreviewClick;
        _closeButton.Click += HandleCloseClick;
        _editorTextBox.TextChanged += HandleTextChanged;
    }

    internal string CurrentDocumentText => _editorTextBox.Text ?? string.Empty;

    internal bool CanSaveDirectly => _canSaveDirectly;

    private async void HandleOpenClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel?.StorageProvider is null)
        {
            _statusTextBlock.Text = AppStrings.Ui.PickerUnavailableStatus;
            return;
        }

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
            {
                AllowMultiple = false,
                Title = AppStrings.Ui.EditorOpenDialogTitle,
            }).ConfigureAwait(true);

        if (files.Count == 0)
        {
            return;
        }

        var filePath = files[0].TryGetLocalPath();
        if (string.IsNullOrWhiteSpace(filePath))
        {
            _statusTextBlock.Text = AppStrings.Ui.NonLocalFolderStatus;
            return;
        }

        LoadFromFile(filePath);
    }

    private async void HandleSaveClick(object? sender, RoutedEventArgs e)
    {
        if (_canSaveDirectly && !string.IsNullOrWhiteSpace(_currentFilePath))
        {
            await SaveToPathAsync(_currentFilePath).ConfigureAwait(true);
            return;
        }

        await SaveAsAsync().ConfigureAwait(true);
    }

    private async void HandleSaveAsClick(object? sender, RoutedEventArgs e)
    {
        await SaveAsAsync().ConfigureAwait(true);
    }

    private async void HandlePrintPreviewClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CurrentDocumentText))
        {
            _statusTextBlock.Text = AppStrings.Ui.NoPrintPreviewStatus;
            return;
        }

        var window = new PrintPreviewWindow(CurrentDocumentText, _currentFilePath, _externalLauncher, _printService);
        await window.ShowDialog(this).ConfigureAwait(true);
    }

    private void HandleCloseClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void HandleTextChanged(object? sender, TextChangedEventArgs e)
    {
        RefreshState();
    }

    private void InitializeDocument(string? initialFilePath, string initialText)
    {
        if (!string.IsNullOrWhiteSpace(initialFilePath) && File.Exists(initialFilePath))
        {
            if (IsEditableFile(initialFilePath))
            {
                LoadFromFile(initialFilePath);
                return;
            }

            _currentFilePath = null;
            _canSaveDirectly = false;
            _editorTextBox.Text = initialText;
            _pathTextBlock.Text = initialFilePath;
            _statusTextBlock.Text = string.IsNullOrWhiteSpace(initialText)
                ? AppStrings.Ui.EditorUnsupportedFileStatus
                : AppStrings.Ui.EditorLoadedFromPreviewStatus;
            return;
        }

        _currentFilePath = null;
        _canSaveDirectly = false;
        _editorTextBox.Text = initialText;
        _pathTextBlock.Text = AppStrings.Ui.EditorUntitledLabel;
        _statusTextBlock.Text = string.IsNullOrWhiteSpace(initialText)
            ? AppStrings.Ui.NoEditorSourceStatus
            : AppStrings.Ui.EditorLoadedFromPreviewStatus;
    }

    private void LoadFromFile(string filePath)
    {
        if (!IsEditableFile(filePath))
        {
            _statusTextBlock.Text = AppStrings.Ui.EditorUnsupportedFileStatus;
            _currentFilePath = null;
            _canSaveDirectly = false;
            RefreshState();
            return;
        }

        _editorTextBox.Text = File.ReadAllText(filePath);
        _currentFilePath = filePath;
        _canSaveDirectly = true;
        _pathTextBlock.Text = filePath;
        _statusTextBlock.Text = string.Concat(AppStrings.Ui.EditorLoadedFromFilePrefix, filePath);
        RefreshState();
    }

    private async Task SaveAsAsync()
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel?.StorageProvider is null)
        {
            _statusTextBlock.Text = AppStrings.Ui.PickerUnavailableStatus;
            return;
        }

        var storageFile = await topLevel.StorageProvider.SaveFilePickerAsync(
            new FilePickerSaveOptions
            {
                Title = AppStrings.Ui.EditorSaveDialogTitle,
                SuggestedFileName = GetSuggestedSaveName(),
            }).ConfigureAwait(true);

        var savePath = storageFile?.TryGetLocalPath();
        if (string.IsNullOrWhiteSpace(savePath))
        {
            return;
        }

        await SaveToPathAsync(savePath).ConfigureAwait(true);
    }

    private async Task SaveToPathAsync(string filePath)
    {
        await File.WriteAllTextAsync(filePath, CurrentDocumentText).ConfigureAwait(true);
        _currentFilePath = filePath;
        _canSaveDirectly = IsEditableFile(filePath);
        _pathTextBlock.Text = filePath;
        _statusTextBlock.Text = AppStrings.Ui.EditorSavedStatus;
        RefreshState();
    }

    private void RefreshState()
    {
        _saveButton.IsEnabled = _canSaveDirectly || !string.IsNullOrWhiteSpace(CurrentDocumentText);
        _saveAsButton.IsEnabled = true;
        _printPreviewButton.IsEnabled = !string.IsNullOrWhiteSpace(CurrentDocumentText);
    }

    private string GetSuggestedSaveName()
    {
        return string.IsNullOrWhiteSpace(_currentFilePath)
            ? AppStrings.Ui.EditorSuggestedSaveName
            : Path.GetFileName(_currentFilePath);
    }

    private static bool IsEditableFile(string filePath)
    {
        var extension = Path.GetExtension(filePath);
        return string.Equals(extension, AppStrings.System.TextFileExtension, StringComparison.OrdinalIgnoreCase)
            || string.Equals(extension, AppStrings.System.MarkdownFileExtension, StringComparison.OrdinalIgnoreCase)
            || string.Equals(extension, AppStrings.System.JsonFileExtension, StringComparison.OrdinalIgnoreCase)
            || string.Equals(extension, AppStrings.System.CsvFileExtension, StringComparison.OrdinalIgnoreCase)
            || string.Equals(extension, AppStrings.System.XmlFileExtension, StringComparison.OrdinalIgnoreCase)
            || string.Equals(extension, AppStrings.System.LogFileExtension, StringComparison.OrdinalIgnoreCase);
    }

    private T GetRequiredControl<T>(string controlName)
        where T : Control
    {
        return this.FindControl<T>(controlName)
            ?? throw new InvalidOperationException(string.Concat(AppStrings.Ui.MissingControlPrefix, controlName));
    }
}
