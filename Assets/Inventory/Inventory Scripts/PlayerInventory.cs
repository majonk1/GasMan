using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
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

    [Header("Inventory")]
    public Item[] slots; 
    public InventorySlotUI[] slotUI;
    public Transform dropPoint;

    [Header("Settings")]
    public GameObject circlePrefab; 
    public GameObject inventoryUI;
    
    // runtime
    public List<Collectible> nearbyCollectibles = new List<Collectible>();
    private bool inventoryOpen = false;
    private Transform pickupTriggerObject;
    
    [SerializeField] private InventoryNearbyUI nearbyUI;
    private BasicPlayerControls _basicPlayerControls;
    private void Awake()
    {
        _basicPlayerControls = GetComponent<BasicPlayerControls>();
    }

    void Start()
    {
        slots = new Item[4];
        
        for (int i = 0; i < slotUI.Length; i++)
            slotUI[i].Setup(i, this);

        RefreshUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool isCurrentlyOpen = inventoryUI.activeSelf;
            bool shouldOpen = !isCurrentlyOpen;

            inventoryUI.SetActive(shouldOpen);

            if (shouldOpen)
            {
                //freeze
                Time.timeScale = 0f; 
            }
            else
            {
                //unfreeze
                Time.timeScale = 1f;
            }
        }

    }

    public bool AddItem(float weight)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty)
            {
                slots[i] = new Item { weight = weight };
                RefreshUI();
                
                _basicPlayerControls.SetWeight(CurrentWeight);
                
                return true;
            }
        }
        
        //Inv full
        return false; 
    }

    public void DropItem(int index)
    {
        if (index < 0 || index >= slots.Length) return;
        if (slots[index].IsEmpty) return;
        
        float droppedWeight = slots[index].weight;
        print("droppedWeight: " + droppedWeight);
        if (circlePrefab != null && dropPoint != null)
        {
            GameObject weightPrefab = Instantiate(circlePrefab, dropPoint.position, Quaternion.identity);
            weightPrefab.GetComponent<Collectible>().weight = droppedWeight;
        }

        // mark empty
        slots[index].weight = 0;
    
        _basicPlayerControls.SetWeight(CurrentWeight);
        //basicPlayerControls.RemoveWeight(droppedWeight);
        
        RefreshUI();
    }


    void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].IsEmpty)
                slotUI[i].UpdateSlot(slots[i].weight, true);
            else
                slotUI[i].UpdateSlot(0, false);
        }
    }
   
    

    public float CurrentWeight
    {
        get
        {
            return AverageWeight;
        }
    }
    

    public float AverageWeight
    {
        get
        {
            float total = 0f;
            int count = 0;

            print("length: " + slots.Length);
            foreach (var slot in slots)
            {
                
                if (!slot.IsEmpty)
                {
                    total += slot.weight;
                    count++;
                }
            }

            if (count == 0) return 0; 

            float avg = total / count;

            // round to nearest one (0.5 rounds up)
            return Mathf.RoundToInt(avg + 0.1f);
        } 
    }
    
    internal void OnCollectibleEnter(Collectible c)
    {
        if (c == null) return;
        if (!nearbyCollectibles.Contains(c))
        {
            nearbyCollectibles.Add(c);
            nearbyUI.RefreshNearbyUI();
        }
    }

    internal void OnCollectibleExit(Collectible c)
    {
        if (c == null) return;
        nearbyCollectibles.Remove(c);
        nearbyUI.RefreshNearbyUI();
        
    }
    
    public bool PickupNearbyAt(int index)
    {
        if (index < 0 || index >= nearbyCollectibles.Count) return false;
        var col = nearbyCollectibles[index];
        if (col == null)
        {
            nearbyCollectibles.RemoveAt(index);
            
            if (nearbyUI != null && nearbyUI.isActiveAndEnabled)
                nearbyUI.RefreshNearbyUI();
            
            return false;
        }

        bool added = AddItem(col.weight);
        if (added)
        {
            //Need to refresh 
            if (nearbyUI != null && nearbyUI.isActiveAndEnabled)
                nearbyUI.RefreshNearbyUI();
            
            nearbyCollectibles.RemoveAt(index);
            Destroy(col.gameObject);
        }

        return added;
    }
}