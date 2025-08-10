using SkiaSharp;

namespace Blueprints;

public abstract class Element : IController
{
    public float Width { get; set; } = float.NaN;

    public float Height { get; set; } = float.NaN;

    public SKRect Bounds { get; protected set; }

    public abstract void Layout(IDrawingContext dc, float offsetX, float offsetY);

    public abstract void Render(IDrawingContext dc);

    public virtual void PointerEntered(PointerEventArgs args)
    {
    }

    public virtual void PointerExited(PointerEventArgs args)
    {
    }

    public virtual void PointerPressed(PointerEventArgs args)
    {
    }

    public virtual void PointerMoved(PointerEventArgs args)
    {
    }

    public virtual void PointerReleased(PointerEventArgs args)
    {
    }

    public virtual void PointerWheelChanged(PointerWheelEventArgs args)
    {
    }
}
