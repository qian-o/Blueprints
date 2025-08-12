using SkiaSharp;

namespace Blueprints;

public interface IBlueprintStyle
{
    string FontFamily { get; }

    float FontSize { get; }

    SKColor Background { get; }

    SKColor Foreground { get; }

    SKColor MinorLineColor { get; }

    SKColor MajorLineColor { get; }

    float MinorLineWidth { get; }

    float MajorLineWidth { get; }

    float MinorLineSpacing { get; }

    float MajorLineSpacing { get; }
}
