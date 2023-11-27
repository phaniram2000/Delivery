using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class people : MonoBehaviour
{
    public bool back;
    public float movespeed, forwardRotation, Backrotation;


    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * movespeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Moveback"))
        {
            if (back == false)
            {
                transform.DORotate(
                    new Vector3(transform.rotation.x, Backrotation, transform.rotation.z),
                    .01f);
                back = true;
            }
            else if (back == true)
            {
                transform.DORotate(
                    new Vector3(transform.rotation.x, forwardRotation, transform.rotation.z),
                    .01f);
                back = false;
            }
        }

       
    }

    
}