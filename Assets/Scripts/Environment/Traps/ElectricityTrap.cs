using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityTrap : Trap 
{
    [SerializeField] private AudioClip activatedSound;
    [SerializeField] private bool active;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            var player = other.gameObject.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(activatedSound, player.transform.position);
            player.KillInstantly();
            SoundManager.Instance.SpawnSound(player.transform.position, 50, 4f, 1.0f, 0f, color: Color.red);
            CameraShake.Instance.ShakeCamera(3f, 0.5f);
        }
    }

    public void Enable() {
        active = true;
        audioSource.Play();
    }

    public void Disable() {
        active = false;
        audioSource.Stop();
    }
}
