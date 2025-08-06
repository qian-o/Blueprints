namespace Blueprints;

public interface IBlueprintEditor
{
    float Dpi { get; }

    float Width { get; }

    float Height { get; }

    IBlueprintOverlay Overlay { get; }

    IBlueprintStyle Style { get; }

    float X { get; set; }

    float Y { get; set; }

    float Zoom { get; set; }

    void Invalidate();
}
