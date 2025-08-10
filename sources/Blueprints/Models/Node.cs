namespace Blueprints;

public abstract class Node
{
    public object? Title { get; set; }

    public object? Content { get; set; }

    public Pin[] Inputs { get; set; } = [];

    public Pin[] Outputs { get; set; } = [];

    public float X { get; set; }

    public float Y { get; set; }
}
