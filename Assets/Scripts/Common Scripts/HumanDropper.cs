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
    public GameObject truck; // Reference to the truck game object
    public Vector3 Botposition;
    public DropperState _dropperState;
    private static readonly int Walk = Animator.StringToHash("Walk");

    void Start()
    {
        truck = GameObject.FindWithTag("Truck");
        Botposition.y = botAnimation.transform.position.y;
        _dropperState = DropperState.WaitingForTruck;
    }

    void Update()
    {
    }

    private void Initialize()
    {
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
        if (truck != null)
        {
            // Change the truck state to PickupState
            truck.GetComponent<Truck>().SwitchState(new PickUpstate());

            // Find a drop point near the truck
            dropPoint = FindDropPointNearTruck(truck.transform.position);
        }

        sequence.Append(botAnimation.transform.DOMove(dropPoint.position, 0.5f).SetEase(Ease.Linear));
        sequence.AppendCallback(() =>
        {
            SwitchState(DropperState.DroppedGoods);
            AtDropPoint();
        });
    }

    private Transform FindDropPointNearTruck(Vector3 truckPosition)
    {
        
        float xOffset = Botposition.x;

        float zOffset = Botposition.z;

        Vector3 offset = new Vector3(xOffset, Botposition.y, zOffset);
        Vector3 dropPointPosition = new Vector3(truckPosition.x + truck.transform.TransformDirection(offset).x,
            Botposition.y, truckPosition.z + truck.transform.TransformDirection(offset).z);
        GameObject newDropPoint = new GameObject("DynamicDropPoint");
        newDropPoint.transform.position = dropPointPosition;

        return newDropPoint.transform;
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
            .OnComplete(() =>
            {
                FinalizeGoodsDrop();
                if (truck != null)
                {
                    // Change the truck state to PickupState
                    truck.GetComponent<Truck>().SwitchState(new DriveState());
                }
            });
    }

    private void FinalizeGoodsDrop()
    {
        goods.GetComponent<Collider>().isTrigger = false;
        goods.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Truck"))
        {
            Initialize();
            GetComponent<Collider>().enabled = false;
        }
    }
}