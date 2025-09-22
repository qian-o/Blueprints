using SkiaSharp;

namespace Blueprints;

public class BlueprintController(IBlueprintEditor editor) : IInputController
{
    private SKPoint? lastScreenPosition;
    private DragEventArgs? dragEventArgs;

    public void PointerMoved(PointerEventArgs args)
    {
        Element[] elements = [.. editor.Elements.Reverse()];

        foreach (Element element in elements)
        {
            ((IInputController)element).PointerMoved(args);
        }

        editor.Cursor = (args.HoveredElement?.Cursor) ?? Cursor.Arrow;

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

        if (args.Pointers.HasFlag(Pointers.LeftButton) && dragEventArgs is not null)
        {
            dragEventArgs.ScreenPosition = args.ScreenPosition;
            dragEventArgs.WorldPosition = args.WorldPosition;

            ((IDragDropController)dragEventArgs.Element!).DragDelta(dragEventArgs);

            dragEventArgs.Handled = false;

            foreach (Element element in elements)
            {
                ((IDragDropController)element).DragOver(dragEventArgs);
            }

            dragEventArgs.Handled = false;
        }
    }

    public void PointerPressed(PointerEventArgs args)
    {
        Element[] elements = [.. editor.Elements.Reverse()];

        foreach (Element element in elements)
        {
            ((IInputController)element).PointerPressed(args);
        }

        if (args.Handled)
        {
            return;
        }

        if (args.Pointers.HasFlag(Pointers.RightButton))
        {
            lastScreenPosition = args.ScreenPosition;
        }

        if (args.Pointers.HasFlag(Pointers.LeftButton))
        {
            dragEventArgs = new()
            {
                ScreenPosition = args.ScreenPosition,
                WorldPosition = args.WorldPosition
            };

            foreach (Element element in elements)
            {
                ((IDragDropController)element).DragStarted(dragEventArgs);

                if (dragEventArgs.Element is not null)
                {
                    break;
                }
            }

            if (dragEventArgs.Element is null)
            {
                dragEventArgs = null;
            }
        }
    }

    public void PointerReleased(PointerEventArgs args)
    {
        Element[] elements = [.. editor.Elements.Reverse()];

        foreach (Element element in elements)
        {
            ((IInputController)element).PointerReleased(args);
        }

        if (args.Handled)
        {
            return;
        }

        lastScreenPosition = null;

        if (!args.Pointers.HasFlag(Pointers.LeftButton) && dragEventArgs is not null)
        {
            dragEventArgs.ScreenPosition = args.ScreenPosition;
            dragEventArgs.WorldPosition = args.WorldPosition;

            foreach (Element element in elements)
            {
                ((IDragDropController)element).Drop(dragEventArgs);
            }

            if (dragEventArgs.Handled)
            {
                ((IDragDropController)dragEventArgs.Element!).DragCompleted(dragEventArgs);
            }
            else
            {
                ((IDragDropController)dragEventArgs.Element!).DragCancelled(dragEventArgs);
            }

            dragEventArgs = null;
        }
    }

    public void PointerWheelChanged(PointerWheelEventArgs args)
    {
        foreach (Element element in editor.Elements.Reverse())
        {
            ((IInputController)element).PointerWheelChanged(args);
        }

        if (args.Handled)
        {
            return;
        }

        float scale = args.Delta > 0.0f ? 1.1f : 0.9f;

        editor.Zoom *= scale;

        SKMatrix scaleMatrix = SKMatrix.CreateScale(scale, scale, args.ScreenPosition.X, args.ScreenPosition.Y);

        editor.X = (editor.X * scaleMatrix.ScaleX) + (args.ScreenPosition.X * (1.0f - scaleMatrix.ScaleX));
        editor.Y = (editor.Y * scaleMatrix.ScaleY) + (args.ScreenPosition.Y * (1.0f - scaleMatrix.ScaleY));

        editor.Invalidate();
    }
}
