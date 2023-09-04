using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private AudioClip playerDeathSound;

    [SerializeField]
    private int soundProjectilesOnDeath = 100;

    public void KillInstantly() {
        GetComponent<Rigidbody2D>().simulated = false;
        GameManager.Instance.EndGame();
    }

    public void KillViolent() {
        // TODO: Might not want to handle this here?
        GetComponent<Rigidbody2D>().simulated = false;
        AudioSource.PlayClipAtPoint(playerDeathSound, transform.position);
        Invoke(nameof(SpawnPlayerScreamAudioBullets), 0.9f);
        GameManager.Instance.EndGame();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Harmful NPC")
        {
            KillViolent();
        }
    }

    private void SpawnPlayerScreamAudioBullets()
    {
        // TODO: Switch to particle system
        // SoundManager.Instance.SpawnSound(transform.position, soundProjectilesOnDeath, 4f, 0.3f, 0f, color: Color.red);
        CameraShake.Instance.ShakeCamera(5f, 2f);
    }
}
