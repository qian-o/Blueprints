using SkiaSharp;

namespace Blueprints;

public interface IBlueprintStyle
{
    SKColor Background { get; }

    SKColor MinorLineColor { get; }

    SKColor MajorLineColor { get; }

    float MinorLineWidth { get; }

    float MajorLineWidth { get; }

    float MinorLineSpacing { get; }

    float MajorLineSpacing { get; }
}
