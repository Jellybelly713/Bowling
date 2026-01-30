using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleUIManager : MonoBehaviour
{
    public GameObject startPanel;
    public Button startButton;
    public TMP_Text instructionText;

    public Behaviour[] disableUntilStart;

    [TextArea] public string desktopText = "Press E to grab Interact";
    [TextArea] public string vrText = "Trigger to press button\nGrab to grab the ball";


    public bool autoStartInVR = true;
    public float vrAutoStartDelay = 1f;

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

        // Show start screen
        if (startPanel != null) startPanel.SetActive(true);

        // Disable gameplay scripts until start (desktop + vr)
        SetGameplayEnabled(false);

        // Update instruction text for VR or Desktop
        bool isVR = IsVR();
        if (instructionText != null)
            instructionText.text = isVR ? vrText : desktopText;

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

        // locks cursor again for desktop gameplay
        SetCursorForMenu(false);
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
            // locks cursor for desktop only.
            if (!IsVR())
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    bool IsVR()
    {
        // Works for XR Plugin Management / OpenXR etc.
        return UnityEngine.XR.XRSettings.isDeviceActive;
    }
}


