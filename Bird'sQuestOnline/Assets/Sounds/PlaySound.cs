using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] private AudioClip[] clip = new AudioClip[0];

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play(int pIndex = 0)
    {
        if (clip.Length > 0)
            source.clip = clip[pIndex];
        source.pitch = (Random.value - 0.5f) / 3 + 1;
        source.Play();
    }
}
