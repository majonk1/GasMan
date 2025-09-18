using System;
using TMPro;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float weight = 5f;
    [SerializeField] TextMeshPro weightText;
    
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