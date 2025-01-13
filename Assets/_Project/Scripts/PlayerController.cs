using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public JoystickController joystickController; // Reference to the joystick
    public float speed = 5f;                      // Speed of the player

    private void Update()
    {
        if (joystickController == null) return;

        // Get input from the joystick
        Vector2 input = joystickController.GetInputVector();

        // Debug input vector
        Debug.Log($"[PlayerController] Input Vector: {input}");

        // Convert the input to 3D movement on the XZ plane
        Vector3 moveDirection = new Vector3(input.x, 0, input.y);

        // Apply movement
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        // Rotate to face movement direction (optional)
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}