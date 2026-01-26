using UnityEngine;

public class ResetPinsButton : MonoBehaviour
{
    [Header("Press Animation")]
    public float pressSpeed = 4f;
    public float pressDistance = 0.2f;

    [Header("References")]
    public PinManager pinManager;

    private Vector3 startPos;
    private bool isPressed = false;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        Vector3 target = isPressed ? startPos - Vector3.up * pressDistance : startPos;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, pressSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.localPosition, target) < 0.001f && isPressed)
            isPressed = false;
    }

    public void Press()
    {
        isPressed = true;

        if (pinManager != null)
            pinManager.ResetAllPins();
        else
            Debug.LogWarning("ResetPinsButton: PinManager is not assigned.");
    }
}

