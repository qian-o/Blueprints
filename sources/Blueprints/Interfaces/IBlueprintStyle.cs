using SkiaSharp;

namespace Blueprints;

public interface IBlueprintStyle
{
    #region Default Style Properties
    string FontFamily { get; }

    SKColor Background { get; }

    SKColor Foreground { get; }

    SKColor Stroke { get; }
    #endregion

    #region Grid Style Properties
    GridLine MinorGridLine { get; }

    GridLine MajorGridLine { get; }
    #endregion
}
