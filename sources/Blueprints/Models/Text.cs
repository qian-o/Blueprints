using SkiaSharp;

namespace Blueprints;

public class Text : Element
{
    public string Content { get; set; } = string.Empty;

    public static implicit operator string(Text text)
    {
        return text.Content;
    }

    public static implicit operator Text(string content)
    {
        return new() { Content = content };
    }

    protected override Element[] GetSubElements()
    {
        return [];
    }

    protected override SKSize OnMeasure(IDrawingContext dc)
    {
        return SKSize.Empty;
    }

    protected override void OnArrange(SKSize finalSize)
    {
    }

    protected override void OnRender(IDrawingContext dc)
    {
    }
}
