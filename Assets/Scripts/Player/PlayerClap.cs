using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClap : MonoBehaviour
{
    public bool Enabled;

    [SerializeField]
    private int soundProjectiles = 100;

    [SerializeField]
    private float soundSpeed = 4f;

    [SerializeField]
    private float fadeSpeed = 0.5f;

    [SerializeField]
    private float linearDrag = 0f;

    [SerializeField]
    private AudioClip clapSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (enabled && Input.GetKeyUp(KeyCode.Space))
        {
            audioSource.PlayOneShot(clapSound);
            SoundManager.Instance.SpawnSound(transform.position, soundProjectiles, soundSpeed, fadeSpeed, linearDrag, spawnedBy: gameObject);
        }
    }
}
