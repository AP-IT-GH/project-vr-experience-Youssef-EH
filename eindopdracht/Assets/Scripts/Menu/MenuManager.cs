using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public AudioClip clickSound;
    public AudioClip hoverSound;
    public SceneFader sceneFader;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnStartButtonClick()
    {
        Debug.Log("Start button clicked");
        PlayClickSound();

        if (sceneFader != null)
            sceneFader.FadeAndLoadScene("MiniGolfScene");
        else
            SceneManager.LoadScene("MiniGolfScene");
    }

    public void OnQuitButtonClick()
    {
        Debug.Log("Quit button clicked");
        PlayClickSound();
        Application.Quit();
    }

    public void OnButtonHover()
    {
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