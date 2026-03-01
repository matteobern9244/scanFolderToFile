#pragma warning disable CA1416

using FluentAssertions;
using Foundation;
using ScanFolderToFile.App;
using ScanFolderToFile.Core.Constants;

namespace ScanFolderToFile.App.Tests;

public sealed class NativeWorkflowServicesTests : IDisposable
{
    private readonly string _tempRoot;

    public NativeWorkflowServicesTests()
    {
        _tempRoot = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(_tempRoot);
    }

    [Fact]
    public void AppKitRichTextEditorService_SupportsDirectEditing_ReturnsExpectedValues()
    {
        AppKitRichTextEditorService.SupportsDirectEditing(Path.Combine(_tempRoot, "alpha.txt")).Should().BeTrue();
        AppKitRichTextEditorService.SupportsDirectEditing(Path.Combine(_tempRoot, "alpha.md")).Should().BeTrue();
        AppKitRichTextEditorService.SupportsDirectEditing(Path.Combine(_tempRoot, "alpha.pdf")).Should().BeFalse();
        AppKitRichTextEditorService.SupportsDirectEditing(null).Should().BeFalse();
    }

    [Fact]
    public void AppKitRichTextEditorService_ToggleBulletList_TogglesSelectedLines()
    {
        var content = string.Join(AppStrings.System.NewLine, new[] { "uno", "due" });

        var updated = AppKitRichTextEditorService.ToggleBulletList(content, new NSRange(0, content.Length));
        var reverted = AppKitRichTextEditorService.ToggleBulletList(updated, new NSRange(0, updated.Length));

        updated.Should().Contain(AppStrings.System.HyphenWithTrailingSpace);
        reverted.Should().Be(content);
    }

    [Fact]
    public void AppKitRichTextEditorService_ChangeTextCase_TransformsSelectionAndWholeBuffer()
    {
        var upperSelection = AppKitRichTextEditorService.ChangeTextCase("alpha beta", new NSRange(6, 4), true);
        var lowerBuffer = AppKitRichTextEditorService.ChangeTextCase("ALPHA", new NSRange(0, 0), false);

        upperSelection.Should().Be("alpha BETA");
        lowerBuffer.Should().Be("alpha");
    }

    [Fact]
    public void AppKitPrintWorkflowService_CanUseSourceFile_RequiresEditableExistingFile()
    {
        var textPath = Path.Combine(_tempRoot, "print.txt");
        var pdfPath = Path.Combine(_tempRoot, "print.pdf");
        File.WriteAllText(textPath, "text");
        File.WriteAllText(pdfPath, "pdf");

        AppKitPrintWorkflowService.CanUseSourceFile(textPath).Should().BeTrue();
        AppKitPrintWorkflowService.CanUseSourceFile(pdfPath).Should().BeFalse();
        AppKitPrintWorkflowService.CanUseSourceFile(Path.Combine(_tempRoot, "missing.txt")).Should().BeFalse();
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempRoot))
        {
            Directory.Delete(_tempRoot, true);
        }
    }
}

#pragma warning restore CA1416
