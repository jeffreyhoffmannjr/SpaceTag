using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform joystickBackground; // Background of the joystick
    public RectTransform joystickHandle;    // Handle of the joystick
    public float joystickRange = 75f;       // Maximum distance the handle can move

    private Vector2 inputVector;

    // Public method to get the current input vector
    public Vector2 GetInputVector()
    {
        return inputVector;
    }

    // Called when the joystick is pressed
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    // Called when the joystick is being dragged
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint))
        {
            // Clamp the joystick handle movement within the joystick range
            localPoint = Vector2.ClampMagnitude(localPoint, joystickRange);
            inputVector = localPoint / joystickRange;

            // Move the joystick handle
            joystickHandle.anchoredPosition = localPoint;
        }
    }

    // Called when the joystick is released
    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero; // Reset the input vector
        joystickHandle.anchoredPosition = Vector2.zero; // Reset the handle position
    }
}