using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents.Policies;
using Unity.VisualScripting;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    public List<GameObject> balls;
    public List<GameObject> enemyBalls;
    public LeaderboardUI leaderboardUI;

    private int levelNr = 1;

    int[] you = { 0, 0, 0, 0, 0, 0 };
    int[] opponent = { 0, 0, 0, 0, 0, 0 };
    
    private bool enemyTurnEnded = false;

    void Start()
    {
        SetBallsState(1);
        GameObject agentBall = enemyBalls[levelNr - 1];
        StartCoroutine(PrewarmAgent());
    }
    //dit is van chatgpt, da laat de agent starten en deftig resetten. als je die direct aan en uit zet zit die vast in die episode ofzo
    private IEnumerator PrewarmAgent()
    {
        yield return null;
        GameObject agentBall = enemyBalls[levelNr - 1];
        var behaviorParams = agentBall.GetComponent<BehaviorParameters>();
        if (behaviorParams.Model != null)
        {
            behaviorParams.BehaviorType = BehaviorType.InferenceOnly;
        }
        agentBall.SetActive(true);
        yield return null;
        agentBall.SetActive(false);
    }

    void SetBallsState(int level)
    {
        for(int i = 0; i < balls.Count; i++)
        {
            balls[i].SetActive(false);
        }
        balls[level - 1].SetActive(true);
    }

    public void BallEnteredHole()
    {
        // Haal de ball van de course
        balls[levelNr - 1].SetActive(false);
    
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        enemyTurnEnded = false;
        GameObject agentBall = enemyBalls[levelNr - 1];
        agentBall.SetActive(true);

        BallAgent agent = agentBall.GetComponent<BallAgent>();
        agent.HasScored = false;
        agent.EpisodeRunning = true;
        agent.shotsTaken = 0;
        agent.lastEpisodeScored = false; // <-- Reset before turn
        agent.lastEpisodeShots = 0;

        Rigidbody agentRb = agentBall.GetComponent<Rigidbody>();

        yield return null;

        agent.RequestDecision();
        Debug.Log("Agent started playing");

        // Wait for the agent to finish
        while (agent.EpisodeRunning)
        {
            yield return null;
        }

        // Use lastEpisodeScored and lastEpisodeShots for result
        if (agent.lastEpisodeScored)
        {
            Debug.Log("Agent scored in " + agent.lastEpisodeShots + " shots");
            AddEnemyScore(Mathf.Max(1, agent.lastEpisodeShots));
        }
        else
        {
            Debug.Log("Agent failed to score, adding penalty");
            AddEnemyScore(12);
        }

        yield return new WaitForSeconds(1f);
        agentBall.SetActive(false);

        if (levelNr == 6)
            leaderboardUI.ShowEndResult(WinOrLose());
        else
            levelNr++;

        leaderboardUI.ShowLeaderboard(you, opponent);
        SetBallsState(levelNr);
    }

    public void hitBall()
    {
        you[levelNr - 1] = you[levelNr - 1] + 1;
    
        if (you[levelNr - 1] == 10)
        {
            you[levelNr - 1] = 12;
            BallEnteredHole();
        }
    }

    private bool WinOrLose()
    {
        int scoreY = 0;
        int scoreE = 0;

        for (int i = 0; i < you.Length; i++)
        {
            scoreY += you[i];
            scoreE += opponent[i];
        }

        return scoreY < scoreE ? true : false;
    }

    public void AddEnemyScore(int strokesEnemy)
    {
        opponent[levelNr - 1] = strokesEnemy;
    }
}
