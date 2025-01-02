using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform joystickBackground;
    public RectTransform joystickHandle;
    public float joystickRange = 75f; // Maximum distance the handle can move

    private Vector2 inputVector; // Current input vector

    public Vector2 GetInputVector()
    {
        return inputVector;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("[JoystickController] PointerDown detected.");

        // Reset joystick handle and input vector
        joystickHandle.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;

        Debug.Log("[JoystickController] Joystick initialized for dragging.");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("[JoystickController] Dragging detected.");

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        // Clamp handle movement within joystick range
        Vector2 clampedPoint = Vector2.ClampMagnitude(localPoint, joystickRange);
        inputVector = clampedPoint / joystickRange; // Normalize input vector

        // Move the joystick handle
        joystickHandle.anchoredPosition = clampedPoint;

        Debug.Log($"[JoystickController] Input Vector: {inputVector}, Handle Position: {joystickHandle.anchoredPosition}");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("[JoystickController] PointerUp detected.");

        // Reset joystick handle and input vector
        joystickHandle.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
    }
}