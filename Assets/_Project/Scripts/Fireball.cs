using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage = 10; // Fireball damage
    public float lifetime = 5f; // Fireball disappears after 5 seconds

    private void Start()
    {
        Destroy(gameObject, lifetime); // Auto destroy fireball after some time
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tree"))
        {
            Debug.Log($"🔥 Fireball hit {other.name}!");
            Health treeHealth = other.GetComponent<Health>();

            if (treeHealth != null)
            {
                treeHealth.TakeDamage(damage);
            }

            Destroy(gameObject); // Destroy fireball on impact
        }
    }
}