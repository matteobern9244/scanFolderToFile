using System.Reflection;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using FluentAssertions;
using ScanFolderToFile.App;
using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Constants;
using ScanFolderToFile.Core.Exceptions;
using ScanFolderToFile.Core.Models;

namespace ScanFolderToFile.App.Tests;

public sealed class MainWindowParityTests : IDisposable
{
    private readonly string _tempRoot;

    public MainWindowParityTests()
    {
        _tempRoot = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(_tempRoot);
    }

    [AvaloniaFact]
    public void MainWindow_ApplyScanResult_WithZipOnly_RunEnablesOpenGeneratedFileViaZipFallback()
    {
        var zipPath = Path.Combine(_tempRoot, AppStrings.Files.OutputZipFileName);
        File.WriteAllText(zipPath, "zip");
        var window = CreateWindow(new RecordingScanService());

        window.ApplyScanResult(new ScanResult
        {
            GeneratedZipPath = zipPath,
        });

        window.FindControl<Button>(AppStrings.Ui.OpenGeneratedFileButtonName)!.IsEnabled.Should().BeTrue();
        window.CurrentStatus.Should().Be(string.Concat(AppStrings.Ui.ZipGenerateSuccessPrefix, zipPath));
    }

    [AvaloniaFact]
    public async Task MainWindow_OpenGeneratedFileAsync_UsesZipWhenGeneratedFilePathIsNull()
    {
        var zipPath = Path.Combine(_tempRoot, AppStrings.Files.OutputZipFileName);
        File.WriteAllText(zipPath, "zip");
        var launcher = new RecordingExternalLauncher();
        var window = CreateWindow(new RecordingScanService(), launcher: launcher);
        window.ApplyScanResult(new ScanResult
        {
            GeneratedZipPath = zipPath,
        });

        await InvokeNonPublicTaskAsync(window, "OpenGeneratedFileAsync");

        launcher.OpenedFiles.Should().ContainSingle().Which.Should().Be(zipPath);
    }

    [AvaloniaFact]
    public async Task MainWindow_GenerateAsync_WithTxtResult_AutoOpensEditor()
    {
        var sourceFolder = Directory.CreateDirectory(Path.Combine(_tempRoot, "source")).FullName;
        var generatedFilePath = Path.Combine(_tempRoot, AppStrings.Files.OutputTextFileName);
        File.WriteAllText(generatedFilePath, "contenuto");
        var editorLauncher = new RecordingEditorWindowLauncher();
        var window = CreateWindow(
            new RecordingScanService
            {
                NextResult = new ScanResult
                {
                    GeneratedFilePath = generatedFilePath,
                    CollectedItems = new[] { "contenuto" },
                },
            },
            editorLauncher: editorLauncher);
        window.FindControl<TextBox>(AppStrings.Ui.SourceFolderTextBoxName)!.Text = sourceFolder;

        await InvokeNonPublicTaskAsync(window, "GenerateAsync");

        editorLauncher.CallCount.Should().Be(1);
        editorLauncher.LastInitialFilePath.Should().Be(generatedFilePath);
    }

    [AvaloniaFact]
    public async Task MainWindow_GenerateAsync_WithMarkdown_DoesNotAutoOpenEditor()
    {
        var sourceFolder = Directory.CreateDirectory(Path.Combine(_tempRoot, "source-md")).FullName;
        var generatedFilePath = Path.Combine(_tempRoot, AppStrings.Files.OutputMarkdownFileName);
        File.WriteAllText(generatedFilePath, "contenuto");
        var editorLauncher = new RecordingEditorWindowLauncher();
        var window = CreateWindow(
            new RecordingScanService
            {
                NextResult = new ScanResult
                {
                    GeneratedFilePath = generatedFilePath,
                    CollectedItems = new[] { "contenuto" },
                },
            },
            editorLauncher: editorLauncher);
        window.FindControl<TextBox>(AppStrings.Ui.SourceFolderTextBoxName)!.Text = sourceFolder;
        window.FindControl<ComboBox>(AppStrings.Ui.OutputFormatComboBoxName)!.SelectedIndex = 2;

        await InvokeNonPublicTaskAsync(window, "GenerateAsync");

        editorLauncher.CallCount.Should().Be(0);
    }

    [AvaloniaFact]
    public async Task MainWindow_GenerateAsync_WithZipOnly_DoesNotAutoOpenEditor()
    {
        var sourceFolder = Directory.CreateDirectory(Path.Combine(_tempRoot, "source-zip")).FullName;
        var zipPath = Path.Combine(_tempRoot, AppStrings.Files.OutputZipFileName);
        File.WriteAllText(zipPath, "zip");
        var editorLauncher = new RecordingEditorWindowLauncher();
        var window = CreateWindow(
            new RecordingScanService
            {
                NextResult = new ScanResult
                {
                    GeneratedZipPath = zipPath,
                },
            },
            editorLauncher: editorLauncher);
        window.FindControl<TextBox>(AppStrings.Ui.SourceFolderTextBoxName)!.Text = sourceFolder;
        window.FindControl<CheckBox>(AppStrings.Ui.CreateZipCheckBoxName)!.IsChecked = true;

        await InvokeNonPublicTaskAsync(window, "GenerateAsync");

        editorLauncher.CallCount.Should().Be(0);
    }

    [AvaloniaFact]
    public async Task MainWindow_GenerateAsync_WhenScanFails_DoesNotAutoOpenEditor()
    {
        var sourceFolder = Directory.CreateDirectory(Path.Combine(_tempRoot, "source-fail")).FullName;
        var editorLauncher = new RecordingEditorWindowLauncher();
        var window = CreateWindow(
            new RecordingScanService
            {
                ExceptionToThrow = new InvalidScanRequestException(AppStrings.Messages.MissingSourceFolder),
            },
            editorLauncher: editorLauncher);
        window.FindControl<TextBox>(AppStrings.Ui.SourceFolderTextBoxName)!.Text = sourceFolder;

        await InvokeNonPublicTaskAsync(window, "GenerateAsync");

        editorLauncher.CallCount.Should().Be(0);
        window.CurrentStatus.Should().Contain(AppStrings.Ui.ErrorPrefix);
    }

    [AvaloniaFact]
    public async Task MainWindow_OpenEditorCommand_InvokesRichTextEditorService()
    {
        var richTextEditorService = new RecordingRichTextEditorService();
        var window = CreateWindow(new RecordingScanService(), richTextEditorService: richTextEditorService);

        await InvokeNonPublicTaskAsync(window, "OpenEditorWindowAsync");

        richTextEditorService.CallCount.Should().Be(1);
    }

    [AvaloniaFact]
    public async Task MainWindow_OpenPrintPreviewCommand_InvokesPrintWorkflowService()
    {
        var printWorkflowService = new RecordingPrintWorkflowService();
        var window = CreateWindow(new RecordingScanService(), printWorkflowService: printWorkflowService);
        window.ApplyScanResult(new ScanResult
        {
            CollectedItems = new[] { "uno.txt" },
        });

        await InvokeNonPublicTaskAsync(window, "OpenPrintPreviewWindowAsync");

        printWorkflowService.PreviewCallCount.Should().Be(1);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempRoot))
        {
            Directory.Delete(_tempRoot, true);
        }
    }

    private MainWindow CreateWindow(
        IScanService scanService,
        RecordingExternalLauncher? launcher = null,
        RecordingEditorWindowLauncher? editorLauncher = null,
        RecordingRichTextEditorService? richTextEditorService = null,
        RecordingPrintWorkflowService? printWorkflowService = null)
    {
        var actualLauncher = launcher ?? new RecordingExternalLauncher();
        var actualPrintWorkflowService = printWorkflowService ?? new RecordingPrintWorkflowService();
        var actualRichTextEditorService = richTextEditorService ?? new RecordingRichTextEditorService();
        var actualEditorLauncher = editorLauncher ?? new RecordingEditorWindowLauncher();
        return new MainWindow(
            scanService,
            new RecordingAppPaths(_tempRoot),
            new RecordingHistoryStore(),
            actualLauncher,
            new RecordingFileOperationsService(),
            new RecordingPrintService(),
            actualRichTextEditorService,
            actualPrintWorkflowService,
            actualEditorLauncher);
    }

    private static async Task InvokeNonPublicTaskAsync(object target, string methodName, params object?[] args)
    {
        var method = target.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new MissingMethodException(target.GetType().FullName, methodName);
        if (method.Invoke(target, args) is Task task)
        {
            await task;
        }
    }

    private sealed class RecordingScanService : IScanService
    {
        public ScanResult NextResult { get; set; } = new();

        public Exception? ExceptionToThrow { get; set; }

        public Task<ScanResult> ExecuteAsync(ScanRequest request, CancellationToken cancellationToken = default)
        {
            if (ExceptionToThrow is not null)
            {
                throw ExceptionToThrow;
            }

            return Task.FromResult(NextResult);
        }
    }

    private sealed class RecordingAppPaths : IAppPaths
    {
        private readonly string _root;

        public RecordingAppPaths(string root)
        {
            _root = root;
        }

        public string GetDefaultOutputFolder()
        {
            var path = Path.Combine(_root, AppStrings.Files.ContentDirectoryName);
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

    private sealed class RecordingHistoryStore : IHistoryStore
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

    private sealed class RecordingExternalLauncher : IExternalLauncher
    {
        public List<string> OpenedFiles { get; } = new();

        public Task OpenFileAsync(string path, CancellationToken cancellationToken = default)
        {
            OpenedFiles.Add(path);
            return Task.CompletedTask;
        }

        public Task OpenFolderAsync(string path, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class RecordingFileOperationsService : IFileOperationsService
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

    private sealed class RecordingPrintService : IPrintService
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

    private sealed class RecordingEditorWindowLauncher : IEditorWindowLauncher
    {
        public int CallCount { get; private set; }

        public string? LastInitialFilePath { get; private set; }

        public Task ShowEditorAsync(
            Window owner,
            string? initialFilePath,
            string initialText,
            IExternalLauncher externalLauncher,
            IPrintService printService,
            CancellationToken cancellationToken = default)
        {
            CallCount++;
            LastInitialFilePath = initialFilePath;
            return Task.CompletedTask;
        }
    }

    private sealed class RecordingRichTextEditorService : IRichTextEditorService
    {
        public int CallCount { get; private set; }

        public Task ShowAsync(Window owner, string? initialFilePath, string initialText, CancellationToken cancellationToken = default)
        {
            CallCount++;
            return Task.CompletedTask;
        }
    }

    private sealed class RecordingPrintWorkflowService : IPrintWorkflowService
    {
        public int PreviewCallCount { get; private set; }

        public int PrintCallCount { get; private set; }

        public Task ShowPrintPreviewAsync(Window owner, string content, string? sourceFilePath, CancellationToken cancellationToken = default)
        {
            PreviewCallCount++;
            return Task.CompletedTask;
        }

        public Task PrintAsync(string content, string? sourceFilePath, CancellationToken cancellationToken = default)
        {
            PrintCallCount++;
            return Task.CompletedTask;
        }
    }
}
