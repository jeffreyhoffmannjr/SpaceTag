using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public JoystickController joystickController;
    public float speed = 5f;

    private void Update()
    {
        if (joystickController == null) return;

        // Get input vector from the joystick
        Vector2 input = joystickController.GetInputVector();

        // Debug input vector
        Debug.Log($"[PlayerController] Input Vector: {input}");

        // Convert input to 3D movement
        Vector3 move = new Vector3(input.x, 0, input.y);

        // Move the player
        transform.Translate(move * speed * Time.deltaTime, Space.World);

        // Rotate player to face movement direction
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}