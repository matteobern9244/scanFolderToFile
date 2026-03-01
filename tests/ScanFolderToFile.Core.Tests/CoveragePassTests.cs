using FluentAssertions;
using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Constants;
using ScanFolderToFile.Core.Exceptions;
using ScanFolderToFile.Core.Models;
using ScanFolderToFile.Core.Services;
using ScanFolderToFile.Core.Tests.TestSupport;

namespace ScanFolderToFile.Core.Tests;

public sealed class CoveragePassTests
{
    [Fact]
    public void ExportContentFormatter_ToDisplayItems_StripsPathsAndPreservesBlanks()
    {
        var items = ExportContentFormatter.ToDisplayItems(
            new[]
            {
                Path.Combine("folder", "alpha.txt"),
                string.Empty,
                "beta.md"
            });

        items.Should().ContainInOrder("alpha.txt", string.Empty, "beta.md");
    }

    [Fact]
    public async Task ExportServices_ReportSupportedFormats_AndWrapIoFailures()
    {
        using var tempDirectory = TestEnvironment.CreateTemporaryDirectory();
        var invalidOutputPath = Path.Combine(tempDirectory.Path, "occupied.txt");
        await File.WriteAllTextAsync(invalidOutputPath, "occupied");

        new TxtExportService().SupportedFormat.Should().Be(OutputFormat.Txt);
        new MarkdownExportService().SupportedFormat.Should().Be(OutputFormat.Markdown);
        new PdfExportService().SupportedFormat.Should().Be(OutputFormat.Pdf);

        var txtAction = async () => await new TxtExportService().ExportAsync(new[] { "alpha" }, invalidOutputPath);
        var markdownAction = async () => await new MarkdownExportService().ExportAsync(new[] { "alpha" }, invalidOutputPath);

        await txtAction.Should().ThrowAsync<OutputGenerationException>();
        await markdownAction.Should().ThrowAsync<OutputGenerationException>();
    }

    [Fact]
    public async Task TxtExportService_WhenInputIsEmpty_CreatesAnEmptyFile()
    {
        using var outputDirectory = TestEnvironment.CreateTemporaryDirectory();
        var service = new TxtExportService();

        var outputPath = await service.ExportAsync(Array.Empty<string>(), outputDirectory.Path);

        File.Exists(outputPath).Should().BeTrue();
        (await File.ReadAllTextAsync(outputPath)).Should().BeEmpty();
    }

    [Fact]
    public async Task MarkdownExportService_WhenInputContainsPaths_StoresOnlyFileNames()
    {
        using var outputDirectory = TestEnvironment.CreateTemporaryDirectory();
        var service = new MarkdownExportService();

        var outputPath = await service.ExportAsync(
            new[]
            {
                Path.Combine("/tmp", "alpha.txt"),
                Path.Combine("/tmp", "beta.md")
            },
            outputDirectory.Path);

        var content = await File.ReadAllTextAsync(outputPath);
        content.Should().Contain("alpha.txt");
        content.Should().Contain("beta.md");
        content.Should().NotContain("/tmp/");
    }

    [Fact]
    public async Task PdfExportService_WhenCanceled_ThrowsOutputGenerationException()
    {
        using var outputDirectory = TestEnvironment.CreateTemporaryDirectory();
        var service = new PdfExportService();
        using var cancellationSource = new CancellationTokenSource();
        cancellationSource.Cancel();

        var action = async () => await service.ExportAsync(new[] { "alpha.txt" }, outputDirectory.Path, cancellationSource.Token);

        await action.Should().ThrowAsync<OutputGenerationException>();
    }

    [Fact]
    public async Task ZipArchiveService_WhenArchiveAlreadyExists_RecreatesIt()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        using var outputDirectory = TestEnvironment.CreateTemporaryDirectory();
        await File.WriteAllTextAsync(Path.Combine(sourceDirectory.Path, "alpha.txt"), "first");
        var zipFolder = Path.Combine(outputDirectory.Path, AppStrings.Files.ZipDirectoryName);
        Directory.CreateDirectory(zipFolder);
        var existingZipPath = Path.Combine(zipFolder, AppStrings.Files.OutputZipFileName);
        await File.WriteAllTextAsync(existingZipPath, "stale");
        var service = new ZipArchiveService();

        var recreatedPath = await service.CreateZipAsync(sourceDirectory.Path, zipFolder);

        recreatedPath.Should().Be(existingZipPath);
        new FileInfo(recreatedPath).Length.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ZipArchiveService_WhenCanceled_ThrowsOperationCanceledException()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        using var outputDirectory = TestEnvironment.CreateTemporaryDirectory();
        var service = new ZipArchiveService();
        using var cancellationSource = new CancellationTokenSource();
        cancellationSource.Cancel();

        var action = async () => await service.CreateZipAsync(sourceDirectory.Path, outputDirectory.Path, cancellationSource.Token);

        await action.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task FileOperationsService_CopyAsync_WhenDestinationMatchesSource_Throws()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        await File.WriteAllTextAsync(Path.Combine(sourceDirectory.Path, "alpha.txt"), "alpha");
        var service = new FileOperationsService();

        var action = async () => await service.CopyAsync(sourceDirectory.Path, sourceDirectory.Path);

        await action.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task FileOperationsService_CopyAsync_WhenTargetExists_AppendsNumericSuffix()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        using var destinationDirectory = TestEnvironment.CreateTemporaryDirectory();
        var sourceFilePath = Path.Combine(sourceDirectory.Path, "alpha.txt");
        var existingPath = Path.Combine(destinationDirectory.Path, "alpha.txt");
        await File.WriteAllTextAsync(sourceFilePath, "alpha");
        await File.WriteAllTextAsync(existingPath, "existing");
        var service = new FileOperationsService();

        var result = await service.CopyAsync(sourceDirectory.Path, destinationDirectory.Path);

        result.AffectedPaths.Should().ContainSingle();
        Path.GetFileName(result.AffectedPaths[0]).Should().Be("alpha(1).txt");
    }

    [Fact]
    public async Task FileOperationsService_ReorderByExtensionAsync_WhenFileAlreadyInTargetFolder_RenamesIt()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        var targetDirectory = Path.Combine(sourceDirectory.Path, "txt");
        Directory.CreateDirectory(targetDirectory);
        var sourceFilePath = Path.Combine(targetDirectory, "alpha.txt");
        await File.WriteAllTextAsync(sourceFilePath, "alpha");
        var service = new FileOperationsService();

        var result = await service.ReorderByExtensionAsync(sourceDirectory.Path);

        result.AffectedPaths.Should().ContainSingle();
        Path.GetFileName(result.AffectedPaths[0]).Should().Be("alpha(1).txt");
        result.SkippedPaths.Should().BeEmpty();
    }

    [Fact]
    public async Task FileOperationsService_ReorderByExtensionAsync_WhenSourceIsMissing_Throws()
    {
        var service = new FileOperationsService();

        var action = async () => await service.ReorderByExtensionAsync(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

        await action.Should().ThrowAsync<DirectoryNotFoundException>();
    }

    [Fact]
    public void SystemClock_Now_ReturnsCurrentLocalTime()
    {
        var before = DateTime.Now.AddSeconds(-1);
        var clock = new SystemClock();
        var after = DateTime.Now.AddSeconds(1);

        clock.Now.Should().BeOnOrAfter(before);
        clock.Now.Should().BeOnOrBefore(after);
    }

    [Fact]
    public async Task ScanService_WhenRequestIsInvalid_ThrowsInvalidScanRequestException()
    {
        using var documentsDirectory = TestEnvironment.CreateTemporaryDirectory();
        var service = new ScanService(
            new MacAppPaths(documentsDirectory.Path),
            new FileCollector(),
            new IExportService[]
            {
                new TxtExportService(),
            },
            new JsonHistoryStore(),
            new ZipArchiveService(),
            new DuplicateDetector(),
            new SystemClock());

        var action = async () => await service.ExecuteAsync(new Core.Models.ScanRequest());

        await action.Should().ThrowAsync<InvalidScanRequestException>();
    }
}
