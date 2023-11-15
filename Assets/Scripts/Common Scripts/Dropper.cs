using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    public Transform DropPoint;
    public Transform package;

    public GameObject truck;

    private void Start()
    {
        truck = GameObject.FindWithTag("Truck");
    }

    private void Initialize()
    {
        package.transform.parent = null;
        package.transform.DOJump(DropPoint.transform.position, 10, 1, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Truck"))
        {
            Initialize();
            if (truck != null)
            {
                // Change the truck state to PickupState
                truck.GetComponent<Truck>().SwitchState(new DroppingGoodsState());
            }

            GetComponent<Collider>().enabled = false;
        }
    }
}