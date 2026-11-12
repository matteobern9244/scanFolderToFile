using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using ScanFolderToFile.Core.Constants;

namespace ScanFolderToFile.App;

public sealed partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        if (OperatingSystem.IsMacOS())
        {
            using var iconStream = AssetLoader.Open(new Uri(AppStrings.MacNative.AppIconResourceUri));
            new MacDockIconService().TryApply(iconStream);
        }

        base.OnFrameworkInitializationCompleted();
    }
}
