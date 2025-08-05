using SkiaSharp;

namespace Blueprints;

public static class BlueprintRenderer
{
    private static readonly SKPaint gridPaint = new()
    {
        IsAntialias = true,
        IsStroke = true
    };

    public static void Render(IBlueprintEditor editor, SKCanvas canvas)
    {
        editor.Styles.Flush();

        canvas.Clear(editor.Styles.BackgroundColor);

        Grid(editor, canvas);
    }

    private static void Grid(IBlueprintEditor editor, SKCanvas canvas)
    {
        SKColor minorLineColor = editor.Styles.MinorLineColor;
        SKColor majorLineColor = editor.Styles.MajorLineColor;

        float minorLineWidth = editor.Styles.MinorLineWidth * editor.Zoom;
        float majorLineWidth = editor.Styles.MajorLineWidth * editor.Zoom;

        float minorLineSpacing = editor.Styles.MinorLineSpacing * editor.Zoom;
        float majorLineSpacing = editor.Styles.MajorLineSpacing * editor.Zoom;

        // Draw minor grid lines
        gridPaint.Color = minorLineColor;
        gridPaint.StrokeWidth = minorLineWidth;

        for (float x = editor.X % minorLineSpacing; x < editor.Width; x += minorLineSpacing)
        {
            canvas.DrawLine(x, 0, x, editor.Height, gridPaint);
        }

        for (float y = editor.Y % minorLineSpacing; y < editor.Height; y += minorLineSpacing)
        {
            canvas.DrawLine(0, y, editor.Width, y, gridPaint);
        }

        // Draw major grid lines
        gridPaint.Color = majorLineColor;
        gridPaint.StrokeWidth = majorLineWidth;

        for (float x = editor.X % majorLineSpacing; x < editor.Width; x += majorLineSpacing)
        {
            canvas.DrawLine(x, 0, x, editor.Height, gridPaint);
        }

        for (float y = editor.Y % majorLineSpacing; y < editor.Height; y += majorLineSpacing)
        {
            canvas.DrawLine(0, y, editor.Width, y, gridPaint);
        }
    }
}
