using System.Runtime.CompilerServices;
using SkiaSharp;

namespace Blueprints;

public class Connection(Pin source, Pin target) : Element
{
    private SKPath? fillPath;

    public int Id { get; } = ComputeId(source, target);

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

    protected override IEnumerable<Element> SubElements(bool includeConnections = true)
    {
        yield break;
    }

    protected override void OnInitialize()
    {
    }

    protected override SKSize OnMeasure(IDrawingContext dc)
    {
        return fillPath is null ? SKSize.Empty : fillPath.Bounds.Size;
    }

    protected override void OnArrange()
    {
    }

    protected override void OnRender(IDrawingContext dc)
    {
        bool isHovered = Source.IsPointerOver || Target.IsPointerOver || IsPointerOver;

        SKPath path = GeometryHelper.BezierPath(Source.ConnectionPoint, Target.ConnectionPoint, Source.Direction, Target.Direction);

        dc.DrawPath(path, isHovered ? Theme.ConnectionHoverColor : Theme.ConnectionColor, isHovered ? Theme.ConnectionHoverWidth : Theme.ConnectionWidth);

        fillPath?.Dispose();
        fillPath = dc.GetFillPath(path, isHovered ? Theme.ConnectionHoverWidth : Theme.ConnectionWidth);
    }

    protected override void OnPointerPressed(PointerEventArgs args)
    {
        if (args.Modifiers is Modifiers.Menu)
        {
            Disconnect();
        }
    }

    private static int ComputeId(Pin a, Pin b)
    {
        int ha = RuntimeHelpers.GetHashCode(a);
        int hb = RuntimeHelpers.GetHashCode(b);

        if (ha > hb)
        {
            (ha, hb) = (hb, ha);
        }

        return HashCode.Combine(ha, hb);
    }
}
