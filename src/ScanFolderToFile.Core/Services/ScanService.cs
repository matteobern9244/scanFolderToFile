using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Constants;
using ScanFolderToFile.Core.Exceptions;
using ScanFolderToFile.Core.Models;

namespace ScanFolderToFile.Core.Services;

public sealed class ScanService : IScanService
{
    private readonly IAppPaths _appPaths;
    private readonly IFileCollector _fileCollector;
    private readonly IReadOnlyDictionary<OutputFormat, IExportService> _exportServices;
    private readonly IHistoryStore _historyStore;
    private readonly IArchiveService _archiveService;
    private readonly IDuplicateDetector _duplicateDetector;
    private readonly IClock _clock;

    public ScanService(
        IAppPaths appPaths,
        IFileCollector fileCollector,
        IEnumerable<IExportService> exportServices,
        IHistoryStore historyStore,
        IArchiveService archiveService,
        IDuplicateDetector duplicateDetector,
        IClock clock)
    {
        _appPaths = appPaths;
        _fileCollector = fileCollector;
        _exportServices = exportServices.ToDictionary(service => service.SupportedFormat);
        _historyStore = historyStore;
        _archiveService = archiveService;
        _duplicateDetector = duplicateDetector;
        _clock = clock;
    }

    public async Task<ScanResult> ExecuteAsync(ScanRequest request, CancellationToken cancellationToken = default)
    {
        ScanRequestValidator.Validate(request);

        var historyFilePath = _appPaths.GetHistoryFilePath(request.OutputFolder);
        if (request.CreateZip)
        {
            var generatedZipPath = await _archiveService
                .CreateZipAsync(request.SourceFolder, _appPaths.GetZipFolder(request.OutputFolder), cancellationToken)
                .ConfigureAwait(false);
            await _historyStore.AppendAsync(historyFilePath, CreateHistoryEntry(generatedZipPath), cancellationToken).ConfigureAwait(false);

            return new ScanResult
            {
                CollectedItems = Array.Empty<string>(),
                GeneratedFilePath = null,
                GeneratedZipPath = generatedZipPath,
                DuplicateGroups = Array.Empty<DuplicateFileGroup>(),
                Warnings = Array.Empty<string>()
            };
        }

        var collectedItems = await _fileCollector.CollectAsync(request, cancellationToken).ConfigureAwait(false);
        var exporter = GetExporter(request.OutputFormat);
        var generatedFilePath = await exporter.ExportAsync(collectedItems, request.OutputFolder, cancellationToken).ConfigureAwait(false);
        await _historyStore.AppendAsync(historyFilePath, CreateHistoryEntry(generatedFilePath), cancellationToken).ConfigureAwait(false);

        IReadOnlyList<DuplicateFileGroup> duplicateGroups = Array.Empty<DuplicateFileGroup>();
        if (request.CollectDuplicates && !request.OnlyExtensions)
        {
            duplicateGroups = _duplicateDetector.FindDuplicates(collectedItems);
        }

        return new ScanResult
        {
            CollectedItems = collectedItems,
            GeneratedFilePath = generatedFilePath,
            GeneratedZipPath = null,
            DuplicateGroups = duplicateGroups,
            Warnings = Array.Empty<string>()
        };
    }

    private HistoryEntry CreateHistoryEntry(string generatedPath)
    {
        return new HistoryEntry
        {
            FileName = Path.GetFileName(generatedPath),
            Extension = Path.GetExtension(generatedPath).TrimStart('.'),
            CreatedAt = _clock.Now
        };
    }

    private IExportService GetExporter(OutputFormat outputFormat)
    {
        if (_exportServices.TryGetValue(outputFormat, out var exporter))
        {
            return exporter;
        }

        throw new OutputGenerationException(AppStrings.Messages.UnsupportedExportFormat);
    }
}
