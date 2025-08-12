using SkiaSharp;

namespace Blueprints;

public interface IDrawingContext
{
    void PushClip(SKRect rect, float radius);

    void PushClip(SKPath path);

    void PushTransform(SKMatrix matrix);

    void Pop();

    void Clear(SKColor color);

    void DrawImage(SKImage image, SKRect src, SKRect dest);

    void DrawPoint(SKPoint point, SKColor color);

    void DrawLine(SKPoint start, SKPoint end, SKColor stroke, float strokeWidth);

    void DrawRectangle(SKRect rect, float radius, SKColor fill);

    void DrawRectangle(SKRect rect, float radius, SKColor stroke, float strokeWidth);

    void DrawEllipse(SKRect rect, SKColor fill);

    void DrawEllipse(SKRect rect, SKColor stroke, float strokeWidth);

    void DrawCircle(SKPoint center, float radius, SKColor fill);

    void DrawCircle(SKPoint center, float radius, SKColor stroke, float strokeWidth);

    void DrawPath(SKPath path, SKColor fill);

    void DrawPath(SKPath path, SKColor stroke, float strokeWidth);

    void DrawText(string text, SKPoint position, string fontFamily, float fontWeight, float fontSize, SKColor color);

    SKSize MeasureText(string text, string fontFamily, float fontWeight, float fontSize);
}
