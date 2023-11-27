using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextLevel();
        }
    }

    private void OnGameWin()
    {
        win.SetActive(true);
        IngameUI.SetActive(false);
    }


    private void NextLevel()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        if (PlayerPrefs.GetInt("Level") >= (SceneManager.sceneCountInBuildSettings) - 1)
        {
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);
            var i = Random.Range(2, SceneManager.sceneCountInBuildSettings);
            PlayerPrefs.SetInt("ThisLevel", i);
            SceneManager.LoadScene(i);
        }
        else
        {
            PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex + 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

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
}