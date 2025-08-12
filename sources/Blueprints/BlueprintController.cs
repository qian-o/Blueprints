using SkiaSharp;

namespace Blueprints;

public class BlueprintController(IBlueprintEditor editor) : IController
{
    private SKPoint? lastPointerPosition;

    public void PointerEntered(PointerEventArgs args)
    {
        foreach (Node node in editor.Nodes)
        {
            if (node.ScreenBounds.Contains(args.Position))
            {
                ((IController)node).PointerEntered(args);
            }
        }
    }

    public void PointerExited(PointerEventArgs args)
    {
        foreach (Node node in editor.Nodes)
        {
            if (node.ScreenBounds.Contains(args.Position))
            {
                ((IController)node).PointerExited(args);
            }
        }
    }

    public void PointerPressed(PointerEventArgs args)
    {
        foreach (Node node in editor.Nodes)
        {
            if (node.ScreenBounds.Contains(args.Position))
            {
                ((IController)node).PointerPressed(args);
            }
        }

        if (args.Handled)
        {
            return;
        }

        if (args.Pointers.HasFlag(PointerFlags.RightButton))
        {
            lastPointerPosition = args.Position;
        }
    }

    public void PointerMoved(PointerEventArgs args)
    {
        foreach (Node node in editor.Nodes)
        {
            if (node.ScreenBounds.Contains(args.Position))
            {
                ((IController)node).PointerMoved(args);
            }
        }

        if (args.Handled)
        {
            return;
        }

        if (lastPointerPosition is not null)
        {
            float deltaX = args.Position.X - lastPointerPosition.Value.X;
            float deltaY = args.Position.Y - lastPointerPosition.Value.Y;

            editor.X += deltaX;
            editor.Y += deltaY;

            lastPointerPosition = args.Position;

            editor.Invalidate();
        }
    }

    public void PointerReleased(PointerEventArgs args)
    {
        foreach (Node node in editor.Nodes)
        {
            if (node.ScreenBounds.Contains(args.Position))
            {
                ((IController)node).PointerReleased(args);
            }
        }

        if (args.Handled)
        {
            return;
        }

        lastPointerPosition = null;
    }

    public void PointerWheelChanged(PointerWheelEventArgs args)
    {
        foreach (Node node in editor.Nodes)
        {
            if (node.ScreenBounds.Contains(args.Position))
            {
                ((IController)node).PointerWheelChanged(args);
            }
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
