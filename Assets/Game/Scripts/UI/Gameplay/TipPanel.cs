using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : MonoBehaviour
{
    public Action<bool> changeUpgradePauseState;

    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private TMP_Text headerText;
    [SerializeField]
    private TMP_Text descriptionText;
    [SerializeField]
    private Image tipImage;

    public void Init()
    {
        CloseTip();
    }

    public void ShowTip(TipItem item)
    {
        changeUpgradePauseState?.Invoke(true);
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
        changeUpgradePauseState?.Invoke(false);
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
