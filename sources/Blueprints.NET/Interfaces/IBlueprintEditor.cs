using SkiaSharp;

namespace Blueprints.NET;

public interface IBlueprintEditor
{
    SKSize Extent { get; }

    float X { get; set; }

    float Y { get; set; }

    float Zoom { get; set; }

    Cursor Cursor { get; set; }

    string FontFamily { get; }

    IBlueprintTheme Theme { get; }

    IEnumerable<Node> Nodes { get; }

    void Invalidate();

    SKTypeface ResolveTypeface(string fontFamily, SKFontStyleWeight weight);
}
