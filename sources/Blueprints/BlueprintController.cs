using SkiaSharp;

namespace Blueprints;

public class BlueprintController(IBlueprintEditor editor) : IController
{
    private SKPoint? lastPointerPosition;

    public void PointerEntered(PointerFlags pointers, SKPoint position)
    {
        foreach (Node node in editor.Nodes)
        {
            if (node.Bounds.Contains(position))
            {
                node.PointerEntered(pointers, position);
            }
        }
    }

    public void PointerExited(PointerFlags pointers, SKPoint position)
    {
        foreach (Node node in editor.Nodes)
        {
            if (node.Bounds.Contains(position))
            {
                node.PointerExited(pointers, position);
            }
        }
    }

    public void PointerPressed(PointerFlags pointers, SKPoint position)
    {
        if (pointers.HasFlag(PointerFlags.RightButton))
        {
            lastPointerPosition = position;
        }

        foreach (Node node in editor.Nodes)
        {
            if (node.Bounds.Contains(position))
            {
                node.PointerPressed(pointers, position);
            }
        }
    }

    public void PointerMoved(PointerFlags pointers, SKPoint position)
    {
        if (lastPointerPosition is not null)
        {
            float deltaX = position.X - lastPointerPosition.Value.X;
            float deltaY = position.Y - lastPointerPosition.Value.Y;

            editor.X += deltaX;
            editor.Y += deltaY;

            lastPointerPosition = position;

            editor.Invalidate();
        }

        foreach (Node node in editor.Nodes)
        {
            if (node.Bounds.Contains(position))
            {
                node.PointerMoved(pointers, position);
            }
        }
    }

    public void PointerReleased(PointerFlags pointers, SKPoint position)
    {
        lastPointerPosition = null;

        foreach (Node node in editor.Nodes)
        {
            if (node.Bounds.Contains(position))
            {
                node.PointerReleased(pointers, position);
            }
        }
    }

    public void PointerWheelChanged(float delta, SKPoint position)
    {
        float scale = delta > 0.0f ? 1.1f : 0.9f;

        editor.Zoom *= scale;

        SKMatrix scaleMatrix = SKMatrix.CreateScale(scale, scale, position.X, position.Y);

        editor.X = (editor.X * scaleMatrix.ScaleX) + (position.X * (1.0f - scaleMatrix.ScaleX));
        editor.Y = (editor.Y * scaleMatrix.ScaleY) + (position.Y * (1.0f - scaleMatrix.ScaleY));

        editor.Invalidate();

        foreach (Node node in editor.Nodes)
        {
            if (node.Bounds.Contains(position))
            {
                node.PointerWheelChanged(delta, position);
            }
        }
    }
}
