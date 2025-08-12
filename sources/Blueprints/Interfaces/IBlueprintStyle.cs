using SkiaSharp;

namespace Blueprints;

public interface IBlueprintStyle
{
    string FontFamily { get; }

    SKColor Background { get; }

    SKColor Foreground { get; }

    GridLine MinorGridLine { get; }

    GridLine MajorGridLine { get; }
}
