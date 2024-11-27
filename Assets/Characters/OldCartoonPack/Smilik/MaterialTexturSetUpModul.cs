using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MaterialTexturSetUpModul : MonoBehaviour
{
    [SerializeField]
    private List<Texture2D> setUpPacks = null;
    [SerializeField]
    private Material material = null;

    [SerializeField]
    [Range(0.05f, 1)]
    private float musicSpritesChangingDelay = 1;

    public void SetUpPuck(int number)
    {
        StopAllCoroutines();
        SettextureByNumber(number);
    }

    public void PlayMusic()
    {
        StartCoroutine(PlayMusicCoroutine());
    }

    private void SettextureByNumber(int number)
    {
        try
        {
            material.SetTexture("_EmissionMap", setUpPacks[number]);
//            material.mainTexture = setUpPacks[number];
        }
        catch (Exception ex)
        {
            Debug.LogError("Возможно, не добавлен элемент с индексом " + number + " но он вызывается из события анимации. Ошибка: "
                + ex.Message);
        }
    }

    private IEnumerator PlayMusicCoroutine()
    {
        int number = 8;
        while(true)
        {
            SettextureByNumber(number);
            number++;
            if(number > 12)
            {
                number = 8;
            }
            yield return new WaitForSeconds(musicSpritesChangingDelay);
        }
    }
}
