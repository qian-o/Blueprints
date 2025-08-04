using SkiaSharp;

namespace Blueprints;

public static class BlueprintRenderer
{
    public static void Render(IBlueprintEditor editor, SKCanvas canvas)
    {
        IBlueprintStyles styles = editor.Styles;

        styles.Flush();

        canvas.Clear(styles.BackgroundColor);
    }
}
