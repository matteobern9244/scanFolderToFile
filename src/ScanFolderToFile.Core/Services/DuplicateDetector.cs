using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Models;

namespace ScanFolderToFile.Core.Services;

public sealed class DuplicateDetector : IDuplicateDetector
{
    public IReadOnlyList<DuplicateFileGroup> FindDuplicates(IReadOnlyList<string> filePaths)
    {
        return filePaths
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .GroupBy(
                path => Path.GetFileNameWithoutExtension(path),
                StringComparer.OrdinalIgnoreCase)
            .Where(group => group.Count() > 1)
            .OrderBy(group => group.Key, StringComparer.OrdinalIgnoreCase)
            .Select(group => new DuplicateFileGroup
            {
                BaseName = group.Key,
                FilePaths = group
                    .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
                    .ToArray()
            })
            .ToArray();
    }
}
