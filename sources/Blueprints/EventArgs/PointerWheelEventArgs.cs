using SkiaSharp;

namespace Blueprints;

public class PointerWheelEventArgs(SKPoint position, float delta) : PointerEventArgs(position, Pointers.None)
{
    public float Delta { get; } = delta;
}
