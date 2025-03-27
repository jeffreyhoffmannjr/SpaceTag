using UnityEngine;
using System.Collections;

public class AlienController : MonoBehaviour
{
    public JoystickController joystickController; // Reference to the joystick
    public float speed = 5f;
    public float dashDistance = 5f;
    public float dashSpeedMultiplier = 2f;
    public float dashDuration = 2f;
    public float dashCooldown = 7f;

    private bool canDash = true;
    private bool isDashing = false;

    public bool isAstronaut = false; // Astronaut = Hunter. Alien = Runner.
    public GameObject fireballPrefab;
    public float fireballCooldown = 1.5f;
    public float fireballSpeed = 8f;
    private bool canShootFireball = true;

    private void Update()
    {
        if (joystickController == null) return;

        Vector2 input = joystickController.GetInputVector();
        Vector3 move = new Vector3(input.x, 0, input.y);

        float currentSpeed = isDashing ? speed * dashSpeedMultiplier : speed;
        transform.Translate(move * currentSpeed * Time.deltaTime, Space.World);

        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Only astronauts shoot
        if (isAstronaut && canShootFireball && fireballPrefab != null)
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

        Vector3 dashPosition = transform.position + transform.forward * dashDistance;
        float elapsedTime = 0f;
        float dashTime = 0.2f;

        while (elapsedTime < dashTime)
        {
            transform.position = Vector3.Lerp(transform.position, dashPosition, elapsedTime / dashTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isDashing = false;

        yield return new WaitForSeconds(dashDuration);
        yield return new WaitForSeconds(dashCooldown - dashDuration);
        canDash = true;
    }

    private IEnumerator FireballAttack()
    {
        canShootFireball = false;

        GameObject fireball = Instantiate(fireballPrefab, transform.position + transform.forward * 1.5f, Quaternion.identity);
        fireball.GetComponent<Rigidbody>().velocity = transform.forward * fireballSpeed;

        yield return new WaitForSeconds(fireballCooldown);
        canShootFireball = true;
    }
}
