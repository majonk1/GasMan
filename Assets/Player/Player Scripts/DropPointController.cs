using UnityEngine;

public class DropPointController : MonoBehaviour
{
    [Tooltip("Scene transform placed where you want drops on the left.")]
    public Transform leftDropPoint;

    [Tooltip("Scene transform placed where you want drops on the right.")]
    public Transform rightDropPoint;

    [Tooltip("If true, start with the left drop point active.")]
    public bool startLeft = true;

    public Transform ActiveDropPoint { get; private set; }

    void Start()
    {
        ActiveDropPoint = (startLeft ? leftDropPoint : rightDropPoint) ?? transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            SetLeft();
        else if (Input.GetKeyDown(KeyCode.D))
            SetRight();
    }

    public void SetLeft()
    {
        ActiveDropPoint = leftDropPoint ?? transform;
    }

    public void SetRight()
    {
        ActiveDropPoint = rightDropPoint ?? transform;
    }
}