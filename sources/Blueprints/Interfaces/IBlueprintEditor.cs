using SkiaSharp;

namespace Blueprints;

public interface IBlueprintEditor
{
    SKSize Extent { get; }

    float X { get; set; }

    float Y { get; set; }

    float Zoom { get; set; }

    string FontFamily { get; }

    IBlueprintTheme Theme { get; }

    IEnumerable<Element> Elements { get; }

    void Invalidate();

    SKTypeface ResolveTypeface(string fontFamily, SKFontStyleWeight weight);
}
