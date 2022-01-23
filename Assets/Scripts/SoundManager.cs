using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource, bgm;
    public void playSound(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }
}
