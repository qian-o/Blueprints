using SkiaSharp;

namespace Blueprints;

public abstract class Element : IController
{
    public float Width { get; set; } = float.NaN;

    public float Height { get; set; } = float.NaN;

    public SKRect Bounds { get; protected set; }

    public abstract void Layout(IDrawingContext dc, float offsetX, float offsetY);

    public abstract void Render(IDrawingContext dc);

    public virtual void PointerEntered(PointerFlags pointers, SKPoint position)
    {
    }

    public virtual void PointerExited(PointerFlags pointers, SKPoint position)
    {
    }

    public virtual void PointerPressed(PointerFlags pointers, SKPoint position)
    {
    }

    public virtual void PointerMoved(PointerFlags pointers, SKPoint position)
    {
    }

    public virtual void PointerReleased(PointerFlags pointers, SKPoint position)
    {
    }

    public virtual void PointerWheelChanged(float delta, SKPoint position)
    {
    }
}
