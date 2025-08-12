using SkiaSharp;

namespace Blueprints.WinUI;

public class BlueprintStyle : IBlueprintStyle
{
    public string FontFamily { get; } = "Segoe UI";

    public float FontSize { get; } = 16.0f;

    public SKColor Background { get; } = SKColors.White;

    public SKColor Foreground { get; } = SKColors.Black;

    public SKColor MinorLineColor { get; } = SKColors.LightGray;

    public SKColor MajorLineColor { get; } = SKColors.Gray;

    public float MinorLineWidth { get; } = 1.0f;

    public float MajorLineWidth { get; } = 2.0f;

    public float MinorLineSpacing { get; } = 20.0f;

    public float MajorLineSpacing { get; } = 120.0f;
}
