using SkiaSharp;

namespace Blueprints;

public static class GeometryHelpers
{
    private static readonly Dictionary<BezierPathDescriptor, SKPath> bezierPathCache = [];

    public static SKPath BezierPath(SKPoint sourcePoint, SKPoint targetPoint, PinDirection sourceDirection, PinDirection targetDirection)
    {
        BezierPathDescriptor descriptor = new(sourcePoint, targetPoint, sourceDirection, targetDirection);

        if (!bezierPathCache.TryGetValue(descriptor, out SKPath? path))
        {
            float controlOffset = Math.Abs(targetPoint.X - sourcePoint.X) * 0.5f;

            SKPoint control1 = sourcePoint;
            SKPoint control2 = targetPoint;

            switch (sourceDirection)
            {
                case PinDirection.Input:
                    control1.X -= controlOffset;
                    break;

                case PinDirection.Output:
                    control1.X += controlOffset;
                    break;
            }

            switch (targetDirection)
            {
                case PinDirection.Input:
                    control2.X -= controlOffset;
                    break;

                case PinDirection.Output:
                    control2.X += controlOffset;
                    break;
            }

            bezierPathCache[descriptor] = path = new();

            path.MoveTo(sourcePoint);
            path.CubicTo(control1, control2, targetPoint);
        }

        return path;
    }

    private record BezierPathDescriptor(SKPoint SourcePoint, SKPoint TargetPoint, PinDirection SourceDirection, PinDirection TargetDirection);
}
