using SkiaSharp;

namespace Blueprints;

public interface IBlueprintStyles
{
    SKColor BackgroundColor { get; }

    float MinorLineWidth { get; }

    float MajorLineWidth { get; }

    SKColor MinorLineColor { get; }

    SKColor MajorLineColor { get; }

    float MinorLineSpacing { get; }

    float MajorLineSpacing { get; }

    void Flush();
}
