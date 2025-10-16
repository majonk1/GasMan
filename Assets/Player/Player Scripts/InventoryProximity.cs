using UnityEngine;

public class InventoryProximity : MonoBehaviour
{
    public GameObject pickupColliderObject;

    PlayerInventory playerInventory;
    
    [SerializeField] private Collider pickupCollider;
    void Awake()
    {
        playerInventory = GetComponentInParent<PlayerInventory>();
    }
    
    // checks if collectible is in player inv proximity
    public bool IsInsideProximity(Vector3 worldPos)
    {
        if (pickupCollider == null) return false;
        return pickupCollider.bounds.Contains(worldPos);
    }
    
    // Hard coded to fix a bug where when we drop a Collectible it doesnt show in this script as the game is paused.
    public void RegisterCollectible(Collectible c)
    {
        if (c == null) return;
        playerInventory.OnCollectibleEnter(c);
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
