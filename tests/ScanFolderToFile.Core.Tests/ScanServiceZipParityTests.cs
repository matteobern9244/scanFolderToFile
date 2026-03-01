using FluentAssertions;
using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Models;
using ScanFolderToFile.Core.Services;

namespace ScanFolderToFile.Core.Tests;

public sealed class ScanServiceZipParityTests
{
    [Fact]
    public async Task ScanService_WhenCreateZipIsTrue_CreatesOnlyZipAndSkipsExport()
    {
        var collector = new RecordingFileCollector();
        var exportService = new RecordingExportService();
        var historyStore = new RecordingHistoryStore();
        var archiveService = new RecordingArchiveService();
        var duplicateDetector = new RecordingDuplicateDetector();
        var service = CreateService(collector, exportService, historyStore, archiveService, duplicateDetector);

        var result = await service.ExecuteAsync(CreateZipRequest());

        result.CollectedItems.Should().BeEmpty();
        result.GeneratedFilePath.Should().BeNull();
        result.GeneratedZipPath.Should().Be(archiveService.NextZipPath);
        exportService.CallCount.Should().Be(0);
    }

    [Fact]
    public async Task ScanService_WhenCreateZipIsTrue_WritesOnlyOneHistoryEntry()
    {
        var collector = new RecordingFileCollector();
        var exportService = new RecordingExportService();
        var historyStore = new RecordingHistoryStore();
        var archiveService = new RecordingArchiveService();
        var duplicateDetector = new RecordingDuplicateDetector();
        var service = CreateService(collector, exportService, historyStore, archiveService, duplicateDetector);

        _ = await service.ExecuteAsync(CreateZipRequest());

        historyStore.Entries.Should().ContainSingle();
        historyStore.Entries[0].FileName.Should().Be(Path.GetFileName(archiveService.NextZipPath));
    }

    [Fact]
    public async Task ScanService_WhenCreateZipIsTrue_SkipsDuplicateDetection()
    {
        var collector = new RecordingFileCollector();
        var exportService = new RecordingExportService();
        var historyStore = new RecordingHistoryStore();
        var archiveService = new RecordingArchiveService();
        var duplicateDetector = new RecordingDuplicateDetector();
        var service = CreateService(collector, exportService, historyStore, archiveService, duplicateDetector);

        var result = await service.ExecuteAsync(CreateZipRequest());

        duplicateDetector.CallCount.Should().Be(0);
        result.DuplicateGroups.Should().BeEmpty();
    }

    [Fact]
    public async Task ScanService_WhenCreateZipIsTrue_DoesNotInvokeCollector()
    {
        var collector = new RecordingFileCollector();
        var exportService = new RecordingExportService();
        var historyStore = new RecordingHistoryStore();
        var archiveService = new RecordingArchiveService();
        var duplicateDetector = new RecordingDuplicateDetector();
        var service = CreateService(collector, exportService, historyStore, archiveService, duplicateDetector);

        _ = await service.ExecuteAsync(CreateZipRequest());

        collector.CallCount.Should().Be(0);
    }

    private static IScanService CreateService(
        RecordingFileCollector collector,
        RecordingExportService exportService,
        RecordingHistoryStore historyStore,
        RecordingArchiveService archiveService,
        RecordingDuplicateDetector duplicateDetector)
    {
        return new ScanService(
            new RecordingAppPaths(),
            collector,
            new[] { exportService },
            historyStore,
            archiveService,
            duplicateDetector,
            new RecordingClock());
    }

    private static ScanRequest CreateZipRequest()
    {
        return new ScanRequest
        {
            SourceFolder = Path.GetTempPath(),
            OutputFolder = Path.GetTempPath(),
            OutputFormat = OutputFormat.Txt,
            CreateZip = true,
            CollectDuplicates = true,
        };
    }

    private sealed class RecordingAppPaths : IAppPaths
    {
        public string GetDefaultOutputFolder()
        {
            return Path.GetTempPath();
        }

        public string GetZipFolder(string outputFolder)
        {
            return outputFolder;
        }

        public string GetHistoryFilePath(string outputFolder)
        {
            return Path.Combine(outputFolder, "history.json");
        }
    }

    private sealed class RecordingFileCollector : IFileCollector
    {
        public int CallCount { get; private set; }

        public Task<IReadOnlyList<string>> CollectAsync(ScanRequest request, CancellationToken cancellationToken = default)
        {
            CallCount++;
            return Task.FromResult<IReadOnlyList<string>>(new[] { "alpha.txt" });
        }
    }

    private sealed class RecordingExportService : IExportService
    {
        public int CallCount { get; private set; }

        public OutputFormat SupportedFormat => OutputFormat.Txt;

        public Task<string> ExportAsync(IReadOnlyList<string> items, string outputFolder, CancellationToken cancellationToken = default)
        {
            CallCount++;
            return Task.FromResult(Path.Combine(outputFolder, "output.txt"));
        }
    }

    private sealed class RecordingHistoryStore : IHistoryStore
    {
        public List<HistoryEntry> Entries { get; } = new();

        public Task<IReadOnlyList<HistoryEntry>> ReadAsync(string historyFilePath, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyList<HistoryEntry>>(Entries);
        }

        public Task AppendAsync(string historyFilePath, HistoryEntry entry, CancellationToken cancellationToken = default)
        {
            Entries.Add(entry);
            return Task.CompletedTask;
        }
    }

    private sealed class RecordingArchiveService : IArchiveService
    {
        public string NextZipPath { get; } = Path.Combine(Path.GetTempPath(), "archive.zip");

        public int CallCount { get; private set; }

        public Task<string> CreateZipAsync(string sourceFolder, string zipFolder, CancellationToken cancellationToken = default)
        {
            CallCount++;
            return Task.FromResult(NextZipPath);
        }
    }

    private sealed class RecordingDuplicateDetector : IDuplicateDetector
    {
        public int CallCount { get; private set; }

        public IReadOnlyList<DuplicateFileGroup> FindDuplicates(IReadOnlyList<string> filePaths)
        {
            CallCount++;
            return Array.Empty<DuplicateFileGroup>();
        }
    }

    private sealed class RecordingClock : IClock
    {
        public DateTime Now => new(2026, 3, 1, 12, 0, 0, DateTimeKind.Local);
    }
}
