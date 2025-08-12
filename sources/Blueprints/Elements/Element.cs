using SkiaSharp;

namespace Blueprints;

public abstract partial class Element : IController
{
    public IBlueprintEditor? Editor { get; private set; }

    public float Width { get; set; } = float.NaN;

    public float Height { get; set; } = float.NaN;

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

        OnRender(dc);

        foreach (Element element in GetSubElements())
        {
            element.OnRender(dc);
        }
    }

    private void Measure(IDrawingContext dc)
    {
        foreach (Element element in GetSubElements())
        {
            element.Measure(dc);
        }

        DesiredSize = OnMeasure(dc);
    }

    private void Arrange(SKRect finalBounds)
    {
        Bounds = finalBounds;

        OnArrange(finalBounds.Size);
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
