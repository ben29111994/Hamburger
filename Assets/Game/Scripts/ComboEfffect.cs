using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboEfffect : MonoBehaviour
{
    public float speed = 15f;
    public float maxRotation = 10f;
    public TextMeshPro textMeshPro;

    private void Awake()
    {
        transform.position = new Vector3(Random.Range(2.5f, 4f), 3, Random.Range(-2,3f));
        
    }
    public void SetText(string combo)
    {
        textMeshPro.SetText("Combo x" + combo);
    }

    void Update()
    {
        //textMeshPro.SetText("Combo x" + GameManager.instance.combo);
        transform.rotation = Quaternion.Euler(60f, 0f, maxRotation * Mathf.Sin(Time.time * speed));
        transform.localScale += new Vector3(1, 1, 1)*Time.deltaTime * 10f;
        if (transform.localScale.y >= 4f)
        {
            Destroy(gameObject);
        }
    }
}
