using System;
using System.Collections.Generic;
using StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum InputState
{
    Idle,
    FinalPoint,
    Move,
    Jump,
    Dead
}

public class PlayerController : MonoBehaviour
{
    private static InputStateBase _currentstate;
    private static FinalStage _finalStage = new FinalStage();
    private static IdleState _idleState = new IdleState();
    private static MoveState _moveState = new MoveState();
    private static JumpState _jumpState = new JumpState();
    private static DeadState _deadState = new DeadState();
    private bool _hasTappedToPlay;
    public Animator Player_anim;
    public float Movespeed, X_Speed;
    float xInput;


    




    private void Awake()
    {
        Vibration.Init();
    }

    void Start()
    {
        _ = new InputStateBase(this);
        _idleState = new IdleState();
        _currentstate = _idleState;
        if (AudioManager.instance)
            AudioManager.instance.Play("BGM");
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Time.timeScale = 3;
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Time.timeScale = 1;
        }

        if (!HandleTapToPlay()) return;
        _currentstate.Execute();
    }

    public static void SwitchState(InputState Inputstate)
    {
        _currentstate?.OnExit();
        switch (Inputstate)
        {
            case InputState.FinalPoint:
                _currentstate = _finalStage;
                break;
            case InputState.Idle:
                _currentstate = _idleState;
                break;
            case InputState.Move:
                _currentstate = _moveState;
                break;
            case InputState.Jump:
                _currentstate = _jumpState;
                break;
            case InputState.Dead:
                _currentstate = _deadState;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(Inputstate), Inputstate, " ");
        }

        _currentstate?.OnEnter();
    }

    private bool HandleTapToPlay()
    {
        if (_hasTappedToPlay) return true;
        if (!HasTappedOverUi()) return false;
        transform.tag = "Player";
        _hasTappedToPlay = true;
        GameEvents.InvokeTapToPlay();
        SwitchState(InputState.Move);
        return true;
    }

    private static bool HasTappedOverUi()
    {
        if (!InputExtensions.GetFingerDown()) return false;
        if (!EventSystem.current)
        {
            return false;
        }

        if (EventSystem.current.IsPointerOverGameObject(InputExtensions.IsUsingTouch ? Input.GetTouch(0).fingerId : -1))
            return false;
        return true;
    }

    
    
    private void OnTriggerEnter(Collider other)
    {
        _currentstate.OnTriggerEnter(other);
    }
}