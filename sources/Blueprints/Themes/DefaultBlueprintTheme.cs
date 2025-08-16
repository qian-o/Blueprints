using SkiaSharp;

namespace Blueprints;

public class DefaultBlueprintTheme : IBlueprintTheme
{
    public SKColor TextColor { get; } = SKColors.White;

    public SKColor BackgroundColor { get; } = SKColors.Black;

    public SKColor MinorGridLineColor { get; } = SKColors.Gray;

    public SKColor MajorGridLineColor { get; } = SKColors.LightGray;

    public SKColor CardBackgroundColor { get; } = SKColors.DarkGray;

    public SKColor CardBorderColor { get; } = SKColors.LightGray;

    public float CardBorderWidth { get; } = 1.0f;

    public float CardCornerRadius { get; } = 8.0f;

    public float CardPadding { get; } = 10.0f;
}
