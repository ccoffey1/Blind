using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private float lowPassCutoffFrequency = 500f;
    private IEnumerable<AudioSource> audioSources;
    private GameObject player;

    void Awake()
    {
        Instance = this;
    }

    void Start() 
    {
        audioSources = FindObjectsOfType<AudioSource>(includeInactive: true).Where(x => !x.CompareTag("Background Audio"));
        player = GameObject.FindWithTag("Player");
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.gameObject.TryGetComponent(out AudioLowPassFilter audioLowPassFilter);
            if (audioLowPassFilter == null)
            {
                audioLowPassFilter = audioSource.gameObject.AddComponent<AudioLowPassFilter>();
            }
            audioLowPassFilter.cutoffFrequency = lowPassCutoffFrequency;
            audioLowPassFilter.enabled = false;
        }
    }

    void Update()
    {
        // TODO: bit of a memory leak issue here -- any audiosources that were removed
        // will be null here, but we'll still iterate over them unnecessarily
        foreach (AudioSource audioSource in audioSources.Where(x => x != null))
        {
            AudioLowPassFilter filter = audioSource.gameObject.GetComponent<AudioLowPassFilter>();
            Vector3 directionToPlayer = player.transform.position - audioSource.transform.position;
            LayerMask layerMask = LayerMask.GetMask("SoundBarrier", "Player");
            RaycastHit2D hit = Physics2D.Raycast(audioSource.transform.position, directionToPlayer, Mathf.Infinity, layerMask);
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                Debug.DrawRay(audioSource.transform.position, directionToPlayer, Color.green);
                filter.enabled = false;
            }
            else
            {
                filter.enabled = true;
                Debug.DrawRay(audioSource.transform.position, directionToPlayer, Color.red);
            }
        }
    }

    public float GetVolume()
    {
        return AudioListener.volume;
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
