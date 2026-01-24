using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    Vector3 startPos;
    bool isPressed = false;

    public float pressSpeed = 4f;
    public float pressDistance = 0.2f;

    public BallSpawner spawner;

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
        if (spawner != null)
            spawner.SpawnBall();

        isPressed = true;
    }
}

