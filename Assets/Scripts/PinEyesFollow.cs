using UnityEngine;

public class PinEyesFollow : MonoBehaviour
{
    [Header("Assign the two eye transforms")]
    public Transform eyeL;
    public Transform eyeR;

    [Header("Target (leave empty = auto Camera.main)")]
    public Transform target;

    [Header("Tuning")]
    public float followSpeed = 10f;
    public float maxYaw = 45f;      // left/right limit
    public float maxPitch = 25f;    // up/down limit

    private Quaternion eyeLStartLocalRot;
    private Quaternion eyeRStartLocalRot;

    void Start()
    {
        // Auto find eyes if not assigned
        if (eyeL == null)
        {
            Transform t = transform.Find("Eye_L");
            if (t != null) eyeL = t;
        }

        if (eyeR == null)
        {
            Transform t = transform.Find("Eye_R");
            if (t != null) eyeR = t;
        }

        // Auto find camera
        if (target == null && Camera.main != null)
            target = Camera.main.transform;

        if (eyeL != null) eyeLStartLocalRot = eyeL.localRotation;
        if (eyeR != null) eyeRStartLocalRot = eyeR.localRotation;
    }

    void LateUpdate()
    {
        if (target == null) return;

        LookWithEye(eyeL, eyeLStartLocalRot);
        LookWithEye(eyeR, eyeRStartLocalRot);
    }

    void LookWithEye(Transform eye, Quaternion startLocalRot)
    {
        if (eye == null) return;

        // World direction from eye to target
        Vector3 dir = target.position - eye.position;
        if (dir.sqrMagnitude < 0.0001f) return;

        // Desired world rotation
        Quaternion lookWorld = Quaternion.LookRotation(dir);

        // Convert to LOCAL rotation relative to the pin
        Quaternion lookLocal = Quaternion.Inverse(transform.rotation) * lookWorld;

        // Clamp yaw/pitch in local space
        Vector3 e = lookLocal.eulerAngles;
        e.x = NormalizeAngle(e.x);
        e.y = NormalizeAngle(e.y);

        float clampedPitch = Mathf.Clamp(e.x, -maxPitch, maxPitch);
        float clampedYaw = Mathf.Clamp(e.y, -maxYaw, maxYaw);

        Quaternion clampedLocal = Quaternion.Euler(clampedPitch, clampedYaw, 0f);

        // Apply smoothly, relative to original eye rotation
        Quaternion targetLocalRot = startLocalRot * clampedLocal;
        eye.localRotation = Quaternion.Slerp(eye.localRotation, targetLocalRot, followSpeed * Time.deltaTime);
    }

    float NormalizeAngle(float a)
    {
        if (a > 180f) a -= 360f;
        return a;
    }
}

