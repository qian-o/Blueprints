using SkiaSharp;
using Windows.Foundation;

namespace Blueprints.WinUI;

public partial class SKView
{
    public SKView()
    {
        SizeChanged += (_, _) => Invalidate();
    }

    public double Dpi => XamlRoot?.RasterizationScale ?? 1.0;


    public event EventHandler<SKCanvas>? Paint;

    protected SKPoint SKPoint(Point point)
    {
        return new SKPoint((float)(point.X / Dpi), (float)(point.Y / Dpi));
    }
}
