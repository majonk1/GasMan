using UnityEngine;

public class DropPointController : MonoBehaviour
{
    public Transform leftDropPoint;

    public Transform rightDropPoint;

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

    private void SetLeft()
    {
        ActiveDropPoint = leftDropPoint ?? transform;
    }

    private void SetRight()
    {
        ActiveDropPoint = rightDropPoint ?? transform;
    }
}