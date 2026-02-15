using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{

    private AudioSource source; // Reference to the AudioSource component
    public AudioClip soundEffectClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();
        
    }
public void PlaySFX()
    {
        if (source != null && soundEffectClip != null)
        {
            // Play the sound effect once using PlayOneShot for non-looping SFX
            source.PlayOneShot(soundEffectClip, 1.0f); // 1.0f is the volume scale
        }
    }
    // Update is called once per frame
}

