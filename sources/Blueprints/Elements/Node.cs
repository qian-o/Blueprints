using SkiaSharp;

namespace Blueprints;

public class Node : Element
{
    public Drawable? Header { get; set; }

    public Pin[] Inputs { get; set; } = [];

    public Pin[] Outputs { get; set; } = [];

    public Drawable? Content { get; set; }

    protected override Element[] SubElements()
    {
        return [.. Inputs, .. Outputs];
    }

    protected override Drawable[] SubDrawables()
    {
        List<Drawable> drawables = [];

        if (Header is not null)
        {
            drawables.Add(Header);
        }

        if (Content is not null)
        {
            drawables.Add(Content);
        }

        return [.. drawables];
    }

    protected override SKSize OnMeasure(IDrawingContext dc)
    {
        return new(100, 50);
    }

    protected override void OnArrange()
    {
    }

    protected override void OnRender(IDrawingContext dc)
    {
        dc.DrawEllipse(Bounds, SKColors.Red);
    }
}
