using System.Collections.ObjectModel;

namespace ScanFolderToFile.Core.Constants;

public static class AppAssets
{
    public sealed record AssetDefinition(string Key, string RelativePath, string Usage, string PreferredTargetFormat);

    public const string OpenFolderIconKey = "OpenFolderIcon";
    public const string OpenFileIconKey = "OpenFileIcon";
    public const string OpenFileAltIconKey = "OpenFileAltIcon";
    public const string PrintIconKey = "PrintIcon";
    public const string GenericFileIconKey = "GenericFileIcon";
    public const string AppIconLegacyKey = "AppIconLegacy";

    public const string OpenFolderIconRelativePath = "scanFolderToFile/Resources/open.png";
    public const string OpenFileIconRelativePath = "scanFolderToFile/Resources/open-file-icon.png";
    public const string OpenFileAltIconRelativePath = "scanFolderToFile/Resources/open-file-icon1.png";
    public const string PrintIconRelativePath = "scanFolderToFile/Resources/print.png";
    public const string GenericFileIconRelativePath = "scanFolderToFile/Resources/file.png";
    public const string AppIconLegacyRelativePath = "scanFolderToFile/Resources/Deleket-Sleek-Xp-Basic-Files.ico";

    public static readonly ReadOnlyCollection<AssetDefinition> All = new(
        new[]
        {
            new AssetDefinition(OpenFolderIconKey, OpenFolderIconRelativePath, "Primary folder open action", "png"),
            new AssetDefinition(OpenFileIconKey, OpenFileIconRelativePath, "Primary file open action", "png"),
            new AssetDefinition(OpenFileAltIconKey, OpenFileAltIconRelativePath, "Alternate file open action", "png"),
            new AssetDefinition(PrintIconKey, PrintIconRelativePath, "Print action", "png"),
            new AssetDefinition(GenericFileIconKey, GenericFileIconRelativePath, "Generic file listing", "png"),
            new AssetDefinition(AppIconLegacyKey, AppIconLegacyRelativePath, "Application icon seed", "ico")
        });
}
