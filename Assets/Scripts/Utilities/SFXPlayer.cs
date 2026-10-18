using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    private static SFXPlayer s_instance;

    private void Awake()
    {
        if (s_instance != null)
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(this);
            s_instance = this;
        }
    }

    /// <summary>
    /// Creates a new object with a copy of the 
    /// source AudioSource, plays it, and sets it to
    /// be destroyed after it finishes.
    /// </summary>
    /// <param name="source"></param>
    static public void Play(AudioSource source)
    {
        if (source == null)
        {
            Debug.LogWarning("Tried playing a null AudioSource!");
            return;
        }

        GameObject sourceObject = source.gameObject;

        GameObject newObject = new(source.clip.name + "AudioSource");
        newObject.transform.position = sourceObject.transform.position;
        newObject.AddComponent<AudioSource>(); 

        // Copy all the parameters over.
        // Not all the parameters are accounted for,
        // but I can always expand this list later if needed.
        //
        // I could cache the newObject in a dictionary
        // for each audio source and duplicate that.
        // I could have used Reflection but that is very slow.
        // Maybe if I cache them I could use it then since I only
        // do it once per source?
        AudioSource newAudioSource = newObject.GetComponent<AudioSource>();
        
        newAudioSource.clip = source.clip;
        newAudioSource.loop = false;
        newAudioSource.priority = source.priority;
        newAudioSource.volume = source.volume;
        newAudioSource.pitch = source.pitch;
        newAudioSource.panStereo = source.panStereo;
        newAudioSource.spatialBlend = source.spatialBlend;

        newAudioSource.Play();
        Destroy(newObject, source.clip.length);
    }
}
