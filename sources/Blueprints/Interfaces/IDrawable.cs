using SkiaSharp;

namespace Blueprints;

public interface IDrawable
{
    SKRect Bounds { get; set; }

    SKSize Measure(IDrawingContext dc);

    void Render(IDrawingContext dc);
}
