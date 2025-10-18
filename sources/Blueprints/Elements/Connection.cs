using SkiaSharp;

namespace Blueprints;

public class Connection(Pin source, Pin target) : Element
{
    private SKPath? path;

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
        return IsHitTestVisible && path?.Contains(position.X, position.Y) is true;
    }

    protected override IEnumerable<Element> SubElements(bool includeConnections = true)
    {
        yield break;
    }

    protected override void OnInitialize()
    {
    }

    protected override SKSize OnMeasure(IDrawingContext dc)
    {
        return path is null ? SKSize.Empty : path.Bounds.Size;
    }

    protected override void OnArrange()
    {
    }

    protected override void OnRender(IDrawingContext dc)
    {
        bool isHovered = Source.IsPointerOver || Target.IsPointerOver || IsPointerOver;

        path = GeometryHelper.BezierPath(Source.ConnectionPoint,
                                         Target.ConnectionPoint,
                                         Source.Direction,
                                         Target.Direction,
                                         isHovered ? Theme.ConnectionHoverWidth : Theme.ConnectionWidth);

        dc.DrawPath(path, isHovered ? Theme.ConnectionHoverColor : Theme.ConnectionColor);
    }

    protected override void OnPointerPressed(PointerEventArgs args)
    {
        if (args.Modifiers is Modifiers.Menu)
        {
            Disconnect();
        }
    }
}
