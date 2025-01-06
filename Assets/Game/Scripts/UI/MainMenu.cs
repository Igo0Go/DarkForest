using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject mainPanel;
    [SerializeField]
    private GameObject sceneSelector;
    [SerializeField]
    private TMP_Text levelNameText;
    [SerializeField]
    private Image levelIcon;
    [SerializeField]
    private TMP_Text levelDescriptionText;
    [SerializeField]
    private GameObject leftButton;
    [SerializeField]
    private GameObject rightButton;
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private List<LevelInfoItem> levelInfoItems;


    private int currentScene;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        currentScene = 0;
        mainPanel.SetActive(true);
        sceneSelector.SetActive(false);
    }

    public void StartGame()
    {
        mainPanel.SetActive(false);
        sceneSelector.SetActive(true);
        CheckLevelSelectorButtons();
        SetLevelInfo();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(levelInfoItems[currentScene].sceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void NextScene()
    {
        currentScene++;
        CheckLevelSelectorButtons();
        SetLevelInfo();
    }
    public void PreviousScene()
    {
        currentScene--;
        CheckLevelSelectorButtons();
        SetLevelInfo();
    }

    [ContextMenu("Открыть второй уровень")]
    public void TestUnlockNextLevel()
    {
        GameCenter.maxLevel = 1;
        CheckLevelSelectorButtons();
    }

    private void SetLevelInfo()
    {
        levelNameText.text = levelInfoItems[currentScene].levelName;
        levelIcon.sprite = levelInfoItems[currentScene].levelIcon;
        levelDescriptionText.text = levelInfoItems[currentScene].levelDescription;

        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(ChangeMusicCoroutine(levelInfoItems[currentScene].levelMusic));
    }
    private void CheckLevelSelectorButtons()
    {
        if (currentScene >= GameCenter.maxLevel)
        {
            rightButton.SetActive(false);
        }
        else
        {
            rightButton.SetActive(true);
        }

        if (currentScene == 0)
        {
            leftButton.SetActive(false);
        }
        else
        {
            leftButton.SetActive(true);
        }
    }


    private Coroutine currentCoroutine;
    private IEnumerator ChangeMusicCoroutine(AudioClip clip)
    {
        while(source.volume > 0)
        {
            source.volume -= Time.deltaTime;
            yield return null;
        }

        source.Stop();
        source.clip = clip;
        source.Play();

        while (source.volume < 1)
        {
            source.volume += Time.deltaTime;
            yield return null;
        }

        source.volume = 1;
    }
}

[System.Serializable]
public class LevelInfoItem
{
    public string levelName;
    public Sprite levelIcon;
    [TextArea(5, 10)]
    public string levelDescription;
    public AudioClip levelMusic;
    public int sceneIndex;
}
