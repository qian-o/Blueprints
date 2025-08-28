using SkiaSharp;

namespace Blueprints;

public class PointerEventArgs(SKPoint screenPosition, SKPoint worldPosition, Modifiers modifiers, Pointers pointers) : EventArgs
{
    public SKPoint ScreenPosition { get; } = screenPosition;

    public SKPoint WorldPosition { get; } = worldPosition;

    public Modifiers Modifiers { get; } = modifiers;

    public Pointers Pointers { get; } = pointers;

    public bool Handled { get; set; }
}
