using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveManager : MonoBehaviour
{
    public GameObject collectiblePrefab;
    public string collectibleTag = "Collectible";
    public string playerTag = "Player";
    public List<Snapshot> snapshots = new List<Snapshot>();


    private FloorController floorController;

    private void Awake()
    {
        floorController = GameObject.FindGameObjectWithTag("Floor").GetComponent<FloorController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddSnapshot();
        }

        //for debug and testing
        if (Input.GetKeyDown(KeyCode.Alpha1)) RestoreSnapshot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) RestoreSnapshot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) RestoreSnapshot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) RestoreSnapshot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) RestoreSnapshot(4);
    }

    private Snapshot CreateSnapshot()
    {
        return new Snapshot
        {
            collectibles = GatherCollectibles(),
            players = GatherPlayers(),
            floor = floorController.GetFloorState() 
            
        };
    }

    public void AddSnapshot()
    {
        var snap = CreateSnapshot();
        snapshots.Add(snap);
    }

    public void SaveFirstSnapshot()
    {
        var snap = CreateSnapshot();
        if (snapshots.Count == 0)
        {
            snapshots.Insert(0, snap);
        }
        else
        {
            Debug.LogError("Indexing problem");
            snapshots[0] = snap;
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

    public void RestoreSnapshot(int index)
    {
        if (!ValidateSnapshotIndex(index)) return;
        Snapshot snap = snapshots[index];
        ClearExistingCollectibles();
        SpawnCollectiblesFromSnapshot(snap.collectibles);
        RestorePlayerFromSnapshot(snap.players);
        
        floorController.ApplyFloorState(snap.floor);
        
    }

    private bool ValidateSnapshotIndex(int index)
    {
        if (index < 0 || index >= snapshots.Count)
        {
            Debug.LogError($"[SaveManager] RestoreSnapshot: index {index} is out of range (count={snapshots.Count}).");
            return false;
        }
        return true;
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
            go.GetComponent<Collectible>();
            collectible.weight = collectible.weight;
            
            var tmp = go.GetComponentInChildren<TextMeshPro>();
            tmp.text = collectible.weight.ToString();
        }
    }

    private void RestorePlayerFromSnapshot(List<PlayerStatus> savedPlayer)
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        PlayerStatus saved = savedPlayer[0];
        CharacterController controller = player.GetComponent<CharacterController>();
        
        controller.enabled = false;
        player.transform.position = saved.position;
        controller.enabled = true;
        
        PlayerInventory inv = player.GetComponent<PlayerInventory>();
        if (inv != null)
        {
            inv.ApplyPlayerStatus(saved);
        }
    }
}
