using System.IO.Compression;
using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Constants;

namespace ScanFolderToFile.Core.Services;

public sealed class ZipArchiveService : IArchiveService
{
    public Task<string> CreateZipAsync(string sourceFolder, string zipFolder, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Directory.CreateDirectory(zipFolder);
        var outputPath = Path.Combine(zipFolder, AppStrings.Files.OutputZipFileName);

        if (File.Exists(outputPath))
        {
            File.Delete(outputPath);
        }

        ZipFile.CreateFromDirectory(sourceFolder, outputPath, CompressionLevel.SmallestSize, false);
        return Task.FromResult(outputPath);
    }
}
