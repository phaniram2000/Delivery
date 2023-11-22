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
    public bool Customer, Endpoint;
    public Animator Customer_Animiation;

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
            if (!Endpoint)
            {
                DOVirtual.DelayedCall(1.5f, (() => { GameEvents.InvokeGameWin(); }));
                DOVirtual.DelayedCall(.5f, (() => { Initialize(); }));

                // Change the truck state to PickupState

                if (Customer)
                    Customer_Animiation.SetTrigger("Celebrate");

                GetComponent<Collider>().enabled = false;
            }
            else
            {
                DOVirtual.DelayedCall(1.5f, (() => { GameEvents.InvokeGameWin(); }));
            }
        }
    }
}