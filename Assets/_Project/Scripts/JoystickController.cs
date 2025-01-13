using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform joystickBackground; // Background of the joystick
    public RectTransform joystickHandle;     // Handle of the joystick
    public float joystickRange = 75f;        // Maximum range for the handle to move

    private Vector2 inputVector = Vector2.zero;

    public Vector2 GetInputVector()
    {
        return inputVector;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData); // Begin dragging immediately
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        // Clamp the handle's movement within the joystick range
        Vector2 clampedPosition = Vector2.ClampMagnitude(localPoint, joystickRange);
        joystickHandle.anchoredPosition = clampedPosition;

        // Normalize the input vector (value between -1 and 1)
        inputVector = clampedPosition / joystickRange;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Reset the handle and input vector
        joystickHandle.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
    }
}