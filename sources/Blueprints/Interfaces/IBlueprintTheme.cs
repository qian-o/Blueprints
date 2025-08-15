using SkiaSharp;

namespace Blueprints;

public interface IBlueprintTheme
{
    SKColor TextColor { get; }

    SKColor MinorGridLineColor { get; }

    SKColor MajorGridLineColor { get; }
}
