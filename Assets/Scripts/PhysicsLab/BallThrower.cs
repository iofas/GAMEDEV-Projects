using UnityEngine;
using UnityEngine.InputSystem;

public class BallThrower : MonoBehaviour
{
    [SerializeField]
    GameObject ballPrefab;

    [SerializeField]
    Transform handBall;

    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    float throwForce = 100f;

    [SerializeField]
    float reloadTime = 0.5f;

    // prevent balls from piling up and destroying my computer
    [SerializeField]
    float ballLifetime = 10f;

    float reloadTimer;

    void Update()
    {
        // Count down, then show the next ball in the hand
        if (reloadTimer > 0)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0)
            {
                handBall.gameObject.SetActive(true);
            }
        }
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed && reloadTimer <= 0)
        {
            Throw();
        }
    }

    void Throw()
    {
        // Create a real ball where the hand ball is
        GameObject ball = Instantiate(ballPrefab, handBall.position, handBall.rotation);

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.AddForce(cameraTransform.forward * throwForce, ForceMode.Impulse);

        Destroy(ball, ballLifetime);

        // Hide the hand ball until the next one is ready
        handBall.gameObject.SetActive(false);
        reloadTimer = reloadTime;
    }
}
