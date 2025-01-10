using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;  // Maximum health of the object
    private int currentHealth;  // Current health of the object

    private void Start()
    {
        currentHealth = maxHealth; // Initialize health
        Debug.Log($"{gameObject.name} initialized with {currentHealth} health.");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce health by damage amount
        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");

        // Check if health has reached zero
        if (currentHealth <= 0)
        {
            Die(); // Call the Die method
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has been destroyed.");
        Destroy(gameObject); // Destroy the game object
    }

    public int GetCurrentHealth()
    {
        return currentHealth; // Optional: Getter for current health
    }

    public void Heal(int amount)
    {
        currentHealth += amount; // Heal the object
        currentHealth = Mathf.Min(currentHealth, maxHealth); // Clamp health to maxHealth
        Debug.Log($"{gameObject.name} healed by {amount}. Current health: {currentHealth}");
    }
}