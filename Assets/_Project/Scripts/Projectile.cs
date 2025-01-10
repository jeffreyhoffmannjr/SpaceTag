using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject target;
    private int damage;
    public float speed = 10f;

    public void SetTarget(GameObject newTarget, int newDamage)
    {
        target = newTarget;
        damage = newDamage;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // Destroy if target is gone
            return;
        }

        // Move towards the target
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Destroy projectile on collision with the target
        if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
        {
            // Apply damage or handle target logic here
            Debug.Log($"Hit {target.name} for {damage} damage!");
            Destroy(gameObject);
        }
    }
}