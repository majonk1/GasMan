using UnityEngine;

/// <summary>
/// Manages the items weight and its corresponding display color.
/// Uses a singleton pattern to ensure a single global instance persists across scenes.
/// </summary>
public class SetDropColour : MonoBehaviour
{
    /// <summary>
    /// Defines the gass colour and weight.
    /// </summary>
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
      
	/// <summary>
    /// Returns the assigned color for the given weight.
    /// If no match is found, returns the default color.
    /// </summary>
    /// <param name="weight">Weight to look up.</param>
    /// <returns>Color to the given weight or default color if its invalid.</returns>
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