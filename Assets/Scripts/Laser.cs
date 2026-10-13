using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    float speed = 5f;

    [SerializeField]
    int damage = 1;

    // The laser is removed once it moves behind this Z position (past the start of the corridor)
    [SerializeField]
    float destroyZ = -5f;

    bool hasHit;

    // Update is called once per frame
    void Update()
    {
        // Move from the end of the corridor towards the start
        transform.position += Vector3.back * speed * Time.deltaTime;

        if (transform.position.z < destroyZ)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only hurt the player once per laser
        if (hasHit)
            return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            hasHit = true;
            playerHealth.TakeDamage(damage);
        }
    }
}
