namespace Blueprints;

public interface IBlueprintEditor
{
    float Width { get; }

    float Height { get; }

    IBlueprintStyle Style { get; }

    float X { get; set; }

    float Y { get; set; }

    float Zoom { get; set; }

    IEnumerable<IBlueprintNode> Nodes { get; set; }

    void Invalidate();
}
