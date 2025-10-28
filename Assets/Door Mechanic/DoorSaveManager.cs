using System.Collections.Generic;
using UnityEngine;

/*
 * Manages saving and loading of door states in the scene.
 *
 * Uses finf all tags to find all doors in the scene -> can cause performance issues like a lag spike if we need to scale the scene. 
 */
public class DoorSaveManager : MonoBehaviour
{

    /// <summary>
    /// Assigns unique IDs to each door in the scene and records their initial state.
    /// </summary>
    /// <param name="doorTag">Tag used to find doors in the scene.</param>
    /// <returns>void</returns>
    public void SetDoorIDs(string doorTag)
    {
        List<DoorState> doorStates = new List<DoorState>();
        
        GameObject[] doors = GameObject.FindGameObjectsWithTag(doorTag);
        int doorIndex = 0;

		//Assign an ID to each door
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
    
    /// <summary>
    /// Collects the current state (id and open/closed) of all doors.
    /// </summary>
    /// <param name="doorTag">Tag used to find doors in the scene.</param>
    /// <returns>List; representing all door states.</returns>
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

    /// <summary>
    /// Applies a previously saved list of door states to the corresponding doors in the scene.
    /// </summary>
    /// <param name="savedDoors">List of previously saved door states.</param>
    /// <param name="doorTag">Tag used to find doors in the scene.</param>
    /// <returns>void</returns>
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
