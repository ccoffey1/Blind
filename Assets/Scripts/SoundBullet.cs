using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SoundBullet : MonoBehaviour
{
    public Vector2 SpawnOrigin { get; private set; }
    public GameObject SpawnedBy { get; private set; }
    public float SpawnTime { get; private set; }
    public ObjectPool<SoundBullet> Pool { get; set; }

    private TrailRenderer primaryTrail;
    private TrailRenderer secondaryTrail;
    private Rigidbody2D rb;

    private void Awake()
    {
        primaryTrail = transform.Find("Primary Trail").GetComponent<TrailRenderer>();
        secondaryTrail = transform.Find("Secondary Trail").GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private IEnumerator FadeOut(float fadeSpeed)
    {
        // TODO: could we just turn off collision for sound, and use trail duration instead, rather than deactivating?
        // This would allow for making the bullets 'shut off' after X seconds without trail abruptly vanishing?
        while (primaryTrail.material.color.a > 0.1f)
        {
            Color primaryTrailColor = Color.Lerp(primaryTrail.material.color, Color.clear, fadeSpeed * Time.deltaTime);
            primaryTrail.material.color = primaryTrailColor;
            Color secondaryTrailColor = Color.Lerp(secondaryTrail.material.color, Color.clear, fadeSpeed * Time.deltaTime);
            secondaryTrail.material.color = secondaryTrailColor;
            yield return null;
        }

        Pool.Release(this);
    }

    public void Spawn(
        Vector2 spawnAt,
        Vector2 projectileMoveDirection,
        float fadeSpeed,
        float linearDrag = 2.0f,
        Color? primaryTrailColor = null,
        GameObject spawnedBy = null)
    {
        gameObject.transform.position = spawnAt;
        SpawnOrigin = spawnAt;
        SpawnedBy = spawnedBy;
        SpawnTime = Time.timeSinceLevelLoad;
        primaryTrail.material.color = primaryTrailColor ?? Color.white;
        secondaryTrail.material.color = Color.red; // Default to red for dangerous junk

        rb.velocity = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);
        rb.drag = linearDrag;

        StartCoroutine(FadeOut(fadeSpeed));
    }

    public void ClearTrails()
    {
        primaryTrail.Clear();
        secondaryTrail.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Harmful Terrain"))
        {
            primaryTrail.emitting = false;
            secondaryTrail.emitting = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Harmful Terrain"))
        {
            primaryTrail.emitting = true;
            secondaryTrail.emitting = false;
        }
    }
}
