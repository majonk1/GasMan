using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float weight = 5f;

    void Reset()
    {
        var c = GetComponent<Collider>();
        if (c) c.isTrigger = true;
    }
}