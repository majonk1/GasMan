using UnityEngine;

public class CollectibleSounds : MonoBehaviour
{
    //pickup sound clip
    public AudioSource pickupSource;
    //drop sound clip
    public AudioSource dropSource;
    
    //if player picks up collectible play the pickup sound
    public void PlayPickup()
    {
        if (pickupSource && pickupSource.clip)
            AudioManager.Instance.PlayOneShot2D(pickupSource.clip, pickupSource.volume);
        
    }
    
    //if the player drops a collectible play the drop sound
    public void PlayDrop()
    {
        if (dropSource && dropSource.clip)
            AudioManager.Instance.PlayOneShot2D(dropSource.clip, dropSource.volume);
    }
}