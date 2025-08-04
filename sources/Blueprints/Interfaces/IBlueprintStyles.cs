using SkiaSharp;

namespace Blueprints;

public interface IBlueprintStyles
{
    SKColor BackgroundColor { get; }

    SKColor MinorLineColor { get; }

    SKColor MajorLineColor { get; }

    void Flush();
}
