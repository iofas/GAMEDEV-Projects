using UnityEngine;

// Needs a Rigidbody with Is Kinematic ticked.
[RequireComponent(typeof(Rigidbody))]
public class MovingTargetPlatform : MonoBehaviour
{
    // How far it slides each way from its start, e.g. (3, 0, 0) moves 3 left and 3 right
    [SerializeField]
    Vector3 moveOffset = new Vector3(3, 0, 0);

    // How fast it slides
    [SerializeField]
    float moveSpeed = 0.8f;

    Rigidbody rb;
    Vector3 startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        // Mathf.Sin smoothly goes -1 -> 1 -> -1, so the platform speeds up and slows down
        // gently instead of reversing instantly (which could knock the tower over by itself)
        Vector3 target = startPos + moveOffset * Mathf.Sin(Time.time * moveSpeed);

        // MovePosition moves a kinematic Rigidbody the physics way, so friction
        // carries the blocks on top along with it
        rb.MovePosition(target);
    }
}
