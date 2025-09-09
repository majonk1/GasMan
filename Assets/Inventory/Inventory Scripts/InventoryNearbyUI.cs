using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple UI manager that lists nearby collectibles (from PlayerInventory.nearbyCollectibles).
/// Provide a prefab for each nearby item that has a TextMeshProUGUI for weight and a Button to pick the item.
/// </summary>
public class InventoryNearbyUI : MonoBehaviour
{
    public PlayerInventory playerInventory;

    public Transform spawnItemsTo;

    public GameObject nearbyItemPrefab;

    [SerializeField] private Transform topOfProximityMenu;

    // Keep track of instantiated entries so we can clear them
    private List<GameObject> entries = new List<GameObject>();

    void Start()
    {
        if (playerInventory == null)
            playerInventory = FindObjectOfType<PlayerInventory>();
        

        RefreshNearbyUI();
    }

    void OnEnable()
    {
        RefreshNearbyUI();
    }

    public void RefreshNearbyUI()
    {
        ClearEntries();

        if (playerInventory == null || nearbyItemPrefab == null || spawnItemsTo == null)
            return;

        var list = playerInventory.nearbyCollectibles;
        
        //independent of i
        int placedIndex = 0;
        for (int i = 0; i < list.Count; i++)
        {
            var collectible = list[i];
            if (collectible == null) continue;

            GameObject go = CreateEntry(collectible, i);
            PositionEntry(go, placedIndex);
            SetupEntryUI(go, collectible, i);

            entries.Add(go);
            placedIndex++;
        }
    }

    private GameObject CreateEntry(Collectible collectible, int index)
    {
        return Instantiate(nearbyItemPrefab, spawnItemsTo);
    }

    private void PositionEntry(GameObject go, int placedIndex)
    {
        RectTransform rt = go.GetComponent<RectTransform>();
        if (rt != null && topOfProximityMenu != null)
        {
            rt.pivot = new Vector2(0.5f, 1f); 
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 1f);
            rt.SetParent(topOfProximityMenu, false);

            //adjust prefab height
            float spacing = 40f; 
            rt.anchoredPosition = new Vector2(0, -placedIndex * spacing);
        }
    }

    private void SetupEntryUI(GameObject go, Collectible collectible, int index)
    {
        var weightText = go.GetComponentInChildren<TextMeshProUGUI>();
        if (weightText != null)
            weightText.text = $"Floating Value: {collectible.weight:F0}";

        var button = go.GetComponentInChildren<Button>();
        if (button != null)
        {
            int capturedIndex = index;
            button.onClick.AddListener(() =>
            {
                bool added = playerInventory.PickupNearbyAt(capturedIndex);
                RefreshNearbyUI();
            });
        }
    }

    void ClearEntries()
    {
        for (int i = 0; i < entries.Count; i++)
        {
            if (entries[i] != null) Destroy(entries[i]);
        }
        entries.Clear();
    }

    void OnDestroy()
    {
        ClearEntries();
    }
}
