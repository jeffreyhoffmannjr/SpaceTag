using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject projectilePrefab; // Assign the projectile prefab
    public Transform firePoint; // Assign the FirePoint in Unity
    public float fireRate = 1f; // How often the tower shoots
    public float range = 10f; // Tower range
    public int damage = 10; // Damage per projectile

    private float fireTimer = 0f; // Timer to control fire rate

    private void Update()
    {
        fireTimer += Time.deltaTime;

        // Find the closest enemy in range
        GameObject target = FindClosestEnemy();
        if (target != null && fireTimer >= fireRate)
        {
            Shoot(target);
            fireTimer = 0f; // Reset timer
        }
    }

    private GameObject FindClosestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range); // Detect objects in range
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy")) // Check if the object is tagged "Enemy"
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hit.gameObject;
                }
            }
        }
        return closestEnemy;
    }

    private void Shoot(GameObject target)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (target.transform.position - firePoint.position).normalized;
            rb.velocity = direction * 10f; // Adjust speed as needed
        }
    }
}