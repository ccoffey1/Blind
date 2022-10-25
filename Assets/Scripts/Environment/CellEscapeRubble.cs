using System.Collections;
using UnityEngine;

public class CellEscapeRubble : MonoBehaviour
{
    [SerializeField]
    private AudioClip rubbleAudio;

    [SerializeField]
    private AudioClip destructionAudio;

    private AudioSource audioSource;

    private bool playerOverlapped;
    private bool audioPausing;
    private bool selfDestructing;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
                StartCoroutine(SpawnSoundBullets());
            }
        }
        else if (!selfDestructing && audioSource.isPlaying && !audioPausing && audioSource.volume > 0)
        {
            StartCoroutine(FadeOutAudio(0.5f, 0f));
        }

        // Audio is finished! Destroy the wall
        if (!selfDestructing && audioSource.time >= audioSource.clip.length - 0.3)
        {
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

        CameraShake.Instance.ShakeCamera(3f, 1f);
        SoundManager.Instance.SpawnSound(transform.position, 30, 3f, 1f, linearDrag: 0f);

        yield return new WaitForSeconds(0.1f);

        float delayBetweenSounds = 0.3f;
        int soundPulseCount = 9;
        for (int i = 0; i < soundPulseCount; i++)
        {
            // TODO: Add some kinda 'small-sound' function to the sound manager to avoid this smell
            float randX = Random.Range(-0.3f, 0.3f);
            float randY = Random.Range(-0.3f, 0.3f);
            int soundCount = Random.Range(15, 30);
            Vector2 spawnLocation = new(transform.position.x + randX, transform.position.y + randY);
            SoundManager.Instance.SpawnSound(spawnLocation, soundCount, 3f, 2f, linearDrag: 1f);

            yield return new WaitForSeconds(delayBetweenSounds);
        }

        yield return new WaitForSeconds(destructionAudio.length - (soundPulseCount * delayBetweenSounds)); // clip size minus the delays above

        // Finally, destroy the object entirely.
        Destroy(gameObject);
    }

    private IEnumerator SpawnSoundBullets()
    {
        while (audioSource.isPlaying && !selfDestructing)
        {
            float randX = Random.Range(-0.3f, 0.3f);
            float randY = Random.Range(-0.3f, 0.3f);
            Vector2 spawnLocation = new Vector2(transform.position.x + randX, transform.position.y + randY);
            SoundManager.Instance.SpawnSound(spawnLocation, 5, 1f, 3f);
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
