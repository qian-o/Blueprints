using SkiaSharp;

namespace Blueprints;

public interface IBlueprintStyle
{
    #region Default Style Properties
    string FontFamily { get; }

    SKColor Foreground { get; }
    #endregion

    #region Grid Style Properties
    GridLine MinorGridLine { get; }

    GridLine MajorGridLine { get; }
    #endregion
}
