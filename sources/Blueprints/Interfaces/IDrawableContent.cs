using SkiaSharp;

namespace Blueprints;

public interface IDrawableContent
{
    SKSize Size { get; }

    void Render(IDrawingContext dc, SKRect bounds);
}
