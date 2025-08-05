using SkiaSharp;

namespace Blueprints;

public static class BlueprintRenderer
{
    private static readonly Dictionary<IBlueprintEditor, IBlueprintDrawingContext> contexts = [];

    public static void Render(IBlueprintEditor editor, SKCanvas canvas)
    {
        editor.Styles.Update();

        if (!contexts.TryGetValue(editor, out IBlueprintDrawingContext? dc))
        {
            contexts[editor] = dc = new BlueprintDrawingContext();
        }

        ((BlueprintDrawingContext)dc).Canvas = canvas;

        dc.Clear(editor.Styles.BackgroundColor);

        dc.PushTransform(SKMatrix.CreateScale(editor.Dpi, editor.Dpi));

        Grid(editor, dc);

#if DEBUG
        Debug(editor, dc);
#endif

        dc.Pop();
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
                      + $"Zoom: {editor.Zoom:F2}\n"
                      + $"X: {editor.X:F2}\n"
                      + $"Y: {editor.Y:F2}\n"
                      + $"Width: {editor.Width:F2}\n"
                      + $"Height: {editor.Height:F2}";

        SKSize rectSize = dc.MeasureText(text, editor.Styles.FontFamily, editor.Styles.TextSize) + new SKSize(margin * 2, margin * 2);

        dc.PushTransform(SKMatrix.CreateTranslation(2, 2));

        dc.DrawRoundRectangle(new(new(0, 0, rectSize.Width, rectSize.Height), margin),
                              SKColors.Transparent,
                              1.0f,
                              editor.Styles.ForegroundColor);

        dc.DrawText(text,
                    new SKPoint(margin, margin),
                    editor.Styles.FontFamily,
                    editor.Styles.TextSize,
                    editor.Styles.ForegroundColor);

        dc.Pop();
    }
}
