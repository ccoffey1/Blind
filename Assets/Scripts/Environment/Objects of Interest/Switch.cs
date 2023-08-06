using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : ObjectOfInterest
{
    [SerializeField] private List<GameObject> objectsToControl;
    [SerializeField] private AudioClip activationClip; 
    [SerializeField] private AudioClip deactivationClip;
    [SerializeField] private bool disableOnActivation;
    [SerializeField] private bool active = true;
    [SerializeField] private bool on;

    private void OnTriggerEnter2D(Collider2D other) {
        if (active) {
            if (other.CompareTag("Player")) {
                if (on) {
                    on = false;
                    AudioSource.PlayClipAtPoint(deactivationClip, transform.position);
                } else {
                    on = true;
                    AudioSource.PlayClipAtPoint(activationClip, transform.position);
                }
                foreach (GameObject gameObject in objectsToControl) {
                    gameObject.SetActive(!gameObject.activeInHierarchy);
                }
                if (disableOnActivation) {
                    active = false;
                    SoundBulletPassoverColor = Color.gray;
                }
            }
        }
    }
}
