using System.Text.Json;
using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Constants;
using ScanFolderToFile.Core.Models;

namespace ScanFolderToFile.Core.Services;

public sealed class JsonHistoryStore : IHistoryStore
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };

    public async Task<IReadOnlyList<HistoryEntry>> ReadAsync(string historyFilePath, CancellationToken cancellationToken = default)
    {
        var entries = await ReadInternalAsync(historyFilePath, cancellationToken).ConfigureAwait(false);
        return entries
            .OrderByDescending(entry => entry.CreatedAt)
            .ToArray();
    }

    public async Task AppendAsync(string historyFilePath, HistoryEntry entry, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entry);

        var entries = await ReadInternalAsync(historyFilePath, cancellationToken).ConfigureAwait(false);
        var updatedEntries = entries.ToList();
        updatedEntries.Add(entry);

        var historyDirectory = Path.GetDirectoryName(historyFilePath);
        if (!string.IsNullOrWhiteSpace(historyDirectory))
        {
            Directory.CreateDirectory(historyDirectory);
        }

        await using var stream = File.Create(historyFilePath);
        await JsonSerializer.SerializeAsync(stream, updatedEntries, SerializerOptions, cancellationToken).ConfigureAwait(false);
    }

    private static async Task<IReadOnlyList<HistoryEntry>> ReadInternalAsync(string historyFilePath, CancellationToken cancellationToken)
    {
        if (!File.Exists(historyFilePath))
        {
            return Array.Empty<HistoryEntry>();
        }

        await using var stream = File.OpenRead(historyFilePath);

        try
        {
            var entries = await JsonSerializer.DeserializeAsync<List<HistoryEntry>>(stream, SerializerOptions, cancellationToken)
                .ConfigureAwait(false);
            return entries is null ? Array.Empty<HistoryEntry>() : entries;
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException(AppStrings.Messages.HistoryFileCorrupted, exception);
        }
    }
}
