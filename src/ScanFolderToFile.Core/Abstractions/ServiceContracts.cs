using ScanFolderToFile.Core.Models;

namespace ScanFolderToFile.Core.Abstractions;

public interface IAppPaths
{
    string GetDefaultOutputFolder();

    string GetZipFolder(string outputFolder);

    string GetHistoryFilePath(string outputFolder);
}

public interface IClock
{
    DateTime Now { get; }
}

public interface IFileCollector
{
    Task<IReadOnlyList<string>> CollectAsync(ScanRequest request, CancellationToken cancellationToken = default);
}

public interface IExportService
{
    OutputFormat SupportedFormat { get; }

    Task<string> ExportAsync(IReadOnlyList<string> items, string outputFolder, CancellationToken cancellationToken = default);
}

public interface IHistoryStore
{
    Task<IReadOnlyList<HistoryEntry>> ReadAsync(string historyFilePath, CancellationToken cancellationToken = default);

    Task AppendAsync(string historyFilePath, HistoryEntry entry, CancellationToken cancellationToken = default);
}

public interface IArchiveService
{
    Task<string> CreateZipAsync(string sourceFolder, string zipFolder, CancellationToken cancellationToken = default);
}

public interface IFileOperationsService
{
    Task<FileOperationResult> CopyAsync(string sourceFolder, string destinationFolder, CancellationToken cancellationToken = default);

    Task<FileOperationResult> MoveAsync(string sourceFolder, string destinationFolder, CancellationToken cancellationToken = default);

    Task<FileOperationResult> ReorderByExtensionAsync(string sourceFolder, CancellationToken cancellationToken = default);
}

public interface IDuplicateDetector
{
    IReadOnlyList<DuplicateFileGroup> FindDuplicates(IReadOnlyList<string> filePaths);
}

public interface IScanService
{
    Task<ScanResult> ExecuteAsync(ScanRequest request, CancellationToken cancellationToken = default);
}

public interface IExternalLauncher
{
    Task OpenFileAsync(string path, CancellationToken cancellationToken = default);

    Task OpenFolderAsync(string path, CancellationToken cancellationToken = default);
}
