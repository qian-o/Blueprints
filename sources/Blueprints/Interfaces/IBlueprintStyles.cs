using SkiaSharp;

namespace Blueprints;

public interface IBlueprintStyles
{
    SKColor BackgroundColor { get; }

    SKColor MinorLineColor { get; }

    SKColor MajorLineColor { get; }

    float MinorLineWidth { get; }

    float MajorLineWidth { get; }

    float MinorLineSpacing { get; }

    float MajorLineSpacing { get; }

    void Flush();
}
