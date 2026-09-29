using UnityEngine;

public class PlatformSwitch : MonoBehaviour
{
    enum SwitchAction { Rotate, MoveVertical, MoveHorizontal, ResetAll }

    [SerializeField]
    SwitchAction action = SwitchAction.Rotate;

    // Not needed for ResetAll
    [SerializeField]
    SwitchPlatform target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlatformerController>() == null)
            return;

        if (action == SwitchAction.ResetAll)
        {
            foreach (SwitchPlatform platform in FindObjectsByType<SwitchPlatform>(FindObjectsSortMode.None))
                platform.ResetPlatform();
            return;
        }

        SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlatformerController>() == null)
            return;

        SetActive(false);
    }

    void SetActive(bool on)
    {
        if (target == null)
            return;

        switch (action)
        {
            case SwitchAction.Rotate:
                target.SetRotating(on);
                break;
            case SwitchAction.MoveVertical:
                target.SetMovingVertical(on);
                break;
            case SwitchAction.MoveHorizontal:
                target.SetMovingHorizontal(on);
                break;
        }
    }
}
