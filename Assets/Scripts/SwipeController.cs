using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    private bool _isMobile;
    private bool _isDragging;
    private Vector2 _tapPosition;
    private Vector2 _swipeDelta;
    private float _minSwipeDelta = 120f;

    public enum SwipeType
    {
        Left,
        Right,
        Up,
        Down
    }

    public delegate void OnSwipeInput(SwipeType type);
    public static event OnSwipeInput SwipeEvent; 


    private void Awake()
    {
    #if UNITY_EDITOR
            _isMobile = false;
#else
            _isMobile = true;
#endif
    }

    void Update()
    {
        
        if (!_isMobile) // регистрация свайпа кнопкой мыши в редакторе Unity
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isDragging = true;
                _tapPosition = Input.mousePosition;
                
            }
            else if (Input.GetMouseButtonUp(0))
            {
                ResetSwipe();
            }
        }
        else // регистрация свайпа на Android
        {
            if (Input.touchCount > 0)
            {
                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    _isDragging = true;
                    _tapPosition = Input.touches[0].position;
                } else if (Input.touches[0].phase == TouchPhase.Canceled || Input.touches[0].phase == TouchPhase.Ended)
                {
                    ResetSwipe();
                }
            }
        }
        
        CalculatedSwipe();
    }

    private void CalculatedSwipe()
    {
        _swipeDelta = Vector2.zero;

        if (_isDragging) // вычисление длины свайпа
        {
            if (!_isMobile && Input.GetMouseButton(0))
            {
                _swipeDelta = (Vector2)Input.mousePosition - _tapPosition;
            }
            else if (Input.touchCount > 0)
            {
                _swipeDelta = (Vector2)Input.touches[0].position - _tapPosition;
            }
        }

        if (_swipeDelta.magnitude > _minSwipeDelta) // определение направления свайпа
        {
            if (SwipeEvent != null)
            {
                if (Mathf.Abs(_swipeDelta.x) > Mathf.Abs(_swipeDelta.y))
                {
                    if (_swipeDelta.x > 0)
                        SwipeEvent(SwipeType.Right);
                    else
                        SwipeEvent(SwipeType.Left);
                }
                else
                {
                    if (_swipeDelta.y > 0)
                        SwipeEvent(SwipeType.Up);
                    else
                        SwipeEvent(SwipeType.Down);
                }
                ResetSwipe();
            }
        }
    }

    private void ResetSwipe() // сброс информации о свайпе
    {
        _isDragging = false;
        _tapPosition = Vector2.zero;
        _swipeDelta = Vector2.zero;
    }
}
