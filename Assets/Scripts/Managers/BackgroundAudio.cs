using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    public static BackgroundAudio Instance;

    [SerializeField]
    private bool paused;

    [SerializeField]
    private AudioClip backgroundMusic;

    [SerializeField]
    private AudioClip backgroundAmbience;

    private AudioSource backgroundMusicAudioSource;
    private AudioSource backgroundAmbienceAudioSource;

    private void Start()
    {
        Instance = this;
    }

    private void Awake()
    {
        backgroundMusicAudioSource = transform.Find("Background Music").GetComponent<AudioSource>();
        backgroundAmbienceAudioSource = transform.Find("Background Ambience").GetComponent<AudioSource>();

        backgroundMusicAudioSource.loop = true;
        backgroundAmbienceAudioSource.loop = true;

        backgroundMusicAudioSource.Play();

        ChangeBackgroundMusic(backgroundMusic);
        ChangeBackgroundAmbience(backgroundAmbience);
    }

    private void Update()
    {
        if (paused && backgroundMusicAudioSource.isPlaying && backgroundAmbienceAudioSource.isPlaying)
        {
            backgroundMusicAudioSource.Pause();
            backgroundAmbienceAudioSource.Pause();
        }

        if (!paused && !backgroundMusicAudioSource.isPlaying && !backgroundAmbienceAudioSource.isPlaying)
        {
            backgroundMusicAudioSource.Play();
            backgroundAmbienceAudioSource.Play();
        }
    }

    public void ChangeBackgroundMusic(AudioClip newMusic)
    {
        backgroundMusicAudioSource.Pause();
        backgroundMusic = newMusic;
        backgroundMusicAudioSource.clip = backgroundMusic;
        backgroundMusicAudioSource.time = 0;
        backgroundMusicAudioSource.Play();
    }

    public void ChangeBackgroundAmbience(AudioClip newAmbience)
    {
        backgroundAmbienceAudioSource.Pause();
        backgroundAmbience = newAmbience;
        backgroundAmbienceAudioSource.clip = backgroundAmbience;
        backgroundAmbienceAudioSource.time = 0;
        backgroundAmbienceAudioSource.Play();
    }

    public string GetCurrentPlayingBackgroundMusic()
    {
        return backgroundMusicAudioSource.clip.name;
    }
}
