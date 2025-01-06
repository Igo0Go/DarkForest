using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneModule : MonoBehaviour
{
    [SerializeField]
    private int SceneIndex = 1;

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneIndex);
    }
}
