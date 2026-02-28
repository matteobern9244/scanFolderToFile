using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Models;

namespace ScanFolderToFile.App;

internal static class ScanRequestFactory
{
    public static ScanRequest Create(
        string? sourceFolder,
        string? outputFolder,
        OutputFormat outputFormat,
        bool onlyExtensions,
        bool createZip,
        bool collectDuplicates,
        MainWindowUiState uiState,
        IAppPaths appPaths)
    {
        ArgumentNullException.ThrowIfNull(uiState);
        ArgumentNullException.ThrowIfNull(appPaths);

        var normalizedOutputFolder = string.IsNullOrWhiteSpace(outputFolder)
            ? appPaths.GetDefaultOutputFolder()
            : outputFolder.Trim();

        return new ScanRequest
        {
            SourceFolder = sourceFolder?.Trim() ?? string.Empty,
            OutputFolder = normalizedOutputFolder,
            OutputFormat = outputFormat,
            OnlyExtensions = onlyExtensions,
            CreateZip = createZip,
            CollectDuplicates = collectDuplicates,
            Filter = uiState.ActiveFilter.Filter,
        };
    }
}
