using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    public static ColorManager instance;

    private void Awake()
    {
        instance = this;
    }

    [Header("References")]
    public Color colorPlane;
    public Material caro1_material;
    public Material caro2_material;
    public Material obtacle_material;
    public Material plane_material;
    public Material plane1_material;
    public Material plane2_material;

    public List<SetColor> listSetColor = new List<SetColor>();
    public SetColor currentSetColor;
    private int currentNumber;

    [System.Serializable]
    public class SetColor
    {
        public Color caro1;
        public float metallic_caro1;
        public float smoothness_caro1;

        public Color caro2;
        public float metallic_caro2;
        public float smoothness_caro2;


        public Color obtacle;
        public float metallic_obtacle;
        public float smoothness_obtacle;


        public Color plane;
        public float metallic_plane;
        public float smoothness_plane;


        public Color plane1;
        public float metallic_plane1;
        public float smoothness_plane1;


        public Color plane2;
        public float metallic_plane2;
        public float smoothness_plane2;
    }

    private void Start()
    {
        RandomCurrentNumber();
        SetColorFromData();
    }

    private void RandomCurrentNumber()
    {
        for (int i = 0; i < 1; i++)
        {
            int r = Random.Range(0, listSetColor.Count);

            if(r == currentNumber)
            {
                i--;
            }
            else
            {
                currentNumber = r;
            }
        }

        currentSetColor = listSetColor[currentNumber];
    }

    private void NextCurrentNumber()
    {
        currentNumber++;

        if (currentNumber >= listSetColor.Count)
        {
            currentNumber = 0;
        }

        currentSetColor = listSetColor[currentNumber];
    }

    private void SetColorFromData()
    {
        caro1_material.color = currentSetColor.caro1;
        caro1_material.SetFloat("_Metallic", currentSetColor.metallic_caro1);
        caro1_material.SetFloat("_Glossiness", currentSetColor.smoothness_caro1);

        caro2_material.color = currentSetColor.caro2;
        caro2_material.SetFloat("_Metallic", currentSetColor.metallic_caro2);
        caro2_material.SetFloat("_Glossiness", currentSetColor.smoothness_caro2);


        obtacle_material.color = currentSetColor.obtacle;
        obtacle_material.SetFloat("_Metallic", currentSetColor.metallic_obtacle);
        obtacle_material.SetFloat("_Glossiness", currentSetColor.smoothness_obtacle);

        plane_material.color = currentSetColor.plane;
        plane_material.SetFloat("_Metallic", currentSetColor.metallic_plane);
        plane_material.SetFloat("_Glossiness", currentSetColor.smoothness_plane);

        plane1_material.color = currentSetColor.plane1;
        plane1_material.SetFloat("_Metallic", currentSetColor.metallic_plane1);
        plane1_material.SetFloat("_Glossiness", currentSetColor.smoothness_plane1);

        plane2_material.color = currentSetColor.plane2;
        plane2_material.SetFloat("_Metallic", currentSetColor.metallic_plane2);
        plane2_material.SetFloat("_Glossiness", currentSetColor.smoothness_plane2);
    }

    public void ChangeColorBrick()
    {
        StartCoroutine(C_ChangeColorBrick());
    }

    private IEnumerator C_ChangeColorBrick()
    {
        float t = 0;
        Color caro1 = caro1_material.color;
        Color caro2 = caro2_material.color;
        Color obtacleColor = obtacle_material.color;

        while(t < 1)
        {
            t += Time.deltaTime * 2.0f;

            caro1_material.color = Color.Lerp(caro1, colorPlane, t);
            caro2_material.color = Color.Lerp(caro2, colorPlane, t);
            obtacle_material.color = Color.Lerp(obtacleColor, colorPlane, t);

            yield return null;
        }
    }

    public void ChangeColor()
    {
        NextCurrentNumber();
        SetColorFromData();
    }
}
