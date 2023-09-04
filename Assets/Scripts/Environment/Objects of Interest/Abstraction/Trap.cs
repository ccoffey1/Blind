using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : ObjectOfInterest
{
    [SerializeField] private bool projectSoundBullets;
    [SerializeField] private float soundBulletPulseDelay = 0.5f;
    // [SerializeField] private SoundBulletConfig additionalConfig;

    private float timer;

    void Update() {
        timer += Time.deltaTime;
        if (projectSoundBullets && timer >= soundBulletPulseDelay) {
            // TODO: Switch to particle system
            // SoundManager.Instance.SpawnSound(transform.position, additionalConfig, spawnedBy: gameObject);
            timer = 0f;
        }
    }
}