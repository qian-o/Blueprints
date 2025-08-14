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
        throw new NotImplementedException();
    }

    void IInputController.PointerExited(PointerEventArgs args)
    {
        throw new NotImplementedException();
    }

    void IInputController.PointerPressed(PointerEventArgs args)
    {
        throw new NotImplementedException();
    }

    void IInputController.PointerMoved(PointerEventArgs args)
    {
        throw new NotImplementedException();
    }

    void IInputController.PointerReleased(PointerEventArgs args)
    {
        throw new NotImplementedException();
    }

    void IInputController.PointerWheelChanged(PointerWheelEventArgs args)
    {
        throw new NotImplementedException();
    }

    protected abstract Element[] Children();

    protected abstract SKSize OnLayout(IDrawingContext dc);

    protected abstract void OnRender(IDrawingContext dc);
}
