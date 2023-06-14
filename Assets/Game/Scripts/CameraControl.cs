using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public static CameraControl instance;

    public RectTransform top;
    public float _posY_ipad;
    public float _posY_X;
    public float _posY_Other;

    private void Awake()
    {
        instance = this;

        SetFOV();

    }

    private float currentFOV;


    void SetFOV()
    {
        float ratio = Camera.main.aspect;

        if (ratio >= 0.74f) // 3:4
        {
            Camera.main.fieldOfView = 55;
            top.anchoredPosition = new Vector2(0, _posY_ipad);

            // 16
        }
        else if (ratio >= 0.56f) // 9:16
        {
            Camera.main.fieldOfView = 55;
            top.anchoredPosition = new Vector2(0, _posY_Other);

            // 17
        }
        else if (ratio >= 0.45f)
        {
            Camera.main.fieldOfView = 65;
            top.anchoredPosition = new Vector2(0, _posY_X);

            // 19
        }

        currentFOV = Camera.main.fieldOfView;
    }


    public void SetFOVSizeMap()
    {
        float _w = GameManager.instance._horizontal;

        float _t = (_w - 4f) / (8 - 4f);
        float _fov = currentFOV * Mathf.Lerp(0.5f, 1.0f, _t);
        Camera.main.fieldOfView = _fov;
    }
    
}