using SkiaSharp;

namespace Blueprints;

public interface IController
{
    void PointerEntered(Pointer pointer, SKPoint position);

    void PointerExited(Pointer pointer, SKPoint position);

    void PointerPressed(Pointer pointer, SKPoint position);

    void PointerMoved(SKPoint position);

    void PointerReleased(SKPoint position);

    void PointerWheelChanged(SKPoint position, float delta);
}
