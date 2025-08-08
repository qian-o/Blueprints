using SkiaSharp;

namespace Blueprints;

public static class BlueprintRenderer
{
    private static readonly Dictionary<IBlueprintEditor, IBlueprintDrawingContext> contexts = [];

    public static void Render(IBlueprintEditor editor, SKCanvas canvas, float dpi)
    {
        editor.Style.Update();

        if (!contexts.TryGetValue(editor, out IBlueprintDrawingContext? dc))
        {
            contexts[editor] = dc = new BlueprintDrawingContext();
        }

        ((BlueprintDrawingContext)dc).Canvas = canvas;

        dc.PushTransform(SKMatrix.CreateScale(dpi, dpi));
        {
            dc.Clear(editor.Style.Background);

            Grid(editor, dc);

            dc.PushTransform(SKMatrix.CreateTranslation(editor.X, editor.Y));
            dc.PushTransform(SKMatrix.CreateScale(editor.Zoom, editor.Zoom));
            {
            }
            dc.Pop();
            dc.Pop();
        }
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
}
