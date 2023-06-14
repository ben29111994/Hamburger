using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SwipeControl : MonoBehaviour
{
    public static SwipeControl instance;
    private void Awake()
    {
        instance = (instance == null) ? this : instance;
    }

    private float magnitudeSwipe = 20.0f;
    private bool isSwipe;
    private Vector2 currentTouchPosition;
    private Vector2 lastTouchPosition;
    private Vector2 deltaTouchPosition;

    private void Update()
    {
        UpdateSwipe();
    }

    private void UpdateSwipe()
    {
        bool touchBegan;
        bool touchMoved;
        bool touchEnded;

#if UNITY_EDITOR
        touchBegan = Input.GetMouseButtonDown(0);
        touchMoved = Input.GetMouseButton(0);
        touchEnded = Input.GetMouseButtonUp(0);
#elif UNITY_IOS
        touchBegan = Input.touches[0].phase == TouchPhase.Began;
        touchMoved = Input.touches[0].phase == TouchPhase.Moved;
        touchEnded = Input.touches[0].phase == TouchPhase.Ended;
#endif

        if (touchBegan)
        {
            currentTouchPosition = lastTouchPosition = Input.mousePosition;
        }
        else if (touchMoved && isSwipe == false)
        {
            currentTouchPosition = Input.mousePosition;
            deltaTouchPosition = currentTouchPosition - lastTouchPosition;
            lastTouchPosition = currentTouchPosition;

            if (deltaTouchPosition.magnitude >= magnitudeSwipe)
            {
                isSwipe = true;

                float x = deltaTouchPosition.x;
                float y = deltaTouchPosition.y;

                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    if (x < 0)
                    {
                        SwipeLeft();
                    }
                    else
                    {
                        SwipeRight();
                    }
                }
                else
                {
                    if (y < 0)
                    {
                        SwipeDown();
                    }
                    else
                    {
                        SwipeUp();
                    }
                }
            }
        }
        else if (touchEnded)
        {
            currentTouchPosition = lastTouchPosition = deltaTouchPosition = Vector2.zero;
            isSwipe = false;
        }
    }

    private void SwipeLeft()
    {
        GameManager.instance.Swipe(Vector3.left);
    }

    private void SwipeRight()
    {
        //    Debug.Log("swipe right");

        GameManager.instance.Swipe(Vector3.right);
    }

    private void SwipeUp()
    {
        //  Debug.Log("swipe up");

        GameManager.instance.Swipe(Vector3.forward);
    }

    private void SwipeDown()
    {
        // Debug.Log("swipe down");

        GameManager.instance.Swipe(Vector3.back);
    }
}