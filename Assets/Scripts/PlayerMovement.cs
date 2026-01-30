using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float mouseSens = 5.0f;

    public float gravity = -20f;
    public float groundStickForce = -2f;

    public CharacterController characterController;
    public Transform cameraTransform;

    float xRotation = 0f;
    float yVelocity = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Move Input
        Vector2 moveInput = Keyboard.current != null
            ? new Vector2(
                (Keyboard.current.aKey.isPressed ? -1 : 0) + (Keyboard.current.dKey.isPressed ? 1 : 0),
                (Keyboard.current.sKey.isPressed ? -1 : 0) + (Keyboard.current.wKey.isPressed ? 1 : 0)
              )
            : Vector2.zero;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move *= speed;

        // Gravity
        if (characterController.isGrounded)
        {
            // keeps you “stuck” to ground so player doesn't float after walking onto edges
            if (yVelocity < 0f) yVelocity = groundStickForce;
        }
        else
        {
            yVelocity += gravity * Time.deltaTime;
        }

        Vector3 velocity = move + Vector3.up * yVelocity;
        characterController.Move(velocity * Time.deltaTime);

        // --- LOOK ---
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            float mouseX = mouseDelta.x * mouseSens * Time.deltaTime;
            float mouseY = mouseDelta.y * mouseSens * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);

            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            transform.Rotate(Vector3.up * mouseX);
        }
    }
}

