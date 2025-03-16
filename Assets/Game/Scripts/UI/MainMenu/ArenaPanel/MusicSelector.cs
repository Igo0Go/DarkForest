using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private CustomMusicHolder customMusicHolder;
    [SerializeField]
    private GameObject musicUIItemPrefab;
    [SerializeField]
    private Transform musicItemsContainer;
    [SerializeField]
    private MusicPlayer musicPlayer;
    [SerializeField]
    private Image levelIcon;
    [SerializeField]
    private TMP_Text levelText;
    [SerializeField]
    private TMP_Text musicText;
    [SerializeField]
    private GameObject finalButton;

    public void ShowPanel()
    {
        finalButton.SetActive(false);
        levelIcon.gameObject.SetActive(false);
        musicText.gameObject.SetActive(false);
        levelText.gameObject.SetActive(false);
        customMusicHolder.loadComplete += OnMusicLoadComplete;
        customMusicHolder.StartLoadCustomMusic();
        panel.SetActive(true);
    }
    public void CloseSelector()
    {
        panel.SetActive(false);
    }

    private void OnMusicLoadComplete()
    {
        finalButton.SetActive(true);
        levelIcon.gameObject.SetActive(true);
        musicText.gameObject.SetActive(true);
        levelText.gameObject.SetActive(true);

        levelIcon.sprite = ArenaSettingsHolder.sceneInfo.levelIcon;
        levelText.text = ArenaSettingsHolder.sceneInfo.levelName;

        List<AudioClip> clipList = customMusicHolder.musicList;

        foreach (AudioClip clip in clipList)
        {
            MusicUIItem item = Instantiate(musicUIItemPrefab, musicItemsContainer).GetComponent<MusicUIItem>();
            item.choseClip.AddListener(OnNewSelectedClip);
            item.Init(clip);
        }



        OnNewSelectedClip(clipList[0]);
    }

    private void OnNewSelectedClip(AudioClip clip)
    {
        ArenaSettingsHolder.music = clip;
        musicPlayer.PlayNewClip(clip);
        musicText.text = clip.name;
    }
}
