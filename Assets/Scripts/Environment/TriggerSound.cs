using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip audioClip;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private bool selfDestructAfterPlay;

    private bool soundCanPlay = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
    }

    private void Update()
    {
        if (selfDestructAfterPlay && audioSource.time >= audioClip.length)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && soundCanPlay)
        {
            audioSource.Play();
            StartCoroutine(BeginSoundCoolDown());
        }
    }

    private IEnumerator BeginSoundCoolDown()
    {
        soundCanPlay = false;
        yield return new WaitForSeconds(1f);
        soundCanPlay = true;
    }
}
