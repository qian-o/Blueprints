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
        SKPoint sourcePoint = Source.ConnectionPoint;
        SKPoint targetPoint = Target.ConnectionPoint;

        Position = new(Math.Min(sourcePoint.X, targetPoint.X), Math.Min(sourcePoint.Y, targetPoint.Y));

        return new(Math.Abs(sourcePoint.X - targetPoint.X), Math.Abs(sourcePoint.Y - targetPoint.Y));
    }

    protected override void OnArrange()
    {
    }

    protected override void OnRender(IDrawingContext dc)
    {
        var sourcePoint = Source.ConnectionPoint;
        var targetPoint = Target.ConnectionPoint;

        var controlOffset = Math.Abs(targetPoint.X - sourcePoint.X) * 0.5f;

        var control1 = sourcePoint;
        var control2 = targetPoint;

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

        using var path = new SKPath();

        path.MoveTo(sourcePoint);
        path.CubicTo(control1, control2, targetPoint);

        var strokeColor = Theme.PinColor;
        var strokeWidth = 2f;

        if (Source.IsPointerOver || Target.IsPointerOver)
        {
            strokeColor = Theme.PinHoverColor;
            strokeWidth = 3f;
        }

        dc.DrawPath(path, strokeColor, strokeWidth);
    }
}
