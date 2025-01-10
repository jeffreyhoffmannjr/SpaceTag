using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;  // Maximum health
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;  // Initialize current health
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Handle death (e.g., destroy the game object, respawn, etc.)
        Debug.Log($"{gameObject.name} has died!");
        Destroy(gameObject);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}