using FluentAssertions;
using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Constants;
using ScanFolderToFile.Core.Exceptions;
using ScanFolderToFile.Core.Models;
using ScanFolderToFile.Core.Services;
using ScanFolderToFile.Core.Tests.TestSupport;

namespace ScanFolderToFile.Core.Tests;

public sealed class ExportAndScanServiceTests
{
    [Fact]
    public async Task TxtExportService_WritesOutputFile()
    {
        using var outputDirectory = TestEnvironment.CreateTemporaryDirectory();
        var service = new TxtExportService();

        var outputPath = await service.ExportAsync(
            new[]
            {
                "/tmp/example.txt",
                "/tmp/second.md"
            },
            outputDirectory.Path);

        File.Exists(outputPath).Should().BeTrue();
        var lines = await File.ReadAllLinesAsync(outputPath);
        lines.Should().ContainInOrder("example.txt", "second.md");
    }

    [Fact]
    public async Task MarkdownExportService_WritesTitleAndEntries()
    {
        using var outputDirectory = TestEnvironment.CreateTemporaryDirectory();
        var service = new MarkdownExportService();

        var outputPath = await service.ExportAsync(
            new[]
            {
                ".txt",
                ".md"
            },
            outputDirectory.Path);

        var lines = await File.ReadAllLinesAsync(outputPath);
        lines.Should().Contain(AppStrings.Files.OutputTitle);
        lines.Should().Contain(".txt");
        lines.Should().Contain(".md");
    }

    [Fact]
    public async Task PdfExportService_GeneratesNonEmptyPdfFile()
    {
        using var outputDirectory = TestEnvironment.CreateTemporaryDirectory();
        var service = new PdfExportService();

        var outputPath = await service.ExportAsync(
            new[]
            {
                "/tmp/example.txt"
            },
            outputDirectory.Path);

        var fileInfo = new FileInfo(outputPath);
        fileInfo.Exists.Should().BeTrue();
        fileInfo.Length.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ZipArchiveService_CreatesZipFile()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        using var outputDirectory = TestEnvironment.CreateTemporaryDirectory();
        await File.WriteAllTextAsync(Path.Combine(sourceDirectory.Path, "alpha.txt"), "zip-content");
        var appPaths = new MacAppPaths(outputDirectory.Path);
        var zipService = new ZipArchiveService();

        var zipPath = await zipService.CreateZipAsync(sourceDirectory.Path, appPaths.GetZipFolder(appPaths.GetDefaultOutputFolder()));

        File.Exists(zipPath).Should().BeTrue();
        new FileInfo(zipPath).Length.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ScanService_ExecutesEndToEnd_WithZipOnly()
    {
        using var documentsDirectory = TestEnvironment.CreateTemporaryDirectory();
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        Directory.CreateDirectory(Path.Combine(sourceDirectory.Path, "nested"));
        await File.WriteAllTextAsync(Path.Combine(sourceDirectory.Path, "alpha.txt"), "alpha");
        await File.WriteAllTextAsync(Path.Combine(sourceDirectory.Path, "nested", "alpha.md"), "alpha-two");
        var appPaths = new MacAppPaths(documentsDirectory.Path);
        var outputDirectory = appPaths.GetDefaultOutputFolder();
        var service = CreateScanService(appPaths, new FixedClock(new DateTime(2024, 2, 1, 9, 30, 0, DateTimeKind.Local)));

        var result = await service.ExecuteAsync(new ScanRequest
        {
            SourceFolder = sourceDirectory.Path,
            OutputFolder = outputDirectory,
            OutputFormat = OutputFormat.Txt,
            CreateZip = true,
            CollectDuplicates = true
        });

        result.CollectedItems.Should().BeEmpty();
        result.GeneratedFilePath.Should().BeNull();
        result.GeneratedZipPath.Should().NotBeNull();
        result.DuplicateGroups.Should().BeEmpty();
        File.Exists(result.GeneratedZipPath!).Should().BeTrue();

        var historyStore = new JsonHistoryStore();
        var historyEntries = await historyStore.ReadAsync(appPaths.GetHistoryFilePath(outputDirectory));
        historyEntries.Should().ContainSingle();
    }

    [Fact]
    public async Task ScanService_WhenExporterIsMissing_ThrowsOutputGenerationException()
    {
        using var documentsDirectory = TestEnvironment.CreateTemporaryDirectory();
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        await File.WriteAllTextAsync(Path.Combine(sourceDirectory.Path, "alpha.txt"), "alpha");
        var appPaths = new MacAppPaths(documentsDirectory.Path);
        var service = new ScanService(
            appPaths,
            new FileCollector(),
            new IExportService[]
            {
                new TxtExportService()
            },
            new JsonHistoryStore(),
            new ZipArchiveService(),
            new DuplicateDetector(),
            new FixedClock(new DateTime(2024, 2, 1, 9, 30, 0, DateTimeKind.Local)));

        var action = async () => await service.ExecuteAsync(new ScanRequest
        {
            SourceFolder = sourceDirectory.Path,
            OutputFolder = appPaths.GetDefaultOutputFolder(),
            OutputFormat = OutputFormat.Pdf
        });

        await action.Should().ThrowAsync<OutputGenerationException>();
    }

    private static IScanService CreateScanService(IAppPaths appPaths, IClock clock)
    {
        return new ScanService(
            appPaths,
            new FileCollector(),
            new IExportService[]
            {
                new TxtExportService(),
                new PdfExportService(),
                new MarkdownExportService()
            },
            new JsonHistoryStore(),
            new ZipArchiveService(),
            new DuplicateDetector(),
            clock);
    }
}
