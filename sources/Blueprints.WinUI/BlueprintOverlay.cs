using Microsoft.UI.Xaml.Controls;

namespace Blueprints.WinUI;

internal partial class BlueprintOverlay : Canvas, IBlueprintOverlay
{
    private readonly Dictionary<object, ContentPresenter> overlays = [];

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
