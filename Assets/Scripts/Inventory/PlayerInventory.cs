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

    public Item[] slots; 
    public InventorySlotUI[] slotUI;
    public Transform dropPoint;

    [Header("Settings")]
    public GameObject circlePrefab; 
    public GameObject inventoryUI;
    
    [SerializeField] private PlayerInventory playerInventory;

    void Start()
    {
        slots = new Item[4];
        
        for (int i = 0; i < slotUI.Length; i++)
            slotUI[i].Setup(i, this);

        RefreshUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && inventoryUI != null)
            inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    public bool AddItem(float weight)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty)
            {
                slots[i] = new Item { weight = weight };
                RefreshUI();
                
                BasicPlayerControls controls = GetComponent<BasicPlayerControls>();
                controls.SetWeight(CurrentWeight);
                
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
    
        var controls = GetComponent<BasicPlayerControls>();
        controls.RemoveWeight(droppedWeight);
        
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
}