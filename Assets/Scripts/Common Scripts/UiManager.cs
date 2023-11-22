using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameObject WheelControl;
    public GameObject Joystick;
    [SerializeField] private Truck mainControl;
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject IngameUI;
    [SerializeField] private GameObject lost;
    [SerializeField] private Button next;
    [SerializeField] private Button restart;
    [SerializeField] private Sprite lowFuel;
    [SerializeField] private Sprite fullFuel;
    [SerializeField] private Image FuelIcon;
    [SerializeField] private Slider FuelGauge;

    private const float LowFuelThreshold = 0.3f;
    private const int VibrationIntensity = 20;

    private void OnEnable()
    {
        GameEvents.TapToPlay += OnTapToPlay;
        GameEvents.GameWin += OnGameWin;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= OnTapToPlay;
        GameEvents.GameWin -= OnGameWin;
    }

    private void OnTapToPlay()
    {
        // Do something when tap to play event is triggered
    }

    private void Start()
    {
        MainControl = FindObjectOfType<Truck>();
        JoyStickDecider();
        next.onClick.AddListener(NextLevel);
        restart.onClick.AddListener(Reset);
    }

    private void JoyStickDecider()
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

    private void Update()
    {
        CheckFuelStatus();
    }

    private void OnGameWin()
    {
        win.SetActive(true);
        IngameUI.SetActive(false);
    }

    private void CheckFuelStatus()
    {
        FuelIcon.sprite = IsFuelLow ? lowFuel : fullFuel;

        var fuelIconAnimator = FuelIcon.GetComponent<Animator>();
        if (fuelIconAnimator != null)
        {
            fuelIconAnimator.enabled = IsFuelLow;
        }

        if (!IsFuelLow)
        {
            FuelIcon.rectTransform.localScale = Vector3.one;
        }
    }

    private void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Vibration.Vibrate(VibrationIntensity);
    }

    private void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Vibration.Vibrate(VibrationIntensity);
    }

    private Truck MainControl
    {
        get => mainControl ??= FindObjectOfType<Truck>();
        set => mainControl = value;
    }

    private bool IsFuelLow => FuelGauge.value <= LowFuelThreshold;
}