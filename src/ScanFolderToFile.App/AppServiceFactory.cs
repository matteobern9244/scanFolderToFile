using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Services;

namespace ScanFolderToFile.App;

internal static class AppServiceFactory
{
    public static DefaultAppServices Create()
    {
        var appPaths = new MacAppPaths();
        var historyStore = new JsonHistoryStore();
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

        return new DefaultAppServices(scanService, appPaths, historyStore, new MacExternalLauncher());
    }
}

internal sealed record DefaultAppServices(
    IScanService ScanService,
    IAppPaths AppPaths,
    IHistoryStore HistoryStore,
    IExternalLauncher ExternalLauncher);
