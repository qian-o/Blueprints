using SkiaSharp;

namespace Blueprints;

public static class BlueprintRenderer
{
    private static readonly Dictionary<IBlueprintEditor, IBlueprintDrawingContext> drawingContexts = [];

    public static void Render(IBlueprintEditor editor, SKCanvas canvas)
    {
        editor.Styles.Update();

        if (!drawingContexts.TryGetValue(editor, out IBlueprintDrawingContext? dc))
        {
            drawingContexts[editor] = dc = new BlueprintDrawingContext();
        }

        ((BlueprintDrawingContext)dc).Canvas = canvas;

        dc.Clear(editor.Styles.BackgroundColor);

        Grid(editor, dc);

#if DEBUG
        Debug(editor, dc);
#endif
    }

    private static void Grid(IBlueprintEditor editor, IBlueprintDrawingContext dc)
    {
        SKColor minorLineColor = editor.Styles.MinorLineColor;
        SKColor majorLineColor = editor.Styles.MajorLineColor;

        float minorLineWidth = editor.Styles.MinorLineWidth * editor.Zoom;
        float majorLineWidth = editor.Styles.MajorLineWidth * editor.Zoom;

        float minorLineSpacing = editor.Styles.MinorLineSpacing * editor.Zoom;
        float majorLineSpacing = editor.Styles.MajorLineSpacing * editor.Zoom;

        for (float x = editor.X % minorLineSpacing; x < editor.Width; x += minorLineSpacing)
        {
            dc.DrawLine(new(x, 0), new(x, editor.Height), minorLineWidth, minorLineColor);
        }

        for (float y = editor.Y % minorLineSpacing; y < editor.Height; y += minorLineSpacing)
        {
            dc.DrawLine(new(0, y), new(editor.Width, y), minorLineWidth, minorLineColor);
        }

        for (float x = editor.X % majorLineSpacing; x < editor.Width; x += majorLineSpacing)
        {
            dc.DrawLine(new(x, 0), new(x, editor.Height), majorLineWidth, majorLineColor);
        }

        for (float y = editor.Y % majorLineSpacing; y < editor.Height; y += majorLineSpacing)
        {
            dc.DrawLine(new(0, y), new(editor.Width, y), majorLineWidth, majorLineColor);
        }
    }

    private static void Debug(IBlueprintEditor editor, IBlueprintDrawingContext dc)
    {
        const float margin = 10.0f;

        string text = "Debug Info\n"
                      + $"Zoom: {editor.Zoom:F4}\n"
                      + $"X: {editor.X:F4}\n"
                      + $"Y: {editor.Y:F4}\n"
                      + $"Width: {editor.Width:F4}\n"
                      + $"Height: {editor.Height:F4}";

        SKSize rectSize = dc.MeasureText(text, editor.Styles.FontFamily, editor.Styles.TextSize) + new SKSize(margin * 2, margin * 2);

        dc.DrawRoundRectangle(new(new(2, 2, rectSize.Width, rectSize.Height), margin),
                              SKColors.Transparent,
                              1.0f,
                              editor.Styles.ForegroundColor);

        dc.DrawText(text,
                    new SKPoint(margin + 2, margin + 2),
                    editor.Styles.FontFamily,
                    editor.Styles.TextSize,
                    editor.Styles.ForegroundColor);
    }
}
