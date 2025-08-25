using SkiaSharp;

namespace Blueprints;

internal class TempConnection(Pin pin, PinDirection targetDirection) : Connection(pin, pin)
{
    public SKPoint TargetPoint { get; set => Set(ref field, value, false); }

    public override bool HitTest(SKPoint position)
    {
        return false;
    }

    protected override void OnRender(IDrawingContext dc)
    {
        using SKPath path = GeometryHelpers.CreateBezierPath(Source.ConnectionPoint, TargetPoint, Source.Direction, targetDirection);

        dc.DrawPath(path, Theme.ConnectionColor, Theme.ConnectionWidth);
    }
}
