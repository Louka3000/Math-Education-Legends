using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void playSound(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }
}
