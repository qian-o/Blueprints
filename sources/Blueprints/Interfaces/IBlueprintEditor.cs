namespace Blueprints;

public interface IBlueprintEditor
{
    float Width { get; }

    float Height { get; }

    float X { get; }

    float Y { get; }

    float Zoom { get; }

    IBlueprintStyles Styles { get; }
}
