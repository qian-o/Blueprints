namespace Blueprints;

public abstract class Behavior
{
    public void Attach(Element element)
    {
        element.PointerPressed += PointerPressed;
        element.PointerMoved += PointerMoved;
        element.PointerReleased += PointerReleased;
        element.PointerWheelChanged += PointerWheelChanged;
        element.DragStarted += DragStarted;
        element.DragDelta += DragDelta;
        element.DragOver += DragOver;
        element.Drop += Drop;
        element.DragCompleted += DragCompleted;
        element.DragCancelled += DragCancelled;
    }

    public void Detach(Element element)
    {
        element.PointerPressed -= PointerPressed;
        element.PointerMoved -= PointerMoved;
        element.PointerReleased -= PointerReleased;
        element.PointerWheelChanged -= PointerWheelChanged;
        element.DragStarted -= DragStarted;
        element.DragDelta -= DragDelta;
        element.DragOver -= DragOver;
        element.Drop -= Drop;
        element.DragCompleted -= DragCompleted;
        element.DragCancelled -= DragCancelled;
    }

    protected virtual void PointerPressed(object? sender, PointerEventArgs args) { }

    protected virtual void PointerMoved(object? sender, PointerEventArgs args) { }

    protected virtual void PointerReleased(object? sender, PointerEventArgs args) { }

    protected virtual void PointerWheelChanged(object? sender, PointerWheelEventArgs args) { }

    protected virtual void DragStarted(object? sender, DragEventArgs args) { }

    protected virtual void DragDelta(object? sender, DragEventArgs args) { }

    protected virtual void DragOver(object? sender, DragEventArgs args) { }

    protected virtual void Drop(object? sender, DragEventArgs args) { }

    protected virtual void DragCompleted(object? sender, DragEventArgs args) { }

    protected virtual void DragCancelled(object? sender, DragEventArgs args) { }
}
