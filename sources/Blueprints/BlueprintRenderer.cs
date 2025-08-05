using SkiaSharp;

namespace Blueprints;

public static class BlueprintRenderer
{
    private static readonly Dictionary<FontDescriptor, SKFont> fontCache = [];
    private static readonly Dictionary<PaintDescriptor, SKPaint> paintCache = [];

    public static void Render(IBlueprintEditor editor, SKCanvas canvas)
    {
        editor.Styles.Update();

        canvas.Clear(editor.Styles.BackgroundColor);

        Grid(editor, canvas);

        Debug(editor, canvas);
    }

    private static void Grid(IBlueprintEditor editor, SKCanvas canvas)
    {
        SKColor minorLineColor = editor.Styles.MinorLineColor;
        SKColor majorLineColor = editor.Styles.MajorLineColor;

        float minorLineWidth = editor.Styles.MinorLineWidth * editor.Zoom;
        float majorLineWidth = editor.Styles.MajorLineWidth * editor.Zoom;

        float minorLineSpacing = editor.Styles.MinorLineSpacing * editor.Zoom;
        float majorLineSpacing = editor.Styles.MajorLineSpacing * editor.Zoom;

        for (float x = editor.X % minorLineSpacing; x < editor.Width; x += minorLineSpacing)
        {
            canvas.DrawLine(x, 0, x, editor.Height, GetPaint(minorLineColor, minorLineWidth));
        }

        for (float y = editor.Y % minorLineSpacing; y < editor.Height; y += minorLineSpacing)
        {
            canvas.DrawLine(0, y, editor.Width, y, GetPaint(minorLineColor, minorLineWidth));
        }

        for (float x = editor.X % majorLineSpacing; x < editor.Width; x += majorLineSpacing)
        {
            canvas.DrawLine(x, 0, x, editor.Height, GetPaint(majorLineColor, majorLineWidth));
        }

        for (float y = editor.Y % majorLineSpacing; y < editor.Height; y += majorLineSpacing)
        {
            canvas.DrawLine(0, y, editor.Width, y, GetPaint(majorLineColor, majorLineWidth));
        }
    }

    private static void Debug(IBlueprintEditor editor, SKCanvas canvas)
    {
        const float margin = 10.0f;

        string debug = "Debug Information:\n"
                       + $"Zoom: {editor.Zoom}\n"
                       + $"X: {editor.X}\n"
                       + $"Y: {editor.Y}\n"
                       + $"Width: {editor.Width}\n"
                       + $"Height: {editor.Height}";

        SKFont font = GetFont(editor.Styles.FontFamily, editor.Styles.TextSize);

        SKSize rectSize = MeasureText(debug, font) + new SKSize(margin * 2, margin * 2);

        canvas.DrawRoundRect(new(0, 0, rectSize.Width, rectSize.Height),
                             new(margin, margin),
                             GetPaint(editor.Styles.ForegroundColor, 1));

        float y = -font.Metrics.Ascent + margin;
        foreach (string text in debug.Split('\n'))
        {
            canvas.DrawText(text, margin, y, font, GetPaint(editor.Styles.ForegroundColor, 0));

            y += font.GetFontMetrics(out _);
        }
    }

    private static SKSize MeasureText(string text, SKFont font)
    {
        string[] lines = text.Split('\n');

        float width = lines.Max(line => font.MeasureText(line, out _));
        float height = lines.Length * font.GetFontMetrics(out _);

        return new SKSize(width, height);
    }

    private static SKFont GetFont(string fontFamily, float fontSize)
    {
        FontDescriptor descriptor = new(fontFamily, fontSize);

        if (!fontCache.TryGetValue(descriptor, out SKFont? font))
        {
            fontCache[descriptor] = font = new(SKTypeface.FromFamilyName(fontFamily), fontSize);
        }

        return font;
    }

    private static SKPaint GetPaint(SKColor color, float strokeWidth)
    {
        PaintDescriptor descriptor = new(color, strokeWidth);

        if (!paintCache.TryGetValue(descriptor, out SKPaint? paint))
        {
            paintCache[descriptor] = paint = new()
            {
                IsAntialias = true,
                IsDither = true,
                Color = color,
                IsStroke = strokeWidth is not 0,
                StrokeWidth = strokeWidth,
                BlendMode = SKBlendMode.SrcOver
            };
        }

        return paint;
    }

    private record FontDescriptor(string FontFamily, float FontSize);

    private record PaintDescriptor(SKColor Color, float StrokeWidth);
}
