using UnityEngine;

public class FloorController : MonoBehaviour
{
    [Header("Weight -> Height mapping")]
    public PlayerInventory playerInventory; 
    public float minWeight = -50f; 
    public float maxWeight = 50f; 
    
    [Header("Movement")]
    //how weight it goes based of weight.
    public float minHeight = -2f;  
    public float maxHeight = 2f; 
    public float smoothTime = 0.25f;
    public bool useLocalPosition = true; 

    [Header("Debug")]
    public bool debugLogs = false;

    Vector3 velocity = Vector3.zero;
    Vector3 initialLocalPosition;
    Vector3 initialWorldPosition;

    void Start()
    {
        initialLocalPosition = transform.localPosition;
        initialWorldPosition = transform.position;
    }

    void Update()
    {
        if (playerInventory == null) return;

        float currentWeight = GetTotalWeight();

        // Map weight to 0..1
        float t = Mathf.InverseLerp(minWeight, maxWeight, currentWeight);
        t = Mathf.Clamp01(t);

        // Map to height
        float targetY = Mathf.Lerp(minHeight, maxHeight, t);

        if (useLocalPosition)
        {
            Vector3 targetLocal = new Vector3(initialLocalPosition.x, initialLocalPosition.y + targetY, initialLocalPosition.z);
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetLocal, ref velocity, smoothTime);
        }
        else
        {
            Vector3 targetWorld = new Vector3(initialWorldPosition.x, initialWorldPosition.y + targetY, initialWorldPosition.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetWorld, ref velocity, smoothTime);
        }

        if (debugLogs)
            Debug.Log($"[FloorController] Weight={currentWeight:F2}, t={t:F2}, targetY={targetY:F2}");
    }

    /// <summary>
    /// Prefer using the inventory's CurrentWeight property, but fall back to summing slots
    /// if CurrentWeight is zero but slot data indicates otherwise.
    /// </summary>
    float GetTotalWeight()
    {
        if (playerInventory == null) return 0f;

        float w = 0f;

        // Try property first
        try
        {
            w = playerInventory.CurrentWeight;
        }
        catch
        {
            w = 0f;
        }

        // If the property is zero but slots exist and contain weight, sum them directly (fallback)
        if (Mathf.Approximately(w, 0f) && playerInventory.slots != null && playerInventory.slots.Length > 0)
        {
            float sum = 0f;
            for (int i = 0; i < playerInventory.slots.Length; i++)
            {
                var s = playerInventory.slots[i];
                // s.IsEmpty is available because Item is public in PlayerInventory
                if (!s.IsEmpty)
                    sum += s.weight;
            }

            if (sum > 0f)
                w = sum;
        }

        return w;
    }

    void OnValidate()
    {
        // keep ranges sane in inspector
        if (maxWeight < minWeight) maxWeight = minWeight;
        if (maxHeight < minHeight) maxHeight = minHeight;
        if (smoothTime < 0.0001f) smoothTime = 0.0001f;
    }
}
