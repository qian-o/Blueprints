using SkiaSharp;

namespace Blueprints.NET;

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

    SKColor PinHoverColor { get; }

    float PinShapeSize { get; }

    float PinShapeStrokeWidth { get; }

    float PinPadding { get; }

    SKColor ConnectionColor { get; }

    SKColor ConnectionHoverColor { get; }

    float ConnectionWidth { get; }

    float ConnectionHoverWidth { get; }
}
