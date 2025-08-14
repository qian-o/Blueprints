namespace Blueprints;

public interface IInputController
{
    void PointerPressed(PointerEventArgs args);

    void PointerMoved(PointerEventArgs args);

    void PointerReleased(PointerEventArgs args);

    void PointerWheelChanged(PointerWheelEventArgs args);
}
