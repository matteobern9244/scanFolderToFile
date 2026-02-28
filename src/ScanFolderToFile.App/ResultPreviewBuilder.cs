using System.Globalization;
using System.Text;
using ScanFolderToFile.Core.Constants;
using ScanFolderToFile.Core.Models;

namespace ScanFolderToFile.App;

internal static class ResultPreviewBuilder
{
    public static string BuildSummary(ScanResult? result)
    {
        if (result is null)
        {
            return AppStrings.Ui.PreviewEmpty;
        }

        var builder = new StringBuilder();
        AppendLabelValueLine(builder, AppStrings.Ui.ResultItemsCountLabel, result.CollectedItems.Count.ToString(CultureInfo.InvariantCulture));
        AppendLabelValueLine(builder, AppStrings.Ui.ResultGeneratedFileLabel, result.GeneratedFilePath ?? AppStrings.Ui.ResultUnavailableValue);
        AppendLabelValueLine(builder, AppStrings.Ui.ResultGeneratedZipLabel, result.GeneratedZipPath ?? AppStrings.Ui.ResultUnavailableValue);
        AppendLabelValueLine(builder, AppStrings.Ui.ResultDuplicatesCountLabel, result.DuplicateGroups.Count.ToString(CultureInfo.InvariantCulture));
        AppendLabelValueLine(builder, AppStrings.Ui.ResultWarningsCountLabel, result.Warnings.Count.ToString(CultureInfo.InvariantCulture));
        return builder.ToString().TrimEnd();
    }

    public static string BuildPreview(ScanResult? result)
    {
        if (result is null || (result.CollectedItems.Count == 0 && result.DuplicateGroups.Count == 0 && result.Warnings.Count == 0))
        {
            return AppStrings.Ui.PreviewEmpty;
        }

        var builder = new StringBuilder();
        builder.AppendLine(AppStrings.Ui.PreviewItemsTitle);

        if (result.CollectedItems.Count == 0)
        {
            builder.AppendLine(AppStrings.Ui.ResultUnavailableValue);
        }
        else
        {
            foreach (var item in result.CollectedItems)
            {
                builder.AppendLine(item);
            }
        }

        if (result.DuplicateGroups.Count > 0)
        {
            builder.AppendLine();
            builder.AppendLine(AppStrings.Ui.PreviewDuplicatesTitle);

            foreach (var duplicateGroup in result.DuplicateGroups)
            {
                builder.Append(duplicateGroup.BaseName);
                builder.Append(AppStrings.System.NameValueSeparator);
                builder.AppendLine(string.Join(AppStrings.System.ListSeparator, duplicateGroup.FilePaths.Select(Path.GetFileName)));
            }
        }

        if (result.Warnings.Count > 0)
        {
            builder.AppendLine();
            builder.AppendLine(AppStrings.Ui.PreviewWarningsTitle);

            foreach (var warning in result.Warnings)
            {
                builder.AppendLine(warning);
            }
        }

        return builder.ToString().TrimEnd();
    }

    private static void AppendLabelValueLine(StringBuilder builder, string label, string value)
    {
        builder.Append(label);
        builder.Append(AppStrings.System.NameValueSeparator);
        builder.AppendLine(value);
    }
}
