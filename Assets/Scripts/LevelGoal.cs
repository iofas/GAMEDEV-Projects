using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlatformerController>() != null)
        {
            Debug.Log("Level Clear!");
        }
    }
}
