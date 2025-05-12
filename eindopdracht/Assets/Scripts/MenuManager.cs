using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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

        var interactable = button.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        if (interactable != null)
        {
            interactable.hoverEntered.AddListener((args) => OnButtonHovered(button));
            interactable.selectEntered.AddListener((args) => OnButtonClicked(button));
            Debug.Log($"SetupButton: {button.name} has XR Simple Interactable");
        }
        else
        {
            Debug.LogError($"SetupButton: {button.name} is missing XR Simple Interactable!");
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
        Debug.Log($"Click detected on: {button.name}");
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