namespace Blueprints;

public interface IInputController
{
    void PointerEntered(PointerEventArgs args);

    void PointerExited(PointerEventArgs args);

    void PointerPressed(PointerEventArgs args);

    void PointerMoved(PointerEventArgs args);

    void PointerReleased(PointerEventArgs args);

    void PointerWheelChanged(PointerWheelEventArgs args);
}
