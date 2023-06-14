using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Test : MonoBehaviour
{

    private void A()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * 100);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            A();
        }
    }
}
