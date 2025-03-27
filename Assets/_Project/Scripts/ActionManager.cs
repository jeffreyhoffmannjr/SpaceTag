using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionManager : MonoBehaviour
{
    public Button buildBaseButton;
    public Button buildLaserButton;
    public Button buildTrapButton;
    public Button dashButton;

    public GameObject basePrefab;
    public GameObject laserPrefab;
    public GameObject trapPrefab;
    public Image dashCooldownOverlay;

    private AlienController playerController;

    private float baseCooldown = 2f;
    private float laserCooldown = 5f;
    private float trapCooldown = 3f;
    private float dashCooldown = 7f;

    public void SetPlayer(GameObject player)
    {
        if (player == null)
        {
            Debug.LogError("❌ SetPlayer: Provided player GameObject is null!");
            return;
        }

        playerController = player.GetComponent<AlienController>();
        if (playerController == null)
        {
            Debug.LogError("❌ SetPlayer: AlienController not found on player!");
            return;
        }

        Debug.Log("✅ SetPlayer: AlienController assigned successfully.");

        buildBaseButton.onClick.AddListener(() => StartCoroutine(SpawnWithCooldown(buildBaseButton, basePrefab, baseCooldown)));
        buildLaserButton.onClick.AddListener(() => StartCoroutine(SpawnWithCooldown(buildLaserButton, laserPrefab, laserCooldown)));
        buildTrapButton.onClick.AddListener(() => StartCoroutine(SpawnWithCooldown(buildTrapButton, trapPrefab, trapCooldown)));
        dashButton.onClick.AddListener(() => StartCoroutine(DashWithCooldown()));
    }

    IEnumerator SpawnWithCooldown(Button button, GameObject prefab, float cooldown)
    {
        if (playerController == null || prefab == null) yield break;

        button.interactable = false;
        Vector3 spawnPosition = playerController.transform.position - playerController.transform.forward * 2f;
        Instantiate(prefab, spawnPosition, Quaternion.identity);
        yield return new WaitForSeconds(cooldown);
        button.interactable = true;
    }

    IEnumerator DashWithCooldown()
    {
        if (playerController == null) yield break;

        dashButton.interactable = false;
        StartCoroutine(ShowCooldownOverlay(dashCooldown));
        playerController.Dash();
        yield return new WaitForSeconds(dashCooldown);
        dashButton.interactable = true;
    }

    IEnumerator ShowCooldownOverlay(float cooldown)
    {
        if (dashCooldownOverlay == null) yield break;

        float elapsed = 0f;
        while (elapsed < cooldown)
        {
            elapsed += Time.deltaTime;
            dashCooldownOverlay.fillAmount = 1 - (elapsed / cooldown);
            yield return null;
        }

        dashCooldownOverlay.fillAmount = 0;
    }
}
