using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    //public Transform[] targets;


    //private void OnEnable()
    //{
    //    Invoke("Move",0.1f);
    //}

    //private void Move()
    //{
    //    Vector3[] paths = new Vector3[targets.Length];

    //    for(int i = 0; i < targets.Length; i++)
    //    {
    //        paths[i] = targets[i].position;
    //    }

    //    iTween.MoveTo(gameObject, iTween.Hash("path", paths, "time", 2.0f));
    //}

    public Transform targets;
    public Transform[] midPoints;
    Vector3 currentPosition;
    Vector3[] paths;
    private bool isRandom;
    Vector3 maxScale;
    float timeAnim;

    private void Start()
    {
    }

    private void OnEnable()
    {
        Move();
    }


    private void Move()
    {
        if(isRandom == false)
        {
            isRandom = true;
 
            currentPosition = transform.position;
        }
        paths = new Vector3[2];
        paths[0] = midPoints[Random.Range(0, midPoints.Length)].position;
        paths[1] = targets.position;
        transform.position = currentPosition;
        timeAnim = 0.7f;

        maxScale = Vector3.one;
        transform.localScale = maxScale;

        iTween.MoveTo(gameObject, iTween.Hash("path", paths, "time", timeAnim, "easetype",iTween.EaseType.easeOutSine));
        iTween.ScaleTo(gameObject, iTween.Hash("scale", maxScale / 5, "time", timeAnim, "easetype", iTween.EaseType.easeInSine, "oncomplete","C_"));
    }

    private void C_()
    {
        transform.localScale = Vector3.zero;
        Invoke("Hide", 1.0f);
       // iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.one * 0.3f, "time", timeAnim/2, "easetype", iTween.EaseType.easeOutSine));
    }

    private void Hide()
    {
       // isRandom = false;
        gameObject.SetActive(false);
    }
}
