using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FinalBlackPanel : MonoBehaviour
{
    private Image panel;

    [SerializeField]
    private UnityEvent finalEvent;

    private void Awake()
    {
        panel = GetComponent<Image>();
        panel.color = Color.black;
        StartCoroutine(BlackToFadeCoroutine());
    }

    private IEnumerator BlackToFadeCoroutine()
    {
        Color fade = Color.black;
        fade.a = 0;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            panel.color = Color.Lerp(Color.black, fade, t);
            yield return null;
        }
        panel.color = fade;
    }

    public void ToBlack()
    {
        StartCoroutine(FadeToBlackCoroutine());
    }

    private IEnumerator FadeToBlackCoroutine()
    {
        Color fade = Color.black;
        fade.a = 0;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            panel.color = Color.Lerp(fade, Color.black, t);
            yield return null;
        }
        panel.color = Color.black;
        finalEvent.Invoke();
    }
}
