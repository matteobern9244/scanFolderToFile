using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Constants;
using ScanFolderToFile.Core.Models;

namespace ScanFolderToFile.Core.Services;

public sealed class FileOperationsService : IFileOperationsService
{
    public Task<FileOperationResult> CopyAsync(string sourceFolder, string destinationFolder, CancellationToken cancellationToken = default)
    {
        ValidateSourceAndDestination(sourceFolder, destinationFolder);
        var filePaths = EnumerateSourceFiles(sourceFolder);
        var affectedPaths = new List<string>(filePaths.Count);

        foreach (var filePath in filePaths)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var relativePath = Path.GetRelativePath(sourceFolder, filePath);
            var destinationPath = EnsureUniquePath(Path.Combine(destinationFolder, relativePath));
            var destinationDirectory = Path.GetDirectoryName(destinationPath);
            if (!string.IsNullOrWhiteSpace(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            File.Copy(filePath, destinationPath, false);
            affectedPaths.Add(destinationPath);
        }

        return Task.FromResult(new FileOperationResult
        {
            AffectedPaths = affectedPaths,
        });
    }

    public Task<FileOperationResult> MoveAsync(string sourceFolder, string destinationFolder, CancellationToken cancellationToken = default)
    {
        ValidateSourceAndDestination(sourceFolder, destinationFolder);
        var filePaths = EnumerateSourceFiles(sourceFolder);
        var affectedPaths = new List<string>(filePaths.Count);

        foreach (var filePath in filePaths)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var relativePath = Path.GetRelativePath(sourceFolder, filePath);
            var destinationPath = EnsureUniquePath(Path.Combine(destinationFolder, relativePath));
            var destinationDirectory = Path.GetDirectoryName(destinationPath);
            if (!string.IsNullOrWhiteSpace(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            File.Move(filePath, destinationPath);
            affectedPaths.Add(destinationPath);
        }

        DeleteEmptyDirectories(sourceFolder);

        return Task.FromResult(new FileOperationResult
        {
            AffectedPaths = affectedPaths,
        });
    }

    public Task<FileOperationResult> ReorderByExtensionAsync(string sourceFolder, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sourceFolder))
        {
            throw new ArgumentException(AppStrings.Messages.MissingSourceFolder, nameof(sourceFolder));
        }

        if (!Directory.Exists(sourceFolder))
        {
            throw new DirectoryNotFoundException(AppStrings.Messages.SourceFolderNotFound);
        }

        var filePaths = EnumerateSourceFiles(sourceFolder);
        var affectedPaths = new List<string>(filePaths.Count);
        var skippedPaths = new List<string>();

        foreach (var filePath in filePaths)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var extensionName = GetExtensionFolderName(filePath);
            var targetDirectory = Path.Combine(sourceFolder, extensionName);
            Directory.CreateDirectory(targetDirectory);

            var targetPath = EnsureUniquePath(Path.Combine(targetDirectory, Path.GetFileName(filePath)));
            if (PathsMatch(filePath, targetPath))
            {
                skippedPaths.Add(filePath);
                continue;
            }

            File.Move(filePath, targetPath);
            affectedPaths.Add(targetPath);
        }

        DeleteEmptyDirectories(sourceFolder);

        return Task.FromResult(new FileOperationResult
        {
            AffectedPaths = affectedPaths,
            SkippedPaths = skippedPaths,
        });
    }

    private static void ValidateSourceAndDestination(string sourceFolder, string destinationFolder)
    {
        if (string.IsNullOrWhiteSpace(sourceFolder))
        {
            throw new ArgumentException(AppStrings.Messages.MissingSourceFolder, nameof(sourceFolder));
        }

        if (!Directory.Exists(sourceFolder))
        {
            throw new DirectoryNotFoundException(AppStrings.Messages.SourceFolderNotFound);
        }

        if (string.IsNullOrWhiteSpace(destinationFolder))
        {
            throw new ArgumentException(AppStrings.Messages.MissingDestinationFolder, nameof(destinationFolder));
        }

        if (PathsMatch(sourceFolder, destinationFolder))
        {
            throw new InvalidOperationException(AppStrings.Messages.DestinationFolderMatchesSource);
        }

        Directory.CreateDirectory(destinationFolder);
    }

    private static List<string> EnumerateSourceFiles(string sourceFolder)
    {
        return Directory
            .EnumerateFiles(sourceFolder, AppStrings.System.WildcardAllFiles, SearchOption.AllDirectories)
            .Where(path => !Path.GetFileName(path).Equals(AppStrings.Filters.DesktopIni, StringComparison.OrdinalIgnoreCase))
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static string EnsureUniquePath(string targetPath)
    {
        if (!File.Exists(targetPath))
        {
            return targetPath;
        }

        var directory = Path.GetDirectoryName(targetPath) ?? string.Empty;
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(targetPath);
        var extension = Path.GetExtension(targetPath);

        var suffix = 1;
        while (true)
        {
            var candidatePath = Path.Combine(
                directory,
                string.Concat(fileNameWithoutExtension, AppStrings.System.OpenParenthesisWithLeadingSpace.TrimStart(), suffix, AppStrings.System.CloseParenthesis, extension));

            if (!File.Exists(candidatePath))
            {
                return candidatePath;
            }

            suffix++;
        }
    }

    private static string GetExtensionFolderName(string filePath)
    {
        var extension = Path.GetExtension(filePath).TrimStart('.');
        return string.IsNullOrWhiteSpace(extension)
            ? AppStrings.Files.NoExtensionDirectoryName
            : extension;
    }

    private static void DeleteEmptyDirectories(string rootFolder)
    {
        foreach (var directoryPath in Directory.EnumerateDirectories(rootFolder, AppStrings.System.WildcardAllFiles, SearchOption.AllDirectories)
                     .OrderByDescending(path => path.Length))
        {
            if (!Directory.EnumerateFileSystemEntries(directoryPath).Any())
            {
                Directory.Delete(directoryPath, false);
            }
        }
    }

    private static bool PathsMatch(string firstPath, string secondPath)
    {
        var firstFullPath = Path.GetFullPath(firstPath)
            .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var secondFullPath = Path.GetFullPath(secondPath)
            .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        return string.Equals(firstFullPath, secondFullPath, StringComparison.OrdinalIgnoreCase);
    }
}
