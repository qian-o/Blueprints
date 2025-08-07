using SkiaSharp;

namespace Blueprints;

public interface IBlueprintOverlay
{
    SKSize Measure(object overlay);

    void Render(object overlay, float x, float y);

    void Destroy(object overlay);
}
