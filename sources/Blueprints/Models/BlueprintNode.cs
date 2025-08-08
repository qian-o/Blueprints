namespace Blueprints;

public class BlueprintNode
{
    public float X { get; set; }

    public float Y { get; set; }

    public BlueprintPort[] InputPorts { get; set; } = [];

    public BlueprintPort[] OutputPorts { get; set; } = [];
}
