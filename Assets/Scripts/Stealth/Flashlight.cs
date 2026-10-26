using UnityEngine;
using UnityEngine.InputSystem;

// Put on the Player (next to PlayerInput). F toggles the flashlight.
public class Flashlight : MonoBehaviour
{
    // A Spot Light that is a child of the camera
    [SerializeField]
    Light flashlight;

    void Start()
    {
        flashlight.enabled = false;
    }

    // Called by PlayerInput when the "Flashlight" action is pressed
    public void OnFlashlight(InputValue value)
    {
        if (value.isPressed)
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }

    // Is this object lit up by the flashlight right now?
    public bool CanSee(Transform target)
    {
        if (!flashlight.enabled)
            return false;

        Vector3 toTarget = target.position - flashlight.transform.position;

        // 1. Further than the light reaches?
        if (toTarget.magnitude > flashlight.range)
            return false;

        // 2. Outside the light cone? (the Spot Angle is the full width of the cone)
        if (Vector3.Angle(flashlight.transform.forward, toTarget) > flashlight.spotAngle / 2)
            return false;

        // 3. Is something (like a wall) blocking the light? The ray must hit the object itself.
        if (Physics.Raycast(flashlight.transform.position, toTarget.normalized, out RaycastHit hit, flashlight.range,
            Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            return hit.transform == target;
        }

        return false;
    }
}
