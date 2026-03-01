using System.Diagnostics;
using ScanFolderToFile.Core.Constants;

namespace ScanFolderToFile.App;

internal interface IPrintService
{
    Task PrintFileAsync(string filePath, CancellationToken cancellationToken = default);

    Task PrintTextAsync(string content, CancellationToken cancellationToken = default);
}

internal sealed class MacPrintService : IPrintService
{
    public Task PrintFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException(null, nameof(filePath));
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(AppStrings.Ui.PrintPreviewMissingFileStatus, filePath);
        }

        StartPrintProcess(filePath);
        return Task.CompletedTask;
    }

    public async Task PrintTextAsync(string content, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new InvalidOperationException(AppStrings.Ui.NoPrintPreviewStatus);
        }

        var tempFilePath = Path.Combine(
            Path.GetTempPath(),
            string.Concat(AppStrings.System.TempPrintFilePrefix, Guid.NewGuid().ToString(AppStrings.System.GuidCompactFormat), AppStrings.System.TextFileExtension));

        await File.WriteAllTextAsync(tempFilePath, content, cancellationToken).ConfigureAwait(false);
        try
        {
            StartPrintProcess(tempFilePath);
        }
        finally
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }

    private static void StartPrintProcess(string filePath)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = AppStrings.System.MacPrintCommandName,
            UseShellExecute = false,
        };
        startInfo.ArgumentList.Add(filePath);

        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException(AppStrings.Messages.ExternalOpenFailed);
        process.WaitForExit();
        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException(AppStrings.Messages.ExternalOpenFailed);
        }
    }
}
