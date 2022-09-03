using System.Collections;
using UnityEngine;

public class Shambler : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1.0f;

    [SerializeField]
    private float soundEmitOnWalkPeriod = 5f;

    [HideInInspector]
    public bool IsWalking;

    [SerializeField]
    private AudioClip shamblerMoanAudio;

    private Vector2? chaseTargetLocation;
    private float lastReceivedBulletAtTime = 0f;
    private Rigidbody2D rb;
    private AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating(nameof(GenerateNoise), soundEmitOnWalkPeriod, soundEmitOnWalkPeriod);
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    private void FixedUpdate()
    {
        if (chaseTargetLocation != null)
        {
            float distanceToSound = Vector2.Distance(transform.position, chaseTargetLocation.Value);
            if (distanceToSound < 0.1f)
            {
                // Bullet spawn location reached.
                IsWalking = false;
                chaseTargetLocation = null;
                rb.velocity = Vector2.zero; // force to sit still 
                StartCoroutine(FadeOutAudio());
            }
            else
            {
                IsWalking = true;
                transform.position = Vector2.MoveTowards(transform.position, chaseTargetLocation.Value, moveSpeed * Time.deltaTime);
                if (!audioSource.isPlaying)
                {
                    var delayTime = Random.Range(0f, audioSource.clip.length);
                    audioSource.time = delayTime;
                    audioSource.Play();
                }
            }
        }
    }

    private IEnumerator FadeOutAudio()
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / 0.05f;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    private void GenerateNoise()
    {
        if (IsWalking)
        {
            SoundManager.Instance.SpawnSound(transform.position, 10, 0.5f, 1f, color: Color.red, spawnedBy: gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out SoundBullet soundBullet) && 
            (soundBullet.SpawnedBy?.CompareTag("Player") ?? false))
        {
            OnCollisionWithSoundBullet(soundBullet);
        }
    }

    private void OnCollisionWithSoundBullet(SoundBullet soundBullet)
    {
        if (soundBullet.SpawnedBy != gameObject && soundBullet.SpawnTime > lastReceivedBulletAtTime)
        {
            lastReceivedBulletAtTime = soundBullet.SpawnTime;
            chaseTargetLocation = soundBullet.SpawnOrigin;
        }
    }
}
