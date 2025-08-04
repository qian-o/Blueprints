using SkiaSharp;

namespace Blueprints;

public static class BlueprintRenderer
{
    public static void Render(IBlueprintEditor editor, SKCanvas canvas)
    {
        IBlueprintStyles styles = editor.Styles;
        styles.Flush();

        canvas.Clear(styles.BackgroundColor);

        canvas.Save();
        canvas.SetMatrix(SKMatrix.CreateScale(editor.Zoom, editor.Zoom));
        {
            Grid(editor, canvas);
        }
        canvas.Restore();
    }

    private static void Grid(IBlueprintEditor editor, SKCanvas canvas)
    {
        IBlueprintStyles styles = editor.Styles;

        using SKPaint paint = new() { IsAntialias = true, IsStroke = true };

        // Draw minor grid lines
        paint.StrokeWidth = styles.MinorLineWidth;
        paint.Color = styles.MinorLineColor;

        for (float x = editor.X % styles.MinorLineSpacing; x < editor.Width; x += styles.MinorLineSpacing)
        {
            canvas.DrawLine(x, 0, x, editor.Height, paint);
        }

        for (float y = editor.Y % styles.MinorLineSpacing; y < editor.Height; y += styles.MinorLineSpacing)
        {
            canvas.DrawLine(0, y, editor.Width, y, paint);
        }

        // Draw major grid lines
        paint.StrokeWidth = styles.MajorLineWidth;
        paint.Color = styles.MajorLineColor;

        for (float x = editor.X % styles.MajorLineSpacing; x < editor.Width; x += styles.MajorLineSpacing)
        {
            canvas.DrawLine(x, 0, x, editor.Height, paint);
        }

        for (float y = editor.Y % styles.MajorLineSpacing; y < editor.Height; y += styles.MajorLineSpacing)
        {
            canvas.DrawLine(0, y, editor.Width, y, paint);
        }
    }
}
