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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddSnapshot();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            QuickLoadFirstSnapshot();
            
        }
    }

    private Snapshot CreateSnapshot()
    {
        return new Snapshot
        {
            collectibles = GatherCollectibles(),
            players = GatherPlayers()
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
        RestorePlayersFromSnapshot(snap.players);
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

    private void RestorePlayersFromSnapshot(List<PlayerStatus> savedPlayers)
    {
        GameObject scenePlayer = GameObject.FindGameObjectWithTag(playerTag);

        PlayerStatus saved = savedPlayers[0];
        scenePlayer.transform.position = saved.position;
        PlayerInventory inv = scenePlayer.GetComponent<PlayerInventory>();
        if (inv != null)
        {
            inv.ApplyPlayerStatus(saved);
        }
    }

    private void QuickLoadFirstSnapshot()
    {
        if (snapshots == null || snapshots.Count == 0)
        {
            Debug.LogWarning("There are no snap shots");
            return;
        }
        RestoreSnapshot(0);
    }
}
