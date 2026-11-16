using ScanFolderToFile.Core.Constants;

namespace ScanFolderToFile.Core.Services;

internal static class SourceFileEnumeration
{
    // AttributesToSkip = 0 is required: the default skips Hidden|System, and on Unix every dotfile
    // and dot-folder is Hidden, so they would silently disappear from scans.
    private static readonly EnumerationOptions RecursiveOptions = new()
    {
        RecurseSubdirectories = true,
        IgnoreInaccessible = true,
        AttributesToSkip = 0
    };

    public static IEnumerable<string> EnumerateFiles(string rootFolder)
    {
        return Directory
            .EnumerateFiles(rootFolder, AppStrings.System.WildcardAllFiles, RecursiveOptions)
            .Where(path => !AppStrings.Filters.ExcludedFileNames.Contains(Path.GetFileName(path), StringComparer.OrdinalIgnoreCase));
    }

    public static IEnumerable<string> EnumerateDirectories(string rootFolder)
    {
        return Directory.EnumerateDirectories(rootFolder, AppStrings.System.WildcardAllFiles, RecursiveOptions);
    }
}
