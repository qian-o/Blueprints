using SkiaSharp;

namespace Blueprints;

public interface IDrawable
{
    SKSize Measure(IDrawingContext dc);

    void Render(IDrawingContext dc, SKRect bounds);
}
