using SkiaSharp;

namespace Blueprints;

public class BlueprintRenderer(IBlueprintEditor editor)
{
    private readonly DrawingContext dc = new();

    public void Render(SKCanvas canvas, float dpi)
    {
        dc.Canvas = canvas;

        dc.PushTransform(SKMatrix.CreateScale(dpi, dpi));

        dc.Clear(editor.Style.Background);

        GridLines(editor.Style.MinorLineSpacing * editor.Zoom,
                  editor.Style.MinorLineWidth * editor.Zoom,
                  editor.Style.MinorLineColor);

        GridLines(editor.Style.MajorLineSpacing * editor.Zoom,
                  editor.Style.MajorLineWidth * editor.Zoom,
                  editor.Style.MajorLineColor);

        dc.PushTransform(SKMatrix.CreateScale(editor.Zoom, editor.Zoom).PostConcat(SKMatrix.CreateTranslation(editor.X, editor.Y)));

        foreach (Node node in editor.Nodes)
        {
            node.Layout(dc, 0, 0);
            node.Render(dc);
        }

        dc.Pop();

        dc.Pop();
    }

    private void GridLines(float spacing, float lineWidth, SKColor color)
    {
        for (float x = editor.X % spacing; x < editor.Width; x += spacing)
        {
            dc.DrawLine(new(x, 0), new(x, editor.Height), lineWidth, color);
        }

        for (float y = editor.Y % spacing; y < editor.Height; y += spacing)
        {
            dc.DrawLine(new(0, y), new(editor.Width, y), lineWidth, color);
        }
    }
}
