using FluentAssertions;
using ScanFolderToFile.Core.Constants;
using ScanFolderToFile.Core.Services;
using ScanFolderToFile.Core.Tests.TestSupport;

namespace ScanFolderToFile.Core.Tests;

public sealed class FileOperationsServiceTests
{
    [Fact]
    public async Task CopyAsync_CopiesFilesAndPreservesRelativeStructure()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        using var destinationDirectory = TestEnvironment.CreateTemporaryDirectory();
        var nestedDirectory = Path.Combine(sourceDirectory.Path, "nested");
        Directory.CreateDirectory(nestedDirectory);
        var sourceFilePath = Path.Combine(nestedDirectory, "alpha.txt");
        await File.WriteAllTextAsync(sourceFilePath, "alpha");

        var service = new FileOperationsService();

        var result = await service.CopyAsync(sourceDirectory.Path, destinationDirectory.Path);

        result.AffectedPaths.Should().ContainSingle();
        var copiedPath = Path.Combine(destinationDirectory.Path, "nested", "alpha.txt");
        File.Exists(copiedPath).Should().BeTrue();
        File.Exists(sourceFilePath).Should().BeTrue();
    }

    [Fact]
    public async Task MoveAsync_MovesFilesAndDeletesEmptyDirectories()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        using var destinationDirectory = TestEnvironment.CreateTemporaryDirectory();
        var nestedDirectory = Path.Combine(sourceDirectory.Path, "nested");
        Directory.CreateDirectory(nestedDirectory);
        var sourceFilePath = Path.Combine(nestedDirectory, "alpha.txt");
        await File.WriteAllTextAsync(sourceFilePath, "alpha");

        var service = new FileOperationsService();

        var result = await service.MoveAsync(sourceDirectory.Path, destinationDirectory.Path);

        result.AffectedPaths.Should().ContainSingle();
        var movedPath = Path.Combine(destinationDirectory.Path, "nested", "alpha.txt");
        File.Exists(movedPath).Should().BeTrue();
        File.Exists(sourceFilePath).Should().BeFalse();
        Directory.Exists(nestedDirectory).Should().BeFalse();
    }

    [Fact]
    public async Task ReorderByExtensionAsync_CreatesExtensionFoldersAndMovesFiles()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        var textFilePath = Path.Combine(sourceDirectory.Path, "alpha.txt");
        var noExtensionFilePath = Path.Combine(sourceDirectory.Path, "beta");
        await File.WriteAllTextAsync(textFilePath, "alpha");
        await File.WriteAllTextAsync(noExtensionFilePath, "beta");

        var service = new FileOperationsService();

        var result = await service.ReorderByExtensionAsync(sourceDirectory.Path);

        result.AffectedPaths.Should().HaveCount(2);
        File.Exists(Path.Combine(sourceDirectory.Path, "txt", "alpha.txt")).Should().BeTrue();
        File.Exists(Path.Combine(sourceDirectory.Path, AppStrings.Files.NoExtensionDirectoryName, "beta")).Should().BeTrue();
        File.Exists(textFilePath).Should().BeFalse();
        File.Exists(noExtensionFilePath).Should().BeFalse();
    }

    [Fact]
    [System.Runtime.Versioning.UnsupportedOSPlatform("windows")]
    public async Task MoveAsync_SkipsInaccessibleSubfolder_AndLeavesDsStoreInSource()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        using var destinationDirectory = TestEnvironment.CreateTemporaryDirectory();
        var readableFile = Path.Combine(sourceDirectory.Path, "a.txt");
        var dsStore = Path.Combine(sourceDirectory.Path, ".DS_Store");
        var lockedDirectory = Directory.CreateDirectory(Path.Combine(sourceDirectory.Path, "locked"));
        await File.WriteAllTextAsync(readableFile, "aa");
        await File.WriteAllTextAsync(dsStore, "x");
        await File.WriteAllTextAsync(Path.Combine(lockedDirectory.FullName, "secret.txt"), "cc");
        using var locked = new LockedDirectory(lockedDirectory.FullName);

        var service = new FileOperationsService();

        var result = await service.MoveAsync(sourceDirectory.Path, destinationDirectory.Path);

        result.AffectedPaths.Should().ContainSingle();
        File.Exists(Path.Combine(destinationDirectory.Path, "a.txt")).Should().BeTrue();
        File.Exists(readableFile).Should().BeFalse();
        File.Exists(dsStore).Should().BeTrue();
        File.Exists(Path.Combine(destinationDirectory.Path, ".DS_Store")).Should().BeFalse();
        Directory.Exists(lockedDirectory.FullName).Should().BeTrue();
    }

    [Fact]
    [System.Runtime.Versioning.UnsupportedOSPlatform("windows")]
    public async Task CopyAsync_SkipsInaccessibleSubfolder()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        using var destinationDirectory = TestEnvironment.CreateTemporaryDirectory();
        var readableFile = Path.Combine(sourceDirectory.Path, "a.txt");
        var lockedDirectory = Directory.CreateDirectory(Path.Combine(sourceDirectory.Path, "locked"));
        await File.WriteAllTextAsync(readableFile, "aa");
        await File.WriteAllTextAsync(Path.Combine(lockedDirectory.FullName, "secret.txt"), "cc");
        using var locked = new LockedDirectory(lockedDirectory.FullName);

        var service = new FileOperationsService();

        var result = await service.CopyAsync(sourceDirectory.Path, destinationDirectory.Path);

        result.AffectedPaths.Should().ContainSingle();
        File.Exists(Path.Combine(destinationDirectory.Path, "a.txt")).Should().BeTrue();
        File.Exists(readableFile).Should().BeTrue();
    }
}
