using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip sound;

    [SerializeField]
    private float delayBetweenPlays = 2.0f;

    [SerializeField]
    private float randomizeSourceAmount = 0.001f;

    private AudioSource audioSource;
    private Vector2 originalPosition;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Start()
    {
        InvokeRepeating(nameof(PlaySound), delayBetweenPlays, delayBetweenPlays);
    }

    private void PlaySound()
    {
        float x = originalPosition.x;
        float y = originalPosition.y;
        float xRand = Random.Range(x - randomizeSourceAmount, x - randomizeSourceAmount);
        float yRand = Random.Range(y - randomizeSourceAmount, y + randomizeSourceAmount);
        Vector2 newSource = new Vector2(xRand, yRand);
        audioSource.transform.position = newSource;
        audioSource.pitch = Random.Range(1f, 1.2f);
        audioSource.PlayOneShot(sound, 0.1f);
        SoundManager.Instance.SpawnSound(newSource, 15, 2f, 2f, 4f);
    }
}
