using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour
{
    [SerializeField]
    GameObject levelClearScreen;

    void Start()
    {
        Time.timeScale = 1f;

        if (levelClearScreen != null)
        {
            levelClearScreen.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlatformerController>() != null)
        {
            Debug.Log("Level Clear!");

            if (levelClearScreen != null)
            {
                levelClearScreen.SetActive(true);

                // Freeze the game
                Time.timeScale = 0f;
            }
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
