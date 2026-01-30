using UnityEngine;

public class StrikeAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] pogClips;

    public void PlayStrike()
    {
        if (pogClips.Length == 0) return;

        int index = Random.Range(0, pogClips.Length);
        audioSource.PlayOneShot(pogClips[index]);
    }
}

