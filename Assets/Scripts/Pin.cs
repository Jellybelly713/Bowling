using System.Collections;
using UnityEngine;

public class Pin : MonoBehaviour
{
    [Header("Knockdown Detection")]
    public float fallenAngle = 45f;
    public float fallenHoldTime = 0.35f; 

    [Header("Despawn")]
    public float despawnDelay = 1.25f;

    private Rigidbody rb;
    private PinManager manager;

    private Vector3 startPos;
    private Quaternion startRot;

    private float fallenTimer = 0f;
    private bool isFallen = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        startPos = transform.position;
        startRot = transform.rotation;

        manager = FindFirstObjectByType<PinManager>();
        if (manager != null)
            manager.RegisterPin(this);
    }

    void Update()
    {
        if (isFallen) return;

        float angle = Vector3.Angle(transform.up, Vector3.up);

        // If tipped over far enough, start counting time
        if (angle >= fallenAngle)
        {
            fallenTimer += Time.deltaTime;

            if (fallenTimer >= fallenHoldTime)
            {
                isFallen = true;
                if (manager != null) manager.OnPinFallen(this);

                StartCoroutine(DespawnAfterDelay());
            }
        }
        else
        {
            // stood back up
            fallenTimer = 0f;
        }
    }

    private IEnumerator DespawnAfterDelay()
    {
        yield return new WaitForSeconds(despawnDelay);
        gameObject.SetActive(false);
    }

    public void ResetPin()
    {
        StopAllCoroutines();
        gameObject.SetActive(true);

        isFallen = false;
        fallenTimer = 0f;

        // reset physics
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            transform.position = startPos;
            transform.rotation = startRot;

            rb.isKinematic = false;
            rb.WakeUp();
        }
        else
        {
            transform.position = startPos;
            transform.rotation = startRot;
        }
    }
}

