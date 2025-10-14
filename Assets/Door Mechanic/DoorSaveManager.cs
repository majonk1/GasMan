using System.Collections.Generic;
using UnityEngine;

public class DoorSaveManager : MonoBehaviour
{
    public void SetDoorIDs(string doorTag)
    {
        List<DoorState> doorStates = new List<DoorState>();
        
        GameObject[] doors = GameObject.FindGameObjectsWithTag(doorTag);
        int doorIndex = 0;
        foreach (GameObject door in doors)
        {
            DoorIdentifier doorIdentifier = door.GetComponent<DoorIdentifier>();

            doorStates.Add(new DoorState
            {
                id = doorIndex,
                //set door to false
                isOpen = doorIdentifier.GetOpen()
            });

            doorIdentifier.doorId = doorIndex;
            
            doorIndex++;
        }
    }
    
    public List<DoorState> GatherDoors(string doorTag)
    {
        List<DoorState> doorStates = new List<DoorState>();

        GameObject[] doors = GameObject.FindGameObjectsWithTag(doorTag);
        foreach (GameObject door in doors)
        {
            DoorIdentifier doorIdentifier = door.GetComponent<DoorIdentifier>();

            doorStates.Add(new DoorState
            {
                id = doorIdentifier.doorId,
                isOpen = doorIdentifier.GetOpen()
            });
        }
        return doorStates;
    }

    public void ApplyDoorStates(List<DoorState> savedDoors, string doorTag)
    {
        GameObject[] doorObjs = GameObject.FindGameObjectsWithTag(doorTag);

        foreach (GameObject door in doorObjs)
        {
            DoorState doorState = door.GetComponent<DoorState>();

            if (doorState != null)
            {
                DoorIdentifier doorIdentifier = door.GetComponent<DoorIdentifier>();

                if (doorIdentifier.doorId == doorState.id)
                {
                    doorIdentifier.SetOpen(doorState.isOpen);
                    break;
                }
            }
        }
    }
}
