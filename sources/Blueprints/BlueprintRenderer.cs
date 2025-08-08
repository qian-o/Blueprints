using SkiaSharp;

namespace Blueprints;

public class BlueprintRenderer(IBlueprintEditor editor)
{
    private readonly DrawingContext dc = new();

    public void Render(SKCanvas canvas, float dpi)
    {
        dc.Canvas = canvas;

        dc.PushTransform(SKMatrix.CreateScale(dpi, dpi));
        {
            dc.Clear(editor.Style.Background);

            float minorLineWidth = editor.Style.MinorLineWidth * editor.Zoom;
            float majorLineWidth = editor.Style.MajorLineWidth * editor.Zoom;

            float minorLineSpacing = editor.Style.MinorLineSpacing * editor.Zoom;
            float majorLineSpacing = editor.Style.MajorLineSpacing * editor.Zoom;

            for (float x = editor.X % minorLineSpacing; x < editor.Width; x += minorLineSpacing)
            {
                dc.DrawLine(new(x, 0), new(x, editor.Height), minorLineWidth, editor.Style.MinorLineColor);
            }

            for (float y = editor.Y % minorLineSpacing; y < editor.Height; y += minorLineSpacing)
            {
                dc.DrawLine(new(0, y), new(editor.Width, y), minorLineWidth, editor.Style.MinorLineColor);
            }

            for (float x = editor.X % majorLineSpacing; x < editor.Width; x += majorLineSpacing)
            {
                dc.DrawLine(new(x, 0), new(x, editor.Height), majorLineWidth, editor.Style.MajorLineColor);
            }

            for (float y = editor.Y % majorLineSpacing; y < editor.Height; y += majorLineSpacing)
            {
                dc.DrawLine(new(0, y), new(editor.Width, y), majorLineWidth, editor.Style.MajorLineColor);
            }

            dc.PushTransform(SKMatrix.CreateTranslation(editor.X, editor.Y).PostConcat(SKMatrix.CreateScale(editor.Zoom, editor.Zoom)));
            {
            }
            dc.Pop();
        }
        dc.Pop();
    }
}
