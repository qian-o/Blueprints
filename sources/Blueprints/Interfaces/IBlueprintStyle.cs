using SkiaSharp;

namespace Blueprints;

public interface IBlueprintStyle
{
    string FontFamily { get; }

    SKColor Foreground { get; }

    GridLine MinorGridLine { get; }

    GridLine MajorGridLine { get; }

    SKColor NodeStroke { get; }

    SKColor NodeBackground { get; }

    Thickness NodePadding { get; }

    float NodeStrokeWidth { get; }

    float NodeCornerRadius { get; }

    SKColor Hover { get; }

    SKColor Pressed { get; }

    SKColor Attention { get; }
}
