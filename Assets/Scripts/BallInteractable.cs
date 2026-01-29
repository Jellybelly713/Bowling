using UnityEngine;

public class BallInteractable : MonoBehaviour
{
    public float throwForce = 12f;

    public float spreadAngle = 0.5f; // left/right randomness in degrees
    public float verticalSpread = 1.0f; // up/down randomness in degrees
    public float spinAmount = 1.5f; // random spin torque

    public Vector3 holdLocalOffset = Vector3.zero; 
    public Vector3 holdLocalEulerOffset = Vector3.zero;

    public float despawnDelay = 10f;

    private Rigidbody rb;
    private Collider col;

    private bool isHeld = false;
    private Transform holdPoint;
    private Transform holder;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    void LateUpdate()
    {
        // Hard lock to hold point after player rotation updates
        if (!isHeld || holdPoint == null) return;

        transform.localPosition = holdLocalOffset;
        transform.localRotation = Quaternion.Euler(holdLocalEulerOffset);
    }

    public void Interact(Transform playerHoldPoint, Transform playerTransform)
    {
        if (!isHeld) PickUp(playerHoldPoint, playerTransform);
        else DropAndThrow();
    }

    private void PickUp(Transform playerHoldPoint, Transform playerTransform)
    {
        CancelInvoke(nameof(Despawn));

        isHeld = true;
        holdPoint = playerHoldPoint;
        holder = playerTransform;

        // stops physics from fighting the player motion
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = true;

        // Disable collider while held so it never gets pushed by nearby geometry/player
        if (col != null) col.enabled = false;

        // Parent to hold point and snap
        transform.SetParent(holdPoint, worldPositionStays: false);
        transform.localPosition = holdLocalOffset;
        transform.localRotation = Quaternion.Euler(holdLocalEulerOffset);
    }

    private void DropAndThrow()
    {
        isHeld = false;

        // Unparent first
        transform.SetParent(null, worldPositionStays: true);

        // Re-enable physics + collider
        if (col != null) col.enabled = true;

        rb.isKinematic = false;
        rb.useGravity = true;

        // Base forward direction
        Vector3 forward = (holdPoint != null) ? holdPoint.forward :
                          (holder != null) ? holder.forward :
                          transform.forward;

        // Randomness to ball throw
        float yaw = Random.Range(-spreadAngle, spreadAngle); // left/right
        float pitch = Random.Range(-verticalSpread, verticalSpread); // up/down

        Quaternion randomRot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 throwDir = randomRot * forward;

        // Apply force
        rb.AddForce(throwDir * throwForce, ForceMode.Impulse);

        // random spin torque to feel more like bowling
        if (spinAmount > 0f)
        {
            rb.AddTorque(Random.insideUnitSphere * spinAmount, ForceMode.Impulse);
        }

        // Despawn after x seconds
        Invoke(nameof(Despawn), despawnDelay);

        holdPoint = null;
        holder = null;
    }

    private void Despawn()
    {
        Destroy(gameObject);
    }
}







