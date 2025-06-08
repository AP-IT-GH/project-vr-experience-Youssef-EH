using UnityEngine;

public class HoleCollision : MonoBehaviour
{
    public GameLoopManager gameLoopManager;

    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GolfBall"))
        {
            if (gameLoopManager != null)
            {
                gameLoopManager.BallEnteredHole();
            }
        }
    }
}
