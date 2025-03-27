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

        Debug.Log($"📦 Total players in allPlayers: {allPlayers.Count}");

        AssignRoles();
        InitializeXP();

        timer = matchDuration;
        gameStarted = true;

        Debug.Log($"✅ Game started. Timer set to {matchDuration} seconds.");
    }

    private void Update()
    {
        Debug.Log("🌀 GameManager.Update() loop running...");

        if (!gameStarted)
        {
            Debug.Log("🟠 Update skipped. Game has not started yet.");
            return;
        }

        if (gameEnded)
        {
            Debug.Log("🛑 Update skipped. Game already ended.");
            return;
        }

        timer -= Time.deltaTime;
        Debug.Log($"⏱️ Timer ticking... Time left: {timer}");

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

    void AssignRoles()
    {
        Debug.Log("🔁 AssignRoles() started.");
        runners.Clear();
        hunters.Clear();

        int hunterCount = Mathf.Max(1, allPlayers.Count / 4);
        Debug.Log($"🧠 Assigning {hunterCount} astronaut(s).");

        List<GameObject> unassigned = new List<GameObject>(allPlayers);

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
        Debug.Log("🔧 Initializing XP for all players...");
        foreach (var player in allPlayers)
        {
            if (player != null && !playerXP.ContainsKey(player))
            {
                playerXP[player] = 0f;
                Debug.Log($"📈 XP initialized for {player.name}");
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
                {
                    Debug.Log($"❗ Player {player.name} had no XP key, creating one.");
                    playerXP[player] = 0f;
                }

                playerXP[player] += xpPerSecond * Time.deltaTime;
                Debug.Log($"💸 Gave {xpPerSecond * Time.deltaTime:F2} XP to {player.name}");
            }
        }
    }

    public void AddBonusXP(GameObject player, float amount)
    {
        if (player != null)
        {
            if (!playerXP.ContainsKey(player))
            {
                Debug.Log($"❗ Player {player.name} had no XP key for bonus, creating one.");
                playerXP[player] = 0f;
            }

            playerXP[player] += amount;
            Debug.Log($"🎁 Bonus XP: Gave {amount} XP to {player.name}");
        }
    }

    public float GetXP(GameObject player)
    {
        if (player != null && playerXP.ContainsKey(player))
        {
            return playerXP[player];
        }

        return 0f;
    }

    public void TagRunner(GameObject runner)
    {
        if (!runners.Contains(runner))
        {
            Debug.LogWarning("❌ Attempted to tag non-runner.");
            return;
        }

        runner.transform.position = jailLocation.position;
        AlienController controller = runner.GetComponent<AlienController>();
        if (controller != null)
        {
            controller.enabled = false;
            Debug.Log($"🚓 Runner tagged and jailed: {runner.name}");
        }
    }

    void CheckAllRunnersCaptured()
    {
        if (runners.Count == 0)
        {
            Debug.LogWarning("⚠️ No runners to check.");
            return;
        }

        foreach (var runner in runners)
        {
            if (runner != null && runner.GetComponent<AlienController>().enabled)
            {
                Debug.Log("🟢 Runner still active.");
                return;
            }
        }

        Debug.Log("🔴 All runners captured. Ending game.");
        EndGame(false);
    }

    void EndGame(bool runnersWin)
    {
        if (gameEnded) return;

        gameEnded = true;

        if (winPanel != null)
            winPanel.SetActive(true);

        if (winText != null)
            winText.text = runnersWin ? "Aliens Win!" : "Astronauts Win!";

        Debug.Log($"🏁 Game ended. {(runnersWin ? "Aliens Win!" : "Astronauts Win!")}");
        //Invoke(nameof(RestartGame), 5f);
    }

    void RestartGame()
    {
        Debug.Log("🔄 Restarting game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateTimerUI()
    {
        if (timerText == null)
        {
            Debug.LogWarning("⚠️ Timer text is null!");
            return;
        }

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        string formatted = $"{minutes:00}:{seconds:00}";
        timerText.text = formatted;
        timerText.ForceMeshUpdate();

        Debug.Log($"📟 Timer updated: {formatted}");
    }
}
