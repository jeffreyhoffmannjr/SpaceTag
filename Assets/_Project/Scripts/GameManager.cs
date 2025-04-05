using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public float matchDuration = 300f;
    private float timer;

    [Header("Players")]
    public List<GameObject> allPlayers = new List<GameObject>();
    public List<GameObject> runners = new List<GameObject>();
    public List<GameObject> hunters = new List<GameObject>();

    [Header("Jail Settings")]
    public Transform jailLocation;

    [Header("UI")]
    public TextMeshProUGUI timerText;
    public GameObject winPanel;
    public TextMeshProUGUI winText;

    [Header("XP Settings")]
    public float xpPerSecond = 1f;
    private Dictionary<GameObject, float> playerXP = new Dictionary<GameObject, float>();

    private bool gameEnded = false;
    private bool gameStarted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("✅ GameManager instance set in Awake.");
        }
        else
        {
            Debug.LogWarning("⚠️ Duplicate GameManager destroyed.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("🟡 GameManager Start() running.");

        if (winPanel != null)
        {
            winPanel.SetActive(false);
            Debug.Log("✅ Win panel disabled.");
        }
        else
        {
            Debug.LogWarning("⚠️ Win panel reference is missing!");
        }

        Debug.Log("⏳ Waiting 0.5s before starting game...");
        Invoke(nameof(BeginGame), 0.5f);
    }

    private void BeginGame()
    {
        Debug.Log("🔵 BeginGame() called.");

        if (allPlayers.Count == 0)
        {
            Debug.LogWarning("🚨 BeginGame aborted. allPlayers is empty!");
            return;
        }

        AssignRoles();

        if (runners.Count == 0)
        {
            Debug.LogWarning("⚠️ No runners in game. Skipping game start.");
            return;
        }

        InitializeXP();

        timer = matchDuration;
        gameStarted = true;

        Debug.Log($"✅ Game started. Timer set to {matchDuration} seconds.");
    }

    private void Update()
    {
        if (!gameStarted) return;
        if (gameEnded) return;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = 0f;
            UpdateTimerUI();
            EndGame(true);
            return;
        }

        UpdateTimerUI();
        GrantXPOverTime();
        CheckAllRunnersCaptured();
    }

    public bool IsGameActive()
    {
        return gameStarted && !gameEnded;
    }

    void AssignRoles()
    {
        runners.Clear();
        hunters.Clear();

        List<GameObject> unassigned = new List<GameObject>(allPlayers);

        // 👇 Ensure at least one runner remains
        int maxHunters = Mathf.Max(0, allPlayers.Count - 1);
        int hunterCount = Mathf.Min(maxHunters, allPlayers.Count / 4);

        Debug.Log($"🧠 Assigning {hunterCount} astronaut(s).");

        for (int i = 0; i < hunterCount && unassigned.Count > 0; i++)
        {
            int index = Random.Range(0, unassigned.Count);
            GameObject hunter = unassigned[index];
            if (hunter != null)
            {
                hunters.Add(hunter);
                AlienController controller = hunter.GetComponent<AlienController>();
                if (controller != null)
                {
                    controller.isAstronaut = true;
                    Debug.Log($"🧑‍🚀 Assigned astronaut: {hunter.name}");
                }
            }
            unassigned.RemoveAt(index);
        }

        foreach (GameObject runner in unassigned)
        {
            if (runner != null)
            {
                runners.Add(runner);
                AlienController controller = runner.GetComponent<AlienController>();
                if (controller != null)
                {
                    controller.isAstronaut = false;
                    Debug.Log($"👽 Assigned alien: {runner.name}");
                }
            }
        }

        Debug.Log($"✅ AssignRoles complete. Runners: {runners.Count}, Hunters: {hunters.Count}");
    }


    void InitializeXP()
    {
        foreach (var player in allPlayers)
        {
            if (player != null && !playerXP.ContainsKey(player))
            {
                playerXP[player] = 0f;
            }
        }
    }

    void GrantXPOverTime()
    {
        foreach (var player in allPlayers)
        {
            if (player != null)
            {
                if (!playerXP.ContainsKey(player))
                    playerXP[player] = 0f;

                playerXP[player] += xpPerSecond * Time.deltaTime;
            }
        }
    }

    public void AddBonusXP(GameObject player, float amount)
    {
        if (player != null)
        {
            if (!playerXP.ContainsKey(player))
                playerXP[player] = 0f;

            playerXP[player] += amount;
        }
    }

    public float GetXP(GameObject player)
    {
        if (player != null && playerXP.ContainsKey(player))
            return playerXP[player];

        return 0f;
    }

    public void TagRunner(GameObject runner)
    {
        if (!runners.Contains(runner)) return;

        runner.transform.position = jailLocation.position;

        var controller = runner.GetComponent<AlienController>();
        if (controller != null)
        {
            controller.enabled = false;
        }
    }

    void CheckAllRunnersCaptured()
    {
        foreach (var runner in runners)
        {
            if (runner != null && runner.GetComponent<AlienController>().enabled)
            {
                return;
            }
        }

        EndGame(false);
    }

    void EndGame(bool runnersWin)
    {
        if (gameEnded) return;

        gameEnded = true;

        foreach (var player in allPlayers)
        {
            if (player == null) continue;

            // Disable movement
            var controller = player.GetComponent<AlienController>();
            if (controller != null)
                controller.enabled = false;

            // Disable joystick input
            var joystick = player.GetComponentInChildren<JoystickController>();
            if (joystick != null)
                joystick.enabled = false;

            // Disable all active ability scripts
            MonoBehaviour[] abilities = player.GetComponents<MonoBehaviour>();
            foreach (var ability in abilities)
            {
                if (ability != null && ability.enabled && ability.GetType() != typeof(AlienController))
                {
                    ability.enabled = false;
                    Debug.Log($"🧯 Disabled {ability.GetType().Name} on {player.name}");
                }
            }
        }

        if (winPanel != null)
            winPanel.SetActive(true);

        if (winText != null)
            winText.text = runnersWin ? "Aliens Win!" : "Astronauts Win!";

        Debug.Log($"🏁 Game ended. {(runnersWin ? "Aliens Win!" : "Astronauts Win!")}");
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
        timerText.ForceMeshUpdate();
    }
}
