using SkiaSharp;

namespace Blueprints;

public class Pin : Element
{
    public PinShape Shape { get; set; }

    public Drawable? Content { get; set; }

    public SKColor? Color { get; set; }

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
        float width = Theme.PinSize;
        float height = Theme.PinSize;

        if (Content is not null)
        {
            width = Theme.PinSize + Content.Size.Width + 6;
            height = Math.Max(Theme.PinSize, Content.Size.Height);
        }

        return new(width, height);
    }

    protected override void OnArrange()
    {
        if (Content is not null)
        {
            switch (Direction)
            {
                case PinDirection.Input:
                    Content.Position = new SKPoint(Bounds.Left + Theme.PinSize + 6, Bounds.MidY - (Content.Size.Height / 2));
                    break;
                case PinDirection.Output:
                    Content.Position = new SKPoint(Bounds.Right - Theme.PinSize - 6 - Content.Size.Width, Bounds.MidY - (Content.Size.Height / 2));
                    break;
            }
        }
    }

    protected override void OnRender(IDrawingContext dc)
    {
        SKRect rect = SKRect.Empty;

        switch (Direction)
        {
            case PinDirection.Input:
                rect = SKRect.Create(Bounds.Left, Bounds.MidY - (Theme.PinSize / 2), Theme.PinSize, Theme.PinSize);
                break;
            case PinDirection.Output:
                rect = SKRect.Create(Bounds.Right - Theme.PinSize, Bounds.MidY - (Theme.PinSize / 2), Theme.PinSize, Theme.PinSize);
                break;
        }

        dc.DrawRectangle(rect, 0, Color ?? Theme.PinColor);
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
