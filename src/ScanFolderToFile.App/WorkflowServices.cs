using Avalonia.Controls;
using ScanFolderToFile.Core.Abstractions;

namespace ScanFolderToFile.App;

internal interface IEditorWindowLauncher
{
    Task ShowEditorAsync(
        Window owner,
        string? initialFilePath,
        string initialText,
        IExternalLauncher externalLauncher,
        IPrintService printService,
        CancellationToken cancellationToken = default);
}

internal interface IRichTextEditorService
{
    Task ShowAsync(
        Window owner,
        string? initialFilePath,
        string initialText,
        CancellationToken cancellationToken = default);
}

internal interface IPrintWorkflowService
{
    Task ShowPrintPreviewAsync(
        Window owner,
        string content,
        string? sourceFilePath,
        CancellationToken cancellationToken = default);

    Task PrintAsync(
        string content,
        string? sourceFilePath,
        CancellationToken cancellationToken = default);
}

internal sealed class DefaultEditorWindowLauncher : IEditorWindowLauncher
{
    private readonly IRichTextEditorService _richTextEditorService;

    public DefaultEditorWindowLauncher(IRichTextEditorService richTextEditorService)
    {
        _richTextEditorService = richTextEditorService;
    }

    public Task ShowEditorAsync(
        Window owner,
        string? initialFilePath,
        string initialText,
        IExternalLauncher externalLauncher,
        IPrintService printService,
        CancellationToken cancellationToken = default)
    {
        return _richTextEditorService.ShowAsync(owner, initialFilePath, initialText, cancellationToken);
    }
}

internal sealed class PassiveEditorWindowLauncher : IEditorWindowLauncher
{
    public Task ShowEditorAsync(
        Window owner,
        string? initialFilePath,
        string initialText,
        IExternalLauncher externalLauncher,
        IPrintService printService,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.CompletedTask;
    }
}

internal sealed class FallbackRichTextEditorService : IRichTextEditorService
{
    private readonly IExternalLauncher _externalLauncher;
    private readonly IPrintService _printService;

    public FallbackRichTextEditorService(IExternalLauncher externalLauncher, IPrintService printService)
    {
        _externalLauncher = externalLauncher;
        _printService = printService;
    }

    public async Task ShowAsync(
        Window owner,
        string? initialFilePath,
        string initialText,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var editorWindow = new EditorWindow(initialFilePath, initialText, _externalLauncher, _printService);
        await editorWindow.ShowDialog(owner).ConfigureAwait(true);
    }
}

internal sealed class FallbackPrintWorkflowService : IPrintWorkflowService
{
    private readonly IExternalLauncher _externalLauncher;
    private readonly IPrintService _printService;

    public FallbackPrintWorkflowService(IExternalLauncher externalLauncher, IPrintService printService)
    {
        _externalLauncher = externalLauncher;
        _printService = printService;
    }

    public async Task ShowPrintPreviewAsync(
        Window owner,
        string content,
        string? sourceFilePath,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var printPreviewWindow = new PrintPreviewWindow(content, sourceFilePath, _externalLauncher, _printService);
        await printPreviewWindow.ShowDialog(owner).ConfigureAwait(true);
    }

    public async Task PrintAsync(
        string content,
        string? sourceFilePath,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!string.IsNullOrWhiteSpace(sourceFilePath) && File.Exists(sourceFilePath))
        {
            await _printService.PrintFileAsync(sourceFilePath, cancellationToken).ConfigureAwait(false);
            return;
        }

        await _printService.PrintTextAsync(content, cancellationToken).ConfigureAwait(false);
    }
}
