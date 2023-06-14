using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObject : MonoBehaviour
{
    public float time;

    private void OnEnable()
    {
        Invoke("Hide", time);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
