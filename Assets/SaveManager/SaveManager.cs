using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [Header("References")]
    public GameObject collectiblePrefab;
    public string collectibleTag = "Collectible";
    public string playerTag = "Player";

    // We still store a Snapshot object, but only one (index 0). Always overwrite.
    //Therefore we dont need an Array
    public Snapshot savedSnapshot = null;

    private FloorController floorController;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        GameObject floorObj = GameObject.FindGameObjectWithTag("Floor");
        if (floorObj != null)
        {
            floorController = floorObj.GetComponent<FloorController>();
        }
    }

    private void Update()
    {
        // debug shortcuts
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SaveOverwrite();
            Debug.Log("[SaveManager] Saved overwrite snapshot (Q).");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestoreSavedSnapshot();
            Debug.Log("[SaveManager] Restored saved snapshot (R).");
        }
    }

    private Snapshot CreateSnapshot()
    {
        return new Snapshot
        {
            collectibles = GatherCollectibles(),
            players = GatherPlayers(),
            floor = floorController != null ? floorController.GetFloorState() : null
        };
    }

    public void SaveOverwrite()
    {
        savedSnapshot = CreateSnapshot();
    }

    /// Restore the saved snapshot (if any). Safe to call even if no saved snapshot exists.
    public void RestoreSavedSnapshot()
    {
        if (savedSnapshot == null)
        {
            Debug.LogWarning("[SaveManager] No saved snapshot to restore.");
            return;
        }

        ClearExistingCollectibles();
        SpawnCollectiblesFromSnapshot(savedSnapshot.collectibles);
        RestorePlayerFromSnapshot(savedSnapshot.players);

        if (floorController != null && savedSnapshot.floor != null)
        {
            floorController.ApplyFloorState(savedSnapshot.floor);
        }
    }

    private List<CollectibleState> GatherCollectibles()
    {
        var list = new List<CollectibleState>();
        var cols = GameObject.FindGameObjectsWithTag(collectibleTag);
        foreach (var go in cols)
        {
            var c = go.GetComponent<Collectible>();
            if (c == null) continue;
            list.Add(new CollectibleState
            {
                position = go.transform.position,
                weight = c.weight
            });
        }
        return list;
    }

    private List<PlayerStatus> GatherPlayers()
    {
        var list = new List<PlayerStatus>();
        var p = GameObject.FindGameObjectWithTag(playerTag);
        if (p == null) return list;
        var inv = p.GetComponent<PlayerInventory>();
        if (inv == null)
        {
            list.Add(new PlayerStatus
            {
                playerName = p.name,
                position = p.transform.position,
                slotWeights = new float[0],
                currentWeight = 0f
            });
        }
        else
        {
            var status = inv.GetPlayerStatus();
            status.position = p.transform.position;
            status.playerName = p.name;
            list.Add(status);
        }
        return list;
    }

    private void ClearExistingCollectibles()
    {
        GameObject[] existing = GameObject.FindGameObjectsWithTag(collectibleTag);
        for (int i = existing.Length - 1; i >= 0; i--)
        {
            Destroy(existing[i]);
        }
    }

    private void SpawnCollectiblesFromSnapshot(List<CollectibleState> collectibles)
    {
        if (collectiblePrefab == null) return;
        if (collectibles == null || collectibles.Count == 0) return;

        foreach (CollectibleState collectible in collectibles)
        {
            GameObject go = Instantiate(collectiblePrefab, collectible.position, Quaternion.identity);
            var c = go.GetComponent<Collectible>();
            if (c != null)
            {
                c.weight = collectible.weight;
            }

            var tmp = go.GetComponentInChildren<TextMeshPro>();
            if (tmp != null)
            {
                tmp.text = collectible.weight.ToString();
            }
        }
    }

    private void RestorePlayerFromSnapshot(List<PlayerStatus> savedPlayers)
    {
        var player = GameObject.FindGameObjectWithTag(playerTag);
        if (player == null)
        {
            Debug.LogError("[SaveManager] No player found to restore.");
            return;
        }

        if (savedPlayers == null || savedPlayers.Count == 0)
        {
            Debug.LogWarning("[SaveManager] Saved player data is empty.");
            return;
        }

        PlayerStatus saved = savedPlayers[0];

        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
            player.transform.position = saved.position;
            controller.enabled = true;
        }
        else
        {
            player.transform.position = saved.position;
        }

        PlayerInventory inv = player.GetComponent<PlayerInventory>();
        if (inv != null)
        {
            inv.ApplyPlayerStatus(saved);
        }
    }
}