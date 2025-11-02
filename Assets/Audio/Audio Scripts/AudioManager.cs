using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //instance so so can call to play a sound
    public static AudioManager Instance
    {
        get; private set;
    }
    
    //used for one shots
    public AudioSource sfxSource;
    //used for looping sounds
    public AudioSource movementSource;
    //sound effect of when player is omving left or right
    public AudioClip playMovementClip;
    //sound effect for when player dies
    public AudioClip deathClip;
        
    //to initalise object
    void Awake()
    {
        //checks if there is already audio manager in the scene
        if (Instance == null)
        {
            //if no creates one
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitSources();
        }
        else if (Instance != this)
        {
            //if yeas keep it an ddestory the old duplicate
            var old = Instance;
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitSources();
            if (old != null)
            {
                Destroy(old.gameObject);
            }
        }
    }

    //prepares the looping audio source
    private void InitSources()
    {
        //if no audio source assigned use the one for sfxsource
        if (movementSource == null)
        {
            movementSource = sfxSource;
        }
        
        //configures audio settings
        if (movementSource != null)
        {
            movementSource.clip = playMovementClip;
            movementSource.loop = true;
            movementSource.playOnAwake = false;
            movementSource.spatialBlend = 0f;
        }
    }

    //enables and disables the looping audio
    public void SetMovementLoop(bool moving)
    {
        //dont do anything if theres noaudio source or audio clip
        if (movementSource == null || playMovementClip == null)
        {
            return;
        }
        
        //if player is moving and the audio not already playing then play the audio
        if (moving)
        {
            if (!movementSource.isPlaying) movementSource.Play();
        }
        //if stopped and auio still playing stop the audio
        else
        {
            if (movementSource.isPlaying) movementSource.Stop();
        }
    }
    
    //plays one shot sound
    public void PlayOneShot2D(AudioClip clip, float volume = 1f)
    {
        //doesnt try to play a null clip
        if (clip == null)
        {
            return;
        }

        //creates a temporary gameobject to ensure that mulitple sounds can overlap safely
        var go = new GameObject($"SFX_{clip.name}");
        DontDestroyOnLoad(go);
        
        //configures the audisource
        var src = go.AddComponent<AudioSource>();
        src.clip = clip;
        src.volume = volume;
        src.spatialBlend = 0f;
        src.playOnAwake = false;
        src.loop = false;
        
        //plays the sound and then deletes the object
        src.Play();
        Destroy(go, clip.length + 0.05f);
    }
    
    //plays the death sound effects
    public void PlayDeathSound(float volume = 1f)
    {
        //doesnt try to play a null clip
        if (deathClip == null)
        {
            return;
        }

        //stops the player moving sound so it doesnt overlap with the death sound
        SetMovementLoop(false);

        //creates a temporary gameobject to ensure that mulitple sounds can overlap safely
        var go = new GameObject("DeathSFX");
        DontDestroyOnLoad(go);

        //configures the audio
        var src = go.AddComponent<AudioSource>();
        src.spatialBlend = 0f;
        src.playOnAwake = false;
        src.loop = false;
        src.volume = volume;
        src.clip = deathClip;
        
        //plays the sound and then deletes the object
        src.Play();
        Destroy(go, deathClip.length + 0.1f);
    }

}
