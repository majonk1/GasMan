using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeightDisplay : MonoBehaviour
{
    public TextMeshProUGUI weightText;

    public void Refresh(float weight)
    {
        weightText.text = $"Your Weight: {weight:F0}";
    }
}
