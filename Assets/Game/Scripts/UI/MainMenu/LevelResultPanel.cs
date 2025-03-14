using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelResultPanel : MonoBehaviour
{
    public static List<LevelResultMessage> levelResultMessages = new List<LevelResultMessage>();

    [SerializeField]
    private GameObject levelResultPanel;
    [SerializeField]
    private TMP_Text headerText;
    [SerializeField]
    private TMP_Text messageText;
    [SerializeField]
    private GameObject finalButton;

    [SerializeField]
    private GameObject buttonsPanel;

    private Color fullColor;
    private Color fadeColor;

    public static bool message;

    private void Start()
    {
        fullColor = headerText.color;
        fadeColor = messageText.color;
        fadeColor.a = 0;

        headerText.color = fadeColor;
        messageText.color = fadeColor;

        buttonsPanel.SetActive(false);
        CheckMessages();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && message)
        {
            if(currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                headerText.color = fullColor;
                messageText.color = fullColor;
                finalButton.SetActive(true);
            }
        }
    }

    public void CheckMessages()
    {
        if(levelResultMessages.Count > 0)
        {
            message = true;
            levelResultPanel.SetActive(true);
            headerText.text = levelResultMessages[0].header;
            messageText.text = levelResultMessages[0].message;
            levelResultMessages.RemoveAt(0);
            currentCoroutine = StartCoroutine(ShowNewMessageCoroutine());
        }
        else
        {
            message = false;
            levelResultPanel.SetActive(false);
            buttonsPanel.SetActive(true);
        }
    }

    private Coroutine currentCoroutine;
    private IEnumerator ShowNewMessageCoroutine()
    {
        headerText.color = fadeColor;
        messageText.color = fadeColor;
        finalButton.SetActive(false);

        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime;
            headerText.color = Color.Lerp(fadeColor, fullColor, t);
            yield return null;
        }

        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            messageText.color = Color.Lerp(fadeColor, fullColor, t);
            yield return null;
        }

        finalButton.SetActive(true);
        currentCoroutine = null;
    }
}

[System.Serializable]
public class LevelResultMessage
{
    public string header;
    [TextArea(5,15)]
    public string message;
}