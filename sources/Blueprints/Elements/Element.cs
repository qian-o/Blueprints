using SkiaSharp;

namespace Blueprints;

public abstract class Element : IInputController, IDragDropController
{
    private SKPoint? lastWorldPosition;

    public IBlueprintEditor? Editor { get; private set; }

    public SKPoint Position { get; set; } = SKPoint.Empty;

    public SKSize Size { get; private set; } = SKSize.Empty;

    public SKRect Bounds { get; private set; } = SKRect.Empty;

    public bool IsDragged { get; set; }

    public IBlueprintStyle Style => Editor?.Style ?? throw new InvalidOperationException("Editor is not bound to this element.");

    public virtual bool HitTest(SKPoint position)
    {
        if (Bounds.IsEmpty)
        {
            return false;
        }

        return Bounds.Contains(position);
    }

    public void Invalidate()
    {
        Editor?.Invalidate();
    }

    internal void Bind(IBlueprintEditor editor)
    {
        Editor = editor;

        foreach (Element element in Children())
        {
            element.Bind(editor);
        }
    }

    internal void Layout(IDrawingContext dc)
    {
        Measure(dc);
        Arrange();
    }

    internal void Render(IDrawingContext dc)
    {
        if (Bounds.IsEmpty)
        {
            return;
        }

        OnRender(dc);

        foreach (Element element in Children())
        {
            element.Render(dc);
        }
    }

    protected abstract Element[] Children();

    protected abstract SKSize OnMeasure(IDrawingContext dc);

    protected abstract void OnArrange();

    protected abstract void OnRender(IDrawingContext dc);

    #region InputController event handlers
    protected virtual void OnPointerPressed(PointerEventArgs args) { }

    protected virtual void OnPointerMoved(PointerEventArgs args) { }

    protected virtual void OnPointerReleased(PointerEventArgs args) { }

    protected virtual void OnPointerWheelChanged(PointerWheelEventArgs args) { }
    #endregion

    #region DragDropController event handlers
    protected virtual void OnDragStarted(DragEventArgs args)
    {
        lastWorldPosition = args.WorldPosition;
    }

    protected virtual void OnDragDelta(DragEventArgs args)
    {
        if (lastWorldPosition.HasValue)
        {
            Position += args.WorldPosition - lastWorldPosition.Value;

            lastWorldPosition = args.WorldPosition;

            Invalidate();
        }
    }

    protected virtual void OnDragOver(DragEventArgs args) { }

    protected virtual void OnDrop(DragEventArgs args) { }

    protected virtual void OnDragCompleted(DragEventArgs args) { }
    #endregion

    private void Measure(IDrawingContext dc)
    {
        foreach (Element element in Children())
        {
            element.Measure(dc);
        }

        Size = OnMeasure(dc);
    }

    private void Arrange()
    {
        Bounds = SKRect.Create(Position, Size);

        OnArrange();

        foreach (Element element in Children())
        {
            element.Arrange();
        }
    }

    #region IInputController implementation
    void IInputController.PointerPressed(PointerEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        foreach (Element element in Children())
        {
            if (element.HitTest(args.WorldPosition))
            {
                ((IInputController)element).PointerPressed(args);

                if (args.Handled)
                {
                    return;
                }

                break;
            }
        }

        OnPointerPressed(args);
    }

    void IInputController.PointerMoved(PointerEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        foreach (Element element in Children())
        {
            if (element.HitTest(args.WorldPosition))
            {
                ((IInputController)element).PointerMoved(args);

                if (args.Handled)
                {
                    return;
                }

                break;
            }
        }

        OnPointerMoved(args);
    }

    void IInputController.PointerReleased(PointerEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        foreach (Element element in Children())
        {
            if (element.HitTest(args.WorldPosition))
            {
                ((IInputController)element).PointerReleased(args);

                if (args.Handled)
                {
                    return;
                }

                break;
            }
        }

        OnPointerReleased(args);
    }

    void IInputController.PointerWheelChanged(PointerWheelEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        foreach (Element element in Children())
        {
            if (element.HitTest(args.WorldPosition))
            {
                ((IInputController)element).PointerWheelChanged(args);

                if (args.Handled)
                {
                    return;
                }

                break;
            }
        }

        OnPointerWheelChanged(args);
    }
    #endregion

    #region IDragDropController implementation
    void IDragDropController.DragStarted(DragEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        foreach (Element element in Children())
        {
            if (element.HitTest(args.WorldPosition))
            {
                ((IDragDropController)element).DragStarted(args);

                return;
            }
        }

        IsDragged = true;

        OnDragStarted(args);
    }

    void IDragDropController.DragDelta(DragEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        foreach (Element element in Children())
        {
            if (element.IsDragged)
            {
                ((IDragDropController)element).DragDelta(args);

                return;
            }
        }

        OnDragDelta(args);
    }

    void IDragDropController.DragOver(DragEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        foreach (Element element in Children())
        {
            if (element.HitTest(args.WorldPosition))
            {
                ((IDragDropController)element).DragOver(args);

                break;
            }
        }

        OnDragOver(args);
    }

    void IDragDropController.Drop(DragEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        foreach (Element element in Children())
        {
            if (element.HitTest(args.WorldPosition))
            {
                ((IDragDropController)element).Drop(args);

                if (args.Handled)
                {
                    return;
                }

                break;
            }
        }

        OnDrop(args);
    }

    void IDragDropController.DragCompleted(DragEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        foreach (Element element in Children())
        {
            if (element.IsDragged)
            {
                ((IDragDropController)element).DragCompleted(args);

                return;
            }
        }

        IsDragged = false;

        OnDragCompleted(args);
    }
    #endregion
}
