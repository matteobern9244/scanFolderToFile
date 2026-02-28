using FluentAssertions;
using ScanFolderToFile.Core.Models;
using ScanFolderToFile.Core.Services;
using ScanFolderToFile.Core.Tests.TestSupport;

namespace ScanFolderToFile.Core.Tests;

public sealed class FileCollectorAndDuplicateTests
{
    [Fact]
    public async Task CollectAsync_ExcludesDesktopIni_AndReturnsOrderedPaths()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        var nestedDirectory = Directory.CreateDirectory(Path.Combine(sourceDirectory.Path, "nested"));
        var secondFile = Path.Combine(nestedDirectory.FullName, "b.txt");
        var firstFile = Path.Combine(sourceDirectory.Path, "a.txt");
        var ignoredFile = Path.Combine(sourceDirectory.Path, "desktop.ini");

        await File.WriteAllTextAsync(secondFile, "bb");
        await File.WriteAllTextAsync(firstFile, "aa");
        await File.WriteAllTextAsync(ignoredFile, "ignore");

        var collector = new FileCollector();
        var items = await collector.CollectAsync(new ScanRequest
        {
            SourceFolder = sourceDirectory.Path,
            OutputFolder = sourceDirectory.Path
        });

        items.Should().ContainInOrder(firstFile, secondFile);
        items.Should().NotContain(ignoredFile);
    }

    [Fact]
    public async Task CollectAsync_WhenOnlyExtensionsIsEnabled_ReturnsDistinctSortedExtensions()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        await File.WriteAllTextAsync(Path.Combine(sourceDirectory.Path, "alpha.TXT"), "aa");
        await File.WriteAllTextAsync(Path.Combine(sourceDirectory.Path, "beta.txt"), "bb");
        await File.WriteAllTextAsync(Path.Combine(sourceDirectory.Path, "gamma.md"), "cc");

        var collector = new FileCollector();
        var items = await collector.CollectAsync(new ScanRequest
        {
            SourceFolder = sourceDirectory.Path,
            OutputFolder = sourceDirectory.Path,
            OnlyExtensions = true
        });

        items.Should().ContainInOrder(".md", ".TXT");
    }

    [Fact]
    public async Task CollectAsync_AppliesSizeAndDateFilters()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        var smallFile = Path.Combine(sourceDirectory.Path, "small.txt");
        var largeFile = Path.Combine(sourceDirectory.Path, "large.txt");

        await File.WriteAllTextAsync(smallFile, "tiny");
        await File.WriteAllTextAsync(largeFile, new string('x', 4096));
        File.SetCreationTime(smallFile, DateTime.Today.AddDays(-10));
        File.SetCreationTime(largeFile, DateTime.Today);

        var collector = new FileCollector();
        var sizeFiltered = await collector.CollectAsync(new ScanRequest
        {
            SourceFolder = sourceDirectory.Path,
            OutputFolder = sourceDirectory.Path,
            Filter = new ScanFilter
            {
                MinSizeMb = 0.001m
            }
        });
        var dateFiltered = await collector.CollectAsync(new ScanRequest
        {
            SourceFolder = sourceDirectory.Path,
            OutputFolder = sourceDirectory.Path,
            Filter = new ScanFilter
            {
                StartDate = DateTime.Today.AddDays(-1)
            }
        });

        sizeFiltered.Should().ContainSingle().Which.Should().Be(largeFile);
        dateFiltered.Should().ContainSingle().Which.Should().Be(largeFile);
    }

    [Fact]
    public async Task CollectAsync_AppliesEndDateFilter()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        var filePath = Path.Combine(sourceDirectory.Path, "recent.txt");

        await File.WriteAllTextAsync(filePath, "recent");
        File.SetCreationTime(filePath, DateTime.Today);

        var collector = new FileCollector();
        var items = await collector.CollectAsync(new ScanRequest
        {
            SourceFolder = sourceDirectory.Path,
            OutputFolder = sourceDirectory.Path,
            Filter = new ScanFilter
            {
                EndDate = DateTime.Today.AddDays(-1)
            }
        });

        items.Should().BeEmpty();
    }

    [Fact]
    public void FindDuplicates_GroupsBaseNamesCaseInsensitively()
    {
        var detector = new DuplicateDetector();
        var duplicates = detector.FindDuplicates(
            new[]
            {
                "/tmp/Alpha.txt",
                "/tmp/nested/alpha.md",
                "/tmp/beta.txt"
            });

        duplicates.Should().ContainSingle();
        duplicates[0].BaseName.Should().Be("Alpha");
        duplicates[0].FilePaths.Should().HaveCount(2);
    }

    [Fact]
    public void FindDuplicates_WhenNoDuplicatesExist_ReturnsEmptyList()
    {
        var detector = new DuplicateDetector();
        var duplicates = detector.FindDuplicates(
            new[]
            {
                "/tmp/alpha.txt",
                "/tmp/beta.md"
            });

        duplicates.Should().BeEmpty();
    }
}
