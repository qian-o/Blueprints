namespace Blueprints;

public interface IDragDropController
{
    void DragStarted(DragEventArgs args);

    void DragDelta(DragEventArgs args);

    void DragOver(DragEventArgs args);

    void Drop(DragEventArgs args);

    void DragCompleted(DragEventArgs args);

    void DragCancelled(DragEventArgs args);
}
