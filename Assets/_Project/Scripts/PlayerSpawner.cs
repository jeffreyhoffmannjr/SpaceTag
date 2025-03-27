using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject alienPrefab;
    public Transform spawnPoint;
    public JoystickController joystick;
    public CameraController cameraController;
    public ActionManager actionManager;

    private GameObject playerInstance;

    void Start()
    {
        StartCoroutine(SpawnPlayerOnceGameManagerIsReady());
    }

    IEnumerator SpawnPlayerOnceGameManagerIsReady()
    {
        while (GameManager.Instance == null)
            yield return null;

        yield return null;

        if (playerInstance == null)
        {
            playerInstance = Instantiate(alienPrefab, spawnPoint.position, Quaternion.identity);

            AlienController controller = playerInstance.GetComponent<AlienController>();
            if (controller != null)
                controller.joystickController = joystick;

            if (cameraController != null)
                cameraController.target = playerInstance.transform;

            GameManager.Instance.allPlayers.Add(playerInstance);
            Debug.Log("✅ Player registered to GameManager.");

            if (actionManager != null)
            {
                actionManager.SetPlayer(playerInstance);
            }
            else
            {
                Debug.LogError("❌ PlayerSpawner: ActionManager reference not assigned!");
            }
        }
    }
}
