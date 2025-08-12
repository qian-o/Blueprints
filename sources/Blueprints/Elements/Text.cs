using SkiaSharp;

namespace Blueprints;

public class Text : Element
{
    public string Content { get; set; } = string.Empty;

    public static implicit operator Text(string content)
    {
        return new() { Content = content };
    }

    public static implicit operator string(Text text)
    {
        return text.Content;
    }

    protected override Element[] GetSubElements()
    {
        return [];
    }

    protected override SKSize OnMeasure(IDrawingContext dc)
    {
        return dc.MeasureText(Content, FontFamily, FontWeight, FontSize);
    }

    protected override void OnArrange()
    {
    }

    protected override void OnRender(IDrawingContext dc)
    {
        dc.DrawText(Content, ContentBounds.Location, FontFamily, FontWeight, FontSize, Foreground);
    }
}
