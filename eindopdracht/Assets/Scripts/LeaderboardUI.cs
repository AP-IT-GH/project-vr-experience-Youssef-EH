using UnityEngine;
using TMPro;
using System.Collections;

public class LeaderboardUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject leaderboardPanel;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI[] youScores;      // Size: 6
    public TextMeshProUGUI[] opponentScores; // Size: 6

    [Header("Settings")]
    public float showTime = 5f; // seconds

    private float timer;

    void Start()
    {
        // For debug: show leaderboard with dummy data
        int[] you = {3, 4, 2, 5, 10, 7};
        int[] opponent = {4, 5, 3, 10, 8, 6};
        ShowLeaderboard(you, opponent);
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
        // Teleport to next course logic goes here (to be handled by another classmate)
    }
}