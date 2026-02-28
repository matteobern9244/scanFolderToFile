using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using ScanFolderToFile.Core.Abstractions;
using ScanFolderToFile.Core.Constants;
using ScanFolderToFile.Core.Exceptions;
using ScanFolderToFile.Core.Models;

namespace ScanFolderToFile.Core.Services;

internal static class ExportContentFormatter
{
    public static IReadOnlyList<string> ToDisplayItems(IReadOnlyList<string> items)
    {
        return items.Select(FormatItem).ToArray();
    }

    private static string FormatItem(string item)
    {
        if (string.IsNullOrWhiteSpace(item))
        {
            return string.Empty;
        }

        if (item.IndexOf(Path.DirectorySeparatorChar) >= 0 || item.IndexOf(Path.AltDirectorySeparatorChar) >= 0)
        {
            return Path.GetFileName(item);
        }

        return item;
    }
}

public sealed class TxtExportService : IExportService
{
    public OutputFormat SupportedFormat => OutputFormat.Txt;

    public async Task<string> ExportAsync(IReadOnlyList<string> items, string outputFolder, CancellationToken cancellationToken = default)
    {
        try
        {
            Directory.CreateDirectory(outputFolder);
            var outputPath = Path.Combine(outputFolder, AppStrings.Files.OutputTextFileName);
            var displayItems = ExportContentFormatter.ToDisplayItems(items);
            await File.WriteAllLinesAsync(outputPath, displayItems, cancellationToken).ConfigureAwait(false);
            return outputPath;
        }
        catch (Exception exception) when (exception is not OutputGenerationException)
        {
            throw new OutputGenerationException(AppStrings.Messages.ExportFailed, exception);
        }
    }
}

public sealed class MarkdownExportService : IExportService
{
    public OutputFormat SupportedFormat => OutputFormat.Markdown;

    public async Task<string> ExportAsync(IReadOnlyList<string> items, string outputFolder, CancellationToken cancellationToken = default)
    {
        try
        {
            Directory.CreateDirectory(outputFolder);
            var outputPath = Path.Combine(outputFolder, AppStrings.Files.OutputMarkdownFileName);
            var content = new List<string>
            {
                AppStrings.Files.OutputTitle,
                string.Empty
            };
            content.AddRange(ExportContentFormatter.ToDisplayItems(items));

            await File.WriteAllLinesAsync(outputPath, content, cancellationToken).ConfigureAwait(false);
            return outputPath;
        }
        catch (Exception exception) when (exception is not OutputGenerationException)
        {
            throw new OutputGenerationException(AppStrings.Messages.ExportFailed, exception);
        }
    }
}

public sealed class PdfExportService : IExportService
{
    static PdfExportService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public OutputFormat SupportedFormat => OutputFormat.Pdf;

    public Task<string> ExportAsync(IReadOnlyList<string> items, string outputFolder, CancellationToken cancellationToken = default)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            Directory.CreateDirectory(outputFolder);
            var outputPath = Path.Combine(outputFolder, AppStrings.Files.OutputPdfFileName);
            var displayItems = ExportContentFormatter.ToDisplayItems(items);

            Document
                .Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(30);
                        page.Content().Column(column =>
                        {
                            column.Spacing(8);
                            column.Item().Text(AppStrings.Files.OutputTitle).Bold().FontSize(18);

                            foreach (var item in displayItems)
                            {
                                column.Item().Text(item);
                            }
                        });
                    });
                })
                .GeneratePdf(outputPath);

            return Task.FromResult(outputPath);
        }
        catch (Exception exception) when (exception is not OutputGenerationException)
        {
            throw new OutputGenerationException(AppStrings.Messages.ExportFailed, exception);
        }
    }
}
