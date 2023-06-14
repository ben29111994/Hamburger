using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailText : MonoBehaviour
{
    public Text text1, text2;

    private void OnEnable()
    {
        StartCoroutine(C_ChangeText());
    }

    private IEnumerator C_ChangeText()
    {
        text1.text = text2.text = "TOP BUN COMES LAST";

        yield return new WaitForSeconds(2.0f);

        text1.text = text2.text = "MORE CHANCE?";
    }
}
