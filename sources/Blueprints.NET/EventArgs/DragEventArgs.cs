using SkiaSharp;

namespace Blueprints.NET;

public class DragEventArgs : EventArgs
{
    public Element? Element { get; internal set; }

    public SKPoint ScreenPosition { get; set; }

    public SKPoint WorldPosition { get; set; }

    public object? Data { get; set; }

    public bool Handled { get; set; }
}
