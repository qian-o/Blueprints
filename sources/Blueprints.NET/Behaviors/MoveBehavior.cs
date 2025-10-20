using SkiaSharp;

namespace Blueprints.NET;

public class MoveBehavior : Behavior
{
    public static readonly MoveBehavior Instance = new();

    private readonly Dictionary<Element, SKPoint> lastWorldPositions = [];

    protected override void PointerEntered(object? sender, PointerEventArgs e)
    {
        if (sender is not Element element)
        {
            return;
        }

        element.Cursor = Cursor.SizeAll;
    }

    protected override void PointerExited(object? sender, PointerEventArgs e)
    {
        if (sender is not Element element)
        {
            return;
        }

        element.Cursor = Cursor.Arrow;
    }

    protected override void DragStarted(object? sender, DragEventArgs args)
    {
        if (sender is not Element element)
        {
            return;
        }

        lastWorldPositions[element] = args.WorldPosition;
    }

    protected override void DragDelta(object? sender, DragEventArgs args)
    {
        if (sender is not Element element)
        {
            return;
        }

        if (lastWorldPositions.TryGetValue(element, out SKPoint lastWorldPosition))
        {
            element.Position += args.WorldPosition - lastWorldPosition;

            lastWorldPositions[element] = args.WorldPosition;
        }
    }

    protected override void DragCancelled(object? sender, DragEventArgs args)
    {
        if (sender is not Element element)
        {
            return;
        }

        lastWorldPositions.Remove(element);
    }
}
