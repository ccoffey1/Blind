using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen Instance;

    private CanvasGroup canvasGroup;
    private float fadeDuration = 3f;

    public void Start()
    {
        Instance = this;
        gameObject.SetActive(false);
        canvasGroup = GetComponent<CanvasGroup>();        
    }

    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(1f);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            yield return null;
        }
    }
}
