using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProgress
{
    public int accountLevel = 1;
    public float totalXP = 0f;

    public List<string> unlockedAbilities = new List<string> { "Dash", "Base", "Laser", "Trap" };
    public string selectedAbility1 = "Laser";
    public string selectedAbility2 = "Trap";

    public void AddXP(float amount)
    {
        totalXP += amount;

        // Level-up system can be custom tuned later
        while (totalXP >= GetXPNeededForLevel(accountLevel))
        {
            totalXP -= GetXPNeededForLevel(accountLevel);
            accountLevel++;
            UnlockAbility(accountLevel);
        }
    }

    float GetXPNeededForLevel(int level)
    {
        return 100 + level * 25;
    }

    void UnlockAbility(int level)
    {
        switch (level)
        {
            case 5: unlockedAbilities.Add("Reflect"); break;
            case 10: unlockedAbilities.Add("Cloak"); break;
            case 15: unlockedAbilities.Add("EMP"); break;
            case 20: unlockedAbilities.Add("Shield"); break;
                // add more ability unlocks here
        }
    }
}
