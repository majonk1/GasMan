using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Snapshot
{
    public int index; // need?
    public List<CollectibleState> collectibles = new List<CollectibleState>();
    public List<PlayerStatus> players = new List<PlayerStatus>();
    public FloorState floor;
}