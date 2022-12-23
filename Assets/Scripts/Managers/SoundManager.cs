using UnityEngine;
using UnityEngine.Pool;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private SoundBullet soundPrefab;

    private ObjectPool<SoundBullet> pool;

    private const float Radius = 1f;

    private void Awake()
    {
        Instance = this;
        pool = new ObjectPool<SoundBullet>(CreateSound, OnGetSoundBulletFromPool, OnReturnSoundBulletToPool);
    }

    private SoundBullet CreateSound()
    {
        var sound = Instantiate(soundPrefab);
        sound.Pool = pool;
        return sound;
    }

    public void SpawnSound(
        Vector2 soundOrigin, 
        int numberOfProjectiles, 
        float speed, 
        float fadeTime, 
        float linearDrag = 2.0f,
        Color? color = null,
        GameObject spawnedBy = null,
        float angleRandomizationFactor = 0f,
        float angleStepRandomizationFactor = 0f)
    {
        float angle = Random.Range(-angleRandomizationFactor, angleRandomizationFactor);
        float angleStep = 360f / numberOfProjectiles;
        for (int i = 0; i <= numberOfProjectiles - 1; i++)
        {
            float randomizedAngle = angle + Random.Range(-angleStepRandomizationFactor, angleStepRandomizationFactor);

            // Direction calculations.
            float projectileDirXPosition = soundOrigin.x + Mathf.Sin((randomizedAngle * Mathf.PI) / 180) * Radius;
            float projectileDirYPosition = soundOrigin.y + Mathf.Cos((randomizedAngle * Mathf.PI) / 180) * Radius;

            // Create vectors.
            Vector2 projectileVector = new(projectileDirXPosition, projectileDirYPosition);
            Vector2 projectileMoveDirection = (projectileVector - soundOrigin).normalized * speed;

            pool.Get().Spawn(soundOrigin,
                             projectileMoveDirection,
                             fadeTime,
                             linearDrag,
                             color,
                             spawnedBy);

            angle += angleStep;
        }
    }

    private void OnGetSoundBulletFromPool(SoundBullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReturnSoundBulletToPool(SoundBullet bullet)
    {
        bullet.ClearTrails();
        bullet.gameObject.SetActive(false);
    }
}