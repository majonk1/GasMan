using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public FloorController floorController;
     
    [Header("Inventory")]
    public Item[] slots; 
    public InventoryUI[] slotUI;
    public Transform dropPoint;

    [Header("Settings")]
    public GameObject circlePrefab; 
    public GameObject inventoryUI;
    
    // runtime
    public List<Collectible> nearbyCollectibles = new List<Collectible>();
    private bool inventoryOpen = false;
    private Transform pickupTriggerObject;
    
    [SerializeField] private InventoryNearbyUI nearbyUI;
    private PlayerMovement _playerMovement;
    
    [Header("TextMeshProUGUI")]
    [SerializeField] private TextMeshProUGUI slotsCountText;
    [SerializeField] private TextMeshProUGUI showCal;
    [SerializeField] private TextMeshProUGUI totalNoRound;
    [SerializeField] private TextMeshProUGUI divideAmount;
    
    
    //There are two weight UI's, one top left, on in inventory, therefore need an array
    [SerializeField] private WeightDisplay weightDisplay;

    [SerializeField] private WeightDisplay playerInventoryWeightText;

    private InventoryProximity _inventoryProximity;
    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();

        _inventoryProximity = GetComponentInChildren<InventoryProximity>();
        
        
        weightDisplay = GameObject.FindGameObjectWithTag("WeightDisplay").GetComponent<WeightDisplay>();
    }

    void Start()
    {
        //slots = new Item[4];
        
        for (int i = 0; i < slotUI.Length; i++)
            slotUI[i].Setup(i, this);

	AddItem(0f); //this will just update the UI, because the refreshUI() doesnt the floating value indicator

        //AddItem(1f);
        
        RefreshUI();

        //Ensure floor matches initial weight
        RequestFloorMove();
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

    private void RequestFloorMove()
    {
        if (floorController != null)
            floorController.RequestMoveToTarget();
    }

    public bool AddItem(float weight)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty)
            {
                slots[i] = new Item { weight = weight };
                RefreshUI();
                
                RefreshWeightDisplay();
                _playerMovement.SetWeight(currentAvgWeight);

                RequestFloorMove();
                
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
        
        if (circlePrefab != null && dropPoint != null)
        {
            //Spawn Item
            GameObject weightPrefab = Instantiate(circlePrefab, dropPoint.position, Quaternion.identity);
            Collectible collectible = weightPrefab.GetComponent<Collectible>();
            collectible.weight = droppedWeight;

            //Force dropped item to show in proximity
            if (_inventoryProximity != null)
            {
                if (_inventoryProximity.IsInsideProximity(weightPrefab.transform.position))
                    _inventoryProximity.RegisterCollectible(collectible);
            }
            
            //Set Ui
            TextMeshPro weightText = weightPrefab.GetComponentInChildren<TextMeshPro>();
            weightText.text = droppedWeight.ToString();
            weightText.color = Color.black;
            
            SetGasColour(weightPrefab, droppedWeight);
            
            //Sound
            var sfx = weightPrefab.GetComponent<CollectibleSounds>();
            sfx.PlayDrop();
        }

        slots[index].weight = 0;
    
        RefreshWeightDisplay();
        
        _playerMovement.SetWeight(currentAvgWeight);
        
        RefreshUI();

        RequestFloorMove();
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

        UpdateSlotsCount();
    }

    private void RefreshWeightDisplay()
    {
        weightDisplay.Refresh(currentAvgWeight);
        playerInventoryWeightText.Refresh(currentAvgWeight);
    }

    public float currentAvgWeight
    {
        get
        {
            float total = 0f;
            int count = 0;

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

    private void SetGasColour(GameObject weightPrefab, float droppedWeight)
    {
        Collectible collectible = weightPrefab.GetComponent<Collectible>();
        collectible.GetComponentInChildren<TextMeshPro>().text = droppedWeight.ToString();
        
        //Gets colour
        Color colour = SetDropColour.Instance.GetColorForWeight(droppedWeight);
        
        //Sets colour
        collectible.SetVisualColor(colour);
    }

    internal void OnCollectibleEnter(Collectible collectible)
    {
        if (collectible == null) return;
        if (!nearbyCollectibles.Contains(collectible))
        {
            nearbyCollectibles.Add(collectible);
            nearbyUI.RefreshNearbyUI();
        }
    }

    internal void OnCollectibleExit(Collectible collectible)
    {
        if (collectible == null) return;
        nearbyCollectibles.Remove(collectible);
        nearbyUI.RefreshNearbyUI();
        
    }
    
    public void UpdateSlotsCount()
    {
        if (slots.Length == 0)
        {
            slotsCountText.text = $"Total: 0/0";
            return;
        }

        int occupied = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].IsEmpty)
            {
                occupied++;
            }
        }

        slotsCountText.text = $"Total: {occupied}/{slots.Length}";
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

        CollectibleSounds sfx = col.GetComponent<CollectibleSounds>();
        sfx.PlayPickup();

        bool added = AddItem(col.weight);
        if (added)
        {
            if (nearbyUI != null && nearbyUI.isActiveAndEnabled)
                nearbyUI.RefreshNearbyUI();
            
            if (!col.isInfinite)
            {
                nearbyCollectibles.RemoveAt(index);
                Destroy(col.gameObject);
            }
        }

        return added;
    }
    
    
    public PlayerStatus GetPlayerStatus()
    {
        var status = new PlayerStatus
        {
            playerName = this.gameObject.name,
            slotWeights = new float[slots != null ? slots.Length : 0],
            currentWeight = this.currentAvgWeight
        };

        if (slots != null)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                float weightForThisSlot = 0f;

                if (!slots[i].IsEmpty)
                {
                    weightForThisSlot = slots[i].weight;
                }

                status.slotWeights[i] = weightForThisSlot;
            }
        }

        return status;
    }
    
    public void ApplyPlayerStatus(PlayerStatus status)
    {
        if (status == null) return;

        if (slots == null || slots.Length != status.slotWeights.Length)
        {
            slots = new Item[status.slotWeights.Length];
        }

        for (int i = 0; i < status.slotWeights.Length; i++)
        {
            float w = status.slotWeights[i];
            if (w > 0f)
                slots[i] = new Item { weight = w };
            else
                //Empty
                slots[i] = new Item(); 
        }

        RefreshUI();
        RefreshWeightDisplay();
        if (_playerMovement != null) _playerMovement.SetWeight(currentAvgWeight);

        RequestFloorMove();
    }
}
