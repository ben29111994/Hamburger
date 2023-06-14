using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    public float time;
    private void Update()
    {
        Destroy(gameObject, time);
    }
}
