using SkiaSharp;

namespace Blueprints;

public class Node : Element
{
    public float X { get; set; }

    public float Y { get; set; }

    public Element? Title { get; set; }

    public Element? Content { get; set; }

    public Pin[] Inputs { get; set; } = [];

    public Pin[] Outputs { get; set; } = [];

    public override void Layout(IDrawingContext dc, float offsetX, float offsetY)
    {
        Bounds = SKRect.Create(offsetX + X, offsetY + Y, 100, 100);
    }

    public override void Render(IDrawingContext dc)
    {
        dc.DrawRoundRectangle(new(Bounds, 10), SKColors.Red, 0, SKColors.Transparent);
    }

    public override void PointerPressed(PointerEventArgs args)
    {
        args.Handled = true;
    }
}
