using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LeaderboardUI : MonoBehaviour
{
    [Header("Leaderboard UI")]
    public GameObject leaderboardPanel;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI[] youScores;      // Assign YouScore1..YouScore6
    public TextMeshProUGUI[] opponentScores; // Assign OpponentScore1..OpponentScore6

    [Header("End Result UI")]
    public TextMeshProUGUI endResultText; // Assign EndResultText
    public GameObject buttonRow;           // Assign ButtonRow (parent of both buttons)

    [Header("Debug")]
    public bool debugShowEndScreen = false;
    public bool debugWin = true; // Toggle to show You Win! or You Lose!

    [Header("Settings")]
    public float showTime = 5f; // seconds for leaderboard

    private float timer;

    void Start()
    {
    }

    public void ShowLeaderboard(int[] you, int[] opponent)
    {
        leaderboardPanel.SetActive(true);
        timer = showTime;
        UpdateScores(you, opponent);
        StopAllCoroutines();
        StartCoroutine(ShowTimer());
    }

    private void UpdateScores(int[] you, int[] opponent)
    {
        for (int i = 0; i < 6; i++)
        {
            youScores[i].text = you[i].ToString();
            opponentScores[i].text = opponent[i].ToString();
        }
    }

    private IEnumerator ShowTimer()
    {
        while (timer > 0)
        {
            timerText.text = $"<color=yellow>(Next course in: {timer:F1} seconds)</color>";
            yield return null;
            timer -= Time.deltaTime;
        }
        leaderboardPanel.SetActive(false);
        // (Teleportion logic goes here, handled elsewhere)
    }

    public void ShowEndResult(bool win)
    {
        StopAllCoroutines(); // Stop timer coroutine
        leaderboardPanel.SetActive(true); // If you want to keep showing the scores/table
        timerText.gameObject.SetActive(false); // Hide timer text

        endResultText.gameObject.SetActive(true);
        buttonRow.SetActive(true);

        endResultText.text = win ? "<color=green>You Win!</color>" : "<color=red>You Lose!</color>";
    }

    public void HideEndResult()
    {
        timerText.gameObject.SetActive(true); // Show timer text again
        endResultText.gameObject.SetActive(false);
        buttonRow.SetActive(false);
    }

    // Button events
    public void OnExitClicked()
    {
        // Loads StartMenuScene (make sure scene is in build settings)
        SceneManager.LoadScene("StartMenuScene");
    }

    public void OnPlayAgainClicked()
    {
        // Reloads current scene (MiniGolfScene)
        SceneManager.LoadScene("MiniGolfScene");
    }
}