using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorManager : MonoBehaviour
{
    public bool isAutoJump;

    public List<Spectator> spectators = new List<Spectator>();

    private void Start()
    {
        for(int i = 2;i < transform.childCount; i++)
        {
            Spectator _spectator = transform.GetChild(i).GetChild(0).gameObject.GetComponent<Spectator>();

            if(_spectator != null)
            {
                spectators.Add(_spectator);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SpectatorAnimation();
        }

        if (GameManager.instance == null) return;

        if (isAutoJump)
        {
            SpectatorAnimation();
        }
    }

    public void SpectatorAnimation()
    {
        if (isAnimation) return;

        if (gameObject.activeSelf == false) return;

        StartCoroutine(C_SpectatorAnimation());
    }

    private bool isAnimation;

    private IEnumerator C_SpectatorAnimation()
    {
        isAnimation = true;

        for (int k = 0; k < 7; k++)
        {
            for (int i = 0; i < spectators.Count; i++)
            {
                spectators[i].Jump();
            }

            yield return new WaitForSeconds(0.2f);
        }

        isAnimation = false;
    }
}
