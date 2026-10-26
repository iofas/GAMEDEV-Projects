using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;

    [SerializeField]
    float jumpHeight = 1.5f;

    [SerializeField]
    float gravity = -9.81f;

    [SerializeField]
    float gravityScale = 2.04f;

    [SerializeField]
    float mouseSensitivity = 0.1f;

    [SerializeField]
    Transform cameraTransform;

    CharacterController cc;
    Vector2 moveInput;
    Vector2 lookInput;
    float velocity;
    bool jumpPressed;
    float cameraPitch;
    Vector3 startPos;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        startPos = transform.position;

        // Hide the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Look();

        velocity += gravity * gravityScale * Time.deltaTime;

        if (cc.isGrounded && velocity < 0)
        {
            velocity = -2f;
        }

        if (cc.isGrounded && jumpPressed)
        {
            velocity = Mathf.Sqrt(jumpHeight * -2f * (gravity * gravityScale));
        }
        jumpPressed = false;

        MovePlayer();
    }

    void Look()
    {
        // Left/right: turn the whole player
        transform.Rotate(0, lookInput.x * mouseSensitivity, 0);

        // Up/down: tilt only the camera
        cameraPitch -= lookInput.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);
        cameraTransform.localEulerAngles = new Vector3(cameraPitch, 0, 0);
    }

    void MovePlayer()
    {
        Vector3 movement = (transform.right * moveInput.x + transform.forward * moveInput.y) * moveSpeed;
        movement.y = velocity;

        cc.Move(movement * Time.deltaTime);
    }

    // Send the player back to where they started (same idea as PlatformerController)
    public void Respawn()
    {
        cc.enabled = false;
        transform.position = startPos;
        cc.enabled = true;
        velocity = 0;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
            jumpPressed = true;
    }
}
