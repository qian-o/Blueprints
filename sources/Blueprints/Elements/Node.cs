using SkiaSharp;

namespace Blueprints;

public class Node : Element
{
    public IDrawable? Header { get; set; }

    public Pin[] Inputs { get; set; } = [];

    public Pin[] Outputs { get; set; } = [];

    public IDrawable? Content { get; set; }

    protected override Element[] Children()
    {
        return [.. Inputs, .. Outputs];
    }

    protected override SKSize OnLayout(IDrawingContext dc)
    {
        return new(100, 50);
    }

    protected override void OnRender(IDrawingContext dc)
    {
        dc.DrawEllipse(Bounds, SKColors.Red);
    }
}
