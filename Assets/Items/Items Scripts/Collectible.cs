using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float weight = 5f;

    void Reset()
    {
        var c = GetComponent<Collider>();
        if (c) c.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        /*
        if (!other.CompareTag("Player")) return;

        PlayerInventory inv = other.GetComponent<PlayerInventory>();
        
        if (!inv)
        {
            Debug.LogWarning("Player has no Inventory component.");
            return;
        }

        bool added = inv.AddItem(weight);
        Debug.Log("Tried to add item. Success = " + added);

        if (added)
            Destroy(gameObject);
            */
    }
}