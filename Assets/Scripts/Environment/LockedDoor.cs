using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField]
    private AudioClip lockedSound;

    private AudioSource audioSource;
    private bool soundCooldown;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = lockedSound;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !soundCooldown)
        {
            audioSource.Play();
            StartCoroutine(BeginSoundCoolDown());
        }
    }

    private IEnumerator BeginSoundCoolDown()
    {
        soundCooldown = true;
        yield return new WaitForSeconds(1f);
        soundCooldown = false;
    }
}
