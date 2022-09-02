using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private int soundPoolInitialSize = 500;

    [SerializeField]
    private int soundPoolGrowthSize = 50;

    [SerializeField]
    private GameObject soundPrefab;

    private Queue<GameObject> availableSoundProjectiles;
    private LinkedList<SoundProjectile> activeSounds;

    private float radius = 1f;

    private void Awake()
    {
        Instance = this;
        availableSoundProjectiles = new Queue<GameObject>();
        activeSounds = new LinkedList<SoundProjectile>();
        growPool(soundPoolInitialSize);
    }

    private void Update()
    {
        FadeOutActiveSounds();
    }

    public void SpawnSound(
        Vector2 soundOrigin, 
        int numberOfProjectiles, 
        float speed, 
        float fadeSpeed, 
        float linearDrag = 2.0f,
        Color? color = null,
        GameObject spawnedBy = null)
    {
        float angle = 0f;
        float angleStep = 360f / numberOfProjectiles;
        for (int i = 0; i <= numberOfProjectiles - 1; i++)
        {
            // Direction calculations.
            float projectileDirXPosition = soundOrigin.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            float projectileDirYPosition = soundOrigin.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

            // Create vectors.
            Vector2 projectileVector = new Vector2(projectileDirXPosition, projectileDirYPosition);
            Vector2 projectileMoveDirection = (projectileVector - soundOrigin).normalized * speed;

            // Create/use game objects from pool.
            if (availableSoundProjectiles.Count == 0)
            {
                growPool(soundPoolGrowthSize);
            }

            GameObject soundBulletGameObject = availableSoundProjectiles.Dequeue();

            soundBulletGameObject.transform.position = soundOrigin;

            TrailRenderer trailRenderer = soundBulletGameObject.GetComponent<TrailRenderer>();
            trailRenderer.material.color = color ?? Color.white;

            // Clear trail to prevent showing trail from last spawn point.
            trailRenderer.Clear();

            soundBulletGameObject.SetActive(true);

            Rigidbody2D soundRb = soundBulletGameObject.GetComponent<Rigidbody2D>();
            soundRb.velocity = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);
            soundRb.drag = linearDrag;

            SoundBullet soundBulletData = soundBulletGameObject.GetComponent<SoundBullet>();
            soundBulletData.SpawnedFrom = soundOrigin;
            soundBulletData.SpawnedBy = spawnedBy;
            soundBulletData.SpawnTime = Time.timeSinceLevelLoad;

            Renderer renderer = soundBulletGameObject.GetComponent<Renderer>();
            renderer.material.color = color ?? Color.white;
            activeSounds.AddLast(new SoundProjectile 
            { 
                soundObject = soundBulletGameObject, 
                fadeSpeed = fadeSpeed,
                initialRendererColor = renderer.material.color,
                initialTrailColor = trailRenderer.material.color
            });

            angle += angleStep;
        }
    }

    private void growPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject soundObject = Instantiate(soundPrefab);
            soundObject.SetActive(false);
            availableSoundProjectiles.Enqueue(soundObject);
        }
    }

    private void FadeOutActiveSounds()
    {
        LinkedListNode<SoundProjectile> node = activeSounds.First;

        while (node != null)
        {
            var next = node.Next;
            var activeSound = node.Value;

            // Fade out projectile.
            Renderer renderer = activeSound.soundObject.GetComponent<Renderer>();
            Color color = Color.Lerp(renderer.material.color, Color.clear, activeSound.fadeSpeed * Time.deltaTime);
            renderer.material.color = color;

            TrailRenderer trailRenderer = activeSound.soundObject.GetComponent<TrailRenderer>();
            Color trailColor = Color.Lerp(trailRenderer.material.color, Color.clear, activeSound.fadeSpeed * Time.deltaTime);
            trailRenderer.material.color = trailColor;

            if (renderer.material.color.a < 0.1f && trailRenderer.material.color.a < 0.1f)
            {
                // Reset and re-add to pool.
                activeSound.soundObject.SetActive(false);
                availableSoundProjectiles.Enqueue(activeSound.soundObject);
                renderer.material.color = activeSound.initialRendererColor;
                trailRenderer.material.color = activeSound.initialTrailColor;

                activeSounds.Remove(node);
            }

            node = next;
        }
    }
}

public class SoundProjectile
{
    public float fadeSpeed;
    public GameObject soundObject;
    public Color initialRendererColor;
    public Color initialTrailColor;
}