using System;
using TMPro;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float weight = 5f;
    [SerializeField] TextMeshPro weightText;
    public bool isInfinite = false;
    
    private void Start()
    {
        weightText.text = weight.ToString();
        
        Color color = SetDropColour.Instance.GetColorForWeight(weight);
        SetVisualColor(color);
    }

    void Reset()
    {
        Collider collider = GetComponent<Collider>();
        if (collider) collider.isTrigger = true;
    }
    
    /// <summary>
    /// Sets the visual color of the collectible’s mesh based on the provided color.
    /// </summary>
    /// <param name="colour">The color to apply to the collectible's material.</param>
    /// <returns>void</returns>
    public void SetVisualColor(Color colour)
    {
        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.material.color = colour;
    }
}