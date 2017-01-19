using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SimpleTouchPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public float smoothing;

    private Vector2 origin;
    private Vector2 direction;
    private Vector2 smoothDirection;

    private bool touched;
    private int pointerID;

    void Awake()
    {
#if !UNITY_ANDROID && !UNITY_IOS && !UNITY_WP8
        Destroy(this);
#endif

        direction = Vector2.zero;
        touched = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //compare the difference between out start point and our current pointer position
        if (eventData.pointerId == pointerID)
        {
            Vector2 currentPosition = eventData.position;
            Vector2 directionRaw = currentPosition - origin;
            direction = directionRaw.normalized;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // set our start point
        if (!touched)
        {
            touched = true;
            pointerID = eventData.pointerId;
            origin = eventData.position;
        }
            
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // reset everything
        if (eventData.pointerId == pointerID)
        {
            direction = Vector2.zero;
            touched = false;
        }
    }

    public Vector2 getDirection()
    {
        smoothDirection = Vector2.MoveTowards(smoothDirection, direction, smoothing);
        return smoothDirection;
    }
}
