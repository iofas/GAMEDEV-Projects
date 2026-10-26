using UnityEngine;

// An object that is invisible unless the player's flashlight is on and pointed at it.
// Needs a Mesh Renderer and a (non-trigger) collider so the flashlight's raycast can hit it.
public class HiddenObject : MonoBehaviour
{
    Renderer rend;
    Flashlight flashlight;
    bool found;

    void Start()
    {
        rend = GetComponent<Renderer>();
        flashlight = FindFirstObjectByType<Flashlight>();
        rend.enabled = false;
    }

    void Update()
    {
        bool lit = flashlight.CanSee(transform);
        rend.enabled = lit;

        if (lit && !found)
        {
            found = true;
            Debug.Log("Found a hidden object: " + name);
        }
    }
}
