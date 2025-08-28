using SkiaSharp;

namespace Blueprints;

public class PointerWheelEventArgs(SKPoint screenPosition, SKPoint worldPosition, float delta, KeyModifiers keyModifiers) : PointerEventArgs(screenPosition, worldPosition, keyModifiers, Pointers.None)
{
    public float Delta { get; } = delta;
}
