using UnityEngine;

// Put on a floor panel with a trigger collider
public class BonusPanel : MonoBehaviour
{
    enum BonusType { Heal, SpeedBoost }

    [SerializeField]
    BonusType bonus = BonusType.Heal;

    [SerializeField]
    int healAmount = 1;

    [SerializeField]
    float speedMultiplier = 1.5f;

    [SerializeField]
    float duration = 5f;

    [SerializeField]
    float cooldown = 10f;

    float nextUseTime;

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth == null)
            return;

        if (Time.time < nextUseTime)
        {
            Debug.Log("This panel is recharging");
            return;
        }
        nextUseTime = Time.time + cooldown;

        switch (bonus)
        {
            case BonusType.Heal:
                playerHealth.Heal(healAmount);
                break;
            case BonusType.SpeedBoost:
                other.GetComponent<PlatformerController>().SpeedBoost(speedMultiplier, duration);
                break;
        }
    }
}
