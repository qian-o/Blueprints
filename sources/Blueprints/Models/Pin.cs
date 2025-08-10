namespace Blueprints;

public abstract class Pin : Element
{
    public PinShape Shape { get; set; } = PinShape.Circle;

    public Element? Content { get; set; }
}
