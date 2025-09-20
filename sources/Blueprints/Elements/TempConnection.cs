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
        SKPath path = GeometryHelpers.BezierPath(Source.ConnectionPoint, TargetPoint, Source.Direction, targetDirection);

        dc.DrawPath(path, Theme.ConnectionColor, Theme.ConnectionWidth);
    }
}
