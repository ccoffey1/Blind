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

    public void EndGame()
    {
        GameOverScreen.Instance.Show();
        Invoke(nameof(Restart), 5f);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}