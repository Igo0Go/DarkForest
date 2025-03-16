using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonsPanel;
    [SerializeField]
    private SceneSelector sceneSelector;
    [SerializeField]
    private SettingsPanel settingsPanel;
    [SerializeField]
    private ArenaPanel arenaPanel;
    [SerializeField]
    MusicPlayer musicPlayer;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;
        buttonsPanel.SetActive(true);
        sceneSelector.Init();
        settingsPanel.Init();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape) && !LevelResultPanel.message)
        {
            if(!buttonsPanel.activeSelf)
            {
                musicPlayer.ToDefault();
                sceneSelector.CloseSelector();
                arenaPanel.ClosePanel();
                buttonsPanel.SetActive(true);
            }
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            sceneSelector.TestUnlockNextLevel();
        }
    }

    public void StartGame()
    {
        sceneSelector.ShowSelector();
        buttonsPanel.SetActive(false);
    }

    public void ToArena()
    {
        arenaPanel.ShowArenaPanel();
        buttonsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
