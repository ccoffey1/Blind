using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{

    private void Awake()
    {
        // set start randomly so it doesn't sound repetitive
        var audioSource = GetComponent<AudioSource>();
        float start = Random.Range(0.0f, audioSource.clip.length);
        audioSource.time = start;
        audioSource.Play();
    }
}
