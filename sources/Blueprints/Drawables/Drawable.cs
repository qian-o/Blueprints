using SkiaSharp;

namespace Blueprints;

public abstract class Drawable
{
    public SKSize Size { get; private set; } = SKSize.Empty;

    public SKRect Bounds { get; private set; } = SKRect.Empty;

    internal void Measure(IDrawingContext dc)
    {
        Size = OnMeasure(dc);
    }

    internal void Arrange(SKPoint position)
    {
        Bounds = SKRect.Create(position, Size);
    }

    internal void Render(IDrawingContext dc)
    {
        if (Bounds.IsEmpty)
        {
            return;
        }

        dc.PushClip(Bounds, 0);
        {
            OnRender(dc);
        }
        dc.Pop();
    }

    protected abstract SKSize OnMeasure(IDrawingContext dc);

    protected abstract void OnRender(IDrawingContext dc);

    #region Implicit Operators
    public static implicit operator Drawable(string text)
    {
        return new Text(text);
    }
    #endregion
}
