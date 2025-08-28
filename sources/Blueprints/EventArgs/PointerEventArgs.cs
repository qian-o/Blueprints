using SkiaSharp;

namespace Blueprints;

public class PointerEventArgs(SKPoint screenPosition, SKPoint worldPosition, KeyModifiers keyModifiers, Pointers pointers) : EventArgs
{
    public SKPoint ScreenPosition { get; } = screenPosition;

    public SKPoint WorldPosition { get; } = worldPosition;

    public KeyModifiers KeyModifiers { get; } = keyModifiers;

    public Pointers Pointers { get; } = pointers;

    public bool Handled { get; set; }
}
