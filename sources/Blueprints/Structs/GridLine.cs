using SkiaSharp;

namespace Blueprints;

public readonly struct GridLine(SKColor color, float width, float spacing)
{
    public SKColor Color { get; } = color;

    public float Width { get; } = width;

    public float Spacing { get; } = spacing;
}
