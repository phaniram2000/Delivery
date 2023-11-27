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
    public bool Customer, Endpoint, MultipleObjects;
    public Animator Customer_Animiation;
    public Transform[] dropPoints;
    public GameObject[] objectsToDeliver;

    private void Start()
    {
        truck = GameObject.FindWithTag("Truck");
    }

    private void Initialize()
    {
        if (!MultipleObjects)
        {
            package.transform.parent = null;
            package.GetComponent<Rigidbody>().isKinematic = true;
            package.GetComponent<Collider>().isTrigger = true;
            package.transform.DOJump(DropPoint.transform.position, 10, 1, .7f).SetEase(Ease.Flash);
        }
        else if (MultipleObjects)
        {
            StartCoroutine(MultipleObjectDelivery());
            for (int i = 0; i < objectsToDeliver.Length; i++)
            {
                objectsToDeliver[i].GetComponent<Rigidbody>().isKinematic = true;
                objectsToDeliver[i].GetComponent<Collider>().isTrigger = true;
            }
        }
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

    private IEnumerator MultipleObjectDelivery()
    {
        for (int i = 0; i < objectsToDeliver.Length; i++)
        {
            GameObject obj = objectsToDeliver[i];

            // Detach the object from its parent
            obj.transform.parent = null;

            // Make the object kinematic during the jump


            // Use DOJump to jump to the current drop point
            obj.transform.DOJump(dropPoints[i].position, 10, 1, 0.7f).SetEase(Ease.Flash);
            obj.transform
                .DOLocalRotate(
                    new Vector3(dropPoints[i].rotation.x, dropPoints[i].rotation.y, dropPoints[i].rotation.z),
                    .7f).SetEase(Ease.Flash);

            yield return new WaitForSeconds(0.3f);
        }
    }
}