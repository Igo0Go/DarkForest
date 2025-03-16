using UnityEngine;

public class ArenaMusicSystem : MonoBehaviour
{
    [SerializeField]
    private MusicRageSystem musicRageSystem;

    private void Start()
    {
        if(ArenaSettingsHolder.music != null)
        {
            musicRageSystem.HardChangeMusic(ArenaSettingsHolder.music);
        }
    }
}

public static class ArenaSettingsHolder
{
    public static LevelInfoItem sceneInfo;
    public static AudioClip music;
}
