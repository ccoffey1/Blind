using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private AudioClip levelAmbience;

    [SerializeField]
    private AudioClip playerDeathSound;

    private Player playerData;

    private void Awake()
    {
        Instance = this;
        playerData = FindObjectOfType<Player>();
    }

    public void PlayerDied(Vector2 deathLocation)
    {
        playerData.GetComponent<Rigidbody2D>().simulated = false;
        AudioSource.PlayClipAtPoint(playerDeathSound, playerData.transform.position);
        StartCoroutine(SpawnPlayerScreamAudioBullets(deathLocation));
        EndGame();
    }

    private IEnumerator SpawnPlayerScreamAudioBullets(Vector2 deathLocation)
    {
        yield return new WaitForSeconds(0.9f);
        SoundManager.Instance.SpawnSound(deathLocation, 150, 4f, 0.3f, 0f, Color.red);
        CameraShake.Instance.ShakeCamera(5f, 2f);
    }
    
    private void EndGame()
    {
        GameOverScreen.Instance.Show();
        Destroy(playerData.gameObject);
        Invoke(nameof(Restart), 5f);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}