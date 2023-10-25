using System;
using DG.Tweening;
using StateMachine;
using UnityEngine;

public class MoveState : InputStateBase
{
    private float xInput;
    private Vector2 touchPosition;
    private Touch touch;
    private float mouseY;
    public int CurrentMeatcount;
    
    public override void OnEnter()
    {
    }

    public override void Execute()
    {
        _player.transform.Translate(Vector3.forward * _player.Movespeed * Time.deltaTime);
        PlayerMovement();
        clampmove();
    }

    private Vector2 GetDeltaMousePos() => InputExtensions.GetInputDelta();
    private bool GetMouseHeld() => InputExtensions.GetFingerHeld();

    /// <summary>
    /// To rotate player when swipting 
    /// </summary>
    private void PlayerMovement()
    {
        if (!Input.GetMouseButton(0)) return;

        var touch = Input.GetMouseButton(0);
        var touchPosition = GetDeltaMousePos();
        float totalRotation = Mathf.Clamp(mouseY + touchPosition.x * _player.X_Speed * Time.deltaTime, -33f, 33);
        float rotation = totalRotation - mouseY;
        _player.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.up) * _player.transform.rotation;
        mouseY = totalRotation;
    }

    /// <summary>
    /// To move player left and right  when swipting uncomment this and comment the above function
    /// </summary>
    // private void PlayerMovement()
    // {
    //     if (!GetMouseHeld()) return;
    //
    //     var position = _player.transform.position;
    //
    //     position += new Vector3(GetDeltaMousePos().x, 0, 0) * (Time.deltaTime * _player.X_Speed);
    //     var x = Mathf.Clamp(position.x, -3.5f, 3.5f);
    //     position = new Vector3(x, position.y, position.z);
    //     _player.transform.position = position;
    // }
    
    
   //To clamp the position of player in X axis
    void clampmove()
    {
        var position = _player.transform.position;
        var x = Mathf.Clamp(position.x, -4.3f, 4.3f);
        position = new Vector3(x, position.y, position.z);
        _player.transform.position = position;
    }

    public override void OnTriggerEnter(Collider Other)
    {
    }

    public override void OnExit()
    {
       // DOTween.KillAll();
    }
}