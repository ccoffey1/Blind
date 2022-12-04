using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField]
    private TextMeshProUGUI textComponent;
    [SerializeField]
    private float textSpeed;
    [SerializeField]
    private float punctuationDelayMultiplier;
    [SerializeField]
    private float fadeOutTime;

    private void Awake()
    {
        instance = this;
    }

    public void Say(string text)
    {
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine(text));
    }

    private IEnumerator TypeLine(string text)
    {
        textComponent.alpha = 1.0f;
        foreach (char c in text)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed * (c == '.' ? punctuationDelayMultiplier : 1));
        }
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            textComponent.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutTime);
            yield return null;
        }
    }
}
