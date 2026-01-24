using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 5f;
    public Camera playerCamera;
    public CrosshairUI crosshairScript;
    public Transform holdPoint;

    private BallInteractable heldBall;

    void Update()
    {
        // Safety checks
        if (playerCamera == null || crosshairScript == null)
            return;

        // ✅ PRIORITY: if holding a ball, pressing E should ALWAYS release/throw it
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame && heldBall != null)
        {
            heldBall.Interact(holdPoint, playerCamera.transform); // this triggers DropAndThrow() in your BallInteractable
            heldBall = null;
            crosshairScript.SetInteract(false);
            return;
        }

        // Otherwise, raycast to interact with buttons/balls
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            if (hit.collider.CompareTag("interactable"))
            {
                crosshairScript.SetInteract(true);

                if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                {
                    // 1) Spawn button
                    SpawnButton spawnBtn = hit.collider.GetComponentInParent<SpawnButton>();
                    if (spawnBtn != null)
                    {
                        spawnBtn.Press();
                        return;
                    }

                    // 2) Old toggle button
                    Button oldBtn = hit.collider.GetComponentInParent<Button>();
                    if (oldBtn != null)
                    {
                        oldBtn.Press();
                        return;
                    }

                    // 3) Ball pickup
                    BallInteractable ball = hit.collider.GetComponentInParent<BallInteractable>();
                    if (ball != null)
                    {
                        if (holdPoint == null)
                        {
                            Debug.LogWarning("HoldPoint is not assigned on PlayerInteraction.");
                            return;
                        }

                        // Only pick up if we're not already holding one
                        if (heldBall != null) return;

                        ball.Interact(holdPoint, playerCamera.transform); // this triggers PickUp()
                        heldBall = ball;
                        return;
                    }

                    Debug.Log("Interactable hit, but no SpawnButton/Button/BallInteractable found on it.");
                }

                return;
            }
        }

        crosshairScript.SetInteract(false);
    }
}

