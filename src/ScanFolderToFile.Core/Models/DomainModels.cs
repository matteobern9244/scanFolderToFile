using System.Text.Json.Serialization;
using ScanFolderToFile.Core.Constants;

namespace ScanFolderToFile.Core.Models;

public enum OutputFormat
{
    Txt = 0,
    Pdf = 1,
    Markdown = 2
}

public sealed class ScanFilter
{
    public decimal? MinSizeMb { get; init; }

    public decimal? MaxSizeMb { get; init; }

    public DateTime? StartDate { get; init; }

    public DateTime? EndDate { get; init; }
}

public sealed class ScanRequest
{
    public string SourceFolder { get; init; } = string.Empty;

    public string OutputFolder { get; init; } = string.Empty;

    public OutputFormat OutputFormat { get; init; } = OutputFormat.Txt;

    public bool OnlyExtensions { get; init; }

    public bool CreateZip { get; init; }

    public bool CollectDuplicates { get; init; }

    public ScanFilter? Filter { get; init; }
}

public sealed class ScanResult
{
    public IReadOnlyList<string> CollectedItems { get; init; } = Array.Empty<string>();

    public string? GeneratedFilePath { get; init; }

    public string? GeneratedZipPath { get; init; }

    public IReadOnlyList<DuplicateFileGroup> DuplicateGroups { get; init; } = Array.Empty<DuplicateFileGroup>();

    public IReadOnlyList<string> Warnings { get; init; } = Array.Empty<string>();
}

public sealed class HistoryEntry
{
    [JsonPropertyName(AppStrings.Json.HistoryEntryFileNameProperty)]
    public string FileName { get; init; } = string.Empty;

    [JsonPropertyName(AppStrings.Json.HistoryEntryExtensionProperty)]
    public string Extension { get; init; } = string.Empty;

    [JsonPropertyName(AppStrings.Json.HistoryEntryCreatedAtProperty)]
    public DateTime CreatedAt { get; init; }
}

public sealed class DuplicateFileGroup
{
    public string BaseName { get; init; } = string.Empty;

    public IReadOnlyList<string> FilePaths { get; init; } = Array.Empty<string>();
}
