using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class XP_HUD_Controller : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI levelText;
    public Image xpBarFill;
    public CanvasGroup canvasGroup;

    [Header("Player")]
    public AlienController player;

    [Header("Fade Settings")]
    public float fadeSpeed = 3f;
    [Range(0f, 1f)] public float fadedAlpha = 0.5f;

    private float targetAlpha = 1f;

    void Start()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

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

        int level = tracker.GetCurrentLevel();
        float progress = tracker.GetProgressToNextLevel();

        if (levelText != null)
            levelText.text = $"Level {level}";

        if (xpBarFill != null)
            xpBarFill.fillAmount = Mathf.Lerp(xpBarFill.fillAmount, progress, Time.deltaTime * 6f);
    }

    void UpdateFade()
    {
        Vector2 input = player.joystickController != null
            ? player.joystickController.GetInputVector()
            : Vector2.zero;

        bool isMoving = input.magnitude > 0.1f;
        targetAlpha = isMoving ? fadedAlpha : 1f;

        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
    }
}
