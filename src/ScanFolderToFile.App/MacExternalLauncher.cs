using System.Diagnostics;
using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Constants;

namespace ScanFolderToFile.App;

public sealed class MacExternalLauncher : IExternalLauncher
{
    public Task OpenFileAsync(string path, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        StartOpenProcess(path);
        return Task.CompletedTask;
    }

    public Task OpenFolderAsync(string path, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        StartOpenProcess(path);
        return Task.CompletedTask;
    }

    private static void StartOpenProcess(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException(null, nameof(path));
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = AppStrings.System.MacOpenCommandName,
            UseShellExecute = false,
        };
        startInfo.ArgumentList.Add(path);

        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException(AppStrings.Messages.ExternalOpenFailed);
    }
}
