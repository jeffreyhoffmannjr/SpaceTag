using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XP_HUD_Controller : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public Image xpBarFill;
    public CanvasGroup canvasGroup;
    public AlienController player;

    private float fadeSpeed = 3f;
    private float targetAlpha = 1f;

    void Start()
    {
        canvasGroup.alpha = 1f;
    }

    void Update()
    {
        if (player == null) return;

        UpdateLevelAndXP();
        UpdateFade();
    }

    void UpdateLevelAndXP()
    {
        XPTracker tracker = player.GetComponent<XPTracker>();
        if (tracker == null) return;

        levelText.text = $"Level {tracker.GetCurrentLevel()}";
        float progress = tracker.GetProgressToNextLevel();
        xpBarFill.fillAmount = Mathf.Lerp(xpBarFill.fillAmount, progress, Time.deltaTime * 8f);
    }

    void UpdateFade()
    {
        Vector2 input = player.joystickController != null ? player.joystickController.GetInputVector() : Vector2.zero;
        targetAlpha = input.magnitude > 0.1f ? 0.5f : 1f;
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
    }
}
