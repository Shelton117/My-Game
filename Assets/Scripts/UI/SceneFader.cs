using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class SceneFader : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    private CanvasGroup canvasGroup;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private float fadeInDuration;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private float fadeOutDuration;


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator FadeInOut()
    {
        yield return FadeOut(fadeOutDuration);
        yield return FadeIn(fadeInDuration);
    }

    public IEnumerator FadeOut(float time)
    {
        while (canvasGroup.alpha < 0)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }

    public IEnumerator FadeIn(float time)
    {
        while (canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }

        Destroy(gameObject);
    }
}
