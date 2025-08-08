using SkiaSharp;

namespace Blueprints.WinUI;

internal class BlueprintStyle : IBlueprintStyle
{
    public SKColor Background { get; set; } = SKColors.White;

    public SKColor MinorLineColor { get; set; } = SKColors.LightGray;

    public SKColor MajorLineColor { get; set; } = SKColors.Gray;

    public float MinorLineWidth { get; set; } = 0.5f;

    public float MajorLineWidth { get; set; } = 1.0f;

    public float MinorLineSpacing { get; set; } = 20.0f;

    public float MajorLineSpacing { get; set; } = 120.0f;

    public void Update()
    {
        Background = SKColors.White;
        MinorLineColor = SKColors.LightGray;
        MajorLineColor = SKColors.Gray;
        MinorLineWidth = 0.5f;
        MajorLineWidth = 1.0f;
        MinorLineSpacing = 20.0f;
        MajorLineSpacing = 120.0f;
    }
}
