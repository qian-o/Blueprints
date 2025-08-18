using SkiaSharp;

namespace Blueprints;

public class Connection : Element
{
    public Connection(Pin source, Pin target)
    {
        CanMove = false;

        Source = source;
        Target = target;
    }

    public Pin Source { get; }

    public Pin Target { get; }

    public bool Connects(Pin pin1, Pin pin2)
    {
        return (Source == pin1 && Target == pin2) || (Source == pin2 && Target == pin1);
    }

    public void Disconnect()
    {
        Source.DisconnectFrom(Target);
    }

    protected override Element[] SubElements()
    {
        return [];
    }

    protected override Drawable[] SubDrawables()
    {
        return [];
    }

    protected override void OnInitialize()
    {
    }

    protected override SKSize OnMeasure(IDrawingContext dc)
    {
        return SKSize.Empty;
    }

    protected override void OnArrange()
    {
    }

    protected override void OnRender(IDrawingContext dc)
    {
    }
}
