using System.Collections.Generic;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    public List<GameObject> balls;
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
        if(levelNr == 6) levelNr = 1; 
        else  levelNr++;

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
}
