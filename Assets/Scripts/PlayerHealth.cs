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

    // Optional UI text (e.g. "SPOTTED!") shown briefly when a hazard detects the player
    [SerializeField]
    GameObject spottedText;

    int health;
    float spottedTimer;

    void Start()
    {
        gameOverScreen.SetActive(false);
        if (spottedText != null)
        {
            spottedText.SetActive(false);
        }

        health = maxHealth;
        UpdateHealthText();
    }

    void Update()
    {
        // Hide the "SPOTTED!" text after a moment
        if (spottedTimer > 0)
        {
            spottedTimer -= Time.deltaTime;
            if (spottedTimer <= 0 && spottedText != null)
            {
                spottedText.SetActive(false);
            }
        }
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

    // Called by stealth hazards when they detect the player:
    // lose 1 health and get sent back to the start
    public void Caught(string byWhat)
    {
        // Already game over, ignore
        if (health <= 0)
            return;

        Debug.Log("Spotted by " + byWhat + "!");

        if (spottedText != null)
        {
            spottedText.SetActive(true);
            spottedTimer = 1.5f;
        }

        TakeDamage(1);

        if (health > 0)
        {
            GetComponent<FirstPersonController>().Respawn();
        }
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

        // Show the mouse again so the Retry button can be clicked
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
