namespace Blueprints;

public interface IInputController
{
    void PointerMoved(PointerEventArgs args);

    void PointerPressed(PointerEventArgs args);

    void PointerReleased(PointerEventArgs args);

    void PointerWheelChanged(PointerWheelEventArgs args);
}
