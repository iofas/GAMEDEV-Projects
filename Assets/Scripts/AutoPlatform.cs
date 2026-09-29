using UnityEngine;

// Runs before the player
[DefaultExecutionOrder(-100)]
public class AutoPlatform : MonoBehaviour
{
    [SerializeField]
    Vector3 rotationSpeed = Vector3.zero;

    [SerializeField]
    Vector3 moveOffset = Vector3.zero;

    [SerializeField]
    float moveSpeed = 0.5f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * moveSpeed, 1);
        transform.position = startPos + moveOffset * t;
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.World);
    }
}
