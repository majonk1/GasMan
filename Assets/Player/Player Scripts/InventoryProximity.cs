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
    
    /// <summary>
    /// Checks if a world position is inside the pickup collider bounds.
    /// </summary>
    /// <param name="worldPos">The world position to check.</param>
    /// <returns>True if the position is inside the pickup collider; otherwise false.</returns>
    public bool IsInsideProximity(Vector3 worldPos)
    {
        if (pickupCollider == null) return false;
        return pickupCollider.bounds.Contains(worldPos);
    }
    
    /// <summary>
    /// Manually registers a collectible as being in proximity.
    /// Used to handle edge cases when the game is paused after dropping an item.
    /// </summary>
    /// <param name="c">The collectible to register.</param>
    /// <returns>void</returns>
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
        if (other.gameObject.CompareTag("Collectible"))
        {
            if (other.TryGetComponent<Collectible>(out var c))
                playerInventory.OnCollectibleExit(c);
        }
    }
}
