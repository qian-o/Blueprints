using SkiaSharp;

namespace Blueprints;

public class Connection(Pin source, Pin target) : Element
{
    private SKPath? fillPath;

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

    public override bool HitTest(SKPoint position)
    {
        return IsHitTestVisible && fillPath?.Contains(position.X, position.Y) is true;
    }

    protected override Element[] SubElements(bool includeConnections = true)
    {
        return [];
    }

    protected override void OnInitialize()
    {
    }

    protected override SKSize OnMeasure(IDrawingContext dc)
    {
        return new(-1, -1);
    }

    protected override void OnArrange()
    {
    }

    protected override void OnRender(IDrawingContext dc)
    {
        bool isHovered = Source.IsPointerOver || Target.IsPointerOver || IsPointerOver;

        using SKPath path = GeometryHelpers.CreateBezierPath(Source.ConnectionPoint, Target.ConnectionPoint, Source.Direction, Target.Direction);

        dc.DrawPath(path, isHovered ? Theme.ConnectionHoverColor : Theme.ConnectionColor, isHovered ? Theme.ConnectionHoverWidth : Theme.ConnectionWidth);

        fillPath?.Dispose();
        fillPath = dc.GetFillPath(path, isHovered ? Theme.ConnectionHoverWidth : Theme.ConnectionWidth);
    }
}
