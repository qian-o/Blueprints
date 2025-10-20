using SkiaSharp;

namespace Blueprints.NET;

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

    public virtual int ConnectionHash()
    {
        return HashCode.Combine(Source.ConnectionPoint,
                                Target.ConnectionPoint,
                                Source.Direction,
                                Target.Direction,
                                Theme.ConnectionWidth);
    }

    public override bool HitTest(SKPoint position)
    {
        if (IsHitTestVisible)
        {
            bool isHovered = Source.IsPointerOver || Target.IsPointerOver || IsPointerOver;

            SKPath path = GeometryHelper.BezierPath(Source.ConnectionPoint,
                                                    Target.ConnectionPoint,
                                                    Source.Direction,
                                                    Target.Direction,
                                                    isHovered ? Theme.ConnectionHoverWidth : Theme.ConnectionWidth);

            return path.Contains(position.X, position.Y);
        }

        return false;
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
        return new(-1, -1);
    }

    protected override void OnArrange()
    {
    }

    protected override void OnRender(IDrawingContext dc)
    {
        bool isHovered = Source.IsPointerOver || Target.IsPointerOver || IsPointerOver;

        SKPath path = GeometryHelper.BezierPath(Source.ConnectionPoint,
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
