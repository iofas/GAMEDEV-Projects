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
        if (other.GetComponent<PlatformerController>() != null || other.GetComponent<FirstPersonController>() != null)
        {
            Debug.Log("Level Clear!");

            if (levelClearScreen != null)
            {
                levelClearScreen.SetActive(true);

                // Freeze the game
                Time.timeScale = 0f;

                // Show the mouse again so the Retry button can be clicked
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
