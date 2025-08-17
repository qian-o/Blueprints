using SkiaSharp;

namespace Blueprints;

public class BlueprintRenderer(IBlueprintEditor editor)
{
    private readonly DrawingContext dc = new(editor);

    public void Render(SKCanvas canvas, float dpi)
    {
        dc.Canvas = canvas;

        dc.PushTransform(SKMatrix.CreateScale(dpi, dpi));
        {
            dc.Clear(editor.Theme.BackgroundColor);

            GridLines(editor.Theme.MinorGridLineColor, 1.0f, 40.0f);
            GridLines(editor.Theme.MajorGridLineColor, 2.0f, 160.0f);

            dc.PushTransform(SKMatrix.CreateScale(editor.Zoom, editor.Zoom).PostConcat(SKMatrix.CreateTranslation(editor.X, editor.Y)));
            {
                foreach (Element element in editor.Elements)
                {
                    element.Bind(editor, null);
                    element.Layout(dc);
                    element.Render(dc);
                }
            }
            dc.Pop();
        }
        dc.Pop();
    }

    private void GridLines(SKColor color, float width, float spacing)
    {
        if (color.Alpha is 0)
        {
            return;
        }

        SKSize extent = editor.Extent;

        width *= editor.Zoom;
        spacing *= editor.Zoom;

        for (float x = editor.X % spacing; x < extent.Width; x += spacing)
        {
            dc.DrawLine(new(x, 0), new(x, extent.Height), color, width);
        }

        for (float y = editor.Y % spacing; y < extent.Height; y += spacing)
        {
            dc.DrawLine(new(0, y), new(extent.Width, y), color, width);
        }
    }
}
