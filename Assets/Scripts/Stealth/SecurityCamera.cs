using UnityEngine;

// A camera that sweeps left and right. A single Raycast "beam" shoots forward from it,
// and the player is detected if they're the first thing the beam hits.
public class SecurityCamera : MonoBehaviour
{
    // How far it turns each way from its starting direction, in degrees
    [SerializeField]
    float sweepAngle = 45f;

    // How fast it sweeps
    [SerializeField]
    float sweepSpeed = 1f;

    // How far the beam reaches
    [SerializeField]
    float beamLength = 20f;

    // The visible beam (a thin cube child with no collider). It's stretched each frame
    // so it ends exactly where the raycast hits something
    [SerializeField]
    Transform beam;

    // Thickness of the visible beam
    [SerializeField]
    float beamWidth = 0.03f;

    Vector3 startAngles;

    void Start()
    {
        startAngles = transform.eulerAngles;
    }

    void Update()
    {
        // Mathf.Sin goes -1 -> 1 -> -1, so the camera turns smoothly back and forth
        transform.eulerAngles = startAngles + new Vector3(0, Mathf.Sin(Time.time * sweepSpeed) * sweepAngle, 0);

        // How long the beam should look: full length, unless the ray hits something first
        float visibleLength = beamLength;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, beamLength,
            Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            visibleLength = hit.distance;

            PlayerHealth player = hit.collider.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.Caught("a security camera");
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * beamLength, Color.green);
        }

        UpdateBeam(visibleLength);
    }

    void UpdateBeam(float length)
    {
        // A cube is centred on its position, so put it halfway along the beam
        // and stretch it to the full length: it then starts at the camera and ends at the hit point
        beam.localPosition = new Vector3(0, 0, length / 2);
        beam.localScale = new Vector3(beamWidth, beamWidth, length);
    }
}
