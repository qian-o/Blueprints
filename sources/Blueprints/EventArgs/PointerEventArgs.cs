using SkiaSharp;

namespace Blueprints;

public class PointerEventArgs(SKPoint position, Pointers pointers)
{
    public SKPoint Position { get; } = position;

    public Pointers Pointers { get; } = pointers;

    public bool Handled { get; set; }
}
