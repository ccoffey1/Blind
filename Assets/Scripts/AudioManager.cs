using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private List<AudioSource> audioSources;
    private GameObject player;

    void Start()
    {
        Instance = this;
        audioSources = FindObjectsOfType<AudioSource>().Where(x => !x.CompareTag("Background Audio")).ToList();
        player = GameObject.FindWithTag("Player");
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.gameObject.TryGetComponent(out AudioLowPassFilter audioLowPassFilter);
            if (audioLowPassFilter == null)
            {
                audioLowPassFilter = audioSource.gameObject.AddComponent<AudioLowPassFilter>();
            }
            audioLowPassFilter.cutoffFrequency = 500f;
            audioLowPassFilter.enabled = false;
        }
    }

    void FixedUpdate()
    {
        // TODO: bit of a memory leak issue here -- any audiosources that were removed
        // will be null here, but we'll still iterate over them unnecessarily
        foreach (AudioSource audioSource in audioSources.Where(x => x != null))
        {
            AudioLowPassFilter filter = audioSource.gameObject.GetComponent<AudioLowPassFilter>();
            LayerMask layerMask = ~LayerMask.GetMask("SoundBarrier");
            Vector3 directionToPlayer = player.gameObject.transform.position - audioSource.transform.position;
            Debug.DrawRay(audioSource.transform.position, directionToPlayer);
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                filter.enabled = true;
            }
            else
            {
                filter.enabled = false;
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
