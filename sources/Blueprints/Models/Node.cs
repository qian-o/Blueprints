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

    protected override Element[] GetSubElements()
    {
        List<Element> children = [];

        if (Title is not null)
        {
            children.Add(Title);
        }

        if (Content is not null)
        {
            children.Add(Content);
        }

        children.AddRange(Inputs);
        children.AddRange(Outputs);

        return [.. children];
    }

    protected override SKSize OnMeasure(IDrawingContext dc)
    {
        return new(100, 100);
    }

    protected override void OnArrange(SKSize finalSize)
    {
    }

    protected override void OnRender(IDrawingContext dc)
    {
        dc.DrawRoundRectangle(new(Bounds, 10), SKColors.Red);
    }
}
