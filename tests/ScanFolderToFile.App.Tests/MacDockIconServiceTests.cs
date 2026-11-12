using Avalonia.Headless.XUnit;
using Avalonia.Platform;
using FluentAssertions;
using ScanFolderToFile.Core.Constants;

namespace ScanFolderToFile.App.Tests;

public sealed class MacDockIconServiceTests
{
    [AvaloniaFact]
    public void CreateImage_WithBundledIcon_ReturnsNativeImage()
    {
        using var iconStream = AssetLoader.Open(new Uri(AppStrings.MacNative.AppIconResourceUri));
        using var memory = new MemoryStream();
        iconStream.CopyTo(memory);

        var image = MacDockIconService.CreateImage(memory.ToArray());

        image.Should().NotBe(IntPtr.Zero);
        MacDockIconService.Release(image);
    }

    [Fact]
    public void CreateImage_WithUnreadableBytes_ReturnsZero()
    {
        MacDockIconService.CreateImage([]).Should().Be(IntPtr.Zero);
        MacDockIconService.CreateImage([1, 2, 3, 4]).Should().Be(IntPtr.Zero);
    }

    [Fact]
    public void TryApply_WithEmptyStream_ReturnsFalse()
    {
        using var emptyStream = new MemoryStream();

        new MacDockIconService().TryApply(emptyStream).Should().BeFalse();
    }

    [Fact]
    public void TryApply_WithNullStream_Throws()
    {
        var action = () => new MacDockIconService().TryApply(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Release_WithZero_DoesNothing()
    {
        var action = () => MacDockIconService.Release(IntPtr.Zero);

        action.Should().NotThrow();
    }
}
