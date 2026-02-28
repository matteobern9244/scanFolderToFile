using ScanFolderToFile.Core.Constants;
using ScanFolderToFile.Core.Exceptions;
using ScanFolderToFile.Core.Models;

namespace ScanFolderToFile.Core.Services;

public static class ScanRequestValidator
{
    public static void Validate(ScanRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.SourceFolder))
        {
            throw new InvalidScanRequestException(AppStrings.Messages.MissingSourceFolder);
        }

        if (string.IsNullOrWhiteSpace(request.OutputFolder))
        {
            throw new InvalidScanRequestException(AppStrings.Messages.MissingOutputFolder);
        }

        if (!Directory.Exists(request.SourceFolder))
        {
            throw new InvalidScanRequestException(AppStrings.Messages.SourceFolderNotFound);
        }

        if (!Enum.IsDefined(request.OutputFormat))
        {
            throw new InvalidScanRequestException(AppStrings.Messages.InvalidOutputFormat);
        }

        if (request.Filter is null)
        {
            return;
        }

        if (request.Filter.MinSizeMb is < 0)
        {
            throw new InvalidScanRequestException(AppStrings.Messages.NegativeMinSize);
        }

        if (request.Filter.MaxSizeMb is < 0)
        {
            throw new InvalidScanRequestException(AppStrings.Messages.NegativeMaxSize);
        }

        if (request.Filter.MinSizeMb.HasValue &&
            request.Filter.MaxSizeMb.HasValue &&
            request.Filter.MinSizeMb.Value > request.Filter.MaxSizeMb.Value)
        {
            throw new InvalidScanRequestException(AppStrings.Messages.InvalidSizeRange);
        }

        if (request.Filter.StartDate.HasValue &&
            request.Filter.EndDate.HasValue &&
            request.Filter.StartDate.Value.Date > request.Filter.EndDate.Value.Date)
        {
            throw new InvalidScanRequestException(AppStrings.Messages.InvalidDateRange);
        }
    }
}
