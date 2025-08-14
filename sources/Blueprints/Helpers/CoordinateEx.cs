using SkiaSharp;

namespace Blueprints;

public static class CoordinateEx
{
    public static SKPoint ToWorld(this SKPoint position, IBlueprintEditor editor)
    {
        return new SKPoint((position.X - editor.X) / editor.Zoom, (position.Y - editor.Y) / editor.Zoom);
    }
}
