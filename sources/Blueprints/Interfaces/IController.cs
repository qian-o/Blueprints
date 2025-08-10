using SkiaSharp;

namespace Blueprints;

public interface IController
{
    void PointerEntered(PointerFlags pointers, SKPoint position);

    void PointerExited(PointerFlags pointers, SKPoint position);

    void PointerPressed(PointerFlags pointers, SKPoint position);

    void PointerMoved(PointerFlags pointers, SKPoint position);

    void PointerReleased(PointerFlags pointers, SKPoint position);

    void PointerWheelChanged(SKPoint position, float delta);
}
