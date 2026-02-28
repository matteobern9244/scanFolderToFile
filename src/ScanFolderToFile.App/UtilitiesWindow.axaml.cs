using System.Globalization;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Constants;
using ScanFolderToFile.Core.Models;
using ScanFolderToFile.Core.Services;

namespace ScanFolderToFile.App;

public sealed partial class UtilitiesWindow : Window
{
    private readonly IFileOperationsService _fileOperationsService;
    private readonly TextBox _sourceTextBox;
    private readonly TextBox _destinationTextBox;
    private readonly TextBlock _statusTextBlock;
    private readonly TextBox _resultTextBox;
    private readonly Button _browseSourceButton;
    private readonly Button _browseDestinationButton;
    private readonly Button _executeButton;
    private readonly Button _closeButton;
    private readonly RadioButton _copyRadioButton;
    private readonly RadioButton _moveRadioButton;
    private readonly RadioButton _reorderRadioButton;

    public UtilitiesWindow()
        : this(string.Empty, string.Empty, UtilityOperationMode.Copy, new FileOperationsService())
    {
    }

    internal UtilitiesWindow(
        string sourceFolder,
        string destinationFolder,
        UtilityOperationMode initialMode,
        IFileOperationsService fileOperationsService)
    {
        _fileOperationsService = fileOperationsService;

        AvaloniaXamlLoader.Load(this);

        _sourceTextBox = GetRequiredControl<TextBox>(AppStrings.Ui.UtilitySourceTextBoxName);
        _destinationTextBox = GetRequiredControl<TextBox>(AppStrings.Ui.UtilityDestinationTextBoxName);
        _statusTextBlock = GetRequiredControl<TextBlock>(AppStrings.Ui.UtilityStatusTextBlockName);
        _resultTextBox = GetRequiredControl<TextBox>(AppStrings.Ui.UtilityResultTextBoxName);
        _browseSourceButton = GetRequiredControl<Button>(AppStrings.Ui.UtilityBrowseSourceButtonName);
        _browseDestinationButton = GetRequiredControl<Button>(AppStrings.Ui.UtilityBrowseDestinationButtonName);
        _executeButton = GetRequiredControl<Button>(AppStrings.Ui.UtilityExecuteButtonName);
        _closeButton = GetRequiredControl<Button>(AppStrings.Ui.UtilityCloseButtonName);
        _copyRadioButton = GetRequiredControl<RadioButton>(AppStrings.Ui.UtilityCopyRadioButtonName);
        _moveRadioButton = GetRequiredControl<RadioButton>(AppStrings.Ui.UtilityMoveRadioButtonName);
        _reorderRadioButton = GetRequiredControl<RadioButton>(AppStrings.Ui.UtilityReorderRadioButtonName);

        Title = AppStrings.Ui.UtilitiesWindowTitle;
        GetRequiredControl<TextBlock>(AppStrings.Ui.UtilitiesModeLabelTextBlockName).Text = AppStrings.Ui.UtilitiesModeLabel;
        GetRequiredControl<TextBlock>(AppStrings.Ui.UtilitySourceLabelTextBlockName).Text = AppStrings.Ui.UtilitySourceLabel;
        GetRequiredControl<TextBlock>(AppStrings.Ui.UtilityDestinationLabelTextBlockName).Text = AppStrings.Ui.UtilityDestinationLabel;
        _copyRadioButton.Content = AppStrings.Ui.UtilityCopyText;
        _moveRadioButton.Content = AppStrings.Ui.UtilityMoveText;
        _reorderRadioButton.Content = AppStrings.Ui.UtilityReorderText;
        _browseSourceButton.Content = AppStrings.Ui.UtilityBrowseSourceText;
        _browseDestinationButton.Content = AppStrings.Ui.UtilityBrowseDestinationText;
        _executeButton.Content = AppStrings.Ui.UtilityExecuteButtonText;
        _closeButton.Content = AppStrings.Ui.UtilityCloseButtonText;

        _sourceTextBox.Text = sourceFolder;
        _destinationTextBox.Text = destinationFolder;
        _statusTextBlock.Text = AppStrings.Ui.UtilityReadyStatus;
        _resultTextBox.Text = AppStrings.Ui.UtilityResultEmpty;

        SetMode(initialMode);
        RefreshDestinationState();

        _browseSourceButton.Click += HandleBrowseSourceClick;
        _browseDestinationButton.Click += HandleBrowseDestinationClick;
        _executeButton.Click += HandleExecuteClick;
        _closeButton.Click += HandleCloseClick;
        _copyRadioButton.IsCheckedChanged += HandleModeChanged;
        _moveRadioButton.IsCheckedChanged += HandleModeChanged;
        _reorderRadioButton.IsCheckedChanged += HandleModeChanged;
    }

    internal bool DestinationEnabled => _destinationTextBox.IsEnabled;

    private async void HandleBrowseSourceClick(object? sender, RoutedEventArgs e)
    {
        await SelectFolderAsync(_sourceTextBox, AppStrings.Ui.UtilityBrowseSourceDialogTitle).ConfigureAwait(true);
    }

    private async void HandleBrowseDestinationClick(object? sender, RoutedEventArgs e)
    {
        await SelectFolderAsync(_destinationTextBox, AppStrings.Ui.UtilityBrowseDestinationDialogTitle).ConfigureAwait(true);
    }

    private async void HandleExecuteClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var mode = GetSelectedMode();
            var sourceFolder = _sourceTextBox.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(sourceFolder) || !Directory.Exists(sourceFolder))
            {
                _statusTextBlock.Text = AppStrings.Ui.UtilityNoSourceStatus;
                return;
            }

            FileOperationResult result;
            if (mode == UtilityOperationMode.Reorder)
            {
                result = await _fileOperationsService.ReorderByExtensionAsync(sourceFolder).ConfigureAwait(true);
                _statusTextBlock.Text = string.Concat(
                    AppStrings.Ui.UtilityReorderSuccessPrefix,
                    result.AffectedPaths.Count.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                var destinationFolder = _destinationTextBox.Text?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(destinationFolder))
                {
                    _statusTextBlock.Text = AppStrings.Ui.UtilityNoDestinationStatus;
                    return;
                }

                result = mode == UtilityOperationMode.Copy
                    ? await _fileOperationsService.CopyAsync(sourceFolder, destinationFolder).ConfigureAwait(true)
                    : await _fileOperationsService.MoveAsync(sourceFolder, destinationFolder).ConfigureAwait(true);

                _statusTextBlock.Text = string.Concat(
                    mode == UtilityOperationMode.Copy
                        ? AppStrings.Ui.UtilityCopySuccessPrefix
                        : AppStrings.Ui.UtilityMoveSuccessPrefix,
                    result.AffectedPaths.Count.ToString(CultureInfo.InvariantCulture));
            }

            _resultTextBox.Text = BuildResultText(result);
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

    private void HandleModeChanged(object? sender, RoutedEventArgs e)
    {
        RefreshDestinationState();
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
    }

    private void SetMode(UtilityOperationMode mode)
    {
        _copyRadioButton.IsChecked = mode == UtilityOperationMode.Copy;
        _moveRadioButton.IsChecked = mode == UtilityOperationMode.Move;
        _reorderRadioButton.IsChecked = mode == UtilityOperationMode.Reorder;
    }

    private UtilityOperationMode GetSelectedMode()
    {
        if (_moveRadioButton.IsChecked == true)
        {
            return UtilityOperationMode.Move;
        }

        if (_reorderRadioButton.IsChecked == true)
        {
            return UtilityOperationMode.Reorder;
        }

        return UtilityOperationMode.Copy;
    }

    private void RefreshDestinationState()
    {
        var isReorder = GetSelectedMode() == UtilityOperationMode.Reorder;
        _destinationTextBox.IsEnabled = !isReorder;
        _browseDestinationButton.IsEnabled = !isReorder;

        if (isReorder)
        {
            _statusTextBlock.Text = AppStrings.Ui.UtilityDestinationDisabledStatus;
        }
        else if (_statusTextBlock.Text == AppStrings.Ui.UtilityDestinationDisabledStatus)
        {
            _statusTextBlock.Text = AppStrings.Ui.UtilityReadyStatus;
        }
    }

    private static string BuildResultText(FileOperationResult result)
    {
        if (result.AffectedPaths.Count == 0 && result.SkippedPaths.Count == 0)
        {
            return AppStrings.Ui.UtilityResultEmpty;
        }

        var builder = new StringBuilder();
        builder.AppendLine(AppStrings.Ui.UtilityAffectedTitle);
        foreach (var path in result.AffectedPaths)
        {
            builder.AppendLine(path);
        }

        if (result.SkippedPaths.Count > 0)
        {
            builder.AppendLine();
            builder.AppendLine(AppStrings.Ui.UtilitySkippedTitle);
            foreach (var path in result.SkippedPaths)
            {
                builder.AppendLine(path);
            }
        }

        return builder.ToString().TrimEnd();
    }

    private T GetRequiredControl<T>(string controlName)
        where T : Control
    {
        return this.FindControl<T>(controlName)
            ?? throw new InvalidOperationException(string.Concat(AppStrings.Ui.MissingControlPrefix, controlName));
    }
}
