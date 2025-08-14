using SkiaSharp;

namespace Blueprints;

public class Pin : Element
{
    public IDrawable? Header { get; set; }

    protected override Element[] Children()
    {
        return [];
    }

    protected override SKSize OnLayout(IDrawingContext dc)
    {
        return new(20, 20);
    }

    protected override void OnRender(IDrawingContext dc)
    {
        dc.DrawEllipse(Bounds, SKColors.Blue);
    }
}
