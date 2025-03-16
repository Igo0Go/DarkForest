using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MusicUIItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text header;

    private AudioClip clip;

    public UnityEvent<AudioClip> choseClip;

    public void Init(AudioClip clip)
    {
        header.text = clip.name;
        this.clip = clip;
    }

    public void OnClick()
    {
        choseClip?.Invoke(clip);
    }
}
