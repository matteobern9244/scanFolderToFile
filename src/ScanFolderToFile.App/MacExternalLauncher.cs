using System.Diagnostics;
using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Constants;

namespace ScanFolderToFile.App;

public sealed class MacExternalLauncher : IExternalLauncher
{
    private readonly string _openCommandName;

    public MacExternalLauncher()
        : this(AppStrings.System.MacOpenCommandName)
    {
    }

    internal MacExternalLauncher(string openCommandName)
    {
        _openCommandName = openCommandName;
    }

    public Task OpenFileAsync(string path, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        StartOpenProcess(path, _openCommandName);
        return Task.CompletedTask;
    }

    public Task OpenFolderAsync(string path, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        StartOpenProcess(path, _openCommandName);
        return Task.CompletedTask;
    }

    private static void StartOpenProcess(string path, string openCommandName)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException(null, nameof(path));
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = openCommandName,
            UseShellExecute = false,
        };
        startInfo.ArgumentList.Add(path);

        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException(AppStrings.Messages.ExternalOpenFailed);
    }
}
