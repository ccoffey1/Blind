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

    private TrailRenderer trailRenderer;
    private Rigidbody2D rb;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private IEnumerator FadeOut(float fadeSpeed)
    {
        // TODO: could we just turn off collision for sound, and use trail duration instead, rather than deactivating?
        // This would allow for making the bullets 'shut off' after X seconds without trail abruptly vanishing?
        while (trailRenderer.material.color.a > 0.1f)
        {
            Color trailColor = Color.Lerp(trailRenderer.material.color, Color.clear, fadeSpeed * Time.deltaTime);
            trailRenderer.material.color = trailColor;
            yield return null;
        }

        Pool.Release(this);
    }

    public void Spawn(
        Vector2 spawnAt,
        Vector2 projectileMoveDirection,
        float fadeSpeed,
        float linearDrag = 2.0f,
        Color? color = null,
        GameObject spawnedBy = null)
    {
        gameObject.transform.position = spawnAt;
        SpawnOrigin = spawnAt;
        SpawnedBy = spawnedBy;
        SpawnTime = Time.timeSinceLevelLoad;
        trailRenderer.material.color = color ?? Color.white;

        rb.velocity = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);
        rb.drag = linearDrag;

        StartCoroutine(FadeOut(fadeSpeed));
    }
}
