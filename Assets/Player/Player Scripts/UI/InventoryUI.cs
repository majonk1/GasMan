using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject InventorySlotPrefab;
    public TextMeshProUGUI weightText;    
    public Button dropButton; 
    private int slotIndex; 
    private PlayerInventory _playerInventory;

    /// <summary>
    /// Initializes the inventory slot UI with its index and a reference to the player inventory.
    /// Also wires up the drop button to drop the correct item when clicked.
    /// </summary>
    /// <param name="index">Slot index in the inventory array.</param>
    /// <param name="inv">Reference to the player's inventory</param>
    /// <returns>void</returns>
    public void Setup(int index, PlayerInventory inv)
    {
        slotIndex = index;
        _playerInventory = inv;

        dropButton.onClick.AddListener(() => _playerInventory.DropItem(slotIndex));
        gameObject.SetActive(false); // start hidden
    }

    /// <summary>
    /// Updates the slot UI based on whether the slot is occupied and the weight of the item.
    /// </summary>
    /// <param name="weight">The weight value of the item in this slot.</param>
    /// <param name="occupied">Whether the slot is currently occupied.</param>
    /// <returns>void</returns>
    public void UpdateSlot(float weight, bool occupied)
    {
        this.gameObject.SetActive(occupied);
            //InventorySlotPrefab.SetActive(occupied);

        if (occupied)
            weightText.text = $"Float Value: {weight:F0}";
    }
    

}