using SkiaSharp;

namespace Blueprints.WinUI;

public class BlueprintStyle : IBlueprintStyle
{
    public SKColor Background { get; } = SKColors.White;

    public SKColor MinorLineColor { get; } = SKColors.LightGray;

    public SKColor MajorLineColor { get; } = SKColors.Gray;

    public float MinorLineWidth { get; } = 1.0f;

    public float MajorLineWidth { get; } = 2.0f;

    public float MinorLineSpacing { get; } = 20.0f;

    public float MajorLineSpacing { get; } = 120.0f;
}
