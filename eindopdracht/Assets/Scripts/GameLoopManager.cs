using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    public List<GameObject> balls;
    public List<GameObject> enemyBalls;
    public LeaderboardUI leaderboardUI;

    private int levelNr = 1;

    int[] you = { 0, 0, 0, 0, 0, 0 };
    int[] opponent = { 0, 0, 0, 0, 0, 0 };

    void Start()
    {
        SetBallsState(1);
    }

    void Update()
    {

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
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        GameObject agentBall = enemyBalls[levelNr - 1];
        agentBall.SetActive(true);

        BallAgent agent = agentBall.GetComponent<BallAgent>();

        while (agentBall.activeInHierarchy && agent.StepCount != 0)
        {
            yield return null; 
        }

        agentBall.SetActive(true); // Dit op false = agent wel werken

        if (levelNr == 6) leaderboardUI.ShowEndResult(WinOrLose());
        else levelNr++;

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
