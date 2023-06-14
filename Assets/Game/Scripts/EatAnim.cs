using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatAnim : MonoBehaviour
{
    public Outline outLine;
    public Color[] colorOutline;

    private void OnEnable()
    {
        outLine.effectColor = colorOutline[Random.Range(0, colorOutline.Length)];
     
    }

    public Image yummyImg;
    public Sprite[] yummy;

    public void SetYummy(int n)
    {
        yummyImg.sprite = yummy[n];

        if(n == 0)
        {
            transform.localScale = Vector3.one * 1.2f;
        }
        else if (n == 1)
        {
            transform.localScale = Vector3.one * 1.6f ;
        }
        else
        {
            transform.localScale = Vector3.one * 2.0f;
        }
    }
}
