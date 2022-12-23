using System.Collections;
using UnityEngine;

public class Telephone : MonoBehaviour
{
    public bool isRinging;
    [SerializeField] AudioClip ringingSound;
    [SerializeField] AudioClip pickupSound;
    [SerializeField] AudioClip dialogSound;
    [SerializeField] AudioClip hangupSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isRinging)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = ringingSound;
                audioSource.Play();
                StartCoroutine(SpawnSound(15));
            }
        }
        else 
        {
            if (!audioSource.isPlaying) 
            {
                if (audioSource.clip == pickupSound)
                {
                    audioSource.clip = dialogSound;
                    audioSource.Play();
                }
                else if (audioSource.clip == dialogSound)
                {
                    audioSource.clip = hangupSound;
                    audioSource.Play();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isRinging && collision.gameObject.tag == "Player")
        {
            isRinging = false;
            audioSource.Stop();
            audioSource.clip = pickupSound;
            audioSource.Play();
        }
    }

    private IEnumerator SpawnSound(int count) 
    {
        
        for (int i = 0; i < count; i++)
        {
            if (!isRinging) 
            {
                yield break;
            }
            int soundCount = Random.Range(10, 20);
            Vector2 spawnLocation = new(transform.position.x, transform.position.y);
            SoundManager.Instance.SpawnSound(spawnLocation, soundCount, 3f, 2f, linearDrag: 1f, angleRandomizationFactor: 90f, angleStepRandomizationFactor: 90f);
            yield return new WaitForSeconds(0.12f);
        }
        
    }
}