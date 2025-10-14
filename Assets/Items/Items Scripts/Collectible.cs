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
    }

    void Reset()
    {
        var c = GetComponent<Collider>();
        if (c) c.isTrigger = true;
    }
}