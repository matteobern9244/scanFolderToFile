#pragma warning disable CA1416
#pragma warning disable CA1422

using System.Diagnostics.CodeAnalysis;
using System.Text;
using AppKit;
using Avalonia.Controls;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using ScanFolderToFile.Core.Constants;
using NFloat = System.Runtime.InteropServices.NFloat;

namespace ScanFolderToFile.App;

[ExcludeFromCodeCoverage]
internal sealed class AppKitRichTextEditorService : IRichTextEditorService
{
    private readonly IPrintWorkflowService _printWorkflowService;
    private readonly IRichTextEditorService _fallbackService;

    public AppKitRichTextEditorService(
        IPrintWorkflowService printWorkflowService,
        IRichTextEditorService fallbackService)
    {
        _printWorkflowService = printWorkflowService;
        _fallbackService = fallbackService;
    }

    public async Task ShowAsync(
        Window owner,
        string? initialFilePath,
        string initialText,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!OperatingSystem.IsMacOS())
        {
            await _fallbackService.ShowAsync(owner, initialFilePath, initialText, cancellationToken).ConfigureAwait(true);
            return;
        }

        try
        {
            var session = new NativeEditorSession(owner, initialFilePath, initialText, _printWorkflowService);
            await session.ShowAsync(cancellationToken).ConfigureAwait(true);
        }
        catch
        {
            await _fallbackService.ShowAsync(owner, initialFilePath, initialText, cancellationToken).ConfigureAwait(true);
        }
    }

    internal static bool SupportsDirectEditing(string? filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return false;
        }

        var extension = Path.GetExtension(filePath);
        return string.Equals(extension, AppStrings.System.TextFileExtension, StringComparison.OrdinalIgnoreCase)
            || string.Equals(extension, AppStrings.System.MarkdownFileExtension, StringComparison.OrdinalIgnoreCase)
            || string.Equals(extension, AppStrings.System.JsonFileExtension, StringComparison.OrdinalIgnoreCase)
            || string.Equals(extension, AppStrings.System.CsvFileExtension, StringComparison.OrdinalIgnoreCase)
            || string.Equals(extension, AppStrings.System.XmlFileExtension, StringComparison.OrdinalIgnoreCase)
            || string.Equals(extension, AppStrings.System.LogFileExtension, StringComparison.OrdinalIgnoreCase);
    }

    internal static NSFont GetFixedPitchFont(NFloat pointSize)
    {
        return NSFont.UserFixedPitchFontOfSize(pointSize)
            ?? NSFont.SystemFontOfSize(pointSize)
            ?? throw new InvalidOperationException(AppStrings.Messages.ExternalOpenFailed);
    }

    internal static string ToggleBulletList(string content, NSRange selectedRange)
    {
        var lines = content.Split(new[] { AppStrings.System.NewLine }, StringSplitOptions.None);
        if (lines.Length == 0)
        {
            return string.Concat(AppStrings.System.HyphenWithTrailingSpace);
        }

        var selection = GetSafeSelection(content, selectedRange);
        var startLine = GetLineIndex(content, selection.Location);
        var endLine = GetLineIndex(content, selection.Location + selection.Length);

        for (var index = startLine; index <= endLine && index < lines.Length; index++)
        {
            if (lines[index].StartsWith(AppStrings.System.HyphenWithTrailingSpace, StringComparison.Ordinal))
            {
                lines[index] = lines[index].Substring(AppStrings.System.HyphenWithTrailingSpace.Length);
            }
            else
            {
                lines[index] = string.Concat(AppStrings.System.HyphenWithTrailingSpace, lines[index]);
            }
        }

        return string.Join(AppStrings.System.NewLine, lines);
    }

    internal static string ChangeTextCase(string content, NSRange selectedRange, bool useUpperCase)
    {
        if (string.IsNullOrEmpty(content))
        {
            return content;
        }

        var selection = GetSafeSelection(content, selectedRange);
        if (selection.Length == 0)
        {
            return useUpperCase
                ? content.ToUpperInvariant()
                : content.ToLowerInvariant();
        }

        var prefix = content.Substring(0, (int)selection.Location);
        var middle = content.Substring((int)selection.Location, (int)selection.Length);
        var suffix = content.Substring((int)(selection.Location + selection.Length));
        var updatedMiddle = useUpperCase
            ? middle.ToUpperInvariant()
            : middle.ToLowerInvariant();

        return string.Concat(prefix, updatedMiddle, suffix);
    }

    private static int GetLineIndex(string content, nint location)
    {
        if (string.IsNullOrEmpty(content))
        {
            return 0;
        }

        var boundedLocation = (int)Math.Clamp(location, 0, content.Length);
        var lineBreakCount = 0;
        for (var index = 0; index < boundedLocation; index++)
        {
            if (content[index] == AppStrings.System.NewLine[0])
            {
                lineBreakCount++;
            }
        }

        return lineBreakCount;
    }

    private static NSRange GetSafeSelection(string content, NSRange selectedRange)
    {
        var maxLength = Math.Max(content.Length, 0);
        var location = (nint)Math.Clamp((int)selectedRange.Location, 0, maxLength);
        var remainingLength = maxLength - (int)location;
        var length = (nint)Math.Clamp((int)selectedRange.Length, 0, remainingLength);
        return new NSRange(location, length);
    }

    [ExcludeFromCodeCoverage]
    private sealed class NativeEditorSession
    {
        private readonly Window _owner;
        private readonly string _initialText;
        private readonly string? _initialFilePath;
        private readonly IPrintWorkflowService _printWorkflowService;
        private readonly TaskCompletionSource<object?> _completionSource;
        private readonly NSWindow _window;
        private readonly NSTextView _textView;
        private readonly NSTextField _pathLabel;
        private readonly NSTextField _statusLabel;
        private readonly NSTextField _cursorLabel;
        private readonly NSColorWell _colorWell;
        private NSObject? _closeObserver;
        private NSObject? _selectionObserver;
        private string? _currentFilePath;
        private bool _canSaveDirectly;
        private bool _wrapEnabled;

        public NativeEditorSession(
            Window owner,
            string? initialFilePath,
            string initialText,
            IPrintWorkflowService printWorkflowService)
        {
            _owner = owner;
            _initialFilePath = initialFilePath;
            _initialText = initialText;
            _printWorkflowService = printWorkflowService;
            _completionSource = new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);
            _window = new NSWindow(
                new CGRect(80, 80, 1280, 820),
                NSWindowStyle.Titled | NSWindowStyle.Closable | NSWindowStyle.Miniaturizable | NSWindowStyle.Resizable,
                NSBackingStore.Buffered,
                false);
            _window.Title = AppStrings.Ui.EditorWindowTitle;

            var rootView = new NSView(new CGRect(0, 0, 1280, 820))
            {
                AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable
            };

            var scrollView = new NSScrollView(new CGRect(20, 56, 1240, 560))
            {
                HasVerticalScroller = true,
                HasHorizontalScroller = false,
                AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable
            };
            _textView = new NSTextView(new CGRect(0, 0, 1240, 560))
            {
                Editable = true,
                Selectable = true,
                AllowsUndo = true,
                RichText = true,
                ImportsGraphics = false,
                AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable,
                Font = GetFixedPitchFont(14),
            };
            scrollView.DocumentView = _textView;
            rootView.AddSubview(scrollView);

            var firstRowY = 770;
            var secondRowY = 730;
            var buttonWidth = 92;
            var buttonHeight = 28;
            var buttonStep = 98;
            var leftInset = 20;

            AddButton(rootView, AppStrings.Ui.NativeEditorNewButtonText, leftInset + (buttonStep * 0), firstRowY, buttonWidth, buttonHeight, (_, _) => ResetDocument());
            AddButton(rootView, AppStrings.Ui.EditorOpenButtonText, leftInset + (buttonStep * 1), firstRowY, buttonWidth, buttonHeight, async (_, _) => await OpenDocumentAsync().ConfigureAwait(true));
            AddButton(rootView, AppStrings.Ui.EditorSaveButtonText, leftInset + (buttonStep * 2), firstRowY, buttonWidth, buttonHeight, async (_, _) => await SaveAsync().ConfigureAwait(true));
            AddButton(rootView, AppStrings.Ui.EditorSaveAsButtonText, leftInset + (buttonStep * 3), firstRowY, buttonWidth + 24, buttonHeight, async (_, _) => await SaveAsAsync().ConfigureAwait(true));
            AddButton(rootView, AppStrings.Ui.NativeEditorUndoButtonText, leftInset + (buttonStep * 5) + 24, firstRowY, buttonWidth, buttonHeight, (_, _) => PerformSelector(AppStrings.System.SelectorUndo));
            AddButton(rootView, AppStrings.Ui.NativeEditorRedoButtonText, leftInset + (buttonStep * 6) + 24, firstRowY, buttonWidth, buttonHeight, (_, _) => PerformSelector(AppStrings.System.SelectorRedo));
            AddButton(rootView, AppStrings.Ui.NativeEditorCutButtonText, leftInset + (buttonStep * 7) + 24, firstRowY, buttonWidth, buttonHeight, (_, _) => _textView.Cut(_textView));
            AddButton(rootView, AppStrings.Ui.NativeEditorCopyButtonText, leftInset + (buttonStep * 8) + 24, firstRowY, buttonWidth, buttonHeight, (_, _) => _textView.Copy(_textView));
            AddButton(rootView, AppStrings.Ui.NativeEditorPasteButtonText, leftInset + (buttonStep * 9) + 24, firstRowY, buttonWidth, buttonHeight, (_, _) => _textView.Paste(_textView));
            AddButton(rootView, AppStrings.Ui.NativeEditorDeleteButtonText, leftInset + (buttonStep * 10) + 24, firstRowY, buttonWidth, buttonHeight, (_, _) => _textView.Delete(_textView));

            AddButton(rootView, AppStrings.Ui.NativeEditorSelectAllButtonText, leftInset + (buttonStep * 0), secondRowY, buttonWidth + 42, buttonHeight, (_, _) => _textView.SelectAll(_textView));
            AddButton(rootView, AppStrings.Ui.NativeEditorWrapButtonText, leftInset + (buttonStep * 1) + 42, secondRowY, buttonWidth, buttonHeight, (_, _) => ToggleWrap());
            AddButton(rootView, AppStrings.Ui.NativeEditorFontButtonText, leftInset + (buttonStep * 2) + 42, secondRowY, buttonWidth, buttonHeight, (_, _) => ShowFontPanel());
            AddButton(rootView, AppStrings.Ui.NativeEditorGrowFontButtonText, leftInset + (buttonStep * 3) + 42, secondRowY, buttonWidth, buttonHeight, (_, _) => AdjustFontSize(1));
            AddButton(rootView, AppStrings.Ui.NativeEditorShrinkFontButtonText, leftInset + (buttonStep * 4) + 42, secondRowY, buttonWidth, buttonHeight, (_, _) => AdjustFontSize(-1));
            AddButton(rootView, AppStrings.Ui.NativeEditorBoldButtonText, leftInset + (buttonStep * 5) + 42, secondRowY, buttonWidth, buttonHeight, (_, _) => PerformSelector(AppStrings.System.SelectorToggleBoldface));
            AddButton(rootView, AppStrings.Ui.NativeEditorItalicButtonText, leftInset + (buttonStep * 6) + 42, secondRowY, buttonWidth, buttonHeight, (_, _) => PerformSelector(AppStrings.System.SelectorToggleItalics));
            AddButton(rootView, AppStrings.Ui.NativeEditorUnderlineButtonText, leftInset + (buttonStep * 7) + 42, secondRowY, buttonWidth, buttonHeight, (_, _) => _textView.Underline(_textView));
            AddButton(rootView, AppStrings.Ui.NativeEditorAlignLeftButtonText, leftInset + (buttonStep * 8) + 42, secondRowY, buttonWidth, buttonHeight, (_, _) => _textView.AlignLeft(_textView));
            AddButton(rootView, AppStrings.Ui.NativeEditorAlignCenterButtonText, leftInset + (buttonStep * 9) + 42, secondRowY, buttonWidth, buttonHeight, (_, _) => _textView.AlignCenter(_textView));
            AddButton(rootView, AppStrings.Ui.NativeEditorAlignRightButtonText, leftInset + (buttonStep * 10) + 42, secondRowY, buttonWidth, buttonHeight, (_, _) => _textView.AlignRight(_textView));
            AddButton(rootView, AppStrings.Ui.NativeEditorBulletsButtonText, leftInset + (buttonStep * 11) + 42, secondRowY, buttonWidth, buttonHeight, (_, _) => ApplyBulletList());

            AddButton(rootView, AppStrings.Ui.NativeEditorLowerCaseButtonText, 20, 690, 110, buttonHeight, (_, _) => ChangeCase(false));
            AddButton(rootView, AppStrings.Ui.NativeEditorUpperCaseButtonText, 136, 690, 110, buttonHeight, (_, _) => ChangeCase(true));
            AddButton(rootView, AppStrings.Ui.NativeEditorZoomInButtonText, 252, 690, 92, buttonHeight, (_, _) => AdjustFontSize(2));
            AddButton(rootView, AppStrings.Ui.NativeEditorZoomOutButtonText, 350, 690, 92, buttonHeight, (_, _) => AdjustFontSize(-2));
            AddButton(rootView, AppStrings.Ui.NativeEditorPreviewButtonText, 448, 690, 110, buttonHeight, async (_, _) => await ShowPreviewAsync().ConfigureAwait(true));
            AddButton(rootView, AppStrings.Ui.NativeEditorPrintButtonText, 564, 690, 92, buttonHeight, async (_, _) => await PrintAsync().ConfigureAwait(true));
            AddButton(rootView, AppStrings.Ui.EditorCloseButtonText, 662, 690, 92, buttonHeight, (_, _) => _window.Close());

            _colorWell = new NSColorWell(new CGRect(760, 690, 64, buttonHeight));
            _colorWell.Activated += HandleColorChanged;
            rootView.AddSubview(_colorWell);

            _pathLabel = CreateLabel(new CGRect(20, 654, 1240, 22), AppStrings.Ui.EditorNoFileLoaded);
            _statusLabel = CreateLabel(new CGRect(20, 630, 1240, 22), AppStrings.Ui.NativeEditorReadyStatus);
            _cursorLabel = CreateLabel(new CGRect(20, 20, 1240, 22), BuildCursorStatus(1, 1));
            rootView.AddSubview(_pathLabel);
            rootView.AddSubview(_statusLabel);
            rootView.AddSubview(_cursorLabel);

            _window.ContentView = rootView;
        }

        public Task ShowAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            InitializeDocument();
            SubscribeNotifications();
            NSApplication.SharedApplication.Activate();
            _window.MakeKeyAndOrderFront(null);
            return _completionSource.Task;
        }

        private void SubscribeNotifications()
        {
            _closeObserver = NSWindow.Notifications.ObserveWillClose(_window, (_, _) =>
            {
                _selectionObserver?.Dispose();
                _closeObserver?.Dispose();
                _completionSource.TrySetResult(null);
            });
            _selectionObserver = NSTextView.Notifications.ObserveDidChangeSelection(_textView, (_, _) => UpdateCursorStatus());
        }

        private void InitializeDocument()
        {
            _wrapEnabled = true;
            ApplyWrapMode();

            if (!string.IsNullOrWhiteSpace(_initialFilePath) && File.Exists(_initialFilePath))
            {
                if (SupportsDirectEditing(_initialFilePath))
                {
                    LoadFile(_initialFilePath);
                    return;
                }

                _textView.Value = _initialText;
                _currentFilePath = null;
                _canSaveDirectly = false;
                _pathLabel.StringValue = _initialFilePath;
                _statusLabel.StringValue = AppStrings.Ui.NativeEditorUnsupportedExtensionStatus;
                UpdateCursorStatus();
                return;
            }

            _textView.Value = _initialText;
            _currentFilePath = null;
            _canSaveDirectly = false;
            _pathLabel.StringValue = AppStrings.Ui.EditorUntitledLabel;
            _statusLabel.StringValue = string.IsNullOrWhiteSpace(_initialText)
                ? AppStrings.Ui.NativeEditorReadyStatus
                : AppStrings.Ui.NativeEditorPreviewLoadedStatus;
            UpdateCursorStatus();
        }

        private void ResetDocument()
        {
            _textView.Value = string.Empty;
            _currentFilePath = null;
            _canSaveDirectly = false;
            _pathLabel.StringValue = AppStrings.Ui.EditorUntitledLabel;
            _statusLabel.StringValue = AppStrings.Ui.NativeEditorReadyStatus;
            _textView.SetSelectedRange(new NSRange(0, 0));
            UpdateCursorStatus();
        }

        private async Task OpenDocumentAsync()
        {
            var openPanel = NSOpenPanel.OpenPanel;
            openPanel.CanChooseDirectories = false;
            openPanel.CanChooseFiles = true;
            openPanel.Title = AppStrings.Ui.EditorOpenDialogTitle;
            var response = (nint)openPanel.RunModal();

            if (response != (nint)NSModalResponse.OK || openPanel.Urls.Length == 0)
            {
                return;
            }

            var selectedPath = openPanel.Urls[0].Path ?? string.Empty;
            if (string.IsNullOrWhiteSpace(selectedPath))
            {
                return;
            }

            await Task.Yield();
            LoadFile(selectedPath);
        }

        private void LoadFile(string filePath)
        {
            if (!SupportsDirectEditing(filePath))
            {
                _currentFilePath = null;
                _canSaveDirectly = false;
                _statusLabel.StringValue = AppStrings.Ui.NativeEditorUnsupportedExtensionStatus;
                return;
            }

            _textView.Value = File.ReadAllText(filePath);
            _currentFilePath = filePath;
            _canSaveDirectly = true;
            _pathLabel.StringValue = filePath;
            _statusLabel.StringValue = string.Concat(AppStrings.Ui.EditorLoadedFromFilePrefix, filePath);
            _textView.SetSelectedRange(new NSRange(0, 0));
            UpdateCursorStatus();
        }

        private async Task SaveAsync()
        {
            if (_canSaveDirectly && !string.IsNullOrWhiteSpace(_currentFilePath))
            {
                await SaveToPathAsync(_currentFilePath).ConfigureAwait(true);
                return;
            }

            await SaveAsAsync().ConfigureAwait(true);
        }

        private async Task SaveAsAsync()
        {
            var savePanel = NSSavePanel.SavePanel;
            savePanel.Title = AppStrings.Ui.EditorSaveDialogTitle;
            savePanel.NameFieldStringValue = string.IsNullOrWhiteSpace(_currentFilePath)
                ? AppStrings.Ui.EditorSuggestedSaveName
                : Path.GetFileName(_currentFilePath);
            var response = (nint)savePanel.RunModal();
            var selectedPath = savePanel.Url?.Path;

            if (response != (nint)NSModalResponse.OK || string.IsNullOrWhiteSpace(selectedPath))
            {
                return;
            }

            await SaveToPathAsync(selectedPath).ConfigureAwait(true);
        }

        private async Task SaveToPathAsync(string filePath)
        {
            await File.WriteAllTextAsync(filePath, _textView.Value ?? string.Empty).ConfigureAwait(true);
            _currentFilePath = filePath;
            _canSaveDirectly = SupportsDirectEditing(filePath);
            _pathLabel.StringValue = filePath;
            _statusLabel.StringValue = AppStrings.Ui.EditorSavedStatus;
        }

        private void ToggleWrap()
        {
            _wrapEnabled = !_wrapEnabled;
            ApplyWrapMode();
            _statusLabel.StringValue = _wrapEnabled
                ? AppStrings.Ui.NativeEditorWrapEnabledStatus
                : AppStrings.Ui.NativeEditorWrapDisabledStatus;
        }

        private void ApplyWrapMode()
        {
            var textContainer = _textView.TextContainer;
            if (textContainer is null)
            {
                return;
            }

            textContainer.WidthTracksTextView = _wrapEnabled;
            textContainer.Size = _wrapEnabled
                ? new CGSize(1240, NFloat.MaxValue)
                : new CGSize(NFloat.MaxValue, NFloat.MaxValue);
        }

        private void ShowFontPanel()
        {
            _window.MakeFirstResponder(_textView);
            NSFontManager.SharedFontManager.OrderFrontFontPanel(_textView);
        }

        private void AdjustFontSize(NFloat delta)
        {
            var currentFont = _textView.Font ?? GetFixedPitchFont(14);
            var nextSize = Math.Max(8, (double)currentFont.PointSize + delta);
            _textView.Font = GetFixedPitchFont((NFloat)nextSize);
            UpdateCursorStatus();
        }

        private void PerformSelector(string selectorName)
        {
            _window.MakeFirstResponder(_textView);
            _textView.PerformSelector(new Selector(selectorName));
        }

        private void ApplyBulletList()
        {
            var selection = _textView.SelectedRange;
            var updatedContent = ToggleBulletList(_textView.Value ?? string.Empty, selection);
            _textView.Value = updatedContent;
            UpdateCursorStatus();
        }

        private void ChangeCase(bool useUpperCase)
        {
            var selection = _textView.SelectedRange;
            _textView.Value = ChangeTextCase(_textView.Value ?? string.Empty, selection, useUpperCase);
            UpdateCursorStatus();
        }

        private async Task ShowPreviewAsync()
        {
            await _printWorkflowService
                .ShowPrintPreviewAsync(_owner, _textView.Value ?? string.Empty, _currentFilePath)
                .ConfigureAwait(true);
        }

        private async Task PrintAsync()
        {
            await _printWorkflowService
                .PrintAsync(_textView.Value ?? string.Empty, _currentFilePath)
                .ConfigureAwait(true);
        }

        private void HandleColorChanged(object? sender, EventArgs e)
        {
            _textView.TextColor = _colorWell.Color;
        }

        private void UpdateCursorStatus()
        {
            var content = _textView.Value ?? string.Empty;
            var selection = _textView.SelectedRange;
            var boundedLocation = (int)Math.Clamp(selection.Location, 0, content.Length);
            var line = 1;
            var column = 1;

            for (var index = 0; index < boundedLocation; index++)
            {
                if (content[index] == AppStrings.System.NewLine[0])
                {
                    line++;
                    column = 1;
                }
                else
                {
                    column++;
                }
            }

            _cursorLabel.StringValue = BuildCursorStatus(line, column);
        }

        private string BuildCursorStatus(int line, int column)
        {
            var builder = new StringBuilder();
            builder.Append(AppStrings.Ui.NativeEditorCursorStatusPrefix);
            builder.Append(line);
            builder.Append(AppStrings.Ui.NativeEditorColumnSeparator);
            builder.Append(column);
            return builder.ToString();
        }

        private static NSFont GetFixedPitchFont(NFloat pointSize)
        {
            return NSFont.UserFixedPitchFontOfSize(pointSize)
                ?? NSFont.SystemFontOfSize(pointSize)
                ?? throw new InvalidOperationException(AppStrings.Messages.ExternalOpenFailed);
        }

        private static void AddButton(
            NSView rootView,
            string title,
            NFloat x,
            NFloat y,
            NFloat width,
            NFloat height,
            EventHandler handler)
        {
            var button = new NSButton(new CGRect(x, y, width, height))
            {
                Title = title,
            };
            button.Activated += handler;
            rootView.AddSubview(button);
        }

        private static NSTextField CreateLabel(CGRect frame, string value)
        {
            return new NSTextField(frame)
            {
                Editable = false,
                Selectable = false,
                Bordered = false,
                DrawsBackground = false,
                StringValue = value,
            };
        }
    }
}

[ExcludeFromCodeCoverage]
internal sealed class AppKitPrintWorkflowService : IPrintWorkflowService
{
    private readonly IPrintService _printService;
    private readonly IPrintWorkflowService _fallbackWorkflowService;

    public AppKitPrintWorkflowService(
        IPrintService printService,
        IPrintWorkflowService fallbackWorkflowService)
    {
        _printService = printService;
        _fallbackWorkflowService = fallbackWorkflowService;
    }

    public async Task ShowPrintPreviewAsync(
        Window owner,
        string content,
        string? sourceFilePath,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!OperatingSystem.IsMacOS())
        {
            await _fallbackWorkflowService.ShowPrintPreviewAsync(owner, content, sourceFilePath, cancellationToken).ConfigureAwait(true);
            return;
        }

        try
        {
            RunPrintOperation(content, sourceFilePath);
        }
        catch
        {
            await _fallbackWorkflowService.ShowPrintPreviewAsync(owner, content, sourceFilePath, cancellationToken).ConfigureAwait(true);
        }
    }

    public async Task PrintAsync(
        string content,
        string? sourceFilePath,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!OperatingSystem.IsMacOS())
        {
            await _fallbackWorkflowService.PrintAsync(content, sourceFilePath, cancellationToken).ConfigureAwait(false);
            return;
        }

        try
        {
            RunPrintOperation(content, sourceFilePath);
        }
        catch
        {
            if (!string.IsNullOrWhiteSpace(sourceFilePath) && File.Exists(sourceFilePath))
            {
                await _printService.PrintFileAsync(sourceFilePath, cancellationToken).ConfigureAwait(false);
                return;
            }

            await _printService.PrintTextAsync(content, cancellationToken).ConfigureAwait(false);
        }
    }

    internal static bool CanUseSourceFile(string? sourceFilePath)
    {
        return !string.IsNullOrWhiteSpace(sourceFilePath)
            && File.Exists(sourceFilePath)
            && AppKitRichTextEditorService.SupportsDirectEditing(sourceFilePath);
    }

    private static void RunPrintOperation(string content, string? sourceFilePath)
    {
        var printableContent = CanUseSourceFile(sourceFilePath)
            ? File.ReadAllText(sourceFilePath!)
            : content;

        var textView = new NSTextView(new CGRect(0, 0, 640, 900))
        {
            Editable = false,
            Selectable = true,
            RichText = false,
            Font = AppKitRichTextEditorService.GetFixedPitchFont(13),
            Value = printableContent,
        };

        var printOperation = NSPrintOperation.FromView(textView);
        printOperation.ShowsPrintPanel = true;
        if (!printOperation.RunOperation())
        {
            throw new InvalidOperationException(AppStrings.Messages.ExternalOpenFailed);
        }
    }
}

#pragma warning restore CA1422
#pragma warning restore CA1416
