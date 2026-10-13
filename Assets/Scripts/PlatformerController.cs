using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlatformerController : MonoBehaviour
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
    float fallY = -10f;

    [HideInInspector]
    public bool canMove = true;
    CharacterController cc;
    Vector2 moveInput;
    float velocity;
    bool jumpPressed;
    Vector3 startPos;

    Transform platform;
    Vector3 platformLastPos;

    // Speed boost bonus
    float speedMultiplier = 1f;
    float speedBoostTimer;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        startPos = transform.position;
    }

    void Update()
    {
        if (transform.position.y < fallY)
        {
            Respawn();
            return;
        }

        // Count down the speed boost, then go back to normal speed
        if (speedBoostTimer > 0)
        {
            speedBoostTimer -= Time.deltaTime;
            if (speedBoostTimer <= 0)
            {
                speedMultiplier = 1f;
                Debug.Log("Speed boost ended");
            }
        }

        velocity += gravity * gravityScale * Time.deltaTime;

        if (cc.isGrounded && velocity < 0)
        {
            velocity = -2f;
        }

        if (cc.isGrounded && jumpPressed && canMove)
        {
            velocity = Mathf.Sqrt(jumpHeight * -2f * (gravity * gravityScale));
        }
        jumpPressed = false;

        MovePlayer();
    }

    void MovePlayer()
    {
        // Horizontal movement along the world X/Z axes
        Vector3 movement = Vector3.zero;
        if (canMove)
            movement = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * speedMultiplier;

        movement.y = velocity;

        // for riding moving platforms
        Vector3 platformMovement = Vector3.zero;
        if (platform != null)
        {
            platformMovement = platform.position - platformLastPos;
        }

        platform = null;

        cc.Move(platformMovement);
        cc.Move(movement * Time.deltaTime);

        if (platform != null)
        {
            platformLastPos = platform.position;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal.y > 0.5f)
        {
            platform = hit.transform;
        }
    }

    public void Respawn()
    {
        cc.enabled = false;
        transform.position = startPos;
        cc.enabled = true;
        velocity = 0;
        platform = null;
        speedMultiplier = 1f;
        speedBoostTimer = 0;
    }

    public void SpeedBoost(float multiplier, float duration)
    {
        speedMultiplier = multiplier;
        speedBoostTimer = duration;
        Debug.Log("Speed boost for " + duration + " seconds!");
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
            jumpPressed = true;
    }
}
