using SkiaSharp;

namespace Blueprints;

public class PointerEventArgs(SKPoint position, PointerFlags pointers)
{
    public SKPoint Position { get; } = position;

    public PointerFlags Pointers { get; } = pointers;

    public bool Handled { get; set; }
}
