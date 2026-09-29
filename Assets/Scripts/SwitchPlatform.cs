using UnityEngine;

// Runs before the player
[DefaultExecutionOrder(-100)]
public class SwitchPlatform : MonoBehaviour
{
    [SerializeField]
    float rotateSpeed = 45f;

    [SerializeField]
    float moveSpeed = 1.5f;

    [SerializeField]
    float maxHeight = 3f;

    [SerializeField]
    float maxDistance = 4f;

    Vector3 startPos;
    Vector3 startAngles;

    bool rotating;
    bool movingVertical;
    bool movingHorizontal;
    bool resetting;

    float angle;
    float height;
    float distance;
    int verticalDir = 1;
    int horizontalDir = 1;

    void Start()
    {
        startPos = transform.position;
        startAngles = transform.eulerAngles;
    }

    void Update()
    {
        if (resetting)
        {
            angle = Mathf.MoveTowardsAngle(angle, 0, rotateSpeed * 2f * Time.deltaTime);
            height = Mathf.MoveTowards(height, 0, moveSpeed * 2f * Time.deltaTime);
            distance = Mathf.MoveTowards(distance, 0, moveSpeed * 2f * Time.deltaTime);
            if (Mathf.Approximately(Mathf.DeltaAngle(angle, 0), 0) && height == 0 && distance == 0)
                resetting = false;
        }

        if (rotating)
            angle = Mathf.Repeat(angle + rotateSpeed * Time.deltaTime, 360f);

        if (movingVertical)
            height = MoveBackAndForth(height, maxHeight, ref verticalDir);

        if (movingHorizontal)
            distance = MoveBackAndForth(distance, maxDistance, ref horizontalDir);

        transform.position = startPos + Vector3.up * height + Vector3.right * distance;
        transform.eulerAngles = startAngles + new Vector3(0, angle, 0);
    }

    // Goes towards max, then back towards -max, repeat
    float MoveBackAndForth(float value, float max, ref int dir)
    {
        value += dir * moveSpeed * Time.deltaTime;
        if (value >= max)
        {
            value = max;
            dir = -1;
        }
        else if (value <= -max)
        {
            value = -max;
            dir = 1;
        }
        return value;
    }

    public void SetRotating(bool on)
    {
        rotating = on;
        if (on) resetting = false;
    }

    public void SetMovingVertical(bool on)
    {
        movingVertical = on;
        if (on) resetting = false;
    }

    public void SetMovingHorizontal(bool on)
    {
        movingHorizontal = on;
        if (on) resetting = false;
    }

    public void ResetPlatform()
    {
        rotating = false;
        movingVertical = false;
        movingHorizontal = false;
        verticalDir = 1;
        horizontalDir = 1;
        resetting = true;
    }
}
