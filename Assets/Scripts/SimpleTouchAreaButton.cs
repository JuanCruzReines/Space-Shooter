using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SimpleTouchAreaButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool touched;
    private int pointerID;
    private bool isFiring;

    void Awake()
    {
#if !UNITY_ANDROID && !UNITY_IOS && !UNITY_WP8
        Destroy(this);
#endif
        touched = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!touched)
        {
            touched = true;
            pointerID = eventData.pointerId;
            isFiring = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(eventData.pointerId == pointerID)
        {
            isFiring = false;
            touched = false;
        }
    }

    public bool IsFiring()
    {
        return isFiring;
    }
}
