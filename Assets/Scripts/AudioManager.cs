using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    void Awake() {
        Instance = this;
    }

    public float GetVolume() {
        return AudioListener.volume;
    }

    public void SetVolume(float volume) {
        AudioListener.volume = volume;
    }
}
