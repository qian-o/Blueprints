using SkiaSharp;

namespace Blueprints;

public abstract class Element : IInputController, IDragDropController
{
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
    protected virtual void OnPointerEntered(PointerEventArgs args) { }

    protected virtual void OnPointerExited(PointerEventArgs args) { }

    protected virtual void OnPointerPressed(PointerEventArgs args) { }

    protected virtual void OnPointerMoved(PointerEventArgs args) { }

    protected virtual void OnPointerReleased(PointerEventArgs args) { }

    protected virtual void OnPointerWheelChanged(PointerWheelEventArgs args) { }
    #endregion

    #region DragDropController event handlers
    protected virtual void OnDragStarted(DragEventArgs args) { }

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
    void IInputController.PointerEntered(PointerEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        if (HitTest(args.Position))
        {
            foreach (Element element in Children())
            {
                ((IInputController)element).PointerEntered(args);
            }

            if (args.Handled)
            {
                return;
            }

            OnPointerEntered(args);
        }
    }

    void IInputController.PointerExited(PointerEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        if (HitTest(args.Position))
        {
            foreach (Element element in Children())
            {
                ((IInputController)element).PointerExited(args);
            }

            if (args.Handled)
            {
                return;
            }

            OnPointerExited(args);
        }
    }

    void IInputController.PointerPressed(PointerEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        if (HitTest(args.Position))
        {
            foreach (Element element in Children())
            {
                ((IInputController)element).PointerPressed(args);
            }

            if (args.Handled)
            {
                return;
            }

            OnPointerPressed(args);
        }
    }

    void IInputController.PointerMoved(PointerEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        if (HitTest(args.Position))
        {
            foreach (Element element in Children())
            {
                ((IInputController)element).PointerMoved(args);
            }

            if (args.Handled)
            {
                return;
            }

            OnPointerMoved(args);
        }
    }

    void IInputController.PointerReleased(PointerEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        if (HitTest(args.Position))
        {
            foreach (Element element in Children())
            {
                ((IInputController)element).PointerReleased(args);
            }

            if (args.Handled)
            {
                return;
            }

            OnPointerReleased(args);
        }
    }

    void IInputController.PointerWheelChanged(PointerWheelEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        if (HitTest(args.Position))
        {
            foreach (Element element in Children())
            {
                ((IInputController)element).PointerWheelChanged(args);
            }

            if (args.Handled)
            {
                return;
            }

            OnPointerWheelChanged(args);
        }
    }
    #endregion

    #region IDragDropController implementation
    void IDragDropController.DragStarted(DragEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        if (HitTest(args.Position))
        {
            foreach (Element element in Children())
            {
                ((IDragDropController)element).DragStarted(args);
            }

            if (args.Handled)
            {
                return;
            }

            IsDragged = true;

            OnDragStarted(args);
        }
    }

    void IDragDropController.DragOver(DragEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        if (HitTest(args.Position))
        {
            foreach (Element element in Children())
            {
                ((IDragDropController)element).DragOver(args);
            }

            if (args.Handled)
            {
                return;
            }

            OnDragOver(args);
        }
    }

    void IDragDropController.Drop(DragEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        if (HitTest(args.Position))
        {
            foreach (Element element in Children())
            {
                ((IDragDropController)element).Drop(args);
            }

            if (args.Handled)
            {
                return;
            }

            OnDrop(args);
        }
    }

    void IDragDropController.DragCompleted(DragEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        if (HitTest(args.Position))
        {
            foreach (Element element in Children())
            {
                ((IDragDropController)element).DragCompleted(args);
            }

            if (args.Handled)
            {
                return;
            }

            IsDragged = false;

            OnDragCompleted(args);
        }
    }
    #endregion
}
