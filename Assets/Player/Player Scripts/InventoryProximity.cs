using UnityEngine;

public class InventoryProximity : MonoBehaviour
{
    public GameObject pickupColliderObject;

    PlayerInventory playerInventory;
    
    void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            Collectible c = other.GetComponent<Collectible>();
            playerInventory.OnCollectibleEnter(c);
        }

    }

    void OnTriggerExit(Collider other)
    {
        
        if (other.TryGetComponent<Collectible>(out var c))
            playerInventory.OnCollectibleExit(c);
    }

}
