using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{

    public float interactRange = 5f;
    public Camera playerCamera;
    public CrosshairUI crosshairScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
{
    Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit, interactRange))
    {
        // DEBUG: what are we looking at?
        Debug.Log("Hit: " + hit.collider.name + " Tag: " + hit.collider.tag);

        if (hit.collider.CompareTag("interactable"))
        {
            crosshairScript.SetInteract(true);

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                Debug.Log("Pressed E on: " + hit.collider.name);

                Button button = hit.collider.GetComponentInParent<Button>();
                if (button != null)
                {
                    Debug.Log("Found Button component, pressing.");
                    button.Press();
                }
                else
                {
                    Debug.Log("NO Button component found on hit object or parents.");
                }
            }
            return;
        }
    }

    crosshairScript.SetInteract(false);
}
}