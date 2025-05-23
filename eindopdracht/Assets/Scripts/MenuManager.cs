using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public AudioClip clickSound;
    public AudioClip hoverSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnButtonClick(Button button)
    {
        Debug.Log("Button clicked: " + button.name);
        PlayClickSound();

        if (button == startButton)
        {
            SceneManager.LoadScene("MiniGolfScene");
        }
        else if (button == quitButton)
        {
            Application.Quit();
        }
    }

    public void OnButtonHover(Button button)
    {
        Debug.Log("Button hovered: " + button.name);
        PlayHoverSound();
    }

    private void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
        else
        {
            Debug.LogWarning("AudioSource or ClickSound not assigned");
        }
    }

    private void PlayHoverSound()
    {
        if (audioSource != null && hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
        else
        {
            Debug.LogWarning("AudioSource or HoverSound not assigned");
        }
    }
}