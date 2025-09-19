using SkiaSharp;
using Windows.Foundation;

namespace Blueprints.WinUI;

public partial class SKView
{
    public event EventHandler<SKCanvas>? Paint;

    protected static SKPoint SKPoint(Point point)
    {
        return new SKPoint((float)point.X, (float)point.Y);
    }
}
