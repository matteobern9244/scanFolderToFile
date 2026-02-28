using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Constants;
using ScanFolderToFile.Core.Models;

namespace ScanFolderToFile.Core.Services;

public sealed class FileCollector : IFileCollector
{
    private const decimal BytesPerMegabyte = 1024m * 1024m;

    public Task<IReadOnlyList<string>> CollectAsync(ScanRequest request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var filePaths = Directory
            .EnumerateFiles(request.SourceFolder, AppStrings.System.WildcardAllFiles, SearchOption.AllDirectories)
            .Where(path => !Path.GetFileName(path).Equals(AppStrings.Filters.DesktopIni, StringComparison.OrdinalIgnoreCase))
            .Select(path => new FileInfo(path))
            .Where(fileInfo => MatchesFilter(fileInfo, request.Filter))
            .Select(fileInfo => fileInfo.FullName)
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (!request.OnlyExtensions)
        {
            return Task.FromResult<IReadOnlyList<string>>(filePaths);
        }

        var extensions = filePaths
            .Select(path => Path.GetExtension(path) ?? string.Empty)
            .Where(extension => !string.IsNullOrWhiteSpace(extension))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(extension => extension, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return Task.FromResult<IReadOnlyList<string>>(extensions);
    }

    private static bool MatchesFilter(FileInfo fileInfo, ScanFilter? filter)
    {
        if (filter is null)
        {
            return true;
        }

        if (filter.MinSizeMb.HasValue && fileInfo.Length < (long)(filter.MinSizeMb.Value * BytesPerMegabyte))
        {
            return false;
        }

        if (filter.MaxSizeMb.HasValue && fileInfo.Length > (long)(filter.MaxSizeMb.Value * BytesPerMegabyte))
        {
            return false;
        }

        if (filter.StartDate.HasValue && fileInfo.CreationTime.Date < filter.StartDate.Value.Date)
        {
            return false;
        }

        if (filter.EndDate.HasValue && fileInfo.CreationTime.Date > filter.EndDate.Value.Date)
        {
            return false;
        }

        return true;
    }
}
