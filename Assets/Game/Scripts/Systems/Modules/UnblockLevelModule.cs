using UnityEngine;

public class UnblockLevelModule : MonoBehaviour
{
    [SerializeField]
    private int levelIndex = 0;

    public void Unblock()
    {
        GameCenter.maxLevel = levelIndex;
    }
}
