using UnityEngine;

public class PersistentMusic : MonoBehaviour
{
    private static PersistentMusic instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this GameObject when changing scenes
        }
        else
        {
            Destroy(gameObject); // Destroy any extra copies
        }
    }
}