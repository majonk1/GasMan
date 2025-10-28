using System;
using UnityEngine;


/// <summary>
/// Represents a snapshot of the player's state at a given point in time.
/// Stores:
///  - Inventory slot data (items and weights)
///  - Player position in the world
///  - Timestamp when the snapshot was created
/// </summary>
public class SaveState
{
    public Item[] slotsSnapshot;
    public Vector3 playerPosition;
    public float timestamp;

    /// <summary>
    /// Creates a new save state snapshot.
    /// Copies the current inventory slots and stores the player position and a timestamp.
    /// </summary>
    /// <param name="slots">The player's current inventory slots.</param>
    /// <param name="pos">The player's current world position.</param>
    /// <returns>SaveState instance containing the snapshot.</returns>
    public SaveState(Item[] slots, Vector3 pos)
    {
        slotsSnapshot = new Item[slots.Length];
        Array.Copy(slots, slotsSnapshot, slots.Length);
        playerPosition = pos;
        timestamp = Time.time;
    }
}
