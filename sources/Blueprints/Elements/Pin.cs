using SkiaSharp;

namespace Blueprints;

public class Pin : Element
{
    public PinShape Shape { get; set; } = PinShape.Circle;

    public Element? Content { get; set; }

    protected override Element[] GetSubElements()
    {
        return Content is not null ? [Content] : [];
    }

    protected override SKSize OnMeasure(IDrawingContext dc)
    {
        return new SKSize(20, 20);
    }

    protected override void OnArrange()
    {
    }

    protected override void OnRender(IDrawingContext dc)
    {
    }
}
