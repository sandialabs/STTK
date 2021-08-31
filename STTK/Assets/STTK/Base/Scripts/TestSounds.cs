using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSounds : MonoBehaviour
{
    public List<AudioClip> clips;
    public AudioSource audioSource;
    void Start()
    {
        audioSource = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }
    public void PlayAudio(int index)
    {
        if (clips[index]) 
        {
            audioSource.clip = clips[index];
            audioSource.Play();
        }
    }
}
