using SkiaSharp;

namespace Blueprints;

public class BlueprintController(IBlueprintEditor editor) : IInputController
{
    private Element? dragedElement;
    private DragEventArgs? dragEventArgs;
    private SKPoint? lastPointerPosition;

    public void PointerEntered(PointerEventArgs args)
    {
        foreach (Element element in editor.Elements)
        {
            ((IInputController)element).PointerPressed(args);
        }
    }

    public void PointerExited(PointerEventArgs args)
    {
        foreach (Element element in editor.Elements)
        {
            ((IInputController)element).PointerExited(args);
        }
    }

    public void PointerPressed(PointerEventArgs args)
    {
        foreach (Element element in editor.Elements)
        {
            ((IInputController)element).PointerPressed(args);
        }

        if (args.Handled)
        {
            return;
        }

        if (args.Pointers.HasFlag(Pointers.LeftButton))
        {
            dragedElement = editor.Elements.FirstOrDefault(e => e.HitTest(args.Position));
        }
        else if (args.Pointers.HasFlag(Pointers.RightButton))
        {
            lastPointerPosition = args.Position;
        }
    }

    public void PointerMoved(PointerEventArgs args)
    {
        foreach (Element element in editor.Elements)
        {
            ((IInputController)element).PointerMoved(args);
        }

        if (args.Handled)
        {
            return;
        }

        if (args.Pointers.HasFlag(Pointers.LeftButton) && dragedElement is not null)
        {
            dragEventArgs ??= new();
            dragEventArgs.Position = args.Position;

            if (dragedElement.IsDragged)
            {
                foreach (Element element in editor.Elements)
                {
                    ((IDragDropController)element).DragOver(dragEventArgs);
                }
            }
            else
            {
                ((IDragDropController)dragedElement).DragStarted(dragEventArgs);
            }
        }
        else if (args.Pointers.HasFlag(Pointers.RightButton) && lastPointerPosition is not null)
        {
            editor.X += args.Position.X - lastPointerPosition.Value.X;
            editor.Y += args.Position.Y - lastPointerPosition.Value.Y;

            lastPointerPosition = args.Position;

            editor.Invalidate();
        }
    }

    public void PointerReleased(PointerEventArgs args)
    {
        foreach (Element element in editor.Elements)
        {
            ((IInputController)element).PointerReleased(args);
        }

        if (args.Handled)
        {
            return;
        }

        if (dragedElement is Element { IsDragged: true } && dragEventArgs is not null)
        {
            foreach (Element element in editor.Elements)
            {
                ((IDragDropController)element).Drop(dragEventArgs);
            }

            ((IDragDropController)dragedElement).DragCompleted(dragEventArgs);

            dragedElement = null;
            dragEventArgs = null;
        }

        lastPointerPosition = null;
    }

    public void PointerWheelChanged(PointerWheelEventArgs args)
    {
        foreach (Element element in editor.Elements)
        {
            ((IInputController)element).PointerWheelChanged(args);
        }

        if (args.Handled)
        {
            return;
        }

        float scale = args.Delta > 0.0f ? 1.1f : 0.9f;

        editor.Zoom *= scale;

        SKMatrix scaleMatrix = SKMatrix.CreateScale(scale, scale, args.Position.X, args.Position.Y);

        editor.X = (editor.X * scaleMatrix.ScaleX) + (args.Position.X * (1.0f - scaleMatrix.ScaleX));
        editor.Y = (editor.Y * scaleMatrix.ScaleY) + (args.Position.Y * (1.0f - scaleMatrix.ScaleY));

        editor.Invalidate();
    }
}
