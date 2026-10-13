using UnityEngine;
using TMPro;

// Put on the Player, next to PlatformerController
public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    int maxHealth = 3;

    [SerializeField]
    TMP_Text healthText;

    [SerializeField]
    GameObject gameOverScreen;

    int health;

    void Start()
    {
        gameOverScreen.SetActive(false);
        health = maxHealth;
        UpdateHealthText();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        UpdateHealthText();

        if (health <= 0)
        {
            GameOver();
        }
    }

    public void Heal(int amount)
    {
        health += amount;
        UpdateHealthText();
    }

    void UpdateHealthText()
    {
        healthText.text = "Health: " + health;
    }

    void GameOver()
    {
        Debug.Log("Game Over!");

        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}
