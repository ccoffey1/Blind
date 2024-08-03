using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private List<Activatable> activatablesToToggle;
    [SerializeField] private List<GameObject> gameObjectsToToggle;
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
                foreach (GameObject gameObject in gameObjectsToToggle) {
                    gameObject.SetActive(!gameObject.activeInHierarchy);
                }
                foreach (Activatable activatable in activatablesToToggle) {
                    activatable.Toggle();
                }
                if (disableOnActivation) {
                    active = false;
                }
            }
        }
    }
}
