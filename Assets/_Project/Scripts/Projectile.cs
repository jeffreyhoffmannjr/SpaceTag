using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject target;     // Target object to hit
    private int damage;            // Damage to apply
    public float speed = 10f;      // Speed of the projectile

    // Sets the target and damage for the projectile
    public void SetTarget(GameObject newTarget, int newDamage)
    {
        target = newTarget;
        damage = newDamage;
    }

    private void Update()
    {
        if (target == null)
        {
            Debug.Log("Target is null. Destroying projectile.");
            Destroy(gameObject); // Destroy the projectile if target is gone
            return;
        }

        // Move towards the target
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        Debug.Log($"Projectile moving towards {target.name}");

        // Check for collision with the target
        if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
        {
            Debug.Log($"Hit {target.name} for {damage} damage!");

            // Apply damage to the target if it has a Health component
            Health health = target.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            Destroy(gameObject); // Destroy the projectile after hitting the target
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Optional: Handle OnTriggerEnter logic if necessary
        if (other.gameObject == target)
        {
            Debug.Log($"Projectile collided with {target.name}.");
            Health health = target.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}