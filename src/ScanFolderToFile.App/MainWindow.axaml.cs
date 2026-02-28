using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ScanFolderToFile.Core.Constants;

namespace ScanFolderToFile.App;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        AvaloniaXamlLoader.Load(this);
        Title = AppStrings.Ui.WindowTitle;
        SetText(AppStrings.Ui.HeadlineTextBlockName, AppStrings.Ui.ShellHeadline);
        SetText(AppStrings.Ui.SubheadlineTextBlockName, AppStrings.Ui.ShellSubheadline);
        SetText(AppStrings.Ui.CoreTitleTextBlockName, AppStrings.Ui.ShellCoreTitle);
        SetText(AppStrings.Ui.CoreBodyTextBlockName, AppStrings.Ui.ShellCoreBody);
        SetText(AppStrings.Ui.LauncherTitleTextBlockName, AppStrings.Ui.ShellLauncherTitle);
        SetText(AppStrings.Ui.LauncherBodyTextBlockName, AppStrings.Ui.ShellLauncherBody);
        SetText(AppStrings.Ui.CiTitleTextBlockName, AppStrings.Ui.ShellCiTitle);
        SetText(AppStrings.Ui.CiBodyTextBlockName, AppStrings.Ui.ShellCiBody);
        SetText(AppStrings.Ui.FooterTextBlockName, AppStrings.Ui.ShellFooter);
    }

    private void SetText(string controlName, string value)
    {
        var control = this.FindControl<TextBlock>(controlName)
            ?? throw new InvalidOperationException(string.Concat(AppStrings.Ui.MissingTextControlPrefix, controlName));

        control.Text = value;
    }
}
