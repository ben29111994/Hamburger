using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emoji : MonoBehaviour
{
    public SpriteRenderer[] emoji;
    public Sprite[] emoji_spr;
    public int[] _n;

    private void OnEnable()
    {
        // emoju handle
        _n = new int[emoji_spr.Length];

        for(int i = 0; i < _n.Length; i++)
        {
            int _r = Random.Range(0, _n.Length);

            if(i != 0)
            {
                for (int j = 0; j < i; j++)
                {
                    if (_n[j] == _r)
                    {
                        _r = Random.Range(0, _n.Length);
                        j = -1;
                    }
                }
            }

            _n[i] = _r;
        }

        for(int i = 0; i < emoji.Length; i++)
        {
            emoji[i].sprite = emoji_spr[_n[i]];
        }

        // position handle

        float _posX = GameManager.instance._pivotHorizontal;
        Vector3 temp = transform.localScale;

        if(transform.position.x  < _posX)
        {
            temp.x = -1;
        }
        else
        {
            temp.x = 1;
        }

        transform.localScale = temp;
    }
}
