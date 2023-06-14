using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        instance = this;


        if (PlayerPrefs.GetInt("f_audio") == 0)
        {
            SetAudio();
            PlayerPrefs.SetInt("f_audio", 1);
        }
    }

    [Header("Audio")]
    public bool   isAudio;
    public Sprite audioSpr;
    public Sprite notaudioSpr;
    public Image  audioBtn;

    void Start()
    {
        int a = PlayerPrefs.GetInt("audio");
        if (a == 0)
        {
            isAudio = false;
            audioBtn.sprite = notaudioSpr;

        }
        else
        {
            isAudio = true;
            audioBtn.sprite = audioSpr;
        }
    }

    public void SetAudio()
    {
        isAudio = !isAudio;

        int a;
        if (isAudio)
        {
            a = 1;
            audioBtn.sprite = audioSpr;
        }
        else
        {
            a = 0;
            audioBtn.sprite = notaudioSpr;
        }
        PlayerPrefs.SetInt("audio", a);
    }
}
