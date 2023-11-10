using System;
using DG.Tweening;
using UnityEngine;

public enum DropperState
{
    WaitingForTruck,
    TruckArrived,
    DroppedGoods
}

public class HumanDropper : MonoBehaviour
{
    public Animator botAnimation;
    public GameObject goods;
    public Transform dropPoint;
    public GameObject goodsDropPoint;

    private DropperState _dropperState;
    private static readonly int Walk = Animator.StringToHash("Walk");

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _dropperState = DropperState.WaitingForTruck;
        SwitchState(DropperState.TruckArrived);
        StateDecider();
    }

    private void StateDecider()
    {
        switch (_dropperState)
        {
            case DropperState.WaitingForTruck:
                break;
            case DropperState.TruckArrived:
                MoveToDropPoint();
                break;
            case DropperState.DroppedGoods:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SwitchState(DropperState state)
    {
        _dropperState = state;
    }

    private void MoveToDropPoint()
    {
        var sequence = DOTween.Sequence();
        botAnimation.SetBool(Walk, true);
        sequence.Append(botAnimation.transform.DOMove(dropPoint.position, 0.5f).SetEase(Ease.Linear));
        sequence.AppendCallback(() =>
        {
            SwitchState(DropperState.DroppedGoods);
            AtDropPoint();
        });
    }

    private void AtDropPoint()
    {
        botAnimation.SetBool(Walk, false);
        SetupGoodsPosition();
        DropGoodsAnimation();
    }

    private void SetupGoodsPosition()
    {
        goods.transform.parent = null;
        goods.transform.SetParent(goodsDropPoint.transform);
        goods.transform.DORotate(Vector3.zero, 0.5f);
    }

    private void DropGoodsAnimation()
    {
        goods.transform.DOJump(goodsDropPoint.transform.position, 10, 1, 0.5f)
            .OnComplete(() => { FinalizeGoodsDrop(); });
    }

    private void FinalizeGoodsDrop()
    {
        goods.GetComponent<Collider>().isTrigger = false;
        goods.GetComponent<Rigidbody>().isKinematic = true;
    }
}