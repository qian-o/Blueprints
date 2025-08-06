using SkiaSharp;

namespace Blueprints;

public interface IBlueprintDrawingContext
{
    void PushClip(SKRect rect);

    void PushClip(SKRoundRect roundRect);

    void PushClip(SKPath path);

    void PushTransform(SKMatrix matrix);

    void Pop();

    void Clear(SKColor color);

    void DrawLine(SKPoint start, SKPoint end, float strokeWidth, SKColor strokeColor);

    void DrawRectangle(SKRect rect, SKColor fillColor, float strokeWidth, SKColor strokeColor);

    void DrawRoundRectangle(SKRoundRect roundRect, SKColor fillColor, float strokeWidth, SKColor strokeColor);

    void DrawEllipse(SKRect rect, SKColor fillColor, float strokeWidth, SKColor strokeColor);

    void DrawPath(SKPath path, SKColor fillColor, float strokeWidth, SKColor strokeColor);

    void DrawText(string text, SKPoint position, string fontFamily, float fontSize, SKColor color);

    SKSize MeasureText(string text, string fontFamily, float fontSize);
}
