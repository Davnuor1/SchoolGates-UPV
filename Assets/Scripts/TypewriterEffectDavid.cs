using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffectDavid : MonoBehaviour
{
    public float delay = 0.05f;
    private Coroutine typingCoroutine;

    public void Play(string fullText, TMP_Text targetText)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(fullText, targetText));
    }

    private IEnumerator TypeText(string fullText, TMP_Text targetText)
    {
        targetText.text = "";
        foreach (char c in fullText)
        {
            targetText.text += c;
            yield return new WaitForSeconds(delay);
        }
    }

    public void Stop()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
    }
}
