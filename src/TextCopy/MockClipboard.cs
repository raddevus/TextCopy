using System.Threading;

namespace TextCopy;

/// <inheritdoc />
public class MockClipboard : IClipboard
{
    /// <inheritdoc />
    public virtual Task<string?> GetTextAsync(CancellationToken cancellation = default)
=>  Task.FromResult<string?>(null);

    /// <inheritdoc />
    public virtual string? GetText()
=> null;

    /// <inheritdoc />
    public virtual Task SetTextAsync(string text, CancellationToken cancellation = default)
=> Task.CompletedTask;

    /// <inheritdoc />
    public void SetText(string text)
    {
    }
}
