using SkiaSharp;

namespace Blueprints;

public interface IBlueprintEditor
{
    IBlueprintStyle Style { get; }

    SKSize Extent { get; }

    float X { get; set; }

    float Y { get; set; }

    float Zoom { get; set; }

    void Invalidate();
}
