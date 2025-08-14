using SkiaSharp;

namespace Blueprints;

public class DragEventArgs
{
    public SKPoint Position { get; set; }

    public object? Data { get; set; }

    public bool Handled { get; set; }
}
