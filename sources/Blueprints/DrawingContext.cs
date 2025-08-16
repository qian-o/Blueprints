using SkiaSharp;

namespace Blueprints;

internal class DrawingContext(Func<string, SKFontStyleWeight, SKTypeface> resolveTypeface) : IDrawingContext
{
    private readonly Dictionary<FontDescriptor, SKFont> fontCache = [];
    private readonly Dictionary<FillPaintDescriptor, SKPaint> fillPaintCache = [];
    private readonly Dictionary<StrokePaintDescriptor, SKPaint> strokePaintCache = [];
    private readonly Dictionary<TextPaintDescriptor, SKPaint> textPaintCache = [];

    public SKCanvas? Canvas { get; set; }

    public void PushClip(SKRect rect, float radius)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        Canvas.Save();

        if (radius is 0)
        {
            Canvas.ClipRect(rect, SKClipOperation.Intersect, true);
        }
        else
        {
            Canvas.ClipRoundRect(new(rect, radius), SKClipOperation.Intersect, true);
        }
    }

    public void PushClip(SKPath path)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        Canvas.Save();
        Canvas.ClipPath(path, SKClipOperation.Intersect, true);
    }

    public void PushTransform(SKMatrix matrix)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        Canvas.Save();
        Canvas.Concat(matrix);
    }

    public void Pop()
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        Canvas.Restore();
    }

    public void Clear(SKColor color)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        Canvas.Clear(color);
    }

    public void DrawImage(SKImage image, SKRect src, SKRect dest)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        Canvas.DrawImage(image, src, dest, new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));
    }

    public void DrawPoint(SKPoint point, SKColor color)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        Canvas.DrawPoint(point, GetFillPaint(color));
    }

    public void DrawLine(SKPoint start, SKPoint end, SKColor stroke, float strokeWidth)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        Canvas.DrawLine(start, end, GetStrokePaint(stroke, strokeWidth));
    }

    public void DrawRectangle(SKRect rect, float radius, SKColor fill)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        if (radius is 0)
        {
            Canvas.DrawRect(rect, GetFillPaint(fill));
        }
        else
        {
            Canvas.DrawRoundRect(new(rect, radius), GetFillPaint(fill));
        }
    }

    public void DrawRectangle(SKRect rect, float radius, SKColor stroke, float strokeWidth)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        if (radius is 0)
        {
            Canvas.DrawRect(rect, GetStrokePaint(stroke, strokeWidth));
        }
        else
        {
            Canvas.DrawRoundRect(new(rect, radius), GetStrokePaint(stroke, strokeWidth));
        }
    }

    public void DrawEllipse(SKRect rect, SKColor fill)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        Canvas.DrawOval(rect, GetFillPaint(fill));
    }

    public void DrawEllipse(SKRect rect, SKColor stroke, float strokeWidth)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        Canvas.DrawOval(rect, GetStrokePaint(stroke, strokeWidth));
    }

    public void DrawCircle(SKPoint center, float radius, SKColor fill)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        Canvas.DrawCircle(center, radius, GetFillPaint(fill));
    }

    public void DrawCircle(SKPoint center, float radius, SKColor stroke, float strokeWidth)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        Canvas.DrawCircle(center, radius, GetStrokePaint(stroke, strokeWidth));
    }

    public void DrawPath(SKPath path, SKColor fill)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        Canvas.DrawPath(path, GetFillPaint(fill));
    }

    public void DrawPath(SKPath path, SKColor stroke, float strokeWidth)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        Canvas.DrawPath(path, GetStrokePaint(stroke, strokeWidth));
    }

    public void DrawText(string text, SKPoint position, string fontFamily, float fontWeight, float fontSize, SKColor color)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        SKFont font = GetFont(fontFamily, fontWeight, fontSize);

        float y = -font.Metrics.Ascent;
        foreach (string line in text.Split('\n'))
        {
            Canvas.DrawText(line,
                            position.X,
                            position.Y + y,
                            GetFont(fontFamily, fontWeight, fontSize),
                            GetTextPaint(color));

            y += font.GetFontMetrics(out _);
        }
    }

    public SKSize MeasureText(string text, string fontFamily, float fontWeight, float fontSize)
    {
        SKFont font = GetFont(fontFamily, fontWeight, fontSize);

        string[] lines = text.Split('\n');

        float width = lines.Max(line => font.MeasureText(line, out _));
        float height = lines.Length * font.GetFontMetrics(out _);

        return new SKSize(width, height);
    }

    private SKFont GetFont(string fontFamily, float fontWeight, float fontSize)
    {
        fontWeight = Math.Clamp((int)fontWeight / 100 * 100, 0, 1000);

        FontDescriptor descriptor = new(fontFamily, fontWeight, fontSize);

        if (!fontCache.TryGetValue(descriptor, out SKFont? font))
        {
            fontCache[descriptor] = font = new(resolveTypeface(fontFamily, (SKFontStyleWeight)fontWeight), fontSize);
        }

        return font;
    }

    private SKPaint GetFillPaint(SKColor color)
    {
        FillPaintDescriptor descriptor = new(color);

        if (!fillPaintCache.TryGetValue(descriptor, out SKPaint? paint))
        {
            fillPaintCache[descriptor] = paint = new()
            {
                IsAntialias = true,
                IsDither = true,
                Style = SKPaintStyle.Fill,
                Color = color,
                BlendMode = SKBlendMode.SrcOver
            };
        }

        return paint;
    }

    private SKPaint GetStrokePaint(SKColor color, float width)
    {
        StrokePaintDescriptor descriptor = new(color, width);

        if (!strokePaintCache.TryGetValue(descriptor, out SKPaint? paint))
        {
            strokePaintCache[descriptor] = paint = new()
            {
                IsAntialias = true,
                IsDither = true,
                Style = SKPaintStyle.Stroke,
                Color = color,
                StrokeWidth = width,
                BlendMode = SKBlendMode.SrcOver
            };
        }

        return paint;
    }

    private SKPaint GetTextPaint(SKColor color)
    {
        TextPaintDescriptor descriptor = new(color);

        if (!textPaintCache.TryGetValue(descriptor, out SKPaint? paint))
        {
            textPaintCache[descriptor] = paint = new()
            {
                IsAntialias = true,
                IsDither = true,
                Style = SKPaintStyle.Fill,
                Color = color,
                BlendMode = SKBlendMode.SrcOver
            };
        }

        return paint;
    }

    private record FontDescriptor(string FontFamily, float FontWeight, float FontSize);

    private record FillPaintDescriptor(SKColor Color);

    private record StrokePaintDescriptor(SKColor Color, float Width);

    private record TextPaintDescriptor(SKColor Color);
}
