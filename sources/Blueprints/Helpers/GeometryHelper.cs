using SkiaSharp;

namespace Blueprints;

public static class GeometryHelper
{
    private static readonly Dictionary<BezierPathDescriptor, SKPath> bezierPathCache = [];
    private static readonly Dictionary<FillPathDescriptor, SKPath> fillPathCache = [];

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

    public static SKPath FillPath(SKPath path, float strokeWidth)
    {
        FillPathDescriptor descriptor = new(path.ToSvgPathData(), strokeWidth);

        if (!fillPathCache.TryGetValue(descriptor, out SKPath? fillPath))
        {
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

            fillPathCache[descriptor] = fillPath = paint.GetFillPath(path);
        }

        return fillPath;
    }

    private record BezierPathDescriptor(SKPoint SourcePoint, SKPoint TargetPoint, PinDirection SourceDirection, PinDirection TargetDirection);

    private record FillPathDescriptor(string PathData, float StrokeWidth);
}
