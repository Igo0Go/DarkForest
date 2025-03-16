using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaPanel : MonoBehaviour
{
    [SerializeField]
    private SceneSelector sceneSelector;
    [SerializeField]
    private MusicSelector musicSelector;

    private bool isOpen;

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape) && !isOpen)
        {
            ClosePanel();
        }
    }


    public void ShowArenaPanel()
    {
        isOpen = true;
        sceneSelector.ShowSelector();
    }

    public void NextStage()
    {
        sceneSelector.CloseSelector();
        musicSelector.ShowPanel();
    }

    public void ClosePanel()
    {
        isOpen = false;
        sceneSelector.CloseSelector();
        musicSelector.CloseSelector();
    }

    public void LoadArenaScene()
    {
        SceneManager.LoadScene(ArenaSettingsHolder.sceneInfo.sceneIndex);
    }
}
