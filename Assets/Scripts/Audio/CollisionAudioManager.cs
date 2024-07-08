using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]
public class CollisionAudioManager : MonoBehaviour
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

    [SerializeField] private CollisionType _objectCollisionType;
    [SerializeField] private List<AudioClip> _oneShotSounds;
    [SerializeField] private List<Collider> _colliders; // Serialized references to all colliders
    [SerializeField] private Rigidbody _rb; // Serialized reference to the rigidbody
    [SerializeField] private float _sensitivity = 2f; // Sensitivity factor for collision volume
    [SerializeField] private float _collisionPowerMultiplier = 10f; // Multiplier to scale up collision power

    private AudioSource _audioSource;
    private AudioLowPassFilter _lowPassFilter;
    private Dictionary<string, CollisionSettings> _collisionSettings;

    void Start()
    {
        Debug.Log("CollisionAudioManager Start() called.");
        
        _audioSource = GetComponent<AudioSource>();
        _lowPassFilter = GetComponent<AudioLowPassFilter>();

        // Initialize the dictionary with default values
        _collisionSettings = new Dictionary<string, CollisionSettings>();

        foreach (CollisionType type1 in System.Enum.GetValues(typeof(CollisionType)))
        {
            foreach (CollisionType type2 in System.Enum.GetValues(typeof(CollisionType)))
            {
                string key = type1.ToString() + "_" + type2.ToString();
                _collisionSettings[key] = new CollisionSettings();
            }
        }

        // Ensure colliders are properly configured
        foreach (var collider in _colliders)
        {
            if (collider != null)
            {
                var relay = collider.gameObject.AddComponent<CollisionRelay>();
                relay.Initialize(this, _objectCollisionType);
                Debug.Log($"CollisionRelay added to {collider.gameObject.name}");
            }
            else
            {
                Debug.LogWarning("A collider reference in the _colliders list is null.");
            }
        }

        Debug.Log("CollisionAudioManager initialized.");
    }

    public void HandleCollision(Collision collision, CollisionType otherCollisionType)
    {
        Debug.Log($"Handling collision with {collision.gameObject.name}, collision type: {otherCollisionType}");

        string key = _objectCollisionType.ToString() + "_" + otherCollisionType.ToString();
        if (_collisionSettings.ContainsKey(key))
        {
            CollisionSettings settings = _collisionSettings[key];
            _lowPassFilter.cutoffFrequency = settings.cutoffFrequency;

            float collisionPower = collision.relativeVelocity.magnitude * _collisionPowerMultiplier;
            float volume = Mathf.Clamp(collisionPower / _sensitivity, 0.1f, 1f); // Adjust the sensitivity factor

            Debug.Log($"Collision detected with {collision.gameObject.name}, power: {collisionPower}, calculated volume: {volume}");
            PlayRandomOneShot(volume);
        }
        else
        {
            Debug.LogWarning($"Collision type key {key} not found in collision settings.");
        }
    }

    private void PlayRandomOneShot(float volume)
    {
        if (_oneShotSounds.Count > 0)
        {
            AudioClip clip = _oneShotSounds[Random.Range(0, _oneShotSounds.Count)];
            Debug.Log($"Attempting to play audio clip: {clip.name} at volume: {volume}");
            _audioSource.PlayOneShot(clip, volume);
            Debug.Log($"Playing audio clip: {clip.name} at volume: {volume}");
        }
        else
        {
            Debug.LogWarning("No audio clips available in oneShotSounds list.");
        }
    }
}
