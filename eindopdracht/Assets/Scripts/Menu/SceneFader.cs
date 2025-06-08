using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.0f;
    public AudioClip fadeSFX; // Assign sound effect
    private AudioSource audioSource;

    void Awake()
    {
        // AudioSource on this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void FadeAndLoadScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    IEnumerator FadeOutAndLoad(string sceneName)
    {
        // Play SFX if assigned
        if (fadeSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(fadeSFX);
        }

        // Fade to black
        float t = 0;
        Color c = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
        c.a = 1;
        fadeImage.color = c;

        // Now load the scene
        SceneManager.LoadScene(sceneName);
    }
}