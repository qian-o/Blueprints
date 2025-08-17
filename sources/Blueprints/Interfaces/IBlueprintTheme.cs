using SkiaSharp;

namespace Blueprints;

public interface IBlueprintTheme
{
    ThemeMode Mode { get; set; }

    SKColor TextColor { get; }

    SKColor BackgroundColor { get; }

    SKColor MinorGridLineColor { get; }

    SKColor MajorGridLineColor { get; }

    SKColor CardBackgroundColor { get; }

    SKColor CardBorderColor { get; }

    float CardBorderWidth { get; }

    float CardCornerRadius { get; }

    float CardPadding { get; }

    SKColor PinColor { get; }

    float PinSize { get; }
}
