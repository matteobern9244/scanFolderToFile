using System.Globalization;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ScanFolderToFile.Core.Constants;

namespace ScanFolderToFile.App;

public sealed partial class FilterWindow : Window
{
    private readonly RadioButton _noneRadioButton;
    private readonly RadioButton _sizeRadioButton;
    private readonly RadioButton _dateRadioButton;
    private readonly TextBox _minSizeTextBox;
    private readonly TextBox _maxSizeTextBox;
    private readonly TextBox _startDateTextBox;
    private readonly TextBox _endDateTextBox;
    private readonly TextBlock _statusTextBlock;
    private readonly Button _applyButton;
    private readonly Button _clearButton;
    private readonly Button _cancelButton;

    public FilterWindow()
        : this(FilterDialogResult.CreateNone())
    {
    }

    internal FilterWindow(FilterDialogResult currentFilter)
    {
        AvaloniaXamlLoader.Load(this);

        _noneRadioButton = GetRequiredControl<RadioButton>(AppStrings.Ui.FilterNoneRadioButtonName);
        _sizeRadioButton = GetRequiredControl<RadioButton>(AppStrings.Ui.FilterSizeRadioButtonName);
        _dateRadioButton = GetRequiredControl<RadioButton>(AppStrings.Ui.FilterDateRadioButtonName);
        _minSizeTextBox = GetRequiredControl<TextBox>(AppStrings.Ui.FilterMinSizeTextBoxName);
        _maxSizeTextBox = GetRequiredControl<TextBox>(AppStrings.Ui.FilterMaxSizeTextBoxName);
        _startDateTextBox = GetRequiredControl<TextBox>(AppStrings.Ui.FilterStartDateTextBoxName);
        _endDateTextBox = GetRequiredControl<TextBox>(AppStrings.Ui.FilterEndDateTextBoxName);
        _statusTextBlock = GetRequiredControl<TextBlock>(AppStrings.Ui.FilterStatusTextBlockName);
        _applyButton = GetRequiredControl<Button>(AppStrings.Ui.FilterApplyButtonName);
        _clearButton = GetRequiredControl<Button>(AppStrings.Ui.FilterClearButtonName);
        _cancelButton = GetRequiredControl<Button>(AppStrings.Ui.FilterCancelButtonName);

        Title = AppStrings.Ui.FilterWindowTitle;
        InitializeText();
        InitializeEvents();
        ApplyCurrentFilter(currentFilter);
        UpdateInputState();
    }

    private void InitializeText()
    {
        SetText(AppStrings.Ui.FilterWindowModeLabelTextBlockName, AppStrings.Ui.FilterWindowModeLabel);
        SetText(AppStrings.Ui.FilterMinSizeLabelTextBlockName, AppStrings.Ui.FilterMinSizeLabel);
        SetText(AppStrings.Ui.FilterMaxSizeLabelTextBlockName, AppStrings.Ui.FilterMaxSizeLabel);
        SetText(AppStrings.Ui.FilterStartDateLabelTextBlockName, AppStrings.Ui.FilterStartDateLabel);
        SetText(AppStrings.Ui.FilterEndDateLabelTextBlockName, AppStrings.Ui.FilterEndDateLabel);
        SetText(AppStrings.Ui.FilterDateHintTextBlockName, AppStrings.Ui.FilterDateHint);
        SetText(AppStrings.Ui.FilterStatusTextBlockName, AppStrings.Ui.FilterStatusReady);
        SetButtonContent(AppStrings.Ui.FilterNoneRadioButtonName, AppStrings.Ui.FilterNoneOptionText);
        SetButtonContent(AppStrings.Ui.FilterSizeRadioButtonName, AppStrings.Ui.FilterSizeOptionText);
        SetButtonContent(AppStrings.Ui.FilterDateRadioButtonName, AppStrings.Ui.FilterDateOptionText);
        SetButtonContent(AppStrings.Ui.FilterApplyButtonName, AppStrings.Ui.FilterApplyButtonText);
        SetButtonContent(AppStrings.Ui.FilterClearButtonName, AppStrings.Ui.FilterClearButtonText);
        SetButtonContent(AppStrings.Ui.FilterCancelButtonName, AppStrings.Ui.FilterCancelButtonText);
    }

    private void InitializeEvents()
    {
        _noneRadioButton.IsCheckedChanged += HandleModeChanged;
        _sizeRadioButton.IsCheckedChanged += HandleModeChanged;
        _dateRadioButton.IsCheckedChanged += HandleModeChanged;
        _applyButton.Click += HandleApplyClick;
        _clearButton.Click += HandleClearClick;
        _cancelButton.Click += HandleCancelClick;
    }

    private void ApplyCurrentFilter(FilterDialogResult currentFilter)
    {
        switch (currentFilter.Mode)
        {
            case FilterMode.SizeRange:
                _sizeRadioButton.IsChecked = true;
                _minSizeTextBox.Text = currentFilter.Filter?.MinSizeMb?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
                _maxSizeTextBox.Text = currentFilter.Filter?.MaxSizeMb?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
                _statusTextBlock.Text = currentFilter.Summary;
                break;
            case FilterMode.DateRange:
                _dateRadioButton.IsChecked = true;
                _startDateTextBox.Text = currentFilter.Filter?.StartDate?.ToString(AppStrings.System.IsoDateFormat, CultureInfo.InvariantCulture) ?? string.Empty;
                _endDateTextBox.Text = currentFilter.Filter?.EndDate?.ToString(AppStrings.System.IsoDateFormat, CultureInfo.InvariantCulture) ?? string.Empty;
                _statusTextBlock.Text = currentFilter.Summary;
                break;
            default:
                _noneRadioButton.IsChecked = true;
                _statusTextBlock.Text = AppStrings.Ui.FilterStatusReady;
                break;
        }
    }

    private void HandleModeChanged(object? sender, RoutedEventArgs e)
    {
        UpdateInputState();
    }

    private void HandleApplyClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            Close(BuildResult());
        }
        catch (Exception exception)
        {
            _statusTextBlock.Text = string.Concat(AppStrings.Ui.ErrorPrefix, exception.Message);
        }
    }

    private void HandleClearClick(object? sender, RoutedEventArgs e)
    {
        Close(FilterDialogResult.CreateNone());
    }

    private void HandleCancelClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private FilterDialogResult BuildResult()
    {
        if (_noneRadioButton.IsChecked == true)
        {
            return FilterDialogResult.CreateNone();
        }

        if (_sizeRadioButton.IsChecked == true)
        {
            var minSize = ParseNullableDecimal(_minSizeTextBox.Text, AppStrings.Ui.FilterMinSizeLabel);
            var maxSize = ParseNullableDecimal(_maxSizeTextBox.Text, AppStrings.Ui.FilterMaxSizeLabel);

            if (!minSize.HasValue && !maxSize.HasValue)
            {
                throw new InvalidOperationException(AppStrings.Ui.FilterStatusMissingSizeValue);
            }

            if (minSize.HasValue && minSize.Value < 0)
            {
                throw new InvalidOperationException(AppStrings.Messages.NegativeMinSize);
            }

            if (maxSize.HasValue && maxSize.Value < 0)
            {
                throw new InvalidOperationException(AppStrings.Messages.NegativeMaxSize);
            }

            if (minSize.HasValue && maxSize.HasValue && minSize.Value > maxSize.Value)
            {
                throw new InvalidOperationException(AppStrings.Messages.InvalidSizeRange);
            }

            return FilterDialogResult.CreateSizeRange(minSize, maxSize);
        }

        var startDate = ParseNullableDate(_startDateTextBox.Text, AppStrings.Ui.FilterStartDateLabel);
        var endDate = ParseNullableDate(_endDateTextBox.Text, AppStrings.Ui.FilterEndDateLabel);

        if (!startDate.HasValue && !endDate.HasValue)
        {
            throw new InvalidOperationException(AppStrings.Ui.FilterStatusMissingDateValue);
        }

        if (startDate.HasValue && endDate.HasValue && startDate.Value.Date > endDate.Value.Date)
        {
            throw new InvalidOperationException(AppStrings.Messages.InvalidDateRange);
        }

        return FilterDialogResult.CreateDateRange(startDate, endDate);
    }

    private void UpdateInputState()
    {
        var sizeEnabled = _sizeRadioButton.IsChecked == true;
        var dateEnabled = _dateRadioButton.IsChecked == true;

        _minSizeTextBox.IsEnabled = sizeEnabled;
        _maxSizeTextBox.IsEnabled = sizeEnabled;
        _startDateTextBox.IsEnabled = dateEnabled;
        _endDateTextBox.IsEnabled = dateEnabled;
    }

    private static decimal? ParseNullableDecimal(string? rawValue, string fieldLabel)
    {
        if (string.IsNullOrWhiteSpace(rawValue))
        {
            return null;
        }

        if (decimal.TryParse(rawValue, NumberStyles.Number, CultureInfo.CurrentCulture, out var currentValue))
        {
            return currentValue;
        }

        if (decimal.TryParse(rawValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var invariantValue))
        {
            return invariantValue;
        }

        throw new FormatException(string.Concat(AppStrings.Ui.InvalidDecimalPrefix, fieldLabel));
    }

    private static DateTime? ParseNullableDate(string? rawValue, string fieldLabel)
    {
        if (string.IsNullOrWhiteSpace(rawValue))
        {
            return null;
        }

        if (DateTime.TryParseExact(
                rawValue,
                AppStrings.System.IsoDateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var exactValue))
        {
            return exactValue;
        }

        if (DateTime.TryParse(rawValue, CultureInfo.CurrentCulture, DateTimeStyles.None, out var currentValue))
        {
            return currentValue;
        }

        throw new FormatException(string.Concat(AppStrings.Ui.InvalidDatePrefix, fieldLabel));
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
        GetRequiredControl<ContentControl>(controlName).Content = value;
    }
}
