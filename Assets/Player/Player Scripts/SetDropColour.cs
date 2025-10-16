using UnityEngine;

public class SetDropColour : MonoBehaviour
{
    [System.Serializable]
    public struct WeightColour
    {
        public int weight;
        public Color color;
    }

    public WeightColour[] weightColours;
    public Color defaultColour = Color.white;

    // Singleton instance 
    public static SetDropColour Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Get colour for a given weight
    public Color GetColorForWeight(float weight)
    {
        int _weight = Mathf.RoundToInt(weight);

        foreach (var weightColour in weightColours)
        {
            if (weightColour.weight == _weight)
                return weightColour.color;
        }

        Debug.LogWarning($"No colour found for weight {_weight}, using default instead");
        return defaultColour;
    }
}