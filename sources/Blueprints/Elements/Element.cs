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

    internal void Bind(IBlueprintEditor editor)
    {
        Editor = editor;

        foreach (Element element in GetSubElements())
        {
            element.Bind(editor);
        }
    }

    internal void Render(IDrawingContext dc, float offsetX, float offsetY)
    {
        if (Editor is null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        Measure(dc);
        Arrange(SKRect.Create(offsetX, offsetY, DesiredSize.Width, DesiredSize.Height));

        Render(dc);
    }

    private void Measure(IDrawingContext dc)
    {
        foreach (Element element in GetSubElements())
        {
            element.Measure(dc);
        }

        float width = 0;
        float height = 0;
        if (float.IsNaN(Width) || float.IsNaN(Height))
        {
            SKSize size = OnMeasure(dc);

            if (float.IsNaN(Width))
            {
                width = Math.Clamp(size.Width, MinWidth, MaxWidth);
            }

            if (float.IsNaN(Height))
            {
                height = Math.Clamp(size.Height, MinHeight, MaxHeight);
            }
        }
        else
        {
            width = Math.Clamp(Width, MinWidth, MaxWidth);
            height = Math.Clamp(Height, MinHeight, MaxHeight);
        }

        DesiredSize = new(width, height);
    }

    private void Arrange(SKRect finalBounds)
    {
        Bounds = finalBounds;

        OnArrange(finalBounds.Size);
    }

    private void Render(IDrawingContext dc)
    {
        foreach (Element element in GetSubElements())
        {
            element.Render(dc);
        }

        dc.PushTransform(SKMatrix.CreateTranslation(Bounds.Left, Bounds.Top));

        OnRender(dc);

        dc.Pop();
    }

    protected abstract Element[] GetSubElements();

    protected abstract SKSize OnMeasure(IDrawingContext dc);

    protected abstract void OnArrange(SKSize finalSize);

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
