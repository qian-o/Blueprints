namespace Blueprints;

public interface IBlueprintNode
{
    string Name { get; set; }

    float X { get; set; }

    float Y { get; set; }

    IEnumerable<IBlueprintPort> InputPorts { get; }

    IEnumerable<IBlueprintPort> OutputPorts { get; }
}
