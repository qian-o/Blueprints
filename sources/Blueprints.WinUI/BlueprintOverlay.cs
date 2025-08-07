using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SkiaSharp;
using SkiaSharp.Views.Windows;
using Windows.Foundation;

namespace Blueprints.WinUI;

internal partial class BlueprintOverlay : Canvas, IBlueprintOverlay
{
    private readonly Dictionary<object, ContentPresenter> overlays = [];

    public SKSize Measure(object overlay)
    {
        if (overlay is FrameworkElement element)
        {
            element.Measure(Size.Empty);

            return element.DesiredSize.ToSKSize();
        }

        return new(0, 0);
    }

    public void Render(object overlay, float x, float y)
    {
        if (!overlays.TryGetValue(overlay, out ContentPresenter? contentPresenter))
        {
            overlays[overlay] = contentPresenter = new() { Content = overlay };

            Children.Add(contentPresenter);
        }

        SetLeft(contentPresenter, x);
        SetTop(contentPresenter, y);
    }

    public void Destroy(object overlay)
    {
        if (overlays.TryGetValue(overlay, out ContentPresenter? contentPresenter))
        {
            contentPresenter.Content = null;

            overlays.Remove(overlay);
            Children.Remove(contentPresenter);
        }
    }
}
