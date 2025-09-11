using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip playMovementClip;
    private AudioSource movementSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (transform.parent != null)
            transform.SetParent(null, true);
        
        DontDestroyOnLoad(gameObject);

        movementSource = gameObject.AddComponent<AudioSource>();
        movementSource.clip = playMovementClip;
        movementSource.loop = true;
        movementSource.playOnAwake = false;
        movementSource.spatialBlend = 0f;
    }

    //movement
    public void SetMovementLoop(bool moving)
    {
        if (movementSource == null || playMovementClip == null) return;

        if (moving)
        {
            if (!movementSource.isPlaying) movementSource.Play();
        }
        else
        {
            if (movementSource.isPlaying) movementSource.Stop();
        }
    }
    
    //one shots
    public void PlayOneShot2D(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }

}
