using UnityEngine;

public class XPTracker : MonoBehaviour
{
    public int currentLevel = 0;
    public float currentXPInMatch = 0f;

    public float baseXPToLevel = 100f;
    public float xpGrowthRate = 20f;

    public delegate void LevelUpDelegate(int newLevel);
    public event LevelUpDelegate OnLevelUp;

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsGameActive()) return;

        // Gain XP over time
        currentXPInMatch += GameManager.Instance.xpPerSecond * Time.deltaTime;

        // Check for level up
        float nextLevelXP = baseXPToLevel + xpGrowthRate * currentLevel;
        while (currentXPInMatch >= nextLevelXP)
        {
            currentXPInMatch -= nextLevelXP;
            currentLevel++;
            Debug.Log($"🔼 Leveled up to {currentLevel}");
            OnLevelUp?.Invoke(currentLevel);

            nextLevelXP = baseXPToLevel + xpGrowthRate * currentLevel;
        }
    }

    public void AddXP(float amount)
    {
        currentXPInMatch += amount;
    }

    public float GetProgressToNextLevel()
    {
        float nextLevelXP = baseXPToLevel + xpGrowthRate * currentLevel;
        return Mathf.Clamp01(currentXPInMatch / nextLevelXP);
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
}
