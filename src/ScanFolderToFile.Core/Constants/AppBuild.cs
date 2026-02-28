namespace ScanFolderToFile.Core.Constants;

public static class AppBuild
{
    public const string DotnetChannel = "10.0.x";
    public const string ConfigurationRelease = "Release";
    public const int CoverageLineThreshold = 90;
    public const int CoverageBranchThreshold = 85;
    public const int CiArtifactsRetentionDays = 14;
    public const int ManualArtifactsRetentionDays = 30;
}
