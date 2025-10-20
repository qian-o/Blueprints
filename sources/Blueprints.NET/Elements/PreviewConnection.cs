using SkiaSharp;

namespace Blueprints;

internal class PreviewConnection(Pin sourcePin, Pin[] sourcePins, PinDirection targetDirection) : Connection(sourcePin, sourcePin)
{
    public Pin[] SourcePins { get; } = sourcePins;

    public SKPoint PreviewPoint { get; set => Set(ref field, value, false); }

    public override int ConnectionHash()
    {
        if (SourcePins.Length is 0)
        {
            return HashCode.Combine(Source.ConnectionPoint,
                                    PreviewPoint,
                                    Source.Direction,
                                    targetDirection,
                                    Theme.ConnectionWidth);
        }
        else
        {
            HashCode hashCode = new();

            foreach (Pin pin in SourcePins)
            {
                hashCode.Add(HashCode.Combine(pin.ConnectionPoint,
                                              PreviewPoint,
                                              pin.Direction,
                                              targetDirection,
                                              Theme.ConnectionWidth));
            }

            return hashCode.ToHashCode();
        }
    }

    public override bool HitTest(SKPoint position)
    {
        return false;
    }

    protected override void OnRender(IDrawingContext dc)
    {
        if (SourcePins.Length is 0)
        {
            SKPath path = GeometryHelper.BezierPath(Source.ConnectionPoint,
                                                    PreviewPoint,
                                                    Source.Direction,
                                                    targetDirection,
                                                    Theme.ConnectionWidth);

            dc.DrawPath(path, Theme.ConnectionColor);
        }
        else
        {
            foreach (Pin pin in SourcePins)
            {
                SKPath path = GeometryHelper.BezierPath(pin.ConnectionPoint,
                                                        PreviewPoint,
                                                        pin.Direction,
                                                        targetDirection,
                                                        Theme.ConnectionWidth);

                dc.DrawPath(path, Theme.ConnectionColor);
            }
        }
    }
}