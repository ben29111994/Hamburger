using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public Color[] colorWave;
    private Color color;

    private void OnEnable()
    {
        int r = Random.Range(0, colorWave.Length);
       // color = colorWave[r];
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Brick"))
        {
            Brick _brick = other.GetComponent<Brick>();
            _brick.WaveAnimation();
        }
    }
}
