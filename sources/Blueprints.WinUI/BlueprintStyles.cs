using SkiaSharp;

namespace Blueprints.WinUI;

internal class BlueprintStyles : IBlueprintStyles
{
    public BlueprintStyles()
    {
        Flush();
    }

    public SKColor BackgroundColor { get; set; }

    public float MinorLineWidth { get; set; }

    public float MajorLineWidth { get; set; }

    public SKColor MinorLineColor { get; set; }

    public SKColor MajorLineColor { get; set; }

    public float MinorLineSpacing { get; set; }

    public float MajorLineSpacing { get; set; }

    public void Flush()
    {
        BackgroundColor = SKColors.White;
        MinorLineWidth = 0.5f;
        MajorLineWidth = 1.0f;
        MinorLineColor = SKColors.LightGray;
        MajorLineColor = SKColors.Gray;
        MinorLineSpacing = 20.0f;
        MajorLineSpacing = 120.0f;
    }
}
