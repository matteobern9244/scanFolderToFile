using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Constants;

namespace ScanFolderToFile.App;

public sealed partial class HistoryWindow : Window
{
    private readonly IExternalLauncher _externalLauncher;
    private readonly ListBox _historyListBox;
    private readonly TextBlock _statusTextBlock;
    private readonly Button _openButton;
    private readonly Button _openFolderButton;
    private readonly Button _refreshButton;
    private readonly Func<Task<IReadOnlyList<HistoryListItem>>>? _reloadHistoryAsync;

    public HistoryWindow()
        : this(Array.Empty<HistoryListItem>(), AppServiceFactory.Create().ExternalLauncher, null)
    {
    }

    internal HistoryWindow(
        IReadOnlyList<HistoryListItem> historyItems,
        IExternalLauncher externalLauncher,
        Func<Task<IReadOnlyList<HistoryListItem>>>? reloadHistoryAsync)
    {
        _externalLauncher = externalLauncher;
        _reloadHistoryAsync = reloadHistoryAsync;

        AvaloniaXamlLoader.Load(this);

        _historyListBox = GetRequiredControl<ListBox>(AppStrings.Ui.HistoryListBoxName);
        _statusTextBlock = GetRequiredControl<TextBlock>(AppStrings.Ui.HistoryStatusTextBlockName);
        _openButton = GetRequiredControl<Button>(AppStrings.Ui.HistoryOpenButtonName);
        _openFolderButton = GetRequiredControl<Button>(AppStrings.Ui.HistoryOpenFolderButtonName);
        _refreshButton = GetRequiredControl<Button>(AppStrings.Ui.HistoryRefreshButtonName);

        Title = AppStrings.Ui.HistoryWindowTitle;
        _statusTextBlock.Text = AppStrings.Ui.HistoryStatusReady;
        _openButton.Content = AppStrings.Ui.HistoryOpenButtonText;
        _openFolderButton.Content = AppStrings.Ui.HistoryOpenFolderButtonText;
        _refreshButton.Content = AppStrings.Ui.HistoryRefreshButtonText;
        GetRequiredControl<TextBlock>(AppStrings.Ui.HistoryNameHeaderTextBlockName).Text = AppStrings.Ui.HistoryNameHeaderText;
        GetRequiredControl<TextBlock>(AppStrings.Ui.HistoryExtensionHeaderTextBlockName).Text = AppStrings.Ui.HistoryExtensionHeaderText;
        GetRequiredControl<TextBlock>(AppStrings.Ui.HistoryCreatedAtHeaderTextBlockName).Text = AppStrings.Ui.HistoryCreatedAtHeaderText;
        GetRequiredControl<TextBlock>(AppStrings.Ui.HistoryStateHeaderTextBlockName).Text = AppStrings.Ui.HistoryStateHeaderText;

        _openButton.Click += HandleOpenClick;
        _openFolderButton.Click += HandleOpenFolderClick;
        _refreshButton.Click += HandleRefreshClick;
        _historyListBox.SelectionChanged += HandleSelectionChanged;
        _refreshButton.IsEnabled = _reloadHistoryAsync is not null;

        SetItems(historyItems);
    }

    internal void SetItems(IReadOnlyList<HistoryListItem> historyItems)
    {
        _historyListBox.ItemsSource = historyItems.Count == 0
            ? new[] { AppStrings.Ui.HistoryEmpty }
            : historyItems;

        if (historyItems.Count > 0)
        {
            _historyListBox.SelectedIndex = 0;
            _statusTextBlock.Text = string.Concat(AppStrings.Ui.HistorySelectedLabelPrefix, historyItems[0].FileName);
        }
        else
        {
            _statusTextBlock.Text = AppStrings.Ui.HistoryEmpty;
        }

        RefreshButtons();
    }

    internal HistoryListItem? SelectedItem => _historyListBox.SelectedItem as HistoryListItem;

    private async void HandleOpenClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var item = SelectedItem;
            if (item is null || !item.Exists || string.IsNullOrWhiteSpace(item.FullPath))
            {
                _statusTextBlock.Text = AppStrings.Ui.HistoryMissingFileStatus;
                RefreshButtons();
                return;
            }

            await _externalLauncher.OpenFileAsync(item.FullPath).ConfigureAwait(true);
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
            var folderPath = item is null || string.IsNullOrWhiteSpace(item.FullPath)
                ? null
                : Path.GetDirectoryName(item.FullPath);

            if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
            {
                _statusTextBlock.Text = AppStrings.Ui.HistoryMissingFolderStatus;
                RefreshButtons();
                return;
            }

            await _externalLauncher.OpenFolderAsync(folderPath).ConfigureAwait(true);
        }
        catch (Exception exception)
        {
            _statusTextBlock.Text = string.Concat(AppStrings.Ui.ErrorPrefix, exception.Message);
        }
    }

    private async void HandleRefreshClick(object? sender, RoutedEventArgs e)
    {
        if (_reloadHistoryAsync is null)
        {
            return;
        }

        try
        {
            SetItems(await _reloadHistoryAsync().ConfigureAwait(true));
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
            ? AppStrings.Ui.HistoryStatusReady
            : string.Concat(AppStrings.Ui.HistorySelectedLabelPrefix, item.FileName);
        RefreshButtons();
    }

    private void RefreshButtons()
    {
        var item = SelectedItem;
        _openButton.IsEnabled = item is not null && item.Exists && !string.IsNullOrWhiteSpace(item.FullPath);
        _openFolderButton.IsEnabled = item is not null && !string.IsNullOrWhiteSpace(item.FullPath);
    }

    private T GetRequiredControl<T>(string controlName)
        where T : Control
    {
        return this.FindControl<T>(controlName)
            ?? throw new InvalidOperationException(string.Concat(AppStrings.Ui.MissingControlPrefix, controlName));
    }
}
