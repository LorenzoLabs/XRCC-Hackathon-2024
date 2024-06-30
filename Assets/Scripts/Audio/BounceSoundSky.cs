using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BounceSoundSky : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] soundEffects;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        
        if (collision.relativeVelocity.magnitude > .001)
        {
            if (soundEffects.Length > 0)
            {
                // Select a random sound effect
                int randomIndex = Random.Range(0, soundEffects.Length);
                audioSource.clip = soundEffects[randomIndex];
                audioSource.Play();
            }
        }
    }
}