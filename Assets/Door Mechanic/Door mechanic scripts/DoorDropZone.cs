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
