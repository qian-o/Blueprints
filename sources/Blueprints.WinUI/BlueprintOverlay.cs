using Microsoft.UI.Xaml.Controls;
using SkiaSharp;

namespace Blueprints.WinUI;

internal partial class BlueprintOverlay : Canvas, IBlueprintOverlay
{
    private readonly Dictionary<object, ContentPresenter> overlays = [];

    public void Render(object overlay, float x, float y)
    {
        if (!overlays.TryGetValue(overlay, out ContentPresenter? presenter))
        {
            overlays[overlay] = presenter = new() { Content = overlay };

            Children.Add(presenter);
        }

        SetLeft(presenter, x);
        SetTop(presenter, y);
    }

    public void Destroy(object overlay)
    {
        if (overlays.TryGetValue(overlay, out ContentPresenter? presenter))
        {
            presenter.Content = null;

            overlays.Remove(overlay);
            Children.Remove(presenter);
        }
    }

    public SKSize Measure(object overlay)
    {
        if (overlays.TryGetValue(overlay, out ContentPresenter? presenter))
        {
            return new((float)presenter.ActualWidth, (float)presenter.ActualHeight);
        }
        else
        {
            return SKSize.Empty;
        }
    }
}
