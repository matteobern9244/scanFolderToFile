using ScanFolderToFile.Core.Abstractions;

namespace ScanFolderToFile.Core.Tests.TestSupport;

internal static class TestEnvironment
{
    public static string RepositoryRoot => FindRepositoryRoot();

    public static TemporaryDirectory CreateTemporaryDirectory()
    {
        var directoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directoryPath);
        return new TemporaryDirectory(directoryPath);
    }

    private static string FindRepositoryRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);

        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, "scanFolderToFile.sln")) &&
                File.Exists(Path.Combine(current.FullName, "ScanFolderToFile.Modern.sln")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new DirectoryNotFoundException("Unable to locate the repository root.");
    }
}

internal sealed class TemporaryDirectory : IDisposable
{
    public TemporaryDirectory(string path)
    {
        Path = path;
    }

    public string Path { get; }

    public void Dispose()
    {
        if (Directory.Exists(Path))
        {
            Directory.Delete(Path, true);
        }
    }
}

internal sealed class FixedClock : IClock
{
    public FixedClock(DateTime now)
    {
        Now = now;
    }

    public DateTime Now { get; }
}
