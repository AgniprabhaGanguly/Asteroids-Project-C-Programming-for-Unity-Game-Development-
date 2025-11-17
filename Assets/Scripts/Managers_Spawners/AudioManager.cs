using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [SerializeField] private AudioSource soundFXObject;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlaySoundFX(AudioClip clip, Transform spawnTransform, float volume)
    {
        // spawn in object
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        
        // assign audio clip
        audioSource.clip = clip;
        
        // adjust volume
        audioSource.volume = volume;
        
        //play clip
        audioSource.Play();
        
        //get length of clip
        float clipLength = clip.length;
        
        //destroy object after clip ends
        Destroy(audioSource.gameObject, clipLength);
    }
    
}
