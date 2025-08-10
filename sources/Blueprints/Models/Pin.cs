using SkiaSharp;

namespace Blueprints;

public class Pin : Element
{
    public PinShape Shape { get; set; } = PinShape.Circle;

    public Element? Content { get; set; }

    public override void Layout(IDrawingContext dc, float offsetX, float offsetY)
    {
        Bounds = SKRect.Create(offsetX, offsetX, 8, 8);
    }

    public override void Render(IDrawingContext dc)
    {
        dc.DrawEllipse(Bounds, SKColors.Red, 0, SKColors.Transparent);
    }

    protected override Element[] Children()
    {
        return Content is not null ? [Content] : [];
    }
}
