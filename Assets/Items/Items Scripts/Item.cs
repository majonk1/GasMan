using UnityEngine;

/// <summary>
/// Represents an inventory item.
/// Each item stores its weight and can be checked if it's considered empty.
/// </summary>
[System.Serializable]
public struct Item
{
    public float weight;
        
    /// <summary>
    /// Indicates whether the item is considered empty.
    /// An item is empty when its weight is equal to zero.
    /// </summary>
    /// <returns>True if the item has zero weight, false otherwise.</returns>
    public bool IsEmpty
    {
        get
        {
            if (weight == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
