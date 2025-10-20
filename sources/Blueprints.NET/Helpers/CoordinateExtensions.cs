using SkiaSharp;

namespace Blueprints;

public static class CoordinateExtensions
{
    public static SKPoint ToWorld(this SKPoint position, IBlueprintEditor editor)
    {
        return new SKPoint((position.X - editor.X) / editor.Zoom, (position.Y - editor.Y) / editor.Zoom);
    }

    public static SKSize ToWorld(this SKSize size, IBlueprintEditor editor)
    {
        return new SKSize(size.Width / editor.Zoom, size.Height / editor.Zoom);
    }

    public static SKRect ToWorld(this SKRect rect, IBlueprintEditor editor)
    {
        return SKRect.Create(rect.Location.ToWorld(editor), rect.Size.ToWorld(editor));
    }
}
