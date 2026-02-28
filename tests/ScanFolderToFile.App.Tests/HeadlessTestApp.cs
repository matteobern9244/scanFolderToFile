using Avalonia;
using Avalonia.Headless;
using Avalonia.Themes.Fluent;

[assembly: AvaloniaTestApplication(typeof(ScanFolderToFile.App.Tests.HeadlessTestAppBuilder))]

namespace ScanFolderToFile.App.Tests;

public sealed class HeadlessTestApplication : Application
{
    public override void Initialize()
    {
        Styles.Add(new FluentTheme());
    }
}

public static class HeadlessTestAppBuilder
{
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<HeadlessTestApplication>()
            .UseHeadless(new AvaloniaHeadlessPlatformOptions());
    }
}
