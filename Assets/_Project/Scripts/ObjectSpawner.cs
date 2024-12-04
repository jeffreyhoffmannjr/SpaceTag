using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject treePrefab; // We'll use cubes for now
    public int objectCount = 20;
    public float spawnRange = 20f;

    void Start()
    {
        for (int i = 0; i < objectCount; i++)
        {
            float x = Random.Range(-spawnRange, spawnRange);
            float z = Random.Range(-spawnRange, spawnRange);
            Vector3 position = new Vector3(x, 0.5f, z);
            
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.position = position;
            obj.transform.localScale = new Vector3(1, 2, 1);
        }
    }
}