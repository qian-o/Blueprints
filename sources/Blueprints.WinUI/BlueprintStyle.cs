using SkiaSharp;

namespace Blueprints.WinUI;

internal class BlueprintStyle : IBlueprintStyle
{
    public string FontFamily { get; set; } = "Segoe UI";

    public SKColor BackgroundColor { get; set; } = SKColors.White;

    public SKColor ForegroundColor { get; set; } = SKColors.Black;

    public SKColor MinorLineColor { get; set; } = SKColors.LightGray;

    public SKColor MajorLineColor { get; set; } = SKColors.Gray;

    public float MinorLineWidth { get; set; } = 0.5f;

    public float MajorLineWidth { get; set; } = 1.0f;

    public float MinorLineSpacing { get; set; } = 20.0f;

    public float MajorLineSpacing { get; set; } = 120.0f;

    public float TextSize { get; set; } = 16.0f;

    public void Update()
    {
        FontFamily = "Segoe UI";
        BackgroundColor = SKColors.White;
        ForegroundColor = SKColors.Black;
        MinorLineColor = SKColors.LightGray;
        MajorLineColor = SKColors.Gray;
        MinorLineWidth = 0.5f;
        MajorLineWidth = 1.0f;
        MinorLineSpacing = 20.0f;
        MajorLineSpacing = 120.0f;
        TextSize = 16.0f;
    }
}
