using System.Collections.Generic;
using UnityEngine;

public class LevelResultModule : MonoBehaviour
{
    [SerializeField]
    private List<LevelResultMessage> messages = new List<LevelResultMessage>();

    public void SaveMessages()
    {
        foreach (var message in messages)
        {
            LevelResultPanel.levelResultMessages.Add(message);
        }
    }
}
