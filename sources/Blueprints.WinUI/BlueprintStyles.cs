using SkiaSharp;

namespace Blueprints.WinUI;

internal class BlueprintStyles : IBlueprintStyles
{
    public SKColor BackgroundColor { get; set; }

    public SKColor MinorLineColor { get; set; }

    public SKColor MajorLineColor { get; set; }

    public void Flush()
    {
        BackgroundColor = SKColors.White;
        MinorLineColor = SKColors.LightGray;
        MajorLineColor = SKColors.Gray;
    }
}
