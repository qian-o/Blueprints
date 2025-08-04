using SkiaSharp;
using SkiaSharp.Views.Windows;

namespace Blueprints.WinUI;

public sealed partial class BlueprintEditor : SKXamlCanvas, IBlueprintEditor
{
    public BlueprintEditor()
    {
        PaintSurface += OnPaintSurface;
    }

    float IBlueprintEditor.OffsetX { get; }

    float IBlueprintEditor.OffsetY { get; }

    float IBlueprintEditor.Zoom { get; }

    private void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        e.Surface.Canvas.Clear(SKColors.Red);
    }
}
