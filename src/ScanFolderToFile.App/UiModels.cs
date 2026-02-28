using ScanFolderToFile.Core.Constants;
using ScanFolderToFile.Core.Models;

namespace ScanFolderToFile.App;

internal enum FilterMode
{
    None = 0,
    SizeRange = 1,
    DateRange = 2,
}

internal enum UtilityOperationMode
{
    Copy = 0,
    Move = 1,
    Reorder = 2,
}

internal sealed record FilterDialogResult(FilterMode Mode, ScanFilter? Filter, string Summary)
{
    public static FilterDialogResult CreateNone()
    {
        return new FilterDialogResult(FilterMode.None, null, AppStrings.Ui.NoFilterSummary);
    }

    public static FilterDialogResult CreateSizeRange(decimal? minSizeMb, decimal? maxSizeMb)
    {
        return new FilterDialogResult(
            FilterMode.SizeRange,
            new ScanFilter
            {
                MinSizeMb = minSizeMb,
                MaxSizeMb = maxSizeMb,
            },
            BuildSizeSummary(minSizeMb, maxSizeMb));
    }

    public static FilterDialogResult CreateDateRange(DateTime? startDate, DateTime? endDate)
    {
        return new FilterDialogResult(
            FilterMode.DateRange,
            new ScanFilter
            {
                StartDate = startDate,
                EndDate = endDate,
            },
            BuildDateSummary(startDate, endDate));
    }

    private static string BuildSizeSummary(decimal? minSizeMb, decimal? maxSizeMb)
    {
        return string.Concat(
            AppStrings.Ui.FilterSizeSummaryPrefix,
            AppStrings.System.NameValueSeparator,
            AppStrings.Ui.FilterFromLabel,
            AppStrings.System.Space,
            minSizeMb?.ToString() ?? AppStrings.System.DashPlaceholder,
            AppStrings.System.Space,
            AppStrings.Ui.FilterToLabel,
            AppStrings.System.Space,
            maxSizeMb?.ToString() ?? AppStrings.System.DashPlaceholder);
    }

    private static string BuildDateSummary(DateTime? startDate, DateTime? endDate)
    {
        return string.Concat(
            AppStrings.Ui.FilterDateSummaryPrefix,
            AppStrings.System.NameValueSeparator,
            AppStrings.Ui.FilterFromLabel,
            AppStrings.System.Space,
            startDate?.ToString(AppStrings.System.IsoDateFormat) ?? AppStrings.System.DashPlaceholder,
            AppStrings.System.Space,
            AppStrings.Ui.FilterToLabel,
            AppStrings.System.Space,
            endDate?.ToString(AppStrings.System.IsoDateFormat) ?? AppStrings.System.DashPlaceholder);
    }
}

internal sealed record HistoryListItem(
    string FileName,
    string Extension,
    DateTime CreatedAt,
    string? FullPath,
    bool Exists)
{
    public string CreatedAtDisplay => CreatedAt.ToString(AppStrings.Ui.HistoryDateFormat);

    public string ExistsLabel => Exists ? AppStrings.System.FileExistsLabel : AppStrings.System.FileMissingLabel;

    public string DisplayText =>
        string.Concat(
            CreatedAtDisplay,
            AppStrings.System.NameValueSeparator,
            FileName,
            AppStrings.System.OpenParenthesisWithLeadingSpace,
            ExistsLabel,
            AppStrings.System.CloseParenthesis);
}

internal sealed record DuplicateFileListItem(
    string BaseName,
    string FilePath)
{
    public string FileName => Path.GetFileName(FilePath);

    public string FolderPath => Path.GetDirectoryName(FilePath) ?? string.Empty;

    public bool Exists => File.Exists(FilePath);

    public string ExistsLabel => Exists ? AppStrings.System.FileExistsLabel : AppStrings.System.FileMissingLabel;

    public string DisplayText =>
        string.Concat(
            BaseName,
            AppStrings.System.NameValueSeparator,
            FileName,
            AppStrings.System.OpenParenthesisWithLeadingSpace,
            ExistsLabel,
            AppStrings.System.CloseParenthesis);
}

internal sealed class MainWindowUiState
{
    public ScanResult? LastResult { get; set; }

    public FilterDialogResult ActiveFilter { get; set; } = FilterDialogResult.CreateNone();
}
