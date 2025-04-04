using UnityEngine;

public class PerkManager : MonoBehaviour
{
    private AlienController controller;
    private XPTracker xpTracker;

    private int lastLevel = 0;

    void Start()
    {
        controller = GetComponent<AlienController>();
        xpTracker = GetComponent<XPTracker>();

        if (controller == null || xpTracker == null)
        {
            Debug.LogWarning("PerkManager missing required components.");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        int currentLevel = xpTracker.GetCurrentLevel();
        if (currentLevel > lastLevel)
        {
            for (int i = lastLevel + 1; i <= currentLevel; i++)
            {
                ApplyPerk(i);
            }

            lastLevel = currentLevel;
        }
    }

    void ApplyPerk(int level)
    {
        Debug.Log($"🎁 Applying perk for Level {level}");

        switch (level)
        {
            case 5:
                controller.speed += 1f;
                break;
            case 10:
                controller.dashCooldown -= 0.5f;
                break;
            case 15:
                controller.speed += 1f;
                break;
            case 20:
                controller.dashDistance += 1f;
                break;
            case 25:
                controller.speed += 1f;
                break;
            case 30:
                controller.dashSpeedMultiplier += 0.5f;
                break;
            default:
                break;
        }
    }
}
