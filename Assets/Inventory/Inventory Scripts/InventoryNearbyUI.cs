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
        for (int i = 0; i < list.Count; i++)
        {
            var collectible = list[i];
            if (collectible == null) continue;

            var go = Instantiate(nearbyItemPrefab, spawnItemsTo);
            entries.Add(go);

            var weightText = go.GetComponentInChildren<TextMeshProUGUI>();
            var button = go.GetComponentInChildren<Button>();

            if (weightText != null)
                weightText.text = $"Weight: {collectible.weight:F1}";

            if (button != null)
            {
                int capturedIndex = i;
                button.onClick.AddListener(() =>
                {
                    bool added = playerInventory.PickupNearbyAt(capturedIndex);
                    // refresh UI after attempting pickup
                    RefreshNearbyUI();
                });
            }
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
