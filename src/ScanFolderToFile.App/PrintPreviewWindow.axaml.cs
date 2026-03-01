using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Constants;

namespace ScanFolderToFile.App;

public sealed partial class PrintPreviewWindow : Window
{
    private readonly IExternalLauncher _externalLauncher;
    private readonly IPrintService _printService;
    private readonly TextBlock _summaryTextBlock;
    private readonly TextBlock _statusTextBlock;
    private readonly TextBox _previewTextBox;
    private readonly Button _printButton;
    private readonly Button _openFileButton;
    private readonly Button _closeButton;
    private readonly string? _sourceFilePath;
    private readonly bool _hasBufferContent;

    public PrintPreviewWindow()
        : this(string.Empty, null, new MacExternalLauncher(), new MacPrintService())
    {
    }

    internal PrintPreviewWindow(
        string previewText,
        string? sourceFilePath,
        IExternalLauncher externalLauncher,
        IPrintService printService)
    {
        _externalLauncher = externalLauncher;
        _printService = printService;
        _sourceFilePath = sourceFilePath;
        _hasBufferContent = !string.IsNullOrWhiteSpace(previewText);

        AvaloniaXamlLoader.Load(this);

        _summaryTextBlock = GetRequiredControl<TextBlock>(AppStrings.Ui.PrintPreviewSummaryTextBlockName);
        _statusTextBlock = GetRequiredControl<TextBlock>(AppStrings.Ui.PrintPreviewStatusTextBlockName);
        _previewTextBox = GetRequiredControl<TextBox>(AppStrings.Ui.PrintPreviewTextBoxName);
        _printButton = GetRequiredControl<Button>(AppStrings.Ui.PrintPreviewPrintButtonName);
        _openFileButton = GetRequiredControl<Button>(AppStrings.Ui.PrintPreviewOpenFileButtonName);
        _closeButton = GetRequiredControl<Button>(AppStrings.Ui.PrintPreviewCloseButtonName);

        Title = AppStrings.Ui.PrintPreviewWindowTitle;
        _printButton.Content = AppStrings.Ui.PrintPreviewPrintButtonText;
        _openFileButton.Content = AppStrings.Ui.PrintPreviewOpenFileButtonText;
        _closeButton.Content = AppStrings.Ui.PrintPreviewCloseButtonText;

        _previewTextBox.Text = string.IsNullOrWhiteSpace(previewText)
            ? AppStrings.Ui.PrintPreviewNoContent
            : previewText;
        _summaryTextBlock.Text = BuildSummary(sourceFilePath);
        _statusTextBlock.Text = string.IsNullOrWhiteSpace(previewText) && string.IsNullOrWhiteSpace(sourceFilePath)
            ? AppStrings.Ui.NoPrintPreviewStatus
            : AppStrings.Ui.PrintPreviewReadyStatus;

        _printButton.Click += HandlePrintClick;
        _openFileButton.Click += HandleOpenFileClick;
        _closeButton.Click += HandleCloseClick;
        RefreshState();
    }

    internal string CurrentPreviewText => _previewTextBox.Text ?? string.Empty;

    internal bool CanPrint => _printButton.IsEnabled;

    private async void HandlePrintClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(_sourceFilePath) && File.Exists(_sourceFilePath))
            {
                await _printService.PrintFileAsync(_sourceFilePath).ConfigureAwait(true);
            }
            else
            {
                await _printService.PrintTextAsync(CurrentPreviewText).ConfigureAwait(true);
            }

            _statusTextBlock.Text = AppStrings.Ui.PrintPreviewPrintedStatus;
        }
        catch (Exception exception)
        {
            _statusTextBlock.Text = string.Concat(AppStrings.Ui.ErrorPrefix, exception.Message);
        }
    }

    private async void HandleOpenFileClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_sourceFilePath) || !File.Exists(_sourceFilePath))
            {
                _statusTextBlock.Text = AppStrings.Ui.PrintPreviewMissingFileStatus;
                RefreshState();
                return;
            }

            await _externalLauncher.OpenFileAsync(_sourceFilePath).ConfigureAwait(true);
        }
        catch (Exception exception)
        {
            _statusTextBlock.Text = string.Concat(AppStrings.Ui.ErrorPrefix, exception.Message);
        }
    }

    private void HandleCloseClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void RefreshState()
    {
        _openFileButton.IsEnabled = !string.IsNullOrWhiteSpace(_sourceFilePath) && File.Exists(_sourceFilePath);
        _printButton.IsEnabled = _openFileButton.IsEnabled || _hasBufferContent;
    }

    private static string BuildSummary(string? sourceFilePath)
    {
        var location = string.IsNullOrWhiteSpace(sourceFilePath)
            ? AppStrings.Ui.EditorUntitledLabel
            : sourceFilePath;
        var origin = string.IsNullOrWhiteSpace(sourceFilePath)
            ? AppStrings.Ui.PrintPreviewFromBufferSuffix
            : AppStrings.Ui.PrintPreviewFromFileSuffix;

        return string.Concat(
            AppStrings.Ui.PrintPreviewSummaryPrefix,
            AppStrings.System.NameValueSeparator,
            location,
            AppStrings.System.OpenParenthesisWithLeadingSpace,
            origin,
            AppStrings.System.CloseParenthesis);
    }

    private T GetRequiredControl<T>(string controlName)
        where T : Control
    {
        return this.FindControl<T>(controlName)
            ?? throw new InvalidOperationException(string.Concat(AppStrings.Ui.MissingControlPrefix, controlName));
    }
}
