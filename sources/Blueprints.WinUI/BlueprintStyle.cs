using SkiaSharp;

namespace Blueprints.WinUI;

public class BlueprintStyle : IBlueprintStyle
{
    public string FontFamily { get; } = "Segoe UI";

    public SKColor Background { get; } = SKColors.White;

    public SKColor Foreground { get; } = SKColors.Black;

    public SKColor Stroke { get; } = SKColors.Black;

    public GridLine MinorGridLine { get; } = new(SKColors.LightGray, 1.0f, 20.0f);

    public GridLine MajorGridLine { get; } = new(SKColors.Gray, 2.0f, 120.0f);
}
