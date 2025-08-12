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

    protected override void OnArrange(SKRect rect)
    {
        if (Title is not null)
        {
            float x = rect.Left + Title.Margin.Left;
            float y = rect.Top + Title.Margin.Top;
            float width = Title.DesiredSize.Width;
            float height = Title.DesiredSize.Height;

            Title.Arrange(SKRect.Create(x, y, width, height));
        }
    }

    protected override void OnRender(IDrawingContext dc)
    {
    }
}
