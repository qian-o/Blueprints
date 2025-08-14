using SkiaSharp;

namespace Blueprints;

public class Text(string text) : IDrawable
{
    public string? FontFamily { get; set; }

    public float FontWeight { get; set; } = 400;

    public float FontSize { get; set; } = 16;

    public SKColor? Color { get; set; }

    public void UseGlobalStyle(IBlueprintStyle style)
    {
        FontFamily ??= style.FontFamily;
        Color ??= style.TextColor;
    }

    public SKSize Measure(IDrawingContext dc)
    {
        return dc.MeasureText(text, FontFamily!, FontWeight, FontSize);
    }

    public void Render(IDrawingContext dc, SKRect bounds)
    {
        dc.DrawText(text, bounds.Location, FontFamily!, FontWeight, FontSize, Color!.Value);
    }

    public static implicit operator Text(string text)
    {
        return new Text(text);
    }
}
