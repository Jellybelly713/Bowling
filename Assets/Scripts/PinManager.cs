using System.Collections.Generic;
using UnityEngine;

public class PinManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioClip strikeClip;

    private readonly List<Pin> pins = new List<Pin>();
    private readonly HashSet<Pin> fallenPins = new HashSet<Pin>();

    private bool strikePlayed = false;

    public void RegisterPin(Pin pin)
    {
        if (pin == null) return;

        if (!pins.Contains(pin))
            pins.Add(pin);
    }

    public void OnPinFallen(Pin pin)
    {
        if (pin == null) return;

        if (!fallenPins.Add(pin))
            return;

        // Strike = all pins have fallen
        if (!strikePlayed && pins.Count > 0 && fallenPins.Count >= pins.Count)
        {
            strikePlayed = true;

            if (sfxSource != null && strikeClip != null)
                sfxSource.PlayOneShot(strikeClip);
            else
                Debug.LogWarning("PinManager: Missing sfxSource or strikeClip for strike sound.");
        }
    }

    public void ResetAllPins()
    {
        // Reset pins
        for (int i = 0; i < pins.Count; i++)
        {
            if (pins[i] != null)
                pins[i].ResetPin();
        }

        // Reset strike tracking
        fallenPins.Clear();
        strikePlayed = false;
    }
    public void StartNewThrow()
    {
        fallenPins.Clear();
        strikePlayed = false;
    }
}


