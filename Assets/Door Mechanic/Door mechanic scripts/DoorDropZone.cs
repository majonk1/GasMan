using System;
using TMPro;
using UnityEngine;

public class DoorDropZone : MonoBehaviour
{
    [SerializeField] private TextMeshPro doorText;
    private float weightInDropZone = 0;

    [SerializeField] private GameObject attachedDoor;
    [SerializeField] private float equalAmountToOpenTheDoor;

    private GameObject _collectible;

    private void Start()
    {
        UpdateDoorUi();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            weightInDropZone = other.GetComponent<Collectible>().weight;
            isDoorOpen(other.gameObject);
            
            
        }
    }

    private void isDoorOpen(GameObject _collectible)
    {
        if (weightInDropZone == equalAmountToOpenTheDoor)
        {
            openTheDoorsDoor(false);
            Destroy(_collectible);
        }
    }
    
    /// <summary>
    /// Toggles the door's active state and updates the UI visibility.
    /// </summary>
    /// <param name="open">True to show the door, false to hide (open it).</param>
    /// <returns>void</returns>
    public void openTheDoorsDoor(bool open)
    {
        attachedDoor.SetActive(open);
        doorText.enabled = open;
    }

    private void UpdateDoorUi()
    {
        doorText.text = $"Weight: {equalAmountToOpenTheDoor}";
    }
}
