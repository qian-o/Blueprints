using Microsoft.UI.Xaml;
using SkiaSharp;
using Windows.Foundation;

namespace Blueprints.NET.WinUI;

public partial class SKView
{
    public SKView()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        SizeChanged += (_, _) => Invalidate();
    }

    public double Dpi => XamlRoot?.RasterizationScale ?? 1.0;


    public event EventHandler<SKCanvas>? Paint;

    protected static SKPoint SKPoint(Point point)
    {
        return new((float)point.X, (float)point.Y);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        LoadedPartial();

        Invalidate();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        UnloadedPartial();
    }

    partial void LoadedPartial();

    partial void UnloadedPartial();
}
