using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Models;

namespace ScanFolderToFile.App;

internal static class HistoryPathResolver
{
    public static IReadOnlyList<HistoryListItem> Resolve(
        IReadOnlyList<HistoryEntry> entries,
        string outputFolder,
        IAppPaths appPaths)
    {
        ArgumentNullException.ThrowIfNull(entries);
        ArgumentNullException.ThrowIfNull(appPaths);

        return entries
            .Select(entry => CreateItem(entry, outputFolder, appPaths))
            .ToArray();
    }

    private static HistoryListItem CreateItem(HistoryEntry entry, string outputFolder, IAppPaths appPaths)
    {
        var fullPath = ResolvePath(entry, outputFolder, appPaths);
        return new HistoryListItem(
            entry.FileName,
            entry.Extension,
            entry.CreatedAt,
            fullPath,
            !string.IsNullOrWhiteSpace(fullPath) && File.Exists(fullPath));
    }

    private static string? ResolvePath(HistoryEntry entry, string outputFolder, IAppPaths appPaths)
    {
        if (string.IsNullOrWhiteSpace(entry.FileName))
        {
            return null;
        }

        var zipExtension = Path.GetExtension(ScanFolderToFile.Core.Constants.AppStrings.Files.OutputZipFileName).TrimStart('.');
        var folder = string.Equals(entry.Extension, zipExtension, StringComparison.OrdinalIgnoreCase)
            ? appPaths.GetZipFolder(outputFolder)
            : outputFolder;

        return Path.Combine(folder, entry.FileName);
    }
}
