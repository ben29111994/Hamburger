using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hole : MonoBehaviour
{
    private Renderer renderer;
    public Animator anim;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        anim.SetTrigger("Idle");
    }

    public void SetAnimationDisk(bool isIdle)
    {
        if (isIdle)
        {
            anim.SetTrigger("Idle");
        }
        else
        {
            anim.SetTrigger("Rotate");
        }
    }

    public void SetBrickColor(bool _isBlack)
    {
        if (_isBlack)
        {
            renderer.material = ColorManager.instance.caro1_material;
        }
        else
        {
            renderer.material = ColorManager.instance.caro2_material;
        }
    }
}
