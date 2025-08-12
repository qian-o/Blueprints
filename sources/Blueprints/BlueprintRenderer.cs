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
                  editor.Style.MinorLineColor,
                  editor.Style.MinorLineWidth * editor.Zoom);

        GridLines(editor.Style.MajorLineSpacing * editor.Zoom,
                  editor.Style.MajorLineColor,
                  editor.Style.MajorLineWidth * editor.Zoom);

        dc.PushTransform(SKMatrix.CreateScale(editor.Zoom, editor.Zoom).PostConcat(SKMatrix.CreateTranslation(editor.X, editor.Y)));

        foreach (Node node in editor.Nodes)
        {
            node.Bind(editor);
            node.Render(dc, node.X, node.Y);
        }

        dc.Pop();

        dc.Pop();
    }

    private void GridLines(float spacing, SKColor color, float width)
    {
        for (float x = editor.X % spacing; x < editor.Width; x += spacing)
        {
            dc.DrawLine(new(x, 0), new(x, editor.Height), color, width);
        }

        for (float y = editor.Y % spacing; y < editor.Height; y += spacing)
        {
            dc.DrawLine(new(0, y), new(editor.Width, y), color, width);
        }
    }
}
