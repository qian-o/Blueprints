using SkiaSharp;

namespace Blueprints;

internal class TemporaryConnection(Pin pin, Pin[] pins, PinDirection direction) : Connection(pin, pin)
{
    public Pin[] Pins { get; } = pins;

    public SKPoint Point { get; set => Set(ref field, value, false); }

    public override bool HitTest(SKPoint position)
    {
        return false;
    }

    protected override void OnRender(IDrawingContext dc)
    {
        if (Pins.Length is 0)
        {
            SKPath path = GeometryHelpers.BezierPath(Source.ConnectionPoint, Point, Source.Direction, direction);

            dc.DrawPath(path, Theme.ConnectionColor, Theme.ConnectionWidth);
        }
        else
        {
            foreach (Pin pin in Pins)
            {
                SKPath path = GeometryHelpers.BezierPath(pin.ConnectionPoint, Point, pin.Direction, direction);

                dc.DrawPath(path, Theme.ConnectionColor, Theme.ConnectionWidth);
            }
        }
    }
}
