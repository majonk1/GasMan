using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public float weight;
    public GameObject dropPrefab; // prefab to instantiate when dropping
    public bool occupied => dropPrefab != null;
}
