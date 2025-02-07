using UnityEngine;
using System.Collections;

public class TreeController : MonoBehaviour
{
    public JoystickController joystickController; // Reference to the joystick
    public float speed = 5f; // Normal movement speed
    public float dashDistance = 5f; // How far the player dashes
    public float dashSpeedMultiplier = 2f; // Speed boost after dashing
    public float dashDuration = 2f; // How long the speed boost lasts
    public float dashCooldown = 7f; // Cooldown time between dashes

    private bool canDash = true; // Prevents dashing when on cooldown
    private bool isDashing = false; // Checks if the player is mid-dash

    public bool isInfernal = false; // Determines if this is an Infernal player
    public GameObject fireballPrefab; // Fireball for Infernal's auto-attack
    public float fireballCooldown = 1.5f; // Infernal's auto-attack cooldown
    public float fireballSpeed = 8f; // Speed of the fireball
    private bool canShootFireball = true;

    private void Update()
    {
        if (joystickController == null) return;

        // Get the joystick input vector
        Vector2 input = joystickController.GetInputVector();

        // Convert the input to a 3D movement vector
        Vector3 move = new Vector3(input.x, 0, input.y);

        // Move the player
        float currentSpeed = isDashing ? speed * dashSpeedMultiplier : speed;
        transform.Translate(move * currentSpeed * Time.deltaTime, Space.World);

        // Rotate the player to face the movement direction
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Infernal auto-attack logic
        if (isInfernal && canShootFireball)
        {
            StartCoroutine(FireballAttack());
        }
    }

    public void Dash()
    {
        if (!canDash) return;
        StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        canDash = false;
        isDashing = true;

        // Push player forward by dashDistance
        Vector3 dashPosition = transform.position + transform.forward * dashDistance;
        float elapsedTime = 0f;
        float dashTime = 0.2f; // Short burst of dash movement

        while (elapsedTime < dashTime)
        {
            transform.position = Vector3.Lerp(transform.position, dashPosition, elapsedTime / dashTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isDashing = false;

        // Apply speed boost for dashDuration
        yield return new WaitForSeconds(dashDuration);

        // Reset cooldown
        yield return new WaitForSeconds(dashCooldown - dashDuration);
        canDash = true;
    }

    private IEnumerator FireballAttack()
    {
        canShootFireball = false;

        // Spawn a fireball in the forward direction
        GameObject fireball = Instantiate(fireballPrefab, transform.position + transform.forward * 1.5f, Quaternion.identity);
        fireball.GetComponent<Rigidbody>().velocity = transform.forward * fireballSpeed;

        yield return new WaitForSeconds(fireballCooldown);

        canShootFireball = true;
    }
}