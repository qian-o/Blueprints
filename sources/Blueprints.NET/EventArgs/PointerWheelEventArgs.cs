using SkiaSharp;

namespace Blueprints;

public class PointerWheelEventArgs(SKPoint screenPosition, SKPoint worldPosition, float delta, Modifiers modifiers) : PointerEventArgs(screenPosition, worldPosition, modifiers, Pointers.None)
{
    public float Delta { get; } = delta;
}
