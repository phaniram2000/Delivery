using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameObject WheelControl, Joystick;
    private Truck MainControl;

    private void OnEnable()
    {
        GameEvents.TapToPlay += OnTapToPlay;

    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= OnTapToPlay;

    }
    private void OnTapToPlay()
    {
        
    }

    private void Start()
    {
        MainControl = FindObjectOfType<Truck>();
        JoyStickDesider();
    }

    private void JoyStickDesider()
    {
        if (MainControl.Steering)
        {
            WheelControl.SetActive(true);
        }
        else
        {
            Joystick.SetActive(true);
        }
    }
}