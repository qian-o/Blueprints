using SkiaSharp;

namespace Blueprints;

public class BlueprintController(IBlueprintEditor editor) : IInputController
{
    private SKPoint? lastPointerPosition;

    public void PointerEntered(PointerEventArgs args)
    {
    }

    public void PointerExited(PointerEventArgs args)
    {
    }

    public void PointerPressed(PointerEventArgs args)
    {
        if (args.Pointers.HasFlag(Pointers.RightButton))
        {
            lastPointerPosition = args.Position;
        }
    }

    public void PointerMoved(PointerEventArgs args)
    {
        if (lastPointerPosition is not null)
        {
            editor.X += args.Position.X - lastPointerPosition.Value.X;
            editor.Y += args.Position.Y - lastPointerPosition.Value.Y;

            lastPointerPosition = args.Position;

            editor.Invalidate();
        }
    }

    public void PointerReleased(PointerEventArgs args)
    {
        lastPointerPosition = null;
    }

    public void PointerWheelChanged(PointerWheelEventArgs args)
    {
        float scale = args.Delta > 0.0f ? 1.1f : 0.9f;

        editor.Zoom *= scale;

        SKMatrix scaleMatrix = SKMatrix.CreateScale(scale, scale, args.Position.X, args.Position.Y);

        editor.X = (editor.X * scaleMatrix.ScaleX) + (args.Position.X * (1.0f - scaleMatrix.ScaleX));
        editor.Y = (editor.Y * scaleMatrix.ScaleY) + (args.Position.Y * (1.0f - scaleMatrix.ScaleY));

        editor.Invalidate();
    }
}
