using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class MenuManager : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public AudioClip clickSound;
    public AudioClip hoverSound;

    void Start()
    {
        SetupButton(startButton);
        SetupButton(quitButton);
    }

    private void SetupButton(Button button)
    {
        if (button == null)
        {
            Debug.LogError("Button is not assigned in MenuManager!");
            return;
        }

        var interactable = button.GetComponent<XRSimpleInteractable>();
        var pokeAffordance = button.GetComponent<XRPokeFollowAffordance>();
        if (interactable != null && pokeAffordance != null)
        {
            if (pokeAffordance.pokeFollowTransform == null)
            {
                Debug.LogError($"Poke Follow Transform missing on {button.name}! Assign a transform in XR Poke Follow Affordance.");
                pokeAffordance.enabled = false; // Ensure it’s disabled if not fixed
                return;
            }
            interactable.hoverEntered.AddListener((args) => OnButtonHovered(button));
            interactable.selectEntered.AddListener((args) => OnButtonClicked(button));
            Debug.Log($"SetupButton: {button.name} with XR Simple Interactable and XR Poke Follow Affordance, Manager: {interactable.interactionManager}");
        }
        else
        {
            Debug.LogError($"SetupButton: {button.name} is missing XR Simple Interactable or XR Poke Follow Affordance!");
        }
    }

    private void OnButtonHovered(Button button)
    {
        Debug.Log($"Hover detected on: {button.name}");
        AudioSource audioSource = button.GetComponent<AudioSource>();
        if (audioSource != null && hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    private void OnButtonClicked(Button button)
    {
        Debug.Log($"Click detected on: {button.name} via Poke Interaction");
        AudioSource audioSource = button.GetComponent<AudioSource>();
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        if (button == startButton)
        {
            SceneManager.LoadScene("MiniGolfScene");
        }
        else if (button == quitButton)
        {
            Application.Quit();
        }
    }
}