using SkiaSharp;

namespace Blueprints;

public class PointerWheelEventArgs(SKPoint position, float delta) : PointerEventArgs(position, PointerFlags.None)
{
    public float Delta { get; } = delta;
}
