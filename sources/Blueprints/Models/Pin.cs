namespace Blueprints;

public abstract class Pin
{
    public PinShape Shape { get; set; } = PinShape.Circle;

    public object? Content { get; set; }
}
