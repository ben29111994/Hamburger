using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public LayerMask filledBrick;
    public bool isFilled;
    private bool isHole;
    public Renderer meshRenderer;
    private Collider boxCollider;
    public Color white;
    public bool isBlack;
    public bool isTest;
    public Animator waveAnimator;
    public bool isColorWave;
    public Color defaultColor;
    public Color colorPlane;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        waveAnimator.SetTrigger("Idle");
        isColorWaveAnimation = false;
        isColorWave = false;
        isFilled = false;
        boxCollider.enabled = true;
    }

    public void SetBrickColor(bool _isBlack)
    {
        if (_isBlack)
        {
            meshRenderer.material = ColorManager.instance.caro1_material;
        }
        else
        {
            meshRenderer.material = ColorManager.instance.caro2_material;
        }
    }

    public IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.5f);
        boxCollider.enabled = true;
    }

    public void DisableCollider()
    {
        boxCollider.enabled = false;
    }

    public void WaveAnimation()
    {
        waveAnimator.SetTrigger("Wave");

        if(isColorWaveAnimation == false)
        {
        //    _waveColor = ColorManager.instance.currentSetColor;
            StartCoroutine(C_ColorWaveAnimation());
        }
    }

    public Color _currentColor;
    public Color _waveColor;
    private IEnumerator OutColor_End;
    private bool isColorWaveAnimation;

    private IEnumerator C_ColorWaveAnimation()
    {
        // animation wave = 1.0s => color animation = 1s .
        isColorWaveAnimation = true;

        if (OutColor_End != null)
        {
            StopCoroutine(OutColor_End);
        }

        if (isColorWave)
        {
            // 1s = 0.5s (color fade out) + 0.5f (color fade in)
            yield return StartCoroutine(C_ColorOut(0.5f,0.0f));
            yield return StartCoroutine(C_ColorIn(0.5f));
        }
        else
        {
            isColorWave = true;
            yield return StartCoroutine(C_ColorIn(1.0f));
        }

        // after end , check fade color wave to current color .

        OutColor_End = C_ColorOut(1.0f, 0.5f);
        StartCoroutine(OutColor_End);

        isColorWaveAnimation = false;
    }

    private IEnumerator C_ColorIn(float time)
    {
        float t = 0.0f;

        MaterialPropertyBlock propBlock = new MaterialPropertyBlock();

        while (t < 1.0f)
        {
            t += Time.deltaTime * (1.0f / time);

            Color _color = Color.Lerp(_currentColor, _waveColor, t);

            meshRenderer.GetPropertyBlock(propBlock);
            propBlock.SetColor("_Color", _color);
            meshRenderer.SetPropertyBlock(propBlock);

            yield return null;
        }
    }

    private IEnumerator C_ColorOut(float time,float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);

        float t = 0.0f;

        MaterialPropertyBlock propBlock = new MaterialPropertyBlock();

        while (t < 1.0f)
        {
            t += Time.deltaTime * (1.0f / time);

            Color _color = Color.Lerp(_waveColor, _currentColor, t);

            meshRenderer.GetPropertyBlock(propBlock);
            propBlock.SetColor("_Color", _color);
            meshRenderer.SetPropertyBlock(propBlock);

            yield return null;
        }
    }
}
