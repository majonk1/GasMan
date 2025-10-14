using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Snapshot
{
    public List<CollectibleState> collectibles = new List<CollectibleState>();
    public List<PlayerStatus> players = new List<PlayerStatus>();
    public FloorState floor;
    public List<DoorState> doors;
}