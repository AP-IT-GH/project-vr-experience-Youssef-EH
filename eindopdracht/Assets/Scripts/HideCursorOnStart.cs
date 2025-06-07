using UnityEngine;

public class HideCursorOnStart : MonoBehaviour
{
    void Start()
    {
        HideAndLockCursor();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            HideAndLockCursor();
        }
    }

    // also for pause/unpause (like on mobile)
    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            HideAndLockCursor();
        }
    }

    void HideAndLockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}