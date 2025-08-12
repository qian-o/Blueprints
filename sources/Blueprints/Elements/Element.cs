using SkiaSharp;

namespace Blueprints;

public abstract partial class Element : IController
{
    public IBlueprintEditor? Editor { get; private set; }

    public float MinWidth { get; set; }

    public float MinHeight { get; set; }

    public float Width { get; set; } = float.NaN;

    public float Height { get; set; } = float.NaN;

    public float MaxWidth { get; set; } = float.PositiveInfinity;

    public float MaxHeight { get; set; } = float.PositiveInfinity;

    public SKSize DesiredSize { get; private set; } = SKSize.Empty;

    public SKRect Bounds { get; private set; } = SKRect.Empty;

    public SKRect ContentBounds => SKRect.Create(Bounds.Left + Padding.Left + StrokeWidth,
                                                 Bounds.Top + Padding.Top + StrokeWidth,
                                                 Bounds.Width - Padding.Horizontal - (StrokeWidth * 2),
                                                 Bounds.Height - Padding.Vertical - (StrokeWidth * 2));

    public SKRect ScreenBounds
    {
        get
        {
            if (Editor is null)
            {
                throw new InvalidOperationException("Editor is not bound to this element.");
            }

            return new(Editor.X + (Bounds.Left * Editor.Zoom),
                       Editor.Y + (Bounds.Top * Editor.Zoom),
                       Editor.X + (Bounds.Right * Editor.Zoom),
                       Editor.Y + (Bounds.Bottom * Editor.Zoom));
        }
    }

    public void Invalidate()
    {
        Editor?.Invalidate();
    }

    public void Measure(IDrawingContext dc)
    {
        foreach (Element element in GetSubElements())
        {
            element.Measure(dc);
        }

        float width = Width;
        float height = Height;

        if (float.IsNaN(width) || float.IsNaN(height))
        {
            SKSize size = OnMeasure(dc);

            if (float.IsNaN(width))
            {
                width = size.Width;
            }

            if (float.IsNaN(height))
            {
                height = size.Height;
            }
        }

        DesiredSize = new(Math.Clamp(width + Padding.Horizontal + (StrokeWidth * 2), MinWidth, MaxWidth),
                          Math.Clamp(height + Padding.Vertical + (StrokeWidth * 2), MinHeight, MaxHeight));
    }

    public void Arrange(SKRect finalBounds)
    {
        Bounds = finalBounds;

        OnArrange(ContentBounds);
    }

    internal void Bind(IBlueprintEditor editor)
    {
        Editor = editor;

        foreach (Element element in GetSubElements())
        {
            element.Bind(editor);
        }
    }

    internal void Render(IDrawingContext dc)
    {
        dc.PushClip(Bounds, CornerRadius);
        {
            dc.DrawRectangle(Bounds, CornerRadius, Background);
            dc.DrawRectangle(Bounds, CornerRadius, Stroke, StrokeWidth * 2);

            dc.PushClip(ContentBounds, CornerRadius);
            {
                OnRender(dc);

                foreach (Element element in GetSubElements())
                {
                    element.Render(dc);
                }
            }
            dc.Pop();
        }
        dc.Pop();
    }

    protected abstract Element[] GetSubElements();

    protected abstract SKSize OnMeasure(IDrawingContext dc);

    protected abstract void OnArrange(SKRect rect);

    protected abstract void OnRender(IDrawingContext dc);

    protected virtual void OnPointerEntered(PointerEventArgs args) { }

    protected virtual void OnPointerExited(PointerEventArgs args) { }

    protected virtual void OnPointerPressed(PointerEventArgs args) { }

    protected virtual void OnPointerMoved(PointerEventArgs args) { }

    protected virtual void OnPointerReleased(PointerEventArgs args) { }

    protected virtual void OnPointerWheelChanged(PointerWheelEventArgs args) { }

    void IController.PointerEntered(PointerEventArgs args)
    {
        foreach (Element element in GetSubElements())
        {
            if (element.ScreenBounds.Contains(args.Position))
            {
                ((IController)element).PointerEntered(args);
            }
        }

        if (args.Handled)
        {
            return;
        }

        OnPointerEntered(args);
    }

    void IController.PointerExited(PointerEventArgs args)
    {
        foreach (Element element in GetSubElements())
        {
            if (element.ScreenBounds.Contains(args.Position))
            {
                ((IController)element).PointerExited(args);
            }
        }

        if (args.Handled)
        {
            return;
        }

        OnPointerExited(args);
    }

    void IController.PointerPressed(PointerEventArgs args)
    {
        foreach (Element element in GetSubElements())
        {
            if (element.ScreenBounds.Contains(args.Position))
            {
                ((IController)element).PointerPressed(args);
            }
        }

        if (args.Handled)
        {
            return;
        }

        OnPointerPressed(args);
    }

    void IController.PointerMoved(PointerEventArgs args)
    {
        foreach (Element element in GetSubElements())
        {
            if (element.ScreenBounds.Contains(args.Position))
            {
                ((IController)element).PointerMoved(args);
            }
        }

        if (args.Handled)
        {
            return;
        }

        OnPointerMoved(args);
    }

    void IController.PointerReleased(PointerEventArgs args)
    {
        foreach (Element element in GetSubElements())
        {
            if (element.ScreenBounds.Contains(args.Position))
            {
                ((IController)element).PointerReleased(args);
            }
        }

        if (args.Handled)
        {
            return;
        }

        OnPointerReleased(args);
    }

    void IController.PointerWheelChanged(PointerWheelEventArgs args)
    {
        foreach (Element element in GetSubElements())
        {
            if (element.ScreenBounds.Contains(args.Position))
            {
                ((IController)element).PointerWheelChanged(args);
            }
        }

        if (args.Handled)
        {
            return;
        }

        OnPointerWheelChanged(args);
    }
}
