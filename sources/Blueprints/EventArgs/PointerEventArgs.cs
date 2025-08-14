using SkiaSharp;

namespace Blueprints;

public class PointerEventArgs(SKPoint screenPosition, SKPoint worldPosition, Pointers pointers)
{
    public SKPoint ScreenPosition { get; } = screenPosition;

    public SKPoint WorldPosition { get; } = worldPosition;

    public Pointers Pointers { get; } = pointers;

    public bool Handled { get; set; }
}
