using UnityEngine;

// A guard that walks back and forth and looks for the player with a cone of vision.
// Detection uses a Raycast so the guard can't see through walls.
public class PatrolGuard : MonoBehaviour
{
    [Header("Patrol")]
    // How far the guard walks from its start before turning back, e.g. (0, 0, 10)
    [SerializeField]
    Vector3 patrolOffset = new Vector3(0, 0, 10);

    // Back-and-forth trips per second (lower = slower)
    [SerializeField]
    float patrolSpeed = 0.15f;

    [Header("Vision")]
    // Where the guard looks from (an empty child at head height, facing forward)
    [SerializeField]
    Transform eyes;

    // How far the guard can see
    [SerializeField]
    float viewDistance = 10f;

    // Full width of the vision cone in degrees (match the Spot Angle of the guard's light)
    [SerializeField]
    float viewAngle = 60f;

    [Header("Awareness")]
    // If the player gets this close, the guard notices them in any direction (even from behind)
    [SerializeField]
    float awarenessRadius = 2.5f;

    // Only colliders on these layers are checked (set it to the "Player" layer)
    [SerializeField]
    LayerMask playerLayer;

    Vector3 startPos;
    PlayerHealth player;

    void Start()
    {
        startPos = transform.position;
        player = FindFirstObjectByType<PlayerHealth>();
    }

    void Update()
    {
        Patrol();

        // CheckSphere: is any part of the player inside the sphere around the guard?
        if (Physics.CheckSphere(transform.position, awarenessRadius, playerLayer))
        {
            player.Caught("a guard (got too close)");
        }
        else if (CanSeePlayer())
        {
            player.Caught("a guard");
        }
    }

    void Patrol()
    {
        Vector3 oldPos = transform.position;
        transform.position = startPos + patrolOffset * Mathf.PingPong(Time.time * patrolSpeed, 1);

        // Face the way we're walking
        Vector3 moved = transform.position - oldPos;
        if (moved != Vector3.zero)
        {
            transform.forward = moved.normalized;
        }
    }

    bool CanSeePlayer()
    {
        Vector3 toPlayer = player.transform.position - eyes.position;

        // 1. Too far away?
        if (toPlayer.magnitude > viewDistance)
            return false;

        // 2. Outside the vision cone? (angle between where we're looking and where the player is)
        if (Vector3.Angle(eyes.forward, toPlayer) > viewAngle / 2)
            return false;

        // 3. Is anything in the way? Fire a ray at the player and see what it hits first.
        // QueryTriggerInteraction.Ignore makes the ray pass through trigger colliders (bonus panels, goal)
        if (Physics.Raycast(eyes.position, toPlayer.normalized, out RaycastHit hit, viewDistance,
            Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            // Draws the ray in the Scene view (and the Game view with Gizmos on) for debugging
            Debug.DrawLine(eyes.position, hit.point, Color.red);

            return hit.collider.GetComponent<PlayerHealth>() != null;
        }

        return false;
    }
}
