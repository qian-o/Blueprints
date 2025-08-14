using SkiaSharp;

namespace Blueprints;

public class Pin : Element
{
    public new SKPoint Position { get; internal set; }

    public Drawable? Header { get; set; }

    protected override Element[] Children()
    {
        return [];
    }

    protected override SKSize OnMeasure(IDrawingContext dc)
    {
        return new(20, 20);
    }

    protected override void OnArrange()
    {
    }

    protected override void OnRender(IDrawingContext dc)
    {
        dc.DrawEllipse(Bounds, SKColors.Blue);
    }
}
