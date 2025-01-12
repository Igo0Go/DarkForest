using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private TMP_Text headerText;
    [SerializeField]
    private TMP_Text descriptionText;
    [SerializeField]
    private Image tipImage;

    public void ShowTip(TipItem item)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        panel.SetActive(true);
        headerText.text = item.title;
        descriptionText.text = item.description;

        if(item.tipImage != null)
        {
            tipImage.gameObject.SetActive(true);
            tipImage.sprite = item.tipImage;
        }
        else
        {
            tipImage.gameObject.SetActive(false);
        }
    }

    public void CloseTip()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        panel.SetActive(false);
    }
}

[System.Serializable]
public class TipItem
{
    public string title;
    [TextArea(5,10)]
    public string description;
    public Sprite tipImage;
}
