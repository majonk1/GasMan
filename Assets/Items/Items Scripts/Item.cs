using UnityEngine;

[System.Serializable]
public struct Item
{
    public float weight;
        
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
