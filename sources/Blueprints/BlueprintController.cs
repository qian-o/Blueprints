using SkiaSharp;

namespace Blueprints;

public class BlueprintController(IBlueprintEditor editor) : IInputController
{
    private SKPoint? lastScreenPosition;

    public void PointerMoved(PointerEventArgs args)
    {
        foreach (Element element in editor.Elements.Reverse())
        {
            ((IInputController)element).PointerMoved(args);
        }

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

    public void PointerPressed(PointerEventArgs args)
    {
        foreach (Element element in editor.Elements.Reverse())
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
    }

    public void PointerReleased(PointerEventArgs args)
    {
        foreach (Element element in editor.Elements.Reverse())
        {
            ((IInputController)element).PointerReleased(args);
        }

        if (args.Handled)
        {
            return;
        }

        lastScreenPosition = null;
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
