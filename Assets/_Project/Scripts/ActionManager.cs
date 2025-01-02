using UnityEngine;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour
{
    public Button buildTreeButton;       // Button for building trees
    public Button buildTowerButton;      // Button for building towers
    public GameObject treePrefab;        // Prefab for the tree
    public GameObject towerPrefab;       // Prefab for the tower
    public GameObject player;            // Reference to the player object

    void Start()
    {
        // Set up button click listeners
        buildTreeButton.onClick.AddListener(() => SpawnObject(treePrefab));
        buildTowerButton.onClick.AddListener(() => SpawnObject(towerPrefab));
    }

    void SpawnObject(GameObject prefab)
    {
        if (player == null || prefab == null) return;

        // Spawn the object directly behind the player
        Vector3 spawnPosition = player.transform.position - player.transform.forward * 2f; // Adjust the distance as needed
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}