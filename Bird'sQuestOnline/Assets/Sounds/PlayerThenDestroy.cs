using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThenDestroy : MonoBehaviour
{
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
        transform.parent = null;
    }

    public void PlaySound()
    {
        source.pitch = (Random.value - 0.5f) / 3 + 1;
        source.Play();

        Destroy(gameObject, 2);
    }
}
