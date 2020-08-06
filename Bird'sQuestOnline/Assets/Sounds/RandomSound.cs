using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] private AudioClip[] clips = new AudioClip[0];

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        source.pitch = (Random.value - 0.5f) / 3 + 1;
        source.clip = clips[Random.Range(0, clips.Length)];
        source.Play();
    }
}
