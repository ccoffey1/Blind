using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityTrap : Activatable 
{
    [SerializeField] private AudioClip activatedSound;
    [SerializeField] private bool active;

    private AudioSource audioSource;
    private ParticleSystem particles;

    void Start()
    { 
        audioSource = GetComponent<AudioSource>();
        particles = GetComponent<ParticleSystem>();
        Deactivate();
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (active && other.gameObject.CompareTag("Player")) {
            var player = other.gameObject.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(activatedSound, player.transform.position);
            player.KillInstantly();
            // SoundManager.Instance.SpawnSound(player.transform.position, 50, 4f, 1.0f, 0f, color: Color.red);
            CameraShake.Instance.ShakeCamera(3f, 0.5f);
        }
    }

    public override void Toggle()
    {
        if (active) {
            Deactivate();
        } else {
            Activate();
        }
    }

    public override void Activate() {
        active = true;
        audioSource.Play();
        particles.Play();
    }

    public override void Deactivate() {
        active = false;
        audioSource.Stop();
        particles.Stop();
    }
}
