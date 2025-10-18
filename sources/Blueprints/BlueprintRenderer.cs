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
                Node[] nodes = [.. editor.Nodes];

                SKRect viewBounds = SKRect.Create(editor.Extent).ToWorld(editor);

                List<SKRect> occluders = new(capacity: Math.Min(nodes.Length, 256));
                List<Node> visibles = new(capacity: Math.Min(nodes.Length, 256));

                for (int i = nodes.Length - 1; i >= 0; i--)
                {
                    Node node = nodes[i];
                    node.Layout(editor, dc);

                    if (!node.Bounds.IntersectsWith(viewBounds))
                    {
                        continue;
                    }

                    if (IsFullyCovered(node.Bounds, occluders))
                    {
                        continue;
                    }

                    occluders.Add(node.Bounds);
                    visibles.Add(node);
                }

                foreach (Connection connection in visibles.SelectMany(static item => item.Connections()).DistinctBy(static item => item.Id))
                {
                    connection.Render(dc);
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

        using SKPath path = new();

        for (float x = editor.X % spacing; x < extent.Width; x += spacing)
        {
            path.MoveTo(x, 0);
            path.LineTo(x, extent.Height);
        }

        for (float y = editor.Y % spacing; y < extent.Height; y += spacing)
        {
            path.MoveTo(0, y);
            path.LineTo(extent.Width, y);
        }

        dc.DrawPath(path, color, width);
    }
}
