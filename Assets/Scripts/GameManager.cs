using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private AudioManager audioManager;
    private float volumeFadeDuration = 5f;
    private float volumeRestoreDuration = 1f;

    private void Awake()
    {
        Instance = this;
        audioManager = AudioManager.Instance;
        StartCoroutine(FadeVolume(1.0f, volumeRestoreDuration));
    }

    public void EndGame()
    {
        GameOverScreen.Instance.Show();
        StartCoroutine(FadeOutVolumeAndReloadScene());
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator FadeOutVolumeAndReloadScene() {
        yield return FadeVolume(0f, volumeFadeDuration);
        ReloadScene();
    }

    private IEnumerator FadeVolume(float targetVolume, float duration)
    {
        float startVolume = audioManager.GetVolume();
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float elapsedTime = Time.time - startTime;
            float t = elapsedTime / duration;
            float currentVolume = Mathf.Lerp(startVolume, targetVolume, t);
            audioManager.SetVolume(currentVolume);
            yield return null;
        }

        audioManager.SetVolume(targetVolume); // Ensure volume is set to the target value at the end
    }
}