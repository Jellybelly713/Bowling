using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 5.0f;
    public float mouseSens = 5.0f;

    public CharacterController characterController;
    public Transform cameraTransform;

    float xRotation = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Debug.Log("Scene has started");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor. visible = false;



    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Scene has updated");

        Vector2 moveInput = Keyboard.current != null
            ? new Vector2 (
                (Keyboard.current.aKey.isPressed ? -1 : 0) + (Keyboard.current.dKey.isPressed ? 1 : 0),
                (Keyboard.current.sKey.isPressed ? -1 : 0) + (Keyboard.current.wKey.isPressed ? 1 : 0)
                ) : Vector2.zero;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        characterController.Move ( move * speed * Time.deltaTime);

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * mouseSens * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSens * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }
}
