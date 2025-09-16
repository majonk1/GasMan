using System;
using TMPro;
using UnityEngine;

public class DoorDropZone : MonoBehaviour
{
    [SerializeField] private TextMeshPro doorText;
    [SerializeField] private float weightInDropZone = 0;

    [SerializeField] private GameObject attachedDoor;
    [SerializeField] private float amountToOpenTheDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            //How does player round effect this?
            weightInDropZone += other.GetComponent<Collectible>().weight;
            UpdateDoorUi();
            isDoorOpen();
        }
    }

    private void isDoorOpen()
    {
        if (weightInDropZone >= amountToOpenTheDoor)
        {
            attachedDoor.SetActive(false);
            doorText.enabled = false;
        }
    }

    private void UpdateDoorUi()
    {
        doorText.text = $"Weight: {weightInDropZone}";
    }
}
