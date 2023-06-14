using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewUnlock : MonoBehaviour
{
    public string[] nameArray;
    public Sprite[] spriteArray;
    public Image img;
    public Text nameText;

    private void OnEnable()
    {
        int m = PlayerPrefs.GetInt("model");

        if(m >= spriteArray.Length)
        {
            Hide();

            return;
        }

        img.sprite = spriteArray[m];
        nameText.text = nameArray[m];

        Invoke("Hide", 2.1f);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
