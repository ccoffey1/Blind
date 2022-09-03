using Assets.Scripts.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Footstep : MonoBehaviour
{
    public ObjectPool<Footstep> Pool { get; set; }

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public Vector2 Spawn(Transform sourceTransform,
                      FootType footType,
                      float fadeSpeed,
                      float offset,
                      Color? color = null)
    {
        spriteRenderer.material.color = color ?? Color.white;
        spriteRenderer.flipY = footType == FootType.LEFT;

        transform.SetPositionAndRotation(sourceTransform.position, sourceTransform.rotation);
        transform.Translate(0, offset, 0, sourceTransform);

        StartCoroutine(FadeOut(fadeSpeed));

        return transform.position;
    }

    private IEnumerator FadeOut(float fadeSpeed)
    {
        while (spriteRenderer.material.color.a > 0.1f)
        {
            Color spriteColor = Color.Lerp(spriteRenderer.material.color, Color.clear, fadeSpeed * Time.deltaTime);
            spriteRenderer.material.color = spriteColor;
            yield return null;
        }

        Pool.Release(this);
    }
}
