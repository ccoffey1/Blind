using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private AudioClip playerDeathSound;

    [SerializeField]
    private int soundProjectilesOnDeath = 100;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Harmful")
        {
            PlayerDied();
            GameManager.Instance.EndGame();
        }
    }

    private void PlayerDied()
    {
        GetComponent<Rigidbody2D>().simulated = false;
        AudioSource.PlayClipAtPoint(playerDeathSound, transform.position);
        Invoke(nameof(SpawnPlayerScreamAudioBullets), 0.9f);
    }

    private void SpawnPlayerScreamAudioBullets()
    {
        SoundManager.Instance.SpawnSound(transform.position, soundProjectilesOnDeath, 4f, 0.3f, 0f, Color.red);
        CameraShake.Instance.ShakeCamera(5f, 2f);
    }
}
