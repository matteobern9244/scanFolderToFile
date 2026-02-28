using FluentAssertions;
using ScanFolderToFile.Core.Exceptions;
using ScanFolderToFile.Core.Models;
using ScanFolderToFile.Core.Services;
using ScanFolderToFile.Core.Tests.TestSupport;

namespace ScanFolderToFile.Core.Tests;

public sealed class ScanRequestValidatorTests
{
    [Fact]
    public void Validate_WhenSourceFolderIsMissing_ThrowsInvalidScanRequestException()
    {
        var request = new ScanRequest
        {
            SourceFolder = string.Empty,
            OutputFolder = "ignored"
        };

        var action = () => ScanRequestValidator.Validate(request);

        action.Should().Throw<InvalidScanRequestException>();
    }

    [Fact]
    public void Validate_WhenOutputFolderIsMissing_ThrowsInvalidScanRequestException()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        var request = new ScanRequest
        {
            SourceFolder = sourceDirectory.Path,
            OutputFolder = string.Empty
        };

        var action = () => ScanRequestValidator.Validate(request);

        action.Should().Throw<InvalidScanRequestException>();
    }

    [Fact]
    public void Validate_WhenSourceFolderDoesNotExist_ThrowsInvalidScanRequestException()
    {
        var request = new ScanRequest
        {
            SourceFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")),
            OutputFolder = Path.GetTempPath()
        };

        var action = () => ScanRequestValidator.Validate(request);

        action.Should().Throw<InvalidScanRequestException>();
    }

    [Fact]
    public void Validate_WhenOutputFormatIsInvalid_ThrowsInvalidScanRequestException()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        var request = new ScanRequest
        {
            SourceFolder = sourceDirectory.Path,
            OutputFolder = sourceDirectory.Path,
            OutputFormat = (OutputFormat)99
        };

        var action = () => ScanRequestValidator.Validate(request);

        action.Should().Throw<InvalidScanRequestException>();
    }

    [Fact]
    public void Validate_WhenMinSizeIsNegative_ThrowsInvalidScanRequestException()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        var request = new ScanRequest
        {
            SourceFolder = sourceDirectory.Path,
            OutputFolder = sourceDirectory.Path,
            Filter = new ScanFilter
            {
                MinSizeMb = -1
            }
        };

        var action = () => ScanRequestValidator.Validate(request);

        action.Should().Throw<InvalidScanRequestException>();
    }

    [Fact]
    public void Validate_WhenMaxSizeIsNegative_ThrowsInvalidScanRequestException()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        var request = new ScanRequest
        {
            SourceFolder = sourceDirectory.Path,
            OutputFolder = sourceDirectory.Path,
            Filter = new ScanFilter
            {
                MaxSizeMb = -1
            }
        };

        var action = () => ScanRequestValidator.Validate(request);

        action.Should().Throw<InvalidScanRequestException>();
    }

    [Fact]
    public void Validate_WhenSizeRangeIsInvalid_ThrowsInvalidScanRequestException()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        var request = new ScanRequest
        {
            SourceFolder = sourceDirectory.Path,
            OutputFolder = sourceDirectory.Path,
            Filter = new ScanFilter
            {
                MinSizeMb = 5,
                MaxSizeMb = 1
            }
        };

        var action = () => ScanRequestValidator.Validate(request);

        action.Should().Throw<InvalidScanRequestException>();
    }

    [Fact]
    public void Validate_WhenDateRangeIsInvalid_ThrowsInvalidScanRequestException()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        var request = new ScanRequest
        {
            SourceFolder = sourceDirectory.Path,
            OutputFolder = sourceDirectory.Path,
            Filter = new ScanFilter
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today
            }
        };

        var action = () => ScanRequestValidator.Validate(request);

        action.Should().Throw<InvalidScanRequestException>();
    }

    [Fact]
    public void Validate_WithValidRequest_DoesNotThrow()
    {
        using var sourceDirectory = TestEnvironment.CreateTemporaryDirectory();
        using var outputDirectory = TestEnvironment.CreateTemporaryDirectory();
        var request = new ScanRequest
        {
            SourceFolder = sourceDirectory.Path,
            OutputFolder = outputDirectory.Path
        };

        var action = () => ScanRequestValidator.Validate(request);

        action.Should().NotThrow();
    }
}
