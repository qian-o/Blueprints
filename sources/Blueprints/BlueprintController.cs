using SkiaSharp;

namespace Blueprints;

public class BlueprintController(IBlueprintEditor editor) : IInputController
{
    private Element? dragedElement;
    private DragEventArgs? dragEventArgs;
    private SKPoint? lastScreenPosition;

    public void PointerEntered(PointerEventArgs args)
    {
        PointerMoved(args);
    }

    public void PointerExited(PointerEventArgs args)
    {
        PointerMoved(args);
    }

    public void PointerPressed(PointerEventArgs args)
    {
        Element? hitElement = editor.Elements.Reverse().FirstOrDefault(e => e.HitTest(args.WorldPosition));

        (hitElement as IInputController)?.PointerPressed(args);

        if (args.Handled)
        {
            return;
        }

        if (args.Pointers.HasFlag(Pointers.LeftButton))
        {
            dragedElement = hitElement;
        }

        if (args.Pointers.HasFlag(Pointers.RightButton))
        {
            lastScreenPosition = args.ScreenPosition;
        }
    }

    public void PointerMoved(PointerEventArgs args)
    {
        Element? hitElement = editor.Elements.Reverse().FirstOrDefault(e => e.HitTest(args.WorldPosition));

        if (hitElement?.IsPointerOver is false)
        {
            ((IInputController)hitElement).PointerEntered(args);
        }

        foreach (Element element in editor.Elements.Reverse())
        {
            if (element != hitElement && element.IsPointerOver)
            {
                ((IInputController)element).PointerExited(args);
            }
        }

        if (args.Pointers.HasFlag(Pointers.LeftButton) && dragedElement is not null)
        {
            if (dragEventArgs is null)
            {
                dragEventArgs = new()
                {
                    WorldPosition = args.WorldPosition,
                    ScreenPosition = args.ScreenPosition
                };

                ((IDragDropController)dragedElement).DragStarted(dragEventArgs);
            }
            else
            {
                dragEventArgs.WorldPosition = args.WorldPosition;
                dragEventArgs.ScreenPosition = args.ScreenPosition;

                ((IDragDropController)dragedElement).DragDelta(dragEventArgs);

                (hitElement as IDragDropController)?.DragOver(dragEventArgs);
            }
        }
        else
        {
            (hitElement as IInputController)?.PointerMoved(args);

            if (args.Handled)
            {
                return;
            }

            if (args.Pointers.HasFlag(Pointers.RightButton) && lastScreenPosition is not null)
            {
                editor.X += args.ScreenPosition.X - lastScreenPosition.Value.X;
                editor.Y += args.ScreenPosition.Y - lastScreenPosition.Value.Y;

                lastScreenPosition = args.ScreenPosition;

                editor.Invalidate();
            }
        }
    }

    public void PointerReleased(PointerEventArgs args)
    {
        Element[] reverseElements = [.. editor.Elements.Reverse()];

        if (dragedElement is not null && dragEventArgs is not null)
        {
            foreach (Element element in reverseElements)
            {
                if (element.HitTest(args.WorldPosition))
                {
                    ((IDragDropController)element).Drop(dragEventArgs);

                    if (dragEventArgs.Handled)
                    {
                        ((IDragDropController)dragedElement).DragCompleted(dragEventArgs);

                        break;
                    }
                }
            }

            if (!dragEventArgs.Handled)
            {
                ((IDragDropController)dragedElement).DragCancelled(dragEventArgs);
            }

            dragedElement = null;
            dragEventArgs = null;
        }
        else
        {
            foreach (Element element in reverseElements)
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

            lastScreenPosition = null;
        }
    }

    public void PointerWheelChanged(PointerWheelEventArgs args)
    {
        foreach (Element element in editor.Elements.Reverse())
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

        float scale = args.Delta > 0.0f ? 1.1f : 0.9f;

        editor.Zoom *= scale;

        SKMatrix scaleMatrix = SKMatrix.CreateScale(scale, scale, args.ScreenPosition.X, args.ScreenPosition.Y);

        editor.X = (editor.X * scaleMatrix.ScaleX) + (args.ScreenPosition.X * (1.0f - scaleMatrix.ScaleX));
        editor.Y = (editor.Y * scaleMatrix.ScaleY) + (args.ScreenPosition.Y * (1.0f - scaleMatrix.ScaleY));

        editor.Invalidate();
    }
}
