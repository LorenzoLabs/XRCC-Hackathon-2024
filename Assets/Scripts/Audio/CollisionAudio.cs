using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(MetaXRAudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]
public class CollisionAudio : MonoBehaviour
{
    public enum CollisionType
    {
        Soft,
        Medium,
        Hard
    }

    [System.Serializable]
    public class CollisionSettings
    {
        public float cutoffFrequency = 5000f; // Default value, can be adjusted in inspector
    }

    public CollisionType objectCollisionType;

    // Dictionary to store low pass filter settings for each collision combination
    private Dictionary<string, CollisionSettings> collisionSettings;

    public List<AudioClip> oneShotSounds;

    private AudioSource audioSource;
    private AudioLowPassFilter lowPassFilter;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lowPassFilter = GetComponent<AudioLowPassFilter>();

        // Initialize the dictionary with default values
        collisionSettings = new Dictionary<string, CollisionSettings>();

        foreach (CollisionType type1 in System.Enum.GetValues(typeof(CollisionType)))
        {
            foreach (CollisionType type2 in System.Enum.GetValues(typeof(CollisionType)))
            {
                string key = type1.ToString() + "_" + type2.ToString();
                collisionSettings[key] = new CollisionSettings();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        CollisionAudio otherCollisionAudio = collision.gameObject.GetComponent<CollisionAudio>();

        if (otherCollisionAudio != null)
        {
            string key = objectCollisionType.ToString() + "_" + otherCollisionAudio.objectCollisionType.ToString();

            if (collisionSettings.ContainsKey(key))
            {
                CollisionSettings settings = collisionSettings[key];
                lowPassFilter.cutoffFrequency = settings.cutoffFrequency;

                PlayRandomOneShot();
            }
        }
    }

    void PlayRandomOneShot()
    {
        if (oneShotSounds.Count > 0)
        {
            AudioClip clip = oneShotSounds[Random.Range(0, oneShotSounds.Count)];
            audioSource.PlayOneShot(clip);
        }
    }
}
