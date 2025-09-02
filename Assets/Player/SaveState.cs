using System;
using UnityEngine;

public class SaveState
{
    public Item[] slotsSnapshot;
    public Vector3 playerPosition;
    public float timestamp;

    public SaveState(Item[] slots, Vector3 pos)
    {
        slotsSnapshot = new Item[slots.Length];
        Array.Copy(slots, slotsSnapshot, slots.Length);
        playerPosition = pos;
        timestamp = Time.time;
    }
}
