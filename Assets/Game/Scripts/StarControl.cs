using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarControl : MonoBehaviour
{
    [Header("References")]
    public GameObject[] stars;
    public GameObject[] congratulations;

    public int currentMove;
    public int maxMove;
    public Text currentMoveText;
    public Text maxMoveText;

    public Text currentLevelText;

    private void OnEnable()
    {
        StartCoroutine(C_UpdateStep());
    }

    private IEnumerator C_UpdateStep()
    {
        for(int i = 0; i < congratulations.Length; i++)
        {
            stars[i].SetActive(false);
            congratulations[i].SetActive(false);
        }

        currentLevelText.text = GameManager.instance.levelGame.ToString();
        currentMove = 0;
        currentMoveText.text = currentMove.ToString();
        maxMove = GameManager.instance.maxMove;
        maxMoveText.text = "/" + maxMove;

        int targetCurrentMove = GameManager.instance.currentMove;

        yield return new WaitForSeconds(0.25f);

        while(currentMove < targetCurrentMove)
        {
            currentMove++;
            currentMoveText.text = currentMove.ToString();

            if(targetCurrentMove < 20)
            {
                yield return new WaitForSeconds(0.06f);
            }
            else if (targetCurrentMove < 30)
            {
                yield return new WaitForSeconds(0.04f);
            }
            else if (targetCurrentMove < 40)
            {
                yield return new WaitForSeconds(0.02f);
            }
            else
            {
                yield return null;
            }
        }

        int _n = 0;

        if(currentMove <= maxMove)
        {
            _n = 3;
            // perfect
        }
        else if(currentMove < (int)(maxMove + 10f))
        {
           _n = 2;
            // good
        }
        else
        {
            _n = 1;
            //great
        }

        for (int i = 0; i < _n; i++)
        {
            stars[i].SetActive(true);

            yield return new WaitForSeconds(0.25f);
        }

        congratulations[_n - 1].SetActive(true);

        yield return new WaitForSeconds(1.5f);

        UIManager.instance.Show_CompleteUI();
        gameObject.SetActive(false);
    }
}
