using SkiaSharp;

namespace Blueprints;

public abstract class Element : IInputController, IDragDropController
{
    public IBlueprintEditor? Editor { get; private set; }

    public SKPoint Position { get; set; } = SKPoint.Empty;

    public SKSize Size { get; set; } = SKSize.Empty;

    public SKRect Bounds { get; set; } = SKRect.Empty;

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
        foreach (Element element in Children())
        {
            element.Layout(dc);
        }

        Size = OnLayout(dc);
    }

    internal void Render(IDrawingContext dc)
    {
        Bounds = SKRect.Create(Position, Size);

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

    protected abstract Element[] Children();

    protected abstract SKSize OnLayout(IDrawingContext dc);

    protected abstract void OnRender(IDrawingContext dc);

    protected virtual void OnPointerEntered(PointerEventArgs args) { }

    protected virtual void OnPointerExited(PointerEventArgs args) { }

    protected virtual void OnPointerPressed(PointerEventArgs args) { }

    protected virtual void OnPointerMoved(PointerEventArgs args) { }

    protected virtual void OnPointerReleased(PointerEventArgs args) { }

    protected virtual void OnPointerWheelChanged(PointerWheelEventArgs args) { }
}
