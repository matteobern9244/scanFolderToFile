using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Constants;

namespace ScanFolderToFile.Core.Services;

public sealed class MacAppPaths : IAppPaths
{
    private readonly string _documentsPath;

    public MacAppPaths(string? documentsPath = null)
    {
        _documentsPath = ResolveDocumentsPath(documentsPath);
    }

    public string GetDefaultOutputFolder()
    {
        return EnsureDirectory(Path.Combine(_documentsPath, AppStrings.Files.ContentDirectoryName));
    }

    public string GetZipFolder(string outputFolder)
    {
        return EnsureDirectory(Path.Combine(outputFolder, AppStrings.Files.ZipDirectoryName));
    }

    public string GetHistoryFilePath(string outputFolder)
    {
        var historyFolder = EnsureDirectory(Path.Combine(outputFolder, AppStrings.Files.HistoryDirectoryName));
        return Path.Combine(historyFolder, AppStrings.Files.HistoryFileName);
    }

    private static string EnsureDirectory(string path)
    {
        Directory.CreateDirectory(path);
        return path;
    }

    private static string ResolveDocumentsPath(string? documentsPath)
    {
        if (!string.IsNullOrWhiteSpace(documentsPath))
        {
            return documentsPath;
        }

        var defaultDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        if (!string.IsNullOrWhiteSpace(defaultDocumentsPath))
        {
            return defaultDocumentsPath;
        }

        return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    }
}
