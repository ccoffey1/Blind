using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClap : MonoBehaviour
{
    public bool Enabled;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip quietClap;


    [SerializeField]
    private AudioClip mediumClap;

    [SerializeField]
    private AudioClip loudClap;

    [SerializeField]
    private ParticleSystem clapParticleSystem;

    // Defaults
    private const float MIN_LIFETIME = 1.0f;
    private const float MAX_LIFETIME = 3.0f;

    private const float MIN_SPEED = 3.5f;
    private const float MAX_SPEED = 7.0f;

    private float timer = 0.0f;
    private float duration = 1.0f;
    private float startLifeTime = MIN_LIFETIME;
    private float startSpeed = MIN_SPEED;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            timer += Time.deltaTime;

            startLifeTime = Mathf.Lerp(MIN_LIFETIME, MAX_LIFETIME, timer / duration);
            startLifeTime = Mathf.Clamp(startLifeTime, MIN_LIFETIME, MAX_LIFETIME);

            startSpeed = Mathf.Lerp(MIN_SPEED, MAX_SPEED, timer / duration);
            startSpeed = Mathf.Clamp(startSpeed, MIN_SPEED, MAX_SPEED);
        }

        if (enabled && Input.GetKeyUp(KeyCode.Space))
        {
            if (timer <= 0.5) {
                audioSource.clip = quietClap;
            }
            if (timer > 0.5) {
                audioSource.clip = mediumClap;
            }
            if (timer >= 0.8) {
                audioSource.clip = loudClap;
            }
            audioSource.Play();
            var main = clapParticleSystem.main;
            main.startLifetime = startLifeTime;
            main.startSpeed = startSpeed;
            clapParticleSystem.Play();
            timer = 0f;
        }
    }
}
