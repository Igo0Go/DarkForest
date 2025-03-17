using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class CustomMusicHolder : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> defaultMusicList;

    [HideInInspector]
    public List<AudioClip> musicList;

    private readonly string musicFolderPath = Path.Combine(Application.streamingAssetsPath, "Music");

    public event Action loadComplete;

    public void StartLoadCustomMusic()
    {
        StartCoroutine(TryLoadPlayerMusic());
    }

    private IEnumerator TryLoadPlayerMusic()
    {
        musicList = new List<AudioClip>();
        musicList.AddRange(defaultMusicList);
        string[] files = Directory.GetFiles(musicFolderPath);

        if (files.Length > 0)
        {
            DirectoryInfo di = new DirectoryInfo(musicFolderPath);
            FileInfo[] UserFiles = di.GetFiles("*.wav", SearchOption.TopDirectoryOnly);

            if (UserFiles.Length > 0)
            {
                for (int i = 0; i < UserFiles.Length; i++)
                {
                    string fileName = Path.Combine(musicFolderPath, UserFiles[i].Name);


                    using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fileName,
                        AudioType.WAV);
                    var synRes = www.SendWebRequest();

                    yield return synRes;

                    if (www.result == UnityWebRequest.Result.Success && synRes.isDone)
                    {
                        var data = DownloadHandlerAudioClip.GetContent(www);
                        AudioClip clip = data;
                        string clipName = UserFiles[i].Name.Split('.')[0];
                        clip.name = clipName;
                        musicList.Add(clip);
                    }
                }
            }
        }

        loadComplete?.Invoke();
    }

    public void OpenFolder()
    {
        Process.Start(musicFolderPath);
    }
}
