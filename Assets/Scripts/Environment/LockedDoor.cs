using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField]
    private AudioClip lockedSound;

    private bool soundCooldown;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !soundCooldown)
        {
            AudioSource.PlayClipAtPoint(lockedSound, transform.position, 1f);
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
