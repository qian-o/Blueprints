using System.Diagnostics;
using SkiaSharp;

namespace Blueprints;

public class BlueprintEditorController(IBlueprintEditor editor) : IBlueprintController
{
    private SKPoint? lastPointerPosition;

    public void PointerPressed(BlueprintPointer pointer, SKPoint position)
    {
        if (pointer is BlueprintPointer.RightButton)
        {
            lastPointerPosition = position;
        }
    }

    public void PointerMoved(SKPoint position)
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
    }

    public void PointerReleased(SKPoint position)
    {
        lastPointerPosition = null;

        Debug.WriteLine(position);
    }

    public void PointerWheelChanged(SKPoint position, float delta)
    {
        float scale = delta > 0.0f ? 1.1f : 0.9f;

        editor.Zoom *= scale;

        SKMatrix scaleMatrix = SKMatrix.CreateScale(scale, scale, position.X, position.Y);

        editor.X = (editor.X * scaleMatrix.ScaleX) + (position.X * (1.0f - scaleMatrix.ScaleX));
        editor.Y = (editor.Y * scaleMatrix.ScaleY) + (position.Y * (1.0f - scaleMatrix.ScaleY));

        editor.Invalidate();
    }
}
