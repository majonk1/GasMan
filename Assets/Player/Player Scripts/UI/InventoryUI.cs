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


    public void Setup(int index, PlayerInventory inv)
    {
        slotIndex = index;
        _playerInventory = inv;

        dropButton.onClick.AddListener(() => _playerInventory.DropItem(slotIndex));
        gameObject.SetActive(false); // start hidden
    }

    public void UpdateSlot(float weight, bool occupied)
    {
        this.gameObject.SetActive(occupied);
            //InventorySlotPrefab.SetActive(occupied);

        if (occupied)
            weightText.text = $"Float Value: {weight:F0}";
        
            //set text colour
            /*
            Color col = SetDropColour.Instance.GetColorForWeight(weight);
            col.a = 1f; // alpha is 0 without this
            weightText.color = col;
            */
    }
    

}