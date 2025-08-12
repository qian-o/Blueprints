using SkiaSharp;

namespace Blueprints;

public readonly struct Thickness(float left, float top, float right, float bottom)
{
    public float Left { get; } = left;

    public float Top { get; } = top;

    public float Right { get; } = right;

    public float Bottom { get; } = bottom;

    public SKSize LeftTop => new(Left, Top);

    public SKSize RightBottom => new(Right, Bottom);

    public SKSize Size => LeftTop + RightBottom;

    public static implicit operator Thickness(float uniform)
    {
        return new Thickness(uniform, uniform, uniform, uniform);
    }
}
