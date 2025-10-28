using System;
using UnityEngine;

[DisallowMultipleComponent]
public class DoorIdentifier : MonoBehaviour
{
    public int doorId;

    public bool isOpen = false;
    
    private DoorDropZone _doorDropZone;
    public bool GetOpen() => isOpen;

    private void Awake()
    {
        _doorDropZone = GetComponentInChildren<DoorDropZone>();
    }

	/// <summary>
    /// Sets door open.
    /// </summary>
    /// <param name="open">Set door to open</param>
    /// <returns>void</returns>
    public void SetOpen(bool open)
    {
        isOpen = open;
        _doorDropZone.openTheDoorsDoor(isOpen);
    }
}