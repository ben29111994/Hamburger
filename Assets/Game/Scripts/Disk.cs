using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disk : MonoBehaviour
{
    public float force;
    public Animator diskAnimator;
    public Animator plate;
    public Animator elementAnimator;
    public GameObject[] elementCheese;
    public GameObject bottom;
    public Rigidbody rigidbody;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Explosion();
        }
    }


    private void OnEnable()
    {
        Transform _cheese = transform.GetChild(0).GetChild(0).GetChild(0).transform;
        // _cheese.localPosition = new Vector3(0, 0.067f, 0);
        //  _cheese.transform.localEulerAngles = Vector3.zero;
        _cheese.gameObject.SetActive(true);

        plate.speed = 1;
        diskAnimator.enabled = true;
        plate.enabled = true;
        elementCheese[0].SetActive(true);
        elementCheese[1].SetActive(true);
        elementCheese[2].SetActive(true);

        StartCoroutine(C_SetPositionBotton());
    }

    private IEnumerator C_SetPositionBotton()
    {
        yield return null; float _posX = GameManager.instance._pivotHorizontal;

        if (transform.position.x > _posX)
        {
            bottom.transform.localPosition = GameManager.instance.left.transform.position;
            bottom.transform.localEulerAngles = GameManager.instance.left.transform.eulerAngles;
        }
        else
        {
            bottom.transform.localPosition = GameManager.instance.right.transform.position;
            bottom.transform.localEulerAngles = GameManager.instance.right.transform.eulerAngles;

        }

        bottom.SetActive(true);
    }


    public void Explosion()
    {
        StartCoroutine(C_Explosion());
    }

    private IEnumerator C_Explosion()
    {
        bottom.SetActive(false);

        Rigidbody r;

        for (int i = 1; i < transform.GetChild(0).GetChild(0).childCount; i++)
        {
            r = transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Rigidbody>();
            Collider c = transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Collider>();

            c.isTrigger = false;
            r.mass = transform.GetChild(0).GetChild(0).childCount * 2 - i * 2;
            r.useGravity = true;
            //GameManager.instance.ballList[i].collider.isTrigger = false;
            //GameManager.instance.ballList[i].rigidbody.useGravity = true;
        }


        Rigidbody _rigidbody = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Rigidbody>();
        _rigidbody.AddForce(Vector3.up * _rigidbody.mass * force * 0.1f);

        yield return new WaitForSeconds(0.15f);

        if(transform.GetChild(0).GetChild(0).childCount < 10)
        {
            _rigidbody.AddForce(Vector3.up * _rigidbody.mass * force * 0.1f);
        }
        else if(transform.GetChild(0).GetChild(0).childCount < 15)
        {
            _rigidbody.AddForce(Vector3.up * _rigidbody.mass * force * 0.3f);
        }
        else
        {
            _rigidbody.AddForce(Vector3.up * _rigidbody.mass * force * 0.5f);
        }


        plate.speed = 0;

    

    }

    public void Hide()
    {
        for (int i = 0; i < transform.GetChild(0).GetChild(0).childCount; i++)
        {
            transform.GetChild(0).GetChild(0).GetChild(i).gameObject.SetActive(false);
        }

        plate.speed = 1;

        plate.enabled = true;
        plate.SetTrigger("Complete");
    }

    public void DisableAnimator()
    {
        diskAnimator.enabled = false;
        plate.enabled = false;
        

        plate.transform.localEulerAngles = Vector3.zero;
        elementAnimator.transform.localEulerAngles = Vector3.zero;
    }

    public void EatCheese(int index)
    {
        elementCheese[index].SetActive(false);
    }
}
