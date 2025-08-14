using SkiaSharp;

namespace Blueprints;

public class BlueprintController(IBlueprintEditor editor) : IInputController
{
    private Element? dragedElement;
    private DragEventArgs? dragEventArgs;
    private SKPoint? lastPointerPosition;

    public void PointerPressed(PointerEventArgs args)
    {
        foreach (Element element in editor.Elements.Reverse())
        {
            if (element.HitTest(args.Position))
            {
                ((IInputController)element).PointerPressed(args);

                if (args.Handled)
                {
                    return;
                }

                break;
            }
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
        foreach (Element element in editor.Elements.Reverse())
        {
            if (element.HitTest(args.Position))
            {
                ((IInputController)element).PointerMoved(args);

                if (args.Handled)
                {
                    return;
                }

                break;
            }
        }

        if (args.Pointers.HasFlag(Pointers.LeftButton) && dragedElement is not null)
        {
            dragEventArgs ??= new();
            dragEventArgs.Position = args.Position;

            if (dragedElement.IsDragged)
            {
                ((IDragDropController)dragedElement).DragDelta(dragEventArgs);

                foreach (Element element in editor.Elements.Reverse())
                {
                    if (element.HitTest(args.Position))
                    {
                        ((IDragDropController)element).DragOver(dragEventArgs);

                        break;
                    }
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
        foreach (Element element in editor.Elements.Reverse())
        {
            if (element.HitTest(args.Position))
            {
                ((IInputController)element).PointerReleased(args);

                if (args.Handled)
                {
                    return;
                }

                break;
            }
        }

        if (dragedElement is Element { IsDragged: true } && dragEventArgs is not null)
        {
            foreach (Element element in editor.Elements.Reverse())
            {
                if (element.HitTest(args.Position))
                {
                    ((IDragDropController)element).Drop(dragEventArgs);

                    if (dragEventArgs.Handled)
                    {
                        ((IDragDropController)dragedElement).DragCompleted(dragEventArgs);

                        break;
                    }
                }
            }

            dragedElement = null;
            dragEventArgs = null;
        }

        lastPointerPosition = null;
    }

    public void PointerWheelChanged(PointerWheelEventArgs args)
    {
        foreach (Element element in editor.Elements.Reverse())
        {
            if (element.HitTest(args.Position))
            {
                ((IInputController)element).PointerWheelChanged(args);

                if (args.Handled)
                {
                    return;
                }

                break;
            }
        }

        float scale = args.Delta > 0.0f ? 1.1f : 0.9f;

        editor.Zoom *= scale;

        SKMatrix scaleMatrix = SKMatrix.CreateScale(scale, scale, args.Position.X, args.Position.Y);

        editor.X = (editor.X * scaleMatrix.ScaleX) + (args.Position.X * (1.0f - scaleMatrix.ScaleX));
        editor.Y = (editor.Y * scaleMatrix.ScaleY) + (args.Position.Y * (1.0f - scaleMatrix.ScaleY));

        editor.Invalidate();
    }
}
