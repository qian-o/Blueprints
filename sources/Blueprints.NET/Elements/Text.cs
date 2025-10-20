using SkiaSharp;

namespace Blueprints.NET;

public class Text(string text) : Drawable
{
    public string? FontFamily { get; set => Set(ref field, value, true); }

    public float FontWeight { get; set => Set(ref field, value, true); } = 400;

    public float FontSize { get; set => Set(ref field, value, true); } = 16;

    public SKColor? Color { get; set => Set(ref field, value, false); }

    protected override void OnInitialize()
    {
    }

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
