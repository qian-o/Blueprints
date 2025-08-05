using SkiaSharp;

namespace Blueprints;

public interface IBlueprintStyles
{
    string FontFamily { get; }

    SKColor BackgroundColor { get; }

    SKColor ForegroundColor { get; }

    SKColor MinorLineColor { get; }

    SKColor MajorLineColor { get; }

    float MinorLineWidth { get; }

    float MajorLineWidth { get; }

    float MinorLineSpacing { get; }

    float MajorLineSpacing { get; }

    void Update();
}
