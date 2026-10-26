using UnityEngine;

// A moving sensor area. Uses Physics.CheckSphere / CheckBox / CheckCapsule to ask
// "is any part of the player inside this shape?" every frame.
// The size comes from the object's Scale, so the visible mesh matches the detection area.
public class DetectionZone : MonoBehaviour
{
    enum Shape { Sphere, Box, Capsule }

    [SerializeField]
    Shape shape = Shape.Sphere;

    // Only colliders on these layers are checked (set it to the "Player" layer)
    [SerializeField]
    LayerMask playerLayer;

    // How far it moves from its start before coming back, e.g. (14, 0, 0) to cross the corridor
    [SerializeField]
    Vector3 moveOffset = new Vector3(14, 0, 0);

    // Back-and-forth trips per second
    [SerializeField]
    float moveSpeed = 0.2f;

    Vector3 startPos;
    PlayerHealth player;

    void Start()
    {
        startPos = transform.position;
        player = FindFirstObjectByType<PlayerHealth>();
    }

    void Update()
    {
        transform.position = startPos + moveOffset * Mathf.PingPong(Time.time * moveSpeed, 1);

        if (PlayerInside())
        {
            player.Caught("a sensor " + shape.ToString().ToLower());
        }
    }

    bool PlayerInside()
    {
        Vector3 size = transform.localScale;

        switch (shape)
        {
            case Shape.Sphere:
                // Unity's sphere is 1 unit wide, so the radius is half the scale
                return Physics.CheckSphere(transform.position, size.x / 2, playerLayer);

            case Shape.Box:
                // CheckBox wants half the size in each direction
                return Physics.CheckBox(transform.position, size / 2, transform.rotation, playerLayer);

            case Shape.Capsule:
                // Unity's capsule is 1 wide and 2 tall, so: radius = half the X scale,
                // and the two end points are (height / 2 - radius) above and below the centre
                float radius = size.x / 2;
                float halfHeight = size.y;
                Vector3 top = transform.position + transform.up * (halfHeight - radius);
                Vector3 bottom = transform.position - transform.up * (halfHeight - radius);
                return Physics.CheckCapsule(top, bottom, radius, playerLayer);
        }

        return false;
    }
}
