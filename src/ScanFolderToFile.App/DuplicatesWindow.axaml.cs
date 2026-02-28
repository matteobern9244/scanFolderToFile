using System.Globalization;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Constants;
using ScanFolderToFile.Core.Models;

namespace ScanFolderToFile.App;

public sealed partial class DuplicatesWindow : Window
{
    private readonly IExternalLauncher _externalLauncher;
    private readonly ListBox _duplicatesListBox;
    private readonly TextBlock _statusTextBlock;
    private readonly TextBlock _summaryTextBlock;
    private readonly TextBlock _emptyTextBlock;
    private readonly Button _openFileButton;
    private readonly Button _openFolderButton;

    public DuplicatesWindow()
        : this(Array.Empty<DuplicateFileGroup>(), AppServiceFactory.Create().ExternalLauncher)
    {
    }

    internal DuplicatesWindow(
        IReadOnlyList<DuplicateFileGroup> duplicateGroups,
        IExternalLauncher externalLauncher)
    {
        _externalLauncher = externalLauncher;

        AvaloniaXamlLoader.Load(this);

        _duplicatesListBox = GetRequiredControl<ListBox>(AppStrings.Ui.DuplicatesListBoxName);
        _statusTextBlock = GetRequiredControl<TextBlock>(AppStrings.Ui.DuplicatesStatusTextBlockName);
        _summaryTextBlock = GetRequiredControl<TextBlock>(AppStrings.Ui.DuplicatesSummaryTextBlockName);
        _emptyTextBlock = GetRequiredControl<TextBlock>(AppStrings.Ui.DuplicatesEmptyTextBlockName);
        _openFileButton = GetRequiredControl<Button>(AppStrings.Ui.DuplicatesOpenFileButtonName);
        _openFolderButton = GetRequiredControl<Button>(AppStrings.Ui.DuplicatesOpenFolderButtonName);

        Title = AppStrings.Ui.DuplicatesWindowTitle;
        _openFileButton.Content = AppStrings.Ui.DuplicatesOpenFileButtonText;
        _openFolderButton.Content = AppStrings.Ui.DuplicatesOpenFolderButtonText;
        _statusTextBlock.Text = AppStrings.Ui.DuplicatesStatusReady;
        _emptyTextBlock.Text = AppStrings.Ui.DuplicatesEmpty;

        _openFileButton.Click += HandleOpenFileClick;
        _openFolderButton.Click += HandleOpenFolderClick;
        _duplicatesListBox.SelectionChanged += HandleSelectionChanged;

        SetItems(Flatten(duplicateGroups));
    }

    internal DuplicateFileListItem? SelectedItem => _duplicatesListBox.SelectedItem as DuplicateFileListItem;

    internal void SetItems(IReadOnlyList<DuplicateFileListItem> duplicateItems)
    {
        var hasItems = duplicateItems.Count > 0;
        _duplicatesListBox.IsVisible = hasItems;
        _duplicatesListBox.ItemsSource = duplicateItems;
        _emptyTextBlock.IsVisible = !hasItems;

        _summaryTextBlock.Text = string.Concat(
            AppStrings.Ui.DuplicatesSummaryPrefix,
            duplicateItems
                .Select(item => item.BaseName)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count()
                .ToString(CultureInfo.InvariantCulture));

        if (hasItems)
        {
            _duplicatesListBox.SelectedIndex = 0;
            _statusTextBlock.Text = string.Concat(AppStrings.Ui.DuplicatesSelectedPrefix, duplicateItems[0].FileName);
        }
        else
        {
            _statusTextBlock.Text = AppStrings.Ui.DuplicatesEmpty;
        }

        RefreshButtons();
    }

    private async void HandleOpenFileClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var item = SelectedItem;
            if (item is null || !item.Exists)
            {
                _statusTextBlock.Text = AppStrings.Ui.DuplicatesMissingFileStatus;
                RefreshButtons();
                return;
            }

            await _externalLauncher.OpenFileAsync(item.FilePath).ConfigureAwait(true);
        }
        catch (Exception exception)
        {
            _statusTextBlock.Text = string.Concat(AppStrings.Ui.ErrorPrefix, exception.Message);
        }
    }

    private async void HandleOpenFolderClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var item = SelectedItem;
            if (item is null || string.IsNullOrWhiteSpace(item.FolderPath) || !Directory.Exists(item.FolderPath))
            {
                _statusTextBlock.Text = AppStrings.Ui.DuplicatesMissingFolderStatus;
                RefreshButtons();
                return;
            }

            await _externalLauncher.OpenFolderAsync(item.FolderPath).ConfigureAwait(true);
        }
        catch (Exception exception)
        {
            _statusTextBlock.Text = string.Concat(AppStrings.Ui.ErrorPrefix, exception.Message);
        }
    }

    private void HandleSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var item = SelectedItem;
        _statusTextBlock.Text = item is null
            ? AppStrings.Ui.DuplicatesStatusReady
            : string.Concat(AppStrings.Ui.DuplicatesSelectedPrefix, item.FileName);
        RefreshButtons();
    }

    private void RefreshButtons()
    {
        var item = SelectedItem;
        _openFileButton.IsEnabled = item is not null && item.Exists;
        _openFolderButton.IsEnabled = item is not null && !string.IsNullOrWhiteSpace(item.FolderPath);
    }

    private static IReadOnlyList<DuplicateFileListItem> Flatten(IReadOnlyList<DuplicateFileGroup> duplicateGroups)
    {
        return duplicateGroups
            .SelectMany(
                duplicateGroup => duplicateGroup.FilePaths.Select(
                    filePath => new DuplicateFileListItem(duplicateGroup.BaseName, filePath)))
            .ToArray();
    }

    private T GetRequiredControl<T>(string controlName)
        where T : Control
    {
        return this.FindControl<T>(controlName)
            ?? throw new InvalidOperationException(string.Concat(AppStrings.Ui.MissingControlPrefix, controlName));
    }
}
