using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimpleUIManager : MonoBehaviour
{
    public GameObject startPanel;
    public Button startButton;
    public TMP_Text instructionText;

    public GameObject gameplayInstructionPanel;
    public TMP_Text gameplayInstructionText;

    public Behaviour[] disableUntilStart;

    [TextArea] public string desktopText = "Press 'E' to pick up / drop a bowling ball...";
    [TextArea] public string vrText = "Trigger to press button\nGrab to grab the ball";

    public bool autoStartInVR = true;
    public float vrAutoStartDelay = 1f;

    public bool hideGameplayInstructionAfterDelay = false;
    public float gameplayInstructionDuration = 5f;

    bool started;

    void Awake()
    {
        if (startButton != null)
        {
            startButton.onClick.RemoveListener(StartGame);
            startButton.onClick.AddListener(StartGame);
        }
    }

    void Start()
    {
        started = false;

        bool isVR = IsVR();

        // Show start screen
        if (startPanel != null) startPanel.SetActive(true);

        // Hide gameplay instruction until game begins
        if (gameplayInstructionPanel != null) gameplayInstructionPanel.SetActive(false);

        // Disable gameplay scripts until start
        SetGameplayEnabled(false);

        // Set start screen instruction text
        if (instructionText != null)
            instructionText.text = isVR ? vrText : desktopText;

        // Set gameplay instruction text
        if (gameplayInstructionText != null)
            gameplayInstructionText.text = isVR ? vrText : desktopText;

        // allows desktop clicking
        SetCursorForMenu(true);

        // VR auto start
        if (isVR && autoStartInVR)
            Invoke(nameof(StartGame), vrAutoStartDelay);
    }

    public void StartGame()
    {
        if (started) return;
        started = true;

        if (startPanel != null) startPanel.SetActive(false);

        SetGameplayEnabled(true);

        // Show gameplay instruction UI once the game starts
        if (gameplayInstructionPanel != null)
        {
            gameplayInstructionPanel.SetActive(true);

            if (hideGameplayInstructionAfterDelay)
                StartCoroutine(HideGameplayInstructionAfterDelay());
        }

        // locks cursor again for desktop gameplay
        SetCursorForMenu(false);
    }

    IEnumerator HideGameplayInstructionAfterDelay()
    {
        yield return new WaitForSeconds(gameplayInstructionDuration);

        if (gameplayInstructionPanel != null)
            gameplayInstructionPanel.SetActive(false);
    }

    void SetGameplayEnabled(bool enabled)
    {
        if (disableUntilStart == null) return;

        foreach (var b in disableUntilStart)
        {
            if (b != null) b.enabled = enabled;
        }
    }

    void SetCursorForMenu(bool menuMode)
    {
        if (menuMode)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            // locks cursor for desktop only
            if (!IsVR())
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    bool IsVR()
    {
        // Works for XR Plugin Management / OpenXR
        return UnityEngine.XR.XRSettings.isDeviceActive;
    }
}