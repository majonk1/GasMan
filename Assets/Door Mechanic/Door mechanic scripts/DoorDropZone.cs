using System;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class DoorDropZone : MonoBehaviour
{
    [SerializeField] private TextMeshPro doorText;
    private float weightInDropZone = 0;

    [SerializeField] private GameObject attachedDoor;
    [SerializeField] private float equalAmountToOpenTheDoor;
    //reference to the director controlling the timeline component
    public PlayableDirector doorDirector;

    public AudioClip doorOpenSound;
    public float doorVolume = 1f;
    
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
            if (AudioManager.Instance != null && doorOpenSound != null)
            {
                AudioManager.Instance.PlayOneShot2D(doorOpenSound, doorVolume);
            }
            
            //checks if the director is assigned
            if (doorDirector != null)
        {
            //resets timeline to 0 so plays animation from begininng
            doorDirector.time = 0;
            //plays the animation
            doorDirector.Play();
        }
            //removes the door a second after animation finsihes did this becuase before was removing the
            //whilst animating so added the delay so the animation plays before its removed
            Invoke(nameof(HideDoor), 1f);
            Destroy(_collectible);
        }
    }

    private void HideDoor()
    {
        openTheDoorsDoor(false);
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
