using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the player's current floating weight value in the UI
/// and updates its color dynamically based on weight.
/// Integrates with SetDropColour for consistent weight-based coloring.
/// </summary>
public class WeightDisplay : MonoBehaviour
{
    public TextMeshProUGUI weightText;

    /// <summary>
    /// Refreshes the displayed weight value and updates the text color.
    /// </summary>
    /// <param name="weight">The current floating weight value.</param>
    /// <returns>void</returns>
    public void Refresh(float weight)
    {
        weightText.text = $"Floating Value: {weight:F0}";
        
        if (SetDropColour.Instance != null)
        {
            Color col = SetDropColour.Instance.GetColorForWeight(weight);
            col.a = 1f; // alpha is 0 without this
            weightText.color = col;
        }
    }
    
}
