using SkiaSharp;

namespace Blueprints;

public class DefaultBlueprintTheme : IBlueprintTheme
{
    public SKColor TextColor { get; } = SKColors.Black;

    public SKColor MinorGridLineColor { get; } = SKColors.LightGray;

    public SKColor MajorGridLineColor { get; } = SKColors.Gray;
}
