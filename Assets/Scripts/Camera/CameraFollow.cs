using DG.Tweening;
using UnityEngine;

public enum CamState
{
    FollowPlayer,
    WatchAttack,
    FollowEnemy,
    slowmove
}

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public CamState _camState;
    public Transform movepoint;
    public Vector3 offset;
    public Transform enemy;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }


    private void Finalpointcamshift()
    {
        Debug.Log("Move");
        transform.DOMove(movepoint.position, 3).SetEase(Ease.Linear);
        transform.DORotate(new Vector3(16.142f, -57.752f, 0.063f), 3).SetEase(Ease.Linear);
        _camState = CamState.WatchAttack;
    }

    private void Finalpointcamshift1()
    {
        _camState = CamState.WatchAttack;
        transform.DORotate(new Vector3(16.142f, -92.142f, 0.063f), 1).SetEase(Ease.Linear);
        transform.DOMove(movepoint.position, 1).SetEase(Ease.Linear);
    }

    void Start()
    {
        _camState = CamState.FollowPlayer;
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        switch (_camState)
        {
            case CamState.FollowPlayer:
                Followplayer();
                break;
            case CamState.WatchAttack:
                break;
            case CamState.FollowEnemy:
                FollowEnemy(enemy);
                break;
            case CamState.slowmove:
                Finalhitpoint(enemy);
                break;
        }
    }

    private void Finalhitpoint(Transform Enemy)
    {
        transform.position = new Vector3(Enemy.transform.position.x + offset.x, Enemy.transform.position.y + offset.y,
            Enemy.transform.position.z + offset.z);
    }

    public void switchstate()
    {
        _camState = CamState.FollowEnemy;
        offset = transform.position - enemy.transform.position;
        DOTween.To(() => offset.x,
            value => offset.x = value, 5f, 1).SetEase(Ease.Linear);
        DOTween.To(() => offset.y,
            value => offset.y = value, 7f, 1).SetEase(Ease.Linear);
        DOTween.To(() => offset.z,
            value => offset.z = value, -15, 1).SetEase(Ease.Linear);
        transform.DORotate(new Vector3(20.556f, -21.253f, 0.043f), 1).SetEase(Ease.Linear);
    }

    public void switchfialstage()
    {
        _camState = CamState.slowmove;
        offset = transform.position - enemy.transform.position;
        DOTween.To(() => offset.x,
            value => offset.x = value, 17.07f, 1).SetEase(Ease.Linear);
        DOTween.To(() => offset.y,
            value => offset.y = value, 9, 1).SetEase(Ease.Linear);
        DOTween.To(() => offset.z,
            value => offset.z = value, 3f, 1).SetEase(Ease.Linear);
        transform.DORotate(new Vector3(20.556f, -90f, 0.043f), 1).SetEase(Ease.Linear);
    }

    public void FollowEnemy(Transform Enemy)
    {
        transform.position = new Vector3(Enemy.transform.position.x + offset.x, Enemy.transform.position.y + offset.y,
            Enemy.transform.position.z + offset.z);
    }

    public void Followplayer()
    {
        transform.position = new Vector3(0, player.transform.position.y + offset.y,
            player.transform.position.z + offset.z);
    }
}