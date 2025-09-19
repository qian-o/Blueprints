using Microsoft.UI.Xaml;
using SkiaSharp;
using Windows.Foundation;
using Windows.Graphics.Display;

namespace Blueprints.WinUI;

public partial class SKView
{
    private const float DpiBase = 96.0f;

    public SKView()
    {
        Loaded += (_, _) =>
        {
#if WINDOWS
            XamlRoot.Changed += OnXamlRootChanged;

            OnXamlRootChanged();
#else
            DisplayInformation display = DisplayInformation.GetForCurrentView();

            display.DpiChanged += OnDpiChanged;

            OnDpiChanged(display);
#endif
        };

        Unloaded += (_, _) =>
        {
#if WINDOWS
            XamlRoot?.Changed -= OnXamlRootChanged;
#else
            DisplayInformation display = DisplayInformation.GetForCurrentView();

            display.DpiChanged -= OnDpiChanged;
#endif
        };

        SizeChanged += (_, _) => Invalidate();
    }

    public double Dpi { get; private set; } = 1;


    public event EventHandler<SKCanvas>? Paint;

    private void OnXamlRootChanged(XamlRoot? xamlRoot = null, XamlRootChangedEventArgs? _ = null)
    {
        double dpi = (xamlRoot ?? XamlRoot)?.RasterizationScale ?? 1.0;

        if (Dpi != dpi)
        {
            Dpi = dpi;

            Invalidate();
        }
    }

    private void OnDpiChanged(DisplayInformation sender, object? _ = null)
    {
        Dpi = sender.LogicalDpi / DpiBase;

        Invalidate();
    }
}
