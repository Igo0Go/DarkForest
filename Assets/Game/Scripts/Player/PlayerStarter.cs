using System.Collections.Generic;
using UnityEngine;

public class PlayerStarter : MonoBehaviour
{
    [SerializeField]
    private List<PlayerPart> partList;

    void Start()
    {
        foreach (PlayerPart part in partList)
        {
            part.Activate();
        }
    }
}
