using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeightDisplay : MonoBehaviour
{
    public PlayerMovement player;
    public TextMeshProUGUI weightText;

    void Update()
    {
        // :)
        weightText.text = $"Your Weight: {player.currentWeight:F0}";
    }
}
