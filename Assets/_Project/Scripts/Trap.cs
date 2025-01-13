using UnityEngine;

public class Trap : MonoBehaviour
{
    public int damage = 20;           // Damage dealt to Infernals
    public float damageInterval = 0.5f; // Time between damage ticks
    public float range = 3f;          // Range within which the trap can detect Infernals

    private float damageCooldown = 0f;

    void Update()
    {
        damageCooldown -= Time.deltaTime;

        // Check for the closest Infernal
        GameObject nearestInfernal = FindClosestInfernal();

        if (nearestInfernal != null && damageCooldown <= 0f)
        {
            ApplyDamage(nearestInfernal);
            damageCooldown = damageInterval; // Reset cooldown
        }
    }

    GameObject FindClosestInfernal()
    {
        GameObject[] infernals = GameObject.FindGameObjectsWithTag("Infernal");
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject infernal in infernals)
        {
            float distance = Vector3.Distance(transform.position, infernal.transform.position);
            if (distance < closestDistance && distance <= range)
            {
                closest = infernal;
                closestDistance = distance;
            }
        }

        return closest;
    }

    void ApplyDamage(GameObject infernal)
    {
        Health health = infernal.GetComponent<Health>();
        if (health != null)
        {
            Debug.Log($"Trap at {transform.position} damages {infernal.name} for {damage}.");
            health.TakeDamage(damage);
        }
        else
        {
            Debug.LogWarning($"{infernal.name} has no Health component!");
        }
    }
}