using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager instance;

    private void Awake()
    {
        instance = this;

        if (PlayerPrefs.GetInt("f_vibration") == 0)
        {
            SetVibration();
            PlayerPrefs.SetInt("f_vibration", 1);
        }
    }

    [Header("Vibration")]
    public bool isVibration;
    public Sprite vibrationSpr;
    public Sprite notvibrationSpr;
    public Image vibrationBtn;

    void Start()
    {
        int v = PlayerPrefs.GetInt("vibration");
        if (v == 0)
        {
            isVibration = false;
            vibrationBtn.sprite = notvibrationSpr;

        }
        else
        {
            isVibration = true;
            vibrationBtn.sprite = vibrationSpr;

        }
    }

    public void SetVibration()
    {
        isVibration = !isVibration;

        int v;
        if (isVibration)
        {
            v = 1;
            vibrationBtn.sprite = vibrationSpr;
        }
        else
        {
            v = 0;
            vibrationBtn.sprite = notvibrationSpr;
        }
        PlayerPrefs.SetInt("vibration", v);
    }
}
