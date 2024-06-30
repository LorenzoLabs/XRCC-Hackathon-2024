using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class AdvancedCollisionAudio : MonoBehaviour
{
    public List<AudioClip> oneShotSounds;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        float collisionPower = collision.relativeVelocity.magnitude;
        float volume = Mathf.Clamp(collisionPower / 10f, 0.1f, 1f); // Adjust the divisor based on desired sensitivity

        PlayRandomOneShot(volume);

        Debug.Log($"Collision detected with {collision.gameObject.name}, power: {collisionPower}, volume: {volume}");
    }

    void PlayRandomOneShot(float volume)
    {
        if (oneShotSounds.Count > 0)
        {
            AudioClip clip = oneShotSounds[Random.Range(0, oneShotSounds.Count)];
            audioSource.PlayOneShot(clip, volume);
            Debug.Log($"Playing audio clip: {clip.name} at volume: {volume}");
        }
        else
        {
            Debug.LogWarning("No audio clips available in oneShotSounds list.");
        }
    }
}