using SkiaSharp;

namespace Blueprints;

public static class BlueprintRenderer
{
    private static readonly Dictionary<IBlueprintEditor, IBlueprintDrawingContext> contexts = [];

    public static void Render(IBlueprintEditor editor, SKCanvas canvas)
    {
        editor.Style.Update();

        if (!contexts.TryGetValue(editor, out IBlueprintDrawingContext? dc))
        {
            contexts[editor] = dc = new BlueprintDrawingContext();
        }

        ((BlueprintDrawingContext)dc).Canvas = canvas;

        dc.Clear(editor.Style.BackgroundColor);

        dc.PushTransform(SKMatrix.CreateScale(editor.Dpi, editor.Dpi));

        Grid(editor, dc);

#if DEBUG
        Debug(editor, dc);
#endif

        dc.Pop();
    }

    private static void Grid(IBlueprintEditor editor, IBlueprintDrawingContext dc)
    {
        SKColor minorLineColor = editor.Style.MinorLineColor;
        SKColor majorLineColor = editor.Style.MajorLineColor;

        float minorLineWidth = editor.Style.MinorLineWidth * editor.Zoom;
        float majorLineWidth = editor.Style.MajorLineWidth * editor.Zoom;

        float minorLineSpacing = editor.Style.MinorLineSpacing * editor.Zoom;
        float majorLineSpacing = editor.Style.MajorLineSpacing * editor.Zoom;

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

        SKSize rectSize = dc.MeasureText(text, editor.Style.FontFamily, editor.Style.TextSize) + new SKSize(margin * 2, margin * 2);

        dc.PushTransform(SKMatrix.CreateTranslation(4, 4));

        dc.DrawRoundRectangle(new(new(0, 0, rectSize.Width, rectSize.Height), margin),
                              SKColors.Transparent,
                              1.0f,
                              editor.Style.ForegroundColor);

        dc.DrawText(text,
                    new SKPoint(margin, margin),
                    editor.Style.FontFamily,
                    editor.Style.TextSize,
                    editor.Style.ForegroundColor);

        dc.Pop();
    }
}
