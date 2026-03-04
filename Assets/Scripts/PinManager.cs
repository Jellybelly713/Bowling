using System.Collections.Generic;
using UnityEngine;

public class PinManager : MonoBehaviour
{
    public StrikeAudio strikeAudio;

    public ParticleSystem strikeConfetti; //

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

        // Strike = all pins fallen
        if (!strikePlayed && pins.Count > 0 && fallenPins.Count >= pins.Count)
        {
            strikePlayed = true;

            // Play strike sound
            if (strikeAudio != null)
                strikeAudio.PlayStrike();
            else
                Debug.LogWarning("StrikeAudio not assigned.");

            // Play confetti effect
            if (strikeConfetti != null)
                strikeConfetti.Play();
            else
                Debug.LogWarning("strikeConfetti not assigned.");
        }
    }

    public void ResetAllPins()
    {
        for (int i = 0; i < pins.Count; i++)
        {
            if (pins[i] != null)
                pins[i].ResetPin();
        }

        fallenPins.Clear();
        strikePlayed = false;

        // Stop confetti if still playing
        if (strikeConfetti != null && strikeConfetti.isPlaying)
            strikeConfetti.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void StartNewThrow()
    {
        fallenPins.Clear();
        strikePlayed = false;
    }
}



