using SkiaSharp;

namespace Blueprints;

public interface IBlueprintStyle
{
    string FontFamily { get; }

    SKColor TextColor { get; }

    SKColor HoverColor { get; }

    SKColor PressedColor { get; }

    SKColor AttentionColor { get; }

    SKColor MinorGridLineColor { get; }

    SKColor MajorGridLineColor { get; }
}
