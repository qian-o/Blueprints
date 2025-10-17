using SkiaSharp;

namespace Blueprints;

internal class DrawingContext(IBlueprintEditor editor) : IDrawingContext
{
    private readonly Dictionary<TypefaceDescriptor, SKTypeface> typefaceCache = [];
    private readonly Dictionary<FontDescriptor, SKFont> fontCache = [];
    private readonly Dictionary<TextMetricsDescriptor, SKSize> textMetricsCache = [];
    private readonly Dictionary<TextBlobDescriptor, SKTextBlob> textBlobCache = [];
    private readonly Dictionary<FillPaintDescriptor, SKPaint> fillPaintCache = [];
    private readonly Dictionary<StrokePaintDescriptor, SKPaint> strokePaintCache = [];
    private readonly Dictionary<TextPaintDescriptor, SKPaint> textPaintCache = [];

    internal SKCanvas Canvas { get; set; } = null!;

    public void PushClip(SKRect rect, float radius)
    {
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
        Canvas.Save();
        Canvas.ClipPath(path, SKClipOperation.Intersect, true);
    }

    public void PushTransform(SKMatrix matrix)
    {
        Canvas.Save();
        Canvas.Concat(matrix);
    }

    public void Pop()
    {
        Canvas.Restore();
    }

    public void Clear(SKColor color)
    {
        Canvas.Clear(color);
    }

    public void DrawImage(SKImage image, SKRect src, SKRect dest)
    {
        Canvas.DrawImage(image, src, dest, new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));
    }

    public void DrawPoint(SKPoint point, SKColor color)
    {
        Canvas.DrawPoint(point, GetFillPaint(color));
    }

    public void DrawLine(SKPoint start, SKPoint end, SKColor stroke, float strokeWidth)
    {
        Canvas.DrawLine(start, end, GetStrokePaint(stroke, strokeWidth));
    }

    public void DrawRectangle(SKRect rect, float radius, SKColor fill)
    {
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
        Canvas.DrawOval(rect, GetFillPaint(fill));
    }

    public void DrawEllipse(SKRect rect, SKColor stroke, float strokeWidth)
    {
        Canvas.DrawOval(rect, GetStrokePaint(stroke, strokeWidth));
    }

    public void DrawCircle(SKPoint center, float radius, SKColor fill)
    {
        Canvas.DrawCircle(center, radius, GetFillPaint(fill));
    }

    public void DrawCircle(SKPoint center, float radius, SKColor stroke, float strokeWidth)
    {
        Canvas.DrawCircle(center, radius, GetStrokePaint(stroke, strokeWidth));
    }

    public void DrawPath(SKPath path, SKColor fill)
    {
        Canvas.DrawPath(path, GetFillPaint(fill));
    }

    public void DrawPath(SKPath path, SKColor stroke, float strokeWidth)
    {
        Canvas.DrawPath(path, GetStrokePaint(stroke, strokeWidth));
    }

    public SKPath GetFillPath(SKPath path, float strokeWidth)
    {
        return GetStrokePaint(SKColors.Black, strokeWidth).GetFillPath(path);
    }

    public void DrawText(string text, SKPoint position, float fontWeight, float fontSize, SKColor color)
    {
        DrawText(text, position, editor.FontFamily, fontWeight, fontSize, color);
    }

    public SKSize MeasureText(string text, float fontWeight, float fontSize)
    {
        return MeasureText(text, editor.FontFamily, fontWeight, fontSize);
    }

    public void DrawText(string text, SKPoint position, string fontFamily, float fontWeight, float fontSize, SKColor color)
    {
        SKFont font = GetFont(GetTypeface(fontFamily, fontWeight), fontSize);
        SKPaint paint = GetTextPaint(color);

        float lineHeight = font.GetFontMetrics(out _);

        float deltaY = 0;
        foreach (string line in text.Split('\n'))
        {
            if (!string.IsNullOrEmpty(line))
            {
                Canvas.DrawText(GetTextBlob(line, font), position.X, position.Y + fontSize + deltaY, paint);
            }

            deltaY += lineHeight;
        }
    }

    public SKSize MeasureText(string text, string fontFamily, float fontWeight, float fontSize)
    {
        SKFont font = GetFont(GetTypeface(fontFamily, fontWeight), fontSize);

        return GetTextMetrics(text, font);
    }

    private SKTypeface GetTypeface(string fontFamily, float fontWeight)
    {
        fontWeight = Math.Clamp((int)fontWeight / 100 * 100, 0, 1000);

        TypefaceDescriptor descriptor = new(fontFamily, fontWeight);

        if (!typefaceCache.TryGetValue(descriptor, out SKTypeface? typeface))
        {
            typefaceCache[descriptor] = typeface = editor.ResolveTypeface(fontFamily, (SKFontStyleWeight)fontWeight);
        }

        return typeface;
    }

    private SKFont GetFont(SKTypeface typeface, float fontSize)
    {
        FontDescriptor descriptor = new(typeface, fontSize);

        if (!fontCache.TryGetValue(descriptor, out SKFont? font))
        {
            fontCache[descriptor] = font = new(typeface, fontSize);
        }

        return font;
    }

    private SKSize GetTextMetrics(string text, SKFont font)
    {
        TextMetricsDescriptor descriptor = new(text, font);

        if (!textMetricsCache.TryGetValue(descriptor, out SKSize size))
        {
            string[] lines = text.Split('\n');

            float width = lines.Max(line => font.MeasureText(line, out _));
            float height = lines.Length * font.GetFontMetrics(out _);

            textMetricsCache[descriptor] = size = new(width, height);
        }

        return size;
    }

    private SKTextBlob GetTextBlob(string text, SKFont font)
    {
        TextBlobDescriptor descriptor = new(text, font);

        if (!textBlobCache.TryGetValue(descriptor, out SKTextBlob? blob))
        {
            textBlobCache[descriptor] = blob = SKTextBlob.Create(text, font)!;
        }

        return blob;
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
                Color = color
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
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round
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
                Color = color
            };
        }

        return paint;
    }

    private record TypefaceDescriptor(string FontFamily, float FontWeight);

    private record FontDescriptor(SKTypeface Typeface, float FontSize);

    private record TextMetricsDescriptor(string Text, SKFont Font);

    private record TextBlobDescriptor(string Text, SKFont Font);

    private record FillPaintDescriptor(SKColor Color);

    private record StrokePaintDescriptor(SKColor Color, float Width);

    private record TextPaintDescriptor(SKColor Color);
}
