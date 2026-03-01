using Avalonia;

namespace ScanFolderToFile.App;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    internal static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .With(
                new AvaloniaNativePlatformOptions
                {
                    // Force software rendering to avoid native renderer crashes on recent macOS builds.
                    RenderingMode = new[] { AvaloniaNativeRenderingMode.Software },
                }
            )
            .LogToTrace();
    }
}
