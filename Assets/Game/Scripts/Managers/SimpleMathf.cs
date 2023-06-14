using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMathf : MonoBehaviour
{
    public static int[] RandomIntArray(int lenght)
    {
        int[] newArray = new int[lenght];

        for (int i = 0; i < lenght; i++)
        {
            int r = Random.Range(0, lenght);

            if (i != 0)
            {
                for (int j = 0; j < i; j++)
                {
                    if (newArray[j] == r)
                    {
                        r = Random.Range(0, lenght);
                        j = -1;
                    }
                }
            }
          
            newArray[i] = r;
        }

        return newArray;
    }

    public static float MediumPoint(float a, float b)
    {
        return (a + b) * 0.5f;
    }

    public static float MediumPoint(float[] a)
    {
        float sum = 0.0f;
        int length = a.Length;

        for(int i = 0; i < length; i++)
        {
            sum += a[i];
        }

        float medium = sum / (float)length;

        return medium;
    }

    public static Vector3 MediumVector(Vector3 a, Vector3 b)
    {
        return (a + b) * 0.5f;
    }

    public static Vector3 MediumVector(Vector3[] a)
    {
        Vector3 sum = Vector3.zero;
        int length = a.Length;

        for (int i = 0; i < length; i++)
        {
            sum += a[i];
        }

        Vector3 medium = sum / (float)length;

        return medium;
    }

    public static Vector3 Direction(Vector3 fromVector, Vector3 toVector)
    {
        return toVector - fromVector;
    }
}
