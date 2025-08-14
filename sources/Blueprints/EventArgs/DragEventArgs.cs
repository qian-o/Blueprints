using SkiaSharp;

namespace Blueprints;

public class DragEventArgs(SKPoint position)
{
    public SKPoint Position { get; } = position;

    public object? Data { get; }

    public bool Handled { get; set; }
}
