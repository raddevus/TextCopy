using System.Threading;
namespace TextCopy;

/// <inheritdoc />
public class Clipboard :
    IClipboard
{
    /// <inheritdoc />
    public virtual Task<string?> GetTextAsync(CancellationToken cancellation = default)
=>  ClipboardService.GetTextAsync(cancellation);

    /// <inheritdoc />
    public virtual string? GetText()
 =>      ClipboardService.GetText();

    /// <inheritdoc />
    public virtual Task SetTextAsync(string text, CancellationToken cancellation = default)
=>   ClipboardService.SetTextAsync(text, cancellation);

    /// <inheritdoc />
    public virtual void SetText(string text)
 =>        ClipboardService.SetText(text);
}
