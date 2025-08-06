using SkiaSharp;

namespace Blueprints;

internal class BlueprintDrawingContext : IBlueprintDrawingContext
{
    private readonly Dictionary<FontDescriptor, SKFont> fontCache = [];
    private readonly Dictionary<FillPaintDescriptor, SKPaint> fillPaintCache = [];
    private readonly Dictionary<StrokePaintDescriptor, SKPaint> strokePaintCache = [];
    private readonly Dictionary<TextPaintDescriptor, SKPaint> textPaintCache = [];

    public SKCanvas? Canvas { get; set; }

    public void PushClip(SKRect rect)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        Canvas.Save();
        Canvas.ClipRect(rect, SKClipOperation.Intersect, true);
    }

    public void PushClip(SKRoundRect roundRect)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        Canvas.Save();
        Canvas.ClipRoundRect(roundRect, SKClipOperation.Intersect, true);
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

    public void DrawLine(SKPoint start, SKPoint end, float strokeWidth, SKColor strokeColor)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        Canvas.DrawLine(start, end, GetStrokePaint(strokeColor, strokeWidth));
    }

    public void DrawRectangle(SKRect rect, SKColor fillColor, float strokeWidth, SKColor strokeColor)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        Canvas.DrawRect(rect, GetFillPaint(fillColor));

        if (strokeWidth > 0)
        {
            Canvas.DrawRect(rect, GetStrokePaint(strokeColor, strokeWidth));
        }
    }

    public void DrawRoundRectangle(SKRoundRect roundRect, SKColor fillColor, float strokeWidth, SKColor strokeColor)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        Canvas.DrawRoundRect(roundRect, GetFillPaint(fillColor));

        if (strokeWidth > 0)
        {
            Canvas.DrawRoundRect(roundRect, GetStrokePaint(strokeColor, strokeWidth));
        }
    }

    public void DrawEllipse(SKRect rect, SKColor fillColor, float strokeWidth, SKColor strokeColor)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        Canvas.DrawOval(rect, GetFillPaint(fillColor));

        if (strokeWidth > 0)
        {
            Canvas.DrawOval(rect, GetStrokePaint(strokeColor, strokeWidth));
        }
    }

    public void DrawPath(SKPath path, SKColor fillColor, float strokeWidth, SKColor strokeColor)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        Canvas.DrawPath(path, GetFillPaint(fillColor));

        if (strokeWidth > 0)
        {
            Canvas.DrawPath(path, GetStrokePaint(strokeColor, strokeWidth));
        }
    }

    public void DrawText(string text, SKPoint position, string fontFamily, float fontSize, SKColor color)
    {
        if (Canvas is null)
        {
            throw new InvalidOperationException("Canvas is not set.");
        }

        SKFont font = GetFont(fontFamily, fontSize);

        float y = -font.Metrics.Ascent;
        foreach (string line in text.Split('\n'))
        {
            Canvas.DrawText(line,
                            position.X,
                            position.Y + y,
                            GetFont(fontFamily, fontSize),
                            GetTextPaint(color));

            y += font.GetFontMetrics(out _);
        }
    }

    public SKSize MeasureText(string text, string fontFamily, float fontSize)
    {
        SKFont font = GetFont(fontFamily, fontSize);

        string[] lines = text.Split('\n');

        float width = lines.Max(line => font.MeasureText(line, out _));
        float height = lines.Length * font.GetFontMetrics(out _);

        return new SKSize(width, height);
    }

    private SKFont GetFont(string fontFamily, float fontSize)
    {
        FontDescriptor descriptor = new(fontFamily, fontSize);

        if (!fontCache.TryGetValue(descriptor, out SKFont? font))
        {
            fontCache[descriptor] = font = new(SKTypeface.FromFamilyName(fontFamily), fontSize);
        }

        return font;
    }

    private SKPaint GetFillPaint(SKColor color)
    {
        FillPaintDescriptor descriptor = new(color);

        if (!fillPaintCache.TryGetValue(descriptor, out SKPaint? paint))
        {
            fillPaintCache[descriptor] = paint = new SKPaint
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

    private SKPaint GetStrokePaint(SKColor color, float strokeWidth)
    {
        StrokePaintDescriptor descriptor = new(color, strokeWidth);

        if (!strokePaintCache.TryGetValue(descriptor, out SKPaint? paint))
        {
            strokePaintCache[descriptor] = paint = new SKPaint
            {
                IsAntialias = true,
                IsDither = true,
                Style = SKPaintStyle.Stroke,
                Color = color,
                StrokeWidth = strokeWidth,
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
            textPaintCache[descriptor] = paint = new SKPaint
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

    private record FontDescriptor(string FontFamily, float FontSize);

    private record FillPaintDescriptor(SKColor Color);

    private record StrokePaintDescriptor(SKColor Color, float StrokeWidth);

    private record TextPaintDescriptor(SKColor Color);
}
