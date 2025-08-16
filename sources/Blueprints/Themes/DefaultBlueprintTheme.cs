using SkiaSharp;

namespace Blueprints;

public class DefaultBlueprintTheme : IBlueprintTheme
{
    public SKColor TextColor { get; } = new SKColor(230, 230, 230);

    public SKColor BackgroundColor { get; } = new SKColor(30, 30, 30);

    public SKColor MinorGridLineColor { get; } = new SKColor(50, 50, 50);

    public SKColor MajorGridLineColor { get; } = new SKColor(70, 70, 70);

    public SKColor CardBackgroundColor { get; } = new SKColor(45, 45, 48);

    public SKColor CardBorderColor { get; } = new SKColor(100, 100, 100);

    public float CardBorderWidth { get; } = 1.0f;

    public float CardCornerRadius { get; } = 8.0f;

    public float CardPadding { get; } = 10.0f;
}
