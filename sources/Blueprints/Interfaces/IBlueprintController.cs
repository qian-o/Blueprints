using SkiaSharp;

namespace Blueprints;

public interface IBlueprintController
{
    void PointerEntered(BlueprintPointer pointer, SKPoint position);

    void PointerExited(BlueprintPointer pointer, SKPoint position);

    void PointerPressed(BlueprintPointer pointer, SKPoint position);

    void PointerMoved(SKPoint position);

    void PointerReleased(SKPoint position);

    void PointerWheelChanged(SKPoint position, float delta);
}
