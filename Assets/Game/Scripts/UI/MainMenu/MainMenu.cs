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
    private SceneSelector sceneSelector;
    [SerializeField]
    private SettingsPanel settingsPanel;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;
        mainPanel.SetActive(true);
        sceneSelector.Init();
        settingsPanel.Init();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            sceneSelector.TestUnlockNextLevel();
        }
    }

    public void StartGame()
    {
        sceneSelector.ShowSelector();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
