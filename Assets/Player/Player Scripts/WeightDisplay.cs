using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeightDisplay : MonoBehaviour
{
    public TextMeshProUGUI weightText;

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
