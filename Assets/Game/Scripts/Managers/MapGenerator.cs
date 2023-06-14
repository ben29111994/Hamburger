using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public MapGenerator instance;
    public List<GameObject> blankObjects;
    public Vector3[][] blankArray;
    private void Awake()
    {
        instance = this;
    }

    public void AddObject(GameObject _blank)
    {
        blankObjects.Add(_blank);
    }

    public void GenerateRandomMap(Texture2D texture)
    {
        int blankObjectsIndex = 0;
        for (int i = 0; i < texture.width - 2; i++)
        {
            for (int j = 0; j < texture.height -2; j++)
            {
                blankArray[i][j] = blankObjects[blankObjectsIndex].transform.position;
                blankObjectsIndex++;
            }
        }

        var iHole = Random.Range(0, texture.width - 2);
        var jHole = Random.Range(0, texture.height - 2);
        int iPlayer;
        int jPlayer;
        bool isSame = true;
        while (isSame)
        {
            iPlayer = Random.Range(0, texture.width - 2);
            jPlayer = Random.Range(0, texture.height - 2);

            if (iPlayer == iHole || jPlayer == jHole) isSame = true;
            else isSame = false;
        }

        for (int i = 0; i < texture.width - 2; i++)
        {
            int obstaclePos = Random.Range(0, texture.height - 2);
            for (int j = 0; j < texture.height - 2; j++)
            {
                if (i == iHole && j == jHole)
                {

                }
            }
        }

    }

}
