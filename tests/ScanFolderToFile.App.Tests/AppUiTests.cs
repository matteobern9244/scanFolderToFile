using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using FluentAssertions;
using ScanFolderToFile.App;
using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Constants;
using ScanFolderToFile.Core.Models;

namespace ScanFolderToFile.App.Tests;

public sealed class AppUiTests : IDisposable
{
    private readonly string _tempRoot;

    public AppUiTests()
    {
        _tempRoot = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(_tempRoot);
    }

    [Fact]
    public void ScanRequestFactory_Create_UsesDefaultOutputFolderAndActiveFilter()
    {
        var appPaths = new FakeAppPaths(_tempRoot);
        var state = new MainWindowUiState
        {
            ActiveFilter = FilterDialogResult.CreateSizeRange(1, 5),
        };

        var request = ScanRequestFactory.Create(
            Path.Combine(_tempRoot, AppStrings.Files.ContentDirectoryName),
            string.Empty,
            OutputFormat.Markdown,
            true,
            true,
            false,
            state,
            appPaths);

        request.OutputFolder.Should().Be(appPaths.GetDefaultOutputFolder());
        request.Filter.Should().NotBeNull();
        request.Filter!.MinSizeMb.Should().Be(1);
        request.Filter.MaxSizeMb.Should().Be(5);
        request.OutputFormat.Should().Be(OutputFormat.Markdown);
        request.OnlyExtensions.Should().BeTrue();
        request.CreateZip.Should().BeTrue();
    }

    [Fact]
    public void ResultPreviewBuilder_BuildPreview_IncludesDuplicateSection()
    {
        var result = new ScanResult
        {
            CollectedItems = new[] { "uno.txt", "due.txt" },
            DuplicateGroups = new[]
            {
                new DuplicateFileGroup
                {
                    BaseName = "uno",
                    FilePaths = new[] { "/tmp/uno.txt", "/tmp/uno.md" },
                },
            },
        };

        var preview = ResultPreviewBuilder.BuildPreview(result);

        preview.Should().Contain(AppStrings.Ui.PreviewItemsTitle);
        preview.Should().Contain(AppStrings.Ui.PreviewDuplicatesTitle);
        preview.Should().Contain("uno");
    }

    [Fact]
    public void HistoryPathResolver_Resolve_MapsZipAndOutputFiles()
    {
        var appPaths = new FakeAppPaths(_tempRoot);
        var outputFolder = appPaths.GetDefaultOutputFolder();
        var zipFolder = appPaths.GetZipFolder(outputFolder);
        var textPath = Path.Combine(outputFolder, AppStrings.Files.OutputTextFileName);
        var zipPath = Path.Combine(zipFolder, AppStrings.Files.OutputZipFileName);
        File.WriteAllText(textPath, string.Empty);
        File.WriteAllText(zipPath, string.Empty);

        var items = HistoryPathResolver.Resolve(
            new[]
            {
                new HistoryEntry
                {
                    FileName = AppStrings.Files.OutputTextFileName,
                    Extension = Path.GetExtension(AppStrings.Files.OutputTextFileName).TrimStart('.'),
                    CreatedAt = DateTime.UtcNow,
                },
                new HistoryEntry
                {
                    FileName = AppStrings.Files.OutputZipFileName,
                    Extension = Path.GetExtension(AppStrings.Files.OutputZipFileName).TrimStart('.'),
                    CreatedAt = DateTime.UtcNow,
                },
            },
            outputFolder,
            appPaths);

        items.Should().HaveCount(2);
        items[0].FullPath.Should().Be(textPath);
        items[0].Exists.Should().BeTrue();
        items[1].FullPath.Should().Be(zipPath);
        items[1].Exists.Should().BeTrue();
    }

    [Fact]
    public void FilterDialogResult_CreateMethods_BuildExpectedSummaries()
    {
        var none = FilterDialogResult.CreateNone();
        var size = FilterDialogResult.CreateSizeRange(2, 8);
        var dates = FilterDialogResult.CreateDateRange(new DateTime(2026, 3, 1), new DateTime(2026, 3, 5));

        none.Summary.Should().Be(AppStrings.Ui.NoFilterSummary);
        size.Summary.Should().Contain(AppStrings.Ui.FilterSizeSummaryPrefix);
        dates.Summary.Should().Contain(AppStrings.Ui.FilterDateSummaryPrefix);
        dates.Summary.Should().Contain("2026-03-01");
    }

    [AvaloniaFact]
    public void MainWindow_WhenCreated_InitializesDefaultUiState()
    {
        var appPaths = new FakeAppPaths(_tempRoot);
        var window = CreateMainWindow(appPaths);

        window.CurrentStatus.Should().Be(AppStrings.Ui.ReadyStatus);
        window.CurrentFilterSummary.Should().Be(AppStrings.Ui.NoFilterSummary);
        window.FindControl<TextBox>(AppStrings.Ui.OutputFolderTextBoxName)!.Text.Should().Be(appPaths.GetDefaultOutputFolder());
        window.FindControl<ComboBox>(AppStrings.Ui.OutputFormatComboBoxName)!.SelectedItem.Should().NotBeNull();
    }

    [AvaloniaFact]
    public void MainWindow_ApplyFilterResult_UpdatesSummaryAndStatus()
    {
        var window = CreateMainWindow(new FakeAppPaths(_tempRoot));
        var filter = FilterDialogResult.CreateDateRange(new DateTime(2026, 3, 1), new DateTime(2026, 3, 4));

        window.ApplyFilterResult(filter);

        window.CurrentFilterSummary.Should().Be(filter.Summary);
        window.CurrentStatus.Should().Be(string.Concat(AppStrings.Ui.FilterAppliedStatusPrefix, filter.Summary));
    }

    [AvaloniaFact]
    public void MainWindow_ApplyScanResult_UpdatesStatusAndPreview()
    {
        var appPaths = new FakeAppPaths(_tempRoot);
        var outputFolder = appPaths.GetDefaultOutputFolder();
        var generatedPath = Path.Combine(outputFolder, AppStrings.Files.OutputTextFileName);
        File.WriteAllText(generatedPath, string.Empty);

        var window = CreateMainWindow(appPaths);
        var result = new ScanResult
        {
            CollectedItems = new[] { "uno.txt" },
            GeneratedFilePath = generatedPath,
        };

        window.ApplyScanResult(result);

        window.CurrentStatus.Should().Be(string.Concat(AppStrings.Ui.GenerateSuccessPrefix, generatedPath));
        window.FindControl<TextBox>(AppStrings.Ui.PreviewTextBoxName)!.Text.Should().Contain("uno.txt");
        window.FindControl<Button>(AppStrings.Ui.OpenGeneratedFileButtonName)!.IsEnabled.Should().BeTrue();
    }

    [AvaloniaFact]
    public void MainWindow_ApplyScanResult_WithDuplicates_EnablesDuplicateAction()
    {
        var window = CreateMainWindow(new FakeAppPaths(_tempRoot));
        var result = new ScanResult
        {
            DuplicateGroups = new[]
            {
                new DuplicateFileGroup
                {
                    BaseName = "uno",
                    FilePaths = new[] { "/tmp/uno.txt", "/tmp/uno.md" },
                },
            },
        };

        window.ApplyScanResult(result);

        window.CanOpenDuplicates.Should().BeTrue();
    }

    [AvaloniaFact]
    public void MainWindow_ApplyScanResult_WithGeneratedFile_EnablesPrintPreview()
    {
        var appPaths = new FakeAppPaths(_tempRoot);
        var outputFolder = appPaths.GetDefaultOutputFolder();
        var generatedPath = Path.Combine(outputFolder, AppStrings.Files.OutputTextFileName);
        File.WriteAllText(generatedPath, "contenuto");

        var window = CreateMainWindow(appPaths);
        window.ApplyScanResult(new ScanResult
        {
            GeneratedFilePath = generatedPath,
            CollectedItems = new[] { "contenuto" },
        });

        window.CanOpenPrintPreview.Should().BeTrue();
    }

    [AvaloniaFact]
    public void HistoryWindow_WithMissingFile_DisablesOpenButton()
    {
        var window = new HistoryWindow(
            new[]
            {
                new HistoryListItem("missing.txt", "txt", DateTime.UtcNow, Path.Combine(_tempRoot, "missing.txt"), false),
            },
            new FakeExternalLauncher(),
            null);

        window.SelectedItem.Should().NotBeNull();
        window.FindControl<Button>(AppStrings.Ui.HistoryOpenButtonName)!.IsEnabled.Should().BeFalse();
        window.FindControl<Button>(AppStrings.Ui.HistoryOpenFolderButtonName)!.IsEnabled.Should().BeTrue();
    }

    [AvaloniaFact]
    public void FilterWindow_WhenCreatedWithSizeFilter_PrepopulatesFields()
    {
        var window = new FilterWindow(FilterDialogResult.CreateSizeRange(3, 9));

        window.FindControl<TextBox>(AppStrings.Ui.FilterMinSizeTextBoxName)!.Text.Should().Be("3");
        window.FindControl<TextBox>(AppStrings.Ui.FilterMaxSizeTextBoxName)!.Text.Should().Be("9");
    }

    [AvaloniaFact]
    public void UtilitiesWindow_WhenCreatedWithReorderMode_DisablesDestination()
    {
        var window = new UtilitiesWindow(
            _tempRoot,
            Path.Combine(_tempRoot, "dest"),
            UtilityOperationMode.Reorder,
            new FakeFileOperationsService());

        window.DestinationEnabled.Should().BeFalse();
    }

    [AvaloniaFact]
    public void DuplicatesWindow_WithMissingFile_DisablesOpenFileButton()
    {
        var window = new DuplicatesWindow(
            new[]
            {
                new DuplicateFileGroup
                {
                    BaseName = "alpha",
                    FilePaths = new[] { Path.Combine(_tempRoot, "missing.txt") },
                },
            },
            new FakeExternalLauncher());

        window.SelectedItem.Should().NotBeNull();
        window.FindControl<Button>(AppStrings.Ui.DuplicatesOpenFileButtonName)!.IsEnabled.Should().BeFalse();
        window.FindControl<Button>(AppStrings.Ui.DuplicatesOpenFolderButtonName)!.IsEnabled.Should().BeTrue();
    }

    [AvaloniaFact]
    public void EditorWindow_WhenCreatedWithTextFile_LoadsFileContent()
    {
        var filePath = Path.Combine(_tempRoot, "note.txt");
        File.WriteAllText(filePath, "ciao editor");

        var window = new EditorWindow(filePath, string.Empty, new FakeExternalLauncher(), new FakePrintService());

        window.CurrentDocumentText.Should().Be("ciao editor");
        window.CanSaveDirectly.Should().BeTrue();
    }

    [AvaloniaFact]
    public void PrintPreviewWindow_WhenCreatedWithoutContent_DisablesPrint()
    {
        var window = new PrintPreviewWindow(string.Empty, null, new FakeExternalLauncher(), new FakePrintService());

        window.CurrentPreviewText.Should().Be(AppStrings.Ui.PrintPreviewNoContent);
        window.CanPrint.Should().BeFalse();
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempRoot))
        {
            Directory.Delete(_tempRoot, true);
        }
    }

    private MainWindow CreateMainWindow(IAppPaths appPaths)
    {
        return new MainWindow(
            new FakeScanService(),
            appPaths,
            new FakeHistoryStore(),
            new FakeExternalLauncher(),
            new FakeFileOperationsService(),
            new FakePrintService());
    }

    private sealed class FakeScanService : IScanService
    {
        public Task<ScanResult> ExecuteAsync(ScanRequest request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new ScanResult());
        }
    }

    private sealed class FakeAppPaths : IAppPaths
    {
        private readonly string _documentsRoot;

        public FakeAppPaths(string documentsRoot)
        {
            _documentsRoot = documentsRoot;
        }

        public string GetDefaultOutputFolder()
        {
            var path = Path.Combine(_documentsRoot, AppStrings.Files.ContentDirectoryName);
            Directory.CreateDirectory(path);
            return path;
        }

        public string GetZipFolder(string outputFolder)
        {
            var path = Path.Combine(outputFolder, AppStrings.Files.ZipDirectoryName);
            Directory.CreateDirectory(path);
            return path;
        }

        public string GetHistoryFilePath(string outputFolder)
        {
            var folder = Path.Combine(outputFolder, AppStrings.Files.HistoryDirectoryName);
            Directory.CreateDirectory(folder);
            return Path.Combine(folder, AppStrings.Files.HistoryFileName);
        }
    }

    private sealed class FakeHistoryStore : IHistoryStore
    {
        public Task<IReadOnlyList<HistoryEntry>> ReadAsync(string historyFilePath, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyList<HistoryEntry>>(Array.Empty<HistoryEntry>());
        }

        public Task AppendAsync(string historyFilePath, HistoryEntry entry, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class FakeFileOperationsService : IFileOperationsService
    {
        public Task<FileOperationResult> CopyAsync(string sourceFolder, string destinationFolder, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new FileOperationResult());
        }

        public Task<FileOperationResult> MoveAsync(string sourceFolder, string destinationFolder, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new FileOperationResult());
        }

        public Task<FileOperationResult> ReorderByExtensionAsync(string sourceFolder, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new FileOperationResult());
        }
    }

    private sealed class FakeExternalLauncher : IExternalLauncher
    {
        public Task OpenFileAsync(string path, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task OpenFolderAsync(string path, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class FakePrintService : IPrintService
    {
        public Task PrintFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task PrintTextAsync(string content, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
