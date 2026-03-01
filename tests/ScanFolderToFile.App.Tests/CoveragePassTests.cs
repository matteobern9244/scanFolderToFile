using System.Reflection;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using FluentAssertions;
using ScanFolderToFile.App;
using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Constants;
using ScanFolderToFile.Core.Exceptions;
using ScanFolderToFile.Core.Models;

namespace ScanFolderToFile.App.Tests;

public sealed class CoveragePassTests : IDisposable
{
    private readonly string _tempRoot;

    public CoveragePassTests()
    {
        _tempRoot = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(_tempRoot);
    }

    [AvaloniaFact]
    public void App_Initialize_LoadsApplicationStyles()
    {
        var app = new App();

        app.Initialize();

        app.Styles.Should().NotBeEmpty();
    }

    [AvaloniaFact]
    public void App_OnFrameworkInitializationCompleted_WithoutDesktopLifetime_DoesNotThrow()
    {
        var app = new App();
        app.Initialize();

        var action = () => app.OnFrameworkInitializationCompleted();

        action.Should().NotThrow();
    }

    [Fact]
    public void Program_BuildAvaloniaApp_ReturnsConfiguredBuilder()
    {
        var builder = Program.BuildAvaloniaApp();

        builder.Should().NotBeNull();
    }

    [Fact]
    public void AppServiceFactory_Create_ReturnsExpectedConcreteServices()
    {
        var services = AppServiceFactory.Create();

        services.ScanService.Should().NotBeNull();
        services.AppPaths.Should().BeOfType<ScanFolderToFile.Core.Services.MacAppPaths>();
        services.HistoryStore.Should().BeOfType<ScanFolderToFile.Core.Services.JsonHistoryStore>();
        services.ExternalLauncher.Should().BeOfType<MacExternalLauncher>();
        services.FileOperationsService.Should().BeOfType<ScanFolderToFile.Core.Services.FileOperationsService>();
        services.PrintService.Should().BeOfType<MacPrintService>();
        services.RichTextEditorService.Should().BeOfType<AppKitRichTextEditorService>();
        services.PrintWorkflowService.Should().BeOfType<AppKitPrintWorkflowService>();
        services.EditorWindowLauncher.Should().BeOfType<DefaultEditorWindowLauncher>();
    }

    [Fact]
    public void HistoryAndDuplicateModels_FormatDisplayProperties()
    {
        var existingFilePath = Path.Combine(_tempRoot, "alpha.txt");
        File.WriteAllText(existingFilePath, "alpha");
        var historyItem = new HistoryListItem("alpha.txt", "txt", new DateTime(2026, 3, 1, 9, 0, 0), existingFilePath, true);
        var duplicateItem = new DuplicateFileListItem("alpha", existingFilePath);

        historyItem.CreatedAtDisplay.Should().Contain("2026");
        historyItem.ExistsLabel.Should().Be(AppStrings.System.FileExistsLabel);
        historyItem.DisplayText.Should().Contain("alpha.txt");
        duplicateItem.FileName.Should().Be("alpha.txt");
        duplicateItem.FolderPath.Should().Be(_tempRoot);
        duplicateItem.Exists.Should().BeTrue();
        duplicateItem.DisplayText.Should().Contain("alpha");
    }

    [Fact]
    public void ResultPreviewBuilder_BuildSummaryAndPreview_HandleWarningsAndNull()
    {
        var result = new ScanResult
        {
            CollectedItems = new[] { "alpha.txt" },
            Warnings = new[] { "warning" },
        };

        ResultPreviewBuilder.BuildSummary(null).Should().Be(AppStrings.Ui.PreviewEmpty);
        ResultPreviewBuilder.BuildSummary(result).Should().Contain(AppStrings.Ui.ResultWarningsCountLabel);
        ResultPreviewBuilder.BuildPreview(result).Should().Contain(AppStrings.Ui.PreviewWarningsTitle);
    }

    [Fact]
    public async Task MacExternalLauncher_ValidatesInputAndSupportsInjectedCommand()
    {
        var scriptPath = await CreateCommandScriptAsync("open-stub.sh", "#!/usr/bin/env bash\nexit 0\n");
        var launcher = new MacExternalLauncher(scriptPath);

        await launcher.OpenFileAsync("/tmp/example.txt");
        await launcher.OpenFolderAsync("/tmp");

        var action = async () => await launcher.OpenFileAsync(" ");
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task MacPrintService_ValidatesInputAndSupportsInjectedCommand()
    {
        var scriptPath = await CreateCommandScriptAsync("lp-stub.sh", "#!/usr/bin/env bash\nexit 0\n");
        var service = new MacPrintService(scriptPath);
        var filePath = Path.Combine(_tempRoot, "print.txt");
        await File.WriteAllTextAsync(filePath, "print");

        await service.PrintFileAsync(filePath);
        await service.PrintTextAsync("buffer");

        using var cancellationSource = new CancellationTokenSource();
        cancellationSource.Cancel();
        var canceledFileAction = async () => await service.PrintFileAsync(filePath, cancellationSource.Token);
        var missingFileAction = async () => await service.PrintFileAsync(Path.Combine(_tempRoot, "missing.txt"));
        var emptyContentAction = async () => await service.PrintTextAsync(string.Empty);

        await canceledFileAction.Should().ThrowAsync<OperationCanceledException>();
        await missingFileAction.Should().ThrowAsync<FileNotFoundException>();
        await emptyContentAction.Should().ThrowAsync<InvalidOperationException>();
    }

    [AvaloniaFact]
    public void FilterWindow_BuildResult_CoversValidAndInvalidStates()
    {
        var sizeWindow = new FilterWindow(FilterDialogResult.CreateNone());
        sizeWindow.FindControl<RadioButton>(AppStrings.Ui.FilterSizeRadioButtonName)!.IsChecked = true;
        sizeWindow.FindControl<TextBox>(AppStrings.Ui.FilterMinSizeTextBoxName)!.Text = "2";
        sizeWindow.FindControl<TextBox>(AppStrings.Ui.FilterMaxSizeTextBoxName)!.Text = "4";

        var sizeResult = InvokeNonPublic<FilterDialogResult>(sizeWindow, "BuildResult");

        sizeResult.Mode.Should().Be(FilterMode.SizeRange);
        sizeResult.Filter!.MinSizeMb.Should().Be(2);
        sizeResult.Filter.MaxSizeMb.Should().Be(4);

        var dateWindow = new FilterWindow(FilterDialogResult.CreateNone());
        dateWindow.FindControl<RadioButton>(AppStrings.Ui.FilterDateRadioButtonName)!.IsChecked = true;
        dateWindow.FindControl<TextBox>(AppStrings.Ui.FilterStartDateTextBoxName)!.Text = "2026-03-05";
        dateWindow.FindControl<TextBox>(AppStrings.Ui.FilterEndDateTextBoxName)!.Text = "2026-03-01";

        var invalidAction = () => InvokeNonPublic<FilterDialogResult>(dateWindow, "BuildResult");
        invalidAction.Should().Throw<TargetInvocationException>()
            .Where(exception => exception.InnerException is InvalidOperationException);
    }

    [AvaloniaFact]
    public async Task HistoryWindow_UsesLauncherAndReloadDelegate()
    {
        var existingFilePath = Path.Combine(_tempRoot, "history.txt");
        await File.WriteAllTextAsync(existingFilePath, "history");
        var launcher = new RecordingExternalLauncher();
        var reloadTriggered = false;
        var window = new HistoryWindow(
            new[]
            {
                new HistoryListItem("history.txt", "txt", DateTime.UtcNow, existingFilePath, true),
            },
            launcher,
            () =>
            {
                reloadTriggered = true;
                return Task.FromResult<IReadOnlyList<HistoryListItem>>(Array.Empty<HistoryListItem>());
            });

        InvokeNonPublicVoid(window, "HandleOpenClick", null, null!);
        InvokeNonPublicVoid(window, "HandleOpenFolderClick", null, null!);
        InvokeNonPublicVoid(window, "HandleRefreshClick", null, null!);
        await Task.Delay(10);

        launcher.OpenedFiles.Should().ContainSingle().Which.Should().Be(existingFilePath);
        launcher.OpenedFolders.Should().ContainSingle().Which.Should().Be(_tempRoot);
        reloadTriggered.Should().BeTrue();
        window.FindControl<Button>(AppStrings.Ui.HistoryOpenButtonName)!.IsEnabled.Should().BeFalse();
    }

    [AvaloniaFact]
    public void DuplicatesWindow_UsesLauncherForExistingEntries()
    {
        var existingFilePath = Path.Combine(_tempRoot, "duplicate.txt");
        File.WriteAllText(existingFilePath, "duplicate");
        var launcher = new RecordingExternalLauncher();
        var window = new DuplicatesWindow(
            new[]
            {
                new DuplicateFileGroup
                {
                    BaseName = "duplicate",
                    FilePaths = new[] { existingFilePath },
                },
            },
            launcher);

        InvokeNonPublicVoid(window, "HandleOpenFileClick", null, null!);
        InvokeNonPublicVoid(window, "HandleOpenFolderClick", null, null!);

        launcher.OpenedFiles.Should().ContainSingle().Which.Should().Be(existingFilePath);
        launcher.OpenedFolders.Should().ContainSingle().Which.Should().Be(_tempRoot);
    }

    [AvaloniaFact]
    public async Task PrintPreviewWindow_PrintsBufferAndSourceFile()
    {
        var filePath = Path.Combine(_tempRoot, "preview.txt");
        await File.WriteAllTextAsync(filePath, "preview");
        var launcher = new RecordingExternalLauncher();
        var printService = new RecordingPrintService();
        var fileWindow = new PrintPreviewWindow("preview", filePath, launcher, printService);

        InvokeNonPublicVoid(fileWindow, "HandleOpenFileClick", null, null!);
        InvokeNonPublicVoid(fileWindow, "HandlePrintClick", null, null!);
        await Task.Delay(10);

        printService.PrintedFiles.Should().ContainSingle().Which.Should().Be(filePath);
        launcher.OpenedFiles.Should().ContainSingle().Which.Should().Be(filePath);

        var bufferWindow = new PrintPreviewWindow("buffer only", null, launcher, printService);
        InvokeNonPublicVoid(bufferWindow, "HandlePrintClick", null, null!);
        await Task.Delay(10);
        printService.PrintedTexts.Should().Contain("buffer only");
    }

    [AvaloniaFact]
    public async Task EditorWindow_CoversUnsupportedSourceAndSaveFlow()
    {
        var unsupportedFilePath = Path.Combine(_tempRoot, "preview.pdf");
        await File.WriteAllTextAsync(unsupportedFilePath, "not a pdf");
        var window = new EditorWindow(unsupportedFilePath, "preview text", new RecordingExternalLauncher(), new RecordingPrintService());

        window.CurrentDocumentText.Should().Be("preview text");
        window.CanSaveDirectly.Should().BeFalse();

        var editorTextBox = window.FindControl<TextBox>(AppStrings.Ui.EditorTextBoxName)!;
        editorTextBox.Text = "saved text";
        var savePath = Path.Combine(_tempRoot, "saved.txt");

        await InvokeNonPublicTaskAsync(window, "SaveToPathAsync", savePath);

        (await File.ReadAllTextAsync(savePath)).Should().Be("saved text");
        window.CanSaveDirectly.Should().BeTrue();
        var unsupportedAction = () => InvokeNonPublicVoid(window, "LoadFromFile", unsupportedFilePath);
        unsupportedAction.Should().NotThrow();
    }

    [AvaloniaFact]
    public async Task UtilitiesWindow_ValidatesInputsAndExecutesOperations()
    {
        var operations = new RecordingFileOperationsService();
        var sourceDirectory = Path.Combine(_tempRoot, "source");
        var destinationDirectory = Path.Combine(_tempRoot, "dest");
        Directory.CreateDirectory(sourceDirectory);
        Directory.CreateDirectory(destinationDirectory);
        var window = new UtilitiesWindow(string.Empty, string.Empty, UtilityOperationMode.Copy, operations);

        InvokeNonPublicVoid(window, "HandleExecuteClick", null, null!);
        window.FindControl<TextBlock>(AppStrings.Ui.UtilityStatusTextBlockName)!.Text.Should().Be(AppStrings.Ui.UtilityNoSourceStatus);

        window.FindControl<TextBox>(AppStrings.Ui.UtilitySourceTextBoxName)!.Text = sourceDirectory;
        InvokeNonPublicVoid(window, "HandleExecuteClick", null, null!);
        window.FindControl<TextBlock>(AppStrings.Ui.UtilityStatusTextBlockName)!.Text.Should().Be(AppStrings.Ui.UtilityNoDestinationStatus);

        window.FindControl<TextBox>(AppStrings.Ui.UtilityDestinationTextBoxName)!.Text = destinationDirectory;
        InvokeNonPublicVoid(window, "HandleExecuteClick", null, null!);
        await Task.Delay(10);
        operations.CopyCalls.Should().ContainSingle();

        window.FindControl<RadioButton>(AppStrings.Ui.UtilityMoveRadioButtonName)!.IsChecked = true;
        InvokeNonPublicVoid(window, "HandleExecuteClick", null, null!);
        await Task.Delay(10);
        operations.MoveCalls.Should().ContainSingle();

        window.FindControl<RadioButton>(AppStrings.Ui.UtilityReorderRadioButtonName)!.IsChecked = true;
        InvokeNonPublicVoid(window, "HandleExecuteClick", null, null!);
        await Task.Delay(10);
        operations.ReorderCalls.Should().ContainSingle();
        window.DestinationEnabled.Should().BeFalse();
    }

    [AvaloniaFact]
    public async Task MainWindow_CoversGenerateOpenAndHelperFlows()
    {
        var appPaths = new RecordingAppPaths(_tempRoot);
        var sourceDirectory = Path.Combine(_tempRoot, "source");
        Directory.CreateDirectory(sourceDirectory);
        var generatedFilePath = Path.Combine(appPaths.GetDefaultOutputFolder(), AppStrings.Files.OutputTextFileName);
        await File.WriteAllTextAsync(generatedFilePath, "generated");
        var scanService = new RecordingScanService
        {
            NextResult = new ScanResult
            {
                CollectedItems = new[] { "generated" },
                GeneratedFilePath = generatedFilePath,
            },
        };
        var historyStore = new RecordingHistoryStore
        {
            Entries = new[]
            {
                new HistoryEntry
                {
                    FileName = AppStrings.Files.OutputTextFileName,
                    Extension = "txt",
                    CreatedAt = DateTime.UtcNow,
                },
            },
        };
        var launcher = new RecordingExternalLauncher();
        var printService = new RecordingPrintService();
        var window = CreateMainWindow(scanService, appPaths, historyStore, launcher, new RecordingFileOperationsService(), printService);

        window.FindControl<TextBox>(AppStrings.Ui.SourceFolderTextBoxName)!.Text = sourceDirectory;
        await InvokeNonPublicTaskAsync(window, "GenerateAsync");
        scanService.LastRequest.Should().NotBeNull();
        window.CurrentStatus.Should().Be(string.Concat(AppStrings.Ui.GenerateSuccessPrefix, generatedFilePath));

        await InvokeNonPublicTaskAsync(window, "OpenGeneratedFileAsync");
        await InvokeNonPublicTaskAsync(window, "OpenOutputFolderAsync");
        await InvokeNonPublicTaskAsync(window, "OpenDuplicatesWindowAsync");
        window.CurrentStatus.Should().Be(AppStrings.Ui.NoDuplicatesStatus);
        await InvokeNonPublicTaskAsync(window, "SelectFolderAsync", window.FindControl<TextBox>(AppStrings.Ui.SourceFolderTextBoxName)!, AppStrings.Ui.BrowseSourceDialogTitle);
        var historyItems = await InvokeNonPublicTaskAsync<IReadOnlyList<HistoryListItem>>(window, "LoadHistoryItemsAsync");
        InvokeNonPublicVoid(window, "ClearFilter");
        window.CurrentStatus.Should().Be(AppStrings.Ui.FilterClearedStatus);

        var printWindow = CreateMainWindow(
            new RecordingScanService(),
            appPaths,
            historyStore,
            launcher,
            new RecordingFileOperationsService(),
            printService);
        await InvokeNonPublicTaskAsync(printWindow, "OpenPrintPreviewWindowAsync");

        launcher.OpenedFiles.Should().ContainSingle().Which.Should().Be(generatedFilePath);
        launcher.OpenedFolders.Should().ContainSingle().Which.Should().Be(appPaths.GetDefaultOutputFolder());
        historyItems.Should().ContainSingle();
        window.CurrentFilterSummary.Should().Be(AppStrings.Ui.NoFilterSummary);
        printWindow.CurrentStatus.Should().Be(AppStrings.Ui.NoPrintPreviewStatus);
    }

    [AvaloniaFact]
    public async Task MainWindow_WhenServicesFail_ShowsErrorStatuses()
    {
        var appPaths = new RecordingAppPaths(_tempRoot);
        var scanService = new RecordingScanService
        {
            ExceptionToThrow = new InvalidScanRequestException(AppStrings.Messages.MissingSourceFolder),
        };
        var historyStore = new RecordingHistoryStore
        {
            ExceptionToThrow = new InvalidOperationException("history failed"),
        };
        var window = CreateMainWindow(
            scanService,
            appPaths,
            historyStore,
            new RecordingExternalLauncher(),
            new RecordingFileOperationsService(),
            new RecordingPrintService());

        await InvokeNonPublicTaskAsync(window, "GenerateAsync");
        window.CurrentStatus.Should().Contain(AppStrings.Ui.ErrorPrefix);

        await InvokeNonPublicTaskAsync(window, "OpenHistoryWindowAsync");
        window.CurrentStatus.Should().Contain("history failed");

        await InvokeNonPublicTaskAsync(window, "OpenGeneratedFileAsync");
        window.CurrentStatus.Should().Be(AppStrings.Ui.MissingGeneratedFileStatus);
    }

    [AvaloniaFact]
    public async Task AdditionalWindowBranches_AreCoveredBySafeHandlerPaths()
    {
        var appPaths = new RecordingAppPaths(_tempRoot);
        var scanService = new RecordingScanService();
        var historyStore = new RecordingHistoryStore();
        var launcher = new RecordingExternalLauncher();
        var fileOperations = new RecordingFileOperationsService();

        _ = new DuplicatesWindow();
        _ = new FilterWindow();
        _ = new HistoryWindow();
        _ = new UtilitiesWindow();
        _ = new EditorWindow();
        _ = new PrintPreviewWindow();
        _ = new MainWindow(scanService, appPaths, historyStore, launcher);
        _ = new MainWindow(scanService, appPaths, historyStore, launcher, fileOperations);

        var editorWindow = new EditorWindow();
        InvokeNonPublicVoid(editorWindow, "HandleOpenClick", null, null!);
        InvokeNonPublicVoid(editorWindow, "HandleSaveAsClick", null, null!);
        InvokeNonPublicVoid(editorWindow, "HandlePrintPreviewClick", null, null!);
        InvokeNonPublicVoid(editorWindow, "HandleCloseClick", null, null!);
        editorWindow.FindControl<TextBlock>(AppStrings.Ui.EditorStatusTextBlockName)!.Text.Should().NotBeNullOrWhiteSpace();

        var filterWindow = new FilterWindow();
        filterWindow.FindControl<RadioButton>(AppStrings.Ui.FilterSizeRadioButtonName)!.IsChecked = true;
        InvokeNonPublicVoid(filterWindow, "HandleApplyClick", null, null!);
        filterWindow.FindControl<TextBlock>(AppStrings.Ui.FilterStatusTextBlockName)!.Text.Should().Contain(AppStrings.Ui.ErrorPrefix);
        var clearedFilterWindow = new FilterWindow();
        InvokeNonPublicVoid(clearedFilterWindow, "HandleClearClick", null, null!);

        var missingHistoryWindow = new HistoryWindow(
            new[]
            {
                new HistoryListItem("missing.txt", "txt", DateTime.UtcNow, Path.Combine(_tempRoot, "missing.txt"), false),
            },
            launcher,
            null);
        InvokeNonPublicVoid(missingHistoryWindow, "HandleOpenClick", null, null!);
        InvokeNonPublicVoid(missingHistoryWindow, "HandleOpenFolderClick", null, null!);
        missingHistoryWindow.FindControl<TextBlock>(AppStrings.Ui.HistoryStatusTextBlockName)!.Text.Should().NotBeNullOrWhiteSpace();

        var missingDuplicatesWindow = new DuplicatesWindow(
            new[]
            {
                new DuplicateFileGroup
                {
                    BaseName = "missing",
                    FilePaths = new[] { Path.Combine(_tempRoot, "missing.txt") },
                },
            },
            launcher);
        InvokeNonPublicVoid(missingDuplicatesWindow, "HandleOpenFileClick", null, null!);
        InvokeNonPublicVoid(missingDuplicatesWindow, "HandleOpenFolderClick", null, null!);
        missingDuplicatesWindow.FindControl<TextBlock>(AppStrings.Ui.DuplicatesStatusTextBlockName)!.Text.Should().NotBeNullOrWhiteSpace();

        var missingPreviewWindow = new PrintPreviewWindow("preview", Path.Combine(_tempRoot, "missing.txt"), launcher, new RecordingPrintService());
        InvokeNonPublicVoid(missingPreviewWindow, "HandleOpenFileClick", null, null!);
        missingPreviewWindow.FindControl<TextBlock>(AppStrings.Ui.PrintPreviewStatusTextBlockName)!.Text.Should().NotBeNullOrWhiteSpace();
        InvokeNonPublicVoid(missingPreviewWindow, "HandleCloseClick", null, null!);

        var utilitiesWindow = new UtilitiesWindow();
        await InvokeNonPublicTaskAsync(
            utilitiesWindow,
            "SelectFolderAsync",
            utilitiesWindow.FindControl<TextBox>(AppStrings.Ui.UtilitySourceTextBoxName)!,
            AppStrings.Ui.UtilityBrowseSourceDialogTitle);
        utilitiesWindow.FindControl<TextBlock>(AppStrings.Ui.UtilityStatusTextBlockName)!.Text.Should().NotBeNullOrWhiteSpace();
        var builtResult = InvokeNonPublic<string>(
            null!,
            typeof(UtilitiesWindow),
            "BuildResultText",
            new FileOperationResult
            {
                SkippedPaths = new[] { "skip.txt" },
            });
        builtResult.Should().Contain(AppStrings.Ui.UtilitySkippedTitle);

        var mainWindow = CreateMainWindow(scanService, appPaths, historyStore, launcher, fileOperations, new RecordingPrintService());
        InvokeNonPublicVoid(mainWindow, "HandleGenerateClick", null, null!);
        InvokeNonPublicVoid(mainWindow, "HandleOpenGeneratedFileClick", null, null!);
        InvokeNonPublicVoid(mainWindow, "HandleOpenOutputFolderClick", null, null!);
        InvokeNonPublicVoid(mainWindow, "HandleOpenDuplicatesClick", null, null!);
        InvokeNonPublicVoid(mainWindow, "HandleOpenPrintPreviewClick", null, null!);
        InvokeNonPublicVoid(mainWindow, "HandleSelectionChanged", null, null!);
        InvokeNonPublicVoid(mainWindow, "HandleTextChanged", null, null!);
        await Task.Delay(10);
        InvokeNonPublic<object?>(mainWindow, "GetSelectedOutputFormat");
        InvokeNonPublic<string>(mainWindow, "GetOutputFolderOrDefault");
        InvokeNonPublic<string>(mainWindow, "GetCurrentPreviewContent");
        InvokeNonPublic<string?>(mainWindow, "GetCurrentEditableSourceFilePath");
        InvokeNonPublicVoid(mainWindow, "SetBusyState", true);
        InvokeNonPublicVoid(mainWindow, "SetBusyState", false);
        var menuItem = InvokeNonPublic<NativeMenuItem>(mainWindow, "CreateMenuItem", "Test", new EventHandler((_, _) => { }));
        menuItem.Header.Should().Be("Test");
        var disabledMenuItem = InvokeNonPublic<NativeMenuItem>(null!, typeof(MainWindow), "CreateDisabledMenuItem", "Disabled");
        disabledMenuItem.IsEnabled.Should().BeFalse();
        var formatChoices = (System.Collections.IEnumerable)InvokeNonPublic<object?>(mainWindow, "CreateOutputFormatChoices")!;
        formatChoices.Cast<object>().Should().HaveCount(3);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempRoot))
        {
            Directory.Delete(_tempRoot, true);
        }
    }

    private MainWindow CreateMainWindow(
        IScanService scanService,
        IAppPaths appPaths,
        IHistoryStore historyStore,
        IExternalLauncher externalLauncher,
        IFileOperationsService fileOperationsService,
        IPrintService printService)
    {
        return new MainWindow(scanService, appPaths, historyStore, externalLauncher, fileOperationsService, printService);
    }

    private async Task<string> CreateCommandScriptAsync(string fileName, string scriptBody)
    {
        var scriptPath = Path.Combine(_tempRoot, fileName);
        await File.WriteAllTextAsync(scriptPath, scriptBody);
        using var chmodProcess = Process.Start(
            new ProcessStartInfo
            {
                FileName = "chmod",
                UseShellExecute = false,
                ArgumentList = { "+x", scriptPath },
            });
        chmodProcess.Should().NotBeNull();
        chmodProcess!.WaitForExit();
        chmodProcess.ExitCode.Should().Be(0);
        return scriptPath;
    }

    private static T InvokeNonPublic<T>(object target, string methodName, params object?[] args)
    {
        var result = InvokeMethod(target, methodName, args);
        return (T)result!;
    }

    private static T InvokeNonPublic<T>(object? target, Type declaringType, string methodName, params object?[] args)
    {
        var result = InvokeMethod(target, declaringType, methodName, args);
        return (T)result!;
    }

    private static void InvokeNonPublicVoid(object target, string methodName, params object?[] args)
    {
        _ = InvokeMethod(target, methodName, args);
    }

    private static async Task InvokeNonPublicTaskAsync(object target, string methodName, params object?[] args)
    {
        var result = InvokeMethod(target, methodName, args);
        if (result is Task task)
        {
            await task;
        }
    }

    private static async Task<T> InvokeNonPublicTaskAsync<T>(object target, string methodName, params object?[] args)
    {
        var result = InvokeMethod(target, methodName, args);
        if (result is Task<T> task)
        {
            return await task;
        }

        if (result is Task taskWithoutResult)
        {
            await taskWithoutResult;
        }

        return (T)result!;
    }

    private static object? InvokeMethod(object target, string methodName, params object?[] args)
    {
        return InvokeMethod(target, target.GetType(), methodName, args);
    }

    private static object? InvokeMethod(object? target, Type declaringType, string methodName, params object?[] args)
    {
        var bindingFlags = BindingFlags.NonPublic | (target is null ? BindingFlags.Static : BindingFlags.Instance);
        var method = declaringType.GetMethod(methodName, bindingFlags)
            ?? throw new MissingMethodException(declaringType.FullName, methodName);
        return method.Invoke(target, args);
    }

    private sealed class RecordingScanService : IScanService
    {
        public ScanResult NextResult { get; set; } = new();

        public Exception? ExceptionToThrow { get; set; }

        public ScanRequest? LastRequest { get; private set; }

        public Task<ScanResult> ExecuteAsync(ScanRequest request, CancellationToken cancellationToken = default)
        {
            LastRequest = request;

            if (ExceptionToThrow is not null)
            {
                throw ExceptionToThrow;
            }

            return Task.FromResult(NextResult);
        }
    }

    private sealed class RecordingAppPaths : IAppPaths
    {
        private readonly string _documentsRoot;

        public RecordingAppPaths(string documentsRoot)
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
            var historyFolder = Path.Combine(outputFolder, AppStrings.Files.HistoryDirectoryName);
            Directory.CreateDirectory(historyFolder);
            return Path.Combine(historyFolder, AppStrings.Files.HistoryFileName);
        }
    }

    private sealed class RecordingHistoryStore : IHistoryStore
    {
        public IReadOnlyList<HistoryEntry> Entries { get; set; } = Array.Empty<HistoryEntry>();

        public Exception? ExceptionToThrow { get; set; }

        public Task<IReadOnlyList<HistoryEntry>> ReadAsync(string historyFilePath, CancellationToken cancellationToken = default)
        {
            if (ExceptionToThrow is not null)
            {
                throw ExceptionToThrow;
            }

            return Task.FromResult(Entries);
        }

        public Task AppendAsync(string historyFilePath, HistoryEntry entry, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class RecordingExternalLauncher : IExternalLauncher
    {
        public List<string> OpenedFiles { get; } = new();

        public List<string> OpenedFolders { get; } = new();

        public Task OpenFileAsync(string path, CancellationToken cancellationToken = default)
        {
            OpenedFiles.Add(path);
            return Task.CompletedTask;
        }

        public Task OpenFolderAsync(string path, CancellationToken cancellationToken = default)
        {
            OpenedFolders.Add(path);
            return Task.CompletedTask;
        }
    }

    private sealed class RecordingFileOperationsService : IFileOperationsService
    {
        public List<(string SourceFolder, string DestinationFolder)> CopyCalls { get; } = new();

        public List<(string SourceFolder, string DestinationFolder)> MoveCalls { get; } = new();

        public List<string> ReorderCalls { get; } = new();

        public Task<FileOperationResult> CopyAsync(string sourceFolder, string destinationFolder, CancellationToken cancellationToken = default)
        {
            CopyCalls.Add((sourceFolder, destinationFolder));
            return Task.FromResult(
                new FileOperationResult
                {
                    AffectedPaths = new[] { Path.Combine(destinationFolder, "copied.txt") },
                });
        }

        public Task<FileOperationResult> MoveAsync(string sourceFolder, string destinationFolder, CancellationToken cancellationToken = default)
        {
            MoveCalls.Add((sourceFolder, destinationFolder));
            return Task.FromResult(
                new FileOperationResult
                {
                    AffectedPaths = new[] { Path.Combine(destinationFolder, "moved.txt") },
                });
        }

        public Task<FileOperationResult> ReorderByExtensionAsync(string sourceFolder, CancellationToken cancellationToken = default)
        {
            ReorderCalls.Add(sourceFolder);
            return Task.FromResult(
                new FileOperationResult
                {
                    AffectedPaths = new[] { Path.Combine(sourceFolder, "txt", "reordered.txt") },
                });
        }
    }

    private sealed class RecordingPrintService : IPrintService
    {
        public List<string> PrintedFiles { get; } = new();

        public List<string> PrintedTexts { get; } = new();

        public Task PrintFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            PrintedFiles.Add(filePath);
            return Task.CompletedTask;
        }

        public Task PrintTextAsync(string content, CancellationToken cancellationToken = default)
        {
            PrintedTexts.Add(content);
            return Task.CompletedTask;
        }
    }
}
