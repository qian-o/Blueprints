using SkiaSharp;

namespace Blueprints;

public class Text(string text) : Drawable
{
    public string? FontFamily { get; set; }

    public float FontWeight { get; set; } = 400;

    public float FontSize { get; set; } = 16;

    public SKColor? Color { get; set; }

    protected override void OnInitialize()
    {
        if (Editor is null)
        {
            return;
        }

        FontFamily ??= Editor.FontFamily;
        Color ??= Editor.Theme.TextColor;
    }

    protected override SKSize OnMeasure(IDrawingContext dc)
    {
        return dc.MeasureText(text, FontFamily!, FontWeight, FontSize);
    }

    protected override void OnRender(IDrawingContext dc)
    {
        dc.DrawText(text, Bounds.Location, FontFamily!, FontWeight, FontSize, Color!.Value);
    }
}
