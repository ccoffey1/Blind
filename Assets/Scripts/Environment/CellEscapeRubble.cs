using System.Collections;
using Unity.Burst;
using UnityEngine;

public class CellEscapeRubble : MonoBehaviour
{
    [SerializeField]
    private AudioClip rubbleAudio;

    [SerializeField]
    private AudioClip destructionAudio;

    private AudioSource audioSource;
    private ParticleSystem particleSystem;

    private bool playerOverlapped;
    private bool audioPausing;
    private bool selfDestructing;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponentInChildren<AudioSource>();
        audioSource.clip = rubbleAudio;
        audioSource.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerOverlapped && Input.GetKey(KeyCode.A)) // Player is pushing into the object
        {
            if (!audioSource.isPlaying)
            {
                audioSource.volume = 1f;
                audioSource.time = 0;
                audioSource.Play();
                particleSystem.Play();
                StartCoroutine(RandomizeParticleSystemLocation());
            } 
        }
        else if (!selfDestructing && audioSource.isPlaying && !audioPausing && audioSource.volume > 0)
        {
            particleSystem.Stop();
            StartCoroutine(FadeOutAudio(0.5f, 0f));
        }

        // Audio is finished! Destroy the wall
        if (!selfDestructing && audioSource.time >= audioSource.clip.length - 0.3)
        {
            particleSystem.Stop();
            StartCoroutine(SelfDestruct());
        }
    }

    private IEnumerator SelfDestruct()
    {
        selfDestructing = true;
        audioSource.volume = 1f;
        audioSource.clip = destructionAudio;
        audioSource.time = 0;
        audioSource.Play();

        // Allow the user to pass through
        Destroy(GetComponent<PolygonCollider2D>());
        Destroy(GetComponent<SpriteRenderer>());

        var mainModule = particleSystem.main;
        var originalStartLifetime = mainModule.startLifetime;
        var originalSpeed = mainModule.startSpeed;
        mainModule.startLifetime = originalStartLifetime.constant * 5f;
        mainModule.startSpeed = originalSpeed.constant * 2f;
        mainModule.loop = false;
        ParticleSystem.EmissionModule em = particleSystem.emission;
        var burst = em.GetBurst(0);
        burst.count = burst.count.constant * 2;
        em.SetBurst(0, burst);

        CameraShake.Instance.ShakeCamera(3f, 1f);
        particleSystem.Play();

        yield return new WaitForSeconds(0.1f);

        mainModule.startLifetime = originalStartLifetime.constant * 1.2f;

        float delayBetweenSounds = 0.3f;
        int soundPulseCount = 7;
        for (int i = 0; i < soundPulseCount; i++)
        {
            float randX = Random.Range(-0.3f, 0.3f);
            float randY = Random.Range(-0.3f, 0.3f);
            int soundCount = Random.Range(15, 30);
            
            burst = em.GetBurst(0);
            burst.count = soundCount;
            em.SetBurst(0, burst);
        
            Vector2 spawnLocation = new(transform.position.x + randX, transform.position.y + randY);
            particleSystem.transform.position = spawnLocation;
            particleSystem.Play();
            yield return new WaitForSeconds(delayBetweenSounds);
        }
        particleSystem.Stop();

        yield return new WaitForSeconds(destructionAudio.length - (soundPulseCount * delayBetweenSounds)); // clip size minus the delays above

        // Finally, destroy the object entirely.
        Destroy(gameObject);
    }

    private IEnumerator RandomizeParticleSystemLocation()
    {
        while (audioSource.isPlaying && !selfDestructing)
        {
            float randX = Random.Range(-0.3f, 0.3f);
            float randY = Random.Range(-0.3f, 0.3f);
            Vector2 spawnLocation = new Vector2(transform.position.x + randX, transform.position.y + randY);
            particleSystem.transform.position = spawnLocation;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator FadeOutAudio(float duration, float targetVolume)
    {
        audioPausing = true;
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            // If we were fading out while self destructing, just abort
            if (selfDestructing)
            {
                audioSource.volume = 1f;
                yield break;
            }

            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        audioSource.Pause();
        audioPausing = false;
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOverlapped = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOverlapped = false;
        }
    }
}
