using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionManager : MonoBehaviour
{
    public Button buildTreeButton;
    public Button buildTowerButton;
    public Button buildTrapButton;
    public Button dashButton;

    public GameObject treePrefab;
    public GameObject towerPrefab;
    public GameObject trapPrefab;
    public GameObject player;

    public Image dashCooldownOverlay;  // UI overlay for cooldown feedback

    private TreeController playerController;

    // Cooldowns
    private float treeCooldown = 2f;
    private float towerCooldown = 5f;
    private float trapCooldown = 3f;
    private float dashCooldown = 7f;

    void Start()
    {
        // Ensure we get the correct player reference
        GameObject treePlayer = GameObject.FindWithTag("Tree");
        if (treePlayer != null)
        {
            player = treePlayer;
            playerController = player.GetComponent<TreeController>();
        }
        else
        {
            Debug.LogError("Tree player not found! Abilities will not work.");
            return; // Stop execution if the player is not found
        }

        // Set up button click listeners with cooldown management
        buildTreeButton.onClick.AddListener(() => StartCoroutine(SpawnWithCooldown(buildTreeButton, treePrefab, treeCooldown)));
        buildTowerButton.onClick.AddListener(() => StartCoroutine(SpawnWithCooldown(buildTowerButton, towerPrefab, towerCooldown)));
        buildTrapButton.onClick.AddListener(() => StartCoroutine(SpawnWithCooldown(buildTrapButton, trapPrefab, trapCooldown)));
        dashButton.onClick.AddListener(() => StartCoroutine(DashWithCooldown()));
    }

    IEnumerator SpawnWithCooldown(Button button, GameObject prefab, float cooldown)
    {
        if (player == null || prefab == null) yield break;

        // Disable the button
        button.interactable = false;

        // Spawn the object behind the player
        Vector3 spawnPosition = player.transform.position - player.transform.forward * 2f;
        Instantiate(prefab, spawnPosition, Quaternion.identity);

        // Wait for cooldown
        yield return new WaitForSeconds(cooldown);

        // Enable the button again
        button.interactable = true;
    }

    IEnumerator DashWithCooldown()
    {
        if (playerController == null) yield break;

        // Disable Dash Button & Start Cooldown UI
        dashButton.interactable = false;
        StartCoroutine(ShowCooldownOverlay(dashCooldown));

        // Trigger Dash Ability
        playerController.Dash();

        // Wait for cooldown
        yield return new WaitForSeconds(dashCooldown);

        // Enable the button
        dashButton.interactable = true;
    }

    IEnumerator ShowCooldownOverlay(float cooldown)
    {
        if (dashCooldownOverlay == null) yield break;

        float elapsed = 0f;
        while (elapsed < cooldown)
        {
            elapsed += Time.deltaTime;
            dashCooldownOverlay.fillAmount = 1 - (elapsed / cooldown); // Fill decreases over time
            yield return null;
        }

        dashCooldownOverlay.fillAmount = 0; // Reset overlay when cooldown ends
    }
}