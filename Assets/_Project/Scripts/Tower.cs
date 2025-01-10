using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject projectilePrefab;   // The projectile prefab to shoot
    public Transform firePoint;           // Where the projectile spawns
    public float fireRate = 1f;           // How often the tower fires
    public float range = 10f;             // Range of the tower
    public int damage = 10;               // Damage per projectile

    private float fireCooldown = 0f;

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        // Find the closest Infernal within range
        GameObject target = FindClosestInfernal();
        if (target != null && fireCooldown <= 0f)
        {
            Fire(target);
            fireCooldown = 1f / fireRate;  // Reset cooldown
        }
    }

    GameObject FindClosestInfernal()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Infernal");
        GameObject closest = null;
        float closestDistance = range;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closest = enemy;
                closestDistance = distance;
                Debug.Log($"Tower {gameObject.name} targeting {GetComponent<Collider>().name}");
            }
        }

        return closest;
    }

    void Fire(GameObject target)
    {
        // Spawn projectile and aim it at the target
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile proj = projectile.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.SetTarget(target, damage);
            Debug.Log($"Tower {gameObject.name} firing at {target.name}");
        }
    }
}