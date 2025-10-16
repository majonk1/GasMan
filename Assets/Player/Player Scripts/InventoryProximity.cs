using UnityEngine;

public class InventoryProximity : MonoBehaviour
{
    public GameObject pickupColliderObject;

    PlayerInventory playerInventory;
    
    void Awake()
    {
        playerInventory = GetComponentInParent<PlayerInventory>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            Collectible collectible = other.GetComponent<Collectible>();
            playerInventory.OnCollectibleEnter(collectible);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Collectible>(out var c))
            playerInventory.OnCollectibleExit(c);
    }

}
