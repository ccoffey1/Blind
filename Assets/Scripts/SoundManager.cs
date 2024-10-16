using System;
using UnityEngine;
using UnityEngine.Pool;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private SoundBullet soundPrefab;

    [SerializeField]
    private int initialPoolSize;

    private ObjectPool<SoundBullet> pool;

    private const float Radius = 1f;

    private void Awake()
    {
        Instance = this;
        pool = new ObjectPool<SoundBullet>(CreateSound, OnGetSoundBulletFromPool, OnReturnSoundBulletToPool);
        for (int i = 0; i < initialPoolSize; i++)
        {
            SoundBullet soundBullet = CreateSound();
            soundBullet.gameObject.SetActive(false);
        }
    }

    private SoundBullet CreateSound()
    {
        var sound = Instantiate(soundPrefab);
        sound.Pool = pool;
        return sound;
    }

    public void SpawnSound(
        Vector2 soundOrigin,
        SoundBulletConfig config,
        GameObject spawnedBy = null)
    {
        SpawnSound(
            soundOrigin, 
            config.numberOfProjectiles, 
            config.speed, 
            config.fadeTime, 
            config.linearDrag, 
            config.positionRandomizationAmount,
            config.color, 
            spawnedBy
        );
    }

    public void SpawnSound(
        Vector2 soundOrigin,
        int numberOfProjectiles,
        float speed,
        float fadeTime,
        float linearDrag = 2.0f,
        float positionRandomizationAmount = 0f,
        Color? color = null,
        GameObject spawnedBy = null)
    {
        float angle = 0f;
        float angleStep = 360f / numberOfProjectiles;

        // Randomization to the location.
        float randomizedX = soundOrigin.x + UnityEngine.Random.Range(-positionRandomizationAmount, positionRandomizationAmount);
        float randomizedY = soundOrigin.y + UnityEngine.Random.Range(-positionRandomizationAmount, positionRandomizationAmount);
        Vector2 newOrigin = new Vector2(randomizedX, randomizedY);

        for (int i = 0; i <= numberOfProjectiles - 1; i++)
        {
            // Direction calculations.
            float projectileDirXPosition = newOrigin.x + Mathf.Sin((angle * Mathf.PI) / 180) * Radius;
            float projectileDirYPosition = newOrigin.y + Mathf.Cos((angle * Mathf.PI) / 180) * Radius;

            // Create vectors.
            Vector2 projectileVector = new(projectileDirXPosition, projectileDirYPosition);
            Vector2 projectileMoveDirection = (projectileVector - newOrigin).normalized * speed;

            pool.Get().Spawn(newOrigin,
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


[Serializable]
public class SoundBulletConfig
{
    public int numberOfProjectiles;
    public float speed;
    public float fadeTime;
    public float linearDrag = 2.0f;
    public Color color = Color.white;
    public float positionRandomizationAmount;
}