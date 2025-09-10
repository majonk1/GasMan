using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeightDisplay : MonoBehaviour
{
    public PlayerMovement player;
    public TextMeshProUGUI weightText;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // :)
        weightText.text = $"Your Weight: {player.currentWeight:F0}";
    }
}
