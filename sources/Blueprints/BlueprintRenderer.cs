using System.Diagnostics;
using SkiaSharp;

namespace Blueprints;

public class BlueprintRenderer(IBlueprintEditor editor)
{
    private readonly DrawingContext dc = new(editor);

    public void Render(SKCanvas canvas, float dpi)
    {
        dc.Canvas = canvas;

        Stopwatch stopwatch = Stopwatch.StartNew();

        dc.PushTransform(SKMatrix.CreateScale(dpi, dpi));
        {
            dc.Clear(editor.Theme.BackgroundColor);

            GridLines(editor.Theme.MinorGridLineColor, 1.0f, 40.0f);
            GridLines(editor.Theme.MajorGridLineColor, 2.0f, 160.0f);

            dc.PushTransform(SKMatrix.CreateScale(editor.Zoom, editor.Zoom).PostConcat(SKMatrix.CreateTranslation(editor.X, editor.Y)));
            {
                foreach (Element element in editor.Elements)
                {
                    element.Layout(editor, dc);
                }

                foreach (Connection connection in editor.Elements.OfType<Node>().SelectMany(static item => item.Connections()))
                {
                    connection.Render(dc);
                }

                SKRect viewBounds = SKRect.Create(editor.Extent).ToWorld(editor);

                Element[] elements = [.. editor.Elements];

                List<SKRect> occluders = new(capacity: Math.Min(elements.Length, 256));
                List<Element> visibles = new(capacity: Math.Min(elements.Length, 256));

                for (int i = elements.Length - 1; i >= 0; i--)
                {
                    Element element = elements[i];

                    if (!element.Bounds.IntersectsWith(viewBounds))
                    {
                        continue;
                    }

                    if (IsFullyCovered(element.Bounds, occluders))
                    {
                        continue;
                    }

                    occluders.Add(element.Bounds);
                    visibles.Add(element);
                }

                for (int i = visibles.Count - 1; i >= 0; i--)
                {
                    visibles[i].Render(dc);
                }

                static bool IsFullyCovered(SKRect targetBounds, List<SKRect> occluders)
                {
                    for (int i = 0; i < occluders.Count; i++)
                    {
                        if (occluders[i].Contains(targetBounds))
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }
            dc.Pop();
        }
        dc.Pop();

        stopwatch.Stop();

        Debug.WriteLine($"Render time: {stopwatch.ElapsedMilliseconds} ms");
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
