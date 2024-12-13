using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public JoystickController joystickController; // Reference to the JoystickController
    public float speed = 5f; // Movement speed of the player

    private void Start()
    {
        // Ensure the JoystickController is assigned
        if (joystickController == null)
        {
            Debug.LogError("JoystickController is not assigned in the PlayerController script!");
        }
    }

    private void Update()
    {
        if (joystickController == null) return;

        // Get the input vector from the joystick
        Vector2 input = joystickController.GetInputVector();

        // Convert the input to a 3D movement vector
        Vector3 move = new Vector3(input.x, 0, input.y);

        // Apply movement to the player
        transform.Translate(move * speed * Time.deltaTime, Space.World);

        // Optional: Rotate the player to face the movement direction
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}