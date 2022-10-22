using UnityEngine;

public class ObjectOfInterest : MonoBehaviour
{
    public ObjectData ObjectData;

    private AudioSource objectAudioSource;

    private void Awake()
    {
        objectAudioSource = GetComponent<AudioSource>();
    }

    public void TryPlayPlayerCollisionSound()
    {
        // TODO: Make random
        if (ObjectData.SoundsOnPlayerCollision.Length > 0)
        {
            objectAudioSource.PlayOneShot(ObjectData.SoundsOnPlayerCollision[0]);
        }
    }

    public void TryPlaySoundBulletCollisionSound()
    {
        if (ObjectData.SoundsOnSoundBulletCollision.Length > 0)
        {
            // TODO: make random
            objectAudioSource.PlayOneShot(ObjectData.SoundsOnSoundBulletCollision[0]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TryPlayPlayerCollisionSound();
        }
    }
}
