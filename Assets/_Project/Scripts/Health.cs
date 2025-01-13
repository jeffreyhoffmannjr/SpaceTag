using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health
    private int currentHealth; // Current health

    private void Start()
    {
        currentHealth = maxHealth; // Set initial health
        Debug.Log($"{gameObject.name} initialized with {currentHealth} health.");
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject); // Destroy the game object when health reaches 0
    }
}