using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameObject WheelControl, Joystick;
    private PrometeoCarController MainControl;
    private void Start()
    {
        MainControl = FindObjectOfType<PrometeoCarController>();
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