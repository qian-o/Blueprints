using SkiaSharp;

namespace Blueprints.NET;

public static class GeometryHelper
{
    private static readonly Dictionary<BezierPathDescriptor, SKPath> bezierPathCache = [];

    public static SKPath BezierPath(SKPoint sourcePoint,
                                    SKPoint targetPoint,
                                    PinDirection sourceDirection,
                                    PinDirection targetDirection,
                                    float strokeWidth)
    {
        BezierPathDescriptor descriptor = new(sourcePoint, targetPoint, sourceDirection, targetDirection, strokeWidth);

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

            using SKPath strokePath = new();
            strokePath.MoveTo(sourcePoint);
            strokePath.CubicTo(control1, control2, targetPoint);

            using SKPaint paint = new()
            {
                IsAntialias = true,
                IsDither = true,
                Color = SKColors.Black,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = strokeWidth,
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round
            };

            bezierPathCache[descriptor] = path = paint.GetFillPath(strokePath);
        }

        return path;
    }

    private record BezierPathDescriptor(SKPoint SourcePoint,
                                        SKPoint TargetPoint,
                                        PinDirection SourceDirection,
                                        PinDirection TargetDirection,
                                        float StrokeWidth);
}
