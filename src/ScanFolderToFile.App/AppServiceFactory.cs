using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Services;

namespace ScanFolderToFile.App;

internal static class AppServiceFactory
{
    public static DefaultAppServices Create()
    {
        var appPaths = new MacAppPaths();
        var historyStore = new JsonHistoryStore();
        var externalLauncher = new MacExternalLauncher();
        var fileOperationsService = new FileOperationsService();
        var printService = new MacPrintService();
        var fallbackPrintWorkflowService = new FallbackPrintWorkflowService(externalLauncher, printService);
        var printWorkflowService = new AppKitPrintWorkflowService(printService, fallbackPrintWorkflowService);
        var fallbackRichTextEditorService = new FallbackRichTextEditorService(externalLauncher, printService);
        var richTextEditorService = new AppKitRichTextEditorService(printWorkflowService, fallbackRichTextEditorService);
        var editorWindowLauncher = new DefaultEditorWindowLauncher(richTextEditorService);
        var scanService = new ScanService(
            appPaths,
            new FileCollector(),
            new IExportService[]
            {
                new TxtExportService(),
                new MarkdownExportService(),
                new PdfExportService(),
            },
            historyStore,
            new ZipArchiveService(),
            new DuplicateDetector(),
            new SystemClock());

        return new DefaultAppServices(
            scanService,
            appPaths,
            historyStore,
            externalLauncher,
            fileOperationsService,
            printService,
            richTextEditorService,
            printWorkflowService,
            editorWindowLauncher);
    }
}

internal sealed record DefaultAppServices(
    IScanService ScanService,
    IAppPaths AppPaths,
    IHistoryStore HistoryStore,
    IExternalLauncher ExternalLauncher,
    IFileOperationsService FileOperationsService,
    IPrintService PrintService,
    IRichTextEditorService RichTextEditorService,
    IPrintWorkflowService PrintWorkflowService,
    IEditorWindowLauncher EditorWindowLauncher);
