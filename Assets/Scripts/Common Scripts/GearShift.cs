using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GearShift : MonoBehaviour
{
    private Camera mainCamera;
    private bool moveback = false;

    public bool MoveBack
    {
        get { return moveback; }
        private set { moveback = value; }
    }

    void Start()
    {
        mainCamera = Camera.main;
        MoveBack = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckForTap(Input.mousePosition);
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            CheckForTap(Input.GetTouch(0).position);
        }
    }

    void CheckForTap(Vector3 screenPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject tappedObject = hit.transform.gameObject;
            if (tappedObject.CompareTag("Gear"))
            {
                OnGearShiftButtonClicked();
            }
        }
    }

    private void OnGearShiftButtonClicked()
    {
        if (MoveBack)
        {
            MoveBack = false;
            transform.DOLocalRotate(new Vector3(25, 0, 0), .3f);
        }
        else if (!MoveBack)
        {
            MoveBack = true;
            transform.DOLocalRotate(new Vector3(-25, 0, 0), .3f);
        }
    }
}