using SkiaSharp;

namespace Blueprints;

public class PointerWheelEventArgs(SKPoint screenPosition, SKPoint worldPosition, float delta) : PointerEventArgs(screenPosition, worldPosition, Pointers.None)
{
    public float Delta { get; } = delta;
}
