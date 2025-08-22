using SkiaSharp;

namespace Blueprints;

public class Connection(Pin source, Pin target) : Element
{
    public Pin Source { get; } = source;

    public Pin Target { get; } = target;

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

    protected override void OnInitialize()
    {
    }

    protected override SKSize OnMeasure(IDrawingContext dc)
    {
        return new(1, 1);
    }

    protected override void OnArrange()
    {
    }

    protected override void OnRender(IDrawingContext dc)
    {
        bool isHovered = Source.IsPointerOver || Target.IsPointerOver || IsPointerOver;

        SKPoint sourcePoint = Source.ConnectionPoint;
        SKPoint targetPoint = Target.ConnectionPoint;

        float controlOffset = Math.Abs(targetPoint.X - sourcePoint.X) * 0.5f;

        SKPoint control1 = sourcePoint;
        SKPoint control2 = targetPoint;

        switch (Source.Direction)
        {
            case PinDirection.Input:
                control1.X -= controlOffset;
                break;
            case PinDirection.Output:
                control1.X += controlOffset;
                break;
        }

        switch (Target.Direction)
        {
            case PinDirection.Input:
                control2.X -= controlOffset;
                break;
            case PinDirection.Output:
                control2.X += controlOffset;
                break;
        }

        using SKPath path = new();
        path.MoveTo(sourcePoint);
        path.CubicTo(control1, control2, targetPoint);

        dc.DrawPath(path, isHovered ? Theme.ConnectionHoverColor : Theme.ConnectionColor, isHovered ? Theme.ConnectionHoverWidth : Theme.ConnectionWidth);
    }
}
