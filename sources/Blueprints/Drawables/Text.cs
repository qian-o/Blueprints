using SkiaSharp;

namespace Blueprints;

public class Text(string text) : Drawable
{
    public string? FontFamily { get; set; }

    public float FontWeight { get; set; } = 400;

    public float FontSize { get; set; } = 16;

    public SKColor? Color { get; set; }

    protected override SKSize OnMeasure(IDrawingContext dc)
    {
        return FontFamily is null ? dc.MeasureText(text, FontWeight, FontSize) : dc.MeasureText(text, FontFamily, FontWeight, FontSize);
    }

    protected override void OnRender(IDrawingContext dc)
    {
        if (FontFamily is null)
        {
            dc.DrawText(text, Bounds.Location, FontWeight, FontSize, Color ?? Theme.TextColor);
        }
        else
        {
            dc.DrawText(text, Bounds.Location, FontFamily, FontWeight, FontSize, Color ?? Theme.TextColor);
        }
    }
}
