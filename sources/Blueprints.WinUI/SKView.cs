using SkiaSharp;

namespace Blueprints.WinUI;

public partial class SKView
{
    public SKView()
    {
        SizeChanged += (_, _) => Invalidate();
    }

    public double Dpi => XamlRoot?.RasterizationScale ?? 1.0;


    public event EventHandler<SKCanvas>? Paint;
}
