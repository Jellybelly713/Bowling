using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallAudio : MonoBehaviour
{
    public AudioClip dropClip;
    public AudioClip rollLoopClip;
    public AudioClip hitPinsClip;

    public float minRollSpeed = 0.6f;
    public float rollMaxVolumeSpeed = 8f;

    private Rigidbody rb;
    private AudioSource rollSource;
    private bool hasDropped = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rollSource = gameObject.AddComponent<AudioSource>();
        rollSource.loop = true;
        rollSource.playOnAwake = false;
        rollSource.clip = rollLoopClip;
        rollSource.spatialBlend = 1f;
    }

    void Update()
    {
        float speed = rb.linearVelocity.magnitude;

        // Rolling loop
        if (rollLoopClip != null && speed > minRollSpeed)
        {
            if (!rollSource.isPlaying) rollSource.Play();

            // volume based on speed
            rollSource.volume = Mathf.Clamp01(speed / rollMaxVolumeSpeed);
            rollSource.pitch = Mathf.Lerp(0.9f, 1.2f, Mathf.Clamp01(speed / rollMaxVolumeSpeed));
        }
        else
        {
            if (rollSource.isPlaying) rollSource.Stop();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Ball drop when first hits the floor
        if (!hasDropped && collision.collider.CompareTag("Floor"))
        {
            hasDropped = true;
            PlayOneShot3D(dropClip, 1f);
        }

        // Hit pins
        if (collision.collider.CompareTag("Pin"))
        {
            PlayOneShot3D(hitPinsClip, 1f);
        }
    }

    private void PlayOneShot3D(AudioClip clip, float volume)
    {
        if (clip == null) return;
        AudioSource.PlayClipAtPoint(clip, transform.position, volume);
    }
}

