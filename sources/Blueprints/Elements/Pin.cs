using SkiaSharp;

namespace Blueprints;

public class Pin : Element
{
    public PinShape Shape { get; set; }

    public Drawable? Content { get; set; }

    public PinDirection Direction { get; private set; }

    protected override Element[] SubElements()
    {
        return [];
    }

    protected override Drawable[] SubDrawables()
    {
        return Content is null ? [] : [Content];
    }

    protected override void OnInitialize()
    {
        if (Parent is not Node node)
        {
            return;
        }

        if (node.Inputs.Contains(this))
        {
            Direction = PinDirection.Input;
        }
        else if (node.Outputs.Contains(this))
        {
            Direction = PinDirection.Output;
        }
        else
        {
            throw new InvalidOperationException("Pin must be part of a Node's Inputs or Outputs.");
        }
    }

    protected override SKSize OnMeasure(IDrawingContext dc)
    {
        return new(20, 20);
    }

    protected override void OnArrange()
    {
    }

    protected override void OnRender(IDrawingContext dc)
    {
        dc.DrawEllipse(Bounds, SKColors.Blue);
    }

    protected override void OnDragStarted(DragEventArgs args)
    {
    }

    protected override void OnDragDelta(DragEventArgs args)
    {
    }

    protected override void OnDragCancelled(DragEventArgs args)
    {
    }
}
