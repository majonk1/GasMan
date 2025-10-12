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

    public void SetOpen(bool open)
    {
        isOpen = open;
        _doorDropZone.openTheDoorsDoor(isOpen);
    }
}