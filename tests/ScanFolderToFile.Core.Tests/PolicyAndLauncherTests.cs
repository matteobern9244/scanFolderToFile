using FluentAssertions;
using ScanFolderToFile.Core.Constants;
using ScanFolderToFile.Core.Tests.TestSupport;

namespace ScanFolderToFile.Core.Tests;

public sealed class PolicyAndLauncherTests
{
    [Fact]
    public void AssetManifest_UsesUniqueKeys()
    {
        var distinctKeys = AppAssets.All.Select(asset => asset.Key).Distinct(StringComparer.Ordinal).Count();

        distinctKeys.Should().Be(AppAssets.All.Count);
    }

    [Fact]
    public void AssetManifest_PointsToExistingFiles()
    {
        var missingAssets = AppAssets.All
            .Select(asset => Path.Combine(TestEnvironment.RepositoryRoot, asset.RelativePath))
            .Where(path => !File.Exists(path))
            .ToArray();

        missingAssets.Should().BeEmpty();
    }

    [Fact]
    public void ProductionCode_KeepsStringLiteralsInsideDedicatedConstantFiles()
    {
        var allowedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "AppStrings.cs",
            "AppAssets.cs",
            "AppBuild.cs"
        };

        var sourceFiles = Directory
            .EnumerateFiles(Path.Combine(TestEnvironment.RepositoryRoot, "src"), "*.cs", SearchOption.AllDirectories)
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
            .Where(path => !allowedFiles.Contains(Path.GetFileName(path)))
            .ToArray();

        foreach (var sourceFile in sourceFiles)
        {
            File.ReadAllText(sourceFile).Should().NotContain("\"", sourceFile);
        }
    }

    [Fact]
    public void LauncherScript_ExistsAndStartsWithBashShebang()
    {
        var launcherPath = Path.Combine(TestEnvironment.RepositoryRoot, "start_scanfolder.command");

        File.Exists(launcherPath).Should().BeTrue();
        var firstLine = File.ReadLines(launcherPath).FirstOrDefault();
        firstLine.Should().Be("#!/usr/bin/env bash");
    }

    [Fact]
    public void Repository_UsesSingleRootCommandScript()
    {
        var commandFiles = Directory.GetFiles(TestEnvironment.RepositoryRoot, "*.command", SearchOption.TopDirectoryOnly);

        commandFiles.Should().ContainSingle();
        Path.GetFileName(commandFiles[0]).Should().Be("start_scanfolder.command");
    }

    [Fact]
    public void MacAppBundleTemplate_Exists()
    {
        var infoPlistPath = Path.Combine(
            TestEnvironment.RepositoryRoot,
            "src",
            "ScanFolderToFile.App",
            "Packaging",
            "Info.plist"
        );
        var bundleScriptPath = Path.Combine(TestEnvironment.RepositoryRoot, "scripts", "create_macos_bundle.sh");

        File.Exists(infoPlistPath).Should().BeTrue();
        File.Exists(bundleScriptPath).Should().BeTrue();
    }
}
