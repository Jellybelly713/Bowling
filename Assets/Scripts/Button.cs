using UnityEngine;

public class Button : MonoBehaviour
{
    Vector3 startPos;
    bool isPressed = false;
    bool isAnimating = false;
    public float pressSpeed = 0f;
    public float pressDistance = 0.5f;
    public GameObject targetObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = isPressed ? startPos - Vector3.up * pressDistance : startPos;

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, pressSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.localPosition, target) < 0.001 )
        {
            if (isPressed)
            {
                isPressed = false;
            }
            else
            {
                isAnimating = false;
            }
        }
    }

    public void Press()
    {
        if (targetObject.activeSelf)
        {
            targetObject.SetActive(false);
        }
        else
        {
            targetObject.SetActive(true);
        }

            isAnimating = true;
        isPressed = true;
    }
}
