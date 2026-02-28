using FluentAssertions;
using ScanFolderToFile.Core.Constants;
using ScanFolderToFile.Core.Models;
using ScanFolderToFile.Core.Services;
using ScanFolderToFile.Core.Tests.TestSupport;

namespace ScanFolderToFile.Core.Tests;

public sealed class AppPathsAndHistoryTests
{
    [Fact]
    public void GetDefaultOutputFolder_CreatesContentDirectoryInsideDocuments()
    {
        using var documentsDirectory = TestEnvironment.CreateTemporaryDirectory();
        var paths = new MacAppPaths(documentsDirectory.Path);

        var outputDirectory = paths.GetDefaultOutputFolder();

        outputDirectory.Should().Be(Path.Combine(documentsDirectory.Path, AppStrings.Files.ContentDirectoryName));
        Directory.Exists(outputDirectory).Should().BeTrue();
        Directory.Exists(paths.GetZipFolder(outputDirectory)).Should().BeTrue();
        File.Exists(paths.GetHistoryFilePath(outputDirectory)).Should().BeFalse();
    }

    [Fact]
    public async Task ReadAsync_WhenHistoryIsMissing_ReturnsEmptyList()
    {
        using var outputDirectory = TestEnvironment.CreateTemporaryDirectory();
        var paths = new MacAppPaths(outputDirectory.Path);
        var historyStore = new JsonHistoryStore();

        var entries = await historyStore.ReadAsync(paths.GetHistoryFilePath(paths.GetDefaultOutputFolder()));

        entries.Should().BeEmpty();
    }

    [Fact]
    public async Task AppendAsync_ThenReadAsync_ReturnsEntriesSortedByDateDescending()
    {
        using var outputDirectory = TestEnvironment.CreateTemporaryDirectory();
        var paths = new MacAppPaths(outputDirectory.Path);
        var historyStore = new JsonHistoryStore();
        var historyPath = paths.GetHistoryFilePath(paths.GetDefaultOutputFolder());
        var olderEntry = new HistoryEntry
        {
            FileName = "first.txt",
            Extension = "txt",
            CreatedAt = new DateTime(2024, 1, 1, 8, 0, 0, DateTimeKind.Local)
        };
        var newerEntry = new HistoryEntry
        {
            FileName = "second.pdf",
            Extension = "pdf",
            CreatedAt = new DateTime(2024, 1, 2, 8, 0, 0, DateTimeKind.Local)
        };

        await historyStore.AppendAsync(historyPath, olderEntry);
        await historyStore.AppendAsync(historyPath, newerEntry);
        var entries = await historyStore.ReadAsync(historyPath);

        entries.Select(entry => entry.FileName).Should().ContainInOrder("second.pdf", "first.txt");
    }

    [Fact]
    public async Task ReadAsync_WhenHistoryIsCorrupted_ThrowsInvalidDataException()
    {
        using var outputDirectory = TestEnvironment.CreateTemporaryDirectory();
        var paths = new MacAppPaths(outputDirectory.Path);
        var historyPath = paths.GetHistoryFilePath(paths.GetDefaultOutputFolder());
        await File.WriteAllTextAsync(historyPath, "{not-valid-json");
        var historyStore = new JsonHistoryStore();

        var action = async () => await historyStore.ReadAsync(historyPath);

        await action.Should().ThrowAsync<InvalidDataException>();
    }
}
