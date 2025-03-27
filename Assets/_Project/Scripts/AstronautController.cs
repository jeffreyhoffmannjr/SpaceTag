using UnityEngine;
using System.Collections;

public class InfernalPlayerController : MonoBehaviour
{
    public JoystickController joystickController; // Reference to the joystick
    public float speed = 4f; // Slightly slower than Tree player
    public GameObject fireballPrefab; // Fireball prefab
    public Transform firePoint; // Where fireballs spawn from
    public float fireballSpeed = 8f;
    public float fireballCooldown = 1.5f;
    
    private bool canShoot = true;

    private void Update()
    {
        if (joystickController == null) return;

        // Get the joystick input
        Vector2 input = joystickController.GetInputVector();
        Vector3 move = new Vector3(input.x, 0, input.y);

        // Move the Infernal player
        transform.Translate(move * speed * Time.deltaTime, Space.World);

        // Rotate Infernal to face movement direction
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Auto-fire fireballs
        if (canShoot)
        {
            StartCoroutine(FireballAttack());
        }
    }

    IEnumerator FireballAttack()
    {
        canShoot = false;

        // Spawn fireball
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
        fireball.GetComponent<Rigidbody>().velocity = transform.forward * fireballSpeed;

        yield return new WaitForSeconds(fireballCooldown);
        canShoot = true;
    }
}