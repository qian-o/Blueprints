namespace Blueprints;

public abstract class Node : Element
{
    public Element? Title { get; set; }

    public Element? Content { get; set; }

    public Pin[] Inputs { get; set; } = [];

    public Pin[] Outputs { get; set; } = [];

    public float X { get; set; }

    public float Y { get; set; }
}
